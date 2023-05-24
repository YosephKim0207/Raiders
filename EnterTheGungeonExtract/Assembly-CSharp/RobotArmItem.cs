using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001496 RID: 5270
public class RobotArmItem : PickupObject
{
	// Token: 0x060077EA RID: 30698 RVA: 0x002FDF54 File Offset: 0x002FC154
	private void Start()
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnPreCollision));
		if (!this.m_pickedUp)
		{
			this.RegisterMinimapIcon();
		}
	}

	// Token: 0x060077EB RID: 30699 RVA: 0x002FDFAC File Offset: 0x002FC1AC
	public void RegisterMinimapIcon()
	{
		if (base.transform.position.y < -300f)
		{
			return;
		}
		if (this.minimapIcon == null)
		{
			GameObject gameObject = (GameObject)BraveResources.Load("Global Prefabs/Minimap_RobotArm_Icon", ".prefab");
			this.minimapIcon = gameObject;
		}
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
	}

	// Token: 0x060077EC RID: 30700 RVA: 0x002FE068 File Offset: 0x002FC268
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x060077ED RID: 30701 RVA: 0x002FE098 File Offset: 0x002FC298
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
			AkSoundEngine.PostEvent("Play_OBJ_item_pickup_01", base.gameObject);
		}
	}

	// Token: 0x060077EE RID: 30702 RVA: 0x002FE0DC File Offset: 0x002FC2DC
	public bool CheckForCombination()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			for (int j = 0; j < GameManager.Instance.AllPlayers[i].additionalItems.Count; j++)
			{
				if (GameManager.Instance.AllPlayers[i].additionalItems[j] is RobotArmBalloonsItem)
				{
					RobotArmQuestController.CombineBalloonsWithArm(GameManager.Instance.AllPlayers[i].additionalItems[j], this, GameManager.Instance.AllPlayers[i]);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060077EF RID: 30703 RVA: 0x002FE17C File Offset: 0x002FC37C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_pickedUp = true;
		this.GetRidOfMinimapIcon();
		if (this.CheckForCombination())
		{
			return;
		}
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_EVER_SEEN_ROBOT_ARM))
		{
			GameManager.BroadcastRoomFsmEvent("armPickedUp", player.CurrentRoom);
			GameStatsManager.Instance.SetFlag(GungeonFlags.META_SHOP_EVER_SEEN_ROBOT_ARM, true);
			List<PickupObject> list = new List<PickupObject>();
			list.Add(PickupObjectDatabase.GetById(GlobalItemIds.RobotBalloons));
			GameManager.Instance.Dungeon.data.DistributeComplexSecretPuzzleItems(list, null, true, 0f);
		}
		base.specRigidbody.enabled = false;
		base.renderer.enabled = false;
		base.HandleEncounterable(player);
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		DebrisObject component = base.GetComponent<DebrisObject>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
			UnityEngine.Object.Destroy(base.specRigidbody);
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

	// Token: 0x060077F0 RID: 30704 RVA: 0x002FE2B4 File Offset: 0x002FC4B4
	protected override void OnDestroy()
	{
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
		base.OnDestroy();
	}

	// Token: 0x04007A0A RID: 31242
	private bool m_pickedUp;

	// Token: 0x04007A0B RID: 31243
	private GameObject minimapIcon;

	// Token: 0x04007A0C RID: 31244
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04007A0D RID: 31245
	private GameObject m_instanceMinimapIcon;
}
