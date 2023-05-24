using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class dfAnimatedInt : dfAnimatedValue<int>
{
	// Token: 0x06001C42 RID: 7234 RVA: 0x000857A8 File Offset: 0x000839A8
	public dfAnimatedInt(int StartValue, int EndValue, float Time)
		: base(StartValue, EndValue, Time)
	{
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x000857B4 File Offset: 0x000839B4
	protected override int Lerp(int startValue, int endValue, float time)
	{
		return Mathf.RoundToInt(Mathf.Lerp((float)startValue, (float)endValue, time));
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x000857C8 File Offset: 0x000839C8
	public static implicit operator dfAnimatedInt(int value)
	{
		return new dfAnimatedInt(value, value, 0f);
	}
}
