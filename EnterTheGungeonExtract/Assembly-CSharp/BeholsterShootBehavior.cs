using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D83 RID: 3459
[InspectorDropdownName("Bosses/Beholster/ShootBehavior")]
public class BeholsterShootBehavior : BasicAttackBehavior
{
	// Token: 0x06004941 RID: 18753 RVA: 0x00186F18 File Offset: 0x00185118
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x00186F20 File Offset: 0x00185120
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_windupTimer, false);
	}

	// Token: 0x06004943 RID: 18755 RVA: 0x00186F38 File Offset: 0x00185138
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		bool flag = this.LineOfSight && !this.m_aiActor.HasLineOfSightToTarget;
		if (this.m_aiActor.TargetRigidbody == null || flag)
		{
			return BehaviorResult.Continue;
		}
		this.m_state = BeholsterShootBehavior.State.Windup;
		this.m_windupTimer = this.WindUpTime;
		this.m_aiActor.ClearPath();
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004944 RID: 18756 RVA: 0x00186FBC File Offset: 0x001851BC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == BeholsterShootBehavior.State.Windup)
		{
			if (this.m_windupTimer <= 0f)
			{
				if (this.m_aiActor.TargetRigidbody)
				{
					this.m_aiActor.bulletBank.FixedPlayerPosition = new Vector2?(this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox));
				}
				this.m_aiAnimator.LockFacingDirection = true;
				this.Tentacle.BulletScriptSource.FreezeTopPosition = true;
				this.Tentacle.ShootBulletScript(this.BulletScript);
				this.m_state = BeholsterShootBehavior.State.Firing;
			}
			return ContinuousBehaviorResult.Continue;
		}
		if (this.m_state == BeholsterShootBehavior.State.Firing && this.Tentacle.BulletScriptSource.IsEnded)
		{
			this.m_state = BeholsterShootBehavior.State.Ready;
			this.m_aiActor.bulletBank.FixedPlayerPosition = null;
			this.m_aiAnimator.LockFacingDirection = false;
			this.Tentacle.BulletScriptSource.FreezeTopPosition = false;
			this.UpdateCooldowns();
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x04003D79 RID: 15737
	public bool LineOfSight = true;

	// Token: 0x04003D7A RID: 15738
	public float WindUpTime = 1f;

	// Token: 0x04003D7B RID: 15739
	public BulletScriptSelector BulletScript;

	// Token: 0x04003D7C RID: 15740
	public BeholsterTentacleController Tentacle;

	// Token: 0x04003D7D RID: 15741
	private BeholsterShootBehavior.State m_state;

	// Token: 0x04003D7E RID: 15742
	private float m_windupTimer;

	// Token: 0x02000D84 RID: 3460
	private enum State
	{
		// Token: 0x04003D80 RID: 15744
		Ready,
		// Token: 0x04003D81 RID: 15745
		Windup,
		// Token: 0x04003D82 RID: 15746
		Firing
	}
}
