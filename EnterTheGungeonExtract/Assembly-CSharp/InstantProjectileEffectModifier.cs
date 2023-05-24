using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001654 RID: 5716
public class InstantProjectileEffectModifier : BraveBehaviour
{
	// Token: 0x06008573 RID: 34163 RVA: 0x0037117C File Offset: 0x0036F37C
	private IEnumerator Start()
	{
		yield return null;
		if (this.DoesWhiteFlash)
		{
			Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.1f);
		}
		RoomHandler currentRoom = base.transform.position.GetAbsoluteRoom();
		currentRoom.ApplyActionToNearbyEnemies(base.transform.position.XY(), this.RoomDamageRadius, delegate(AIActor a, float b)
		{
			if (a && a.IsNormalEnemy && a.healthHaver)
			{
				a.healthHaver.ApplyDamage(base.projectile.ModifiedDamage, Vector2.zero, "projectile", base.projectile.damageTypes, DamageCategory.Normal, false, null, false);
			}
		});
		this.AdditionalVFX.SpawnAtPosition(base.transform.position.XY(), 0f, null, null, null, null, false, null, null, false);
		if (this.DoesAdditionalScreenShake)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(this.AdditionalScreenShake, new Vector2?(base.transform.position.XY()), true);
		}
		if (this.DoesRadialProjectileModule && base.projectile.Owner is PlayerController)
		{
			this.RadialModule.DoBurst(base.projectile.Owner as PlayerController, null, null);
		}
		base.projectile.DieInAir(false, true, true, false);
		yield break;
	}

	// Token: 0x040089AF RID: 35247
	public bool DoesWhiteFlash;

	// Token: 0x040089B0 RID: 35248
	public float RoomDamageRadius = 10f;

	// Token: 0x040089B1 RID: 35249
	public VFXPool AdditionalVFX;

	// Token: 0x040089B2 RID: 35250
	public bool DoesAdditionalScreenShake;

	// Token: 0x040089B3 RID: 35251
	[ShowInInspectorIf("DoesAdditionalScreenShake", false)]
	public ScreenShakeSettings AdditionalScreenShake;

	// Token: 0x040089B4 RID: 35252
	public bool DoesRadialProjectileModule;

	// Token: 0x040089B5 RID: 35253
	[ShowInInspectorIf("DoesRadialProjectileModule", false)]
	public RadialBurstInterface RadialModule;
}
