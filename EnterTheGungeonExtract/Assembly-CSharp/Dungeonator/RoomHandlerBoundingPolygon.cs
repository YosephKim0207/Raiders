using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F2E RID: 3886
	public class RoomHandlerBoundingPolygon
	{
		// Token: 0x06005358 RID: 21336 RVA: 0x001E5B9C File Offset: 0x001E3D9C
		public RoomHandlerBoundingPolygon(List<Vector2> points)
		{
			this.m_points = points;
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x001E5BAC File Offset: 0x001E3DAC
		public RoomHandlerBoundingPolygon(List<Vector2> points, float inset)
		{
			this.m_points = new List<Vector2>();
			for (int i = 0; i < points.Count; i++)
			{
				Vector2 vector = points[(i + points.Count - 1) % points.Count];
				Vector2 vector2 = points[i];
				Vector2 vector3 = points[(i + 1) % points.Count];
				Vector2 normalized = (vector2 - vector).normalized;
				normalized = new Vector2(normalized.y, -normalized.x);
				Vector2 normalized2 = (vector3 - vector2).normalized;
				normalized2 = new Vector2(normalized2.y, -normalized2.x);
				Vector2 normalized3 = ((normalized + normalized2) / 2f).normalized;
				this.m_points.Add(vector2 + normalized3 * inset);
			}
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x001E5C9C File Offset: 0x001E3E9C
		public bool ContainsPoint(Vector2 point)
		{
			int i = this.m_points.Count - 1;
			int num = this.m_points.Count - 1;
			bool flag = false;
			for (i = 0; i < this.m_points.Count; i++)
			{
				if (((this.m_points[i].y < point.y && this.m_points[num].y >= point.y) || (this.m_points[num].y < point.y && this.m_points[i].y >= point.y)) && (this.m_points[i].x <= point.x || this.m_points[num].x <= point.x))
				{
					flag ^= this.m_points[i].x + (point.y - this.m_points[i].y) / (this.m_points[num].y - this.m_points[i].y) * (this.m_points[num].x - this.m_points[i].x) < point.x;
				}
				num = i;
			}
			return flag;
		}

		// Token: 0x04004BBD RID: 19389
		private List<Vector2> m_points;
	}
}
