using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dungeonator;
using UnityEngine;

// Token: 0x020014CA RID: 5322
public class TableFlipItem : PassiveItem
{
	// Token: 0x060078E8 RID: 30952 RVA: 0x00305E38 File Offset: 0x00304038
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_owner = player;
		base.Pickup(player);
		player.OnTableFlipped = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipped, new Action<FlippableCover>(this.DoEffect));
		player.OnTableFlipCompleted = (Action<FlippableCover>)Delegate.Combine(player.OnTableFlipCompleted, new Action<FlippableCover>(this.DoEffectCompleted));
	}

	// Token: 0x060078E9 RID: 30953 RVA: 0x00305EA4 File Offset: 0x003040A4
	private void DoEffect(FlippableCover obj)
	{
		this.HandleBlankEffect(obj);
		this.HandleStunEffect();
		this.HandleRageEffect();
		this.HandleVolleyEffect();
		base.StartCoroutine(this.HandleDelayedEffect(0.25f, new Action<FlippableCover>(this.HandleTableVolley), obj));
		this.HandleTemporalEffects();
		this.HandleHeatEffects(obj);
		if (this.UsesTimeSlowSynergy && base.Owner && base.Owner.HasActiveBonusSynergy(this.TimeSlowRequiredSynergy, false))
		{
			this.RadialSlow.DoRadialSlow(base.Owner.CenterPosition, base.Owner.CurrentRoom);
		}
	}

	// Token: 0x060078EA RID: 30954 RVA: 0x00305F4C File Offset: 0x0030414C
	private void DoEffectCompleted(FlippableCover obj)
	{
		this.HandleMoneyEffect(obj);
		this.HandleProjectileEffect(obj);
		this.HandleTableFlocking(obj);
	}

	// Token: 0x060078EB RID: 30955 RVA: 0x00305F64 File Offset: 0x00304164
	private IEnumerator HandleDelayedEffect(float delayTime, Action<FlippableCover> effect, FlippableCover table)
	{
		yield return new WaitForSeconds(delayTime);
		effect(table);
		yield break;
	}

	// Token: 0x060078EC RID: 30956 RVA: 0x00305F90 File Offset: 0x00304190
	private void HandleTableVolley(FlippableCover table)
	{
		if (this.TableFiresVolley)
		{
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(table.DirectionFlipped);
			ProjectileVolleyData projectileVolleyData = this.Volley;
			float num = 1f;
			if (this.VolleyOverrideSynergies != null)
			{
				for (int i = 0; i < this.VolleyOverrideSynergies.Count; i++)
				{
					if (this.m_owner && this.m_owner.HasActiveBonusSynergy(this.VolleyOverrideSynergies[i], false))
					{
						projectileVolleyData = this.VolleyOverrides[i];
						num = 2f;
					}
				}
			}
			VolleyUtility.FireVolley(projectileVolleyData, table.sprite.WorldCenter + intVector2FromDirection.ToVector2() * num, intVector2FromDirection.ToVector2(), this.m_owner, false);
		}
	}

	// Token: 0x060078ED RID: 30957 RVA: 0x0030605C File Offset: 0x0030425C
	private void HandleTableFlocking(FlippableCover table)
	{
		if (this.TableFlocking)
		{
			RoomHandler currentRoom = base.Owner.CurrentRoom;
			ReadOnlyCollection<IPlayerInteractable> roomInteractables = currentRoom.GetRoomInteractables();
			for (int i = 0; i < roomInteractables.Count; i++)
			{
				if (currentRoom.IsRegistered(roomInteractables[i]))
				{
					FlippableCover flippableCover = roomInteractables[i] as FlippableCover;
					if (flippableCover != null && !flippableCover.IsFlipped && !flippableCover.IsGilded)
					{
						if (flippableCover.flipStyle == FlippableCover.FlipStyle.ANY)
						{
							flippableCover.ForceSetFlipper(base.Owner);
							flippableCover.Flip(table.DirectionFlipped);
						}
						else if (flippableCover.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_LEFT_RIGHT)
						{
							if (table.DirectionFlipped == DungeonData.Direction.NORTH || table.DirectionFlipped == DungeonData.Direction.SOUTH)
							{
								flippableCover.ForceSetFlipper(base.Owner);
								flippableCover.Flip((UnityEngine.Random.value <= 0.5f) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST);
							}
							else
							{
								flippableCover.ForceSetFlipper(base.Owner);
								flippableCover.Flip(table.DirectionFlipped);
							}
						}
						else if (flippableCover.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_UP_DOWN)
						{
							if (table.DirectionFlipped == DungeonData.Direction.EAST || table.DirectionFlipped == DungeonData.Direction.WEST)
							{
								flippableCover.ForceSetFlipper(base.Owner);
								flippableCover.Flip((UnityEngine.Random.value <= 0.5f) ? DungeonData.Direction.SOUTH : DungeonData.Direction.NORTH);
							}
							else
							{
								flippableCover.ForceSetFlipper(base.Owner);
								flippableCover.Flip(table.DirectionFlipped);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060078EE RID: 30958 RVA: 0x003061E4 File Offset: 0x003043E4
	private void HandleProjectileEffect(FlippableCover table)
	{
		if (this.TableBecomesProjectile)
		{
			GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Table_Exhaust");
			Vector2 vector = DungeonData.GetIntVector2FromDirection(table.DirectionFlipped).ToVector2();
			float num = BraveMathCollege.Atan2Degrees(vector);
			Vector3 vector2 = Vector3.zero;
			switch (table.DirectionFlipped)
			{
			case DungeonData.Direction.NORTH:
				vector2 = Vector3.zero;
				break;
			case DungeonData.Direction.EAST:
				vector2 = new Vector3(-0.5f, 0.25f, 0f);
				break;
			case DungeonData.Direction.SOUTH:
				vector2 = new Vector3(0f, 0.5f, 1f);
				break;
			case DungeonData.Direction.WEST:
				vector2 = new Vector3(0.5f, 0.25f, 0f);
				break;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, table.specRigidbody.UnitCenter.ToVector3ZisY(0f) + vector2, Quaternion.Euler(0f, 0f, num));
			gameObject2.transform.parent = table.specRigidbody.transform;
			Projectile projectile = table.specRigidbody.gameObject.AddComponent<Projectile>();
			projectile.Shooter = base.Owner.specRigidbody;
			projectile.Owner = base.Owner;
			projectile.baseData.damage = this.DirectHitBonusDamage;
			projectile.baseData.range = 1000f;
			projectile.baseData.speed = 20f;
			projectile.baseData.force = 50f;
			projectile.baseData.UsesCustomAccelerationCurve = true;
			projectile.baseData.AccelerationCurve = this.CustomAccelerationCurve;
			projectile.baseData.CustomAccelerationCurveDuration = this.CustomAccelerationCurveDuration;
			projectile.shouldRotate = false;
			projectile.Start();
			projectile.SendInDirection(vector, true, true);
			projectile.collidesWithProjectiles = true;
			projectile.projectileHitHealth = 20;
			Action<Projectile> action = delegate(Projectile p)
			{
				if (table && table.shadowSprite)
				{
					table.shadowSprite.renderer.enabled = false;
				}
			};
			projectile.OnDestruction += action;
			ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
			explosiveModifier.explosionData = this.ProjectileExplosionData;
			table.PreventPitFalls = true;
			if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.ROCKET_POWERED_TABLES, false))
			{
				HomingModifier homingModifier = projectile.gameObject.AddComponent<HomingModifier>();
				homingModifier.AssignProjectile(projectile);
				homingModifier.HomingRadius = 20f;
				homingModifier.AngularVelocity = 720f;
				BounceProjModifier bounceProjModifier = projectile.gameObject.AddComponent<BounceProjModifier>();
				bounceProjModifier.numberOfBounces = 4;
				bounceProjModifier.onlyBounceOffTiles = true;
			}
		}
	}

	// Token: 0x060078EF RID: 30959 RVA: 0x003064B0 File Offset: 0x003046B0
	private void HandleBlankEffect(FlippableCover table)
	{
		if (this.TableTriggersBlankEffect)
		{
			GameManager.Instance.StartCoroutine(this.DelayedBlankEffect(table));
		}
	}

	// Token: 0x060078F0 RID: 30960 RVA: 0x003064D0 File Offset: 0x003046D0
	private IEnumerator DelayedBlankEffect(FlippableCover table)
	{
		yield return new WaitForSeconds(0.15f);
		if (base.Owner)
		{
			if (this.UsesTableTechBeesSynergy && base.Owner.HasActiveBonusSynergy(CustomSynergyType.TABLE_TECH_BEES, false))
			{
				this.m_beeCount = 0;
				if (table && table.sprite)
				{
					this.InternalForceBlank(table.sprite.WorldCenter, 25f, 0.5f, false, true, true, -1f, new Action<Projectile>(this.PostProcessTableTechBees));
				}
			}
			else if (table && table.sprite)
			{
				this.InternalForceBlank(table.sprite.WorldCenter, 25f, 0.5f, false, true, true, -1f, null);
			}
		}
		yield break;
	}

	// Token: 0x060078F1 RID: 30961 RVA: 0x003064F4 File Offset: 0x003046F4
	private void PostProcessTableTechBees(Projectile target)
	{
		for (int i = 0; i < UnityEngine.Random.Range(this.MinNumberOfBeesPerEnemyBullet, this.MaxNumberOfBeesPerEnemyBullet); i++)
		{
			if (target && base.Owner && this.m_beeCount < 49)
			{
				this.m_beeCount++;
				GameObject gameObject = SpawnManager.SpawnProjectile(this.BeeProjectile.gameObject, target.transform.position + UnityEngine.Random.insideUnitCircle.ToVector3ZisY(0f), target.transform.rotation, true);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.Owner = base.Owner;
				component.Shooter = base.Owner.specRigidbody;
				component.collidesWithPlayer = false;
				component.collidesWithEnemies = true;
				component.collidesWithProjectiles = false;
			}
		}
	}

	// Token: 0x060078F2 RID: 30962 RVA: 0x003065D0 File Offset: 0x003047D0
	private void InternalForceBlank(Vector2 center, float overrideRadius = 25f, float overrideTimeAtMaxRadius = 0.5f, bool silent = false, bool breaksWalls = true, bool breaksObjects = true, float overrideForce = -1f, Action<Projectile> customCallback = null)
	{
		GameObject gameObject = ((!silent) ? ((GameObject)BraveResources.Load("Global VFX/BlankVFX", ".prefab")) : null);
		if (!silent)
		{
			AkSoundEngine.PostEvent("Play_OBJ_silenceblank_use_01", base.gameObject);
			AkSoundEngine.PostEvent("Stop_ENM_attack_cancel_01", base.gameObject);
		}
		GameObject gameObject2 = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject2.AddComponent<SilencerInstance>();
		if (customCallback != null)
		{
			silencerInstance.UsesCustomProjectileCallback = true;
			silencerInstance.OnCustomBlankedProjectile = customCallback;
		}
		silencerInstance.TriggerSilencer(center, 50f, overrideRadius, gameObject, (!silent) ? 0.15f : 0f, (!silent) ? 0.2f : 0f, (float)((!silent) ? 50 : 0), (float)((!silent) ? 10 : 0), (!silent) ? ((overrideForce < 0f) ? 140f : overrideForce) : 0f, (float)((!breaksObjects) ? 0 : ((!silent) ? 15 : 5)), overrideTimeAtMaxRadius, base.Owner, breaksWalls, false);
		if (base.Owner)
		{
			base.Owner.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
		}
	}

	// Token: 0x060078F3 RID: 30963 RVA: 0x0030671C File Offset: 0x0030491C
	private void HandleStunEffect()
	{
		if (this.TableStunsEnemies)
		{
			List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (UnityEngine.Random.value < this.ChanceToStun)
					{
						if (this.StunsAllEnemiesInRoom)
						{
							this.StunEnemy(activeEnemies[i]);
						}
						else
						{
							float num = Vector2.Distance(activeEnemies[i].CenterPosition, base.Owner.CenterPosition);
							if (num < this.StunRadius)
							{
								this.StunEnemy(activeEnemies[i]);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060078F4 RID: 30964 RVA: 0x003067C8 File Offset: 0x003049C8
	private void StunEnemy(AIActor enemy)
	{
		if (!enemy.healthHaver.IsBoss && enemy && enemy.behaviorSpeculator)
		{
			enemy.ClearPath();
			enemy.behaviorSpeculator.Interrupt();
			enemy.behaviorSpeculator.Stun(this.StunDuration, true);
		}
	}

	// Token: 0x060078F5 RID: 30965 RVA: 0x00306824 File Offset: 0x00304A24
	private void HandleMoneyEffect(FlippableCover sourceCover)
	{
		if (this.TableGivesCurrency)
		{
			float chanceToGiveCurrency = this.ChanceToGiveCurrency;
			if (UnityEngine.Random.value < chanceToGiveCurrency)
			{
				int num = UnityEngine.Random.Range(this.CurrencyToGiveMin, this.CurrencyToGiveMax);
				LootEngine.SpawnCurrency(sourceCover.specRigidbody.UnitCenter, num, false);
			}
		}
	}

	// Token: 0x060078F6 RID: 30966 RVA: 0x00306874 File Offset: 0x00304A74
	private void HandleTemporalEffects()
	{
		if (this.TableSlowsTime && (!this.UsesTimeSlowSynergy || !base.Owner || !base.Owner.HasActiveBonusSynergy(this.TimeSlowRequiredSynergy, false)))
		{
			base.Owner.StartCoroutine(this.HandleTimeSlowDuration());
		}
		if (this.TableProvidesInvulnerability)
		{
			base.Owner.healthHaver.TriggerInvulnerabilityPeriod(this.InvulnerableTimeDuration);
		}
	}

	// Token: 0x060078F7 RID: 30967 RVA: 0x003068F4 File Offset: 0x00304AF4
	private IEnumerator HandleTimeSlowDuration()
	{
		TableFlipItem.AdditionalTableFlipSlowTime += this.SlowTimeDuration;
		TableFlipItem.AdditionalTableFlipSlowTime = Mathf.Min(2f * this.SlowTimeDuration, TableFlipItem.AdditionalTableFlipSlowTime);
		if (TableFlipItem.TableFlipTimeIsActive)
		{
			yield break;
		}
		TableFlipItem.TableFlipTimeIsActive = true;
		BraveTime.RegisterTimeScaleMultiplier(this.SlowTimeAmount, base.gameObject);
		float elapsed = 0f;
		while (elapsed < TableFlipItem.AdditionalTableFlipSlowTime)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		TableFlipItem.AdditionalTableFlipSlowTime = 0f;
		TableFlipItem.TableFlipTimeIsActive = false;
		BraveTime.ClearMultiplier(base.gameObject);
		yield break;
	}

	// Token: 0x060078F8 RID: 30968 RVA: 0x00306910 File Offset: 0x00304B10
	private void HandleRageEffect()
	{
		if (this.TableGivesRage)
		{
			if (this.m_rageElapsed > 0f)
			{
				this.m_rageElapsed = this.RageDuration;
				if (base.Owner.HasActiveBonusSynergy(CustomSynergyType.ANGRIER_BULLETS, false))
				{
					this.m_rageElapsed *= 3f;
				}
				if (this.RageOverheadVFX && this.rageInstanceVFX == null)
				{
					this.rageInstanceVFX = base.Owner.PlayEffectOnActor(this.RageOverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
				}
			}
			else
			{
				base.Owner.StartCoroutine(this.HandleRageCooldown());
			}
		}
	}

	// Token: 0x060078F9 RID: 30969 RVA: 0x003069D0 File Offset: 0x00304BD0
	private IEnumerator HandleRageCooldown()
	{
		this.rageInstanceVFX = null;
		if (this.RageOverheadVFX)
		{
			this.rageInstanceVFX = base.Owner.PlayEffectOnActor(this.RageOverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
		}
		this.m_rageElapsed = this.RageDuration;
		if (base.Owner.HasActiveBonusSynergy(CustomSynergyType.ANGRIER_BULLETS, false))
		{
			this.m_rageElapsed *= 3f;
		}
		StatModifier damageStat = new StatModifier();
		damageStat.amount = this.RageDamageMultiplier;
		damageStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
		damageStat.statToBoost = PlayerStats.StatType.Damage;
		PlayerController cachedOwner = base.Owner;
		cachedOwner.ownerlessStatModifiers.Add(damageStat);
		cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
		Color rageColor = this.RageFlatColor;
		while (this.m_rageElapsed > 0f)
		{
			cachedOwner.baseFlatColorOverride = rageColor.WithAlpha(Mathf.Lerp(rageColor.a, 0f, 1f - Mathf.Clamp01(this.m_rageElapsed)));
			if (this.rageInstanceVFX && this.m_rageElapsed < this.RageDuration - 1f)
			{
				this.rageInstanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
				this.rageInstanceVFX = null;
			}
			yield return null;
			this.m_rageElapsed -= BraveTime.DeltaTime;
		}
		if (this.rageInstanceVFX)
		{
			this.rageInstanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
		}
		cachedOwner.ownerlessStatModifiers.Remove(damageStat);
		cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
		yield break;
	}

	// Token: 0x060078FA RID: 30970 RVA: 0x003069EC File Offset: 0x00304BEC
	private void HandleVolleyEffect()
	{
		if (this.AddsModuleCopies)
		{
			if (this.m_volleyElapsed < 0f)
			{
				base.Owner.StartCoroutine(this.HandleVolleyCooldown());
			}
			else
			{
				this.m_volleyElapsed = 0f;
			}
		}
	}

	// Token: 0x060078FB RID: 30971 RVA: 0x00306A2C File Offset: 0x00304C2C
	private IEnumerator HandleVolleyCooldown()
	{
		this.m_volleyElapsed = 0f;
		PlayerController cachedOwner = base.Owner;
		bool wasFiring = false;
		if (cachedOwner.CurrentGun != null && cachedOwner.CurrentGun.IsFiring)
		{
			cachedOwner.CurrentGun.CeaseAttack(true, null);
			wasFiring = true;
		}
		cachedOwner.stats.AdditionalVolleyModifiers += this.ModifyVolley;
		cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
		if (wasFiring)
		{
			cachedOwner.CurrentGun.Attack(null, null);
			for (int i = 0; i < cachedOwner.CurrentGun.ActiveBeams.Count; i++)
			{
				if (cachedOwner.CurrentGun.ActiveBeams[i] != null && cachedOwner.CurrentGun.ActiveBeams[i].beam is BasicBeamController)
				{
					(cachedOwner.CurrentGun.ActiveBeams[i].beam as BasicBeamController).ForceChargeTimer(10f);
				}
			}
		}
		while (this.m_volleyElapsed < this.ModuleCopyDuration)
		{
			this.m_volleyElapsed += BraveTime.DeltaTime;
			yield return null;
		}
		bool wasEndFiring = cachedOwner.CurrentGun != null && cachedOwner.CurrentGun.IsFiring;
		if (wasEndFiring)
		{
			cachedOwner.CurrentGun.CeaseAttack(true, null);
		}
		cachedOwner.stats.AdditionalVolleyModifiers -= this.ModifyVolley;
		cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
		if (wasEndFiring)
		{
			cachedOwner.CurrentGun.Attack(null, null);
			for (int j = 0; j < cachedOwner.CurrentGun.ActiveBeams.Count; j++)
			{
				if (cachedOwner.CurrentGun.ActiveBeams[j] != null && cachedOwner.CurrentGun.ActiveBeams[j].beam is BasicBeamController)
				{
					(cachedOwner.CurrentGun.ActiveBeams[j].beam as BasicBeamController).ForceChargeTimer(10f);
				}
			}
		}
		this.m_volleyElapsed = -1f;
		yield return null;
		yield break;
	}

	// Token: 0x060078FC RID: 30972 RVA: 0x00306A48 File Offset: 0x00304C48
	private void HandleHeatEffects(FlippableCover table)
	{
		if (this.TableHeat && table)
		{
			table.StartCoroutine(this.HandleHeatEffectsCR(table));
		}
	}

	// Token: 0x060078FD RID: 30973 RVA: 0x00306A70 File Offset: 0x00304C70
	private IEnumerator HandleHeatEffectsCR(FlippableCover table)
	{
		this.HandleRadialIndicator(table);
		float elapsed = 0f;
		int ct = -1;
		bool hasSynergy = PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.HIDDEN_TECH_FLARE, out ct);
		RoomHandler r = table.transform.position.GetAbsoluteRoom();
		Vector3 tableCenter = ((!table.sprite) ? table.transform.position : table.sprite.WorldCenter.ToVector3ZisY(0f));
		Action<AIActor, float> AuraAction = delegate(AIActor actor, float dist)
		{
			actor.ApplyEffect((!hasSynergy) ? this.TableHeatEffect : this.TableHeatSynergyEffect, 1f, null);
		};
		float modRadius = ((!hasSynergy) ? this.TableHeatRadius : this.TableHeatSynergyRadius);
		while (elapsed < this.TableHeatDuration)
		{
			elapsed += BraveTime.DeltaTime;
			r.ApplyActionToNearbyEnemies(tableCenter.XY(), modRadius, AuraAction);
			yield return null;
		}
		this.UnhandleRadialIndicator(table);
		yield break;
	}

	// Token: 0x060078FE RID: 30974 RVA: 0x00306A94 File Offset: 0x00304C94
	private void HandleRadialIndicator(FlippableCover table)
	{
		if (this.m_radialIndicators == null)
		{
			this.m_radialIndicators = new Dictionary<FlippableCover, HeatIndicatorController>();
		}
		if (!this.m_radialIndicators.ContainsKey(table))
		{
			Vector3 vector = ((!table.sprite) ? table.transform.position : table.sprite.WorldCenter.ToVector3ZisY(0f));
			this.m_radialIndicators.Add(table, ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), vector, Quaternion.identity, table.transform)).GetComponent<HeatIndicatorController>());
			int num = -1;
			float num2 = ((!PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.HIDDEN_TECH_FLARE, out num)) ? this.TableHeatRadius : this.TableHeatSynergyRadius);
			this.m_radialIndicators[table].CurrentRadius = num2;
		}
	}

	// Token: 0x060078FF RID: 30975 RVA: 0x00306B68 File Offset: 0x00304D68
	private void UnhandleRadialIndicator(FlippableCover table)
	{
		if (this.m_radialIndicators.ContainsKey(table))
		{
			HeatIndicatorController heatIndicatorController = this.m_radialIndicators[table];
			heatIndicatorController.EndEffect();
			this.m_radialIndicators.Remove(table);
		}
	}

	// Token: 0x06007900 RID: 30976 RVA: 0x00306BA8 File Offset: 0x00304DA8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<TableFlipItem>().m_pickedUpThisRun = true;
		if (player)
		{
			player.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipped, new Action<FlippableCover>(this.DoEffect));
			player.OnTableFlipCompleted = (Action<FlippableCover>)Delegate.Remove(player.OnTableFlipCompleted, new Action<FlippableCover>(this.DoEffectCompleted));
		}
		this.m_owner = null;
		return debrisObject;
	}

	// Token: 0x06007901 RID: 30977 RVA: 0x00306C20 File Offset: 0x00304E20
	protected override void OnDestroy()
	{
		this.m_radialIndicators = null;
		BraveTime.ClearMultiplier(base.gameObject);
		if (base.Owner)
		{
			PlayerController owner = base.Owner;
			owner.OnTableFlipped = (Action<FlippableCover>)Delegate.Remove(owner.OnTableFlipped, new Action<FlippableCover>(this.DoEffect));
			PlayerController owner2 = base.Owner;
			owner2.OnTableFlipCompleted = (Action<FlippableCover>)Delegate.Remove(owner2.OnTableFlipCompleted, new Action<FlippableCover>(this.DoEffectCompleted));
		}
		base.OnDestroy();
	}

	// Token: 0x06007902 RID: 30978 RVA: 0x00306CA4 File Offset: 0x00304EA4
	public void ModifyVolley(ProjectileVolleyData volleyToModify)
	{
		if (this.ModuleCopyCount > 0)
		{
			int count = volleyToModify.projectiles.Count;
			for (int i = 0; i < count; i++)
			{
				ProjectileModule projectileModule = volleyToModify.projectiles[i];
				float num = (float)this.ModuleCopyCount * 10f * -1f / 2f;
				for (int j = 0; j < this.ModuleCopyCount; j++)
				{
					int num2 = i;
					if (projectileModule.CloneSourceIndex >= 0)
					{
						num2 = projectileModule.CloneSourceIndex;
					}
					ProjectileModule projectileModule2 = ProjectileModule.CreateClone(projectileModule, false, num2);
					float num3 = num + 10f * (float)j;
					projectileModule2.angleFromAim = num3;
					projectileModule2.ignoredForReloadPurposes = true;
					projectileModule2.ammoCost = 0;
					volleyToModify.projectiles.Add(projectileModule2);
				}
			}
		}
	}

	// Token: 0x04007B39 RID: 31545
	public bool TableTriggersBlankEffect;

	// Token: 0x04007B3A RID: 31546
	public bool TableStunsEnemies;

	// Token: 0x04007B3B RID: 31547
	[ShowInInspectorIf("TableStunsEnemies", false)]
	public float ChanceToStun = 1f;

	// Token: 0x04007B3C RID: 31548
	[ShowInInspectorIf("TableStunsEnemies", false)]
	public float StunDuration = 4f;

	// Token: 0x04007B3D RID: 31549
	[ShowInInspectorIf("TableStunsEnemies", false)]
	public float StunRadius = 10f;

	// Token: 0x04007B3E RID: 31550
	[ShowInInspectorIf("TableStunsEnemies", false)]
	public bool StunsAllEnemiesInRoom;

	// Token: 0x04007B3F RID: 31551
	public bool TableGivesCurrency;

	// Token: 0x04007B40 RID: 31552
	[ShowInInspectorIf("TableGivesCurrency", false)]
	public float ChanceToGiveCurrency = 1f;

	// Token: 0x04007B41 RID: 31553
	[ShowInInspectorIf("TableGivesCurrency", false)]
	public int CurrencyToGiveMin = 1;

	// Token: 0x04007B42 RID: 31554
	[ShowInInspectorIf("TableGivesCurrency", false)]
	public int CurrencyToGiveMax = 1;

	// Token: 0x04007B43 RID: 31555
	public bool TableGivesRage;

	// Token: 0x04007B44 RID: 31556
	[ShowInInspectorIf("TableGivesRage", false)]
	public float RageDamageMultiplier = 2f;

	// Token: 0x04007B45 RID: 31557
	[ShowInInspectorIf("TableGivesRage", false)]
	public float RageDuration = 5f;

	// Token: 0x04007B46 RID: 31558
	[ShowInInspectorIf("TableGivesRage", false)]
	public Color RageFlatColor = new Color(0.5f, 0f, 0f, 0.75f);

	// Token: 0x04007B47 RID: 31559
	[ShowInInspectorIf("TableGivesRage", false)]
	public GameObject RageOverheadVFX;

	// Token: 0x04007B48 RID: 31560
	public bool AddsModuleCopies;

	// Token: 0x04007B49 RID: 31561
	[ShowInInspectorIf("AddsModuleCopies", false)]
	public float ModuleCopyDuration = 5f;

	// Token: 0x04007B4A RID: 31562
	[ShowInInspectorIf("AddsModuleCopies", false)]
	public int ModuleCopyCount = 1;

	// Token: 0x04007B4B RID: 31563
	public bool TableBecomesProjectile;

	// Token: 0x04007B4C RID: 31564
	[ShowInInspectorIf("TableBecomesProjectile", false)]
	public ExplosionData ProjectileExplosionData;

	// Token: 0x04007B4D RID: 31565
	[ShowInInspectorIf("TableBecomesProjectile", false)]
	public float DirectHitBonusDamage = 10f;

	// Token: 0x04007B4E RID: 31566
	[ShowInInspectorIf("TableBecomesProjectile", false)]
	public AnimationCurve CustomAccelerationCurve;

	// Token: 0x04007B4F RID: 31567
	[ShowInInspectorIf("TableBecomesProjectile", false)]
	public float CustomAccelerationCurveDuration;

	// Token: 0x04007B50 RID: 31568
	public bool TableSlowsTime;

	// Token: 0x04007B51 RID: 31569
	[ShowInInspectorIf("TableSlowsTime", false)]
	public float SlowTimeAmount = 0.5f;

	// Token: 0x04007B52 RID: 31570
	[ShowInInspectorIf("TableSlowsTime", false)]
	public float SlowTimeDuration = 3f;

	// Token: 0x04007B53 RID: 31571
	public bool TableProvidesInvulnerability;

	// Token: 0x04007B54 RID: 31572
	[ShowInInspectorIf("TableProvidesInvulnerability", false)]
	public float InvulnerableTimeDuration = 3f;

	// Token: 0x04007B55 RID: 31573
	public bool TableFlocking;

	// Token: 0x04007B56 RID: 31574
	[Space(10f)]
	public bool TableFiresVolley;

	// Token: 0x04007B57 RID: 31575
	[ShowInInspectorIf("TableFiresVolley", false)]
	public ProjectileVolleyData Volley;

	// Token: 0x04007B58 RID: 31576
	[LongNumericEnum]
	public List<CustomSynergyType> VolleyOverrideSynergies;

	// Token: 0x04007B59 RID: 31577
	public List<ProjectileVolleyData> VolleyOverrides;

	// Token: 0x04007B5A RID: 31578
	[Space(10f)]
	public bool TableHeat;

	// Token: 0x04007B5B RID: 31579
	[ShowInInspectorIf("TableHeat", false)]
	public float TableHeatRadius = 5f;

	// Token: 0x04007B5C RID: 31580
	[ShowInInspectorIf("TableHeat", false)]
	public float TableHeatSynergyRadius = 20f;

	// Token: 0x04007B5D RID: 31581
	[ShowInInspectorIf("TableHeat", false)]
	public float TableHeatDuration = 5f;

	// Token: 0x04007B5E RID: 31582
	public GameActorFireEffect TableHeatEffect;

	// Token: 0x04007B5F RID: 31583
	public GameActorFireEffect TableHeatSynergyEffect;

	// Token: 0x04007B60 RID: 31584
	[Header("Other Synergies")]
	public bool UsesTableTechBeesSynergy;

	// Token: 0x04007B61 RID: 31585
	[ShowInInspectorIf("UsesTableTechBeesSynergy", false)]
	public Projectile BeeProjectile;

	// Token: 0x04007B62 RID: 31586
	public int MinNumberOfBeesPerEnemyBullet = 1;

	// Token: 0x04007B63 RID: 31587
	public int MaxNumberOfBeesPerEnemyBullet = 1;

	// Token: 0x04007B64 RID: 31588
	public bool UsesTimeSlowSynergy;

	// Token: 0x04007B65 RID: 31589
	[LongNumericEnum]
	public CustomSynergyType TimeSlowRequiredSynergy;

	// Token: 0x04007B66 RID: 31590
	[ShowInInspectorIf("UsesTimeSlowSynergy", false)]
	public RadialSlowInterface RadialSlow;

	// Token: 0x04007B67 RID: 31591
	private const int c_beeCap = 49;

	// Token: 0x04007B68 RID: 31592
	private int m_beeCount;

	// Token: 0x04007B69 RID: 31593
	private static bool TableFlipTimeIsActive;

	// Token: 0x04007B6A RID: 31594
	private static float AdditionalTableFlipSlowTime;

	// Token: 0x04007B6B RID: 31595
	private float m_rageElapsed;

	// Token: 0x04007B6C RID: 31596
	private GameObject rageInstanceVFX;

	// Token: 0x04007B6D RID: 31597
	private float m_volleyElapsed = -1f;

	// Token: 0x04007B6E RID: 31598
	private Dictionary<FlippableCover, HeatIndicatorController> m_radialIndicators;
}
