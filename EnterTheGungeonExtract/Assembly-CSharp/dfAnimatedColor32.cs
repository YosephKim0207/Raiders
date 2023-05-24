using System;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class dfAnimatedColor32 : dfAnimatedValue<Color32>
{
	// Token: 0x06001C54 RID: 7252 RVA: 0x000858A0 File Offset: 0x00083AA0
	public dfAnimatedColor32(Color32 StartValue, Color32 EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x000858AC File Offset: 0x00083AAC
	protected override Color32 Lerp(Color32 startValue, Color32 endValue, float time)
	{
		return Color.Lerp(startValue, endValue, time);
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x000858C8 File Offset: 0x00083AC8
	public static implicit operator dfAnimatedColor32(Color32 value)
	{
		return new dfAnimatedColor32(value, value, 0f);
	}
}
