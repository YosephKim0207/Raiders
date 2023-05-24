using System;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class dfAnimatedColor : dfAnimatedValue<Color>
{
	// Token: 0x06001C51 RID: 7249 RVA: 0x00085878 File Offset: 0x00083A78
	public dfAnimatedColor(Color StartValue, Color EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x00085884 File Offset: 0x00083A84
	protected override Color Lerp(Color startValue, Color endValue, float time)
	{
		return Color.Lerp(startValue, endValue, time);
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x00085890 File Offset: 0x00083A90
	public static implicit operator dfAnimatedColor(Color value)
	{
		return new dfAnimatedColor(value, value, 0f);
	}
}
