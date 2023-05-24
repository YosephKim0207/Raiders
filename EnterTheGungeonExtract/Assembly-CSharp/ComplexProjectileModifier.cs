using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001385 RID: 4997
public class ComplexProjectileModifier : PassiveItem
{
	// Token: 0x06007146 RID: 28998 RVA: 0x002CFE50 File Offset: 0x002CE050
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeam += this.PostProcessBeam;
		player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
		if (this.AddsCriticalChance)
		{
			player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Combine(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModifier));
		}
	}

	// Token: 0x06007147 RID: 28999 RVA: 0x002CFEDC File Offset: 0x002CE0DC
	private void PostProcessBeam(BeamController obj)
	{
		if (this.AddsLinearChainExplosionOnKill && obj && obj.projectile)
		{
			Projectile projectile = obj.projectile;
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleLinearChainBeamHitEnemy));
		}
	}

	// Token: 0x06007148 RID: 29000 RVA: 0x002CFF38 File Offset: 0x002CE138
	private IEnumerator HandleChainExplosion(SpeculativeRigidbody enemySRB, Vector2 startPosition, Vector2 direction)
	{
		float perExplosionTime = this.LCEChainDuration / (float)this.LCEChainNumExplosions;
		float[] explosionTimes = new float[this.LCEChainNumExplosions];
		explosionTimes[0] = 0f;
		explosionTimes[1] = perExplosionTime;
		for (int i = 2; i < this.LCEChainNumExplosions; i++)
		{
			explosionTimes[i] = explosionTimes[i - 1] + perExplosionTime;
		}
		Vector2 lastValidPosition = startPosition;
		bool hitWall = false;
		int index = 0;
		float elapsed = 0f;
		lastValidPosition = startPosition;
		hitWall = false;
		Vector2 currentDirection = direction;
		RoomHandler currentRoom = startPosition.GetAbsoluteRoom();
		float enemyDistance = -1f;
		AIActor nearestEnemy = currentRoom.GetNearestEnemyInDirection(startPosition, currentDirection, 35f, out enemyDistance, true, (!enemySRB) ? null : enemySRB.aiActor);
		if (nearestEnemy && enemyDistance < 20f)
		{
			currentDirection = (nearestEnemy.CenterPosition - startPosition).normalized;
		}
		while (elapsed < this.LCEChainDuration)
		{
			elapsed += BraveTime.DeltaTime;
			while (index < this.LCEChainNumExplosions && elapsed >= explosionTimes[index])
			{
				Vector2 vector = startPosition + currentDirection.normalized * this.LCEChainDistance;
				Vector2 vector2 = Vector2.Lerp(startPosition, vector, ((float)index + 1f) / (float)this.LCEChainNumExplosions);
				if (!this.ValidExplosionPosition(vector2))
				{
					hitWall = true;
				}
				if (!hitWall)
				{
					lastValidPosition = vector2;
				}
				Exploder.Explode(lastValidPosition, this.LinearChainExplosionData, currentDirection, null, false, CoreDamageTypes.None, false);
				index++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007149 RID: 29001 RVA: 0x002CFF68 File Offset: 0x002CE168
	private bool ValidExplosionPosition(Vector2 pos)
	{
		IntVector2 intVector = pos.ToIntVector2(VectorConversions.Floor);
		return GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector) && GameManager.Instance.Dungeon.data[intVector].type != CellType.WALL;
	}

	// Token: 0x0600714A RID: 29002 RVA: 0x002CFFBC File Offset: 0x002CE1BC
	private Projectile HandlePreFireProjectileModifier(Gun sourceGun, Projectile sourceProjectile)
	{
		if (this.AddsCriticalChance)
		{
			float num = this.ActivationChance;
			if (this.NormalizeAcrossFireRate && sourceGun)
			{
				float num2 = 1f / sourceGun.DefaultModule.cooldownTime;
				if (sourceGun.Volley != null && sourceGun.Volley.UsesShotgunStyleVelocityRandomizer)
				{
					num2 *= (float)Mathf.Max(1, sourceGun.Volley.projectiles.Count);
				}
				num = Mathf.Clamp01(this.ActivationsPerSecond / num2);
				num = Mathf.Max(this.MinActivationChance, num);
			}
			if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.VORPAL_BLADE, false))
			{
				num *= 0.25f;
			}
			if (UnityEngine.Random.value < num)
			{
				return this.CriticalProjectile;
			}
		}
		return sourceProjectile;
	}

	// Token: 0x0600714B RID: 29003 RVA: 0x002D009C File Offset: 0x002CE29C
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		float num = this.ActivationChance;
		Gun gun = ((!this.m_player) ? null : this.m_player.CurrentGun);
		if (this.NormalizeAcrossFireRate && gun)
		{
			float num2 = 1f / gun.DefaultModule.cooldownTime;
			if (this.AddsChanceToBlank && gun.Volley != null && gun.Volley.UsesShotgunStyleVelocityRandomizer)
			{
				num2 *= (float)gun.Volley.projectiles.Count;
			}
			num = Mathf.Clamp01(this.ActivationsPerSecond / num2);
			if (this.UsesAlternateActivationChanceInBossRooms && this.m_player && this.m_player.CurrentRoom != null && this.m_player.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				num = Mathf.Clamp01(this.BossActivationsPerSecond / num2);
			}
			num = Mathf.Max(this.MinActivationChance, num);
		}
		if (this.UsesChanceForAdditionalProjectile && this.m_player && this.m_player.HasActiveBonusSynergy(CustomSynergyType.SHADOW_BACKUP, false) && Vector2.Dot(obj.transform.right, this.m_player.unadjustedAimPoint.XY() - this.m_player.CenterPosition) < -0.75f)
		{
			num = 1f;
		}
		if (UnityEngine.Random.value < num)
		{
			if (this.AddsChainLightning)
			{
				ChainLightningModifier orAddComponent = obj.gameObject.GetOrAddComponent<ChainLightningModifier>();
				orAddComponent.LinkVFXPrefab = this.ChainLightningVFX;
				orAddComponent.damageTypes = this.ChainLightningDamageTypes;
				orAddComponent.maximumLinkDistance = this.ChainLightningMaxLinkDistance;
				orAddComponent.damagePerHit = this.ChainLightningDamagePerHit;
				orAddComponent.damageCooldown = this.ChainLightningDamageCooldown;
				if (this.m_player && this.m_player.HasActiveBonusSynergy(CustomSynergyType.TESLA_UNBOUND, false))
				{
					orAddComponent.maximumLinkDistance *= 3f;
					orAddComponent.CanChainToAnyProjectile = true;
				}
				if (this.ChainLightningDispersalParticles != null)
				{
					orAddComponent.UsesDispersalParticles = true;
					orAddComponent.DispersalParticleSystemPrefab = this.ChainLightningDispersalParticles;
					orAddComponent.DispersalDensity = this.ChainLightningDispersalDensity;
					orAddComponent.DispersalMinCoherency = this.ChainLightningDispersalMinCoherence;
					orAddComponent.DispersalMaxCoherency = this.ChainLightningDispersalMaxCoherence;
				}
				else
				{
					orAddComponent.UsesDispersalParticles = false;
				}
			}
			if (this.AddsExplosino && !obj.gameObject.GetComponent<ExplosiveModifier>())
			{
				ExplosiveModifier explosiveModifier = obj.gameObject.AddComponent<ExplosiveModifier>();
				explosiveModifier.doExplosion = true;
				explosiveModifier.explosionData = this.ExplosionData;
			}
			if (this.UsesChanceForAdditionalProjectile)
			{
				base.Owner.SpawnShadowBullet(obj, true);
			}
			if (this.AddsSpawnProjectileModifier && !obj.gameObject.GetComponent<SpawnProjModifier>())
			{
				SpawnProjModifier spawnProjModifier = obj.gameObject.AddComponent<SpawnProjModifier>();
				spawnProjModifier.SpawnedProjectilesInheritAppearance = this.SpawnProjectileInheritsApperance;
				spawnProjModifier.SpawnedProjectileScaleModifier = this.SpawnProjectileScaleModifier;
				spawnProjModifier.SpawnedProjectilesInheritData = true;
				spawnProjModifier.spawnProjectilesOnCollision = true;
				spawnProjModifier.spawnProjecitlesOnDieInAir = true;
				spawnProjModifier.doOverrideObjectCollisionSpawnStyle = true;
				spawnProjModifier.startAngle = UnityEngine.Random.Range(0, 180);
				int num3 = this.NumberToSpawnOnCollision;
				if (this.ScaleSpawnsByFireRate && gun)
				{
					float num4 = 1f / gun.DefaultModule.cooldownTime;
					if (gun.Volley.projectiles.Count > 2)
					{
						int num5 = 0;
						for (int i = 0; i < gun.Volley.projectiles.Count; i++)
						{
							if (gun.Volley.projectiles[i] != null && gun.Volley.projectiles[i].mirror)
							{
								num5 += 2;
							}
							else
							{
								num5++;
							}
						}
						num4 = Mathf.Lerp(this.MinFlakFireRate, this.MaxFlakFireRate, (float)num5 / 5f);
					}
					num3 = Mathf.RoundToInt(Mathf.Lerp((float)this.MinFlakSpawns, (float)this.MaxFlakSpawns * 1f, Mathf.InverseLerp(this.MaxFlakFireRate, this.MinFlakFireRate, num4)));
				}
				if (obj.SpawnedFromOtherPlayerProjectile)
				{
					num3 = 2;
				}
				spawnProjModifier.numberToSpawnOnCollison = num3;
				spawnProjModifier.projectileToSpawnOnCollision = this.CollisionSpawnProjectile;
				spawnProjModifier.collisionSpawnStyle = SpawnProjModifier.CollisionSpawnStyle.FLAK_BURST;
			}
			else if (this.AddsSpawnProjectileModifier)
			{
				SpawnProjModifier component = obj.gameObject.GetComponent<SpawnProjModifier>();
				component.PostprocessSpawnedProjectiles = true;
			}
			if (this.AddsTrailedSpawn)
			{
				obj.StartCoroutine(this.HandleTrailedSpawn(obj));
			}
			if (this.AddsDevolverModifier && !obj.gameObject.GetComponent<DevolverModifier>())
			{
				DevolverModifier devolverModifier = obj.gameObject.AddComponent<DevolverModifier>();
				devolverModifier.chanceToDevolve = this.DevolverSourceModifier.chanceToDevolve;
				devolverModifier.DevolverHierarchy = this.DevolverSourceModifier.DevolverHierarchy;
				devolverModifier.EnemyGuidsToIgnore = this.DevolverSourceModifier.EnemyGuidsToIgnore;
			}
			if (this.AddsHungryBullets && !obj.gameObject.GetComponent<HungryProjectileModifier>())
			{
				HungryProjectileModifier hungryProjectileModifier = obj.gameObject.AddComponent<HungryProjectileModifier>();
				hungryProjectileModifier.HungryRadius = this.HungryRadius;
				hungryProjectileModifier.DamagePercentGainPerSnack = this.DamagePercentGainPerSnack;
				hungryProjectileModifier.MaxMultiplier = this.HungryMaxMultiplier;
				hungryProjectileModifier.MaximumBulletsEaten = this.MaximumBulletsEaten;
			}
			if (this.AddsLinearChainExplosionOnKill)
			{
				obj.OnWillKillEnemy = (Action<Projectile, SpeculativeRigidbody>)Delegate.Combine(obj.OnWillKillEnemy, new Action<Projectile, SpeculativeRigidbody>(this.HandleWillKillEnemy));
			}
			if (this.m_player && this.AddsChanceToBlank)
			{
				obj.OnDestruction += this.HandleBlankOnDestruction;
			}
		}
	}

	// Token: 0x0600714C RID: 29004 RVA: 0x002D0668 File Offset: 0x002CE868
	private void HandleLinearChainBeamHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody enemy, bool fatal)
	{
		if (this.AddsLinearChainExplosionOnKill && enemy && fatal)
		{
			Vector2 vector = ((!enemy.aiActor) ? enemy.transform.position.XY() : enemy.aiActor.CenterPosition);
			Debug.LogError(vector);
			Vector2 vector2 = ((!sourceProjectile) ? ((!enemy.healthHaver) ? BraveMathCollege.DegreesToVector(base.Owner.FacingDirection, 1f) : enemy.healthHaver.lastIncurredDamageDirection) : sourceProjectile.LastVelocity.normalized);
			if (sourceProjectile)
			{
				BasicBeamController component = sourceProjectile.GetComponent<BasicBeamController>();
				if (component)
				{
					vector2 = component.Direction.normalized;
				}
			}
			if (vector2.magnitude < 0.05f)
			{
				vector2 = UnityEngine.Random.insideUnitCircle.normalized;
			}
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleChainExplosion(enemy, vector, vector2.normalized));
		}
	}

	// Token: 0x0600714D RID: 29005 RVA: 0x002D0790 File Offset: 0x002CE990
	private void HandleWillKillEnemy(Projectile sourceProjectile, SpeculativeRigidbody enemy)
	{
		if (this.AddsLinearChainExplosionOnKill && enemy)
		{
			Vector2 vector = ((!enemy.aiActor) ? enemy.transform.position.XY() : enemy.aiActor.CenterPosition);
			Debug.LogError(vector);
			Vector2 vector2 = ((!sourceProjectile) ? ((!enemy.healthHaver) ? BraveMathCollege.DegreesToVector(base.Owner.FacingDirection, 1f) : enemy.healthHaver.lastIncurredDamageDirection) : sourceProjectile.LastVelocity.normalized);
			if (vector2.magnitude < 0.05f)
			{
				vector2 = UnityEngine.Random.insideUnitCircle.normalized;
			}
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleChainExplosion(enemy, vector, vector2.normalized));
		}
	}

	// Token: 0x0600714E RID: 29006 RVA: 0x002D0884 File Offset: 0x002CEA84
	private void DoTrailedSpawns(Projectile p, ref Vector2 lastSpawnedPosition, ref float lastElapsedDistance)
	{
		float num = (p.transform.position.XY() - lastSpawnedPosition).magnitude;
		if (num > this.TrailedObjectSpawnDistance)
		{
			Vector2 vector = p.transform.position.XY() - lastSpawnedPosition;
			while (num > this.TrailedObjectSpawnDistance)
			{
				num -= this.TrailedObjectSpawnDistance;
				lastSpawnedPosition += vector.normalized * this.TrailedObjectSpawnDistance;
				Vector2 vector2 = new Vector2(-0.5f, -1f) + UnityEngine.Random.insideUnitCircle * 0.25f;
				SpawnManager.SpawnVFX(this.TrailedObjectToSpawn, lastSpawnedPosition + vector2, Quaternion.identity);
				Exploder.DoRadialDamage(5f, lastSpawnedPosition + vector2, 0.5f, false, true, false, null);
			}
			lastElapsedDistance = p.GetElapsedDistance();
		}
	}

	// Token: 0x0600714F RID: 29007 RVA: 0x002D098C File Offset: 0x002CEB8C
	private IEnumerator HandleTrailedSpawn(Projectile p)
	{
		Vector2 lastSpawnedPosition = p.transform.position.XY();
		float lastElapsedDistance = p.GetElapsedDistance();
		p.OnDestruction += delegate(Projectile src)
		{
			this.DoTrailedSpawns(src, ref lastSpawnedPosition, ref lastElapsedDistance);
		};
		while (p)
		{
			this.DoTrailedSpawns(p, ref lastSpawnedPosition, ref lastElapsedDistance);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007150 RID: 29008 RVA: 0x002D09B0 File Offset: 0x002CEBB0
	private void HandleBlankOnDestruction(Projectile obj)
	{
		if (this.m_player && obj)
		{
			this.DoMicroBlank((!obj.specRigidbody) ? obj.transform.position.XY() : obj.specRigidbody.UnitCenter);
		}
	}

	// Token: 0x06007151 RID: 29009 RVA: 0x002D0A10 File Offset: 0x002CEC10
	private void DoMicroBlank(Vector2 center)
	{
		GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
		AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
		GameObject gameObject2 = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject2.AddComponent<SilencerInstance>();
		float num = 0.25f;
		silencerInstance.TriggerSilencer(center, 20f, this.BlankRadius, gameObject, 0f, 3f, 3f, 3f, 30f, 3f, num, this.m_player, true, false);
	}

	// Token: 0x06007152 RID: 29010 RVA: 0x002D0A90 File Offset: 0x002CEC90
	private void PostProcessBeamChanceTick(BeamController beamController)
	{
		if (UnityEngine.Random.value < this.ActivationChance)
		{
			beamController.ChanceBasedShadowBullet = true;
		}
	}

	// Token: 0x06007153 RID: 29011 RVA: 0x002D0AAC File Offset: 0x002CECAC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<ComplexProjectileModifier>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
		player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModifier));
		return debrisObject;
	}

	// Token: 0x06007154 RID: 29012 RVA: 0x002D0B30 File Offset: 0x002CED30
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
			this.m_player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			PlayerController player = this.m_player;
			player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModifier));
		}
	}

	// Token: 0x040070E0 RID: 28896
	public float ActivationChance = 1f;

	// Token: 0x040070E1 RID: 28897
	public bool NormalizeAcrossFireRate;

	// Token: 0x040070E2 RID: 28898
	[ShowInInspectorIf("NormalizeAcrossFireRate", false)]
	public float ActivationsPerSecond = 1f;

	// Token: 0x040070E3 RID: 28899
	[ShowInInspectorIf("NormalizeAcrossFireRate", false)]
	public float MinActivationChance = 0.05f;

	// Token: 0x040070E4 RID: 28900
	public bool UsesAlternateActivationChanceInBossRooms;

	// Token: 0x040070E5 RID: 28901
	[ShowInInspectorIf("UsesAlternateActivationChanceInBossRooms", false)]
	public float BossActivationsPerSecond = 1f;

	// Token: 0x040070E6 RID: 28902
	public bool AddsChainLightning;

	// Token: 0x040070E7 RID: 28903
	[ShowInInspectorIf("AddsChainLightning", false)]
	public GameObject ChainLightningVFX;

	// Token: 0x040070E8 RID: 28904
	[ShowInInspectorIf("AddsChainLightning", false)]
	public CoreDamageTypes ChainLightningDamageTypes;

	// Token: 0x040070E9 RID: 28905
	[ShowInInspectorIf("AddsChainLightning", false)]
	public float ChainLightningMaxLinkDistance = 15f;

	// Token: 0x040070EA RID: 28906
	[ShowInInspectorIf("AddsChainLightning", false)]
	public float ChainLightningDamagePerHit = 6f;

	// Token: 0x040070EB RID: 28907
	[ShowInInspectorIf("AddsChainLightning", false)]
	public float ChainLightningDamageCooldown = 1f;

	// Token: 0x040070EC RID: 28908
	[ShowInInspectorIf("AddsChainLightning", false)]
	public GameObject ChainLightningDispersalParticles;

	// Token: 0x040070ED RID: 28909
	[ShowInInspectorIf("AddsChainLightning", false)]
	public float ChainLightningDispersalDensity;

	// Token: 0x040070EE RID: 28910
	[ShowInInspectorIf("AddsChainLightning", false)]
	public float ChainLightningDispersalMinCoherence;

	// Token: 0x040070EF RID: 28911
	[ShowInInspectorIf("AddsChainLightning", false)]
	public float ChainLightningDispersalMaxCoherence;

	// Token: 0x040070F0 RID: 28912
	public bool AddsExplosino;

	// Token: 0x040070F1 RID: 28913
	[ShowInInspectorIf("AddsExplosino", false)]
	public ExplosionData ExplosionData;

	// Token: 0x040070F2 RID: 28914
	public bool UsesChanceForAdditionalProjectile;

	// Token: 0x040070F3 RID: 28915
	[Header("Adds Spawned Projectiles")]
	public bool AddsSpawnProjectileModifier;

	// Token: 0x040070F4 RID: 28916
	[ShowInInspectorIf("AddsSpawnProjectileModifier", false)]
	public bool SpawnProjectileInheritsApperance;

	// Token: 0x040070F5 RID: 28917
	[ShowInInspectorIf("AddsSpawnProjectileModifier", false)]
	public float SpawnProjectileScaleModifier = 1f;

	// Token: 0x040070F6 RID: 28918
	[ShowInInspectorIf("AddsSpawnProjectileModifier", false)]
	public int NumberToSpawnOnCollision = 3;

	// Token: 0x040070F7 RID: 28919
	[ShowInInspectorIf("AddsSpawnProjectileModifier", false)]
	public Projectile CollisionSpawnProjectile;

	// Token: 0x040070F8 RID: 28920
	[ShowInInspectorIf("AddsSpawnProjectileModifier", false)]
	public bool ScaleSpawnsByFireRate;

	// Token: 0x040070F9 RID: 28921
	[ShowInInspectorIf("ScaleSpawnsByFireRate", false)]
	public int MinFlakSpawns = 2;

	// Token: 0x040070FA RID: 28922
	[ShowInInspectorIf("ScaleSpawnsByFireRate", false)]
	public int MaxFlakSpawns = 8;

	// Token: 0x040070FB RID: 28923
	[ShowInInspectorIf("ScaleSpawnsByFireRate", false)]
	public float MinFlakFireRate = 0.25f;

	// Token: 0x040070FC RID: 28924
	[ShowInInspectorIf("ScaleSpawnsByFireRate", false)]
	public float MaxFlakFireRate = 2f;

	// Token: 0x040070FD RID: 28925
	[Header("Adds Chance To Blank")]
	public bool AddsChanceToBlank;

	// Token: 0x040070FE RID: 28926
	[ShowInInspectorIf("AddsChanceToBlank", false)]
	public float BlankRadius = 5f;

	// Token: 0x040070FF RID: 28927
	[Header("Adds Trailed Spawns")]
	public bool AddsTrailedSpawn;

	// Token: 0x04007100 RID: 28928
	[ShowInInspectorIf("AddsTrailedSpawn", false)]
	public GameObject TrailedObjectToSpawn;

	// Token: 0x04007101 RID: 28929
	[ShowInInspectorIf("AddsTrailedSpawn", false)]
	public float TrailedObjectSpawnDistance = 1f;

	// Token: 0x04007102 RID: 28930
	[Header("Critical")]
	public bool AddsCriticalChance;

	// Token: 0x04007103 RID: 28931
	[ShowInInspectorIf("AddsCriticalChance", false)]
	public Projectile CriticalProjectile;

	// Token: 0x04007104 RID: 28932
	[Header("Devolver")]
	[Space(20f)]
	public bool AddsDevolverModifier;

	// Token: 0x04007105 RID: 28933
	[ShowInInspectorIf("AddsDevolverModifier", false)]
	public DevolverModifier DevolverSourceModifier;

	// Token: 0x04007106 RID: 28934
	[Header("Hungry Bullets")]
	public bool AddsHungryBullets;

	// Token: 0x04007107 RID: 28935
	[ShowInInspectorIf("AddsHungryBullets", false)]
	public float HungryRadius = 1.5f;

	// Token: 0x04007108 RID: 28936
	[ShowInInspectorIf("AddsHungryBullets", false)]
	public float DamagePercentGainPerSnack = 0.25f;

	// Token: 0x04007109 RID: 28937
	[ShowInInspectorIf("AddsHungryBullets", false)]
	public float HungryMaxMultiplier = 3f;

	// Token: 0x0400710A RID: 28938
	[ShowInInspectorIf("AddsHungryBullets", false)]
	public int MaximumBulletsEaten = 10;

	// Token: 0x0400710B RID: 28939
	[Header("Katana Bullets")]
	public bool AddsLinearChainExplosionOnKill;

	// Token: 0x0400710C RID: 28940
	[ShowInInspectorIf("AddsLinearChainExplosionOnKill", false)]
	public float LCEChainDuration = 1f;

	// Token: 0x0400710D RID: 28941
	[ShowInInspectorIf("AddsLinearChainExplosionOnKill", false)]
	public float LCEChainDistance = 10f;

	// Token: 0x0400710E RID: 28942
	[ShowInInspectorIf("AddsLinearChainExplosionOnKill", false)]
	public int LCEChainNumExplosions = 5;

	// Token: 0x0400710F RID: 28943
	[ShowInInspectorIf("AddsLinearChainExplosionOnKill", false)]
	public GameObject LCEChainTargetSprite;

	// Token: 0x04007110 RID: 28944
	[ShowInInspectorIf("AddsLinearChainExplosionOnKill", false)]
	public ExplosionData LinearChainExplosionData;

	// Token: 0x04007111 RID: 28945
	private PlayerController m_player;
}
