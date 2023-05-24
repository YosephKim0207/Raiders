using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001712 RID: 5906
public class TransformGunSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008938 RID: 35128 RVA: 0x0038EB98 File Offset: 0x0038CD98
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x06008939 RID: 35129 RVA: 0x0038EBA8 File Offset: 0x0038CDA8
	private void Update()
	{
		if (Dungeon.IsGenerating || Dungeon.ShouldAttemptToLoadFromMidgameSave)
		{
			return;
		}
		if (this.m_gun && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (!this.m_gun.enabled)
			{
				return;
			}
			if (playerController.HasActiveBonusSynergy(this.SynergyToCheck, false) && !this.m_transformed)
			{
				this.m_transformed = true;
				this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.SynergyGunId) as Gun);
				if (this.ShouldResetAmmoAfterTransformation)
				{
					this.m_gun.ammo = this.ResetAmmoCount;
				}
			}
			else if (!playerController.HasActiveBonusSynergy(this.SynergyToCheck, false) && this.m_transformed)
			{
				this.m_transformed = false;
				this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.NonSynergyGunId) as Gun);
				if (this.ShouldResetAmmoAfterTransformation)
				{
					this.m_gun.ammo = this.ResetAmmoCount;
				}
			}
		}
		else if (this.m_gun && !this.m_gun.CurrentOwner && this.m_transformed)
		{
			this.m_transformed = false;
			this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.NonSynergyGunId) as Gun);
			if (this.ShouldResetAmmoAfterTransformation)
			{
				this.m_gun.ammo = this.ResetAmmoCount;
			}
		}
		this.ShouldResetAmmoAfterTransformation = false;
	}

	// Token: 0x04008F21 RID: 36641
	[LongNumericEnum]
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04008F22 RID: 36642
	[PickupIdentifier(typeof(Gun))]
	public int NonSynergyGunId = -1;

	// Token: 0x04008F23 RID: 36643
	[PickupIdentifier(typeof(Gun))]
	public int SynergyGunId = -1;

	// Token: 0x04008F24 RID: 36644
	private Gun m_gun;

	// Token: 0x04008F25 RID: 36645
	private bool m_transformed;

	// Token: 0x04008F26 RID: 36646
	[NonSerialized]
	public bool ShouldResetAmmoAfterTransformation;

	// Token: 0x04008F27 RID: 36647
	[NonSerialized]
	public int ResetAmmoCount;
}
