using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB8 RID: 3512
[InspectorDropdownName("Bosses/GatlingGull/RepositionBehavior")]
public class GatlingGullRepositionBehavior : BasicAttackBehavior
{
	// Token: 0x06004A6D RID: 19053 RVA: 0x0018F7B4 File Offset: 0x0018D9B4
	public override void Start()
	{
		base.Start();
		AttackBehaviorGroup attackBehaviorGroup = (AttackBehaviorGroup)this.m_aiActor.behaviorSpeculator.AttackBehaviors.Find((AttackBehaviorBase b) => b is AttackBehaviorGroup);
		this.m_leapBehavior = (GatlingGullLeapBehavior)attackBehaviorGroup.AttackBehaviors.Find((AttackBehaviorGroup.AttackGroupItem b) => b.Behavior is GatlingGullLeapBehavior).Behavior;
		this.m_room = GameManager.Instance.Dungeon.GetRoomFromPosition(this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		List<GatlingGullLeapPoint> componentsInRoom = this.m_room.GetComponentsInRoom<GatlingGullLeapPoint>();
		for (int i = 0; i < componentsInRoom.Count; i++)
		{
			if (componentsInRoom[i].ForReposition)
			{
				this.m_leapPositions.Add(componentsInRoom[i].PlacedPosition - this.m_room.area.basePosition);
			}
		}
	}

	// Token: 0x06004A6E RID: 19054 RVA: 0x0018F8C4 File Offset: 0x0018DAC4
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_passthrough)
		{
			this.m_leapBehavior.Upkeep();
		}
		else
		{
			Vector2 vector = BraveUtility.WorldPointToViewport(this.m_aiActor.specRigidbody.UnitCenter, ViewportType.Gameplay);
			bool flag = vector.x >= 0f && vector.x <= 1f && vector.y >= -0.15f && vector.y <= 1f;
			if (!flag || !this.m_aiActor.HasLineOfSightToTarget)
			{
				this.m_lostSightTimer += this.m_deltaTime;
			}
			else
			{
				this.m_lostSightTimer = 0f;
			}
		}
	}

	// Token: 0x06004A6F RID: 19055 RVA: 0x0018F990 File Offset: 0x0018DB90
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_leapPositions.Count == 0)
		{
			return BehaviorResult.Continue;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_lostSightTimer >= this.LostSightTime)
		{
			Vector2 vector = Vector2.zero;
			float num = float.MaxValue;
			for (int i = 0; i < this.m_leapPositions.Count; i++)
			{
				Vector2 vector2 = (this.m_room.area.basePosition + this.m_leapPositions[i]).ToVector2() + new Vector2(1f, 0.5f);
				float num2 = Vector2.Distance(vector2, this.m_aiActor.TargetRigidbody.UnitCenter);
				if (num2 < num && num2 > this.MinDistanceToPlayer && this.m_aiActor.HasLineOfSightToTargetFromPosition(vector2))
				{
					vector = vector2;
					num = num2;
				}
			}
			if (vector != Vector2.zero)
			{
				this.m_leapBehavior.OverridePosition = new Vector2?(vector);
				this.m_leapBehavior.SpeedMultiplier = this.LeapSpeedMultiplier;
				BehaviorResult behaviorResult = this.m_leapBehavior.Update();
				if (behaviorResult == BehaviorResult.RunContinuous)
				{
					this.m_passthrough = true;
				}
				else
				{
					this.m_leapBehavior.OverridePosition = null;
					this.m_leapBehavior.SpeedMultiplier = 1f;
				}
				return behaviorResult;
			}
			Debug.Log("no jumps found!?");
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x06004A70 RID: 19056 RVA: 0x0018FB1C File Offset: 0x0018DD1C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_passthrough)
		{
			return this.m_leapBehavior.ContinuousUpdate();
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A71 RID: 19057 RVA: 0x0018FB40 File Offset: 0x0018DD40
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_passthrough)
		{
			this.m_leapBehavior.EndContinuousUpdate();
		}
		this.m_passthrough = false;
		this.m_leapBehavior.OverridePosition = null;
		this.m_leapBehavior.SpeedMultiplier = 1f;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A72 RID: 19058 RVA: 0x0018FB9C File Offset: 0x0018DD9C
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		if (this.m_passthrough)
		{
			this.m_leapBehavior.SetDeltaTime(deltaTime);
		}
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x0018FBBC File Offset: 0x0018DDBC
	public override bool IsReady()
	{
		if (this.m_passthrough)
		{
			return this.m_leapBehavior.IsReady();
		}
		return base.IsReady();
	}

	// Token: 0x06004A74 RID: 19060 RVA: 0x0018FBDC File Offset: 0x0018DDDC
	public override bool UpdateEveryFrame()
	{
		if (this.m_passthrough)
		{
			return this.m_leapBehavior.UpdateEveryFrame();
		}
		return base.UpdateEveryFrame();
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x0018FBFC File Offset: 0x0018DDFC
	public override bool IsOverridable()
	{
		return !this.m_passthrough || this.m_leapBehavior.IsOverridable();
	}

	// Token: 0x04003F2F RID: 16175
	public float LostSightTime = 5f;

	// Token: 0x04003F30 RID: 16176
	public float MinDistanceToPlayer = 4f;

	// Token: 0x04003F31 RID: 16177
	public float LeapSpeedMultiplier = 1f;

	// Token: 0x04003F32 RID: 16178
	private bool m_passthrough;

	// Token: 0x04003F33 RID: 16179
	private GatlingGullLeapBehavior m_leapBehavior;

	// Token: 0x04003F34 RID: 16180
	private float m_lostSightTimer;

	// Token: 0x04003F35 RID: 16181
	private RoomHandler m_room;

	// Token: 0x04003F36 RID: 16182
	private List<IntVector2> m_leapPositions = new List<IntVector2>();
}
