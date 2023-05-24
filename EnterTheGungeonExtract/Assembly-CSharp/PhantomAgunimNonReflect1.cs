using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002B9 RID: 697
[InspectorDropdownName("Minibosses/PhantomAgunim/Reflect1")]
public class PhantomAgunimNonReflect1 : Script
{
	// Token: 0x06000AB1 RID: 2737 RVA: 0x0003366C File Offset: 0x0003186C
	protected override IEnumerator Top()
	{
		yield return base.Wait(48);
		if (base.BulletBank)
		{
			base.BulletBank.aiAnimator.PlayVfx("hover_charge_loop", null, null, null);
		}
		float sign = (float)((!BraveUtility.RandomBool()) ? 1 : (-1));
		for (int i = 0; i < 6; i++)
		{
			float startDirection = base.AimDirection + sign * (float)(i + 1) * 10f;
			float rotationSpeed = -sign * (45f + (float)(i + 1) * 10f);
			for (int j = 0; j < 5; j++)
			{
				float num = base.SubdivideCircle(0f, 5, j, 1f, false);
				PhantomAgunimNonReflect1.RingBullet ringBullet = new PhantomAgunimNonReflect1.RingBullet(num, rotationSpeed);
				base.Fire(new Direction(startDirection, DirectionType.Absolute, -1f), new Speed(10f + (float)i * 2f, SpeedType.Absolute), ringBullet);
				ringBullet.Projectile.IgnoreTileCollisionsFor(1f);
			}
			sign *= -1f;
			yield return base.Wait(20);
			AkSoundEngine.PostEvent("Play_BOSS_agunim_orb_01", base.BulletBank.gameObject);
		}
		yield return base.Wait(20);
		if (base.BulletBank)
		{
			base.BulletBank.aiAnimator.StopVfx("hover_charge_loop");
			base.BulletBank.aiAnimator.PlayVfx("hover_charge_end", null, null, null);
		}
		yield return base.Wait(40);
		yield break;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00033688 File Offset: 0x00031888
	public override void OnForceEnded()
	{
		if (base.BulletBank && base.BulletBank.aiAnimator)
		{
			base.BulletBank.aiAnimator.StopVfx("hover_charge_loop");
		}
	}

	// Token: 0x04000B2E RID: 2862
	private const int NumRings = 6;

	// Token: 0x04000B2F RID: 2863
	private const int NumBulletsPerRing = 5;

	// Token: 0x04000B30 RID: 2864
	private const float RingRadius = 0.55f;

	// Token: 0x04000B31 RID: 2865
	private const float RingSpinSpeed = 450f;

	// Token: 0x04000B32 RID: 2866
	private const int RingDelay = 20;

	// Token: 0x04000B33 RID: 2867
	private const float DeltaStartAim = 10f;

	// Token: 0x04000B34 RID: 2868
	private const float StartSpeed = 10f;

	// Token: 0x04000B35 RID: 2869
	private const float SpeedIncrease = 2f;

	// Token: 0x04000B36 RID: 2870
	private const float RotationSpeed = 45f;

	// Token: 0x04000B37 RID: 2871
	private const float RotationSpeedIncrease = 10f;

	// Token: 0x020002BA RID: 698
	public class RingBullet : Bullet
	{
		// Token: 0x06000AB3 RID: 2739 RVA: 0x000336C4 File Offset: 0x000318C4
		public RingBullet(float angle, float rotationSpeed)
			: base("ring", false, false, false)
		{
			this.m_angle = angle;
			this.m_rotationSpeed = rotationSpeed;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000336E4 File Offset: 0x000318E4
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.IgnoreTileCollisionsFor(0.6f);
			Vector2 centerPosition = base.Position;
			for (int i = 0; i < 20; i++)
			{
				this.m_angle += 7.5f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.m_angle, 0.55f);
				yield return base.Wait(1);
			}
			for (int j = 0; j < 300; j++)
			{
				this.Direction += this.m_rotationSpeed / 60f * Mathf.Lerp(1f, 0f, (float)((j - 35) / 30));
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				this.m_angle += 7.5f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.m_angle, 0.55f);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000B38 RID: 2872
		private const int TicksBeforeStrighteningOut = 35;

		// Token: 0x04000B39 RID: 2873
		private const int TicksToStraightenOut = 30;

		// Token: 0x04000B3A RID: 2874
		private float m_angle;

		// Token: 0x04000B3B RID: 2875
		private float m_rotationSpeed;
	}
}
