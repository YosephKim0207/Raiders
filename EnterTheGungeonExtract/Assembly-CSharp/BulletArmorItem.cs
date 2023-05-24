using System;

// Token: 0x02001362 RID: 4962
public class BulletArmorItem : PassiveItem
{
	// Token: 0x06007072 RID: 28786 RVA: 0x002C97F4 File Offset: 0x002C79F4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		if (!this.m_pickedUpThisRun)
		{
			this.m_player.healthHaver.Armor += 1f;
		}
		base.Pickup(player);
		GameManager.Instance.OnNewLevelFullyLoaded += this.GainArmorOnLevelLoad;
		this.ProcessLegendaryStatus(player);
	}

	// Token: 0x06007073 RID: 28787 RVA: 0x002C9860 File Offset: 0x002C7A60
	private void ProcessLegendaryStatus(PlayerController player)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		PassiveItem passiveItem = null;
		PassiveItem passiveItem2 = null;
		PassiveItem passiveItem3 = null;
		PassiveItem passiveItem4 = null;
		for (int i = 0; i < player.passiveItems.Count; i++)
		{
			if (player.passiveItems[i].DisplayName == "Gunknight Gauntlet")
			{
				flag = true;
				passiveItem3 = player.passiveItems[i];
			}
			if (player.passiveItems[i].DisplayName == "Gunknight Armor")
			{
				flag2 = true;
				passiveItem2 = player.passiveItems[i];
			}
			if (player.passiveItems[i].DisplayName == "Gunknight Helmet")
			{
				flag3 = true;
				passiveItem = player.passiveItems[i];
			}
			if (player.passiveItems[i].DisplayName == "Gunknight Greaves")
			{
				flag4 = true;
				passiveItem4 = player.passiveItems[i];
			}
		}
		if (flag && flag2 && flag3 && flag4)
		{
			passiveItem.CanBeDropped = false;
			passiveItem.CanBeSold = false;
			passiveItem2.CanBeDropped = false;
			passiveItem2.CanBeSold = false;
			passiveItem3.CanBeDropped = false;
			passiveItem3.CanBeSold = false;
			passiveItem4.CanBeDropped = false;
			passiveItem4.CanBeSold = false;
			player.OverrideAnimationLibrary = this.knightLibrary;
			player.SetOverrideShader(ShaderCache.Acquire(player.LocalShaderName));
			if (player.characterIdentity == PlayableCharacters.Eevee)
			{
				player.GetComponent<CharacterAnimationRandomizer>().AddOverrideAnimLibrary(this.knightLibrary);
			}
			StatModifier statModifier = new StatModifier();
			statModifier.amount = -1000f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier.statToBoost = PlayerStats.StatType.ReloadSpeed;
			player.ownerlessStatModifiers.Add(statModifier);
			StatModifier statModifier2 = new StatModifier();
			statModifier2.amount = 3f;
			statModifier2.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier2.statToBoost = PlayerStats.StatType.Curse;
			player.ownerlessStatModifiers.Add(statModifier2);
			player.stats.RecalculateStats(player, false, false);
			if (!PassiveItem.IsFlagSetForCharacter(player, typeof(BulletArmorItem)))
			{
				PassiveItem.IncrementFlag(player, typeof(BulletArmorItem));
			}
		}
		else if (PassiveItem.IsFlagSetForCharacter(player, typeof(BulletArmorItem)))
		{
			PassiveItem.DecrementFlag(player, typeof(BulletArmorItem));
			player.OverrideAnimationLibrary = null;
			player.ClearOverrideShader();
			if (player.characterIdentity == PlayableCharacters.Eevee)
			{
				player.GetComponent<CharacterAnimationRandomizer>().RemoveOverrideAnimLibrary(this.knightLibrary);
			}
			for (int j = 0; j < player.ownerlessStatModifiers.Count; j++)
			{
				if (player.ownerlessStatModifiers[j].statToBoost == PlayerStats.StatType.ReloadSpeed && player.ownerlessStatModifiers[j].amount == -1000f)
				{
					player.ownerlessStatModifiers.RemoveAt(j);
					break;
				}
			}
			player.stats.RecalculateStats(player, false, false);
		}
	}

	// Token: 0x06007074 RID: 28788 RVA: 0x002C9B5C File Offset: 0x002C7D5C
	public void GainArmorOnLevelLoad()
	{
		this.m_player.healthHaver.Armor += 1f;
	}

	// Token: 0x06007075 RID: 28789 RVA: 0x002C9B7C File Offset: 0x002C7D7C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<BulletArmorItem>().m_pickedUpThisRun = true;
		GameManager.Instance.OnNewLevelFullyLoaded -= this.GainArmorOnLevelLoad;
		this.ProcessLegendaryStatus(player);
		this.m_player = null;
		return debrisObject;
	}

	// Token: 0x06007076 RID: 28790 RVA: 0x002C9BC4 File Offset: 0x002C7DC4
	protected override void OnDestroy()
	{
		if (this.m_pickedUp && GameManager.HasInstance)
		{
			GameManager.Instance.OnNewLevelFullyLoaded -= this.GainArmorOnLevelLoad;
		}
		base.OnDestroy();
	}

	// Token: 0x04006FE5 RID: 28645
	public tk2dSpriteAnimation knightLibrary;

	// Token: 0x04006FE6 RID: 28646
	private PlayerController m_player;
}
