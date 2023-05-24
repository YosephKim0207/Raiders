using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014C9 RID: 5321
public class SynergyDatabase : ScriptableObject
{
	// Token: 0x060078E5 RID: 30949 RVA: 0x00305C8C File Offset: 0x00303E8C
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
			if (this.synergies[i].SynergyIsAvailable(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer))
			{
				p.ActiveExtraSynergies.Add(i);
			}
		}
	}

	// Token: 0x04007B37 RID: 31543
	public static Color SynergyBlue = new Color(0.59607846f, 0.98039216f, 1f);

	// Token: 0x04007B38 RID: 31544
	[SerializeField]
	public SynergyEntry[] synergies;
}
