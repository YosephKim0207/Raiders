using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001553 RID: 5459
public class SellCellController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06007D02 RID: 32002 RVA: 0x00326DA4 File Offset: 0x00324FA4
	private void Start()
	{
		if (this.SellPitDweller && this.SellPitDweller.spriteAnimator)
		{
			this.SellPitDweller.spriteAnimator.alwaysUpdateOffscreen = true;
		}
	}

	// Token: 0x06007D03 RID: 32003 RVA: 0x00326DDC File Offset: 0x00324FDC
	public void AttemptSellItem(PickupObject targetItem)
	{
		if (this.m_isExploded)
		{
			return;
		}
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
		if (base.specRigidbody.ContainsPoint(targetItem.sprite.WorldCenter, 2147483647, true))
		{
			base.StartCoroutine(this.HandleSoldItem(targetItem));
		}
	}

	// Token: 0x06007D04 RID: 32004 RVA: 0x00326E6C File Offset: 0x0032506C
	private void HandleFlightCollider()
	{
		if (!GameManager.Instance.IsLoadingLevel && this.m_isExploded)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController && !playerController.IsGhost && playerController.IsFlying)
				{
					Rect rect = new Rect(base.transform.position.XY(), new Vector2(3f, 3f));
					if (rect.Contains(playerController.CenterPosition))
					{
						this.m_timeHovering += BraveTime.DeltaTime;
						if (this.m_timeHovering > 2f)
						{
							playerController.ForceFall();
							this.m_timeHovering = 0f;
						}
					}
				}
			}
		}
	}

	// Token: 0x06007D05 RID: 32005 RVA: 0x00326F4C File Offset: 0x0032514C
	private IEnumerator HandleSellPitOpening()
	{
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
		{
			yield break;
		}
		this.m_isExploded = true;
		this.SellPitDweller.PreventInteraction = true;
		this.SellPitDweller.PreventCoopInteraction = true;
		this.SellPitDweller.playerApproachRadius = -1f;
		yield return new WaitForSeconds(3f);
		UnityEngine.Object.Instantiate<GameObject>(this.SellExplosionVFX, base.transform.position, Quaternion.identity);
		float elapsed = 0f;
		while (elapsed < 0.25f)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		this.CellTopSprite.SetSprite(this.ExplodedSellSpriteName);
		for (int i = 1; i < this.GetWidth(); i++)
		{
			for (int j = 0; j < this.GetHeight(); j++)
			{
				IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round) + new IntVector2(i, j);
				if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
				{
					CellData cellData = GameManager.Instance.Dungeon.data[intVector];
					cellData.fallingPrevented = false;
				}
			}
		}
		yield break;
	}

	// Token: 0x06007D06 RID: 32006 RVA: 0x00326F68 File Offset: 0x00325168
	private void OnDisable()
	{
		if (this.m_isExploded && this.CellTopSprite.CurrentSprite.name != this.ExplodedSellSpriteName)
		{
			this.CellTopSprite.SetSprite(this.ExplodedSellSpriteName);
			for (int i = 1; i < this.GetWidth(); i++)
			{
				for (int j = 0; j < this.GetHeight(); j++)
				{
					IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round) + new IntVector2(i, j);
					if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
					{
						CellData cellData = GameManager.Instance.Dungeon.data[intVector];
						cellData.fallingPrevented = false;
					}
				}
			}
		}
	}

	// Token: 0x06007D07 RID: 32007 RVA: 0x00327038 File Offset: 0x00325238
	private IEnumerator HandleSoldItem(PickupObject targetItem)
	{
		targetItem.IsBeingSold = true;
		while (this.m_currentlySellingAnItem)
		{
			yield return null;
		}
		if (!targetItem)
		{
			yield break;
		}
		if (!targetItem.sprite || !base.specRigidbody.ContainsPoint(targetItem.sprite.WorldCenter, 2147483647, true))
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
		Vector3 finalOffset = Vector3.zero;
		tk2dBaseSprite targetSprite = targetItem.GetComponentInChildren<tk2dBaseSprite>();
		if (targetSprite)
		{
			finalOffset = targetSprite.GetBounds().extents;
		}
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (!targetItem || !targetItem.transform)
			{
				this.m_currentlySellingAnItem = false;
				yield break;
			}
			targetItem.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.01f, 0.01f, 1f), elapsed / duration);
			targetItem.transform.position = Vector3.Lerp(startPos, startPos + new Vector3(finalOffset.x, 0f, 0f), elapsed / duration);
			yield return null;
		}
		if (!targetItem || !targetItem.transform)
		{
			this.m_currentlySellingAnItem = false;
			yield break;
		}
		this.SellPitDweller.SendPlaymakerEvent("playerSoldSomething");
		int sellPrice = Mathf.Clamp(Mathf.CeilToInt((float)targetItem.PurchasePrice * this.SellValueModifier), 0, 200);
		if (targetItem.quality == PickupObject.ItemQuality.SPECIAL || targetItem.quality == PickupObject.ItemQuality.EXCLUDED)
		{
			sellPrice = 3;
		}
		LootEngine.SpawnCurrency(targetItem.sprite.WorldCenter, sellPrice, false);
		this.m_thingsSold++;
		if (targetItem.PickupObjectId == GlobalItemIds.MasteryToken_Castle || targetItem.PickupObjectId == GlobalItemIds.MasteryToken_Catacombs || targetItem.PickupObjectId == GlobalItemIds.MasteryToken_Gungeon || targetItem.PickupObjectId == GlobalItemIds.MasteryToken_Forge || targetItem.PickupObjectId == GlobalItemIds.MasteryToken_Mines)
		{
			this.m_masteryRoundsSold++;
		}
		if (targetItem is Gun && targetItem.GetComponentInParent<DebrisObject>())
		{
			UnityEngine.Object.Destroy(targetItem.transform.parent.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(targetItem.gameObject);
		}
		if (this.m_thingsSold >= 3 && this.m_masteryRoundsSold > 0)
		{
			base.StartCoroutine(this.HandleSellPitOpening());
		}
		this.m_currentlySellingAnItem = false;
		yield break;
	}

	// Token: 0x06007D08 RID: 32008 RVA: 0x0032705C File Offset: 0x0032525C
	private void Update()
	{
		this.HandleFlightCollider();
	}

	// Token: 0x06007D09 RID: 32009 RVA: 0x00327064 File Offset: 0x00325264
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06007D0A RID: 32010 RVA: 0x0032706C File Offset: 0x0032526C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		for (int i = 1; i < this.GetWidth(); i++)
		{
			for (int j = 0; j < this.GetHeight(); j++)
			{
				IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round) + new IntVector2(i, j);
				if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
				{
					CellData cellData = GameManager.Instance.Dungeon.data[intVector];
					cellData.type = CellType.PIT;
					cellData.fallingPrevented = true;
				}
			}
		}
	}

	// Token: 0x04008007 RID: 32775
	public float SellValueModifier = 0.1f;

	// Token: 0x04008008 RID: 32776
	public TalkDoerLite SellPitDweller;

	// Token: 0x04008009 RID: 32777
	public GameObject SellExplosionVFX;

	// Token: 0x0400800A RID: 32778
	public tk2dSprite CellTopSprite;

	// Token: 0x0400800B RID: 32779
	public string ExplodedSellSpriteName;

	// Token: 0x0400800C RID: 32780
	private bool m_isExploded;

	// Token: 0x0400800D RID: 32781
	private int m_thingsSold;

	// Token: 0x0400800E RID: 32782
	private int m_masteryRoundsSold;

	// Token: 0x0400800F RID: 32783
	private bool m_currentlySellingAnItem;

	// Token: 0x04008010 RID: 32784
	private float m_timeHovering;
}
