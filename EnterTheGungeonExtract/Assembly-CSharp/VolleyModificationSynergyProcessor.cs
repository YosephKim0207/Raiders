using System;
using UnityEngine;

// Token: 0x02001713 RID: 5907
public class VolleyModificationSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600893B RID: 35131 RVA: 0x0038ED50 File Offset: 0x0038CF50
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.m_gun)
		{
			Gun gun = this.m_gun;
			gun.PostProcessVolley = (Action<ProjectileVolleyData>)Delegate.Combine(gun.PostProcessVolley, new Action<ProjectileVolleyData>(this.HandleVolleyRebuild));
			bool flag = false;
			if (this.synergies != null)
			{
				for (int i = 0; i < this.synergies.Length; i++)
				{
					if (this.synergies[i].ReplacesSourceProjectile)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				Gun gun2 = this.m_gun;
				gun2.OnPreFireProjectileModifier = (Func<Gun, Projectile, ProjectileModule, Projectile>)Delegate.Combine(gun2.OnPreFireProjectileModifier, new Func<Gun, Projectile, ProjectileModule, Projectile>(this.HandlePreFireProjectileReplacement));
			}
		}
		else
		{
			this.m_item = base.GetComponent<PassiveItem>();
			if (this.m_item)
			{
				PassiveItem item = this.m_item;
				item.OnPickedUp = (Action<PlayerController>)Delegate.Combine(item.OnPickedUp, new Action<PlayerController>(this.LinkPassiveItem));
				PassiveItem item2 = this.m_item;
				item2.OnDisabled = (Action<PlayerController>)Delegate.Combine(item2.OnDisabled, new Action<PlayerController>(this.DelinkPassiveItem));
			}
		}
	}

	// Token: 0x0600893C RID: 35132 RVA: 0x0038EE7C File Offset: 0x0038D07C
	private Projectile HandlePreFireProjectileReplacementPlayer(Gun sourceGun, Projectile sourceProjectile)
	{
		Projectile projectile = sourceProjectile;
		PlayerController playerController = sourceGun.CurrentOwner as PlayerController;
		if (this.synergies != null)
		{
			for (int i = 0; i < this.synergies.Length; i++)
			{
				VolleyModificationSynergyData volleyModificationSynergyData = this.synergies[i];
				if (volleyModificationSynergyData.ReplacesSourceProjectile && playerController && playerController.HasActiveBonusSynergy(volleyModificationSynergyData.RequiredSynergy, false) && UnityEngine.Random.value < volleyModificationSynergyData.ReplacementChance)
				{
					if (volleyModificationSynergyData.UsesMultipleReplacementProjectiles)
					{
						if (volleyModificationSynergyData.MultipleReplacementsSequential)
						{
							projectile = volleyModificationSynergyData.MultipleReplacementProjectiles[volleyModificationSynergyData.multipleSequentialReplacementIndex];
							volleyModificationSynergyData.multipleSequentialReplacementIndex = (volleyModificationSynergyData.multipleSequentialReplacementIndex + 1) % volleyModificationSynergyData.MultipleReplacementProjectiles.Length;
						}
						else
						{
							projectile = volleyModificationSynergyData.MultipleReplacementProjectiles[UnityEngine.Random.Range(0, volleyModificationSynergyData.MultipleReplacementProjectiles.Length)];
						}
					}
					else
					{
						projectile = volleyModificationSynergyData.ReplacementProjectile;
					}
				}
			}
		}
		return projectile;
	}

	// Token: 0x0600893D RID: 35133 RVA: 0x0038EF60 File Offset: 0x0038D160
	private Projectile HandlePreFireProjectileReplacement(Gun sourceGun, Projectile sourceProjectile, ProjectileModule sourceModule)
	{
		Projectile projectile = sourceProjectile;
		PlayerController playerController = sourceGun.CurrentOwner as PlayerController;
		if (this.synergies != null)
		{
			for (int i = 0; i < this.synergies.Length; i++)
			{
				VolleyModificationSynergyData volleyModificationSynergyData = this.synergies[i];
				if (volleyModificationSynergyData.ReplacesSourceProjectile && playerController && playerController.HasActiveBonusSynergy(volleyModificationSynergyData.RequiredSynergy, false))
				{
					if (!volleyModificationSynergyData.OnlyReplacesAdditionalProjectiles || sourceModule.ignoredForReloadPurposes)
					{
						if (!sourceGun || !sourceGun.IsCharging || volleyModificationSynergyData.RequiredSynergy == CustomSynergyType.ANTIMATTER_BODY)
						{
							if (volleyModificationSynergyData.ReplacementSkipsChargedShots && sourceModule.shootStyle == ProjectileModule.ShootStyle.Charged)
							{
								bool flag = false;
								for (int j = 0; j < sourceModule.chargeProjectiles.Count; j++)
								{
									if (sourceModule.chargeProjectiles[j].Projectile == sourceProjectile && sourceModule.chargeProjectiles[j].ChargeTime > 0f)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									goto IL_190;
								}
							}
							if (UnityEngine.Random.value < volleyModificationSynergyData.ReplacementChance)
							{
								if (volleyModificationSynergyData.UsesMultipleReplacementProjectiles)
								{
									if (volleyModificationSynergyData.MultipleReplacementsSequential)
									{
										projectile = volleyModificationSynergyData.MultipleReplacementProjectiles[volleyModificationSynergyData.multipleSequentialReplacementIndex];
										volleyModificationSynergyData.multipleSequentialReplacementIndex = (volleyModificationSynergyData.multipleSequentialReplacementIndex + 1) % volleyModificationSynergyData.MultipleReplacementProjectiles.Length;
									}
									else
									{
										projectile = volleyModificationSynergyData.MultipleReplacementProjectiles[UnityEngine.Random.Range(0, volleyModificationSynergyData.MultipleReplacementProjectiles.Length)];
									}
								}
								else
								{
									projectile = volleyModificationSynergyData.ReplacementProjectile;
								}
							}
						}
					}
				}
				IL_190:;
			}
		}
		return projectile;
	}

	// Token: 0x0600893E RID: 35134 RVA: 0x0038F110 File Offset: 0x0038D310
	private void LinkPassiveItem(PlayerController p)
	{
		p.stats.AdditionalVolleyModifiers -= this.HandleVolleyRebuild;
		p.stats.AdditionalVolleyModifiers += this.HandleVolleyRebuild;
		bool flag = false;
		if (this.synergies != null)
		{
			for (int i = 0; i < this.synergies.Length; i++)
			{
				if (this.synergies[i].ReplacesSourceProjectile)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			p.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Combine(p.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileReplacementPlayer));
		}
	}

	// Token: 0x0600893F RID: 35135 RVA: 0x0038F1B4 File Offset: 0x0038D3B4
	private void DelinkPassiveItem(PlayerController p)
	{
		if (p && p.stats != null)
		{
			p.stats.AdditionalVolleyModifiers -= this.HandleVolleyRebuild;
			p.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(p.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileReplacementPlayer));
		}
	}

	// Token: 0x06008940 RID: 35136 RVA: 0x0038F218 File Offset: 0x0038D418
	private void HandleVolleyRebuild(ProjectileVolleyData targetVolley)
	{
		PlayerController playerController = null;
		if (this.m_gun)
		{
			playerController = this.m_gun.CurrentOwner as PlayerController;
		}
		else if (this.m_item)
		{
			playerController = this.m_item.Owner;
		}
		if (playerController && this.synergies != null)
		{
			for (int i = 0; i < this.synergies.Length; i++)
			{
				if (playerController.HasActiveBonusSynergy(this.synergies[i].RequiredSynergy, false))
				{
					this.ApplySynergy(targetVolley, this.synergies[i], playerController);
				}
			}
		}
	}

	// Token: 0x06008941 RID: 35137 RVA: 0x0038F2C4 File Offset: 0x0038D4C4
	private void ApplySynergy(ProjectileVolleyData volley, VolleyModificationSynergyData synergy, PlayerController owner)
	{
		if (synergy.AddsChargeProjectile)
		{
			volley.projectiles[0].chargeProjectiles.Add(synergy.ChargeProjectileToAdd);
		}
		if (synergy.AddsModules)
		{
			bool flag = true;
			if (volley != null && volley.projectiles.Count > 0 && volley.projectiles[0].projectiles != null && volley.projectiles[0].projectiles.Count > 0)
			{
				Projectile projectile = volley.projectiles[0].projectiles[0];
				if (projectile && projectile.GetComponent<ArtfulDodgerProjectileController>())
				{
					flag = false;
				}
			}
			if (flag)
			{
				for (int i = 0; i < synergy.ModulesToAdd.Length; i++)
				{
					synergy.ModulesToAdd[i].isExternalAddedModule = true;
					volley.projectiles.Add(synergy.ModulesToAdd[i]);
				}
			}
		}
		if (synergy.AddsDuplicatesOfBaseModule)
		{
			GunVolleyModificationItem.AddDuplicateOfBaseModule(volley, this.m_gun.CurrentOwner as PlayerController, synergy.DuplicatesOfBaseModule, synergy.BaseModuleDuplicateAngle, 0f);
		}
		if (synergy.SetsNumberFinalProjectiles)
		{
			bool flag2 = false;
			for (int j = 0; j < volley.projectiles.Count; j++)
			{
				if (!flag2 && synergy.AddsNewFinalProjectile)
				{
					if (!volley.projectiles[j].usesOptionalFinalProjectile)
					{
						flag2 = true;
						this.m_gun.OverrideFinaleAudio = true;
						volley.projectiles[j].usesOptionalFinalProjectile = true;
						volley.projectiles[j].numberOfFinalProjectiles = 1;
						volley.projectiles[j].finalProjectile = synergy.NewFinalProjectile;
						volley.projectiles[j].finalAmmoType = GameUIAmmoType.AmmoType.CUSTOM;
						volley.projectiles[j].finalCustomAmmoType = synergy.NewFinalProjectileAmmoType;
						if (string.IsNullOrEmpty(this.m_gun.finalShootAnimation))
						{
							this.m_gun.finalShootAnimation = this.m_gun.shootAnimation;
						}
					}
				}
				if (volley.projectiles[j].usesOptionalFinalProjectile)
				{
					volley.projectiles[j].numberOfFinalProjectiles = synergy.NumberFinalProjectiles;
				}
			}
		}
		if (synergy.SetsBurstCount)
		{
			if (synergy.MakesDefaultModuleBurst && volley.projectiles.Count > 0 && volley.projectiles[0].shootStyle != ProjectileModule.ShootStyle.Burst)
			{
				volley.projectiles[0].shootStyle = ProjectileModule.ShootStyle.Burst;
			}
			for (int k = 0; k < volley.projectiles.Count; k++)
			{
				if (volley.projectiles[k].shootStyle == ProjectileModule.ShootStyle.Burst)
				{
					int burstShotCount = volley.projectiles[k].burstShotCount;
					int num = volley.projectiles[k].GetModNumberOfShotsInClip(owner);
					if (num < 0)
					{
						num = int.MaxValue;
					}
					int num2 = Mathf.Clamp(Mathf.RoundToInt((float)burstShotCount * synergy.BurstMultiplier) + synergy.BurstShift, 1, num);
					volley.projectiles[k].burstShotCount = num2;
				}
			}
		}
		if (synergy.AddsPossibleProjectileToPrimaryModule)
		{
			volley.projectiles[0].projectiles.Add(synergy.AdditionalModuleProjectile);
		}
	}

	// Token: 0x04008F28 RID: 36648
	public VolleyModificationSynergyData[] synergies;

	// Token: 0x04008F29 RID: 36649
	private Gun m_gun;

	// Token: 0x04008F2A RID: 36650
	private PassiveItem m_item;
}
