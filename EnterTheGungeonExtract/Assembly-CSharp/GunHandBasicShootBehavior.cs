using System;
using System.Collections.Generic;

// Token: 0x02000D2C RID: 3372
public class GunHandBasicShootBehavior : BasicAttackBehavior
{
	// Token: 0x06004733 RID: 18227 RVA: 0x00174BD0 File Offset: 0x00172DD0
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004734 RID: 18228 RVA: 0x00174BD8 File Offset: 0x00172DD8
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x00174BE0 File Offset: 0x00172DE0
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
			for (int i = 0; i < this.GunHands.Count; i++)
			{
				if (this.GunHands[i])
				{
					this.GunHands[i].CeaseAttack();
				}
			}
			return BehaviorResult.Continue;
		}
		if (this.FireAllGuns)
		{
			for (int j = 0; j < this.GunHands.Count; j++)
			{
				if (this.GunHands[j])
				{
					this.GunHands[j].StartFiring();
				}
			}
			this.UpdateCooldowns();
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		GunHandController gunHandController = BraveUtility.RandomElement<GunHandController>(this.GunHands);
		if (gunHandController)
		{
			gunHandController.StartFiring();
		}
		this.UpdateCooldowns();
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x00174D0C File Offset: 0x00172F0C
	public override bool IsReady()
	{
		if (!base.IsReady())
		{
			return false;
		}
		if (this.FireAllGuns)
		{
			for (int i = 0; i < this.GunHands.Count; i++)
			{
				if (!this.GunHands[i].IsReady)
				{
					return false;
				}
			}
			return true;
		}
		for (int j = 0; j < this.GunHands.Count; j++)
		{
			if (this.GunHands[j].IsReady)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04003A22 RID: 14882
	public bool LineOfSight = true;

	// Token: 0x04003A23 RID: 14883
	public bool FireAllGuns;

	// Token: 0x04003A24 RID: 14884
	public List<GunHandController> GunHands;
}
