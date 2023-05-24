using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001402 RID: 5122
public class EyeOfTheTigerItem : PassiveItem
{
	// Token: 0x0600743D RID: 29757 RVA: 0x002E3E50 File Offset: 0x002E2050
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		HealthHaver healthHaver = player.healthHaver;
		healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeam += this.PostProcessBeam;
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollTimeMultiplier *= this.DodgeRollTimeMultiplier;
			player.rollStats.rollDistanceMultiplier *= this.DodgeRollDistanceMultiplier;
		}
		player.OnKilledEnemy += this.HandleKilledEnemy;
		if (!this.m_pickedUpThisRun && this.DoesFullHeal)
		{
			player.healthHaver.FullHeal();
		}
		base.Pickup(player);
	}

	// Token: 0x0600743E RID: 29758 RVA: 0x002E3F2C File Offset: 0x002E212C
	private void HandleKilledEnemy(PlayerController targetPlayer)
	{
		if (this.HasTimedStatSynergyBuffOnKill && targetPlayer.HasActiveBonusSynergy(this.KillTimedSynergyBuff.RequiredSynergy, false))
		{
			if (this.m_temporaryKillStatModifier == null)
			{
				this.m_temporaryKillStatModifier = new StatModifier();
				this.m_temporaryKillStatModifier.statToBoost = this.KillTimedSynergyBuff.statToBoost;
				this.m_temporaryKillStatModifier.modifyType = this.KillTimedSynergyBuff.modifyType;
				this.m_temporaryKillStatModifier.amount = this.KillTimedSynergyBuff.amount;
			}
			if (this.m_remainingKillModifierTime <= 0f)
			{
				targetPlayer.StartCoroutine(this.HandleTimedKillStatBoost(targetPlayer));
			}
			this.m_remainingKillModifierTime = this.KillTimedSynergyBuff.duration;
		}
	}

	// Token: 0x0600743F RID: 29759 RVA: 0x002E3FE4 File Offset: 0x002E21E4
	private IEnumerator HandleTimedKillStatBoost(PlayerController target)
	{
		target.ownerlessStatModifiers.Add(this.m_temporaryKillStatModifier);
		target.stats.RecalculateStats(target, false, false);
		GameObject buffVFXPrefab = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Buff_Status");
		GameObject spawnedVFX = target.PlayEffectOnActor(buffVFXPrefab, new Vector3(0f, 1.25f, 0f), true, false, false);
		yield return null;
		while (this.m_remainingKillModifierTime > 0f)
		{
			this.m_remainingKillModifierTime -= BraveTime.DeltaTime;
			yield return null;
		}
		SpawnManager.Despawn(spawnedVFX.gameObject);
		target.ownerlessStatModifiers.Remove(this.m_temporaryKillStatModifier);
		target.stats.RecalculateStats(target, false, false);
		yield break;
	}

	// Token: 0x06007440 RID: 29760 RVA: 0x002E4008 File Offset: 0x002E2208
	private void PostProcessBeam(BeamController obj)
	{
		obj.projectile.baseData.damage *= this.DamageMultiplier;
	}

	// Token: 0x06007441 RID: 29761 RVA: 0x002E4028 File Offset: 0x002E2228
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		obj.baseData.damage *= this.DamageMultiplier;
	}

	// Token: 0x06007442 RID: 29762 RVA: 0x002E4044 File Offset: 0x002E2244
	private void ModifyIncomingDamage(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
	{
		if (args == EventArgs.Empty)
		{
			return;
		}
		if (source.GetCurrentHealth() <= this.HealthThreshold && UnityEngine.Random.value < this.ChanceToIgnoreDamage)
		{
			if (this.OnIgnoredDamageVFX != null)
			{
				source.GetComponent<PlayerController>().PlayEffectOnActor(this.OnIgnoredDamageVFX, Vector3.zero, true, false, false);
			}
			args.ModifiedDamage = 0f;
		}
	}

	// Token: 0x06007443 RID: 29763 RVA: 0x002E40B4 File Offset: 0x002E22B4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		HealthHaver healthHaver = player.healthHaver;
		healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		player.OnKilledEnemy -= this.HandleKilledEnemy;
		if (this.ModifiesDodgeRoll)
		{
			player.rollStats.rollTimeMultiplier /= this.DodgeRollTimeMultiplier;
			player.rollStats.rollDistanceMultiplier /= this.DodgeRollDistanceMultiplier;
		}
		debrisObject.GetComponent<EyeOfTheTigerItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007444 RID: 29764 RVA: 0x002E4170 File Offset: 0x002E2370
	protected override void OnDestroy()
	{
		if (this.m_pickedUp)
		{
			HealthHaver healthHaver = this.m_owner.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.ModifyIncomingDamage));
			this.m_owner.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_owner.PostProcessBeam -= this.PostProcessBeam;
			this.m_owner.OnKilledEnemy -= this.HandleKilledEnemy;
			if (this.ModifiesDodgeRoll)
			{
				this.m_owner.rollStats.rollTimeMultiplier /= this.DodgeRollTimeMultiplier;
				this.m_owner.rollStats.rollDistanceMultiplier /= this.DodgeRollDistanceMultiplier;
			}
		}
		base.OnDestroy();
	}

	// Token: 0x040075DD RID: 30173
	public float HealthThreshold = 1f;

	// Token: 0x040075DE RID: 30174
	public float ChanceToIgnoreDamage = 0.5f;

	// Token: 0x040075DF RID: 30175
	public float DamageMultiplier = 2f;

	// Token: 0x040075E0 RID: 30176
	public bool ModifiesDodgeRoll;

	// Token: 0x040075E1 RID: 30177
	public float DodgeRollTimeMultiplier = 1f;

	// Token: 0x040075E2 RID: 30178
	public float DodgeRollDistanceMultiplier = 1f;

	// Token: 0x040075E3 RID: 30179
	public bool DoesFullHeal;

	// Token: 0x040075E4 RID: 30180
	public GameObject OnIgnoredDamageVFX;

	// Token: 0x040075E5 RID: 30181
	public bool HasTimedStatSynergyBuffOnKill;

	// Token: 0x040075E6 RID: 30182
	[ShowInInspectorIf("HasTimedStatSynergyBuffOnKill", false)]
	public TimedSynergyStatBuff KillTimedSynergyBuff;

	// Token: 0x040075E7 RID: 30183
	private float m_remainingKillModifierTime;

	// Token: 0x040075E8 RID: 30184
	private StatModifier m_temporaryKillStatModifier;
}
