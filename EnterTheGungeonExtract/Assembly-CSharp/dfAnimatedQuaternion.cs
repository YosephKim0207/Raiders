using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class dfAnimatedQuaternion : dfAnimatedValue<Quaternion>
{
	// Token: 0x06001C4E RID: 7246 RVA: 0x00085850 File Offset: 0x00083A50
	public dfAnimatedQuaternion(Quaternion StartValue, Quaternion EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x0008585C File Offset: 0x00083A5C
	protected override Quaternion Lerp(Quaternion startValue, Quaternion endValue, float time)
	{
		return Quaternion.Lerp(startValue, endValue, time);
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x00085868 File Offset: 0x00083A68
	public static implicit operator dfAnimatedQuaternion(Quaternion value)
	{
		return new dfAnimatedQuaternion(value, value, 0f);
	}
}
