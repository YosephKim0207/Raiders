using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200120A RID: 4618
public class ShrineController : DungeonPlaceableBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x0600674E RID: 26446 RVA: 0x0028733C File Offset: 0x0028553C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_parentRoom = room;
		room.OptionalDoorTopDecorable = ResourceCache.Acquire("Global Prefabs/Shrine_Lantern") as GameObject;
		this.RegisterMinimapIcon();
	}

	// Token: 0x0600674F RID: 26447 RVA: 0x00287360 File Offset: 0x00285560
	public void RegisterMinimapIcon()
	{
		this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_parentRoom, (GameObject)BraveResources.Load("Global Prefabs/Minimap_Shrine_Icon", ".prefab"), false);
	}

	// Token: 0x06006750 RID: 26448 RVA: 0x00287390 File Offset: 0x00285590
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_parentRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06006751 RID: 26449 RVA: 0x002873C0 File Offset: 0x002855C0
	private void DoShrineEffect(PlayerController player)
	{
		if (this.takesCurrentGun && (player.CurrentGun == null || !player.CurrentGun.CanActuallyBeDropped(player)))
		{
			this.m_useCount--;
			this.m_parentRoom.RegisterInteractable(this);
			return;
		}
		AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
		if (this.healthToGive > 0)
		{
			AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", base.gameObject);
			player.healthHaver.ApplyHealing((float)this.healthToGive);
		}
		else if (this.healthToGive < 0)
		{
			player.healthHaver.ApplyDamage((float)(this.healthToGive * -1), Vector2.zero, StringTableManager.GetEnemiesString("#SHRINE", -1), CoreDamageTypes.None, DamageCategory.Environment, true, null, false);
		}
		if (this.armorToGive > 0)
		{
			player.healthHaver.Armor += (float)this.armorToGive;
		}
		if (this.moneyToGive > 0)
		{
			AkSoundEngine.PostEvent("Play_OBJ_item_purchase_01", base.gameObject);
			player.carriedConsumables.Currency += this.moneyToGive;
		}
		if (this.ammoPercentageToReplenish > 0)
		{
			for (int i = 0; i < player.inventory.AllGuns.Count; i++)
			{
				int num = player.inventory.AllGuns[i].AdjustedMaxAmmo * this.ammoPercentageToReplenish;
				if (num <= 0)
				{
					AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
					num = player.inventory.AllGuns[i].ammo * this.ammoPercentageToReplenish;
				}
				if (num <= 0)
				{
					Debug.LogError("Shrine is attempting to give negative ammo!");
					num = 1;
				}
				player.inventory.AllGuns[i].GainAmmo(num);
			}
		}
		if (this.takesCurrentGun && player.CurrentGun != null && player.CurrentGun.CanActuallyBeDropped(player))
		{
			player.inventory.DestroyCurrentGun();
		}
		if (this.appliesStatChanges)
		{
			for (int j = 0; j < this.statModifiers.Count; j++)
			{
				if (player.ownerlessStatModifiers == null)
				{
					player.ownerlessStatModifiers = new List<StatModifier>();
				}
				player.ownerlessStatModifiers.Add(this.statModifiers[j]);
			}
			player.stats.RecalculateStats(player, false, false);
		}
		if (this.cleansesCurse)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.amount = player.stats.GetStatValue(PlayerStats.StatType.Curse) * -1f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			player.ownerlessStatModifiers.Add(statModifier);
			player.stats.RecalculateStats(player, false, false);
		}
		if (this.onPlayerVFX != null)
		{
			player.PlayEffectOnActor(this.onPlayerVFX, this.playerVFXOffset, true, false, false);
		}
		if (base.transform.parent != null)
		{
			EncounterTrackable component = base.transform.parent.gameObject.GetComponent<EncounterTrackable>();
			if (component != null)
			{
				component.ForceDoNotification(this.m_instanceMinimapIcon.GetComponent<tk2dBaseSprite>());
			}
		}
		this.GetRidOfMinimapIcon();
	}

	// Token: 0x06006752 RID: 26450 RVA: 0x002876FC File Offset: 0x002858FC
	public float GetDistanceToPoint(Vector2 point)
	{
		if (base.sprite == null)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x06006753 RID: 26451 RVA: 0x00287754 File Offset: 0x00285954
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006754 RID: 26452 RVA: 0x0028775C File Offset: 0x0028595C
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
	}

	// Token: 0x06006755 RID: 26453 RVA: 0x00287770 File Offset: 0x00285970
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
	}

	// Token: 0x06006756 RID: 26454 RVA: 0x00287780 File Offset: 0x00285980
	private IEnumerator HandleShrineConversation(PlayerController interactor)
	{
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, StringTableManager.GetString(this.displayTextKey), true, false);
		int selectedResponse = -1;
		interactor.SetInputOverride("shrineConversation");
		yield return null;
		GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, StringTableManager.GetString(this.acceptOptionKey), StringTableManager.GetString(this.declineOptionKey));
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		interactor.ClearInputOverride("shrineConversation");
		TextBoxManager.ClearTextBox(this.talkPoint);
		if (selectedResponse == 0)
		{
			this.DoShrineEffect(interactor);
		}
		else
		{
			this.m_useCount--;
			this.m_parentRoom.RegisterInteractable(this);
		}
		yield break;
	}

	// Token: 0x06006757 RID: 26455 RVA: 0x002877A4 File Offset: 0x002859A4
	public void Interact(PlayerController interactor)
	{
		if (this.m_useCount > 0)
		{
			return;
		}
		this.m_useCount++;
		this.m_parentRoom.DeregisterInteractable(this);
		base.StartCoroutine(this.HandleShrineConversation(interactor));
	}

	// Token: 0x06006758 RID: 26456 RVA: 0x002877DC File Offset: 0x002859DC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06006759 RID: 26457 RVA: 0x002877E8 File Offset: 0x002859E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400632B RID: 25387
	public string displayTextKey;

	// Token: 0x0400632C RID: 25388
	public string acceptOptionKey;

	// Token: 0x0400632D RID: 25389
	public string declineOptionKey;

	// Token: 0x0400632E RID: 25390
	public Transform talkPoint;

	// Token: 0x0400632F RID: 25391
	public int healthToGive;

	// Token: 0x04006330 RID: 25392
	public int armorToGive;

	// Token: 0x04006331 RID: 25393
	public int moneyToGive;

	// Token: 0x04006332 RID: 25394
	public int ammoPercentageToReplenish;

	// Token: 0x04006333 RID: 25395
	public bool takesCurrentGun;

	// Token: 0x04006334 RID: 25396
	public bool appliesStatChanges;

	// Token: 0x04006335 RID: 25397
	public List<StatModifier> statModifiers;

	// Token: 0x04006336 RID: 25398
	public bool cleansesCurse;

	// Token: 0x04006337 RID: 25399
	public GameObject onPlayerVFX;

	// Token: 0x04006338 RID: 25400
	public Vector3 playerVFXOffset = Vector3.zero;

	// Token: 0x04006339 RID: 25401
	private int m_useCount;

	// Token: 0x0400633A RID: 25402
	private RoomHandler m_parentRoom;

	// Token: 0x0400633B RID: 25403
	private GameObject m_instanceMinimapIcon;
}
