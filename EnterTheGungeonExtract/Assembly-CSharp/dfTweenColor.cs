using System;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
[AddComponentMenu("Daikon Forge/Tweens/Color")]
public class dfTweenColor : dfTweenComponent<Color>
{
	// Token: 0x06001C89 RID: 7305 RVA: 0x00086508 File Offset: 0x00084708
	public override Color offset(Color lhs, Color rhs)
	{
		return lhs + rhs;
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x00086514 File Offset: 0x00084714
	public override Color evaluate(Color startValue, Color endValue, float time)
	{
		Vector4 vector = startValue;
		Vector4 vector2 = endValue;
		Vector4 vector3 = new Vector4(dfTweenComponent<Color>.Lerp(vector.x, vector2.x, time), dfTweenComponent<Color>.Lerp(vector.y, vector2.y, time), dfTweenComponent<Color>.Lerp(vector.z, vector2.z, time), dfTweenComponent<Color>.Lerp(vector.w, vector2.w, time));
		return vector3;
	}
}
