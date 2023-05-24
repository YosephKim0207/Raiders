using System;
using AK.Wwise;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x020018EE RID: 6382
public class AkEventPlayableBehavior : PlayableBehaviour
{
	// Token: 0x06009D42 RID: 40258 RVA: 0x003EEB9C File Offset: 0x003ECD9C
	public override void PrepareFrame(Playable playable, FrameData info)
	{
		if (this.eventTracker != null)
		{
			bool flag = info.evaluationType == FrameData.EvaluationType.Evaluate && Application.isPlaying;
			if (flag && this.ShouldPlay(playable))
			{
				if (!this.eventTracker.eventIsPlaying)
				{
					this.requiredActions |= 1U;
					this.requiredActions |= 8U;
					this.checkForFadeIn((float)playable.GetTime<Playable>());
					this.checkForFadeOut(playable);
				}
				this.requiredActions |= 16U;
			}
			else
			{
				if (!this.eventTracker.eventIsPlaying && (this.requiredActions & 1U) == 0U)
				{
					this.requiredActions |= 2U;
					this.checkForFadeIn((float)playable.GetTime<Playable>());
				}
				this.checkForFadeOut(playable);
			}
		}
	}

	// Token: 0x06009D43 RID: 40259 RVA: 0x003EEC70 File Offset: 0x003ECE70
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		if (this.akEvent != null && this.ShouldPlay(playable))
		{
			this.requiredActions |= 1U;
			if (info.evaluationType == FrameData.EvaluationType.Evaluate && Application.isPlaying)
			{
				this.requiredActions |= 8U;
				this.checkForFadeIn((float)playable.GetTime<Playable>());
				this.checkForFadeOut(playable);
			}
			else
			{
				float proportionalTime = this.getProportionalTime(playable);
				float num = 0.05f;
				if (proportionalTime > num)
				{
					this.requiredActions |= 16U;
				}
				this.checkForFadeIn((float)playable.GetTime<Playable>());
				this.checkForFadeOut(playable);
			}
		}
	}

	// Token: 0x06009D44 RID: 40260 RVA: 0x003EED18 File Offset: 0x003ECF18
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (this.eventObject != null)
		{
			this.stopEvent(0);
		}
	}

	// Token: 0x06009D45 RID: 40261 RVA: 0x003EED34 File Offset: 0x003ECF34
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (!this.overrideTrackEmittorObject)
		{
			GameObject gameObject = playerData as GameObject;
			if (gameObject != null)
			{
				this.eventObject = gameObject;
			}
		}
		if (this.eventObject != null)
		{
			float num = (float)playable.GetTime<Playable>();
			if (this.actionIsRequired(AkEventPlayableBehavior.AkPlayableAction.Playback))
			{
				this.playEvent();
			}
			if (this.eventShouldRetrigger && this.actionIsRequired(AkEventPlayableBehavior.AkPlayableAction.Retrigger))
			{
				this.retriggerEvent(playable);
			}
			if (this.actionIsRequired(AkEventPlayableBehavior.AkPlayableAction.Stop))
			{
				this.akEvent.Stop(this.eventObject, 0, AkCurveInterpolation.AkCurveInterpolation_Linear);
			}
			if (this.actionIsRequired(AkEventPlayableBehavior.AkPlayableAction.DelayedStop))
			{
				this.stopEvent(AkEventPlayableBehavior.scrubPlaybackLengthMs);
			}
			if (this.actionIsRequired(AkEventPlayableBehavior.AkPlayableAction.Seek))
			{
				this.seekToTime(playable);
			}
			if (this.actionIsRequired(AkEventPlayableBehavior.AkPlayableAction.FadeIn))
			{
				this.triggerFadeIn(num);
			}
			if (this.actionIsRequired(AkEventPlayableBehavior.AkPlayableAction.FadeOut))
			{
				float num2 = (float)(playable.GetDuration<Playable>() - playable.GetTime<Playable>());
				this.triggerFadeOut(num2);
			}
		}
		this.requiredActions = 0U;
	}

	// Token: 0x06009D46 RID: 40262 RVA: 0x003EEE38 File Offset: 0x003ED038
	private bool actionIsRequired(AkEventPlayableBehavior.AkPlayableAction actionType)
	{
		return (this.requiredActions & (uint)actionType) != 0U;
	}

	// Token: 0x06009D47 RID: 40263 RVA: 0x003EEE48 File Offset: 0x003ED048
	private bool ShouldPlay(Playable playable)
	{
		if (this.eventTracker == null)
		{
			return false;
		}
		if (this.akEventMaxDuration == this.akEventMinDuration && this.akEventMinDuration != -1f)
		{
			return (float)playable.GetTime<Playable>() < this.akEventMaxDuration || this.eventShouldRetrigger;
		}
		float num = (float)playable.GetTime<Playable>() - this.eventTracker.previousEventStartTime;
		float currentDuration = this.eventTracker.currentDuration;
		float num2 = ((currentDuration != -1f) ? currentDuration : ((float)playable.GetDuration<Playable>()));
		return num < num2 || this.eventShouldRetrigger;
	}

	// Token: 0x06009D48 RID: 40264 RVA: 0x003EEEE8 File Offset: 0x003ED0E8
	private bool fadeInRequired(float currentClipTime)
	{
		float num = this.blendInDuration - currentClipTime;
		float num2 = this.easeInDuration - currentClipTime;
		return num > 0f || num2 > 0f;
	}

	// Token: 0x06009D49 RID: 40265 RVA: 0x003EEF20 File Offset: 0x003ED120
	private void checkForFadeIn(float currentClipTime)
	{
		if (this.fadeInRequired(currentClipTime))
		{
			this.requiredActions |= 32U;
		}
	}

	// Token: 0x06009D4A RID: 40266 RVA: 0x003EEF40 File Offset: 0x003ED140
	private void checkForFadeInImmediate(float currentClipTime)
	{
		if (this.fadeInRequired(currentClipTime))
		{
			this.triggerFadeIn(currentClipTime);
		}
	}

	// Token: 0x06009D4B RID: 40267 RVA: 0x003EEF58 File Offset: 0x003ED158
	private bool fadeOutRequired(Playable playable)
	{
		float num = (float)(playable.GetDuration<Playable>() - playable.GetTime<Playable>());
		float num2 = this.blendOutDuration - num;
		float num3 = this.easeOutDuration - num;
		return num2 >= 0f || num3 >= 0f;
	}

	// Token: 0x06009D4C RID: 40268 RVA: 0x003EEFA0 File Offset: 0x003ED1A0
	private void checkForFadeOutImmediate(Playable playable)
	{
		if (this.eventTracker != null && !this.eventTracker.fadeoutTriggered && this.fadeOutRequired(playable))
		{
			float num = (float)(playable.GetDuration<Playable>() - playable.GetTime<Playable>());
			this.triggerFadeOut(num);
		}
	}

	// Token: 0x06009D4D RID: 40269 RVA: 0x003EEFEC File Offset: 0x003ED1EC
	private void checkForFadeOut(Playable playable)
	{
		if (this.eventTracker != null && !this.eventTracker.fadeoutTriggered && this.fadeOutRequired(playable))
		{
			this.requiredActions |= 64U;
		}
	}

	// Token: 0x06009D4E RID: 40270 RVA: 0x003EF024 File Offset: 0x003ED224
	protected void triggerFadeIn(float currentClipTime)
	{
		if (this.eventObject != null && this.akEvent != null)
		{
			float num = Mathf.Max(this.easeInDuration - currentClipTime, this.blendInDuration - currentClipTime);
			if (num > 0f)
			{
				this.akEvent.ExecuteAction(this.eventObject, AkActionOnEventType.AkActionOnEventType_Pause, 0, AkCurveInterpolation.AkCurveInterpolation_Linear);
				this.akEvent.ExecuteAction(this.eventObject, AkActionOnEventType.AkActionOnEventType_Resume, (int)(num * 1000f), AkCurveInterpolation.AkCurveInterpolation_Linear);
			}
		}
	}

	// Token: 0x06009D4F RID: 40271 RVA: 0x003EF0A0 File Offset: 0x003ED2A0
	protected void triggerFadeOut(float fadeDuration)
	{
		if (this.eventObject != null && this.akEvent != null)
		{
			if (this.eventTracker != null)
			{
				this.eventTracker.fadeoutTriggered = true;
			}
			this.akEvent.ExecuteAction(this.eventObject, AkActionOnEventType.AkActionOnEventType_Stop, (int)(fadeDuration * 1000f), AkCurveInterpolation.AkCurveInterpolation_Linear);
		}
	}

	// Token: 0x06009D50 RID: 40272 RVA: 0x003EF0FC File Offset: 0x003ED2FC
	protected void stopEvent(int transition = 0)
	{
		if (this.eventObject != null && this.akEvent != null && this.eventTracker.eventIsPlaying)
		{
			this.akEvent.Stop(this.eventObject, transition, AkCurveInterpolation.AkCurveInterpolation_Linear);
			if (this.eventTracker != null)
			{
				this.eventTracker.eventIsPlaying = false;
			}
		}
	}

	// Token: 0x06009D51 RID: 40273 RVA: 0x003EF160 File Offset: 0x003ED360
	protected void playEvent()
	{
		if (this.eventObject != null && this.akEvent != null && this.eventTracker != null)
		{
			this.eventTracker.playingID = this.akEvent.Post(this.eventObject, 9U, new AkCallbackManager.EventCallback(this.eventTracker.CallbackHandler), null);
			if (this.eventTracker.playingID != 0U)
			{
				this.eventTracker.eventIsPlaying = true;
				this.eventTracker.currentDurationProportion = 1f;
				this.eventTracker.previousEventStartTime = 0f;
			}
		}
	}

	// Token: 0x06009D52 RID: 40274 RVA: 0x003EF200 File Offset: 0x003ED400
	protected void retriggerEvent(Playable playable)
	{
		if (this.eventObject != null && this.akEvent != null && this.eventTracker != null)
		{
			this.eventTracker.playingID = this.akEvent.Post(this.eventObject, 9U, new AkCallbackManager.EventCallback(this.eventTracker.CallbackHandler), null);
			if (this.eventTracker.playingID != 0U)
			{
				this.eventTracker.eventIsPlaying = true;
				float num = this.seekToTime(playable);
				this.eventTracker.currentDurationProportion = num;
				this.eventTracker.previousEventStartTime = (float)playable.GetTime<Playable>();
			}
		}
	}

	// Token: 0x06009D53 RID: 40275 RVA: 0x003EF2A8 File Offset: 0x003ED4A8
	protected float getProportionalTime(Playable playable)
	{
		if (this.eventTracker == null)
		{
			return 0f;
		}
		if (this.akEventMaxDuration == this.akEventMinDuration && this.akEventMinDuration != -1f)
		{
			return (float)playable.GetTime<Playable>() % this.akEventMaxDuration / this.akEventMaxDuration;
		}
		float num = (float)playable.GetTime<Playable>() - this.eventTracker.previousEventStartTime;
		float currentDuration = this.eventTracker.currentDuration;
		float num2 = ((currentDuration != -1f) ? currentDuration : ((float)playable.GetDuration<Playable>()));
		return num % num2 / num2;
	}

	// Token: 0x06009D54 RID: 40276 RVA: 0x003EF33C File Offset: 0x003ED53C
	protected float seekToTime(Playable playable)
	{
		if (this.eventObject != null && this.akEvent != null)
		{
			float proportionalTime = this.getProportionalTime(playable);
			if (proportionalTime < 1f)
			{
				AkSoundEngine.SeekOnEvent((uint)this.akEvent.ID, this.eventObject, proportionalTime);
				return 1f - proportionalTime;
			}
		}
		return 1f;
	}

	// Token: 0x04009EC8 RID: 40648
	public static int scrubPlaybackLengthMs = 100;

	// Token: 0x04009EC9 RID: 40649
	public AK.Wwise.Event akEvent;

	// Token: 0x04009ECA RID: 40650
	public float akEventMaxDuration = -1f;

	// Token: 0x04009ECB RID: 40651
	public float akEventMinDuration = -1f;

	// Token: 0x04009ECC RID: 40652
	public float blendInDuration;

	// Token: 0x04009ECD RID: 40653
	public float blendOutDuration;

	// Token: 0x04009ECE RID: 40654
	public float easeInDuration;

	// Token: 0x04009ECF RID: 40655
	public float easeOutDuration;

	// Token: 0x04009ED0 RID: 40656
	public GameObject eventObject;

	// Token: 0x04009ED1 RID: 40657
	public bool eventShouldRetrigger;

	// Token: 0x04009ED2 RID: 40658
	public WwiseEventTracker eventTracker;

	// Token: 0x04009ED3 RID: 40659
	public float lastEffectiveWeight = 1f;

	// Token: 0x04009ED4 RID: 40660
	public bool overrideTrackEmittorObject;

	// Token: 0x04009ED5 RID: 40661
	public uint requiredActions;

	// Token: 0x020018EF RID: 6383
	public enum AkPlayableAction
	{
		// Token: 0x04009ED7 RID: 40663
		None,
		// Token: 0x04009ED8 RID: 40664
		Playback,
		// Token: 0x04009ED9 RID: 40665
		Retrigger,
		// Token: 0x04009EDA RID: 40666
		Stop = 4,
		// Token: 0x04009EDB RID: 40667
		DelayedStop = 8,
		// Token: 0x04009EDC RID: 40668
		Seek = 16,
		// Token: 0x04009EDD RID: 40669
		FadeIn = 32,
		// Token: 0x04009EDE RID: 40670
		FadeOut = 64
	}
}
