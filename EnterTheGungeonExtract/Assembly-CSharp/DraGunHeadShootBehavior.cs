using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DA8 RID: 3496
[InspectorDropdownName("Bosses/DraGun/HeadShootBehavior")]
public class DraGunHeadShootBehavior : BasicAttackBehavior
{
	// Token: 0x06004A08 RID: 18952 RVA: 0x0018C5B8 File Offset: 0x0018A7B8
	public override void Start()
	{
		base.Start();
		this.m_dragun = this.m_aiActor.GetComponent<DraGunController>();
		this.m_unityAnimation = this.m_dragun.neck.GetComponent<Animation>();
		this.m_head = this.m_dragun.head;
		this.m_headAnimator = this.m_head.aiAnimator;
		this.m_clip = this.m_unityAnimation.GetClip(this.MotionClip);
		if (this.fireType == DraGunHeadShootBehavior.FireType.tk2dAnimEvent)
		{
			tk2dSpriteAnimator spriteAnimator = this.m_head.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.tk2dAnimationEventTriggered));
		}
		if (this.fireType == DraGunHeadShootBehavior.FireType.UnityAnimEvent)
		{
			this.m_aiActor.behaviorSpeculator.AnimationEventTriggered += this.UnityAnimationEventTriggered;
		}
	}

	// Token: 0x06004A09 RID: 18953 RVA: 0x0018C68C File Offset: 0x0018A88C
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004A0A RID: 18954 RVA: 0x0018C694 File Offset: 0x0018A894
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
		this.m_state = DraGunHeadShootBehavior.State.MovingToPosition;
		this.m_head.OverrideDesiredPosition = new Vector2?(this.GetStartPosition());
		if (this.ShootPoint)
		{
			this.m_cachedShootPointRotation = this.ShootPoint.transform.eulerAngles.z;
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x06004A0B RID: 18955 RVA: 0x0018C710 File Offset: 0x0018A910
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.m_state == DraGunHeadShootBehavior.State.MovingToPosition)
		{
			if (this.m_head.ReachedOverridePosition)
			{
				this.m_state = DraGunHeadShootBehavior.State.Animating;
				this.m_head.OverrideDesiredPosition = null;
				this.m_headAnimator.AnimatedFacingDirection = this.m_headAnimator.FacingDirection;
				this.m_headAnimator.UseAnimatedFacingDirection = true;
				if (this.fireType == DraGunHeadShootBehavior.FireType.Immediate)
				{
					this.ShootBulletScript();
				}
				this.m_clip.SampleAnimation(this.m_aiActor.gameObject, 0f);
				this.m_unityAnimation.Stop();
				this.m_unityAnimation.cullingType = AnimationCullingType.BasedOnRenderers;
				this.m_unityAnimation.cullingType = AnimationCullingType.AlwaysAnimate;
				this.m_unityAnimation.Play(this.MotionClip);
				this.m_headAnimator.PlayUntilCancelled(this.HeadAiAnim, false, null, -1f, false);
			}
		}
		else if (this.m_state == DraGunHeadShootBehavior.State.Animating)
		{
			if (this.ShootPoint)
			{
				this.ShootPoint.transform.rotation = Quaternion.Euler(this.ShootPoint.transform.eulerAngles.WithZ(this.m_headAnimator.FacingDirection));
			}
			if (!this.m_unityAnimation.IsPlaying(this.MotionClip))
			{
				return ContinuousBehaviorResult.Finished;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004A0C RID: 18956 RVA: 0x0018C868 File Offset: 0x0018AA68
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.m_state = DraGunHeadShootBehavior.State.None;
		this.m_headAnimator.UseAnimatedFacingDirection = false;
		this.m_headAnimator.EndAnimationIf(this.HeadAiAnim);
		if (this.m_unityAnimation)
		{
			this.m_unityAnimation.Stop();
			this.m_unityAnimation.GetClip(this.MotionClip).SampleAnimation(this.m_unityAnimation.gameObject, 1000f);
		}
		if (this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
		if (this.ShootPoint)
		{
			this.ShootPoint.transform.rotation = Quaternion.Euler(this.ShootPoint.transform.eulerAngles.WithZ(this.m_cachedShootPointRotation));
		}
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x06004A0D RID: 18957 RVA: 0x0018C94C File Offset: 0x0018AB4C
	public override bool IsOverridable()
	{
		return false;
	}

	// Token: 0x06004A0E RID: 18958 RVA: 0x0018C950 File Offset: 0x0018AB50
	private void tk2dAnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		this.UnityAnimationEventTriggered(clip.GetFrame(frame).eventInfo);
	}

	// Token: 0x06004A0F RID: 18959 RVA: 0x0018C964 File Offset: 0x0018AB64
	private void UnityAnimationEventTriggered(string eventInfo)
	{
		if (eventInfo == "fire")
		{
			this.ShootBulletScript();
		}
		else if (eventInfo == "cease_fire" && this.m_bulletSource)
		{
			this.m_bulletSource.ForceStop();
		}
	}

	// Token: 0x06004A10 RID: 18960 RVA: 0x0018C9B8 File Offset: 0x0018ABB8
	private void ShootBulletScript()
	{
		if (string.IsNullOrEmpty(this.BulletScript.scriptTypeName))
		{
			return;
		}
		if (!this.m_bulletSource)
		{
			this.m_bulletSource = this.ShootPoint.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletSource.BulletManager = this.m_aiActor.bulletBank;
		this.m_bulletSource.BulletScript = this.BulletScript;
		this.m_bulletSource.Initialize();
	}

	// Token: 0x06004A11 RID: 18961 RVA: 0x0018CA30 File Offset: 0x0018AC30
	private Vector2 GetStartPosition()
	{
		if (!this.s_dummyGameObject)
		{
			this.s_dummyGameObject = new GameObject("Dummy Game Object");
			this.s_dummyHeadObject = new GameObject("head");
			this.s_dummyHeadObject.transform.parent = this.s_dummyGameObject.transform;
		}
		this.m_clip.SampleAnimation(this.s_dummyGameObject, 0f);
		return this.s_dummyHeadObject.transform.position + this.m_aiActor.transform.position;
	}

	// Token: 0x04003E9A RID: 16026
	public GameObject ShootPoint;

	// Token: 0x04003E9B RID: 16027
	public BulletScriptSelector BulletScript;

	// Token: 0x04003E9C RID: 16028
	public DraGunHeadShootBehavior.FireType fireType;

	// Token: 0x04003E9D RID: 16029
	public string HeadAiAnim = "sweep";

	// Token: 0x04003E9E RID: 16030
	public string MotionClip = "DraGunHeadSweep1";

	// Token: 0x04003E9F RID: 16031
	private DraGunController m_dragun;

	// Token: 0x04003EA0 RID: 16032
	private DraGunHeadController m_head;

	// Token: 0x04003EA1 RID: 16033
	private AIAnimator m_headAnimator;

	// Token: 0x04003EA2 RID: 16034
	private Animation m_unityAnimation;

	// Token: 0x04003EA3 RID: 16035
	private DraGunHeadShootBehavior.State m_state;

	// Token: 0x04003EA4 RID: 16036
	private AnimationClip m_clip;

	// Token: 0x04003EA5 RID: 16037
	private float m_cachedShootPointRotation;

	// Token: 0x04003EA6 RID: 16038
	private BulletScriptSource m_bulletSource;

	// Token: 0x04003EA7 RID: 16039
	private GameObject s_dummyGameObject;

	// Token: 0x04003EA8 RID: 16040
	private GameObject s_dummyHeadObject;

	// Token: 0x02000DA9 RID: 3497
	private enum State
	{
		// Token: 0x04003EAA RID: 16042
		None,
		// Token: 0x04003EAB RID: 16043
		MovingToPosition,
		// Token: 0x04003EAC RID: 16044
		Animating
	}

	// Token: 0x02000DAA RID: 3498
	public enum FireType
	{
		// Token: 0x04003EAE RID: 16046
		Immediate,
		// Token: 0x04003EAF RID: 16047
		tk2dAnimEvent,
		// Token: 0x04003EB0 RID: 16048
		UnityAnimEvent
	}
}
