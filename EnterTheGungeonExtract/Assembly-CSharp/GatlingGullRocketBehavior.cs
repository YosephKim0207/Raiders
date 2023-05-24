using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000DB9 RID: 3513
[InspectorDropdownName("Bosses/GatlingGull/RocketBehavior")]
public class GatlingGullRocketBehavior : BasicAttackBehavior
{
	// Token: 0x06004A79 RID: 19065 RVA: 0x0018FC68 File Offset: 0x0018DE68
	public override void Start()
	{
		base.Start();
		AttackBehaviorGroup attackBehaviorGroup = (AttackBehaviorGroup)this.m_aiActor.behaviorSpeculator.AttackBehaviors.Find((AttackBehaviorBase b) => b is AttackBehaviorGroup);
		this.m_leapBehavior = (GatlingGullLeapBehavior)attackBehaviorGroup.AttackBehaviors.Find((AttackBehaviorGroup.AttackGroupItem b) => b.Behavior is GatlingGullLeapBehavior).Behavior;
		this.m_room = GameManager.Instance.Dungeon.GetRoomFromPosition(this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		List<GatlingGullLeapPoint> componentsInRoom = this.m_room.GetComponentsInRoom<GatlingGullLeapPoint>();
		for (int i = 0; i < componentsInRoom.Count; i++)
		{
			if (componentsInRoom[i].ForRockets)
			{
				this.m_leapPositions.Add(componentsInRoom[i].PlacedPosition - this.m_room.area.basePosition);
			}
		}
	}

	// Token: 0x06004A7A RID: 19066 RVA: 0x0018FD78 File Offset: 0x0018DF78
	public override void Upkeep()
	{
		base.Upkeep();
		if (this.m_passthrough)
		{
			this.m_leapBehavior.Upkeep();
		}
	}

	// Token: 0x06004A7B RID: 19067 RVA: 0x0018FD98 File Offset: 0x0018DF98
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.m_leapPositions.Count == 0)
		{
			return BehaviorResult.Continue;
		}
		int num = UnityEngine.Random.Range(0, this.m_leapPositions.Count);
		this.m_leapBehavior.OverridePosition = new Vector2?((this.m_room.area.basePosition + this.m_leapPositions[num]).ToVector2() + new Vector2(1f, 0.5f));
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

	// Token: 0x06004A7C RID: 19068 RVA: 0x0018FE54 File Offset: 0x0018E054
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_passthrough)
		{
			ContinuousBehaviorResult continuousBehaviorResult = this.m_leapBehavior.ContinuousUpdate();
			if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
			{
				this.m_leapBehavior.EndContinuousUpdate();
				this.m_aiAnimator.SuppressHitStates = true;
				this.m_aiAnimator.PlayUntilFinished("rocket", true, null, -1f, false);
				tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
				spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
				this.m_rocketCount = 0;
				this.m_fireTimer = this.PerRocketCooldown;
				this.m_firedThisCycle = false;
				this.m_healthToHalt = this.m_aiActor.healthHaver.GetCurrentHealth() - this.DamageToHalt;
				this.m_aiActor.ClearPath();
				this.m_passthrough = false;
				this.m_leapBehavior.OverridePosition = null;
			}
			return ContinuousBehaviorResult.Continue;
		}
		this.m_fireTimer -= this.m_deltaTime;
		if (this.m_fireTimer <= 0f)
		{
			if (!this.m_firedThisCycle)
			{
				this.FireRocket();
			}
			this.m_firedThisCycle = false;
			this.m_fireTimer += this.PerRocketCooldown;
			this.m_aiAnimator.PlayUntilFinished("rocket", true, null, -1f, false);
		}
		if (this.m_aiActor.healthHaver.GetCurrentHealth() <= this.m_healthToHalt || this.m_rocketCount >= this.MaxRockets)
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x0018FFD8 File Offset: 0x0018E1D8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_aiAnimator.SuppressHitStates = false;
		this.m_aiAnimator.EndAnimationIf("rocket");
		tk2dSpriteAnimator spriteAnimator = this.m_aiActor.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		this.UpdateCooldowns();
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x0019003C File Offset: 0x0018E23C
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		if (this.m_passthrough)
		{
			this.m_leapBehavior.SetDeltaTime(deltaTime);
		}
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x0019005C File Offset: 0x0018E25C
	public override bool IsReady()
	{
		if (this.m_passthrough)
		{
			return this.m_leapBehavior.IsReady();
		}
		return base.IsReady();
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x0019007C File Offset: 0x0018E27C
	public override bool UpdateEveryFrame()
	{
		if (this.m_passthrough)
		{
			return this.m_leapBehavior.UpdateEveryFrame();
		}
		return base.UpdateEveryFrame();
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x0019009C File Offset: 0x0018E29C
	public override bool IsOverridable()
	{
		return !this.m_passthrough || this.m_leapBehavior.IsOverridable();
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x001900BC File Offset: 0x0018E2BC
	private void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (frame.eventInfo == "fire_rocket")
		{
			this.FireRocket();
		}
	}

	// Token: 0x06004A83 RID: 19075 RVA: 0x001900EC File Offset: 0x0018E2EC
	private void FireRocket()
	{
		SkyRocket component = SpawnManager.SpawnProjectile(this.Rocket, this.RocketOrigin.position, Quaternion.identity, true).GetComponent<SkyRocket>();
		component.Target = this.m_aiActor.TargetRigidbody;
		tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
		component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
		component.ExplosionData.ignoreList.Add(this.m_aiActor.specRigidbody);
		this.m_aiActor.sprite.AttachRenderer(component.GetComponentInChildren<tk2dSprite>());
		this.m_firedThisCycle = true;
		this.m_rocketCount++;
	}

	// Token: 0x04003F39 RID: 16185
	public Transform RocketOrigin;

	// Token: 0x04003F3A RID: 16186
	public GameObject Rocket;

	// Token: 0x04003F3B RID: 16187
	public int MaxRockets = 5;

	// Token: 0x04003F3C RID: 16188
	public float DamageToHalt = 20f;

	// Token: 0x04003F3D RID: 16189
	public float PerRocketCooldown = 1f;

	// Token: 0x04003F3E RID: 16190
	private bool m_passthrough;

	// Token: 0x04003F3F RID: 16191
	private GatlingGullLeapBehavior m_leapBehavior;

	// Token: 0x04003F40 RID: 16192
	private float m_fireTimer;

	// Token: 0x04003F41 RID: 16193
	private float m_healthToHalt;

	// Token: 0x04003F42 RID: 16194
	private bool m_firedThisCycle;

	// Token: 0x04003F43 RID: 16195
	private int m_rocketCount;

	// Token: 0x04003F44 RID: 16196
	private RoomHandler m_room;

	// Token: 0x04003F45 RID: 16197
	private List<IntVector2> m_leapPositions = new List<IntVector2>();
}
