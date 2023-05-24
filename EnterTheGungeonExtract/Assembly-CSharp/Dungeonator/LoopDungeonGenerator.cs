using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EFF RID: 3839
	public class LoopDungeonGenerator : IDungeonGenerator
	{
		// Token: 0x060051DE RID: 20958 RVA: 0x001D325C File Offset: 0x001D145C
		public LoopDungeonGenerator(Dungeon d, int dungeonSeed)
		{
			this.m_patternSettings = d.PatternSettings;
			this.m_assignedFlow = this.m_patternSettings.GetRandomFlow();
			this.m_lastAssignedSeed = dungeonSeed;
			UnityEngine.Random.InitState(dungeonSeed);
			BraveRandom.InitializeWithSeed(dungeonSeed);
			GameManager.SEED_LABEL = dungeonSeed.ToString();
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x001D32B4 File Offset: 0x001D14B4
		public void AssignFlow(DungeonFlow flow)
		{
			this.m_forceAssignedFlow = true;
			this.m_assignedFlow = flow;
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x001D32C4 File Offset: 0x001D14C4
		protected void GetNewFlowForIteration()
		{
			if (this.m_forceAssignedFlow)
			{
				return;
			}
			this.m_assignedFlow = this.m_patternSettings.GetRandomFlow();
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x001D32E4 File Offset: 0x001D14E4
		protected void RecalculateRoomDistances(RoomHandler entrance)
		{
			Queue<Tuple<RoomHandler, int>> queue = new Queue<Tuple<RoomHandler, int>>();
			List<RoomHandler> list = new List<RoomHandler>();
			queue.Enqueue(new Tuple<RoomHandler, int>(entrance, 0));
			while (queue.Count > 0)
			{
				Tuple<RoomHandler, int> tuple = queue.Dequeue();
				tuple.First.distanceFromEntrance = tuple.Second;
				list.Add(tuple.First);
				for (int i = 0; i < tuple.First.connectedRooms.Count; i++)
				{
					RoomHandler roomHandler = tuple.First.connectedRooms[i];
					if (!list.Contains(roomHandler))
					{
						queue.Enqueue(new Tuple<RoomHandler, int>(roomHandler, tuple.Second + 1));
					}
				}
			}
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x001D339C File Offset: 0x001D159C
		public IEnumerable GenerateDungeonLayoutDeferred()
		{
			for (;;)
			{
				IEnumerator<ProcessStatus> tracker = this.GenerateDungeonLayoutDeferred_Internal().GetEnumerator();
				bool didSucceed = false;
				while (tracker.MoveNext())
				{
					if (tracker.Current == ProcessStatus.Incomplete)
					{
						yield return null;
					}
					else
					{
						if (tracker.Current == ProcessStatus.Success)
						{
							UnityEngine.Debug.Log("Succeeded generation iteration on: " + this.m_assignedFlow.name);
							didSucceed = true;
							break;
						}
						if (tracker.Current == ProcessStatus.Fail)
						{
							UnityEngine.Debug.Log("Failed generation iteration on: " + this.m_assignedFlow.name);
							didSucceed = false;
							break;
						}
					}
				}
				if (didSucceed)
				{
					break;
				}
				this.GetNewFlowForIteration();
				yield return null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060051E3 RID: 20963 RVA: 0x001D33C0 File Offset: 0x001D15C0
		public IEnumerable<ProcessStatus> GenerateDungeonLayoutDeferred_Internal()
		{
			BraveMemory.DoCollect();
			if (this.m_timer == null)
			{
				this.m_timer = new Stopwatch();
			}
			bool generationSucceeded = false;
			SemioticLayoutManager layout = null;
			int attempts = 0;
			int generationAttempts = 50;
			LoopDungeonGenerator.NUM_FAILS_COMPOSITE_ATTACHMENT = 0;
			LoopDungeonGenerator.NUM_FAILS_COMPOSITE_REGENERATION = 0;
			UnityEngine.Debug.Log("Attempting to generate flow: " + this.m_assignedFlow.name);
			while (!generationSucceeded && attempts < generationAttempts)
			{
				attempts++;
				LoopFlowBuilder builder = new LoopFlowBuilder(this.m_assignedFlow, this);
				IEnumerator buildTracker = builder.DeferredBuild().GetEnumerator();
				this.m_timer.Reset();
				this.m_timer.Start();
				while (buildTracker.MoveNext())
				{
					if (this.m_timer.ElapsedMilliseconds > 30L)
					{
						this.m_timer.Reset();
						yield return ProcessStatus.Incomplete;
					}
				}
				layout = builder.DeferredGeneratedLayout;
				generationSucceeded = builder.DeferredGenerationSuccess;
				if (!generationSucceeded && attempts % 3 == 0)
				{
					BraveMemory.DoCollect();
				}
				yield return ProcessStatus.Incomplete;
			}
			if (this.RAPID_DEBUG_ITERATION_MODE && !generationSucceeded)
			{
				yield return ProcessStatus.Fail;
			}
			if (layout == null)
			{
				yield return ProcessStatus.Fail;
			}
			if (this.m_assignedFlow != null)
			{
				GameStatsManager.Instance.EncounterFlow(this.m_assignedFlow.name);
			}
			IntVector2 min = layout.GetSafelyBoundedMinimumCellPosition();
			IntVector2 max = layout.GetSafelyBoundedMaximumCellPosition();
			IntVector2 offsetRequired = new IntVector2(-min.x + 10, -min.y + 10);
			IntVector2 span = max - min;
			layout.HandleOffsetRooms(offsetRequired);
			CellData[][] cells = new CellData[span.x + 20][];
			int cellsCreated = this.CreateCellDataIntelligently(cells, layout, span, offsetRequired);
			DungeonData dungeonData = new DungeonData(cells);
			List<RoomHandler> rooms = layout.Rooms;
			for (int i = 0; i < rooms.Count; i++)
			{
				for (int j = 0; j < rooms[i].connectedRooms.Count; j++)
				{
					if (!rooms.Contains(rooms[i].connectedRooms[j]))
					{
						UnityEngine.Debug.LogWarning(rooms[i].connectedRooms[j].GetRoomName() + " is not in the list!!!!!!");
					}
				}
			}
			dungeonData.InitializeCoreData(rooms);
			dungeonData.Entrance = rooms[0];
			dungeonData.Exit = rooms[rooms.Count - 1];
			for (int k = 0; k < rooms.Count; k++)
			{
				if (rooms[k].area.prototypeRoom != null)
				{
					if (rooms[k].area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.ENTRANCE)
					{
						dungeonData.Entrance = rooms[k];
					}
					else if (rooms[k].area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.EXIT)
					{
						dungeonData.Exit = rooms[k];
					}
				}
			}
			this.RecalculateRoomDistances(dungeonData.Entrance);
			this.DeferredGeneratedData = dungeonData;
			yield return ProcessStatus.Success;
			yield break;
		}

		// Token: 0x060051E4 RID: 20964 RVA: 0x001D33E4 File Offset: 0x001D15E4
		private int CreateCellDataIntelligently(CellData[][] cells, SemioticLayoutManager layout, IntVector2 span, IntVector2 offsetRequired)
		{
			int num = 0;
			float[] array = new float[(span.y + 20) * (span.x + 20)];
			for (int i = 0; i < span.x + 20; i++)
			{
				for (int j = 0; j < span.y + 20; j++)
				{
					array[j * (span.x + 20) + i] = 1000000f;
				}
			}
			Queue<IntVector2> queue = new Queue<IntVector2>();
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			foreach (IntVector2 intVector in layout.OccupiedCells)
			{
				IntVector2 intVector2 = intVector + offsetRequired;
				array[intVector2.y * (span.x + 20) + intVector2.x] = 0f;
				queue.Enqueue(intVector2);
				hashSet.Add(intVector2);
			}
			IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
			while (queue.Count > 0)
			{
				IntVector2 intVector3 = queue.Dequeue();
				hashSet.Remove(intVector3);
				float num2 = array[intVector3.y * (span.x + 20) + intVector3.x];
				for (int k = 0; k < cardinalsAndOrdinals.Length; k++)
				{
					IntVector2 intVector4 = intVector3 + cardinalsAndOrdinals[k];
					if (intVector4.x >= 0 && intVector4.y >= 0 && intVector4.x < span.x + 20 && intVector4.y < span.y + 20)
					{
						float num3 = array[intVector4.y * (span.x + 20) + intVector4.x];
						float num4 = ((k % 2 != 1) ? (num2 + 1f) : (num2 + 1.414f));
						if (num3 > num4)
						{
							array[intVector4.y * (span.x + 20) + intVector4.x] = num4;
							if (!hashSet.Contains(intVector4))
							{
								queue.Enqueue(intVector4);
								hashSet.Add(intVector4);
							}
						}
					}
				}
			}
			for (int l = 0; l < cells.Length; l++)
			{
				cells[l] = new CellData[span.y + 20];
			}
			for (int m = 0; m < span.x + 20; m++)
			{
				for (int n = 0; n < span.y + 20; n++)
				{
					float num5 = array[n * (span.x + 20) + m];
					if (num5 <= 7f)
					{
						cells[m][n] = new CellData(m, n, CellType.WALL);
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x001D36DC File Offset: 0x001D18DC
		public void GenerateDungeonLayoutThreaded()
		{
			IEnumerable enumerable = this.GenerateDungeonLayoutDeferred();
			IEnumerator enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x001D3708 File Offset: 0x001D1908
		public DungeonData GenerateDungeonLayout()
		{
			IEnumerable enumerable = this.GenerateDungeonLayoutDeferred();
			IEnumerator enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
			return this.DeferredGeneratedData;
		}

		// Token: 0x04004A01 RID: 18945
		public const bool c_ROOM_MIRRORING = false;

		// Token: 0x04004A02 RID: 18946
		public static int NUM_FAILS_COMPOSITE_REGENERATION;

		// Token: 0x04004A03 RID: 18947
		public static int NUM_FAILS_COMPOSITE_ATTACHMENT;

		// Token: 0x04004A04 RID: 18948
		public DungeonData DeferredGeneratedData;

		// Token: 0x04004A05 RID: 18949
		public bool RAPID_DEBUG_ITERATION_MODE;

		// Token: 0x04004A06 RID: 18950
		public int RAPID_DEBUG_ITERATION_INDEX;

		// Token: 0x04004A07 RID: 18951
		private SemioticDungeonGenSettings m_patternSettings;

		// Token: 0x04004A08 RID: 18952
		private DungeonFlow m_assignedFlow;

		// Token: 0x04004A09 RID: 18953
		private int m_lastAssignedSeed;

		// Token: 0x04004A0A RID: 18954
		private bool m_forceAssignedFlow;

		// Token: 0x04004A0B RID: 18955
		private Stopwatch m_timer;
	}
}
