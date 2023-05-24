using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Dungeonator;
using UnityEngine;

// Token: 0x020012E6 RID: 4838
public abstract class GameActor : DungeonPlaceableBehaviour, IAutoAimTarget
{
	// Token: 0x17001015 RID: 4117
	// (get) Token: 0x06006C5C RID: 27740
	public abstract Gun CurrentGun { get; }

	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x06006C5D RID: 27741
	public abstract Transform GunPivot { get; }

	// Token: 0x17001017 RID: 4119
	// (get) Token: 0x06006C5E RID: 27742 RVA: 0x002AA390 File Offset: 0x002A8590
	public virtual Transform SecondaryGunPivot
	{
		get
		{
			return this.GunPivot;
		}
	}

	// Token: 0x17001018 RID: 4120
	// (get) Token: 0x06006C5F RID: 27743
	public abstract bool SpriteFlipped { get; }

	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x06006C60 RID: 27744
	public abstract Vector3 SpriteDimensions { get; }

	// Token: 0x1700101A RID: 4122
	// (get) Token: 0x06006C61 RID: 27745 RVA: 0x002AA398 File Offset: 0x002A8598
	public List<MovingPlatform> SupportingPlatforms
	{
		get
		{
			return this.m_supportingPlatforms;
		}
	}

	// Token: 0x1700101B RID: 4123
	// (get) Token: 0x06006C62 RID: 27746 RVA: 0x002AA3A0 File Offset: 0x002A85A0
	public Vector2 CenterPosition
	{
		get
		{
			if (base.specRigidbody && base.specRigidbody.HitboxPixelCollider != null)
			{
				return base.specRigidbody.HitboxPixelCollider.UnitCenter;
			}
			if (base.sprite)
			{
				return base.sprite.WorldCenter;
			}
			return base.transform.position.XY();
		}
	}

	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x06006C63 RID: 27747 RVA: 0x002AA40C File Offset: 0x002A860C
	public bool IsFalling
	{
		get
		{
			return this.m_isFalling;
		}
	}

	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x06006C64 RID: 27748 RVA: 0x002AA414 File Offset: 0x002A8614
	public virtual bool IsFlying
	{
		get
		{
			return this.m_isFlying.Value;
		}
	}

	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x06006C65 RID: 27749 RVA: 0x002AA424 File Offset: 0x002A8624
	public bool IsGrounded
	{
		get
		{
			return base.spriteAnimator.QueryGroundedFrame() && !this.IsFlying && !this.FallingProhibited;
		}
	}

	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x06006C66 RID: 27750 RVA: 0x002AA450 File Offset: 0x002A8650
	public bool IsStealthed
	{
		get
		{
			return this.m_isStealthed.Value;
		}
	}

	// Token: 0x06006C67 RID: 27751 RVA: 0x002AA460 File Offset: 0x002A8660
	public float GetResistanceForEffectType(EffectResistanceType resistType)
	{
		if (resistType == EffectResistanceType.None)
		{
			return 0f;
		}
		for (int i = 0; i < this.EffectResistances.Length; i++)
		{
			if (this.EffectResistances[i].resistType == resistType)
			{
				return this.EffectResistances[i].resistAmount;
			}
		}
		return 0f;
	}

	// Token: 0x06006C68 RID: 27752 RVA: 0x002AA4C0 File Offset: 0x002A86C0
	public void SetIsStealthed(bool value, string reason)
	{
		bool isStealthed = this.IsStealthed;
		this.m_isStealthed.SetOverride(reason, value, null);
		if (this.IsStealthed != isStealthed)
		{
			if (this.IsStealthed)
			{
				this.m_stealthVfx = this.PlayEffectOnActor(BraveResources.Load<GameObject>("Global VFX/VFX_Stealthed", ".prefab"), new Vector3(0f, 1.375f, 0f), true, true, false);
			}
			else if (this.m_stealthVfx)
			{
				UnityEngine.Object.Destroy(this.m_stealthVfx);
			}
		}
	}

	// Token: 0x06006C69 RID: 27753 RVA: 0x002AA554 File Offset: 0x002A8754
	public void SetIsFlying(bool value, string reason, bool adjustShadow = true, bool modifyPathing = false)
	{
		this.m_isFlying.SetOverride(reason, value, null);
		if (adjustShadow && this.HasShadow && this.ShadowObject)
		{
			if (value)
			{
				this.ShadowObject.transform.position = this.ShadowObject.transform.position + new Vector3(0f, -0.3f, 0f);
			}
			else
			{
				this.ShadowObject.transform.position = this.ShadowObject.transform.position + new Vector3(0f, 0.3f, 0f);
			}
		}
		base.specRigidbody.CanBeCarried = !this.m_isFlying.Value;
		AIActor aiactor = this as AIActor;
		if (modifyPathing && aiactor)
		{
			if (value)
			{
				aiactor.PathableTiles |= CellTypes.PIT;
			}
			else
			{
				aiactor.PathableTiles &= ~CellTypes.PIT;
			}
		}
	}

	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x06006C6A RID: 27754 RVA: 0x002AA670 File Offset: 0x002A8870
	protected virtual float DustUpMultiplier
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x06006C6B RID: 27755 RVA: 0x002AA678 File Offset: 0x002A8878
	public GoopDefinition CurrentGoop
	{
		get
		{
			return this.m_currentGoop;
		}
	}

	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x06006C6C RID: 27756 RVA: 0x002AA680 File Offset: 0x002A8880
	// (set) Token: 0x06006C6D RID: 27757 RVA: 0x002AA688 File Offset: 0x002A8888
	public float FreezeAmount { get; set; }

	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x06006C6E RID: 27758 RVA: 0x002AA694 File Offset: 0x002A8894
	// (set) Token: 0x06006C6F RID: 27759 RVA: 0x002AA69C File Offset: 0x002A889C
	public float CheeseAmount { get; set; }

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x06006C70 RID: 27760 RVA: 0x002AA6A8 File Offset: 0x002A88A8
	// (remove) Token: 0x06006C71 RID: 27761 RVA: 0x002AA6E0 File Offset: 0x002A88E0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GameActor.MovementModifier MovementModifiers;

	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x06006C72 RID: 27762 RVA: 0x002AA718 File Offset: 0x002A8918
	// (set) Token: 0x06006C73 RID: 27763 RVA: 0x002AA720 File Offset: 0x002A8920
	public bool StealthDeath { get; set; }

	// Token: 0x17001025 RID: 4133
	// (get) Token: 0x06006C74 RID: 27764 RVA: 0x002AA72C File Offset: 0x002A892C
	// (set) Token: 0x06006C75 RID: 27765 RVA: 0x002AA734 File Offset: 0x002A8934
	public bool IsFrozen { get; set; }

	// Token: 0x17001026 RID: 4134
	// (get) Token: 0x06006C76 RID: 27766 RVA: 0x002AA740 File Offset: 0x002A8940
	// (set) Token: 0x06006C77 RID: 27767 RVA: 0x002AA748 File Offset: 0x002A8948
	public bool IsCheezen { get; set; }

	// Token: 0x17001027 RID: 4135
	// (get) Token: 0x06006C78 RID: 27768 RVA: 0x002AA754 File Offset: 0x002A8954
	// (set) Token: 0x06006C79 RID: 27769 RVA: 0x002AA75C File Offset: 0x002A895C
	public bool IsGone { get; set; }

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x06006C7A RID: 27770 RVA: 0x002AA768 File Offset: 0x002A8968
	// (set) Token: 0x06006C7B RID: 27771 RVA: 0x002AA770 File Offset: 0x002A8970
	public bool SuppressEffectUpdates { get; set; }

	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x06006C7C RID: 27772 RVA: 0x002AA77C File Offset: 0x002A897C
	public float FacingDirection
	{
		get
		{
			if (base.aiAnimator)
			{
				return base.aiAnimator.FacingDirection;
			}
			if (base.gameActor is PlayerController)
			{
				PlayerController playerController = base.gameActor as PlayerController;
				return (playerController.unadjustedAimPoint.XY() - playerController.specRigidbody.GetUnitCenter(ColliderType.HitBox)).ToAngle();
			}
			return -90f;
		}
	}

	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x06006C7D RID: 27773 RVA: 0x002AA7E8 File Offset: 0x002A89E8
	// (set) Token: 0x06006C7E RID: 27774 RVA: 0x002AA7F0 File Offset: 0x002A89F0
	public bool PreventAutoAimVelocity { get; set; }

	// Token: 0x06006C7F RID: 27775 RVA: 0x002AA7FC File Offset: 0x002A89FC
	public virtual void Awake()
	{
		this.m_overrideColorID = Shader.PropertyToID("_OverrideColor");
		this.RegisterOverrideColor(new Color(1f, 1f, 1f, 0f), "base");
	}

	// Token: 0x06006C80 RID: 27776 RVA: 0x002AA834 File Offset: 0x002A8A34
	public virtual void Start()
	{
		if (base.specRigidbody)
		{
			base.specRigidbody.Initialize();
		}
	}

	// Token: 0x06006C81 RID: 27777 RVA: 0x002AA854 File Offset: 0x002A8A54
	public virtual void Update()
	{
		this.BeamStatusAmount = Mathf.Max(0f, this.BeamStatusAmount - BraveTime.DeltaTime / 2f);
		if (!this.SuppressEffectUpdates)
		{
			for (int i = 0; i < this.m_activeEffects.Count; i++)
			{
				GameActorEffect gameActorEffect = this.m_activeEffects[i];
				if (gameActorEffect != null)
				{
					if (this.m_activeEffectData != null && i < this.m_activeEffectData.Count)
					{
						RuntimeGameActorEffectData runtimeGameActorEffectData = this.m_activeEffectData[i];
						if (runtimeGameActorEffectData != null)
						{
							gameActorEffect.EffectTick(this, runtimeGameActorEffectData);
							if (runtimeGameActorEffectData.instanceOverheadVFX != null)
							{
								if (base.healthHaver && base.healthHaver.IsAlive && runtimeGameActorEffectData.instanceOverheadVFX)
								{
									Vector2 vector = base.transform.position.XY();
									if (gameActorEffect.PlaysVFXOnActor)
									{
										if (base.specRigidbody && base.specRigidbody.HitboxPixelCollider != null)
										{
											vector = base.specRigidbody.HitboxPixelCollider.UnitBottomCenter.Quantize(0.0625f);
										}
										runtimeGameActorEffectData.instanceOverheadVFX.transform.position = vector;
									}
									else
									{
										if (base.specRigidbody && base.specRigidbody.HitboxPixelCollider != null)
										{
											vector = base.specRigidbody.HitboxPixelCollider.UnitTopCenter.Quantize(0.0625f);
										}
										runtimeGameActorEffectData.instanceOverheadVFX.transform.position = vector;
									}
									runtimeGameActorEffectData.instanceOverheadVFX.renderer.enabled = !this.IsGone;
								}
								else if (runtimeGameActorEffectData.instanceOverheadVFX)
								{
									UnityEngine.Object.Destroy(runtimeGameActorEffectData.instanceOverheadVFX.gameObject);
								}
							}
							float num = 1f;
							if (gameActorEffect is GameActorCharmEffect && PassiveItem.IsFlagSetAtAll(typeof(BattleStandardItem)))
							{
								num /= BattleStandardItem.BattleStandardCharmDurationMultiplier;
							}
							runtimeGameActorEffectData.elapsed += BraveTime.DeltaTime * num;
							runtimeGameActorEffectData.tickCounter += BraveTime.DeltaTime;
							if (gameActorEffect.IsFinished(this, runtimeGameActorEffectData, runtimeGameActorEffectData.elapsed))
							{
								this.RemoveEffect(gameActorEffect);
							}
						}
					}
				}
			}
		}
		if (this.DoDustUps && !GameManager.Instance.IsLoadingLevel && base.specRigidbody)
		{
			bool flag = base.specRigidbody.Velocity.magnitude > 0f && !this.m_isFalling && !this.IsFlying;
			bool flag2 = false;
			Vector2 unitBottomCenter = base.specRigidbody.PrimaryPixelCollider.UnitBottomCenter;
			IntVector2 intVector = unitBottomCenter.ToIntVector2(VectorConversions.Floor);
			if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
			{
				CellData cellData = GameManager.Instance.Dungeon.data[intVector];
				CellVisualData.CellFloorType cellFloorType = cellData.cellVisualData.floorType;
				if (flag && this is PlayerController)
				{
					flag &= base.spriteAnimator.QueryGroundedFrame() || this.IsFlying;
					flag &= !GameManager.Instance.Dungeon.CellIsPit(base.specRigidbody.UnitCenter);
					flag &= ((PlayerController)this).IsVisible;
					flag &= cellFloorType != CellVisualData.CellFloorType.Ice;
				}
				PlayerController playerController = this as PlayerController;
				if (playerController && playerController.IsGhost)
				{
					flag = true;
					flag2 = true;
				}
				else if (playerController && playerController.IsSlidingOverSurface)
				{
					flag = false;
				}
				if (flag)
				{
					this.m_dustUpTimer += BraveTime.DeltaTime;
					if (this.m_dustUpTimer >= this.DustUpInterval / this.DustUpMultiplier)
					{
						if (this.OverrideDustUp)
						{
							SpawnManager.SpawnVFX(this.OverrideDustUp, unitBottomCenter, Quaternion.identity);
							this.m_dustUpTimer = 0f;
						}
						else if (flag2)
						{
							SpawnManager.SpawnVFX(ResourceCache.Acquire("Global VFX/GhostDustUp") as GameObject, unitBottomCenter, Quaternion.identity);
							this.m_dustUpTimer = 0f;
						}
						else
						{
							SharedDungeonSettings sharedSettingsPrefab = GameManager.Instance.Dungeon.sharedSettingsPrefab;
							DustUpVFX dungeonDustups = GameManager.Instance.Dungeon.dungeonDustups;
							Color color = Color.clear;
							bool flag3 = false;
							bool flag4 = false;
							if (this.m_currentGoop != null)
							{
								if (this.m_currentGoopFrozen)
								{
									cellFloorType = CellVisualData.CellFloorType.Ice;
								}
								else
								{
									cellFloorType = ((!this.m_currentGoop.usesWaterVfx) ? CellVisualData.CellFloorType.ThickGoop : CellVisualData.CellFloorType.Water);
									flag3 = this.m_currentGoop.AppliesCheese;
									flag4 = this.m_currentGoop.AppliesSpeedModifierContinuously && this.m_currentGoop.playerStepsChangeLifetime && this.m_currentGoop.SpeedModifierEffect.effectIdentifier.StartsWith("phase web", StringComparison.Ordinal);
								}
								color = this.m_currentGoop.baseColor32;
							}
							if (cellFloorType == CellVisualData.CellFloorType.Water && dungeonDustups.waterDustup != null)
							{
								GameObject gameObject = SpawnManager.SpawnVFX(dungeonDustups.waterDustup, unitBottomCenter, Quaternion.identity);
								if (gameObject)
								{
									Renderer component = gameObject.GetComponent<Renderer>();
									if (component)
									{
										gameObject.GetComponent<tk2dBaseSprite>().OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
										component.material.SetColor(this.m_overrideColorID, color);
									}
								}
								if (dungeonDustups.additionalWaterDustup != null)
								{
									SpawnManager.SpawnVFX(dungeonDustups.additionalWaterDustup, unitBottomCenter, Quaternion.identity, true);
								}
							}
							else if (cellFloorType != CellVisualData.CellFloorType.Ice)
							{
								if (cellFloorType == CellVisualData.CellFloorType.ThickGoop)
								{
									if (flag3)
									{
										if (sharedSettingsPrefab.additionalCheeseDustup != null)
										{
											SpawnManager.SpawnVFX(sharedSettingsPrefab.additionalCheeseDustup, unitBottomCenter + new Vector2(0.0625f, -0.25f), Quaternion.identity, true);
										}
									}
									else if (flag4)
									{
										if (sharedSettingsPrefab.additionalWebDustup != null)
										{
											SpawnManager.SpawnVFX(sharedSettingsPrefab.additionalWebDustup, unitBottomCenter + new Vector2(0.0625f, -0.25f), Quaternion.identity, true);
										}
									}
								}
								else
								{
									SpawnManager.SpawnVFX(dungeonDustups.runDustup, unitBottomCenter, Quaternion.identity);
								}
							}
							this.m_dustUpTimer = 0f;
							if (flag4)
							{
								this.m_dustUpTimer = -this.DustUpInterval / this.DustUpMultiplier;
							}
						}
					}
				}
				else if (playerController && playerController.IsSlidingOverSurface)
				{
					this.m_dustUpTimer += BraveTime.DeltaTime;
					if (this.m_dustUpTimer >= this.DustUpInterval / this.DustUpMultiplier)
					{
						DustUpVFX dungeonDustups2 = GameManager.Instance.Dungeon.dungeonDustups;
						GameObject gameObject2 = SpawnManager.SpawnVFX(GameManager.Instance.Dungeon.sharedSettingsPrefab.additionalTableDustup, unitBottomCenter + new Vector2(0.0625f, 0.25f), Quaternion.identity);
						if (gameObject2)
						{
							tk2dBaseSprite component2 = gameObject2.GetComponent<tk2dBaseSprite>();
							if (component2)
							{
								component2.HeightOffGround = 0f;
								component2.UpdateZDepth();
							}
						}
						this.m_dustUpTimer = 0f;
					}
				}
			}
		}
		if (!GameManager.Instance.IsLoadingLevel)
		{
			this.HandleGoopChecks();
		}
	}

	// Token: 0x06006C82 RID: 27778 RVA: 0x002AB018 File Offset: 0x002A9218
	public void ApplyEffect(GameActorEffect effect, float sourcePartialAmount = 1f, Projectile sourceProjectile = null)
	{
		if (this.ImmuneToAllEffects)
		{
			return;
		}
		if (!effect.AffectsPlayers && this is PlayerController)
		{
			return;
		}
		if (!effect.AffectsEnemies && this is AIActor)
		{
			return;
		}
		EffectResistanceType effectResistanceType = effect.resistanceType;
		if (effectResistanceType == EffectResistanceType.None)
		{
			if (effect.effectIdentifier == "poison")
			{
				effectResistanceType = EffectResistanceType.Poison;
			}
			if (effect.effectIdentifier == "fire")
			{
				effectResistanceType = EffectResistanceType.Fire;
			}
			if (effect.effectIdentifier == "freeze")
			{
				effectResistanceType = EffectResistanceType.Freeze;
			}
			if (effect.effectIdentifier == "charm")
			{
				effectResistanceType = EffectResistanceType.Charm;
			}
		}
		float num = sourcePartialAmount * (1f - this.GetResistanceForEffectType(effectResistanceType));
		if (num == 0f)
		{
			return;
		}
		if (effect is GameActorCharmEffect && base.healthHaver != null && base.healthHaver.IsBoss)
		{
			return;
		}
		for (int i = 0; i < this.m_activeEffects.Count; i++)
		{
			if (this.m_activeEffects[i].effectIdentifier == effect.effectIdentifier)
			{
				switch (effect.stackMode)
				{
				case GameActorEffect.EffectStackingMode.Refresh:
					this.m_activeEffectData[i].elapsed = 0f;
					break;
				case GameActorEffect.EffectStackingMode.Stack:
					this.m_activeEffectData[i].elapsed -= effect.duration;
					if (effect.maxStackedDuration > 0f)
					{
						this.m_activeEffectData[i].elapsed = Mathf.Max(effect.duration - effect.maxStackedDuration, this.m_activeEffectData[i].elapsed);
					}
					break;
				case GameActorEffect.EffectStackingMode.DarkSoulsAccumulate:
					effect.OnDarkSoulsAccumulate(this, this.m_activeEffectData[i], num, sourceProjectile);
					break;
				}
				return;
			}
		}
		RuntimeGameActorEffectData runtimeGameActorEffectData = new RuntimeGameActorEffectData();
		runtimeGameActorEffectData.actor = this;
		effect.ApplyTint(this);
		if (effect.OverheadVFX != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(effect.OverheadVFX);
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			gameObject.transform.parent = base.transform;
			if (base.healthHaver.IsBoss)
			{
				gameObject.transform.position = base.specRigidbody.HitboxPixelCollider.UnitTopCenter;
			}
			else
			{
				Bounds bounds = base.sprite.GetBounds();
				Vector3 vector = base.transform.position + new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.max.y, 0f).Quantize(0.0625f);
				if (effect.PlaysVFXOnActor)
				{
					vector.y = base.transform.position.y + bounds.min.y;
				}
				gameObject.transform.position = base.sprite.WorldCenter.ToVector3ZUp(0f).WithY(vector.y);
			}
			component.HeightOffGround = 0.5f;
			base.sprite.AttachRenderer(component);
			runtimeGameActorEffectData.instanceOverheadVFX = gameObject.GetComponent<tk2dBaseSprite>();
			if (this.IsGone)
			{
				runtimeGameActorEffectData.instanceOverheadVFX.renderer.enabled = false;
			}
		}
		this.m_activeEffects.Add(effect);
		this.m_activeEffectData.Add(runtimeGameActorEffectData);
		effect.OnEffectApplied(this, this.m_activeEffectData[this.m_activeEffectData.Count - 1], num);
	}

	// Token: 0x06006C83 RID: 27779 RVA: 0x002AB3E4 File Offset: 0x002A95E4
	public GameActorEffect GetEffect(string effectIdentifier)
	{
		for (int i = 0; i < this.m_activeEffects.Count; i++)
		{
			if (this.m_activeEffects[i].effectIdentifier == effectIdentifier)
			{
				return this.m_activeEffects[i];
			}
		}
		return null;
	}

	// Token: 0x06006C84 RID: 27780 RVA: 0x002AB438 File Offset: 0x002A9638
	public GameActorEffect GetEffect(EffectResistanceType resistanceType)
	{
		for (int i = 0; i < this.m_activeEffects.Count; i++)
		{
			if (this.m_activeEffects[i].resistanceType == resistanceType)
			{
				return this.m_activeEffects[i];
			}
		}
		return null;
	}

	// Token: 0x06006C85 RID: 27781 RVA: 0x002AB488 File Offset: 0x002A9688
	public void RemoveEffect(string effectIdentifier)
	{
		for (int i = this.m_activeEffects.Count - 1; i >= 0; i--)
		{
			if (this.m_activeEffects[i].effectIdentifier == effectIdentifier)
			{
				this.RemoveEffect(i, false);
			}
		}
	}

	// Token: 0x06006C86 RID: 27782 RVA: 0x002AB4D8 File Offset: 0x002A96D8
	public void RemoveEffect(GameActorEffect effect)
	{
		for (int i = 0; i < this.m_activeEffects.Count; i++)
		{
			if (this.m_activeEffects[i].effectIdentifier == effect.effectIdentifier)
			{
				this.RemoveEffect(i, false);
				return;
			}
		}
	}

	// Token: 0x06006C87 RID: 27783 RVA: 0x002AB52C File Offset: 0x002A972C
	public void RemoveAllEffects(bool ignoreDeathCheck = false)
	{
		for (int i = this.m_activeEffects.Count - 1; i >= 0; i--)
		{
			this.RemoveEffect(i, ignoreDeathCheck);
		}
	}

	// Token: 0x06006C88 RID: 27784 RVA: 0x002AB560 File Offset: 0x002A9760
	private void RemoveEffect(int index, bool ignoreDeathCheck = false)
	{
		if (!ignoreDeathCheck && base.healthHaver && base.healthHaver.IsDead)
		{
			return;
		}
		GameActorEffect gameActorEffect = this.m_activeEffects[index];
		gameActorEffect.OnEffectRemoved(this, this.m_activeEffectData[index]);
		if (gameActorEffect.AppliesTint)
		{
			this.DeregisterOverrideColor(gameActorEffect.effectIdentifier);
		}
		if (gameActorEffect.AppliesOutlineTint && this is AIActor)
		{
			(this as AIActor).ClearOverrideOutlineColor();
		}
		this.m_activeEffects.RemoveAt(index);
		if (this.m_activeEffectData[index].instanceOverheadVFX)
		{
			UnityEngine.Object.Destroy(this.m_activeEffectData[index].instanceOverheadVFX.gameObject);
		}
		this.m_activeEffectData.RemoveAt(index);
	}

	// Token: 0x06006C89 RID: 27785 RVA: 0x002AB63C File Offset: 0x002A983C
	protected Vector2 ApplyMovementModifiers(Vector2 voluntaryVel, Vector2 involuntaryVel)
	{
		if (this.MovementModifiers != null)
		{
			this.MovementModifiers(ref voluntaryVel, ref involuntaryVel);
		}
		return voluntaryVel + involuntaryVel;
	}

	// Token: 0x06006C8A RID: 27786 RVA: 0x002AB660 File Offset: 0x002A9860
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006C8B RID: 27787 RVA: 0x002AB668 File Offset: 0x002A9868
	public void SetResistance(EffectResistanceType resistType, float resistAmount)
	{
		bool flag = false;
		for (int i = 0; i < base.aiActor.EffectResistances.Length; i++)
		{
			if (base.aiActor.EffectResistances[i].resistType == resistType)
			{
				base.aiActor.EffectResistances[i].resistAmount = resistAmount;
				flag = true;
			}
		}
		if (!flag)
		{
			ActorEffectResistance actorEffectResistance = new ActorEffectResistance
			{
				resistType = resistType,
				resistAmount = resistAmount
			};
			base.aiActor.EffectResistances = BraveUtility.AppendArray<ActorEffectResistance>(base.aiActor.EffectResistances, actorEffectResistance);
		}
	}

	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x06006C8C RID: 27788 RVA: 0x002AB708 File Offset: 0x002A9908
	public bool IsValid
	{
		get
		{
			if (!this || base.healthHaver.IsDead || this.IsFalling || this.IsGone)
			{
				return false;
			}
			if (!base.specRigidbody.enabled || base.specRigidbody.GetPixelCollider(ColliderType.HitBox) == null)
			{
				return false;
			}
			AIActor aiactor = this as AIActor;
			return !aiactor || aiactor.IsWorthShootingAt;
		}
	}

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x06006C8D RID: 27789 RVA: 0x002AB788 File Offset: 0x002A9988
	public Vector2 AimCenter
	{
		get
		{
			return base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x1700102D RID: 4141
	// (get) Token: 0x06006C8E RID: 27790 RVA: 0x002AB798 File Offset: 0x002A9998
	public Vector2 Velocity
	{
		get
		{
			if (this.PreventAutoAimVelocity)
			{
				return Vector2.zero;
			}
			return base.specRigidbody.Velocity;
		}
	}

	// Token: 0x1700102E RID: 4142
	// (get) Token: 0x06006C8F RID: 27791 RVA: 0x002AB7B8 File Offset: 0x002A99B8
	public bool IgnoreForSuperDuperAutoAim
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06006C90 RID: 27792 RVA: 0x002AB7BC File Offset: 0x002A99BC
	public float MinDistForSuperDuperAutoAim
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06006C91 RID: 27793 RVA: 0x002AB7C4 File Offset: 0x002A99C4
	protected void HandleGoopChecks()
	{
		this.m_currentGoop = null;
		this.m_currentGoopFrozen = false;
		if (GameManager.Instance.Dungeon == null)
		{
			return;
		}
		RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round));
		List<DeadlyDeadlyGoopManager> roomGoops = absoluteRoomFromPosition.RoomGoops;
		if (roomGoops != null)
		{
			for (int i = 0; i < roomGoops.Count; i++)
			{
				bool flag = roomGoops[i].ProcessGameActor(this);
				if (flag)
				{
					this.m_currentGoop = roomGoops[i].goopDefinition;
					this.m_currentGoopFrozen = roomGoops[i].IsPositionFrozen(base.specRigidbody.UnitCenter);
				}
			}
		}
	}

	// Token: 0x06006C92 RID: 27794 RVA: 0x002AB884 File Offset: 0x002A9A84
	public void ForceFall()
	{
		this.Fall();
	}

	// Token: 0x06006C93 RID: 27795 RVA: 0x002AB88C File Offset: 0x002A9A8C
	protected virtual void Fall()
	{
		if (base.healthHaver != null)
		{
			base.healthHaver.EndFlashEffects();
		}
		this.m_isFalling = true;
		if (this.HasShadow && this.ShadowObject)
		{
			this.ShadowObject.GetComponent<Renderer>().enabled = false;
		}
	}

	// Token: 0x06006C94 RID: 27796 RVA: 0x002AB8E8 File Offset: 0x002A9AE8
	public virtual void RecoverFromFall()
	{
		this.m_isFalling = false;
		if (this.HasShadow && this.ShadowObject)
		{
			this.ShadowObject.GetComponent<Renderer>().enabled = true;
		}
	}

	// Token: 0x06006C95 RID: 27797 RVA: 0x002AB920 File Offset: 0x002A9B20
	protected virtual bool QueryGroundedFrame()
	{
		return !base.spriteAnimator || base.spriteAnimator.QueryGroundedFrame();
	}

	// Token: 0x06006C96 RID: 27798 RVA: 0x002AB940 File Offset: 0x002A9B40
	protected void HandlePitChecks()
	{
		if (GameManager.Instance.Dungeon == null || GameManager.Instance.Dungeon.data == null)
		{
			return;
		}
		bool flag = this.QueryGroundedFrame() && !this.IsFlying && !this.FallingProhibited;
		PlayerController playerController = this as PlayerController;
		if (playerController && playerController.CurrentRoom != null && playerController.CurrentRoom.RoomFallValidForMaintenance())
		{
			flag = true;
		}
		if (this.m_isFalling)
		{
			return;
		}
		Rect rect = new Rect
		{
			min = PhysicsEngine.PixelToUnitMidpoint(base.specRigidbody.PrimaryPixelCollider.LowerLeft),
			max = PhysicsEngine.PixelToUnitMidpoint(base.specRigidbody.PrimaryPixelCollider.UpperRight)
		};
		Rect rect2 = new Rect(rect);
		this.ModifyPitVectors(ref rect2);
		Dungeon dungeon = GameManager.Instance.Dungeon;
		bool flag2 = dungeon.ShouldReallyFall(rect2.min);
		bool flag3 = dungeon.ShouldReallyFall(new Vector3(rect2.xMax, rect2.yMin));
		bool flag4 = dungeon.ShouldReallyFall(new Vector3(rect2.xMin, rect2.yMax));
		bool flag5 = dungeon.ShouldReallyFall(rect2.max);
		bool flag6 = dungeon.ShouldReallyFall(rect2.center);
		this.IsOverPitAtAll = flag2 || flag3 || flag4 || flag5 || flag6;
		if (this.IsOverPitAtAll)
		{
			flag2 |= dungeon.data.isWall((int)rect2.xMin, (int)rect2.yMin);
			flag3 |= dungeon.data.isWall((int)rect2.xMax, (int)rect2.yMin);
			flag4 |= dungeon.data.isWall((int)rect2.xMin, (int)rect2.yMax);
			flag5 |= dungeon.data.isWall((int)rect2.xMax, (int)rect2.yMax);
			flag6 |= dungeon.data.isWall((int)rect2.center.x, (int)rect2.center.y);
			bool flag7 = flag2 && flag3 && flag4 && flag5 && flag6;
			bool flag8 = this.OnAboutToFall == null || this.OnAboutToFall(!flag7);
			if (flag7 && flag && flag8)
			{
				this.Fall();
				return;
			}
		}
		bool flag9 = true;
		for (int i = 0; i < this.SupportingPlatforms.Count; i++)
		{
			if (!this.SupportingPlatforms[i].StaticForPitfall)
			{
				flag9 = false;
				break;
			}
		}
		if (flag9)
		{
			if (this.SupportingPlatforms.Count > 0)
			{
				this.m_cachedPosition = this.SupportingPlatforms[0].specRigidbody.UnitCenter;
			}
			else if (Vector3.Distance(this.m_cachedPosition, base.specRigidbody.Position.GetPixelVector2()) > 3f)
			{
				bool flag10 = dungeon.CellSupportsFalling(rect.min) || dungeon.PositionInCustomPitSRB(rect.min);
				bool flag11 = dungeon.CellSupportsFalling(new Vector3(rect.xMax, rect.yMin)) || dungeon.PositionInCustomPitSRB(new Vector3(rect.xMax, rect.yMin));
				bool flag12 = dungeon.CellSupportsFalling(new Vector3(rect.xMin, rect.yMax)) || dungeon.PositionInCustomPitSRB(new Vector3(rect.xMin, rect.yMax));
				bool flag13 = dungeon.CellSupportsFalling(rect.max) || dungeon.PositionInCustomPitSRB(rect.max);
				bool flag14 = dungeon.CellSupportsFalling(rect.center) || dungeon.PositionInCustomPitSRB(rect.center);
				IntVector2 intVector = rect.min.ToIntVector2(VectorConversions.Floor);
				bool flag15 = dungeon.data.CheckInBoundsAndValid(intVector) && dungeon.data[intVector].type == CellType.FLOOR;
				if (!flag10 && !flag11 && !flag12 && !flag13 && !flag14 && flag15)
				{
					this.m_cachedPosition = base.specRigidbody.Position.GetPixelVector2();
				}
			}
			else
			{
				bool flag16 = dungeon.CellIsNearPit(rect.min) || dungeon.PositionInCustomPitSRB(rect.min);
				bool flag17 = dungeon.CellIsNearPit(new Vector3(rect.xMax, rect.yMin)) || dungeon.PositionInCustomPitSRB(new Vector3(rect.xMax, rect.yMin));
				bool flag18 = dungeon.CellIsNearPit(new Vector3(rect.xMin, rect.yMax)) || dungeon.PositionInCustomPitSRB(new Vector3(rect.xMin, rect.yMax));
				bool flag19 = dungeon.CellIsNearPit(rect.max) || dungeon.PositionInCustomPitSRB(rect.max);
				bool flag20 = dungeon.CellIsNearPit(rect.center) || dungeon.PositionInCustomPitSRB(rect.center);
				IntVector2 intVector2 = rect.min.ToIntVector2(VectorConversions.Floor);
				bool flag21 = dungeon.data.CheckInBoundsAndValid(intVector2) && dungeon.data[intVector2].type == CellType.FLOOR;
				if (!flag16 && !flag17 && !flag18 && !flag19 && !flag20 && flag21)
				{
					this.m_cachedPosition = base.specRigidbody.Position.GetPixelVector2();
				}
			}
		}
	}

	// Token: 0x06006C97 RID: 27799 RVA: 0x002ABFC0 File Offset: 0x002AA1C0
	public void PlaySmallExplosionsStyleEffect(GameObject vfxPrefab, int count, float midDelay)
	{
		if (base.sprite)
		{
			base.StartCoroutine(this.HandleSmallExplosionsStyleEffect(vfxPrefab, count, midDelay));
		}
	}

	// Token: 0x06006C98 RID: 27800 RVA: 0x002ABFE4 File Offset: 0x002AA1E4
	private IEnumerator HandleSmallExplosionsStyleEffect(GameObject vfxPrefab, int explosionCount, float explosionMidDelay)
	{
		for (int i = 0; i < explosionCount; i++)
		{
			if (!base.sprite)
			{
				break;
			}
			Vector2 minPos = base.sprite.WorldBottomLeft;
			Vector2 maxPos = base.sprite.WorldTopRight;
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2((maxPos.x - minPos.x) * 0.1f, (maxPos.y - minPos.y) * 0.1f));
			SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			yield return new WaitForSeconds(explosionMidDelay);
		}
		yield break;
	}

	// Token: 0x06006C99 RID: 27801 RVA: 0x002AC014 File Offset: 0x002AA214
	protected virtual void ModifyPitVectors(ref Rect rect)
	{
	}

	// Token: 0x06006C9A RID: 27802 RVA: 0x002AC018 File Offset: 0x002AA218
	public GameObject PlayEffectOnActor(GameObject effect, Vector3 offset, bool attached = true, bool alreadyMiddleCenter = false, bool useHitbox = false)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(effect, false);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		Vector3 vector = ((!useHitbox || !base.specRigidbody || base.specRigidbody.HitboxPixelCollider == null) ? base.sprite.WorldCenter.ToVector3ZUp(0f) : base.specRigidbody.HitboxPixelCollider.UnitCenter.ToVector3ZUp(0f));
		if (!alreadyMiddleCenter)
		{
			component.PlaceAtPositionByAnchor(vector + offset, tk2dBaseSprite.Anchor.MiddleCenter);
		}
		else
		{
			component.transform.position = vector + offset;
		}
		if (attached)
		{
			gameObject.transform.parent = base.transform;
			component.HeightOffGround = 0.2f;
			base.sprite.AttachRenderer(component);
			if (this is PlayerController)
			{
				SmartOverheadVFXController component2 = gameObject.GetComponent<SmartOverheadVFXController>();
				if (component2 != null)
				{
					component2.Initialize(this as PlayerController, offset);
				}
			}
		}
		if (!alreadyMiddleCenter)
		{
			gameObject.transform.localPosition = gameObject.transform.localPosition.QuantizeFloor(0.0625f);
		}
		return gameObject;
	}

	// Token: 0x06006C9B RID: 27803 RVA: 0x002AC140 File Offset: 0x002AA340
	public GameObject PlayFairyEffectOnActor(GameObject effect, Vector3 offset, float duration, bool alreadyMiddleCenter = false)
	{
		if (base.sprite.FlipX)
		{
			offset += new Vector3(base.sprite.GetBounds().extents.x * 2f, 0f, 0f);
		}
		GameObject gameObject = SpawnManager.SpawnVFX(effect, true);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		gameObject.transform.parent = base.transform;
		component.HeightOffGround = 0.2f;
		base.sprite.AttachRenderer(component);
		base.StartCoroutine(this.HandleFairyFlyEffect(component, offset, duration, alreadyMiddleCenter));
		return gameObject;
	}

	// Token: 0x06006C9C RID: 27804 RVA: 0x002AC1E0 File Offset: 0x002AA3E0
	protected IEnumerator HandleFairyFlyEffect(tk2dBaseSprite instantiated, Vector3 offset, float duration, bool alreadyMiddleCenter)
	{
		float ela = 0f;
		float centerX = base.sprite.WorldTopCenter.x - base.sprite.transform.position.x;
		float startY = base.sprite.WorldTopCenter.y - base.sprite.transform.position.y;
		float currentRotationAngle = 0f;
		float radius = 1f;
		Transform target = instantiated.transform;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			float t = ela / duration;
			currentRotationAngle = Mathf.Lerp(0f, 720f, t);
			float curX = radius * Mathf.Sin(currentRotationAngle * 0.017453292f);
			float curY = Mathf.Lerp(startY, 0f, t);
			Vector2 constructedPos = new Vector2(centerX + curX, curY);
			target.localPosition = constructedPos.ToVector3ZUp(0f) + offset;
			if (currentRotationAngle % 360f > 90f && currentRotationAngle % 360f < 270f)
			{
				instantiated.HeightOffGround = -0.2f;
			}
			else
			{
				instantiated.HeightOffGround = 0.2f;
			}
			instantiated.UpdateZDepth();
			yield return null;
		}
		SpawnManager.Despawn(instantiated.gameObject);
		yield break;
	}

	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x06006C9D RID: 27805 RVA: 0x002AC210 File Offset: 0x002AA410
	public Color CurrentOverrideColor
	{
		get
		{
			if (this.m_overrideColorStack.Count == 0)
			{
				this.RegisterOverrideColor(new Color(1f, 1f, 1f, 0f), "base");
			}
			return this.m_overrideColorStack[this.m_overrideColorStack.Count - 1];
		}
	}

	// Token: 0x06006C9E RID: 27806 RVA: 0x002AC26C File Offset: 0x002AA46C
	public bool HasSourcedOverrideColor(string source)
	{
		return this.m_overrideColorSources.Contains(source);
	}

	// Token: 0x06006C9F RID: 27807 RVA: 0x002AC27C File Offset: 0x002AA47C
	public bool HasOverrideColor()
	{
		return this.m_overrideColorSources.Count != 0 && (this.m_overrideColorSources.Count != 1 || !(this.m_overrideColorSources[0] == "base"));
	}

	// Token: 0x06006CA0 RID: 27808 RVA: 0x002AC2CC File Offset: 0x002AA4CC
	public void RegisterOverrideColor(Color overrideColor, string source)
	{
		int num = this.m_overrideColorSources.IndexOf(source);
		if (num >= 0)
		{
			this.m_overrideColorStack[num] = overrideColor;
		}
		else
		{
			this.m_overrideColorSources.Add(source);
			this.m_overrideColorStack.Add(overrideColor);
		}
		this.OnOverrideColorsChanged();
	}

	// Token: 0x06006CA1 RID: 27809 RVA: 0x002AC320 File Offset: 0x002AA520
	public void DeregisterOverrideColor(string source)
	{
		int num = this.m_overrideColorSources.IndexOf(source);
		if (num >= 0)
		{
			this.m_overrideColorStack.RemoveAt(num);
			this.m_overrideColorSources.RemoveAt(num);
		}
		this.OnOverrideColorsChanged();
	}

	// Token: 0x06006CA2 RID: 27810 RVA: 0x002AC360 File Offset: 0x002AA560
	public void OnOverrideColorsChanged()
	{
		if (this.OverrideColorOverridden)
		{
			return;
		}
		for (int i = 0; i < base.healthHaver.bodySprites.Count; i++)
		{
			if (base.healthHaver.bodySprites[i])
			{
				base.healthHaver.bodySprites[i].usesOverrideMaterial = true;
				base.healthHaver.bodySprites[i].renderer.material.SetColor(this.m_overrideColorID, this.CurrentOverrideColor);
			}
		}
		if (base.renderer && base.renderer.material)
		{
			this.m_colorOverridenMaterial = base.renderer.material;
			this.m_colorOverridenShader = this.m_colorOverridenMaterial.shader;
		}
	}

	// Token: 0x06006CA3 RID: 27811 RVA: 0x002AC440 File Offset: 0x002AA640
	public void ToggleShadowVisiblity(bool value)
	{
		if (this.ShadowObject)
		{
			this.ShadowObject.GetComponent<Renderer>().enabled = value;
		}
	}

	// Token: 0x06006CA4 RID: 27812 RVA: 0x002AC464 File Offset: 0x002AA664
	protected GameObject GenerateDefaultBlobShadow(float heightOffset = 0f)
	{
		if (this.ShadowObject)
		{
			BraveUtility.Log("We are trying to generate a GameActor shadow when we already have one!", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
			return this.ShadowObject;
		}
		Transform transform = base.transform;
		SpeculativeRigidbody componentInChildren = base.gameObject.GetComponentInChildren<SpeculativeRigidbody>();
		if (componentInChildren)
		{
			componentInChildren.Reinitialize();
		}
		if (this.ShadowPrefab)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ShadowPrefab);
			gameObject.transform.parent = transform;
			if (base.specRigidbody)
			{
				gameObject.transform.localPosition = base.specRigidbody.UnitCenter.ToVector3ZUp(0f) - base.transform.position.WithZ(0f);
			}
			else
			{
				gameObject.transform.localPosition = Vector3.zero;
			}
			DepthLookupManager.ProcessRenderer(gameObject.GetComponent<Renderer>(), DepthLookupManager.GungeonSortingLayer.BACKGROUND);
			if (base.aiActor != null && base.aiActor.ActorName == "Gatling Gull")
			{
				tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
				component.HeightOffGround = 6f;
			}
			else if (this.ShadowHeightOffGround != 0f)
			{
				tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
				component2.HeightOffGround = this.ShadowHeightOffGround;
			}
			this.ShadowObject = gameObject;
			gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
			return gameObject;
		}
		if (transform.Find("PlayerSprite") != null)
		{
			if (transform.Find("PlayerShadow") != null)
			{
				return transform.Find("PlayerShadow").gameObject;
			}
			PlayerController component3 = transform.GetComponent<PlayerController>();
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("DefaultShadowSprite"));
			gameObject2.transform.parent = transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localPosition = new Vector3(component3.SpriteBottomCenter.x - transform.position.x, 0f, 0.1f);
			gameObject2.GetComponent<tk2dSprite>().HeightOffGround = -0.1f;
			DepthLookupManager.ProcessRenderer(gameObject2.GetComponent<Renderer>(), DepthLookupManager.GungeonSortingLayer.PLAYFIELD);
			this.ShadowObject = gameObject2;
			gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
			return gameObject2;
		}
		else
		{
			if (componentInChildren != null)
			{
				GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("DefaultShadowSprite"));
				gameObject3.transform.parent = transform;
				float num = componentInChildren.UnitBottomLeft.y - transform.position.y + heightOffset;
				Vector3 vector = new Vector3(componentInChildren.UnitCenter.x - transform.position.x, num, 0.1f);
				gameObject3.transform.localPosition = vector;
				gameObject3.GetComponent<tk2dSprite>().HeightOffGround = -heightOffset * 2f + this.ShadowHeightOffGround;
				DepthLookupManager.ProcessRenderer(gameObject3.GetComponent<Renderer>(), DepthLookupManager.GungeonSortingLayer.PLAYFIELD);
				this.ShadowObject = gameObject3;
				gameObject3.transform.position = gameObject3.transform.position.Quantize(0.0625f);
				this.ShadowObject.transform.localPosition += this.ActorShadowOffset;
				return gameObject3;
			}
			return null;
		}
	}

	// Token: 0x04006963 RID: 26979
	[Header("Actor Shared Properties")]
	public string ActorName;

	// Token: 0x04006964 RID: 26980
	public string OverrideDisplayName;

	// Token: 0x04006965 RID: 26981
	[EnumFlags]
	public CoreActorTypes actorTypes;

	// Token: 0x04006966 RID: 26982
	[Space(3f)]
	public bool HasShadow = true;

	// Token: 0x04006967 RID: 26983
	[ShowInInspectorIf("HasShadow", true)]
	public GameObject ShadowPrefab;

	// Token: 0x04006968 RID: 26984
	[ShowInInspectorIf("HasShadow", true)]
	public GameObject ShadowObject;

	// Token: 0x04006969 RID: 26985
	[ShowInInspectorIf("HasShadow", true)]
	public float ShadowHeightOffGround;

	// Token: 0x0400696A RID: 26986
	[ShowInInspectorIf("HasShadow", true)]
	public Transform ShadowParent;

	// Token: 0x0400696B RID: 26987
	[ShowInInspectorIf("HasShadow", true)]
	public Vector3 ActorShadowOffset;

	// Token: 0x0400696C RID: 26988
	[Space(3f)]
	public bool DoDustUps;

	// Token: 0x0400696D RID: 26989
	[ShowInInspectorIf("DoDustUps", true)]
	public float DustUpInterval;

	// Token: 0x0400696E RID: 26990
	[ShowInInspectorIf("DoDustUps", true)]
	public GameObject OverrideDustUp;

	// Token: 0x0400696F RID: 26991
	[Space(3f)]
	public float FreezeDispelFactor = 20f;

	// Token: 0x04006970 RID: 26992
	public bool ImmuneToAllEffects;

	// Token: 0x04006971 RID: 26993
	public ActorEffectResistance[] EffectResistances;

	// Token: 0x04006972 RID: 26994
	public const float OUTLINE_DEPTH = 0.1f;

	// Token: 0x04006973 RID: 26995
	public const float GUN_DEPTH = 0.075f;

	// Token: 0x04006974 RID: 26996
	public const float ACTOR_VFX_DEPTH = 0.2f;

	// Token: 0x04006975 RID: 26997
	public const float BACKFACING_ANGLE_MAX = 155f;

	// Token: 0x04006976 RID: 26998
	public const float BACKFACING_ANGLE_MIN = 25f;

	// Token: 0x04006977 RID: 26999
	public const float BACKWARDS_ANGLE_MAX = 120f;

	// Token: 0x04006978 RID: 27000
	public const float BACKWARDS_ANGLE_MIN = 60f;

	// Token: 0x04006979 RID: 27001
	public const float FORWARDS_ANGLE_MAX = -60f;

	// Token: 0x0400697A RID: 27002
	public const float FORWARDS_ANGLE_MIN = -120f;

	// Token: 0x0400697B RID: 27003
	public const float FLIP_LEFT_THRESHOLD_FRONT = 105f;

	// Token: 0x0400697C RID: 27004
	public const float FLIP_RIGHT_THRESHOLD_FRONT = 75f;

	// Token: 0x0400697D RID: 27005
	public const float FLIP_LEFT_THRESHOLD_BACK = 105f;

	// Token: 0x0400697E RID: 27006
	public const float FLIP_RIGHT_THRESHOLD_BACK = 75f;

	// Token: 0x0400697F RID: 27007
	[NonSerialized]
	public bool FallingProhibited;

	// Token: 0x04006980 RID: 27008
	private GameObject m_stealthVfx;

	// Token: 0x04006981 RID: 27009
	[NonSerialized]
	public float actorReflectionAdditionalOffset;

	// Token: 0x04006982 RID: 27010
	protected GoopDefinition m_currentGoop;

	// Token: 0x04006983 RID: 27011
	protected bool m_currentGoopFrozen;

	// Token: 0x0400698C RID: 27020
	[NonSerialized]
	public Vector2 ImpartedVelocity;

	// Token: 0x0400698E RID: 27022
	protected int m_overrideColorID;

	// Token: 0x0400698F RID: 27023
	protected int m_overrideFlatColorID;

	// Token: 0x04006990 RID: 27024
	protected int m_specialFlagsID;

	// Token: 0x04006991 RID: 27025
	protected int m_stencilID;

	// Token: 0x04006992 RID: 27026
	[NonSerialized]
	public bool IsOverPitAtAll;

	// Token: 0x04006993 RID: 27027
	public Func<bool, bool> OnAboutToFall;

	// Token: 0x04006994 RID: 27028
	public bool OverrideColorOverridden;

	// Token: 0x04006995 RID: 27029
	private List<string> m_overrideColorSources = new List<string>();

	// Token: 0x04006996 RID: 27030
	private List<Color> m_overrideColorStack = new List<Color>();

	// Token: 0x04006997 RID: 27031
	protected Material m_colorOverridenMaterial;

	// Token: 0x04006998 RID: 27032
	protected Shader m_colorOverridenShader;

	// Token: 0x04006999 RID: 27033
	[NonSerialized]
	public float BeamStatusAmount;

	// Token: 0x0400699A RID: 27034
	protected Vector2 m_cachedPosition;

	// Token: 0x0400699B RID: 27035
	protected bool m_isFalling;

	// Token: 0x0400699C RID: 27036
	protected OverridableBool m_isFlying = new OverridableBool(false);

	// Token: 0x0400699D RID: 27037
	protected OverridableBool m_isStealthed = new OverridableBool(false);

	// Token: 0x0400699E RID: 27038
	protected float m_dustUpTimer;

	// Token: 0x0400699F RID: 27039
	protected List<MovingPlatform> m_supportingPlatforms = new List<MovingPlatform>();

	// Token: 0x040069A0 RID: 27040
	protected List<GameActorEffect> m_activeEffects = new List<GameActorEffect>();

	// Token: 0x040069A1 RID: 27041
	protected List<RuntimeGameActorEffectData> m_activeEffectData = new List<RuntimeGameActorEffectData>();

	// Token: 0x020012E7 RID: 4839
	// (Invoke) Token: 0x06006CA6 RID: 27814
	public delegate void MovementModifier(ref Vector2 volundaryVel, ref Vector2 involuntaryVel);
}
