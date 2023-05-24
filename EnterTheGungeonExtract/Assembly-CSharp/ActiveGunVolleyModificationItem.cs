using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001332 RID: 4914
public class ActiveGunVolleyModificationItem : PlayerItem
{
	// Token: 0x06006F64 RID: 28516 RVA: 0x002C25FC File Offset: 0x002C07FC
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		this.m_cachedUser = user;
		base.StartCoroutine(this.HandleDuration(user));
	}

	// Token: 0x06006F65 RID: 28517 RVA: 0x002C2624 File Offset: 0x002C0824
	private IEnumerator HandleDuration(PlayerController user)
	{
		bool wasFiring = false;
		if (user.CurrentGun != null && user.CurrentGun.IsFiring)
		{
			user.CurrentGun.CeaseAttack(true, null);
			wasFiring = true;
		}
		base.IsCurrentlyActive = true;
		user.stats.RecalculateStats(user, false, false);
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.duration;
		if (wasFiring)
		{
			user.CurrentGun.Attack(null, null);
			for (int i = 0; i < user.CurrentGun.ActiveBeams.Count; i++)
			{
				if (user.CurrentGun.ActiveBeams[i] != null && user.CurrentGun.ActiveBeams[i].beam is BasicBeamController)
				{
					(user.CurrentGun.ActiveBeams[i].beam as BasicBeamController).ForceChargeTimer(10f);
				}
			}
		}
		while (this.m_activeElapsed < this.m_activeDuration && base.IsCurrentlyActive)
		{
			yield return null;
		}
		bool wasEndFiring = user.CurrentGun != null && user.CurrentGun.IsFiring;
		if (wasEndFiring)
		{
			user.CurrentGun.CeaseAttack(true, null);
		}
		base.IsCurrentlyActive = false;
		user.stats.RecalculateStats(user, false, false);
		if (wasEndFiring)
		{
			user.CurrentGun.Attack(null, null);
			for (int j = 0; j < user.CurrentGun.ActiveBeams.Count; j++)
			{
				if (user.CurrentGun.ActiveBeams[j] != null && user.CurrentGun.ActiveBeams[j].beam is BasicBeamController)
				{
					(user.CurrentGun.ActiveBeams[j].beam as BasicBeamController).ForceChargeTimer(10f);
				}
			}
		}
		yield break;
	}

	// Token: 0x06006F66 RID: 28518 RVA: 0x002C2648 File Offset: 0x002C0848
	public void ModifyVolley(ProjectileVolleyData volleyToModify)
	{
		if (this.AddsModule)
		{
			this.ModuleToAdd.isExternalAddedModule = true;
			volleyToModify.projectiles.Add(this.ModuleToAdd);
		}
		int num = this.DuplicatesOfBaseModule;
		for (int i = 0; i < this.SynergiesToIncrementDuplicates.Length; i++)
		{
			if (this.LastOwner && this.LastOwner.HasActiveBonusSynergy(this.SynergiesToIncrementDuplicates[i], false))
			{
				num++;
			}
		}
		if (num > 0)
		{
			int count = volleyToModify.projectiles.Count;
			for (int j = 0; j < count; j++)
			{
				ProjectileModule projectileModule = volleyToModify.projectiles[j];
				float num2 = (float)num * this.DuplicateAngleOffset * -1f / 2f;
				for (int k = 0; k < num; k++)
				{
					int num3 = j;
					if (projectileModule.CloneSourceIndex >= 0)
					{
						num3 = projectileModule.CloneSourceIndex;
					}
					ProjectileModule projectileModule2 = ProjectileModule.CreateClone(projectileModule, false, num3);
					float num4 = num2 + this.DuplicateAngleOffset * (float)k;
					projectileModule2.angleFromAim = num4;
					projectileModule2.ignoredForReloadPurposes = true;
					projectileModule2.ammoCost = 0;
					volleyToModify.projectiles.Add(projectileModule2);
				}
			}
		}
	}

	// Token: 0x06006F67 RID: 28519 RVA: 0x002C2784 File Offset: 0x002C0984
	public override void OnItemSwitched(PlayerController user)
	{
		base.OnItemSwitched(user);
		base.IsCurrentlyActive = false;
		if (this.m_cachedUser != null)
		{
			this.m_cachedUser.stats.RecalculateStats(this.m_cachedUser, false, false);
		}
	}

	// Token: 0x06006F68 RID: 28520 RVA: 0x002C27C0 File Offset: 0x002C09C0
	protected override void OnDestroy()
	{
		base.IsCurrentlyActive = false;
		if (this.m_cachedUser)
		{
			this.m_cachedUser.stats.RecalculateStats(this.m_cachedUser, false, false);
		}
		base.OnDestroy();
	}

	// Token: 0x04006EA4 RID: 28324
	public float duration = 5f;

	// Token: 0x04006EA5 RID: 28325
	[Header("Gun Mod Settings")]
	public bool AddsModule;

	// Token: 0x04006EA6 RID: 28326
	[ShowInInspectorIf("AddsModule", false)]
	public ProjectileModule ModuleToAdd;

	// Token: 0x04006EA7 RID: 28327
	public int DuplicatesOfBaseModule;

	// Token: 0x04006EA8 RID: 28328
	public float DuplicateAngleOffset = 10f;

	// Token: 0x04006EA9 RID: 28329
	[LongNumericEnum]
	public CustomSynergyType[] SynergiesToIncrementDuplicates;

	// Token: 0x04006EAA RID: 28330
	private PlayerController m_cachedUser;
}
