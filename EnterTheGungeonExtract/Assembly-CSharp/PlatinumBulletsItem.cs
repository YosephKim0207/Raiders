using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013D5 RID: 5077
public class PlatinumBulletsItem : PassiveItem, ILevelLoadedListener
{
	// Token: 0x06007331 RID: 29489 RVA: 0x002DD4A4 File Offset: 0x002DB6A4
	public override void Pickup(PlayerController player)
	{
		if (!this.m_pickedUpThisRun)
		{
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			switch (tilesetId)
			{
			case GlobalDungeonData.ValidTilesets.GUNGEON:
				this.m_totalBulletsFiredNormalizedByFireRate = this.GungeonStartingValue;
				break;
			case GlobalDungeonData.ValidTilesets.CASTLEGEON:
				this.m_totalBulletsFiredNormalizedByFireRate = this.CastleStartingValue;
				break;
			default:
				if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
						{
							if (tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
							{
								if (tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
								{
									this.m_totalBulletsFiredNormalizedByFireRate = this.RatStartingValue;
								}
							}
							else
							{
								this.m_totalBulletsFiredNormalizedByFireRate = this.HellStartingValue;
							}
						}
						else
						{
							this.m_totalBulletsFiredNormalizedByFireRate = this.ForgeStartingValue;
						}
					}
					else
					{
						this.m_totalBulletsFiredNormalizedByFireRate = this.HollowStartingValue;
					}
				}
				else
				{
					this.m_totalBulletsFiredNormalizedByFireRate = this.MinesStartingValue;
				}
				break;
			case GlobalDungeonData.ValidTilesets.SEWERGEON:
				this.m_totalBulletsFiredNormalizedByFireRate = this.SewersStartingValue;
				break;
			case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
				this.m_totalBulletsFiredNormalizedByFireRate = this.AbbeyStartingValue;
				break;
			}
		}
		base.Pickup(player);
		player.PostProcessProjectile += this.HandlePostProcessProjectile;
		player.PostProcessBeam += this.HandlePostProcessBeam;
		player.PostProcessBeamTick += this.HandlePostProcessBeamTick;
		player.OnKilledEnemyContext += this.HandleEnemyKilled;
		player.GunChanged += this.HandleGunChanged;
		this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
		if (player.CurrentGun)
		{
			this.ProcessGunShader(player.CurrentGun);
		}
	}

	// Token: 0x06007332 RID: 29490 RVA: 0x002DD654 File Offset: 0x002DB854
	private void HandleGunChanged(Gun oldGun, Gun newGun, bool arg3)
	{
		this.RemoveGunShader(oldGun);
		this.ProcessGunShader(newGun);
	}

	// Token: 0x06007333 RID: 29491 RVA: 0x002DD664 File Offset: 0x002DB864
	private void RemoveGunShader(Gun g)
	{
		if (!g)
		{
			return;
		}
		MeshRenderer component = g.GetComponent<MeshRenderer>();
		if (!component)
		{
			return;
		}
		Material[] sharedMaterials = component.sharedMaterials;
		List<Material> list = new List<Material>();
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			if (sharedMaterials[i].shader != this.m_glintShader)
			{
				list.Add(sharedMaterials[i]);
			}
		}
		component.sharedMaterials = list.ToArray();
	}

	// Token: 0x06007334 RID: 29492 RVA: 0x002DD6E0 File Offset: 0x002DB8E0
	private void ProcessGunShader(Gun g)
	{
		MeshRenderer component = g.GetComponent<MeshRenderer>();
		if (!component)
		{
			return;
		}
		Material[] sharedMaterials = component.sharedMaterials;
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			if (sharedMaterials[i].shader == this.m_glintShader)
			{
				return;
			}
		}
		Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
		Material material = new Material(this.m_glintShader);
		material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
		sharedMaterials[sharedMaterials.Length - 1] = material;
		component.sharedMaterials = sharedMaterials;
	}

	// Token: 0x06007335 RID: 29493 RVA: 0x002DD774 File Offset: 0x002DB974
	private void HandleEnemyKilled(PlayerController sourcePlayer, HealthHaver enemy)
	{
		if (sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.PLATINUM_AND_GOLD, false) && enemy && enemy.aiActor)
		{
			LootEngine.SpawnCurrency(enemy.aiActor.CenterPosition, UnityEngine.Random.Range(1, 6), false);
		}
	}

	// Token: 0x06007336 RID: 29494 RVA: 0x002DD7C8 File Offset: 0x002DB9C8
	public void BraveOnLevelWasLoaded()
	{
		this.m_lastProjectileTimeslice = -1f;
	}

	// Token: 0x06007337 RID: 29495 RVA: 0x002DD7D8 File Offset: 0x002DB9D8
	private void UpdateContributions()
	{
		if (base.Owner)
		{
			if (this.DamageStat == null)
			{
				this.DamageStat = new StatModifier();
				this.DamageStat.amount = 1f;
				this.DamageStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
				this.DamageStat.statToBoost = PlayerStats.StatType.Damage;
				base.Owner.ownerlessStatModifiers.Add(this.DamageStat);
			}
			if (this.RateOfFireStat == null)
			{
				this.RateOfFireStat = new StatModifier();
				this.RateOfFireStat.amount = 1f;
				this.RateOfFireStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
				this.RateOfFireStat.statToBoost = PlayerStats.StatType.RateOfFire;
				base.Owner.ownerlessStatModifiers.Add(this.RateOfFireStat);
			}
			this.DamageStat.amount = Mathf.Min(this.MaximumDamageMultiplier, 1f + this.m_totalBulletsFiredNormalizedByFireRate / this.ShootSecondsPerDamageDouble);
			this.RateOfFireStat.amount = Mathf.Min(this.MaximumRateOfFireMultiplier, 1f + this.m_totalBulletsFiredNormalizedByFireRate / this.ShootSecondsPerRateOfFireDouble);
		}
	}

	// Token: 0x06007338 RID: 29496 RVA: 0x002DD8F0 File Offset: 0x002DBAF0
	private void HandlePostProcessProjectile(Projectile targetProjectile, float effectChanceScalar)
	{
		targetProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(targetProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
	}

	// Token: 0x06007339 RID: 29497 RVA: 0x002DD914 File Offset: 0x002DBB14
	private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
	{
		if (sourceProjectile.PossibleSourceGun && sourceProjectile.PlayerProjectileSourceGameTimeslice > this.m_lastProjectileTimeslice)
		{
			this.m_lastProjectileTimeslice = sourceProjectile.PlayerProjectileSourceGameTimeslice;
			float num = 1f / sourceProjectile.PossibleSourceGun.DefaultModule.cooldownTime;
			this.m_totalBulletsFiredNormalizedByFireRate += ((num <= 0f) ? 1f : (1f / num));
			this.UpdateContributions();
		}
	}

	// Token: 0x0600733A RID: 29498 RVA: 0x002DD994 File Offset: 0x002DBB94
	private void HandlePostProcessBeam(BeamController targetBeam)
	{
		this.UpdateContributions();
	}

	// Token: 0x0600733B RID: 29499 RVA: 0x002DD99C File Offset: 0x002DBB9C
	private void HandlePostProcessBeamTick(BeamController arg1, SpeculativeRigidbody arg2, float arg3)
	{
		this.m_totalBulletsFiredNormalizedByFireRate += BraveTime.DeltaTime;
	}

	// Token: 0x0600733C RID: 29500 RVA: 0x002DD9B0 File Offset: 0x002DBBB0
	public override DebrisObject Drop(PlayerController player)
	{
		if (player)
		{
			if (player.CurrentGun)
			{
				this.RemoveGunShader(player.CurrentGun);
			}
			player.PostProcessProjectile -= this.HandlePostProcessProjectile;
			player.PostProcessBeam -= this.HandlePostProcessBeam;
			player.PostProcessBeamTick -= this.HandlePostProcessBeamTick;
			player.GunChanged -= this.HandleGunChanged;
			player.ownerlessStatModifiers.Remove(this.DamageStat);
			player.ownerlessStatModifiers.Remove(this.RateOfFireStat);
			this.DamageStat = null;
			this.RateOfFireStat = null;
		}
		return base.Drop(player);
	}

	// Token: 0x0600733D RID: 29501 RVA: 0x002DDA68 File Offset: 0x002DBC68
	protected override void OnDestroy()
	{
		if (base.Owner)
		{
			if (base.Owner.CurrentGun)
			{
				this.RemoveGunShader(base.Owner.CurrentGun);
			}
			base.Owner.PostProcessProjectile -= this.HandlePostProcessProjectile;
			base.Owner.PostProcessBeam -= this.HandlePostProcessBeam;
			base.Owner.PostProcessBeamTick -= this.HandlePostProcessBeamTick;
			base.Owner.GunChanged -= this.HandleGunChanged;
			base.Owner.ownerlessStatModifiers.Remove(this.DamageStat);
			base.Owner.ownerlessStatModifiers.Remove(this.RateOfFireStat);
			this.DamageStat = null;
			this.RateOfFireStat = null;
		}
		base.OnDestroy();
	}

	// Token: 0x040074B0 RID: 29872
	public float ShootSecondsPerDamageDouble = 500f;

	// Token: 0x040074B1 RID: 29873
	public float ShootSecondsPerRateOfFireDouble = 250f;

	// Token: 0x040074B2 RID: 29874
	public float MaximumDamageMultiplier = 3f;

	// Token: 0x040074B3 RID: 29875
	public float MaximumRateOfFireMultiplier = 3f;

	// Token: 0x040074B4 RID: 29876
	[Header("Per-Floor Starting Values")]
	public float CastleStartingValue;

	// Token: 0x040074B5 RID: 29877
	public float SewersStartingValue;

	// Token: 0x040074B6 RID: 29878
	public float GungeonStartingValue;

	// Token: 0x040074B7 RID: 29879
	public float AbbeyStartingValue;

	// Token: 0x040074B8 RID: 29880
	public float MinesStartingValue;

	// Token: 0x040074B9 RID: 29881
	public float RatStartingValue;

	// Token: 0x040074BA RID: 29882
	public float HollowStartingValue;

	// Token: 0x040074BB RID: 29883
	public float ForgeStartingValue;

	// Token: 0x040074BC RID: 29884
	public float HellStartingValue;

	// Token: 0x040074BD RID: 29885
	private StatModifier DamageStat;

	// Token: 0x040074BE RID: 29886
	private StatModifier RateOfFireStat;

	// Token: 0x040074BF RID: 29887
	private float m_totalBulletsFiredNormalizedByFireRate;

	// Token: 0x040074C0 RID: 29888
	private float m_lastProjectileTimeslice = -1f;

	// Token: 0x040074C1 RID: 29889
	private Shader m_glintShader;
}
