using System;
using UnityEngine;

// Token: 0x0200170A RID: 5898
public class ReloadSwitchSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008920 RID: 35104 RVA: 0x0038E254 File Offset: 0x0038C454
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnPostFired, new Action<PlayerController, Gun>(this.HandlePostFired));
	}

	// Token: 0x06008921 RID: 35105 RVA: 0x0038E28C File Offset: 0x0038C48C
	private void HandlePostFired(PlayerController sourcePlayer, Gun sourceGun)
	{
		if (sourcePlayer.HasActiveBonusSynergy(this.RequiredSynergy, false) && this.m_gun.ClipShotsRemaining == 0)
		{
			for (int i = 0; i < sourcePlayer.inventory.AllGuns.Count; i++)
			{
				if (sourcePlayer.inventory.AllGuns[i].PickupObjectId == this.PartnerGunID && sourcePlayer.inventory.AllGuns[i].ammo > 0)
				{
					sourcePlayer.inventory.GunChangeForgiveness = true;
					sourcePlayer.ChangeToGunSlot(i, false);
					sourcePlayer.inventory.AllGuns[i].ForceImmediateReload(true);
					sourcePlayer.inventory.GunChangeForgiveness = false;
					return;
				}
			}
		}
	}

	// Token: 0x04008EF5 RID: 36597
	[PickupIdentifier]
	public int PartnerGunID;

	// Token: 0x04008EF6 RID: 36598
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008EF7 RID: 36599
	public bool ReloadsTargetGun = true;

	// Token: 0x04008EF8 RID: 36600
	private Gun m_gun;
}
