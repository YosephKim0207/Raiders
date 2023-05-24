using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D8A RID: 3466
[InspectorDropdownName("Bosses/BossFinalConvict/ShootBehavior")]
public class BossFinalConvictShootBehavior : BasicAttackBehavior
{
	// Token: 0x06004961 RID: 18785 RVA: 0x00187F08 File Offset: 0x00186108
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x00187F10 File Offset: 0x00186110
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004963 RID: 18787 RVA: 0x00187F18 File Offset: 0x00186118
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
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.m_aiActor.ClearPath();
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = Vector2.zero;
		this.m_verticalVelocity = 0f;
		if (!string.IsNullOrEmpty(this.anim))
		{
			this.m_aiAnimator.PlayUntilCancelled(this.anim, false, null, -1f, false);
		}
		this.Fire();
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004964 RID: 18788 RVA: 0x00187FC4 File Offset: 0x001861C4
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (!this.m_aiActor.TargetRigidbody || this.m_bulletSource.IsEnded)
		{
			return ContinuousBehaviorResult.Finished;
		}
		if (this.IsTargetUnreachable(3.4028235E+38f))
		{
			return ContinuousBehaviorResult.Finished;
		}
		Vector2 vector = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		float num = ((Mathf.Abs(vector.y) <= 5f) ? this.maxMoveSpeed : (1.5f * this.maxMoveSpeed));
		this.m_verticalVelocity = Mathf.Clamp(this.m_verticalVelocity + Mathf.Sign(vector.y) * this.moveAcceleration * this.m_deltaTime, -num, num);
		this.m_aiActor.BehaviorVelocity = new Vector2(0f, this.m_verticalVelocity);
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004965 RID: 18789 RVA: 0x001880B8 File Offset: 0x001862B8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiActor.BehaviorOverridesVelocity = false;
		if (!string.IsNullOrEmpty(this.anim))
		{
			this.m_aiAnimator.EndAnimationIf(this.anim);
		}
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004966 RID: 18790 RVA: 0x00188124 File Offset: 0x00186324
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004967 RID: 18791 RVA: 0x00188128 File Offset: 0x00186328
	public override bool IsReady()
	{
		return base.IsReady() && !this.IsTargetUnreachable(float.MaxValue);
	}

	// Token: 0x06004968 RID: 18792 RVA: 0x0018814C File Offset: 0x0018634C
	private void Fire()
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.shootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.bulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x06004969 RID: 18793 RVA: 0x001881AC File Offset: 0x001863AC
	private bool IsTargetUnreachable(float maxDist = 3.4028235E+38f)
	{
		if (!this.m_aiActor.TargetRigidbody)
		{
			return true;
		}
		Vector2 vector = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		int num = CollisionMask.LayerToMask(CollisionLayer.LowObstacle, CollisionLayer.HighObstacle);
		float num2 = Mathf.Min(vector.y, maxDist);
		CollisionData collisionData;
		bool flag = PhysicsEngine.Instance.RigidbodyCastWithIgnores(this.m_aiActor.specRigidbody, PhysicsEngine.UnitToPixel(new Vector2(0f, num2)), out collisionData, true, true, new int?(num), false, new SpeculativeRigidbody[] { this.m_aiActor.specRigidbody });
		CollisionData.Pool.Free(ref collisionData);
		return flag;
	}

	// Token: 0x04003DB5 RID: 15797
	public GameObject shootPoint;

	// Token: 0x04003DB6 RID: 15798
	public BulletScriptSelector bulletScript;

	// Token: 0x04003DB7 RID: 15799
	public float maxMoveSpeed = 5f;

	// Token: 0x04003DB8 RID: 15800
	public float moveAcceleration = 10f;

	// Token: 0x04003DB9 RID: 15801
	[InspectorCategory("Visuals")]
	public string anim;

	// Token: 0x04003DBA RID: 15802
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003DBB RID: 15803
	private float m_verticalVelocity;
}
