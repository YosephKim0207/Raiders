using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000D52 RID: 3410
public class SpawnReinforcementsBehavior : BasicAttackBehavior
{
	// Token: 0x06004816 RID: 18454 RVA: 0x0017C720 File Offset: 0x0017A920
	private bool ShowStaggerDelay()
	{
		return this.StaggerSpawns && this.staggerMode == SpawnReinforcementsBehavior.StaggerMode.Timer;
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x0017C73C File Offset: 0x0017A93C
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x0017C744 File Offset: 0x0017A944
	public override void Upkeep()
	{
		base.Upkeep();
		if (SpawnReinforcementsBehavior.s_staticCooldown > 0f && SpawnReinforcementsBehavior.s_lastStaticUpdateFrameNum != Time.frameCount)
		{
			SpawnReinforcementsBehavior.s_staticCooldown = Mathf.Max(0f, SpawnReinforcementsBehavior.s_staticCooldown - this.m_deltaTime);
			SpawnReinforcementsBehavior.s_lastStaticUpdateFrameNum = Time.frameCount;
		}
		base.DecrementTimer(ref this.m_staggerTimer, false);
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x0017C7A8 File Offset: 0x0017A9A8
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
		this.m_reinforceIndex = ((this.indexType != SpawnReinforcementsBehavior.IndexType.Ordered) ? BraveUtility.RandomElement<int>(this.ReinforcementIndices) : this.ReinforcementIndices[this.m_timesReinforced]);
		this.m_thingsToSpawn = this.m_aiActor.ParentRoom.GetEnemiesInReinforcementLayer(this.m_reinforceIndex);
		int num = this.MaxRoomOccupancy;
		if (this.OverrideMaxOccupancyToSpawn > 0)
		{
			num = this.OverrideMaxOccupancyToSpawn;
		}
		if (num >= 0)
		{
			int count = this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count;
			if (count >= num)
			{
				this.m_timesReinforced++;
				this.UpdateCooldowns();
				return BehaviorResult.Continue;
			}
			this.m_thingsToSpawn = this.MaxRoomOccupancy - count;
		}
		this.m_timesReinforced++;
		SpawnReinforcementsBehavior.s_staticCooldown += this.StaticCooldown;
		if (!string.IsNullOrEmpty(this.DirectionalAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.DirectionalAnimation, true, null, -1f, false);
		}
		if (this.HideGun)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(false, "SpawnReinforcementBehavior");
		}
		if (this.StopDuringAnimation)
		{
			this.m_aiActor.ClearPath();
		}
		if (this.StaggerSpawns)
		{
			this.m_reinforceSubIndex = 0;
			if (this.staggerMode == SpawnReinforcementsBehavior.StaggerMode.Animation)
			{
				tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
				spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
			}
			else if (this.staggerMode == SpawnReinforcementsBehavior.StaggerMode.Timer)
			{
				this.m_staggerTimer = this.staggerDelay;
			}
		}
		else if (this.m_thingsToSpawn > 0)
		{
			RoomHandler parentRoom = this.m_aiActor.ParentRoom;
			int reinforceIndex = this.m_reinforceIndex;
			bool flag = false;
			bool disableDrops = this.DisableDrops;
			int thingsToSpawn = this.m_thingsToSpawn;
			parentRoom.TriggerReinforcementLayer(reinforceIndex, flag, disableDrops, -1, thingsToSpawn, false);
		}
		if (this.StopDuringAnimation || this.StaggerSpawns)
		{
			this.m_updateEveryFrame = true;
			this.m_state = SpawnReinforcementsBehavior.State.Spawning;
			return BehaviorResult.RunContinuous;
		}
		this.UpdateCooldowns();
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x0017C9D8 File Offset: 0x0017ABD8
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_state == SpawnReinforcementsBehavior.State.Spawning)
		{
			bool flag = false;
			if (!this.StaggerSpawns)
			{
				if (!this.m_aiAnimator.IsPlaying(this.DirectionalAnimation))
				{
					flag = true;
				}
			}
			else if (this.staggerMode == SpawnReinforcementsBehavior.StaggerMode.Timer)
			{
				if (this.m_staggerTimer <= 0f)
				{
					this.SpawnOneDude();
					this.m_staggerTimer = this.staggerDelay;
					if (this.m_thingsToSpawn <= 0)
					{
						flag = true;
					}
				}
			}
			else if (this.staggerMode == SpawnReinforcementsBehavior.StaggerMode.Animation && !this.m_aiAnimator.IsPlaying(this.DirectionalAnimation))
			{
				if (this.m_thingsToSpawn > 0)
				{
					this.m_aiActor.ParentRoom.TriggerReinforcementLayer(this.m_reinforceIndex, false, this.DisableDrops, this.m_reinforceSubIndex, this.m_thingsToSpawn, false);
				}
				flag = true;
			}
			if (flag)
			{
				if (this.DelayAfterSpawn > 0f)
				{
					this.m_timer = this.DelayAfterSpawn;
					this.m_state = SpawnReinforcementsBehavior.State.PostSpawnDelay;
					return ContinuousBehaviorResult.Continue;
				}
				return ContinuousBehaviorResult.Finished;
			}
		}
		else if (this.m_state == SpawnReinforcementsBehavior.State.PostSpawnDelay)
		{
			base.DecrementTimer(ref this.m_timer, false);
			if (this.DelayAfterSpawnMinOccupancy > 0)
			{
				int count = this.m_aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count;
				if (count < this.DelayAfterSpawnMinOccupancy)
				{
					return ContinuousBehaviorResult.Finished;
				}
			}
			if (this.m_timer <= 0f)
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x0017CB48 File Offset: 0x0017AD48
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.HideGun)
		{
			this.m_aiShooter.ToggleGunAndHandRenderers(true, "SpawnReinforcementBehavior");
		}
		if (this.StaggerSpawns && this.staggerMode == SpawnReinforcementsBehavior.StaggerMode.Animation)
		{
			tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x0017CBC8 File Offset: 0x0017ADC8
	public override bool IsReady()
	{
		return base.IsReady() && SpawnReinforcementsBehavior.s_staticCooldown <= 0f;
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x0017CBE8 File Offset: 0x0017ADE8
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNum)
	{
		if (clip.GetFrame(frameNum).eventInfo == "spawn" && this.m_thingsToSpawn > 0)
		{
			this.SpawnOneDude();
		}
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x0017CC18 File Offset: 0x0017AE18
	private void SpawnOneDude()
	{
		this.m_aiActor.ParentRoom.TriggerReinforcementLayer(this.m_reinforceIndex, false, this.DisableDrops, this.m_reinforceSubIndex, 1, false);
		this.m_reinforceSubIndex++;
		this.m_thingsToSpawn--;
	}

	// Token: 0x04003B82 RID: 15234
	public int MaxRoomOccupancy = -1;

	// Token: 0x04003B83 RID: 15235
	public int OverrideMaxOccupancyToSpawn = -1;

	// Token: 0x04003B84 RID: 15236
	public List<int> ReinforcementIndices;

	// Token: 0x04003B85 RID: 15237
	public SpawnReinforcementsBehavior.IndexType indexType;

	// Token: 0x04003B86 RID: 15238
	public bool StaggerSpawns;

	// Token: 0x04003B87 RID: 15239
	[InspectorIndent]
	[InspectorShowIf("StaggerSpawns")]
	public SpawnReinforcementsBehavior.StaggerMode staggerMode;

	// Token: 0x04003B88 RID: 15240
	[InspectorIndent]
	[InspectorShowIf("ShowStaggerDelay")]
	public float staggerDelay = 1f;

	// Token: 0x04003B89 RID: 15241
	public bool StopDuringAnimation = true;

	// Token: 0x04003B8A RID: 15242
	public bool DisableDrops = true;

	// Token: 0x04003B8B RID: 15243
	public float DelayAfterSpawn;

	// Token: 0x04003B8C RID: 15244
	public int DelayAfterSpawnMinOccupancy;

	// Token: 0x04003B8D RID: 15245
	[InspectorCategory("Visuals")]
	public string DirectionalAnimation;

	// Token: 0x04003B8E RID: 15246
	[InspectorCategory("Visuals")]
	public bool HideGun;

	// Token: 0x04003B8F RID: 15247
	[InspectorCategory("Conditions")]
	public float StaticCooldown;

	// Token: 0x04003B90 RID: 15248
	private int m_timesReinforced;

	// Token: 0x04003B91 RID: 15249
	private int m_reinforceIndex;

	// Token: 0x04003B92 RID: 15250
	private int m_reinforceSubIndex;

	// Token: 0x04003B93 RID: 15251
	private int m_thingsToSpawn;

	// Token: 0x04003B94 RID: 15252
	private float m_staggerTimer;

	// Token: 0x04003B95 RID: 15253
	private float m_timer;

	// Token: 0x04003B96 RID: 15254
	private static float s_staticCooldown;

	// Token: 0x04003B97 RID: 15255
	private static int s_lastStaticUpdateFrameNum = -1;

	// Token: 0x04003B98 RID: 15256
	private SpawnReinforcementsBehavior.State m_state;

	// Token: 0x02000D53 RID: 3411
	public enum IndexType
	{
		// Token: 0x04003B9A RID: 15258
		Random,
		// Token: 0x04003B9B RID: 15259
		Ordered
	}

	// Token: 0x02000D54 RID: 3412
	public enum StaggerMode
	{
		// Token: 0x04003B9D RID: 15261
		Animation,
		// Token: 0x04003B9E RID: 15262
		Timer
	}

	// Token: 0x02000D55 RID: 3413
	private enum State
	{
		// Token: 0x04003BA0 RID: 15264
		Idle,
		// Token: 0x04003BA1 RID: 15265
		Spawning,
		// Token: 0x04003BA2 RID: 15266
		PostSpawnDelay
	}
}
