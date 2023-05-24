using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EE0 RID: 3808
	public class FlowActionLine
	{
		// Token: 0x0600511C RID: 20764 RVA: 0x001CB1FC File Offset: 0x001C93FC
		public FlowActionLine(Vector2 p1, Vector2 p2)
		{
			this.point1 = p1;
			this.point2 = p2;
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x001CB214 File Offset: 0x001C9414
		protected bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
		{
			return q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) && q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y);
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x001CB2A0 File Offset: 0x001C94A0
		protected int GetOrientation(Vector2 p, Vector2 q, Vector2 r)
		{
			float num = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
			if (num == 0f)
			{
				return 0;
			}
			return (num <= 0f) ? 2 : 1;
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x001CB30C File Offset: 0x001C950C
		public bool Crosses(FlowActionLine other)
		{
			int orientation = this.GetOrientation(this.point1, this.point2, other.point1);
			int orientation2 = this.GetOrientation(this.point1, this.point2, other.point2);
			int orientation3 = this.GetOrientation(other.point1, other.point2, this.point1);
			int orientation4 = this.GetOrientation(other.point1, other.point2, this.point2);
			return (orientation != orientation2 && orientation3 != orientation4) || (orientation == 0 && this.OnSegment(this.point1, other.point1, this.point2)) || (orientation2 == 0 && this.OnSegment(this.point1, other.point2, this.point2)) || (orientation3 == 0 && this.OnSegment(other.point1, this.point1, other.point2)) || (orientation4 == 0 && this.OnSegment(other.point1, this.point2, other.point2));
		}

		// Token: 0x04004928 RID: 18728
		public Vector2 point1;

		// Token: 0x04004929 RID: 18729
		public Vector2 point2;
	}
}
