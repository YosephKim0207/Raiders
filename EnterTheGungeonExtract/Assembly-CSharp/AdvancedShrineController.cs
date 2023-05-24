using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020010DD RID: 4317
public class AdvancedShrineController : DungeonPlaceableBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x06005F14 RID: 24340 RVA: 0x00248574 File Offset: 0x00246774
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_parentRoom = room;
		if (!this.IsLegendaryHeroShrine)
		{
			room.OptionalDoorTopDecorable = ResourceCache.Acquire("Global Prefabs/Shrine_Lantern") as GameObject;
			if (!room.IsOnCriticalPath && room.connectedRooms.Count == 1)
			{
				room.ShouldAttemptProceduralLock = true;
				room.AttemptProceduralLockChance = Mathf.Max(room.AttemptProceduralLockChance, UnityEngine.Random.Range(0.3f, 0.5f));
			}
		}
		this.RegisterMinimapIcon();
	}

	// Token: 0x06005F15 RID: 24341 RVA: 0x002485F4 File Offset: 0x002467F4
	public void Start()
	{
		if (base.specRigidbody)
		{
			base.specRigidbody.PreventPiercing = true;
		}
		if (!StaticReferenceManager.AllAdvancedShrineControllers.Contains(this))
		{
			StaticReferenceManager.AllAdvancedShrineControllers.Add(this);
		}
	}

	// Token: 0x06005F16 RID: 24342 RVA: 0x00248630 File Offset: 0x00246830
	public void RegisterMinimapIcon()
	{
		this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_parentRoom, (GameObject)BraveResources.Load("Global Prefabs/Minimap_Shrine_Icon", ".prefab"), false);
	}

	// Token: 0x06005F17 RID: 24343 RVA: 0x00248660 File Offset: 0x00246860
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_parentRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06005F18 RID: 24344 RVA: 0x00248690 File Offset: 0x00246890
	private bool CheckCosts(PlayerController interactor)
	{
		bool flag = true;
		for (int i = 0; i < this.Costs.Count; i++)
		{
			if (!this.Costs[i].CheckCost(interactor))
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06005F19 RID: 24345 RVA: 0x002486DC File Offset: 0x002468DC
	private bool CheckAndApplyCosts(PlayerController interactor)
	{
		bool flag = this.CheckCosts(interactor);
		if (flag)
		{
			for (int i = 0; i < this.Costs.Count; i++)
			{
				this.Costs[i].ApplyCost(interactor);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06005F1A RID: 24346 RVA: 0x00248728 File Offset: 0x00246928
	private void ResetForReuse()
	{
		this.m_useCount--;
	}

	// Token: 0x06005F1B RID: 24347 RVA: 0x00248738 File Offset: 0x00246938
	private ShrineCost GetRandomCost()
	{
		float num = 0f;
		for (int i = 0; i < this.Costs.Count; i++)
		{
			num += this.Costs[i].rngWeight;
		}
		float num2 = UnityEngine.Random.value * num;
		float num3 = 0f;
		for (int j = 0; j < this.Costs.Count; j++)
		{
			num3 += this.Costs[j].rngWeight;
			if (num3 >= num2)
			{
				return this.Costs[j];
			}
		}
		return this.Costs[this.Costs.Count - 1];
	}

	// Token: 0x06005F1C RID: 24348 RVA: 0x002487EC File Offset: 0x002469EC
	private ShrineBenefit GetRandomBenefit()
	{
		float num = 0f;
		for (int i = 0; i < this.Benefits.Count; i++)
		{
			num += this.Benefits[i].rngWeight;
		}
		float num2 = UnityEngine.Random.value * num;
		float num3 = 0f;
		for (int j = 0; j < this.Benefits.Count; j++)
		{
			num3 += this.Benefits[j].rngWeight;
			if (num3 >= num2)
			{
				return this.Benefits[j];
			}
		}
		return this.Benefits[this.Benefits.Count - 1];
	}

	// Token: 0x06005F1D RID: 24349 RVA: 0x002488A0 File Offset: 0x00246AA0
	private void DoShrineEffect(PlayerController player)
	{
		if (this.IsJunkShrine)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SER_JUNKAN_UNLOCKED, true);
		}
		if (this.IsGlassShrine)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_GLASS_SHRINE, true);
			AkSoundEngine.PostEvent("Play_OBJ_mirror_shatter_01", base.gameObject);
			AkSoundEngine.PostEvent("Play_OBJ_crystal_shatter_01", base.gameObject);
		}
		if (this.IsBloodShrine)
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_BLOOD_SHRINED, 1f);
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_BLOOD_SHRINED) >= 2f)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_LIFE_ORB, true);
			}
		}
		if (this.IsHealthArmorSwapShrine)
		{
			AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
			player.HealthAndArmorSwapped = !player.HealthAndArmorSwapped;
			if (this.onPlayerVFX != null)
			{
				player.PlayEffectOnActor(this.onPlayerVFX, this.playerVFXOffset, true, false, false);
			}
			if (base.transform.parent != null)
			{
				EncounterTrackable component = base.transform.parent.gameObject.GetComponent<EncounterTrackable>();
				if (component != null)
				{
					if (this.m_instanceMinimapIcon == null && this.EncounterNotificationSprite == null)
					{
						this.RegisterMinimapIcon();
					}
					component.ForceDoNotification(this.EncounterNotificationSprite ?? this.m_instanceMinimapIcon.GetComponent<tk2dBaseSprite>());
				}
			}
		}
		else if (this.IsRNGShrine)
		{
			if (UnityEngine.Random.value < 0.001f)
			{
				player.healthHaver.TriggerInvulnerabilityPeriod(-1f);
				player.knockbackDoer.ApplyKnockback(player.CenterPosition - base.specRigidbody.UnitCenter, 150f, false);
				Exploder.DoDefaultExplosion(base.specRigidbody.UnitCenter, Vector2.zero, null, false, CoreDamageTypes.None, false);
				StatModifier statModifier = new StatModifier();
				statModifier.statToBoost = PlayerStats.StatType.Health;
				statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				statModifier.amount = Mathf.Min(0f, -1f * (Mathf.Ceil(player.healthHaver.GetMaxHealth()) - 1f));
				StatModifier statModifier2 = new StatModifier();
				statModifier2.statToBoost = PlayerStats.StatType.Damage;
				statModifier2.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
				statModifier2.amount = 4f;
				StatModifier statModifier3 = new StatModifier();
				statModifier3.statToBoost = PlayerStats.StatType.Curse;
				statModifier3.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				statModifier3.amount = 10f;
				player.ownerlessStatModifiers.Add(statModifier);
				player.ownerlessStatModifiers.Add(statModifier2);
				player.stats.RecalculateStats(player, false, false);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
				ShrineCost randomCost = this.GetRandomCost();
				ShrineBenefit randomBenefit = this.GetRandomBenefit();
				if (randomCost.costType == ShrineCost.CostType.HEALTH)
				{
					randomCost.cost = UnityEngine.Random.Range(1, 3);
					if ((float)randomCost.cost >= player.healthHaver.GetCurrentHealth())
					{
						randomCost.cost = 1;
					}
				}
				if (randomCost.costType == ShrineCost.CostType.STATS && player.healthHaver.GetMaxHealth() > 2f)
				{
					randomCost.cost = UnityEngine.Random.Range(1, 3);
				}
				if (randomCost.costType == ShrineCost.CostType.BLANK)
				{
					randomCost.cost = UnityEngine.Random.Range(1, player.Blanks + 1);
				}
				if (randomCost.costType == ShrineCost.CostType.MONEY)
				{
					randomCost.cost = Mathf.FloorToInt((float)player.carriedConsumables.Currency * UnityEngine.Random.Range(0.25f, 1f));
				}
				if (randomBenefit.benefitType == ShrineBenefit.BenefitType.MONEY)
				{
					randomBenefit.amount = (float)UnityEngine.Random.Range(20, 100);
				}
				if (randomBenefit.benefitType == ShrineBenefit.BenefitType.HEALTH)
				{
					randomBenefit.amount = (float)Mathf.RoundToInt(UnityEngine.Random.Range(1f, player.healthHaver.GetMaxHealth()));
				}
				if (randomBenefit.benefitType == ShrineBenefit.BenefitType.STATS)
				{
					if (randomBenefit.statMods[0].statToBoost == PlayerStats.StatType.Health)
					{
						randomBenefit.statMods[0].amount = (float)UnityEngine.Random.Range(1, 3);
					}
					else if (randomBenefit.statMods[0].statToBoost == PlayerStats.StatType.MovementSpeed)
					{
						randomBenefit.statMods[0].amount = UnityEngine.Random.Range(1.5f, 4f);
					}
					else if (randomBenefit.statMods[0].statToBoost == PlayerStats.StatType.Damage)
					{
						randomBenefit.statMods[0].amount = UnityEngine.Random.Range(1.2f, 1.5f);
					}
				}
				if (randomBenefit.benefitType == ShrineBenefit.BenefitType.BLANK)
				{
					randomBenefit.amount = (float)UnityEngine.Random.Range(1, 11);
				}
				if (randomBenefit.benefitType == ShrineBenefit.BenefitType.ARMOR)
				{
					randomBenefit.amount = (float)UnityEngine.Random.Range(1, 4);
				}
				if (randomBenefit.benefitType == ShrineBenefit.BenefitType.SPAWN_CHEST)
				{
					randomBenefit.IsRNGChest = true;
				}
				string text = string.Empty;
				if (randomCost.CheckCost(player))
				{
					randomCost.ApplyCost(player);
					text += StringTableManager.GetItemsString(randomCost.rngString, -1);
				}
				else
				{
					text += StringTableManager.GetItemsString("#SHRINE_DICE_BAD_FAIL", -1);
				}
				text += " + ";
				randomBenefit.ApplyBenefit(player);
				text += StringTableManager.GetItemsString(randomBenefit.rngString, -1);
				if (this.m_instanceMinimapIcon == null)
				{
					this.RegisterMinimapIcon();
				}
				if (this.EncounterNotificationSprite != null)
				{
					GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetItemsString("#SHRINE_DICE_ENCNAME", -1), text, this.EncounterNotificationSprite.Collection, this.EncounterNotificationSprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
				}
				else
				{
					GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetItemsString("#SHRINE_DICE_ENCNAME", -1), text, this.m_instanceMinimapIcon.GetComponent<tk2dBaseSprite>().Collection, this.m_instanceMinimapIcon.GetComponent<tk2dBaseSprite>().spriteId, UINotificationController.NotificationColor.SILVER, false, false);
				}
			}
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.DICE_SHRINES_USED, 1f);
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.DICE_SHRINES_USED) >= 2f && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_FORGE) >= 1f)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.DAISUKE_IS_UNLOCKABLE, true);
			}
		}
		else if (this.IsLegendaryHeroShrine)
		{
			int totalCurse = PlayerStats.GetTotalCurse();
			int num = ((this.m_useCount <= 0) ? 5 : 9);
			if (totalCurse >= 5)
			{
				num = 9;
			}
			Debug.LogError(string.Concat(new object[] { "total curse: ", totalCurse, "|", num, "|", this.m_useCount }));
			if (totalCurse < num)
			{
				StatModifier statModifier4 = new StatModifier();
				statModifier4.statToBoost = PlayerStats.StatType.Curse;
				statModifier4.amount = (float)(num - totalCurse);
				statModifier4.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				player.ownerlessStatModifiers.Add(statModifier4);
				player.stats.RecalculateStats(player, false, false);
			}
		}
		else
		{
			if (!this.CheckAndApplyCosts(player))
			{
				this.ResetForReuse();
				return;
			}
			AkSoundEngine.PostEvent("Play_OBJ_shrine_accept_01", base.gameObject);
			for (int i = 0; i < this.Benefits.Count; i++)
			{
				this.Benefits[i].ApplyBenefit(player);
			}
			if (this.IncrementMoneyCostEachUse)
			{
				for (int j = 0; j < this.Costs.Count; j++)
				{
					if (this.Costs[j].costType == ShrineCost.CostType.MONEY)
					{
						this.Costs[j].cost = this.Costs[j].cost + this.IncrementMoneyCostAmount;
					}
				}
			}
			if (this.onPlayerVFX != null)
			{
				player.PlayEffectOnActor(this.onPlayerVFX, this.playerVFXOffset, true, false, false);
			}
			if (base.transform.parent != null)
			{
				EncounterTrackable component2 = base.transform.parent.gameObject.GetComponent<EncounterTrackable>();
				if (component2 != null)
				{
					if (this.m_instanceMinimapIcon == null && this.EncounterNotificationSprite == null)
					{
						this.RegisterMinimapIcon();
					}
					component2.ForceDoNotification(this.EncounterNotificationSprite ?? this.m_instanceMinimapIcon.GetComponent<tk2dBaseSprite>());
				}
			}
		}
		if (!this.CanBeReused)
		{
			this.GetRidOfMinimapIcon();
		}
		if (this.ShattersOnUse)
		{
			this.ShatterSpriteDisable.renderer.enabled = false;
			this.ShatterSystem.SetActive(true);
			this.ShatterSystem.GetComponent<ParticleSystem>().Play();
		}
	}

	// Token: 0x06005F1E RID: 24350 RVA: 0x0024914C File Offset: 0x0024734C
	public float GetDistanceToPoint(Vector2 point)
	{
		if (base.sprite == null)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
		if (this.IsLegendaryHeroShrine && point.y > vector.y + 0.5f)
		{
			return 1000f;
		}
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x06005F1F RID: 24351 RVA: 0x002491D0 File Offset: 0x002473D0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06005F20 RID: 24352 RVA: 0x002491D8 File Offset: 0x002473D8
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

	// Token: 0x06005F21 RID: 24353 RVA: 0x00249210 File Offset: 0x00247410
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

	// Token: 0x06005F22 RID: 24354 RVA: 0x00249240 File Offset: 0x00247440
	private IEnumerator HandleShrineConversation(PlayerController interactor)
	{
		string targetDisplayKey = this.displayTextKey;
		if (this.IsCleanseShrine)
		{
			int totalCurse = PlayerStats.GetTotalCurse();
			if (totalCurse < 3)
			{
				targetDisplayKey = "#SHRINE_CLEANSE_DISPLAY_01";
			}
			else if (totalCurse < 5)
			{
				targetDisplayKey = "#SHRINE_CLEANSE_DISPLAY_02";
			}
			else if (totalCurse < 7)
			{
				targetDisplayKey = "#SHRINE_CLEANSE_DISPLAY_03";
			}
			else if (totalCurse < 10)
			{
				targetDisplayKey = "#SHRINE_CLEANSE_DISPLAY_04";
			}
			else
			{
				targetDisplayKey = "#SHRINE_CLEANSE_DISPLAY_05";
			}
		}
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, StringTableManager.GetLongString(targetDisplayKey), true, false);
		int selectedResponse = -1;
		interactor.SetInputOverride("shrineConversation");
		yield return null;
		bool canUse = (this.IsHealthArmorSwapShrine && this.m_totalUseCount == 0) || this.IsRNGShrine || this.CheckCosts(interactor);
		if (this.Costs.Count == 1 && this.Costs[0].costType == ShrineCost.CostType.CURRENT_GUN && this.Benefits.Count == 1 && this.Benefits[0].benefitType == ShrineBenefit.BenefitType.HEALTH && interactor.healthHaver && interactor.healthHaver.GetCurrentHealthPercentage() == 1f)
		{
			canUse = false;
		}
		if (this.IsCleanseShrine && PlayerStats.GetTotalCurse() == 0)
		{
			canUse = false;
		}
		if (this.IsLegendaryHeroShrine && PlayerStats.GetTotalCurse() >= 9)
		{
			canUse = false;
		}
		if (canUse)
		{
			string text = StringTableManager.GetString(this.acceptOptionKey);
			if (this.IsCleanseShrine)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					" (",
					(this.Costs[0].cost * PlayerStats.GetTotalCurse()).ToString(),
					" ",
					StringTableManager.GetString("#COINS"),
					")"
				});
			}
			else if (this.IncrementMoneyCostEachUse)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					" (",
					this.Costs[0].cost.ToString(),
					" ",
					StringTableManager.GetString("#COINS"),
					")"
				});
			}
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, text, StringTableManager.GetString(this.declineOptionKey));
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
			this.m_totalUseCount++;
			if (this.IsLegendaryHeroShrine && this.m_totalUseCount >= 2)
			{
				this.CanBeReused = false;
			}
			if (this.CanBeReused)
			{
				this.ResetForReuse();
			}
		}
		else
		{
			this.ResetForReuse();
		}
		yield break;
	}

	// Token: 0x06005F23 RID: 24355 RVA: 0x00249264 File Offset: 0x00247464
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

	// Token: 0x06005F24 RID: 24356 RVA: 0x00249288 File Offset: 0x00247488
	public void Interact(PlayerController interactor)
	{
		if (TextBoxManager.HasTextBox(this.talkPoint))
		{
			return;
		}
		if (this.m_useCount > 0 || this.IsBlankShrine)
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

	// Token: 0x06005F25 RID: 24357 RVA: 0x002492F8 File Offset: 0x002474F8
	public void OnBlank()
	{
		if (this.IsBlankShrine && UnityEngine.Random.value < this.m_curChanceToBlankChestIntoExistence)
		{
			this.m_useCount++;
			this.m_curChanceToBlankChestIntoExistence = Mathf.Max(0.25f, this.m_curChanceToBlankChestIntoExistence - 0.45f);
			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			IntVector2? randomAvailableCell = absoluteRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 4), new CellTypes?(CellTypes.FLOOR), false, null);
			IntVector2? intVector = ((randomAvailableCell == null) ? null : new IntVector2?(randomAvailableCell.GetValueOrDefault() + IntVector2.One));
			if (intVector != null)
			{
				GameManager.Instance.RewardManager.SpawnRoomClearChestAt(intVector.Value);
			}
			else
			{
				GameManager.Instance.RewardManager.SpawnRoomClearChestAt(absoluteRoom.GetBestRewardLocation(new IntVector2(3, 3), RoomHandler.RewardLocationStyle.Original, true) + IntVector2.Up);
			}
		}
	}

	// Token: 0x06005F26 RID: 24358 RVA: 0x002493FC File Offset: 0x002475FC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06005F27 RID: 24359 RVA: 0x00249408 File Offset: 0x00247608
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllAdvancedShrineControllers.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x0400594E RID: 22862
	public string displayTextKey;

	// Token: 0x0400594F RID: 22863
	public string acceptOptionKey;

	// Token: 0x04005950 RID: 22864
	public string declineOptionKey;

	// Token: 0x04005951 RID: 22865
	public string spentOptionKey = "#SHRINE_GENERIC_SPENT";

	// Token: 0x04005952 RID: 22866
	public bool IsBlankShrine;

	// Token: 0x04005953 RID: 22867
	public bool IsRNGShrine;

	// Token: 0x04005954 RID: 22868
	public bool IsHealthArmorSwapShrine;

	// Token: 0x04005955 RID: 22869
	public bool IsJunkShrine;

	// Token: 0x04005956 RID: 22870
	public bool IsBloodShrine;

	// Token: 0x04005957 RID: 22871
	public bool IsGlassShrine;

	// Token: 0x04005958 RID: 22872
	public List<ShrineCost> Costs;

	// Token: 0x04005959 RID: 22873
	public List<ShrineBenefit> Benefits;

	// Token: 0x0400595A RID: 22874
	public bool CanBeReused;

	// Token: 0x0400595B RID: 22875
	public bool IsCleanseShrine;

	// Token: 0x0400595C RID: 22876
	public bool IsLegendaryHeroShrine;

	// Token: 0x0400595D RID: 22877
	public bool IncrementMoneyCostEachUse;

	// Token: 0x0400595E RID: 22878
	public int IncrementMoneyCostAmount = 10;

	// Token: 0x0400595F RID: 22879
	public bool ShattersOnUse;

	// Token: 0x04005960 RID: 22880
	public GameObject ShatterSystem;

	// Token: 0x04005961 RID: 22881
	public tk2dSprite ShatterSpriteDisable;

	// Token: 0x04005962 RID: 22882
	public tk2dBaseSprite AlternativeOutlineTarget;

	// Token: 0x04005963 RID: 22883
	public Transform talkPoint;

	// Token: 0x04005964 RID: 22884
	public GameObject onPlayerVFX;

	// Token: 0x04005965 RID: 22885
	public Vector3 playerVFXOffset;

	// Token: 0x04005966 RID: 22886
	public tk2dBaseSprite EncounterNotificationSprite;

	// Token: 0x04005967 RID: 22887
	private RoomHandler m_parentRoom;

	// Token: 0x04005968 RID: 22888
	private GameObject m_instanceMinimapIcon;

	// Token: 0x04005969 RID: 22889
	private int m_useCount;

	// Token: 0x0400596A RID: 22890
	private int m_totalUseCount;

	// Token: 0x0400596B RID: 22891
	private const float ChanceToGoApeshit = 0.001f;

	// Token: 0x0400596C RID: 22892
	private float m_curChanceToBlankChestIntoExistence = 0.9f;
}
