using System;
using UnityEngine;

// Token: 0x02001417 RID: 5143
public class GunVolleyModificationItem : PassiveItem
{
	// Token: 0x060074B5 RID: 29877 RVA: 0x002E7478 File Offset: 0x002E5678
	public void ModifyVolley(ProjectileVolleyData volleyToModify)
	{
		if (this.AddsModule)
		{
			bool flag = true;
			if (volleyToModify != null && volleyToModify.projectiles.Count > 0 && volleyToModify.projectiles[0].projectiles != null && volleyToModify.projectiles[0].projectiles.Count > 0)
			{
				Projectile projectile = volleyToModify.projectiles[0].projectiles[0];
				if (projectile && projectile.GetComponent<ArtfulDodgerProjectileController>())
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.ModuleToAdd.isExternalAddedModule = true;
				volleyToModify.projectiles.Add(this.ModuleToAdd);
			}
		}
		if (this.DuplicatesOfEachModule > 0)
		{
			int count = volleyToModify.projectiles.Count;
			for (int i = 0; i < count; i++)
			{
				ProjectileModule projectileModule = volleyToModify.projectiles[i];
				for (int j = 0; j < this.DuplicatesOfEachModule; j++)
				{
					int num = i;
					if (projectileModule.CloneSourceIndex >= 0)
					{
						num = projectileModule.CloneSourceIndex;
					}
					ProjectileModule projectileModule2 = ProjectileModule.CreateClone(projectileModule, false, num);
					if (projectileModule2.projectiles != null && projectileModule2.projectiles.Count > 0 && projectileModule2.projectiles[0] is InputGuidedProjectile)
					{
						projectileModule2.positionOffset = UnityEngine.Random.insideUnitCircle.normalized;
					}
					projectileModule2.angleVariance = Mathf.Max(projectileModule2.angleVariance * 2f, this.EachSingleModuleMinAngleVariance);
					projectileModule2.ignoredForReloadPurposes = true;
					projectileModule2.angleFromAim = projectileModule.angleFromAim + this.EachModuleOffsetAngle;
					projectileModule2.ammoCost = 0;
					volleyToModify.projectiles.Add(projectileModule2);
				}
				projectileModule.angleVariance = Mathf.Max(projectileModule.angleVariance, 5f);
			}
			if (!volleyToModify.UsesShotgunStyleVelocityRandomizer)
			{
				volleyToModify.UsesShotgunStyleVelocityRandomizer = true;
				volleyToModify.DecreaseFinalSpeedPercentMin = -10f;
				volleyToModify.IncreaseFinalSpeedPercentMax = 10f;
			}
		}
		if (this.DuplicatesOfBaseModule > 0)
		{
			GunVolleyModificationItem.AddDuplicateOfBaseModule(volleyToModify, base.Owner, this.DuplicatesOfBaseModule, this.DuplicateAngleOffset, this.DuplicateAngleBaseOffset);
		}
	}

	// Token: 0x060074B6 RID: 29878 RVA: 0x002E76BC File Offset: 0x002E58BC
	public static void AddDuplicateOfBaseModule(ProjectileVolleyData volleyToModify, PlayerController Owner, int DuplicatesOfBaseModule, float DuplicateAngleOffset, float DuplicateAngleBaseOffset)
	{
		ProjectileModule projectileModule = volleyToModify.projectiles[0];
		int num = 0;
		if (volleyToModify.ModulesAreTiers && Owner && Owner.CurrentGun && Owner.CurrentGun.CurrentStrengthTier >= 0 && Owner.CurrentGun.CurrentStrengthTier < volleyToModify.projectiles.Count)
		{
			projectileModule = volleyToModify.projectiles[Owner.CurrentGun.CurrentStrengthTier];
			num = Owner.CurrentGun.CurrentStrengthTier;
		}
		if (!projectileModule.mirror)
		{
			float num2 = (float)DuplicatesOfBaseModule * DuplicateAngleOffset * -1f / 2f;
			num2 += DuplicateAngleBaseOffset;
			projectileModule.angleFromAim = num2;
			for (int i = 0; i < DuplicatesOfBaseModule; i++)
			{
				int num3 = num;
				if (projectileModule.CloneSourceIndex >= 0)
				{
					num3 = projectileModule.CloneSourceIndex;
				}
				ProjectileModule projectileModule2 = ProjectileModule.CreateClone(projectileModule, false, num3);
				float num4 = num2 + DuplicateAngleOffset * (float)(i + 1);
				projectileModule2.angleFromAim = num4;
				projectileModule2.ignoredForReloadPurposes = true;
				projectileModule2.ammoCost = 0;
				volleyToModify.projectiles.Add(projectileModule2);
			}
		}
	}

	// Token: 0x060074B7 RID: 29879 RVA: 0x002E77DC File Offset: 0x002E59DC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		if (this.AddsHelixModifier)
		{
			player.PostProcessProjectile += this.PostProcessProjectileHelix;
			player.PostProcessBeam += this.PostProcessProjectileHelixBeam;
		}
		if (this.AddsOrbitModifier)
		{
			player.PostProcessProjectile += this.PostProcessProjectileOrbit;
			player.PostProcessBeam += this.PostProcessProjectileOrbitBeam;
		}
	}

	// Token: 0x060074B8 RID: 29880 RVA: 0x002E785C File Offset: 0x002E5A5C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<GunVolleyModificationItem>().m_pickedUpThisRun = true;
		if (this.AddsHelixModifier)
		{
			player.PostProcessProjectile -= this.PostProcessProjectileHelix;
			player.PostProcessBeam -= this.PostProcessProjectileHelixBeam;
		}
		if (this.AddsOrbitModifier)
		{
			player.PostProcessProjectile -= this.PostProcessProjectileOrbit;
			player.PostProcessBeam -= this.PostProcessProjectileOrbitBeam;
		}
		return debrisObject;
	}

	// Token: 0x060074B9 RID: 29881 RVA: 0x002E78DC File Offset: 0x002E5ADC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_owner)
		{
			if (this.AddsHelixModifier)
			{
				this.m_owner.PostProcessProjectile -= this.PostProcessProjectileHelix;
				this.m_owner.PostProcessBeam -= this.PostProcessProjectileHelixBeam;
			}
			if (this.AddsOrbitModifier)
			{
				this.m_owner.PostProcessProjectile -= this.PostProcessProjectileOrbit;
				this.m_owner.PostProcessBeam -= this.PostProcessProjectileOrbitBeam;
			}
		}
	}

	// Token: 0x060074BA RID: 29882 RVA: 0x002E7974 File Offset: 0x002E5B74
	private void PostProcessProjectileHelix(Projectile obj, float effectChanceScalar)
	{
		if (obj is InstantDamageOneEnemyProjectile)
		{
			return;
		}
		if (obj is InstantlyDamageAllProjectile)
		{
			return;
		}
		if (obj is HelixProjectile)
		{
			if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.DOUBLE_DOUBLE_HELIX, false))
			{
				HelixProjectile helixProjectile = obj as HelixProjectile;
				helixProjectile.helixAmplitude *= 0.5f;
				helixProjectile.helixWavelength *= 0.75f;
			}
		}
		else
		{
			if (obj.OverrideMotionModule != null && obj.OverrideMotionModule is OrbitProjectileMotionModule)
			{
				OrbitProjectileMotionModule orbitProjectileMotionModule = obj.OverrideMotionModule as OrbitProjectileMotionModule;
				orbitProjectileMotionModule.StackHelix = true;
				orbitProjectileMotionModule.ForceInvert = !this.UpOrDown;
			}
			else if (this.UpOrDown)
			{
				obj.OverrideMotionModule = new HelixProjectileMotionModule();
			}
			else
			{
				obj.OverrideMotionModule = new HelixProjectileMotionModule
				{
					ForceInvert = true
				};
			}
			this.UpOrDown = !this.UpOrDown;
		}
	}

	// Token: 0x060074BB RID: 29883 RVA: 0x002E7A7C File Offset: 0x002E5C7C
	private void PostProcessProjectileHelixBeam(BeamController beam)
	{
		if (beam.Owner is AIActor)
		{
			return;
		}
		if (beam.projectile.OverrideMotionModule != null && beam.projectile.OverrideMotionModule is OrbitProjectileMotionModule)
		{
			OrbitProjectileMotionModule orbitProjectileMotionModule = beam.projectile.OverrideMotionModule as OrbitProjectileMotionModule;
			orbitProjectileMotionModule.StackHelix = true;
			orbitProjectileMotionModule.ForceInvert = !this.UpOrDown;
		}
		else if (this.UpOrDown)
		{
			beam.projectile.OverrideMotionModule = new HelixProjectileMotionModule();
		}
		else
		{
			HelixProjectileMotionModule helixProjectileMotionModule = new HelixProjectileMotionModule();
			helixProjectileMotionModule.ForceInvert = true;
			beam.projectile.OverrideMotionModule = helixProjectileMotionModule;
		}
		this.UpOrDown = !this.UpOrDown;
	}

	// Token: 0x060074BC RID: 29884 RVA: 0x002E7B34 File Offset: 0x002E5D34
	private void PostProcessProjectileOrbit(Projectile obj, float effectChanceScalar)
	{
		if (obj is InstantDamageOneEnemyProjectile)
		{
			return;
		}
		if (obj is InstantlyDamageAllProjectile)
		{
			return;
		}
		if (obj.GetComponent<ArtfulDodgerProjectileController>())
		{
			return;
		}
		BounceProjModifier orAddComponent = obj.gameObject.GetOrAddComponent<BounceProjModifier>();
		orAddComponent.numberOfBounces = Mathf.Max(orAddComponent.numberOfBounces, 1);
		orAddComponent.onlyBounceOffTiles = true;
		BounceProjModifier bounceProjModifier = orAddComponent;
		bounceProjModifier.OnBounceContext = (Action<BounceProjModifier, SpeculativeRigidbody>)Delegate.Combine(bounceProjModifier.OnBounceContext, new Action<BounceProjModifier, SpeculativeRigidbody>(this.HandleStartOrbit));
	}

	// Token: 0x060074BD RID: 29885 RVA: 0x002E7BB4 File Offset: 0x002E5DB4
	private void PostProcessProjectileOrbitBeam(BeamController beam)
	{
		if (beam.Owner is AIActor)
		{
			return;
		}
		float num = 2.75f + (float)OrbitProjectileMotionModule.ActiveBeams;
		if (beam.projectile.baseData.range > 0f)
		{
			beam.projectile.baseData.range = beam.projectile.baseData.range + 6.2831855f * num;
		}
		if (beam.projectile.baseData.speed > 0f)
		{
			beam.projectile.baseData.speed = Mathf.Max(beam.projectile.baseData.speed, 75f);
		}
		if (beam is BasicBeamController)
		{
			(beam as BasicBeamController).PenetratesCover = true;
			(beam as BasicBeamController).penetration += 10;
		}
		OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
		orbitProjectileMotionModule.BeamOrbitRadius = num;
		orbitProjectileMotionModule.RegisterAsBeam(beam);
		if (beam.projectile.OverrideMotionModule != null && beam.projectile.OverrideMotionModule is HelixProjectileMotionModule)
		{
			orbitProjectileMotionModule.StackHelix = true;
			orbitProjectileMotionModule.ForceInvert = (beam.projectile.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
		}
		beam.projectile.OverrideMotionModule = orbitProjectileMotionModule;
	}

	// Token: 0x060074BE RID: 29886 RVA: 0x002E7CF8 File Offset: 0x002E5EF8
	private void HandleStartOrbit(BounceProjModifier bouncer, SpeculativeRigidbody srb)
	{
		int orbitersInGroup = OrbitProjectileMotionModule.GetOrbitersInGroup(-1);
		if (orbitersInGroup >= 20)
		{
			return;
		}
		bouncer.projectile.specRigidbody.CollideWithTileMap = false;
		bouncer.projectile.ResetDistance();
		bouncer.projectile.baseData.range = Mathf.Max(bouncer.projectile.baseData.range, 500f);
		if (bouncer.projectile.baseData.speed > 50f)
		{
			bouncer.projectile.baseData.speed = 20f;
			bouncer.projectile.UpdateSpeed();
		}
		OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
		orbitProjectileMotionModule.lifespan = 15f;
		if (bouncer.projectile.OverrideMotionModule != null && bouncer.projectile.OverrideMotionModule is HelixProjectileMotionModule)
		{
			orbitProjectileMotionModule.StackHelix = true;
			orbitProjectileMotionModule.ForceInvert = (bouncer.projectile.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
		}
		bouncer.projectile.OverrideMotionModule = orbitProjectileMotionModule;
	}

	// Token: 0x04007689 RID: 30345
	public bool AddsModule;

	// Token: 0x0400768A RID: 30346
	[ShowInInspectorIf("AddsModule", false)]
	public ProjectileModule ModuleToAdd;

	// Token: 0x0400768B RID: 30347
	public int DuplicatesOfBaseModule;

	// Token: 0x0400768C RID: 30348
	public float DuplicateAngleOffset = 10f;

	// Token: 0x0400768D RID: 30349
	public float DuplicateAngleBaseOffset;

	// Token: 0x0400768E RID: 30350
	public int DuplicatesOfEachModule;

	// Token: 0x0400768F RID: 30351
	public float EachModuleOffsetAngle;

	// Token: 0x04007690 RID: 30352
	public float EachSingleModuleMinAngleVariance = 15f;

	// Token: 0x04007691 RID: 30353
	[Header("For Helix Bullets")]
	public bool AddsHelixModifier;

	// Token: 0x04007692 RID: 30354
	[Header("For Orbit Bullets")]
	public bool AddsOrbitModifier;

	// Token: 0x04007693 RID: 30355
	private bool UpOrDown;

	// Token: 0x04007694 RID: 30356
	private const int MAX_ORBIT_PROJECTILES = 20;

	// Token: 0x04007695 RID: 30357
	private const float ORBIT_LIFESPAN = 15f;
}
