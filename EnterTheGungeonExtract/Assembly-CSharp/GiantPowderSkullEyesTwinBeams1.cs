using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001B2 RID: 434
[InspectorDropdownName("Bosses/GiantPowderSkull/EyesTwinBeams1")]
public class GiantPowderSkullEyesTwinBeams1 : Script
{
	// Token: 0x06000676 RID: 1654 RVA: 0x0001EF04 File Offset: 0x0001D104
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float startDirection = base.AimDirection;
		float sign = BraveUtility.RandomSign();
		for (int i = 0; i < 210; i++)
		{
			float offset = 0f;
			if (i < 30)
			{
				offset = Mathf.Lerp(135f, 0f, (float)i / 29f);
			}
			float currentAngle = startDirection + sign * Mathf.Max(0f, (float)(i - 60) * 1f);
			for (int j = 0; j < 5; j++)
			{
				float num = offset + 20f + (float)j * 10f;
				base.Fire(new Offset("right eye"), new Direction(currentAngle + num, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
				base.Fire(new Offset("left eye"), new Direction(currentAngle - num, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
			}
			if (i > 30 && i % 30 == 29)
			{
				base.Fire(new Direction(currentAngle + UnityEngine.Random.Range(-1f, 1f) * 20f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			}
			if (i >= 60)
			{
				float num2 = Vector2.Distance(this.BulletManager.PlayerPosition(), base.Position);
				float num3 = num2 / 18f * 30f;
				if (num3 > (float)(i - 60))
				{
					num3 = (float)Mathf.Max(0, i - 60);
				}
				float num4 = -sign * num3 * 1f;
				float num5 = currentAngle + 40f + num4;
				float num6 = currentAngle - 40f + num4;
				if (BraveMathCollege.ClampAngle180(num5 - base.GetAimDirection("right eye")) < 0f)
				{
					yield break;
				}
				if (BraveMathCollege.ClampAngle180(num6 - base.GetAimDirection("left eye")) > 0f)
				{
					yield break;
				}
			}
			yield return base.Wait(2);
		}
		yield break;
	}

	// Token: 0x0400064A RID: 1610
	private const float CoreSpread = 20f;

	// Token: 0x0400064B RID: 1611
	private const float IncSpread = 10f;

	// Token: 0x0400064C RID: 1612
	private const float TurnSpeed = 1f;

	// Token: 0x0400064D RID: 1613
	private const float BulletSpeed = 18f;
}
