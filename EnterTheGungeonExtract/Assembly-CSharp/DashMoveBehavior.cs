using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000DE9 RID: 3561
public class DashMoveBehavior : MovementBehaviorBase
{
	// Token: 0x06004B68 RID: 19304 RVA: 0x00199420 File Offset: 0x00197620
	public override void Start()
	{
		base.Start();
		this.m_cooldownTimer = this.InitialCooldown;
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x00199434 File Offset: 0x00197634
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_cooldownTimer, true);
		base.DecrementTimer(ref this.m_dashTimer, false);
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x00199458 File Offset: 0x00197658
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
		float num = this.dashDistance / this.dashTime;
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = num * this.GetDashDirection();
		this.m_updateEveryFrame = true;
		this.m_dashTimer = this.dashTime;
		if (this.enableShadowTrail)
		{
			this.m_aiActor.GetComponent<AfterImageTrailController>().spawnShadows = true;
		}
		AkSoundEngine.PostEvent("Play_ENM_highpriest_dash_01", GameManager.Instance.PrimaryPlayer.gameObject);
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004B6B RID: 19307 RVA: 0x00199514 File Offset: 0x00197714
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.enableShadowTrail && this.m_dashTimer <= 0.1f)
		{
			this.m_aiActor.GetComponent<AfterImageTrailController>().spawnShadows = false;
		}
		return (this.m_dashTimer > 0f) ? ContinuousBehaviorResult.Continue : ContinuousBehaviorResult.Finished;
	}

	// Token: 0x06004B6C RID: 19308 RVA: 0x0019956C File Offset: 0x0019776C
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_updateEveryFrame = false;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		if (this.enableShadowTrail)
		{
			this.m_aiActor.GetComponent<AfterImageTrailController>().spawnShadows = false;
		}
		this.RefreshCooldowns();
	}

	// Token: 0x06004B6D RID: 19309 RVA: 0x001995AC File Offset: 0x001977AC
	public bool IsReady()
	{
		return this.m_cooldownTimer == 0f;
	}

	// Token: 0x06004B6E RID: 19310 RVA: 0x001995BC File Offset: 0x001977BC
	public void RefreshCooldowns()
	{
		if (this.HealthModifiesCooldown)
		{
			this.m_cooldownTimer = Mathf.Lerp(this.NoHealthCooldown, this.FullHealthCooldown, this.m_aiActor.healthHaver.GetCurrentHealthPercentage());
		}
		else
		{
			this.m_cooldownTimer = this.Cooldown;
		}
		if (this.GlobalCooldown > 0f)
		{
			this.m_aiActor.behaviorSpeculator.GlobalCooldown = this.GlobalCooldown;
		}
	}

	// Token: 0x06004B6F RID: 19311 RVA: 0x00199634 File Offset: 0x00197834
	private Vector2 GetDashDirection()
	{
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 normalized = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.Ground) - unitCenter).normalized;
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < 2; i++)
		{
			bool flag = false;
			bool flag2 = false;
			Vector2 vector = normalized.Rotate((float)((i != 0) ? (-90) : 90));
			RaycastResult raycastResult;
			bool flag3 = PhysicsEngine.Instance.Raycast(unitCenter, vector, 3f, out raycastResult, true, true, int.MaxValue, new CollisionLayer?(CollisionLayer.EnemyCollider), false, null, this.m_aiActor.specRigidbody);
			RaycastResult.Pool.Free(ref raycastResult);
			float num = 0.25f;
			while (num <= this.dashDistance && !flag && !flag3)
			{
				Vector2 vector2 = unitCenter + num * vector;
				if (!GameManager.Instance.Dungeon.CellExists(vector2))
				{
					flag = true;
				}
				else if (GameManager.Instance.Dungeon.ShouldReallyFall(vector2))
				{
					flag = true;
				}
				num += 0.25f;
			}
			num = 0.25f;
			while (num <= this.dashDistance && !flag && !flag2 && !flag3)
			{
				IntVector2 intVector = (unitCenter + num * vector).ToIntVector2(VectorConversions.Floor);
				if (!GameManager.Instance.Dungeon.CellExists(intVector))
				{
					flag2 = true;
				}
				else if (GameManager.Instance.Dungeon.data[intVector].isExitCell)
				{
					flag2 = true;
				}
				num += 0.25f;
			}
			if (!flag3 && !flag && !flag2)
			{
				list.Add(vector);
			}
		}
		if (list.Count > 0)
		{
			return BraveUtility.RandomElement<Vector2>(list).normalized;
		}
		return normalized.Rotate(BraveUtility.RandomSign() * 90f).normalized;
	}

	// Token: 0x040040FE RID: 16638
	[InspectorCategory("Conditions")]
	public float Cooldown = 1f;

	// Token: 0x040040FF RID: 16639
	[InspectorCategory("Conditions")]
	public bool HealthModifiesCooldown;

	// Token: 0x04004100 RID: 16640
	[InspectorCategory("Conditions")]
	[InspectorShowIf("HealthModifiesCooldown")]
	public float NoHealthCooldown = 1f;

	// Token: 0x04004101 RID: 16641
	[InspectorShowIf("HealthModifiesCooldown")]
	[InspectorCategory("Conditions")]
	public float FullHealthCooldown = 1f;

	// Token: 0x04004102 RID: 16642
	[InspectorCategory("Conditions")]
	public float InitialCooldown;

	// Token: 0x04004103 RID: 16643
	[InspectorCategory("Conditions")]
	public float GlobalCooldown;

	// Token: 0x04004104 RID: 16644
	public float dashDistance;

	// Token: 0x04004105 RID: 16645
	public float dashTime;

	// Token: 0x04004106 RID: 16646
	[InspectorCategory("Visuals")]
	public bool enableShadowTrail;

	// Token: 0x04004107 RID: 16647
	protected float m_cooldownTimer;

	// Token: 0x04004108 RID: 16648
	protected float m_dashTimer;
}
