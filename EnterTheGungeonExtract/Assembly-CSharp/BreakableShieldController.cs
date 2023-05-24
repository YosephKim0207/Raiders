using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200110B RID: 4363
public class BreakableShieldController : BraveBehaviour, SingleSpawnableGunPlacedObject
{
	// Token: 0x06006038 RID: 24632 RVA: 0x00250E58 File Offset: 0x0024F058
	public void Deactivate()
	{
		base.majorBreakable.Break(Vector2.zero);
	}

	// Token: 0x06006039 RID: 24633 RVA: 0x00250E6C File Offset: 0x0024F06C
	public void Initialize(Gun sourceGun)
	{
		if (sourceGun && sourceGun.CurrentOwner)
		{
			this.ownerPlayer = sourceGun.CurrentOwner as PlayerController;
			base.transform.position = sourceGun.CurrentOwner.CenterPosition.ToVector3ZUp(0f);
			base.specRigidbody.Reinitialize();
		}
		this.m_room = base.transform.position.GetAbsoluteRoom();
		base.spriteAnimator.Play(this.introAnimation);
	}

	// Token: 0x0600603A RID: 24634 RVA: 0x00250EF8 File Offset: 0x0024F0F8
	private void HandleIdleAnimation()
	{
		if (base.spriteAnimator.IsPlaying(this.introAnimation))
		{
			return;
		}
		float currentHealthPercentage = base.majorBreakable.GetCurrentHealthPercentage();
		string text = this.idleAnimation;
		if (currentHealthPercentage < 0.25f)
		{
			text = this.idleBreak3Animation;
		}
		else if (currentHealthPercentage < 0.5f)
		{
			text = this.idleBreak2Animation;
		}
		else if (currentHealthPercentage < 0.75f)
		{
			text = this.idleBreak1Animation;
		}
		if (!base.spriteAnimator.IsPlaying(text))
		{
			base.spriteAnimator.Play(text);
		}
	}

	// Token: 0x0600603B RID: 24635 RVA: 0x00250F8C File Offset: 0x0024F18C
	private void Update()
	{
		if (base.majorBreakable.IsDestroyed)
		{
			return;
		}
		this.m_elapsed += BraveTime.DeltaTime;
		this.HandleIdleAnimation();
		if (this.m_elapsed > this.maxDuration)
		{
			base.majorBreakable.Break(Vector2.zero);
		}
		if (this.ownerPlayer && this.ownerPlayer.CurrentRoom != this.m_room)
		{
			base.majorBreakable.Break(Vector2.zero);
		}
	}

	// Token: 0x04005AD5 RID: 23253
	[CheckAnimation(null)]
	public string introAnimation;

	// Token: 0x04005AD6 RID: 23254
	[CheckAnimation(null)]
	public string idleAnimation;

	// Token: 0x04005AD7 RID: 23255
	[CheckAnimation(null)]
	public string idleBreak1Animation;

	// Token: 0x04005AD8 RID: 23256
	[CheckAnimation(null)]
	public string idleBreak2Animation;

	// Token: 0x04005AD9 RID: 23257
	[CheckAnimation(null)]
	public string idleBreak3Animation;

	// Token: 0x04005ADA RID: 23258
	public float maxDuration = 60f;

	// Token: 0x04005ADB RID: 23259
	private float m_elapsed;

	// Token: 0x04005ADC RID: 23260
	private PlayerController ownerPlayer;

	// Token: 0x04005ADD RID: 23261
	private RoomHandler m_room;
}
