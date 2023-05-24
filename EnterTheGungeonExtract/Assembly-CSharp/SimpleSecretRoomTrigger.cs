using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001211 RID: 4625
public class SimpleSecretRoomTrigger : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06006773 RID: 26483 RVA: 0x00287DB0 File Offset: 0x00285FB0
	public void Initialize()
	{
		this.parentRoom.RegisterInteractable(this);
	}

	// Token: 0x06006774 RID: 26484 RVA: 0x00287DC0 File Offset: 0x00285FC0
	private void HandleTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody)
	{
		if (!this.referencedSecretRoom.IsOpen && specRigidbody.projectile != null)
		{
			this.parentRoom.DeregisterInteractable(this);
			if (base.spriteAnimator != null)
			{
				base.spriteAnimator.Play();
			}
			this.referencedSecretRoom.OpenDoor();
		}
	}

	// Token: 0x06006775 RID: 26485 RVA: 0x00287E24 File Offset: 0x00286024
	public float GetDistanceToPoint(Vector2 point)
	{
		return Vector2.Distance(point, base.sprite.WorldCenter);
	}

	// Token: 0x06006776 RID: 26486 RVA: 0x00287E38 File Offset: 0x00286038
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006777 RID: 26487 RVA: 0x00287E40 File Offset: 0x00286040
	public void OnEnteredRange(PlayerController interactor)
	{
		if (this.referencedSecretRoom.IsOpen)
		{
			return;
		}
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006778 RID: 26488 RVA: 0x00287E7C File Offset: 0x0028607C
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
	}

	// Token: 0x06006779 RID: 26489 RVA: 0x00287E98 File Offset: 0x00286098
	public void Disable()
	{
		this.parentRoom.DeregisterInteractable(this);
	}

	// Token: 0x0600677A RID: 26490 RVA: 0x00287EA8 File Offset: 0x002860A8
	public void Interact(PlayerController interactor)
	{
		this.parentRoom.DeregisterInteractable(this);
		if (this.referencedSecretRoom.IsOpen)
		{
			return;
		}
		if (base.spriteAnimator != null)
		{
			base.spriteAnimator.Play();
		}
		this.referencedSecretRoom.OpenDoor();
	}

	// Token: 0x0600677B RID: 26491 RVA: 0x00287EFC File Offset: 0x002860FC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600677C RID: 26492 RVA: 0x00287F08 File Offset: 0x00286108
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006352 RID: 25426
	public SecretRoomManager referencedSecretRoom;

	// Token: 0x04006353 RID: 25427
	public RoomHandler parentRoom;
}
