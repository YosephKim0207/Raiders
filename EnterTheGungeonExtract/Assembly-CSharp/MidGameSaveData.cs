using System;
using System.Collections.Generic;
using FullSerializer;
using InControl;
using UnityEngine;

// Token: 0x02001565 RID: 5477
public class MidGameSaveData
{
	// Token: 0x06007D79 RID: 32121 RVA: 0x0032C4AC File Offset: 0x0032A6AC
	public MidGameSaveData()
	{
	}

	// Token: 0x06007D7A RID: 32122 RVA: 0x0032C4BC File Offset: 0x0032A6BC
	public MidGameSaveData(PlayerController p1, PlayerController p2, GlobalDungeonData.ValidTilesets targetLevel, string midGameSaveGuid)
	{
		this.midGameSaveGuid = midGameSaveGuid;
		this.levelSaved = targetLevel;
		this.savedGameMode = GameManager.Instance.CurrentGameMode;
		if (this.savedGameMode == GameManager.GameMode.SHORTCUT)
		{
			this.LastShortcutFloorLoaded = GameManager.Instance.LastShortcutFloorLoaded;
		}
		if (p2 != null)
		{
			this.savedGameType = GameManager.GameType.COOP_2_PLAYER;
		}
		else
		{
			this.savedGameType = GameManager.GameType.SINGLE_PLAYER;
		}
		this.playerOneData = new MidGamePlayerData(p1);
		if (this.savedGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.playerTwoData = new MidGamePlayerData(p2);
		}
		this.PriorSessionStats = GameStatsManager.Instance.MoveSessionStatsToSavedSessionStats();
		this.StaticShopData = BaseShopController.GetStaticShopDataForMidGameSave();
		this.RunData = GameManager.Instance.RunData;
	}

	// Token: 0x06007D7B RID: 32123 RVA: 0x0032C580 File Offset: 0x0032A780
	public bool IsValid()
	{
		return !this.invalidated;
	}

	// Token: 0x06007D7C RID: 32124 RVA: 0x0032C58C File Offset: 0x0032A78C
	public void Invalidate()
	{
		this.invalidated = true;
	}

	// Token: 0x06007D7D RID: 32125 RVA: 0x0032C598 File Offset: 0x0032A798
	public void Revalidate()
	{
		this.invalidated = false;
	}

	// Token: 0x06007D7E RID: 32126 RVA: 0x0032C5A4 File Offset: 0x0032A7A4
	public GameObject GetPlayerOnePrefab()
	{
		string text = CharacterSelectController.GetCharacterPathFromIdentity(this.playerOneData.CharacterIdentity);
		if (this.levelSaved == GlobalDungeonData.ValidTilesets.FINALGEON && this.playerOneData.CharacterIdentity == PlayableCharacters.Pilot)
		{
			text = "PlayerRogueShip";
		}
		return (GameObject)BraveResources.Load(text, ".prefab");
	}

	// Token: 0x06007D7F RID: 32127 RVA: 0x0032C5FC File Offset: 0x0032A7FC
	public void LoadPreGenDataFromMidGameSave()
	{
		GameManager.Instance.RunData = this.RunData;
	}

	// Token: 0x06007D80 RID: 32128 RVA: 0x0032C610 File Offset: 0x0032A810
	public void LoadDataFromMidGameSave(PlayerController p1, PlayerController p2)
	{
		if (this.StaticShopData != null)
		{
			BaseShopController.LoadFromMidGameSave(this.StaticShopData);
		}
		GameManager.Instance.CurrentGameMode = this.savedGameMode;
		GameManager.Instance.LastShortcutFloorLoaded = this.LastShortcutFloorLoaded;
		GameStatsManager.Instance.AssignMidGameSavedSessionStats(this.PriorSessionStats);
		if (p1)
		{
			PassiveItem.DecrementFlag(p1, typeof(SevenLeafCloverItem));
		}
		if (p2)
		{
			PassiveItem.DecrementFlag(p2, typeof(SevenLeafCloverItem));
		}
		this.InitializePlayerData(p1, this.playerOneData, true);
		if (this.savedGameType == GameManager.GameType.COOP_2_PLAYER && p2)
		{
			this.InitializePlayerData(p2, this.playerTwoData, false);
			BraveInput.ReassignAllControllers(MidGameSaveData.ContinuePressedDevice);
		}
		MidGameSaveData.ContinuePressedDevice = null;
	}

	// Token: 0x06007D81 RID: 32129 RVA: 0x0032C6DC File Offset: 0x0032A8DC
	public void InitializePlayerData(PlayerController p1, MidGamePlayerData playerData, bool isPlayerOne)
	{
		MidGameSaveData.IsInitializingPlayerData = true;
		p1.MasteryTokensCollectedThisRun = playerData.MasteryTokensCollected;
		p1.CharacterUsesRandomGuns = playerData.CharacterUsesRandomGuns;
		p1.HasTakenDamageThisRun = playerData.HasTakenDamageThisRun;
		p1.HasFiredNonStartingGun = playerData.HasFiredNonStartingGun;
		GameObject gameObject = (GameObject)ResourceCache.Acquire("Global Prefabs/VFX_ParadoxPortal");
		ParadoxPortalController component = gameObject.GetComponent<ParadoxPortalController>();
		p1.portalEeveeTex = component.CosmicTex;
		p1.IsTemporaryEeveeForUnlock = playerData.IsTemporaryEeveeForUnlock;
		ChallengeManager.ChallengeModeType = playerData.ChallengeMode;
		if (this.levelSaved == GlobalDungeonData.ValidTilesets.FINALGEON)
		{
			p1.CharacterUsesRandomGuns = false;
		}
		if (this.levelSaved != GlobalDungeonData.ValidTilesets.FINALGEON || !(p1 is PlayerSpaceshipController))
		{
			p1.inventory.DestroyAllGuns();
			p1.RemoveAllPassiveItems();
			p1.RemoveAllActiveItems();
			if (playerData.passiveItems != null)
			{
				for (int i = 0; i < playerData.passiveItems.Count; i++)
				{
					EncounterTrackable.SuppressNextNotification = true;
					LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(playerData.passiveItems[i].PickupID).gameObject, p1);
				}
			}
			if (playerData.activeItems != null)
			{
				for (int j = 0; j < playerData.activeItems.Count; j++)
				{
					EncounterTrackable.SuppressNextNotification = true;
					LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(playerData.activeItems[j].PickupID).gameObject, p1);
				}
			}
			if (playerData.guns != null)
			{
				for (int k = 0; k < playerData.guns.Count; k++)
				{
					EncounterTrackable.SuppressNextNotification = true;
					LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(playerData.guns[k].PickupID).gameObject, p1);
				}
				for (int l = 0; l < playerData.guns.Count; l++)
				{
					for (int m = 0; m < p1.inventory.AllGuns.Count; m++)
					{
						if (p1.inventory.AllGuns[m].PickupObjectId == playerData.guns[l].PickupID)
						{
							p1.inventory.AllGuns[m].MidGameDeserialize(playerData.guns[l].SerializedData);
							for (int n = 0; n < playerData.guns[l].DuctTapedGunIDs.Count; n++)
							{
								Gun gun = PickupObjectDatabase.GetById(playerData.guns[l].DuctTapedGunIDs[n]) as Gun;
								if (gun)
								{
									DuctTapeItem.DuctTapeGuns(gun, p1.inventory.AllGuns[m]);
								}
							}
							p1.inventory.AllGuns[m].ammo = playerData.guns[l].CurrentAmmo;
							TransformGunSynergyProcessor[] componentsInChildren = p1.inventory.AllGuns[m].GetComponentsInChildren<TransformGunSynergyProcessor>();
							for (int num = 0; num < componentsInChildren.Length; num++)
							{
								componentsInChildren[num].ShouldResetAmmoAfterTransformation = true;
								componentsInChildren[num].ResetAmmoCount = playerData.guns[l].CurrentAmmo;
							}
						}
					}
				}
			}
			if (playerData.CurrentHealth <= 0f && playerData.CurrentArmor <= 0f)
			{
				p1.healthHaver.Armor = 0f;
				p1.DieOnMidgameLoad();
			}
			else
			{
				p1.healthHaver.ForceSetCurrentHealth(playerData.CurrentHealth);
				p1.healthHaver.Armor = playerData.CurrentArmor;
			}
			if (isPlayerOne)
			{
				p1.carriedConsumables.KeyBullets = playerData.CurrentKeys;
				p1.carriedConsumables.Currency = playerData.CurrentCurrency;
			}
			p1.Blanks = Mathf.Max(p1.Blanks, playerData.CurrentBlanks);
			if (playerData.activeItems != null)
			{
				for (int num2 = 0; num2 < playerData.activeItems.Count; num2++)
				{
					for (int num3 = 0; num3 < p1.activeItems.Count; num3++)
					{
						if (playerData.activeItems[num2].PickupID == p1.activeItems[num3].PickupObjectId)
						{
							p1.activeItems[num3].MidGameDeserialize(playerData.activeItems[num2].SerializedData);
							p1.activeItems[num3].CurrentDamageCooldown = playerData.activeItems[num2].DamageCooldown;
							p1.activeItems[num3].CurrentRoomCooldown = playerData.activeItems[num2].RoomCooldown;
							p1.activeItems[num3].CurrentTimeCooldown = playerData.activeItems[num2].TimeCooldown;
							if (p1.activeItems[num3].consumable && playerData.activeItems[num2].NumberOfUses > 0)
							{
								p1.activeItems[num3].numberOfUses = playerData.activeItems[num2].NumberOfUses;
							}
						}
					}
				}
			}
			if (playerData.passiveItems != null)
			{
				for (int num4 = 0; num4 < playerData.passiveItems.Count; num4++)
				{
					for (int num5 = 0; num5 < p1.passiveItems.Count; num5++)
					{
						if (playerData.passiveItems[num4].PickupID == p1.passiveItems[num5].PickupObjectId)
						{
							p1.passiveItems[num5].MidGameDeserialize(playerData.passiveItems[num4].SerializedData);
						}
					}
				}
			}
			if (playerData.ownerlessStatModifiers != null)
			{
				if (p1.ownerlessStatModifiers == null)
				{
					p1.ownerlessStatModifiers = new List<StatModifier>();
				}
				for (int num6 = 0; num6 < playerData.ownerlessStatModifiers.Count; num6++)
				{
					p1.ownerlessStatModifiers.Add(playerData.ownerlessStatModifiers[num6]);
				}
			}
			if (this.levelSaved == GlobalDungeonData.ValidTilesets.FINALGEON && p1.characterIdentity != PlayableCharacters.Gunslinger)
			{
				p1.ResetToFactorySettings(true, true, false);
			}
			if (p1 && p1.stats != null)
			{
				p1.stats.RecalculateStats(p1, false, false);
			}
			if (playerData.HasBloodthirst)
			{
				p1.gameObject.GetOrAddComponent<Bloodthirst>();
			}
		}
		MidGameSaveData.IsInitializingPlayerData = false;
		EncounterTrackable.SuppressNextNotification = false;
	}

	// Token: 0x04008083 RID: 32899
	[fsProperty]
	public GlobalDungeonData.ValidTilesets levelSaved = GlobalDungeonData.ValidTilesets.CASTLEGEON;

	// Token: 0x04008084 RID: 32900
	[fsProperty]
	public GameManager.GameType savedGameType;

	// Token: 0x04008085 RID: 32901
	[fsProperty]
	public GameManager.GameMode savedGameMode;

	// Token: 0x04008086 RID: 32902
	[fsProperty]
	public int LastShortcutFloorLoaded;

	// Token: 0x04008087 RID: 32903
	[fsProperty]
	public MidGamePlayerData playerOneData;

	// Token: 0x04008088 RID: 32904
	[fsProperty]
	public MidGamePlayerData playerTwoData;

	// Token: 0x04008089 RID: 32905
	[fsProperty]
	public GameStats PriorSessionStats;

	// Token: 0x0400808A RID: 32906
	[fsProperty]
	public MidGameStaticShopData StaticShopData;

	// Token: 0x0400808B RID: 32907
	[fsProperty]
	public RunData RunData;

	// Token: 0x0400808C RID: 32908
	[fsProperty]
	public string midGameSaveGuid;

	// Token: 0x0400808D RID: 32909
	[fsProperty]
	public bool invalidated;

	// Token: 0x0400808E RID: 32910
	public static InputDevice ContinuePressedDevice;

	// Token: 0x0400808F RID: 32911
	public static bool IsInitializingPlayerData;
}
