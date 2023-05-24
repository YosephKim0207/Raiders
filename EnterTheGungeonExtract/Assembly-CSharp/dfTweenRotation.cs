using System;
using UnityEngine;

// Token: 0x020004D2 RID: 1234
[AddComponentMenu("Daikon Forge/Tweens/Rotation")]
public class dfTweenRotation : dfTweenComponent<Quaternion>
{
	// Token: 0x06001D1F RID: 7455 RVA: 0x00087F4C File Offset: 0x0008614C
	public override Quaternion offset(Quaternion lhs, Quaternion rhs)
	{
		return lhs * rhs;
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x00087F58 File Offset: 0x00086158
	public override Quaternion evaluate(Quaternion startValue, Quaternion endValue, float time)
	{
		Vector3 eulerAngles = startValue.eulerAngles;
		Vector3 eulerAngles2 = endValue.eulerAngles;
		return Quaternion.Euler(dfTweenRotation.LerpEuler(eulerAngles, eulerAngles2, time));
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x00087F84 File Offset: 0x00086184
	private static Vector3 LerpEuler(Vector3 startValue, Vector3 endValue, float time)
	{
		return new Vector3(dfTweenRotation.LerpAngle(startValue.x, endValue.x, time), dfTweenRotation.LerpAngle(startValue.y, endValue.y, time), dfTweenRotation.LerpAngle(startValue.z, endValue.z, time));
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x00087FD4 File Offset: 0x000861D4
	private static float LerpAngle(float startValue, float endValue, float time)
	{
		float num = Mathf.Repeat(endValue - startValue, 360f);
		if (num > 180f)
		{
			num -= 360f;
		}
		return startValue + num * time;
	}
}
