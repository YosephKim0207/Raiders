using System;
using UnityEngine;

// Token: 0x02001410 RID: 5136
public class GuidedBulletsPassiveItem : PassiveItem
{
	// Token: 0x06007491 RID: 29841 RVA: 0x002E6908 File Offset: 0x002E4B08
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeam += this.PostProcessBeam;
	}

	// Token: 0x06007492 RID: 29842 RVA: 0x002E6948 File Offset: 0x002E4B48
	private void PostProcessBeam(BeamController obj)
	{
	}

	// Token: 0x06007493 RID: 29843 RVA: 0x002E694C File Offset: 0x002E4B4C
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		obj.PreMoveModifiers = (Action<Projectile>)Delegate.Combine(obj.PreMoveModifiers, new Action<Projectile>(this.PreMoveProjectileModifier));
	}

	// Token: 0x06007494 RID: 29844 RVA: 0x002E6970 File Offset: 0x002E4B70
	private void PreMoveProjectileModifier(Projectile p)
	{
		if (this.m_owner && p && p.Owner is PlayerController)
		{
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(this.m_owner.PlayerIDX);
			if (instanceForPlayer == null)
			{
				return;
			}
			Vector2 vector = Vector2.zero;
			if (instanceForPlayer.IsKeyboardAndMouse(false))
			{
				vector = (p.Owner as PlayerController).unadjustedAimPoint.XY() - p.specRigidbody.UnitCenter;
			}
			else
			{
				if (instanceForPlayer.ActiveActions == null)
				{
					return;
				}
				vector = instanceForPlayer.ActiveActions.Aim.Vector;
			}
			float num = vector.ToAngle();
			float num2 = BraveMathCollege.Atan2Degrees(p.Direction);
			float num3 = 0f;
			if (p.ElapsedTime < this.trackingTime)
			{
				num3 = this.trackingCurve.Evaluate(p.ElapsedTime / this.trackingTime) * this.trackingSpeed;
			}
			float num4 = Mathf.MoveTowardsAngle(num2, num, num3 * BraveTime.DeltaTime);
			Vector2 vector2 = Quaternion.Euler(0f, 0f, Mathf.DeltaAngle(num2, num4)) * p.Direction;
			if (p is HelixProjectile)
			{
				HelixProjectile helixProjectile = p as HelixProjectile;
				helixProjectile.AdjustRightVector(Mathf.DeltaAngle(num2, num4));
			}
			if (p.OverrideMotionModule != null)
			{
				p.OverrideMotionModule.AdjustRightVector(Mathf.DeltaAngle(num2, num4));
			}
			p.Direction = vector2.normalized;
			if (p.shouldRotate)
			{
				p.transform.eulerAngles = new Vector3(0f, 0f, p.Direction.ToAngle());
			}
		}
	}

	// Token: 0x06007495 RID: 29845 RVA: 0x002E6B30 File Offset: 0x002E4D30
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<GuidedBulletsPassiveItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		return debrisObject;
	}

	// Token: 0x06007496 RID: 29846 RVA: 0x002E6B80 File Offset: 0x002E4D80
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
		}
	}

	// Token: 0x04007669 RID: 30313
	public float trackingSpeed = 45f;

	// Token: 0x0400766A RID: 30314
	public float trackingTime = 6f;

	// Token: 0x0400766B RID: 30315
	[CurveRange(0f, 0f, 1f, 1f)]
	public AnimationCurve trackingCurve;

	// Token: 0x0400766C RID: 30316
	private PlayerController m_player;
}
