using System;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB4 RID: 3508
[InspectorDropdownName("Bosses/GatlingGull/LastStandBehavior")]
public class GatlingGullLastStandBehavior : BasicAttackBehavior
{
	// Token: 0x06004A4E RID: 19022 RVA: 0x0018E180 File Offset: 0x0018C380
	public override void Start()
	{
		base.Start();
		AttackBehaviorGroup attackBehaviorGroup = (AttackBehaviorGroup)this.m_aiActor.behaviorSpeculator.AttackBehaviors.Find((AttackBehaviorBase b) => b is AttackBehaviorGroup);
		this.m_leapBehavior = (GatlingGullLeapBehavior)attackBehaviorGroup.AttackBehaviors.Find((AttackBehaviorGroup.AttackGroupItem b) => b.Behavior is GatlingGullLeapBehavior).Behavior;
		this.m_room = GameManager.Instance.Dungeon.GetRoomFromPosition(this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		this.m_aiActor.healthHaver.minimumHealth = 1f;
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x0018E244 File Offset: 0x0018C444
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_passthrough)
		{
			this.m_leapBehavior.Upkeep();
		}
	}

	// Token: 0x06004A50 RID: 19024 RVA: 0x0018E264 File Offset: 0x0018C464
	public override bool OverrideOtherBehaviors()
	{
		return this.m_aiActor.healthHaver.GetCurrentHealth() <= this.HealthThreshold;
	}

	// Token: 0x06004A51 RID: 19025 RVA: 0x0018E284 File Offset: 0x0018C484
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_aiActor.healthHaver.GetCurrentHealth() <= this.HealthThreshold)
		{
			this.m_leapBehavior.OverridePosition = new Vector2?(this.m_room.area.basePosition.ToVector2() + this.m_leapPosition);
			BehaviorResult behaviorResult = this.m_leapBehavior.Update();
			if (behaviorResult == BehaviorResult.RunContinuous)
			{
				this.m_passthrough = true;
			}
			else
			{
				this.m_leapBehavior.OverridePosition = null;
			}
			return behaviorResult;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x0018E31C File Offset: 0x0018C51C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_passthrough)
		{
			ContinuousBehaviorResult continuousBehaviorResult = this.m_leapBehavior.ContinuousUpdate();
			if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
			{
				this.m_passthrough = false;
				this.m_leapBehavior.OverridePosition = null;
				this.UpdateCooldowns();
				this.m_aiActor.healthHaver.minimumHealth = 0f;
				this.m_isInDeathPosition = true;
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					GameManager.Instance.AllPlayers[i].BossKillingMode = true;
				}
			}
		}
		else if (this.m_isInDeathPosition)
		{
			if (!this.m_aiActor.TargetRigidbody)
			{
				this.m_aiShooter.ManualGunAngle = false;
			}
			else
			{
				Vector2 vector = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.CenterPosition;
				int num = BraveMathCollege.VectorToOctant(vector);
				this.m_aiShooter.ManualGunAngle = true;
				this.m_aiShooter.GunAngle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				Vector2 vector2 = Quaternion.Euler(0f, 0f, (float)(num * -45)) * Vector2.up;
				this.m_aiShooter.volley.projectiles[0].angleVariance = this.AngleVariance;
				this.m_aiShooter.ShootInDirection(vector2, this.OverrideBulletName);
				this.m_cachedAnimationName = this.m_animNames[num];
				this.m_aiAnimator.PlayUntilCancelled(this.m_cachedAnimationName, true, null, -1f, false);
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A53 RID: 19027 RVA: 0x0018E4D4 File Offset: 0x0018C6D4
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		if (this.m_passthrough)
		{
			this.m_leapBehavior.SetDeltaTime(deltaTime);
		}
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x0018E4F4 File Offset: 0x0018C6F4
	public override bool IsReady()
	{
		if (this.m_passthrough)
		{
			return this.m_leapBehavior.IsReady();
		}
		return base.IsReady();
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x0018E514 File Offset: 0x0018C714
	public override bool UpdateEveryFrame()
	{
		if (this.m_passthrough)
		{
			return this.m_leapBehavior.UpdateEveryFrame();
		}
		return base.UpdateEveryFrame();
	}

	// Token: 0x06004A56 RID: 19030 RVA: 0x0018E534 File Offset: 0x0018C734
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x04003F01 RID: 16129
	public float HealthThreshold = 5f;

	// Token: 0x04003F02 RID: 16130
	public float AngleVariance = 20f;

	// Token: 0x04003F03 RID: 16131
	public string OverrideBulletName;

	// Token: 0x04003F04 RID: 16132
	private bool m_passthrough;

	// Token: 0x04003F05 RID: 16133
	private GatlingGullLeapBehavior m_leapBehavior;

	// Token: 0x04003F06 RID: 16134
	private RoomHandler m_room;

	// Token: 0x04003F07 RID: 16135
	private readonly Vector2 m_leapPosition = new Vector2(19f, 13f);

	// Token: 0x04003F08 RID: 16136
	private bool m_isInDeathPosition;

	// Token: 0x04003F09 RID: 16137
	private readonly string[] m_animNames = new string[] { "fire_up", "fire_north_east", "fire_right", "fire_south_east", "fire_down", "fire_south_west", "fire_left", "fire_north_west" };

	// Token: 0x04003F0A RID: 16138
	private string m_cachedAnimationName;
}
