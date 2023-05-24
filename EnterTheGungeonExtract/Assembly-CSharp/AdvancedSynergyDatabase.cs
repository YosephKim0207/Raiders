using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200133B RID: 4923
public class AdvancedSynergyDatabase : ScriptableObject
{
	// Token: 0x06006FA2 RID: 28578 RVA: 0x002C40BC File Offset: 0x002C22BC
	public void RebuildSynergies(PlayerController p, List<int> previouslyActiveSynergies)
	{
		if (!p)
		{
			return;
		}
		if (p.ActiveExtraSynergies == null)
		{
			p.ActiveExtraSynergies = new List<int>();
		}
		p.ActiveExtraSynergies.Clear();
		if (p.inventory == null)
		{
			return;
		}
		for (int i = 0; i < this.synergies.Length; i++)
		{
			if (this.synergies[i].SynergyIsAvailable(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer, -1))
			{
				p.ActiveExtraSynergies.Add(i);
			}
		}
	}

	// Token: 0x04006EE9 RID: 28393
	public static Color SynergyBlue = new Color(0.59607846f, 0.98039216f, 1f);

	// Token: 0x04006EEA RID: 28394
	[SerializeField]
	public AdvancedSynergyEntry[] synergies;
}
