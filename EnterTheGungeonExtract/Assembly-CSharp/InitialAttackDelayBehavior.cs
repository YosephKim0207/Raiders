using System;
using UnityEngine;

// Token: 0x02000D2E RID: 3374
public class InitialAttackDelayBehavior : AttackBehaviorBase
{
	// Token: 0x0600473E RID: 18238 RVA: 0x00174F30 File Offset: 0x00173130
	public override void Start()
	{
		base.Start();
		if (this.m_aiActor.healthHaver && this.EndOnDamage)
		{
			this.m_aiActor.healthHaver.OnDamaged += this.OnDamaged;
		}
	}

	// Token: 0x0600473F RID: 18239 RVA: 0x00174F80 File Offset: 0x00173180
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004740 RID: 18240 RVA: 0x00174F98 File Offset: 0x00173198
	public override BehaviorResult Update()
	{
		base.Update();
		if (!this.m_done)
		{
			if (!string.IsNullOrEmpty(this.PlayDirectionalAnimation))
			{
				this.m_aiAnimator.PlayUntilFinished(this.PlayDirectionalAnimation, true, null, -1f, false);
			}
			if (!string.IsNullOrEmpty(this.SetDefaultDirectionalAnimation))
			{
				this.m_aiAnimator.SetBaseAnim(this.SetDefaultDirectionalAnimation, false);
			}
			this.m_timer = this.Time;
			return BehaviorResult.RunContinuousInClass;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x00175014 File Offset: 0x00173214
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_timer > 0f)
		{
			this.m_aiActor.ClearPath();
			return ContinuousBehaviorResult.Continue;
		}
		return ContinuousBehaviorResult.Finished;
	}

	// Token: 0x06004742 RID: 18242 RVA: 0x00175034 File Offset: 0x00173234
	public override void EndContinuousUpdate()
	{
		if (!string.IsNullOrEmpty(this.PlayDirectionalAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.PlayDirectionalAnimation);
		}
		this.m_done = true;
		if (this.m_aiActor.healthHaver && this.EndOnDamage)
		{
			this.m_aiActor.healthHaver.OnDamaged -= this.OnDamaged;
		}
	}

	// Token: 0x06004743 RID: 18243 RVA: 0x001750A8 File Offset: 0x001732A8
	public override bool IsReady()
	{
		return !this.m_done;
	}

	// Token: 0x06004744 RID: 18244 RVA: 0x001750B4 File Offset: 0x001732B4
	public override float GetMinReadyRange()
	{
		return -1f;
	}

	// Token: 0x06004745 RID: 18245 RVA: 0x001750BC File Offset: 0x001732BC
	public override float GetMaxRange()
	{
		return -1f;
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x001750C4 File Offset: 0x001732C4
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (this.EndOnDamage)
		{
			this.m_timer = 0f;
		}
	}

	// Token: 0x04003A2A RID: 14890
	public float Time = 2f;

	// Token: 0x04003A2B RID: 14891
	public string PlayDirectionalAnimation;

	// Token: 0x04003A2C RID: 14892
	public string SetDefaultDirectionalAnimation;

	// Token: 0x04003A2D RID: 14893
	public bool EndOnDamage;

	// Token: 0x04003A2E RID: 14894
	private float m_timer;

	// Token: 0x04003A2F RID: 14895
	private bool m_done;
}
