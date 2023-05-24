using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D82 RID: 3458
[InspectorDropdownName("Bosses/Beholster/RocketBehavior2")]
public class BeholsterRocketBehavior : BasicAttackBehavior
{
	// Token: 0x06004939 RID: 18745 RVA: 0x00186BF0 File Offset: 0x00184DF0
	public override void Start()
	{
		base.Start();
		this.m_beholster = this.m_aiActor.GetComponent<BeholsterController>();
	}

	// Token: 0x0600493A RID: 18746 RVA: 0x00186C0C File Offset: 0x00184E0C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_windupTimer, false);
		base.DecrementTimer(ref this.m_fireTimer, false);
	}

	// Token: 0x0600493B RID: 18747 RVA: 0x00186C30 File Offset: 0x00184E30
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
			this.m_beholster.StopFiringTentacles(this.Tentacles);
			return BehaviorResult.Continue;
		}
		if (this.WindUpTime > 0f)
		{
			this.m_windupTimer = this.WindUpTime;
			this.m_aiActor.ClearPath();
			if (this.TargetVFX)
			{
				this.m_spawnedTargetVfx = UnityEngine.Object.Instantiate<GameObject>(this.TargetVFX, this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox), Quaternion.identity);
				this.m_spawnedTargetVfx.transform.parent = this.m_aiActor.TargetRigidbody.transform;
				tk2dBaseSprite component = this.m_spawnedTargetVfx.GetComponent<tk2dBaseSprite>();
				tk2dBaseSprite sprite = this.m_aiActor.TargetRigidbody.sprite;
				if (component && sprite)
				{
					sprite.AttachRenderer(component);
					component.HeightOffGround = 5f;
					component.UpdateZDepth();
				}
			}
		}
		this.m_fireIndex = 0;
		this.m_fireTimer = 0f;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x0600493C RID: 18748 RVA: 0x00186D88 File Offset: 0x00184F88
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_windupTimer > 0f)
		{
			return ContinuousBehaviorResult.Continue;
		}
		if (this.m_fireTimer <= 0f)
		{
			this.m_beholster.SingleFireTentacle(this.Tentacles, new float?(this.FiringAngles[this.m_fireIndex % this.FiringAngles.Length]));
			this.m_fireTimer = this.FireCooldown;
			this.m_fireIndex++;
			if (this.m_spawnedTargetVfx)
			{
				UnityEngine.Object.Destroy(this.m_spawnedTargetVfx);
				this.m_spawnedTargetVfx = null;
			}
			int num = ((!this.m_aiActor.IsBlackPhantom) ? 1 : 2);
			if (this.m_fireIndex >= this.FiringAngles.Length * num)
			{
				this.UpdateCooldowns();
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600493D RID: 18749 RVA: 0x00186E5C File Offset: 0x0018505C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_spawnedTargetVfx)
		{
			UnityEngine.Object.Destroy(this.m_spawnedTargetVfx);
			this.m_spawnedTargetVfx = null;
		}
	}

	// Token: 0x0600493E RID: 18750 RVA: 0x00186E88 File Offset: 0x00185088
	public override void Destroy()
	{
		base.Destroy();
		if (this.m_spawnedTargetVfx)
		{
			UnityEngine.Object.Destroy(this.m_spawnedTargetVfx);
			this.m_spawnedTargetVfx = null;
		}
	}

	// Token: 0x0600493F RID: 18751 RVA: 0x00186EB4 File Offset: 0x001850B4
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

	// Token: 0x04003D6E RID: 15726
	public bool LineOfSight = true;

	// Token: 0x04003D6F RID: 15727
	public float WindUpTime = 1f;

	// Token: 0x04003D70 RID: 15728
	public GameObject TargetVFX;

	// Token: 0x04003D71 RID: 15729
	public float[] FiringAngles;

	// Token: 0x04003D72 RID: 15730
	public float FireCooldown;

	// Token: 0x04003D73 RID: 15731
	public BeholsterTentacleController[] Tentacles;

	// Token: 0x04003D74 RID: 15732
	private BeholsterController m_beholster;

	// Token: 0x04003D75 RID: 15733
	private float m_windupTimer;

	// Token: 0x04003D76 RID: 15734
	private GameObject m_spawnedTargetVfx;

	// Token: 0x04003D77 RID: 15735
	private float m_fireTimer;

	// Token: 0x04003D78 RID: 15736
	private int m_fireIndex;
}
