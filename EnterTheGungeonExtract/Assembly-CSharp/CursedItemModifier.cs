using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001391 RID: 5009
public class CursedItemModifier : MonoBehaviour
{
	// Token: 0x06007186 RID: 29062 RVA: 0x002D1ADC File Offset: 0x002CFCDC
	private void Start()
	{
		this.m_pickup = base.GetComponent<PickupObject>();
		if (this.m_pickup is PassiveItem)
		{
			PassiveItem passiveItem = this.m_pickup as PassiveItem;
			StatModifier[] passiveStatModifiers = passiveItem.passiveStatModifiers;
			StatModifier statModifier = new StatModifier();
			statModifier.amount = 1f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			Array.Resize<StatModifier>(ref passiveStatModifiers, passiveStatModifiers.Length + 1);
			this.m_addedModifier = statModifier;
			passiveStatModifiers[passiveStatModifiers.Length - 1] = statModifier;
			if (passiveItem.Owner != null)
			{
				passiveItem.Owner.stats.RecalculateStats(passiveItem.Owner, false, false);
			}
		}
		else if (this.m_pickup is PlayerItem)
		{
		}
	}

	// Token: 0x06007187 RID: 29063 RVA: 0x002D1B94 File Offset: 0x002CFD94
	private void OnDestroy()
	{
		if (this.m_pickup is PassiveItem)
		{
			PassiveItem passiveItem = this.m_pickup as PassiveItem;
			StatModifier[] passiveStatModifiers = passiveItem.passiveStatModifiers;
			List<StatModifier> list = new List<StatModifier>(passiveStatModifiers);
			bool flag = list.Remove(this.m_addedModifier);
			passiveItem.passiveStatModifiers = list.ToArray();
			if (passiveItem.Owner != null)
			{
				passiveItem.Owner.stats.RecalculateStats(passiveItem.Owner, false, false);
			}
		}
	}

	// Token: 0x04007149 RID: 29001
	private PickupObject m_pickup;

	// Token: 0x0400714A RID: 29002
	private StatModifier m_addedModifier;
}
