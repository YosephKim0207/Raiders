using System;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
public class dfAnimatedFloat : dfAnimatedValue<float>
{
	// Token: 0x06001C3F RID: 7231 RVA: 0x00085780 File Offset: 0x00083980
	public dfAnimatedFloat(float StartValue, float EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x0008578C File Offset: 0x0008398C
	protected override float Lerp(float startValue, float endValue, float time)
	{
		return Mathf.Lerp(startValue, endValue, time);
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x00085798 File Offset: 0x00083998
	public static implicit operator dfAnimatedFloat(float value)
	{
		return new dfAnimatedFloat(value, value, 0f);
	}
}
