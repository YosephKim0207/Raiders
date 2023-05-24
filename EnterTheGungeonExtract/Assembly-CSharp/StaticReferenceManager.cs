using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02001834 RID: 6196
public static class StaticReferenceManager
{
	// Token: 0x060092AE RID: 37550 RVA: 0x003DF994 File Offset: 0x003DDB94
	public static void ClearStaticPerLevelData()
	{
		StaticReferenceManager.AllForgeHammers.Clear();
		StaticReferenceManager.AllProjectileTraps.Clear();
		StaticReferenceManager.AllTriggeredTraps.Clear();
		StaticReferenceManager.AllShops.Clear();
		StaticReferenceManager.AllMajorBreakables.Clear();
		StaticReferenceManager.AllPortals.Clear();
		StaticReferenceManager.AllLocks.Clear();
		StaticReferenceManager.AllAdvancedShrineControllers.Clear();
		StaticReferenceManager.AllRatTrapdoors.Clear();
		StaticReferenceManager.AllShadowSystemDepthHavers.Clear();
		GlobalDispersalParticleManager.Clear();
		HeartDispenser.ClearPerLevelData();
		SynercacheManager.ClearPerLevelData();
		DecalObject.ClearPerLevelData();
		ExtraLifeItem.ClearPerLevelData();
		Projectile.m_cachedDungeon = null;
	}

	// Token: 0x060092AF RID: 37551 RVA: 0x003DFA24 File Offset: 0x003DDC24
	public static void ForceClearAllStaticMemory()
	{
		StaticReferenceManager.m_allProjectiles.Clear();
		StaticReferenceManager.AllCorpses.Clear();
		StaticReferenceManager.AllDebris.Clear();
		StaticReferenceManager.AllEnemies.Clear();
		StaticReferenceManager.AllNpcs.Clear();
		StaticReferenceManager.AllForgeHammers.Clear();
		StaticReferenceManager.AllProjectileTraps.Clear();
		StaticReferenceManager.AllTriggeredTraps.Clear();
		StaticReferenceManager.AllShops.Clear();
		StaticReferenceManager.AllMajorBreakables.Clear();
		StaticReferenceManager.AllMinorBreakables.Clear();
		StaticReferenceManager.AllHealthHavers.Clear();
		StaticReferenceManager.AllGoops.Clear();
		StaticReferenceManager.AllBros.Clear();
		StaticReferenceManager.AllChests.Clear();
		StaticReferenceManager.AllLocks.Clear();
		StaticReferenceManager.ActiveMineCarts.Clear();
		StaticReferenceManager.AllGrasses.Clear();
		StaticReferenceManager.AllPortals.Clear();
		StaticReferenceManager.MushroomMap.Clear();
		StaticReferenceManager.AllBulletScriptSources.Clear();
		StaticReferenceManager.AllAdvancedShrineControllers.Clear();
		StaticReferenceManager.AllRatTrapdoors.Clear();
		StaticReferenceManager.AllShadowSystemDepthHavers.Clear();
		StaticReferenceManager.WeaponChestsSpawnedOnFloor = 0;
		StaticReferenceManager.ItemChestsSpawnedOnFloor = 0;
		StaticReferenceManager.DChestsSpawnedInTotal = 0;
		StaticReferenceManager.DChestsSpawnedOnFloor = 0;
		if (SecretRoomDoorBeer.AllSecretRoomDoors != null)
		{
			SecretRoomDoorBeer.AllSecretRoomDoors.Clear();
		}
		GlobalSparksDoer.CleanupOnSceneTransition();
		BaseShopController.ClearStaticMemory();
		SilencerInstance.s_MaxRadiusLimiter = null;
		CollisionData.Pool.Clear();
		LinearCastResult.Pool.Clear();
		RaycastResult.Pool.Clear();
		TimeTubeCreditsController.ClearPerLevelData();
		HeartDispenser.ClearPerLevelData();
		Projectile.m_cachedDungeon = null;
		if (StaticReferenceManager.AllClusteredTimeInvariantBehaviours != null)
		{
			StaticReferenceManager.AllClusteredTimeInvariantBehaviours.Clear();
		}
	}

	// Token: 0x140000D2 RID: 210
	// (add) Token: 0x060092B0 RID: 37552 RVA: 0x003DFBA8 File Offset: 0x003DDDA8
	// (remove) Token: 0x060092B1 RID: 37553 RVA: 0x003DFBDC File Offset: 0x003DDDDC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action<Projectile> ProjectileAdded;

	// Token: 0x140000D3 RID: 211
	// (add) Token: 0x060092B2 RID: 37554 RVA: 0x003DFC10 File Offset: 0x003DDE10
	// (remove) Token: 0x060092B3 RID: 37555 RVA: 0x003DFC44 File Offset: 0x003DDE44
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action<Projectile> ProjectileRemoved;

	// Token: 0x170015E2 RID: 5602
	// (get) Token: 0x060092B4 RID: 37556 RVA: 0x003DFC78 File Offset: 0x003DDE78
	public static ReadOnlyCollection<Projectile> AllProjectiles
	{
		get
		{
			return StaticReferenceManager.m_readOnlyProjectiles;
		}
	}

	// Token: 0x060092B5 RID: 37557 RVA: 0x003DFC80 File Offset: 0x003DDE80
	public static void AddProjectile(Projectile p)
	{
		StaticReferenceManager.m_allProjectiles.Add(p);
		if (StaticReferenceManager.ProjectileAdded != null)
		{
			StaticReferenceManager.ProjectileAdded(p);
		}
	}

	// Token: 0x060092B6 RID: 37558 RVA: 0x003DFCA4 File Offset: 0x003DDEA4
	public static void RemoveProjectile(Projectile p)
	{
		StaticReferenceManager.m_allProjectiles.Remove(p);
		if (StaticReferenceManager.ProjectileRemoved != null)
		{
			StaticReferenceManager.ProjectileRemoved(p);
		}
	}

	// Token: 0x060092B7 RID: 37559 RVA: 0x003DFCC8 File Offset: 0x003DDEC8
	public static void DestroyAllProjectiles()
	{
		List<Projectile> list = new List<Projectile>();
		for (int i = 0; i < StaticReferenceManager.m_allProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.m_allProjectiles[i];
			if (projectile)
			{
				list.Add(projectile);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			list[j].DieInAir(false, false, true, false);
		}
	}

	// Token: 0x060092B8 RID: 37560 RVA: 0x003DFD40 File Offset: 0x003DDF40
	public static void DestroyAllEnemyProjectiles()
	{
		List<Projectile> list = new List<Projectile>();
		for (int i = 0; i < StaticReferenceManager.m_allProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.m_allProjectiles[i];
			if (projectile)
			{
				if (!(projectile.Owner is PlayerController))
				{
					if (projectile.collidesWithPlayer || projectile.Owner is AIActor)
					{
						list.Add(projectile);
					}
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			list[j].DieInAir(false, false, true, false);
		}
	}

	// Token: 0x04009A35 RID: 39477
	public static List<ClusteredTimeInvariantMonoBehaviour> AllClusteredTimeInvariantBehaviours = new List<ClusteredTimeInvariantMonoBehaviour>();

	// Token: 0x04009A36 RID: 39478
	public static List<GameObject> AllCorpses = new List<GameObject>();

	// Token: 0x04009A37 RID: 39479
	public static List<DebrisObject> AllDebris = new List<DebrisObject>();

	// Token: 0x04009A38 RID: 39480
	public static List<AIActor> AllEnemies = new List<AIActor>();

	// Token: 0x04009A39 RID: 39481
	public static List<TalkDoerLite> AllNpcs = new List<TalkDoerLite>();

	// Token: 0x04009A3A RID: 39482
	public static List<ProjectileTrapController> AllProjectileTraps = new List<ProjectileTrapController>();

	// Token: 0x04009A3B RID: 39483
	public static List<BasicTrapController> AllTriggeredTraps = new List<BasicTrapController>();

	// Token: 0x04009A3C RID: 39484
	public static List<ForgeHammerController> AllForgeHammers = new List<ForgeHammerController>();

	// Token: 0x04009A3D RID: 39485
	public static List<BaseShopController> AllShops = new List<BaseShopController>();

	// Token: 0x04009A3E RID: 39486
	public static List<MajorBreakable> AllMajorBreakables = new List<MajorBreakable>();

	// Token: 0x04009A3F RID: 39487
	public static List<MinorBreakable> AllMinorBreakables = new List<MinorBreakable>();

	// Token: 0x04009A40 RID: 39488
	public static List<HealthHaver> AllHealthHavers = new List<HealthHaver>();

	// Token: 0x04009A41 RID: 39489
	public static List<DeadlyDeadlyGoopManager> AllGoops = new List<DeadlyDeadlyGoopManager>();

	// Token: 0x04009A42 RID: 39490
	public static List<BroController> AllBros = new List<BroController>();

	// Token: 0x04009A43 RID: 39491
	public static List<Chest> AllChests = new List<Chest>();

	// Token: 0x04009A44 RID: 39492
	public static List<InteractableLock> AllLocks = new List<InteractableLock>();

	// Token: 0x04009A45 RID: 39493
	public static List<BulletScriptSource> AllBulletScriptSources = new List<BulletScriptSource>();

	// Token: 0x04009A46 RID: 39494
	public static int WeaponChestsSpawnedOnFloor = 0;

	// Token: 0x04009A47 RID: 39495
	public static int ItemChestsSpawnedOnFloor = 0;

	// Token: 0x04009A48 RID: 39496
	public static int DChestsSpawnedOnFloor = 0;

	// Token: 0x04009A49 RID: 39497
	public static int DChestsSpawnedInTotal = 0;

	// Token: 0x04009A4A RID: 39498
	public static List<PortalGunPortalController> AllPortals = new List<PortalGunPortalController>();

	// Token: 0x04009A4B RID: 39499
	public static List<TallGrassPatch> AllGrasses = new List<TallGrassPatch>();

	// Token: 0x04009A4C RID: 39500
	public static List<AdvancedShrineController> AllAdvancedShrineControllers = new List<AdvancedShrineController>();

	// Token: 0x04009A4D RID: 39501
	public static List<ResourcefulRatMinesHiddenTrapdoor> AllRatTrapdoors = new List<ResourcefulRatMinesHiddenTrapdoor>();

	// Token: 0x04009A4E RID: 39502
	public static List<Transform> AllShadowSystemDepthHavers = new List<Transform>();

	// Token: 0x04009A4F RID: 39503
	public static Dictionary<PlayerController, MineCartController> ActiveMineCarts = new Dictionary<PlayerController, MineCartController>();

	// Token: 0x04009A50 RID: 39504
	public static Dictionary<IntVector2, ElectricMushroom> MushroomMap = new Dictionary<IntVector2, ElectricMushroom>(new IntVector2EqualityComparer());

	// Token: 0x04009A53 RID: 39507
	private static List<Projectile> m_allProjectiles = new List<Projectile>();

	// Token: 0x04009A54 RID: 39508
	private static ReadOnlyCollection<Projectile> m_readOnlyProjectiles = StaticReferenceManager.m_allProjectiles.AsReadOnly();
}
