using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200137B RID: 4987
public class CompanionController : BraveBehaviour
{
	// Token: 0x060070FD RID: 28925 RVA: 0x002CD3F0 File Offset: 0x002CB5F0
	private IEnumerator HandleDelayedInitialization()
	{
		yield return null;
		if (this.CanCrossPits)
		{
			base.aiActor.PathableTiles = base.aiActor.PathableTiles | CellTypes.PIT;
			base.aiActor.SetIsFlying(true, "innate", false, false);
		}
		yield break;
	}

	// Token: 0x060070FE RID: 28926 RVA: 0x002CD40C File Offset: 0x002CB60C
	public void Initialize(PlayerController owner)
	{
		this.m_owner = owner;
		base.gameActor.ImmuneToAllEffects = true;
		base.aiActor.SetResistance(EffectResistanceType.Poison, 1f);
		base.aiActor.SetResistance(EffectResistanceType.Fire, 1f);
		base.aiActor.SetResistance(EffectResistanceType.Freeze, 1f);
		base.aiActor.SetResistance(EffectResistanceType.Charm, 1f);
		base.aiActor.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.PlayerCollider));
		if (this.companionID == CompanionController.CompanionIdentifier.GATLING_GULL)
		{
			base.aiActor.specRigidbody.AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyCollider));
		}
		base.aiActor.IsNormalEnemy = false;
		base.aiActor.CompanionOwner = this.m_owner;
		base.aiActor.CanTargetPlayers = false;
		base.aiActor.CanTargetEnemies = true;
		base.aiActor.CustomPitDeathHandling += this.CustomPitDeathHandling;
		base.aiActor.State = AIActor.ActorState.Normal;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
		if (base.bulletBank)
		{
			AIBulletBank bulletBank = base.bulletBank;
			bulletBank.OnProjectileCreated = (Action<Projectile>)Delegate.Combine(bulletBank.OnProjectileCreated, new Action<Projectile>(this.MarkNondamaging));
		}
		if (!this.CanInterceptBullets)
		{
			base.specRigidbody.HitboxPixelCollider.IsTrigger = true;
			base.specRigidbody.HitboxPixelCollider.CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);
		}
		if (this.IsCop)
		{
			base.healthHaver.ManualDeathHandling = true;
			base.healthHaver.OnPreDeath += this.HandleCopDeath;
			HealthHaver healthHaver = base.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.HandleCopModifyDamage));
		}
		if (this.PredictsChests)
		{
			this.m_hologram = base.GetComponentInChildren<HologramDoer>();
		}
		if (base.bulletBank)
		{
			AIBulletBank bulletBank2 = base.bulletBank;
			bulletBank2.OnProjectileCreated = (Action<Projectile>)Delegate.Combine(bulletBank2.OnProjectileCreated, new Action<Projectile>(this.HandleCompanionPostProcessProjectile));
		}
		if (base.aiShooter)
		{
			AIShooter aiShooter = base.aiShooter;
			aiShooter.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(aiShooter.PostProcessProjectile, new Action<Projectile>(this.HandleCompanionPostProcessProjectile));
		}
		if (this.BlanksOnActiveItemUsed)
		{
			owner.OnUsedPlayerItem += this.HandleItemUsed;
		}
		owner.OnPitfall += this.HandlePitfall;
		owner.OnRoomClearEvent += this.HandleRoomCleared;
		owner.companions.Add(base.aiActor);
		base.StartCoroutine(this.HandleDelayedInitialization());
	}

	// Token: 0x060070FF RID: 28927 RVA: 0x002CD6D8 File Offset: 0x002CB8D8
	private void HandleCopModifyDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
	{
		if (args == EventArgs.Empty)
		{
			return;
		}
		if (this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.COP_VEST, false))
		{
			args.ModifiedDamage /= 2f;
		}
	}

	// Token: 0x06007100 RID: 28928 RVA: 0x002CD728 File Offset: 0x002CB928
	protected virtual void HandleRoomCleared(PlayerController callingPlayer)
	{
	}

	// Token: 0x06007101 RID: 28929 RVA: 0x002CD72C File Offset: 0x002CB92C
	protected void MarkNondamaging(Projectile obj)
	{
		if (obj)
		{
			obj.collidesWithPlayer = false;
		}
	}

	// Token: 0x06007102 RID: 28930 RVA: 0x002CD740 File Offset: 0x002CB940
	protected void HandlePitfall()
	{
	}

	// Token: 0x06007103 RID: 28931 RVA: 0x002CD744 File Offset: 0x002CB944
	protected void HandleCompanionPostProcessProjectile(Projectile obj)
	{
		if (obj)
		{
			obj.collidesWithPlayer = false;
			obj.TreatedAsNonProjectileForChallenge = true;
		}
		if (this.m_owner)
		{
			if (PassiveItem.IsFlagSetForCharacter(this.m_owner, typeof(BattleStandardItem)))
			{
				obj.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
			}
			if (this.m_owner.CurrentGun && this.m_owner.CurrentGun.LuteCompanionBuffActive)
			{
				obj.baseData.damage *= 2f;
				obj.RuntimeUpdateScale(1f / obj.AdditionalScaleMultiplier);
				obj.RuntimeUpdateScale(1.75f);
			}
			this.m_owner.DoPostProcessProjectile(obj);
		}
	}

	// Token: 0x06007104 RID: 28932 RVA: 0x002CD818 File Offset: 0x002CBA18
	protected void HandleItemUsed(PlayerController arg1, PlayerItem arg2)
	{
		if (arg1.HasActiveBonusSynergy(CustomSynergyType.ELDER_AND_YOUNGER, false))
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleDelayedBlank(arg1));
		}
		else if (this.m_internalBlankCooldown <= 0f)
		{
			Vector2? vector = new Vector2?(base.sprite.WorldCenter);
			arg1.ForceBlank(25f, 0.5f, false, true, vector, true, -1f);
			this.m_internalBlankCooldown = this.InternalBlankCooldown;
		}
	}

	// Token: 0x06007105 RID: 28933 RVA: 0x002CD898 File Offset: 0x002CBA98
	private IEnumerator HandleDelayedBlank(PlayerController arg1)
	{
		yield return new WaitForSeconds(1f);
		if (this.m_internalBlankCooldown <= 0f)
		{
			Vector2? vector = new Vector2?(base.sprite.WorldCenter);
			arg1.ForceBlank(25f, 0.5f, false, true, vector, true, -1f);
			this.m_internalBlankCooldown = this.InternalBlankCooldown;
		}
		yield break;
	}

	// Token: 0x06007106 RID: 28934 RVA: 0x002CD8BC File Offset: 0x002CBABC
	protected void HandleCopDeath(Vector2 obj)
	{
		base.StartCoroutine(this.HandleCopDeath_CR());
	}

	// Token: 0x06007107 RID: 28935 RVA: 0x002CD8CC File Offset: 0x002CBACC
	public virtual void Update()
	{
		if (!GameManager.Instance || GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (this.IsBeingPet && (!this.m_pettingDoer || this.m_pettingDoer.m_pettingTarget != this || !base.aiAnimator.IsPlaying("pet") || Vector2.Distance(base.specRigidbody.UnitCenter, this.m_pettingDoer.specRigidbody.UnitCenter) > 3f))
		{
			this.StopPet();
		}
		if (!this.m_hasDoneJunkanCheck)
		{
			if (this.m_owner.companions.Count >= 2)
			{
				int num = 0;
				for (int i = 0; i < this.m_owner.companions.Count; i++)
				{
					if (this.m_owner.companions[i])
					{
						num++;
					}
				}
				if (num >= 2)
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SER_JUNKAN_MAXLVL, true);
				}
			}
			this.m_hasDoneJunkanCheck = true;
		}
		if (this.m_internalBlankCooldown > 0f)
		{
			this.m_internalBlankCooldown -= BraveTime.DeltaTime;
		}
		if (this.BlanksOnActiveItemUsed && this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.MY_LITTLE_FRIEND, false))
		{
			if (!this.m_hasAttemptedSynergy && this.m_owner.CurrentGun && this.m_owner.CurrentGun.ClipShotsRemaining == 0)
			{
				this.m_hasAttemptedSynergy = true;
				if (UnityEngine.Random.value < 0.25f)
				{
					this.HandleItemUsed(this.m_owner, null);
				}
			}
			else if (this.m_hasAttemptedSynergy && this.m_owner.CurrentGun && this.m_owner.CurrentGun.ClipShotsRemaining != 0)
			{
				this.m_hasAttemptedSynergy = false;
			}
		}
		if (this.companionID == CompanionController.CompanionIdentifier.SUPER_SPACE_TURTLE && this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.OUTER_TURTLE, false) && !base.aiActor.IsBlackPhantom)
		{
			base.aiActor.BecomeBlackPhantom();
		}
		if (this.HasStealthMode && this.m_owner)
		{
			if (this.m_owner.IsStealthed && !this.m_isStealthed)
			{
				this.m_isStealthed = true;
				base.aiAnimator.IdleAnimation.AnimNames[0] = "sst_dis_idle_right";
				base.aiAnimator.IdleAnimation.AnimNames[1] = "sst_dis_idle_left";
				base.aiAnimator.MoveAnimation.AnimNames[0] = "sst_dis_move_right";
				base.aiAnimator.MoveAnimation.AnimNames[1] = "sst_dis_move_left";
			}
			else if (!this.m_owner.IsStealthed && this.m_isStealthed)
			{
				this.m_isStealthed = false;
				base.aiAnimator.IdleAnimation.AnimNames[0] = "sst_idle_right";
				base.aiAnimator.IdleAnimation.AnimNames[1] = "sst_idle_left";
				base.aiAnimator.MoveAnimation.AnimNames[0] = "sst_move_right";
				base.aiAnimator.MoveAnimation.AnimNames[1] = "sst_move_left";
			}
		}
		if (this.m_owner && base.sprite && this.m_owner.CurrentGun && this.m_owner.CurrentGun.IsReloading && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.TEA_FOR_TWO, false))
		{
			AuraOnReloadModifier component = this.m_owner.CurrentGun.GetComponent<AuraOnReloadModifier>();
			if (this.TeaSynergyHeatRing == null)
			{
				this.TeaSynergyHeatRing = new HeatRingModule();
			}
			if (this.TeaSynergyHeatRing != null && !this.TeaSynergyHeatRing.IsActive && component && component.IgnitesEnemies && this.m_owner && this.m_owner.CurrentGun && base.sprite)
			{
				this.TeaSynergyHeatRing.Trigger(component.AuraRadius, this.m_owner.CurrentGun.reloadTime, component.IgniteEffect, base.sprite);
			}
		}
		if (this.m_owner && this.companionID == CompanionController.CompanionIdentifier.SHELLETON)
		{
			bool flag = this.m_owner.HasActiveBonusSynergy(CustomSynergyType.BIRTHRIGHT, false);
			ShootBehavior shootBehavior = (ShootBehavior)base.behaviorSpeculator.AttackBehaviors[1];
			shootBehavior.IsBlackPhantom = !flag;
			flag = this.m_owner.HasActiveBonusSynergy(CustomSynergyType.SHELL_A_TON, false);
			base.behaviorSpeculator.LocalTimeScale = ((!flag) ? 1f : 2f);
		}
		if (this.IsCop && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
		{
			CellData cell = base.transform.position.GetCell();
			if (base.transform.position.GetAbsoluteRoom().area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				base.healthHaver.ApplyDamage(1000000f, Vector2.zero, "Inevitability", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
			}
			else if (base.transform.position.GetAbsoluteRoom().HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear) && this.m_owner.CurrentRoom.distanceFromEntrance > 1 && (cell == null || !cell.isExitCell))
			{
				if (this.m_timeInDeadlyRoom > 5f && Vector2.Distance(this.m_owner.CenterPosition, base.transform.position.XY()) < 12f)
				{
					base.healthHaver.ApplyDamage(1000000f, Vector2.zero, "Inevitability", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
				}
				else
				{
					this.m_timeInDeadlyRoom += BraveTime.DeltaTime;
				}
			}
			else
			{
				this.m_timeInDeadlyRoom = 0f;
			}
		}
		IntVector2 intVector = base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
		if (GameManager.Instance.Dungeon.data.CheckInBounds(intVector))
		{
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			if (cellData != null)
			{
				this.m_lastCellType = cellData.type;
			}
		}
		if (this.PredictsChests && this.m_owner && this.m_owner.HasActiveBonusSynergy(this.PredictsChestSynergy, false))
		{
			Chest chest = null;
			float num2 = float.MaxValue;
			for (int j = 0; j < StaticReferenceManager.AllChests.Count; j++)
			{
				Chest chest2 = StaticReferenceManager.AllChests[j];
				if (chest2 && chest2.sprite && !chest2.IsOpen && !chest2.IsBroken && !chest2.IsSealed)
				{
					float num3 = Vector2.Distance(this.m_owner.CenterPosition, chest2.sprite.WorldCenter);
					if (num3 < num2)
					{
						chest = chest2;
						num2 = num3;
					}
				}
			}
			if (num2 > 5f)
			{
				chest = null;
			}
			if (this.m_lastPredictedChest != chest)
			{
				if (this.m_lastPredictedChest)
				{
					this.m_hologram.HideSprite(base.gameObject, false);
				}
				if (chest)
				{
					List<PickupObject> list = chest.PredictContents(this.m_owner);
					if (list.Count > 0 && list[0].encounterTrackable)
					{
						tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.ForceInstance.EncounterIconCollection;
						this.m_hologram.ChangeToSprite(base.gameObject, encounterIconCollection, encounterIconCollection.GetSpriteIdByName(list[0].encounterTrackable.journalData.AmmonomiconSprite));
					}
				}
				this.m_lastPredictedChest = chest;
			}
		}
		else if (this.m_hologram && this.m_hologram.ArcRenderer.enabled)
		{
			this.m_hologram.HideSprite(base.gameObject, true);
		}
		if (this.companionID == CompanionController.CompanionIdentifier.BABY_GOOD_MIMIC && !this.m_isMimicTransforming)
		{
			this.HandleBabyGoodMimic();
		}
		if (this.m_owner && this.m_owner.CurrentGun && base.aiActor)
		{
			if (this.m_hasLuteBuff && !this.m_owner.CurrentGun.LuteCompanionBuffActive)
			{
				if (this.m_luteOverheadVfx)
				{
					UnityEngine.Object.Destroy(this.m_luteOverheadVfx);
					this.m_luteOverheadVfx = null;
				}
				this.m_hasLuteBuff = false;
			}
			else if (!this.m_hasLuteBuff && this.m_owner.CurrentGun.LuteCompanionBuffActive)
			{
				GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Buff_Status");
				this.m_luteOverheadVfx = base.aiActor.PlayEffectOnActor(gameObject, new Vector3(0f, 1.25f, 0f), true, true, false);
				this.m_hasLuteBuff = true;
			}
		}
	}

	// Token: 0x06007108 RID: 28936 RVA: 0x002CE258 File Offset: 0x002CC458
	protected void HandleBabyGoodMimic()
	{
		if (this.m_owner)
		{
			CompanionItem companionItem = null;
			string text = string.Empty;
			for (int i = 0; i < this.m_owner.passiveItems.Count; i++)
			{
				if (this.m_owner.passiveItems[i] is CompanionItem)
				{
					companionItem = this.m_owner.passiveItems[i] as CompanionItem;
					if (!(companionItem.ExtantCompanion != base.gameObject))
					{
						break;
					}
					companionItem = null;
				}
			}
			for (int j = 0; j < this.m_owner.companions.Count; j++)
			{
				CompanionController component = this.m_owner.companions[j].GetComponent<CompanionController>();
				if (!component || component.companionID != CompanionController.CompanionIdentifier.GATLING_GULL)
				{
					if (!component || component.companionID != CompanionController.CompanionIdentifier.BABY_GOOD_MIMIC)
					{
						text = this.m_owner.companions[j].EnemyGuid;
						break;
					}
				}
			}
			PlayerOrbitalItem playerOrbitalItem = null;
			if (string.IsNullOrEmpty(text))
			{
				for (int k = 0; k < this.m_owner.passiveItems.Count; k++)
				{
					if (this.m_owner.passiveItems[k] is PlayerOrbitalItem)
					{
						PlayerOrbitalItem playerOrbitalItem2 = this.m_owner.passiveItems[k] as PlayerOrbitalItem;
						if (playerOrbitalItem2.CanBeMimicked)
						{
							playerOrbitalItem = playerOrbitalItem2;
							break;
						}
					}
				}
			}
			if (companionItem && (!string.IsNullOrEmpty(text) || playerOrbitalItem != null))
			{
				base.StartCoroutine(this.HandleBabyMimicTransform(companionItem, text, playerOrbitalItem));
			}
		}
	}

	// Token: 0x06007109 RID: 28937 RVA: 0x002CE438 File Offset: 0x002CC638
	private IEnumerator HandleBabyMimicTransform(CompanionItem sourceItem, string targetGuid, PlayerOrbitalItem orbitalItemTarget = null)
	{
		this.m_isMimicTransforming = true;
		base.behaviorSpeculator.enabled = false;
		base.aiAnimator.PlayUntilFinished("transform", false, null, -1f, false);
		yield return new WaitForSeconds(1.4f);
		Vector2 sourcePosition = base.transform.position;
		sourceItem.ForceDisconnectCompanion();
		if (string.IsNullOrEmpty(targetGuid) && orbitalItemTarget)
		{
			sourceItem.BabyGoodMimicOrbitalOverridden = true;
			sourceItem.OverridePlayerOrbitalItem = orbitalItemTarget;
		}
		else
		{
			sourceItem.CompanionGuid = targetGuid;
			sourceItem.CompanionPastGuid = string.Empty;
		}
		sourceItem.UsesAlternatePastPrefab = false;
		sourceItem.ForceCompanionRegeneration(this.m_owner, new Vector2?(sourcePosition));
		yield return new WaitForSeconds(0.5f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600710A RID: 28938 RVA: 0x002CE468 File Offset: 0x002CC668
	protected bool PlayerRoomHasActiveEnemies()
	{
		bool flag = base.transform.position.GetAbsoluteRoom().HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (!flag)
		{
			flag = GameManager.Instance.PrimaryPlayer.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
		}
		return flag;
	}

	// Token: 0x0600710B RID: 28939 RVA: 0x002CE4AC File Offset: 0x002CC6AC
	private IEnumerator HandleCopDeath_CR()
	{
		this.IsCopDead = true;
		for (int i = 0; i < this.m_owner.passiveItems.Count; i++)
		{
			if (this.m_owner.passiveItems[i] is CompanionItem)
			{
				CompanionItem companionItem = this.m_owner.passiveItems[i] as CompanionItem;
				if (companionItem && companionItem.CompanionGuid == base.aiActor.EnemyGuid)
				{
					companionItem.PreventRespawnOnFloorLoad = true;
				}
			}
		}
		this.m_owner.companions.Remove(base.aiActor);
		base.sprite.HeightOffGround = -1f;
		base.sprite.UpdateZDepth();
		base.aiAnimator.PlayUntilCancelled("die", false, null, -1f, false);
		if (base.knockbackDoer)
		{
			base.knockbackDoer.SetImmobile(true, "dying");
		}
		bool playerRoomHasActiveEnemies = true;
		while (playerRoomHasActiveEnemies)
		{
			yield return null;
			playerRoomHasActiveEnemies = this.PlayerRoomHasActiveEnemies();
			if (!playerRoomHasActiveEnemies)
			{
				while (GameManager.Instance.MainCameraController.ManualControl)
				{
					yield return null;
				}
				yield return new WaitForSeconds(0.5f);
				playerRoomHasActiveEnemies = this.PlayerRoomHasActiveEnemies();
			}
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON && GameManager.Instance.PrimaryPlayer.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				break;
			}
		}
		yield return null;
		if (Vector2.Distance(this.m_owner.CenterPosition, base.sprite.WorldCenter) < 15f)
		{
			base.GetDungeonFSM().SendEvent("copdeath");
			yield return null;
		}
		GameObject instanceIcon = Minimap.Instance.RegisterRoomIcon(base.talkDoer.transform.position.GetAbsoluteRoom(), ResourceCache.Acquire("Global Prefabs/Minimap_NPC_Icon") as GameObject, false);
		bool didBreak = false;
		while (!base.talkDoer.IsTalking)
		{
			yield return null;
		}
		Minimap.Instance.DeregisterRoomIcon(base.talkDoer.transform.position.GetAbsoluteRoom(), instanceIcon);
		if (!didBreak)
		{
			while (base.talkDoer.IsTalking)
			{
				yield return null;
			}
		}
		if (didBreak)
		{
			while (this.m_owner && this.m_owner.IsInCombat)
			{
				yield return null;
			}
		}
		this.m_owner.ownerlessStatModifiers.Add(this.CopDeathStatModifier);
		StatModifier curseMod = new StatModifier();
		curseMod.statToBoost = PlayerStats.StatType.Curse;
		curseMod.modifyType = StatModifier.ModifyMethod.ADDITIVE;
		curseMod.amount = (float)this.CurseOnCopDeath;
		this.m_owner.ownerlessStatModifiers.Add(curseMod);
		this.m_owner.stats.RecalculateStats(this.m_owner, false, false);
		yield return new WaitForSeconds(0.25f);
		string header = StringTableManager.GetString("#COP_REVENGE_HEADER");
		string body = StringTableManager.GetString("#COP_REVENGE_BODY");
		GameUIRoot.Instance.notificationController.DoCustomNotification(header, body, base.sprite.Collection, base.sprite.spriteId, UINotificationController.NotificationColor.GOLD, false, false);
		base.healthHaver.DeathAnimationComplete(null, null);
		for (int j = 0; j < this.m_owner.passiveItems.Count; j++)
		{
			if (this.m_owner.passiveItems[j] is CompanionItem)
			{
				CompanionItem companionItem2 = this.m_owner.passiveItems[j] as CompanionItem;
				if (companionItem2 && companionItem2.CompanionGuid == base.aiActor.EnemyGuid)
				{
					this.m_owner.RemovePassiveItem(companionItem2.PickupObjectId);
					break;
				}
			}
		}
		yield break;
	}

	// Token: 0x0600710C RID: 28940 RVA: 0x002CE4C8 File Offset: 0x002CC6C8
	protected void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody.transform.parent != null && (otherRigidbody.transform.parent.GetComponent<DungeonDoorController>() || otherRigidbody.transform.parent.GetComponent<DungeonDoorSubsidiaryBlocker>()))
		{
			PhysicsEngine.SkipCollision = true;
		}
		if (this.IsCop && this.IsCopDead)
		{
			PhysicsEngine.SkipCollision = true;
		}
		if (GameManager.Instance.IsFoyer && otherRigidbody.GetComponent<TalkDoerLite>())
		{
			PhysicsEngine.SkipCollision = true;
		}
		if (this.companionID == CompanionController.CompanionIdentifier.GATLING_GULL && otherRigidbody.aiActor && otherRigidbody.healthHaver && !otherRigidbody.healthHaver.IsBoss)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x0600710D RID: 28941 RVA: 0x002CE5AC File Offset: 0x002CC7AC
	private void CustomPitDeathHandling(AIActor actor, ref bool suppressDamage)
	{
		suppressDamage = true;
		if (this.m_owner && this.m_owner.IsInMinecart)
		{
			base.StartCoroutine(this.DelayedPitReturn());
		}
		else
		{
			base.transform.position = this.m_owner.transform.position;
			base.specRigidbody.Reinitialize();
			base.aiActor.RecoverFromFall();
		}
	}

	// Token: 0x0600710E RID: 28942 RVA: 0x002CE620 File Offset: 0x002CC820
	private IEnumerator DelayedPitReturn()
	{
		while (this.m_owner.IsInMinecart)
		{
			yield return null;
		}
		base.transform.position = this.m_owner.transform.position;
		base.specRigidbody.Reinitialize();
		base.aiActor.RecoverFromFall();
		yield break;
	}

	// Token: 0x0600710F RID: 28943 RVA: 0x002CE63C File Offset: 0x002CC83C
	private IEnumerator ScoopPlayerToSafety()
	{
		RoomHandler currentRoom = this.m_owner.CurrentRoom;
		if (currentRoom.area.PrototypeRoomNormalSubcategory != PrototypeDungeonRoom.RoomNormalSubCategory.TRAP)
		{
			yield break;
		}
		bool hasFoundExit = false;
		float maxDistance = float.MinValue;
		IntVector2 mostDistantExit = IntVector2.NegOne;
		for (int i = 0; i < currentRoom.connectedRooms.Count; i++)
		{
			PrototypeRoomExit exitConnectedToRoom = currentRoom.GetExitConnectedToRoom(currentRoom.connectedRooms[i]);
			if (exitConnectedToRoom != null)
			{
				IntVector2 intVector = exitConnectedToRoom.GetExitAttachPoint() - IntVector2.One;
				IntVector2 intVector2 = intVector + currentRoom.area.basePosition + DungeonData.GetIntVector2FromDirection(exitConnectedToRoom.exitDirection);
				hasFoundExit = true;
				float num = Vector2.Distance(this.m_owner.CenterPosition, intVector2.ToCenterVector2());
				if (num > maxDistance)
				{
					maxDistance = num;
					mostDistantExit = intVector2;
				}
			}
		}
		if (!hasFoundExit)
		{
			yield break;
		}
		CompanionFollowPlayerBehavior followBehavior = base.behaviorSpeculator.MovementBehaviors[0] as CompanionFollowPlayerBehavior;
		followBehavior.TemporarilyDisabled = true;
		base.aiActor.ClearPath();
		base.sprite.SpriteChanged += this.UpdatePlayerPosition;
		base.aiAnimator.PlayUntilFinished("grab", false, null, -1f, false);
		yield return null;
		Transform attachPoint = base.transform.Find("carry");
		while (base.aiAnimator.IsPlaying("grab"))
		{
			Vector2 preferredPrimaryPosition = attachPoint.position.XY() + (this.m_owner.transform.position.XY() - this.m_owner.sprite.WorldBottomCenter) + new Vector2(0f, -0.125f);
			this.m_owner.transform.position = preferredPrimaryPosition;
			this.m_owner.specRigidbody.Reinitialize();
			yield return null;
		}
		float cachedSpeed = base.aiActor.MovementSpeed;
		base.aiActor.MovementSpeed = 12f;
		this.m_owner.SetInputOverride("raccoon");
		this.m_owner.SetIsFlying(true, "raccoon", true, false);
		this.m_owner.IsEthereal = true;
		this.m_owner.healthHaver.IsVulnerable = false;
		base.aiActor.PathableTiles = CellTypes.FLOOR | CellTypes.PIT;
		base.aiActor.PathfindToPosition(mostDistantExit.ToVector2(), null, true, null, null, null, false);
		base.aiAnimator.PlayUntilCancelled("carry", true, null, -1f, false);
		while (!base.aiActor.PathComplete)
		{
			if (this.m_owner)
			{
				Vector2 vector = attachPoint.position.XY() + (this.m_owner.transform.position.XY() - this.m_owner.sprite.WorldBottomCenter) + new Vector2(0f, -0.125f);
				this.m_owner.transform.position = vector;
				this.m_owner.specRigidbody.Reinitialize();
			}
			yield return null;
		}
		base.sprite.SpriteChanged -= this.UpdatePlayerPosition;
		base.aiActor.MovementSpeed = cachedSpeed;
		this.m_owner.healthHaver.IsVulnerable = true;
		this.m_owner.SetIsFlying(false, "raccoon", true, false);
		this.m_owner.ClearInputOverride("raccoon");
		this.m_owner.IsEthereal = false;
		base.aiActor.PathableTiles = CellTypes.FLOOR;
		followBehavior.TemporarilyDisabled = false;
		yield break;
	}

	// Token: 0x06007110 RID: 28944 RVA: 0x002CE658 File Offset: 0x002CC858
	private void UpdatePlayerPosition(tk2dBaseSprite obj)
	{
		if (this.m_owner && obj)
		{
			Transform transform = obj.transform.Find("carry");
			if (transform)
			{
				Vector2 vector = transform.position.XY() + (this.m_owner.transform.position.XY() - this.m_owner.sprite.WorldBottomCenter) + new Vector2(0f, -0.125f);
				this.m_owner.transform.position = vector;
				this.m_owner.specRigidbody.Reinitialize();
			}
		}
	}

	// Token: 0x06007111 RID: 28945 RVA: 0x002CE714 File Offset: 0x002CC914
	public void DoPet(PlayerController player)
	{
		base.aiAnimator.LockFacingDirection = true;
		if (base.specRigidbody.UnitCenter.x > player.specRigidbody.UnitCenter.x)
		{
			base.aiAnimator.FacingDirection = 180f;
			this.m_petOffset = new Vector2(0.3125f, -0.625f);
		}
		else
		{
			base.aiAnimator.FacingDirection = 0f;
			this.m_petOffset = new Vector2(-0.8125f, -0.625f);
		}
		base.aiAnimator.PlayUntilCancelled("pet", false, null, -1f, false);
		this.m_pettingDoer = player;
	}

	// Token: 0x06007112 RID: 28946 RVA: 0x002CE7C8 File Offset: 0x002CC9C8
	public void StopPet()
	{
		if (this.m_pettingDoer != null)
		{
			base.aiAnimator.EndAnimationIf("pet");
			base.aiAnimator.LockFacingDirection = false;
			this.m_pettingDoer = null;
		}
	}

	// Token: 0x17001113 RID: 4371
	// (get) Token: 0x06007113 RID: 28947 RVA: 0x002CE800 File Offset: 0x002CCA00
	public bool IsBeingPet
	{
		get
		{
			return this.m_pettingDoer != null;
		}
	}

	// Token: 0x06007114 RID: 28948 RVA: 0x002CE810 File Offset: 0x002CCA10
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			this.m_owner.OnUsedPlayerItem -= this.HandleItemUsed;
			this.m_owner.companions.Remove(base.aiActor);
			this.m_owner.OnPitfall -= this.HandlePitfall;
			this.m_owner.OnRoomClearEvent -= this.HandleRoomCleared;
		}
		if (base.aiShooter)
		{
			AIShooter aiShooter = base.aiShooter;
			aiShooter.PostProcessProjectile = (Action<Projectile>)Delegate.Remove(aiShooter.PostProcessProjectile, new Action<Projectile>(this.HandleCompanionPostProcessProjectile));
		}
		base.OnDestroy();
	}

	// Token: 0x0400707E RID: 28798
	public bool CanInterceptBullets;

	// Token: 0x0400707F RID: 28799
	public bool IsCopDead;

	// Token: 0x04007080 RID: 28800
	public bool IsCop;

	// Token: 0x04007081 RID: 28801
	public StatModifier CopDeathStatModifier;

	// Token: 0x04007082 RID: 28802
	public int CurseOnCopDeath = 2;

	// Token: 0x04007083 RID: 28803
	public bool CanCrossPits;

	// Token: 0x04007084 RID: 28804
	public bool BlanksOnActiveItemUsed;

	// Token: 0x04007085 RID: 28805
	public float InternalBlankCooldown = 10f;

	// Token: 0x04007086 RID: 28806
	public bool HasStealthMode;

	// Token: 0x04007087 RID: 28807
	public bool PredictsChests;

	// Token: 0x04007088 RID: 28808
	[LongNumericEnum]
	public CustomSynergyType PredictsChestSynergy;

	// Token: 0x04007089 RID: 28809
	public bool CanBePet;

	// Token: 0x0400708A RID: 28810
	public CompanionController.CompanionIdentifier companionID;

	// Token: 0x0400708B RID: 28811
	public HeatRingModule TeaSynergyHeatRing;

	// Token: 0x0400708C RID: 28812
	protected PlayerController m_owner;

	// Token: 0x0400708D RID: 28813
	protected Chest m_lastPredictedChest;

	// Token: 0x0400708E RID: 28814
	protected HologramDoer m_hologram;

	// Token: 0x0400708F RID: 28815
	protected float m_internalBlankCooldown;

	// Token: 0x04007090 RID: 28816
	protected CellType m_lastCellType = CellType.FLOOR;

	// Token: 0x04007091 RID: 28817
	protected Vector2 m_cachedRollDirection;

	// Token: 0x04007092 RID: 28818
	protected bool m_isStealthed;

	// Token: 0x04007093 RID: 28819
	protected float m_timeInDeadlyRoom;

	// Token: 0x04007094 RID: 28820
	private bool m_hasDoneJunkanCheck;

	// Token: 0x04007095 RID: 28821
	private bool m_hasAttemptedSynergy;

	// Token: 0x04007096 RID: 28822
	private bool m_hasLuteBuff;

	// Token: 0x04007097 RID: 28823
	private GameObject m_luteOverheadVfx;

	// Token: 0x04007098 RID: 28824
	private bool m_isMimicTransforming;

	// Token: 0x04007099 RID: 28825
	public PlayerController m_pettingDoer;

	// Token: 0x0400709A RID: 28826
	public Vector2 m_petOffset;

	// Token: 0x0200137C RID: 4988
	public enum CompanionIdentifier
	{
		// Token: 0x0400709C RID: 28828
		NONE,
		// Token: 0x0400709D RID: 28829
		SUPER_SPACE_TURTLE,
		// Token: 0x0400709E RID: 28830
		PAYDAY_SHOOT,
		// Token: 0x0400709F RID: 28831
		PAYDAY_BLOCK,
		// Token: 0x040070A0 RID: 28832
		PAYDAY_STUN,
		// Token: 0x040070A1 RID: 28833
		BABY_GOOD_MIMIC,
		// Token: 0x040070A2 RID: 28834
		PHOENIX,
		// Token: 0x040070A3 RID: 28835
		PIG,
		// Token: 0x040070A4 RID: 28836
		SHELLETON,
		// Token: 0x040070A5 RID: 28837
		GATLING_GULL
	}
}
