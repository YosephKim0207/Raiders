using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F35 RID: 3893
	public class SemioticDungeonGenerator : IDungeonGenerator
	{
		// Token: 0x06005369 RID: 21353 RVA: 0x001E62CC File Offset: 0x001E44CC
		public SemioticDungeonGenerator(Dungeon d, SemioticDungeonGenSettings sdgs)
		{
			this.m_patternSettings = sdgs;
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x001E62DC File Offset: 0x001E44DC
		public DungeonData GenerateDungeonLayout()
		{
			PrototypeDungeonRoom prototypeDungeonRoom = this.GetRandomEntranceRoomFromList(this.m_patternSettings.flows[0].fallbackRoomTable.GetCompiledList());
			if (prototypeDungeonRoom == null)
			{
				prototypeDungeonRoom = this.m_patternSettings.flows[0].fallbackRoomTable.SelectByWeight().room;
			}
			SemioticLayoutManager semioticLayoutManager = null;
			DungeonFlowBuilder dungeonFlowBuilder = null;
			CellArea cellArea = null;
			bool flag = true;
			int num = 0;
			int num2 = 10;
			for (;;)
			{
				BraveMemory.DoCollect();
				if (num == num2)
				{
					break;
				}
				num++;
				cellArea = new CellArea(IntVector2.Zero, new IntVector2(prototypeDungeonRoom.Width, prototypeDungeonRoom.Height), 0);
				cellArea.prototypeRoom = prototypeDungeonRoom;
				cellArea.instanceUsedExits = new List<PrototypeRoomExit>();
				RoomHandler roomHandler = new RoomHandler(cellArea);
				roomHandler.distanceFromEntrance = 0;
				semioticLayoutManager = new SemioticLayoutManager();
				semioticLayoutManager.StampCellAreaToLayout(roomHandler, false);
				dungeonFlowBuilder = new DungeonFlowBuilder(this.m_patternSettings.flows[0], semioticLayoutManager);
				bool flag2 = dungeonFlowBuilder.Build(roomHandler);
				if (flag2)
				{
					for (int i = 0; i < this.m_patternSettings.mandatoryExtraRooms.Count; i++)
					{
						flag2 = dungeonFlowBuilder.AttemptAppendExtraRoom(this.m_patternSettings.mandatoryExtraRooms[i]);
						if (!flag2)
						{
							break;
						}
					}
					if (flag2)
					{
						goto IL_15B;
					}
				}
			}
			Debug.LogError("DUNGEON GENERATION FAILED.");
			flag = false;
			goto IL_17B;
			IL_15B:
			Debug.Log("DUNGEON GENERATION SUCCEEDED ON ATTEMPT #" + num);
			IL_17B:
			if (flag)
			{
				dungeonFlowBuilder.AppendCapChains();
			}
			IntVector2 minimumCellPosition = semioticLayoutManager.GetMinimumCellPosition();
			IntVector2 maximumCellPosition = semioticLayoutManager.GetMaximumCellPosition();
			IntVector2 intVector = new IntVector2(-minimumCellPosition.x + 10, -minimumCellPosition.y + 10);
			IntVector2 intVector2 = maximumCellPosition - minimumCellPosition;
			semioticLayoutManager.HandleOffsetRooms(intVector);
			dungeonFlowBuilder.DebugActionLines();
			if (this.RAPID_DEBUG_ITERATION_MODE)
			{
				Texture2D texture2D = new Texture2D(intVector2.x + 20, intVector2.y + 20);
				Color[] array = new Color[(intVector2.x + 20) * (intVector2.y + 20)];
				for (int j = 0; j < intVector2.x + 20; j++)
				{
					for (int k = 0; k < intVector2.y + 20; k++)
					{
						array[k * (intVector2.x + 20) + j] = ((!flag) ? new Color(0.5f, 0f, 0f) : new Color(0f, 0.5f, 0f));
					}
				}
				texture2D.SetPixels(array);
				texture2D.Apply();
				byte[] array2 = texture2D.EncodeToPNG();
				FileStream fileStream = File.Open(Application.dataPath + "/DungeonDebug/debug_" + this.RAPID_DEBUG_ITERATION_INDEX.ToString() + ".png", FileMode.Create);
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					binaryWriter.Write(array2);
				}
				return null;
			}
			CellData[][] array3 = new CellData[intVector2.x + 20][];
			for (int l = 0; l < array3.Length; l++)
			{
				array3[l] = new CellData[intVector2.y + 20];
				for (int m = 0; m < array3[l].Length; m++)
				{
					array3[l][m] = new CellData(l, m, CellType.WALL);
				}
			}
			DungeonData dungeonData = new DungeonData(array3);
			List<RoomHandler> rooms = semioticLayoutManager.Rooms;
			dungeonData.InitializeCoreData(rooms);
			dungeonData.Entrance = this.GetRoomHandlerByArea(rooms, cellArea);
			dungeonData.Exit = dungeonFlowBuilder.EndRoom;
			return dungeonData;
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x001E66AC File Offset: 0x001E48AC
		private PrototypeDungeonRoom GetRandomEntranceRoomFromList(List<WeightedRoom> source)
		{
			List<PrototypeDungeonRoom> list = new List<PrototypeDungeonRoom>();
			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].room.category == PrototypeDungeonRoom.RoomCategory.ENTRANCE)
				{
					list.Add(source[i].room);
				}
			}
			if (list.Count > 0)
			{
				return list[BraveRandom.GenerationRandomRange(0, list.Count)];
			}
			return null;
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x001E6720 File Offset: 0x001E4920
		private RoomHandler GetRoomHandlerByArea(List<RoomHandler> rooms, CellArea area)
		{
			for (int i = 0; i < rooms.Count; i++)
			{
				if (rooms[i].area == area)
				{
					return rooms[i];
				}
			}
			return null;
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x001E6760 File Offset: 0x001E4960
		private void DrawDebugSquare(IntVector2 pos, Color col)
		{
			Debug.DrawLine(pos.ToVector2(), pos.ToVector2() + Vector2.up, col, 1000f);
			Debug.DrawLine(pos.ToVector2(), pos.ToVector2() + Vector2.right, col, 1000f);
			Debug.DrawLine(pos.ToVector2() + Vector2.up, pos.ToVector2() + Vector2.right + Vector2.up, col, 1000f);
			Debug.DrawLine(pos.ToVector2() + Vector2.right, pos.ToVector2() + Vector2.right + Vector2.up, col, 1000f);
		}

		// Token: 0x04004BE2 RID: 19426
		public bool RAPID_DEBUG_ITERATION_MODE;

		// Token: 0x04004BE3 RID: 19427
		public int RAPID_DEBUG_ITERATION_INDEX;

		// Token: 0x04004BE4 RID: 19428
		private SemioticDungeonGenSettings m_patternSettings;

		// Token: 0x04004BE5 RID: 19429
		private int m_numberRoomCells;
	}
}
