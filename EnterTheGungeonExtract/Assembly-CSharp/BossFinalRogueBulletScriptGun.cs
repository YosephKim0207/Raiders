using System;

// Token: 0x02000FE2 RID: 4066
public class BossFinalRogueBulletScriptGun : BossFinalRogueGunController
{
	// Token: 0x060058AA RID: 22698 RVA: 0x0021E28C File Offset: 0x0021C48C
	public override void Start()
	{
		base.Start();
		this.ShootBehavior.Init(this.ship.gameObject, this.ship.aiActor, this.ship.aiShooter);
		this.ShootBehavior.Start();
	}

	// Token: 0x060058AB RID: 22699 RVA: 0x0021E2CC File Offset: 0x0021C4CC
	public override void Update()
	{
		base.Update();
		this.ShootBehavior.SetDeltaTime(BraveTime.DeltaTime);
		this.ShootBehavior.Upkeep();
		if (this.m_isRunning)
		{
			ContinuousBehaviorResult continuousBehaviorResult = this.ShootBehavior.ContinuousUpdate();
			if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
			{
				this.m_isRunning = false;
				this.ShootBehavior.EndContinuousUpdate();
			}
		}
	}

	// Token: 0x060058AC RID: 22700 RVA: 0x0021E32C File Offset: 0x0021C52C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060058AD RID: 22701 RVA: 0x0021E334 File Offset: 0x0021C534
	public override void Fire()
	{
		BehaviorResult behaviorResult = this.ShootBehavior.Update();
		if (behaviorResult == BehaviorResult.RunContinuous || behaviorResult == BehaviorResult.RunContinuousInClass)
		{
			this.m_isRunning = true;
		}
	}

	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x060058AE RID: 22702 RVA: 0x0021E364 File Offset: 0x0021C564
	public override bool IsFinished
	{
		get
		{
			return !this.m_isRunning;
		}
	}

	// Token: 0x060058AF RID: 22703 RVA: 0x0021E370 File Offset: 0x0021C570
	public override void CeaseFire()
	{
		if (this.m_isRunning)
		{
			this.ShootBehavior.EndContinuousUpdate();
		}
	}

	// Token: 0x040051D2 RID: 20946
	public ShootBehavior ShootBehavior;

	// Token: 0x040051D3 RID: 20947
	private bool m_isRunning;
}
