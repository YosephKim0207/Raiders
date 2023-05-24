using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EAA RID: 3754
	public class CellArea
	{
		// Token: 0x06004F72 RID: 20338 RVA: 0x001B91E0 File Offset: 0x001B73E0
		public CellArea(IntVector2 p, IntVector2 d, int borderOffset = 0)
		{
			this.basePosition = p;
			this.dimensions = d;
			this.borderDistance = borderOffset;
			this.instanceUsedExits = new List<PrototypeRoomExit>();
			this.exitToLocalDataMap = new Dictionary<PrototypeRoomExit, RuntimeRoomExitData>();
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06004F73 RID: 20339 RVA: 0x001B922C File Offset: 0x001B742C
		// (set) Token: 0x06004F74 RID: 20340 RVA: 0x001B9234 File Offset: 0x001B7434
		public PrototypeDungeonRoom prototypeRoom
		{
			get
			{
				return this.m_prototypeRoom;
			}
			set
			{
				this.IsProceduralRoom = value == null && this.IsProceduralRoom;
				this.PrototypeRoomName = ((!(value == null)) ? value.name : this.PrototypeRoomName);
				this.PrototypeRoomCategory = ((!(value == null)) ? value.category : this.PrototypeRoomCategory);
				this.PrototypeRoomNormalSubcategory = ((!(value == null)) ? value.subCategoryNormal : this.PrototypeRoomNormalSubcategory);
				this.PrototypeRoomSpecialSubcategory = ((!(value == null)) ? value.subCategorySpecial : this.PrototypeRoomSpecialSubcategory);
				this.PrototypeRoomBossSubcategory = ((!(value == null)) ? value.subCategoryBoss : this.PrototypeRoomBossSubcategory);
				this.PrototypeLostWoodsRoom = ((!(value == null)) ? value.IsLostWoodsRoom : this.PrototypeLostWoodsRoom);
				this.m_prototypeRoom = value;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06004F75 RID: 20341 RVA: 0x001B9338 File Offset: 0x001B7538
		public Vector2 UnitBottomLeft
		{
			get
			{
				return this.basePosition.ToVector2();
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06004F76 RID: 20342 RVA: 0x001B9348 File Offset: 0x001B7548
		public Vector2 UnitCenter
		{
			get
			{
				return this.basePosition.ToVector2() + this.dimensions.ToVector2() * 0.5f;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06004F77 RID: 20343 RVA: 0x001B9370 File Offset: 0x001B7570
		public Vector2 UnitTopRight
		{
			get
			{
				return (this.basePosition + this.dimensions).ToVector2();
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06004F78 RID: 20344 RVA: 0x001B9398 File Offset: 0x001B7598
		public float UnitLeft
		{
			get
			{
				return (float)this.basePosition.x;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06004F79 RID: 20345 RVA: 0x001B93A8 File Offset: 0x001B75A8
		public float UnitRight
		{
			get
			{
				return (float)(this.basePosition.x + this.dimensions.x);
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06004F7A RID: 20346 RVA: 0x001B93C4 File Offset: 0x001B75C4
		public float UnitBottom
		{
			get
			{
				return (float)this.basePosition.y;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06004F7B RID: 20347 RVA: 0x001B93D4 File Offset: 0x001B75D4
		public float UnitTop
		{
			get
			{
				return (float)(this.basePosition.y + this.dimensions.y);
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06004F7C RID: 20348 RVA: 0x001B93F0 File Offset: 0x001B75F0
		public Vector2 Center
		{
			get
			{
				return new Vector2((float)this.basePosition.x + (float)this.dimensions.x / 2f, (float)this.basePosition.y + (float)this.dimensions.y / 2f);
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06004F7D RID: 20349 RVA: 0x001B9440 File Offset: 0x001B7640
		public IntVector2 IntCenter
		{
			get
			{
				return new IntVector2((int)this.Center.x, (int)this.Center.y);
			}
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x001B9470 File Offset: 0x001B7670
		public bool Overlaps(CellArea other)
		{
			IntVector2 intVector = this.basePosition + this.dimensions;
			IntVector2 intVector2 = other.basePosition + other.dimensions;
			return this.basePosition.x < intVector2.x && intVector.x > other.basePosition.x && this.basePosition.y < intVector2.y && intVector.y > other.basePosition.y;
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x001B9500 File Offset: 0x001B7700
		public bool OverlapsWithUnitBorder(CellArea other)
		{
			IntVector2 intVector = this.basePosition + IntVector2.NegOne;
			IntVector2 intVector2 = this.basePosition + this.dimensions + IntVector2.One * 2;
			IntVector2 intVector3 = other.basePosition + IntVector2.NegOne;
			IntVector2 intVector4 = other.basePosition + other.dimensions + IntVector2.One * 2;
			return intVector.x < intVector4.x && intVector2.x > intVector3.x && intVector.y < intVector4.y && intVector2.y > intVector3.y;
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x001B95C4 File Offset: 0x001B77C4
		public bool ContainsWithUnitBorder(IntVector2 point)
		{
			return point.x >= this.basePosition.x - 1 && point.x <= this.basePosition.x + 1 && point.y >= this.basePosition.y - 1 && point.y <= this.basePosition.y + 1;
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x001B9638 File Offset: 0x001B7838
		public bool Contains(IntVector2 point)
		{
			return point.x >= this.basePosition.x + this.borderDistance && point.x <= this.basePosition.x + this.dimensions.x - this.borderDistance && point.y >= this.basePosition.y + this.borderDistance && point.y <= this.basePosition.y + this.dimensions.y - this.borderDistance;
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x001B96D8 File Offset: 0x001B78D8
		public bool CellOnBorder(IntVector2 pos)
		{
			bool flag = false;
			if ((pos.x < this.basePosition.x + 1 && this.Contains(pos)) || (pos.x >= this.basePosition.x + this.dimensions.x - 1 && this.Contains(pos)) || (pos.y < this.basePosition.y + 1 && this.Contains(pos)) || (pos.y >= this.basePosition.y + this.dimensions.y - 1 && this.Contains(pos)))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x001B9798 File Offset: 0x001B7998
		public int CheckSharedEdge(CellArea other, int lengthOfSharedEdge, out IntVector2 position, out DungeonData.Direction dir)
		{
			int num = Math.Max(this.basePosition.x, other.basePosition.x);
			int num2 = Math.Min(this.basePosition.x + this.dimensions.x, other.basePosition.x + other.dimensions.x);
			int num3 = num2 - num;
			if (num3 >= lengthOfSharedEdge)
			{
				if (other.basePosition.y > this.basePosition.y)
				{
					dir = DungeonData.Direction.NORTH;
					position = new IntVector2(num, this.basePosition.y + this.dimensions.y);
				}
				else
				{
					dir = DungeonData.Direction.SOUTH;
					position = new IntVector2(num, this.basePosition.y);
				}
				return num3;
			}
			num = Math.Max(this.basePosition.y, other.basePosition.y);
			num2 = Math.Min(this.basePosition.y + this.dimensions.y, other.basePosition.y + other.dimensions.y);
			int num4 = num2 - num;
			if (num4 >= lengthOfSharedEdge)
			{
				if (other.basePosition.x > this.basePosition.x)
				{
					dir = DungeonData.Direction.EAST;
					position = new IntVector2(this.basePosition.x + this.dimensions.x, num);
				}
				else
				{
					dir = DungeonData.Direction.WEST;
					position = new IntVector2(this.basePosition.x, num);
				}
				return num4;
			}
			dir = DungeonData.Direction.NORTHWEST;
			position = IntVector2.Zero;
			return -1;
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x001B9920 File Offset: 0x001B7B20
		public bool LineIntersect(IntVector2 p1, IntVector2 p2)
		{
			return this.LineIntersectsLine(p1, p2, this.basePosition, this.basePosition + new IntVector2(0, this.dimensions.y)) || this.LineIntersectsLine(p1, p2, this.basePosition, this.basePosition + new IntVector2(this.dimensions.x, 0)) || this.LineIntersectsLine(p1, p2, this.basePosition + new IntVector2(0, this.dimensions.y), this.basePosition + this.dimensions) || this.LineIntersectsLine(p1, p2, this.basePosition + new IntVector2(this.dimensions.x, 0), this.basePosition + this.dimensions) || (this.Contains(p1) && this.Contains(p2));
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x001B9A18 File Offset: 0x001B7C18
		private bool LineIntersectsLine(IntVector2 l1p1, IntVector2 l1p2, IntVector2 l2p1, IntVector2 l2p2)
		{
			float num = (float)((l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y));
			float num2 = (float)((l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X));
			if (num2 == 0f)
			{
				return false;
			}
			float num3 = num / num2;
			num = (float)((l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y));
			float num4 = num / num2;
			return num3 >= 0f && num3 <= 1f && num4 >= 0f && num4 <= 1f;
		}

		// Token: 0x040046DB RID: 18139
		public IntVector2 basePosition;

		// Token: 0x040046DC RID: 18140
		public IntVector2 dimensions;

		// Token: 0x040046DD RID: 18141
		public Vector2 weightedOverlapMovementVector;

		// Token: 0x040046DE RID: 18142
		public int variableBorderSizeX;

		// Token: 0x040046DF RID: 18143
		public int variableBorderSizeY;

		// Token: 0x040046E0 RID: 18144
		public bool IsProceduralRoom = true;

		// Token: 0x040046E1 RID: 18145
		public string PrototypeRoomName;

		// Token: 0x040046E2 RID: 18146
		public PrototypeDungeonRoom.RoomCategory PrototypeRoomCategory = PrototypeDungeonRoom.RoomCategory.NORMAL;

		// Token: 0x040046E3 RID: 18147
		public PrototypeDungeonRoom.RoomNormalSubCategory PrototypeRoomNormalSubcategory;

		// Token: 0x040046E4 RID: 18148
		public PrototypeDungeonRoom.RoomSpecialSubCategory PrototypeRoomSpecialSubcategory;

		// Token: 0x040046E5 RID: 18149
		public PrototypeDungeonRoom.RoomBossSubCategory PrototypeRoomBossSubcategory;

		// Token: 0x040046E6 RID: 18150
		public bool PrototypeLostWoodsRoom;

		// Token: 0x040046E7 RID: 18151
		public RuntimePrototypeRoomData runtimePrototypeData;

		// Token: 0x040046E8 RID: 18152
		private PrototypeDungeonRoom m_prototypeRoom;

		// Token: 0x040046E9 RID: 18153
		public List<PrototypeRoomExit> instanceUsedExits;

		// Token: 0x040046EA RID: 18154
		public Dictionary<PrototypeRoomExit, RuntimeRoomExitData> exitToLocalDataMap;

		// Token: 0x040046EB RID: 18155
		public List<IntVector2> proceduralCells;

		// Token: 0x040046EC RID: 18156
		private int borderDistance;
	}
}
