using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200155E RID: 5470
public static class LootEngine
{
	// Token: 0x06007D49 RID: 32073 RVA: 0x0032AB28 File Offset: 0x00328D28
	public static void ClearPerLevelData()
	{
		StaticReferenceManager.WeaponChestsSpawnedOnFloor = 0;
		StaticReferenceManager.ItemChestsSpawnedOnFloor = 0;
		StaticReferenceManager.DChestsSpawnedOnFloor = 0;
	}

	// Token: 0x06007D4A RID: 32074 RVA: 0x0032AB3C File Offset: 0x00328D3C
	public static void SpawnHealth(Vector2 centerPoint, int halfHearts, Vector2? direction, float startingZForce = 4f, float startingHeight = 0.05f)
	{
		int i;
		for (i = halfHearts; i >= 2; i -= 2)
		{
			LootEngine.SpawnItem(GameManager.Instance.RewardManager.FullHeartPrefab.gameObject, centerPoint, (direction == null) ? Vector2.up : direction.Value, startingZForce, true, false, false);
		}
		while (i >= 1)
		{
			LootEngine.SpawnItem(GameManager.Instance.RewardManager.HalfHeartPrefab.gameObject, centerPoint, (direction == null) ? Vector2.up : direction.Value, startingZForce, true, false, false);
			i--;
		}
	}

	// Token: 0x06007D4B RID: 32075 RVA: 0x0032ABF0 File Offset: 0x00328DF0
	public static GameObject SpawnBowlerNote(GameObject note, Vector2 position, RoomHandler parentRoom, bool doPoof = false)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(note, position.ToVector3ZisY(0f), Quaternion.identity);
		if (gameObject)
		{
			IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
			for (int i = 0; i < interfacesInChildren.Length; i++)
			{
				parentRoom.RegisterInteractable(interfacesInChildren[i]);
			}
		}
		if (doPoof)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
			tk2dBaseSprite component = gameObject2.GetComponent<tk2dBaseSprite>();
			component.PlaceAtPositionByAnchor(position.ToVector3ZUp(0f) + new Vector3(0.5f, 0.75f, 0f), tk2dBaseSprite.Anchor.MiddleCenter);
			component.HeightOffGround = 5f;
			component.UpdateZDepth();
		}
		return gameObject;
	}

	// Token: 0x06007D4C RID: 32076 RVA: 0x0032ACA8 File Offset: 0x00328EA8
	public static void SpawnCurrency(Vector2 centerPoint, int amountToDrop, bool isMetaCurrency, Vector2? direction, float? angleVariance, float startingZForce = 4f, float startingHeight = 0.05f)
	{
		if (!isMetaCurrency && PassiveItem.IsFlagSetAtAll(typeof(BankBagItem)))
		{
			amountToDrop *= 2;
		}
		List<GameObject> currencyToDrop = GameManager.Instance.Dungeon.sharedSettingsPrefab.GetCurrencyToDrop(amountToDrop, isMetaCurrency, false);
		float num = 360f / (float)currencyToDrop.Count;
		if (angleVariance != null)
		{
			num = angleVariance.Value * 2f / (float)currencyToDrop.Count;
		}
		Vector3 vector = Vector3.up;
		if (direction != null && angleVariance != null)
		{
			vector = Quaternion.Euler(0f, 0f, -angleVariance.Value) * direction.Value;
		}
		else if (direction != null)
		{
			vector = direction.Value.ToVector3ZUp(0f);
		}
		for (int i = 0; i < currencyToDrop.Count; i++)
		{
			Vector3 vector2 = Quaternion.Euler(0f, 0f, num * (float)i) * vector;
			vector2 *= 2f;
			GameObject gameObject = SpawnManager.SpawnDebris(currencyToDrop[i], centerPoint.ToVector3ZUp(centerPoint.y), Quaternion.identity);
			DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
			orAddComponent.shouldUseSRBMotion = true;
			orAddComponent.angularVelocity = 0f;
			orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
			orAddComponent.Trigger(vector2.WithZ(startingZForce), startingHeight, 1f);
			orAddComponent.canRotate = false;
		}
	}

	// Token: 0x06007D4D RID: 32077 RVA: 0x0032AE2C File Offset: 0x0032902C
	public static void SpawnCurrencyManual(Vector2 centerPoint, int amountToDrop)
	{
		List<GameObject> currencyToDrop = GameManager.Instance.Dungeon.sharedSettingsPrefab.GetCurrencyToDrop(amountToDrop, false, true);
		float num = 360f / (float)currencyToDrop.Count;
		Vector3 up = Vector3.up;
		List<CurrencyPickup> list = new List<CurrencyPickup>();
		for (int i = 0; i < currencyToDrop.Count; i++)
		{
			Vector3 vector = Quaternion.Euler(0f, 0f, num * (float)i) * up;
			vector *= 2f;
			GameObject gameObject = SpawnManager.SpawnDebris(currencyToDrop[i], centerPoint.ToVector3ZUp(centerPoint.y), Quaternion.identity);
			CurrencyPickup component = gameObject.GetComponent<CurrencyPickup>();
			component.PreventPickup = true;
			list.Add(component);
			PickupMover component2 = gameObject.GetComponent<PickupMover>();
			if (component2)
			{
				component2.enabled = false;
			}
			DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
			DebrisObject debrisObject = orAddComponent;
			debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(delegate(DebrisObject sourceDebris)
			{
				sourceDebris.GetComponent<CurrencyPickup>().PreventPickup = false;
				sourceDebris.OnGrounded = null;
			}));
			orAddComponent.shouldUseSRBMotion = true;
			orAddComponent.angularVelocity = 0f;
			orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
			orAddComponent.Trigger(vector.WithZ(2f) * UnityEngine.Random.Range(1.5f, 2.125f), 0.05f, 1f);
			orAddComponent.canRotate = false;
		}
		GameManager.Instance.Dungeon.StartCoroutine(LootEngine.HandleManualCoinSpawnLifespan(list));
	}

	// Token: 0x06007D4E RID: 32078 RVA: 0x0032AFB0 File Offset: 0x003291B0
	private static IEnumerator HandleManualCoinSpawnLifespan(List<CurrencyPickup> coins)
	{
		float elapsed = 0f;
		while (elapsed < BankBagItem.cachedCoinLifespan * 0.75f)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		float flickerTimer = 0f;
		while (elapsed < BankBagItem.cachedCoinLifespan)
		{
			elapsed += BraveTime.DeltaTime;
			flickerTimer += BraveTime.DeltaTime;
			for (int i = 0; i < coins.Count; i++)
			{
				if (coins[i] && coins[i].renderer)
				{
					bool flag = flickerTimer % 0.2f > 0.15f;
					coins[i].renderer.enabled = flag;
				}
			}
			yield return null;
		}
		for (int j = 0; j < coins.Count; j++)
		{
			if (coins[j])
			{
				UnityEngine.Object.Destroy(coins[j].gameObject);
			}
		}
		yield break;
	}

	// Token: 0x06007D4F RID: 32079 RVA: 0x0032AFCC File Offset: 0x003291CC
	public static void SpawnCurrency(Vector2 centerPoint, int amountToDrop, bool isMetaCurrency = false)
	{
		LootEngine.SpawnCurrency(centerPoint, amountToDrop, isMetaCurrency, null, null, 4f, 0.05f);
	}

	// Token: 0x06007D50 RID: 32080 RVA: 0x0032B000 File Offset: 0x00329200
	public static bool DoAmmoClipCheck(FloorRewardData currentRewardData, out LootEngine.AmmoDropType AmmoToDrop)
	{
		bool flag = LootEngine.DoAmmoClipCheck(currentRewardData.FloorChanceToDropAmmo);
		AmmoToDrop = LootEngine.AmmoDropType.DEFAULT_AMMO;
		if (flag)
		{
			AmmoToDrop = ((UnityEngine.Random.value >= currentRewardData.FloorChanceForSpreadAmmo) ? LootEngine.AmmoDropType.DEFAULT_AMMO : LootEngine.AmmoDropType.SPREAD_AMMO);
			return true;
		}
		return false;
	}

	// Token: 0x06007D51 RID: 32081 RVA: 0x0032B040 File Offset: 0x00329240
	public static bool DoAmmoClipCheck(float baseAmmoDropChance)
	{
		float num = baseAmmoDropChance;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num *= GameManager.Instance.RewardManager.CoopAmmoChanceModifier;
		}
		float num2 = 1f;
		float num3 = (float)PlayerStats.GetTotalCurse();
		num2 += Mathf.Clamp01(num3 / 10f) / 2f;
		num *= num2;
		if (GameManager.Instance.AllPlayers != null)
		{
			float num4 = 0f;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].inventory != null && GameManager.Instance.AllPlayers[i].inventory.AllGuns != null)
				{
					for (int j = 0; j < GameManager.Instance.AllPlayers[i].inventory.AllGuns.Count; j++)
					{
						Gun gun = GameManager.Instance.AllPlayers[i].inventory.AllGuns[j];
						if (gun && !gun.InfiniteAmmo)
						{
							num4 = 1f;
						}
					}
				}
			}
			num *= num4;
		}
		return UnityEngine.Random.value < num;
	}

	// Token: 0x06007D52 RID: 32082 RVA: 0x0032B194 File Offset: 0x00329394
	private static void PostprocessGunSpawn(Gun spawnedGun)
	{
		spawnedGun.gameObject.SetActive(true);
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_HAS_BEEN_PEDESTAL_MIMICKED) && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE && UnityEngine.Random.value < GameManager.Instance.RewardManager.GunMimicMimicGunChance)
		{
			spawnedGun.gameObject.AddComponent<MimicGunMimicModifier>();
		}
	}

	// Token: 0x06007D53 RID: 32083 RVA: 0x0032B1F8 File Offset: 0x003293F8
	private static void PostprocessItemSpawn(DebrisObject item)
	{
		tk2dSpriteAnimator component = item.gameObject.GetComponent<tk2dSpriteAnimator>();
		CurrencyPickup component2 = item.GetComponent<CurrencyPickup>();
		if (component2 == null && !item.GetComponent<BulletThatCanKillThePast>() && (component == null || !component.playAutomatically))
		{
			item.gameObject.GetOrAddComponent<SquishyBounceWiggler>();
		}
		PlayerItem component3 = item.GetComponent<PlayerItem>();
		if (component3 != null && !RoomHandler.unassignedInteractableObjects.Contains(component3))
		{
			RoomHandler.unassignedInteractableObjects.Add(component3);
		}
		PassiveItem component4 = item.GetComponent<PassiveItem>();
		if (component4 != null && !RoomHandler.unassignedInteractableObjects.Contains(component4))
		{
			RoomHandler.unassignedInteractableObjects.Add(component4);
		}
		AmmoPickup component5 = item.GetComponent<AmmoPickup>();
		if (component5 != null && !RoomHandler.unassignedInteractableObjects.Contains(component5))
		{
			RoomHandler.unassignedInteractableObjects.Add(component5);
		}
		HealthPickup component6 = item.GetComponent<HealthPickup>();
		if (component6 != null && !RoomHandler.unassignedInteractableObjects.Contains(component6))
		{
			RoomHandler.unassignedInteractableObjects.Add(component6);
		}
		item.OnGrounded = (Action<DebrisObject>)Delegate.Remove(item.OnGrounded, new Action<DebrisObject>(LootEngine.PostprocessItemSpawn));
	}

	// Token: 0x06007D54 RID: 32084 RVA: 0x0032B34C File Offset: 0x0032954C
	private static DebrisObject SpawnInternal(GameObject spawnedItem, Vector3 spawnPosition, Vector2 spawnDirection, float force, bool invalidUntilGrounded = true, bool doDefaultItemPoof = false, bool disablePostprocessing = false, bool disableHeightBoost = false)
	{
		Vector3 vector = spawnDirection.ToVector3ZUp(0f).normalized;
		vector *= force;
		Gun component = spawnedItem.GetComponent<Gun>();
		if (component != null)
		{
			LootEngine.PostprocessGunSpawn(component);
			DebrisObject debrisObject = component.DropGun(2f);
			if (doDefaultItemPoof)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
				tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
				component2.PlaceAtPositionByAnchor(debrisObject.sprite.WorldCenter.ToVector3ZUp(0f) + new Vector3(0f, 0.5f, 0f), tk2dBaseSprite.Anchor.MiddleCenter);
				component2.HeightOffGround = 5f;
				component2.UpdateZDepth();
			}
			return debrisObject;
		}
		DebrisObject orAddComponent = spawnedItem.GetOrAddComponent<DebrisObject>();
		if (!disablePostprocessing)
		{
			DebrisObject debrisObject2 = orAddComponent;
			debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(LootEngine.PostprocessItemSpawn));
		}
		orAddComponent.additionalHeightBoost = ((!disableHeightBoost) ? 1.5f : 0f);
		orAddComponent.shouldUseSRBMotion = true;
		orAddComponent.angularVelocity = 0f;
		orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
		orAddComponent.sprite.UpdateZDepth();
		orAddComponent.Trigger(vector.WithZ(2f), (!disableHeightBoost) ? 0.5f : 0f, 1f);
		orAddComponent.canRotate = false;
		if (invalidUntilGrounded && orAddComponent.specRigidbody != null)
		{
			orAddComponent.specRigidbody.CollideWithOthers = false;
			DebrisObject debrisObject3 = orAddComponent;
			debrisObject3.OnTouchedGround = (Action<DebrisObject>)Delegate.Combine(debrisObject3.OnTouchedGround, new Action<DebrisObject>(LootEngine.BecomeViableItem));
		}
		orAddComponent.AssignFinalWorldDepth(-0.5f);
		if (doDefaultItemPoof)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
			tk2dBaseSprite component3 = gameObject2.GetComponent<tk2dBaseSprite>();
			component3.PlaceAtPositionByAnchor(orAddComponent.sprite.WorldCenter.ToVector3ZUp(0f) + new Vector3(0f, 0.5f, 0f), tk2dBaseSprite.Anchor.MiddleCenter);
			component3.HeightOffGround = 5f;
			component3.UpdateZDepth();
		}
		return orAddComponent;
	}

	// Token: 0x06007D55 RID: 32085 RVA: 0x0032B5A0 File Offset: 0x003297A0
	public static void DoDefaultSynergyPoof(Vector2 worldPosition, bool ignoreTimeScale = false)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Synergy_Poof_001"));
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.PlaceAtPositionByAnchor(worldPosition.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
		component.HeightOffGround = 5f;
		component.UpdateZDepth();
		if (ignoreTimeScale)
		{
			tk2dSpriteAnimator component2 = component.GetComponent<tk2dSpriteAnimator>();
			if (component2 != null)
			{
				component2.ignoreTimeScale = true;
				component2.alwaysUpdateOffscreen = true;
			}
		}
	}

	// Token: 0x06007D56 RID: 32086 RVA: 0x0032B614 File Offset: 0x00329814
	public static void DoDefaultPurplePoof(Vector2 worldPosition, bool ignoreTimeScale = false)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Purple_Smoke_001"));
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.PlaceAtPositionByAnchor(worldPosition.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
		component.HeightOffGround = 5f;
		component.UpdateZDepth();
		if (ignoreTimeScale)
		{
			tk2dSpriteAnimator component2 = component.GetComponent<tk2dSpriteAnimator>();
			if (component2 != null)
			{
				component2.ignoreTimeScale = true;
				component2.alwaysUpdateOffscreen = true;
			}
		}
	}

	// Token: 0x06007D57 RID: 32087 RVA: 0x0032B688 File Offset: 0x00329888
	public static void DoDefaultItemPoof(Vector2 worldPosition, bool ignoreTimeScale = false, bool muteAudio = false)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.PlaceAtPositionByAnchor(worldPosition.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
		component.HeightOffGround = 5f;
		component.UpdateZDepth();
		if (ignoreTimeScale || muteAudio)
		{
			tk2dSpriteAnimator component2 = component.GetComponent<tk2dSpriteAnimator>();
			if (component2 != null)
			{
				if (ignoreTimeScale)
				{
					component2.ignoreTimeScale = true;
					component2.alwaysUpdateOffscreen = true;
				}
				if (muteAudio)
				{
					component2.MuteAudio = true;
				}
			}
		}
	}

	// Token: 0x06007D58 RID: 32088 RVA: 0x0032B714 File Offset: 0x00329914
	public static DebrisObject DropItemWithoutInstantiating(GameObject item, Vector3 spawnPosition, Vector2 spawnDirection, float force, bool invalidUntilGrounded = true, bool doDefaultItemPoof = false, bool disablePostprocessing = false, bool disableHeightBoost = false)
	{
		if (item.GetComponent<DebrisObject>() != null)
		{
			UnityEngine.Object.DestroyImmediate(item.GetComponent<DebrisObject>());
		}
		item.GetComponent<Renderer>().enabled = true;
		item.transform.parent = null;
		item.transform.position = spawnPosition;
		item.transform.rotation = Quaternion.identity;
		return LootEngine.SpawnInternal(item, spawnPosition, spawnDirection, force, invalidUntilGrounded, doDefaultItemPoof, disablePostprocessing, disableHeightBoost);
	}

	// Token: 0x06007D59 RID: 32089 RVA: 0x0032B784 File Offset: 0x00329984
	public static DebrisObject SpawnItem(GameObject item, Vector3 spawnPosition, Vector2 spawnDirection, float force, bool invalidUntilGrounded = true, bool doDefaultItemPoof = false, bool disableHeightBoost = false)
	{
		if (GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE))
		{
			PickupObject component = item.GetComponent<PickupObject>();
			if (component && component.PickupObjectId == GlobalItemIds.UnfinishedGun)
			{
				item = PickupObjectDatabase.GetById(GlobalItemIds.FinishedGun).gameObject;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item, spawnPosition, Quaternion.identity);
		GameObject gameObject2 = gameObject;
		return LootEngine.SpawnInternal(gameObject2, spawnPosition, spawnDirection, force, invalidUntilGrounded, doDefaultItemPoof, false, disableHeightBoost);
	}

	// Token: 0x06007D5A RID: 32090 RVA: 0x0032B818 File Offset: 0x00329A18
	public static void DelayedSpawnItem(float delay, GameObject item, Vector3 spawnPosition, Vector2 spawnDirection, float force, bool invalidUntilGrounded = true, bool doDefaultItemPoof = false, bool disableHeightBoost = false)
	{
		GameManager.Instance.StartCoroutine(LootEngine.DelayedSpawnItem_CR(delay, item, spawnPosition, spawnDirection, force, invalidUntilGrounded, doDefaultItemPoof, disableHeightBoost));
	}

	// Token: 0x06007D5B RID: 32091 RVA: 0x0032B844 File Offset: 0x00329A44
	private static IEnumerator DelayedSpawnItem_CR(float delay, GameObject item, Vector3 spawnPosition, Vector2 spawnDirection, float force, bool invalidUntilGrounded = true, bool doDefaultItemPoof = false, bool disableHeightBoost = false)
	{
		float elapsed = 0f;
		while (elapsed < delay)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		LootEngine.SpawnItem(item, spawnPosition, spawnDirection, force, invalidUntilGrounded, doDefaultItemPoof, disableHeightBoost);
		yield break;
	}

	// Token: 0x06007D5C RID: 32092 RVA: 0x0032B894 File Offset: 0x00329A94
	public static void GivePrefabToPlayer(GameObject item, PlayerController player)
	{
		Gun component = item.GetComponent<Gun>();
		if (component != null)
		{
			EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
			if (component2 != null)
			{
				component2.HandleEncounter();
			}
			if (player.CharacterUsesRandomGuns)
			{
				player.ChangeToRandomGun();
			}
			else
			{
				player.inventory.AddGunToInventory(component, true);
			}
		}
		else
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item, Vector3.zero, Quaternion.identity);
			PickupObject component3 = gameObject.GetComponent<PickupObject>();
			if (component3 != null)
			{
				if (component3 is PlayerItem)
				{
					(component3 as PlayerItem).ForceAsExtant = true;
				}
				component3.Pickup(player);
			}
			else
			{
				Debug.LogError("Failed in giving item to player; item " + item.name + " is not a pickupObject.");
			}
		}
	}

	// Token: 0x06007D5D RID: 32093 RVA: 0x0032B958 File Offset: 0x00329B58
	public static Gun TryGiveGunToPlayer(GameObject item, PlayerController player, bool attemptForce = false)
	{
		Gun g = item.GetComponent<Gun>();
		if (g != null)
		{
			if (player.inventory.AllGuns.Count >= player.inventory.maxGuns && !attemptForce)
			{
				Gun gun = player.inventory.AllGuns.Find((Gun g2) => g2.PickupObjectId == g.PickupObjectId);
				if (gun == null || gun.CurrentAmmo >= gun.AdjustedMaxAmmo)
				{
					LootEngine.SpewLoot(item, player.specRigidbody.UnitCenter);
					return null;
				}
			}
			EncounterTrackable component = g.GetComponent<EncounterTrackable>();
			if (component != null)
			{
				component.HandleEncounter();
			}
			return player.inventory.AddGunToInventory(g, true);
		}
		return null;
	}

	// Token: 0x06007D5E RID: 32094 RVA: 0x0032BA34 File Offset: 0x00329C34
	public static bool TryGivePrefabToPlayer(GameObject item, PlayerController player, bool attemptForce = false)
	{
		Gun g = item.GetComponent<Gun>();
		if (g != null)
		{
			if (player.inventory.AllGuns.Count >= player.inventory.maxGuns && !attemptForce)
			{
				Gun gun = player.inventory.AllGuns.Find((Gun g2) => g2.PickupObjectId == g.PickupObjectId);
				if (gun == null || gun.CurrentAmmo >= gun.AdjustedMaxAmmo)
				{
					LootEngine.SpewLoot(item, player.specRigidbody.UnitCenter);
					return false;
				}
			}
			EncounterTrackable component = g.GetComponent<EncounterTrackable>();
			if (component != null)
			{
				component.HandleEncounter();
			}
			player.inventory.AddGunToInventory(g, true);
			return true;
		}
		PlayerItem component2 = item.GetComponent<PlayerItem>();
		if (component2 && player.activeItems.Count >= player.maxActiveItemsHeld && !attemptForce)
		{
			LootEngine.SpewLoot(item, player.specRigidbody.UnitCenter);
			return false;
		}
		PickupObject component3 = UnityEngine.Object.Instantiate<GameObject>(item, Vector3.zero, Quaternion.identity).GetComponent<PickupObject>();
		if (component3 == null)
		{
			Debug.LogError("Failed in giving item to player; item " + item.name + " is not a pickupObject.");
			return false;
		}
		if (component3 is PlayerItem)
		{
			(component3 as PlayerItem).ForceAsExtant = true;
		}
		component3.Pickup(player);
		return true;
	}

	// Token: 0x06007D5F RID: 32095 RVA: 0x0032BBBC File Offset: 0x00329DBC
	private static void BecomeViableItem(DebrisObject debris)
	{
		debris.specRigidbody.CollideWithOthers = true;
	}

	// Token: 0x06007D60 RID: 32096 RVA: 0x0032BBCC File Offset: 0x00329DCC
	public static DebrisObject SpewLoot(GameObject itemToSpawn, Vector3 spawnPosition)
	{
		Vector3 vector = Quaternion.Euler(0f, 0f, 0f) * Vector3.down;
		vector *= 2f;
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE))
		{
			PickupObject component = itemToSpawn.GetComponent<PickupObject>();
			if (component && component.PickupObjectId == GlobalItemIds.UnfinishedGun)
			{
				itemToSpawn = PickupObjectDatabase.GetById(GlobalItemIds.FinishedGun).gameObject;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(itemToSpawn, spawnPosition, Quaternion.identity);
		Gun component2 = gameObject.GetComponent<Gun>();
		DebrisObject debrisObject;
		if (component2 != null)
		{
			LootEngine.PostprocessGunSpawn(component2);
			debrisObject = component2.DropGun(2f);
		}
		else
		{
			DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
			if (component2 == null)
			{
				DebrisObject debrisObject2 = orAddComponent;
				debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(LootEngine.PostprocessItemSpawn));
			}
			orAddComponent.FlagAsPickup();
			orAddComponent.additionalHeightBoost = 1.5f;
			orAddComponent.shouldUseSRBMotion = true;
			orAddComponent.angularVelocity = 0f;
			orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
			orAddComponent.sprite.UpdateZDepth();
			orAddComponent.AssignFinalWorldDepth(-0.5f);
			orAddComponent.Trigger(Vector3.zero, 0.5f, 1f);
			orAddComponent.canRotate = false;
			debrisObject = orAddComponent;
		}
		return debrisObject;
	}

	// Token: 0x06007D61 RID: 32097 RVA: 0x0032BD3C File Offset: 0x00329F3C
	public static List<DebrisObject> SpewLoot(List<GameObject> itemsToSpawn, Vector3 spawnPosition)
	{
		List<DebrisObject> list = new List<DebrisObject>();
		float num = ((itemsToSpawn.Count != 8) ? 0f : 22.5f);
		float num2 = 360f / (float)itemsToSpawn.Count;
		for (int i = 0; i < itemsToSpawn.Count; i++)
		{
			Vector3 vector = Quaternion.Euler(0f, 0f, num + num2 * (float)i) * Vector3.down;
			vector *= 2f;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(itemsToSpawn[i], spawnPosition, Quaternion.identity);
			Gun component = gameObject.GetComponent<Gun>();
			if (component != null)
			{
				LootEngine.PostprocessGunSpawn(component);
				list.Add(component.DropGun(2f));
			}
			else
			{
				DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
				if (component == null)
				{
					DebrisObject debrisObject = orAddComponent;
					debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(LootEngine.PostprocessItemSpawn));
				}
				orAddComponent.additionalHeightBoost = 1.5f;
				orAddComponent.shouldUseSRBMotion = true;
				orAddComponent.angularVelocity = 0f;
				orAddComponent.Priority = EphemeralObject.EphemeralPriority.Critical;
				orAddComponent.sprite.UpdateZDepth();
				orAddComponent.AssignFinalWorldDepth(-0.5f);
				orAddComponent.Trigger(vector.WithZ(2f), 0.5f, 1f);
				orAddComponent.canRotate = false;
				list.Add(orAddComponent);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].sprite)
			{
				list[j].sprite.UpdateZDepth();
			}
		}
		return list;
	}

	// Token: 0x06007D62 RID: 32098 RVA: 0x0032BF04 File Offset: 0x0032A104
	private static PickupObject.ItemQuality GetRandomItemTier()
	{
		return (PickupObject.ItemQuality)UnityEngine.Random.Range(0, 6);
	}

	// Token: 0x06007D63 RID: 32099 RVA: 0x0032BF10 File Offset: 0x0032A110
	public static List<T> GetItemsOfQualityFromList<T>(List<T> validObjects, PickupObject.ItemQuality quality) where T : PickupObject
	{
		List<T> list = new List<T>();
		for (int i = 0; i < validObjects.Count; i++)
		{
			if (validObjects[i].quality == quality)
			{
				list.Add(validObjects[i]);
			}
		}
		return list;
	}

	// Token: 0x06007D64 RID: 32100 RVA: 0x0032BF60 File Offset: 0x0032A160
	public static T GetItemOfTypeAndQuality<T>(PickupObject.ItemQuality itemQuality, GenericLootTable lootTable, bool anyQuality = false) where T : PickupObject
	{
		List<T> list = new List<T>();
		if (lootTable != null)
		{
			List<WeightedGameObject> compiledRawItems = lootTable.GetCompiledRawItems();
			for (int i = 0; i < compiledRawItems.Count; i++)
			{
				if (!(compiledRawItems[i].gameObject == null))
				{
					T component = compiledRawItems[i].gameObject.GetComponent<T>();
					if (component != null && component.PrerequisitesMet())
					{
						EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
						if (component2 != null)
						{
							int num = GameStatsManager.Instance.QueryEncounterableDifferentiator(component2);
							if (num > 0)
							{
								goto IL_AF;
							}
						}
						list.Add(component);
					}
				}
				IL_AF:;
			}
		}
		else
		{
			for (int j = 0; j < PickupObjectDatabase.Instance.Objects.Count; j++)
			{
				T t = PickupObjectDatabase.Instance.Objects[j] as T;
				if (!(t is ContentTeaserGun) && !(t is ContentTeaserItem))
				{
					if (t != null && t.PrerequisitesMet())
					{
						EncounterTrackable component3 = t.GetComponent<EncounterTrackable>();
						if (component3 != null)
						{
							int num2 = GameStatsManager.Instance.QueryEncounterableDifferentiator(component3);
							if (num2 > 0)
							{
								goto IL_173;
							}
						}
						list.Add(t);
					}
				}
				IL_173:;
			}
		}
		if (list.Count == 0)
		{
			return (T)((object)null);
		}
		if (anyQuality)
		{
			if (list.Count > 0)
			{
				return list[UnityEngine.Random.Range(0, list.Count)];
			}
		}
		else
		{
			while (itemQuality >= PickupObject.ItemQuality.COMMON)
			{
				List<T> itemsOfQualityFromList = LootEngine.GetItemsOfQualityFromList<T>(list, itemQuality);
				if (itemsOfQualityFromList.Count > 0)
				{
					return itemsOfQualityFromList[UnityEngine.Random.Range(0, itemsOfQualityFromList.Count)];
				}
				itemQuality--;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x04008063 RID: 32867
	private const float HIGH_AMMO_THRESHOLD = 0.9f;

	// Token: 0x04008064 RID: 32868
	private const float LOW_AMMO_THRESHOLD = 0.1f;

	// Token: 0x04008065 RID: 32869
	private const float AMMO_DROP_CHANCE_REDUCTION_FACTOR = 0.05f;

	// Token: 0x0200155F RID: 5471
	public enum AmmoDropType
	{
		// Token: 0x0400806D RID: 32877
		DEFAULT_AMMO,
		// Token: 0x0400806E RID: 32878
		SPREAD_AMMO
	}
}
