using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013EF RID: 5103
public class DuctTapeItem : PlayerItem
{
	// Token: 0x060073BF RID: 29631 RVA: 0x002E0868 File Offset: 0x002DEA68
	public override bool CanBeUsed(PlayerController user)
	{
		if (!user || user.inventory == null || user.inventory.AllGuns.Count < 2)
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < user.inventory.AllGuns.Count; i++)
		{
			if (user.inventory.AllGuns[i])
			{
				if (!user.inventory.AllGuns[i].InfiniteAmmo && user.inventory.AllGuns[i].CanActuallyBeDropped(user))
				{
					num++;
				}
			}
		}
		return num >= 2 && this.IsGunValid(user.CurrentGun, this.m_validSourceGun) && base.CanBeUsed(user);
	}

	// Token: 0x060073C0 RID: 29632 RVA: 0x002E0950 File Offset: 0x002DEB50
	public static ProjectileVolleyData TransferDuctTapeModules(ProjectileVolleyData source, ProjectileVolleyData target, Gun targetGun)
	{
		ProjectileVolleyData projectileVolleyData = ScriptableObject.CreateInstance<ProjectileVolleyData>();
		if (target != null)
		{
			projectileVolleyData.InitializeFrom(target);
		}
		else
		{
			projectileVolleyData.projectiles = new List<ProjectileModule>();
			projectileVolleyData.projectiles.Add(ProjectileModule.CreateClone(targetGun.singleModule, true, -1));
			projectileVolleyData.BeamRotationDegreesPerSecond = float.MaxValue;
		}
		for (int i = 0; i < source.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = source.projectiles[i];
			if (projectileModule.IsDuctTapeModule)
			{
				ProjectileModule projectileModule2 = ProjectileModule.CreateClone(projectileModule, true, -1);
				projectileVolleyData.projectiles.Add(projectileModule2);
			}
		}
		DuctTapeItem.ReconfigureVolley(projectileVolleyData);
		return projectileVolleyData;
	}

	// Token: 0x060073C1 RID: 29633 RVA: 0x002E09FC File Offset: 0x002DEBFC
	protected static ProjectileVolleyData CombineVolleys(Gun sourceGun, Gun mergeGun)
	{
		ProjectileVolleyData projectileVolleyData = ScriptableObject.CreateInstance<ProjectileVolleyData>();
		if (sourceGun.RawSourceVolley != null)
		{
			projectileVolleyData.InitializeFrom(sourceGun.RawSourceVolley);
		}
		else
		{
			projectileVolleyData.projectiles = new List<ProjectileModule>();
			projectileVolleyData.projectiles.Add(ProjectileModule.CreateClone(sourceGun.singleModule, true, -1));
			projectileVolleyData.BeamRotationDegreesPerSecond = float.MaxValue;
		}
		if (mergeGun.RawSourceVolley != null)
		{
			for (int i = 0; i < mergeGun.RawSourceVolley.projectiles.Count; i++)
			{
				ProjectileModule projectileModule = ProjectileModule.CreateClone(mergeGun.RawSourceVolley.projectiles[i], true, -1);
				projectileModule.IsDuctTapeModule = true;
				projectileModule.ignoredForReloadPurposes = projectileModule.ammoCost <= 0 || projectileModule.numberOfShotsInClip <= 0;
				projectileVolleyData.projectiles.Add(projectileModule);
				if (!string.IsNullOrEmpty(mergeGun.gunSwitchGroup) && i == 0)
				{
					projectileModule.runtimeGuid = ((projectileModule.runtimeGuid == null) ? Guid.NewGuid().ToString() : projectileModule.runtimeGuid);
					sourceGun.AdditionalShootSoundsByModule.Add(projectileModule.runtimeGuid, mergeGun.gunSwitchGroup);
				}
				if (mergeGun.RawSourceVolley.projectiles[i].runtimeGuid != null && mergeGun.AdditionalShootSoundsByModule.ContainsKey(mergeGun.RawSourceVolley.projectiles[i].runtimeGuid))
				{
					sourceGun.AdditionalShootSoundsByModule.Add(mergeGun.RawSourceVolley.projectiles[i].runtimeGuid, mergeGun.AdditionalShootSoundsByModule[mergeGun.RawSourceVolley.projectiles[i].runtimeGuid]);
				}
			}
		}
		else
		{
			ProjectileModule projectileModule2 = ProjectileModule.CreateClone(mergeGun.singleModule, true, -1);
			projectileModule2.IsDuctTapeModule = true;
			projectileModule2.ignoredForReloadPurposes = projectileModule2.ammoCost <= 0 || projectileModule2.numberOfShotsInClip <= 0;
			projectileVolleyData.projectiles.Add(projectileModule2);
			if (!string.IsNullOrEmpty(mergeGun.gunSwitchGroup))
			{
				projectileModule2.runtimeGuid = ((projectileModule2.runtimeGuid == null) ? Guid.NewGuid().ToString() : projectileModule2.runtimeGuid);
				sourceGun.AdditionalShootSoundsByModule.Add(projectileModule2.runtimeGuid, mergeGun.gunSwitchGroup);
			}
		}
		return projectileVolleyData;
	}

	// Token: 0x060073C2 RID: 29634 RVA: 0x002E0C68 File Offset: 0x002DEE68
	protected static void ReconfigureVolley(ProjectileVolleyData newVolley)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		int num = 0;
		for (int i = 0; i < newVolley.projectiles.Count; i++)
		{
			if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Automatic)
			{
				flag = true;
			}
			if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Beam)
			{
				flag = true;
			}
			if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Burst)
			{
				flag4 = true;
			}
			if (newVolley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Charged)
			{
				flag3 = true;
				num++;
			}
		}
		if (!flag && !flag2 && !flag3 && !flag4)
		{
			return;
		}
		if (!flag && !flag2 && !flag3 && flag4)
		{
			return;
		}
		int num2 = 0;
		for (int j = 0; j < newVolley.projectiles.Count; j++)
		{
			if (newVolley.projectiles[j].shootStyle == ProjectileModule.ShootStyle.SemiAutomatic)
			{
				newVolley.projectiles[j].shootStyle = ProjectileModule.ShootStyle.Automatic;
			}
			if (newVolley.projectiles[j].shootStyle == ProjectileModule.ShootStyle.Charged && num > 1)
			{
				num2++;
				if (num > 1)
				{
				}
			}
		}
	}

	// Token: 0x060073C3 RID: 29635 RVA: 0x002E0DBC File Offset: 0x002DEFBC
	protected Gun GetValidGun(PlayerController user, Gun excluded = null)
	{
		int num = user.inventory.AllGuns.IndexOf(user.CurrentGun);
		if (num < 0)
		{
			num = 0;
		}
		for (int i = num; i < num + user.inventory.AllGuns.Count; i++)
		{
			int num2 = i % user.inventory.AllGuns.Count;
			Gun gun = user.inventory.AllGuns[num2];
			if (!gun.InfiniteAmmo && gun.CanActuallyBeDropped(user))
			{
				if (!(gun == excluded))
				{
					return gun;
				}
			}
		}
		return null;
	}

	// Token: 0x060073C4 RID: 29636 RVA: 0x002E0E64 File Offset: 0x002DF064
	protected bool IsGunValid(Gun g, Gun excluded)
	{
		return !g.InfiniteAmmo && g.CanActuallyBeDropped(g.CurrentOwner as PlayerController) && !(g == excluded);
	}

	// Token: 0x060073C5 RID: 29637 RVA: 0x002E0E98 File Offset: 0x002DF098
	public static void DuctTapeGuns(Gun merged, Gun target)
	{
		ProjectileVolleyData projectileVolleyData = DuctTapeItem.CombineVolleys(target, merged);
		DuctTapeItem.ReconfigureVolley(projectileVolleyData);
		target.RawSourceVolley = projectileVolleyData;
		target.SetBaseMaxAmmo(target.GetBaseMaxAmmo() + merged.GetBaseMaxAmmo());
		target.GainAmmo(merged.CurrentAmmo);
		if (target.DuctTapeMergedGunIDs == null)
		{
			target.DuctTapeMergedGunIDs = new List<int>();
		}
		if (merged.DuctTapeMergedGunIDs != null)
		{
			target.DuctTapeMergedGunIDs.AddRange(merged.DuctTapeMergedGunIDs);
		}
		target.DuctTapeMergedGunIDs.Add(merged.PickupObjectId);
	}

	// Token: 0x060073C6 RID: 29638 RVA: 0x002E0F1C File Offset: 0x002DF11C
	protected override void DoActiveEffect(PlayerController user)
	{
		if (user && user.CurrentGun && this.IsGunValid(user.CurrentGun, this.m_validSourceGun))
		{
			this.m_validTargetGun = user.CurrentGun;
			if (!this.m_validSourceGun || !this.m_validTargetGun)
			{
				return;
			}
			DuctTapeItem.DuctTapeGuns(this.m_validSourceGun, this.m_validTargetGun);
			user.inventory.RemoveGunFromInventory(this.m_validSourceGun);
			UnityEngine.Object.Destroy(this.m_validSourceGun.gameObject);
			user.stats.RecalculateStats(user, false, false);
		}
		this.m_isCurrentlyActive = false;
	}

	// Token: 0x060073C7 RID: 29639 RVA: 0x002E0FD0 File Offset: 0x002DF1D0
	protected override void DoEffect(PlayerController user)
	{
		if (user.inventory.AllGuns.Count < 2)
		{
			return;
		}
		this.m_validSourceGun = null;
		this.m_validTargetGun = null;
		if (user && user.CurrentGun && this.IsGunValid(user.CurrentGun, this.m_validSourceGun))
		{
			this.m_validSourceGun = user.CurrentGun;
			this.m_isCurrentlyActive = true;
		}
	}

	// Token: 0x060073C8 RID: 29640 RVA: 0x002E1048 File Offset: 0x002DF248
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007552 RID: 30034
	private Gun m_validSourceGun;

	// Token: 0x04007553 RID: 30035
	private Gun m_validTargetGun;
}
