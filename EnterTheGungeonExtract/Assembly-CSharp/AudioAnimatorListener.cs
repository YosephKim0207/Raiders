using System;

// Token: 0x0200181B RID: 6171
public class AudioAnimatorListener : BraveBehaviour
{
	// Token: 0x06009181 RID: 37249 RVA: 0x003D8EF0 File Offset: 0x003D70F0
	private void Start()
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		if (base.spriteAnimator.CurrentClip != null)
		{
			this.HandleAnimationEvent(base.spriteAnimator, base.spriteAnimator.CurrentClip, 0);
		}
	}

	// Token: 0x06009182 RID: 37250 RVA: 0x003D8F4C File Offset: 0x003D714C
	protected void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		for (int i = 0; i < this.animationAudioEvents.Length; i++)
		{
			if (this.animationAudioEvents[i].eventTag == frame.eventInfo)
			{
				AkSoundEngine.PostEvent(this.animationAudioEvents[i].eventName, base.gameObject);
			}
		}
	}

	// Token: 0x06009183 RID: 37251 RVA: 0x003D8FB0 File Offset: 0x003D71B0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040099CD RID: 39373
	public ActorAudioEvent[] animationAudioEvents;
}
