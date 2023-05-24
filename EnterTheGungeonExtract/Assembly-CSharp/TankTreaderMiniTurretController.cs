using System;
using UnityEngine;

// Token: 0x0200107C RID: 4220
public class TankTreaderMiniTurretController : BodyPartController
{
	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x06005CE1 RID: 23777 RVA: 0x00239158 File Offset: 0x00237358
	// (set) Token: 0x06005CE2 RID: 23778 RVA: 0x00239160 File Offset: 0x00237360
	public TankTreaderMiniTurretController.AimMode aimMode { get; set; }

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x06005CE3 RID: 23779 RVA: 0x0023916C File Offset: 0x0023736C
	// (set) Token: 0x06005CE4 RID: 23780 RVA: 0x00239174 File Offset: 0x00237374
	public float? OverrideAngle { get; set; }

	// Token: 0x06005CE5 RID: 23781 RVA: 0x00239180 File Offset: 0x00237380
	public override void Start()
	{
		base.Start();
		this.aimMode = TankTreaderMiniTurretController.AimMode.Away;
	}

	// Token: 0x06005CE6 RID: 23782 RVA: 0x00239190 File Offset: 0x00237390
	public void OnEnable()
	{
		this.m_state = TankTreaderMiniTurretController.State.Cooldown;
		this.m_cooldown = UnityEngine.Random.Range(this.MinCooldown, this.MaxCooldown);
	}

	// Token: 0x06005CE7 RID: 23783 RVA: 0x002391B0 File Offset: 0x002373B0
	public override void Update()
	{
		base.Update();
		bool flag = false;
		if (this.aimMode == TankTreaderMiniTurretController.AimMode.AtPlayer)
		{
			if (this.m_body.TargetRigidbody)
			{
				Vector2 unitCenter = this.m_body.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
				float num = (unitCenter - base.transform.position).ToAngle();
				flag = BraveMathCollege.IsAngleWithinSweepArea(num, this.StartAngle + this.m_body.aiAnimator.FacingDirection + 90f, this.SweepAngle);
			}
		}
		else if (this.aimMode == TankTreaderMiniTurretController.AimMode.Away)
		{
			flag = true;
		}
		if (this.m_state == TankTreaderMiniTurretController.State.Idle)
		{
			if (flag)
			{
				this.m_state = TankTreaderMiniTurretController.State.Firing;
				this.m_fireTimeRemaining = this.FireTime;
				this.m_timeUntilNextShot = 0f;
			}
		}
		else if (this.m_state == TankTreaderMiniTurretController.State.Firing)
		{
			this.m_fireTimeRemaining -= BraveTime.DeltaTime;
			this.m_timeUntilNextShot -= BraveTime.DeltaTime;
			if (!flag || this.m_fireTimeRemaining <= 0f)
			{
				this.m_state = TankTreaderMiniTurretController.State.Cooldown;
				this.m_cooldown = UnityEngine.Random.Range(this.MinCooldown, this.MaxCooldown);
			}
			else if (this.m_timeUntilNextShot <= 0f)
			{
				this.Fire();
				this.m_timeUntilNextShot = this.ShotCooldown;
			}
		}
		else if (this.m_state == TankTreaderMiniTurretController.State.Cooldown)
		{
			this.m_cooldown = Mathf.Max(0f, this.m_cooldown - BraveTime.DeltaTime);
			if (this.m_cooldown <= 0f)
			{
				this.m_state = TankTreaderMiniTurretController.State.Idle;
			}
		}
	}

	// Token: 0x06005CE8 RID: 23784 RVA: 0x00239354 File Offset: 0x00237554
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005CE9 RID: 23785 RVA: 0x0023935C File Offset: 0x0023755C
	private void Fire()
	{
		GameObject gameObject = this.m_body.bulletBank.CreateProjectileFromBank(this.ShootPoint.transform.position, base.transform.eulerAngles.z, this.BulletName, null, false, true, false);
		Projectile component = gameObject.GetComponent<Projectile>();
		Vector2 vector = BraveMathCollege.DegreesToVector(base.transform.eulerAngles.z, component.baseData.speed);
		Vector2 velocity = this.specifyActor.specRigidbody.Velocity;
		component.Direction = (vector + velocity).normalized;
		component.Speed = (vector + velocity).magnitude;
	}

	// Token: 0x06005CEA RID: 23786 RVA: 0x00239418 File Offset: 0x00237618
	protected override bool TryGetAimAngle(out float angle)
	{
		if (this.OverrideAngle != null)
		{
			angle = this.OverrideAngle.Value;
			return true;
		}
		if (this.aimMode == TankTreaderMiniTurretController.AimMode.Away)
		{
			angle = this.StartAngle + 0.5f * this.SweepAngle;
			angle += this.m_body.aiAnimator.FacingDirection + 90f;
			return true;
		}
		return base.TryGetAimAngle(out angle);
	}

	// Token: 0x0400569C RID: 22172
	public GameObject ShootPoint;

	// Token: 0x0400569D RID: 22173
	public string BulletName;

	// Token: 0x0400569E RID: 22174
	public float FireTime;

	// Token: 0x0400569F RID: 22175
	public float ShotCooldown;

	// Token: 0x040056A0 RID: 22176
	public float MinCooldown;

	// Token: 0x040056A1 RID: 22177
	public float MaxCooldown;

	// Token: 0x040056A2 RID: 22178
	public float StartAngle;

	// Token: 0x040056A3 RID: 22179
	public float SweepAngle;

	// Token: 0x040056A6 RID: 22182
	private float m_fireTimeRemaining;

	// Token: 0x040056A7 RID: 22183
	private float m_timeUntilNextShot;

	// Token: 0x040056A8 RID: 22184
	private float m_cooldown;

	// Token: 0x040056A9 RID: 22185
	private static int m_arcCount;

	// Token: 0x040056AA RID: 22186
	private static int m_lastFrame;

	// Token: 0x040056AB RID: 22187
	private TankTreaderMiniTurretController.State m_state;

	// Token: 0x0200107D RID: 4221
	public enum AimMode
	{
		// Token: 0x040056AD RID: 22189
		AtPlayer,
		// Token: 0x040056AE RID: 22190
		Away
	}

	// Token: 0x0200107E RID: 4222
	private enum State
	{
		// Token: 0x040056B0 RID: 22192
		Idle,
		// Token: 0x040056B1 RID: 22193
		Firing,
		// Token: 0x040056B2 RID: 22194
		Cooldown
	}
}
