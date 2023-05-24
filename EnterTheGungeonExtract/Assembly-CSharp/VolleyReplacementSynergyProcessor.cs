using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001715 RID: 5909
public class VolleyReplacementSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008944 RID: 35140 RVA: 0x0038F67C File Offset: 0x0038D87C
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.m_gun.RawSourceVolley != null)
		{
			this.m_cachedSourceVolley = this.m_gun.RawSourceVolley;
		}
		else
		{
			this.m_cachedSingleModule = this.m_gun.singleModule;
		}
	}

	// Token: 0x06008945 RID: 35141 RVA: 0x0038F6D4 File Offset: 0x0038D8D4
	private void Update()
	{
		PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
		if (!this.m_transformed && playerController && playerController.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			this.m_transformed = true;
			ProjectileVolleyData volley = this.m_gun.Volley;
			if (volley)
			{
				this.m_gun.RawSourceVolley = DuctTapeItem.TransferDuctTapeModules(volley, this.SynergyVolley, this.m_gun);
			}
			else
			{
				this.m_gun.RawSourceVolley = this.SynergyVolley;
			}
			playerController.stats.RecalculateStats(playerController, false, false);
		}
		else if (this.m_transformed && (!playerController || !playerController.HasActiveBonusSynergy(this.RequiredSynergy, false)))
		{
			this.m_transformed = false;
			ProjectileVolleyData volley2 = this.m_gun.Volley;
			if (volley2)
			{
				ProjectileVolleyData projectileVolleyData = ScriptableObject.CreateInstance<ProjectileVolleyData>();
				if (this.m_cachedSourceVolley != null)
				{
					projectileVolleyData.InitializeFrom(this.m_cachedSourceVolley);
				}
				else
				{
					projectileVolleyData.projectiles = new List<ProjectileModule>();
					projectileVolleyData.projectiles.Add(this.m_cachedSingleModule);
				}
				this.m_gun.RawSourceVolley = DuctTapeItem.TransferDuctTapeModules(volley2, projectileVolleyData, this.m_gun);
			}
			else if (this.m_cachedSourceVolley != null)
			{
				this.m_gun.RawSourceVolley = this.m_cachedSourceVolley;
			}
			else
			{
				this.m_gun.RawSourceVolley = null;
			}
			if (playerController)
			{
				playerController.stats.RecalculateStats(playerController, false, false);
			}
		}
	}

	// Token: 0x04008F47 RID: 36679
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008F48 RID: 36680
	public ProjectileVolleyData SynergyVolley;

	// Token: 0x04008F49 RID: 36681
	private ProjectileVolleyData m_cachedSourceVolley;

	// Token: 0x04008F4A RID: 36682
	private ProjectileModule m_cachedSingleModule;

	// Token: 0x04008F4B RID: 36683
	private Gun m_gun;

	// Token: 0x04008F4C RID: 36684
	private bool m_transformed;
}
