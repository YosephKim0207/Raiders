using System;
using System.Collections.Generic;
using FullInspector;

// Token: 0x02000D92 RID: 3474
[InspectorDropdownName("Bosses/BossFinalRogue/ShootBehavior")]
public class BossFinalRogueShootBehavior : BasicAttackBehavior
{
	// Token: 0x0600498D RID: 18829 RVA: 0x00188F88 File Offset: 0x00187188
	public override void Start()
	{
		base.Start();
		this.m_ship = this.m_aiActor.GetComponent<BossFinalRogueController>();
	}

	// Token: 0x0600498E RID: 18830 RVA: 0x00188FA4 File Offset: 0x001871A4
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.CheckPlayerInArea && BasicAttackBehavior.DrawDebugFiringArea && this.m_aiActor.TargetRigidbody)
		{
			this.playerArea.DrawDebugLines(this.GetOrigin(this.playerArea.targetAreaOrigin), this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox), this.m_aiActor);
		}
		if (this.CheckPlayerInArea)
		{
			base.DecrementTimer(ref this.m_checkPlayerInAreaTimer, false);
		}
	}

	// Token: 0x0600498F RID: 18831 RVA: 0x0018902C File Offset: 0x0018722C
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
		for (int i = 0; i < this.Guns.Count; i++)
		{
			this.Guns[i].Fire();
		}
		for (int j = 0; j < this.Guns.Count; j++)
		{
			if (!this.Guns[j].IsFinished)
			{
				if (this.SuppressBaseGuns)
				{
					this.m_ship.SuppressBaseGuns = true;
				}
				if (this.CheckPlayerInArea)
				{
					this.m_checkPlayerInAreaTimer = this.playerAreaSetupTime;
				}
				this.m_updateEveryFrame = true;
				return BehaviorResult.RunContinuousInClass;
			}
		}
		this.UpdateCooldowns();
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004990 RID: 18832 RVA: 0x001890FC File Offset: 0x001872FC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.CheckPlayerInArea && this.m_checkPlayerInAreaTimer <= 0f && !this.TargetStillInFiringArea())
		{
			return ContinuousBehaviorResult.Finished;
		}
		if (this.EndIfAnyGunsFinish)
		{
			for (int i = 0; i < this.Guns.Count; i++)
			{
				if (this.Guns[i].IsFinished)
				{
					return ContinuousBehaviorResult.Finished;
				}
			}
			return ContinuousBehaviorResult.Continue;
		}
		for (int j = 0; j < this.Guns.Count; j++)
		{
			if (!this.Guns[j].IsFinished)
			{
				return ContinuousBehaviorResult.Continue;
			}
		}
		return ContinuousBehaviorResult.Finished;
	}

	// Token: 0x06004991 RID: 18833 RVA: 0x001891A8 File Offset: 0x001873A8
	public override void EndContinuousUpdate()
	{
		if (this.SuppressBaseGuns)
		{
			this.m_ship.SuppressBaseGuns = false;
		}
		for (int i = 0; i < this.Guns.Count; i++)
		{
			this.Guns[i].CeaseFire();
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004992 RID: 18834 RVA: 0x00189208 File Offset: 0x00187408
	protected bool TargetStillInFiringArea()
	{
		return this.playerArea == null || (this.m_aiActor.TargetRigidbody && this.playerArea.TargetInFiringArea(this.GetOrigin(this.playerArea.targetAreaOrigin), this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox)));
	}

	// Token: 0x04003DF1 RID: 15857
	public bool SuppressBaseGuns;

	// Token: 0x04003DF2 RID: 15858
	public List<BossFinalRogueGunController> Guns;

	// Token: 0x04003DF3 RID: 15859
	public bool CheckPlayerInArea;

	// Token: 0x04003DF4 RID: 15860
	[InspectorShowIf("CheckPlayerInArea")]
	public ShootBehavior.FiringAreaStyle playerArea;

	// Token: 0x04003DF5 RID: 15861
	[InspectorShowIf("CheckPlayerInArea")]
	public float playerAreaSetupTime;

	// Token: 0x04003DF6 RID: 15862
	public bool EndIfAnyGunsFinish;

	// Token: 0x04003DF7 RID: 15863
	private BossFinalRogueController m_ship;

	// Token: 0x04003DF8 RID: 15864
	private float m_checkPlayerInAreaTimer;
}
