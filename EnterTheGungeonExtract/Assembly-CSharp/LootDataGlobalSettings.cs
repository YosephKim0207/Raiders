using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001550 RID: 5456
public class LootDataGlobalSettings : ScriptableObject
{
	// Token: 0x1700126C RID: 4716
	// (get) Token: 0x06007CEB RID: 31979 RVA: 0x00325F10 File Offset: 0x00324110
	public static LootDataGlobalSettings Instance
	{
		get
		{
			if (LootDataGlobalSettings.m_instance == null)
			{
				LootDataGlobalSettings.m_instance = (LootDataGlobalSettings)BraveResources.Load("GlobalLootSettings", ".asset");
			}
			return LootDataGlobalSettings.m_instance;
		}
	}

	// Token: 0x06007CEC RID: 31980 RVA: 0x00325F40 File Offset: 0x00324140
	public float GetModifierForClass(GunClass targetClass)
	{
		for (int i = 0; i < this.GunClassOverrides.Count; i++)
		{
			if (this.GunClassOverrides[i].classToModify == targetClass)
			{
				return this.GunClassOverrides[i].modifier;
			}
		}
		return this.GunClassModifier;
	}

	// Token: 0x04007FF4 RID: 32756
	private static LootDataGlobalSettings m_instance;

	// Token: 0x04007FF5 RID: 32757
	public float GunClassModifier = 0.2f;

	// Token: 0x04007FF6 RID: 32758
	[SerializeField]
	public List<GunClassModifierOverride> GunClassOverrides;
}
