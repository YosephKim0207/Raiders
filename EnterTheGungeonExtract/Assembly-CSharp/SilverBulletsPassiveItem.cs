using System;
using UnityEngine;

// Token: 0x020014AA RID: 5290
public class SilverBulletsPassiveItem : PassiveItem
{
	// Token: 0x0600784E RID: 30798 RVA: 0x00301A84 File Offset: 0x002FFC84
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
		player.OnKilledEnemyContext += this.HandleKilledEnemy;
	}

	// Token: 0x0600784F RID: 30799 RVA: 0x00301AE4 File Offset: 0x002FFCE4
	private void HandleKilledEnemy(PlayerController sourcePlayer, HealthHaver killedEnemy)
	{
		if (sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.BLESSED_CURSED_BULLETS, false) && killedEnemy && killedEnemy.aiActor && killedEnemy.aiActor.IsBlackPhantom)
		{
			if (this.m_synergyStat == null)
			{
				this.m_synergyStat = StatModifier.Create(PlayerStats.StatType.Damage, StatModifier.ModifyMethod.MULTIPLICATIVE, 1f);
				sourcePlayer.ownerlessStatModifiers.Add(this.m_synergyStat);
			}
			this.m_synergyStat.amount = this.m_synergyStat.amount + 0.0025f;
			sourcePlayer.PlayEffectOnActor(this.SynergyPowerVFX, new Vector3(0f, -0.5f, 0f), true, false, false);
			sourcePlayer.stats.RecalculateStats(sourcePlayer, false, false);
		}
	}

	// Token: 0x06007850 RID: 30800 RVA: 0x00301BAC File Offset: 0x002FFDAC
	private void PostProcessBeam(BeamController beam)
	{
		if (beam)
		{
			Projectile projectile = beam.projectile;
			if (projectile)
			{
				this.PostProcessProjectile(projectile, 1f);
			}
			beam.AdjustPlayerBeamTint(this.TintColor.WithAlpha(this.TintColor.a / 2f), this.TintPriority, 0f);
		}
	}

	// Token: 0x06007851 RID: 30801 RVA: 0x00301C10 File Offset: 0x002FFE10
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		if (!this.m_player)
		{
			return;
		}
		obj.BlackPhantomDamageMultiplier *= this.BlackPhantomDamageMultiplier;
		if (this.m_player && this.m_player.HasActiveBonusSynergy(CustomSynergyType.DEMONHUNTER, false))
		{
			obj.BlackPhantomDamageMultiplier *= 1.5f;
		}
		if (this.TintBullets)
		{
			obj.AdjustPlayerProjectileTint(this.TintColor, this.TintPriority, 0f);
		}
	}

	// Token: 0x06007852 RID: 30802 RVA: 0x00301C98 File Offset: 0x002FFE98
	private void RemoveSynergyStat(PlayerController targetPlayer)
	{
		if (this.m_synergyStat != null && targetPlayer)
		{
			targetPlayer.ownerlessStatModifiers.Remove(this.m_synergyStat);
			targetPlayer.stats.RecalculateStats(targetPlayer, false, false);
			this.m_synergyStat = null;
		}
	}

	// Token: 0x06007853 RID: 30803 RVA: 0x00301CD8 File Offset: 0x002FFED8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<SilverBulletsPassiveItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		player.OnKilledEnemyContext -= this.HandleKilledEnemy;
		this.RemoveSynergyStat(player);
		return debrisObject;
	}

	// Token: 0x06007854 RID: 30804 RVA: 0x00301D40 File Offset: 0x002FFF40
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
			this.m_player.OnKilledEnemyContext -= this.HandleKilledEnemy;
			this.RemoveSynergyStat(this.m_player);
		}
	}

	// Token: 0x04007A83 RID: 31363
	public float BlackPhantomDamageMultiplier = 2f;

	// Token: 0x04007A84 RID: 31364
	private PlayerController m_player;

	// Token: 0x04007A85 RID: 31365
	public bool TintBullets;

	// Token: 0x04007A86 RID: 31366
	public bool TintBeams;

	// Token: 0x04007A87 RID: 31367
	public Color TintColor = Color.grey;

	// Token: 0x04007A88 RID: 31368
	public int TintPriority = 1;

	// Token: 0x04007A89 RID: 31369
	public GameObject SynergyPowerVFX;

	// Token: 0x04007A8A RID: 31370
	private StatModifier m_synergyStat;
}
