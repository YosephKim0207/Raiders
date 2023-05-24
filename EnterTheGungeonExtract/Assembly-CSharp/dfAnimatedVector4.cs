using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class dfAnimatedVector4 : dfAnimatedValue<Vector4>
{
	// Token: 0x06001C48 RID: 7240 RVA: 0x00085800 File Offset: 0x00083A00
	public dfAnimatedVector4(Vector4 StartValue, Vector4 EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x0008580C File Offset: 0x00083A0C
	protected override Vector4 Lerp(Vector4 startValue, Vector4 endValue, float time)
	{
		return Vector4.Lerp(startValue, endValue, time);
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x00085818 File Offset: 0x00083A18
	public static implicit operator dfAnimatedVector4(Vector4 value)
	{
		return new dfAnimatedVector4(value, value, 0f);
	}
}
