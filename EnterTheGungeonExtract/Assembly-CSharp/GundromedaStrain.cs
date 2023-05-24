using System;
using UnityEngine;

// Token: 0x02001413 RID: 5139
public class GundromedaStrain : PassiveItem
{
	// Token: 0x0600749E RID: 29854 RVA: 0x002E6DC0 File Offset: 0x002E4FC0
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		AIActor.HealthModifier *= Mathf.Clamp01(1f - this.percentageHealthReduction);
	}

	// Token: 0x0600749F RID: 29855 RVA: 0x002E6DF4 File Offset: 0x002E4FF4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<GundromedaStrain>().m_pickedUpThisRun = true;
		AIActor.HealthModifier /= Mathf.Clamp01(1f - this.percentageHealthReduction);
		return debrisObject;
	}

	// Token: 0x060074A0 RID: 29856 RVA: 0x002E6E34 File Offset: 0x002E5034
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400767F RID: 30335
	public float percentageHealthReduction = 0.1f;
}
