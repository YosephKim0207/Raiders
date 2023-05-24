using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DBB RID: 3515
[InspectorDropdownName("Bosses/GiantPowderSkull/ArmosBehavior")]
public class GiantPowderSkullArmosBehavior : BasicAttackBehavior
{
	// Token: 0x06004A8E RID: 19086 RVA: 0x0019046C File Offset: 0x0018E66C
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.m_aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimEventTriggered));
	}

	// Token: 0x06004A8F RID: 19087 RVA: 0x001904A0 File Offset: 0x0018E6A0
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_timer, false);
	}

	// Token: 0x06004A90 RID: 19088 RVA: 0x001904B8 File Offset: 0x0018E6B8
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
		this.m_aiAnimator.PlayUntilFinished(this.armosAnim, false, null, -1f, false);
		this.m_timer = this.time;
		this.m_isRunning = true;
		this.m_aiActor.ClearPath();
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_currentAngle = this.startingAngle;
		this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_currentAngle, this.speed);
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A91 RID: 19089 RVA: 0x00190558 File Offset: 0x0018E758
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_timer <= 0f)
		{
			this.m_aiAnimator.EndAnimation();
			return ContinuousBehaviorResult.Finished;
		}
		this.m_currentAngle = BraveMathCollege.ClampAngle180(this.m_currentAngle + this.rotationSpeed * this.m_deltaTime);
		this.m_aiActor.BehaviorVelocity = BraveMathCollege.DegreesToVector(this.m_currentAngle, this.speed);
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A92 RID: 19090 RVA: 0x001905C8 File Offset: 0x0018E7C8
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_isRunning = false;
		this.m_updateEveryFrame = false;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A93 RID: 19091 RVA: 0x001905F0 File Offset: 0x0018E7F0
	private void ShootBulletScript()
	{
		if (!this.m_bulletScriptSource)
		{
			this.m_bulletScriptSource = this.shootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletScriptSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletScriptSource.BulletScript = this.bulletScript;
		this.m_bulletScriptSource.Initialize();
	}

	// Token: 0x06004A94 RID: 19092 RVA: 0x00190650 File Offset: 0x0018E850
	private void AnimEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		if (this.m_isRunning && clip.GetFrame(frameNum).eventInfo == "fire")
		{
			this.ShootBulletScript();
		}
	}

	// Token: 0x04003F4D RID: 16205
	public GameObject shootPoint;

	// Token: 0x04003F4E RID: 16206
	public BulletScriptSelector bulletScript;

	// Token: 0x04003F4F RID: 16207
	public float time = 8f;

	// Token: 0x04003F50 RID: 16208
	public float speed = 6f;

	// Token: 0x04003F51 RID: 16209
	public float startingAngle = -90f;

	// Token: 0x04003F52 RID: 16210
	public float rotationSpeed = -180f;

	// Token: 0x04003F53 RID: 16211
	[InspectorCategory("Visuals")]
	public string armosAnim;

	// Token: 0x04003F54 RID: 16212
	private bool m_isRunning;

	// Token: 0x04003F55 RID: 16213
	private float m_timer;

	// Token: 0x04003F56 RID: 16214
	private float m_currentAngle;

	// Token: 0x04003F57 RID: 16215
	private BulletScriptSource m_bulletScriptSource;
}
