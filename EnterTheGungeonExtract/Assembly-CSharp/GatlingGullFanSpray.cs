using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB3 RID: 3507
[InspectorDropdownName("Bosses/GatlingGull/FanSprayBehavior")]
public class GatlingGullFanSpray : BasicAttackBehavior
{
	// Token: 0x06004A47 RID: 19015 RVA: 0x0018DEDC File Offset: 0x0018C0DC
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x0018DEE4 File Offset: 0x0018C0E4
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x0018DEEC File Offset: 0x0018C0EC
	public override BehaviorResult Update()
	{
		base.Update();
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.m_aiShooter.volley.projectiles[0].angleVariance = 0f;
		AkSoundEngine.PostEvent("Play_ANM_Gull_Shoot_01", this.m_gameObject);
		this.m_totalDuration = this.SprayAngle / this.SpraySpeed * (float)this.SprayIterations;
		this.m_remainingDuration = this.m_totalDuration;
		this.m_aiActor.ClearPath();
		AkSoundEngine.PostEvent("Play_ANM_Gull_Loop_01", this.m_gameObject);
		AkSoundEngine.PostEvent("Play_ANM_Gull_Gatling_01", this.m_gameObject);
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x0018DFA0 File Offset: 0x0018C1A0
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		base.DecrementTimer(ref this.m_remainingDuration, false);
		if (this.m_remainingDuration <= 0f || !this.m_aiActor.TargetRigidbody)
		{
			return ContinuousBehaviorResult.Finished;
		}
		float num = 1f - this.m_remainingDuration / this.m_totalDuration;
		float num2 = num * (float)this.SprayIterations % 2f;
		float num3 = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiShooter.volleyShootPosition.position.XY()).ToAngle();
		num3 = BraveMathCollege.QuantizeFloat(num3, 45f);
		num3 += -this.SprayAngle / 2f + Mathf.PingPong(num2 * this.SprayAngle, this.SprayAngle);
		this.m_aiShooter.ShootInDirection(Quaternion.Euler(0f, 0f, num3) * Vector2.right, this.OverrideBulletName);
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x0018E0A4 File Offset: 0x0018C2A4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		AkSoundEngine.PostEvent("Stop_ANM_Gull_Loop_01", this.m_gameObject);
		this.UpdateCooldowns();
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x0018E0C4 File Offset: 0x0018C2C4
	public override void Destroy()
	{
		base.Destroy();
		if (this.m_aiActor.GetComponent<AkGameObj>())
		{
			AkSoundEngine.PostEvent("Stop_ANM_Gull_Loop_01", this.m_gameObject);
		}
	}

	// Token: 0x04003EFB RID: 16123
	public float SprayAngle = 120f;

	// Token: 0x04003EFC RID: 16124
	public float SpraySpeed = 60f;

	// Token: 0x04003EFD RID: 16125
	public int SprayIterations = 4;

	// Token: 0x04003EFE RID: 16126
	public string OverrideBulletName;

	// Token: 0x04003EFF RID: 16127
	private float m_remainingDuration;

	// Token: 0x04003F00 RID: 16128
	private float m_totalDuration;
}
