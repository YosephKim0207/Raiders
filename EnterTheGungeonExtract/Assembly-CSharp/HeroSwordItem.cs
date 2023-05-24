using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200141C RID: 5148
public class HeroSwordItem : PlayerItem
{
	// Token: 0x060074D7 RID: 29911 RVA: 0x002E87EC File Offset: 0x002E69EC
	protected override void DoEffect(PlayerController user)
	{
		Vector2 vector = user.unadjustedAimPoint.XY() - user.CenterPosition;
		float num = BraveMathCollege.Atan2Degrees(vector);
		float num2 = this.Damage;
		float num3 = this.DamageLength;
		if (user.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			num2 = this.MaxHealthDamage;
			num3 = this.MaxHealthDamageLength;
			this.MaxHealthSwordVFX.SpawnAtPosition(user.CenterPosition, num, user.transform, null, null, new float?(1f), false, null, user.sprite, false);
		}
		else
		{
			this.NormalSwordVFX.SpawnAtPosition(user.CenterPosition, num, user.transform, null, null, new float?(1f), false, null, user.sprite, false);
		}
		user.StartCoroutine(this.HandleSwing(user, vector, num2, num3));
	}

	// Token: 0x060074D8 RID: 29912 RVA: 0x002E88E8 File Offset: 0x002E6AE8
	private IEnumerator HandleSwing(PlayerController user, Vector2 aimVec, float rayDamage, float rayLength)
	{
		float elapsed = 0f;
		while (elapsed < this.SwingDuration)
		{
			elapsed += BraveTime.DeltaTime;
			SpeculativeRigidbody hitRigidbody = this.IterativeRaycast(user.CenterPosition, aimVec, rayLength, int.MaxValue, user.specRigidbody);
			if (hitRigidbody && hitRigidbody.aiActor && hitRigidbody.aiActor.IsNormalEnemy)
			{
				hitRigidbody.aiActor.healthHaver.ApplyDamage(rayDamage, aimVec, "Hero's Sword", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060074D9 RID: 29913 RVA: 0x002E8920 File Offset: 0x002E6B20
	protected SpeculativeRigidbody IterativeRaycast(Vector2 rayOrigin, Vector2 rayDirection, float rayDistance, int collisionMask, SpeculativeRigidbody ignoreRigidbody)
	{
		int num = 0;
		RaycastResult raycastResult;
		while (PhysicsEngine.Instance.Raycast(rayOrigin, rayDirection, rayDistance, out raycastResult, true, true, collisionMask, new CollisionLayer?(CollisionLayer.Projectile), false, null, ignoreRigidbody))
		{
			num++;
			SpeculativeRigidbody speculativeRigidbody = raycastResult.SpeculativeRigidbody;
			if (num < 3 && speculativeRigidbody != null)
			{
				MinorBreakable component = speculativeRigidbody.GetComponent<MinorBreakable>();
				if (component != null)
				{
					component.Break(rayDirection.normalized * 3f);
					RaycastResult.Pool.Free(ref raycastResult);
					continue;
				}
			}
			RaycastResult.Pool.Free(ref raycastResult);
			return speculativeRigidbody;
		}
		return null;
	}

	// Token: 0x060074DA RID: 29914 RVA: 0x002E89C0 File Offset: 0x002E6BC0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040076A6 RID: 30374
	public float Damage = 20f;

	// Token: 0x040076A7 RID: 30375
	public float MaxHealthDamage = 30f;

	// Token: 0x040076A8 RID: 30376
	public float DamageLength = 1.25f;

	// Token: 0x040076A9 RID: 30377
	public float MaxHealthDamageLength = 2.5f;

	// Token: 0x040076AA RID: 30378
	private float SwingDuration = 0.5f;

	// Token: 0x040076AB RID: 30379
	public VFXPool NormalSwordVFX;

	// Token: 0x040076AC RID: 30380
	public VFXPool MaxHealthSwordVFX;
}
