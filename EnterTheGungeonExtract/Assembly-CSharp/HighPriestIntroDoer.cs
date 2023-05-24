using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200104A RID: 4170
[RequireComponent(typeof(GenericIntroDoer))]
public class HighPriestIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005B8D RID: 23437 RVA: 0x00230A48 File Offset: 0x0022EC48
	public void Start()
	{
		base.aiActor.ParentRoom.Entered += this.PlayerEnteredRoom;
		base.aiActor.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x00230A84 File Offset: 0x0022EC84
	protected override void OnDestroy()
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.BodyAnimationEventTriggered));
		tk2dSpriteAnimator spriteAnimator2 = base.spriteAnimator;
		spriteAnimator2.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator2.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.BodyAnimationComplete));
		tk2dSpriteAnimator spriteAnimator3 = this.head.spriteAnimator;
		spriteAnimator3.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator3.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.HeadAnimationComplete));
		this.RestrictMotion(false);
		if (base.aiActor && base.aiActor.ParentRoom != null)
		{
			base.aiActor.ParentRoom.Entered -= this.PlayerEnteredRoom;
		}
		if (base.aiActor && base.aiActor.healthHaver)
		{
			base.aiActor.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x06005B8F RID: 23439 RVA: 0x00230B98 File Offset: 0x0022ED98
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.BodyAnimationEventTriggered));
		tk2dSpriteAnimator spriteAnimator2 = base.spriteAnimator;
		spriteAnimator2.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator2.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.BodyAnimationComplete));
		tk2dSpriteAnimator spriteAnimator3 = this.head.spriteAnimator;
		spriteAnimator3.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator3.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.HeadAnimationComplete));
		animators.Add(this.head.spriteAnimator);
		this.head.spriteAnimator.enabled = false;
	}

	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x06005B90 RID: 23440 RVA: 0x00230C5C File Offset: 0x0022EE5C
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_finished;
		}
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x00230C64 File Offset: 0x0022EE64
	public override void EndIntro()
	{
		base.aiAnimator.EndAnimation();
	}

	// Token: 0x06005B92 RID: 23442 RVA: 0x00230C74 File Offset: 0x0022EE74
	public override void OnCleanup()
	{
		this.head.spriteAnimator.enabled = true;
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x00230C88 File Offset: 0x0022EE88
	private void PlayerEnteredRoom(PlayerController playerController)
	{
		this.RestrictMotion(true);
	}

	// Token: 0x06005B94 RID: 23444 RVA: 0x00230C94 File Offset: 0x0022EE94
	private void OnPreDeath(Vector2 finalDirection)
	{
		this.RestrictMotion(false);
		base.aiActor.ParentRoom.Entered -= this.PlayerEnteredRoom;
	}

	// Token: 0x06005B95 RID: 23445 RVA: 0x00230CBC File Offset: 0x0022EEBC
	private void PlayerMovementRestrictor(SpeculativeRigidbody playerSpecRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		if (pixelOffset.y < prevPixelOffset.y)
		{
			int num = playerSpecRigidbody.PixelColliders[0].MinY + pixelOffset.y;
			if (num < this.m_minPlayerY)
			{
				validLocation = false;
			}
		}
	}

	// Token: 0x06005B96 RID: 23446 RVA: 0x00230D10 File Offset: 0x0022EF10
	private void BodyAnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (clip.GetFrame(frame).eventInfo == "show_head")
		{
			this.head.enabled = false;
			this.head.spriteAnimator.Play("gun_appear_intro");
		}
	}

	// Token: 0x06005B97 RID: 23447 RVA: 0x00230D50 File Offset: 0x0022EF50
	private void BodyAnimationComplete(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		if (clip.name == "priest_recloak")
		{
			this.m_finished = true;
			this.head.enabled = true;
			base.spriteAnimator.Play("priest_idle");
		}
	}

	// Token: 0x06005B98 RID: 23448 RVA: 0x00230D8C File Offset: 0x0022EF8C
	private void HeadAnimationComplete(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		if (clip.name == "gun_appear_intro")
		{
			base.aiAnimator.PlayUntilFinished("recloak", false, null, -1f, false);
		}
	}

	// Token: 0x06005B99 RID: 23449 RVA: 0x00230DBC File Offset: 0x0022EFBC
	public void RestrictMotion(bool value)
	{
		if (this.m_isMotionRestricted == value)
		{
			return;
		}
		if (value)
		{
			if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
			{
				return;
			}
			this.m_minPlayerY = base.aiActor.ParentRoom.area.basePosition.y * 16 + 8;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				SpeculativeRigidbody specRigidbody = GameManager.Instance.AllPlayers[i].specRigidbody;
				specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
			}
		}
		else
		{
			if (!GameManager.HasInstance || GameManager.IsReturningToBreach)
			{
				return;
			}
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController)
				{
					SpeculativeRigidbody specRigidbody2 = playerController.specRigidbody;
					specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
				}
			}
		}
		this.m_isMotionRestricted = value;
	}

	// Token: 0x04005519 RID: 21785
	public AIAnimator head;

	// Token: 0x0400551A RID: 21786
	private bool m_isMotionRestricted;

	// Token: 0x0400551B RID: 21787
	private bool m_finished;

	// Token: 0x0400551C RID: 21788
	private int m_minPlayerY;
}
