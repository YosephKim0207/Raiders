using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
[AddComponentMenu("Daikon Forge/Tweens/Color32")]
public class dfTweenColor32 : dfTweenComponent<Color32>
{
	// Token: 0x06001C8C RID: 7308 RVA: 0x00086594 File Offset: 0x00084794
	public override Color32 offset(Color32 lhs, Color32 rhs)
	{
		return lhs + rhs;
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000865AC File Offset: 0x000847AC
	public override Color32 evaluate(Color32 startValue, Color32 endValue, float time)
	{
		Vector4 vector = startValue;
		Vector4 vector2 = endValue;
		Vector4 vector3 = new Vector4(dfTweenComponent<Color32>.Lerp(vector.x, vector2.x, time), dfTweenComponent<Color32>.Lerp(vector.y, vector2.y, time), dfTweenComponent<Color32>.Lerp(vector.z, vector2.z, time), dfTweenComponent<Color32>.Lerp(vector.w, vector2.w, time));
		return vector3;
	}
}
