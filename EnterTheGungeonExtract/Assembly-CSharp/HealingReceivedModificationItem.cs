using System;
using UnityEngine;

// Token: 0x02001418 RID: 5144
public class HealingReceivedModificationItem : PassiveItem
{
	// Token: 0x060074C0 RID: 29888 RVA: 0x002E7E1C File Offset: 0x002E601C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		HealthHaver healthHaver = player.healthHaver;
		healthHaver.ModifyHealing = (Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>)Delegate.Combine(healthHaver.ModifyHealing, new Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>(this.ModifyIncomingHealing));
		base.Pickup(player);
	}

	// Token: 0x060074C1 RID: 29889 RVA: 0x002E7E58 File Offset: 0x002E6058
	private void ModifyIncomingHealing(HealthHaver source, HealthHaver.ModifyHealingEventArgs args)
	{
		if (args == EventArgs.Empty)
		{
			return;
		}
		if (UnityEngine.Random.value < this.ChanceToImproveHealing)
		{
			if (this.OnImprovedHealingVFX != null)
			{
				source.GetComponent<PlayerController>().PlayEffectOnActor(this.OnImprovedHealingVFX, Vector3.zero, true, false, false);
			}
			args.ModifiedHealing += this.HealingImprovedBy;
		}
	}

	// Token: 0x060074C2 RID: 29890 RVA: 0x002E7EC0 File Offset: 0x002E60C0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		HealthHaver healthHaver = player.healthHaver;
		healthHaver.ModifyHealing = (Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>)Delegate.Remove(healthHaver.ModifyHealing, new Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>(this.ModifyIncomingHealing));
		debrisObject.GetComponent<HealingReceivedModificationItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060074C3 RID: 29891 RVA: 0x002E7F0C File Offset: 0x002E610C
	protected override void OnDestroy()
	{
		if (this.m_pickedUp)
		{
			HealthHaver healthHaver = this.m_owner.healthHaver;
			healthHaver.ModifyHealing = (Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>)Delegate.Combine(healthHaver.ModifyHealing, new Action<HealthHaver, HealthHaver.ModifyHealingEventArgs>(this.ModifyIncomingHealing));
		}
		base.OnDestroy();
	}

	// Token: 0x04007696 RID: 30358
	public float ChanceToImproveHealing = 0.5f;

	// Token: 0x04007697 RID: 30359
	public float HealingImprovedBy = 0.5f;

	// Token: 0x04007698 RID: 30360
	public GameObject OnImprovedHealingVFX;
}
