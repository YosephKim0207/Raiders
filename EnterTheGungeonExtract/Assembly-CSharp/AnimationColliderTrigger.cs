using System;

// Token: 0x02000FA6 RID: 4006
public class AnimationColliderTrigger : BraveBehaviour
{
	// Token: 0x06005735 RID: 22325 RVA: 0x00214218 File Offset: 0x00212418
	private void Awake()
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
	}

	// Token: 0x06005736 RID: 22326 RVA: 0x00214244 File Offset: 0x00212444
	protected override void OnDestroy()
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		base.OnDestroy();
	}

	// Token: 0x06005737 RID: 22327 RVA: 0x00214274 File Offset: 0x00212474
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (clip.GetFrame(frame).eventInfo == "collider_on")
		{
			if (base.aiActor)
			{
				base.aiActor.IsGone = false;
			}
			if (base.specRigidbody)
			{
				base.specRigidbody.CollideWithOthers = true;
			}
		}
		else if (clip.GetFrame(frame).eventInfo == "collider_off")
		{
			if (base.aiActor)
			{
				base.aiActor.IsGone = true;
			}
			if (base.specRigidbody)
			{
				base.specRigidbody.CollideWithOthers = false;
			}
		}
	}
}
