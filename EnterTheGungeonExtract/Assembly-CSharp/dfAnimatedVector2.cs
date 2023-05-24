using System;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class dfAnimatedVector2 : dfAnimatedValue<Vector2>
{
	// Token: 0x06001C4B RID: 7243 RVA: 0x00085828 File Offset: 0x00083A28
	public dfAnimatedVector2(Vector2 StartValue, Vector2 EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x00085834 File Offset: 0x00083A34
	protected override Vector2 Lerp(Vector2 startValue, Vector2 endValue, float time)
	{
		return Vector2.Lerp(startValue, endValue, time);
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x00085840 File Offset: 0x00083A40
	public static implicit operator dfAnimatedVector2(Vector2 value)
	{
		return new dfAnimatedVector2(value, value, 0f);
	}
}
