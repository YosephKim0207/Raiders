using System;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
[AddComponentMenu("Daikon Forge/Tweens/Vector3")]
public class dfTweenVector3 : dfTweenComponent<Vector3>
{
	// Token: 0x06001D27 RID: 7463 RVA: 0x00088054 File Offset: 0x00086254
	public override Vector3 offset(Vector3 lhs, Vector3 rhs)
	{
		return lhs + rhs;
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x00088060 File Offset: 0x00086260
	public override Vector3 evaluate(Vector3 startValue, Vector3 endValue, float time)
	{
		return new Vector3(dfTweenComponent<Vector3>.Lerp(startValue.x, endValue.x, time), dfTweenComponent<Vector3>.Lerp(startValue.y, endValue.y, time), dfTweenComponent<Vector3>.Lerp(startValue.z, endValue.z, time));
	}
}
