using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020015BE RID: 5566
public abstract class PickupObject : BraveBehaviour
{
	// Token: 0x06007FDC RID: 32732 RVA: 0x0033A770 File Offset: 0x00338970
	public bool CanActuallyBeDropped(PlayerController owner)
	{
		if (!this.CanBeDropped)
		{
			return false;
		}
		if (this is Gun && owner.CurrentGun == this && owner.inventory.GunLocked.Value)
		{
			return false;
		}
		if (owner)
		{
			for (int i = 0; i < owner.startingGunIds.Count; i++)
			{
				if (owner.startingGunIds[i] == this.PickupObjectId)
				{
					return !this.PreventStartingOwnerFromDropping;
				}
			}
			for (int j = 0; j < owner.startingAlternateGunIds.Count; j++)
			{
				if (owner.startingAlternateGunIds[j] == this.PickupObjectId)
				{
					return !this.PreventStartingOwnerFromDropping;
				}
			}
			for (int k = 0; k < owner.startingPassiveItemIds.Count; k++)
			{
				if (owner.startingPassiveItemIds[k] == this.PickupObjectId)
				{
					return !this.PreventStartingOwnerFromDropping;
				}
			}
			for (int l = 0; l < owner.startingActiveItemIds.Count; l++)
			{
				if (owner.startingActiveItemIds[l] == this.PickupObjectId)
				{
					return !this.PreventStartingOwnerFromDropping;
				}
			}
		}
		return true;
	}

	// Token: 0x170012F5 RID: 4853
	// (get) Token: 0x06007FDD RID: 32733 RVA: 0x0033A8C0 File Offset: 0x00338AC0
	public virtual string DisplayName
	{
		get
		{
			return this.itemName;
		}
	}

	// Token: 0x170012F6 RID: 4854
	// (get) Token: 0x06007FDE RID: 32734 RVA: 0x0033A8C8 File Offset: 0x00338AC8
	public string EncounterNameOrDisplayName
	{
		get
		{
			EncounterTrackable component = base.GetComponent<EncounterTrackable>();
			if (component)
			{
				return component.GetModifiedDisplayName();
			}
			return this.itemName;
		}
	}

	// Token: 0x170012F7 RID: 4855
	// (get) Token: 0x06007FDF RID: 32735 RVA: 0x0033A8F4 File Offset: 0x00338AF4
	public int PurchasePrice
	{
		get
		{
			return (!this.UsesCustomCost) ? GlobalDungeonData.GetBasePrice(this.quality) : this.CustomCost;
		}
	}

	// Token: 0x06007FE0 RID: 32736 RVA: 0x0033A918 File Offset: 0x00338B18
	public bool PrerequisitesMet()
	{
		if (this.quality == PickupObject.ItemQuality.EXCLUDED)
		{
			return false;
		}
		EncounterTrackable component = base.GetComponent<EncounterTrackable>();
		return component == null || component.PrerequisitesMet();
	}

	// Token: 0x06007FE1 RID: 32737 RVA: 0x0033A950 File Offset: 0x00338B50
	public virtual bool ShouldBeDestroyedOnExistence(bool isForEnemyInventory = false)
	{
		return false;
	}

	// Token: 0x06007FE2 RID: 32738 RVA: 0x0033A954 File Offset: 0x00338B54
	protected void HandleEncounterable(PlayerController player)
	{
		EncounterTrackable component = base.GetComponent<EncounterTrackable>();
		if (component != null)
		{
			component.HandleEncounter();
			if (this && this.PickupObjectId == GlobalItemIds.FinishedGun)
			{
				GameStatsManager.Instance.SingleIncrementDifferentiator(PickupObjectDatabase.GetById(GlobalItemIds.UnfinishedGun).encounterTrackable);
			}
		}
		if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			this.HandleMagnficence();
		}
		if (this.SaveFlagToSetOnAcquisition != GungeonFlags.NONE)
		{
			GameStatsManager.Instance.SetFlag(this.SaveFlagToSetOnAcquisition, true);
		}
	}

	// Token: 0x06007FE3 RID: 32739 RVA: 0x0033A9E0 File Offset: 0x00338BE0
	protected void HandleMagnficence()
	{
		GameManager.Instance.PrimaryPlayer.stats.AddFloorMagnificence(this.additionalMagnificenceModifier);
		if (!this.ItemRespectsHeartMagnificence)
		{
			switch (this.quality)
			{
			case PickupObject.ItemQuality.COMMON:
				GameManager.Instance.PrimaryPlayer.stats.AddFloorMagnificence(0f);
				break;
			case PickupObject.ItemQuality.A:
				GameManager.Instance.PrimaryPlayer.stats.AddFloorMagnificence(1f);
				break;
			case PickupObject.ItemQuality.S:
				GameManager.Instance.PrimaryPlayer.stats.AddFloorMagnificence(1f);
				break;
			}
		}
	}

	// Token: 0x06007FE4 RID: 32740 RVA: 0x0033AAA0 File Offset: 0x00338CA0
	protected void HandleLootMods(PlayerController player)
	{
		if (this.associatedItemChanceMods == null)
		{
			return;
		}
		for (int i = 0; i < this.associatedItemChanceMods.Length; i++)
		{
			player.lootModData.Add(this.associatedItemChanceMods[i]);
		}
	}

	// Token: 0x06007FE5 RID: 32741
	public abstract void Pickup(PlayerController player);

	// Token: 0x06007FE6 RID: 32742 RVA: 0x0033AAE8 File Offset: 0x00338CE8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06007FE7 RID: 32743 RVA: 0x0033AAF0 File Offset: 0x00338CF0
	protected bool ShouldBeTakenByRat(Vector2 point)
	{
		if (GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating)
		{
			return false;
		}
		if (!base.gameObject.activeSelf)
		{
			return false;
		}
		if (this.IgnoredByRat)
		{
			return false;
		}
		if (this is NotePassiveItem)
		{
			return false;
		}
		if (this is AmmoPickup && base.transform.position.GetAbsoluteRoom().IsSecretRoom)
		{
			return false;
		}
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
		{
			return false;
		}
		if (PickupObject.RatBeatenAtPunchout && !PassiveItem.IsFlagSetAtAll(typeof(RingOfResourcefulRatItem)))
		{
			return false;
		}
		if (base.transform.position == Vector3.zero)
		{
			return false;
		}
		if (PickupObject.ItemIsBeingTakenByRat)
		{
			return false;
		}
		if (GameManager.Instance.AllPlayers != null)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i].PlayerIsRatTransformed)
				{
					return false;
				}
				if (Vector2.Distance(point, GameManager.Instance.AllPlayers[i].CenterPosition) < 10f)
				{
					return false;
				}
			}
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.TUTORIAL)
		{
			return false;
		}
		if ((GameManager.Instance.PrimaryPlayer == null || GameManager.Instance.PrimaryPlayer.healthHaver.IsDead) && (GameManager.Instance.SecondaryPlayer == null || GameManager.Instance.SecondaryPlayer.healthHaver.IsDead))
		{
			return false;
		}
		if (base.encounterTrackable && base.encounterTrackable.UsesPurpleNotifications)
		{
			return false;
		}
		if (this is SilencerItem)
		{
			return false;
		}
		if (this is RobotUnlockTelevisionItem || this.m_numberTimesRatTheftAttempted == 0)
		{
			RoomHandler currentRoom = GameManager.Instance.GetPlayerClosestToPoint(point).CurrentRoom;
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
			return currentRoom != absoluteRoomFromPosition;
		}
		RoomHandler currentRoom2 = GameManager.Instance.GetPlayerClosestToPoint(point).CurrentRoom;
		RoomHandler absoluteRoomFromPosition2 = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		return currentRoom2 != absoluteRoomFromPosition2 && !currentRoom2.connectedRooms.Contains(absoluteRoomFromPosition2);
	}

	// Token: 0x170012F8 RID: 4856
	// (get) Token: 0x06007FE8 RID: 32744 RVA: 0x0033ADB8 File Offset: 0x00338FB8
	public bool IsBeingEyedByRat
	{
		get
		{
			return this.m_isBeingEyedByRat;
		}
	}

	// Token: 0x06007FE9 RID: 32745 RVA: 0x0033ADC0 File Offset: 0x00338FC0
	protected IEnumerator HandleRatTheft()
	{
		Debug.Log("starting grabby..." + base.name);
		this.m_isBeingEyedByRat = true;
		PickupObject.ItemIsBeingTakenByRat = true;
		this.m_numberTimesRatTheftAttempted++;
		float elapsed = 0f;
		float duration = 2f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (!this || !this.m_isBeingEyedByRat)
		{
			PickupObject.ItemIsBeingTakenByRat = false;
			yield break;
		}
		Debug.Log("doing grabby...");
		Vector2 spawnPos = base.sprite.WorldCenter + Vector2.right / -2f;
		GameObject ratInstance = UnityEngine.Object.Instantiate<GameObject>(PrefabDatabase.Instance.ResourcefulRatThief, spawnPos.ToVector3ZUp(0f), Quaternion.identity);
		ThievingRatGrabby grabby = null;
		PlayMakerFSM fsm = ratInstance.GetComponent<PlayMakerFSM>();
		for (int i = 0; i < fsm.FsmStates.Length; i++)
		{
			for (int j = 0; j < fsm.FsmStates[i].Actions.Length; j++)
			{
				if (fsm.FsmStates[i].Actions[j] is ThievingRatGrabby)
				{
					grabby = fsm.FsmStates[i].Actions[j] as ThievingRatGrabby;
				}
			}
		}
		if (grabby != null)
		{
			grabby.TargetObject = this;
		}
		while (ratInstance)
		{
			yield return null;
		}
		PickupObject.ItemIsBeingTakenByRat = false;
		this.m_isBeingEyedByRat = false;
		yield break;
	}

	// Token: 0x06007FEA RID: 32746 RVA: 0x0033ADDC File Offset: 0x00338FDC
	public static void HandlePickupCurseParticles(tk2dBaseSprite targetSprite, float zOffset = 0f)
	{
		if (targetSprite)
		{
			Vector3 vector = targetSprite.WorldBottomLeft.ToVector3ZisY(zOffset);
			Vector3 vector2 = targetSprite.WorldTopRight.ToVector3ZisY(zOffset);
			float num = (vector2.y - vector.y) * (vector2.x - vector.x);
			float num2 = 25f * num;
			int num3 = Mathf.CeilToInt(Mathf.Max(1f, num2 * BraveTime.DeltaTime));
			int num4 = num3;
			Vector3 vector3 = vector;
			Vector3 vector4 = vector2;
			Vector3 vector5 = Vector3.up / 2f;
			float num5 = 120f;
			float num6 = 0.2f;
			float? num7 = new float?(UnityEngine.Random.Range(0.8f, 1.25f));
			GlobalSparksDoer.DoRandomParticleBurst(num4, vector3, vector4, vector5, num5, num6, null, num7, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
		}
	}

	// Token: 0x06007FEB RID: 32747 RVA: 0x0033AEB8 File Offset: 0x003390B8
	protected void HandlePickupCurseParticles()
	{
		if (!this || !base.sprite)
		{
			return;
		}
		bool flag = false;
		if (this is Gun)
		{
			Gun gun = this as Gun;
			for (int i = 0; i < gun.passiveStatModifiers.Length; i++)
			{
				if (gun.passiveStatModifiers[i].statToBoost == PlayerStats.StatType.Curse && gun.passiveStatModifiers[i].amount > 0f)
				{
					flag = true;
					break;
				}
			}
		}
		else if (this is PlayerItem)
		{
			PlayerItem playerItem = this as PlayerItem;
			for (int j = 0; j < playerItem.passiveStatModifiers.Length; j++)
			{
				if (playerItem.passiveStatModifiers[j].statToBoost == PlayerStats.StatType.Curse && playerItem.passiveStatModifiers[j].amount > 0f)
				{
					flag = true;
					break;
				}
			}
		}
		else if (this is PassiveItem)
		{
			PassiveItem passiveItem = this as PassiveItem;
			for (int k = 0; k < passiveItem.passiveStatModifiers.Length; k++)
			{
				if (passiveItem.passiveStatModifiers[k].statToBoost == PlayerStats.StatType.Curse && passiveItem.passiveStatModifiers[k].amount > 0f)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			PickupObject.HandlePickupCurseParticles(base.sprite, 0f);
		}
	}

	// Token: 0x06007FEC RID: 32748 RVA: 0x0033B02C File Offset: 0x0033922C
	protected void OnSharedPickup()
	{
		if (this.IgnoredByRat && this.ClearIgnoredByRatFlagOnPickup)
		{
			this.IgnoredByRat = false;
		}
	}

	// Token: 0x06007FED RID: 32749 RVA: 0x0033B04C File Offset: 0x0033924C
	public virtual void MidGameSerialize(List<object> data)
	{
	}

	// Token: 0x06007FEE RID: 32750 RVA: 0x0033B050 File Offset: 0x00339250
	public virtual void MidGameDeserialize(List<object> data)
	{
	}

	// Token: 0x04008263 RID: 33379
	public static bool RatBeatenAtPunchout;

	// Token: 0x04008264 RID: 33380
	[DisableInInspector]
	public int PickupObjectId = -1;

	// Token: 0x04008265 RID: 33381
	public PickupObject.ItemQuality quality;

	// Token: 0x04008266 RID: 33382
	public float additionalMagnificenceModifier;

	// Token: 0x04008267 RID: 33383
	public bool ItemSpansBaseQualityTiers;

	// Token: 0x04008268 RID: 33384
	[HideInInspectorIf("ItemSpansBaseQualityTiers", false)]
	public bool ItemRespectsHeartMagnificence;

	// Token: 0x04008269 RID: 33385
	public LootModData[] associatedItemChanceMods;

	// Token: 0x0400826A RID: 33386
	public ContentSource contentSource;

	// Token: 0x0400826B RID: 33387
	public bool ShouldBeExcludedFromShops;

	// Token: 0x0400826C RID: 33388
	public bool CanBeDropped = true;

	// Token: 0x0400826D RID: 33389
	public bool PreventStartingOwnerFromDropping;

	// Token: 0x0400826E RID: 33390
	public bool PersistsOnDeath;

	// Token: 0x0400826F RID: 33391
	public bool RespawnsIfPitfall;

	// Token: 0x04008270 RID: 33392
	public bool PreventSaveSerialization;

	// Token: 0x04008271 RID: 33393
	public bool IgnoredByRat;

	// Token: 0x04008272 RID: 33394
	[NonSerialized]
	public bool ClearIgnoredByRatFlagOnPickup;

	// Token: 0x04008273 RID: 33395
	[NonSerialized]
	public bool IsBeingSold;

	// Token: 0x04008274 RID: 33396
	[LongEnum]
	public GungeonFlags SaveFlagToSetOnAcquisition;

	// Token: 0x04008275 RID: 33397
	[SerializeField]
	protected string itemName;

	// Token: 0x04008276 RID: 33398
	protected static int s_lastRainbowPickupFrame = -1;

	// Token: 0x04008277 RID: 33399
	[NonSerialized]
	public bool HasBeenStatProcessed;

	// Token: 0x04008278 RID: 33400
	[HideInInspector]
	public int ForcedPositionInAmmonomicon = -1;

	// Token: 0x04008279 RID: 33401
	public bool UsesCustomCost;

	// Token: 0x0400827A RID: 33402
	[FormerlySerializedAs("costInStore")]
	public int CustomCost;

	// Token: 0x0400827B RID: 33403
	public bool PersistsOnPurchase;

	// Token: 0x0400827C RID: 33404
	public bool CanBeSold = true;

	// Token: 0x0400827D RID: 33405
	[NonSerialized]
	public bool HasProcessedStatMods;

	// Token: 0x0400827E RID: 33406
	protected Color m_alienPickupColor = new Color(1f, 1f, 0f, 1f);

	// Token: 0x0400827F RID: 33407
	public static bool ItemIsBeingTakenByRat;

	// Token: 0x04008280 RID: 33408
	protected bool m_isBeingEyedByRat;

	// Token: 0x04008281 RID: 33409
	protected int m_numberTimesRatTheftAttempted;

	// Token: 0x020015BF RID: 5567
	public enum ItemQuality
	{
		// Token: 0x04008283 RID: 33411
		EXCLUDED = -100,
		// Token: 0x04008284 RID: 33412
		SPECIAL = -50,
		// Token: 0x04008285 RID: 33413
		COMMON = 0,
		// Token: 0x04008286 RID: 33414
		D,
		// Token: 0x04008287 RID: 33415
		C,
		// Token: 0x04008288 RID: 33416
		B,
		// Token: 0x04008289 RID: 33417
		A,
		// Token: 0x0400828A RID: 33418
		S
	}
}
