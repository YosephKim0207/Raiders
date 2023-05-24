using System;
using UnityEngine;

// Token: 0x020004CB RID: 1227
[AddComponentMenu("Daikon Forge/Tweens/Float")]
public class dfTweenFloat : dfTweenComponent<float>
{
	// Token: 0x06001CE9 RID: 7401 RVA: 0x000875F0 File Offset: 0x000857F0
	public override float offset(float lhs, float rhs)
	{
		return lhs + rhs;
	}

	// Token: 0x06001CEA RID: 7402 RVA: 0x000875F8 File Offset: 0x000857F8
	public override float evaluate(float startValue, float endValue, float time)
	{
		return startValue + (endValue - startValue) * time;
	}
}
