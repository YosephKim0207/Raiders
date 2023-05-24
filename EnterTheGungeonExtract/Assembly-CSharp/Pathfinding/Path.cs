using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020015B5 RID: 5557
	public class Path
	{
		// Token: 0x06007F78 RID: 32632 RVA: 0x003371C8 File Offset: 0x003353C8
		public Path()
		{
			this.Positions = new LinkedList<IntVector2>();
			this.WillReachFinalGoal = true;
		}

		// Token: 0x06007F79 RID: 32633 RVA: 0x003371F8 File Offset: 0x003353F8
		public Path(LinkedList<IntVector2> positions, IntVector2 clearance)
		{
			this.Positions = positions;
			this.Clearance = clearance;
			this.WillReachFinalGoal = true;
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06007F7A RID: 32634 RVA: 0x0033722C File Offset: 0x0033542C
		public int Count
		{
			get
			{
				return (this.Positions == null) ? 0 : this.Positions.Count;
			}
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06007F7B RID: 32635 RVA: 0x0033724C File Offset: 0x0033544C
		public IntVector2 First
		{
			get
			{
				return this.Positions.First.Value;
			}
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06007F7C RID: 32636 RVA: 0x00337260 File Offset: 0x00335460
		// (set) Token: 0x06007F7D RID: 32637 RVA: 0x00337268 File Offset: 0x00335468
		public bool WillReachFinalGoal { get; set; }

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06007F7E RID: 32638 RVA: 0x00337274 File Offset: 0x00335474
		public float InaccurateLength
		{
			get
			{
				if (this.Positions.Count == 0)
				{
					return 0f;
				}
				float num = 0f;
				LinkedListNode<IntVector2> linkedListNode = this.Positions.First;
				LinkedListNode<IntVector2> linkedListNode2 = linkedListNode.Next;
				while (linkedListNode != null && linkedListNode2 != null)
				{
					num += (float)IntVector2.ManhattanDistance(linkedListNode.Value, linkedListNode2.Value);
					linkedListNode = linkedListNode2;
					linkedListNode2 = linkedListNode2.Next;
				}
				return num;
			}
		}

		// Token: 0x06007F7F RID: 32639 RVA: 0x003372E0 File Offset: 0x003354E0
		public Vector2 GetFirstCenterVector2()
		{
			return Pathfinder.GetClearanceOffset(this.Positions.First.Value, this.Clearance);
		}

		// Token: 0x06007F80 RID: 32640 RVA: 0x00337300 File Offset: 0x00335500
		public Vector2 GetSecondCenterVector2()
		{
			return Pathfinder.GetClearanceOffset(this.Positions.First.Next.Value, this.Clearance);
		}

		// Token: 0x06007F81 RID: 32641 RVA: 0x00337324 File Offset: 0x00335524
		public void RemoveFirst()
		{
			this.Positions.RemoveFirst();
		}

		// Token: 0x06007F82 RID: 32642 RVA: 0x00337334 File Offset: 0x00335534
		public void Smooth(Vector2 startPos, Vector2 extents, CellTypes passableCellTypes, bool canPassOccupied, IntVector2 clearance)
		{
			Pathfinder.Instance.Smooth(this, startPos, extents, passableCellTypes, canPassOccupied, clearance);
		}

		// Token: 0x0400821D RID: 33309
		public LinkedList<IntVector2> Positions;

		// Token: 0x0400821E RID: 33310
		public LinkedList<IntVector2> PreSmoothedPositions = new LinkedList<IntVector2>();

		// Token: 0x0400821F RID: 33311
		public IntVector2 Clearance = IntVector2.One;
	}
}
