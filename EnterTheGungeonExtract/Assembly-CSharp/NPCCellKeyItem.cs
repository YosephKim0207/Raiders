using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001445 RID: 5189
public class NPCCellKeyItem : PickupObject
{
	// Token: 0x060075CE RID: 30158 RVA: 0x002EE778 File Offset: 0x002EC978
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnPreCollision));
		if (!this.m_pickedUp)
		{
			this.RegisterMinimapIcon();
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
	}

	// Token: 0x060075CF RID: 30159 RVA: 0x002EE7D0 File Offset: 0x002EC9D0
	private void Update()
	{
		if (!this.m_pickedUp && this && !GameManager.Instance.IsAnyPlayerInRoom(base.transform.position.GetAbsoluteRoom()))
		{
			PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
			if (bestActivePlayer && !bestActivePlayer.IsGhost && bestActivePlayer.AcceptingAnyInput)
			{
				this.Pickup(bestActivePlayer);
			}
		}
	}

	// Token: 0x060075D0 RID: 30160 RVA: 0x002EE848 File Offset: 0x002ECA48
	public void RegisterMinimapIcon()
	{
		if (base.transform.position.y < -300f)
		{
			return;
		}
		if (this.minimapIcon == null)
		{
			if (NPCCellKeyItem.m_defaultIcon == null)
			{
				NPCCellKeyItem.m_defaultIcon = (GameObject)BraveResources.Load("Global Prefabs/Minimap_CellKey_Icon", ".prefab");
			}
			this.minimapIcon = NPCCellKeyItem.m_defaultIcon;
		}
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
	}

	// Token: 0x060075D1 RID: 30161 RVA: 0x002EE91C File Offset: 0x002ECB1C
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x060075D2 RID: 30162 RVA: 0x002EE94C File Offset: 0x002ECB4C
	private void OnPreCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			this.Pickup(component);
			AkSoundEngine.PostEvent("Play_OBJ_goldkey_pickup_01", base.gameObject);
		}
	}

	// Token: 0x060075D3 RID: 30163 RVA: 0x002EE990 File Offset: 0x002ECB90
	public void DropLogic()
	{
		this.m_forceExtant = true;
		this.m_pickedUp = false;
	}

	// Token: 0x060075D4 RID: 30164 RVA: 0x002EE9A0 File Offset: 0x002ECBA0
	public override void Pickup(PlayerController player)
	{
		if (this.IsBeingDestroyed)
		{
			return;
		}
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_pickedUp = true;
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		this.GetRidOfMinimapIcon();
		if (base.specRigidbody)
		{
			base.specRigidbody.enabled = false;
		}
		if (base.renderer)
		{
			base.renderer.enabled = false;
		}
		base.HandleEncounterable(player);
		DebrisObject component = base.GetComponent<DebrisObject>();
		if (component != null || this.m_forceExtant)
		{
			if (component)
			{
				UnityEngine.Object.Destroy(component);
			}
			if (base.specRigidbody)
			{
				UnityEngine.Object.Destroy(base.specRigidbody);
			}
			player.BloopItemAboveHead(base.sprite, string.Empty);
			player.AcquirePuzzleItem(this);
		}
		else
		{
			UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			player.BloopItemAboveHead(base.sprite, string.Empty);
			player.AcquirePuzzleItem(this);
		}
		GameUIRoot.Instance.UpdatePlayerConsumables(player.carriedConsumables);
	}

	// Token: 0x060075D5 RID: 30165 RVA: 0x002EEAB8 File Offset: 0x002ECCB8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.GetRidOfMinimapIcon();
	}

	// Token: 0x04007790 RID: 30608
	private static GameObject m_defaultIcon;

	// Token: 0x04007791 RID: 30609
	private bool m_pickedUp;

	// Token: 0x04007792 RID: 30610
	private GameObject minimapIcon;

	// Token: 0x04007793 RID: 30611
	private GameObject m_instanceMinimapIcon;

	// Token: 0x04007794 RID: 30612
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04007795 RID: 30613
	[NonSerialized]
	public bool IsBeingDestroyed;

	// Token: 0x04007796 RID: 30614
	private bool m_forceExtant;
}
