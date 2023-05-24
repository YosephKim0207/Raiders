using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020010FF RID: 4351
public class BeholsterShrineController : DungeonPlaceableBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x06005FEC RID: 24556 RVA: 0x0024F078 File Offset: 0x0024D278
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		this.m_room.OptionalDoorTopDecorable = ResourceCache.Acquire("Global Prefabs/Shrine_Lantern") as GameObject;
		this.UpdateSpriteVisibility();
	}

	// Token: 0x06005FED RID: 24557 RVA: 0x0024F0A4 File Offset: 0x0024D2A4
	private void UpdateSpriteVisibility()
	{
		this.UpdateSingleSpriteVisibility(this.Gun01Sprite, GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_01));
		this.UpdateSingleSpriteVisibility(this.Gun02Sprite, GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_02));
		this.UpdateSingleSpriteVisibility(this.Gun03Sprite, GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_03));
		this.UpdateSingleSpriteVisibility(this.Gun04Sprite, GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_04));
		this.UpdateSingleSpriteVisibility(this.Gun05Sprite, GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_05));
		this.UpdateSingleSpriteVisibility(this.Gun06Sprite, GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_06));
	}

	// Token: 0x06005FEE RID: 24558 RVA: 0x0024F154 File Offset: 0x0024D354
	private void UpdateSingleSpriteVisibility(tk2dSprite gunSprite, bool visibility)
	{
		if (gunSprite.renderer.enabled != visibility)
		{
			gunSprite.renderer.enabled = visibility;
			if (this.VFXStonePuff)
			{
				GameObject gameObject = SpawnManager.SpawnVFX(this.VFXStonePuff, gunSprite.transform.position, Quaternion.identity);
				tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
				component.HeightOffGround = 10f;
				component.UpdateZDepth();
				AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", base.gameObject);
			}
		}
	}

	// Token: 0x06005FEF RID: 24559 RVA: 0x0024F1D4 File Offset: 0x0024D3D4
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06005FF0 RID: 24560 RVA: 0x0024F1E0 File Offset: 0x0024D3E0
	public float GetDistanceToPoint(Vector2 point)
	{
		if (base.sprite == null)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x06005FF1 RID: 24561 RVA: 0x0024F238 File Offset: 0x0024D438
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06005FF2 RID: 24562 RVA: 0x0024F240 File Offset: 0x0024D440
	public void OnEnteredRange(PlayerController interactor)
	{
		if (this.AlternativeOutlineTarget != null)
		{
			SpriteOutlineManager.AddOutlineToSprite(this.AlternativeOutlineTarget, Color.white);
		}
		else
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		}
	}

	// Token: 0x06005FF3 RID: 24563 RVA: 0x0024F278 File Offset: 0x0024D478
	public void OnExitRange(PlayerController interactor)
	{
		if (this.AlternativeOutlineTarget != null)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternativeOutlineTarget, false);
		}
		else
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		}
	}

	// Token: 0x06005FF4 RID: 24564 RVA: 0x0024F2A8 File Offset: 0x0024D4A8
	private bool NeedsGun(int pickupID)
	{
		return (pickupID == this.Gun01ID && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_01)) || (pickupID == this.Gun02ID && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_02)) || (pickupID == this.Gun03ID && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_03)) || (pickupID == this.Gun04ID && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_04)) || (pickupID == this.Gun05ID && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_05)) || (pickupID == this.Gun06ID && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_06));
	}

	// Token: 0x06005FF5 RID: 24565 RVA: 0x0024F384 File Offset: 0x0024D584
	private bool CheckCanBeUsed(PlayerController interactor)
	{
		return interactor && interactor.CurrentGun && this.m_useCount <= 10 && this.NeedsGun(interactor.CurrentGun.PickupObjectId);
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x0024F3C4 File Offset: 0x0024D5C4
	private void SetFlagForID(int id)
	{
		if (id == this.Gun01ID)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_01, true);
		}
		if (id == this.Gun02ID)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_02, true);
		}
		if (id == this.Gun03ID)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_03, true);
		}
		if (id == this.Gun04ID)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_04, true);
		}
		if (id == this.Gun05ID)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_05, true);
		}
		if (id == this.Gun06ID)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_06, true);
		}
		this.UpdateSpriteVisibility();
	}

	// Token: 0x06005FF7 RID: 24567 RVA: 0x0024F480 File Offset: 0x0024D680
	private void DoShrineEffect(PlayerController interactor)
	{
		this.SetFlagForID(interactor.CurrentGun.PickupObjectId);
		int num = 0;
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_01))
		{
			num++;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_02))
		{
			num++;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_03))
		{
			num++;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_04))
		{
			num++;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_05))
		{
			num++;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_06))
		{
			num++;
		}
		if (num == 6)
		{
			LootEngine.TryGiveGunToPlayer(PickupObjectDatabase.GetById(this.Gun01ID).gameObject, interactor, false);
			LootEngine.TryGiveGunToPlayer(PickupObjectDatabase.GetById(this.Gun02ID).gameObject, interactor, false);
			LootEngine.TryGiveGunToPlayer(PickupObjectDatabase.GetById(this.Gun03ID).gameObject, interactor, false);
			LootEngine.TryGiveGunToPlayer(PickupObjectDatabase.GetById(this.Gun04ID).gameObject, interactor, false);
			LootEngine.TryGiveGunToPlayer(PickupObjectDatabase.GetById(this.Gun05ID).gameObject, interactor, false);
			LootEngine.TryGiveGunToPlayer(PickupObjectDatabase.GetById(this.Gun06ID).gameObject, interactor, false);
			base.StartCoroutine(this.HandleShrineCompletionVisuals());
			this.m_useCount = 100;
			interactor.inventory.GunChangeForgiveness = true;
			for (int i = 0; i < 100; i++)
			{
				Gun targetGunWithChange = interactor.inventory.GetTargetGunWithChange(i);
				if (targetGunWithChange.PickupObjectId == this.Gun01ID)
				{
					if (i != 0)
					{
						interactor.inventory.ChangeGun(i, false, false);
					}
					break;
				}
			}
			interactor.inventory.GunChangeForgiveness = false;
		}
		else
		{
			interactor.inventory.DestroyCurrentGun();
		}
	}

	// Token: 0x06005FF8 RID: 24568 RVA: 0x0024F650 File Offset: 0x0024D850
	private IEnumerator HandleShrineCompletionVisuals()
	{
		AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
		yield return new WaitForSeconds(0.5f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_01, false);
		this.UpdateSpriteVisibility();
		yield return new WaitForSeconds(0.2f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_02, false);
		this.UpdateSpriteVisibility();
		yield return new WaitForSeconds(0.2f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_03, false);
		this.UpdateSpriteVisibility();
		yield return new WaitForSeconds(0.2f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_04, false);
		this.UpdateSpriteVisibility();
		yield return new WaitForSeconds(0.2f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_05, false);
		this.UpdateSpriteVisibility();
		yield return new WaitForSeconds(0.2f);
		GameStatsManager.Instance.SetFlag(GungeonFlags.SHRINE_BEHOLSTER_GUN_06, false);
		this.UpdateSpriteVisibility();
		yield break;
	}

	// Token: 0x06005FF9 RID: 24569 RVA: 0x0024F66C File Offset: 0x0024D86C
	private IEnumerator HandleShrineConversation(PlayerController interactor)
	{
		string targetDisplayKey = this.displayTextKey;
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, StringTableManager.GetLongString(targetDisplayKey), true, false);
		int selectedResponse = -1;
		interactor.SetInputOverride("shrineConversation");
		yield return null;
		bool canUse = this.CheckCanBeUsed(interactor);
		if (canUse)
		{
			string @string = StringTableManager.GetString(this.acceptOptionKey);
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, @string, StringTableManager.GetString(this.declineOptionKey));
		}
		else
		{
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, StringTableManager.GetString(this.declineOptionKey), string.Empty);
		}
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		interactor.ClearInputOverride("shrineConversation");
		TextBoxManager.ClearTextBox(this.talkPoint);
		if (canUse && selectedResponse == 0)
		{
			this.DoShrineEffect(interactor);
		}
		this.ResetForReuse();
		yield break;
	}

	// Token: 0x06005FFA RID: 24570 RVA: 0x0024F690 File Offset: 0x0024D890
	private void ResetForReuse()
	{
		this.m_useCount--;
	}

	// Token: 0x06005FFB RID: 24571 RVA: 0x0024F6A0 File Offset: 0x0024D8A0
	private IEnumerator HandleSpentText(PlayerController interactor)
	{
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, StringTableManager.GetLongString(this.spentOptionKey), true, false);
		int selectedResponse = -1;
		interactor.SetInputOverride("shrineConversation");
		GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, StringTableManager.GetString(this.declineOptionKey), string.Empty);
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		interactor.ClearInputOverride("shrineConversation");
		TextBoxManager.ClearTextBox(this.talkPoint);
		yield break;
	}

	// Token: 0x06005FFC RID: 24572 RVA: 0x0024F6C4 File Offset: 0x0024D8C4
	public void Interact(PlayerController interactor)
	{
		if (TextBoxManager.HasTextBox(this.talkPoint))
		{
			return;
		}
		if (this.m_useCount > 0)
		{
			if (!string.IsNullOrEmpty(this.spentOptionKey))
			{
				base.StartCoroutine(this.HandleSpentText(interactor));
			}
			return;
		}
		this.m_useCount++;
		base.StartCoroutine(this.HandleShrineConversation(interactor));
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x0024F72C File Offset: 0x0024D92C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005A6C RID: 23148
	public string displayTextKey;

	// Token: 0x04005A6D RID: 23149
	public string acceptOptionKey;

	// Token: 0x04005A6E RID: 23150
	public string declineOptionKey;

	// Token: 0x04005A6F RID: 23151
	public string spentOptionKey = "#SHRINE_GENERIC_SPENT";

	// Token: 0x04005A70 RID: 23152
	public Transform talkPoint;

	// Token: 0x04005A71 RID: 23153
	public tk2dSprite AlternativeOutlineTarget;

	// Token: 0x04005A72 RID: 23154
	[PickupIdentifier]
	public int Gun01ID;

	// Token: 0x04005A73 RID: 23155
	[PickupIdentifier]
	public int Gun02ID;

	// Token: 0x04005A74 RID: 23156
	[PickupIdentifier]
	public int Gun03ID;

	// Token: 0x04005A75 RID: 23157
	[PickupIdentifier]
	public int Gun04ID;

	// Token: 0x04005A76 RID: 23158
	[PickupIdentifier]
	public int Gun05ID;

	// Token: 0x04005A77 RID: 23159
	[PickupIdentifier]
	public int Gun06ID;

	// Token: 0x04005A78 RID: 23160
	public tk2dSprite Gun01Sprite;

	// Token: 0x04005A79 RID: 23161
	public tk2dSprite Gun02Sprite;

	// Token: 0x04005A7A RID: 23162
	public tk2dSprite Gun03Sprite;

	// Token: 0x04005A7B RID: 23163
	public tk2dSprite Gun04Sprite;

	// Token: 0x04005A7C RID: 23164
	public tk2dSprite Gun05Sprite;

	// Token: 0x04005A7D RID: 23165
	public tk2dSprite Gun06Sprite;

	// Token: 0x04005A7E RID: 23166
	public GameObject VFXStonePuff;

	// Token: 0x04005A7F RID: 23167
	private RoomHandler m_room;

	// Token: 0x04005A80 RID: 23168
	private int m_useCount;
}
