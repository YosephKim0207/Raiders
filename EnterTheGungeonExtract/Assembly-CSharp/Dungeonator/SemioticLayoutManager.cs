using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F37 RID: 3895
	public class SemioticLayoutManager
	{
		// Token: 0x06005372 RID: 21362 RVA: 0x001E6A40 File Offset: 0x001E4C40
		public SemioticLayoutManager()
		{
			this.m_allRooms = new List<RoomHandler>();
			if (SemioticLayoutManager.PooledResizedHashsets.Count > 0)
			{
				int num = 0;
				for (int i = 0; i < SemioticLayoutManager.PooledResizedHashsets.Count; i++)
				{
					if (SemioticLayoutManager.PooledResizedHashsets[num].Count < SemioticLayoutManager.PooledResizedHashsets[i].Count)
					{
						num = i;
					}
				}
				this.m_occupiedCells = SemioticLayoutManager.PooledResizedHashsets[num];
				SemioticLayoutManager.PooledResizedHashsets.RemoveAt(num);
			}
			else
			{
				this.m_occupiedCells = new HashSet<IntVector2>();
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06005373 RID: 21363 RVA: 0x001E6B08 File Offset: 0x001E4D08
		public List<RoomHandler> Rooms
		{
			get
			{
				return this.m_allRooms;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06005374 RID: 21364 RVA: 0x001E6B10 File Offset: 0x001E4D10
		public HashSet<IntVector2> OccupiedCells
		{
			get
			{
				return this.m_occupiedCells;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06005375 RID: 21365 RVA: 0x001E6B18 File Offset: 0x001E4D18
		public IntVector2 Dimensions
		{
			get
			{
				Tuple<IntVector2, IntVector2> minAndMaxCellPositions = this.GetMinAndMaxCellPositions();
				return minAndMaxCellPositions.Second - minAndMaxCellPositions.First;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06005376 RID: 21366 RVA: 0x001E6B40 File Offset: 0x001E4D40
		public IntVector2 NegativeDimensions
		{
			get
			{
				return IntVector2.Max(IntVector2.Zero, IntVector2.Zero - this.GetMinimumCellPosition());
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06005377 RID: 21367 RVA: 0x001E6B5C File Offset: 0x001E4D5C
		public IntVector2 PositiveDimensions
		{
			get
			{
				return IntVector2.Max(IntVector2.Zero, this.GetMaximumCellPosition());
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06005378 RID: 21368 RVA: 0x001E6B70 File Offset: 0x001E4D70
		public List<Tuple<IntVector2, IntVector2>> RectangleDecomposition
		{
			get
			{
				return this.m_rectangleDecomposition;
			}
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x001E6B78 File Offset: 0x001E4D78
		public void ComputeRectangleDecomposition()
		{
			if (this.m_rectangleDecomposition == null)
			{
				this.m_rectangleDecomposition = new List<Tuple<IntVector2, IntVector2>>();
			}
			else if (this.m_rectangleDecomposition.Count != 0)
			{
				return;
			}
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			foreach (IntVector2 intVector in this.m_occupiedCells)
			{
				if (!hashSet.Contains(intVector))
				{
					int num = 1;
					int num2 = 1;
					for (;;)
					{
						int num3 = intVector.y + num2;
						IntVector2 intVector2 = new IntVector2(intVector.x, num3);
						if (!this.m_occupiedCells.Contains(intVector2))
						{
							break;
						}
						num2++;
					}
					for (;;)
					{
						int num4 = intVector.x + num;
						bool flag = true;
						for (int i = intVector.y; i < intVector.y + num2; i++)
						{
							IntVector2 intVector3 = new IntVector2(num4, i);
							if (!this.m_occupiedCells.Contains(intVector3))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
						num++;
					}
					for (int j = intVector.x; j < intVector.x + num; j++)
					{
						for (int k = intVector.y; k < intVector.y + num2; k++)
						{
							IntVector2 intVector4 = new IntVector2(j, k);
							hashSet.Add(intVector4);
						}
					}
					this.m_rectangleDecomposition.Add(new Tuple<IntVector2, IntVector2>(intVector, new IntVector2(num, num2)));
				}
			}
			this.m_rectangleDecomposition = this.m_rectangleDecomposition.OrderByDescending((Tuple<IntVector2, IntVector2> a) => a.Second.x * a.Second.y).ToList<Tuple<IntVector2, IntVector2>>();
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x001E6D84 File Offset: 0x001E4F84
		public void OnDestroy()
		{
			this.m_occupiedCells.Clear();
			if (SemioticLayoutManager.PooledResizedHashsets.Count > 10)
			{
				int num = 0;
				for (int i = 0; i < SemioticLayoutManager.PooledResizedHashsets.Count; i++)
				{
					if (SemioticLayoutManager.PooledResizedHashsets[num].Count > SemioticLayoutManager.PooledResizedHashsets[i].Count)
					{
						num = i;
					}
				}
				SemioticLayoutManager.PooledResizedHashsets.RemoveAt(num);
			}
			SemioticLayoutManager.PooledResizedHashsets.Add(this.m_occupiedCells);
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x001E6E0C File Offset: 0x001E500C
		public void DebugListLengths()
		{
			Debug.Log(string.Concat(new object[]
			{
				"SLayoutManager list sizes: ",
				this.m_allRooms.Count,
				"|",
				this.m_occupiedCells.Count
			}));
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x001E6E60 File Offset: 0x001E5060
		public void DebugDrawOccupiedCells(Vector2 positionOffset)
		{
			foreach (IntVector2 intVector in this.m_occupiedCells)
			{
				BraveUtility.DrawDebugSquare(intVector.ToVector2() + positionOffset, Color.red, 1000f);
			}
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x001E6ED4 File Offset: 0x001E50D4
		public void DebugDrawBoundingBox(Vector2 positionOffset, Color c)
		{
			Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
			foreach (IntVector2 intVector in this.m_occupiedCells)
			{
				vector.x = Mathf.Min((float)intVector.x, vector.x);
				vector.y = Mathf.Min((float)intVector.y, vector.x);
				vector2.x = Mathf.Max((float)intVector.x, vector2.x);
				vector2.y = Mathf.Max((float)intVector.y, vector2.x);
			}
			BraveUtility.DrawDebugSquare(vector + positionOffset, vector2 + Vector2.one + positionOffset, c, 1000f);
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x001E6FDC File Offset: 0x001E51DC
		public void ClearTemporary()
		{
			this.m_temporaryPathfindingWalls.Clear();
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x001E6FEC File Offset: 0x001E51EC
		public void StampComplexExitTemporary(RuntimeRoomExitData exit, CellArea area)
		{
			PrototypeRoomExit referencedExit = exit.referencedExit;
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(referencedExit.exitDirection);
			int num = ((!exit.jointedExit || referencedExit.exitDirection == DungeonData.Direction.WEST) ? 0 : 1);
			for (int i = 0; i < referencedExit.containedCells.Count; i++)
			{
				for (int j = 0; j < exit.TotalExitLength + num; j++)
				{
					IntVector2 intVector = referencedExit.containedCells[i].ToIntVector2(VectorConversions.Round) - IntVector2.One + area.basePosition + intVector2FromDirection * j;
					this.m_temporaryPathfindingWalls.Add(intVector);
					for (int k = 0; k < SemioticLayoutManager.SimpleCardinals.Length; k++)
					{
						this.m_temporaryPathfindingWalls.Add(intVector + SemioticLayoutManager.LayoutCardinals[k]);
					}
				}
			}
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x001E70E8 File Offset: 0x001E52E8
		public void StampComplexExitToLayout(RuntimeRoomExitData exit, CellArea area, bool unstamp = false)
		{
			PrototypeRoomExit referencedExit = exit.referencedExit;
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(referencedExit.exitDirection);
			int num = ((!exit.jointedExit || referencedExit.exitDirection == DungeonData.Direction.WEST) ? 0 : 1);
			for (int i = 0; i < referencedExit.containedCells.Count; i++)
			{
				for (int j = 0; j < exit.TotalExitLength + num; j++)
				{
					IntVector2 intVector = referencedExit.containedCells[i].ToIntVector2(VectorConversions.Round) - IntVector2.One + area.basePosition + intVector2FromDirection * j;
					if (unstamp)
					{
						this.m_occupiedCells.Remove(intVector);
						for (int k = 0; k < SemioticLayoutManager.LayoutCardinals.Length; k++)
						{
							this.m_occupiedCells.Remove(intVector + SemioticLayoutManager.LayoutCardinals[k]);
						}
					}
					else
					{
						this.m_occupiedCells.Add(intVector);
						for (int l = 0; l < SemioticLayoutManager.LayoutCardinals.Length; l++)
						{
							this.m_occupiedCells.Add(intVector + SemioticLayoutManager.LayoutCardinals[l]);
						}
					}
				}
			}
			IntVector2 intVector2 = referencedExit.containedCells[0].ToIntVector2(VectorConversions.Round) - IntVector2.One + area.basePosition + intVector2FromDirection * (exit.TotalExitLength + num - 1);
			if (unstamp)
			{
				this.m_exitTestPoints.Remove(intVector2);
			}
			else
			{
				this.m_exitTestPoints.Add(intVector2);
			}
			if (this.m_rectangleDecomposition != null)
			{
				this.m_rectangleDecomposition.Clear();
			}
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x001E72B8 File Offset: 0x001E54B8
		public void DebugDrawComplexit(RuntimeRoomExitData exit, CellArea area, Color c)
		{
			PrototypeRoomExit referencedExit = exit.referencedExit;
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(referencedExit.exitDirection);
			int num = ((!exit.jointedExit || referencedExit.exitDirection == DungeonData.Direction.WEST) ? 0 : 1);
			for (int i = 0; i < referencedExit.containedCells.Count; i++)
			{
				for (int j = 0; j < exit.TotalExitLength + num; j++)
				{
					IntVector2 intVector = referencedExit.containedCells[i].ToIntVector2(VectorConversions.Round) - IntVector2.One + area.basePosition + intVector2FromDirection * j;
					this.m_occupiedCells.Add(intVector);
					for (int k = 0; k < SemioticLayoutManager.LayoutCardinals.Length; k++)
					{
						BraveUtility.DrawDebugSquare((intVector + SemioticLayoutManager.LayoutCardinals[k]).ToVector2(), c, 1000f);
					}
				}
			}
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x001E73BC File Offset: 0x001E55BC
		public SemioticLayoutManager.BBoxPrepassResults CheckRoomBoundingBoxCollisions(SemioticLayoutManager otherCanvas, IntVector2 otherCanvasOffset)
		{
			SemioticLayoutManager.BBoxPrepassResults bboxPrepassResults = default(SemioticLayoutManager.BBoxPrepassResults);
			bboxPrepassResults.overlapping = false;
			for (int i = 0; i < this.m_allRooms.Count; i++)
			{
				RoomHandler roomHandler = this.m_allRooms[i];
				for (int j = 0; j < otherCanvas.m_allRooms.Count; j++)
				{
					RoomHandler roomHandler2 = otherCanvas.m_allRooms[j];
					bboxPrepassResults.numPairs++;
					int num = 0;
					if (IntVector2.AABBOverlapWithArea(roomHandler.area.basePosition, roomHandler.area.dimensions, roomHandler2.area.basePosition + otherCanvasOffset, roomHandler2.area.dimensions, out num))
					{
						bboxPrepassResults.overlapping = true;
						bboxPrepassResults.numPairsOverlapping++;
						bboxPrepassResults.totalOverlapArea += num;
					}
				}
			}
			return bboxPrepassResults;
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x001E74A8 File Offset: 0x001E56A8
		private bool CheckRectangleDecompositionCollisions(SemioticLayoutManager otherCanvas, IntVector2 otherCanvasOffset)
		{
			for (int i = 0; i < otherCanvas.m_rectangleDecomposition.Count; i++)
			{
				Tuple<IntVector2, IntVector2> tuple = otherCanvas.m_rectangleDecomposition[i];
				for (int j = 0; j < this.m_rectangleDecomposition.Count; j++)
				{
					Tuple<IntVector2, IntVector2> tuple2 = this.m_rectangleDecomposition[j];
					if (IntVector2.AABBOverlap(tuple.First + otherCanvasOffset, tuple.Second, tuple2.First, tuple2.Second))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x001E7534 File Offset: 0x001E5734
		public IEnumerable FindNearestValidLocationForLayout2(SemioticLayoutManager canvas, RuntimeRoomExitData staticExit, RuntimeRoomExitData newExit, IntVector2 staticAreaBasePosition, IntVector2 newAreaBasePosition)
		{
			this.FindNearestValidLocataionForLayout2Success = false;
			IntVector2 currentPosition = newAreaBasePosition;
			this.FindNearestValidLocationForLayout2Result = currentPosition;
			int ix = 0;
			int iy = 0;
			int dx = 3;
			int dy = 3;
			Queue<IntVector2> spiralPointQueue = new Queue<IntVector2>();
			int iterations = 5000;
			while (iterations > 0)
			{
				iterations--;
				currentPosition = newAreaBasePosition + new IntVector2(ix, iy);
				IntVector2 staticExitCanvasPosition = staticAreaBasePosition + staticExit.ExitOrigin - IntVector2.One;
				IntVector2 newExitCanvasPosition = currentPosition + newExit.ExitOrigin - IntVector2.One;
				IntVector2 canvasTranslation = staticExitCanvasPosition - newExitCanvasPosition;
				spiralPointQueue.Enqueue(canvasTranslation);
				if (ix == 0 && iy >= 0)
				{
					iy += 3;
				}
				if (ix == 0)
				{
					dy *= -1;
				}
				if (iy == 0)
				{
					dx *= -1;
				}
				ix += dx;
				iy += dy;
				if (iterations % 250 == 0)
				{
					yield return null;
				}
			}
			SpiralPointLayoutHandler.spiralOffsets = spiralPointQueue;
			SpiralPointLayoutHandler.nextElementIndex = 0;
			SpiralPointLayoutHandler.currentResultElementIndex = -1;
			int NUM_THREADS = 6;
			Thread[] spiralThreads = new Thread[NUM_THREADS];
			for (int i = 0; i < NUM_THREADS; i++)
			{
				SpiralPointLayoutHandler spiralPointLayoutHandler = new SpiralPointLayoutHandler(this, canvas, i);
				Thread thread = new Thread(new ThreadStart(spiralPointLayoutHandler.ThreadRun));
				spiralThreads[i] = thread;
			}
			try
			{
				for (int j = 0; j < NUM_THREADS; j++)
				{
					spiralThreads[j].Start();
				}
				for (int k = 0; k < NUM_THREADS; k++)
				{
					spiralThreads[k].Join();
				}
			}
			catch (ThreadStateException ex)
			{
				Debug.LogError("WELL THIS FUCKING SUCKS : " + ex.Message);
				SpiralPointLayoutHandler.currentResultElementIndex = -1;
			}
			catch (ThreadInterruptedException ex2)
			{
				Debug.LogError("THIS SUCKS MARGINALLY LESS : " + ex2.Message);
				SpiralPointLayoutHandler.currentResultElementIndex = -1;
			}
			if (SpiralPointLayoutHandler.currentResultElementIndex == -1)
			{
				BraveUtility.Log("Failed iterations on find nearest valid location for layout.", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
				this.FindNearestValidLocationForLayout2Result = IntVector2.Zero;
				this.FindNearestValidLocataionForLayout2Success = false;
			}
			else
			{
				this.FindNearestValidLocationForLayout2Result = SpiralPointLayoutHandler.resultOffset;
				this.FindNearestValidLocataionForLayout2Success = true;
			}
			yield break;
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x001E757C File Offset: 0x001E577C
		public bool FindNearestValidLocationForLayout(SemioticLayoutManager canvas, RuntimeRoomExitData staticExit, RuntimeRoomExitData newExit, IntVector2 staticAreaBasePosition, IntVector2 newAreaBasePosition, out IntVector2 idealPosition)
		{
			IntVector2 intVector = newAreaBasePosition;
			idealPosition = intVector;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = -1;
			int i = 50000;
			while (i > 0)
			{
				i--;
				intVector = newAreaBasePosition + new IntVector2(num, num2);
				IntVector2 intVector2 = staticAreaBasePosition + staticExit.ExitOrigin - IntVector2.One;
				IntVector2 intVector3 = intVector + newExit.ExitOrigin - IntVector2.One;
				IntVector2 intVector4 = intVector2 - intVector3;
				bool flag = true;
				foreach (IntVector2 intVector5 in canvas.m_occupiedCells)
				{
					IntVector2 intVector6 = intVector5 + intVector4;
					if (this.m_occupiedCells.Contains(intVector6))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					idealPosition = intVector;
					return true;
				}
				if (num == num2 || (num < 0 && num == -num2) || (num > 0 && num == 1 - num2))
				{
					int num5 = num3;
					num3 = -num4;
					num4 = num5;
				}
				num += num3;
				num2 += num4;
			}
			BraveUtility.Log("Failed iterations on find nearest valid location for layout.", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
			idealPosition = IntVector2.Zero;
			return false;
		}

		// Token: 0x06005386 RID: 21382 RVA: 0x001E76DC File Offset: 0x001E58DC
		public bool FindNearestValidLocationForRoom(PrototypeDungeonRoom prototype, IntVector2 startPosition, out IntVector2 idealPosition)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = -1;
			int i = 10000;
			while (i > 0)
			{
				i--;
				bool flag = true;
				IntVector2 intVector = startPosition + new IntVector2(num, num2);
				for (int j = -1; j < prototype.Width + 1; j++)
				{
					for (int k = -1; k < prototype.Height + 1; k++)
					{
						IntVector2 intVector2 = intVector + new IntVector2(j, k);
						if (this.m_occupiedCells.Contains(intVector2))
						{
							flag = false;
							goto IL_93;
						}
					}
				}
				IL_93:
				if (flag)
				{
					idealPosition = intVector;
					return true;
				}
				if (num == num2 || (num < 0 && num == -num2) || (num > 0 && num == 1 - num2))
				{
					int num5 = num3;
					num3 = -num4;
					num4 = num5;
				}
				num += num3;
				num2 += num4;
			}
			idealPosition = IntVector2.Zero;
			return false;
		}

		// Token: 0x06005387 RID: 21383 RVA: 0x001E77DC File Offset: 0x001E59DC
		public void StampCellAreaToLayout(RoomHandler newRoom, bool unstamp = false)
		{
			CellArea area = newRoom.area;
			if (!unstamp)
			{
				this.m_allRooms.Add(newRoom);
			}
			else
			{
				this.m_allRooms.Remove(newRoom);
			}
			if (area.prototypeRoom != null)
			{
				List<IntVector2> cellRepresentationIncFacewalls = area.prototypeRoom.GetCellRepresentationIncFacewalls();
				foreach (IntVector2 intVector in cellRepresentationIncFacewalls)
				{
					if (unstamp)
					{
						this.m_occupiedCells.Remove(intVector + area.basePosition);
					}
					else
					{
						this.m_occupiedCells.Add(intVector + area.basePosition);
					}
				}
			}
			else if (area.proceduralCells != null && area.proceduralCells.Count > 0)
			{
				for (int i = 0; i < area.proceduralCells.Count; i++)
				{
					this.m_occupiedCells.Add(area.proceduralCells[i] + area.basePosition);
					for (int j = 0; j < SemioticLayoutManager.LayoutCardinals.Length; j++)
					{
						this.m_occupiedCells.Add(area.proceduralCells[i] + SemioticLayoutManager.LayoutCardinals[j] + area.basePosition);
					}
				}
			}
			else
			{
				for (int k = 0; k < area.dimensions.x; k++)
				{
					for (int l = 0; l < area.dimensions.y; l++)
					{
						IntVector2 intVector2 = new IntVector2(area.basePosition.x + k, area.basePosition.y + l);
						if (unstamp)
						{
							this.m_occupiedCells.Remove(intVector2);
							for (int m = 0; m < SemioticLayoutManager.LayoutCardinals.Length; m++)
							{
								this.m_occupiedCells.Remove(intVector2 + SemioticLayoutManager.LayoutCardinals[m]);
							}
						}
						else
						{
							this.m_occupiedCells.Add(intVector2);
							for (int n = 0; n < SemioticLayoutManager.LayoutCardinals.Length; n++)
							{
								this.m_occupiedCells.Add(intVector2 + SemioticLayoutManager.LayoutCardinals[n]);
							}
						}
					}
				}
			}
			if (this.m_rectangleDecomposition != null)
			{
				this.m_rectangleDecomposition.Clear();
			}
		}

		// Token: 0x06005388 RID: 21384 RVA: 0x001E7A90 File Offset: 0x001E5C90
		public void HandleOffsetRooms(IntVector2 offset)
		{
			this.m_currentOffset += offset;
			for (int i = 0; i < this.m_allRooms.Count; i++)
			{
				RoomHandler roomHandler = this.m_allRooms[i];
				roomHandler.area.basePosition += offset;
				this.m_allRooms[i] = roomHandler;
			}
		}

		// Token: 0x06005389 RID: 21385 RVA: 0x001E7AFC File Offset: 0x001E5CFC
		public IntVector2 GetSafelyBoundedMinimumCellPosition()
		{
			IntVector2 intVector = this.GetMinimumCellPosition();
			for (int i = 0; i < this.m_allRooms.Count; i++)
			{
				intVector = IntVector2.Min(intVector, this.m_allRooms[i].area.basePosition);
			}
			return intVector;
		}

		// Token: 0x0600538A RID: 21386 RVA: 0x001E7B4C File Offset: 0x001E5D4C
		public IntVector2 GetSafelyBoundedMaximumCellPosition()
		{
			IntVector2 intVector = this.GetMaximumCellPosition();
			for (int i = 0; i < this.m_allRooms.Count; i++)
			{
				intVector = IntVector2.Max(intVector, this.m_allRooms[i].area.basePosition + this.m_allRooms[i].area.dimensions);
			}
			return intVector;
		}

		// Token: 0x0600538B RID: 21387 RVA: 0x001E7BB8 File Offset: 0x001E5DB8
		public Tuple<IntVector2, IntVector2> GetMinAndMaxCellPositions()
		{
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			int num3 = int.MinValue;
			int num4 = int.MinValue;
			foreach (IntVector2 intVector in this.m_occupiedCells)
			{
				num = Math.Min(num, intVector.x);
				num2 = Math.Min(num2, intVector.y);
				num3 = Math.Max(num3, intVector.x);
				num4 = Math.Max(num4, intVector.y);
			}
			return new Tuple<IntVector2, IntVector2>(new IntVector2(num, num2), new IntVector2(num3, num4));
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x001E7C74 File Offset: 0x001E5E74
		public IntVector2 GetMinimumCellPosition()
		{
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			foreach (IntVector2 intVector in this.m_occupiedCells)
			{
				num = Math.Min(num, intVector.x);
				num2 = Math.Min(num2, intVector.y);
			}
			return new IntVector2(num, num2);
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x001E7CF8 File Offset: 0x001E5EF8
		public IntVector2 GetMaximumCellPosition()
		{
			int num = int.MinValue;
			int num2 = int.MinValue;
			foreach (IntVector2 intVector in this.m_occupiedCells)
			{
				num = Math.Max(num, intVector.x);
				num2 = Math.Max(num2, intVector.y);
			}
			return new IntVector2(num, num2);
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x001E7D7C File Offset: 0x001E5F7C
		public bool CanPlaceCellBounds(CellArea newArea)
		{
			for (int i = 0; i < this.m_allRooms.Count; i++)
			{
				if (this.m_allRooms[i].area.OverlapsWithUnitBorder(newArea))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x001E7DC4 File Offset: 0x001E5FC4
		private bool CheckExitsClearForPlacement(PrototypeDungeonRoom newRoom, RuntimeRoomExitData exitToTest, IntVector2 attachPoint)
		{
			IntVector2 intVector = attachPoint - (exitToTest.ExitOrigin - IntVector2.One);
			return this.CheckExitClearForPlacement(exitToTest, intVector, false, null);
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x001E7DF4 File Offset: 0x001E5FF4
		private Tuple<IntVector2, IntVector2> GetExitRectCells(RuntimeRoomExitData exit, IntVector2 areaBasePosition)
		{
			PrototypeRoomExit referencedExit = exit.referencedExit;
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(referencedExit.exitDirection);
			int num = ((!exit.jointedExit || referencedExit.exitDirection == DungeonData.Direction.WEST) ? 0 : 1);
			if (exit.jointedExit)
			{
				num++;
			}
			int num2 = exit.TotalExitLength + num;
			num2 = Mathf.Max(4, num2);
			int num3 = int.MaxValue;
			int num4 = int.MaxValue;
			int num5 = int.MinValue;
			int num6 = int.MinValue;
			for (int i = 0; i < referencedExit.containedCells.Count; i++)
			{
				num3 = Mathf.Min((int)referencedExit.containedCells[i].x, num3);
				num4 = Mathf.Min((int)referencedExit.containedCells[i].y, num4);
				num5 = Mathf.Max((int)referencedExit.containedCells[i].x, num5);
				num6 = Mathf.Max((int)referencedExit.containedCells[i].y, num6);
			}
			IntVector2 intVector = new IntVector2(num3, num4) - IntVector2.One;
			IntVector2 intVector2 = new IntVector2(num5, num6) + IntVector2.One;
			IntVector2 intVector3 = intVector + areaBasePosition + intVector2FromDirection * 3;
			IntVector2 intVector4 = intVector2 + areaBasePosition + intVector2FromDirection * num2;
			IntVector2 intVector5 = IntVector2.Min(intVector3, intVector4);
			IntVector2 intVector6 = IntVector2.Max(intVector4, intVector3);
			if (!exit.jointedExit && (referencedExit.exitDirection == DungeonData.Direction.NORTH || referencedExit.exitDirection == DungeonData.Direction.SOUTH))
			{
				intVector6 += new IntVector2(1, 0);
				intVector5 -= new IntVector2(1, 0);
			}
			else
			{
				intVector6 += new IntVector2(2, 3);
				intVector5 -= new IntVector2(2, 2);
			}
			return new Tuple<IntVector2, IntVector2>(intVector5, intVector6);
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x001E7FEC File Offset: 0x001E61EC
		private bool CheckRectAgainstLayout(Tuple<IntVector2, IntVector2> rectTuple, SemioticLayoutManager layout)
		{
			for (int i = rectTuple.First.x; i < rectTuple.Second.x; i++)
			{
				for (int j = rectTuple.First.y; j < rectTuple.Second.y; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (layout.m_occupiedCells.Contains(intVector))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x001E8064 File Offset: 0x001E6264
		public IEnumerable<ProcessStatus> CheckExitsAgainstDisparateLayouts(SemioticLayoutManager otherLayout, RuntimeRoomExitData staticExit, IntVector2 staticAreaBasePosition, RuntimeRoomExitData newExit, IntVector2 newAreaBasePosition)
		{
			IntVector2 staticExitCanvasPosition = staticAreaBasePosition + staticExit.ExitOrigin - IntVector2.One;
			IntVector2 newExitCanvasPosition = newAreaBasePosition + newExit.ExitOrigin - IntVector2.One;
			IntVector2 canvasTranslation = staticExitCanvasPosition - newExitCanvasPosition;
			Tuple<IntVector2, IntVector2> staticRect = this.GetExitRectCells(staticExit, staticAreaBasePosition);
			Tuple<IntVector2, IntVector2> newRect = this.GetExitRectCells(newExit, newAreaBasePosition + canvasTranslation);
			yield return ProcessStatus.Incomplete;
			Tuple<IntVector2, IntVector2> staticRectOther = new Tuple<IntVector2, IntVector2>(staticRect.First - canvasTranslation, staticRect.Second - canvasTranslation);
			Tuple<IntVector2, IntVector2> newRectOther = new Tuple<IntVector2, IntVector2>(newRect.First - canvasTranslation, newRect.Second - canvasTranslation);
			bool firstCheck = this.CheckRectAgainstLayout(staticRect, this);
			if (!firstCheck)
			{
				this.m_FIRST_FAILS++;
				yield return ProcessStatus.Fail;
				yield break;
			}
			bool secondCheck = this.CheckRectAgainstLayout(newRect, this);
			if (!secondCheck)
			{
				this.m_SECOND_FAILS++;
				yield return ProcessStatus.Fail;
				yield break;
			}
			yield return ProcessStatus.Incomplete;
			bool thirdCheck = this.CheckRectAgainstLayout(staticRectOther, otherLayout);
			if (!thirdCheck)
			{
				this.m_THIRD_FAILS++;
				yield return ProcessStatus.Fail;
				yield break;
			}
			bool fourthCheck = this.CheckRectAgainstLayout(newRectOther, otherLayout);
			if (!fourthCheck)
			{
				this.m_FOURTH_FAILS++;
				yield return ProcessStatus.Fail;
				yield break;
			}
			if (firstCheck && secondCheck && thirdCheck && fourthCheck)
			{
				yield return ProcessStatus.Success;
				yield break;
			}
			yield return ProcessStatus.Fail;
			yield break;
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x001E80AC File Offset: 0x001E62AC
		private bool CheckExitClearForPlacement(RuntimeRoomExitData exit, IntVector2 areaBasePosition, bool debugMode = false, SemioticLayoutManager debugManager = null)
		{
			PrototypeRoomExit referencedExit = exit.referencedExit;
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(referencedExit.exitDirection);
			int num = ((!exit.jointedExit || referencedExit.exitDirection == DungeonData.Direction.WEST) ? 0 : 1);
			for (int i = 0; i < referencedExit.containedCells.Count; i++)
			{
				int num2 = 3;
				for (int j = num2; j < exit.TotalExitLength + num; j++)
				{
					IntVector2 intVector = referencedExit.containedCells[i].ToIntVector2(VectorConversions.Round) - IntVector2.One + areaBasePosition + intVector2FromDirection * j;
					if (this.m_occupiedCells.Contains(intVector))
					{
						return false;
					}
					for (int k = 0; k < SemioticLayoutManager.LayoutCardinals.Length; k++)
					{
						if (this.m_occupiedCells.Contains(intVector + SemioticLayoutManager.LayoutCardinals[k]))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x001E81B4 File Offset: 0x001E63B4
		private bool CheckExitsClearForPlacement(RuntimeRoomExitData exitToTest, IntVector2 attachPoint)
		{
			IntVector2 intVector = attachPoint - (exitToTest.ExitOrigin - IntVector2.One);
			return this.CheckExitClearForPlacement(exitToTest, intVector, false, null);
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x001E81E4 File Offset: 0x001E63E4
		private bool CheckExitsClearForPlacement(PrototypeDungeonRoom newRoom, RuntimeRoomExitData exitToTest, IntVector2 basePositionOfPreviousRoom, RuntimeRoomExitData previousExit, IntVector2 attachPoint)
		{
			IntVector2 intVector = attachPoint - (exitToTest.ExitOrigin - IntVector2.One);
			return this.CheckExitClearForPlacement(exitToTest, intVector, false, null);
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x001E8214 File Offset: 0x001E6414
		private bool CheckExitsClearForPlacement2(PrototypeDungeonRoom newRoom, RuntimeRoomExitData exitToTest, IntVector2 basePositionOfPreviousRoom, RuntimeRoomExitData previousExit, IntVector2 attachPoint)
		{
			IntVector2 intVector = attachPoint - (exitToTest.ExitOrigin - IntVector2.One);
			IntVector2 intVector2 = attachPoint - (previousExit.ExitOrigin - IntVector2.One);
			Tuple<IntVector2, IntVector2> exitRectCells = this.GetExitRectCells(exitToTest, intVector);
			Tuple<IntVector2, IntVector2> exitRectCells2 = this.GetExitRectCells(previousExit, intVector2);
			return this.CheckRectAgainstLayout(exitRectCells, this) && this.CheckRectAgainstLayout(exitRectCells2, this);
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x001E8280 File Offset: 0x001E6480
		public IEnumerable CanPlaceLayoutAtPoint(SemioticLayoutManager layout, RuntimeRoomExitData staticExit, RuntimeRoomExitData newExit, IntVector2 staticAreaBasePosition, IntVector2 newAreaBasePosition)
		{
			this.CanPlaceLayoutAtPointSuccess = false;
			staticExit.additionalExitLength = 0;
			newExit.additionalExitLength = 0;
			Tuple<PrototypeRoomExit, PrototypeRoomExit> exitPair = new Tuple<PrototypeRoomExit, PrototypeRoomExit>(staticExit.referencedExit, newExit.referencedExit);
			bool isInterestingCombo = false;
			if (((exitPair.First.exitDirection == DungeonData.Direction.NORTH || exitPair.First.exitDirection == DungeonData.Direction.SOUTH) && (exitPair.Second.exitDirection == DungeonData.Direction.EAST || exitPair.Second.exitDirection == DungeonData.Direction.WEST)) || ((exitPair.Second.exitDirection == DungeonData.Direction.NORTH || exitPair.Second.exitDirection == DungeonData.Direction.SOUTH) && (exitPair.First.exitDirection == DungeonData.Direction.EAST || exitPair.First.exitDirection == DungeonData.Direction.WEST)))
			{
				isInterestingCombo = true;
			}
			if (isInterestingCombo)
			{
				newExit.additionalExitLength = 3;
			}
			staticExit.jointedExit = isInterestingCombo;
			newExit.jointedExit = isInterestingCombo;
			IntVector2 initialExitOffsets = new IntVector2(staticExit.additionalExitLength, newExit.additionalExitLength);
			staticExit.additionalExitLength = initialExitOffsets.x;
			newExit.additionalExitLength = initialExitOffsets.y;
			IntVector2 staticExitCanvasPosition = staticAreaBasePosition + staticExit.ExitOrigin - IntVector2.One;
			IntVector2 newExitCanvasPosition = newAreaBasePosition + newExit.ExitOrigin - IntVector2.One;
			HashSet<IntVector2> problemChildren = new HashSet<IntVector2>();
			int EXIT_FAILS = 0;
			int CELL_FAILS = 0;
			this.m_FIRST_FAILS = (this.m_SECOND_FAILS = (this.m_THIRD_FAILS = (this.m_FOURTH_FAILS = 0)));
			int modHallwayExtensionMax = 12;
			for (int diag = 0; diag < modHallwayExtensionMax * 2 - 1; diag++)
			{
				int numCellsInDiag = Mathf.RoundToInt(Mathf.PingPong((float)diag, (float)modHallwayExtensionMax));
				IntVector2 diagInitialCell = new IntVector2(Mathf.Clamp(diag - modHallwayExtensionMax, 0, modHallwayExtensionMax - 1), Mathf.Clamp(diag, 0, modHallwayExtensionMax - 1));
				for (int diagCoord = 0; diagCoord < numCellsInDiag; diagCoord++)
				{
					IntVector2 currentCell = diagInitialCell + new IntVector2(1, -1) * diagCoord;
					staticExit.additionalExitLength = initialExitOffsets.x + currentCell.x;
					newExit.additionalExitLength = initialExitOffsets.y + currentCell.y;
					staticExitCanvasPosition = staticAreaBasePosition + staticExit.ExitOrigin - IntVector2.One;
					newExitCanvasPosition = newAreaBasePosition + newExit.ExitOrigin - IntVector2.One;
					IntVector2 canvasTranslation = staticExitCanvasPosition - newExitCanvasPosition;
					bool exitCheckSucceeded = false;
					IEnumerator<ProcessStatus> ExitCheckTracker = this.CheckExitsAgainstDisparateLayouts(layout, staticExit, staticAreaBasePosition, newExit, newAreaBasePosition).GetEnumerator();
					while (ExitCheckTracker.MoveNext())
					{
						if (ExitCheckTracker.Current == ProcessStatus.Success)
						{
							exitCheckSucceeded = true;
							break;
						}
					}
					if (!exitCheckSucceeded)
					{
						EXIT_FAILS++;
					}
					else
					{
						bool success = true;
						int iterator = 0;
						foreach (IntVector2 problemChild in problemChildren)
						{
							iterator++;
							IntVector2 readjustedPoint = problemChild + canvasTranslation;
							if (this.m_occupiedCells.Contains(readjustedPoint))
							{
								success = false;
								CELL_FAILS++;
								break;
							}
							if (iterator % 600 == 0)
							{
								yield return null;
							}
						}
						if (success)
						{
							foreach (IntVector2 cellPosition in layout.m_occupiedCells)
							{
								iterator++;
								IntVector2 adjustedPosition = cellPosition + canvasTranslation;
								if (this.m_occupiedCells.Contains(adjustedPosition))
								{
									success = false;
									CELL_FAILS++;
									problemChildren.Add(cellPosition);
									break;
								}
								if (iterator % 600 == 0)
								{
									yield return null;
								}
							}
							if (success)
							{
								this.CanPlaceLayoutAtPointSuccess = true;
								yield break;
							}
							yield return null;
						}
					}
				}
			}
			this.CanPlaceLayoutAtPointSuccess = false;
			yield break;
		}

		// Token: 0x06005398 RID: 21400 RVA: 0x001E82C8 File Offset: 0x001E64C8
		public bool CanPlaceRoomAtAttachPointByExit2(PrototypeDungeonRoom newRoom, RuntimeRoomExitData exitToTest, IntVector2 basePositionOfPreviousRoom, RuntimeRoomExitData previousExit)
		{
			exitToTest.additionalExitLength = 0;
			previousExit.additionalExitLength = 0;
			Tuple<PrototypeRoomExit, PrototypeRoomExit> tuple = new Tuple<PrototypeRoomExit, PrototypeRoomExit>(exitToTest.referencedExit, previousExit.referencedExit);
			bool flag = false;
			if (((tuple.First.exitDirection == DungeonData.Direction.NORTH || tuple.First.exitDirection == DungeonData.Direction.SOUTH) && (tuple.Second.exitDirection == DungeonData.Direction.EAST || tuple.Second.exitDirection == DungeonData.Direction.WEST)) || ((tuple.Second.exitDirection == DungeonData.Direction.NORTH || tuple.Second.exitDirection == DungeonData.Direction.SOUTH) && (tuple.First.exitDirection == DungeonData.Direction.EAST || tuple.First.exitDirection == DungeonData.Direction.WEST)))
			{
				flag = true;
			}
			if (flag)
			{
				exitToTest.additionalExitLength = 3;
			}
			IntVector2 intVector = new IntVector2(exitToTest.additionalExitLength, previousExit.additionalExitLength);
			for (int i = 0; i < 7; i++)
			{
				int num = Mathf.RoundToInt(Mathf.PingPong((float)i, 4f));
				IntVector2 intVector2 = new IntVector2(Mathf.Clamp(i - 4, 0, 3), Mathf.Clamp(i, 0, 3));
				for (int j = 0; j < num; j++)
				{
					IntVector2 intVector3 = intVector2 + new IntVector2(1, -1) * j;
					exitToTest.additionalExitLength = intVector.x + intVector3.x;
					previousExit.additionalExitLength = intVector.y + intVector3.y;
					IntVector2 intVector4 = basePositionOfPreviousRoom + previousExit.ExitOrigin - IntVector2.One;
					IntVector2 intVector5 = intVector4 - (exitToTest.ExitOrigin - IntVector2.One);
					int num2 = intVector5.x - 1;
					int num3 = intVector5.x + newRoom.Width + 2;
					int num4 = intVector5.y - 1;
					int num5 = intVector5.y + newRoom.Height + 4;
					bool flag2 = false;
					if (this.CheckExitsClearForPlacement2(newRoom, exitToTest, basePositionOfPreviousRoom, previousExit, intVector4))
					{
						bool flag3 = true;
						for (int k = 0; k < this.m_allRooms.Count; k++)
						{
							CellArea area = this.m_allRooms[k].area;
							int num6 = area.basePosition.x - 1;
							int num7 = area.basePosition.x + area.dimensions.x + 2;
							int num8 = area.basePosition.y - 1;
							int num9 = area.basePosition.y + area.dimensions.y + 4;
							if (num2 < num7 && num3 > num6 && num4 < num9 && num5 > num8)
							{
								flag3 = false;
								break;
							}
						}
						if (!flag3 || true)
						{
							List<IntVector2> cellRepresentationIncFacewalls = newRoom.GetCellRepresentationIncFacewalls();
							for (int l = 0; l < cellRepresentationIncFacewalls.Count; l++)
							{
								if (flag2)
								{
									break;
								}
								if (this.m_occupiedCells.Contains(intVector5 + cellRepresentationIncFacewalls[l]))
								{
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2)
						{
							if (flag)
							{
								exitToTest.jointedExit = true;
								previousExit.jointedExit = true;
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005399 RID: 21401 RVA: 0x001E8608 File Offset: 0x001E6808
		public bool CanPlaceRawCellPositions(List<IntVector2> positions)
		{
			for (int i = 0; i < positions.Count; i++)
			{
				IntVector2 intVector = positions[i];
				if (this.m_occupiedCells.Contains(intVector))
				{
					return false;
				}
				for (int j = 0; j < SemioticLayoutManager.LayoutPathCardinals.Length; j++)
				{
					if (this.m_occupiedCells.Contains(intVector + SemioticLayoutManager.LayoutPathCardinals[j]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x001E8688 File Offset: 0x001E6888
		public IEnumerable<ProcessStatus> CanPlaceRoomAtAttachPointByExit(PrototypeDungeonRoom newRoom, RuntimeRoomExitData exitToTest, IntVector2 basePositionOfPreviousRoom, RuntimeRoomExitData previousExit)
		{
			exitToTest.additionalExitLength = 0;
			previousExit.additionalExitLength = 0;
			Tuple<PrototypeRoomExit, PrototypeRoomExit> exitPair = new Tuple<PrototypeRoomExit, PrototypeRoomExit>(exitToTest.referencedExit, previousExit.referencedExit);
			bool isInterestingCombo = false;
			if (((exitPair.First.exitDirection == DungeonData.Direction.NORTH || exitPair.First.exitDirection == DungeonData.Direction.SOUTH) && (exitPair.Second.exitDirection == DungeonData.Direction.EAST || exitPair.Second.exitDirection == DungeonData.Direction.WEST)) || ((exitPair.Second.exitDirection == DungeonData.Direction.NORTH || exitPair.Second.exitDirection == DungeonData.Direction.SOUTH) && (exitPair.First.exitDirection == DungeonData.Direction.EAST || exitPair.First.exitDirection == DungeonData.Direction.WEST)))
			{
				isInterestingCombo = true;
			}
			if (isInterestingCombo)
			{
				exitToTest.additionalExitLength = 3;
			}
			IntVector2 initialExitOffsets = new IntVector2(exitToTest.additionalExitLength, previousExit.additionalExitLength);
			for (int diag = 0; diag < 7; diag++)
			{
				int numCellsInDiag = Mathf.RoundToInt(Mathf.PingPong((float)diag, 4f));
				IntVector2 diagInitialCell = new IntVector2(Mathf.Clamp(diag - 4, 0, 3), Mathf.Clamp(diag, 0, 3));
				for (int diagCoord = 0; diagCoord < numCellsInDiag; diagCoord++)
				{
					IntVector2 currentCell = diagInitialCell + new IntVector2(1, -1) * diagCoord;
					exitToTest.additionalExitLength = initialExitOffsets.x + currentCell.x;
					previousExit.additionalExitLength = initialExitOffsets.y + currentCell.y;
					IntVector2 attachPoint = basePositionOfPreviousRoom + previousExit.ExitOrigin - IntVector2.One;
					IntVector2 baseWorldPositionOfNewRoom = attachPoint - (exitToTest.ExitOrigin - IntVector2.One);
					int RectAX = baseWorldPositionOfNewRoom.x - 1;
					int RectAX2 = baseWorldPositionOfNewRoom.x + newRoom.Width + 2;
					int RectAY = baseWorldPositionOfNewRoom.y - 1;
					int RectAY2 = baseWorldPositionOfNewRoom.y + newRoom.Height + 4;
					bool failedPlacement = false;
					if (this.CheckExitsClearForPlacement(newRoom, exitToTest, basePositionOfPreviousRoom, previousExit, attachPoint))
					{
						bool broadphaseSuccess = true;
						for (int j = 0; j < this.m_allRooms.Count; j++)
						{
							CellArea area = this.m_allRooms[j].area;
							int num = area.basePosition.x - 1;
							int num2 = area.basePosition.x + area.dimensions.x + 2;
							int num3 = area.basePosition.y - 1;
							int num4 = area.basePosition.y + area.dimensions.y + 4;
							if (RectAX < num2 && RectAX2 > num && RectAY < num4 && RectAY2 > num3)
							{
								broadphaseSuccess = false;
								break;
							}
						}
						if (broadphaseSuccess)
						{
							foreach (IntVector2 intVector in this.m_exitTestPoints)
							{
								if (RectAX < intVector.x && RectAX2 > intVector.x && RectAY < intVector.y && RectAY2 > intVector.y)
								{
									broadphaseSuccess = false;
									break;
								}
							}
						}
						if (!broadphaseSuccess)
						{
							yield return ProcessStatus.Incomplete;
							List<IntVector2> newRoomPotentialCells = newRoom.GetCellRepresentationIncFacewalls();
							int iterator = 0;
							for (int i = 0; i < newRoomPotentialCells.Count; i++)
							{
								if (failedPlacement)
								{
									break;
								}
								iterator++;
								if (this.m_occupiedCells.Contains(baseWorldPositionOfNewRoom + newRoomPotentialCells[i]))
								{
									failedPlacement = true;
									break;
								}
								if (iterator % 1000 == 0)
								{
									yield return ProcessStatus.Incomplete;
								}
							}
						}
						if (!failedPlacement)
						{
							if (isInterestingCombo)
							{
								exitToTest.jointedExit = true;
								previousExit.jointedExit = true;
							}
							yield return ProcessStatus.Success;
							yield break;
						}
						yield return ProcessStatus.Incomplete;
					}
				}
			}
			yield return ProcessStatus.Fail;
			yield break;
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x001E86C8 File Offset: 0x001E68C8
		public bool CanPlaceRoomAtAttachPointByExit(PrototypeDungeonRoom newRoom, PrototypeRoomExit exitToTest, IntVector2 attachPoint)
		{
			IntVector2 intVector = attachPoint - (exitToTest.GetExitOrigin(exitToTest.exitLength) - IntVector2.One);
			int num = intVector.x - 1;
			int num2 = intVector.x + newRoom.Width + 2;
			int num3 = intVector.y - 1;
			int num4 = intVector.y + newRoom.Height + 4;
			bool flag = false;
			if (!this.CheckExitsClearForPlacement(newRoom, new RuntimeRoomExitData(exitToTest), attachPoint))
			{
				return false;
			}
			bool flag2 = true;
			for (int i = 0; i < this.m_allRooms.Count; i++)
			{
				CellArea area = this.m_allRooms[i].area;
				int num5 = area.basePosition.x - 1;
				int num6 = area.basePosition.x + area.dimensions.x + 2;
				int num7 = area.basePosition.y - 1;
				int num8 = area.basePosition.y + area.dimensions.y + 4;
				if (num < num6 && num2 > num5 && num3 < num8 && num4 > num7)
				{
					flag2 = false;
					break;
				}
			}
			if (!flag2)
			{
				List<IntVector2> cellRepresentationIncFacewalls = newRoom.GetCellRepresentationIncFacewalls();
				for (int j = 0; j < cellRepresentationIncFacewalls.Count; j++)
				{
					if (flag)
					{
						break;
					}
					if (this.m_occupiedCells.Contains(intVector + cellRepresentationIncFacewalls[j]))
					{
						flag = true;
						break;
					}
				}
			}
			return !flag;
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x001E8864 File Offset: 0x001E6A64
		public void ReinitializeFromLayout(SemioticLayoutManager snapshot)
		{
			this.m_allRooms = new List<RoomHandler>(snapshot.m_allRooms);
			this.m_occupiedCells = new HashSet<IntVector2>(snapshot.m_occupiedCells);
			this.m_currentOffset = snapshot.m_currentOffset;
		}

		// Token: 0x0600539D RID: 21405 RVA: 0x001E8894 File Offset: 0x001E6A94
		public void MergeLayout(SemioticLayoutManager other)
		{
			for (int i = 0; i < other.Rooms.Count; i++)
			{
				this.m_allRooms.Add(other.Rooms[i]);
			}
			foreach (IntVector2 intVector in other.m_occupiedCells)
			{
				this.m_occupiedCells.Add(other.m_currentOffset + intVector);
			}
			if (this.m_rectangleDecomposition != null)
			{
				this.m_rectangleDecomposition.Clear();
			}
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x001E894C File Offset: 0x001E6B4C
		public bool CanPlacePathHallway(List<IntVector2> cellPositions)
		{
			for (int i = 0; i < cellPositions.Count; i++)
			{
				if (this.m_occupiedCells.Contains(cellPositions[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x001E898C File Offset: 0x001E6B8C
		public bool CanPlaceRectangle(IntRect rectangle)
		{
			for (int i = rectangle.Left - 1; i < rectangle.Right + 1; i++)
			{
				for (int j = rectangle.Bottom - 1; j < rectangle.Top + 1; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (this.m_occupiedCells.Contains(intVector))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x001E89F8 File Offset: 0x001E6BF8
		private bool CheckCellAndNeighborsOccupied(IntVector2 position)
		{
			if (this.m_occupiedCells.Contains(position))
			{
				return true;
			}
			for (int i = 0; i < 4; i++)
			{
				if (this.m_occupiedCells.Contains(position + IntVector2.Cardinals[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x001E8A54 File Offset: 0x001E6C54
		public List<IntVector2> PathfindHallway(IntVector2 startPosition, IntVector2 endPosition)
		{
			return this.PathfindHallwayCompact(startPosition, IntVector2.Zero, endPosition);
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x001E8A64 File Offset: 0x001E6C64
		public List<IntVector2> PathfindHallwayCompact(IntVector2 startPosition, IntVector2 startDirection, IntVector2 endPosition)
		{
			IntVector2 intVector = IntVector2.Min(startPosition, endPosition);
			IntVector2 intVector2 = IntVector2.Max(startPosition, endPosition);
			IntVector2 intVector3 = intVector2 - intVector;
			IntVector2 intVector4 = intVector * -1;
			IntVector2 intVector5 = new IntVector2(4, 4);
			int num = intVector3.x + intVector5.x * 2;
			int num2 = intVector3.y + intVector5.y * 2;
			int num3 = Mathf.NextPowerOfTwo(Mathf.Max(num, num2));
			byte[,] array = new byte[num3, num3];
			byte b = 0;
			byte b2 = 1;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (i > num || j > num2)
					{
						array[i, j] = b;
					}
					else
					{
						array[i, j] = b2;
					}
				}
			}
			foreach (IntVector2 intVector6 in this.m_occupiedCells)
			{
				int num4 = intVector6.x + intVector4.x + intVector5.x;
				int num5 = intVector6.y + intVector4.y + intVector5.y;
				if (num4 >= 3 && num4 < num - 3 && num5 >= 3 && num5 < num2 - 3)
				{
					for (int k = -3; k < 4; k++)
					{
						for (int l = -3; l < 4; l++)
						{
							array[num4 + k, num5 + l] = b;
						}
					}
				}
			}
			foreach (IntVector2 intVector7 in this.m_temporaryPathfindingWalls)
			{
				IntVector2 intVector8 = intVector7 + intVector4 + intVector5;
				if (intVector8.x >= 1 && intVector8.x <= array.GetLength(0) - 2 && intVector8.y >= 1 && intVector8.y <= array.GetLength(1) - 4)
				{
					array[intVector8.x, intVector8.y] = b;
				}
			}
			FastDungeonLayoutPathfinder fastDungeonLayoutPathfinder = new FastDungeonLayoutPathfinder(array);
			fastDungeonLayoutPathfinder.Diagonals = false;
			fastDungeonLayoutPathfinder.PunishChangeDirection = true;
			fastDungeonLayoutPathfinder.TieBreaker = true;
			IntVector2 intVector9 = startPosition + intVector4 + intVector5;
			IntVector2 intVector10 = endPosition + intVector4 + intVector5;
			List<PathFinderNode> list = fastDungeonLayoutPathfinder.FindPath(intVector9, startDirection, intVector10);
			if (list == null || list.Count == 0)
			{
				return null;
			}
			List<IntVector2> list2 = new List<IntVector2>();
			for (int m = 0; m < list.Count; m++)
			{
				IntVector2 intVector11 = new IntVector2(list[m].X, list[m].Y) - intVector4 - intVector5;
				list2.Add(intVector11);
			}
			return list2;
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x001E8DB4 File Offset: 0x001E6FB4
		public List<IntVector2> PathfindHallway(IntVector2 startPosition, IntVector2 startDirection, IntVector2 endPosition)
		{
			Tuple<IntVector2, IntVector2> minAndMaxCellPositions = this.GetMinAndMaxCellPositions();
			IntVector2 intVector = minAndMaxCellPositions.Second - minAndMaxCellPositions.First;
			IntVector2 intVector2 = IntVector2.Max(IntVector2.Zero, IntVector2.Zero - minAndMaxCellPositions.First);
			IntVector2 intVector3 = new IntVector2(8, 8);
			int num = intVector.x + intVector3.x * 2;
			int num2 = intVector.y + intVector3.y * 2;
			int num3 = Mathf.NextPowerOfTwo(Mathf.Max(num, num2));
			byte[,] array = new byte[num3, num3];
			byte b = 0;
			byte b2 = 1;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					array[i, j] = b2;
				}
			}
			foreach (IntVector2 intVector4 in this.m_occupiedCells)
			{
				int num4 = intVector4.x + intVector2.x + intVector3.x;
				int num5 = intVector4.y + intVector2.y + intVector3.y;
				for (int k = -3; k < 4; k++)
				{
					for (int l = -3; l < 4; l++)
					{
						array[num4 + k, num5 + l] = b;
					}
				}
			}
			foreach (IntVector2 intVector5 in this.m_temporaryPathfindingWalls)
			{
				IntVector2 intVector6 = intVector5 + intVector2 + intVector3;
				if (intVector6.x >= 1 && intVector6.x <= array.GetLength(0) - 2 && intVector6.y >= 1 && intVector6.y <= array.GetLength(1) - 4)
				{
					array[intVector6.x, intVector6.y] = b;
				}
			}
			FastDungeonLayoutPathfinder fastDungeonLayoutPathfinder = new FastDungeonLayoutPathfinder(array);
			fastDungeonLayoutPathfinder.Diagonals = false;
			fastDungeonLayoutPathfinder.PunishChangeDirection = true;
			fastDungeonLayoutPathfinder.TieBreaker = true;
			IntVector2 intVector7 = startPosition + intVector2 + intVector3;
			IntVector2 intVector8 = endPosition + intVector2 + intVector3;
			List<PathFinderNode> list = fastDungeonLayoutPathfinder.FindPath(intVector7, startDirection, intVector8);
			if (list == null || list.Count == 0)
			{
				return null;
			}
			List<IntVector2> list2 = new List<IntVector2>();
			for (int m = 0; m < list.Count; m++)
			{
				IntVector2 intVector9 = new IntVector2(list[m].X, list[m].Y) - intVector2 - intVector3;
				list2.Add(intVector9);
			}
			return list2;
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x001E90C8 File Offset: 0x001E72C8
		public List<IntVector2> TraceHallway(IntVector2 startPosition, IntVector2 endPosition, DungeonData.Direction currentHallwayDirection, DungeonData.Direction endHallwayDirection)
		{
			IntVector2 intVector = startPosition;
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			for (int i = 0; i < 3; i++)
			{
				hashSet.Add(intVector);
				hashSet.Add(intVector + IntVector2.Up);
				hashSet.Add(intVector + IntVector2.Right);
				hashSet.Add(intVector + IntVector2.Up + IntVector2.Right);
				intVector += DungeonData.GetIntVector2FromDirection(currentHallwayDirection);
				hashSet.Add(endPosition);
				hashSet.Add(endPosition + IntVector2.Up);
				hashSet.Add(endPosition + IntVector2.Right);
				hashSet.Add(endPosition + IntVector2.Up + IntVector2.Right);
				endPosition += DungeonData.GetIntVector2FromDirection(endHallwayDirection);
			}
			IntVector2 intVector2 = endPosition - intVector;
			DungeonData.Direction direction = ((intVector2.x <= 0) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST);
			DungeonData.Direction direction2 = ((intVector2.y <= 0) ? DungeonData.Direction.SOUTH : DungeonData.Direction.NORTH);
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(direction);
			IntVector2 intVector2FromDirection2 = DungeonData.GetIntVector2FromDirection(direction2);
			if (currentHallwayDirection != direction && currentHallwayDirection != direction2)
			{
				return null;
			}
			bool flag = true;
			DungeonData.Direction direction3 = currentHallwayDirection;
			int num = 0;
			while (intVector != endPosition && num < 200)
			{
				num++;
				bool flag2 = direction == direction3;
				intVector2 = endPosition - intVector;
				if (flag2)
				{
					bool flag3 = true;
					bool flag4 = true;
					if (intVector2.x == 0 || Mathf.Sign((float)intVector2.x) != Mathf.Sign((float)intVector2FromDirection.x))
					{
						flag3 = false;
					}
					else if (this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection) || this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection + IntVector2.Up))
					{
						flag3 = false;
					}
					if (flag3)
					{
						intVector += intVector2FromDirection;
						hashSet.Add(intVector);
						hashSet.Add(intVector + IntVector2.Right);
						hashSet.Add(intVector + IntVector2.Up);
						hashSet.Add(intVector + IntVector2.Right + IntVector2.Up);
						direction3 = direction;
						continue;
					}
					if (intVector2.y == 0 || Mathf.Sign((float)intVector2.y) != Mathf.Sign((float)intVector2FromDirection2.y))
					{
						flag4 = false;
					}
					else if (this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection2) || this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection2 + IntVector2.Right))
					{
						flag4 = false;
					}
					if (flag4)
					{
						intVector += intVector2FromDirection2;
						hashSet.Add(intVector);
						hashSet.Add(intVector + IntVector2.Right);
						hashSet.Add(intVector + IntVector2.Up);
						hashSet.Add(intVector + IntVector2.Right + IntVector2.Up);
						direction3 = direction2;
						continue;
					}
				}
				else
				{
					bool flag5 = true;
					bool flag6 = true;
					if (intVector2.y == 0 || Mathf.Sign((float)intVector2.y) != Mathf.Sign((float)intVector2FromDirection2.y))
					{
						flag6 = false;
					}
					else if (this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection2) || this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection2 + IntVector2.Right))
					{
						flag6 = false;
					}
					if (flag6)
					{
						intVector += intVector2FromDirection2;
						hashSet.Add(intVector);
						hashSet.Add(intVector + IntVector2.Right);
						hashSet.Add(intVector + IntVector2.Up);
						hashSet.Add(intVector + IntVector2.Right + IntVector2.Up);
						direction3 = direction2;
						continue;
					}
					if (intVector2.x == 0 || Mathf.Sign((float)intVector2.x) != Mathf.Sign((float)intVector2FromDirection.x))
					{
						flag5 = false;
					}
					else if (this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection) || this.CheckCellAndNeighborsOccupied(intVector + intVector2FromDirection + IntVector2.Up))
					{
						flag5 = false;
					}
					if (flag5)
					{
						intVector += intVector2FromDirection;
						hashSet.Add(intVector);
						hashSet.Add(intVector + IntVector2.Right);
						hashSet.Add(intVector + IntVector2.Up);
						hashSet.Add(intVector + IntVector2.Right + IntVector2.Up);
						direction3 = direction;
						continue;
					}
				}
				flag = false;
				break;
			}
			if (num > 10000)
			{
				Debug.LogError("FUCK FUCK FUCK");
			}
			if (flag)
			{
				return hashSet.ToList<IntVector2>();
			}
			return null;
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x001E9590 File Offset: 0x001E7790
		public Dictionary<int, int> DetermineViableExpanse(IntVector2 connectionCenter, DungeonData.Direction direction)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int num = int.MaxValue;
			for (int i = 1; i <= this.MAXIMUM_ROOM_DIMENSION; i++)
			{
				int num2 = 0;
				for (int j = 1; j <= this.MAXIMUM_ROOM_DIMENSION / 2; j++)
				{
					IntVector2 cellToCheck = this.GetCellToCheck(connectionCenter, i, j, false, direction);
					IntVector2 cellToCheck2 = this.GetCellToCheck(connectionCenter, i, j, true, direction);
					if (this.m_occupiedCells.Contains(cellToCheck) || this.m_occupiedCells.Contains(cellToCheck2))
					{
						if (dictionary.ContainsKey(i - 1))
						{
							dictionary.Remove(i - 1);
						}
						break;
					}
					num2 = j - 1;
				}
				num = Math.Min(num, num2 * 2);
				if (num2 == 0)
				{
					break;
				}
				dictionary.Add(i, num);
			}
			return dictionary;
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x001E9660 File Offset: 0x001E7860
		private IntVector2 GetCellToCheck(IntVector2 start, int extendMagnitude, int halfWidth, bool invert, DungeonData.Direction dir)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			switch (dir)
			{
			case DungeonData.Direction.NORTH:
				flag = true;
				num2 = start.y + extendMagnitude;
				goto IL_7D;
			case DungeonData.Direction.EAST:
				num = start.x + extendMagnitude;
				goto IL_7D;
			case DungeonData.Direction.SOUTH:
				flag = true;
				num2 = start.y - extendMagnitude;
				goto IL_7D;
			case DungeonData.Direction.WEST:
				num = start.x - extendMagnitude;
				goto IL_7D;
			}
			Debug.LogError("Switching on invalid direction in SemioticLayoutManager!");
			IL_7D:
			if (flag)
			{
				num = ((!invert) ? (start.x - halfWidth) : (start.x + halfWidth));
			}
			else
			{
				num2 = ((!invert) ? (start.y - halfWidth) : (start.y + halfWidth));
			}
			return new IntVector2(num, num2);
		}

		// Token: 0x04004BEE RID: 19438
		private int MAXIMUM_ROOM_DIMENSION = 50;

		// Token: 0x04004BEF RID: 19439
		private List<RoomHandler> m_allRooms;

		// Token: 0x04004BF0 RID: 19440
		private HashSet<IntVector2> m_exitTestPoints = new HashSet<IntVector2>();

		// Token: 0x04004BF1 RID: 19441
		private IntVector2 m_currentOffset = IntVector2.Zero;

		// Token: 0x04004BF2 RID: 19442
		private HashSet<IntVector2> m_occupiedCells;

		// Token: 0x04004BF3 RID: 19443
		private HashSet<IntVector2> m_temporaryPathfindingWalls = new HashSet<IntVector2>();

		// Token: 0x04004BF4 RID: 19444
		private List<Tuple<IntVector2, IntVector2>> m_rectangleDecomposition;

		// Token: 0x04004BF5 RID: 19445
		private static List<HashSet<IntVector2>> PooledResizedHashsets = new List<HashSet<IntVector2>>();

		// Token: 0x04004BF6 RID: 19446
		private static IntVector2[] SimpleCardinals = new IntVector2[]
		{
			IntVector2.Up,
			2 * IntVector2.Up,
			IntVector2.Right,
			IntVector2.Down,
			IntVector2.Left
		};

		// Token: 0x04004BF7 RID: 19447
		private static IntVector2[] LayoutCardinals = new IntVector2[]
		{
			IntVector2.Up,
			IntVector2.Right,
			IntVector2.Down,
			IntVector2.Left,
			2 * IntVector2.Up,
			3 * IntVector2.Up,
			new IntVector2(1, 1),
			new IntVector2(1, 2),
			new IntVector2(-1, 1),
			new IntVector2(-1, 2),
			new IntVector2(1, -1),
			new IntVector2(-1, -1)
		};

		// Token: 0x04004BF8 RID: 19448
		private static IntVector2[] LayoutPathCardinals = new IntVector2[]
		{
			IntVector2.Up,
			IntVector2.Right,
			IntVector2.Down,
			IntVector2.Left,
			2 * IntVector2.Up,
			3 * IntVector2.Up,
			2 * IntVector2.Right,
			new IntVector2(2, 1),
			new IntVector2(2, 2),
			new IntVector2(1, 3),
			new IntVector2(2, 3),
			new IntVector2(1, 1),
			new IntVector2(1, 2),
			new IntVector2(-1, 1),
			new IntVector2(-1, 2),
			new IntVector2(1, -1),
			new IntVector2(-1, -1)
		};

		// Token: 0x04004BF9 RID: 19449
		private const int SEARCH_DISTANCE_LAYOUT = 3;

		// Token: 0x04004BFA RID: 19450
		public bool FindNearestValidLocataionForLayout2Success;

		// Token: 0x04004BFB RID: 19451
		public IntVector2 FindNearestValidLocationForLayout2Result;

		// Token: 0x04004BFC RID: 19452
		private const int PER_ROOM_HALLWAY_EXTENSION_MAX = 4;

		// Token: 0x04004BFD RID: 19453
		private const int PER_LAYOUT_HALLWAY_EXTENSION_MAX = 12;

		// Token: 0x04004BFE RID: 19454
		private int m_FIRST_FAILS;

		// Token: 0x04004BFF RID: 19455
		private int m_SECOND_FAILS;

		// Token: 0x04004C00 RID: 19456
		private int m_THIRD_FAILS;

		// Token: 0x04004C01 RID: 19457
		private int m_FOURTH_FAILS;

		// Token: 0x04004C02 RID: 19458
		public bool CanPlaceLayoutAtPointSuccess;

		// Token: 0x02000F38 RID: 3896
		public struct BBoxPrepassResults
		{
			// Token: 0x04004C04 RID: 19460
			public bool overlapping;

			// Token: 0x04004C05 RID: 19461
			public int numPairs;

			// Token: 0x04004C06 RID: 19462
			public int numPairsOverlapping;

			// Token: 0x04004C07 RID: 19463
			public int totalOverlapArea;
		}
	}
}
