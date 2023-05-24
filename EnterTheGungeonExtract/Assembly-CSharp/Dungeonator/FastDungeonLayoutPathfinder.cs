using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Dungeonator
{
	// Token: 0x02000EED RID: 3821
	public class FastDungeonLayoutPathfinder
	{
		// Token: 0x0600514E RID: 20814 RVA: 0x001CD4F4 File Offset: 0x001CB6F4
		public FastDungeonLayoutPathfinder(byte[,] grid)
		{
			if (grid == null)
			{
				throw new Exception("Grid cannot be null");
			}
			this.mGrid = grid;
			this.mGridX = (ushort)(this.mGrid.GetUpperBound(0) + 1);
			this.mGridY = (ushort)(this.mGrid.GetUpperBound(1) + 1);
			this.mGridXMinus1 = this.mGridX - 1;
			this.mGridYLog2 = (ushort)Math.Log((double)this.mGridY, 2.0);
			if (Math.Log((double)this.mGridX, 2.0) != (double)((int)Math.Log((double)this.mGridX, 2.0)) || Math.Log((double)this.mGridY, 2.0) != (double)((int)Math.Log((double)this.mGridY, 2.0)))
			{
				throw new Exception("Invalid Grid, size in X and Y must be power of 2");
			}
			if (this.mCalcGrid == null || this.mCalcGrid.Length != (int)(this.mGridX * this.mGridY))
			{
				this.mCalcGrid = new FastDungeonLayoutPathfinder.PathFinderNodeFast[(int)(this.mGridX * this.mGridY)];
			}
			this.mOpen = new PriorityQueueB<int>(new FastDungeonLayoutPathfinder.ComparePFNodeMatrix(this.mCalcGrid));
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x001CD68C File Offset: 0x001CB88C
		public bool Stopped
		{
			get
			{
				return this.mStopped;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06005150 RID: 20816 RVA: 0x001CD694 File Offset: 0x001CB894
		// (set) Token: 0x06005151 RID: 20817 RVA: 0x001CD69C File Offset: 0x001CB89C
		public HeuristicFormula Formula
		{
			get
			{
				return this.mFormula;
			}
			set
			{
				this.mFormula = value;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x001CD6A8 File Offset: 0x001CB8A8
		// (set) Token: 0x06005153 RID: 20819 RVA: 0x001CD6B0 File Offset: 0x001CB8B0
		public bool Diagonals
		{
			get
			{
				return this.mDiagonals;
			}
			set
			{
				this.mDiagonals = value;
				if (this.mDiagonals)
				{
					this.mDirection = new sbyte[,]
					{
						{ 0, -1 },
						{ 1, 0 },
						{ 0, 1 },
						{ -1, 0 },
						{ 1, -1 },
						{ 1, 1 },
						{ -1, 1 },
						{ -1, -1 }
					};
				}
				else
				{
					this.mDirection = new sbyte[,]
					{
						{ 0, -1 },
						{ 1, 0 },
						{ 0, 1 },
						{ -1, 0 }
					};
				}
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06005154 RID: 20820 RVA: 0x001CD704 File Offset: 0x001CB904
		// (set) Token: 0x06005155 RID: 20821 RVA: 0x001CD70C File Offset: 0x001CB90C
		public bool HeavyDiagonals
		{
			get
			{
				return this.mHeavyDiagonals;
			}
			set
			{
				this.mHeavyDiagonals = value;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06005156 RID: 20822 RVA: 0x001CD718 File Offset: 0x001CB918
		// (set) Token: 0x06005157 RID: 20823 RVA: 0x001CD720 File Offset: 0x001CB920
		public int HeuristicEstimate
		{
			get
			{
				return this.mHEstimate;
			}
			set
			{
				this.mHEstimate = value;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06005158 RID: 20824 RVA: 0x001CD72C File Offset: 0x001CB92C
		// (set) Token: 0x06005159 RID: 20825 RVA: 0x001CD734 File Offset: 0x001CB934
		public bool PunishChangeDirection
		{
			get
			{
				return this.mPunishChangeDirection;
			}
			set
			{
				this.mPunishChangeDirection = value;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600515A RID: 20826 RVA: 0x001CD740 File Offset: 0x001CB940
		// (set) Token: 0x0600515B RID: 20827 RVA: 0x001CD748 File Offset: 0x001CB948
		public bool TieBreaker
		{
			get
			{
				return this.mTieBreaker;
			}
			set
			{
				this.mTieBreaker = value;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x0600515C RID: 20828 RVA: 0x001CD754 File Offset: 0x001CB954
		// (set) Token: 0x0600515D RID: 20829 RVA: 0x001CD75C File Offset: 0x001CB95C
		public int SearchLimit
		{
			get
			{
				return this.mSearchLimit;
			}
			set
			{
				this.mSearchLimit = value;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x0600515E RID: 20830 RVA: 0x001CD768 File Offset: 0x001CB968
		// (set) Token: 0x0600515F RID: 20831 RVA: 0x001CD770 File Offset: 0x001CB970
		public double CompletedTime
		{
			get
			{
				return this.mCompletedTime;
			}
			set
			{
				this.mCompletedTime = value;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06005160 RID: 20832 RVA: 0x001CD77C File Offset: 0x001CB97C
		// (set) Token: 0x06005161 RID: 20833 RVA: 0x001CD784 File Offset: 0x001CB984
		public bool DebugProgress
		{
			get
			{
				return this.mDebugProgress;
			}
			set
			{
				this.mDebugProgress = value;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x001CD790 File Offset: 0x001CB990
		// (set) Token: 0x06005163 RID: 20835 RVA: 0x001CD798 File Offset: 0x001CB998
		public bool DebugFoundPath
		{
			get
			{
				return this.mDebugFoundPath;
			}
			set
			{
				this.mDebugFoundPath = value;
			}
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001CD7A4 File Offset: 0x001CB9A4
		public void FindPathStop()
		{
			this.mStop = true;
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001CD7B0 File Offset: 0x001CB9B0
		public List<PathFinderNode> FindPath(IntVector2 start, IntVector2 end)
		{
			return this.FindPath(start, IntVector2.Zero, end);
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x001CD7C0 File Offset: 0x001CB9C0
		public List<PathFinderNode> FindPath(IntVector2 start, IntVector2 startDirection, IntVector2 end)
		{
			List<PathFinderNode> list;
			lock (this)
			{
				this.mFound = false;
				this.mStop = false;
				this.mStopped = false;
				this.mCloseNodeCounter = 0;
				this.mOpenNodeValue += 2;
				this.mCloseNodeValue += 2;
				this.mOpen.Clear();
				this.mClose.Clear();
				this.mLocation = (start.Y << (int)this.mGridYLog2) + start.X;
				this.mEndLocation = (end.Y << (int)this.mGridYLog2) + end.X;
				this.mCalcGrid[this.mLocation].G = 0;
				this.mCalcGrid[this.mLocation].F = this.mHEstimate;
				this.mCalcGrid[this.mLocation].PX = (ushort)start.X;
				this.mCalcGrid[this.mLocation].PY = (ushort)start.Y;
				this.mCalcGrid[this.mLocation].Status = this.mOpenNodeValue;
				this.mOpen.Push(this.mLocation);
				while (this.mOpen.Count > 0 && !this.mStop)
				{
					this.mLocation = this.mOpen.Pop();
					if (this.mCalcGrid[this.mLocation].Status != this.mCloseNodeValue)
					{
						this.mLocationX = (ushort)(this.mLocation & (int)this.mGridXMinus1);
						this.mLocationY = (ushort)(this.mLocation >> (int)this.mGridYLog2);
						if (this.mLocation == this.mEndLocation)
						{
							this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
							this.mFound = true;
							break;
						}
						if (this.mCloseNodeCounter > this.mSearchLimit)
						{
							this.mStopped = true;
							return null;
						}
						if (this.mPunishChangeDirection)
						{
							this.mHoriz = (int)(this.mLocationX - this.mCalcGrid[this.mLocation].PX);
							if ((int)this.mLocationX == start.x && (int)this.mLocationY == start.y)
							{
								this.mHoriz = startDirection.x;
							}
						}
						for (int i = 0; i < ((!this.mDiagonals) ? 4 : 8); i++)
						{
							this.mNewLocationX = (ushort)((int)this.mLocationX + (int)this.mDirection[i, 0]);
							this.mNewLocationY = (ushort)((int)this.mLocationY + (int)this.mDirection[i, 1]);
							this.mNewLocation = ((int)this.mNewLocationY << (int)this.mGridYLog2) + (int)this.mNewLocationX;
							if (this.mNewLocationX < this.mGridX && this.mNewLocationY < this.mGridY)
							{
								if (this.mGrid[(int)this.mNewLocationX, (int)this.mNewLocationY] != 0)
								{
									if (this.mHeavyDiagonals && i > 3)
									{
										this.mNewG = this.mCalcGrid[this.mLocation].G + (int)((double)this.mGrid[(int)this.mNewLocationX, (int)this.mNewLocationY] * 2.41);
									}
									else
									{
										this.mNewG = this.mCalcGrid[this.mLocation].G + (int)this.mGrid[(int)this.mNewLocationX, (int)this.mNewLocationY];
									}
									if (this.mPunishChangeDirection)
									{
										if (this.mNewLocationX - this.mLocationX != 0 && this.mHoriz == 0)
										{
											this.mNewG += Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y);
										}
										if (this.mNewLocationY - this.mLocationY != 0 && this.mHoriz != 0)
										{
											this.mNewG += Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y);
										}
									}
									if ((this.mCalcGrid[this.mNewLocation].Status != this.mOpenNodeValue && this.mCalcGrid[this.mNewLocation].Status != this.mCloseNodeValue) || this.mCalcGrid[this.mNewLocation].G > this.mNewG)
									{
										this.mCalcGrid[this.mNewLocation].PX = this.mLocationX;
										this.mCalcGrid[this.mNewLocation].PY = this.mLocationY;
										this.mCalcGrid[this.mNewLocation].G = this.mNewG;
										switch (this.mFormula)
										{
										default:
											this.mH = this.mHEstimate * (Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y));
											break;
										case HeuristicFormula.MaxDXDY:
											this.mH = this.mHEstimate * Math.Max(Math.Abs((int)this.mNewLocationX - end.X), Math.Abs((int)this.mNewLocationY - end.Y));
											break;
										case HeuristicFormula.DiagonalShortCut:
										{
											int num = Math.Min(Math.Abs((int)this.mNewLocationX - end.X), Math.Abs((int)this.mNewLocationY - end.Y));
											int num2 = Math.Abs((int)this.mNewLocationX - end.X) + Math.Abs((int)this.mNewLocationY - end.Y);
											this.mH = this.mHEstimate * 2 * num + this.mHEstimate * (num2 - 2 * num);
											break;
										}
										case HeuristicFormula.Euclidean:
											this.mH = (int)((double)this.mHEstimate * Math.Sqrt(Math.Pow((double)((int)this.mNewLocationY - end.X), 2.0) + Math.Pow((double)((int)this.mNewLocationY - end.Y), 2.0)));
											break;
										case HeuristicFormula.EuclideanNoSQR:
											this.mH = (int)((double)this.mHEstimate * (Math.Pow((double)((int)this.mNewLocationX - end.X), 2.0) + Math.Pow((double)((int)this.mNewLocationY - end.Y), 2.0)));
											break;
										case HeuristicFormula.Custom1:
										{
											IntVector2 intVector = new IntVector2(Math.Abs(end.X - (int)this.mNewLocationX), Math.Abs(end.Y - (int)this.mNewLocationY));
											int num3 = Math.Abs(intVector.X - intVector.Y);
											int num4 = Math.Abs((intVector.X + intVector.Y - num3) / 2);
											this.mH = this.mHEstimate * (num4 + num3 + intVector.X + intVector.Y);
											break;
										}
										}
										if (this.mTieBreaker)
										{
											int num5 = (int)this.mLocationX - end.X;
											int num6 = (int)this.mLocationY - end.Y;
											int num7 = start.X - end.X;
											int num8 = start.Y - end.Y;
											int num9 = Math.Abs(num5 * num8 - num7 * num6);
											this.mH = (int)((double)this.mH + (double)num9 * 0.001);
										}
										this.mCalcGrid[this.mNewLocation].F = this.mNewG + this.mH;
										this.mOpen.Push(this.mNewLocation);
										this.mCalcGrid[this.mNewLocation].Status = this.mOpenNodeValue;
									}
								}
							}
						}
						this.mCloseNodeCounter++;
						this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
					}
				}
				if (this.mFound)
				{
					this.mClose.Clear();
					int num10 = end.X;
					int num11 = end.Y;
					FastDungeonLayoutPathfinder.PathFinderNodeFast pathFinderNodeFast = this.mCalcGrid[(end.Y << (int)this.mGridYLog2) + end.X];
					PathFinderNode pathFinderNode;
					pathFinderNode.F = pathFinderNodeFast.F;
					pathFinderNode.G = pathFinderNodeFast.G;
					pathFinderNode.H = 0;
					pathFinderNode.PX = (int)pathFinderNodeFast.PX;
					pathFinderNode.PY = (int)pathFinderNodeFast.PY;
					pathFinderNode.X = end.X;
					pathFinderNode.Y = end.Y;
					while (pathFinderNode.X != pathFinderNode.PX || pathFinderNode.Y != pathFinderNode.PY)
					{
						this.mClose.Add(pathFinderNode);
						num10 = pathFinderNode.PX;
						num11 = pathFinderNode.PY;
						pathFinderNodeFast = this.mCalcGrid[(num11 << (int)this.mGridYLog2) + num10];
						pathFinderNode.F = pathFinderNodeFast.F;
						pathFinderNode.G = pathFinderNodeFast.G;
						pathFinderNode.H = 0;
						pathFinderNode.PX = (int)pathFinderNodeFast.PX;
						pathFinderNode.PY = (int)pathFinderNodeFast.PY;
						pathFinderNode.X = num10;
						pathFinderNode.Y = num11;
					}
					this.mClose.Add(pathFinderNode);
					this.mStopped = true;
					list = this.mClose;
				}
				else
				{
					this.mStopped = true;
					list = null;
				}
			}
			return list;
		}

		// Token: 0x04004959 RID: 18777
		private byte[,] mGrid;

		// Token: 0x0400495A RID: 18778
		private PriorityQueueB<int> mOpen;

		// Token: 0x0400495B RID: 18779
		private List<PathFinderNode> mClose = new List<PathFinderNode>();

		// Token: 0x0400495C RID: 18780
		private bool mStop;

		// Token: 0x0400495D RID: 18781
		private bool mStopped = true;

		// Token: 0x0400495E RID: 18782
		private int mHoriz;

		// Token: 0x0400495F RID: 18783
		private HeuristicFormula mFormula = HeuristicFormula.Manhattan;

		// Token: 0x04004960 RID: 18784
		private bool mDiagonals = true;

		// Token: 0x04004961 RID: 18785
		private int mHEstimate = 2;

		// Token: 0x04004962 RID: 18786
		private bool mPunishChangeDirection;

		// Token: 0x04004963 RID: 18787
		private bool mTieBreaker;

		// Token: 0x04004964 RID: 18788
		private bool mHeavyDiagonals;

		// Token: 0x04004965 RID: 18789
		private int mSearchLimit = 2000;

		// Token: 0x04004966 RID: 18790
		private double mCompletedTime;

		// Token: 0x04004967 RID: 18791
		private bool mDebugProgress;

		// Token: 0x04004968 RID: 18792
		private bool mDebugFoundPath;

		// Token: 0x04004969 RID: 18793
		private FastDungeonLayoutPathfinder.PathFinderNodeFast[] mCalcGrid;

		// Token: 0x0400496A RID: 18794
		private byte mOpenNodeValue = 1;

		// Token: 0x0400496B RID: 18795
		private byte mCloseNodeValue = 2;

		// Token: 0x0400496C RID: 18796
		private int mH;

		// Token: 0x0400496D RID: 18797
		private int mLocation;

		// Token: 0x0400496E RID: 18798
		private int mNewLocation;

		// Token: 0x0400496F RID: 18799
		private ushort mLocationX;

		// Token: 0x04004970 RID: 18800
		private ushort mLocationY;

		// Token: 0x04004971 RID: 18801
		private ushort mNewLocationX;

		// Token: 0x04004972 RID: 18802
		private ushort mNewLocationY;

		// Token: 0x04004973 RID: 18803
		private int mCloseNodeCounter;

		// Token: 0x04004974 RID: 18804
		private ushort mGridX;

		// Token: 0x04004975 RID: 18805
		private ushort mGridY;

		// Token: 0x04004976 RID: 18806
		private ushort mGridXMinus1;

		// Token: 0x04004977 RID: 18807
		private ushort mGridYLog2;

		// Token: 0x04004978 RID: 18808
		private bool mFound;

		// Token: 0x04004979 RID: 18809
		private sbyte[,] mDirection = new sbyte[,]
		{
			{ 0, -1 },
			{ 1, 0 },
			{ 0, 1 },
			{ -1, 0 },
			{ 1, -1 },
			{ 1, 1 },
			{ -1, 1 },
			{ -1, -1 }
		};

		// Token: 0x0400497A RID: 18810
		private int mEndLocation;

		// Token: 0x0400497B RID: 18811
		private int mNewG;

		// Token: 0x02000EEE RID: 3822
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct PathFinderNodeFast
		{
			// Token: 0x0400497C RID: 18812
			public int F;

			// Token: 0x0400497D RID: 18813
			public int G;

			// Token: 0x0400497E RID: 18814
			public ushort PX;

			// Token: 0x0400497F RID: 18815
			public ushort PY;

			// Token: 0x04004980 RID: 18816
			public byte Status;
		}

		// Token: 0x02000EEF RID: 3823
		internal class ComparePFNodeMatrix : IComparer<int>
		{
			// Token: 0x06005167 RID: 20839 RVA: 0x001CE1E8 File Offset: 0x001CC3E8
			public ComparePFNodeMatrix(FastDungeonLayoutPathfinder.PathFinderNodeFast[] matrix)
			{
				this.mMatrix = matrix;
			}

			// Token: 0x06005168 RID: 20840 RVA: 0x001CE1F8 File Offset: 0x001CC3F8
			public int Compare(int a, int b)
			{
				if (this.mMatrix[a].F > this.mMatrix[b].F)
				{
					return 1;
				}
				if (this.mMatrix[a].F < this.mMatrix[b].F)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x04004981 RID: 18817
			private FastDungeonLayoutPathfinder.PathFinderNodeFast[] mMatrix;
		}
	}
}
