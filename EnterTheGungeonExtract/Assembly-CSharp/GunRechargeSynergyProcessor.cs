using System;
using UnityEngine;

// Token: 0x020016F0 RID: 5872
public class GunRechargeSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600887E RID: 34942 RVA: 0x0038933C File Offset: 0x0038753C
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.m_gun)
		{
			Gun gun = this.m_gun;
			gun.ModifyActiveCooldownDamage = (Func<float, float>)Delegate.Combine(gun.ModifyActiveCooldownDamage, new Func<float, float>(this.HandleActiveCooldownModification));
		}
	}

	// Token: 0x0600887F RID: 34943 RVA: 0x0038938C File Offset: 0x0038758C
	private float HandleActiveCooldownModification(float inDamage)
	{
		if (this.m_gun && this.m_gun.CurrentOwner is PlayerController && (this.m_gun.CurrentOwner as PlayerController).HasActiveBonusSynergy(this.SynergyToCheck, false))
		{
			return inDamage * this.CDR_Multiplier;
		}
		return inDamage;
	}

	// Token: 0x04008DE3 RID: 36323
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008DE4 RID: 36324
	public float CDR_Multiplier = 1f;

	// Token: 0x04008DE5 RID: 36325
	protected Gun m_gun;
}
