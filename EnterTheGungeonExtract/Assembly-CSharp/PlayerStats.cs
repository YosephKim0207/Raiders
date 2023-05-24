using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020015FE RID: 5630
public class PlayerStats : MonoBehaviour
{
	// Token: 0x060082D0 RID: 33488 RVA: 0x00357378 File Offset: 0x00355578
	public static int GetTotalCurse()
	{
		int num = 0;
		if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.stats.StatValues != null)
		{
			num += Mathf.FloorToInt(GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Curse));
		}
		if (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.stats.StatValues != null)
		{
			num += Mathf.FloorToInt(GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.Curse));
		}
		GameStatsManager.Instance.UpdateMaximum(TrackedMaximums.HIGHEST_CURSE_LEVEL, (float)num);
		return num;
	}

	// Token: 0x060082D1 RID: 33489 RVA: 0x0035742C File Offset: 0x0035562C
	public static int GetTotalCoolness()
	{
		int num = 0;
		if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.stats.StatValues != null)
		{
			num += Mathf.FloorToInt(GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness));
		}
		if (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.stats.StatValues != null)
		{
			num += Mathf.FloorToInt(GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness));
		}
		return num;
	}

	// Token: 0x060082D2 RID: 33490 RVA: 0x003574D0 File Offset: 0x003556D0
	public static float GetTotalEnemyProjectileSpeedMultiplier()
	{
		float num = 1f;
		if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.stats.StatValues != null)
		{
			num *= GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier);
		}
		if (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.stats.StatValues != null)
		{
			num *= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.EnemyProjectileSpeedMultiplier);
		}
		return num;
	}

	// Token: 0x17001386 RID: 4998
	// (get) Token: 0x060082D3 RID: 33491 RVA: 0x00357570 File Offset: 0x00355770
	public float MovementSpeed
	{
		get
		{
			return this.StatValues[0];
		}
	}

	// Token: 0x060082D4 RID: 33492 RVA: 0x00357580 File Offset: 0x00355780
	public void CopyFrom(PlayerStats prefab)
	{
		this.NumBlanksPerFloor = prefab.NumBlanksPerFloor;
		this.NumBlanksPerFloorCoop = prefab.NumBlanksPerFloorCoop;
		this.rollDamage = prefab.rollDamage;
		this.UsesFireSourceEffect = prefab.UsesFireSourceEffect;
		this.OnFireSourceEffect = prefab.OnFireSourceEffect;
		this.BaseStatValues = new List<float>();
		for (int i = 0; i < prefab.BaseStatValues.Count; i++)
		{
			this.BaseStatValues.Add(prefab.BaseStatValues[i]);
		}
	}

	// Token: 0x060082D5 RID: 33493 RVA: 0x00357608 File Offset: 0x00355808
	public float GetBaseStatValue(PlayerStats.StatType stat)
	{
		return this.BaseStatValues[(int)stat];
	}

	// Token: 0x060082D6 RID: 33494 RVA: 0x00357618 File Offset: 0x00355818
	public void SetBaseStatValue(PlayerStats.StatType stat, float value, PlayerController owner)
	{
		this.BaseStatValues[(int)stat] = value;
		this.RecalculateStats(owner, true, false);
	}

	// Token: 0x060082D7 RID: 33495 RVA: 0x00357630 File Offset: 0x00355830
	public float GetStatValue(PlayerStats.StatType stat)
	{
		return this.StatValues[(int)stat];
	}

	// Token: 0x060082D8 RID: 33496 RVA: 0x00357640 File Offset: 0x00355840
	public float GetStatModifier(PlayerStats.StatType stat)
	{
		if (!Application.isPlaying)
		{
			return 1f;
		}
		if (stat < PlayerStats.StatType.MovementSpeed || stat >= (PlayerStats.StatType)this.StatValues.Count)
		{
			return 1f;
		}
		return this.StatValues[(int)stat] / this.BaseStatValues[(int)stat];
	}

	// Token: 0x060082D9 RID: 33497 RVA: 0x00357698 File Offset: 0x00355898
	public void RebuildGunVolleys(PlayerController owner)
	{
		if (owner.inventory == null || owner.inventory.AllGuns == null || owner.inventory.AllGuns.Count == 0)
		{
			return;
		}
		for (int i = 0; i < owner.inventory.AllGuns.Count; i++)
		{
			Gun gun = owner.inventory.AllGuns[i];
			ProjectileVolleyData modifiedVolley = gun.modifiedVolley;
			gun.modifiedVolley = null;
			gun.modifiedFinalVolley = null;
			ProjectileVolleyData projectileVolleyData = ScriptableObject.CreateInstance<ProjectileVolleyData>();
			if (gun.Volley != null)
			{
				projectileVolleyData.InitializeFrom(gun.Volley);
			}
			else
			{
				projectileVolleyData.projectiles = new List<ProjectileModule>();
				projectileVolleyData.projectiles.Add(ProjectileModule.CreateClone(gun.singleModule, true, -1));
				projectileVolleyData.BeamRotationDegreesPerSecond = float.MaxValue;
			}
			this.ModVolley(owner, projectileVolleyData);
			for (int j = 0; j < projectileVolleyData.projectiles.Count; j++)
			{
				if (projectileVolleyData.projectiles[j].numberOfShotsInClip > 0)
				{
					projectileVolleyData.projectiles[j].numberOfShotsInClip = Mathf.Max(1, projectileVolleyData.projectiles[j].numberOfShotsInClip + gun.AdditionalClipCapacity);
				}
			}
			if (gun.PostProcessVolley != null)
			{
				gun.PostProcessVolley(projectileVolleyData);
			}
			gun.modifiedVolley = projectileVolleyData;
			if (gun.DefaultModule.HasFinalVolleyOverride())
			{
				ProjectileVolleyData projectileVolleyData2 = ScriptableObject.CreateInstance<ProjectileVolleyData>();
				projectileVolleyData2.InitializeFrom(gun.DefaultModule.finalVolley);
				this.ModVolley(owner, projectileVolleyData2);
				gun.modifiedFinalVolley = projectileVolleyData2;
			}
			if (gun.rawOptionalReloadVolley != null)
			{
				ProjectileVolleyData projectileVolleyData3 = ScriptableObject.CreateInstance<ProjectileVolleyData>();
				projectileVolleyData3.InitializeFrom(gun.rawOptionalReloadVolley);
				this.ModVolley(owner, projectileVolleyData3);
				gun.modifiedOptionalReloadVolley = projectileVolleyData3;
			}
			for (int k = 0; k < projectileVolleyData.projectiles.Count; k++)
			{
				if (string.IsNullOrEmpty(projectileVolleyData.projectiles[k].runtimeGuid))
				{
					projectileVolleyData.projectiles[k].runtimeGuid = Guid.NewGuid().ToString();
				}
			}
			gun.ReinitializeModuleData(modifiedVolley);
		}
		for (int l = 0; l < owner.passiveItems.Count; l++)
		{
			if (owner.passiveItems[l] is FireVolleyOnRollItem)
			{
				FireVolleyOnRollItem fireVolleyOnRollItem = owner.passiveItems[l] as FireVolleyOnRollItem;
				fireVolleyOnRollItem.ModVolley = null;
				ProjectileVolleyData projectileVolleyData4 = ScriptableObject.CreateInstance<ProjectileVolleyData>();
				projectileVolleyData4.InitializeFrom(fireVolleyOnRollItem.Volley);
				this.ModVolley(owner, projectileVolleyData4);
				fireVolleyOnRollItem.ModVolley = projectileVolleyData4;
			}
		}
	}

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x060082DA RID: 33498 RVA: 0x00357958 File Offset: 0x00355B58
	// (remove) Token: 0x060082DB RID: 33499 RVA: 0x00357990 File Offset: 0x00355B90
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<ProjectileVolleyData> AdditionalVolleyModifiers;

	// Token: 0x060082DC RID: 33500 RVA: 0x003579C8 File Offset: 0x00355BC8
	private void ModVolley(PlayerController owner, ProjectileVolleyData volley)
	{
		for (int i = 0; i < owner.passiveItems.Count; i++)
		{
			PassiveItem passiveItem = owner.passiveItems[i];
			if (passiveItem is GunVolleyModificationItem)
			{
				GunVolleyModificationItem gunVolleyModificationItem = passiveItem as GunVolleyModificationItem;
				gunVolleyModificationItem.ModifyVolley(volley);
			}
		}
		PlayerItem currentItem = owner.CurrentItem;
		if (currentItem is ActiveGunVolleyModificationItem && currentItem.IsActive)
		{
			ActiveGunVolleyModificationItem activeGunVolleyModificationItem = currentItem as ActiveGunVolleyModificationItem;
			activeGunVolleyModificationItem.ModifyVolley(volley);
		}
		if (this.AdditionalVolleyModifiers != null)
		{
			this.AdditionalVolleyModifiers(volley);
		}
	}

	// Token: 0x060082DD RID: 33501 RVA: 0x00357A5C File Offset: 0x00355C5C
	private void ApplyStatModifier(StatModifier modifier, float[] statModsAdditive, float[] statModsMultiplic)
	{
		int statToBoost = (int)modifier.statToBoost;
		if (modifier.modifyType == StatModifier.ModifyMethod.ADDITIVE)
		{
			statModsAdditive[statToBoost] += modifier.amount;
		}
		else if (modifier.modifyType == StatModifier.ModifyMethod.MULTIPLICATIVE)
		{
			statModsMultiplic[statToBoost] *= modifier.amount;
		}
	}

	// Token: 0x060082DE RID: 33502 RVA: 0x00357AB0 File Offset: 0x00355CB0
	public void RecalculateStats(PlayerController owner, bool force = false, bool recursive = false)
	{
		this.RecalculateStatsInternal(owner);
		if (!recursive && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(owner);
			if (otherPlayer && otherPlayer.stats != null)
			{
				otherPlayer.stats.RecalculateStats(otherPlayer, force, true);
			}
		}
	}

	// Token: 0x060082DF RID: 33503 RVA: 0x00357B10 File Offset: 0x00355D10
	private void RecalculateSynergies(PlayerController owner)
	{
		if (this.PreviouslyActiveSynergies == null)
		{
			this.PreviouslyActiveSynergies = new List<int>();
		}
		this.PreviouslyActiveSynergies.Clear();
		this.PreviouslyActiveSynergies.AddRange(owner.ActiveExtraSynergies);
		if (!GameManager.Instance || !GameManager.Instance.SynergyManager || !owner)
		{
			return;
		}
		GameManager.Instance.SynergyManager.RebuildSynergies(owner, this.PreviouslyActiveSynergies);
		bool flag = false;
		int num = -1;
		for (int i = 0; i < owner.ActiveExtraSynergies.Count; i++)
		{
			if (!GameManager.Instance.SynergyManager.synergies[owner.ActiveExtraSynergies[i]].SuppressVFX)
			{
				if (GameManager.Instance.SynergyManager.synergies[owner.ActiveExtraSynergies[i]].ActivationStatus != SynergyEntry.SynergyActivation.INACTIVE)
				{
					if (!this.PreviouslyActiveSynergies.Contains(owner.ActiveExtraSynergies[i]))
					{
						flag = true;
						num = owner.ActiveExtraSynergies[i];
						GameStatsManager.Instance.HandleEncounteredSynergy(num);
						break;
					}
				}
			}
		}
		if (flag)
		{
			owner.PlayEffectOnActor((GameObject)ResourceCache.Acquire("Global VFX/VFX_Synergy"), new Vector3(0f, 0.5f, 0f), true, false, false);
			AdvancedSynergyEntry advancedSynergyEntry = GameManager.Instance.SynergyManager.synergies[num];
			if (advancedSynergyEntry.ActivationStatus != SynergyEntry.SynergyActivation.INACTIVE && !string.IsNullOrEmpty(advancedSynergyEntry.NameKey))
			{
				GameUIRoot.Instance.notificationController.AttemptSynergyAttachment(advancedSynergyEntry);
			}
		}
		this.PreviouslyActiveSynergies.Clear();
		this.PreviouslyActiveSynergies.AddRange(owner.ActiveExtraSynergies);
	}

	// Token: 0x060082E0 RID: 33504 RVA: 0x00357CD4 File Offset: 0x00355ED4
	public void RecalculateStatsInternal(PlayerController owner)
	{
		owner.DeferredStatRecalculationRequired = false;
		this.RecalculateSynergies(owner);
		int totalCurse = PlayerStats.GetTotalCurse();
		if (this.StatValues == null)
		{
			this.StatValues = new List<float>();
		}
		this.StatValues.Clear();
		for (int i = 0; i < this.BaseStatValues.Count; i++)
		{
			this.StatValues.Add(this.BaseStatValues[i]);
		}
		float[] array = new float[this.StatValues.Count];
		float[] array2 = new float[this.StatValues.Count];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = 1f;
		}
		float num = 0f;
		this.ActiveCustomSynergies.Clear();
		for (int k = 0; k < owner.ActiveExtraSynergies.Count; k++)
		{
			AdvancedSynergyEntry advancedSynergyEntry = GameManager.Instance.SynergyManager.synergies[owner.ActiveExtraSynergies[k]];
			if (advancedSynergyEntry.SynergyIsActive(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer))
			{
				for (int l = 0; l < advancedSynergyEntry.statModifiers.Count; l++)
				{
					StatModifier statModifier = advancedSynergyEntry.statModifiers[l];
					int statToBoost = (int)statModifier.statToBoost;
					if (statModifier.modifyType == StatModifier.ModifyMethod.ADDITIVE)
					{
						array[statToBoost] += statModifier.amount;
					}
					else if (statModifier.modifyType == StatModifier.ModifyMethod.MULTIPLICATIVE)
					{
						array2[statToBoost] *= statModifier.amount;
					}
				}
				for (int m = 0; m < advancedSynergyEntry.bonusSynergies.Count; m++)
				{
					this.ActiveCustomSynergies.Add(advancedSynergyEntry.bonusSynergies[m]);
				}
			}
		}
		for (int n = 0; n < owner.ownerlessStatModifiers.Count; n++)
		{
			StatModifier statModifier2 = owner.ownerlessStatModifiers[n];
			if (!statModifier2.hasBeenOwnerlessProcessed && statModifier2.statToBoost == PlayerStats.StatType.Health && statModifier2.amount > 0f)
			{
				num += statModifier2.amount;
			}
			int statToBoost2 = (int)statModifier2.statToBoost;
			if (statModifier2.modifyType == StatModifier.ModifyMethod.ADDITIVE)
			{
				array[statToBoost2] += statModifier2.amount;
			}
			else if (statModifier2.modifyType == StatModifier.ModifyMethod.MULTIPLICATIVE)
			{
				array2[statToBoost2] *= statModifier2.amount;
			}
			statModifier2.hasBeenOwnerlessProcessed = true;
		}
		for (int num2 = 0; num2 < owner.passiveItems.Count; num2++)
		{
			PassiveItem passiveItem = owner.passiveItems[num2];
			if (passiveItem.passiveStatModifiers != null && passiveItem.passiveStatModifiers.Length > 0)
			{
				for (int num3 = 0; num3 < passiveItem.passiveStatModifiers.Length; num3++)
				{
					StatModifier statModifier3 = passiveItem.passiveStatModifiers[num3];
					if (!passiveItem.HasBeenStatProcessed && statModifier3.statToBoost == PlayerStats.StatType.Health && statModifier3.amount > 0f)
					{
						num += statModifier3.amount;
					}
					this.ApplyStatModifier(statModifier3, array, array2);
				}
			}
			if (passiveItem is BasicStatPickup)
			{
				BasicStatPickup basicStatPickup = passiveItem as BasicStatPickup;
				for (int num4 = 0; num4 < basicStatPickup.modifiers.Count; num4++)
				{
					StatModifier statModifier4 = basicStatPickup.modifiers[num4];
					if (!passiveItem.HasBeenStatProcessed && statModifier4.statToBoost == PlayerStats.StatType.Health && statModifier4.amount > 0f)
					{
						num += statModifier4.amount;
					}
					this.ApplyStatModifier(statModifier4, array, array2);
				}
			}
			if (passiveItem is CoopPassiveItem && (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER || (GameManager.Instance.PrimaryPlayer.healthHaver && GameManager.Instance.PrimaryPlayer.healthHaver.IsDead) || owner.HasActiveBonusSynergy(CustomSynergyType.THE_TRUE_HERO, false)))
			{
				CoopPassiveItem coopPassiveItem = passiveItem as CoopPassiveItem;
				for (int num5 = 0; num5 < coopPassiveItem.modifiers.Count; num5++)
				{
					StatModifier statModifier5 = coopPassiveItem.modifiers[num5];
					this.ApplyStatModifier(statModifier5, array, array2);
				}
			}
			if (passiveItem is MetronomeItem)
			{
				float currentMultiplier = (passiveItem as MetronomeItem).GetCurrentMultiplier();
				array2[5] *= currentMultiplier;
			}
			passiveItem.HasBeenStatProcessed = true;
		}
		if (owner.inventory != null && owner.inventory.AllGuns != null)
		{
			if (owner.inventory.CurrentGun != null && owner.inventory.CurrentGun.currentGunStatModifiers != null && owner.inventory.CurrentGun.currentGunStatModifiers.Length > 0)
			{
				for (int num6 = 0; num6 < owner.inventory.CurrentGun.currentGunStatModifiers.Length; num6++)
				{
					StatModifier statModifier6 = owner.inventory.CurrentGun.currentGunStatModifiers[num6];
					this.ApplyStatModifier(statModifier6, array, array2);
				}
			}
			for (int num7 = 0; num7 < owner.inventory.AllGuns.Count; num7++)
			{
				if (owner.inventory.AllGuns[num7] && owner.inventory.AllGuns[num7].passiveStatModifiers != null && owner.inventory.AllGuns[num7].passiveStatModifiers.Length > 0)
				{
					for (int num8 = 0; num8 < owner.inventory.AllGuns[num7].passiveStatModifiers.Length; num8++)
					{
						StatModifier statModifier7 = owner.inventory.AllGuns[num7].passiveStatModifiers[num8];
						this.ApplyStatModifier(statModifier7, array, array2);
					}
				}
			}
		}
		for (int num9 = 0; num9 < owner.activeItems.Count; num9++)
		{
			PlayerItem playerItem = owner.activeItems[num9];
			if (playerItem.passiveStatModifiers != null && playerItem.passiveStatModifiers.Length > 0)
			{
				for (int num10 = 0; num10 < playerItem.passiveStatModifiers.Length; num10++)
				{
					StatModifier statModifier8 = playerItem.passiveStatModifiers[num10];
					if (!playerItem.HasBeenStatProcessed && statModifier8.statToBoost == PlayerStats.StatType.Health && statModifier8.amount > 0f)
					{
						num += statModifier8.amount;
					}
					this.ApplyStatModifier(statModifier8, array, array2);
				}
			}
			StatHolder component = playerItem.GetComponent<StatHolder>();
			if (component && (!component.RequiresPlayerItemActive || playerItem.IsCurrentlyActive))
			{
				for (int num11 = 0; num11 < component.modifiers.Length; num11++)
				{
					StatModifier statModifier9 = component.modifiers[num11];
					if (!playerItem.HasBeenStatProcessed && statModifier9.statToBoost == PlayerStats.StatType.Health && statModifier9.amount > 0f)
					{
						num += statModifier9.amount;
					}
					this.ApplyStatModifier(statModifier9, array, array2);
				}
			}
			playerItem.HasBeenStatProcessed = true;
		}
		PlayerItem currentItem = owner.CurrentItem;
		if (currentItem && currentItem is ActiveBasicStatItem && currentItem.IsActive)
		{
			ActiveBasicStatItem activeBasicStatItem = currentItem as ActiveBasicStatItem;
			for (int num12 = 0; num12 < activeBasicStatItem.modifiers.Count; num12++)
			{
				StatModifier statModifier10 = activeBasicStatItem.modifiers[num12];
				this.ApplyStatModifier(statModifier10, array, array2);
			}
		}
		for (int num13 = 0; num13 < this.StatValues.Count; num13++)
		{
			this.StatValues[num13] = this.BaseStatValues[num13] * array2[num13] + array[num13];
		}
		float num14 = 0f;
		int num15 = ((!owner.AllowZeroHealthState) ? 1 : 0);
		if (this.StatValues[3] < (float)num15)
		{
			this.StatValues[3] = (float)num15;
		}
		if (owner.ForceZeroHealthState)
		{
			this.StatValues[3] = 0f;
		}
		if (owner.healthHaver.GetMaxHealth() != this.StatValues[3] + num14)
		{
			owner.healthHaver.SetHealthMaximum(this.StatValues[3] + num14, new float?(num), false);
		}
		owner.UpdateInventoryMaxGuns();
		owner.UpdateInventoryMaxItems();
		this.RebuildGunVolleys(owner);
		int totalCurse2 = PlayerStats.GetTotalCurse();
		if (totalCurse2 > totalCurse && !MidGameSaveData.IsInitializingPlayerData)
		{
			owner.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, true, false, false);
		}
		if (totalCurse2 >= 10 && !MidGameSaveData.IsInitializingPlayerData)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.HAVE_MAX_CURSE, 0);
			if (!GameManager.Instance.Dungeon.CurseReaperActive)
			{
				GameManager.Instance.Dungeon.SpawnCurseReaper();
			}
		}
	}

	// Token: 0x17001387 RID: 4999
	// (get) Token: 0x060082E1 RID: 33505 RVA: 0x00358628 File Offset: 0x00356828
	public float Magnificence
	{
		get
		{
			return this.m_magnificence + this.m_floorMagnificence;
		}
	}

	// Token: 0x060082E2 RID: 33506 RVA: 0x00358638 File Offset: 0x00356838
	public void AddFloorMagnificence(float m)
	{
		this.m_floorMagnificence += m;
	}

	// Token: 0x060082E3 RID: 33507 RVA: 0x00358648 File Offset: 0x00356848
	public void ToNextLevel()
	{
		this.m_magnificence += this.m_floorMagnificence;
		this.m_floorMagnificence = 0f;
	}

	// Token: 0x040085D1 RID: 34257
	public int NumBlanksPerFloor = 3;

	// Token: 0x040085D2 RID: 34258
	public int NumBlanksPerFloorCoop = 2;

	// Token: 0x040085D3 RID: 34259
	public float rollDamage = 4f;

	// Token: 0x040085D4 RID: 34260
	[Header("Status Effect Things")]
	public bool UsesFireSourceEffect;

	// Token: 0x040085D5 RID: 34261
	public GameActorFireEffect OnFireSourceEffect;

	// Token: 0x040085D6 RID: 34262
	[SerializeField]
	[Header("Base Stat Values")]
	public List<float> BaseStatValues;

	// Token: 0x040085D7 RID: 34263
	[NonSerialized]
	public List<int> PreviouslyActiveSynergies;

	// Token: 0x040085D8 RID: 34264
	[NonSerialized]
	public List<CustomSynergyType> ActiveCustomSynergies = new List<CustomSynergyType>();

	// Token: 0x040085D9 RID: 34265
	protected List<float> StatValues;

	// Token: 0x040085DB RID: 34267
	private const bool c_BonusSynergies = true;

	// Token: 0x040085DC RID: 34268
	protected float m_magnificence;

	// Token: 0x040085DD RID: 34269
	protected float m_floorMagnificence;

	// Token: 0x020015FF RID: 5631
	public enum StatType
	{
		// Token: 0x040085DF RID: 34271
		MovementSpeed,
		// Token: 0x040085E0 RID: 34272
		RateOfFire,
		// Token: 0x040085E1 RID: 34273
		Accuracy,
		// Token: 0x040085E2 RID: 34274
		Health,
		// Token: 0x040085E3 RID: 34275
		Coolness,
		// Token: 0x040085E4 RID: 34276
		Damage,
		// Token: 0x040085E5 RID: 34277
		ProjectileSpeed,
		// Token: 0x040085E6 RID: 34278
		AdditionalGunCapacity,
		// Token: 0x040085E7 RID: 34279
		AdditionalItemCapacity,
		// Token: 0x040085E8 RID: 34280
		AmmoCapacityMultiplier,
		// Token: 0x040085E9 RID: 34281
		ReloadSpeed,
		// Token: 0x040085EA RID: 34282
		AdditionalShotPiercing,
		// Token: 0x040085EB RID: 34283
		KnockbackMultiplier,
		// Token: 0x040085EC RID: 34284
		GlobalPriceMultiplier,
		// Token: 0x040085ED RID: 34285
		Curse,
		// Token: 0x040085EE RID: 34286
		PlayerBulletScale,
		// Token: 0x040085EF RID: 34287
		AdditionalClipCapacityMultiplier,
		// Token: 0x040085F0 RID: 34288
		AdditionalShotBounces,
		// Token: 0x040085F1 RID: 34289
		AdditionalBlanksPerFloor,
		// Token: 0x040085F2 RID: 34290
		ShadowBulletChance,
		// Token: 0x040085F3 RID: 34291
		ThrownGunDamage,
		// Token: 0x040085F4 RID: 34292
		DodgeRollDamage,
		// Token: 0x040085F5 RID: 34293
		DamageToBosses,
		// Token: 0x040085F6 RID: 34294
		EnemyProjectileSpeedMultiplier,
		// Token: 0x040085F7 RID: 34295
		ExtremeShadowBulletChance,
		// Token: 0x040085F8 RID: 34296
		ChargeAmountMultiplier,
		// Token: 0x040085F9 RID: 34297
		RangeMultiplier,
		// Token: 0x040085FA RID: 34298
		DodgeRollDistanceMultiplier,
		// Token: 0x040085FB RID: 34299
		DodgeRollSpeedMultiplier,
		// Token: 0x040085FC RID: 34300
		TarnisherClipCapacityMultiplier,
		// Token: 0x040085FD RID: 34301
		MoneyMultiplierFromEnemies
	}
}
