using System;
using FullInspector;

// Token: 0x02000D85 RID: 3461
[InspectorDropdownName("Bosses/Beholster/ShootGunBehavior")]
public class BeholsterShootGunBehavior : BasicAttackBehavior
{
	// Token: 0x06004946 RID: 18758 RVA: 0x001870D8 File Offset: 0x001852D8
	public override void Start()
	{
		base.Start();
		this.m_beholster = this.m_aiActor.GetComponent<BeholsterController>();
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x001870F4 File Offset: 0x001852F4
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004948 RID: 18760 RVA: 0x001870FC File Offset: 0x001852FC
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
		bool flag = this.LineOfSight && !this.m_aiActor.HasLineOfSightToTarget;
		if (this.m_aiActor.TargetRigidbody == null || flag)
		{
			this.m_beholster.StopFiringTentacles(this.Tentacles);
			return BehaviorResult.Continue;
		}
		this.m_beholster.StartFiringTentacles(this.Tentacles);
		this.UpdateCooldowns();
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004949 RID: 18761 RVA: 0x00187190 File Offset: 0x00185390
	public override bool IsReady()
	{
		if (!base.IsReady())
		{
			return false;
		}
		for (int i = 0; i < this.Tentacles.Length; i++)
		{
			if (this.Tentacles[i].IsReady)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04003D83 RID: 15747
	public bool LineOfSight = true;

	// Token: 0x04003D84 RID: 15748
	public BeholsterTentacleController[] Tentacles;

	// Token: 0x04003D85 RID: 15749
	private BeholsterController m_beholster;
}
