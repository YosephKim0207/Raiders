using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DBA RID: 3514
[InspectorDropdownName("Bosses/GatlingGull/WalkAndShoot")]
public class GatlingGullWalkAndShoot : BasicAttackBehavior
{
	// Token: 0x06004A87 RID: 19079 RVA: 0x001901F8 File Offset: 0x0018E3F8
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004A88 RID: 19080 RVA: 0x00190200 File Offset: 0x0018E400
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_durationTimer, false);
	}

	// Token: 0x06004A89 RID: 19081 RVA: 0x00190218 File Offset: 0x0018E418
	public override BehaviorResult Update()
	{
		base.Update();
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.m_durationTimer = this.Duration;
		this.m_aiActor.SuppressTargetSwitch = true;
		AkSoundEngine.PostEvent("Play_ANM_Gull_Loop_01", this.m_gameObject);
		AkSoundEngine.PostEvent("Play_ANM_Gull_Gatling_01", this.m_gameObject);
		return BehaviorResult.RunContinuousInClass;
	}

	// Token: 0x06004A8A RID: 19082 RVA: 0x00190280 File Offset: 0x0018E480
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.ContinuesOnPathComplete)
		{
			this.m_aiAnimator.OverrideIdleAnimation = "idle_shoot";
		}
		if (this.m_durationTimer <= 0f || !this.m_aiActor.TargetRigidbody || (this.m_aiActor.PathComplete && !this.ContinuesOnPathComplete))
		{
			return ContinuousBehaviorResult.Finished;
		}
		Vector2 vector = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.CenterPosition;
		int num = BraveMathCollege.VectorToOctant(vector);
		this.m_aiShooter.ManualGunAngle = true;
		this.m_aiShooter.GunAngle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		Vector2 vector2 = Quaternion.Euler(0f, 0f, (float)(num * -45)) * Vector2.up;
		this.m_aiShooter.volley.projectiles[0].angleVariance = this.AngleVariance;
		this.m_aiShooter.ShootInDirection(vector2, this.OverrideBulletName);
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x001903A8 File Offset: 0x0018E5A8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.ContinuesOnPathComplete)
		{
			this.m_aiAnimator.OverrideIdleAnimation = string.Empty;
		}
		this.m_aiShooter.ManualGunAngle = false;
		this.UpdateCooldowns();
		this.m_aiActor.SuppressTargetSwitch = false;
		AkSoundEngine.PostEvent("Stop_ANM_Gull_Loop_01", this.m_gameObject);
	}

	// Token: 0x06004A8C RID: 19084 RVA: 0x00190408 File Offset: 0x0018E608
	public override void Destroy()
	{
		base.Destroy();
		if (this.m_aiActor.GetComponent<AkGameObj>())
		{
			AkSoundEngine.PostEvent("Stop_ANM_Gull_Loop_01", this.m_gameObject);
		}
	}

	// Token: 0x04003F48 RID: 16200
	public float Duration = 5f;

	// Token: 0x04003F49 RID: 16201
	public float AngleVariance = 20f;

	// Token: 0x04003F4A RID: 16202
	public bool ContinuesOnPathComplete;

	// Token: 0x04003F4B RID: 16203
	public string OverrideBulletName;

	// Token: 0x04003F4C RID: 16204
	private float m_durationTimer;
}
