using System;
using UnityEngine;

// Token: 0x02000DDF RID: 3551
public class ResourcefulRatChamberBehavior : OverrideBehaviorBase
{
	// Token: 0x06004B33 RID: 19251 RVA: 0x00196E1C File Offset: 0x0019501C
	public override void Start()
	{
		base.Start();
		this.m_updateEveryFrame = true;
		this.m_ignoreGlobalCooldown = true;
	}

	// Token: 0x06004B34 RID: 19252 RVA: 0x00196E34 File Offset: 0x00195034
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004B35 RID: 19253 RVA: 0x00196E3C File Offset: 0x0019503C
	private bool ReadyForNextPhase()
	{
		return (this.m_currentPhase == 1 && this.m_aiActor.healthHaver.GetCurrentHealthPercentage() < this.HealthThresholdPhaseTwo) || (this.m_currentPhase == 2 && this.m_aiActor.healthHaver.GetCurrentHealthPercentage() < this.HealthThresholdPhaseThree);
	}

	// Token: 0x06004B36 RID: 19254 RVA: 0x00196E9C File Offset: 0x0019509C
	public override bool OverrideOtherBehaviors()
	{
		return this.ReadyForNextPhase() || this.m_isActive;
	}

	// Token: 0x06004B37 RID: 19255 RVA: 0x00196EB4 File Offset: 0x001950B4
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.ReadyForNextPhase())
		{
			this.m_currentPhase++;
			this.m_aiActor.MovementModifiers += this.m_aiActor_MovementModifiers;
			this.m_aiActor.BehaviorOverridesVelocity = false;
			this.m_aiAnimator.LockFacingDirection = false;
			this.m_aiActor.healthHaver.IsVulnerable = false;
			this.m_aiActor.specRigidbody.CollideWithOthers = false;
			IntVector2 basePosition = this.m_aiActor.ParentRoom.area.basePosition;
			Vector2 vector = basePosition.ToVector2() + this.m_aiActor.ParentRoom.area.dimensions.ToVector2().WithY(0f) / 2f;
			Vector2 vector2 = new Vector2(0f, 35f);
			if (this.m_currentPhase == 3)
			{
				vector2 = new Vector2(0f, 52f);
			}
			this.m_aiActor.PathfindToPosition(vector + vector2, null, true, null, null, null, false);
			this.m_isActive = true;
			return BehaviorResult.RunContinuous;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004B38 RID: 19256 RVA: 0x00196FE8 File Offset: 0x001951E8
	private void m_aiActor_MovementModifiers(ref Vector2 volundaryVel, ref Vector2 involuntaryVel)
	{
		volundaryVel *= 4f;
	}

	// Token: 0x06004B39 RID: 19257 RVA: 0x00197000 File Offset: 0x00195200
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004B3A RID: 19258 RVA: 0x00197004 File Offset: 0x00195204
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_aiActor.PathComplete)
		{
			this.m_aiActor.MovementModifiers -= this.m_aiActor_MovementModifiers;
			this.m_aiActor.healthHaver.IsVulnerable = true;
			this.m_aiActor.specRigidbody.CollideWithOthers = true;
			this.m_isActive = false;
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004B3B RID: 19259 RVA: 0x0019706C File Offset: 0x0019526C
	public override void Destroy()
	{
		base.Destroy();
	}

	// Token: 0x0400409C RID: 16540
	public float HealthThresholdPhaseTwo = 0.66f;

	// Token: 0x0400409D RID: 16541
	public float HealthThresholdPhaseThree = 0.33f;

	// Token: 0x0400409E RID: 16542
	private bool m_isActive;

	// Token: 0x0400409F RID: 16543
	private int m_currentPhase = 1;
}
