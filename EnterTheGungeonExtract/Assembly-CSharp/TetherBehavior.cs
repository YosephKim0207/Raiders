using System;
using UnityEngine;

// Token: 0x02000DF8 RID: 3576
public class TetherBehavior : MovementBehaviorBase
{
	// Token: 0x06004BBE RID: 19390 RVA: 0x0019D564 File Offset: 0x0019B764
	public override void Start()
	{
		base.Start();
		this.m_tetherPosition = this.m_aiActor.specRigidbody.UnitCenter;
	}

	// Token: 0x06004BBF RID: 19391 RVA: 0x0019D584 File Offset: 0x0019B784
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004BC0 RID: 19392 RVA: 0x0019D59C File Offset: 0x0019B79C
	public override BehaviorResult Update()
	{
		if (this.m_state == TetherBehavior.State.Idle)
		{
			if (Vector2.Distance(this.m_tetherPosition, this.m_aiActor.specRigidbody.UnitCenter) > 0.1f)
			{
				this.m_state = TetherBehavior.State.ReturningToSpawn;
				this.m_aiActor.PathfindToPosition(this.m_tetherPosition, new Vector2?(this.m_tetherPosition), true, null, null, null, false);
				this.m_repathTimer = this.PathInterval;
				this.m_preventKnockbackTimer = this.KnockbackInvulnerabilityDelay;
			}
		}
		else if (this.m_state == TetherBehavior.State.ReturningToSpawn)
		{
			if (this.m_preventKnockbackTimer > 0f)
			{
				this.m_preventKnockbackTimer -= this.m_deltaTime;
				if (this.m_preventKnockbackTimer <= 0f)
				{
					this.m_aiActor.knockbackDoer.SetImmobile(true, "TetherBehavior");
				}
			}
			if (this.m_aiActor.PathComplete)
			{
				this.m_state = TetherBehavior.State.Idle;
				this.m_aiActor.knockbackDoer.SetImmobile(false, "TetherBehavior");
			}
			else if (this.m_repathTimer <= 0f)
			{
				this.m_aiActor.PathfindToPosition(this.m_tetherPosition, new Vector2?(this.m_tetherPosition), true, null, null, null, false);
				this.m_repathTimer = this.PathInterval;
			}
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x06004BC1 RID: 19393 RVA: 0x0019D6F8 File Offset: 0x0019B8F8
	public override float DesiredCombatDistance
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0400419D RID: 16797
	public float KnockbackInvulnerabilityDelay = 0.5f;

	// Token: 0x0400419E RID: 16798
	public float PathInterval = 0.25f;

	// Token: 0x0400419F RID: 16799
	private float m_repathTimer;

	// Token: 0x040041A0 RID: 16800
	private float m_preventKnockbackTimer;

	// Token: 0x040041A1 RID: 16801
	private Vector2 m_tetherPosition;

	// Token: 0x040041A2 RID: 16802
	private TetherBehavior.State m_state;

	// Token: 0x02000DF9 RID: 3577
	private enum State
	{
		// Token: 0x040041A4 RID: 16804
		Idle,
		// Token: 0x040041A5 RID: 16805
		PathingToTarget,
		// Token: 0x040041A6 RID: 16806
		ReturningToSpawn
	}
}
