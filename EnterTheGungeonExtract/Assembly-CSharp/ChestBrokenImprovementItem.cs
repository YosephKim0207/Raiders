using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200136E RID: 4974
public class ChestBrokenImprovementItem : PassiveItem
{
	// Token: 0x060070B4 RID: 28852 RVA: 0x002CB9E0 File Offset: 0x002C9BE0
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		float num = this.ChanceForPickupQuality + this.ChanceForMinusOneQuality + this.ChanceForEqualQuality + this.ChanceForPlusOneQuality;
		ChestBrokenImprovementItem.PickupQualChance = this.ChanceForPickupQuality / num;
		ChestBrokenImprovementItem.MinusOneQualChance = this.ChanceForMinusOneQuality / num;
		ChestBrokenImprovementItem.EqualQualChance = this.ChanceForEqualQuality / num;
		ChestBrokenImprovementItem.PlusQualChance = this.ChanceForPlusOneQuality / num;
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
		base.Pickup(player);
	}

	// Token: 0x060070B5 RID: 28853 RVA: 0x002CBAD8 File Offset: 0x002C9CD8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		if (debrisObject && debrisObject.GetComponent<ChestBrokenImprovementItem>())
		{
			debrisObject.GetComponent<ChestBrokenImprovementItem>().m_pickedUpThisRun = true;
		}
		return debrisObject;
	}

	// Token: 0x060070B6 RID: 28854 RVA: 0x002CBB98 File Offset: 0x002C9D98
	protected override void OnDestroy()
	{
		BraveTime.ClearMultiplier(base.gameObject);
		if (this.m_pickedUp && PassiveItem.ActiveFlagItems != null && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		base.OnDestroy();
	}

	// Token: 0x0400703D RID: 28733
	public static float PickupQualChance;

	// Token: 0x0400703E RID: 28734
	public static float MinusOneQualChance = 0.5f;

	// Token: 0x0400703F RID: 28735
	public static float EqualQualChance = 0.45f;

	// Token: 0x04007040 RID: 28736
	public static float PlusQualChance = 0.05f;

	// Token: 0x04007041 RID: 28737
	public float ChanceForPickupQuality;

	// Token: 0x04007042 RID: 28738
	public float ChanceForMinusOneQuality = 0.5f;

	// Token: 0x04007043 RID: 28739
	public float ChanceForEqualQuality = 0.45f;

	// Token: 0x04007044 RID: 28740
	public float ChanceForPlusOneQuality = 0.05f;
}
