using System;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
[AddComponentMenu("Daikon Forge/Tweens/Vector2")]
public class dfTweenVector2 : dfTweenComponent<Vector2>
{
	// Token: 0x06001D24 RID: 7460 RVA: 0x00088010 File Offset: 0x00086210
	public override Vector2 offset(Vector2 lhs, Vector2 rhs)
	{
		return lhs + rhs;
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x0008801C File Offset: 0x0008621C
	public override Vector2 evaluate(Vector2 startValue, Vector2 endValue, float time)
	{
		return new Vector2(dfTweenComponent<Vector2>.Lerp(startValue.x, endValue.x, time), dfTweenComponent<Vector2>.Lerp(startValue.y, endValue.y, time));
	}
}
