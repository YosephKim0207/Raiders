using System;
using UnityEngine;

// Token: 0x020016FC RID: 5884
public class InfiniteAmmoSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088C6 RID: 35014 RVA: 0x0038B19C File Offset: 0x0038939C
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x060088C7 RID: 35015 RVA: 0x0038B1AC File Offset: 0x003893AC
	public void Update()
	{
		bool flag = this.m_gun && this.m_gun.OwnerHasSynergy(this.RequiredSynergy);
		if (flag && !this.m_processed)
		{
			this.m_gun.GainAmmo(this.m_gun.AdjustedMaxAmmo);
			this.m_gun.InfiniteAmmo = true;
			this.m_processed = true;
			if (this.PreventsReload)
			{
				this.m_cachedReloadTime = this.m_gun.reloadTime;
				this.m_gun.reloadTime = 0f;
			}
		}
		else if (!flag && this.m_processed)
		{
			this.m_gun.InfiniteAmmo = false;
			this.m_processed = false;
			if (this.PreventsReload)
			{
				this.m_gun.reloadTime = this.m_cachedReloadTime;
			}
		}
	}

	// Token: 0x04008E45 RID: 36421
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008E46 RID: 36422
	public bool PreventsReload = true;

	// Token: 0x04008E47 RID: 36423
	private bool m_processed;

	// Token: 0x04008E48 RID: 36424
	private Gun m_gun;

	// Token: 0x04008E49 RID: 36425
	private float m_cachedReloadTime = -1f;
}
