using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001495 RID: 5269
public class RobotArmBalloonsItem : PickupObject
{
	// Token: 0x060077E1 RID: 30689 RVA: 0x002FDC28 File Offset: 0x002FBE28
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

	// Token: 0x060077E2 RID: 30690 RVA: 0x002FDC80 File Offset: 0x002FBE80
	public void RegisterMinimapIcon()
	{
		if (base.transform.position.y < -300f)
		{
			return;
		}
		if (this.minimapIcon == null)
		{
			GameObject gameObject = (GameObject)BraveResources.Load("Global Prefabs/Minimap_RobotBalloon_Icon", ".prefab");
			this.minimapIcon = gameObject;
		}
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
	}

	// Token: 0x060077E3 RID: 30691 RVA: 0x002FDD3C File Offset: 0x002FBF3C
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x060077E4 RID: 30692 RVA: 0x002FDD6C File Offset: 0x002FBF6C
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

	// Token: 0x060077E5 RID: 30693 RVA: 0x002FDDB0 File Offset: 0x002FBFB0
	public bool CheckForCombination()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			for (int j = 0; j < GameManager.Instance.AllPlayers[i].additionalItems.Count; j++)
			{
				if (GameManager.Instance.AllPlayers[i].additionalItems[j] is RobotArmItem)
				{
					RobotArmQuestController.CombineBalloonsWithArm(this, GameManager.Instance.AllPlayers[i].additionalItems[j], GameManager.Instance.AllPlayers[i]);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060077E6 RID: 30694 RVA: 0x002FDE50 File Offset: 0x002FC050
	public void AttachBalloonToGameActor(GameActor target)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BalloonAttachPrefab.gameObject);
		BalloonAttachmentDoer component = gameObject.GetComponent<BalloonAttachmentDoer>();
		component.Initialize(target);
	}

	// Token: 0x060077E7 RID: 30695 RVA: 0x002FDE7C File Offset: 0x002FC07C
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
		base.specRigidbody.enabled = false;
		base.renderer.enabled = false;
		base.HandleEncounterable(player);
		this.AttachBalloonToGameActor(player);
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		DebrisObject component = base.GetComponent<DebrisObject>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
			UnityEngine.Object.Destroy(base.specRigidbody);
			player.AcquirePuzzleItem(this);
		}
		else
		{
			UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			player.AcquirePuzzleItem(this);
		}
		GameUIRoot.Instance.UpdatePlayerConsumables(player.carriedConsumables);
	}

	// Token: 0x060077E8 RID: 30696 RVA: 0x002FDF34 File Offset: 0x002FC134
	protected override void OnDestroy()
	{
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
		base.OnDestroy();
	}

	// Token: 0x04007A05 RID: 31237
	public BalloonAttachmentDoer BalloonAttachPrefab;

	// Token: 0x04007A06 RID: 31238
	private bool m_pickedUp;

	// Token: 0x04007A07 RID: 31239
	private GameObject minimapIcon;

	// Token: 0x04007A08 RID: 31240
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04007A09 RID: 31241
	private GameObject m_instanceMinimapIcon;
}
