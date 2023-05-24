using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001098 RID: 4248
public class HealthHaver : BraveBehaviour
{
	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06005D75 RID: 23925 RVA: 0x0023D748 File Offset: 0x0023B948
	// (remove) Token: 0x06005D76 RID: 23926 RVA: 0x0023D780 File Offset: 0x0023B980
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Vector2> OnPreDeath;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06005D77 RID: 23927 RVA: 0x0023D7B8 File Offset: 0x0023B9B8
	// (remove) Token: 0x06005D78 RID: 23928 RVA: 0x0023D7F0 File Offset: 0x0023B9F0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Vector2> OnDeath;

	// Token: 0x140000AD RID: 173
	// (add) Token: 0x06005D79 RID: 23929 RVA: 0x0023D828 File Offset: 0x0023BA28
	// (remove) Token: 0x06005D7A RID: 23930 RVA: 0x0023D860 File Offset: 0x0023BA60
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event HealthHaver.OnDamagedEvent OnDamaged;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x06005D7B RID: 23931 RVA: 0x0023D898 File Offset: 0x0023BA98
	// (remove) Token: 0x06005D7C RID: 23932 RVA: 0x0023D8D0 File Offset: 0x0023BAD0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event HealthHaver.OnHealthChangedEvent OnHealthChanged;

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x06005D7D RID: 23933 RVA: 0x0023D908 File Offset: 0x0023BB08
	// (set) Token: 0x06005D7E RID: 23934 RVA: 0x0023D910 File Offset: 0x0023BB10
	public float CursedMaximum
	{
		get
		{
			return this.m_curseHealthMaximum;
		}
		set
		{
			this.m_curseHealthMaximum = value;
			this.currentHealth = Mathf.Min(this.currentHealth, this.AdjustedMaxHealth);
			if (this.OnHealthChanged != null)
			{
				this.OnHealthChanged(this.GetCurrentHealth(), this.GetMaxHealth());
			}
		}
	}

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x06005D7F RID: 23935 RVA: 0x0023D960 File Offset: 0x0023BB60
	// (set) Token: 0x06005D80 RID: 23936 RVA: 0x0023D968 File Offset: 0x0023BB68
	protected float AdjustedMaxHealth
	{
		get
		{
			return this.GetMaxHealth();
		}
		set
		{
			this.maximumHealth = value;
		}
	}

	// Token: 0x06005D81 RID: 23937 RVA: 0x0023D974 File Offset: 0x0023BB74
	public float GetMaxHealth()
	{
		return Mathf.Min(this.CursedMaximum, this.maximumHealth);
	}

	// Token: 0x06005D82 RID: 23938 RVA: 0x0023D988 File Offset: 0x0023BB88
	public float GetCurrentHealth()
	{
		return this.currentHealth;
	}

	// Token: 0x06005D83 RID: 23939 RVA: 0x0023D990 File Offset: 0x0023BB90
	public void ForceSetCurrentHealth(float h)
	{
		this.currentHealth = h;
		this.currentHealth = Mathf.Min(this.currentHealth, this.GetMaxHealth());
		if (this.OnHealthChanged != null)
		{
			this.OnHealthChanged(this.currentHealth, this.GetMaxHealth());
		}
	}

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x06005D84 RID: 23940 RVA: 0x0023D9E0 File Offset: 0x0023BBE0
	// (set) Token: 0x06005D85 RID: 23941 RVA: 0x0023D9E8 File Offset: 0x0023BBE8
	public float Armor
	{
		get
		{
			return this.currentArmor;
		}
		set
		{
			if (this.m_player && !this.m_player.ForceZeroHealthState && this.IsDead)
			{
				return;
			}
			this.currentArmor = value;
			if (this.OnHealthChanged != null)
			{
				this.OnHealthChanged(this.currentHealth, this.AdjustedMaxHealth);
			}
		}
	}

	// Token: 0x06005D86 RID: 23942 RVA: 0x0023DA4C File Offset: 0x0023BC4C
	public float GetCurrentHealthPercentage()
	{
		return this.currentHealth / this.AdjustedMaxHealth;
	}

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x06005D87 RID: 23943 RVA: 0x0023DA5C File Offset: 0x0023BC5C
	// (set) Token: 0x06005D88 RID: 23944 RVA: 0x0023DADC File Offset: 0x0023BCDC
	public bool IsVulnerable
	{
		get
		{
			if (this.isPlayerCharacter && this.m_player.rollStats.additionalInvulnerabilityFrames > 0)
			{
				for (int i = 1; i <= this.m_player.rollStats.additionalInvulnerabilityFrames; i++)
				{
					if (base.spriteAnimator.QueryPreviousInvulnerabilityFrame(i))
					{
						return false;
					}
				}
			}
			return this.vulnerable && !base.spriteAnimator.QueryInvulnerabilityFrame();
		}
		set
		{
			this.vulnerable = value;
		}
	}

	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x06005D89 RID: 23945 RVA: 0x0023DAE8 File Offset: 0x0023BCE8
	public bool IsAlive
	{
		get
		{
			return this.GetCurrentHealth() > 0f || this.Armor > 0f;
		}
	}

	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x06005D8A RID: 23946 RVA: 0x0023DB0C File Offset: 0x0023BD0C
	public bool IsDead
	{
		get
		{
			return this.GetCurrentHealth() <= 0f && this.Armor <= 0f;
		}
	}

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x06005D8B RID: 23947 RVA: 0x0023DB34 File Offset: 0x0023BD34
	// (set) Token: 0x06005D8C RID: 23948 RVA: 0x0023DB3C File Offset: 0x0023BD3C
	public bool ManualDeathHandling { get; set; }

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x06005D8D RID: 23949 RVA: 0x0023DB48 File Offset: 0x0023BD48
	// (set) Token: 0x06005D8E RID: 23950 RVA: 0x0023DB50 File Offset: 0x0023BD50
	public bool DisableStickyFriction { get; set; }

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x06005D8F RID: 23951 RVA: 0x0023DB5C File Offset: 0x0023BD5C
	public bool IsBoss
	{
		get
		{
			return this.bossHealthBar != HealthHaver.BossBarType.None;
		}
	}

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x06005D90 RID: 23952 RVA: 0x0023DB6C File Offset: 0x0023BD6C
	public bool IsSubboss
	{
		get
		{
			return this.bossHealthBar == HealthHaver.BossBarType.SubbossBar;
		}
	}

	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x06005D91 RID: 23953 RVA: 0x0023DB78 File Offset: 0x0023BD78
	public bool UsesSecondaryBossBar
	{
		get
		{
			return this.bossHealthBar == HealthHaver.BossBarType.SecondaryBar;
		}
	}

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x06005D92 RID: 23954 RVA: 0x0023DB84 File Offset: 0x0023BD84
	public bool UsesVerticalBossBar
	{
		get
		{
			return this.bossHealthBar == HealthHaver.BossBarType.VerticalBar;
		}
	}

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x06005D93 RID: 23955 RVA: 0x0023DB90 File Offset: 0x0023BD90
	public bool HasHealthBar
	{
		get
		{
			return this.bossHealthBar != HealthHaver.BossBarType.None && this.bossHealthBar != HealthHaver.BossBarType.SecretBar && this.bossHealthBar != HealthHaver.BossBarType.SubbossBar;
		}
	}

	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x06005D94 RID: 23956 RVA: 0x0023DBB8 File Offset: 0x0023BDB8
	// (set) Token: 0x06005D95 RID: 23957 RVA: 0x0023DBC0 File Offset: 0x0023BDC0
	public Vector2? OverrideKillCamPos { get; set; }

	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x06005D96 RID: 23958 RVA: 0x0023DBCC File Offset: 0x0023BDCC
	// (set) Token: 0x06005D97 RID: 23959 RVA: 0x0023DBD4 File Offset: 0x0023BDD4
	public float? OverrideKillCamTime { get; set; }

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x06005D98 RID: 23960 RVA: 0x0023DBE0 File Offset: 0x0023BDE0
	// (set) Token: 0x06005D99 RID: 23961 RVA: 0x0023DBE8 File Offset: 0x0023BDE8
	public bool TrackDuringDeath { get; set; }

	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x06005D9A RID: 23962 RVA: 0x0023DBF4 File Offset: 0x0023BDF4
	// (set) Token: 0x06005D9B RID: 23963 RVA: 0x0023DBFC File Offset: 0x0023BDFC
	public bool SuppressContinuousKillCamBulletDestruction { get; set; }

	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x06005D9C RID: 23964 RVA: 0x0023DC08 File Offset: 0x0023BE08
	// (set) Token: 0x06005D9D RID: 23965 RVA: 0x0023DC10 File Offset: 0x0023BE10
	public bool SuppressDeathSounds { get; set; }

	// Token: 0x17000DCA RID: 3530
	// (get) Token: 0x06005D9E RID: 23966 RVA: 0x0023DC1C File Offset: 0x0023BE1C
	public bool CanCurrentlyBeKilled
	{
		get
		{
			return this.IsVulnerable && !this.PreventAllDamage && this.minimumHealth <= 0f;
		}
	}

	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x06005D9F RID: 23967 RVA: 0x0023DC48 File Offset: 0x0023BE48
	// (set) Token: 0x06005DA0 RID: 23968 RVA: 0x0023DC50 File Offset: 0x0023BE50
	public bool PreventCooldownGainFromDamage { get; set; }

	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x06005DA1 RID: 23969 RVA: 0x0023DC5C File Offset: 0x0023BE5C
	// (set) Token: 0x06005DA2 RID: 23970 RVA: 0x0023DC64 File Offset: 0x0023BE64
	public bool TrackPixelColliderDamage { get; private set; }

	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x06005DA3 RID: 23971 RVA: 0x0023DC70 File Offset: 0x0023BE70
	// (set) Token: 0x06005DA4 RID: 23972 RVA: 0x0023DC78 File Offset: 0x0023BE78
	public Dictionary<PixelCollider, float> PixelColliderDamage { get; private set; }

	// Token: 0x06005DA5 RID: 23973 RVA: 0x0023DC84 File Offset: 0x0023BE84
	public void AddTrackedDamagePixelCollider(PixelCollider pixelCollider)
	{
		this.TrackPixelColliderDamage = true;
		if (this.PixelColliderDamage == null)
		{
			this.PixelColliderDamage = new Dictionary<PixelCollider, float>();
		}
		this.PixelColliderDamage.Add(pixelCollider, 0f);
	}

	// Token: 0x06005DA6 RID: 23974 RVA: 0x0023DCB4 File Offset: 0x0023BEB4
	public void Awake()
	{
		StaticReferenceManager.AllHealthHavers.Add(this);
		if (GameManager.Instance.InTutorial)
		{
			if (base.name.StartsWith("BulletMan"))
			{
				this.maximumHealth = 10f;
			}
			if (base.name.StartsWith("BulletShotgunMan"))
			{
				this.maximumHealth = 15f;
			}
		}
		this.currentHealth = this.AdjustedMaxHealth;
		this.RegisterBodySprite(base.sprite, false, 0);
		if (this.IsBoss)
		{
			base.aiActor.SetResistance(EffectResistanceType.Freeze, Mathf.Max(base.aiActor.GetResistanceForEffectType(EffectResistanceType.Freeze), 0.6f));
			base.aiActor.SetResistance(EffectResistanceType.Fire, Mathf.Max(base.aiActor.GetResistanceForEffectType(EffectResistanceType.Fire), 0.25f));
			if (base.knockbackDoer)
			{
				base.knockbackDoer.SetImmobile(true, "Like-a-boss");
			}
		}
	}

	// Token: 0x06005DA7 RID: 23975 RVA: 0x0023DDA8 File Offset: 0x0023BFA8
	private void Start()
	{
		if (base.spriteAnimator == null)
		{
			base.spriteAnimator = base.GetComponentInChildren<tk2dSpriteAnimator>();
		}
		this.m_player = base.GetComponent<PlayerController>();
		if (this.m_player != null)
		{
			this.isPlayerCharacter = true;
		}
		GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
		if (lastLoadedLevelDefinition != null)
		{
			this.m_damageCap = lastLoadedLevelDefinition.damageCap;
			if (this.IsBoss && !this.IsSubboss && lastLoadedLevelDefinition.bossDpsCap > 0f)
			{
				float num = 1f;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					num = (GameManager.Instance.COOP_ENEMY_HEALTH_MULTIPLIER + 2f) / 2f;
				}
				this.m_bossDpsCap = lastLoadedLevelDefinition.bossDpsCap * num;
			}
		}
	}

	// Token: 0x06005DA8 RID: 23976 RVA: 0x0023DE74 File Offset: 0x0023C074
	public void Update()
	{
		this.isFirstFrame = false;
		if (this.m_bossDpsCap > 0f && this.m_recentBossDps > 0f)
		{
			this.m_recentBossDps = Mathf.Max(0f, this.m_recentBossDps - this.m_bossDpsCap * BraveTime.DeltaTime);
		}
	}

	// Token: 0x06005DA9 RID: 23977 RVA: 0x0023DECC File Offset: 0x0023C0CC
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllHealthHavers.Remove(this);
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.BulletScriptEventTriggered));
		base.OnDestroy();
	}

	// Token: 0x06005DAA RID: 23978 RVA: 0x0023DF08 File Offset: 0x0023C108
	public void RegisterBodySprite(tk2dBaseSprite sprite, bool flashIndependentlyOnDamage = false, int flashPixelCollider = 0)
	{
		if (!this.bodySprites.Contains(sprite))
		{
			this.bodySprites.Add(sprite);
		}
		if (flashIndependentlyOnDamage)
		{
			if (this.m_independentDamageFlashers == null)
			{
				this.m_independentDamageFlashers = new Dictionary<PixelCollider, tk2dBaseSprite>();
			}
			this.m_independentDamageFlashers.Add(base.specRigidbody.PixelColliders[flashPixelCollider], sprite);
		}
	}

	// Token: 0x06005DAB RID: 23979 RVA: 0x0023DF6C File Offset: 0x0023C16C
	public void ApplyHealing(float healing)
	{
		if (this.isPlayerCharacter && this.m_player.IsGhost)
		{
			return;
		}
		if (this.ModifyHealing != null)
		{
			HealthHaver.ModifyHealingEventArgs modifyHealingEventArgs = new HealthHaver.ModifyHealingEventArgs
			{
				InitialHealing = healing,
				ModifiedHealing = healing
			};
			this.ModifyHealing(this, modifyHealingEventArgs);
			healing = modifyHealingEventArgs.ModifiedHealing;
		}
		this.currentHealth += healing;
		if (this.quantizeHealth)
		{
			this.currentHealth = BraveMathCollege.QuantizeFloat(this.currentHealth, this.quantizedIncrement);
		}
		if (this.currentHealth > this.AdjustedMaxHealth)
		{
			this.currentHealth = this.AdjustedMaxHealth;
		}
		if (this.OnHealthChanged != null)
		{
			this.OnHealthChanged(this.currentHealth, this.AdjustedMaxHealth);
		}
	}

	// Token: 0x06005DAC RID: 23980 RVA: 0x0023E03C File Offset: 0x0023C23C
	public void FullHeal()
	{
		this.currentHealth = this.AdjustedMaxHealth;
		if (this.OnHealthChanged != null)
		{
			this.OnHealthChanged(this.currentHealth, this.AdjustedMaxHealth);
		}
	}

	// Token: 0x06005DAD RID: 23981 RVA: 0x0023E06C File Offset: 0x0023C26C
	public void SetHealthMaximum(float targetValue, float? amountOfHealthToGain = null, bool keepHealthPercentage = false)
	{
		if (targetValue == this.maximumHealth)
		{
			return;
		}
		float currentHealthPercentage = this.GetCurrentHealthPercentage();
		if (!keepHealthPercentage)
		{
			if (amountOfHealthToGain != null)
			{
				this.currentHealth += amountOfHealthToGain.Value;
			}
			else if (targetValue > this.maximumHealth)
			{
				this.currentHealth += targetValue - this.maximumHealth;
			}
		}
		this.maximumHealth = targetValue;
		if (keepHealthPercentage)
		{
			this.currentHealth = currentHealthPercentage * this.AdjustedMaxHealth;
			if (amountOfHealthToGain != null)
			{
				this.currentHealth += amountOfHealthToGain.Value;
			}
		}
		this.currentHealth = Mathf.Min(this.currentHealth, this.AdjustedMaxHealth);
		if (this.quantizeHealth)
		{
			this.currentHealth = BraveMathCollege.QuantizeFloat(this.currentHealth, this.quantizedIncrement);
			this.maximumHealth = BraveMathCollege.QuantizeFloat(this.maximumHealth, this.quantizedIncrement);
		}
		if (this.OnHealthChanged != null)
		{
			this.OnHealthChanged(this.currentHealth, this.AdjustedMaxHealth);
		}
	}

	// Token: 0x06005DAE RID: 23982 RVA: 0x0023E188 File Offset: 0x0023C388
	public void ApplyDamage(float damage, Vector2 direction, string sourceName, CoreDamageTypes damageTypes = CoreDamageTypes.None, DamageCategory damageCategory = DamageCategory.Normal, bool ignoreInvulnerabilityFrames = false, PixelCollider hitPixelCollider = null, bool ignoreDamageCaps = false)
	{
		this.ApplyDamageDirectional(damage, direction, sourceName, damageTypes, damageCategory, ignoreInvulnerabilityFrames, hitPixelCollider, ignoreDamageCaps);
	}

	// Token: 0x06005DAF RID: 23983 RVA: 0x0023E1A8 File Offset: 0x0023C3A8
	public float GetDamageModifierForType(CoreDamageTypes damageTypes)
	{
		float num = 1f;
		for (int i = 0; i < this.damageTypeModifiers.Count; i++)
		{
			if ((damageTypes & this.damageTypeModifiers[i].damageType) == this.damageTypeModifiers[i].damageType)
			{
				num *= this.damageTypeModifiers[i].damageMultiplier;
			}
		}
		if (this.isPlayerCharacter && this.m_player && this.m_player.CurrentGun && this.m_player.CurrentGun.currentGunDamageTypeModifiers != null)
		{
			for (int j = 0; j < this.m_player.CurrentGun.currentGunDamageTypeModifiers.Length; j++)
			{
				if ((damageTypes & this.m_player.CurrentGun.currentGunDamageTypeModifiers[j].damageType) == this.m_player.CurrentGun.currentGunDamageTypeModifiers[j].damageType)
				{
					num *= this.m_player.CurrentGun.currentGunDamageTypeModifiers[j].damageMultiplier;
				}
			}
		}
		return num;
	}

	// Token: 0x06005DB0 RID: 23984 RVA: 0x0023E2CC File Offset: 0x0023C4CC
	private bool BossHealthSanityCheck(float rawDamage)
	{
		if (GameManager.Instance.PrimaryPlayer.healthHaver.IsDead)
		{
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				if ((!GameManager.Instance.SecondaryPlayer || GameManager.Instance.SecondaryPlayer.healthHaver.IsDead) && this.GetCurrentHealth() <= rawDamage)
				{
					return false;
				}
			}
			else if (this.GetCurrentHealth() <= rawDamage)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x06005DB1 RID: 23985 RVA: 0x0023E354 File Offset: 0x0023C554
	// (set) Token: 0x06005DB2 RID: 23986 RVA: 0x0023E35C File Offset: 0x0023C55C
	public List<PixelCollider> DamageableColliders { get; set; }

	// Token: 0x06005DB3 RID: 23987 RVA: 0x0023E368 File Offset: 0x0023C568
	protected void ApplyDamageDirectional(float damage, Vector2 direction, string damageSource, CoreDamageTypes damageTypes, DamageCategory damageCategory = DamageCategory.Normal, bool ignoreInvulnerabilityFrames = false, PixelCollider hitPixelCollider = null, bool ignoreDamageCaps = false)
	{
		if (this.GetCurrentHealth() > this.GetMaxHealth())
		{
			UnityEngine.Debug.Log("Something went wrong in HealthHaver, but we caught it! " + this.currentHealth);
			this.currentHealth = this.GetMaxHealth();
		}
		if (this.PreventAllDamage && damageCategory == DamageCategory.Unstoppable)
		{
			this.PreventAllDamage = false;
		}
		if (this.PreventAllDamage)
		{
			return;
		}
		if (this.m_player && this.m_player.IsGhost)
		{
			return;
		}
		if (hitPixelCollider != null && this.DamageableColliders != null && !this.DamageableColliders.Contains(hitPixelCollider))
		{
			return;
		}
		if (this.IsBoss && !this.BossHealthSanityCheck(damage))
		{
			return;
		}
		if (this.isFirstFrame)
		{
			return;
		}
		if (ignoreInvulnerabilityFrames)
		{
			if (!this.vulnerable)
			{
				return;
			}
		}
		else if (!this.IsVulnerable)
		{
			return;
		}
		if (damage <= 0f)
		{
			return;
		}
		damage *= this.GetDamageModifierForType(damageTypes);
		damage *= this.AllDamageMultiplier;
		if (this.OnlyAllowSpecialBossDamage && (damageTypes & CoreDamageTypes.SpecialBossDamage) != CoreDamageTypes.SpecialBossDamage)
		{
			damage = 0f;
		}
		if (this.IsBoss && !string.IsNullOrEmpty(damageSource))
		{
			if (damageSource == "primaryplayer")
			{
				damage *= GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
			}
			else if (damageSource == "secondaryplayer")
			{
				damage *= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.DamageToBosses);
			}
		}
		if (this.m_player && !ignoreInvulnerabilityFrames)
		{
			damage = Mathf.Min(damage, 0.5f);
		}
		if (this.m_player && damageCategory == DamageCategory.BlackBullet)
		{
			damage = 1f;
		}
		if (this.ModifyDamage != null)
		{
			HealthHaver.ModifyDamageEventArgs modifyDamageEventArgs = new HealthHaver.ModifyDamageEventArgs
			{
				InitialDamage = damage,
				ModifiedDamage = damage
			};
			this.ModifyDamage(this, modifyDamageEventArgs);
			damage = modifyDamageEventArgs.ModifiedDamage;
		}
		if (!this.m_player && !ignoreInvulnerabilityFrames && damage <= 999f && !ignoreDamageCaps)
		{
			if (this.m_damageCap > 0f)
			{
				damage = Mathf.Min(this.m_damageCap, damage);
			}
			if (this.m_bossDpsCap > 0f)
			{
				damage = Mathf.Min(damage, this.m_bossDpsCap * 3f - this.m_recentBossDps);
				this.m_recentBossDps += damage;
			}
		}
		if (damage <= 0f)
		{
			return;
		}
		if (this.NextShotKills)
		{
			damage = 100000f;
		}
		if (damage > 0f && this.HasCrest)
		{
			this.HasCrest = false;
		}
		if (this.healthIsNumberOfHits)
		{
			damage = 1f;
		}
		if (!this.NextDamageIgnoresArmor && !this.NextShotKills)
		{
			bool flag = this.Armor > 0f;
			if (flag)
			{
				this.Armor -= 1f;
				damage = 0f;
				if (this.isPlayerCharacter)
				{
					this.m_player.OnLostArmor();
				}
			}
		}
		this.NextDamageIgnoresArmor = false;
		float num = damage;
		if (num > 999f)
		{
			num = 0f;
		}
		num = Mathf.Min(this.currentHealth, num);
		if (this.TrackPixelColliderDamage)
		{
			if (hitPixelCollider != null)
			{
				float num2;
				if (this.PixelColliderDamage.TryGetValue(hitPixelCollider, out num2))
				{
					this.PixelColliderDamage[hitPixelCollider] = num2 + damage;
				}
			}
			else if (damage <= 999f)
			{
				float num3 = damage * this.GlobalPixelColliderDamageMultiplier;
				List<PixelCollider> list = new List<PixelCollider>(this.PixelColliderDamage.Keys);
				for (int i = 0; i < list.Count; i++)
				{
					PixelCollider pixelCollider = list[i];
					Dictionary<PixelCollider, float> pixelColliderDamage;
					PixelCollider pixelCollider2;
					(pixelColliderDamage = this.PixelColliderDamage)[pixelCollider2 = pixelCollider] = pixelColliderDamage[pixelCollider2] + num3;
				}
			}
		}
		this.currentHealth -= damage;
		if (this.isPlayerCharacter)
		{
			UnityEngine.Debug.Log(this.currentHealth + "||" + damage);
		}
		if (this.quantizeHealth)
		{
			this.currentHealth = BraveMathCollege.QuantizeFloat(this.currentHealth, this.quantizedIncrement);
		}
		this.currentHealth = Mathf.Clamp(this.currentHealth, this.minimumHealth, this.AdjustedMaxHealth);
		if (!this.isPlayerCharacter)
		{
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				GameManager.Instance.AllPlayers[j].OnAnyEnemyTookAnyDamage(num, this.currentHealth <= 0f && this.Armor <= 0f, this);
			}
			if (!string.IsNullOrEmpty(damageSource))
			{
				if (damageSource == "primaryplayer" || damageSource == "Player ID 0")
				{
					GameManager.Instance.PrimaryPlayer.OnDidDamage(damage, this.currentHealth <= 0f && this.Armor <= 0f, this);
				}
				else if (damageSource == "secondaryplayer" || damageSource == "Player ID 1")
				{
					GameManager.Instance.SecondaryPlayer.OnDidDamage(damage, this.currentHealth <= 0f && this.Armor <= 0f, this);
				}
			}
		}
		if (this.flashesOnDamage && base.spriteAnimator != null && !this.m_isFlashing)
		{
			if (this.m_flashOnHitCoroutine != null)
			{
				base.StopCoroutine(this.m_flashOnHitCoroutine);
			}
			this.m_flashOnHitCoroutine = null;
			if (this.materialsToFlash == null)
			{
				this.materialsToFlash = new List<Material>();
				this.outlineMaterialsToFlash = new List<Material>();
				this.sourceColors = new List<Color>();
			}
			if (base.gameActor)
			{
				for (int k = 0; k < this.materialsToFlash.Count; k++)
				{
					this.materialsToFlash[k].SetColor("_OverrideColor", base.gameActor.CurrentOverrideColor);
				}
			}
			if (this.outlineMaterialsToFlash != null)
			{
				for (int l = 0; l < this.outlineMaterialsToFlash.Count; l++)
				{
					if (l >= this.sourceColors.Count)
					{
						UnityEngine.Debug.LogError("NOT ENOUGH SOURCE COLORS");
						break;
					}
					this.outlineMaterialsToFlash[l].SetColor("_OverrideColor", this.sourceColors[l]);
				}
			}
			this.m_flashOnHitCoroutine = base.StartCoroutine(this.FlashOnHit(damageCategory, hitPixelCollider));
		}
		if (this.incorporealityOnDamage && !this.m_isIncorporeal)
		{
			base.StartCoroutine("IncorporealityOnHit");
		}
		this.lastIncurredDamageSource = damageSource;
		this.lastIncurredDamageDirection = direction;
		if (this.shakesCameraOnDamage)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(this.cameraShakeOnDamage, new Vector2?(base.specRigidbody.UnitCenter), false);
		}
		if (this.NextShotKills)
		{
			this.Armor = 0f;
		}
		if (this.OnDamaged != null)
		{
			this.OnDamaged(this.currentHealth, this.AdjustedMaxHealth, damageTypes, damageCategory, direction);
		}
		if (this.OnHealthChanged != null)
		{
			this.OnHealthChanged(this.currentHealth, this.AdjustedMaxHealth);
		}
		if (this.currentHealth == 0f && this.Armor == 0f)
		{
			this.NextShotKills = false;
			if (!this.SuppressDeathSounds)
			{
				AkSoundEngine.PostEvent("Play_ENM_death", base.gameObject);
				AkSoundEngine.PostEvent(string.IsNullOrEmpty(this.overrideDeathAudioEvent) ? "Play_CHR_general_death_01" : this.overrideDeathAudioEvent, base.gameObject);
			}
			this.Die(direction);
		}
		else if (this.usesInvulnerabilityPeriod)
		{
			base.StartCoroutine(this.HandleInvulnerablePeriod(-1f));
		}
		if (damageCategory == DamageCategory.Normal || damageCategory == DamageCategory.Collision)
		{
			if (this.currentHealth <= 0f && this.Armor <= 0f)
			{
				if (!this.DisableStickyFriction)
				{
					StickyFrictionManager.Instance.RegisterDeathStickyFriction();
				}
			}
			else if (this.isPlayerCharacter)
			{
				StickyFrictionManager.Instance.RegisterPlayerDamageStickyFriction(damage);
			}
			else
			{
				StickyFrictionManager.Instance.RegisterOtherDamageStickyFriction(damage);
			}
		}
	}

	// Token: 0x06005DB4 RID: 23988 RVA: 0x0023EC28 File Offset: 0x0023CE28
	public void Die(Vector2 finalDamageDirection)
	{
		this.EndFlashEffects();
		bool flag = false;
		if (this.spawnBulletScript && (!base.gameActor || !base.gameActor.IsFalling) && (this.chanceToSpawnBulletScript >= 1f || UnityEngine.Random.value < this.chanceToSpawnBulletScript))
		{
			flag = true;
			if (this.noCorpseWhenBulletScriptDeath)
			{
				base.aiActor.CorpseObject = null;
			}
			if (this.bulletScriptType == HealthHaver.BulletScriptType.OnAnimEvent)
			{
				tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
				spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.BulletScriptEventTriggered));
			}
		}
		if (this.OnPreDeath != null)
		{
			this.OnPreDeath(finalDamageDirection);
		}
		if (flag && this.bulletScriptType == HealthHaver.BulletScriptType.OnPreDeath)
		{
			SpawnManager.SpawnBulletScript(base.aiActor, this.bulletScript, null, null, false, null);
		}
		if (this.GetCurrentHealth() > 0f || this.Armor > 0f)
		{
			return;
		}
		this.IsVulnerable = false;
		if (this.deathEffect != null)
		{
			SpawnManager.SpawnVFX(this.deathEffect, base.transform.position, Quaternion.identity);
		}
		if (this.IsBoss)
		{
			this.EndBossState(true);
		}
		if (this.ManualDeathHandling)
		{
			return;
		}
		if (base.spriteAnimator != null)
		{
			string text = ((!flag || string.IsNullOrEmpty(this.overrideDeathAnimBulletScript)) ? this.overrideDeathAnimation : this.overrideDeathAnimBulletScript);
			if (!string.IsNullOrEmpty(text))
			{
				tk2dSpriteAnimationClip tk2dSpriteAnimationClip;
				if (base.aiAnimator != null)
				{
					base.aiAnimator.PlayUntilCancelled(text, false, null, -1f, false);
					tk2dSpriteAnimationClip = base.spriteAnimator.CurrentClip;
				}
				else
				{
					tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(this.overrideDeathAnimation);
					if (tk2dSpriteAnimationClip != null)
					{
						base.spriteAnimator.Play(tk2dSpriteAnimationClip);
					}
				}
				if (tk2dSpriteAnimationClip != null && !this.isPlayerCharacter && (!base.gameActor || !base.gameActor.IsFalling))
				{
					tk2dSpriteAnimator spriteAnimator2 = base.spriteAnimator;
					spriteAnimator2.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator2.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.DeathEventTriggered));
					tk2dSpriteAnimator spriteAnimator3 = base.spriteAnimator;
					spriteAnimator3.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator3.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DeathAnimationComplete));
					return;
				}
			}
			else
			{
				if (base.aiAnimator != null)
				{
					base.aiAnimator.enabled = false;
				}
				float num = finalDamageDirection.ToAngle();
				tk2dSpriteAnimationClip tk2dSpriteAnimationClip2;
				if (base.aiAnimator != null && base.aiAnimator.HasDirectionalAnimation("death"))
				{
					if (!base.aiAnimator.LockFacingDirection)
					{
						base.aiAnimator.LockFacingDirection = true;
						base.aiAnimator.FacingDirection = (num + 180f) % 360f;
					}
					base.aiAnimator.PlayUntilCancelled("death", false, null, -1f, false);
					tk2dSpriteAnimationClip2 = base.spriteAnimator.CurrentClip;
				}
				else if (base.gameActor && base.gameActor is PlayerSpaceshipController)
				{
					Exploder.DoDefaultExplosion(base.gameActor.CenterPosition, Vector2.zero, null, false, CoreDamageTypes.None, false);
					tk2dSpriteAnimationClip2 = null;
				}
				else
				{
					tk2dSpriteAnimationClip2 = this.GetDeathClip(BraveMathCollege.ClampAngle360(num + 22.5f));
					if (tk2dSpriteAnimationClip2 != null)
					{
						base.spriteAnimator.Play(tk2dSpriteAnimationClip2);
					}
				}
				if (tk2dSpriteAnimationClip2 != null && !this.isPlayerCharacter && (!base.gameActor || !base.gameActor.IsFalling))
				{
					tk2dSpriteAnimator spriteAnimator4 = base.spriteAnimator;
					spriteAnimator4.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator4.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.DeathEventTriggered));
					tk2dSpriteAnimator spriteAnimator5 = base.spriteAnimator;
					spriteAnimator5.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator5.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DeathAnimationComplete));
					return;
				}
			}
		}
		if (this.spawnBulletScript && this.bulletScriptType == HealthHaver.BulletScriptType.OnDeath && (!base.gameActor || !base.gameActor.IsFalling))
		{
			SpawnManager.SpawnBulletScript(base.aiActor, this.bulletScript, null, null, false, null);
		}
		this.FinalizeDeath();
	}

	// Token: 0x06005DB5 RID: 23989 RVA: 0x0023F0B8 File Offset: 0x0023D2B8
	public void EndBossState(bool triggerKillCam)
	{
		bool flag = false;
		List<AIActor> activeEnemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			HealthHaver healthHaver = activeEnemies[i].healthHaver;
			if (healthHaver && healthHaver.IsBoss && healthHaver.IsAlive && activeEnemies[i] != base.aiActor)
			{
				flag = true;
				break;
			}
		}
		if (this.HasHealthBar)
		{
			GameUIBossHealthController gameUIBossHealthController = ((!base.healthHaver.UsesVerticalBossBar) ? (base.healthHaver.UsesSecondaryBossBar ? GameUIRoot.Instance.bossController2 : GameUIRoot.Instance.bossController) : GameUIRoot.Instance.bossControllerSide);
			gameUIBossHealthController.DeregisterBossHealthHaver(this);
		}
		if (!flag)
		{
			if (triggerKillCam)
			{
				GameUIRoot.Instance.TriggerBossKillCam(null, base.specRigidbody);
			}
			if (this.HasHealthBar)
			{
				GameUIRoot.Instance.bossController.DisableBossHealth();
				GameUIRoot.Instance.bossController2.DisableBossHealth();
				GameUIRoot.Instance.bossControllerSide.DisableBossHealth();
			}
			if (triggerKillCam)
			{
				if (!this.forcePreventVictoryMusic)
				{
					GameManager.Instance.DungeonMusicController.EndBossMusic();
				}
			}
			else
			{
				GameManager.Instance.DungeonMusicController.EndBossMusicNoVictory();
			}
		}
	}

	// Token: 0x06005DB6 RID: 23990 RVA: 0x0023F224 File Offset: 0x0023D424
	public tk2dSpriteAnimationClip GetDeathClip(float damageAngle)
	{
		if (!base.spriteAnimator)
		{
			return null;
		}
		int num = Mathf.FloorToInt(BraveMathCollege.ClampAngle360(damageAngle) / 45f);
		num = Mathf.Max(0, Mathf.Min(num, 7));
		string[] array = new string[] { "die_right", "die_back_right", "die_back", "die_back_left", "die_left", "die_front_left", "die_front", "die_front_right" };
		string text = array[num];
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(text);
		if (tk2dSpriteAnimationClip == null)
		{
			if (num == 7 || num == 0 || num == 1 || num == 2)
			{
				tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName("die_right");
			}
			else
			{
				tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName("die_left");
			}
		}
		if (tk2dSpriteAnimationClip == null)
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName("death");
		}
		if (tk2dSpriteAnimationClip == null)
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName("die");
		}
		if (this.isPlayerCharacter && this.m_player.hasArmorlessAnimations && this.m_player.healthHaver.Armor == 0f)
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName("death_armorless");
		}
		return tk2dSpriteAnimationClip;
	}

	// Token: 0x06005DB7 RID: 23991 RVA: 0x0023F378 File Offset: 0x0023D578
	public void EndFlashEffects()
	{
		if (this.m_flashOnHitCoroutine != null)
		{
			base.StopCoroutine(this.m_flashOnHitCoroutine);
		}
		this.m_flashOnHitCoroutine = null;
		this.EndFlashOnHit();
		base.StopCoroutine("IncorporealityOnHit");
		this.EndIncorporealityOnHit();
	}

	// Token: 0x06005DB8 RID: 23992 RVA: 0x0023F3B0 File Offset: 0x0023D5B0
	private void BulletScriptEventTriggered(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
		if (frame.eventInfo == "fire")
		{
			SpawnManager.SpawnBulletScript(base.aiActor, this.bulletScript, null, null, false, null);
		}
	}

	// Token: 0x06005DB9 RID: 23993 RVA: 0x0023F400 File Offset: 0x0023D600
	public void UpdateCachedOutlineColor(Material m, Color c)
	{
		if (this.outlineMaterialsToFlash != null && this.outlineMaterialsToFlash.Contains(m))
		{
			int num = this.outlineMaterialsToFlash.IndexOf(m);
			if (this.sourceColors != null && this.sourceColors.Count > num && num >= 0)
			{
				this.sourceColors[num] = c;
			}
		}
	}

	// Token: 0x06005DBA RID: 23994 RVA: 0x0023F468 File Offset: 0x0023D668
	private IEnumerator FlashOnHit(DamageCategory sourceDamageCategory, PixelCollider hitPixelCollider)
	{
		if (this.currentHealth <= 0f && this.Armor <= 0f)
		{
			this.m_flashOnHitCoroutine = null;
			yield break;
		}
		this.m_isFlashing = true;
		if (this.isPlayerCharacter || sourceDamageCategory != DamageCategory.DamageOverTime)
		{
			AkSoundEngine.PostEvent("Play_CHR_general_hurt_01", base.gameObject);
		}
		else if (HealthHaver.m_hitBarkLimiter % 2 == 0)
		{
			AkSoundEngine.PostEvent("Play_CHR_general_hurt_01", base.gameObject);
		}
		HealthHaver.m_hitBarkLimiter++;
		if (this.isPlayerCharacter || sourceDamageCategory != DamageCategory.DamageOverTime)
		{
			AkSoundEngine.PostEvent("Play_ENM_hurt", base.gameObject);
		}
		if (base.gameActor)
		{
			base.gameActor.OverrideColorOverridden = true;
		}
		Color overrideColor = Color.white;
		overrideColor.a = 1f;
		if (base.sprite)
		{
			base.sprite.usesOverrideMaterial = true;
		}
		if (this.materialsToEnableBrightnessClampOn == null)
		{
			this.materialsToEnableBrightnessClampOn = new List<Material>();
		}
		else
		{
			this.materialsToEnableBrightnessClampOn.Clear();
		}
		List<tk2dBaseSprite> spritesToFlash = this.bodySprites;
		tk2dBaseSprite tk2dBaseSprite;
		if (this.m_independentDamageFlashers != null && hitPixelCollider != null && this.m_independentDamageFlashers.TryGetValue(hitPixelCollider, out tk2dBaseSprite))
		{
			spritesToFlash = new List<tk2dBaseSprite>(1);
			spritesToFlash.Add(tk2dBaseSprite);
		}
		this.materialsToFlash.Clear();
		this.outlineMaterialsToFlash.Clear();
		for (int i = 0; i < spritesToFlash.Count; i++)
		{
			Material material = spritesToFlash[i].renderer.material;
			this.materialsToFlash.Add(material);
			for (int j = 0; j < material.shaderKeywords.Length; j++)
			{
				if (material.shaderKeywords[j] == "BRIGHTNESS_CLAMP_ON")
				{
					material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
					material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
					this.materialsToEnableBrightnessClampOn.Add(material);
					break;
				}
			}
			tk2dSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(spritesToFlash[i]);
			for (int k = 0; k < outlineSprites.Length; k++)
			{
				if (outlineSprites[k])
				{
					if (outlineSprites[k].renderer)
					{
						if (outlineSprites[k].renderer.material)
						{
							this.outlineMaterialsToFlash.Add(outlineSprites[k].renderer.material);
						}
					}
				}
			}
		}
		this.sourceColors.Clear();
		for (int l = 0; l < this.materialsToFlash.Count; l++)
		{
			this.materialsToFlash[l].SetColor("_OverrideColor", overrideColor);
		}
		for (int m = 0; m < this.outlineMaterialsToFlash.Count; m++)
		{
			this.sourceColors.Add(this.outlineMaterialsToFlash[m].GetColor("_OverrideColor"));
			this.outlineMaterialsToFlash[m].SetColor("_OverrideColor", overrideColor);
		}
		float elapsed = 0f;
		while (elapsed < 0.04f && (this.currentHealth > 0f || this.Armor > 0f))
		{
			float t = 1f - elapsed / 0.04f;
			if (this.currentHealth > 0f || this.Armor > 0f)
			{
				if (base.gameActor)
				{
					for (int n = 0; n < this.materialsToFlash.Count; n++)
					{
						this.materialsToFlash[n].SetColor("_OverrideColor", Color.Lerp(base.gameActor.CurrentOverrideColor, overrideColor, t));
					}
				}
				for (int num = 0; num < this.outlineMaterialsToFlash.Count; num++)
				{
					this.outlineMaterialsToFlash[num].SetColor("_OverrideColor", Color.Lerp(this.sourceColors[num], overrideColor, t));
				}
			}
			yield return null;
			elapsed += BraveTime.DeltaTime;
		}
		if (base.gameActor)
		{
			base.gameActor.OverrideColorOverridden = false;
			for (int num2 = 0; num2 < this.materialsToFlash.Count; num2++)
			{
				this.materialsToFlash[num2].SetColor("_OverrideColor", base.gameActor.CurrentOverrideColor);
			}
		}
		for (int num3 = 0; num3 < this.outlineMaterialsToFlash.Count; num3++)
		{
			this.outlineMaterialsToFlash[num3].SetColor("_OverrideColor", this.sourceColors[num3]);
		}
		for (int num4 = 0; num4 < this.materialsToEnableBrightnessClampOn.Count; num4++)
		{
			this.materialsToEnableBrightnessClampOn[num4].DisableKeyword("BRIGHTNESS_CLAMP_OFF");
			this.materialsToEnableBrightnessClampOn[num4].EnableKeyword("BRIGHTNESS_CLAMP_ON");
		}
		yield return new WaitForSeconds(0.2f);
		this.m_flashOnHitCoroutine = null;
		this.m_isFlashing = false;
		yield break;
	}

	// Token: 0x06005DBB RID: 23995 RVA: 0x0023F494 File Offset: 0x0023D694
	private void EndFlashOnHit()
	{
		if (this.m_flashOnHitCoroutine != null)
		{
			base.StopCoroutine(this.m_flashOnHitCoroutine);
		}
		this.m_flashOnHitCoroutine = null;
		if (base.gameActor)
		{
			base.gameActor.OverrideColorOverridden = false;
		}
		if (this.materialsToFlash != null && this.materialsToFlash.Count > 0 && base.gameActor)
		{
			for (int i = 0; i < this.materialsToFlash.Count; i++)
			{
				this.materialsToFlash[i].SetColor("_OverrideColor", base.gameActor.CurrentOverrideColor);
			}
		}
		if (this.outlineMaterialsToFlash != null && this.outlineMaterialsToFlash.Count > 0)
		{
			for (int j = 0; j < this.outlineMaterialsToFlash.Count; j++)
			{
				this.outlineMaterialsToFlash[j].SetColor("_OverrideColor", this.sourceColors[j]);
			}
		}
		if (this.materialsToEnableBrightnessClampOn != null && this.materialsToEnableBrightnessClampOn.Count > 0)
		{
			for (int k = 0; k < this.materialsToEnableBrightnessClampOn.Count; k++)
			{
				this.materialsToEnableBrightnessClampOn[k].DisableKeyword("BRIGHTNESS_CLAMP_OFF");
				this.materialsToEnableBrightnessClampOn[k].EnableKeyword("BRIGHTNESS_CLAMP_ON");
			}
		}
		this.m_isFlashing = false;
	}

	// Token: 0x06005DBC RID: 23996 RVA: 0x0023F60C File Offset: 0x0023D80C
	private IEnumerator IncorporealityOnHit()
	{
		this.m_isIncorporeal = true;
		if (!this.isPlayerCharacter)
		{
			UnityEngine.Debug.LogError("Incorporeality is currently only supported on the player.", this);
		}
		PlayerController player = base.GetComponent<PlayerController>();
		if (player == null)
		{
			UnityEngine.Debug.LogError("Failed to incorporeal something...");
			yield break;
		}
		int enemyMask = CollisionMask.LayerToMask(CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox, CollisionLayer.Projectile);
		player.specRigidbody.AddCollisionLayerIgnoreOverride(enemyMask);
		yield return null;
		float timer = 0f;
		float subtimer = 0f;
		while (timer < this.incorporealityTime)
		{
			while (timer < this.incorporealityTime)
			{
				timer += BraveTime.DeltaTime;
				subtimer += BraveTime.DeltaTime;
				if (subtimer > 0.12f)
				{
					player.IsVisible = false;
					subtimer -= 0.12f;
					break;
				}
				yield return null;
			}
			while (timer < this.incorporealityTime)
			{
				timer += BraveTime.DeltaTime;
				subtimer += BraveTime.DeltaTime;
				if (subtimer > 0.12f)
				{
					player.IsVisible = true;
					subtimer -= 0.12f;
					break;
				}
				yield return null;
			}
		}
		this.EndIncorporealityOnHit();
		yield break;
	}

	// Token: 0x06005DBD RID: 23997 RVA: 0x0023F628 File Offset: 0x0023D828
	private void EndIncorporealityOnHit()
	{
		PlayerController component = base.GetComponent<PlayerController>();
		if (component == null)
		{
			return;
		}
		int num = CollisionMask.LayerToMask(CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox, CollisionLayer.Projectile);
		component.IsVisible = true;
		component.specRigidbody.RemoveCollisionLayerIgnoreOverride(num);
		this.m_isIncorporeal = false;
	}

	// Token: 0x06005DBE RID: 23998 RVA: 0x0023F66C File Offset: 0x0023D86C
	private void DeathEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (frame.eventInfo == "disableColliders")
		{
			for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
			{
				base.specRigidbody.PixelColliders[i].Enabled = false;
			}
		}
	}

	// Token: 0x06005DBF RID: 23999 RVA: 0x0023F6D0 File Offset: 0x0023D8D0
	public void DeathAnimationComplete(tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip)
	{
		this.FinalizeDeath();
	}

	// Token: 0x06005DC0 RID: 24000 RVA: 0x0023F6D8 File Offset: 0x0023D8D8
	private void FinalizeDeath()
	{
		if (this.OnDeath != null)
		{
			this.OnDeath(this.lastIncurredDamageDirection);
		}
		if (base.aiActor && base.aiActor.IsFalling && !base.aiActor.HasSplashed)
		{
			GameManager.Instance.Dungeon.tileIndices.DoSplashAtPosition(base.sprite.WorldCenter);
			base.aiActor.HasSplashed = true;
		}
		if (GameManager.Instance.InTutorial && !this.isPlayerCharacter)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("enemyKilled");
		}
		if (!this.persistsOnDeath)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06005DC1 RID: 24001 RVA: 0x0023F798 File Offset: 0x0023D998
	public void TriggerInvulnerabilityPeriod(float overrideTime = -1f)
	{
		if (this.usesInvulnerabilityPeriod)
		{
			base.StartCoroutine(this.HandleInvulnerablePeriod(overrideTime));
		}
	}

	// Token: 0x06005DC2 RID: 24002 RVA: 0x0023F7B4 File Offset: 0x0023D9B4
	protected IEnumerator HandleInvulnerablePeriod(float overrideTime = -1f)
	{
		this.vulnerable = false;
		if (this.useFortunesFavorInvulnerability && base.ultraFortunesFavor)
		{
			base.ultraFortunesFavor.enabled = true;
		}
		if (overrideTime <= 0f)
		{
			yield return new WaitForSeconds(this.invulnerabilityPeriod);
		}
		else
		{
			yield return new WaitForSeconds(overrideTime);
		}
		this.vulnerable = true;
		if (this.useFortunesFavorInvulnerability && base.ultraFortunesFavor)
		{
			base.ultraFortunesFavor.enabled = false;
		}
		yield break;
	}

	// Token: 0x06005DC3 RID: 24003 RVA: 0x0023F7D8 File Offset: 0x0023D9D8
	public void ApplyDamageModifiers(List<DamageTypeModifier> newDamageTypeModifiers)
	{
		for (int i = 0; i < newDamageTypeModifiers.Count; i++)
		{
			DamageTypeModifier damageTypeModifier = newDamageTypeModifiers[i];
			bool flag = false;
			for (int j = 0; j < this.damageTypeModifiers.Count; j++)
			{
				DamageTypeModifier damageTypeModifier2 = this.damageTypeModifiers[j];
				if (damageTypeModifier.damageType == damageTypeModifier2.damageType)
				{
					damageTypeModifier2.damageMultiplier = damageTypeModifier.damageMultiplier;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.damageTypeModifiers.Add(new DamageTypeModifier(damageTypeModifier));
			}
		}
	}

	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x06005DC4 RID: 24004 RVA: 0x0023F870 File Offset: 0x0023DA70
	public int NumBodyRigidbodies
	{
		get
		{
			if (this.bodyRigidbodies != null)
			{
				return this.bodyRigidbodies.Count;
			}
			if (base.specRigidbody)
			{
				return 1;
			}
			return 0;
		}
	}

	// Token: 0x06005DC5 RID: 24005 RVA: 0x0023F89C File Offset: 0x0023DA9C
	public SpeculativeRigidbody GetBodyRigidbody(int index)
	{
		if (this.bodyRigidbodies != null)
		{
			return this.bodyRigidbodies[index];
		}
		return base.specRigidbody;
	}

	// Token: 0x04005779 RID: 22393
	protected const float c_flashTime = 0.04f;

	// Token: 0x0400577A RID: 22394
	protected const float c_flashDowntime = 0.2f;

	// Token: 0x0400577B RID: 22395
	protected const float c_incorporealityFlashOnTime = 0.12f;

	// Token: 0x0400577C RID: 22396
	protected const float c_incorporealityFlashOffTime = 0.12f;

	// Token: 0x0400577D RID: 22397
	protected const float c_bossDpsCapWindow = 3f;

	// Token: 0x04005781 RID: 22401
	public Action<HealthHaver, HealthHaver.ModifyDamageEventArgs> ModifyDamage;

	// Token: 0x04005782 RID: 22402
	public Action<HealthHaver, HealthHaver.ModifyHealingEventArgs> ModifyHealing;

	// Token: 0x04005784 RID: 22404
	[TogglesProperty("quantizedIncrement", null)]
	public bool quantizeHealth;

	// Token: 0x04005785 RID: 22405
	[HideInInspector]
	public float quantizedIncrement = 0.5f;

	// Token: 0x04005786 RID: 22406
	public bool flashesOnDamage = true;

	// Token: 0x04005787 RID: 22407
	[TogglesProperty("incorporealityTime", "Incorporeality Period")]
	public bool incorporealityOnDamage;

	// Token: 0x04005788 RID: 22408
	[HideInInspector]
	public float incorporealityTime = 1f;

	// Token: 0x04005789 RID: 22409
	public bool PreventAllDamage;

	// Token: 0x0400578A RID: 22410
	[HideInInspector]
	public bool persistsOnDeath;

	// Token: 0x0400578B RID: 22411
	[NonSerialized]
	protected float m_curseHealthMaximum = float.MaxValue;

	// Token: 0x0400578C RID: 22412
	[NonSerialized]
	public bool HasCrest;

	// Token: 0x0400578D RID: 22413
	[NonSerialized]
	public bool HasRatchetHealthBar;

	// Token: 0x0400578E RID: 22414
	[SerializeField]
	protected float maximumHealth = 10f;

	// Token: 0x0400578F RID: 22415
	[HideInInspector]
	[SerializeField]
	protected float currentHealth = 10f;

	// Token: 0x04005790 RID: 22416
	[SerializeField]
	protected float currentArmor;

	// Token: 0x04005791 RID: 22417
	[SerializeField]
	[TogglesProperty("invulnerabilityPeriod", "Invulnerability Period")]
	protected bool usesInvulnerabilityPeriod;

	// Token: 0x04005792 RID: 22418
	[HideInInspector]
	[SerializeField]
	protected float invulnerabilityPeriod = 0.5f;

	// Token: 0x04005793 RID: 22419
	[ShowInInspectorIf("usesInvulnerabilityPeriod", true)]
	public bool useFortunesFavorInvulnerability;

	// Token: 0x04005794 RID: 22420
	public GameObject deathEffect;

	// Token: 0x04005795 RID: 22421
	public string damagedAudioEvent = string.Empty;

	// Token: 0x04005796 RID: 22422
	public string overrideDeathAudioEvent = string.Empty;

	// Token: 0x04005797 RID: 22423
	public string overrideDeathAnimation = string.Empty;

	// Token: 0x04005798 RID: 22424
	[Space(5f)]
	public bool shakesCameraOnDamage;

	// Token: 0x04005799 RID: 22425
	[ShowInInspectorIf("shakesCameraOnDamage", false)]
	public ScreenShakeSettings cameraShakeOnDamage;

	// Token: 0x0400579A RID: 22426
	[Header("Damage Overrides")]
	public List<DamageTypeModifier> damageTypeModifiers;

	// Token: 0x0400579B RID: 22427
	public bool healthIsNumberOfHits;

	// Token: 0x0400579C RID: 22428
	public bool OnlyAllowSpecialBossDamage;

	// Token: 0x0400579D RID: 22429
	[Header("BulletScript")]
	[FormerlySerializedAs("spawnsBulletMl")]
	public bool spawnBulletScript;

	// Token: 0x0400579E RID: 22430
	[FormerlySerializedAs("chanceToSpawnBulletMl")]
	[ShowInInspectorIf("spawnBulletScript", true)]
	public float chanceToSpawnBulletScript;

	// Token: 0x0400579F RID: 22431
	[FormerlySerializedAs("overrideDeathAnimBulletMl")]
	[ShowInInspectorIf("spawnBulletScript", true)]
	public string overrideDeathAnimBulletScript;

	// Token: 0x040057A0 RID: 22432
	[ShowInInspectorIf("spawnBulletScript", true)]
	[FormerlySerializedAs("noCorpseWhenBulletMlDeath")]
	public bool noCorpseWhenBulletScriptDeath;

	// Token: 0x040057A1 RID: 22433
	[FormerlySerializedAs("bulletMlType")]
	[ShowInInspectorIf("spawnBulletScript", true)]
	public HealthHaver.BulletScriptType bulletScriptType;

	// Token: 0x040057A2 RID: 22434
	public BulletScriptSelector bulletScript;

	// Token: 0x040057A3 RID: 22435
	[Header("For Bosses")]
	public HealthHaver.BossBarType bossHealthBar;

	// Token: 0x040057A4 RID: 22436
	public string overrideBossName;

	// Token: 0x040057A5 RID: 22437
	public bool forcePreventVictoryMusic;

	// Token: 0x040057A6 RID: 22438
	[NonSerialized]
	public string lastIncurredDamageSource;

	// Token: 0x040057A7 RID: 22439
	[NonSerialized]
	public Vector2 lastIncurredDamageDirection;

	// Token: 0x040057A8 RID: 22440
	[NonSerialized]
	public bool NextShotKills;

	// Token: 0x040057A9 RID: 22441
	protected List<Material> materialsToFlash;

	// Token: 0x040057AA RID: 22442
	protected List<Material> outlineMaterialsToFlash;

	// Token: 0x040057AB RID: 22443
	protected List<Material> materialsToEnableBrightnessClampOn;

	// Token: 0x040057AC RID: 22444
	protected List<Color> sourceColors;

	// Token: 0x040057AD RID: 22445
	protected bool isPlayerCharacter;

	// Token: 0x040057AE RID: 22446
	private bool m_isFlashing;

	// Token: 0x040057AF RID: 22447
	private bool m_isIncorporeal;

	// Token: 0x040057B0 RID: 22448
	private float m_damageCap = -1f;

	// Token: 0x040057B1 RID: 22449
	private float m_bossDpsCap = -1f;

	// Token: 0x040057B2 RID: 22450
	private float m_recentBossDps;

	// Token: 0x040057B3 RID: 22451
	private PlayerController m_player;

	// Token: 0x040057B4 RID: 22452
	[NonSerialized]
	public float minimumHealth;

	// Token: 0x040057B5 RID: 22453
	[NonSerialized]
	public List<tk2dBaseSprite> bodySprites = new List<tk2dBaseSprite>();

	// Token: 0x040057B6 RID: 22454
	[NonSerialized]
	public List<SpeculativeRigidbody> bodyRigidbodies;

	// Token: 0x040057B7 RID: 22455
	[NonSerialized]
	public float AllDamageMultiplier = 1f;

	// Token: 0x040057B8 RID: 22456
	[NonSerialized]
	private Dictionary<PixelCollider, tk2dBaseSprite> m_independentDamageFlashers;

	// Token: 0x040057B9 RID: 22457
	protected bool vulnerable = true;

	// Token: 0x040057C3 RID: 22467
	public float GlobalPixelColliderDamageMultiplier = 1f;

	// Token: 0x040057C5 RID: 22469
	[NonSerialized]
	public bool NextDamageIgnoresArmor;

	// Token: 0x040057C6 RID: 22470
	private bool isFirstFrame = true;

	// Token: 0x040057C8 RID: 22472
	private static int m_hitBarkLimiter;

	// Token: 0x040057C9 RID: 22473
	private Coroutine m_flashOnHitCoroutine;

	// Token: 0x02001099 RID: 4249
	public class ModifyDamageEventArgs : EventArgs
	{
		// Token: 0x040057CA RID: 22474
		public float InitialDamage;

		// Token: 0x040057CB RID: 22475
		public float ModifiedDamage;
	}

	// Token: 0x0200109A RID: 4250
	public class ModifyHealingEventArgs : EventArgs
	{
		// Token: 0x040057CC RID: 22476
		public float InitialHealing;

		// Token: 0x040057CD RID: 22477
		public float ModifiedHealing;
	}

	// Token: 0x0200109B RID: 4251
	public enum BossBarType
	{
		// Token: 0x040057CF RID: 22479
		None,
		// Token: 0x040057D0 RID: 22480
		MainBar,
		// Token: 0x040057D1 RID: 22481
		SecondaryBar,
		// Token: 0x040057D2 RID: 22482
		CombinedBar,
		// Token: 0x040057D3 RID: 22483
		SecretBar,
		// Token: 0x040057D4 RID: 22484
		VerticalBar,
		// Token: 0x040057D5 RID: 22485
		SubbossBar
	}

	// Token: 0x0200109C RID: 4252
	// (Invoke) Token: 0x06005DCA RID: 24010
	public delegate void OnDamagedEvent(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection);

	// Token: 0x0200109D RID: 4253
	// (Invoke) Token: 0x06005DCE RID: 24014
	public delegate void OnHealthChangedEvent(float resultValue, float maxValue);

	// Token: 0x0200109E RID: 4254
	public enum BulletScriptType
	{
		// Token: 0x040057D7 RID: 22487
		OnPreDeath,
		// Token: 0x040057D8 RID: 22488
		OnDeath,
		// Token: 0x040057D9 RID: 22489
		OnAnimEvent
	}
}
