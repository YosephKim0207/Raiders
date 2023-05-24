using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020010FB RID: 4347
public class BabyDragunJailController : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x06005FD1 RID: 24529 RVA: 0x0024E488 File Offset: 0x0024C688
	private void Start()
	{
		this.m_isOpen = true;
		this.m_room = base.transform.position.GetAbsoluteRoom();
		this.m_room.RegisterInteractable(this);
	}

	// Token: 0x06005FD2 RID: 24530 RVA: 0x0024E4B4 File Offset: 0x0024C6B4
	private void Update()
	{
		if (Dungeon.IsGenerating)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i].CurrentRoom == this.m_room)
			{
				flag = true;
				break;
			}
		}
		if (flag && this.m_itemsEaten < this.RequiredItems)
		{
			for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
			{
				DebrisObject debrisObject = StaticReferenceManager.AllDebris[j];
				if (debrisObject && debrisObject.IsPickupObject && debrisObject.Static)
				{
					PickupObject componentInChildren = debrisObject.GetComponentInChildren<PickupObject>();
					if (componentInChildren && !(componentInChildren is GungeonEggItem))
					{
						this.AttemptSellItem(componentInChildren);
					}
				}
			}
			if (!this.m_currentlySellingAnItem)
			{
				for (int k = 0; k < StaticReferenceManager.AllNpcs.Count; k++)
				{
					TalkDoerLite talkDoerLite = StaticReferenceManager.AllNpcs[k];
					if (talkDoerLite && talkDoerLite.name.Contains("ResourcefulRat_Beaten"))
					{
						float magnitude = (talkDoerLite.specRigidbody.UnitCenter - this.CagedBabyDragun.WorldCenter).magnitude;
						if (magnitude < 3f)
						{
							RoomHandler.unassignedInteractableObjects.Remove(talkDoerLite);
							base.StartCoroutine(this.EatCorpse(talkDoerLite));
						}
					}
				}
			}
		}
	}

	// Token: 0x06005FD3 RID: 24531 RVA: 0x0024E640 File Offset: 0x0024C840
	private IEnumerator EatCorpse(TalkDoerLite targetCorpse)
	{
		float elapsed = 0f;
		float duration = 0.5f;
		Vector3 startPos = targetCorpse.transform.position;
		Vector3 finalOffset = this.CagedBabyDragun.WorldCenter - startPos.XY();
		tk2dBaseSprite targetSprite = targetCorpse.GetComponentInChildren<tk2dBaseSprite>();
		UnityEngine.Object.Destroy(targetCorpse);
		UnityEngine.Object.Destroy(targetCorpse.specRigidbody);
		this.CagedBabyDragun.spriteAnimator.PlayForDuration("baby_dragun_weak_eat", -1f, "baby_dragun_weak_idle", false);
		AkSoundEngine.PostEvent("Play_NPC_BabyDragun_Munch_01", base.gameObject);
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (!targetSprite || !targetSprite.transform)
			{
				this.m_currentlySellingAnItem = false;
				yield break;
			}
			targetSprite.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.01f, 0.01f, 1f), elapsed / duration);
			targetSprite.transform.position = Vector3.Lerp(startPos, startPos + finalOffset, elapsed / duration);
			yield return null;
		}
		if (!targetSprite || !targetSprite.transform)
		{
			this.m_currentlySellingAnItem = false;
			yield break;
		}
		UnityEngine.Object.Destroy(targetSprite.gameObject);
		this.m_itemsEaten++;
		if (this.m_itemsEaten >= this.RequiredItems)
		{
			while (this.CagedBabyDragun.spriteAnimator.IsPlaying("baby_dragun_weak_eat"))
			{
				yield return null;
			}
			LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(this.ItemID).gameObject, GameManager.Instance.BestActivePlayer);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x06005FD4 RID: 24532 RVA: 0x0024E664 File Offset: 0x0024C864
	public void AttemptSellItem(PickupObject targetItem)
	{
		if (targetItem == null)
		{
			return;
		}
		if (!targetItem.CanBeSold)
		{
			return;
		}
		if (targetItem.IsBeingSold)
		{
			return;
		}
		if (targetItem is CurrencyPickup || targetItem is KeyBulletPickup || targetItem is HealthPickup)
		{
			return;
		}
		if (this.m_itemsEaten >= this.RequiredItems)
		{
			return;
		}
		if (this.m_currentlySellingAnItem)
		{
			return;
		}
		if (this.SellRegionRigidbody.ContainsPoint(targetItem.sprite.WorldCenter, 2147483647, true))
		{
			base.StartCoroutine(this.HandleSoldItem(targetItem));
		}
	}

	// Token: 0x06005FD5 RID: 24533 RVA: 0x0024E708 File Offset: 0x0024C908
	private IEnumerator HandleSoldItem(PickupObject targetItem)
	{
		targetItem.IsBeingSold = true;
		while (this.m_currentlySellingAnItem)
		{
			yield return null;
		}
		if (this.m_itemsEaten >= this.RequiredItems)
		{
			yield break;
		}
		if (!targetItem)
		{
			yield break;
		}
		if (!targetItem.sprite || !this.SellRegionRigidbody.ContainsPoint(targetItem.sprite.WorldCenter, 2147483647, true))
		{
			yield break;
		}
		this.m_currentlySellingAnItem = true;
		IPlayerInteractable ixable = null;
		if (targetItem is PassiveItem)
		{
			PassiveItem passiveItem = targetItem as PassiveItem;
			passiveItem.GetRidOfMinimapIcon();
			ixable = targetItem as PassiveItem;
		}
		else if (targetItem is Gun)
		{
			Gun gun = targetItem as Gun;
			gun.GetRidOfMinimapIcon();
			ixable = targetItem as Gun;
		}
		else if (targetItem is PlayerItem)
		{
			PlayerItem playerItem = targetItem as PlayerItem;
			playerItem.GetRidOfMinimapIcon();
			ixable = targetItem as PlayerItem;
		}
		if (ixable != null)
		{
			RoomHandler.unassignedInteractableObjects.Remove(ixable);
			GameManager.Instance.PrimaryPlayer.RemoveBrokenInteractable(ixable);
		}
		float elapsed = 0f;
		float duration = 0.5f;
		Vector3 startPos = targetItem.transform.position;
		Vector3 finalOffset = this.CagedBabyDragun.WorldCenter - startPos.XY();
		tk2dBaseSprite targetSprite = targetItem.GetComponentInChildren<tk2dBaseSprite>();
		this.CagedBabyDragun.spriteAnimator.PlayForDuration("baby_dragun_weak_eat", -1f, "baby_dragun_weak_idle", false);
		AkSoundEngine.PostEvent("Play_NPC_BabyDragun_Munch_01", base.gameObject);
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (!targetItem || !targetItem.transform)
			{
				this.m_currentlySellingAnItem = false;
				yield break;
			}
			targetItem.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.01f, 0.01f, 1f), elapsed / duration);
			targetItem.transform.position = Vector3.Lerp(startPos, startPos + finalOffset, elapsed / duration);
			yield return null;
		}
		if (!targetItem || !targetItem.transform)
		{
			this.m_currentlySellingAnItem = false;
			yield break;
		}
		this.m_itemsEaten++;
		if (this.m_itemsEaten >= this.RequiredItems)
		{
			while (this.CagedBabyDragun.spriteAnimator.IsPlaying("baby_dragun_weak_eat"))
			{
				yield return null;
			}
			LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(this.ItemID).gameObject, GameManager.Instance.BestActivePlayer);
			LootEngine.DoDefaultPurplePoof(this.CagedBabyDragun.WorldCenter, false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (targetItem is Gun && targetItem.GetComponentInParent<DebrisObject>())
		{
			UnityEngine.Object.Destroy(targetItem.transform.parent.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(targetItem.gameObject);
		}
		this.m_currentlySellingAnItem = false;
		yield break;
	}

	// Token: 0x06005FD6 RID: 24534 RVA: 0x0024E72C File Offset: 0x0024C92C
	private void Talk(PlayerController interactor)
	{
		string text = ((this.m_itemsEaten != 0) ? "#BABYDRAGUN_FED_ONCE" : "#BABYDRAGUN_UNFED");
		TextBoxManager.ShowThoughtBubble(interactor.sprite.WorldTopCenter + new Vector2(0f, 0.5f), interactor.transform, 3f, StringTableManager.GetString(text), true, false, string.Empty);
	}

	// Token: 0x06005FD7 RID: 24535 RVA: 0x0024E798 File Offset: 0x0024C998
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this.m_isOpen)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, this.CagedBabyDragun.WorldBottomLeft, this.CagedBabyDragun.WorldTopRight - this.CagedBabyDragun.WorldBottomLeft);
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x06005FD8 RID: 24536 RVA: 0x0024E7FC File Offset: 0x0024C9FC
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.AddOutlineToSprite(this.CagedBabyDragun, Color.white);
	}

	// Token: 0x06005FD9 RID: 24537 RVA: 0x0024E810 File Offset: 0x0024CA10
	public void OnExitRange(PlayerController interactor)
	{
		if (SpriteOutlineManager.HasOutline(this.CagedBabyDragun))
		{
			TextBoxManager.ClearTextBox(interactor.transform);
			SpriteOutlineManager.RemoveOutlineFromSprite(this.CagedBabyDragun, false);
		}
	}

	// Token: 0x06005FDA RID: 24538 RVA: 0x0024E83C File Offset: 0x0024CA3C
	public void Interact(PlayerController interactor)
	{
		this.Talk(interactor);
	}

	// Token: 0x06005FDB RID: 24539 RVA: 0x0024E848 File Offset: 0x0024CA48
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06005FDC RID: 24540 RVA: 0x0024E854 File Offset: 0x0024CA54
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06005FDD RID: 24541 RVA: 0x0024E85C File Offset: 0x0024CA5C
	protected override void OnDestroy()
	{
		this.m_room.DeregisterInteractable(this);
		base.OnDestroy();
	}

	// Token: 0x04005A4E RID: 23118
	public tk2dSprite CagedBabyDragun;

	// Token: 0x04005A4F RID: 23119
	public Transform CagedBabyDragunTalkPoint;

	// Token: 0x04005A50 RID: 23120
	public SpeculativeRigidbody SellRegionRigidbody;

	// Token: 0x04005A51 RID: 23121
	public int RequiredItems = 2;

	// Token: 0x04005A52 RID: 23122
	[PickupIdentifier]
	public int ItemID;

	// Token: 0x04005A53 RID: 23123
	private bool m_isOpen;

	// Token: 0x04005A54 RID: 23124
	private RoomHandler m_room;

	// Token: 0x04005A55 RID: 23125
	private int m_itemsEaten;

	// Token: 0x04005A56 RID: 23126
	private bool m_currentlySellingAnItem;
}
