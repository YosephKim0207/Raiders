using System;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class dfAnimatedVector3 : dfAnimatedValue<Vector3>
{
	// Token: 0x06001C45 RID: 7237 RVA: 0x000857D8 File Offset: 0x000839D8
	public dfAnimatedVector3(Vector3 StartValue, Vector3 EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000857E4 File Offset: 0x000839E4
	protected override Vector3 Lerp(Vector3 startValue, Vector3 endValue, float time)
	{
		return Vector3.Lerp(startValue, endValue, time);
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x000857F0 File Offset: 0x000839F0
	public static implicit operator dfAnimatedVector3(Vector3 value)
	{
		return new dfAnimatedVector3(value, value, 0f);
	}
}
