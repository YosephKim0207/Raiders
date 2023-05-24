using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200150D RID: 5389
public class KeyBulletPickup : PickupObject
{
	// Token: 0x06007AEC RID: 31468 RVA: 0x003141C4 File Offset: 0x003123C4
	private void Start()
	{
		this.m_srb = base.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody srb = this.m_srb;
		srb.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(srb.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnPreCollision));
		if (this.minimapIcon != null && !this.m_hasBeenPickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
	}

	// Token: 0x06007AED RID: 31469 RVA: 0x00314268 File Offset: 0x00312468
	private void Update()
	{
		if (base.spriteAnimator != null && base.spriteAnimator.DefaultClip != null)
		{
			base.spriteAnimator.SetFrame(Mathf.FloorToInt(Time.time * base.spriteAnimator.DefaultClip.fps % (float)base.spriteAnimator.DefaultClip.frames.Length));
		}
		if (this.IsRatKey && !GameManager.Instance.IsLoadingLevel && !this.m_hasBeenPickedUp && this && !GameManager.Instance.IsAnyPlayerInRoom(base.transform.position.GetAbsoluteRoom()))
		{
			PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
			if (bestActivePlayer && !bestActivePlayer.IsGhost && bestActivePlayer.AcceptingAnyInput)
			{
				this.m_hasBeenPickedUp = true;
				this.Pickup(bestActivePlayer);
			}
		}
	}

	// Token: 0x06007AEE RID: 31470 RVA: 0x0031435C File Offset: 0x0031255C
	private void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06007AEF RID: 31471 RVA: 0x0031438C File Offset: 0x0031258C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
	}

	// Token: 0x06007AF0 RID: 31472 RVA: 0x003143A4 File Offset: 0x003125A4
	private void OnPreCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (this.m_hasBeenPickedUp)
		{
			return;
		}
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			this.m_hasBeenPickedUp = true;
			this.Pickup(component);
		}
	}

	// Token: 0x06007AF1 RID: 31473 RVA: 0x003143E0 File Offset: 0x003125E0
	public override void Pickup(PlayerController player)
	{
		player.HasGottenKeyThisRun = true;
		base.HandleEncounterable(player);
		this.GetRidOfMinimapIcon();
		if (base.spriteAnimator)
		{
			base.spriteAnimator.StopAndResetFrame();
		}
		player.BloopItemAboveHead(base.sprite, this.overrideBloopSpriteName);
		player.carriedConsumables.KeyBullets += this.numberKeyBullets;
		if (this.IsRatKey)
		{
			player.carriedConsumables.ResourcefulRatKeys++;
		}
		AkSoundEngine.PostEvent("Play_OBJ_key_pickup_01", base.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04007D63 RID: 32099
	public int numberKeyBullets = 1;

	// Token: 0x04007D64 RID: 32100
	public bool IsRatKey;

	// Token: 0x04007D65 RID: 32101
	public string overrideBloopSpriteName = string.Empty;

	// Token: 0x04007D66 RID: 32102
	private bool m_hasBeenPickedUp;

	// Token: 0x04007D67 RID: 32103
	private SpeculativeRigidbody m_srb;

	// Token: 0x04007D68 RID: 32104
	public GameObject minimapIcon;

	// Token: 0x04007D69 RID: 32105
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04007D6A RID: 32106
	private GameObject m_instanceMinimapIcon;
}
