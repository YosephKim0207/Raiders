using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200145E RID: 5214
public class PegasusBootsItem : PassiveItem
{
	// Token: 0x06007670 RID: 30320 RVA: 0x002F2548 File Offset: 0x002F0748
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier *= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier *= this.DodgeRollTimeMultiplier;
			player.rollStats.additionalInvulnerabilityFrames += this.AdditionalInvulnerabilityFrames;
		}
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
		player.OnRollStarted += this.OnRollStarted;
		base.Pickup(player);
	}

	// Token: 0x06007671 RID: 30321 RVA: 0x002F2654 File Offset: 0x002F0854
	private void OnRollStarted(PlayerController obj, Vector2 dirVec)
	{
	}

	// Token: 0x06007672 RID: 30322 RVA: 0x002F2658 File Offset: 0x002F0858
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollDistanceMultiplier /= this.DodgeRollDistanceMultiplier;
			player.rollStats.rollTimeMultiplier /= this.DodgeRollTimeMultiplier;
			player.rollStats.additionalInvulnerabilityFrames -= this.AdditionalInvulnerabilityFrames;
			player.rollStats.additionalInvulnerabilityFrames = Mathf.Max(player.rollStats.additionalInvulnerabilityFrames, 0);
		}
		if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		player.OnRollStarted -= this.OnRollStarted;
		debrisObject.GetComponent<PegasusBootsItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007673 RID: 30323 RVA: 0x002F277C File Offset: 0x002F097C
	protected override void OnDestroy()
	{
		if (this.m_pickedUp && this.m_owner && PassiveItem.ActiveFlagItems != null && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		if (this.m_owner != null)
		{
			this.m_owner.OnRollStarted -= this.OnRollStarted;
		}
		base.OnDestroy();
	}

	// Token: 0x0400784B RID: 30795
	public bool ModifiesDodgeRoll;

	// Token: 0x0400784C RID: 30796
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public float DodgeRollTimeMultiplier = 0.9f;

	// Token: 0x0400784D RID: 30797
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public float DodgeRollDistanceMultiplier = 1.25f;

	// Token: 0x0400784E RID: 30798
	[ShowInInspectorIf("ModifiesDodgeRoll", false)]
	public int AdditionalInvulnerabilityFrames;
}
