using System;
using UnityEngine;

// Token: 0x02001364 RID: 4964
public class BulletStatusEffectItem : PassiveItem
{
	// Token: 0x06007079 RID: 28793 RVA: 0x002C9C48 File Offset: 0x002C7E48
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		if (this.ConfersElectricityImmunity)
		{
			this.m_electricityImmunity = new DamageTypeModifier();
			this.m_electricityImmunity.damageMultiplier = 0f;
			this.m_electricityImmunity.damageType = CoreDamageTypes.Electric;
			player.healthHaver.damageTypeModifiers.Add(this.m_electricityImmunity);
		}
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
		player.PostProcessBeam += this.PostProcessBeam;
		player.PostProcessBeamTick += this.PostProcessBeamTick;
	}

	// Token: 0x0600707A RID: 28794 RVA: 0x002C9CF0 File Offset: 0x002C7EF0
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		float num = this.chanceOfActivating;
		if (this.chanceOfActivating < 1f)
		{
			num = this.chanceOfActivating * effectChanceScalar;
		}
		if (this.AppliesFreeze || this.AppliesFire || this.AppliesDamageOverTime)
		{
			if (this.m_player && this.m_player.HasActiveBonusSynergy(CustomSynergyType.ALPHA_STATUS, false) && this.m_player.CurrentGun.LastShotIndex == 0)
			{
				num = 1f;
			}
			if (this.m_player && this.m_player.HasActiveBonusSynergy(CustomSynergyType.OMEGA_STATUS, false) && this.m_player.CurrentGun.LastShotIndex == this.m_player.CurrentGun.ClipCapacity - 1)
			{
				num = 1f;
			}
		}
		if (this.AppliesCharm && base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.UNBELIEVABLY_CHARMING, false))
		{
			num = 1f;
		}
		if (this.AppliesTransmog && base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.BE_A_CHICKEN, false))
		{
			num *= 1.5f;
		}
		if (this.m_player)
		{
			for (int i = 0; i < this.Synergies.Length; i++)
			{
				if (this.m_player.HasActiveBonusSynergy(this.Synergies[i].RequiredSynergy, false))
				{
					num *= this.Synergies[i].ChanceMultiplier;
				}
			}
		}
		if (UnityEngine.Random.value < num)
		{
			if (this.AddsDamageType)
			{
				obj.damageTypes |= this.DamageTypesToAdd;
			}
			if (this.ParticlesToAdd != null)
			{
				GameObject gameObject = SpawnManager.SpawnVFX(this.ParticlesToAdd, true);
				gameObject.transform.parent = obj.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0.5f);
				ParticleKiller component = gameObject.GetComponent<ParticleKiller>();
				if (component != null)
				{
					component.Awake();
				}
			}
			if (this.AppliesSpeedModifier)
			{
				obj.statusEffectsToApply.Add(this.SpeedModifierEffect);
			}
			if (this.AppliesDamageOverTime)
			{
				obj.statusEffectsToApply.Add(this.HealthModifierEffect);
			}
			if (this.AppliesFreeze)
			{
				GameActorFreezeEffect freezeModifierEffect = this.FreezeModifierEffect;
				if (this.FreezeScalesWithDamage)
				{
					freezeModifierEffect.FreezeAmount = obj.ModifiedDamage * this.FreezeAmountPerDamage;
				}
				obj.statusEffectsToApply.Add(freezeModifierEffect);
			}
			if (this.AppliesCharm)
			{
				obj.statusEffectsToApply.Add(this.CharmModifierEffect);
			}
			if (this.AppliesFire)
			{
				obj.statusEffectsToApply.Add(this.FireModifierEffect);
			}
			if (this.AppliesTransmog && !obj.CanTransmogrify)
			{
				obj.CanTransmogrify = true;
				obj.ChanceToTransmogrify = 1f;
				obj.TransmogrifyTargetGuids = new string[1];
				obj.TransmogrifyTargetGuids[0] = this.TransmogTargetGuid;
			}
			if (this.TintBullets)
			{
				obj.AdjustPlayerProjectileTint(this.TintColor, this.TintPriority, 0f);
			}
		}
	}

	// Token: 0x0600707B RID: 28795 RVA: 0x002CA030 File Offset: 0x002C8230
	private void PostProcessBeam(BeamController beam)
	{
		if (this.TintBeams)
		{
			beam.AdjustPlayerBeamTint(this.TintColor.WithAlpha(this.TintColor.a / 2f), this.TintPriority, 0f);
		}
	}

	// Token: 0x0600707C RID: 28796 RVA: 0x002CA06C File Offset: 0x002C826C
	private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidbody, float tickRate)
	{
		GameActor gameActor = hitRigidbody.gameActor;
		if (!gameActor)
		{
			return;
		}
		if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.chanceFromBeamPerSecond, tickRate))
		{
			if (this.AppliesSpeedModifier)
			{
				gameActor.ApplyEffect(this.SpeedModifierEffect, 1f, null);
			}
			if (this.AppliesDamageOverTime)
			{
				gameActor.ApplyEffect(this.HealthModifierEffect, 1f, null);
			}
			if (this.AppliesFreeze)
			{
				gameActor.ApplyEffect(this.FreezeModifierEffect, 1f, null);
			}
			if (this.AppliesCharm)
			{
				gameActor.ApplyEffect(this.CharmModifierEffect, 1f, null);
			}
			if (this.AppliesFire)
			{
				gameActor.ApplyEffect(this.FireModifierEffect, 1f, null);
			}
			if (this.AppliesTransmog && gameActor is AIActor)
			{
				AIActor aiactor = gameActor as AIActor;
				aiactor.Transmogrify(EnemyDatabase.GetOrLoadByGuid(this.TransmogTargetGuid), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
			}
		}
	}

	// Token: 0x0600707D RID: 28797 RVA: 0x002CA170 File Offset: 0x002C8370
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<BulletStatusEffectItem>().m_pickedUpThisRun = true;
		if (this.m_electricityImmunity != null)
		{
			player.healthHaver.damageTypeModifiers.Remove(this.m_electricityImmunity);
		}
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		player.PostProcessBeamTick -= this.PostProcessBeamTick;
		return debrisObject;
	}

	// Token: 0x0600707E RID: 28798 RVA: 0x002CA1F4 File Offset: 0x002C83F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			if (this.m_electricityImmunity != null)
			{
				this.m_player.healthHaver.damageTypeModifiers.Remove(this.m_electricityImmunity);
			}
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
			this.m_player.PostProcessBeamTick -= this.PostProcessBeamTick;
		}
	}

	// Token: 0x04006FE9 RID: 28649
	public float chanceOfActivating = 1f;

	// Token: 0x04006FEA RID: 28650
	public float chanceFromBeamPerSecond = 1f;

	// Token: 0x04006FEB RID: 28651
	public bool TintBullets;

	// Token: 0x04006FEC RID: 28652
	public bool TintBeams;

	// Token: 0x04006FED RID: 28653
	public Color TintColor = Color.green;

	// Token: 0x04006FEE RID: 28654
	public int TintPriority = 5;

	// Token: 0x04006FEF RID: 28655
	public GameObject ParticlesToAdd;

	// Token: 0x04006FF0 RID: 28656
	public bool AddsDamageType;

	// Token: 0x04006FF1 RID: 28657
	[EnumFlags]
	public CoreDamageTypes DamageTypesToAdd;

	// Token: 0x04006FF2 RID: 28658
	[Header("Status Effects")]
	public bool AppliesSpeedModifier;

	// Token: 0x04006FF3 RID: 28659
	public GameActorSpeedEffect SpeedModifierEffect;

	// Token: 0x04006FF4 RID: 28660
	public bool AppliesDamageOverTime;

	// Token: 0x04006FF5 RID: 28661
	public GameActorHealthEffect HealthModifierEffect;

	// Token: 0x04006FF6 RID: 28662
	public bool AppliesCharm;

	// Token: 0x04006FF7 RID: 28663
	public GameActorCharmEffect CharmModifierEffect;

	// Token: 0x04006FF8 RID: 28664
	public bool AppliesFreeze;

	// Token: 0x04006FF9 RID: 28665
	public GameActorFreezeEffect FreezeModifierEffect;

	// Token: 0x04006FFA RID: 28666
	[ShowInInspectorIf("AppliesFreeze", false)]
	public bool FreezeScalesWithDamage;

	// Token: 0x04006FFB RID: 28667
	[ShowInInspectorIf("FreezeScalesWithDamage", false)]
	public float FreezeAmountPerDamage = 1f;

	// Token: 0x04006FFC RID: 28668
	public bool AppliesFire;

	// Token: 0x04006FFD RID: 28669
	public GameActorFireEffect FireModifierEffect;

	// Token: 0x04006FFE RID: 28670
	public bool ConfersElectricityImmunity;

	// Token: 0x04006FFF RID: 28671
	public bool AppliesTransmog;

	// Token: 0x04007000 RID: 28672
	[EnemyIdentifier]
	public string TransmogTargetGuid;

	// Token: 0x04007001 RID: 28673
	public BulletStatusEffectItemSynergy[] Synergies;

	// Token: 0x04007002 RID: 28674
	private PlayerController m_player;

	// Token: 0x04007003 RID: 28675
	private DamageTypeModifier m_electricityImmunity;
}
