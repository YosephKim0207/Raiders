using System;
using UnityEngine;

// Token: 0x0200136D RID: 4973
public class ChaosBulletsItem : PassiveItem
{
	// Token: 0x060070AD RID: 28845 RVA: 0x002CB360 File Offset: 0x002C9560
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
		player.PostProcessBeamTick += this.PostProcessBeamTick;
	}

	// Token: 0x060070AE RID: 28846 RVA: 0x002CB3C0 File Offset: 0x002C95C0
	private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
	{
		if (this.UsesVelocityModificationCurve)
		{
			obj.baseData.speed *= this.VelocityModificationCurve.Evaluate(UnityEngine.Random.value);
		}
		int num = 0;
		while (UnityEngine.Random.value < this.ChanceToAddBounce && num < 10)
		{
			num++;
			BounceProjModifier bounceProjModifier = obj.GetComponent<BounceProjModifier>();
			if (bounceProjModifier == null)
			{
				bounceProjModifier = obj.gameObject.AddComponent<BounceProjModifier>();
				bounceProjModifier.numberOfBounces = 1;
			}
			else
			{
				bounceProjModifier.numberOfBounces++;
			}
		}
		num = 0;
		while (UnityEngine.Random.value < this.ChanceToAddPierce && num < 10)
		{
			num++;
			PierceProjModifier pierceProjModifier = obj.GetComponent<PierceProjModifier>();
			if (pierceProjModifier == null)
			{
				pierceProjModifier = obj.gameObject.AddComponent<PierceProjModifier>();
				pierceProjModifier.penetration = 2;
				pierceProjModifier.penetratesBreakables = true;
				pierceProjModifier.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;
			}
			else
			{
				pierceProjModifier.penetration += 2;
			}
		}
		if (UnityEngine.Random.value < this.ChanceToFat)
		{
			float num2 = UnityEngine.Random.Range(this.MinFatScale, this.MaxFatScale);
			obj.AdditionalScaleMultiplier *= num2;
		}
		float num3 = this.ChanceOfActivatingStatusEffect;
		if (this.ChanceOfActivatingStatusEffect < 1f)
		{
			num3 = this.ChanceOfActivatingStatusEffect * effectChanceScalar;
		}
		if (UnityEngine.Random.value < num3)
		{
			Color color = Color.white;
			float num4 = this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight + this.BurnModifierWeight + this.TransmogrifyModifierWeight;
			float num5 = num4 * UnityEngine.Random.value;
			if (num5 < this.SpeedModifierWeight)
			{
				color = this.SpeedTintColor;
				obj.statusEffectsToApply.Add(this.SpeedModifierEffect);
			}
			else if (num5 < this.SpeedModifierWeight + this.PoisonModifierWeight)
			{
				color = this.PoisonTintColor;
				obj.statusEffectsToApply.Add(this.HealthModifierEffect);
			}
			else if (num5 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight)
			{
				color = this.FreezeTintColor;
				GameActorFreezeEffect freezeModifierEffect = this.FreezeModifierEffect;
				if (this.FreezeScalesWithDamage)
				{
					freezeModifierEffect.FreezeAmount = obj.ModifiedDamage * this.FreezeAmountPerDamage;
				}
				obj.statusEffectsToApply.Add(freezeModifierEffect);
			}
			else if (num5 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight)
			{
				color = this.CharmTintColor;
				obj.statusEffectsToApply.Add(this.CharmModifierEffect);
			}
			else if (num5 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight + this.BurnModifierWeight)
			{
				color = this.FireTintColor;
				obj.statusEffectsToApply.Add(this.FireModifierEffect);
			}
			else if (num5 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight + this.BurnModifierWeight + this.TransmogrifyModifierWeight)
			{
				color = this.TransmogrifyTintColor;
				obj.CanTransmogrify = true;
				obj.ChanceToTransmogrify = 1f;
				obj.TransmogrifyTargetGuids = new string[1];
				obj.TransmogrifyTargetGuids[0] = this.TransmogTargetGuid;
			}
			if (this.TintBullets)
			{
				obj.AdjustPlayerProjectileTint(color, this.TintPriority, 0f);
			}
		}
	}

	// Token: 0x060070AF RID: 28847 RVA: 0x002CB71C File Offset: 0x002C991C
	private void PostProcessBeam(BeamController beam)
	{
		if (this.TintBeams)
		{
		}
	}

	// Token: 0x060070B0 RID: 28848 RVA: 0x002CB72C File Offset: 0x002C992C
	private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidbody, float tickRate)
	{
		GameActor gameActor = hitRigidbody.gameActor;
		if (!gameActor)
		{
			return;
		}
		if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.ChanceOfStatusEffectFromBeamPerSecond, tickRate))
		{
			float num = this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight + this.BurnModifierWeight + this.TransmogrifyModifierWeight;
			float num2 = num * UnityEngine.Random.value;
			if (num2 < this.SpeedModifierWeight)
			{
				gameActor.ApplyEffect(this.SpeedModifierEffect, 1f, null);
			}
			else if (num2 < this.SpeedModifierWeight + this.PoisonModifierWeight)
			{
				gameActor.ApplyEffect(this.HealthModifierEffect, 1f, null);
			}
			else if (num2 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight)
			{
				gameActor.ApplyEffect(this.FreezeModifierEffect, 1f, null);
			}
			else if (num2 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight)
			{
				gameActor.ApplyEffect(this.CharmModifierEffect, 1f, null);
			}
			else if (num2 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight + this.BurnModifierWeight)
			{
				gameActor.ApplyEffect(this.FireModifierEffect, 1f, null);
			}
			else if (num2 < this.SpeedModifierWeight + this.PoisonModifierWeight + this.FreezeModifierWeight + this.CharmModifierWeight + this.BurnModifierWeight + this.TransmogrifyModifierWeight && gameActor is AIActor)
			{
				AIActor aiactor = gameActor as AIActor;
				aiactor.Transmogrify(EnemyDatabase.GetOrLoadByGuid(this.TransmogTargetGuid), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
			}
		}
	}

	// Token: 0x060070B1 RID: 28849 RVA: 0x002CB8EC File Offset: 0x002C9AEC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<ChaosBulletsItem>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		player.PostProcessBeam -= this.PostProcessBeam;
		player.PostProcessBeamTick -= this.PostProcessBeamTick;
		return debrisObject;
	}

	// Token: 0x060070B2 RID: 28850 RVA: 0x002CB94C File Offset: 0x002C9B4C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
			this.m_player.PostProcessBeam -= this.PostProcessBeam;
			this.m_player.PostProcessBeamTick -= this.PostProcessBeamTick;
		}
	}

	// Token: 0x0400701C RID: 28700
	[Header("Nonstatus Effects")]
	public float ChanceToAddBounce;

	// Token: 0x0400701D RID: 28701
	public float ChanceToAddPierce;

	// Token: 0x0400701E RID: 28702
	public float ChanceToFat = 0.1f;

	// Token: 0x0400701F RID: 28703
	public float MinFatScale = 1.25f;

	// Token: 0x04007020 RID: 28704
	public float MaxFatScale = 1.75f;

	// Token: 0x04007021 RID: 28705
	public bool UsesVelocityModificationCurve;

	// Token: 0x04007022 RID: 28706
	public AnimationCurve VelocityModificationCurve;

	// Token: 0x04007023 RID: 28707
	[Header("Status Effects")]
	public float ChanceOfActivatingStatusEffect = 1f;

	// Token: 0x04007024 RID: 28708
	public float ChanceOfStatusEffectFromBeamPerSecond = 1f;

	// Token: 0x04007025 RID: 28709
	public float SpeedModifierWeight;

	// Token: 0x04007026 RID: 28710
	public GameActorSpeedEffect SpeedModifierEffect;

	// Token: 0x04007027 RID: 28711
	public Color SpeedTintColor;

	// Token: 0x04007028 RID: 28712
	public float PoisonModifierWeight;

	// Token: 0x04007029 RID: 28713
	public GameActorHealthEffect HealthModifierEffect;

	// Token: 0x0400702A RID: 28714
	public Color PoisonTintColor;

	// Token: 0x0400702B RID: 28715
	public float CharmModifierWeight;

	// Token: 0x0400702C RID: 28716
	public GameActorCharmEffect CharmModifierEffect;

	// Token: 0x0400702D RID: 28717
	public Color CharmTintColor;

	// Token: 0x0400702E RID: 28718
	public float FreezeModifierWeight;

	// Token: 0x0400702F RID: 28719
	public GameActorFreezeEffect FreezeModifierEffect;

	// Token: 0x04007030 RID: 28720
	public bool FreezeScalesWithDamage;

	// Token: 0x04007031 RID: 28721
	public float FreezeAmountPerDamage = 1f;

	// Token: 0x04007032 RID: 28722
	public Color FreezeTintColor;

	// Token: 0x04007033 RID: 28723
	public float BurnModifierWeight;

	// Token: 0x04007034 RID: 28724
	public GameActorFireEffect FireModifierEffect;

	// Token: 0x04007035 RID: 28725
	public Color FireTintColor;

	// Token: 0x04007036 RID: 28726
	public float TransmogrifyModifierWeight;

	// Token: 0x04007037 RID: 28727
	[EnemyIdentifier]
	public string TransmogTargetGuid;

	// Token: 0x04007038 RID: 28728
	public Color TransmogrifyTintColor;

	// Token: 0x04007039 RID: 28729
	public bool TintBullets;

	// Token: 0x0400703A RID: 28730
	public bool TintBeams;

	// Token: 0x0400703B RID: 28731
	public int TintPriority = 6;

	// Token: 0x0400703C RID: 28732
	private PlayerController m_player;
}
