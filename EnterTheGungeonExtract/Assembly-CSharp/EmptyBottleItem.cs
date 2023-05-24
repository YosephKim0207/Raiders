using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020013F5 RID: 5109
public class EmptyBottleItem : PlayerItem
{
	// Token: 0x17001170 RID: 4464
	// (get) Token: 0x060073EC RID: 29676 RVA: 0x002E1AD8 File Offset: 0x002DFCD8
	// (set) Token: 0x060073ED RID: 29677 RVA: 0x002E1AE0 File Offset: 0x002DFCE0
	public EmptyBottleItem.EmptyBottleContents Contents
	{
		get
		{
			return this.m_contents;
		}
		set
		{
			this.m_contents = value;
			this.UpdateSprite();
		}
	}

	// Token: 0x060073EE RID: 29678 RVA: 0x002E1AF0 File Offset: 0x002DFCF0
	public override bool CanBeUsed(PlayerController user)
	{
		if (this.m_contents == EmptyBottleItem.EmptyBottleContents.NONE)
		{
			if (!this.CanReallyBeUsed(user))
			{
				return false;
			}
		}
		else if (this.m_contents == EmptyBottleItem.EmptyBottleContents.ENEMY_SOUL)
		{
			if (user.CurrentRoom == null || !user.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
			{
				return false;
			}
		}
		else if (this.m_contents == EmptyBottleItem.EmptyBottleContents.FAIRY && user.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			return false;
		}
		return base.CanBeUsed(user);
	}

	// Token: 0x060073EF RID: 29679 RVA: 0x002E1B74 File Offset: 0x002DFD74
	private bool BottleFullCanBeConsumed(PlayerController user)
	{
		switch (this.m_contents)
		{
		case EmptyBottleItem.EmptyBottleContents.NONE:
			if (!this.CanReallyBeUsed(user))
			{
				return false;
			}
			break;
		case EmptyBottleItem.EmptyBottleContents.HALF_HEART:
		case EmptyBottleItem.EmptyBottleContents.FULL_HEART:
			if (user.healthHaver.GetCurrentHealthPercentage() >= 1f)
			{
				return false;
			}
			break;
		case EmptyBottleItem.EmptyBottleContents.AMMO:
			if (user.CurrentGun == null || user.CurrentGun.ammo == user.CurrentGun.AdjustedMaxAmmo || !user.CurrentGun.CanGainAmmo || user.CurrentGun.InfiniteAmmo)
			{
				return false;
			}
			break;
		case EmptyBottleItem.EmptyBottleContents.FAIRY:
			if (user.healthHaver.GetCurrentHealthPercentage() >= 1f)
			{
				return false;
			}
			break;
		case EmptyBottleItem.EmptyBottleContents.ENEMY_SOUL:
			if (user.CurrentRoom == null || !user.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
			{
				return false;
			}
			break;
		case EmptyBottleItem.EmptyBottleContents.SPREAD_AMMO:
			if (user.CurrentGun == null || user.CurrentGun.ammo == user.CurrentGun.AdjustedMaxAmmo || !user.CurrentGun.CanGainAmmo || user.CurrentGun.InfiniteAmmo)
			{
				return false;
			}
			break;
		}
		return true;
	}

	// Token: 0x060073F0 RID: 29680 RVA: 0x002E1CD8 File Offset: 0x002DFED8
	private bool CanReallyBeUsed(PlayerController user)
	{
		if (!user)
		{
			return false;
		}
		if (user.CurrentRoom != null)
		{
			List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor = activeEnemies[i];
					if (aiactor && aiactor.encounterTrackable && aiactor.encounterTrackable.journalData.PrimaryDisplayName == "#GUNFAIRY_ENCNAME")
					{
						return true;
					}
				}
			}
		}
		List<DebrisObject> allDebris = StaticReferenceManager.AllDebris;
		if (allDebris != null)
		{
			for (int j = 0; j < allDebris.Count; j++)
			{
				DebrisObject debrisObject = allDebris[j];
				if (debrisObject && debrisObject.IsPickupObject)
				{
					float sqrMagnitude = (user.CenterPosition - debrisObject.transform.position.XY()).sqrMagnitude;
					if (sqrMagnitude <= 25f)
					{
						HealthPickup component = debrisObject.GetComponent<HealthPickup>();
						AmmoPickup component2 = debrisObject.GetComponent<AmmoPickup>();
						KeyBulletPickup component3 = debrisObject.GetComponent<KeyBulletPickup>();
						SilencerItem component4 = debrisObject.GetComponent<SilencerItem>();
						if ((component && component.armorAmount == 0 && (component.healAmount == 0.5f || component.healAmount == 1f)) || component2 || component3 || component4)
						{
							float num = Mathf.Sqrt(sqrMagnitude);
							if (num < 5f)
							{
								return true;
							}
						}
					}
				}
			}
		}
		if (user)
		{
			IPlayerInteractable lastInteractable = user.GetLastInteractable();
			if (lastInteractable is HeartDispenser)
			{
				HeartDispenser heartDispenser = lastInteractable as HeartDispenser;
				if (heartDispenser && HeartDispenser.CurrentHalfHeartsStored > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060073F1 RID: 29681 RVA: 0x002E1EC8 File Offset: 0x002E00C8
	protected override void OnPreDrop(PlayerController user)
	{
		if (user)
		{
			user.OnReceivedDamage -= this.HandleOwnerTookDamage;
		}
		base.OnPreDrop(user);
	}

	// Token: 0x060073F2 RID: 29682 RVA: 0x002E1EF0 File Offset: 0x002E00F0
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnReceivedDamage += this.HandleOwnerTookDamage;
	}

	// Token: 0x060073F3 RID: 29683 RVA: 0x002E1F0C File Offset: 0x002E010C
	private void HandleOwnerTookDamage(PlayerController sourcePlayer)
	{
		if (sourcePlayer && sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.EMPTY_VESSELS, false) && this.Contents == EmptyBottleItem.EmptyBottleContents.NONE)
		{
			this.Contents = EmptyBottleItem.EmptyBottleContents.ENEMY_SOUL;
		}
	}

	// Token: 0x060073F4 RID: 29684 RVA: 0x002E1F3C File Offset: 0x002E013C
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add((int)this.Contents);
	}

	// Token: 0x060073F5 RID: 29685 RVA: 0x002E1F58 File Offset: 0x002E0158
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 1)
		{
			this.Contents = (EmptyBottleItem.EmptyBottleContents)data[0];
		}
	}

	// Token: 0x060073F6 RID: 29686 RVA: 0x002E1F80 File Offset: 0x002E0180
	private IEnumerator HandleSuck(tk2dSprite targetSprite)
	{
		float elapsed = 0f;
		float duration = 0.25f;
		PlayerController owner = this.LastOwner;
		if (targetSprite)
		{
			Vector3 startPosition = targetSprite.transform.position;
			while (elapsed < duration && owner)
			{
				elapsed += BraveTime.DeltaTime;
				if (targetSprite)
				{
					targetSprite.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), elapsed / duration);
					targetSprite.transform.position = Vector3.Lerp(startPosition, owner.CenterPosition.ToVector3ZisY(0f), elapsed / duration);
				}
				yield return null;
			}
		}
		UnityEngine.Object.Destroy(targetSprite.gameObject);
		yield break;
	}

	// Token: 0x060073F7 RID: 29687 RVA: 0x002E1FA4 File Offset: 0x002E01A4
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_bottle_cork_01", base.gameObject);
		if (this.Contents == EmptyBottleItem.EmptyBottleContents.NONE)
		{
			tk2dSpriteCollectionData tk2dSpriteCollectionData = null;
			int num = -1;
			Vector3 vector = Vector3.zero;
			AIActor aiactor = null;
			List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i] && activeEnemies[i].encounterTrackable && activeEnemies[i].encounterTrackable.journalData.PrimaryDisplayName == "#GUNFAIRY_ENCNAME")
					{
						aiactor = activeEnemies[i];
					}
				}
			}
			if (aiactor)
			{
				if (aiactor.sprite)
				{
					tk2dSpriteCollectionData = aiactor.sprite.Collection;
					num = aiactor.sprite.spriteId;
					vector = aiactor.transform.position;
				}
				aiactor.EraseFromExistence(false);
				this.Contents = EmptyBottleItem.EmptyBottleContents.FAIRY;
			}
			else
			{
				if (user)
				{
					IPlayerInteractable lastInteractable = user.GetLastInteractable();
					if (lastInteractable is HeartDispenser)
					{
						HeartDispenser heartDispenser = lastInteractable as HeartDispenser;
						if (heartDispenser && HeartDispenser.CurrentHalfHeartsStored > 0)
						{
							if (HeartDispenser.CurrentHalfHeartsStored > 1)
							{
								HeartDispenser.CurrentHalfHeartsStored -= 2;
								this.Contents = EmptyBottleItem.EmptyBottleContents.FULL_HEART;
							}
							else
							{
								HeartDispenser.CurrentHalfHeartsStored--;
								this.Contents = EmptyBottleItem.EmptyBottleContents.HALF_HEART;
							}
							return;
						}
					}
				}
				if (StaticReferenceManager.AllDebris != null)
				{
					DebrisObject debrisObject = null;
					float num2 = float.MaxValue;
					for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
					{
						DebrisObject debrisObject2 = StaticReferenceManager.AllDebris[j];
						if (debrisObject2.IsPickupObject)
						{
							float sqrMagnitude = (user.CenterPosition - debrisObject2.transform.position.XY()).sqrMagnitude;
							if (sqrMagnitude <= 25f)
							{
								HealthPickup component = debrisObject2.GetComponent<HealthPickup>();
								AmmoPickup component2 = debrisObject2.GetComponent<AmmoPickup>();
								KeyBulletPickup component3 = debrisObject2.GetComponent<KeyBulletPickup>();
								SilencerItem component4 = debrisObject2.GetComponent<SilencerItem>();
								if ((component && component.armorAmount == 0 && (component.healAmount == 0.5f || component.healAmount == 1f)) || component2 || component3 || component4)
								{
									float num3 = Mathf.Sqrt(sqrMagnitude);
									if (num3 < num2 && num3 < 5f)
									{
										num2 = num3;
										debrisObject = debrisObject2;
									}
								}
							}
						}
					}
					if (debrisObject)
					{
						HealthPickup component5 = debrisObject.GetComponent<HealthPickup>();
						AmmoPickup component6 = debrisObject.GetComponent<AmmoPickup>();
						KeyBulletPickup component7 = debrisObject.GetComponent<KeyBulletPickup>();
						SilencerItem component8 = debrisObject.GetComponent<SilencerItem>();
						if (component5)
						{
							if (component5.sprite)
							{
								tk2dSpriteCollectionData = component5.sprite.Collection;
								num = component5.sprite.spriteId;
								vector = component5.transform.position;
							}
							if (component5.armorAmount == 0 && component5.healAmount == 0.5f)
							{
								this.Contents = EmptyBottleItem.EmptyBottleContents.HALF_HEART;
								UnityEngine.Object.Destroy(component5.gameObject);
							}
							else if (component5.armorAmount == 0 && component5.healAmount == 1f)
							{
								this.Contents = EmptyBottleItem.EmptyBottleContents.FULL_HEART;
								UnityEngine.Object.Destroy(component5.gameObject);
							}
						}
						else if (component6)
						{
							if (component6.sprite)
							{
								tk2dSpriteCollectionData = component6.sprite.Collection;
								num = component6.sprite.spriteId;
								vector = component6.transform.position;
							}
							this.Contents = ((component6.mode != AmmoPickup.AmmoPickupMode.SPREAD_AMMO) ? EmptyBottleItem.EmptyBottleContents.AMMO : EmptyBottleItem.EmptyBottleContents.SPREAD_AMMO);
							UnityEngine.Object.Destroy(component6.gameObject);
						}
						else if (component7)
						{
							if (component7.sprite)
							{
								tk2dSpriteCollectionData = component7.sprite.Collection;
								num = component7.sprite.spriteId;
								vector = component7.transform.position;
							}
							this.Contents = EmptyBottleItem.EmptyBottleContents.KEY;
							UnityEngine.Object.Destroy(component7.gameObject);
						}
						else if (component8)
						{
							if (component8.sprite)
							{
								tk2dSpriteCollectionData = component8.sprite.Collection;
								num = component8.sprite.spriteId;
								vector = component8.transform.position;
							}
							this.Contents = EmptyBottleItem.EmptyBottleContents.BLANK;
							UnityEngine.Object.Destroy(component8.gameObject);
						}
					}
				}
			}
			if (tk2dSpriteCollectionData != null)
			{
				tk2dSprite tk2dSprite = tk2dSprite.AddComponent(new GameObject("sucked sprite")
				{
					transform = 
					{
						position = vector
					}
				}, tk2dSpriteCollectionData, num);
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleSuck(tk2dSprite));
			}
		}
		else if (this.BottleFullCanBeConsumed(user))
		{
			this.UseContainedItem(user);
		}
		else
		{
			this.ThrowContainedItem(user);
		}
	}

	// Token: 0x060073F8 RID: 29688 RVA: 0x002E24C8 File Offset: 0x002E06C8
	private void ThrowContainedItem(PlayerController user)
	{
		switch (this.Contents)
		{
		case EmptyBottleItem.EmptyBottleContents.HALF_HEART:
			LootEngine.SpawnHealth(user.CenterPosition, 1, new Vector2?(UnityEngine.Random.insideUnitCircle.normalized), 4f, 0.05f);
			break;
		case EmptyBottleItem.EmptyBottleContents.FULL_HEART:
			LootEngine.SpawnHealth(user.CenterPosition, 2, new Vector2?(UnityEngine.Random.insideUnitCircle.normalized), 4f, 0.05f);
			break;
		case EmptyBottleItem.EmptyBottleContents.AMMO:
			LootEngine.SpawnItem((GameObject)BraveResources.Load("Ammo_Pickup", ".prefab"), user.CenterPosition.ToVector3ZUp(0f), UnityEngine.Random.insideUnitCircle.normalized, 4f, true, false, false);
			break;
		case EmptyBottleItem.EmptyBottleContents.SPREAD_AMMO:
			LootEngine.SpawnItem((GameObject)BraveResources.Load("Ammo_Pickup_Spread", ".prefab"), user.CenterPosition.ToVector3ZUp(0f), UnityEngine.Random.insideUnitCircle.normalized, 4f, true, false, false);
			break;
		case EmptyBottleItem.EmptyBottleContents.BLANK:
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(GlobalItemIds.Blank).gameObject, user.CenterPosition.ToVector3ZUp(0f), UnityEngine.Random.insideUnitCircle.normalized, 4f, true, false, false);
			break;
		case EmptyBottleItem.EmptyBottleContents.KEY:
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(GlobalItemIds.Key).gameObject, user.CenterPosition.ToVector3ZUp(0f), UnityEngine.Random.insideUnitCircle.normalized, 4f, true, false, false);
			break;
		}
		this.Contents = EmptyBottleItem.EmptyBottleContents.NONE;
	}

	// Token: 0x060073F9 RID: 29689 RVA: 0x002E267C File Offset: 0x002E087C
	private void UseContainedItem(PlayerController user)
	{
		switch (this.Contents)
		{
		case EmptyBottleItem.EmptyBottleContents.HALF_HEART:
			user.healthHaver.ApplyHealing(0.5f);
			AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
			user.PlayEffectOnActor(this.HealVFX, Vector3.zero, true, false, false);
			break;
		case EmptyBottleItem.EmptyBottleContents.FULL_HEART:
			user.healthHaver.ApplyHealing(1f);
			AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
			user.PlayEffectOnActor(this.HealVFX, Vector3.zero, true, false, false);
			break;
		case EmptyBottleItem.EmptyBottleContents.AMMO:
			if (user.CurrentGun != null && user.CurrentGun.AdjustedMaxAmmo > 0)
			{
				user.CurrentGun.GainAmmo(user.CurrentGun.AdjustedMaxAmmo);
				user.CurrentGun.ForceImmediateReload(false);
				AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
				user.PlayEffectOnActor(this.AmmoVFX, Vector3.zero, true, false, false);
				string @string = StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_HEADER");
				string text = user.CurrentGun.GetComponent<EncounterTrackable>().journalData.GetPrimaryDisplayName(false) + " " + StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_BODY");
				tk2dBaseSprite sprite = user.CurrentGun.GetSprite();
				if (!GameUIRoot.Instance.BossHealthBarVisible)
				{
					GameUIRoot.Instance.notificationController.DoCustomNotification(@string, text, sprite.Collection, sprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
				}
			}
			break;
		case EmptyBottleItem.EmptyBottleContents.FAIRY:
			AkSoundEngine.PostEvent("Play_NPC_faerie_heal_01", base.gameObject);
			user.PlayFairyEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Fairy_Fly") as GameObject, Vector3.zero, 4.5f, true);
			user.StartCoroutine(this.HandleHearts(user));
			break;
		case EmptyBottleItem.EmptyBottleContents.ENEMY_SOUL:
			user.CurrentRoom.ApplyActionToNearbyEnemies(user.transform.position.XY(), 100f, new Action<AIActor, float>(this.SoulProcessEnemy));
			break;
		case EmptyBottleItem.EmptyBottleContents.SPREAD_AMMO:
		{
			float num = 0.5f;
			float num2 = 0.2f;
			user.CurrentGun.GainAmmo(Mathf.CeilToInt((float)user.CurrentGun.AdjustedMaxAmmo * num));
			for (int i = 0; i < user.inventory.AllGuns.Count; i++)
			{
				if (user.inventory.AllGuns[i] && user.CurrentGun != user.inventory.AllGuns[i])
				{
					user.inventory.AllGuns[i].GainAmmo(Mathf.FloorToInt((float)user.inventory.AllGuns[i].AdjustedMaxAmmo * num2));
				}
			}
			user.CurrentGun.ForceImmediateReload(false);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(user);
				if (!otherPlayer.IsGhost)
				{
					for (int j = 0; j < otherPlayer.inventory.AllGuns.Count; j++)
					{
						if (otherPlayer.inventory.AllGuns[j])
						{
							otherPlayer.inventory.AllGuns[j].GainAmmo(Mathf.FloorToInt((float)otherPlayer.inventory.AllGuns[j].AdjustedMaxAmmo * num2));
						}
					}
					otherPlayer.CurrentGun.ForceImmediateReload(false);
				}
			}
			AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
			user.PlayEffectOnActor(this.AmmoVFX, Vector3.zero, true, false, false);
			string string2 = StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_HEADER");
			string string3 = StringTableManager.GetString("#AMMO_SPREAD_REFILLED_BODY");
			tk2dBaseSprite sprite2 = user.CurrentGun.GetSprite();
			if (!GameUIRoot.Instance.BossHealthBarVisible)
			{
				GameUIRoot.Instance.notificationController.DoCustomNotification(string2, string3, sprite2.Collection, sprite2.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
			}
			break;
		}
		case EmptyBottleItem.EmptyBottleContents.BLANK:
			user.Blanks++;
			break;
		case EmptyBottleItem.EmptyBottleContents.KEY:
			user.carriedConsumables.KeyBullets = user.carriedConsumables.KeyBullets + 1;
			break;
		}
		this.Contents = EmptyBottleItem.EmptyBottleContents.NONE;
	}

	// Token: 0x060073FA RID: 29690 RVA: 0x002E2AC8 File Offset: 0x002E0CC8
	private void SoulProcessEnemy(AIActor a, float distance)
	{
		if (a && a.IsNormalEnemy && a.healthHaver && !a.IsGone)
		{
			if (this.LastOwner)
			{
				a.healthHaver.ApplyDamage(this.SoulDamage, Vector2.zero, this.LastOwner.ActorName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			else
			{
				a.healthHaver.ApplyDamage(this.SoulDamage, Vector2.zero, "projectile", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
			if (this.OnBurstDamageVFX)
			{
				a.PlayEffectOnActor(this.OnBurstDamageVFX, Vector3.zero, true, false, false);
			}
		}
	}

	// Token: 0x060073FB RID: 29691 RVA: 0x002E2B88 File Offset: 0x002E0D88
	private IEnumerator HandleHearts(PlayerController targetPlayer)
	{
		float duration = 4.5f;
		int halfHeartsToHeal = Mathf.RoundToInt((targetPlayer.healthHaver.GetMaxHealth() - targetPlayer.healthHaver.GetCurrentHealth()) / 0.5f);
		float timeStep = duration / (float)halfHeartsToHeal;
		while (targetPlayer.healthHaver.GetCurrentHealth() < targetPlayer.healthHaver.GetMaxHealth())
		{
			targetPlayer.healthHaver.ApplyHealing(0.5f);
			yield return new WaitForSeconds(timeStep);
		}
		yield break;
	}

	// Token: 0x060073FC RID: 29692 RVA: 0x002E2BA4 File Offset: 0x002E0DA4
	private void UpdateSprite()
	{
		switch (this.Contents)
		{
		case EmptyBottleItem.EmptyBottleContents.NONE:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.EmptySprite);
			break;
		case EmptyBottleItem.EmptyBottleContents.HALF_HEART:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.ContainsHalfHeartSprite);
			break;
		case EmptyBottleItem.EmptyBottleContents.FULL_HEART:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.ContainsHeartSprite);
			break;
		case EmptyBottleItem.EmptyBottleContents.AMMO:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.ContainsAmmoSprite);
			break;
		case EmptyBottleItem.EmptyBottleContents.FAIRY:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.ContainsFairySprite);
			break;
		case EmptyBottleItem.EmptyBottleContents.ENEMY_SOUL:
			base.sprite.SetSprite(this.ContainsSoulSprite);
			base.spriteAnimator.Play("empty_bottle_soul");
			break;
		case EmptyBottleItem.EmptyBottleContents.SPREAD_AMMO:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.ContainsSpreadAmmoSprite);
			break;
		case EmptyBottleItem.EmptyBottleContents.BLANK:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.ContainsBlankSprite);
			break;
		case EmptyBottleItem.EmptyBottleContents.KEY:
			base.spriteAnimator.Stop();
			base.sprite.SetSprite(this.ContainsKeySprite);
			break;
		}
	}

	// Token: 0x060073FD RID: 29693 RVA: 0x002E2D20 File Offset: 0x002E0F20
	protected override void CopyStateFrom(PlayerItem other)
	{
		base.CopyStateFrom(other);
		EmptyBottleItem emptyBottleItem = other as EmptyBottleItem;
		if (emptyBottleItem)
		{
			this.m_contents = emptyBottleItem.m_contents;
			base.sprite.SetSprite(emptyBottleItem.sprite.spriteId);
		}
	}

	// Token: 0x060073FE RID: 29694 RVA: 0x002E2D68 File Offset: 0x002E0F68
	protected override void OnDestroy()
	{
		if (this.LastOwner)
		{
			this.LastOwner.OnReceivedDamage -= this.HandleOwnerTookDamage;
		}
		base.OnDestroy();
	}

	// Token: 0x0400757D RID: 30077
	private EmptyBottleItem.EmptyBottleContents m_contents;

	// Token: 0x0400757E RID: 30078
	public string EmptySprite;

	// Token: 0x0400757F RID: 30079
	public string ContainsHeartSprite;

	// Token: 0x04007580 RID: 30080
	public string ContainsHalfHeartSprite;

	// Token: 0x04007581 RID: 30081
	public string ContainsAmmoSprite;

	// Token: 0x04007582 RID: 30082
	public string ContainsFairySprite;

	// Token: 0x04007583 RID: 30083
	public string ContainsSoulSprite;

	// Token: 0x04007584 RID: 30084
	public string ContainsSpreadAmmoSprite;

	// Token: 0x04007585 RID: 30085
	public string ContainsBlankSprite;

	// Token: 0x04007586 RID: 30086
	public string ContainsKeySprite;

	// Token: 0x04007587 RID: 30087
	public GameObject HealVFX;

	// Token: 0x04007588 RID: 30088
	public GameObject AmmoVFX;

	// Token: 0x04007589 RID: 30089
	public GameObject FairyVFX;

	// Token: 0x0400758A RID: 30090
	public float SoulDamage = 30f;

	// Token: 0x0400758B RID: 30091
	public GameObject OnBurstDamageVFX;

	// Token: 0x020013F6 RID: 5110
	public enum EmptyBottleContents
	{
		// Token: 0x0400758D RID: 30093
		NONE,
		// Token: 0x0400758E RID: 30094
		HALF_HEART,
		// Token: 0x0400758F RID: 30095
		FULL_HEART,
		// Token: 0x04007590 RID: 30096
		AMMO,
		// Token: 0x04007591 RID: 30097
		FAIRY,
		// Token: 0x04007592 RID: 30098
		ENEMY_SOUL,
		// Token: 0x04007593 RID: 30099
		SPREAD_AMMO,
		// Token: 0x04007594 RID: 30100
		BLANK,
		// Token: 0x04007595 RID: 30101
		KEY
	}
}
