using System;
using UnityEngine;

// Token: 0x02000CF0 RID: 3312
public class CurveRangeAttribute : PropertyAttribute
{
	// Token: 0x0600463D RID: 17981 RVA: 0x0016D2F8 File Offset: 0x0016B4F8
	public CurveRangeAttribute(float xMin, float yMin, float xMax, float yMax)
	{
		this.Range = new Rect(xMin, yMin, xMax, yMax);
		this.Color = Color.green;
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x0016D31C File Offset: 0x0016B51C
	public CurveRangeAttribute(float xMin, float yMin, float xMax, float yMax, Color color)
	{
		this.Range = new Rect(xMin, yMin, xMax, yMax);
		this.Color = color;
	}

	// Token: 0x040038C1 RID: 14529
	public Color Color;

	// Token: 0x040038C2 RID: 14530
	public Rect Range;
}
