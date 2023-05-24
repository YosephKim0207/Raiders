using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001AC RID: 428
[InspectorDropdownName("Bosses/GiantPowderSkull/CannonVolley1")]
public class GiantPowderSkullCannonVolley1 : Script
{
	// Token: 0x0600065E RID: 1630 RVA: 0x0001E9C8 File Offset: 0x0001CBC8
	protected override IEnumerator Top()
	{
		AIAnimator aiAnimator = base.BulletBank.aiAnimator;
		string text = "eyeflash";
		Vector2? vector = new Vector2?(base.Position);
		aiAnimator.PlayVfx(text, null, null, vector);
		yield return base.Wait(30);
		float angle = base.AimDirection;
		for (int i = 0; i < 5; i++)
		{
			float y = Mathf.Lerp(-4.5f, 4.5f, (float)i / 4f);
			base.Fire(new Offset(new Vector2(1f, y), angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(13f, SpeedType.Absolute), new Bullet("cannon", false, false, false));
			yield return base.Wait(5);
		}
		yield break;
	}

	// Token: 0x04000630 RID: 1584
	private const int NumBullets = 5;

	// Token: 0x04000631 RID: 1585
	private const float HalfWidth = 4.5f;
}
