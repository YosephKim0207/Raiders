using System;
using UnityEngine;

// Token: 0x02001842 RID: 6210
public class LaserSightController : BraveBehaviour
{
	// Token: 0x060092FC RID: 37628 RVA: 0x003E0CB4 File Offset: 0x003DEEB4
	public void Start()
	{
		if (base.spriteAnimator)
		{
			this.m_idleClip = base.spriteAnimator.GetClipByName(this.idleAnim);
			this.m_preFireClip = base.spriteAnimator.GetClipByName(this.preFireAnim);
			this.m_preFireLength = this.m_preFireClip.BaseClipLength;
		}
	}

	// Token: 0x060092FD RID: 37629 RVA: 0x003E0D10 File Offset: 0x003DEF10
	public void UpdateCountdown(float m_prefireTimer, float PreFireLaserTime)
	{
		base.renderer.enabled = true;
		if (this.DoFlash)
		{
			float num = 1f - m_prefireTimer / PreFireLaserTime;
			base.renderer.enabled = this.flashCurve.Evaluate(num) > 0.5f;
		}
		if (this.DoAnim && base.spriteAnimator)
		{
			if (m_prefireTimer < this.m_preFireLength)
			{
				base.spriteAnimator.Play(this.m_preFireClip, this.m_preFireLength - m_prefireTimer, this.m_preFireClip.fps, false);
			}
			else
			{
				base.spriteAnimator.Play(this.m_idleClip);
			}
		}
	}

	// Token: 0x060092FE RID: 37630 RVA: 0x003E0DC0 File Offset: 0x003DEFC0
	public void ResetCountdown()
	{
		base.renderer.enabled = false;
		if (this.DoAnim && base.spriteAnimator)
		{
			base.spriteAnimator.Play(this.m_idleClip);
		}
	}

	// Token: 0x04009A8A RID: 39562
	public bool DoFlash;

	// Token: 0x04009A8B RID: 39563
	[CurveRange(0f, 0f, 1f, 1f)]
	public AnimationCurve flashCurve;

	// Token: 0x04009A8C RID: 39564
	public bool DoAnim;

	// Token: 0x04009A8D RID: 39565
	[CheckAnimation(null)]
	public string idleAnim;

	// Token: 0x04009A8E RID: 39566
	[CheckAnimation(null)]
	public string preFireAnim;

	// Token: 0x04009A8F RID: 39567
	private tk2dSpriteAnimationClip m_idleClip;

	// Token: 0x04009A90 RID: 39568
	private tk2dSpriteAnimationClip m_preFireClip;

	// Token: 0x04009A91 RID: 39569
	private float m_preFireLength;
}
