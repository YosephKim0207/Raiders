using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DA3 RID: 3491
[InspectorDropdownName("Bosses/DraGun/GlockBehavior")]
public class DraGunGlockBehavior : BasicAttackBehavior
{
	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x060049EE RID: 18926 RVA: 0x0018B9A0 File Offset: 0x00189BA0
	// (set) Token: 0x060049EF RID: 18927 RVA: 0x0018B9A8 File Offset: 0x00189BA8
	private DraGunGlockBehavior.HandState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x060049F0 RID: 18928 RVA: 0x0018B9D8 File Offset: 0x00189BD8
	public override void Start()
	{
		base.Start();
		tk2dSpriteAnimator spriteAnimator = this.aiAnimator.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		this.m_facingLeft = this.aiAnimator.name.Contains("left", true);
		this.m_unityAnimPrefix = ((!this.m_facingLeft) ? "DraGunRight" : "DraGunLeft");
	}

	// Token: 0x060049F1 RID: 18929 RVA: 0x0018BA54 File Offset: 0x00189C54
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
		this.m_attackIndex = -1;
		this.State = DraGunGlockBehavior.HandState.Intro;
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x060049F2 RID: 18930 RVA: 0x0018BA94 File Offset: 0x00189C94
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == DraGunGlockBehavior.HandState.Intro)
		{
			if (!this.aiAnimator.IsPlaying("glock_draw"))
			{
				this.State = DraGunGlockBehavior.HandState.Out;
				this.AdvanceAttack();
			}
		}
		else if (this.State == DraGunGlockBehavior.HandState.Out || this.State == DraGunGlockBehavior.HandState.In)
		{
			if (this.m_attackIndex >= this.attacks.Length)
			{
				this.State = ((this.State != DraGunGlockBehavior.HandState.Out) ? DraGunGlockBehavior.HandState.MoveToOut : DraGunGlockBehavior.HandState.Outro);
			}
			else if (this.m_delayTimer > 0f)
			{
				this.m_delayTimer -= this.m_deltaTime;
				if (this.m_delayTimer <= 0f)
				{
					this.HandleAim();
				}
			}
			else if (!this.m_isShooting)
			{
				this.Fire();
			}
			else if (!this.aiAnimator.IsPlaying((this.State != DraGunGlockBehavior.HandState.Out) ? "glock_fire_in" : "glock_fire_out"))
			{
				this.m_isShooting = false;
				this.AdvanceAttack();
			}
		}
		else if (this.State == DraGunGlockBehavior.HandState.MoveToIn)
		{
			if (!this.aiAnimator.IsPlaying("glock_flip_in"))
			{
				this.State = DraGunGlockBehavior.HandState.In;
			}
		}
		else if (this.State == DraGunGlockBehavior.HandState.MoveToOut)
		{
			if (!this.aiAnimator.IsPlaying("glock_flip_out"))
			{
				this.State = DraGunGlockBehavior.HandState.Out;
			}
		}
		else if (this.State == DraGunGlockBehavior.HandState.Outro && !this.aiAnimator.IsPlaying("glock_putaway"))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060049F3 RID: 18931 RVA: 0x0018BC34 File Offset: 0x00189E34
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.State = DraGunGlockBehavior.HandState.None;
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
		if (this.aiAnimator)
		{
			this.aiAnimator.EndAnimation();
		}
		if (this.unityAnimation)
		{
			this.unityAnimation.Stop();
			this.unityAnimation.GetClip(this.m_unityAnimPrefix + "GlockPutAway").SampleAnimation(this.unityAnimation.gameObject, 1000f);
			this.unityAnimation.GetComponent<DraGunArmController>().ClipArmSprites();
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x0018BCF0 File Offset: 0x00189EF0
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x060049F5 RID: 18933 RVA: 0x0018BCF4 File Offset: 0x00189EF4
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.m_isShooting && clip.GetFrame(frame).eventInfo == "fire")
		{
			this.ShootBulletScript();
		}
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x0018BD24 File Offset: 0x00189F24
	private void AdvanceAttack()
	{
		this.m_attackIndex++;
		if (this.m_attackIndex < this.attacks.Length)
		{
			this.m_delayTimer = this.attacks[this.m_attackIndex].preDelay;
			if (this.m_delayTimer <= 0f)
			{
				this.HandleAim();
			}
		}
	}

	// Token: 0x060049F7 RID: 18935 RVA: 0x0018BD80 File Offset: 0x00189F80
	private void HandleAim()
	{
		if (this.m_attackIndex < this.attacks.Length)
		{
			DraGunGlockBehavior.GlockAttack glockAttack = this.attacks[this.m_attackIndex];
			DraGunGlockBehavior.FacingDirection facingDirection = glockAttack.dir;
			if (facingDirection == DraGunGlockBehavior.FacingDirection.Aim && this.m_aiActor.TargetRigidbody)
			{
				if (this.m_facingLeft)
				{
					if (this.m_aiActor.TargetRigidbody.UnitCenter.x < this.m_aiActor.specRigidbody.UnitCenter.x - 12.5f)
					{
						facingDirection = DraGunGlockBehavior.FacingDirection.Out;
					}
					else
					{
						facingDirection = DraGunGlockBehavior.FacingDirection.In;
					}
				}
				else if (this.m_aiActor.TargetRigidbody.UnitCenter.x > this.m_aiActor.specRigidbody.UnitCenter.x + 12.5f)
				{
					facingDirection = DraGunGlockBehavior.FacingDirection.Out;
				}
				else
				{
					facingDirection = DraGunGlockBehavior.FacingDirection.In;
				}
			}
			if (facingDirection == DraGunGlockBehavior.FacingDirection.In)
			{
				if (this.State == DraGunGlockBehavior.HandState.Out)
				{
					this.State = DraGunGlockBehavior.HandState.MoveToIn;
				}
			}
			else if (facingDirection == DraGunGlockBehavior.FacingDirection.Out && this.State == DraGunGlockBehavior.HandState.In)
			{
				this.State = DraGunGlockBehavior.HandState.MoveToOut;
			}
		}
	}

	// Token: 0x060049F8 RID: 18936 RVA: 0x0018BEA4 File Offset: 0x0018A0A4
	private void Fire()
	{
		this.m_isShooting = true;
		if (this.State == DraGunGlockBehavior.HandState.In)
		{
			this.aiAnimator.PlayUntilCancelled("glock_fire_in", false, null, -1f, false);
		}
		else if (this.State == DraGunGlockBehavior.HandState.Out)
		{
			this.aiAnimator.PlayUntilCancelled("glock_fire_out", false, null, -1f, false);
		}
	}

	// Token: 0x060049F9 RID: 18937 RVA: 0x0018BF08 File Offset: 0x0018A108
	private void ShootBulletScript()
	{
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.shootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.attacks[this.m_attackIndex].bulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x060049FA RID: 18938 RVA: 0x0018BF74 File Offset: 0x0018A174
	private void BeginState(DraGunGlockBehavior.HandState state)
	{
		if (state == DraGunGlockBehavior.HandState.Intro)
		{
			this.aiAnimator.PlayUntilCancelled("glock_draw", false, null, -1f, false);
			if (this.unityAnimation)
			{
				this.unityAnimation.Play(this.m_unityAnimPrefix + "GlockDraw");
			}
		}
		if (state == DraGunGlockBehavior.HandState.MoveToOut)
		{
			this.aiAnimator.PlayUntilCancelled("glock_flip_out", false, null, -1f, false);
			this.unityAnimation.Play(this.m_unityAnimPrefix + "GlockFlipOut");
		}
		else if (state == DraGunGlockBehavior.HandState.MoveToIn)
		{
			this.aiAnimator.PlayUntilCancelled("glock_flip_in", false, null, -1f, false);
			this.unityAnimation.Play(this.m_unityAnimPrefix + "GlockFlipIn");
		}
		else if (state == DraGunGlockBehavior.HandState.Outro)
		{
			this.aiAnimator.PlayUntilCancelled("glock_putaway", false, null, -1f, false);
			this.unityAnimation.Play(this.m_unityAnimPrefix + "GlockPutAway");
		}
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x0018C08C File Offset: 0x0018A28C
	private void EndState(DraGunGlockBehavior.HandState state)
	{
		if ((state == DraGunGlockBehavior.HandState.In || state == DraGunGlockBehavior.HandState.Out) && this.m_isShooting && this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
	}

	// Token: 0x04003E6D RID: 15981
	public GameObject shootPoint;

	// Token: 0x04003E6E RID: 15982
	public Animation unityAnimation;

	// Token: 0x04003E6F RID: 15983
	public AIAnimator aiAnimator;

	// Token: 0x04003E70 RID: 15984
	public DraGunGlockBehavior.GlockAttack[] attacks;

	// Token: 0x04003E71 RID: 15985
	private DraGunGlockBehavior.HandState m_state;

	// Token: 0x04003E72 RID: 15986
	private float m_delayTimer;

	// Token: 0x04003E73 RID: 15987
	private int m_attackIndex;

	// Token: 0x04003E74 RID: 15988
	private bool m_isShooting;

	// Token: 0x04003E75 RID: 15989
	private bool m_facingLeft;

	// Token: 0x04003E76 RID: 15990
	private string m_unityAnimPrefix;

	// Token: 0x04003E77 RID: 15991
	private BulletScriptSource m_bulletSource;

	// Token: 0x02000DA4 RID: 3492
	private enum HandState
	{
		// Token: 0x04003E79 RID: 15993
		None,
		// Token: 0x04003E7A RID: 15994
		Intro,
		// Token: 0x04003E7B RID: 15995
		In,
		// Token: 0x04003E7C RID: 15996
		MoveToOut,
		// Token: 0x04003E7D RID: 15997
		Out,
		// Token: 0x04003E7E RID: 15998
		MoveToIn,
		// Token: 0x04003E7F RID: 15999
		Outro
	}

	// Token: 0x02000DA5 RID: 3493
	public enum FacingDirection
	{
		// Token: 0x04003E81 RID: 16001
		Out,
		// Token: 0x04003E82 RID: 16002
		In,
		// Token: 0x04003E83 RID: 16003
		Aim
	}

	// Token: 0x02000DA6 RID: 3494
	[Serializable]
	public class GlockAttack
	{
		// Token: 0x04003E84 RID: 16004
		public float preDelay;

		// Token: 0x04003E85 RID: 16005
		public DraGunGlockBehavior.FacingDirection dir;

		// Token: 0x04003E86 RID: 16006
		public BulletScriptSelector bulletScript;
	}
}
