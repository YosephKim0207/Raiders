using System;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
[AddComponentMenu("Daikon Forge/Tweens/Vector4")]
public class dfTweenVector4 : dfTweenComponent<Vector4>
{
	// Token: 0x06001D2A RID: 7466 RVA: 0x000880B8 File Offset: 0x000862B8
	public override Vector4 offset(Vector4 lhs, Vector4 rhs)
	{
		return lhs + rhs;
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x000880C4 File Offset: 0x000862C4
	public override Vector4 evaluate(Vector4 startValue, Vector4 endValue, float time)
	{
		return new Vector4(dfTweenComponent<Vector4>.Lerp(startValue.x, endValue.x, time), dfTweenComponent<Vector4>.Lerp(startValue.y, endValue.y, time), dfTweenComponent<Vector4>.Lerp(startValue.z, endValue.z, time), dfTweenComponent<Vector4>.Lerp(startValue.w, endValue.w, time));
	}
}
