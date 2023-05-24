using System;
using UnityEngine;

// Token: 0x02000D2D RID: 3373
public class IncrediblySimpleShootBehavior : BasicAttackBehavior
{
	// Token: 0x06004738 RID: 18232 RVA: 0x00174DB0 File Offset: 0x00172FB0
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x00174DB8 File Offset: 0x00172FB8
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x00174DC0 File Offset: 0x00172FC0
	private void HandleAIShootVolley()
	{
		this.m_aiShooter.ShootInDirection(this.ShootDirection, this.OverrideBulletName);
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x00174DDC File Offset: 0x00172FDC
	private void HandleAIShoot()
	{
		this.m_aiShooter.ShootInDirection(this.ShootDirection, this.OverrideBulletName);
	}

	// Token: 0x0600473C RID: 18236 RVA: 0x00174DF8 File Offset: 0x00172FF8
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.HandleAIShoot();
		if (!string.IsNullOrEmpty(this.OverrideDirectionalAnimation))
		{
			if (this.m_aiAnimator != null)
			{
				this.m_aiAnimator.PlayUntilFinished(this.OverrideDirectionalAnimation, true, null, -1f, false);
			}
			else
			{
				this.m_aiActor.spriteAnimator.PlayForDuration(this.OverrideDirectionalAnimation, -1f, this.m_aiActor.spriteAnimator.CurrentClip.name, false);
			}
		}
		else if (!string.IsNullOrEmpty(this.OverrideAnimation))
		{
			if (this.m_aiAnimator != null)
			{
				this.m_aiAnimator.PlayUntilFinished(this.OverrideAnimation, false, null, -1f, false);
			}
			else
			{
				this.m_aiActor.spriteAnimator.PlayForDuration(this.OverrideAnimation, -1f, this.m_aiActor.spriteAnimator.CurrentClip.name, false);
			}
		}
		this.UpdateCooldowns();
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x04003A25 RID: 14885
	public Vector2 ShootDirection = Vector2.right;

	// Token: 0x04003A26 RID: 14886
	public WeaponType WeaponType;

	// Token: 0x04003A27 RID: 14887
	public string OverrideBulletName;

	// Token: 0x04003A28 RID: 14888
	public string OverrideAnimation;

	// Token: 0x04003A29 RID: 14889
	public string OverrideDirectionalAnimation;
}
