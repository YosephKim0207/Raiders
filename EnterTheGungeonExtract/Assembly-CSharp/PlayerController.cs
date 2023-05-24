using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Dungeonator;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020015D7 RID: 5591
public class PlayerController : GameActor, ILevelLoadedListener
{
	// Token: 0x170012FF RID: 4863
	// (get) Token: 0x06008053 RID: 32851 RVA: 0x0033E100 File Offset: 0x0033C300
	// (set) Token: 0x06008054 RID: 32852 RVA: 0x0033E108 File Offset: 0x0033C308
	public bool IsTemporaryEeveeForUnlock
	{
		get
		{
			return this.m_isTemporaryEeveeForUnlock;
		}
		set
		{
			this.m_isTemporaryEeveeForUnlock = value;
			this.ClearOverrideShader();
			if (value)
			{
				Texture2D texture2D = this.portalEeveeTex;
				if (this && base.sprite && base.sprite.renderer && base.sprite.renderer.material)
				{
					base.sprite.renderer.material.SetTexture("_EeveeTex", texture2D);
				}
			}
		}
	}

	// Token: 0x06008055 RID: 32853 RVA: 0x0033E194 File Offset: 0x0033C394
	public void SetTemporaryEeveeSafeNoShader(bool value)
	{
		this.m_isTemporaryEeveeForUnlock = value;
	}

	// Token: 0x17001300 RID: 4864
	// (get) Token: 0x06008056 RID: 32854 RVA: 0x0033E1A0 File Offset: 0x0033C3A0
	// (set) Token: 0x06008057 RID: 32855 RVA: 0x0033E1A8 File Offset: 0x0033C3A8
	public int Blanks
	{
		get
		{
			return this.m_blanks;
		}
		set
		{
			this.m_blanks = value;
			GameStatsManager.Instance.UpdateMaximum(TrackedMaximums.MOST_BLANKS_HELD, (float)this.m_blanks);
			GameUIRoot.Instance.UpdatePlayerBlankUI(this);
			GameUIRoot.Instance.UpdatePlayerConsumables(this.carriedConsumables);
		}
	}

	// Token: 0x17001301 RID: 4865
	// (get) Token: 0x06008058 RID: 32856 RVA: 0x0033E1E0 File Offset: 0x0033C3E0
	// (set) Token: 0x06008059 RID: 32857 RVA: 0x0033E1E8 File Offset: 0x0033C3E8
	public bool IsOnFire
	{
		get
		{
			return this.m_isOnFire;
		}
		set
		{
			if (value && this.HasActiveBonusSynergy(CustomSynergyType.FOSSIL_PHOENIX, false))
			{
				value = false;
			}
			if (value && this.stats.UsesFireSourceEffect)
			{
				if (this.m_onFireEffectData == null)
				{
					this.m_onFireEffectData = GameActorFireEffect.ApplyFlamesToTarget(this, this.stats.OnFireSourceEffect);
				}
			}
			else if (!value && this.stats.UsesFireSourceEffect && this.m_onFireEffectData != null)
			{
				GameActorFireEffect.DestroyFlames(this.m_onFireEffectData);
				this.m_onFireEffectData = null;
			}
			if (value && !this.m_isOnFire && this.OnIgnited != null)
			{
				this.OnIgnited(this);
			}
			this.m_isOnFire = value;
		}
	}

	// Token: 0x0600805A RID: 32858 RVA: 0x0033E2AC File Offset: 0x0033C4AC
	public void IncreaseFire(float amount)
	{
		if (base.SuppressEffectUpdates)
		{
			return;
		}
		this.CurrentFireMeterValue += amount * base.healthHaver.GetDamageModifierForType(CoreDamageTypes.Fire);
	}

	// Token: 0x0600805B RID: 32859 RVA: 0x0033E2D8 File Offset: 0x0033C4D8
	public void IncreasePoison(float amount)
	{
		if (base.SuppressEffectUpdates)
		{
			return;
		}
		if (this.IsGhost)
		{
			return;
		}
		if (base.healthHaver && !base.healthHaver.IsVulnerable)
		{
			return;
		}
		this.CurrentPoisonMeterValue += amount * base.healthHaver.GetDamageModifierForType(CoreDamageTypes.Poison);
	}

	// Token: 0x17001302 RID: 4866
	// (get) Token: 0x0600805C RID: 32860 RVA: 0x0033E33C File Offset: 0x0033C53C
	public Vector2 SmoothedCameraCenter
	{
		get
		{
			if (base.specRigidbody && base.specRigidbody.HitboxPixelCollider != null)
			{
				return base.specRigidbody.HitboxPixelCollider.UnitCenter + base.specRigidbody.Position.Remainder.Quantize(0.0625f / Pixelator.Instance.ScaleTileScale);
			}
			if (base.sprite)
			{
				return base.sprite.WorldCenter;
			}
			return base.transform.position.XY();
		}
	}

	// Token: 0x17001303 RID: 4867
	// (get) Token: 0x0600805D RID: 32861 RVA: 0x0033E3D4 File Offset: 0x0033C5D4
	public bool IsCapableOfStealing
	{
		get
		{
			return this.m_capableOfStealing.Value;
		}
	}

	// Token: 0x0600805E RID: 32862 RVA: 0x0033E3E4 File Offset: 0x0033C5E4
	public void SetCapableOfStealing(bool value, string reason, float? duration = null)
	{
		this.m_capableOfStealing.SetOverride(reason, value, duration);
		this.ForceRefreshInteractable = true;
	}

	// Token: 0x17001304 RID: 4868
	// (get) Token: 0x0600805F RID: 32863 RVA: 0x0033E3FC File Offset: 0x0033C5FC
	public int KillsThisRun
	{
		get
		{
			return this.m_enemiesKilled;
		}
	}

	// Token: 0x17001305 RID: 4869
	// (get) Token: 0x06008060 RID: 32864 RVA: 0x0033E404 File Offset: 0x0033C604
	public override Gun CurrentGun
	{
		get
		{
			if (this.inventory == null)
			{
				return null;
			}
			if (this.IsGhost)
			{
				return null;
			}
			return this.inventory.CurrentGun;
		}
	}

	// Token: 0x17001306 RID: 4870
	// (get) Token: 0x06008061 RID: 32865 RVA: 0x0033E42C File Offset: 0x0033C62C
	public Gun CurrentSecondaryGun
	{
		get
		{
			if (this.inventory == null)
			{
				return null;
			}
			if (!this.inventory.DualWielding)
			{
				return null;
			}
			if (this.IsGhost)
			{
				return null;
			}
			return this.inventory.CurrentSecondaryGun;
		}
	}

	// Token: 0x17001307 RID: 4871
	// (get) Token: 0x06008062 RID: 32866 RVA: 0x0033E468 File Offset: 0x0033C668
	public PlayerItem CurrentItem
	{
		get
		{
			if (this.m_selectedItemIndex <= 0 || this.m_selectedItemIndex >= this.activeItems.Count)
			{
				this.m_selectedItemIndex = 0;
			}
			if (this.activeItems.Count > 0)
			{
				return this.activeItems[this.m_selectedItemIndex];
			}
			return null;
		}
	}

	// Token: 0x17001308 RID: 4872
	// (get) Token: 0x06008063 RID: 32867 RVA: 0x0033E4C4 File Offset: 0x0033C6C4
	public override Transform GunPivot
	{
		get
		{
			return this.gunAttachPoint;
		}
	}

	// Token: 0x17001309 RID: 4873
	// (get) Token: 0x06008064 RID: 32868 RVA: 0x0033E4CC File Offset: 0x0033C6CC
	public override Transform SecondaryGunPivot
	{
		get
		{
			if (this.secondaryGunAttachPoint == null)
			{
				GameObject gameObject = new GameObject("secondary attach point");
				this.secondaryGunAttachPoint = gameObject.transform;
				this.secondaryGunAttachPoint.parent = this.gunAttachPoint.parent;
				this.secondaryGunAttachPoint.localPosition = this.gunAttachPoint.localPosition;
			}
			return this.secondaryGunAttachPoint;
		}
	}

	// Token: 0x1700130A RID: 4874
	// (get) Token: 0x06008065 RID: 32869 RVA: 0x0033E534 File Offset: 0x0033C734
	public override Vector3 SpriteDimensions
	{
		get
		{
			return this.m_spriteDimensions;
		}
	}

	// Token: 0x1700130B RID: 4875
	// (get) Token: 0x06008066 RID: 32870 RVA: 0x0033E53C File Offset: 0x0033C73C
	public override bool IsFlying
	{
		get
		{
			return GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES && (this.m_isFlying.Value || this.IsGhost);
		}
	}

	// Token: 0x1700130C RID: 4876
	// (get) Token: 0x06008067 RID: 32871 RVA: 0x0033E56C File Offset: 0x0033C76C
	public Vector3 LockedApproximateSpriteCenter
	{
		get
		{
			return base.CenterPosition;
		}
	}

	// Token: 0x1700130D RID: 4877
	// (get) Token: 0x06008068 RID: 32872 RVA: 0x0033E57C File Offset: 0x0033C77C
	public Vector3 SpriteBottomCenter
	{
		get
		{
			return base.sprite.transform.position.WithX(base.sprite.transform.position.x + ((!base.sprite.FlipX) ? (this.m_spriteDimensions.x / 2f) : (-1f * this.m_spriteDimensions.x / 2f)));
		}
	}

	// Token: 0x1700130E RID: 4878
	// (get) Token: 0x06008069 RID: 32873 RVA: 0x0033E5F4 File Offset: 0x0033C7F4
	public override bool SpriteFlipped
	{
		get
		{
			return base.sprite.FlipX;
		}
	}

	// Token: 0x1700130F RID: 4879
	// (get) Token: 0x0600806A RID: 32874 RVA: 0x0033E604 File Offset: 0x0033C804
	// (set) Token: 0x0600806B RID: 32875 RVA: 0x0033E60C File Offset: 0x0033C80C
	public bool BossKillingMode { get; set; }

	// Token: 0x17001310 RID: 4880
	// (get) Token: 0x0600806C RID: 32876 RVA: 0x0033E618 File Offset: 0x0033C818
	public bool CanReturnTeleport
	{
		get
		{
			return this.m_returnTeleporter != null;
		}
	}

	// Token: 0x17001311 RID: 4881
	// (get) Token: 0x0600806D RID: 32877 RVA: 0x0033E628 File Offset: 0x0033C828
	// (set) Token: 0x0600806E RID: 32878 RVA: 0x0033E698 File Offset: 0x0033C898
	public bool ReceivesTouchDamage
	{
		get
		{
			if (PassiveItem.ActiveFlagItems.ContainsKey(this))
			{
				Dictionary<Type, int> dictionary = PassiveItem.ActiveFlagItems[this];
				if (dictionary.ContainsKey(typeof(LiveAmmoItem)) || dictionary.ContainsKey(typeof(SpikedArmorItem)) || dictionary.ContainsKey(typeof(HelmetItem)))
				{
					return false;
				}
			}
			return this.m_additionalReceivesTouchDamage;
		}
		set
		{
			this.m_additionalReceivesTouchDamage = value;
		}
	}

	// Token: 0x17001312 RID: 4882
	// (get) Token: 0x0600806F RID: 32879 RVA: 0x0033E6A4 File Offset: 0x0033C8A4
	protected bool m_CanAttack
	{
		get
		{
			return (!this.IsDodgeRolling || this.IsSlidingOverSurface) && !this.IsGunLocked && this.CurrentStoneGunTimer <= 0f;
		}
	}

	// Token: 0x17001313 RID: 4883
	// (get) Token: 0x06008070 RID: 32880 RVA: 0x0033E6DC File Offset: 0x0033C8DC
	public bool WasTalkingThisFrame
	{
		get
		{
			return this.m_wasTalkingThisFrame;
		}
	}

	// Token: 0x17001314 RID: 4884
	// (get) Token: 0x06008071 RID: 32881 RVA: 0x0033E6E4 File Offset: 0x0033C8E4
	// (set) Token: 0x06008072 RID: 32882 RVA: 0x0033E6EC File Offset: 0x0033C8EC
	public bool IgnoredByCamera { get; private set; }

	// Token: 0x17001315 RID: 4885
	// (get) Token: 0x06008073 RID: 32883 RVA: 0x0033E6F8 File Offset: 0x0033C8F8
	public bool IsDodgeRolling
	{
		get
		{
			return this.m_dodgeRollState != PlayerController.DodgeRollState.None && this.m_dodgeRollState != PlayerController.DodgeRollState.AdditionalDelay;
		}
	}

	// Token: 0x17001316 RID: 4886
	// (get) Token: 0x06008074 RID: 32884 RVA: 0x0033E718 File Offset: 0x0033C918
	public bool IsInMinecart
	{
		get
		{
			return this.currentMineCart;
		}
	}

	// Token: 0x17001317 RID: 4887
	// (get) Token: 0x06008075 RID: 32885 RVA: 0x0033E728 File Offset: 0x0033C928
	public bool IsInCombat
	{
		get
		{
			return this.CurrentRoom != null && this.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
		}
	}

	// Token: 0x17001318 RID: 4888
	// (get) Token: 0x06008076 RID: 32886 RVA: 0x0033E744 File Offset: 0x0033C944
	public bool CanBeGrabbed
	{
		get
		{
			return base.healthHaver.IsVulnerable && !base.IsFalling && !this.IsGhost && !this.IsEthereal;
		}
	}

	// Token: 0x17001319 RID: 4889
	// (get) Token: 0x06008077 RID: 32887 RVA: 0x0033E778 File Offset: 0x0033C978
	// (set) Token: 0x06008078 RID: 32888 RVA: 0x0033E780 File Offset: 0x0033C980
	public bool IsThief { get; set; }

	// Token: 0x1700131A RID: 4890
	// (get) Token: 0x06008079 RID: 32889 RVA: 0x0033E78C File Offset: 0x0033C98C
	public RoomHandler CurrentRoom
	{
		get
		{
			return this.m_currentRoom;
		}
	}

	// Token: 0x1700131B RID: 4891
	// (get) Token: 0x0600807A RID: 32890 RVA: 0x0033E794 File Offset: 0x0033C994
	// (set) Token: 0x0600807B RID: 32891 RVA: 0x0033E79C File Offset: 0x0033C99C
	public bool IsVisible
	{
		get
		{
			return this.m_isVisible;
		}
		set
		{
			if (value != this.m_isVisible)
			{
				this.m_isVisible = value;
				this.ToggleRenderer(this.m_isVisible, "isVisible");
				this.ToggleGunRenderers(this.m_isVisible, "isVisible");
				this.ToggleHandRenderers(this.m_isVisible, "isVisible");
			}
		}
	}

	// Token: 0x1700131C RID: 4892
	// (get) Token: 0x0600807C RID: 32892 RVA: 0x0033E7F0 File Offset: 0x0033C9F0
	public bool CanDetectHiddenEnemies
	{
		get
		{
			return this.CurrentGun && this.CurrentGun.GetComponent<PredatorGunController>();
		}
	}

	// Token: 0x1700131D RID: 4893
	// (get) Token: 0x0600807D RID: 32893 RVA: 0x0033E818 File Offset: 0x0033CA18
	// (set) Token: 0x0600807E RID: 32894 RVA: 0x0033E820 File Offset: 0x0033CA20
	private IAutoAimTarget SuperAutoAimTarget { get; set; }

	// Token: 0x1700131E RID: 4894
	// (get) Token: 0x0600807F RID: 32895 RVA: 0x0033E82C File Offset: 0x0033CA2C
	// (set) Token: 0x06008080 RID: 32896 RVA: 0x0033E834 File Offset: 0x0033CA34
	private IAutoAimTarget SuperDuperAimTarget { get; set; }

	// Token: 0x1700131F RID: 4895
	// (get) Token: 0x06008081 RID: 32897 RVA: 0x0033E840 File Offset: 0x0033CA40
	// (set) Token: 0x06008082 RID: 32898 RVA: 0x0033E848 File Offset: 0x0033CA48
	private Vector2 SuperDuperAimPoint { get; set; }

	// Token: 0x17001320 RID: 4896
	// (get) Token: 0x06008083 RID: 32899 RVA: 0x0033E854 File Offset: 0x0033CA54
	public float BulletScaleModifier
	{
		get
		{
			return this.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale);
		}
	}

	// Token: 0x17001321 RID: 4897
	// (get) Token: 0x06008084 RID: 32900 RVA: 0x0033E864 File Offset: 0x0033CA64
	// (set) Token: 0x06008085 RID: 32901 RVA: 0x0033E86C File Offset: 0x0033CA6C
	public bool SuppressThisClick { get; set; }

	// Token: 0x17001322 RID: 4898
	// (get) Token: 0x06008086 RID: 32902 RVA: 0x0033E878 File Offset: 0x0033CA78
	// (set) Token: 0x06008087 RID: 32903 RVA: 0x0033E880 File Offset: 0x0033CA80
	public bool InExitCell { get; set; }

	// Token: 0x17001323 RID: 4899
	// (get) Token: 0x06008088 RID: 32904 RVA: 0x0033E88C File Offset: 0x0033CA8C
	// (set) Token: 0x06008089 RID: 32905 RVA: 0x0033E894 File Offset: 0x0033CA94
	public CellData CurrentExitCell { get; set; }

	// Token: 0x17001324 RID: 4900
	// (get) Token: 0x0600808A RID: 32906 RVA: 0x0033E8A0 File Offset: 0x0033CAA0
	// (set) Token: 0x0600808B RID: 32907 RVA: 0x0033E8A8 File Offset: 0x0033CAA8
	public Vector2 AverageVelocity { get; set; }

	// Token: 0x17001325 RID: 4901
	// (get) Token: 0x0600808C RID: 32908 RVA: 0x0033E8B4 File Offset: 0x0033CAB4
	// (set) Token: 0x0600808D RID: 32909 RVA: 0x0033E8D4 File Offset: 0x0033CAD4
	public bool ArmorlessAnimations
	{
		get
		{
			return this.hasArmorlessAnimations && !GameManager.Instance.IsFoyer;
		}
		set
		{
			this.hasArmorlessAnimations = value;
		}
	}

	// Token: 0x17001326 RID: 4902
	// (get) Token: 0x0600808E RID: 32910 RVA: 0x0033E8E0 File Offset: 0x0033CAE0
	public bool UseArmorlessAnim
	{
		get
		{
			return this.ArmorlessAnimations && base.healthHaver.Armor == 0f && this.OverrideAnimationLibrary == null;
		}
	}

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x0600808F RID: 32911 RVA: 0x0033E914 File Offset: 0x0033CB14
	// (remove) Token: 0x06008090 RID: 32912 RVA: 0x0033E94C File Offset: 0x0033CB4C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPitfall;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x06008091 RID: 32913 RVA: 0x0033E984 File Offset: 0x0033CB84
	// (remove) Token: 0x06008092 RID: 32914 RVA: 0x0033E9BC File Offset: 0x0033CBBC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Projectile, float> PostProcessProjectile;

	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06008093 RID: 32915 RVA: 0x0033E9F4 File Offset: 0x0033CBF4
	// (remove) Token: 0x06008094 RID: 32916 RVA: 0x0033EA2C File Offset: 0x0033CC2C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<BeamController> PostProcessBeam;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06008095 RID: 32917 RVA: 0x0033EA64 File Offset: 0x0033CC64
	// (remove) Token: 0x06008096 RID: 32918 RVA: 0x0033EA9C File Offset: 0x0033CC9C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<BeamController, SpeculativeRigidbody, float> PostProcessBeamTick;

	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x06008097 RID: 32919 RVA: 0x0033EAD4 File Offset: 0x0033CCD4
	// (remove) Token: 0x06008098 RID: 32920 RVA: 0x0033EB0C File Offset: 0x0033CD0C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<BeamController> PostProcessBeamChanceTick;

	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x06008099 RID: 32921 RVA: 0x0033EB44 File Offset: 0x0033CD44
	// (remove) Token: 0x0600809A RID: 32922 RVA: 0x0033EB7C File Offset: 0x0033CD7C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Projectile> PostProcessThrownGun;

	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x0600809B RID: 32923 RVA: 0x0033EBB4 File Offset: 0x0033CDB4
	// (remove) Token: 0x0600809C RID: 32924 RVA: 0x0033EBEC File Offset: 0x0033CDEC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, float> OnDealtDamage;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x0600809D RID: 32925 RVA: 0x0033EC24 File Offset: 0x0033CE24
	// (remove) Token: 0x0600809E RID: 32926 RVA: 0x0033EC5C File Offset: 0x0033CE5C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, float, bool, HealthHaver> OnDealtDamageContext;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x0600809F RID: 32927 RVA: 0x0033EC94 File Offset: 0x0033CE94
	// (remove) Token: 0x060080A0 RID: 32928 RVA: 0x0033ECCC File Offset: 0x0033CECC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController> OnKilledEnemy;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x060080A1 RID: 32929 RVA: 0x0033ED04 File Offset: 0x0033CF04
	// (remove) Token: 0x060080A2 RID: 32930 RVA: 0x0033ED3C File Offset: 0x0033CF3C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, HealthHaver> OnKilledEnemyContext;

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x060080A3 RID: 32931 RVA: 0x0033ED74 File Offset: 0x0033CF74
	// (remove) Token: 0x060080A4 RID: 32932 RVA: 0x0033EDAC File Offset: 0x0033CFAC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, PlayerItem> OnUsedPlayerItem;

	// Token: 0x140000BD RID: 189
	// (add) Token: 0x060080A5 RID: 32933 RVA: 0x0033EDE4 File Offset: 0x0033CFE4
	// (remove) Token: 0x060080A6 RID: 32934 RVA: 0x0033EE1C File Offset: 0x0033D01C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController> OnTriedToInitiateAttack;

	// Token: 0x140000BE RID: 190
	// (add) Token: 0x060080A7 RID: 32935 RVA: 0x0033EE54 File Offset: 0x0033D054
	// (remove) Token: 0x060080A8 RID: 32936 RVA: 0x0033EE8C File Offset: 0x0033D08C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, int> OnUsedBlank;

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x060080A9 RID: 32937 RVA: 0x0033EEC4 File Offset: 0x0033D0C4
	// (remove) Token: 0x060080AA RID: 32938 RVA: 0x0033EEFC File Offset: 0x0033D0FC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Gun, Gun, bool> GunChanged;

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x060080AB RID: 32939 RVA: 0x0033EF34 File Offset: 0x0033D134
	// (remove) Token: 0x060080AC RID: 32940 RVA: 0x0033EF6C File Offset: 0x0033D16C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController> OnDidUnstealthyAction;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x060080AD RID: 32941 RVA: 0x0033EFA4 File Offset: 0x0033D1A4
	// (remove) Token: 0x060080AE RID: 32942 RVA: 0x0033EFDC File Offset: 0x0033D1DC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController> OnPreDodgeRoll;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x060080AF RID: 32943 RVA: 0x0033F014 File Offset: 0x0033D214
	// (remove) Token: 0x060080B0 RID: 32944 RVA: 0x0033F04C File Offset: 0x0033D24C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, Vector2> OnRollStarted;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x060080B1 RID: 32945 RVA: 0x0033F084 File Offset: 0x0033D284
	// (remove) Token: 0x060080B2 RID: 32946 RVA: 0x0033F0BC File Offset: 0x0033D2BC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController> OnIsRolling;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x060080B3 RID: 32947 RVA: 0x0033F0F4 File Offset: 0x0033D2F4
	// (remove) Token: 0x060080B4 RID: 32948 RVA: 0x0033F12C File Offset: 0x0033D32C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, AIActor> OnRolledIntoEnemy;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x060080B5 RID: 32949 RVA: 0x0033F164 File Offset: 0x0033D364
	// (remove) Token: 0x060080B6 RID: 32950 RVA: 0x0033F19C File Offset: 0x0033D39C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Projectile> OnDodgedProjectile;

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x060080B7 RID: 32951 RVA: 0x0033F1D4 File Offset: 0x0033D3D4
	// (remove) Token: 0x060080B8 RID: 32952 RVA: 0x0033F20C File Offset: 0x0033D40C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<BeamController, PlayerController> OnDodgedBeam;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x060080B9 RID: 32953 RVA: 0x0033F244 File Offset: 0x0033D444
	// (remove) Token: 0x060080BA RID: 32954 RVA: 0x0033F27C File Offset: 0x0033D47C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController> OnReceivedDamage;

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x060080BB RID: 32955 RVA: 0x0033F2B4 File Offset: 0x0033D4B4
	// (remove) Token: 0x060080BC RID: 32956 RVA: 0x0033F2EC File Offset: 0x0033D4EC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, ShopItemController> OnItemPurchased;

	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x060080BD RID: 32957 RVA: 0x0033F324 File Offset: 0x0033D524
	// (remove) Token: 0x060080BE RID: 32958 RVA: 0x0033F35C File Offset: 0x0033D55C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController, ShopItemController> OnItemStolen;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x060080BF RID: 32959 RVA: 0x0033F394 File Offset: 0x0033D594
	// (remove) Token: 0x060080C0 RID: 32960 RVA: 0x0033F3CC File Offset: 0x0033D5CC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<PlayerController> OnRoomClearEvent;

	// Token: 0x17001327 RID: 4903
	// (get) Token: 0x060080C1 RID: 32961 RVA: 0x0033F404 File Offset: 0x0033D604
	// (set) Token: 0x060080C2 RID: 32962 RVA: 0x0033F40C File Offset: 0x0033D60C
	public float AdditionalChestSpawnChance { get; set; }

	// Token: 0x17001328 RID: 4904
	// (get) Token: 0x060080C3 RID: 32963 RVA: 0x0033F418 File Offset: 0x0033D618
	// (set) Token: 0x060080C4 RID: 32964 RVA: 0x0033F478 File Offset: 0x0033D678
	public PlayerInputState CurrentInputState
	{
		get
		{
			if (this.m_disableInput.Value)
			{
				return PlayerInputState.NoInput;
			}
			if (this.m_inputState == PlayerInputState.AllInput && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
			{
				return PlayerInputState.FoyerInputOnly;
			}
			if (this.m_inputState == PlayerInputState.AllInput && GameManager.Instance.IsFoyer)
			{
				return PlayerInputState.FoyerInputOnly;
			}
			return this.m_inputState;
		}
		set
		{
			this.m_inputState = value;
		}
	}

	// Token: 0x17001329 RID: 4905
	// (get) Token: 0x060080C5 RID: 32965 RVA: 0x0033F484 File Offset: 0x0033D684
	public bool AcceptingAnyInput
	{
		get
		{
			return this.CurrentInputState != PlayerInputState.NoInput;
		}
	}

	// Token: 0x1700132A RID: 4906
	// (get) Token: 0x060080C6 RID: 32966 RVA: 0x0033F494 File Offset: 0x0033D694
	public bool AcceptingNonMotionInput
	{
		get
		{
			return (this.CurrentInputState == PlayerInputState.AllInput || this.CurrentInputState == PlayerInputState.NoMovement) && !GameManager.Instance.PreventPausing;
		}
	}

	// Token: 0x1700132B RID: 4907
	// (get) Token: 0x060080C7 RID: 32967 RVA: 0x0033F4C0 File Offset: 0x0033D6C0
	public bool IsInputOverridden
	{
		get
		{
			return this.m_disableInput.Value;
		}
	}

	// Token: 0x060080C8 RID: 32968 RVA: 0x0033F4D0 File Offset: 0x0033D6D0
	public void SetInputOverride(string reason)
	{
		this.m_disableInput.AddOverride(reason, null);
	}

	// Token: 0x060080C9 RID: 32969 RVA: 0x0033F4F4 File Offset: 0x0033D6F4
	public void ClearInputOverride(string reason)
	{
		this.m_disableInput.RemoveOverride(reason);
	}

	// Token: 0x060080CA RID: 32970 RVA: 0x0033F504 File Offset: 0x0033D704
	public void ClearAllInputOverrides()
	{
		this.m_disableInput.ClearOverrides();
		this.CurrentInputState = PlayerInputState.AllInput;
	}

	// Token: 0x060080CB RID: 32971 RVA: 0x0033F518 File Offset: 0x0033D718
	public IPlayerInteractable GetLastInteractable()
	{
		return this.m_lastInteractionTarget;
	}

	// Token: 0x1700132C RID: 4908
	// (get) Token: 0x060080CC RID: 32972 RVA: 0x0033F520 File Offset: 0x0033D720
	public PlayerController.DodgeRollState CurrentRollState
	{
		get
		{
			return this.m_dodgeRollState;
		}
	}

	// Token: 0x1700132D RID: 4909
	// (get) Token: 0x060080CD RID: 32973 RVA: 0x0033F528 File Offset: 0x0033D728
	// (set) Token: 0x060080CE RID: 32974 RVA: 0x0033F530 File Offset: 0x0033D730
	public bool IsSlidingOverSurface
	{
		get
		{
			return this.m_isSlidingOverSurface;
		}
		set
		{
			this.m_isSlidingOverSurface = value;
		}
	}

	// Token: 0x1700132E RID: 4910
	// (get) Token: 0x060080CF RID: 32975 RVA: 0x0033F53C File Offset: 0x0033D73C
	private bool RenderBodyHand
	{
		get
		{
			return !this.ForceHandless && this.CurrentSecondaryGun == null && (this.CurrentGun == null || this.CurrentGun.Handedness != GunHandedness.TwoHanded);
		}
	}

	// Token: 0x1700132F RID: 4911
	// (get) Token: 0x060080D0 RID: 32976 RVA: 0x0033F590 File Offset: 0x0033D790
	// (set) Token: 0x060080D1 RID: 32977 RVA: 0x0033F598 File Offset: 0x0033D798
	public bool IsFiring { get; set; }

	// Token: 0x17001330 RID: 4912
	// (get) Token: 0x060080D2 RID: 32978 RVA: 0x0033F5A4 File Offset: 0x0033D7A4
	// (set) Token: 0x060080D3 RID: 32979 RVA: 0x0033F5AC File Offset: 0x0033D7AC
	public bool ForceRefreshInteractable { get; set; }

	// Token: 0x060080D4 RID: 32980 RVA: 0x0033F5B8 File Offset: 0x0033D7B8
	public bool IsCachedLeapInteractable(IPlayerInteractable ixable)
	{
		return this.m_leapInteractables.Contains(ixable);
	}

	// Token: 0x17001331 RID: 4913
	// (get) Token: 0x060080D5 RID: 32981 RVA: 0x0033F5C8 File Offset: 0x0033D7C8
	// (set) Token: 0x060080D6 RID: 32982 RVA: 0x0033F5D0 File Offset: 0x0033D7D0
	public bool HighAccuracyAimMode
	{
		get
		{
			return this.m_highAccuracyAimMode;
		}
		set
		{
			if (this.m_highAccuracyAimMode != value)
			{
				this.m_previousAimVector = Vector2.zero;
			}
			this.m_highAccuracyAimMode = value;
		}
	}

	// Token: 0x17001332 RID: 4914
	// (get) Token: 0x060080D7 RID: 32983 RVA: 0x0033F5F0 File Offset: 0x0033D7F0
	// (set) Token: 0x060080D8 RID: 32984 RVA: 0x0033F5F8 File Offset: 0x0033D7F8
	public bool HasTakenDamageThisRun { get; set; }

	// Token: 0x17001333 RID: 4915
	// (get) Token: 0x060080D9 RID: 32985 RVA: 0x0033F604 File Offset: 0x0033D804
	// (set) Token: 0x060080DA RID: 32986 RVA: 0x0033F60C File Offset: 0x0033D80C
	public bool HasTakenDamageThisFloor { get; set; }

	// Token: 0x17001334 RID: 4916
	// (get) Token: 0x060080DB RID: 32987 RVA: 0x0033F618 File Offset: 0x0033D818
	// (set) Token: 0x060080DC RID: 32988 RVA: 0x0033F620 File Offset: 0x0033D820
	public bool HasReceivedNewGunThisFloor { get; set; }

	// Token: 0x17001335 RID: 4917
	// (get) Token: 0x060080DD RID: 32989 RVA: 0x0033F62C File Offset: 0x0033D82C
	// (set) Token: 0x060080DE RID: 32990 RVA: 0x0033F634 File Offset: 0x0033D834
	public bool HasFiredNonStartingGun { get; set; }

	// Token: 0x17001336 RID: 4918
	// (get) Token: 0x060080DF RID: 32991 RVA: 0x0033F640 File Offset: 0x0033D840
	// (set) Token: 0x060080E0 RID: 32992 RVA: 0x0033F648 File Offset: 0x0033D848
	public int MasteryTokensCollectedThisRun
	{
		get
		{
			return this.m_masteryTokensCollectedThisRun;
		}
		set
		{
			this.m_masteryTokensCollectedThisRun = value;
			if (this.m_masteryTokensCollectedThisRun >= 5)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COLLECT_FIVE_MASTERY_TOKENS, 0);
			}
		}
	}

	// Token: 0x060080E1 RID: 32993 RVA: 0x0033F670 File Offset: 0x0033D870
	protected override bool QueryGroundedFrame()
	{
		return (!this.IsDodgeRolling || !this.DodgeRollIsBlink || this.m_dodgeRollTimer >= 0.5555556f * this.rollStats.GetModifiedTime(this)) && (!this.IsDodgeRolling || this.m_dodgeRollTimer >= 0.5555556f * this.rollStats.GetModifiedTime(this)) && base.spriteAnimator.QueryGroundedFrame();
	}

	// Token: 0x17001337 RID: 4919
	// (get) Token: 0x060080E2 RID: 32994 RVA: 0x0033F6E8 File Offset: 0x0033D8E8
	// (set) Token: 0x060080E3 RID: 32995 RVA: 0x0033F6F0 File Offset: 0x0033D8F0
	public string OverridePlayerSwitchState
	{
		get
		{
			return this.m_overridePlayerSwitchState;
		}
		set
		{
			this.m_overridePlayerSwitchState = value;
			AkSoundEngine.SetSwitch("CHR_Player", (this.m_overridePlayerSwitchState == null) ? this.characterIdentity.ToString() : this.m_overridePlayerSwitchState, base.gameObject);
		}
	}

	// Token: 0x17001338 RID: 4920
	// (get) Token: 0x060080E4 RID: 32996 RVA: 0x0033F73C File Offset: 0x0033D93C
	protected override float DustUpMultiplier
	{
		get
		{
			return this.stats.MovementSpeed / this.m_startingMovementSpeed;
		}
	}

	// Token: 0x17001339 RID: 4921
	// (get) Token: 0x060080E5 RID: 32997 RVA: 0x0033F750 File Offset: 0x0033D950
	public bool IsPrimaryPlayer
	{
		get
		{
			return this.PlayerIDX == 0;
		}
	}

	// Token: 0x060080E6 RID: 32998 RVA: 0x0033F75C File Offset: 0x0033D95C
	public override void Awake()
	{
		base.Awake();
		this.m_overrideFlatColorID = Shader.PropertyToID("_FlatColor");
		this.m_specialFlagsID = Shader.PropertyToID("_SpecialFlags");
		this.m_stencilID = Shader.PropertyToID("_StencilVal");
		this.m_blooper = base.GetComponentInChildren<CoinBloop>();
		Transform transform = base.transform.Find("PlayerSprite");
		base.sprite = ((!(transform != null)) ? null : transform.GetComponent<tk2dSprite>());
		if (base.sprite == null)
		{
			base.sprite = base.transform.Find("PlayerRotatePoint").Find("PlayerSprite").GetComponent<tk2dSprite>();
		}
		this.m_renderer = base.sprite.GetComponent<MeshRenderer>();
		base.spriteAnimator = base.sprite.GetComponent<tk2dSpriteAnimator>();
		PlayerStats playerStats = base.gameObject.AddComponent<PlayerStats>();
		playerStats.CopyFrom(this.stats);
		this.stats = playerStats;
		this.stats.RecalculateStats(this, true, false);
		if (this.characterIdentity == PlayableCharacters.Eevee)
		{
			this.m_usesRandomStartingEquipment = true;
		}
		if (GameManager.Instance && GameManager.Instance.PrimaryPlayer)
		{
			if (this.characterIdentity == PlayableCharacters.CoopCultist && GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Eevee)
			{
				this.m_usesRandomStartingEquipment = true;
			}
		}
		else if (this.characterIdentity == PlayableCharacters.CoopCultist)
		{
			PlayerController[] array = UnityEngine.Object.FindObjectsOfType<PlayerController>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].characterIdentity == PlayableCharacters.Eevee)
				{
					this.m_usesRandomStartingEquipment = true;
					break;
				}
			}
		}
		if (this.m_usesRandomStartingEquipment)
		{
			if (GameManager.Instance && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				GameStatsManager.Instance.CurrentEeveeEquipSeed = -1;
			}
			if (GameStatsManager.Instance.CurrentEeveeEquipSeed < 0)
			{
				GameStatsManager.Instance.CurrentEeveeEquipSeed = UnityEngine.Random.Range(1, 10000000);
			}
			this.m_randomStartingEquipmentSeed = GameStatsManager.Instance.CurrentEeveeEquipSeed;
			this.SetUpRandomStartingEquipment();
		}
	}

	// Token: 0x060080E7 RID: 32999 RVA: 0x0033F978 File Offset: 0x0033DB78
	public PickupObject.ItemQuality GetQualityFromChances(System.Random r, float dChance, float cChance, float bChance, float aChance, float sChance)
	{
		float num = (dChance + cChance + bChance + aChance + sChance) * (float)r.NextDouble();
		if (num < dChance)
		{
			return PickupObject.ItemQuality.D;
		}
		if (num < dChance + cChance)
		{
			return PickupObject.ItemQuality.C;
		}
		if (num < dChance + cChance + bChance)
		{
			return PickupObject.ItemQuality.B;
		}
		if (num < dChance + cChance + bChance + aChance)
		{
			return PickupObject.ItemQuality.A;
		}
		return PickupObject.ItemQuality.S;
	}

	// Token: 0x060080E8 RID: 33000 RVA: 0x0033F9D0 File Offset: 0x0033DBD0
	private void SetUpRandomStartingEquipment()
	{
		this.startingGunIds.Clear();
		this.startingAlternateGunIds.Clear();
		this.startingPassiveItemIds.Clear();
		this.startingActiveItemIds.Clear();
		this.finalFightGunIds.Clear();
		System.Random random = new System.Random(this.m_randomStartingEquipmentSeed);
		PickupObject.ItemQuality qualityFromChances = this.GetQualityFromChances(random, this.randomStartingEquipmentSettings.D_CHANCE, this.randomStartingEquipmentSettings.C_CHANCE, this.randomStartingEquipmentSettings.B_CHANCE, this.randomStartingEquipmentSettings.A_CHANCE, this.randomStartingEquipmentSettings.S_CHANCE);
		PickupObject.ItemQuality qualityFromChances2 = this.GetQualityFromChances(random, this.randomStartingEquipmentSettings.D_CHANCE, this.randomStartingEquipmentSettings.C_CHANCE, this.randomStartingEquipmentSettings.B_CHANCE, this.randomStartingEquipmentSettings.A_CHANCE, this.randomStartingEquipmentSettings.S_CHANCE);
		Gun randomStartingGun = PickupObjectDatabase.GetRandomStartingGun(random);
		Gun randomGunOfQualities = PickupObjectDatabase.GetRandomGunOfQualities(random, new List<int>(this.randomStartingEquipmentSettings.ExcludedPickups) { GlobalItemIds.Blasphemy }, new PickupObject.ItemQuality[] { qualityFromChances });
		PassiveItem randomPassiveOfQualities = PickupObjectDatabase.GetRandomPassiveOfQualities(random, this.randomStartingEquipmentSettings.ExcludedPickups, new PickupObject.ItemQuality[] { qualityFromChances2 });
		this.startingGunIds.Add(randomStartingGun.PickupObjectId);
		if (randomGunOfQualities)
		{
			this.startingGunIds.Add(randomGunOfQualities.PickupObjectId);
		}
		if (randomPassiveOfQualities)
		{
			this.startingPassiveItemIds.Add(randomPassiveOfQualities.PickupObjectId);
		}
		if (randomGunOfQualities)
		{
			this.finalFightGunIds.Add(randomGunOfQualities.PickupObjectId);
		}
	}

	// Token: 0x060080E9 RID: 33001 RVA: 0x0033FB60 File Offset: 0x0033DD60
	public override void Start()
	{
		base.Start();
		if (PassiveItem.ActiveFlagItems == null)
		{
			PassiveItem.ActiveFlagItems = new Dictionary<PlayerController, Dictionary<Type, int>>();
		}
		if (!PassiveItem.ActiveFlagItems.ContainsKey(this))
		{
			PassiveItem.ActiveFlagItems.Add(this, new Dictionary<Type, int>());
		}
		this.m_allowMoveAsAim = GameManager.Options.autofaceMovementDirection;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		AkSoundEngine.SetSwitch("CHR_Player", (this.m_overridePlayerSwitchState == null) ? this.characterIdentity.ToString() : this.m_overridePlayerSwitchState, base.gameObject);
		if (this.IsPrimaryPlayer)
		{
			AkAudioListener component = base.GetComponent<AkAudioListener>();
			if (component)
			{
				UnityEngine.Object.Destroy(component);
			}
			AkAudioListener orAddComponent = new GameObject("listener")
			{
				transform = 
				{
					parent = base.transform,
					localPosition = (base.specRigidbody.UnitBottomCenter - base.transform.position.XY()).ToVector3ZUp(5f)
				}
			}.GetOrAddComponent<AkAudioListener>();
			orAddComponent.listenerId = ((!this.IsPrimaryPlayer) ? 1 : 0);
		}
		this.ActorName = "Player ID 0";
		base.spriteAnimator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleteDelegate);
		this.m_spriteDimensions = base.sprite.GetUntrimmedBounds().size;
		this.m_startingAttachPointPosition = this.gunAttachPoint.localPosition;
		this.gunAttachPoint.localPosition = BraveUtility.QuantizeVector(this.gunAttachPoint.localPosition, 16f);
		this.stats.RecalculateStats(this, false, false);
		this.m_startingMovementSpeed = this.stats.MovementSpeed;
		this.Blanks = ((GameManager.Instance.CurrentGameType != GameManager.GameType.SINGLE_PLAYER) ? this.stats.NumBlanksPerFloorCoop : this.stats.NumBlanksPerFloor);
		this.InitializeInventory();
		this.InitializeCallbacks();
		if (this.HasShadow)
		{
			GameObject gameObject = base.GenerateDefaultBlobShadow(0f);
			base.sprite.AttachRenderer(gameObject.GetComponent<tk2dSprite>());
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, this.outlineColor, 0.1f, 0f, (this.characterIdentity != PlayableCharacters.Eevee) ? SpriteOutlineManager.OutlineType.NORMAL : SpriteOutlineManager.OutlineType.EEVEE);
		this.OnGunChanged(null, this.CurrentGun, null, null, true);
		base.gameObject.AddComponent<AkGameObj>();
		if (!GameStatsManager.Instance.IsInSession)
		{
			GameStatsManager.Instance.BeginNewSession(this);
		}
		tk2dSpriteAttachPoint tk2dSpriteAttachPoint = base.sprite.GetComponent<tk2dSpriteAttachPoint>();
		if (tk2dSpriteAttachPoint == null)
		{
			tk2dSpriteAttachPoint = base.sprite.gameObject.AddComponent<tk2dSpriteAttachPoint>();
		}
		if (tk2dSpriteAttachPoint.GetAttachPointByName("jetpack") == null)
		{
			tk2dSpriteAttachPoint.ForceAddAttachPoint("jetpack");
		}
		tk2dSpriteAttachPoint.centerUnusedAttachPoints = true;
		if (this.IsPrimaryPlayer)
		{
			this.carriedConsumables.Initialize();
			for (int i = 0; i < this.passiveItems.Count; i++)
			{
				if (this.passiveItems[i] && this.passiveItems[i] is BriefcaseFullOfCashItem)
				{
					this.carriedConsumables.Currency += (this.passiveItems[i] as BriefcaseFullOfCashItem).CurrencyAmount;
				}
			}
		}
		else
		{
			this.carriedConsumables = GameManager.Instance.PrimaryPlayer.carriedConsumables;
			this.lootModData = GameManager.Instance.PrimaryPlayer.lootModData;
		}
		this.unadjustedAimPoint = this.LockedApproximateSpriteCenter + new Vector3(5f, 0f);
		if (this.primaryHand)
		{
			this.primaryHand.InitializeWithPlayer(this, true);
		}
		if (this.secondaryHand)
		{
			this.secondaryHand.InitializeWithPlayer(this, false);
		}
		this.ProcessHandAttachment();
		base.sprite.usesOverrideMaterial = true;
		base.sprite.renderer.material.SetFloat("_Perpendicular", base.sprite.renderer.material.GetFloat("_Perpendicular"));
		if (this.characterIdentity == PlayableCharacters.Pilot || this.characterIdentity == PlayableCharacters.Robot || this.characterIdentity == PlayableCharacters.Guide)
		{
			base.sprite.renderer.material.SetFloat("_PlayerGhostAdjustFactor", 4f);
		}
		else
		{
			base.sprite.renderer.material.SetFloat("_PlayerGhostAdjustFactor", 3f);
		}
		base.healthHaver.RegisterBodySprite(this.primaryHand.sprite, false, 0);
		base.healthHaver.RegisterBodySprite(this.secondaryHand.sprite, false, 0);
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			GameManager.Instance.FrameDelayedEnteredFoyer(this);
		}
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
		if (GameUIRoot.Instance != null)
		{
			GameUIRoot.Instance.UpdatePlayerConsumables(this.carriedConsumables);
		}
		if ((GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER || this.characterIdentity == PlayableCharacters.Eevee) && base.sprite && base.sprite.renderer && !(this is PlayerSpaceshipController))
		{
			base.sprite.renderer.material.shader = ShaderCache.Acquire(this.LocalShaderName);
		}
	}

	// Token: 0x060080EA RID: 33002 RVA: 0x003400EC File Offset: 0x0033E2EC
	private void Instance_OnNewLevelFullyLoaded()
	{
		GameManager.Instance.OnNewLevelFullyLoaded -= this.Instance_OnNewLevelFullyLoaded;
		base.StartCoroutine(this.FrameDelayedInitialDeath(false));
	}

	// Token: 0x060080EB RID: 33003 RVA: 0x00340114 File Offset: 0x0033E314
	public void DieOnMidgameLoad()
	{
		base.StartCoroutine(this.FrameDelayedInitialDeath(true));
	}

	// Token: 0x060080EC RID: 33004 RVA: 0x00340124 File Offset: 0x0033E324
	public static bool AnyoneHasActiveBonusSynergy(CustomSynergyType synergy, out int count)
	{
		count = 0;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (!GameManager.Instance.AllPlayers[i].IsGhost)
			{
				count += GameManager.Instance.AllPlayers[i].CountActiveBonusSynergies(synergy);
			}
		}
		return count > 0;
	}

	// Token: 0x060080ED RID: 33005 RVA: 0x00340188 File Offset: 0x0033E388
	public int CountActiveBonusSynergies(CustomSynergyType synergy)
	{
		if (this.stats != null)
		{
			int num = 0;
			for (int i = 0; i < this.stats.ActiveCustomSynergies.Count; i++)
			{
				if (this.stats.ActiveCustomSynergies[i] == synergy)
				{
					num++;
				}
			}
			for (int j = 0; j < this.CustomEventSynergies.Count; j++)
			{
				if (this.CustomEventSynergies[j] == synergy)
				{
					num++;
				}
			}
			return num;
		}
		return 0;
	}

	// Token: 0x060080EE RID: 33006 RVA: 0x0034021C File Offset: 0x0033E41C
	public bool HasActiveBonusSynergy(CustomSynergyType synergy, bool recursive = false)
	{
		return this.CustomEventSynergies.Contains(synergy) || (this.stats != null && this.stats.ActiveCustomSynergies.Contains(synergy));
	}

	// Token: 0x060080EF RID: 33007 RVA: 0x00340258 File Offset: 0x0033E458
	private IEnumerator FrameDelayedInitialDeath(bool delayTilPostGeneration = false)
	{
		if (delayTilPostGeneration)
		{
			while (Dungeon.IsGenerating)
			{
				yield return null;
			}
		}
		yield return null;
		if (!delayTilPostGeneration)
		{
			this.m_isFalling = true;
		}
		base.healthHaver.ForceSetCurrentHealth(0f);
		base.StartCoroutine(this.HandleCoopDeath(true));
		yield break;
	}

	// Token: 0x060080F0 RID: 33008 RVA: 0x0034027C File Offset: 0x0033E47C
	protected void HandlePostDodgeRollTimer()
	{
		if (this.m_postDodgeRollGunTimer > 0f)
		{
			this.m_postDodgeRollGunTimer -= BraveTime.DeltaTime;
			if (this.m_postDodgeRollGunTimer <= 0f)
			{
				this.ToggleGunRenderers(true, "postdodgeroll");
				this.ToggleHandRenderers(true, "postdodgeroll");
			}
		}
	}

	// Token: 0x060080F1 RID: 33009 RVA: 0x003402D4 File Offset: 0x0033E4D4
	private IEnumerator DestroyEnemyBulletsInCircleForDuration(Vector2 center, float radius, float duration)
	{
		float ela = 0f;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			SilencerInstance.DestroyBulletsInRange(center, radius, true, false, null, false, null, false, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060080F2 RID: 33010 RVA: 0x00340300 File Offset: 0x0033E500
	protected void EndBlinkDodge()
	{
		this.IsEthereal = false;
		this.IsVisible = true;
		this.m_dodgeRollState = PlayerController.DodgeRollState.AdditionalDelay;
		if (this.IsPrimaryPlayer)
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = false;
		}
		else
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = false;
		}
		this.WarpToPoint(this.m_cachedBlinkPosition + (base.transform.position.XY() - base.specRigidbody.UnitCenter), false, false);
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		base.StartCoroutine(this.DestroyEnemyBulletsInCircleForDuration(base.specRigidbody.UnitCenter, 2f, 0.05f));
		this.previousMineCart = null;
		this.ClearBlinkShadow();
	}

	// Token: 0x060080F3 RID: 33011 RVA: 0x003403D0 File Offset: 0x0033E5D0
	private void ClearDodgeRollState()
	{
		this.m_dodgeRollState = PlayerController.DodgeRollState.None;
		this.m_currentDodgeRollDepth = 0;
		this.m_leapInteractables.Clear();
	}

	// Token: 0x060080F4 RID: 33012 RVA: 0x003403EC File Offset: 0x0033E5EC
	public override void Update()
	{
		base.Update();
		if (GameManager.Instance.IsPaused || GameManager.Instance.UnpausedThisFrame)
		{
			return;
		}
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		this.m_interactedThisFrame = false;
		if (this.IsPetting && (!base.spriteAnimator.IsPlaying("pet") || !this.m_pettingTarget || this.m_pettingTarget.m_pettingDoer != this || Vector2.Distance(base.specRigidbody.UnitCenter, this.m_pettingTarget.specRigidbody.UnitCenter) > 3f || this.IsDodgeRolling))
		{
			this.ToggleGunRenderers(true, "petting");
			this.ToggleHandRenderers(true, "petting");
			this.m_pettingTarget = null;
		}
		if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D9)
		{
			this.dx9counter += GameManager.INVARIANT_DELTA_TIME;
			if (this.dx9counter > 5f)
			{
				this.dx9counter = 0f;
				tk2dSprite[] componentsInChildren = base.GetComponentsInChildren<tk2dSprite>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].ForceBuild();
				}
			}
			if (Input.GetKeyDown(KeyCode.F8))
			{
				tk2dBaseSprite[] array = UnityEngine.Object.FindObjectsOfType<tk2dBaseSprite>();
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j])
					{
						array[j].ForceBuild();
					}
				}
				ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
				for (int k = 0; k < allProjectiles.Count; k++)
				{
					Projectile projectile = allProjectiles[k];
					if (projectile && projectile.sprite)
					{
						projectile.sprite.ForceBuild();
					}
				}
			}
		}
		if (base.healthHaver.IsDead && !this.IsGhost)
		{
			return;
		}
		if (this.CharacterUsesRandomGuns && this.inventory != null)
		{
			while (this.inventory.AllGuns.Count > 1)
			{
				this.inventory.DestroyGun(this.inventory.AllGuns[0]);
			}
		}
		this.HandlePostDodgeRollTimer();
		this.m_activeActions = BraveInput.GetInstanceForPlayer(this.PlayerIDX).ActiveActions;
		if ((!this.AcceptingNonMotionInput || this.CurrentStoneGunTimer > 0f) && this.CurrentGun != null && this.CurrentGun.IsFiring && (!this.CurrentGun.IsCharging || (this.CurrentInputState != PlayerInputState.OnlyMovement && !GameManager.IsBossIntro)))
		{
			this.CurrentGun.CeaseAttack(false, null);
			if (this.CurrentSecondaryGun)
			{
				this.CurrentSecondaryGun.CeaseAttack(false, null);
			}
		}
		if (this.inventory != null)
		{
			this.inventory.FrameUpdate();
		}
		Projectile.UpdateEnemyBulletSpeedMultiplier();
		float num = Mathf.Clamp01(BraveTime.DeltaTime / 0.5f);
		if (num > 0f && num < 1f)
		{
			Vector2 vector = this.AverageVelocity * (1f - num) + base.specRigidbody.Velocity * num;
			this.AverageVelocity = BraveMathCollege.ClampSafe(vector, -20f, 20f);
		}
		if (this.m_isFalling)
		{
			return;
		}
		if ((this.IsDodgeRolling || this.m_dodgeRollState == PlayerController.DodgeRollState.AdditionalDelay) && this.m_dodgeRollTimer >= this.rollStats.GetModifiedTime(this))
		{
			if (this.DodgeRollIsBlink)
			{
				if (this.m_dodgeRollTimer > this.rollStats.GetModifiedTime(this) + 0.1f)
				{
					this.IsEthereal = false;
					this.IsVisible = true;
					this.ClearDodgeRollState();
					this.previousMineCart = null;
				}
				else if (this.m_dodgeRollTimer > this.rollStats.GetModifiedTime(this))
				{
					this.EndBlinkDodge();
				}
			}
			else
			{
				this.ClearDodgeRollState();
				this.previousMineCart = null;
			}
		}
		if (this.IsDodgeRolling && this.OnIsRolling != null)
		{
			this.OnIsRolling(this);
		}
		CellVisualData.CellFloorType cellFloorType = CellVisualData.CellFloorType.Stone;
		cellFloorType = GameManager.Instance.Dungeon.GetFloorTypeFromPosition(base.specRigidbody.UnitBottomCenter);
		if (this.m_prevFloorType == null || this.m_prevFloorType.Value != cellFloorType)
		{
			this.m_prevFloorType = new CellVisualData.CellFloorType?(cellFloorType);
			AkSoundEngine.SetSwitch("FS_Surfaces", cellFloorType.ToString(), base.gameObject);
		}
		this.m_playerCommandedDirection = Vector2.zero;
		this.IsFiring = false;
		if (!BraveUtility.isLoadingLevel && !GameManager.Instance.IsLoadingLevel)
		{
			this.ProcessDebugInput();
			if (GameUIRoot.Instance.MetalGearActive)
			{
				if (this.m_activeActions.GunDownAction.WasPressed || this.m_activeActions.GunUpAction.WasPressed)
				{
					this.m_gunChangePressedWhileMetalGeared = true;
				}
			}
			else
			{
				this.m_gunChangePressedWhileMetalGeared = false;
			}
			if (this.AcceptingAnyInput)
			{
				try
				{
					this.m_playerCommandedDirection = this.HandlePlayerInput();
				}
				catch (Exception ex)
				{
					throw new Exception(string.Format("Caught PlayerController.HandlePlayerInput() exception. i={0}, ex={1}", this.exceptionTracker, ex.ToString()));
				}
			}
			if (this.m_newFloorNoInput && this.m_playerCommandedDirection.magnitude > 0f)
			{
				this.m_newFloorNoInput = false;
			}
			if (this.usingForcedInput)
			{
				this.m_playerCommandedDirection = this.forcedInput;
			}
			if (this.m_playerCommandedDirection != Vector2.zero)
			{
				GameManager.Instance.platformInterface.ProcessDlcUnlocks();
			}
		}
		if (this.IsDodgeRolling || this.m_dodgeRollState == PlayerController.DodgeRollState.AdditionalDelay)
		{
			this.HandleContinueDodgeRoll();
		}
		if (PassiveItem.IsFlagSetForCharacter(this, typeof(HeavyBootsItem)))
		{
			this.knockbackComponent = Vector2.zero;
		}
		if (this.IsDodgeRolling)
		{
			if (this.usingForcedInput)
			{
				base.specRigidbody.Velocity = this.forcedInput.normalized * this.GetDodgeRollSpeed() + this.knockbackComponent + this.immutableKnockbackComponent;
			}
			else if (this.DodgeRollIsBlink)
			{
				base.specRigidbody.Velocity = Vector2.zero;
			}
			else
			{
				base.specRigidbody.Velocity = this.lockedDodgeRollDirection.normalized * this.GetDodgeRollSpeed() + this.knockbackComponent + this.immutableKnockbackComponent;
			}
		}
		else
		{
			float num2 = 1f;
			if (!this.IsInCombat && GameManager.Options.IncreaseSpeedOutOfCombat)
			{
				bool flag = true;
				List<AIActor> allEnemies = StaticReferenceManager.AllEnemies;
				if (allEnemies != null)
				{
					for (int l = 0; l < allEnemies.Count; l++)
					{
						AIActor aiactor = allEnemies[l];
						if (aiactor && aiactor.IsMimicEnemy && !aiactor.IsGone)
						{
							float num3 = Vector2.Distance(aiactor.CenterPosition, base.CenterPosition);
							if (num3 < 40f)
							{
								flag = false;
								break;
							}
						}
					}
				}
				if (flag)
				{
					num2 *= 1.5f;
				}
			}
			Vector2 vector2 = this.m_playerCommandedDirection * this.stats.MovementSpeed * num2;
			Vector2 vector3 = this.knockbackComponent;
			base.specRigidbody.Velocity = base.ApplyMovementModifiers(vector2, vector3) + this.immutableKnockbackComponent;
		}
		base.specRigidbody.Velocity += this.ImpartedVelocity;
		this.ImpartedVelocity = Vector2.zero;
		if (cellFloorType == CellVisualData.CellFloorType.Ice && !this.IsFlying && !PassiveItem.IsFlagSetForCharacter(this, typeof(HeavyBootsItem)))
		{
			this.m_maxIceFactor = Mathf.Clamp01(this.m_maxIceFactor + BraveTime.DeltaTime * 4f);
		}
		else if (this.IsFlying && !PassiveItem.IsFlagSetForCharacter(this, typeof(HeavyBootsItem)))
		{
			this.m_maxIceFactor = 0f;
		}
		else
		{
			this.m_maxIceFactor = Mathf.Clamp01(this.m_maxIceFactor - BraveTime.DeltaTime * 1.5f);
		}
		if (this.m_maxIceFactor > 0f)
		{
			float num4 = Mathf.Max(this.m_lastVelocity.magnitude, base.specRigidbody.Velocity.magnitude);
			float num5 = 1f - Mathf.Clamp01(Mathf.Abs(Vector2.Angle(this.m_lastVelocity, base.specRigidbody.Velocity)) / 180f);
			float num6 = Mathf.Lerp(1f / BraveTime.DeltaTime, Mathf.Lerp(0.5f, 1.5f, num5), this.m_maxIceFactor);
			if (this.m_lastVelocity.magnitude < 0.25f)
			{
				num6 = Mathf.Min(1f / BraveTime.DeltaTime, Mathf.Max(num6 * (1f / (30f * BraveTime.DeltaTime)), num6));
			}
			base.specRigidbody.Velocity = Vector2.Lerp(this.m_lastVelocity, base.specRigidbody.Velocity, num6 * BraveTime.DeltaTime);
			base.specRigidbody.Velocity = base.specRigidbody.Velocity.normalized * Mathf.Clamp(base.specRigidbody.Velocity.magnitude, 0f, num4);
			if (float.IsNaN(base.specRigidbody.Velocity.x) || float.IsNaN(base.specRigidbody.Velocity.y))
			{
				base.specRigidbody.Velocity = Vector2.zero;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					this.m_lastVelocity,
					"|",
					this.m_lastVelocity.magnitude,
					"| NaN correction"
				}));
			}
			if (base.specRigidbody.Velocity.magnitude < this.c_iceVelocityMinClamp)
			{
				base.specRigidbody.Velocity = Vector2.zero;
			}
		}
		if (this.ZeroVelocityThisFrame)
		{
			base.specRigidbody.Velocity = Vector2.zero;
			this.ZeroVelocityThisFrame = false;
		}
		this.HandleFlipping(this.m_currentGunAngle);
		this.HandleAnimations(this.m_playerCommandedDirection, this.m_currentGunAngle);
		if (!this.IsPrimaryPlayer)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
			if (otherPlayer)
			{
				float num7 = -0.55f;
				float heightOffGround = base.sprite.HeightOffGround;
				float z = otherPlayer.sprite.transform.position.z;
				float z2 = base.sprite.transform.position.z;
				if (z == z2)
				{
					if (heightOffGround == num7)
					{
						base.sprite.HeightOffGround = num7 + 0.1f;
					}
					else if (heightOffGround == num7 + 0.1f)
					{
						base.sprite.HeightOffGround = num7;
					}
					base.sprite.UpdateZDepth();
				}
			}
		}
		if (this.IsSlidingOverSurface)
		{
			if (base.sprite.HeightOffGround < 0f)
			{
				base.sprite.HeightOffGround = 1.5f;
			}
		}
		else if (base.sprite.HeightOffGround > 0f)
		{
			base.sprite.HeightOffGround = ((!this.IsPrimaryPlayer) ? (-0.55f) : (-0.5f));
		}
		this.HandleAttachedSpriteDepth(this.m_currentGunAngle);
		this.HandleShellCasingDisplacement();
		base.HandlePitChecks();
		this.HandleRoomProcessing();
		this.HandleGunAttachPoint();
		this.CheckSpawnEmergencyCrate();
		this.CheckSpawnAlertArrows();
		bool flag2 = this.QueryGroundedFrame() && !this.IsFlying;
		if (!this.m_cachedGrounded && flag2 && !this.m_isFalling && this.IsVisible)
		{
			GameManager.Instance.Dungeon.dungeonDustups.InstantiateLandDustup(base.specRigidbody.UnitCenter);
		}
		this.m_cachedGrounded = flag2;
		if (this.m_playerCommandedDirection != Vector2.zero)
		{
			this.m_lastNonzeroCommandedDirection = this.m_playerCommandedDirection;
		}
		base.transform.position = base.transform.position.WithZ(base.transform.position.y - base.sprite.HeightOffGround);
		if (this.CurrentGun != null)
		{
			this.CurrentGun.transform.position = this.CurrentGun.transform.position.WithZ(this.gunAttachPoint.position.z);
		}
		if (this.CurrentSecondaryGun != null && this.SecondaryGunPivot)
		{
			this.CurrentSecondaryGun.transform.position = this.CurrentSecondaryGun.transform.position.WithZ(this.SecondaryGunPivot.position.z);
		}
		bool flag3 = this.m_capableOfStealing.UpdateTimers(BraveTime.DeltaTime);
		if (flag3)
		{
			this.ForceRefreshInteractable = true;
		}
		if (this.m_superDuperAutoAimTimer > 0f)
		{
			this.m_superDuperAutoAimTimer = Mathf.Max(0f, this.m_superDuperAutoAimTimer - BraveTime.DeltaTime);
		}
	}

	// Token: 0x060080F5 RID: 33013 RVA: 0x003411A8 File Offset: 0x0033F3A8
	private void UpdatePlayerShadowPosition()
	{
		GameObject gameObject = base.GenerateDefaultBlobShadow(0f);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localPosition = new Vector3(this.SpriteBottomCenter.x - base.transform.position.x, 0f, 0.1f);
		gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
	}

	// Token: 0x060080F6 RID: 33014 RVA: 0x00341230 File Offset: 0x0033F430
	public void SwapToAlternateCostume(tk2dSpriteAnimation overrideTargetLibrary = null)
	{
		if (this.AlternateCostumeLibrary == null && overrideTargetLibrary == null)
		{
			return;
		}
		if (this.BaseAnimationLibrary != null)
		{
			this.ResetOverrideAnimationLibrary();
		}
		tk2dSpriteAnimation library = base.spriteAnimator.Library;
		base.spriteAnimator.Library = this.AlternateCostumeLibrary;
		this.AlternateCostumeLibrary = library;
		base.spriteAnimator.StopAndResetFrame();
		if (base.spriteAnimator.CurrentClip != null)
		{
			base.spriteAnimator.Play(base.spriteAnimator.CurrentClip.name);
		}
		this.IsUsingAlternateCostume = !this.IsUsingAlternateCostume;
		if (this.HandsOnAltCostume)
		{
			this.ForceHandless = !this.IsUsingAlternateCostume;
		}
		if (this.SwapHandsOnAltCostume)
		{
			this.RevertHandsToBaseType();
			tk2dSpriteCollectionData tk2dSpriteCollectionData = base.sprite.Collection;
			tk2dSpriteAnimationClip clipByName = base.spriteAnimator.Library.GetClipByName(this.GetBaseAnimationName(Vector2.zero, 0f, false, false));
			if (clipByName != null && clipByName.frames != null && clipByName.frames.Length > 0)
			{
				tk2dSpriteCollectionData = clipByName.frames[0].spriteCollection;
			}
			string text = this.altHandName;
			if (this.primaryHand)
			{
				this.altHandName = this.primaryHand.sprite.GetCurrentSpriteDef().name;
				this.primaryHand.sprite.SetSprite(tk2dSpriteCollectionData, text);
			}
			if (this.secondaryHand)
			{
				this.secondaryHand.sprite.SetSprite(tk2dSpriteCollectionData, text);
			}
		}
		if (this.lostAllArmorVFX && this.lostAllArmorAltVfx)
		{
			GameObject gameObject = this.lostAllArmorVFX;
			this.lostAllArmorVFX = this.lostAllArmorAltVfx;
			this.lostAllArmorAltVfx = gameObject;
		}
		this.m_spriteDimensions = base.sprite.GetUntrimmedBounds().size;
		this.UpdatePlayerShadowPosition();
	}

	// Token: 0x060080F7 RID: 33015 RVA: 0x00341428 File Offset: 0x0033F628
	public void RevertHandsToBaseType()
	{
		if (this.m_usingCustomHandType)
		{
			this.m_usingCustomHandType = false;
			if (this.primaryHand)
			{
				this.primaryHand.sprite.SetSprite(this.m_baseHandCollection, this.m_baseHandId);
			}
			if (this.secondaryHand)
			{
				this.secondaryHand.sprite.SetSprite(this.m_baseHandCollection, this.m_baseHandId);
			}
			this.m_baseHandCollection = null;
		}
	}

	// Token: 0x060080F8 RID: 33016 RVA: 0x003414A8 File Offset: 0x0033F6A8
	public void ChangeHandsToCustomType(tk2dSpriteCollectionData handCollection, int handId)
	{
		if (!this.m_usingCustomHandType)
		{
			this.m_baseHandId = this.primaryHand.sprite.spriteId;
			this.m_baseHandCollection = this.primaryHand.sprite.Collection;
		}
		this.m_usingCustomHandType = true;
		if (this.primaryHand)
		{
			this.primaryHand.sprite.SetSprite(handCollection, handId);
		}
		if (this.secondaryHand)
		{
			this.secondaryHand.sprite.SetSprite(handCollection, handId);
		}
	}

	// Token: 0x060080F9 RID: 33017 RVA: 0x00341538 File Offset: 0x0033F738
	private void ResetOverrideAnimationLibrary()
	{
		if (this.BaseAnimationLibrary != null && base.spriteAnimator.Library != this.BaseAnimationLibrary)
		{
			base.spriteAnimator.Library = this.BaseAnimationLibrary;
			base.spriteAnimator.StopAndResetFrame();
			base.spriteAnimator.Play(base.spriteAnimator.CurrentClip.name);
			this.BaseAnimationLibrary = null;
		}
	}

	// Token: 0x060080FA RID: 33018 RVA: 0x003415B0 File Offset: 0x0033F7B0
	private void UpdateTurboModeStats()
	{
		if (GameManager.IsTurboMode)
		{
			if (this.m_turboSpeedModifier == null)
			{
				this.m_turboSpeedModifier = StatModifier.Create(PlayerStats.StatType.MovementSpeed, StatModifier.ModifyMethod.MULTIPLICATIVE, TurboModeController.sPlayerSpeedMultiplier);
				this.m_turboSpeedModifier.ignoredForSaveData = true;
				this.ownerlessStatModifiers.Add(this.m_turboSpeedModifier);
			}
			if (this.m_turboRollSpeedModifier == null)
			{
				this.m_turboRollSpeedModifier = StatModifier.Create(PlayerStats.StatType.DodgeRollSpeedMultiplier, StatModifier.ModifyMethod.MULTIPLICATIVE, TurboModeController.sPlayerRollSpeedMultiplier);
				this.m_turboRollSpeedModifier.ignoredForSaveData = true;
				this.ownerlessStatModifiers.Add(this.m_turboRollSpeedModifier);
			}
			if (this.IsPrimaryPlayer)
			{
				if (this.m_turboEnemyBulletModifier == null)
				{
					this.m_turboEnemyBulletModifier = StatModifier.Create(PlayerStats.StatType.EnemyProjectileSpeedMultiplier, StatModifier.ModifyMethod.MULTIPLICATIVE, TurboModeController.sEnemyBulletSpeedMultiplier);
					this.m_turboEnemyBulletModifier.ignoredForSaveData = true;
					this.ownerlessStatModifiers.Add(this.m_turboEnemyBulletModifier);
					this.stats.RecalculateStats(this, false, false);
				}
			}
			else if (this.m_turboEnemyBulletModifier != null)
			{
				this.ownerlessStatModifiers.Remove(this.m_turboEnemyBulletModifier);
				this.m_turboEnemyBulletModifier = null;
				this.stats.RecalculateStats(this, false, false);
			}
			if ((this.m_turboEnemyBulletModifier != null && this.m_turboEnemyBulletModifier.amount != TurboModeController.sEnemyBulletSpeedMultiplier) || this.m_turboSpeedModifier.amount != TurboModeController.sPlayerSpeedMultiplier || this.m_turboRollSpeedModifier.amount != TurboModeController.sPlayerRollSpeedMultiplier)
			{
				this.m_turboRollSpeedModifier.amount = TurboModeController.sPlayerRollSpeedMultiplier;
				this.m_turboSpeedModifier.amount = TurboModeController.sPlayerSpeedMultiplier;
				this.m_turboEnemyBulletModifier.amount = TurboModeController.sEnemyBulletSpeedMultiplier;
				this.stats.RecalculateStats(this, false, false);
			}
		}
		else if (this.m_turboSpeedModifier != null || this.m_turboEnemyBulletModifier != null || this.m_turboRollSpeedModifier != null)
		{
			this.ownerlessStatModifiers.Remove(this.m_turboEnemyBulletModifier);
			this.m_turboEnemyBulletModifier = null;
			this.ownerlessStatModifiers.Remove(this.m_turboSpeedModifier);
			this.m_turboSpeedModifier = null;
			this.ownerlessStatModifiers.Remove(this.m_turboRollSpeedModifier);
			this.m_turboRollSpeedModifier = null;
			this.stats.RecalculateStats(this, false, false);
		}
	}

	// Token: 0x060080FB RID: 33019 RVA: 0x003417D0 File Offset: 0x0033F9D0
	private void LateUpdate()
	{
		this.UpdateTurboModeStats();
		this.WasPausedThisFrame = false;
		if (!this.m_handleDodgeRollStartThisFrame)
		{
			this.m_timeHeldBlinkButton = 0f;
		}
		if (this.DeferredStatRecalculationRequired)
		{
			this.stats.RecalculateStatsInternal(this);
		}
		this.m_wasTalkingThisFrame = this.IsTalking;
		this.m_lastVelocity = base.specRigidbody.Velocity;
		if (this.IsPrimaryPlayer && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && !this.m_newFloorNoInput && !GameManager.Instance.IsPaused && !Dungeon.IsGenerating && !GameManager.Instance.IsLoadingLevel)
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIME_PLAYED, Time.unscaledDeltaTime);
		}
		if (GameManager.Options.RealtimeReflections)
		{
			base.sprite.renderer.sharedMaterial.SetFloat("_ReflectionYOffset", this.actorReflectionAdditionalOffset);
		}
		if (GameManager.Instance.IsPaused)
		{
			return;
		}
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (this.CurrentRoom == null)
		{
			this.m_isInCombat = false;
		}
		else
		{
			bool isInCombat = this.m_isInCombat;
			this.m_isInCombat = this.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
			if (this.OnEnteredCombat != null && this.m_isInCombat && !isInCombat)
			{
				this.OnEnteredCombat();
			}
		}
		if (!this.IsPrimaryPlayer && this.CharacterUsesRandomGuns != GameManager.Instance.GetOtherPlayer(this).CharacterUsesRandomGuns)
		{
			this.CharacterUsesRandomGuns = GameManager.Instance.GetOtherPlayer(this).CharacterUsesRandomGuns;
		}
		this.UpdateStencilVal();
		if (this.CharacterUsesRandomGuns)
		{
			this.m_gunGameElapsed += BraveTime.DeltaTime;
			if (this.CurrentGun != null && this.CurrentGun.CurrentAmmo == 0)
			{
				this.ChangeToRandomGun();
			}
			else if (this.CurrentRoom != null && this.m_gunGameElapsed > 20f && this.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && this.IsInCombat)
			{
				this.ChangeToRandomGun();
			}
			else if (this.CurrentGun == null && !this.IsGhost && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES && !GameManager.Instance.IsLoadingLevel)
			{
				UnityEngine.Debug.Log("Changing to random gun because we don't have any gun at all!");
				this.ChangeToRandomGun();
			}
		}
		if (base.specRigidbody)
		{
			float magnitude = (base.specRigidbody.Velocity * BraveTime.DeltaTime).magnitude;
			if (this.IsFlying)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.DISTANCE_FLOWN, magnitude);
			}
			else
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.DISTANCE_WALKED, magnitude);
			}
		}
		if (this.characterIdentity != PlayableCharacters.Eevee)
		{
			if (this.OverrideAnimationLibrary != null)
			{
				if (base.spriteAnimator.Library != this.OverrideAnimationLibrary)
				{
					this.BaseAnimationLibrary = base.spriteAnimator.Library;
					base.spriteAnimator.Library = this.OverrideAnimationLibrary;
					base.spriteAnimator.StopAndResetFrame();
					base.spriteAnimator.Play(base.spriteAnimator.CurrentClip.name);
				}
			}
			else if (this.BaseAnimationLibrary != null && base.spriteAnimator.Library != this.BaseAnimationLibrary)
			{
				this.ResetOverrideAnimationLibrary();
			}
		}
		this.CurrentFloorDamageCooldown = Mathf.Max(0f, this.CurrentFloorDamageCooldown - BraveTime.DeltaTime);
		if (this.m_blankCooldownTimer > 0f)
		{
			this.m_blankCooldownTimer = Mathf.Max(0f, this.m_blankCooldownTimer - BraveTime.DeltaTime);
			if (this.IsGhost && this.m_blankCooldownTimer <= 0f)
			{
				this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
			}
		}
		if (this.m_highStressTimer > 0f)
		{
			this.m_highStressTimer -= BraveTime.DeltaTime;
			if (this.m_highStressTimer <= 0f && base.healthHaver)
			{
				base.healthHaver.NextShotKills = false;
			}
		}
		if (!this.IsGhost)
		{
			base.DeregisterOverrideColor("player status effects");
			Color color = new Color(0f, 0f, 0f, 0f);
			Color color2 = this.baseFlatColorOverride;
			float num = 0.25f + Mathf.PingPong(Time.timeSinceLevelLoad, 0.25f);
			GameUIRoot.Instance.SetAmmoCountColor(Color.white, this);
			if (this.CurrentDrainMeterValue > 0f)
			{
				if (this.m_currentGoop == null || !this.m_currentGoop.DrainsAmmo || !this.QueryGroundedFrame())
				{
					this.CurrentDrainMeterValue = Mathf.Max(0f, this.CurrentDrainMeterValue - BraveTime.DeltaTime * 0.1f);
				}
				GameUIRoot.Instance.SetAmmoCountColor(Color.Lerp(Color.white, new Color(1f, 0f, 0f, 1f), this.CurrentDrainMeterValue), this);
				if (this.CurrentDrainMeterValue >= 1f)
				{
					GameUIRoot.Instance.SetAmmoCountColor(new Color(1f, 0f, 0f, 1f), this);
				}
			}
			else
			{
				this.inventory.ClearAmmoDrain();
			}
			color = Color.Lerp(color, new Color(0.65f, 0f, 0.6f, num), this.CurrentDrainMeterValue);
			if (this.IsOnFire && base.healthHaver.GetDamageModifierForType(CoreDamageTypes.Fire) > 0f && !this.IsEthereal && !this.IsTalking && !this.HasActiveBonusSynergy(CustomSynergyType.FIRE_IMMUNITY, false))
			{
				if (!this.IsDodgeRolling)
				{
					this.IncreaseFire(BraveTime.DeltaTime * 0.666666f);
				}
				else
				{
					this.IncreaseFire(BraveTime.DeltaTime * 0.2f);
				}
				if (this.CurrentFireMeterValue >= 1f)
				{
					this.CurrentFireMeterValue -= 1f;
					if (!this.m_isFalling)
					{
						base.healthHaver.ApplyDamage(0.5f, Vector2.zero, StringTableManager.GetEnemiesString("#FIRE", -1), CoreDamageTypes.Fire, DamageCategory.Environment, true, null, false);
					}
					int num2 = 12;
					Vector3 vector = base.specRigidbody.HitboxPixelCollider.UnitBottomLeft.ToVector3ZisY(0f);
					Vector3 vector2 = base.specRigidbody.HitboxPixelCollider.UnitTopRight.ToVector3ZisY(0f);
					float num3 = 15f;
					float num4 = 2.25f;
					float num5 = 1f;
					Color? color3 = new Color?(Color.red);
					GlobalSparksDoer.DoRadialParticleBurst(num2, vector, vector2, num3, num4, num5, null, null, color3, GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
				}
				color2 = new Color(1f, 0f, 0f, 0.7f);
			}
			else
			{
				this.CurrentFireMeterValue = 0f;
				this.IsOnFire = false;
			}
			if (this.CurrentPoisonMeterValue > 0f && base.healthHaver.GetDamageModifierForType(CoreDamageTypes.Poison) > 0f)
			{
				if (this.m_currentGoop == null || !this.m_currentGoop.damagesPlayers || !this.QueryGroundedFrame())
				{
					this.CurrentPoisonMeterValue = Mathf.Max(0f, this.CurrentPoisonMeterValue - BraveTime.DeltaTime * 0.5f);
				}
			}
			else
			{
				this.CurrentPoisonMeterValue = 0f;
			}
			color = Color.Lerp(color, new Color(0f, 1f, 0f, num), this.CurrentPoisonMeterValue);
			if (this.CurrentCurseMeterValue > 0f && this.CurseIsDecaying)
			{
				this.CurrentCurseMeterValue = Mathf.Max(0f, this.CurrentCurseMeterValue - BraveTime.DeltaTime * 0.5f);
			}
			color = Color.Lerp(color, new Color(0f, 0f, 0f, num), this.CurrentCurseMeterValue);
			if (this.CurrentStoneGunTimer > 0f)
			{
				this.CurrentStoneGunTimer -= BraveTime.DeltaTime;
				color2 = new Color(0.4f, 0.4f, 0.33f, Mathf.Clamp01(this.CurrentStoneGunTimer / 0.25f));
			}
			base.RegisterOverrideColor(color, "player status effects");
			if (!this.FlatColorOverridden)
			{
				this.ChangeFlatColorOverride(color2);
			}
			GameUIRoot.Instance.UpdatePlayerHealthUI(this, base.healthHaver);
			GameUIRoot.Instance.UpdatePlayerBlankUI(this);
			if (GameUIRoot.Instance != null)
			{
				GameUIRoot.Instance.UpdateGunData(this.inventory, this.m_equippedGunShift, this);
				GameUIRoot.Instance.UpdateItemData(this, this.CurrentItem, this.activeItems);
				GameUIRoot.Instance.GetReloadBarForPlayer(this).UpdateStatusBars(this);
				for (int i = 0; i < this.activeItems.Count; i++)
				{
					if (this.activeItems[i] == null || !this.activeItems[i])
					{
						UnityEngine.Debug.Log("We have encountered a null item at item index: " + i);
					}
				}
				if (this.CurrentItem == null)
				{
					this.m_selectedItemIndex = 0;
				}
			}
		}
		else
		{
			GameUIRoot.Instance.UpdateGhostUI(this);
			this.ToggleHandRenderers(false, "ghostliness");
			this.IsOnFire = false;
			this.CurrentPoisonMeterValue = 0f;
			this.CurrentFireMeterValue = 0f;
			this.CurrentDrainMeterValue = 0f;
			this.CurrentCurseMeterValue = 0f;
			float num6 = Mathf.Clamp01(this.m_blankCooldownTimer / 5f);
			this.ChangeFlatColorOverride(Color.Lerp(this.m_ghostChargedColor, this.m_ghostUnchargedColor, num6));
			if (this.CurrentInputState != PlayerInputState.NoInput && !GameManager.Instance.MainCameraController.ManualControl)
			{
				if (!GameManager.Instance.MainCameraController.PointIsVisible(base.CenterPosition, 0.05f))
				{
					PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
					IntVector2? intVector = null;
					if (otherPlayer && otherPlayer.CurrentRoom != null)
					{
						CellValidator cellValidator = (IntVector2 p) => GameManager.Instance.MainCameraController.PointIsVisible(p.ToCenterVector2());
						Vector2 vector3 = BraveMathCollege.ClosestPointOnRectangle(base.CenterPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint - GameManager.Instance.MainCameraController.MinVisiblePoint);
						intVector = otherPlayer.CurrentRoom.GetNearestAvailableCell(vector3, new IntVector2?(IntVector2.One * 3), new CellTypes?(CellTypes.FLOOR | CellTypes.PIT), false, cellValidator);
					}
					if (intVector != null)
					{
						LootEngine.DoDefaultPurplePoof(base.CenterPosition, true);
						this.WarpToPoint(intVector.Value.ToVector2() + Vector2.one, false, false);
						LootEngine.DoDefaultPurplePoof(base.CenterPosition, true);
					}
					else
					{
						LootEngine.DoDefaultPurplePoof(base.CenterPosition, true);
						this.ReuniteWithOtherPlayer(GameManager.Instance.GetOtherPlayer(this), false);
						LootEngine.DoDefaultPurplePoof(base.CenterPosition, true);
					}
				}
				else if (!GameManager.Instance.MainCameraController.PointIsVisible(base.CenterPosition, 0f))
				{
					Vector2 vector4 = BraveMathCollege.ClosestPointOnRectangle(base.CenterPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint - GameManager.Instance.MainCameraController.MinVisiblePoint);
					Vector2 vector5 = vector4 - base.CenterPosition;
					IntVector2 intVector2 = (vector5 * 16f).ToIntVector2(VectorConversions.Round);
					base.specRigidbody.ImpartedPixelsToMove = intVector2;
				}
			}
		}
		if (Minimap.Instance != null)
		{
			Minimap.Instance.UpdatePlayerPositionExact(base.transform.position, this, false);
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.HandleCoopSpecificTimers();
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			GameUIRoot.Instance.UpdatePlayerConsumables(this.carriedConsumables);
		}
	}

	// Token: 0x060080FC RID: 33020 RVA: 0x0034243C File Offset: 0x0034063C
	private void SetStencilVal(int v)
	{
		if (base.sprite && base.sprite.renderer)
		{
			base.sprite.renderer.material.SetInt(this.m_stencilID, v);
		}
	}

	// Token: 0x060080FD RID: 33021 RVA: 0x0034248C File Offset: 0x0034068C
	private void UpdateStencilVal()
	{
		if (base.sprite && base.sprite.renderer)
		{
			int @int = base.sprite.renderer.material.GetInt(this.m_stencilID);
			if (@int != 147 && @int != 146)
			{
				this.SetStencilVal(146);
			}
		}
	}

	// Token: 0x060080FE RID: 33022 RVA: 0x003424FC File Offset: 0x003406FC
	public void ChangeSpecialShaderFlag(int flagIndex, float val)
	{
		Vector4 vector = base.healthHaver.bodySprites[0].renderer.material.GetVector(this.m_specialFlagsID);
		vector[flagIndex] = val;
		for (int i = 0; i < base.healthHaver.bodySprites.Count; i++)
		{
			base.healthHaver.bodySprites[i].usesOverrideMaterial = true;
			base.healthHaver.bodySprites[i].renderer.material.SetColor(this.m_specialFlagsID, vector);
		}
		if (this.primaryHand && this.primaryHand.sprite)
		{
			this.primaryHand.sprite.renderer.material.SetColor(this.m_specialFlagsID, vector);
		}
		if (this.secondaryHand && this.secondaryHand.sprite)
		{
			this.secondaryHand.sprite.renderer.material.SetColor(this.m_specialFlagsID, vector);
		}
	}

	// Token: 0x060080FF RID: 33023 RVA: 0x00342634 File Offset: 0x00340834
	public void ChangeFlatColorOverride(Color targetColor)
	{
		for (int i = 0; i < base.healthHaver.bodySprites.Count; i++)
		{
			base.healthHaver.bodySprites[i].usesOverrideMaterial = true;
			base.healthHaver.bodySprites[i].renderer.material.SetColor(this.m_overrideFlatColorID, targetColor);
		}
	}

	// Token: 0x06008100 RID: 33024 RVA: 0x003426A0 File Offset: 0x003408A0
	public void UpdateRandomStartingEquipmentCoop(bool shouldUseRandom)
	{
		if (shouldUseRandom && !this.m_usesRandomStartingEquipment)
		{
			this.m_usesRandomStartingEquipment = true;
			if (GameManager.Instance && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				GameStatsManager.Instance.CurrentEeveeEquipSeed = -1;
			}
			if (GameStatsManager.Instance.CurrentEeveeEquipSeed < 0)
			{
				GameStatsManager.Instance.CurrentEeveeEquipSeed = UnityEngine.Random.Range(1, 10000000);
			}
			this.m_randomStartingEquipmentSeed = GameStatsManager.Instance.CurrentEeveeEquipSeed;
			this.SetUpRandomStartingEquipment();
			this.m_turboEnemyBulletModifier = null;
			this.m_turboRollSpeedModifier = null;
			this.m_turboSpeedModifier = null;
			this.ResetToFactorySettings(false, false, true);
		}
		else if (!shouldUseRandom && this.m_usesRandomStartingEquipment)
		{
			this.m_usesRandomStartingEquipment = false;
			PlayerController component = GameManager.LastUsedCoopPlayerPrefab.GetComponent<PlayerController>();
			this.startingGunIds = new List<int>(component.startingGunIds);
			this.startingAlternateGunIds = new List<int>(component.startingAlternateGunIds);
			this.startingPassiveItemIds = new List<int>(component.startingPassiveItemIds);
			this.startingActiveItemIds = new List<int>(component.startingActiveItemIds);
			this.finalFightGunIds = new List<int>(component.finalFightGunIds);
			this.m_turboEnemyBulletModifier = null;
			this.m_turboRollSpeedModifier = null;
			this.m_turboSpeedModifier = null;
			this.ResetToFactorySettings(false, false, true);
		}
	}

	// Token: 0x06008101 RID: 33025 RVA: 0x003427E4 File Offset: 0x003409E4
	public void ResetToFactorySettings(bool includeFullHeal = false, bool useFinalFightGuns = false, bool forceAllItems = false)
	{
		if (!this.IsDarkSoulsHollow || useFinalFightGuns)
		{
			this.inventory.DestroyAllGuns();
		}
		if (useFinalFightGuns && this.finalFightGunIds != null && this.finalFightGunIds.Count > 0)
		{
			for (int i = 0; i < this.finalFightGunIds.Count; i++)
			{
				if (this.finalFightGunIds[i] >= 0)
				{
					this.inventory.AddGunToInventory(PickupObjectDatabase.GetById(this.finalFightGunIds[i]) as Gun, true);
				}
			}
		}
		else if (this.UsingAlternateStartingGuns)
		{
			for (int j = 0; j < this.startingAlternateGunIds.Count; j++)
			{
				Gun gun = PickupObjectDatabase.GetById(this.startingAlternateGunIds[j]) as Gun;
				if (forceAllItems || includeFullHeal || useFinalFightGuns || gun.PreventStartingOwnerFromDropping)
				{
					Gun gun2 = this.inventory.AddGunToInventory(gun, true);
				}
			}
		}
		else
		{
			for (int k = 0; k < this.startingGunIds.Count; k++)
			{
				Gun gun3 = PickupObjectDatabase.GetById(this.startingGunIds[k]) as Gun;
				if (forceAllItems || includeFullHeal || useFinalFightGuns || gun3.PreventStartingOwnerFromDropping)
				{
					Gun gun4 = this.inventory.AddGunToInventory(gun3, true);
				}
			}
		}
		for (int l = 0; l < this.passiveItems.Count; l++)
		{
			if (!this.passiveItems[l].PersistsOnDeath)
			{
				DebrisObject debrisObject = this.DropPassiveItem(this.passiveItems[l]);
				if (debrisObject != null)
				{
					UnityEngine.Object.Destroy(debrisObject.gameObject);
					l--;
				}
			}
		}
		for (int m = 0; m < this.activeItems.Count; m++)
		{
			if (!this.activeItems[m].PersistsOnDeath)
			{
				DebrisObject debrisObject2 = this.DropActiveItem(this.activeItems[m], 4f, true);
				if (debrisObject2 != null)
				{
					UnityEngine.Object.Destroy(debrisObject2.gameObject);
					m--;
				}
			}
		}
		for (int n = 0; n < this.startingActiveItemIds.Count; n++)
		{
			PlayerItem playerItem = PickupObjectDatabase.GetById(this.startingActiveItemIds[n]) as PlayerItem;
			if (forceAllItems || !playerItem.consumable)
			{
				if (!this.HasActiveItem(playerItem.PickupObjectId))
				{
					if (forceAllItems || includeFullHeal || useFinalFightGuns || playerItem.PreventStartingOwnerFromDropping)
					{
						EncounterTrackable.SuppressNextNotification = true;
						playerItem.Pickup(this);
						EncounterTrackable.SuppressNextNotification = false;
					}
				}
			}
		}
		for (int num = 0; num < this.startingPassiveItemIds.Count; num++)
		{
			PassiveItem passiveItem = PickupObjectDatabase.GetById(this.startingPassiveItemIds[num]) as PassiveItem;
			if (!this.HasPassiveItem(passiveItem.PickupObjectId))
			{
				EncounterTrackable.SuppressNextNotification = true;
				LootEngine.GivePrefabToPlayer(passiveItem.gameObject, this);
				EncounterTrackable.SuppressNextNotification = false;
			}
		}
		if (this.ownerlessStatModifiers != null)
		{
			if (useFinalFightGuns || includeFullHeal)
			{
				this.ownerlessStatModifiers.Clear();
			}
			else
			{
				for (int num2 = 0; num2 < this.ownerlessStatModifiers.Count; num2++)
				{
					if (!this.ownerlessStatModifiers[num2].PersistsOnCoopDeath)
					{
						this.ownerlessStatModifiers.RemoveAt(num2);
						num2--;
					}
				}
			}
		}
		this.stats.RecalculateStats(this, false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(this).stats.RecalculateStats(GameManager.Instance.GetOtherPlayer(this), false, false);
		}
		if (useFinalFightGuns && this.characterIdentity == PlayableCharacters.Robot)
		{
			base.healthHaver.Armor = 6f;
		}
		if (includeFullHeal)
		{
			base.healthHaver.FullHeal();
		}
	}

	// Token: 0x06008102 RID: 33026 RVA: 0x00342C3C File Offset: 0x00340E3C
	private IEnumerator CoopResurrectInternal(Vector3 targetPosition, tk2dSpriteAnimationClip clipToWaitFor, bool isChest = false)
	{
		GameManager.Instance.MainCameraController.IsLerping = true;
		this.m_cloneWaitingForCoopDeath = false;
		this.ForceBlank(5f, 0.5f, true, false, new Vector2?(targetPosition.XY()), false, -1f);
		if (!isChest)
		{
			this.IsCurrentlyCoopReviving = true;
			this.SetInputOverride("revivepause");
			base.PlayEffectOnActor((GameObject)ResourceCache.Acquire("Global VFX/VFX_GhostRevive"), Vector3.zero, true, true, false);
			float elapsed = 0f;
			while (elapsed < 0.75f)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
			this.ClearInputOverride("revivepause");
			this.IsCurrentlyCoopReviving = false;
			GameManager.Instance.MainCameraController.OverrideRecoverySpeed = 7.5f;
			GameManager.Instance.MainCameraController.IsLerping = true;
		}
		this.ChangeSpecialShaderFlag(0, 0f);
		this.IsGhost = false;
		base.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
		this.m_blankCooldownTimer = 0f;
		GameUIRoot.Instance.TransitionToGhostUI(this);
		this.CurrentInputState = PlayerInputState.NoInput;
		this.m_cachedAimDirection = -Vector2.up;
		GameUIRoot.Instance.ReenableCoopPlayerUI(this);
		this.stats.RecalculateStats(this, false, false);
		base.transform.position = targetPosition;
		base.specRigidbody.CollideWithTileMap = true;
		base.specRigidbody.CollideWithOthers = true;
		base.specRigidbody.Reinitialize();
		base.healthHaver.FullHeal();
		if (this.characterIdentity == PlayableCharacters.Robot)
		{
			base.healthHaver.Armor = 6f;
		}
		this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
		this.m_handlingQueuedAnimation = true;
		if (clipToWaitFor != null)
		{
			base.spriteAnimator.Play(clipToWaitFor);
			while (base.spriteAnimator.IsPlaying(clipToWaitFor))
			{
				yield return null;
			}
		}
		this.m_handlingQueuedAnimation = false;
		this.IsVisible = true;
		if (!SpriteOutlineManager.HasOutline(base.sprite))
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, this.outlineColor, 0.1f, 0f, (this.characterIdentity != PlayableCharacters.Eevee) ? SpriteOutlineManager.OutlineType.NORMAL : SpriteOutlineManager.OutlineType.EEVEE);
		}
		this.m_hideRenderers.ClearOverrides();
		this.m_hideGunRenderers.ClearOverrides();
		this.m_hideHandRenderers.ClearOverrides();
		base.ToggleShadowVisiblity(true);
		this.ToggleRenderer(true, string.Empty);
		this.ToggleRenderer(true, "isVisible");
		this.ToggleGunRenderers(true, string.Empty);
		this.ToggleGunRenderers(true, "isVisible");
		this.ToggleHandRenderers(true, string.Empty);
		this.ToggleHandRenderers(true, "isVisible");
		List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody, null, false);
		for (int i = 0; i < overlappingRigidbodies.Count; i++)
		{
			base.specRigidbody.RegisterGhostCollisionException(overlappingRigidbodies[i]);
			overlappingRigidbodies[i].RegisterGhostCollisionException(base.specRigidbody);
		}
		this.m_isFalling = false;
		this.ClearDodgeRollState();
		this.previousMineCart = null;
		this.m_interruptingPitRespawn = false;
		GameManager.Instance.GetOtherPlayer(this).stats.RecalculateStats(GameManager.Instance.GetOtherPlayer(this), false, false);
		this.CurrentInputState = PlayerInputState.AllInput;
		base.healthHaver.IsVulnerable = true;
		yield break;
	}

	// Token: 0x06008103 RID: 33027 RVA: 0x00342C6C File Offset: 0x00340E6C
	public virtual void ResurrectFromBossKill()
	{
		PlayerController playerController = ((!(GameManager.Instance.PrimaryPlayer == this)) ? GameManager.Instance.PrimaryPlayer : GameManager.Instance.SecondaryPlayer);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip;
		if (base.spriteAnimator.GetClipByName("chest_recover") != null)
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName((!this.UseArmorlessAnim) ? "chest_recover" : "chest_recover_armorless");
		}
		else
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName((!this.UseArmorlessAnim) ? "pitfall_return" : "pitfall_return_armorless");
		}
		Chest.ToggleCoopChests(false);
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		CellData cellData = GameManager.Instance.Dungeon.data[intVector];
		Vector3 vector = base.transform.position;
		if (cellData == null || cellData.type != CellType.FLOOR || cellData.IsPlayerInaccessible)
		{
			vector = playerController.CurrentRoom.GetBestRewardLocation(IntVector2.One, RoomHandler.RewardLocationStyle.PlayerCenter, true).ToVector3();
		}
		base.StartCoroutine(this.CoopResurrectInternal(vector, tk2dSpriteAnimationClip, false));
	}

	// Token: 0x06008104 RID: 33028 RVA: 0x00342DB4 File Offset: 0x00340FB4
	public void ResurrectFromChest(Vector2 chestBottomCenter)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip;
		if (base.spriteAnimator.GetClipByName("chest_recover") != null)
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName((!this.UseArmorlessAnim) ? "chest_recover" : "chest_recover_armorless");
		}
		else
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName((!this.UseArmorlessAnim) ? "pitfall_return" : "pitfall_return_armorless");
		}
		Chest.ToggleCoopChests(false);
		if (this.confettiPaths == null)
		{
			this.confettiPaths = new string[] { "Global VFX/Confetti_Blue_001", "Global VFX/Confetti_Yellow_001", "Global VFX/Confetti_Green_001" };
		}
		Vector2 vector = chestBottomCenter + new Vector2(-0.75f, -0.25f);
		for (int i = 0; i < 8; i++)
		{
			GameObject gameObject = (GameObject)ResourceCache.Acquire(this.confettiPaths[UnityEngine.Random.Range(0, 3)]);
			WaftingDebrisObject component = UnityEngine.Object.Instantiate<GameObject>(gameObject).GetComponent<WaftingDebrisObject>();
			component.sprite.PlaceAtPositionByAnchor(vector.ToVector3ZUp(0f) + new Vector3(0.5f, 0.5f, 0f), tk2dBaseSprite.Anchor.MiddleCenter);
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			insideUnitCircle.y = -Mathf.Abs(insideUnitCircle.y);
			component.Trigger(insideUnitCircle.ToVector3ZUp(1.5f) * UnityEngine.Random.Range(0.5f, 2f), 0.5f, 0f);
		}
		base.StartCoroutine(this.CoopResurrectInternal(vector.ToVector3ZUp(0f), tk2dSpriteAnimationClip, true));
	}

	// Token: 0x06008105 RID: 33029 RVA: 0x00342F44 File Offset: 0x00341144
	private void HandleCoopSpecificTimers()
	{
		PlayerController playerController = ((!this.IsPrimaryPlayer) ? GameManager.Instance.PrimaryPlayer : GameManager.Instance.SecondaryPlayer);
		if (playerController != null && !playerController.healthHaver.IsDead && playerController.CurrentRoom != null && playerController.CurrentRoom.IsSealed && playerController.CurrentRoom != this.CurrentRoom)
		{
			this.m_coopRoomTimer += BraveTime.DeltaTime;
			if (this.m_coopRoomTimer > 1f)
			{
				if (this.IsGhost)
				{
					LootEngine.DoDefaultPurplePoof(base.CenterPosition, true);
				}
				this.ReuniteWithOtherPlayer(playerController, false);
				if (this.IsGhost)
				{
					LootEngine.DoDefaultPurplePoof(base.CenterPosition, true);
				}
				base.healthHaver.TriggerInvulnerabilityPeriod(-1f);
				this.m_coopRoomTimer = 0f;
			}
		}
		else
		{
			this.m_coopRoomTimer = 0f;
		}
	}

	// Token: 0x06008106 RID: 33030 RVA: 0x00343044 File Offset: 0x00341244
	public void DoPostProcessProjectile(Projectile p)
	{
		p.Owner = this;
		this.HandleShadowBulletStat(p);
		float num = 1f;
		if (this.CurrentGun && this.CurrentGun.DefaultModule != null)
		{
			float num2 = 0f;
			if (this.CurrentGun.Volley != null)
			{
				List<ProjectileModule> projectiles = this.CurrentGun.Volley.projectiles;
				for (int i = 0; i < projectiles.Count; i++)
				{
					num2 += projectiles[i].GetEstimatedShotsPerSecond(this.CurrentGun.reloadTime);
				}
			}
			else if (this.CurrentGun.DefaultModule != null)
			{
				num2 += this.CurrentGun.DefaultModule.GetEstimatedShotsPerSecond(this.CurrentGun.reloadTime);
			}
			if (num2 > 0f)
			{
				num = 3.5f / num2;
			}
		}
		if (this.PostProcessProjectile != null)
		{
			this.PostProcessProjectile(p, num);
		}
	}

	// Token: 0x06008107 RID: 33031 RVA: 0x00343144 File Offset: 0x00341344
	public void CustomPostProcessProjectile(Projectile p, float effectChanceScalar)
	{
		if (this.PostProcessProjectile != null)
		{
			this.PostProcessProjectile(p, effectChanceScalar);
		}
	}

	// Token: 0x06008108 RID: 33032 RVA: 0x00343160 File Offset: 0x00341360
	public void DoPostProcessThrownGun(Projectile p)
	{
		if (this.PostProcessThrownGun != null)
		{
			this.PostProcessThrownGun(p);
		}
	}

	// Token: 0x06008109 RID: 33033 RVA: 0x0034317C File Offset: 0x0034137C
	public void SpawnShadowBullet(Projectile obj, bool shadowColoration)
	{
		float num = 0f;
		if (obj.sprite && obj.sprite.GetBounds().size.x > 0.5f)
		{
			num += obj.sprite.GetBounds().size.x / 10f;
		}
		num = Mathf.Max(num, 0.1f);
		base.StartCoroutine(this.SpawnShadowBullet(obj, num, shadowColoration));
	}

	// Token: 0x0600810A RID: 33034 RVA: 0x00343208 File Offset: 0x00341408
	protected IEnumerator SpawnShadowBullet(Projectile obj, float additionalDelay, bool shadowColoration)
	{
		Vector3 cachedSpawnPosition = obj.transform.position;
		Quaternion cachedSpawnRotation = obj.transform.rotation;
		if (additionalDelay > 0f)
		{
			float ela = 0f;
			while (ela < additionalDelay)
			{
				ela += BraveTime.DeltaTime;
				yield return null;
			}
		}
		if (obj)
		{
			bool flag = false;
			if (this.HasActiveBonusSynergy(CustomSynergyType.MR_SHADOW, false) && this.CurrentGun && this.CurrentGun.DefaultModule.usesOptionalFinalProjectile)
			{
				Projectile projectile = this.CurrentGun.DefaultModule.finalProjectile;
				if (this.CurrentGun.DefaultModule.finalVolley != null)
				{
					projectile = this.CurrentGun.DefaultModule.finalVolley.projectiles[0].GetCurrentProjectile();
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(projectile.gameObject, cachedSpawnPosition, cachedSpawnRotation);
				gameObject.transform.position += gameObject.transform.right * -0.5f;
				Projectile component = gameObject.GetComponent<Projectile>();
				component.specRigidbody.Reinitialize();
				component.collidesWithPlayer = false;
				component.Owner = obj.Owner;
				component.Shooter = obj.Shooter;
				if (shadowColoration)
				{
					component.ChangeColor(0f, new Color(0.35f, 0.25f, 0.65f, 1f));
				}
				flag = true;
			}
			if (!flag)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(obj.gameObject, cachedSpawnPosition, cachedSpawnRotation);
				gameObject2.transform.position += gameObject2.transform.right * -0.5f;
				Projectile component2 = gameObject2.GetComponent<Projectile>();
				component2.specRigidbody.Reinitialize();
				component2.collidesWithPlayer = false;
				component2.Owner = obj.Owner;
				component2.Shooter = obj.Shooter;
				component2.baseData.damage = obj.baseData.damage;
				component2.baseData.range = obj.baseData.range;
				component2.baseData.speed = obj.baseData.speed;
				component2.baseData.force = obj.baseData.force;
				if (shadowColoration)
				{
					component2.ChangeColor(0f, new Color(0.35f, 0.25f, 0.65f, 1f));
				}
			}
		}
		yield break;
	}

	// Token: 0x0600810B RID: 33035 RVA: 0x00343238 File Offset: 0x00341438
	protected void HandleShadowBulletStat(Projectile obj)
	{
		float num = this.stats.GetStatValue(PlayerStats.StatType.ExtremeShadowBulletChance) / 100f;
		if (UnityEngine.Random.value < num)
		{
			base.StartCoroutine(this.SpawnShadowBullet(obj, 0.05f, false));
			if (UnityEngine.Random.value < 0.5f)
			{
				base.StartCoroutine(this.SpawnShadowBullet(obj, 0.1f, false));
				if (UnityEngine.Random.value < 0.5f)
				{
					base.StartCoroutine(this.SpawnShadowBullet(obj, 0.15f, false));
					if (UnityEngine.Random.value < 0.5f)
					{
						base.StartCoroutine(this.SpawnShadowBullet(obj, 0.2f, false));
					}
				}
			}
			return;
		}
		float num2 = this.stats.GetStatValue(PlayerStats.StatType.ShadowBulletChance) / 100f;
		if (UnityEngine.Random.value < num2)
		{
			this.SpawnShadowBullet(obj, true);
		}
	}

	// Token: 0x0600810C RID: 33036 RVA: 0x0034330C File Offset: 0x0034150C
	public void DoPostProcessBeam(BeamController beam)
	{
		int num = Mathf.FloorToInt(this.stats.GetStatValue(PlayerStats.StatType.AdditionalShotBounces));
		int num2 = Mathf.FloorToInt(this.stats.GetStatValue(PlayerStats.StatType.AdditionalShotPiercing));
		if ((num > 0 || num2 > 0) && beam is BasicBeamController)
		{
			BasicBeamController basicBeamController = beam as BasicBeamController;
			if (!basicBeamController.playerStatsModified)
			{
				basicBeamController.penetration += num2;
				basicBeamController.reflections += num;
				basicBeamController.playerStatsModified = true;
			}
		}
		if (this.PostProcessBeam != null)
		{
			this.PostProcessBeam(beam);
		}
	}

	// Token: 0x0600810D RID: 33037 RVA: 0x003433A4 File Offset: 0x003415A4
	public void DoPostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidbody, float tickRate)
	{
		if (beam && beam.projectile && beam.projectile.baseData.damage == 0f)
		{
			return;
		}
		if (this.PostProcessBeamTick != null)
		{
			this.PostProcessBeamTick(beam, hitRigidbody, tickRate);
		}
	}

	// Token: 0x0600810E RID: 33038 RVA: 0x00343400 File Offset: 0x00341600
	public void DoPostProcessBeamChanceTick(BeamController beam)
	{
		if (this.PostProcessBeamChanceTick != null)
		{
			this.PostProcessBeamChanceTick(beam);
		}
	}

	// Token: 0x0600810F RID: 33039 RVA: 0x0034341C File Offset: 0x0034161C
	public Material[] SetOverrideShader(Shader overrideShader)
	{
		if (this.m_cachedOverrideMaterials == null)
		{
			this.m_cachedOverrideMaterials = new Material[3];
		}
		for (int i = 0; i < this.m_cachedOverrideMaterials.Length; i++)
		{
			this.m_cachedOverrideMaterials[i] = null;
		}
		base.sprite.renderer.material.shader = overrideShader;
		this.m_cachedOverrideMaterials[0] = base.sprite.renderer.material;
		if (this.primaryHand && this.primaryHand.sprite)
		{
			this.m_cachedOverrideMaterials[1] = this.primaryHand.SetOverrideShader(overrideShader);
		}
		if (this.secondaryHand && this.secondaryHand.sprite)
		{
			this.m_cachedOverrideMaterials[2] = this.secondaryHand.SetOverrideShader(overrideShader);
		}
		return this.m_cachedOverrideMaterials;
	}

	// Token: 0x1700133A RID: 4922
	// (get) Token: 0x06008110 RID: 33040 RVA: 0x00343508 File Offset: 0x00341708
	public static string DefaultShaderName
	{
		get
		{
			if (!GameOptions.SupportsStencil)
			{
				return "Brave/PlayerShaderNoStencil";
			}
			return "Brave/PlayerShader";
		}
	}

	// Token: 0x1700133B RID: 4923
	// (get) Token: 0x06008111 RID: 33041 RVA: 0x00343520 File Offset: 0x00341720
	public string LocalShaderName
	{
		get
		{
			if (!GameOptions.SupportsStencil)
			{
				return "Brave/PlayerShaderNoStencil";
			}
			if (this.characterIdentity == PlayableCharacters.Eevee || this.IsTemporaryEeveeForUnlock)
			{
				return "Brave/PlayerShaderEevee";
			}
			return "Brave/PlayerShader";
		}
	}

	// Token: 0x06008112 RID: 33042 RVA: 0x00343558 File Offset: 0x00341758
	public void ClearOverrideShader()
	{
		if (this && base.sprite && base.sprite.renderer && base.sprite.renderer.material)
		{
			base.sprite.renderer.material.shader = ShaderCache.Acquire(this.LocalShaderName);
		}
		if (this.primaryHand && this.primaryHand.sprite)
		{
			this.primaryHand.ClearOverrideShader();
		}
		if (this.secondaryHand && this.secondaryHand.sprite)
		{
			this.secondaryHand.ClearOverrideShader();
		}
	}

	// Token: 0x06008113 RID: 33043 RVA: 0x00343630 File Offset: 0x00341830
	public void Reinitialize()
	{
		base.specRigidbody.Reinitialize();
		this.WarpFollowersToPlayer(false);
	}

	// Token: 0x06008114 RID: 33044 RVA: 0x00343644 File Offset: 0x00341844
	public void ReinitializeGuns()
	{
		this.inventory.DestroyAllGuns();
		List<int> list = this.startingGunIds;
		if (this.UsingAlternateStartingGuns)
		{
			list = this.startingAlternateGunIds;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Gun gun = PickupObjectDatabase.GetById(list[i]) as Gun;
			if (gun.encounterTrackable)
			{
				EncounterTrackable.SuppressNextNotification = true;
				gun.encounterTrackable.HandleEncounter();
				EncounterTrackable.SuppressNextNotification = false;
			}
			Gun gun2 = this.inventory.AddGunToInventory(gun, true);
		}
		this.inventory.ChangeGun(1, false, false);
	}

	// Token: 0x06008115 RID: 33045 RVA: 0x003436E4 File Offset: 0x003418E4
	private void InitializeInventory()
	{
		this.inventory = new GunInventory(this);
		this.inventory.maxGuns = this.MAX_GUNS_HELD + (int)this.stats.GetStatValue(PlayerStats.StatType.AdditionalGunCapacity);
		this.inventory.maxGuns = int.MaxValue;
		if (this.CharacterUsesRandomGuns)
		{
			this.inventory.maxGuns = 1;
		}
		List<int> list = this.startingGunIds;
		if (this.UsingAlternateStartingGuns)
		{
			list = this.startingAlternateGunIds;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Gun gun = PickupObjectDatabase.GetById(list[i]) as Gun;
			if (gun.encounterTrackable)
			{
				EncounterTrackable.SuppressNextNotification = true;
				gun.encounterTrackable.HandleEncounter();
				EncounterTrackable.SuppressNextNotification = false;
			}
			Gun gun2 = this.inventory.AddGunToInventory(gun, true);
		}
		this.inventory.ChangeGun(1, false, false);
		if (!this.m_usesRandomStartingEquipment || GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
		{
			for (int j = 0; j < this.startingPassiveItemIds.Count; j++)
			{
				this.AcquirePassiveItemPrefabDirectly(PickupObjectDatabase.GetById(this.startingPassiveItemIds[j]) as PassiveItem);
			}
			for (int k = 0; k < this.startingActiveItemIds.Count; k++)
			{
				EncounterTrackable.SuppressNextNotification = true;
				PlayerItem playerItem = PickupObjectDatabase.GetById(this.startingActiveItemIds[k]) as PlayerItem;
				playerItem.Pickup(this);
				EncounterTrackable.SuppressNextNotification = false;
			}
		}
	}

	// Token: 0x06008116 RID: 33046 RVA: 0x00343870 File Offset: 0x00341A70
	public DebrisObject ForceDropGun(Gun g)
	{
		if (!g.CanActuallyBeDropped(this))
		{
			return null;
		}
		if (this.inventory.GunLocked.Value)
		{
			return null;
		}
		bool flag = g == this.CurrentGun;
		g.HasEverBeenAcquiredByPlayer = true;
		this.inventory.RemoveGunFromInventory(g);
		g.ToggleRenderers(true);
		DebrisObject debrisObject = g.DropGun(0.5f);
		if (flag)
		{
			this.ProcessHandAttachment();
		}
		return debrisObject;
	}

	// Token: 0x06008117 RID: 33047 RVA: 0x003438E4 File Offset: 0x00341AE4
	public void UpdateInventoryMaxGuns()
	{
		if (this.inventory != null)
		{
			if (this.inventory.maxGuns > 1000)
			{
				return;
			}
			this.inventory.maxGuns = this.MAX_GUNS_HELD + (int)this.stats.GetStatValue(PlayerStats.StatType.AdditionalGunCapacity);
			this.inventory.maxGuns = int.MaxValue;
			while (this.inventory.maxGuns < this.inventory.GunCountModified)
			{
				Gun currentGun = this.CurrentGun;
				currentGun.HasEverBeenAcquiredByPlayer = true;
				this.inventory.RemoveGunFromInventory(currentGun);
				currentGun.DropGun(0.5f);
			}
		}
	}

	// Token: 0x06008118 RID: 33048 RVA: 0x00343988 File Offset: 0x00341B88
	public void UpdateInventoryMaxItems()
	{
		if (this.activeItems != null)
		{
			this.maxActiveItemsHeld = this.MAX_ITEMS_HELD + (int)this.stats.GetStatValue(PlayerStats.StatType.AdditionalItemCapacity);
			while (this.maxActiveItemsHeld < this.activeItems.Count)
			{
				this.DropActiveItem(this.activeItems[this.activeItems.Count - 1], 4f, false);
			}
		}
	}

	// Token: 0x06008119 RID: 33049 RVA: 0x003439FC File Offset: 0x00341BFC
	public void ResetTarnisherClipCapacity()
	{
		for (int i = this.ownerlessStatModifiers.Count - 1; i >= 0; i--)
		{
			if (this.ownerlessStatModifiers[i].statToBoost == PlayerStats.StatType.TarnisherClipCapacityMultiplier)
			{
				this.ownerlessStatModifiers.RemoveAt(i);
			}
		}
		this.stats.RecalculateStats(this, false, false);
	}

	// Token: 0x0600811A RID: 33050 RVA: 0x00343A5C File Offset: 0x00341C5C
	public void ChangeAttachedSpriteDepth(tk2dBaseSprite targetSprite, float targetDepth)
	{
		if (this.m_attachedSprites.Contains(targetSprite))
		{
			int num = this.m_attachedSprites.IndexOf(targetSprite);
			this.m_attachedSpriteDepths[num] = targetDepth;
		}
	}

	// Token: 0x0600811B RID: 33051 RVA: 0x00343A94 File Offset: 0x00341C94
	public GameObject RegisterAttachedObject(GameObject prefab, string attachPoint, float depth = 0f)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		if (!string.IsNullOrEmpty(attachPoint))
		{
			tk2dSpriteAttachPoint orAddComponent = base.sprite.gameObject.GetOrAddComponent<tk2dSpriteAttachPoint>();
			gameObject.transform.parent = orAddComponent.GetAttachPointByName(attachPoint);
		}
		else
		{
			gameObject.transform.parent = base.sprite.transform;
		}
		gameObject.transform.localPosition = Vector3.zero;
		if (gameObject.transform.parent == null)
		{
			UnityEngine.Debug.LogError("FAILED TO FIND ATTACHPOINT " + attachPoint + " ON PLAYER");
		}
		tk2dBaseSprite tk2dBaseSprite = gameObject.GetComponent<tk2dBaseSprite>();
		if (tk2dBaseSprite == null)
		{
			tk2dBaseSprite = gameObject.GetComponentInChildren<tk2dBaseSprite>();
		}
		base.sprite.AttachRenderer(tk2dBaseSprite);
		tk2dBaseSprite[] componentsInChildren = gameObject.GetComponentsInChildren<tk2dBaseSprite>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.m_attachedSprites.Add(componentsInChildren[i]);
			this.m_attachedSpriteDepths.Add(depth);
		}
		return gameObject;
	}

	// Token: 0x0600811C RID: 33052 RVA: 0x00343B90 File Offset: 0x00341D90
	public void DeregisterAttachedObject(GameObject instance, bool completeDestruction = true)
	{
		if (!instance)
		{
			return;
		}
		tk2dBaseSprite[] componentsInChildren = instance.GetComponentsInChildren<tk2dBaseSprite>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i])
			{
				this.m_attachedSpriteDepths.RemoveAt(this.m_attachedSprites.IndexOf(componentsInChildren[i]));
				this.m_attachedSprites.Remove(componentsInChildren[i]);
			}
		}
		if (completeDestruction)
		{
			UnityEngine.Object.Destroy(instance);
		}
		else
		{
			instance.transform.parent = null;
		}
	}

	// Token: 0x0600811D RID: 33053 RVA: 0x00343C1C File Offset: 0x00341E1C
	public void ForceStaticFaceDirection(Vector2 dir)
	{
		this.m_lastNonzeroCommandedDirection = dir;
		this.unadjustedAimPoint = base.CenterPosition + dir.normalized * 5f;
	}

	// Token: 0x0600811E RID: 33054 RVA: 0x00343C4C File Offset: 0x00341E4C
	public void ForceIdleFacePoint(Vector2 dir, bool quadrantize = true)
	{
		float num = ((!quadrantize) ? BraveMathCollege.Atan2Degrees(dir) : ((float)(BraveMathCollege.VectorToQuadrant(dir) * 90)));
		string baseAnimationName = this.GetBaseAnimationName(Vector2.zero, num, false, false);
		if (!base.spriteAnimator.IsPlaying(baseAnimationName))
		{
			base.spriteAnimator.Play(baseAnimationName);
		}
		base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
		this.m_currentGunAngle = num;
		this.ForceStaticFaceDirection(dir);
		if (this.CurrentGun)
		{
			this.CurrentGun.HandleAimRotation(base.CenterPosition + dir, false, 1f);
		}
	}

	// Token: 0x0600811F RID: 33055 RVA: 0x00343CF4 File Offset: 0x00341EF4
	public void TeleportToPoint(Vector2 targetPoint, bool useDefaultTeleportVFX)
	{
		if (this.m_isStartingTeleport)
		{
			return;
		}
		this.m_isStartingTeleport = true;
		GameObject gameObject = null;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
			if (otherPlayer)
			{
				otherPlayer.TeleportToPoint(targetPoint, useDefaultTeleportVFX);
			}
		}
		this.m_isStartingTeleport = false;
		if (useDefaultTeleportVFX)
		{
			gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
		}
		this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
		base.StartCoroutine(this.HandleTeleportToPoint(targetPoint, gameObject, null, gameObject));
	}

	// Token: 0x06008120 RID: 33056 RVA: 0x00343D80 File Offset: 0x00341F80
	private IEnumerator HandleTeleportToPoint(Vector2 targetPoint, GameObject departureVFXPrefab, GameObject arrivalVFX1Prefab, GameObject arrivalVFX2Prefab)
	{
		base.healthHaver.IsVulnerable = false;
		CameraController cameraController = GameManager.Instance.MainCameraController;
		Vector2 offsetVector = cameraController.transform.position - base.transform.position;
		offsetVector -= cameraController.GetAimContribution();
		Minimap.Instance.ToggleMinimap(false, false);
		cameraController.SetManualControl(true, false);
		cameraController.OverridePosition = cameraController.transform.position;
		this.CurrentInputState = PlayerInputState.NoInput;
		yield return new WaitForSeconds(0.1f);
		this.ToggleRenderer(false, "arbitrary teleporter");
		this.ToggleGunRenderers(false, "arbitrary teleporter");
		this.ToggleHandRenderers(false, "arbitrary teleporter");
		if (departureVFXPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(departureVFXPrefab);
			gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(base.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
			gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
			gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		}
		yield return new WaitForSeconds(0.4f);
		Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
		yield return new WaitForSeconds(0.1f);
		base.specRigidbody.Position = new Position(targetPoint);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			cameraController.OverridePosition = cameraController.GetIdealCameraPosition();
		}
		else
		{
			cameraController.OverridePosition = (targetPoint + offsetVector).ToVector3ZUp(0f);
		}
		Pixelator.Instance.MarkOcclusionDirty();
		if (arrivalVFX1Prefab != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(arrivalVFX1Prefab);
			gameObject2.transform.position = targetPoint;
			gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
		}
		Pixelator.Instance.FadeToBlack(0.1f, true, 0f);
		yield return null;
		cameraController.SetManualControl(false, true);
		yield return new WaitForSeconds(0.75f);
		if (arrivalVFX2Prefab != null)
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(arrivalVFX2Prefab);
			gameObject3.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(base.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
			gameObject3.transform.position = gameObject3.transform.position.Quantize(0.0625f);
			gameObject3.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		}
		this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
		yield return new WaitForSeconds(0.25f);
		this.ToggleRenderer(true, "arbitrary teleporter");
		this.ToggleGunRenderers(true, "arbitrary teleporter");
		this.ToggleHandRenderers(true, "arbitrary teleporter");
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		this.CurrentInputState = PlayerInputState.AllInput;
		base.healthHaver.IsVulnerable = true;
		yield break;
	}

	// Token: 0x06008121 RID: 33057 RVA: 0x00343DB8 File Offset: 0x00341FB8
	public bool IsPositionObscuredByTopWall(Vector2 newPosition)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num = newPosition.ToIntVector2(VectorConversions.Floor).x + i;
				int num2 = newPosition.ToIntVector2(VectorConversions.Floor).y + j;
				if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(newPosition.ToIntVector2(VectorConversions.Floor) + new IntVector2(i, j)) && (GameManager.Instance.Dungeon.data.isTopWall(num, num2) || GameManager.Instance.Dungeon.data.isWall(num, num2)))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06008122 RID: 33058 RVA: 0x00343E78 File Offset: 0x00342078
	public bool IsValidPlayerPosition(Vector2 newPosition)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(newPosition.ToIntVector2(VectorConversions.Floor) + new IntVector2(i, j)))
				{
					return false;
				}
			}
		}
		int num = CollisionMask.LayerToMask(CollisionLayer.EnemyCollider, CollisionLayer.EnemyHitBox, CollisionLayer.Projectile);
		Func<SpeculativeRigidbody, bool> func = (SpeculativeRigidbody rigidbody) => rigidbody.minorBreakable;
		PhysicsEngine instance = PhysicsEngine.Instance;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		List<CollisionData> list = null;
		bool flag = true;
		bool flag2 = true;
		int? num2 = null;
		int? num3 = new int?(num);
		Func<SpeculativeRigidbody, bool> func2 = func;
		bool flag3 = instance.OverlapCast(specRigidbody, list, flag, flag2, num2, num3, false, new Vector2?(newPosition), func2, new SpeculativeRigidbody[0]);
		return !flag3;
	}

	// Token: 0x06008123 RID: 33059 RVA: 0x00343F54 File Offset: 0x00342154
	public void WarpFollowersToPlayer(bool excludeCompanions = false)
	{
		for (int i = 0; i < this.orbitals.Count; i++)
		{
			this.orbitals[i].GetTransform().position = base.transform.position;
			this.orbitals[i].Reinitialize();
		}
		for (int j = 0; j < this.trailOrbitals.Count; j++)
		{
			this.trailOrbitals[j].transform.position = base.transform.position;
			this.trailOrbitals[j].specRigidbody.Reinitialize();
		}
		if (!excludeCompanions)
		{
			this.WarpCompanionsToPlayer(false);
		}
	}

	// Token: 0x06008124 RID: 33060 RVA: 0x00344010 File Offset: 0x00342210
	public void WarpCompanionsToPlayer(bool isRoomSealWarp = false)
	{
		Vector3 vector = base.transform.position;
		if (this.InExitCell && this.CurrentRoom != null)
		{
			vector = this.CurrentRoom.GetBestRewardLocation(new IntVector2(2, 2), RoomHandler.RewardLocationStyle.PlayerCenter, false).ToVector3() + new Vector3(1f, 1f, 0f);
		}
		for (int i = 0; i < this.companions.Count; i++)
		{
			Vector3 vector2 = vector;
			if (isRoomSealWarp && this.companions[i].CompanionSettings.WarpsToRandomPoint)
			{
				IntVector2? randomAvailableCell = this.CurrentRoom.GetRandomAvailableCell(new IntVector2?(this.companions[i].Clearance * 3), new CellTypes?(CellTypes.FLOOR), false, new CellValidator(Pathfinder.CellValidator_NoTopWalls));
				if (randomAvailableCell != null)
				{
					vector2 = (randomAvailableCell.Value + IntVector2.One).ToVector3();
				}
			}
			this.companions[i].CompanionWarp(vector2);
		}
	}

	// Token: 0x06008125 RID: 33061 RVA: 0x0034413C File Offset: 0x0034233C
	public void WarpToPointAndBringCoopPartner(Vector2 targetPoint, bool useDefaultPoof = false, bool doFollowers = false)
	{
		this.WarpToPoint(targetPoint, useDefaultPoof, doFollowers);
		PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
		if (otherPlayer)
		{
			Vector2 vector = base.specRigidbody.UnitBottomLeft - base.transform.position.XY();
			Vector2 vector2 = otherPlayer.specRigidbody.UnitBottomLeft - otherPlayer.transform.position.XY();
			otherPlayer.WarpToPoint(targetPoint + (vector - vector2), useDefaultPoof, doFollowers);
		}
	}

	// Token: 0x06008126 RID: 33062 RVA: 0x003441C0 File Offset: 0x003423C0
	public void WarpToPoint(Vector2 targetPoint, bool useDefaultPoof = false, bool doFollowers = false)
	{
		if (useDefaultPoof)
		{
			LootEngine.DoDefaultItemPoof(base.CenterPosition, true, false);
		}
		base.transform.position = targetPoint;
		base.specRigidbody.Reinitialize();
		base.specRigidbody.RecheckTriggers = true;
		if (this.CurrentItem && this.CurrentItem is GrapplingHookItem)
		{
			GrapplingHookItem grapplingHookItem = this.CurrentItem as GrapplingHookItem;
			if (grapplingHookItem && grapplingHookItem.IsActive)
			{
				float num = -1f;
				grapplingHookItem.Use(this, out num);
			}
		}
		if (doFollowers)
		{
			this.WarpFollowersToPlayer(false);
		}
	}

	// Token: 0x06008127 RID: 33063 RVA: 0x00344268 File Offset: 0x00342468
	public void AttemptTeleportToRoom(RoomHandler targetRoom, bool force = false, bool noFX = false)
	{
		if (this.IsInMinecart)
		{
			return;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
			if (otherPlayer && otherPlayer.IsInMinecart)
			{
				return;
			}
		}
		bool flag = this.CurrentRoom != null && this.CurrentRoom.CanTeleportFromRoom() && targetRoom != null && targetRoom.CanTeleportToRoom();
		if (GameManager.Instance.InTutorial && !flag && this.CurrentRoom == targetRoom && targetRoom.GetRoomName().Equals("Tutorial_Room_0065_teleporter", StringComparison.OrdinalIgnoreCase))
		{
			flag = true;
		}
		if (force)
		{
			flag = true;
		}
		if (flag)
		{
			if (this.OnDidUnstealthyAction != null)
			{
				this.OnDidUnstealthyAction(this);
			}
			AkSoundEngine.PostEvent("Play_OBJ_teleport_depart_01", base.gameObject);
			this.m_cachedTeleportSpot = ((!force) ? base.specRigidbody.Position.UnitPosition : this.CurrentRoom.GetCenteredVisibleClearSpot(2, 2).ToVector2());
			targetRoom.SetRoomActive(true);
			TeleporterController teleporterController = ((!targetRoom.hierarchyParent) ? null : targetRoom.hierarchyParent.GetComponentInChildren<TeleporterController>(true));
			if (!teleporterController)
			{
				List<TeleporterController> componentsInRoom = targetRoom.GetComponentsInRoom<TeleporterController>();
				if (componentsInRoom.Count > 0)
				{
					teleporterController = componentsInRoom[0];
				}
			}
			Vector2 vector;
			if (teleporterController)
			{
				vector = teleporterController.sprite.WorldCenter;
			}
			else
			{
				IntVector2? randomAvailableCell = targetRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), new CellTypes?(CellTypes.FLOOR), false, null);
				vector = ((randomAvailableCell == null) ? targetRoom.GetCenterCell().ToVector2() : randomAvailableCell.Value.ToVector2());
			}
			vector -= this.SpriteDimensions.XY().WithY(0f) / 2f;
			base.StartCoroutine(this.HandleTeleport(teleporterController, vector, false, noFX));
		}
	}

	// Token: 0x06008128 RID: 33064 RVA: 0x0034448C File Offset: 0x0034268C
	public void AttemptReturnTeleport(TeleporterController teleporter)
	{
		if (this.CurrentRoom != null && this.CurrentRoom.CanTeleportFromRoom() && this.CanReturnTeleport && teleporter == this.m_returnTeleporter)
		{
			AkSoundEngine.PostEvent("Play_OBJ_teleport_depart_01", base.gameObject);
			base.StartCoroutine(this.HandleTeleport(teleporter, this.m_cachedTeleportSpot, true, false));
		}
	}

	// Token: 0x06008129 RID: 33065 RVA: 0x003444F8 File Offset: 0x003426F8
	private IEnumerator HandleTeleport(TeleporterController teleporter, Vector2 targetSpot, bool isReturnTeleport, bool noFX = false)
	{
		CameraController cameraController = GameManager.Instance.MainCameraController;
		Vector2 offsetVector = cameraController.transform.position - base.transform.position;
		offsetVector -= cameraController.GetAimContribution();
		Minimap.Instance.ToggleMinimap(false, false);
		cameraController.SetManualControl(true, false);
		cameraController.OverridePosition = cameraController.transform.position;
		this.CurrentInputState = PlayerInputState.NoInput;
		yield return new WaitForSeconds(0.1f);
		this.ToggleRenderer(false, "minimap teleporter");
		this.ToggleGunRenderers(false, "minimap teleporter");
		this.ToggleHandRenderers(false, "minimap teleporter");
		if (!noFX)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(teleporter.teleportDepartureVFX);
			gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(base.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
			gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
			gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		}
		this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
		yield return new WaitForSeconds(0.4f);
		Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
		yield return new WaitForSeconds(0.1f);
		if (!cameraController.ManualControl)
		{
			cameraController.SetManualControl(true, false);
		}
		if (offsetVector.magnitude > 15f)
		{
			offsetVector = offsetVector.normalized * 15f;
		}
		base.specRigidbody.Position = new Position(targetSpot);
		base.specRigidbody.RecheckTriggers = true;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			cameraController.OverridePosition = cameraController.GetIdealCameraPosition();
		}
		else
		{
			cameraController.OverridePosition = (targetSpot + offsetVector).ToVector3ZUp(0f);
		}
		Pixelator.Instance.MarkOcclusionDirty();
		GameObject arrivalVFX = UnityEngine.Object.Instantiate<GameObject>(teleporter.teleportArrivalVFX);
		arrivalVFX.transform.position = teleporter.transform.position;
		arrivalVFX.transform.position = arrivalVFX.transform.position.Quantize(0.0625f);
		BraveMemory.HandleTeleportation();
		if (isReturnTeleport)
		{
			RoomHandler absoluteRoom = targetSpot.GetAbsoluteRoom();
			if (absoluteRoom != null && absoluteRoom.visibility != RoomHandler.VisibilityStatus.OBSCURED && absoluteRoom.visibility != RoomHandler.VisibilityStatus.REOBSCURED)
			{
				if (this.m_currentRoom != null)
				{
					this.m_currentRoom.PlayerExit(this);
				}
				this.m_currentRoom = absoluteRoom;
				this.m_currentRoom.PlayerEnter(this);
				this.EnteredNewRoom(this.m_currentRoom);
				GameManager.Instance.MainCameraController.AssignBoundingPolygon(this.m_currentRoom.cameraBoundingPolygon);
			}
		}
		Pixelator.Instance.FadeToBlack(0.1f, true, 0f);
		yield return null;
		cameraController.SetManualControl(false, true);
		yield return new WaitForSeconds(0.75f);
		GameObject arrivalVFX2 = UnityEngine.Object.Instantiate<GameObject>(teleporter.teleportDepartureVFX);
		arrivalVFX2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(base.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
		arrivalVFX2.transform.position = arrivalVFX2.transform.position.Quantize(0.0625f);
		arrivalVFX2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
		yield return new WaitForSeconds(0.25f);
		this.ToggleRenderer(true, "minimap teleporter");
		this.ToggleGunRenderers(true, "minimap teleporter");
		this.ToggleHandRenderers(true, "minimap teleporter");
		if (isReturnTeleport)
		{
			teleporter.ClearReturnActive();
			this.m_returnTeleporter = null;
		}
		else
		{
			if (this.m_returnTeleporter != null)
			{
				this.m_returnTeleporter.ClearReturnActive();
			}
			teleporter.SetReturnActive();
			this.m_returnTeleporter = teleporter;
		}
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		this.CurrentInputState = PlayerInputState.AllInput;
		this.WarpFollowersToPlayer(false);
		yield break;
	}

	// Token: 0x0600812A RID: 33066 RVA: 0x00344530 File Offset: 0x00342730
	protected virtual void CheckSpawnAlertArrows()
	{
		if (!this.IsPrimaryPlayer)
		{
			return;
		}
		if (GameManager.IsReturningToBreach || GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating)
		{
			this.m_elapsedNonalertTime = 0f;
			this.m_isThreatArrowing = false;
			this.m_threadArrowTarget = null;
			return;
		}
		if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.NONE || this.CurrentRoom == null)
		{
			this.m_elapsedNonalertTime = 0f;
			this.m_isThreatArrowing = false;
			this.m_threadArrowTarget = null;
			return;
		}
		if (this.CurrentRoom != null && this.IsInCombat)
		{
			List<AIActor> activeEnemies = this.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies == null || activeEnemies.Count == 0)
			{
				this.m_elapsedNonalertTime = 0f;
				this.m_isThreatArrowing = false;
				this.m_threadArrowTarget = null;
				return;
			}
			AIActor aiactor = null;
			bool flag = false;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor2 = activeEnemies[i];
				if (aiactor2)
				{
					if (!aiactor2.IgnoreForRoomClear || aiactor2.AlwaysShowOffscreenArrow)
					{
						if (!aiactor2.healthHaver || !aiactor2.healthHaver.IsBoss)
						{
							if (GameManager.Instance.MainCameraController.PointIsVisible(aiactor2.CenterPosition))
							{
								flag = true;
							}
							else if (!aiactor || (!aiactor.AlwaysShowOffscreenArrow && aiactor2.AlwaysShowOffscreenArrow))
							{
								aiactor = aiactor2;
							}
						}
					}
				}
			}
			bool flag2 = aiactor && (!flag || aiactor.AlwaysShowOffscreenArrow);
			if (flag2)
			{
				this.m_elapsedNonalertTime += BraveTime.DeltaTime;
				this.m_threadArrowTarget = aiactor;
				if ((this.m_elapsedNonalertTime > 3f || aiactor.AlwaysShowOffscreenArrow) && !this.m_isThreatArrowing)
				{
					base.StartCoroutine(this.HandleThreatArrow());
				}
			}
			else
			{
				this.m_elapsedNonalertTime = 0f;
				this.m_isThreatArrowing = false;
				this.m_threadArrowTarget = null;
			}
		}
	}

	// Token: 0x0600812B RID: 33067 RVA: 0x00344758 File Offset: 0x00342958
	protected virtual void CheckSpawnEmergencyCrate()
	{
		if (this.CurrentRoom == null || this.CurrentRoom.ExtantEmergencyCrate != null || GameManager.Instance.Dungeon.SuppressEmergencyCrates)
		{
			return;
		}
		if (!this.CurrentRoom.IsSealed && !this.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
		{
			return;
		}
		if (!this.CurrentRoom.area.IsProceduralRoom && this.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
		{
			return;
		}
		if (this.CharacterUsesRandomGuns)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.inventory.AllGuns.Count; i++)
		{
			if (this.inventory.AllGuns[i].CurrentAmmo > 0 || this.inventory.AllGuns[i].InfiniteAmmo)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.SpawnEmergencyCrate(null);
		}
	}

	// Token: 0x0600812C RID: 33068 RVA: 0x00344864 File Offset: 0x00342A64
	public IntVector2 SpawnEmergencyCrate(GenericLootTable overrideTable = null)
	{
		GameObject gameObject = (GameObject)BraveResources.Load("EmergencyCrate", ".prefab");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		EmergencyCrateController component = gameObject2.GetComponent<EmergencyCrateController>();
		if (overrideTable != null)
		{
			component.gunTable = overrideTable;
		}
		IntVector2 bestRewardLocation = this.CurrentRoom.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
		component.Trigger(new Vector3(-5f, -5f, -5f), bestRewardLocation.ToVector3() + new Vector3(15f, 15f, 15f), this.CurrentRoom, overrideTable == null);
		this.CurrentRoom.ExtantEmergencyCrate = gameObject2;
		return bestRewardLocation;
	}

	// Token: 0x0600812D RID: 33069 RVA: 0x00344910 File Offset: 0x00342B10
	public void ReinitializeMovementRestrictors()
	{
		base.specRigidbody.MovementRestrictor = null;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.RollPitMovementRestrictor));
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.CameraBoundsMovementRestrictor));
		}
	}

	// Token: 0x0600812E RID: 33070 RVA: 0x00344988 File Offset: 0x00342B88
	private void InitializeCallbacks()
	{
		base.healthHaver.persistsOnDeath = true;
		base.healthHaver.OnDeath += this.Die;
		base.healthHaver.OnDamaged += this.Damaged;
		base.healthHaver.OnHealthChanged += this.HealthChanged;
		base.spriteAnimator.AnimationEventTriggered = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.RollPitMovementRestrictor));
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
			specRigidbody3.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody3.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.CameraBoundsMovementRestrictor));
		}
		this.inventory.OnGunChanged += this.OnGunChanged;
	}

	// Token: 0x0600812F RID: 33071 RVA: 0x00344A9C File Offset: 0x00342C9C
	private void CameraBoundsMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		IntVector2 intVector = PhysicsEngine.UnitToPixel(GameManager.Instance.MainCameraController.MinVisiblePoint);
		IntVector2 intVector2 = PhysicsEngine.UnitToPixel(GameManager.Instance.MainCameraController.MaxVisiblePoint);
		if (specRigidbody.PixelColliders[0].LowerLeft.x < intVector.x && pixelOffset.x < prevPixelOffset.x)
		{
			validLocation = false;
		}
		else if (specRigidbody.PixelColliders[0].UpperRight.x > intVector2.x && pixelOffset.x > prevPixelOffset.x)
		{
			validLocation = false;
		}
		else if (specRigidbody.PixelColliders[0].LowerLeft.y < intVector.y && pixelOffset.y < prevPixelOffset.y)
		{
			validLocation = false;
		}
		else if (specRigidbody.PixelColliders[1].UpperRight.y > intVector2.y && pixelOffset.y > prevPixelOffset.y)
		{
			validLocation = false;
		}
		if (!validLocation && StaticReferenceManager.ActiveMineCarts.ContainsKey(this))
		{
			StaticReferenceManager.ActiveMineCarts[this].EvacuateSpecificPlayer(this, false);
		}
	}

	// Token: 0x06008130 RID: 33072 RVA: 0x00344C08 File Offset: 0x00342E08
	public void ReuniteWithOtherPlayer(PlayerController other, bool useDefaultVFX = false)
	{
		this.WarpToPoint(other.transform.position, useDefaultVFX, false);
	}

	// Token: 0x06008131 RID: 33073 RVA: 0x00344C24 File Offset: 0x00342E24
	public void HandleItemStolen(ShopItemController item)
	{
		if (this.OnItemStolen != null)
		{
			this.OnItemStolen(this, item);
		}
	}

	// Token: 0x06008132 RID: 33074 RVA: 0x00344C40 File Offset: 0x00342E40
	public void HandleItemPurchased(ShopItemController item)
	{
		if (this.OnItemPurchased != null)
		{
			this.OnItemPurchased(this, item);
		}
	}

	// Token: 0x06008133 RID: 33075 RVA: 0x00344C5C File Offset: 0x00342E5C
	public void OnRoomCleared()
	{
		for (int i = 0; i < this.activeItems.Count; i++)
		{
			this.activeItems[i].ClearedRoom();
		}
		this.NumRoomsCleared++;
		if (this.CharacterUsesRandomGuns && this.m_gunGameElapsed > 20f)
		{
			this.ChangeToRandomGun();
		}
		if (this.OnRoomClearEvent != null)
		{
			this.OnRoomClearEvent(this);
		}
	}

	// Token: 0x06008134 RID: 33076 RVA: 0x00344CDC File Offset: 0x00342EDC
	public void ChangeToRandomGun()
	{
		if (this.IsGhost)
		{
			return;
		}
		this.m_gunGameElapsed = 0f;
		this.m_gunGameDamageThreshold = 200f;
		if (this.inventory.GunLocked.Value)
		{
			return;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.TUTORIAL)
		{
			return;
		}
		Gun currentGun = this.CurrentGun;
		this.inventory.AddGunToInventory(PickupObjectDatabase.GetRandomGun(), false);
		base.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_MagicFavor_Change") as GameObject, new Vector3(0f, -1f, 0f), true, false, false);
		if (currentGun)
		{
			if (currentGun.IsFiring)
			{
				currentGun.CeaseAttack(true, null);
			}
			this.inventory.RemoveGunFromInventory(currentGun);
			UnityEngine.Object.Destroy(currentGun.gameObject);
		}
	}

	// Token: 0x06008135 RID: 33077 RVA: 0x00344DD0 File Offset: 0x00342FD0
	public void OnAnyEnemyTookAnyDamage(float damageDone, bool fatal, HealthHaver target)
	{
		if (this.OnAnyEnemyReceivedDamage != null)
		{
			this.OnAnyEnemyReceivedDamage(damageDone, fatal, target);
		}
		AIActor aiactor = ((!target) ? null : target.aiActor);
		if (aiactor && !aiactor.IsNormalEnemy)
		{
			return;
		}
		if (!this.IsGhost && !target.PreventCooldownGainFromDamage)
		{
			for (int i = 0; i < this.activeItems.Count; i++)
			{
				this.activeItems[i].DidDamage(this, damageDone);
			}
			if (this.inventory != null && this.inventory.AllGuns != null)
			{
				for (int j = 0; j < this.inventory.AllGuns.Count; j++)
				{
					if (this.inventory.AllGuns[j].UsesRechargeLikeActiveItem)
					{
						this.inventory.AllGuns[j].ApplyActiveCooldownDamage(this, damageDone);
					}
				}
			}
		}
	}

	// Token: 0x06008136 RID: 33078 RVA: 0x00344EDC File Offset: 0x003430DC
	public void OnDidDamage(float damageDone, bool fatal, HealthHaver target)
	{
		if (this.OnDealtDamage != null)
		{
			this.OnDealtDamage(this, damageDone);
		}
		if (this.OnDealtDamageContext != null)
		{
			this.OnDealtDamageContext(this, damageDone, fatal, target);
		}
		if (fatal)
		{
			this.m_enemiesKilled++;
			this.m_gunGameDamageThreshold = 200f;
			if (this.CharacterUsesRandomGuns && this.m_enemiesKilled % 5 == 0)
			{
				this.ChangeToRandomGun();
			}
		}
		if (this.CharacterUsesRandomGuns)
		{
			this.m_gunGameDamageThreshold -= Mathf.Max(damageDone, 3f);
			if (this.m_gunGameDamageThreshold < 0f)
			{
				this.ChangeToRandomGun();
			}
		}
		if (fatal && this.OnKilledEnemy != null)
		{
			this.OnKilledEnemy(this);
		}
		if (fatal && this.OnKilledEnemyContext != null)
		{
			this.OnKilledEnemyContext(this, target);
		}
	}

	// Token: 0x06008137 RID: 33079 RVA: 0x00344FCC File Offset: 0x003431CC
	protected void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		for (int i = 0; i < this.animationAudioEvents.Count; i++)
		{
			if (this.animationAudioEvents[i].eventTag == frame.eventInfo)
			{
				AkSoundEngine.PostEvent(this.animationAudioEvents[i].eventName, base.gameObject);
			}
		}
	}

	// Token: 0x06008138 RID: 33080 RVA: 0x0034503C File Offset: 0x0034323C
	public void HandleDodgedBeam(BeamController beam)
	{
		if (this.OnDodgedBeam != null)
		{
			this.OnDodgedBeam(beam, this);
		}
	}

	// Token: 0x06008139 RID: 33081 RVA: 0x00345058 File Offset: 0x00343258
	protected virtual void OnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (this.DodgeRollIsBlink && this.IsDodgeRolling && otherRigidbody && (otherRigidbody.projectile || !otherRigidbody.GetComponent<DungeonDoorController>()))
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (this.IsGhost && otherRigidbody && otherRigidbody.aiActor)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (this.IsDodgeRolling && otherRigidbody)
		{
			if (otherRigidbody.projectile && this.OnDodgedProjectile != null)
			{
				this.OnDodgedProjectile(otherRigidbody.projectile);
			}
			if (otherRigidbody.aiActor)
			{
				if (this.DodgeRollIsBlink)
				{
					PhysicsEngine.SkipCollision = true;
					return;
				}
				FreezeOnDeath component = otherRigidbody.GetComponent<FreezeOnDeath>();
				if (component && component.IsDeathFrozen)
				{
					return;
				}
				AIActor aiActor = otherRigidbody.aiActor;
				if (aiActor.healthHaver)
				{
					float num = this.stats.rollDamage * this.stats.GetStatValue(PlayerStats.StatType.DodgeRollDamage);
					if (aiActor.healthHaver.IsDead)
					{
						PhysicsEngine.SkipCollision = true;
					}
					else if (!this.m_rollDamagedEnemies.Contains(aiActor) && aiActor.healthHaver.GetCurrentHealth() < num && aiActor.healthHaver.CanCurrentlyBeKilled)
					{
						this.ApplyRollDamage(aiActor);
						PhysicsEngine.SkipCollision = true;
					}
				}
				if (aiActor.IsFrozen)
				{
					GameActorFreezeEffect gameActorFreezeEffect = aiActor.GetEffect("freeze") as GameActorFreezeEffect;
					float num2 = ((gameActorFreezeEffect == null) ? 0f : (aiActor.healthHaver.GetMaxHealth() * gameActorFreezeEffect.UnfreezeDamagePercent));
					if (gameActorFreezeEffect != null && num2 >= aiActor.healthHaver.GetCurrentHealth() && aiActor.healthHaver.CanCurrentlyBeKilled)
					{
						aiActor.healthHaver.ApplyDamage(num2, this.lockedDodgeRollDirection, "DODGEROLL OF AWESOME", CoreDamageTypes.None, DamageCategory.Collision, true, null, false);
						GameManager.Instance.platformInterface.AchievementUnlock(Achievement.KILL_FROZEN_ENEMY_WITH_ROLL, 0);
						PhysicsEngine.SkipCollision = true;
					}
					else if (aiActor.knockbackDoer)
					{
						aiActor.knockbackDoer.ApplyKnockback(this.lockedDodgeRollDirection, 5f, false);
					}
				}
			}
		}
	}

	// Token: 0x0600813A RID: 33082 RVA: 0x003452B4 File Offset: 0x003434B4
	public void ApplyRollDamage(AIActor actor)
	{
		if (!this.m_rollDamagedEnemies.Contains(actor))
		{
			bool flag = false;
			if (actor.HasOverrideDodgeRollDeath && string.IsNullOrEmpty(actor.healthHaver.overrideDeathAnimation))
			{
				flag = true;
				actor.healthHaver.overrideDeathAnimation = actor.OverrideDodgeRollDeath;
			}
			if (actor.specRigidbody && PassiveItem.ActiveFlagItems.ContainsKey(this) && (PassiveItem.ActiveFlagItems[this].ContainsKey(typeof(SpikedArmorItem)) || PassiveItem.ActiveFlagItems[this].ContainsKey(typeof(HelmetItem))))
			{
				PixelCollider hitboxPixelCollider = actor.specRigidbody.HitboxPixelCollider;
				if (hitboxPixelCollider != null)
				{
					Vector2 vector = BraveMathCollege.ClosestPointOnRectangle(base.specRigidbody.GetUnitCenter(ColliderType.HitBox), hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
					SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/VFX_DodgeRollHit", ".prefab"), vector, Quaternion.identity, true);
				}
			}
			actor.healthHaver.ApplyDamage(this.stats.rollDamage * this.stats.GetStatValue(PlayerStats.StatType.DodgeRollDamage), this.lockedDodgeRollDirection, "DODGEROLL", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			this.m_rollDamagedEnemies.Add(actor);
			if (this.OnRolledIntoEnemy != null)
			{
				this.OnRolledIntoEnemy(this, actor);
			}
			if (flag)
			{
				actor.healthHaver.overrideDeathAnimation = string.Empty;
			}
		}
	}

	// Token: 0x0600813B RID: 33083 RVA: 0x0034542C File Offset: 0x0034362C
	private void HealthChanged(float result, float max)
	{
		if (GameUIRoot.Instance == null)
		{
			return;
		}
		UnityEngine.Debug.Log(string.Concat(new object[] { "changing health to: ", result, "|", max }));
		GameUIRoot.Instance.UpdatePlayerHealthUI(this, base.healthHaver);
	}

	// Token: 0x0600813C RID: 33084 RVA: 0x00345490 File Offset: 0x00343690
	public void HandleCloneItem(ExtraLifeItem source)
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
			if (otherPlayer.IsGhost)
			{
				this.DoCloneEffect();
			}
			else
			{
				this.m_cloneWaitingForCoopDeath = true;
			}
		}
		else
		{
			this.DoCloneEffect();
		}
	}

	// Token: 0x0600813D RID: 33085 RVA: 0x003454E4 File Offset: 0x003436E4
	private void DoCloneEffect()
	{
		base.StartCoroutine(this.HandleCloneEffect());
	}

	// Token: 0x0600813E RID: 33086 RVA: 0x003454F4 File Offset: 0x003436F4
	private IEnumerator HandleCloneEffect()
	{
		Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
		GameUIRoot.Instance.ToggleUICamera(false);
		base.healthHaver.FullHeal();
		this.IsOnFire = false;
		this.CurrentFireMeterValue = 0f;
		this.CurrentPoisonMeterValue = 0f;
		this.CurrentCurseMeterValue = 0f;
		this.CurrentDrainMeterValue = 0f;
		if (this.characterIdentity == PlayableCharacters.Robot)
		{
			base.healthHaver.Armor = 6f;
		}
		float ela = 0f;
		while (ela < 0.5f)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		int targetLevelIndex = 1;
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
		{
			targetLevelIndex += GameManager.Instance.LastShortcutFloorLoaded;
		}
		GameManager.Instance.SetNextLevelIndex(targetLevelIndex);
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
		{
			GameManager.Instance.DelayedLoadBossrushFloor(0.5f);
		}
		else
		{
			GameManager.Instance.DelayedLoadNextLevel(0.5f);
		}
		this.m_cloneWaitingForCoopDeath = false;
		ExtraLifeItem cloneItem = null;
		for (int i = 0; i < this.passiveItems.Count; i++)
		{
			if (this.passiveItems[i] is ExtraLifeItem)
			{
				ExtraLifeItem extraLifeItem = this.passiveItems[i] as ExtraLifeItem;
				if (extraLifeItem.extraLifeMode == ExtraLifeItem.ExtraLifeMode.CLONE)
				{
					cloneItem = extraLifeItem;
				}
			}
		}
		if (cloneItem != null)
		{
			this.RemovePassiveItem(cloneItem.PickupObjectId);
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController.IsGhost)
				{
					playerController.StartCoroutine(playerController.CoopResurrectInternal(playerController.transform.position, null, true));
				}
				playerController.healthHaver.FullHeal();
				playerController.specRigidbody.Velocity = Vector2.zero;
				playerController.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
				if (playerController.m_returnTeleporter != null)
				{
					playerController.m_returnTeleporter.ClearReturnActive();
					playerController.m_returnTeleporter = null;
				}
			}
			Chest.ToggleCoopChests(false);
		}
		else
		{
			base.healthHaver.FullHeal();
			base.specRigidbody.Velocity = Vector2.zero;
			base.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
			if (this.m_returnTeleporter != null)
			{
				this.m_returnTeleporter.ClearReturnActive();
				this.m_returnTeleporter = null;
			}
		}
		yield return new WaitForSeconds(1f);
		this.IsOnFire = false;
		this.CurrentFireMeterValue = 0f;
		this.CurrentPoisonMeterValue = 0f;
		this.CurrentCurseMeterValue = 0f;
		this.CurrentDrainMeterValue = 0f;
		base.healthHaver.FullHeal();
		if (this.characterIdentity == PlayableCharacters.Robot)
		{
			base.healthHaver.Armor = 6f;
		}
		yield break;
	}

	// Token: 0x0600813F RID: 33087 RVA: 0x00345510 File Offset: 0x00343710
	public void EscapeRoom(PlayerController.EscapeSealedRoomStyle escapeStyle, bool resetCurrentRoom, RoomHandler targetRoom = null)
	{
		this.RespawnInPreviousRoom(false, escapeStyle, resetCurrentRoom, targetRoom);
		if (targetRoom != null)
		{
			targetRoom.EnsureUpstreamLocksUnlocked();
		}
		base.specRigidbody.Velocity = Vector2.zero;
		base.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
	}

	// Token: 0x06008140 RID: 33088 RVA: 0x00345548 File Offset: 0x00343748
	public void RespawnInPreviousRoom(bool doFullHeal, PlayerController.EscapeSealedRoomStyle escapeStyle, bool resetCurrentRoom, RoomHandler targetRoom = null)
	{
		RoomHandler currentRoom = this.CurrentRoom;
		if (targetRoom == null)
		{
			targetRoom = this.GetPreviousRoom(this.CurrentRoom);
		}
		this.m_lastInteractionTarget = null;
		if (escapeStyle == PlayerController.EscapeSealedRoomStyle.TELEPORTER)
		{
			IntVector2? randomAvailableCell = targetRoom.GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, null);
			if (randomAvailableCell != null)
			{
				this.TeleportToPoint(randomAvailableCell.Value.ToCenterVector2(), true);
			}
			if (resetCurrentRoom && this.CurrentRoom != targetRoom)
			{
				base.StartCoroutine(this.DelayedRoomReset(currentRoom));
			}
		}
		else if (resetCurrentRoom)
		{
			base.StartCoroutine(this.HandleResetAndRespawn_CR(targetRoom, currentRoom, doFullHeal, escapeStyle));
		}
	}

	// Token: 0x06008141 RID: 33089 RVA: 0x003455FC File Offset: 0x003437FC
	private RoomHandler GetPreviousRoom(RoomHandler currentRoom)
	{
		RoomHandler roomHandler = null;
		for (int i = 0; i < currentRoom.connectedRooms.Count; i++)
		{
			if (currentRoom.connectedRooms[i].visibility != RoomHandler.VisibilityStatus.OBSCURED && currentRoom.distanceFromEntrance > currentRoom.connectedRooms[i].distanceFromEntrance)
			{
				roomHandler = currentRoom.connectedRooms[i];
				break;
			}
		}
		if (roomHandler == null)
		{
			for (int j = 0; j < currentRoom.connectedRooms.Count; j++)
			{
				if (currentRoom.connectedRooms[j].visibility != RoomHandler.VisibilityStatus.OBSCURED)
				{
					roomHandler = currentRoom.connectedRooms[j];
					break;
				}
			}
		}
		if (roomHandler != null && roomHandler.area.IsProceduralRoom && roomHandler.area.proceduralCells != null)
		{
			for (int k = 0; k < roomHandler.connectedRooms.Count; k++)
			{
				if (roomHandler.connectedRooms[k].visibility != RoomHandler.VisibilityStatus.OBSCURED && roomHandler.distanceFromEntrance > roomHandler.connectedRooms[k].distanceFromEntrance && roomHandler.connectedRooms[k] != currentRoom)
				{
					roomHandler = roomHandler.connectedRooms[k];
					break;
				}
			}
		}
		if (roomHandler == null)
		{
			UnityEngine.Debug.Log("Could not find a previous room that has been visited!");
			roomHandler = GameManager.Instance.Dungeon.data.Entrance;
		}
		return roomHandler;
	}

	// Token: 0x06008142 RID: 33090 RVA: 0x00345774 File Offset: 0x00343974
	private IEnumerator DelayedRoomReset(RoomHandler targetRoom)
	{
		if (GameManager.Instance.InTutorial)
		{
			targetRoom.npcSealState = RoomHandler.NPCSealState.SealNone;
		}
		while (this.CurrentRoom == targetRoom)
		{
			yield return null;
		}
		yield return null;
		if (targetRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear) || !targetRoom.EverHadEnemies || GameManager.Instance.InTutorial)
		{
			targetRoom.ResetPredefinedRoomLikeDarkSouls();
		}
		if (!targetRoom.EverHadEnemies)
		{
			targetRoom.forceTeleportersActive = true;
		}
		if (GameManager.Instance.InTutorial)
		{
			this.CurrentRoom.UnsealRoom();
		}
		ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
		for (int i = allProjectiles.Count - 1; i >= 0; i--)
		{
			if (allProjectiles[i])
			{
				allProjectiles[i].DieInAir(false, true, true, false);
			}
		}
		yield break;
	}

	// Token: 0x06008143 RID: 33091 RVA: 0x00345798 File Offset: 0x00343998
	private IEnumerator HandleResetAndRespawn_CR(RoomHandler roomToSpawnIn, RoomHandler roomToReset, bool doFullHeal, PlayerController.EscapeSealedRoomStyle escapeStyle)
	{
		if (this.CurrentGun)
		{
			this.CurrentGun.CeaseAttack(false, null);
		}
		if (doFullHeal)
		{
			base.healthHaver.FullHeal();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
				if (otherPlayer.healthHaver.IsAlive)
				{
					otherPlayer.healthHaver.FullHeal();
				}
				otherPlayer.CurrentInputState = PlayerInputState.NoInput;
			}
		}
		GameManager.Instance.PauseRaw(true);
		this.CurrentInputState = PlayerInputState.NoInput;
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		Transform cameraTransform = GameManager.Instance.MainCameraController.transform;
		Vector3 cameraStartPosition = cameraTransform.position;
		Vector3 cameraEndPosition = base.CenterPosition;
		if (escapeStyle == PlayerController.EscapeSealedRoomStyle.GRIP_MASTER)
		{
			cameraEndPosition += new Vector3(0f, 3f);
		}
		GameManager.Instance.MainCameraController.OverridePosition = cameraStartPosition;
		this.ToggleGunRenderers(false, "death");
		this.ToggleHandRenderers(false, "death");
		if (escapeStyle == PlayerController.EscapeSealedRoomStyle.GRIP_MASTER)
		{
			this.ToggleRenderer(false, "gripmaster");
		}
		float elapsed = 0f;
		float duration = 0.8f;
		Pixelator.Instance.LerpToLetterbox(0.35f, 0.8f);
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float smoothT = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			GameManager.Instance.MainCameraController.OverridePosition = Vector3.Lerp(cameraStartPosition, cameraEndPosition, smoothT);
			yield return null;
		}
		elapsed = 0f;
		duration = 0f;
		switch (escapeStyle)
		{
		case PlayerController.EscapeSealedRoomStyle.DEATH_SEQUENCE:
			duration = 0.8f;
			break;
		case PlayerController.EscapeSealedRoomStyle.ESCAPE_SPIN:
			duration = 1.5f;
			break;
		case PlayerController.EscapeSealedRoomStyle.NONE:
			duration = 0.5f;
			break;
		case PlayerController.EscapeSealedRoomStyle.GRIP_MASTER:
			duration = 2.25f;
			break;
		}
		if (escapeStyle == PlayerController.EscapeSealedRoomStyle.DEATH_SEQUENCE)
		{
			base.spriteAnimator.Play((!this.UseArmorlessAnim) ? "death_shot" : "death_shot_armorless");
		}
		else if (escapeStyle == PlayerController.EscapeSealedRoomStyle.ESCAPE_SPIN)
		{
			base.spriteAnimator.Play((!this.UseArmorlessAnim) ? "spinfall" : "spinfall_armorless");
		}
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float timeMod = ((escapeStyle != PlayerController.EscapeSealedRoomStyle.ESCAPE_SPIN) ? 1f : BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, elapsed / duration));
			base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME * timeMod);
			yield return null;
		}
		Pixelator.Instance.FadeToBlack(1f, false, 0f);
		elapsed = 0f;
		duration = 1f;
		while (elapsed < duration)
		{
			if (escapeStyle == PlayerController.EscapeSealedRoomStyle.ESCAPE_SPIN)
			{
				base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		this.m_interruptingPitRespawn = true;
		Pixelator.Instance.LerpToLetterbox(0.5f, 0f);
		IntVector2 availableCell = roomToSpawnIn.GetCenteredVisibleClearSpot(3, 3);
		base.transform.position = new Vector3((float)availableCell.x + 0.5f, (float)availableCell.y + 0.5f, -0.1f);
		this.ForceChangeRoom(roomToSpawnIn);
		this.Reinitialize();
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer2 = GameManager.Instance.GetOtherPlayer(this);
			if (otherPlayer2.healthHaver.IsAlive)
			{
				otherPlayer2.transform.position = base.transform.position + Vector3.right;
				otherPlayer2.ForceChangeRoom(roomToSpawnIn);
				otherPlayer2.Reinitialize();
			}
		}
		GameUIRoot.Instance.bossController.DisableBossHealth();
		GameUIRoot.Instance.bossController2.DisableBossHealth();
		GameUIRoot.Instance.bossControllerSide.DisableBossHealth();
		GameManager.Instance.MainCameraController.OverridePosition = base.CenterPosition;
		yield return null;
		this.ToggleGunRenderers(true, "death");
		this.ToggleHandRenderers(true, "death");
		if (escapeStyle == PlayerController.EscapeSealedRoomStyle.GRIP_MASTER)
		{
			this.ToggleRenderer(true, "gripmaster");
		}
		GameManager.Instance.ForceUnpause();
		GameManager.Instance.PreventPausing = false;
		this.CurrentInputState = PlayerInputState.AllInput;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer3 = GameManager.Instance.GetOtherPlayer(this);
			otherPlayer3.CurrentInputState = PlayerInputState.AllInput;
		}
		if (roomToReset != GameManager.Instance.Dungeon.data.Entrance && (roomToReset.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear) || !roomToReset.EverHadEnemies))
		{
			roomToReset.ResetPredefinedRoomLikeDarkSouls();
		}
		Pixelator.Instance.FadeToBlack(1f, true, 0f);
		ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
		for (int i = allProjectiles.Count - 1; i >= 0; i--)
		{
			if (allProjectiles[i])
			{
				allProjectiles[i].DieInAir(false, true, true, false);
			}
		}
		yield break;
	}

	// Token: 0x06008144 RID: 33092 RVA: 0x003457D0 File Offset: 0x003439D0
	public void OnLostArmor()
	{
		this.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
		if (this.lostAllArmorVFX != null && base.healthHaver.Armor == 0f)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(this.lostAllArmorVFX, base.specRigidbody.UnitTopCenter, Quaternion.identity);
			gameObject.GetComponent<DebrisObject>().Trigger(Vector3.zero, 0.5f, 1f);
		}
		if (this.LostArmor != null)
		{
			this.LostArmor();
		}
	}

	// Token: 0x06008145 RID: 33093 RVA: 0x00345878 File Offset: 0x00343A78
	private void Damaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		PlatformInterface.SetAlienFXColor(this.m_alienDamageColor, 1f);
		this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
		this.HasTakenDamageThisRun = true;
		this.HasTakenDamageThisFloor = true;
		if (this.CurrentRoom != null)
		{
			this.CurrentRoom.PlayerHasTakenDamageInThisRoom = true;
		}
		if (this.CurrentGun && this.CurrentGun.IsCharging)
		{
			this.CurrentGun.CeaseAttack(false, null);
		}
		Pixelator.Instance.HandleDamagedVignette(damageDirection);
		Exploder.DoRadialKnockback(base.CenterPosition, 50f, 3f);
		bool flag = base.healthHaver.Armor > 0f;
		bool flag2 = resultValue <= 0f && !flag;
		if (flag2 && !this.m_revenging && PassiveItem.IsFlagSetForCharacter(this, typeof(PoweredByRevengeItem)))
		{
			base.StartCoroutine(this.HandleFueledByRevenge());
			base.healthHaver.ApplyHealing(0.5f - resultValue);
			resultValue = 0.5f;
		}
		flag2 = resultValue <= 0f && !flag;
		if (flag2 && this.CurrentItem is RationItem)
		{
			RationItem rationItem = this.CurrentItem as RationItem;
			this.UseItem();
			resultValue += rationItem.healingAmount;
		}
		flag2 = resultValue <= 0f && !flag;
		if (damageCategory != DamageCategory.DamageOverTime && flag2)
		{
			ScreenShakeSettings screenShakeSettings = new ScreenShakeSettings(0.25f, 7f, 0.1f, 0.3f);
			GameManager.Instance.MainCameraController.DoScreenShake(screenShakeSettings, new Vector2?(base.specRigidbody.UnitCenter), false);
		}
		bool flag3 = false;
		if (GameManager.Instance.InTutorial || flag3)
		{
			flag2 = resultValue <= 0f && !flag;
			if (flag2)
			{
				this.RespawnInPreviousRoom(true, PlayerController.EscapeSealedRoomStyle.DEATH_SEQUENCE, true, null);
				base.specRigidbody.Velocity = Vector2.zero;
				base.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
				foreach (Gun gun in this.inventory.AllGuns)
				{
					gun.ammo = gun.AdjustedMaxAmmo;
				}
				return;
			}
		}
		flag2 = resultValue <= 0f && !flag;
		if (flag2)
		{
			if (this.CurrentGun)
			{
				this.CurrentGun.CeaseAttack(false, null);
			}
			this.CurrentInputState = PlayerInputState.NoInput;
			this.m_handlingQueuedAnimation = true;
			this.HandleDarkSoulsHollowTransition(false);
		}
		else
		{
			if (this.OnReceivedDamage != null)
			{
				this.OnReceivedDamage(this);
			}
			if (this.ownerlessStatModifiers != null)
			{
				bool flag4 = false;
				for (int i = 0; i < this.ownerlessStatModifiers.Count; i++)
				{
					if (this.ownerlessStatModifiers[i].isMeatBunBuff)
					{
						flag4 = true;
						this.ownerlessStatModifiers.RemoveAt(i);
						i--;
					}
				}
				if (flag4 && this.stats != null)
				{
					UnityEngine.Debug.LogError("Did remove meatbun buff!");
					this.stats.RecalculateStats(this, false, false);
				}
			}
		}
	}

	// Token: 0x06008146 RID: 33094 RVA: 0x00345BE0 File Offset: 0x00343DE0
	private IEnumerator HandleFueledByRevenge()
	{
		this.m_revenging = true;
		base.healthHaver.IsVulnerable = false;
		float ela = 0f;
		float duration = 3f;
		this.OnKilledEnemy += this.RevengeRevive;
		int cachedKills = this.m_enemiesKilled;
		Material vignetteMaterial = Pixelator.Instance.FadeMaterial;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			float t = Mathf.Lerp(0f, 1f, ela / duration);
			vignetteMaterial.SetColor("_VignetteColor", Color.red);
			vignetteMaterial.SetFloat("_VignettePower", Mathf.Lerp(0.5f, 2.5f, t));
			Pixelator.Instance.saturation = 1f - Mathf.Sqrt(t);
			if (this.m_enemiesKilled > cachedKills)
			{
				break;
			}
			yield return null;
		}
		vignetteMaterial.SetColor("_VignetteColor", Color.black);
		vignetteMaterial.SetFloat("_VignettePower", 1f);
		Pixelator.Instance.saturation = 1f;
		this.OnKilledEnemy -= this.RevengeRevive;
		base.healthHaver.IsVulnerable = true;
		if (this.m_enemiesKilled <= cachedKills)
		{
			base.healthHaver.ApplyDamage(100f, Vector2.zero, base.healthHaver.lastIncurredDamageSource, CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
		}
		this.m_revenging = false;
		yield break;
	}

	// Token: 0x06008147 RID: 33095 RVA: 0x00345BFC File Offset: 0x00343DFC
	private void RevengeRevive(PlayerController obj)
	{
		base.healthHaver.FullHeal();
	}

	// Token: 0x06008148 RID: 33096 RVA: 0x00345C0C File Offset: 0x00343E0C
	public void HandleDarkSoulsHollowTransition(bool isHollow = true)
	{
		if (isHollow)
		{
			this.IsDarkSoulsHollow = true;
			if (this.m_hollowAfterImage == null)
			{
				this.m_hollowAfterImage = base.sprite.gameObject.AddComponent<AfterImageTrailController>();
				this.m_hollowAfterImage.spawnShadows = true;
				this.m_hollowAfterImage.shadowTimeDelay = 0.05f;
				this.m_hollowAfterImage.shadowLifetime = 0.3f;
				this.m_hollowAfterImage.minTranslation = 0.05f;
				this.m_hollowAfterImage.maxEmission = 0f;
				this.m_hollowAfterImage.minEmission = 0f;
				this.m_hollowAfterImage.dashColor = new Color(0f, 0.44140625f, 0.55859375f);
				this.m_hollowAfterImage.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
			}
			else
			{
				this.m_hollowAfterImage.spawnShadows = true;
			}
			this.ChangeSpecialShaderFlag(2, 1f);
		}
		else
		{
			this.IsDarkSoulsHollow = false;
			if (this.m_hollowAfterImage != null)
			{
				this.m_hollowAfterImage.spawnShadows = false;
			}
			this.ChangeSpecialShaderFlag(2, 0f);
		}
	}

	// Token: 0x06008149 RID: 33097 RVA: 0x00345D30 File Offset: 0x00343F30
	public void TriggerDarkSoulsReset(bool dropItems = true, int cursedHealthMaximum = 1)
	{
		this.IsOnFire = false;
		this.CurrentFireMeterValue = 0f;
		this.CurrentPoisonMeterValue = 0f;
		this.CurrentCurseMeterValue = 0f;
		this.CurrentDrainMeterValue = 0f;
		AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !GameManager.Instance.GetOtherPlayer(this).IsGhost)
		{
			this.DropPileOfSouls();
			this.HandleDarkSoulsHollowTransition(true);
			base.StartCoroutine(this.HandleCoopDeath(this.m_isFalling));
		}
		else
		{
			this.m_interruptingPitRespawn = true;
			base.healthHaver.FullHeal();
			if (this.characterIdentity == PlayableCharacters.Robot)
			{
				base.healthHaver.Armor = 2f;
			}
			base.specRigidbody.Velocity = Vector2.zero;
			base.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
			if (this.m_returnTeleporter != null)
			{
				this.m_returnTeleporter.ClearReturnActive();
				this.m_returnTeleporter = null;
			}
			GameManager.Instance.Dungeon.DarkSoulsReset(this, dropItems, cursedHealthMaximum);
		}
	}

	// Token: 0x0600814A RID: 33098 RVA: 0x00345E54 File Offset: 0x00344054
	private void ContinueDarkSoulResetCoop()
	{
		base.StartCoroutine(this.CoopResurrectInternal(base.transform.position, null, true));
		base.healthHaver.FullHeal();
		base.specRigidbody.Velocity = Vector2.zero;
		base.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
		if (this.m_returnTeleporter != null)
		{
			this.m_returnTeleporter.ClearReturnActive();
			this.m_returnTeleporter = null;
		}
		GameManager.Instance.Dungeon.DarkSoulsReset(this, false, 1);
	}

	// Token: 0x0600814B RID: 33099 RVA: 0x00345EDC File Offset: 0x003440DC
	protected virtual void Die(Vector2 finalDamageDirection)
	{
		this.DeathsThisRun++;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.DIE_IN_PAST, 0);
		}
		GameUIRoot.Instance.GetReloadBarForPlayer(this).UpdateStatusBars(this);
		bool flag = true;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
			if (otherPlayer && otherPlayer.healthHaver.IsAlive)
			{
				flag = false;
			}
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER || flag)
		{
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer2 = GameManager.Instance.GetOtherPlayer(this);
				if (otherPlayer2.m_cloneWaitingForCoopDeath)
				{
					otherPlayer2.DoCloneEffect();
					return;
				}
				if (otherPlayer2.IsDarkSoulsHollow && otherPlayer2.IsGhost)
				{
					otherPlayer2.ContinueDarkSoulResetCoop();
					base.StartCoroutine(this.HandleCoopDeath(this.m_isFalling));
					return;
				}
			}
			GameManager.Instance.PauseRaw(true);
			BraveTime.RegisterTimeScaleMultiplier(0f, GameManager.Instance.gameObject);
			AkSoundEngine.PostEvent("Stop_SND_All", base.gameObject);
			base.StartCoroutine(this.HandleDeath_CR());
			AkSoundEngine.PostEvent("Play_UI_gameover_start_01", base.gameObject);
		}
		else
		{
			base.StartCoroutine(this.HandleCoopDeath(this.m_isFalling));
		}
	}

	// Token: 0x0600814C RID: 33100 RVA: 0x00346040 File Offset: 0x00344240
	private void HandleCoopDeathItemDropping()
	{
		List<Gun> list = new List<Gun>();
		List<PickupObject> list2 = new List<PickupObject>();
		if (this.CurrentGun)
		{
			MimicGunController component = this.CurrentGun.GetComponent<MimicGunController>();
			if (component)
			{
				component.ForceClearMimic(true);
			}
		}
		for (int i = 0; i < this.inventory.AllGuns.Count; i++)
		{
			if (this.inventory.AllGuns[i].CanActuallyBeDropped(this) && !this.inventory.AllGuns[i].PersistsOnDeath)
			{
				bool flag = false;
				for (int j = 0; j < this.startingGunIds.Count; j++)
				{
					if (this.inventory.AllGuns[i].PickupObjectId == this.startingGunIds[j])
					{
						flag = true;
					}
				}
				for (int k = 0; k < this.startingAlternateGunIds.Count; k++)
				{
					if (this.inventory.AllGuns[i].PickupObjectId == this.startingAlternateGunIds[k])
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(this.inventory.AllGuns[i]);
					list2.Add(this.inventory.AllGuns[i]);
				}
			}
		}
		for (int l = 0; l < this.passiveItems.Count; l++)
		{
			if (this.passiveItems[l].CanActuallyBeDropped(this) && !this.passiveItems[l].PersistsOnDeath && !(this.passiveItems[l] is ExtraLifeItem))
			{
				list2.Add(this.passiveItems[l]);
			}
		}
		for (int m = 0; m < this.activeItems.Count; m++)
		{
			if (this.activeItems[m].CanActuallyBeDropped(this) && !this.activeItems[m].PersistsOnDeath)
			{
				list2.Add(this.activeItems[m]);
			}
		}
		int count = list2.Count;
		for (int n = 0; n < count; n++)
		{
			if (n == 0 && list.Count > 0)
			{
				int num = UnityEngine.Random.Range(0, list.Count);
				list2.Remove(list[num]);
				this.ForceDropGun(list[num]);
				list.RemoveAt(num);
			}
			else if (list2.Count > 0)
			{
				int num2 = UnityEngine.Random.Range(0, list2.Count);
				if (list2[num2] is Gun)
				{
					DebrisObject debrisObject = this.ForceDropGun(list2[num2] as Gun);
					PickupObject pickupObject = ((!debrisObject) ? null : debrisObject.GetComponentInChildren<PickupObject>());
					if (pickupObject)
					{
						pickupObject.IgnoredByRat = true;
						pickupObject.ClearIgnoredByRatFlagOnPickup = true;
					}
				}
				else if (list2[num2] is PassiveItem)
				{
					DebrisObject debrisObject2 = this.DropPassiveItem(list2[num2] as PassiveItem);
					PickupObject pickupObject2 = ((!debrisObject2) ? null : debrisObject2.GetComponentInChildren<PickupObject>());
					if (pickupObject2)
					{
						pickupObject2.IgnoredByRat = true;
						pickupObject2.ClearIgnoredByRatFlagOnPickup = true;
					}
				}
				else
				{
					DebrisObject debrisObject3 = this.DropActiveItem(list2[num2] as PlayerItem, 4f, true);
					PickupObject pickupObject3 = ((!debrisObject3) ? null : debrisObject3.GetComponentInChildren<PickupObject>());
					if (pickupObject3)
					{
						pickupObject3.IgnoredByRat = true;
						pickupObject3.ClearIgnoredByRatFlagOnPickup = true;
					}
				}
				list2.RemoveAt(num2);
			}
		}
	}

	// Token: 0x0600814D RID: 33101 RVA: 0x00346430 File Offset: 0x00344630
	public IEnumerator HandleCoopDeath(bool ignoreCorpse = false)
	{
		this.ResetOverrideAnimationLibrary();
		this.m_handlingQueuedAnimation = true;
		this.CurrentInputState = PlayerInputState.NoInput;
		if (!this.IsDarkSoulsHollow)
		{
			this.HandleCoopDeathItemDropping();
		}
		if (!GameManager.PVP_ENABLED)
		{
			this.ResetToFactorySettings(false, false, false);
		}
		this.m_turboSpeedModifier = null;
		this.m_turboRollSpeedModifier = null;
		this.m_turboEnemyBulletModifier = null;
		GameUIRoot.Instance.ClearGunName(this.IsPrimaryPlayer);
		GameUIRoot.Instance.ClearItemName(this.IsPrimaryPlayer);
		GameUIRoot.Instance.UpdateGunData(this.inventory, 0, this);
		GameUIRoot.Instance.UpdateItemData(this, this.CurrentItem, this.activeItems);
		GameUIRoot.Instance.DisableCoopPlayerUI(this);
		if (Minimap.Instance != null)
		{
			Minimap.Instance.UpdatePlayerPositionExact(base.transform.position, this, true);
		}
		Chest.ToggleCoopChests(true);
		GameManager.Instance.MainCameraController.IsLerping = true;
		base.specRigidbody.Velocity = Vector2.zero;
		base.specRigidbody.enabled = false;
		if (this.IsOnFire)
		{
			this.IsOnFire = false;
		}
		this.ToggleHandRenderers(false, string.Empty);
		this.ToggleGunRenderers(false, string.Empty);
		GameUIRoot.Instance.ForceClearReload(this.PlayerIDX);
		if (!ignoreCorpse)
		{
			string coopDeathAnimName = ((!this.UseArmorlessAnim) ? "death_coop" : "death_coop_armorless");
			base.spriteAnimator.Play(coopDeathAnimName);
			while (base.spriteAnimator.IsPlaying(coopDeathAnimName))
			{
				yield return null;
			}
			GameObject corpse = SpawnManager.SpawnDebris((GameObject)BraveResources.Load("Global Prefabs/PlayerCorpse", ".prefab"), base.transform.position, Quaternion.identity);
			tk2dSprite corpseSprite = corpse.GetComponent<tk2dSprite>();
			corpseSprite.SetSprite(base.sprite.Collection, base.sprite.spriteId);
			corpseSprite.scale = base.sprite.scale;
			corpse.transform.position = base.sprite.transform.position;
			corpseSprite.HeightOffGround = -3.5f;
			corpseSprite.UpdateZDepth();
		}
		this.BecomeGhost();
		yield break;
	}

	// Token: 0x0600814E RID: 33102 RVA: 0x00346454 File Offset: 0x00344654
	private void BecomeGhost()
	{
		this.IsGhost = true;
		GameManager.Instance.MainCameraController.IsLerping = true;
		this.ChangeSpecialShaderFlag(0, 1f);
		GameUIRoot.Instance.TransitionToGhostUI(this);
		this.ChangeFlatColorOverride(new Color(0.2f, 0.3f, 1f, 1f));
		base.specRigidbody.enabled = true;
		base.specRigidbody.CollideWithTileMap = true;
		base.specRigidbody.CollideWithOthers = true;
		base.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
		base.specRigidbody.Reinitialize();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		base.ToggleShadowVisiblity(false);
		this.ToggleHandRenderers(false, "ghostliness");
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		this.m_handlingQueuedAnimation = false;
		this.CurrentInputState = PlayerInputState.AllInput;
	}

	// Token: 0x0600814F RID: 33103 RVA: 0x00346538 File Offset: 0x00344738
	public void DoCoopArrow()
	{
		if (base.healthHaver.IsDead || !base.gameObject.activeSelf)
		{
			return;
		}
		if (this.m_isCoopArrowing)
		{
			return;
		}
		this.m_isCoopArrowing = true;
		base.StartCoroutine(this.HandleCoopArrow());
	}

	// Token: 0x06008150 RID: 33104 RVA: 0x00346588 File Offset: 0x00344788
	private IEnumerator HandleThreatArrow()
	{
		this.m_isThreatArrowing = true;
		GameObject extantArrow = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global VFX/Alert_Arrow", ".prefab"));
		extantArrow.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		extantArrow.transform.parent = base.sprite.transform;
		tk2dBaseSprite extantArrowSprite = extantArrow.GetComponent<tk2dBaseSprite>();
		extantArrowSprite.HeightOffGround = 8f;
		extantArrowSprite.UpdateZDepth();
		do
		{
			if (extantArrowSprite.GetCurrentSpriteDef().name != "blankframe")
			{
				Vector2 vector = this.m_threadArrowTarget.CenterPosition - base.CenterPosition;
				if (vector.magnitude < 3f)
				{
					break;
				}
				float num = BraveMathCollege.Atan2Degrees(vector);
				num = (float)(Mathf.RoundToInt(num / 5f) * 5);
				vector = Quaternion.Euler(0f, 0f, num) * Vector2.right;
				Vector2 vector2 = Vector2.zero;
				bool flag = BraveMathCollege.LineSegmentRectangleIntersection(base.CenterPosition, this.m_threadArrowTarget.CenterPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint, ref vector2);
				if (flag)
				{
					vector2 -= vector.normalized * 0.5f;
					extantArrow.transform.position = vector2.ToVector3ZUp(0f);
					extantArrow.transform.position = extantArrow.transform.position.Quantize(0.0625f);
					extantArrow.transform.localRotation = Quaternion.Euler(0f, 0f, num);
				}
			}
			yield return null;
		}
		while (this.m_isThreatArrowing && this.m_threadArrowTarget && (this.m_threadArrowTarget.HasBeenEngaged || this.m_threadArrowTarget.AlwaysShowOffscreenArrow) && this.m_threadArrowTarget.healthHaver && this.m_threadArrowTarget.healthHaver.IsAlive);
		UnityEngine.Object.Destroy(extantArrow);
		this.m_isThreatArrowing = false;
		this.m_threadArrowTarget = null;
		yield break;
	}

	// Token: 0x06008151 RID: 33105 RVA: 0x003465A4 File Offset: 0x003447A4
	private IEnumerator HandleCoopArrow()
	{
		GameObject extantArrow = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global VFX/Coop_Arrow", ".prefab"));
		extantArrow.transform.parent = base.sprite.transform;
		tk2dBaseSprite extantArrowSprite = extantArrow.GetComponent<tk2dBaseSprite>();
		tk2dSpriteAnimator arrowAnimator = extantArrowSprite.spriteAnimator;
		do
		{
			if (extantArrowSprite.GetCurrentSpriteDef().name != "blankframe")
			{
				Vector2 vector = ((!this.IsPrimaryPlayer) ? GameManager.Instance.PrimaryPlayer.CenterPosition : GameManager.Instance.SecondaryPlayer.CenterPosition) - base.CenterPosition;
				if (vector.magnitude < 3f)
				{
					break;
				}
				float num = BraveMathCollege.Atan2Degrees(vector);
				num = (float)(Mathf.RoundToInt(num / 5f) * 5);
				vector = Quaternion.Euler(0f, 0f, num) * Vector2.right;
				extantArrow.transform.position = (base.CenterPosition + vector * 2f).ToVector3ZUp(0f);
				extantArrow.transform.position = extantArrow.transform.position.Quantize(0.0625f);
				extantArrow.transform.localRotation = Quaternion.Euler(0f, 0f, num);
			}
			yield return null;
		}
		while (arrowAnimator.Playing);
		UnityEngine.Object.Destroy(extantArrow);
		this.m_isCoopArrowing = false;
		yield break;
	}

	// Token: 0x06008152 RID: 33106 RVA: 0x003465C0 File Offset: 0x003447C0
	public void QueueSpecificAnimation(string animationName)
	{
		this.m_handlingQueuedAnimation = true;
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.QueuedAnimationComplete));
		base.spriteAnimator.Play(animationName);
	}

	// Token: 0x06008153 RID: 33107 RVA: 0x003465FC File Offset: 0x003447FC
	protected void QueuedAnimationComplete(tk2dSpriteAnimator anima, tk2dSpriteAnimationClip clippy)
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.QueuedAnimationComplete));
		this.m_handlingQueuedAnimation = false;
	}

	// Token: 0x06008154 RID: 33108 RVA: 0x0034662C File Offset: 0x0034482C
	private IEnumerator InvariantWait(float delay)
	{
		float elapsed = 0f;
		while (elapsed < delay)
		{
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008155 RID: 33109 RVA: 0x00346650 File Offset: 0x00344850
	protected void HandleDeathPhotography()
	{
		GameUIRoot.Instance.ForceClearReload(-1);
		GameUIRoot.Instance.notificationController.ForceHide();
		Pixelator.Instance.CacheCurrentFrameToBuffer = true;
		Pixelator.Instance.CacheScreenSpacePositionsForDeathFrame(base.CenterPosition, base.CenterPosition);
	}

	// Token: 0x06008156 RID: 33110 RVA: 0x00346690 File Offset: 0x00344890
	private IEnumerator HandleDeath_CR()
	{
		bool wasPitFalling = base.IsFalling;
		Pixelator.Instance.DoFinalNonFadedLayer = true;
		if (this.CurrentGun)
		{
			this.CurrentGun.CeaseAttack(false, null);
		}
		this.CurrentInputState = PlayerInputState.NoInput;
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		this.ToggleGunRenderers(false, "death");
		this.ToggleHandRenderers(false, "death");
		this.ToggleAttachedRenderers(false);
		Transform cameraTransform = GameManager.Instance.MainCameraController.transform;
		Vector3 cameraStartPosition = cameraTransform.position;
		Vector3 cameraEndPosition = base.CenterPosition;
		GameManager.Instance.MainCameraController.OverridePosition = cameraStartPosition;
		if (this.CurrentGun)
		{
			this.CurrentGun.DespawnVFX();
		}
		this.HandleDeathPhotography();
		yield return null;
		this.ToggleHandRenderers(false, "death");
		if (this.CurrentGun)
		{
			this.CurrentGun.DespawnVFX();
		}
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unfaded"));
		GameUIRoot.Instance.ForceClearReload(this.PlayerIDX);
		GameUIRoot.Instance.notificationController.ForceHide();
		float elapsed = 0f;
		float duration = 0.8f;
		tk2dBaseSprite spotlightSprite = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("DeathShadow", ".prefab"), base.specRigidbody.UnitCenter, Quaternion.identity)).GetComponent<tk2dBaseSprite>();
		spotlightSprite.spriteAnimator.ignoreTimeScale = true;
		spotlightSprite.spriteAnimator.Play();
		tk2dSpriteAnimator whooshAnimator = spotlightSprite.transform.GetChild(0).GetComponent<tk2dSpriteAnimator>();
		whooshAnimator.ignoreTimeScale = true;
		whooshAnimator.Play();
		Pixelator.Instance.CustomFade(0.6f, 0f, Color.white, Color.black, 0.1f, 0.5f);
		Pixelator.Instance.LerpToLetterbox(0.35f, 0.8f);
		BraveInput.AllowPausedRumble = true;
		this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Hard);
		CompanionItem pigItem = null;
		tk2dSpriteAnimator pigVFX = null;
		bool isDoingPigSave = false;
		string pigMoveAnim = "pig_move_right";
		string pigSaveAnim = "pig_jump_right";
		for (int i = 0; i < this.passiveItems.Count; i++)
		{
			if (this.passiveItems[i] is CompanionItem)
			{
				CompanionItem companionItem = this.passiveItems[i] as CompanionItem;
				CompanionController companionController = ((!companionItem || !companionItem.ExtantCompanion) ? null : companionItem.ExtantCompanion.GetComponent<CompanionController>());
				if (companionController && companionController.name.StartsWith("Pig"))
				{
					pigVFX = (UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_HeroPig")) as GameObject).GetComponent<tk2dSpriteAnimator>();
					pigItem = companionItem;
					isDoingPigSave = true;
				}
				else if (companionItem.DisplayName == "Pig")
				{
					pigVFX = (UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_HeroPig")) as GameObject).GetComponent<tk2dSpriteAnimator>();
					pigItem = companionItem;
					isDoingPigSave = true;
				}
				if (companionItem.ExtantCompanion && companionItem.ExtantCompanion.GetComponent<SackKnightController>())
				{
					SackKnightController component = companionItem.ExtantCompanion.GetComponent<SackKnightController>();
					if (component.CurrentForm == SackKnightController.SackKnightPhase.HOLY_KNIGHT)
					{
						pigItem = companionItem;
						pigVFX = (UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_HeroJunk")) as GameObject).GetComponent<tk2dSpriteAnimator>();
						isDoingPigSave = true;
						pigMoveAnim = "junk_shspcg_move_right";
						pigSaveAnim = "junk_shspcg_sacrifice_right";
					}
				}
			}
		}
		if (!isDoingPigSave && this.OverrideAnimationLibrary != null)
		{
			this.OverrideAnimationLibrary = null;
			this.ResetOverrideAnimationLibrary();
			GameObject gameObject = (this.BlankVFXPrefab = (GameObject)BraveResources.Load("Global VFX/VFX_BulletArmor_Death", ".prefab"));
			base.PlayEffectOnActor(gameObject, Vector3.zero, true, false, false);
		}
		while (elapsed < duration)
		{
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / duration;
			GameManager.Instance.MainCameraController.OverridePosition = Vector3.Lerp(cameraStartPosition, cameraEndPosition, t);
			base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			spotlightSprite.color = new Color(1f, 1f, 1f, t);
			Pixelator.Instance.saturation = Mathf.Clamp01(1f - t);
			yield return null;
		}
		spotlightSprite.color = Color.white;
		yield return base.StartCoroutine(this.InvariantWait(0.4f));
		Transform clockhairTransform = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
		ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
		elapsed = 0f;
		duration = clockhair.ClockhairInDuration;
		Vector3 clockhairTargetPosition = base.CenterPosition;
		Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
		clockhair.renderer.enabled = false;
		clockhair.spriteAnimator.Play("clockhair_intro");
		clockhair.hourAnimator.Play("hour_hand_intro");
		clockhair.minuteAnimator.Play("minute_hand_intro");
		clockhair.secondAnimator.Play("second_hand_intro");
		if (!isDoingPigSave && (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER || GameManager.Instance.GetOtherPlayer(this).IsGhost) && this.OnRealPlayerDeath != null)
		{
			this.OnRealPlayerDeath(this);
		}
		bool hasWobbled = false;
		while (elapsed < duration)
		{
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t2 = elapsed / duration;
			float smoothT = Mathf.SmoothStep(0f, 1f, t2);
			Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
			clockhairTransform.position = currentPosition.WithZ(0f);
			if (t2 > 0.5f)
			{
				clockhair.renderer.enabled = true;
				clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			}
			if (t2 > 0.75f)
			{
				clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
				clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
				clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
				clockhair.hourAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				clockhair.minuteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				clockhair.secondAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			}
			if (!hasWobbled && clockhair.spriteAnimator.CurrentFrame == clockhair.spriteAnimator.CurrentClip.frames.Length - 1)
			{
				clockhair.spriteAnimator.Play("clockhair_wobble");
				hasWobbled = true;
			}
			clockhair.sprite.UpdateZDepth();
			base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			yield return null;
		}
		if (!hasWobbled)
		{
			clockhair.spriteAnimator.Play("clockhair_wobble");
		}
		clockhair.SpinToSessionStart(clockhair.ClockhairSpinDuration);
		elapsed = 0f;
		duration = clockhair.ClockhairSpinDuration + clockhair.ClockhairPauseBeforeShot;
		while (elapsed < duration)
		{
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			yield return null;
		}
		if (isDoingPigSave)
		{
			elapsed = 0f;
			duration = 2f;
			Vector2 targetPosition = clockhairTargetPosition;
			Vector2 startPosition = targetPosition + new Vector2(-18f, 0f);
			Vector2 pigOffset = pigVFX.sprite.WorldCenter - pigVFX.transform.position.XY();
			pigVFX.Play(pigMoveAnim);
			while (elapsed < duration)
			{
				Vector2 lerpPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
				pigVFX.transform.position = (lerpPosition - pigOffset).ToVector3ZisY(0f);
				pigVFX.sprite.UpdateZDepth();
				if (duration - elapsed < 0.1f && !pigVFX.IsPlaying(pigSaveAnim))
				{
					pigVFX.Play(pigSaveAnim);
				}
				pigVFX.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				if (GameManager.INVARIANT_DELTA_TIME == 0f)
				{
					elapsed += 0.05f;
				}
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				yield return null;
			}
		}
		elapsed = 0f;
		duration = 0.1f;
		clockhairStartPosition = clockhairTransform.position;
		clockhairTargetPosition = clockhairStartPosition + new Vector3(0f, 12f, 0f);
		clockhair.spriteAnimator.Play("clockhair_fire");
		clockhair.hourAnimator.GetComponent<Renderer>().enabled = false;
		clockhair.minuteAnimator.GetComponent<Renderer>().enabled = false;
		clockhair.secondAnimator.GetComponent<Renderer>().enabled = false;
		this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Hard);
		if (!isDoingPigSave)
		{
			base.spriteAnimator.Play((!this.UseArmorlessAnim) ? "death_shot" : "death_shot_armorless");
		}
		while (elapsed < duration)
		{
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			if (!isDoingPigSave)
			{
				base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			}
			if (isDoingPigSave)
			{
				pigVFX.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				pigVFX.transform.position += new Vector3(6f * GameManager.INVARIANT_DELTA_TIME, 0f, 0f);
			}
			yield return null;
		}
		elapsed = 0f;
		duration = 1f;
		while (elapsed < duration)
		{
			if (GameManager.INVARIANT_DELTA_TIME == 0f)
			{
				elapsed += 0.05f;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			if (clockhair.spriteAnimator.CurrentFrame == clockhair.spriteAnimator.CurrentClip.frames.Length - 1)
			{
				clockhair.renderer.enabled = false;
			}
			else
			{
				clockhair.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			}
			base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
			if (isDoingPigSave)
			{
				pigVFX.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				pigVFX.transform.position += new Vector3(Mathf.Lerp(6f, 0f, elapsed / duration) * GameManager.INVARIANT_DELTA_TIME, 0f, 0f);
			}
			yield return null;
		}
		BraveInput.AllowPausedRumble = false;
		if (isDoingPigSave)
		{
			yield return base.StartCoroutine(this.InvariantWait(1f));
			Pixelator.Instance.saturation = 1f;
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_HERO_PIG, true);
			pigVFX.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
			Pixelator.Instance.FadeToColor(0.25f, Pixelator.Instance.FadeColor, true, 0f);
			Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
			UnityEngine.Object.Destroy(spotlightSprite.gameObject);
			Pixelator.Instance.DoFinalNonFadedLayer = false;
			base.healthHaver.FullHeal();
			if (this.ForceZeroHealthState)
			{
				base.healthHaver.Armor = 6f;
			}
			this.CurrentInputState = PlayerInputState.AllInput;
			if (pigItem.HasGunTransformationSacrificeSynergy && this.HasActiveBonusSynergy(pigItem.GunTransformationSacrificeSynergy, false))
			{
				GunFormeSynergyProcessor.AssignTemporaryOverrideGun(this, pigItem.SacrificeGunID, pigItem.SacrificeGunDuration);
			}
			this.RemovePassiveItem(pigItem.PickupObjectId);
			this.IsVisible = true;
			this.ToggleGunRenderers(true, "death");
			this.ToggleHandRenderers(true, "death");
			this.ToggleAttachedRenderers(true);
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
			GameManager.Instance.DungeonMusicController.ResetForNewFloor(GameManager.Instance.Dungeon);
			if (this.CurrentRoom != null)
			{
				GameManager.Instance.DungeonMusicController.NotifyEnteredNewRoom(this.CurrentRoom);
			}
			GameManager.Instance.ForceUnpause();
			GameManager.Instance.PreventPausing = false;
			BraveTime.ClearMultiplier(GameManager.Instance.gameObject);
			Exploder.DoRadialKnockback(base.CenterPosition, 50f, 5f);
			if (wasPitFalling)
			{
				base.StartCoroutine(this.PitRespawn(Vector2.zero));
			}
			base.healthHaver.IsVulnerable = true;
			base.healthHaver.TriggerInvulnerabilityPeriod(-1f);
		}
		else
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.NUMBER_DEATHS, 1f);
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_MINES) < 1f)
			{
				GameStatsManager.Instance.isChump = false;
			}
			AmmonomiconDeathPageController.LastKilledPlayerPrimary = this.IsPrimaryPlayer;
			GameManager.Instance.DoGameOver(base.healthHaver.lastIncurredDamageSource);
		}
		yield break;
	}

	// Token: 0x06008157 RID: 33111 RVA: 0x003466AC File Offset: 0x003448AC
	public void ClearDeadFlags()
	{
		this.CurrentInputState = PlayerInputState.AllInput;
		this.m_handlingQueuedAnimation = false;
	}

	// Token: 0x06008158 RID: 33112 RVA: 0x003466BC File Offset: 0x003448BC
	private void RollPitMovementRestrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		if (this.m_dodgeRollState == PlayerController.DodgeRollState.OnGround && !this.IsFlying)
		{
			Func<IntVector2, bool> func = delegate(IntVector2 pixel)
			{
				Vector2 vector = PhysicsEngine.PixelToUnitMidpoint(pixel);
				if (!GameManager.Instance.Dungeon.CellSupportsFalling(vector))
				{
					return false;
				}
				List<SpeculativeRigidbody> platformsAt = GameManager.Instance.Dungeon.GetPlatformsAt(vector);
				if (platformsAt != null)
				{
					for (int i = 0; i < platformsAt.Count; i++)
					{
						if (platformsAt[i].PrimaryPixelCollider.ContainsPixel(pixel))
						{
							return false;
						}
					}
				}
				return true;
			};
			PixelCollider primaryPixelCollider = specRigidbody.PrimaryPixelCollider;
			if (primaryPixelCollider != null)
			{
				IntVector2 intVector = pixelOffset - prevPixelOffset;
				if (intVector == IntVector2.Down && func(primaryPixelCollider.LowerLeft + pixelOffset) && func(primaryPixelCollider.LowerRight + pixelOffset) && (!func(primaryPixelCollider.UpperRight + prevPixelOffset) || !func(primaryPixelCollider.UpperLeft + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
				if (intVector == IntVector2.Right && func(primaryPixelCollider.LowerRight + pixelOffset) && func(primaryPixelCollider.UpperRight + pixelOffset) && (!func(primaryPixelCollider.UpperLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerLeft + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
				if (intVector == IntVector2.Up && func(primaryPixelCollider.UpperRight + pixelOffset) && func(primaryPixelCollider.UpperLeft + pixelOffset) && (!func(primaryPixelCollider.LowerLeft + prevPixelOffset) || !func(primaryPixelCollider.LowerRight + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
				if (intVector == IntVector2.Left && func(primaryPixelCollider.UpperLeft + pixelOffset) && func(primaryPixelCollider.LowerLeft + pixelOffset) && (!func(primaryPixelCollider.LowerRight + prevPixelOffset) || !func(primaryPixelCollider.UpperRight + prevPixelOffset)))
				{
					validLocation = false;
					return;
				}
			}
		}
	}

	// Token: 0x06008159 RID: 33113 RVA: 0x003468E0 File Offset: 0x00344AE0
	public void AcquirePuzzleItem(PickupObject item)
	{
		item.transform.parent = this.GunPivot;
		item.transform.localPosition = Vector3.zero;
		if (item && item.sprite)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(item.sprite, true);
		}
		this.additionalItems.Add(item);
	}

	// Token: 0x0600815A RID: 33114 RVA: 0x00346944 File Offset: 0x00344B44
	public void UsePuzzleItem(PickupObject item)
	{
		if (this.additionalItems.Contains(item))
		{
			UnityEngine.Object.Destroy(item.gameObject);
			this.additionalItems.Remove(item);
		}
	}

	// Token: 0x0600815B RID: 33115 RVA: 0x00346970 File Offset: 0x00344B70
	public PickupObject DropPuzzleItem(PickupObject item)
	{
		if (this.additionalItems.Contains(item) && item is NPCCellKeyItem)
		{
			this.additionalItems.Remove(item);
			item.transform.parent = null;
			(item as NPCCellKeyItem).DropLogic();
			GameUIRoot.Instance.UpdatePlayerConsumables(this.carriedConsumables);
			return item;
		}
		return null;
	}

	// Token: 0x0600815C RID: 33116 RVA: 0x003469D0 File Offset: 0x00344BD0
	public void AcquirePassiveItemPrefabDirectly(PassiveItem item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item.gameObject);
		PassiveItem component = gameObject.GetComponent<PassiveItem>();
		EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
		if (component2 != null)
		{
			component2.DoNotificationOnEncounter = false;
		}
		component.suppressPickupVFX = true;
		component.Pickup(this);
	}

	// Token: 0x0600815D RID: 33117 RVA: 0x00346A18 File Offset: 0x00344C18
	public void AcquirePassiveItem(PassiveItem item)
	{
		AkSoundEngine.PostEvent("Play_OBJ_passive_get_01", base.gameObject);
		this.passiveItems.Add(item);
		item.transform.parent = this.GunPivot;
		item.transform.localPosition = Vector3.zero;
		item.renderer.enabled = false;
		if (item.GetComponent<DebrisObject>() != null)
		{
			UnityEngine.Object.Destroy(item.GetComponent<DebrisObject>());
		}
		if (item.GetComponent<SquishyBounceWiggler>() != null)
		{
			UnityEngine.Object.Destroy(item.GetComponent<SquishyBounceWiggler>());
		}
		GameUIRoot.Instance.AddPassiveItemToDock(item, this);
		this.stats.RecalculateStats(this, false, false);
	}

	// Token: 0x0600815E RID: 33118 RVA: 0x00346AC4 File Offset: 0x00344CC4
	public void DropPileOfSouls()
	{
		Vector3 vector = base.specRigidbody.UnitBottomLeft.ToVector3ZUp(0f);
		if (this.CurrentRoom != null)
		{
			vector = this.CurrentRoom.GetBestRewardLocation(new IntVector2(2, 2), base.specRigidbody.UnitBottomLeft, false).ToVector3();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((GameObject)BraveResources.Load("Global Prefabs/PileOfSouls", ".prefab"), vector, Quaternion.identity);
		PileOfDarkSoulsPickup component = gameObject.GetComponent<PileOfDarkSoulsPickup>();
		component.TargetPlayerID = this.PlayerIDX;
		RoomHandler.unassignedInteractableObjects.Add(component);
		component.containedCurrency = this.carriedConsumables.Currency;
		this.carriedConsumables.Currency = 0;
		for (int i = 0; i < this.passiveItems.Count; i++)
		{
			if (this.passiveItems[i].CanActuallyBeDropped(this) && !this.passiveItems[i].PersistsOnDeath && this.passiveItems[i] is ExtraLifeItem && (this.passiveItems[i] as ExtraLifeItem).extraLifeMode == ExtraLifeItem.ExtraLifeMode.DARK_SOULS)
			{
				DebrisObject debrisObject = this.DropPassiveItem(this.passiveItems[i]);
				if (debrisObject)
				{
					component.passiveItems.Add(debrisObject.GetComponent<PassiveItem>());
					debrisObject.enabled = false;
					i--;
				}
			}
		}
		component.ToggleItems(false);
	}

	// Token: 0x0600815F RID: 33119 RVA: 0x00346C40 File Offset: 0x00344E40
	private void DontDontDestroyOnLoad(GameObject target)
	{
		if (target && GameManager.Instance.Dungeon && target.transform.parent == null)
		{
			target.transform.parent = GameManager.Instance.Dungeon.transform;
			target.transform.parent = null;
		}
	}

	// Token: 0x06008160 RID: 33120 RVA: 0x00346CA8 File Offset: 0x00344EA8
	public DebrisObject DropPassiveItem(PassiveItem item)
	{
		if (item && this.startingPassiveItemIds != null && this.characterIdentity != PlayableCharacters.Eevee)
		{
			for (int i = 0; i < this.startingPassiveItemIds.Count; i++)
			{
				if (this.startingPassiveItemIds[i] == item.PickupObjectId)
				{
					return null;
				}
			}
		}
		if (this.passiveItems.Contains(item))
		{
			this.passiveItems.Remove(item);
			GameUIRoot.Instance.RemovePassiveItemFromDock(item);
			DebrisObject debrisObject = item.Drop(this);
			this.stats.RecalculateStats(this, false, false);
			this.DontDontDestroyOnLoad(debrisObject.gameObject);
			return debrisObject;
		}
		UnityEngine.Debug.LogError("Failed to drop item because the player doesn't have it? " + item.DisplayName);
		return null;
	}

	// Token: 0x06008161 RID: 33121 RVA: 0x00346D70 File Offset: 0x00344F70
	public DebrisObject DropActiveItem(PlayerItem item, float overrideForce = 4f, bool isDeathDrop = false)
	{
		if (isDeathDrop && item && this.startingActiveItemIds != null)
		{
			for (int i = 0; i < this.startingActiveItemIds.Count; i++)
			{
				PlayerItem playerItem = PickupObjectDatabase.GetById(this.startingActiveItemIds[i]) as PlayerItem;
				if (playerItem.PickupObjectId == item.PickupObjectId && !playerItem.CanActuallyBeDropped(this))
				{
					return null;
				}
			}
		}
		if (this.activeItems.Contains(item))
		{
			UnityEngine.Debug.Log("DROPPING ACTIVE ITEM NOW");
			this.activeItems.Remove(item);
			DebrisObject debrisObject = item.Drop(this, overrideForce);
			UnityEngine.Object.Destroy(item.gameObject);
			return debrisObject;
		}
		UnityEngine.Debug.LogError("Failed to drop item because the player doesn't have it? " + item.DisplayName);
		return null;
	}

	// Token: 0x06008162 RID: 33122 RVA: 0x00346E40 File Offset: 0x00345040
	public void GetEquippedWith(PlayerItem item, bool switchTo = false)
	{
		if (this.m_preventItemSwitching)
		{
			this.RemoveActiveItemAt(this.m_selectedItemIndex);
			base.StopCoroutine(this.m_currentActiveItemDestructionCoroutine);
			this.m_currentActiveItemDestructionCoroutine = null;
			this.m_preventItemSwitching = false;
		}
		if (this.m_suppressItemSwitchTo)
		{
			switchTo = false;
		}
		item.transform.parent = this.GunPivot;
		item.transform.localPosition = Vector3.zero;
		int num = -1;
		for (int i = 0; i < this.activeItems.Count; i++)
		{
			if (this.activeItems[i].PickupObjectId == item.PickupObjectId)
			{
				num = i;
				break;
			}
		}
		int num2 = 0;
		for (int j = 0; j < item.passiveStatModifiers.Length; j++)
		{
			if (item.passiveStatModifiers[j].statToBoost == PlayerStats.StatType.AdditionalItemCapacity)
			{
				num2 += Mathf.RoundToInt(item.passiveStatModifiers[j].amount);
			}
		}
		if (item is TeleporterPrototypeItem)
		{
			for (int k = 0; k < this.activeItems.Count; k++)
			{
				if (this.activeItems[k] is ChestTeleporterItem)
				{
					num2++;
					break;
				}
			}
		}
		else if (item is ChestTeleporterItem)
		{
			for (int l = 0; l < this.activeItems.Count; l++)
			{
				if (this.activeItems[l] is TeleporterPrototypeItem)
				{
					num2++;
					break;
				}
			}
		}
		if (num == -1)
		{
			int num3 = this.MAX_ITEMS_HELD + (int)this.stats.GetStatValue(PlayerStats.StatType.AdditionalItemCapacity) + num2;
			if (this.stats != null)
			{
				int num4 = 0;
				while (this.activeItems.Count >= num3 && num4 < 100)
				{
					num4++;
					this.DropActiveItem(this.CurrentItem, 4f, false);
					this.stats.RecalculateStats(this, false, false);
					num3 = this.MAX_ITEMS_HELD + (int)this.stats.GetStatValue(PlayerStats.StatType.AdditionalItemCapacity) + num2;
				}
			}
			this.activeItems.Add(item);
			if (switchTo)
			{
				this.m_selectedItemIndex = this.activeItems.Count - 1;
			}
		}
		else
		{
			if (item.canStack)
			{
				this.activeItems[num].numberOfUses += item.numberOfUses;
				if (switchTo)
				{
					this.m_selectedItemIndex = num;
				}
			}
			UnityEngine.Object.Destroy(item.gameObject);
		}
		this.stats.RecalculateStats(this, false, false);
	}

	// Token: 0x06008163 RID: 33123 RVA: 0x003470D8 File Offset: 0x003452D8
	public void ForceConsumableBlank()
	{
		if (this.AcceptingNonMotionInput && Time.timeScale > 0f)
		{
			this.DoConsumableBlank();
		}
	}

	// Token: 0x06008164 RID: 33124 RVA: 0x003470FC File Offset: 0x003452FC
	protected void DoConsumableBlank()
	{
		if (this.Blanks > 0)
		{
			this.Blanks--;
			PlatformInterface.SetAlienFXColor(this.m_alienBlankColor, 1f);
			this.ForceBlank(25f, 0.5f, false, true, null, true, -1f);
			if (!this.IsInCombat)
			{
				for (int i = 0; i < StaticReferenceManager.AllAdvancedShrineControllers.Count; i++)
				{
					if (StaticReferenceManager.AllAdvancedShrineControllers[i].IsBlankShrine && StaticReferenceManager.AllAdvancedShrineControllers[i].transform.position.GetAbsoluteRoom() == this.CurrentRoom)
					{
						StaticReferenceManager.AllAdvancedShrineControllers[i].OnBlank();
					}
				}
			}
			for (int j = 0; j < StaticReferenceManager.AllRatTrapdoors.Count; j++)
			{
				if (StaticReferenceManager.AllRatTrapdoors[j])
				{
					StaticReferenceManager.AllRatTrapdoors[j].OnBlank();
				}
			}
			this.m_blankCooldownTimer = 0.5f;
			this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
			if (this.OnUsedBlank != null)
			{
				this.OnUsedBlank(this, this.Blanks);
			}
		}
	}

	// Token: 0x06008165 RID: 33125 RVA: 0x0034723C File Offset: 0x0034543C
	public void ForceBlank(float overrideRadius = 25f, float overrideTimeAtMaxRadius = 0.5f, bool silent = false, bool breaksWalls = true, Vector2? overrideCenter = null, bool breaksObjects = true, float overrideForce = -1f)
	{
		if (!silent)
		{
			if (this.BlankVFXPrefab == null)
			{
				this.BlankVFXPrefab = (GameObject)BraveResources.Load("Global VFX/BlankVFX", ".prefab");
			}
			AkSoundEngine.PostEvent("Play_OBJ_silenceblank_use_01", base.gameObject);
			AkSoundEngine.PostEvent("Stop_ENM_attack_cancel_01", base.gameObject);
		}
		GameObject gameObject = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
		silencerInstance.TriggerSilencer((overrideCenter == null) ? base.CenterPosition : overrideCenter.Value, 50f, overrideRadius, (!silent) ? this.BlankVFXPrefab : null, (!silent) ? 0.15f : 0f, (!silent) ? 0.2f : 0f, (float)((!silent) ? 50 : 0), (float)((!silent) ? 10 : 0), (!silent) ? ((overrideForce < 0f) ? 140f : overrideForce) : 0f, (float)((!breaksObjects) ? 0 : ((!silent) ? 15 : 5)), overrideTimeAtMaxRadius, this, breaksWalls, false);
		this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
	}

	// Token: 0x06008166 RID: 33126 RVA: 0x00347388 File Offset: 0x00345588
	protected void DoGhostBlank()
	{
		if (this.BlankVFXPrefab == null)
		{
			this.BlankVFXPrefab = (GameObject)BraveResources.Load("Global VFX/BlankVFX_Ghost", ".prefab");
		}
		PlatformInterface.SetAlienFXColor(this.m_alienBlankColor, 1f);
		AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
		GameObject gameObject = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
		float num = 0.25f;
		silencerInstance.TriggerSilencer(base.CenterPosition, 20f, 3f, this.BlankVFXPrefab, 0f, 3f, 50f, 4f, 30f, 3f, num, this, false, false);
		this.QueueSpecificAnimation("ghost_sneeze_right");
		this.m_blankCooldownTimer = 5f;
		this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
	}

	// Token: 0x06008167 RID: 33127 RVA: 0x0034745C File Offset: 0x0034565C
	protected void UseItem()
	{
		PlayerItem currentItem = this.CurrentItem;
		if (currentItem != null)
		{
			if (!currentItem.CanBeUsed(this))
			{
				return;
			}
			if (this.OnUsedPlayerItem != null && !currentItem.IsOnCooldown)
			{
				this.OnUsedPlayerItem(this, currentItem);
			}
			float num = -1f;
			bool flag = currentItem.Use(this, out num);
			if (flag)
			{
				if (num >= 0f)
				{
					this.m_currentActiveItemDestructionCoroutine = base.StartCoroutine(this.TimedRemoveActiveItem(this.m_selectedItemIndex, num));
				}
				else
				{
					this.RemoveActiveItemAt(this.m_selectedItemIndex);
				}
			}
			else if (!currentItem.consumable || currentItem.numberOfUses > 0)
			{
			}
			this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
		}
	}

	// Token: 0x06008168 RID: 33128 RVA: 0x00347520 File Offset: 0x00345720
	private IEnumerator TimedRemoveActiveItem(int indexToRemove, float delay)
	{
		this.m_preventItemSwitching = true;
		yield return new WaitForSeconds(delay);
		this.m_currentActiveItemDestructionCoroutine = null;
		this.m_preventItemSwitching = false;
		this.RemoveActiveItemAt(indexToRemove);
		yield break;
	}

	// Token: 0x06008169 RID: 33129 RVA: 0x0034754C File Offset: 0x0034574C
	public void RemoveAllActiveItems()
	{
		for (int i = this.activeItems.Count - 1; i >= 0; i--)
		{
			this.RemoveActiveItemAt(i);
		}
	}

	// Token: 0x0600816A RID: 33130 RVA: 0x00347580 File Offset: 0x00345780
	public void RemoveAllPassiveItems()
	{
		for (int i = this.passiveItems.Count - 1; i >= 0; i--)
		{
			this.RemovePassiveItemAt(i);
		}
	}

	// Token: 0x0600816B RID: 33131 RVA: 0x003475B4 File Offset: 0x003457B4
	public void RemoveActiveItem(int pickupId)
	{
		int num = this.activeItems.FindIndex((PlayerItem a) => a.PickupObjectId == pickupId);
		if (num >= 0)
		{
			this.RemoveActiveItemAt(num);
		}
		else
		{
			UnityEngine.Debug.LogError("Failed to remove active item because the player doesn't have it? pickupId = " + pickupId);
		}
	}

	// Token: 0x0600816C RID: 33132 RVA: 0x00347614 File Offset: 0x00345814
	protected void RemoveActiveItemAt(int index)
	{
		if (index >= 0 && index < this.activeItems.Count)
		{
			UnityEngine.Object.Destroy(this.activeItems[index].gameObject);
			this.activeItems.RemoveAt(index);
			if (this.m_selectedItemIndex < 0 || this.m_selectedItemIndex >= this.activeItems.Count)
			{
				this.m_selectedItemIndex = 0;
			}
		}
	}

	// Token: 0x0600816D RID: 33133 RVA: 0x00347684 File Offset: 0x00345884
	public bool HasPickupID(int pickupId)
	{
		return this.HasGun(pickupId) || this.HasActiveItem(pickupId) || this.HasPassiveItem(pickupId);
	}

	// Token: 0x0600816E RID: 33134 RVA: 0x003476A8 File Offset: 0x003458A8
	public bool HasGun(int pickupId)
	{
		for (int i = 0; i < this.inventory.AllGuns.Count; i++)
		{
			if (this.inventory.AllGuns[i].PickupObjectId == pickupId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600816F RID: 33135 RVA: 0x003476F8 File Offset: 0x003458F8
	public bool HasActiveItem(int pickupId)
	{
		int num = this.activeItems.FindIndex((PlayerItem a) => a.PickupObjectId == pickupId);
		return num >= 0;
	}

	// Token: 0x06008170 RID: 33136 RVA: 0x00347734 File Offset: 0x00345934
	public bool HasPassiveItem(int pickupId)
	{
		int num = this.passiveItems.FindIndex((PassiveItem a) => a.PickupObjectId == pickupId);
		return num >= 0;
	}

	// Token: 0x06008171 RID: 33137 RVA: 0x00347770 File Offset: 0x00345970
	public void RemovePassiveItem(int pickupId)
	{
		int num = this.passiveItems.FindIndex((PassiveItem p) => p.PickupObjectId == pickupId);
		if (num >= 0)
		{
			this.RemovePassiveItemAt(num);
		}
		else
		{
			UnityEngine.Debug.LogError("Failed to remove passive item because the player doesn't have it? pickupId = " + pickupId);
		}
	}

	// Token: 0x06008172 RID: 33138 RVA: 0x003477D0 File Offset: 0x003459D0
	protected void RemovePassiveItemAt(int index)
	{
		PassiveItem passiveItem = this.passiveItems[index];
		this.passiveItems.RemoveAt(index);
		GameUIRoot.Instance.RemovePassiveItemFromDock(passiveItem);
		UnityEngine.Object.Destroy(passiveItem);
		this.stats.RecalculateStats(this, false, false);
	}

	// Token: 0x06008173 RID: 33139 RVA: 0x00347818 File Offset: 0x00345A18
	public void BloopItemAboveHead(tk2dBaseSprite targetSprite, string overrideSprite = "")
	{
		this.m_blooper.DoBloop(targetSprite, overrideSprite, Color.white, false);
	}

	// Token: 0x06008174 RID: 33140 RVA: 0x00347830 File Offset: 0x00345A30
	public void BloopItemAboveHead(tk2dBaseSprite targetSprite, string overrideSprite, Color tintColor, bool addOutline = false)
	{
		this.m_blooper.DoBloop(targetSprite, overrideSprite, tintColor, addOutline);
	}

	// Token: 0x06008175 RID: 33141 RVA: 0x00347844 File Offset: 0x00345A44
	protected override void Fall()
	{
		if (this.m_isFalling)
		{
			return;
		}
		if (this.IsDodgeRolling && this.DodgeRollIsBlink)
		{
			return;
		}
		base.Fall();
		if (this.OnPitfall != null)
		{
			this.OnPitfall();
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.FALL_IN_END_TIMES, 0);
		}
		if (GameManager.Instance.InTutorial)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerFellInPit");
			if (this.m_dodgeRollState == PlayerController.DodgeRollState.OnGround)
			{
				GameManager.BroadcastRoomTalkDoerFsmEvent("playerFellInPitEarly");
			}
			else
			{
				GameManager.BroadcastRoomTalkDoerFsmEvent("playerFellInPitLate");
			}
		}
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.PITS_FALLEN_INTO, 1f);
		this.CurrentInputState = PlayerInputState.NoInput;
		base.healthHaver.IsVulnerable = false;
		base.healthHaver.EndFlashEffects();
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON && this.CurrentRoom != null && this.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL && (this.CurrentRoom.area.PrototypeRoomSpecialSubcategory == PrototypeDungeonRoom.RoomSpecialSubCategory.STANDARD_SHOP || this.CurrentRoom.area.PrototypeRoomSpecialSubcategory == PrototypeDungeonRoom.RoomSpecialSubCategory.WEIRD_SHOP))
		{
			this.LevelToLoadOnPitfall = "tt_nakatomi";
		}
		if (!string.IsNullOrEmpty(this.LevelToLoadOnPitfall) && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(this).m_inputState = PlayerInputState.NoInput;
		}
		this.m_cachedLevelToLoadOnPitfall = this.LevelToLoadOnPitfall;
		if (!string.IsNullOrEmpty(this.m_cachedLevelToLoadOnPitfall))
		{
			Pixelator.Instance.FadeToBlack(0.5f, false, 0.5f);
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		}
		this.LevelToLoadOnPitfall = string.Empty;
		base.StartCoroutine(this.FallDownCR());
	}

	// Token: 0x06008176 RID: 33142 RVA: 0x00347A2C File Offset: 0x00345C2C
	protected override void ModifyPitVectors(ref Rect modifiedRect)
	{
		base.ModifyPitVectors(ref modifiedRect);
		if (this.m_dodgeRollState == PlayerController.DodgeRollState.OnGround)
		{
			if (Mathf.Abs(this.lockedDodgeRollDirection.x) > 0.01f)
			{
				if (this.lockedDodgeRollDirection.x > 0.01f)
				{
					modifiedRect.xMax += PhysicsEngine.PixelToUnit(this.pitHelpers.Landing.x);
				}
				else if (this.lockedDodgeRollDirection.x < -0.01f)
				{
					modifiedRect.xMin -= PhysicsEngine.PixelToUnit(this.pitHelpers.Landing.x);
				}
			}
			if (Mathf.Abs(this.lockedDodgeRollDirection.y) > 0.01f)
			{
				if (this.lockedDodgeRollDirection.y > 0.01f)
				{
					modifiedRect.yMax += PhysicsEngine.PixelToUnit(this.pitHelpers.Landing.y);
				}
				else if (this.lockedDodgeRollDirection.y < -0.01f)
				{
					modifiedRect.yMin -= PhysicsEngine.PixelToUnit(this.pitHelpers.Landing.y);
				}
			}
		}
		else
		{
			if (Mathf.Abs(this.m_playerCommandedDirection.x) > 0.01f)
			{
				if (this.m_playerCommandedDirection.x < -0.01f)
				{
					modifiedRect.xMax += PhysicsEngine.PixelToUnit(this.pitHelpers.PreJump.x);
				}
				else if (this.m_playerCommandedDirection.x > 0.01f)
				{
					modifiedRect.xMin -= PhysicsEngine.PixelToUnit(this.pitHelpers.PreJump.x);
				}
			}
			if (Mathf.Abs(this.m_playerCommandedDirection.y) > 0.01f)
			{
				if (this.m_playerCommandedDirection.y < -0.01f)
				{
					modifiedRect.yMax += PhysicsEngine.PixelToUnit(this.pitHelpers.PreJump.y);
					modifiedRect.yMax += PhysicsEngine.PixelToUnit(base.specRigidbody.PrimaryPixelCollider.Width - base.specRigidbody.PrimaryPixelCollider.Height);
				}
				else if (this.m_playerCommandedDirection.y > 0.01f)
				{
					modifiedRect.yMin -= PhysicsEngine.PixelToUnit(this.pitHelpers.PreJump.y);
				}
			}
		}
	}

	// Token: 0x06008177 RID: 33143 RVA: 0x00347CB8 File Offset: 0x00345EB8
	public void PrepareForSceneTransition()
	{
		this.m_inputState = PlayerInputState.NoInput;
		this.IsVisible = false;
	}

	// Token: 0x06008178 RID: 33144 RVA: 0x00347CC8 File Offset: 0x00345EC8
	public void DoInitialFallSpawn(float invisibleDelay)
	{
		base.StartCoroutine(this.HandleFallSpawn(invisibleDelay));
	}

	// Token: 0x06008179 RID: 33145 RVA: 0x00347CD8 File Offset: 0x00345ED8
	public void DoSpinfallSpawn(float invisibleDelay)
	{
		if (base.healthHaver.IsDead)
		{
			return;
		}
		base.StartCoroutine(this.HandleSpinfallSpawn(invisibleDelay));
	}

	// Token: 0x0600817A RID: 33146 RVA: 0x00347CFC File Offset: 0x00345EFC
	protected IEnumerator HandleSpinfallSpawn(float invisibleDelay)
	{
		this.CurrentInputState = PlayerInputState.NoInput;
		yield return null;
		this.IsVisible = true;
		this.ToggleGunRenderers(false, string.Empty);
		this.ToggleHandRenderers(false, string.Empty);
		this.ToggleRenderer(false, "initial spawn");
		this.ToggleGunRenderers(false, string.Empty);
		this.ToggleHandRenderers(false, string.Empty);
		base.ToggleShadowVisiblity(false);
		yield return new WaitForSeconds(invisibleDelay);
		base.ToggleShadowVisiblity(false);
		AkSoundEngine.PostEvent("Play_Fall", base.gameObject);
		this.ToggleRenderer(true, "initial spawn");
		this.m_handlingQueuedAnimation = true;
		base.spriteAnimator.Play((!this.UseArmorlessAnim) ? "spinfall" : "spinfall_armorless");
		float startY = base.transform.position.y;
		SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/Spinfall_Shadow_VFX", ".prefab"), base.specRigidbody.UnitCenter, Quaternion.identity, true);
		float cachedHeightOffGround = base.sprite.HeightOffGround;
		bool m_cachedUpdateOffscreen = base.spriteAnimator.alwaysUpdateOffscreen;
		float elapsed = 1f;
		while (elapsed > 0f)
		{
			base.spriteAnimator.alwaysUpdateOffscreen = true;
			elapsed -= BraveTime.DeltaTime;
			float t = 1f - elapsed / 1f;
			float extraY = Mathf.Lerp(13f, 0f, t);
			base.sprite.transform.position = base.sprite.transform.position.WithY(startY + extraY);
			base.sprite.HeightOffGround = cachedHeightOffGround + extraY;
			base.sprite.UpdateZDepth();
			base.ToggleShadowVisiblity(false);
			yield return null;
		}
		base.sprite.HeightOffGround = cachedHeightOffGround;
		base.sprite.UpdateZDepth();
		base.spriteAnimator.alwaysUpdateOffscreen = m_cachedUpdateOffscreen;
		SpawnManager.SpawnVFX((GameObject)BraveResources.Load("Global VFX/Spinfall_Poof_VFX", ".prefab"), base.specRigidbody.UnitCenter, Quaternion.identity, true);
		this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Hard);
		this.m_handlingQueuedAnimation = false;
		this.ToggleGunRenderers(true, string.Empty);
		this.ToggleHandRenderers(true, string.Empty);
		base.ToggleShadowVisiblity(true);
		this.CurrentInputState = PlayerInputState.AllInput;
		yield break;
	}

	// Token: 0x0600817B RID: 33147 RVA: 0x00347D20 File Offset: 0x00345F20
	protected IEnumerator HandleFallSpawn(float invisibleDelay)
	{
		this.CurrentInputState = PlayerInputState.NoInput;
		if (this.IsGhost)
		{
			this.ToggleRenderer(false, "initial spawn");
			yield return new WaitForSeconds(invisibleDelay);
			this.ToggleRenderer(true, "initial spawn");
			this.IsVisible = true;
			this.ToggleHandRenderers(false, "ghostliness");
		}
		else
		{
			yield return null;
			this.IsVisible = true;
			this.ToggleRenderer(false, "initial spawn");
			this.ToggleGunRenderers(false, string.Empty);
			this.ToggleHandRenderers(false, string.Empty);
			yield return new WaitForSeconds(invisibleDelay);
			this.ToggleRenderer(true, "initial spawn");
			this.m_handlingQueuedAnimation = true;
			if (this.UseArmorlessAnim)
			{
				base.spriteAnimator.Play("pitfall_return_armorless");
				while (base.spriteAnimator.IsPlaying("pitfall_return_armorless"))
				{
					yield return null;
				}
			}
			else
			{
				base.spriteAnimator.Play("pitfall_return");
				while (base.spriteAnimator.IsPlaying("pitfall_return"))
				{
					yield return null;
				}
			}
			this.m_handlingQueuedAnimation = false;
			if (base.knockbackDoer)
			{
				base.knockbackDoer.ClearContinuousKnockbacks();
			}
			this.ToggleGunRenderers(true, string.Empty);
			this.ToggleHandRenderers(true, string.Empty);
			this.ToggleFollowerRenderers(true);
		}
		this.CurrentInputState = PlayerInputState.AllInput;
		if (this.carriedConsumables != null)
		{
			this.carriedConsumables.ForceUpdateUI();
		}
		yield break;
	}

	// Token: 0x0600817C RID: 33148 RVA: 0x00347D44 File Offset: 0x00345F44
	public void DoSpitOut()
	{
		base.StartCoroutine(this.HandleSpitOut());
	}

	// Token: 0x0600817D RID: 33149 RVA: 0x00347D54 File Offset: 0x00345F54
	protected IEnumerator HandleSpitOut()
	{
		if (this.IsGhost)
		{
			yield break;
		}
		this.CurrentInputState = PlayerInputState.NoInput;
		this.IsVisible = true;
		this.ToggleGunRenderers(false, "spit out");
		this.ToggleHandRenderers(false, "spit out");
		this.m_handlingQueuedAnimation = true;
		if (this.UseArmorlessAnim)
		{
			base.spriteAnimator.Play("spit_out_armorless");
			while (base.spriteAnimator.IsPlaying("spit_out_armorless"))
			{
				yield return null;
			}
		}
		else
		{
			base.spriteAnimator.Play("spit_out");
			while (base.spriteAnimator.IsPlaying("spit_out"))
			{
				yield return null;
			}
		}
		this.m_handlingQueuedAnimation = false;
		if (base.knockbackDoer)
		{
			base.knockbackDoer.ClearContinuousKnockbacks();
		}
		this.ToggleGunRenderers(true, "spit out");
		this.ToggleHandRenderers(true, "spit out");
		this.CurrentInputState = PlayerInputState.AllInput;
		base.PlayEffectOnActor((GameObject)ResourceCache.Acquire("Global VFX/VFX_Tarnisher_Effect"), new Vector3(0f, 0.5f, 0f), true, false, false);
		if (this.carriedConsumables != null)
		{
			this.carriedConsumables.ForceUpdateUI();
		}
		yield break;
	}

	// Token: 0x0600817E RID: 33150 RVA: 0x00347D70 File Offset: 0x00345F70
	protected void TogglePitClipping(bool doClip)
	{
		if (!this || !base.sprite || !base.sprite.gameObject)
		{
			return;
		}
		TileSpriteClipper tileSpriteClipper = base.sprite.gameObject.GetComponent<TileSpriteClipper>();
		if (tileSpriteClipper)
		{
			tileSpriteClipper.enabled = doClip;
		}
		tk2dBaseSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(base.sprite);
		if (outlineSprites == null)
		{
			return;
		}
		for (int i = 0; i < outlineSprites.Length; i++)
		{
			if (outlineSprites[i])
			{
				tileSpriteClipper = outlineSprites[i].GetComponent<TileSpriteClipper>();
			}
			if (tileSpriteClipper)
			{
				tileSpriteClipper.enabled = doClip;
			}
		}
	}

	// Token: 0x0600817F RID: 33151 RVA: 0x00347E20 File Offset: 0x00346020
	private RoomHandler GetCurrentCellPitfallTarget()
	{
		IntVector2 intVector = base.CenterPosition.ToIntVector2(VectorConversions.Floor);
		if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
		{
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			return cellData.targetPitfallRoom;
		}
		return null;
	}

	// Token: 0x06008180 RID: 33152 RVA: 0x00347E74 File Offset: 0x00346074
	protected IEnumerator PitRespawn(Vector2 splashPoint)
	{
		this.m_interruptingPitRespawn = false;
		base.healthHaver.IsVulnerable = true;
		this.actorReflectionAdditionalOffset = 0f;
		bool IsFallingIntoElevatorShaft = this.CurrentRoom != null && this.CurrentRoom.RoomFallValidForMaintenance();
		bool IsFallingIntoOtherRoom = (this.CurrentRoom != null && this.CurrentRoom.TargetPitfallRoom != null) || this.GetCurrentCellPitfallTarget() != null;
		bool DoLayerPass = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.PVP_ENABLED || IsFallingIntoElevatorShaft || IsFallingIntoOtherRoom;
		if (DoLayerPass)
		{
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
			SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
		}
		this.TogglePitClipping(false);
		base.ToggleShadowVisiblity(false);
		this.SetStencilVal(146);
		UnityEngine.Debug.Log(GameManager.Instance.CurrentLevelOverrideState + " clos");
		if (this.m_skipPitRespawn)
		{
			this.m_skipPitRespawn = false;
			this.m_interruptingPitRespawn = true;
			yield return new WaitForSeconds(0.5f);
		}
		else if (!string.IsNullOrEmpty(this.m_cachedLevelToLoadOnPitfall))
		{
			this.m_interruptingPitRespawn = true;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(this).m_inputState = PlayerInputState.NoInput;
				GameManager.Instance.GetOtherPlayer(this).m_interruptingPitRespawn = true;
			}
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			yield return new WaitForSeconds(0.5f);
		}
		else if (IsFallingIntoOtherRoom)
		{
			RoomHandler targetRoom = this.GetCurrentCellPitfallTarget();
			if (targetRoom == null)
			{
				targetRoom = this.CurrentRoom.TargetPitfallRoom;
			}
			if (targetRoom != null)
			{
				this.m_interruptingPitRespawn = true;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer2 = GameManager.Instance.GetOtherPlayer(this);
					if (otherPlayer2.IsFalling)
					{
						otherPlayer2.m_skipPitRespawn = true;
					}
				}
				this.TogglePitClipping(true);
				if (!this.m_skipPitRespawn)
				{
					Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
				}
				yield return new WaitForSeconds(0.5f);
				this.TogglePitClipping(false);
				bool succeeded = false;
				Transform[] childTransforms = targetRoom.hierarchyParent.GetComponentsInChildren<Transform>(true);
				for (int i = 0; i < childTransforms.Length; i++)
				{
					if (childTransforms[i].name == "Arrival" && !this.m_skipPitRespawn)
					{
						this.WarpToPoint(childTransforms[i].position.XY(), false, false);
						succeeded = true;
					}
				}
				if (!succeeded)
				{
					this.WarpToPoint(targetRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), new CellTypes?(CellTypes.FLOOR), false, null).Value.ToCenterVector2(), false, false);
				}
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer3 = GameManager.Instance.GetOtherPlayer(this);
					if (otherPlayer3)
					{
						otherPlayer3.ReuniteWithOtherPlayer(this, false);
						if (DoLayerPass)
						{
							otherPlayer3.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
							SpriteOutlineManager.ToggleOutlineRenderers(otherPlayer3.sprite, true);
						}
					}
				}
				if (!this.m_skipPitRespawn)
				{
					Pixelator.Instance.FadeToBlack(0.5f, true, 0f);
				}
				this.m_skipPitRespawn = false;
				if (this.CurrentRoom.OnTargetPitfallRoom != null)
				{
					this.CurrentRoom.OnTargetPitfallRoom();
				}
			}
		}
		else if (IsFallingIntoElevatorShaft)
		{
			GameObject maintenanceRoomObject = GameObject.Find("MaintenanceRoom(Clone)");
			if (maintenanceRoomObject != null)
			{
				this.m_interruptingPitRespawn = true;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer4 = GameManager.Instance.GetOtherPlayer(this);
					if (otherPlayer4.IsFalling)
					{
						otherPlayer4.m_skipPitRespawn = true;
					}
				}
				this.TogglePitClipping(true);
				if (!this.m_skipPitRespawn)
				{
					Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
				}
				yield return new WaitForSeconds(0.5f);
				this.TogglePitClipping(false);
				if (!this.m_skipPitRespawn)
				{
					this.WarpToPoint(maintenanceRoomObject.GetComponentInChildren<UsableBasicWarp>().sprite.WorldBottomCenter, false, false);
				}
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer5 = GameManager.Instance.GetOtherPlayer(this);
					otherPlayer5.ReuniteWithOtherPlayer(this, false);
				}
				if (!this.m_skipPitRespawn)
				{
					Pixelator.Instance.FadeToBlack(0.5f, true, 0f);
				}
				this.m_skipPitRespawn = false;
			}
		}
		else if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES && !this.ImmuneToPits.Value)
		{
			bool flag = false;
			if (this.CurrentGun && this.CurrentGun.gunName == "Mermaid Gun")
			{
				flag = true;
			}
			if (!flag)
			{
				base.healthHaver.ApplyDamage(0.5f, Vector2.zero, StringTableManager.GetEnemiesString("#PIT", -1), CoreDamageTypes.None, DamageCategory.Environment, false, null, false);
			}
		}
		if (this.m_interruptingPitRespawn)
		{
			this.m_isFalling = false;
			this.ClearDodgeRollState();
			this.previousMineCart = null;
			this.m_handlingQueuedAnimation = false;
			this.m_renderer.enabled = true;
			SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
			if (this.ShadowObject)
			{
				this.ShadowObject.GetComponent<Renderer>().enabled = true;
			}
			base.specRigidbody.CollideWithTileMap = true;
			base.specRigidbody.CollideWithOthers = true;
			this.ToggleGunRenderers(true, string.Empty);
			this.ToggleHandRenderers(true, string.Empty);
			base.ToggleShadowVisiblity(true);
			this.CurrentInputState = PlayerInputState.AllInput;
			this.m_interruptingPitRespawn = false;
			if (!string.IsNullOrEmpty(this.m_cachedLevelToLoadOnPitfall))
			{
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer6 = GameManager.Instance.GetOtherPlayer(this);
					if (otherPlayer6)
					{
						otherPlayer6.m_cachedLevelToLoadOnPitfall = string.Empty;
						otherPlayer6.LevelToLoadOnPitfall = string.Empty;
					}
				}
				base.ToggleShadowVisiblity(false);
				this.ToggleGunRenderers(false, string.Empty);
				this.ToggleHandRenderers(false, string.Empty);
				if (this.m_cachedLevelToLoadOnPitfall != "midgamesave")
				{
					if (this.m_cachedLevelToLoadOnPitfall == "ss_resourcefulrat")
					{
						FoyerPreloader.IsRatLoad = true;
						GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.RATGEON);
					}
					if (this.m_cachedLevelToLoadOnPitfall == "tt_nakatomi")
					{
						GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.OFFICEGEON);
					}
					GameManager.Instance.LoadCustomLevel(this.m_cachedLevelToLoadOnPitfall);
				}
			}
			if (IsFallingIntoOtherRoom)
			{
				GameManager.Instance.MainCameraController.transform.position = base.transform.position.WithZ(base.transform.position.y + GameManager.Instance.MainCameraController.CurrentZOffset);
				GameManager.Instance.MainCameraController.ForceToPlayerPosition(this, base.transform.position);
			}
			yield break;
		}
		this.m_cachedLevelToLoadOnPitfall = string.Empty;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER && base.healthHaver.IsDead)
		{
			this.m_renderer.enabled = true;
			SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
			if (this.ShadowObject)
			{
				this.ShadowObject.GetComponent<Renderer>().enabled = true;
			}
			yield break;
		}
		if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES)
		{
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.GetOtherPlayer(this).healthHaver.IsAlive)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
				if (otherPlayer.IsInMinecart)
				{
					this.TogglePitClipping(true);
					this.IgnoredByCamera = true;
					do
					{
						while (otherPlayer.IsInMinecart || !otherPlayer.IsGrounded)
						{
							yield return null;
						}
						yield return new WaitForSeconds(0.25f);
						while (otherPlayer.IsFalling)
						{
							yield return null;
						}
					}
					while (otherPlayer.IsInMinecart);
					this.IgnoredByCamera = false;
					this.TogglePitClipping(false);
				}
			}
			bool spawnPointOffscreen = !GameManager.Instance.MainCameraController.PointIsVisible(this.m_cachedPosition);
			if (this.m_cachedPosition.x <= 0f || this.m_cachedPosition.y <= 0f || this.IsPositionObscuredByTopWall(this.m_cachedPosition) || GameManager.Instance.Dungeon.CellSupportsFalling(this.m_cachedPosition.ToVector3ZUp(0f)) || (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && spawnPointOffscreen))
			{
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.GetOtherPlayer(this).healthHaver.IsAlive && !GameManager.Instance.Dungeon.CellSupportsFalling(GameManager.Instance.GetOtherPlayer(this).SpriteBottomCenter))
				{
					this.m_cachedPosition = GameManager.Instance.GetOtherPlayer(this).transform.position;
				}
				else
				{
					IntVector2? nearestAvailableCell = this.CurrentRoom.GetNearestAvailableCell(this.m_cachedPosition, new IntVector2?(new IntVector2(2, 3)), new CellTypes?(CellTypes.FLOOR), false, null);
					if (nearestAvailableCell != null)
					{
						this.m_cachedPosition = nearestAvailableCell.Value.ToVector2() + new Vector2(0f, 1f);
					}
					else
					{
						this.m_cachedPosition = this.CurrentRoom.GetBestRewardLocation(IntVector2.One, RoomHandler.RewardLocationStyle.CameraCenter, true).ToVector2();
					}
				}
			}
		}
		base.transform.position = this.m_cachedPosition.ToVector3ZUp(base.transform.position.z);
		base.specRigidbody.Velocity = Vector2.zero;
		base.specRigidbody.Reinitialize();
		this.WarpFollowersToPlayer(false);
		this.m_isFalling = false;
		this.ClearDodgeRollState();
		this.previousMineCart = null;
		this.m_handlingQueuedAnimation = true;
		this.m_renderer.enabled = true;
		base.ToggleShadowVisiblity(true);
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
		if (this.ShadowObject)
		{
			this.ShadowObject.GetComponent<Renderer>().enabled = true;
		}
		if (this.UseArmorlessAnim)
		{
			base.spriteAnimator.Play("pitfall_return_armorless");
			while (base.spriteAnimator.IsPlaying("pitfall_return_armorless"))
			{
				yield return null;
			}
		}
		else
		{
			base.spriteAnimator.Play("pitfall_return");
			while (base.spriteAnimator.IsPlaying("pitfall_return"))
			{
				yield return null;
			}
		}
		this.m_handlingQueuedAnimation = false;
		base.specRigidbody.CollideWithTileMap = true;
		base.specRigidbody.CollideWithOthers = true;
		if (base.healthHaver.IsAlive)
		{
			this.ToggleGunRenderers(true, string.Empty);
			this.ToggleHandRenderers(true, string.Empty);
		}
		if (!GameManager.Instance.IsLoadingLevel)
		{
			List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody, null, false);
			for (int j = 0; j < overlappingRigidbodies.Count; j++)
			{
				base.specRigidbody.RegisterGhostCollisionException(overlappingRigidbodies[j]);
				overlappingRigidbodies[j].RegisterGhostCollisionException(base.specRigidbody);
			}
			base.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
			if (this.CurrentRoom.OnPlayerReturnedFromPit != null)
			{
				this.CurrentRoom.OnPlayerReturnedFromPit(this);
			}
		}
		this.CurrentInputState = PlayerInputState.AllInput;
		yield break;
	}

	// Token: 0x06008181 RID: 33153 RVA: 0x00347E90 File Offset: 0x00346090
	private IEnumerator FallDownCR()
	{
		base.specRigidbody.CollideWithTileMap = false;
		base.specRigidbody.CollideWithOthers = false;
		base.specRigidbody.Velocity = Vector2.zero;
		if (!this.IsDodgeRolling)
		{
			yield return new WaitForSeconds(0.2f);
		}
		if (this.UseArmorlessAnim)
		{
			base.spriteAnimator.Play((!this.IsDodgeRolling) ? "pitfall_armorless" : "pitfall_down_armorless");
		}
		else
		{
			base.spriteAnimator.Play((!this.IsDodgeRolling) ? "pitfall" : "pitfall_down");
		}
		this.ToggleGunRenderers(false, "pitfall");
		this.ToggleHandRenderers(false, "pitfall");
		Vector2 accelVec = new Vector2(0f, -22f);
		float elapsed = 0f;
		Tribool readyForDepthSwap = Tribool.Unready;
		float m_cachedHeightOffGround = base.sprite.HeightOffGround;
		bool hasSplashed = false;
		float startY = base.CenterPosition.y;
		bool IsFallingIntoElevatorShaft = this.CurrentRoom != null && this.CurrentRoom.RoomFallValidForMaintenance();
		bool IsFallingIntoOtherRoom = (this.CurrentRoom != null && this.CurrentRoom.TargetPitfallRoom != null) || this.GetCurrentCellPitfallTarget() != null;
		Vector2 splashPoint = Vector2.zero;
		while (elapsed < 2f)
		{
			base.specRigidbody.Velocity = base.specRigidbody.Velocity + accelVec * BraveTime.DeltaTime;
			bool swappyDoos = !base.spriteAnimator.IsPlaying("pitfall") && !base.spriteAnimator.IsPlaying("pitfall_down");
			if (this.UseArmorlessAnim)
			{
				swappyDoos = swappyDoos && !base.spriteAnimator.IsPlaying("pitfall_armorless") && !base.spriteAnimator.IsPlaying("pitfall_down_armorless");
			}
			if (swappyDoos && elapsed > 0.1f)
			{
				this.m_renderer.enabled = false;
				SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
				base.specRigidbody.Velocity = Vector2.zero;
				accelVec = Vector2.zero;
				if (!hasSplashed)
				{
					hasSplashed = true;
					splashPoint = base.sprite.WorldCenter;
					if (!IsFallingIntoOtherRoom)
					{
						GameManager.Instance.Dungeon.tileIndices.DoSplashAtPosition(splashPoint);
					}
					this.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
				}
			}
			if (!(readyForDepthSwap == Tribool.Complete) || !this.m_renderer.enabled)
			{
				if (readyForDepthSwap)
				{
					base.sprite.HeightOffGround = -5f;
					bool flag = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.PVP_ENABLED || IsFallingIntoElevatorShaft || IsFallingIntoOtherRoom;
					bool flag2 = GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && !GameManager.PVP_ENABLED;
					if (this.CurrentRoom != null && this.CurrentRoom.RoomMovingPlatforms != null && this.CurrentRoom.RoomMovingPlatforms.Count > 0)
					{
						this.SetStencilVal(147);
						SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
					}
					if (flag)
					{
						base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
						SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
					}
					if (flag2)
					{
						TileSpriteClipper tileSpriteClipper = base.sprite.gameObject.GetOrAddComponent<TileSpriteClipper>();
						tileSpriteClipper.updateEveryFrame = true;
						tileSpriteClipper.doOptimize = false;
						tileSpriteClipper.clipMode = TileSpriteClipper.ClipMode.PitBounds;
						tileSpriteClipper.enabled = true;
						tk2dBaseSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(base.sprite);
						for (int i = 0; i < outlineSprites.Length; i++)
						{
							if (outlineSprites[i] && outlineSprites[i].gameObject)
							{
								tileSpriteClipper = outlineSprites[i].gameObject.GetOrAddComponent<TileSpriteClipper>();
								tileSpriteClipper.updateEveryFrame = true;
								tileSpriteClipper.doOptimize = false;
								tileSpriteClipper.clipMode = TileSpriteClipper.ClipMode.PitBounds;
								tileSpriteClipper.enabled = true;
								if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && !GameManager.PVP_ENABLED)
								{
									outlineSprites[i].gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
								}
							}
						}
					}
					readyForDepthSwap = Tribool.op_Increment(readyForDepthSwap);
				}
				else if (!readyForDepthSwap)
				{
					float num = Mathf.Lerp(0f, base.sprite.GetBounds().extents.y, elapsed / 0.2f);
					Vector3 vector = base.sprite.transform.position + base.sprite.GetBounds().center + new Vector3(0f, base.sprite.GetBounds().extents.y - num, 0f);
					if (GameManager.Instance.Dungeon.CellSupportsFalling(vector) || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.PVP_ENABLED)
					{
						readyForDepthSwap = Tribool.op_Increment(readyForDepthSwap);
					}
				}
			}
			this.actorReflectionAdditionalOffset = (startY - base.CenterPosition.y) * 1.25f;
			base.sprite.UpdateZDepth();
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		base.sprite.HeightOffGround = m_cachedHeightOffGround;
		base.StartCoroutine(this.PitRespawn(splashPoint));
		yield break;
	}

	// Token: 0x06008182 RID: 33154 RVA: 0x00347EAC File Offset: 0x003460AC
	protected void AnimationCompleteDelegate(tk2dSpriteAnimator anima, tk2dSpriteAnimationClip clippy)
	{
		bool flag = clippy.name.ToLowerInvariant().Contains("dodge");
		if (flag)
		{
			this.ToggleGunRenderers(true, "dodgeroll");
			this.ToggleHandRenderers(true, "dodgeroll");
			if (this.CurrentGun == null || string.IsNullOrEmpty(this.CurrentGun.dodgeAnimation))
			{
				this.ToggleGunRenderers(false, "postdodgeroll");
				this.ToggleHandRenderers(false, "postdodgeroll");
				this.m_postDodgeRollGunTimer = 0.05f;
			}
		}
		if (clippy.name.ToLowerInvariant().Contains("item_get"))
		{
			this.CurrentInputState = PlayerInputState.AllInput;
			base.GetComponent<HealthHaver>().IsVulnerable = true;
			this.ToggleGunRenderers(true, "itemGet");
			this.ToggleHandRenderers(true, "itemGet");
		}
		this.m_handlingQueuedAnimation = false;
		this.m_overrideGunAngle = null;
	}

	// Token: 0x06008183 RID: 33155 RVA: 0x00347F94 File Offset: 0x00346194
	public void TriggerItemAcquisition()
	{
		this.m_handlingQueuedAnimation = true;
		this.CurrentInputState = PlayerInputState.NoInput;
		base.specRigidbody.Velocity = Vector2.zero;
		this.ToggleGunRenderers(false, "itemGet");
		this.ToggleHandRenderers(false, "itemGet");
		base.GetComponent<HealthHaver>().IsVulnerable = false;
		base.spriteAnimator.Play((!this.UseArmorlessAnim) ? "item_get" : "item_get_armorless");
	}

	// Token: 0x06008184 RID: 33156 RVA: 0x00348008 File Offset: 0x00346208
	private void HandleAttachedSpriteDepth(float gunAngle)
	{
		float num = 1f;
		if (this.IsDodgeRolling)
		{
			gunAngle = BraveMathCollege.Atan2Degrees(this.lockedDodgeRollDirection);
		}
		float num2;
		if (gunAngle <= 155f && gunAngle >= 25f)
		{
			num = -1f;
			if (gunAngle < 120f && gunAngle >= 60f)
			{
				num2 = 0.15f;
			}
			else
			{
				num2 = 0.15f;
			}
		}
		else if (gunAngle <= -60f && gunAngle >= -120f)
		{
			num2 = -0.15f;
		}
		else
		{
			num2 = -0.15f;
		}
		for (int i = 0; i < this.m_attachedSprites.Count; i++)
		{
			this.m_attachedSprites[i].HeightOffGround = num2 + num * this.m_attachedSpriteDepths[i];
		}
	}

	// Token: 0x06008185 RID: 33157 RVA: 0x003480E8 File Offset: 0x003462E8
	public void ForceWalkInDirectionWhilePaused(DungeonData.Direction direction, float thresholdValue)
	{
		base.StartCoroutine(this.HandleForceWalkInDirectionWhilePaused(direction, thresholdValue));
	}

	// Token: 0x06008186 RID: 33158 RVA: 0x003480FC File Offset: 0x003462FC
	private IEnumerator HandleForceWalkInDirectionWhilePaused(DungeonData.Direction direction, float thresholdValue)
	{
		if (this.IsDodgeRolling)
		{
			this.ForceStopDodgeRoll();
		}
		Vector2 dirVec = DungeonData.GetIntVector2FromDirection(direction).ToVector2();
		Vector2 adjVelocity = dirVec * this.stats.MovementSpeed;
		this.m_handlingQueuedAnimation = true;
		base.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(0.25f);
		switch (direction)
		{
		case DungeonData.Direction.NORTH:
			while (base.CenterPosition.y < thresholdValue && Time.timeScale == 0f)
			{
				float modDeltaTime = Mathf.Clamp(GameManager.INVARIANT_DELTA_TIME, 0f, 0.1f);
				base.transform.position = base.transform.position + (adjVelocity * modDeltaTime).ToVector3ZUp(0f);
				base.specRigidbody.Reinitialize();
				string animationToPlay = this.GetBaseAnimationName(adjVelocity, 90f, false, false);
				if (!base.spriteAnimator.IsPlaying(animationToPlay))
				{
					base.spriteAnimator.Play(animationToPlay);
				}
				base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				this.ForceStaticFaceDirection(adjVelocity);
				yield return null;
			}
			break;
		case DungeonData.Direction.EAST:
			while (base.CenterPosition.x < thresholdValue && Time.timeScale == 0f)
			{
				float modDeltaTime2 = Mathf.Clamp(GameManager.INVARIANT_DELTA_TIME, 0f, 0.1f);
				base.transform.position = base.transform.position + (adjVelocity * modDeltaTime2).ToVector3ZUp(0f);
				base.specRigidbody.Reinitialize();
				string animationToPlay2 = this.GetBaseAnimationName(adjVelocity, 0f, false, false);
				if (!base.spriteAnimator.IsPlaying(animationToPlay2))
				{
					base.spriteAnimator.Play(animationToPlay2);
				}
				base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				this.ForceStaticFaceDirection(adjVelocity);
				yield return null;
			}
			break;
		case DungeonData.Direction.SOUTH:
			while (base.CenterPosition.y > thresholdValue && Time.timeScale == 0f)
			{
				float modDeltaTime3 = Mathf.Clamp(GameManager.INVARIANT_DELTA_TIME, 0f, 0.1f);
				base.transform.position = base.transform.position + (adjVelocity * modDeltaTime3).ToVector3ZUp(0f);
				base.specRigidbody.Reinitialize();
				string animationToPlay3 = this.GetBaseAnimationName(adjVelocity, -90f, false, false);
				if (!base.spriteAnimator.IsPlaying(animationToPlay3))
				{
					base.spriteAnimator.Play(animationToPlay3);
				}
				base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				this.ForceStaticFaceDirection(adjVelocity);
				yield return null;
			}
			break;
		case DungeonData.Direction.WEST:
			while (base.CenterPosition.x > thresholdValue && Time.timeScale == 0f)
			{
				float modDeltaTime4 = Mathf.Clamp(GameManager.INVARIANT_DELTA_TIME, 0f, 0.1f);
				base.transform.position = base.transform.position + (adjVelocity * modDeltaTime4).ToVector3ZUp(0f);
				base.specRigidbody.Reinitialize();
				string animationToPlay4 = this.GetBaseAnimationName(adjVelocity, 179.9f, false, false);
				if (!base.spriteAnimator.IsPlaying(animationToPlay4))
				{
					base.spriteAnimator.Play(animationToPlay4);
				}
				base.spriteAnimator.UpdateAnimation(GameManager.INVARIANT_DELTA_TIME);
				this.ForceStaticFaceDirection(adjVelocity);
				yield return null;
			}
			break;
		}
		if (this.IsDodgeRolling)
		{
			this.ForceStopDodgeRoll();
		}
		base.specRigidbody.Velocity = Vector2.zero;
		this.m_handlingQueuedAnimation = false;
		yield break;
	}

	// Token: 0x06008187 RID: 33159 RVA: 0x00348128 File Offset: 0x00346328
	public bool IsBackfacing()
	{
		float num = ((!this.IsDodgeRolling || !this.m_handlingQueuedAnimation) ? this.m_currentGunAngle : BraveMathCollege.Atan2Degrees(this.lockedDodgeRollDirection));
		return num <= 155f && num >= 25f;
	}

	// Token: 0x06008188 RID: 33160 RVA: 0x0034817C File Offset: 0x0034637C
	public string GetBaseAnimationSuffix(bool useCardinal = false)
	{
		float num = ((!this.IsDodgeRolling || !this.m_handlingQueuedAnimation) ? this.m_currentGunAngle : BraveMathCollege.Atan2Degrees(this.lockedDodgeRollDirection));
		if (num <= 155f && num >= 25f)
		{
			if (num < 120f && num >= 60f)
			{
				return (!useCardinal) ? "_back" : "_north";
			}
			return (!useCardinal) ? "_back_right" : "_north_east";
		}
		else
		{
			if (num <= -60f && num >= -120f)
			{
				return (!useCardinal) ? "_front" : "_south";
			}
			return (!useCardinal) ? "_front_right" : "_south_east";
		}
	}

	// Token: 0x06008189 RID: 33161 RVA: 0x00348250 File Offset: 0x00346450
	public int GetMirrorSpriteID()
	{
		float num = BraveMathCollege.Atan2Degrees(Vector2.Scale(BraveMathCollege.DegreesToVector(this.m_currentGunAngle, 1f), new Vector2(1f, -1f)));
		string baseAnimationName = this.GetBaseAnimationName(this.m_playerCommandedDirection.WithY(this.m_playerCommandedDirection.y * -1f), num, true, false);
		tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(baseAnimationName);
		int currentFrame = base.spriteAnimator.CurrentFrame;
		if (clipByName != null && currentFrame >= 0 && currentFrame < clipByName.frames.Length)
		{
			return clipByName.frames[currentFrame].spriteId;
		}
		return base.sprite.spriteId;
	}

	// Token: 0x0600818A RID: 33162 RVA: 0x003482FC File Offset: 0x003464FC
	protected virtual string GetBaseAnimationName(Vector2 v, float gunAngle, bool invertThresholds = false, bool forceTwoHands = false)
	{
		string text = string.Empty;
		bool flag = this.CurrentGun != null;
		if (flag && this.CurrentGun.Handedness == GunHandedness.NoHanded)
		{
			forceTwoHands = true;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			flag = false;
		}
		float num = 155f;
		float num2 = 25f;
		if (invertThresholds)
		{
			num = -155f;
			num2 -= 50f;
		}
		float num3 = 120f;
		float num4 = 60f;
		float num5 = -60f;
		float num6 = -120f;
		bool flag2 = gunAngle <= num && gunAngle >= num2;
		if (invertThresholds)
		{
			flag2 = gunAngle <= num || gunAngle >= num2;
		}
		if (this.IsGhost)
		{
			if (flag2)
			{
				if (gunAngle < num3 && gunAngle >= num4)
				{
					text = "ghost_idle_back";
				}
				else
				{
					float num7 = 105f;
					if (Mathf.Abs(gunAngle) > num7)
					{
						text = "ghost_idle_back_left";
					}
					else
					{
						text = "ghost_idle_back_right";
					}
				}
			}
			else if (gunAngle <= num5 && gunAngle >= num6)
			{
				text = "ghost_idle_front";
			}
			else
			{
				float num8 = 105f;
				if (Mathf.Abs(gunAngle) > num8)
				{
					text = "ghost_idle_left";
				}
				else
				{
					text = "ghost_idle_right";
				}
			}
		}
		else if (this.IsFlying)
		{
			if (flag2)
			{
				if (gunAngle < num3 && gunAngle >= num4)
				{
					text = "jetpack_up";
				}
				else
				{
					text = "jetpack_right_bw";
				}
			}
			else if (gunAngle <= num5 && gunAngle >= num6)
			{
				text = ((!this.RenderBodyHand) ? "jetpack_down" : "jetpack_down_hand");
			}
			else
			{
				text = ((!this.RenderBodyHand) ? "jetpack_right" : "jetpack_right_hand");
			}
		}
		else if (v == Vector2.zero || this.IsStationary)
		{
			if (this.IsPetting)
			{
				text = "pet";
			}
			else if (flag2)
			{
				if (gunAngle < num3 && gunAngle >= num4)
				{
					string text2 = (((!forceTwoHands && flag) || this.ForceHandless) ? ((!this.RenderBodyHand) ? "idle_backward" : "idle_backward_hand") : "idle_backward_twohands");
					text = text2;
				}
				else
				{
					string text3 = (((!forceTwoHands && flag) || this.ForceHandless) ? "idle_bw" : "idle_bw_twohands");
					text = text3;
				}
			}
			else if (gunAngle <= num5 && gunAngle >= num6)
			{
				string text4 = (((!forceTwoHands && flag) || this.ForceHandless) ? ((!this.RenderBodyHand) ? "idle_forward" : "idle_forward_hand") : "idle_forward_twohands");
				text = text4;
			}
			else
			{
				string text5 = (((!forceTwoHands && flag) || this.ForceHandless) ? ((!this.RenderBodyHand) ? "idle" : "idle_hand") : "idle_twohands");
				text = text5;
			}
		}
		else if (flag2)
		{
			string text6 = (((!forceTwoHands && flag) || this.ForceHandless) ? "run_right_bw" : "run_right_bw_twohands");
			if (gunAngle < num3 && gunAngle >= num4)
			{
				text6 = (((!forceTwoHands && flag) || this.ForceHandless) ? ((!this.RenderBodyHand) ? "run_up" : "run_up_hand") : "run_up_twohands");
			}
			text = text6;
		}
		else
		{
			string text7 = "run_right";
			if (gunAngle <= num5 && gunAngle >= num6)
			{
				text7 = "run_down";
			}
			if ((forceTwoHands || !flag) && !this.ForceHandless)
			{
				text7 += "_twohands";
			}
			else if (this.RenderBodyHand)
			{
				text7 += "_hand";
			}
			text = text7;
		}
		if (this.UseArmorlessAnim && !this.IsGhost)
		{
			text += "_armorless";
		}
		return text;
	}

	// Token: 0x0600818B RID: 33163 RVA: 0x00348740 File Offset: 0x00346940
	private void HandleAnimations(Vector2 v, float gunAngle)
	{
		if (this.m_handlingQueuedAnimation)
		{
			return;
		}
		if (this.CurrentGun == null || this.IsGhost)
		{
			gunAngle = BraveMathCollege.Atan2Degrees((!(this.m_playerCommandedDirection == Vector2.zero)) ? this.m_playerCommandedDirection : this.m_lastNonzeroCommandedDirection);
		}
		string baseAnimationName = this.GetBaseAnimationName(v, gunAngle, false, false);
		if (!base.spriteAnimator.IsPlaying(baseAnimationName))
		{
			base.spriteAnimator.Play(baseAnimationName);
		}
	}

	// Token: 0x0600818C RID: 33164 RVA: 0x003487CC File Offset: 0x003469CC
	protected bool IsKeyboardAndMouse()
	{
		return BraveInput.GetInstanceForPlayer(this.PlayerIDX).IsKeyboardAndMouse(false);
	}

	// Token: 0x1700133C RID: 4924
	// (get) Token: 0x0600818D RID: 33165 RVA: 0x003487E0 File Offset: 0x003469E0
	protected bool UseFakeSemiAutoCooldown
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600818E RID: 33166 RVA: 0x003487E4 File Offset: 0x003469E4
	protected Vector3 DetermineAimPointInWorld()
	{
		if (Time.timeScale == 0f)
		{
			return this.unadjustedAimPoint;
		}
		Vector3 vector = Vector3.zero;
		Camera component = GameManager.Instance.MainCameraController.GetComponent<Camera>();
		Vector3 position = this.gunAttachPoint.position;
		Vector2? vector2 = this.forceAimPoint;
		if (vector2 != null)
		{
			this.unadjustedAimPoint = this.forceAimPoint.Value;
			vector = this.unadjustedAimPoint;
		}
		else if (this.IsKeyboardAndMouse())
		{
			Ray ray = component.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
			Plane plane = new Plane(Vector3.forward, position);
			float num;
			if (plane.Raycast(ray, out num))
			{
				this.unadjustedAimPoint = ray.GetPoint(num);
				vector = this.unadjustedAimPoint;
			}
		}
		else
		{
			bool flag = BraveInput.AutoAimMode == BraveInput.AutoAim.SuperAutoAim;
			Vector2 unitCenter = base.specRigidbody.HitboxPixelCollider.UnitCenter;
			Vector2 vector3 = this.m_activeActions.Aim.Vector;
			bool flag2 = BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButton(GungeonActions.GungeonActionType.Shoot) || BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButtonDown(GungeonActions.GungeonActionType.Shoot) || this.SuperAutoAimTarget != null;
			flag2 &= vector3.magnitude > 0.4f;
			bool flag3 = false;
			bool flag4 = false;
			switch (GameManager.Options.controllerAutoAim)
			{
			case GameOptions.ControllerAutoAim.ALWAYS:
				flag4 = true;
				break;
			case GameOptions.ControllerAutoAim.NEVER:
				flag4 = false;
				break;
			case GameOptions.ControllerAutoAim.COOP_ONLY:
				flag4 = this.PlayerIDX != 0;
				break;
			}
			if (GameManager.Options.controllerAutoAim == GameOptions.ControllerAutoAim.AUTO_DETECT && !this.IsKeyboardAndMouse() && this.IsPrimaryPlayer)
			{
				if (this.IsInCombat)
				{
					if (vector3.magnitude < 0.4f)
					{
						float aanonStickTime = PlayerController.AANonStickTime;
						PlayerController.AANonStickTime = Mathf.Min(PlayerController.AANonStickTime + BraveTime.DeltaTime, 660f);
						PlayerController.AAStickTime = Mathf.Min(PlayerController.AAStickTime, 660f - PlayerController.AANonStickTime);
					}
					else
					{
						PlayerController.AAStickTime = Mathf.Min(PlayerController.AAStickTime + BraveTime.DeltaTime * 1.5f, 660f);
						PlayerController.AANonStickTime = Mathf.Min(PlayerController.AANonStickTime, 660f - PlayerController.AAStickTime);
					}
					if (!PlayerController.AACanWarn && PlayerController.AANonStickTime < 300f && Time.realtimeSinceStartup > PlayerController.AALastWarnTime + 300f)
					{
						PlayerController.AACanWarn = true;
					}
				}
				else if (PlayerController.AANonStickTime > 600f)
				{
					this.DoAutoAimNotification(false);
					GameManager.Options.controllerAutoAim = GameOptions.ControllerAutoAim.ALWAYS;
					PlayerController.AAStickTime = 0f;
					PlayerController.AANonStickTime = 0f;
					PlayerController.AALastWarnTime = -1000f;
					PlayerController.AACanWarn = true;
				}
				else if (PlayerController.AACanWarn && PlayerController.AANonStickTime > 300f)
				{
					this.DoAutoAimNotification(true);
					PlayerController.AALastWarnTime = Time.realtimeSinceStartup;
					PlayerController.AACanWarn = false;
				}
			}
			flag4 = flag4 && vector3.magnitude < 0.4f;
			if (this.HighAccuracyAimMode)
			{
				if (!this.m_activeActions.HighAccuracyAimMode)
				{
					this.m_activeActions.HighAccuracyAimMode = true;
				}
				if (vector3.magnitude < 0.2f)
				{
					vector3 = Vector2.zero;
				}
				else
				{
					vector3 = vector3.normalized * Mathf.Lerp(0.2f, 1f, vector3.magnitude);
				}
				if (this.m_previousAimVector != Vector2.zero && (double)this.m_previousAimVector.magnitude > 0.8 && vector3 != Vector2.zero && vector3.magnitude < 0.6f)
				{
					float num2 = BraveMathCollege.AbsAngleBetween(vector3.ToAngle(), this.m_previousAimVector.ToAngle());
					if (num2 < 15f || num2 > 155f)
					{
						vector3 = this.m_previousAimVector.normalized * 0.5f;
					}
				}
				if (vector3 == Vector2.zero || this.m_previousAimVector == Vector2.zero || BraveMathCollege.AbsAngleBetween(vector3.ToAngle(), this.m_previousAimVector.ToAngle()) > 10f)
				{
					this.m_previousAimVector = vector3;
				}
				vector3 = BraveMathCollege.MovingAverage(this.m_previousAimVector, vector3, 3);
				this.m_previousAimVector = vector3;
			}
			else
			{
				if (this.m_activeActions.HighAccuracyAimMode)
				{
					this.m_activeActions.HighAccuracyAimMode = false;
				}
				if (vector3.magnitude < 0.4f)
				{
					if (this.m_allowMoveAsAim)
					{
						vector3 = this.m_activeActions.Move.Vector;
					}
					else
					{
						flag3 = true;
					}
				}
				vector3 = this.AdjustInputVector(vector3, BraveInput.MagnetAngles.aimCardinal, BraveInput.MagnetAngles.aimOrdinal);
			}
			if (flag && !flag2)
			{
				this.SuperAutoAimTarget = null;
			}
			if (vector3.magnitude < 0.4f)
			{
				vector3 = this.m_cachedAimDirection;
			}
			this.m_cachedAimDirection = vector3;
			this.unadjustedAimPoint = position + vector3.normalized * 6f;
			vector = position + vector3.normalized * 150f;
			bool flag5 = false;
			float num3 = 20f;
			bool flag6 = this.CurrentGun || this is PlayerSpaceshipController;
			bool flag7 = !(this.CurrentGun == null) && this.CurrentGun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Beam;
			if ((GameManager.Options.controllerAimAssistMultiplier > 0f || flag4) && flag6 && (GameManager.Options.controllerBeamAimAssist || !flag7) && this.CurrentRoom != null && !flag3)
			{
				Vector2 vector4 = unitCenter + vector3.normalized * num3;
				float num4 = (vector4 - unitCenter).ToAngle();
				List<IAutoAimTarget> autoAimTargets = this.CurrentRoom.GetAutoAimTargets();
				if (this.CurrentRoom != null && (autoAimTargets != null || GameManager.PVP_ENABLED))
				{
					Projectile projectile = null;
					if (this.CurrentGun && this.CurrentGun.DefaultModule != null)
					{
						projectile = this.CurrentGun.DefaultModule.GetCurrentProjectile();
					}
					float num5 = ((!projectile) ? float.MaxValue : projectile.baseData.speed);
					if (num5 < 0f)
					{
						num5 = float.MaxValue;
					}
					IAutoAimTarget autoAimTarget = null;
					float num6 = 0f;
					float num7 = 0f;
					int num8 = ((autoAimTargets == null) ? 0 : autoAimTargets.Count);
					num8 += ((!GameManager.PVP_ENABLED) ? 0 : 1);
					for (int i = 0; i < num8; i++)
					{
						IAutoAimTarget autoAimTarget2;
						if (autoAimTargets != null && i < autoAimTargets.Count)
						{
							autoAimTarget2 = autoAimTargets[i];
						}
						else
						{
							autoAimTarget2 = GameManager.Instance.GetOtherPlayer(this);
						}
						if (autoAimTarget2 != null)
						{
							if (!(autoAimTarget2 is Component) || autoAimTarget2 as Component)
							{
								if (autoAimTarget2.IsValid)
								{
									Vector2 aimCenter = autoAimTarget2.AimCenter;
									if (GameManager.Instance.MainCameraController.PointIsVisible(aimCenter, 0.05f))
									{
										float num9 = Vector2.Distance(unitCenter, aimCenter) / num5;
										Vector2 vector5 = aimCenter + autoAimTarget2.Velocity * num9;
										float num10 = (vector5 - unitCenter).ToAngle();
										float num11 = Mathf.Abs(BraveMathCollege.ClampAngle180(num10 - num4));
										if (flag && this.SuperAutoAimTarget == autoAimTarget2)
										{
											num11 *= BraveInput.ControllerAutoAimDegrees / BraveInput.ControllerSuperAutoAimDegrees;
											if (flag7)
											{
												num11 *= 3f;
											}
										}
										else if (flag7)
										{
											num11 *= 2f;
										}
										if (flag4)
										{
											Vector2 vector6 = vector5 - unitCenter;
											float num12 = ((!(vector6 == Vector2.zero)) ? vector6.magnitude : 0f);
											if (!autoAimTarget2.IgnoreForSuperDuperAutoAim && num12 >= autoAimTarget2.MinDistForSuperDuperAutoAim && (autoAimTarget == null || num12 < num7) && (this.m_superDuperAutoAimTimer <= 0f || autoAimTarget2 == this.SuperDuperAimTarget))
											{
												RaycastResult raycastResult;
												if (!PhysicsEngine.Instance.Raycast(unitCenter, vector6.normalized, vector6.magnitude - 2f, out raycastResult, true, false, 2147483647, null, false, null, null))
												{
													vector = vector5;
													flag5 = true;
													this.SuperDuperAimPoint = vector;
													autoAimTarget = autoAimTarget2;
													num7 = num12;
												}
												RaycastResult.Pool.Free(ref raycastResult);
											}
										}
										else if (num11 < BraveInput.ControllerAutoAimDegrees && (autoAimTarget == null || num11 < num6))
										{
											Vector2 vector7 = vector5 - unitCenter;
											RaycastResult raycastResult2;
											if (!PhysicsEngine.Instance.Raycast(unitCenter, vector7.normalized, vector7.magnitude - 2f, out raycastResult2, true, false, 2147483647, null, false, null, null))
											{
												vector = vector5;
												flag5 = true;
												autoAimTarget = autoAimTarget2;
												num6 = num11;
											}
											RaycastResult.Pool.Free(ref raycastResult2);
										}
									}
								}
							}
						}
					}
					if (flag4)
					{
						if (!flag5 && this.m_superDuperAutoAimTimer > 0f)
						{
							vector = this.SuperDuperAimPoint;
						}
						if (autoAimTarget != this.SuperDuperAimTarget)
						{
							this.SuperDuperAimTarget = autoAimTarget;
							this.m_superDuperAutoAimTimer = 0.5f;
						}
						else if (autoAimTarget == null && this.m_superDuperAutoAimTimer <= 0f)
						{
							this.SuperDuperAimTarget = null;
						}
					}
					if (flag)
					{
						if (this.SuperAutoAimTarget != null && this.SuperAutoAimTarget != autoAimTarget)
						{
							this.SuperAutoAimTarget = null;
						}
						else if (this.SuperAutoAimTarget == null && autoAimTarget != null && flag2)
						{
							this.SuperAutoAimTarget = autoAimTarget;
						}
					}
				}
			}
		}
		this.m_cachedAimDirection = vector - position;
		return vector;
	}

	// Token: 0x0600818F RID: 33167 RVA: 0x00349294 File Offset: 0x00347494
	public void ForceStopDodgeRoll()
	{
		this.m_handlingQueuedAnimation = false;
		this.m_dodgeRollTimer = this.rollStats.GetModifiedTime(this);
		this.ClearDodgeRollState();
		this.previousMineCart = null;
	}

	// Token: 0x1700133D RID: 4925
	// (get) Token: 0x06008190 RID: 33168 RVA: 0x003492BC File Offset: 0x003474BC
	protected virtual bool CanDodgeRollWhileFlying
	{
		get
		{
			return this.AdditionalCanDodgeRollWhileFlying.Value || (PassiveItem.ActiveFlagItems.ContainsKey(this) && PassiveItem.ActiveFlagItems[this].ContainsKey(typeof(WingsItem)));
		}
	}

	// Token: 0x1700133E RID: 4926
	// (get) Token: 0x06008191 RID: 33169 RVA: 0x00349308 File Offset: 0x00347508
	public virtual bool DodgeRollIsBlink
	{
		get
		{
			return (!GameManager.Instance.Dungeon || !GameManager.Instance.Dungeon.IsEndTimes) && PassiveItem.ActiveFlagItems.ContainsKey(this) && PassiveItem.ActiveFlagItems[this].ContainsKey(typeof(BlinkPassiveItem));
		}
	}

	// Token: 0x06008192 RID: 33170 RVA: 0x0034936C File Offset: 0x0034756C
	private IEnumerator HandleBlinkDodgeRoll()
	{
		if (this.IsDodgeRolling)
		{
			yield break;
		}
		if (this.IsFlying && !this.CanDodgeRollWhileFlying)
		{
			yield break;
		}
		if (this.OnPreDodgeRoll != null)
		{
			this.OnPreDodgeRoll(this);
		}
		if (this.IsStationary)
		{
			yield break;
		}
		if (base.knockbackDoer)
		{
			base.knockbackDoer.ClearContinuousKnockbacks();
		}
		this.m_rollDamagedEnemies.Clear();
		this.m_dodgeRollTimer = 0f;
		this.m_dodgeRollState = PlayerController.DodgeRollState.Blink;
		this.m_currentDodgeRollDepth++;
		if (this.OnRollStarted != null)
		{
			this.OnRollStarted(this, this.lockedDodgeRollDirection);
		}
		this.IsEthereal = true;
		this.IsVisible = false;
		if (this.IsPrimaryPlayer)
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerOnePosition = true;
			GameManager.Instance.MainCameraController.OverridePlayerOnePosition = base.CenterPosition;
		}
		else
		{
			GameManager.Instance.MainCameraController.UseOverridePlayerTwoPosition = true;
			GameManager.Instance.MainCameraController.OverridePlayerTwoPosition = base.CenterPosition;
		}
		if (this.CurrentFireMeterValue <= 0f)
		{
			yield break;
		}
		this.CurrentFireMeterValue = Mathf.Max(0f, this.CurrentFireMeterValue -= 0.5f);
		if (this.CurrentFireMeterValue == 0f)
		{
			this.IsOnFire = false;
			yield break;
		}
		yield break;
	}

	// Token: 0x06008193 RID: 33171 RVA: 0x00349388 File Offset: 0x00347588
	private bool CheckDodgeRollDepth()
	{
		if (this.IsSlidingOverSurface && !this.DodgeRollIsBlink)
		{
			return !this.CurrentRoom.IsShop && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.TUTORIAL;
		}
		bool flag = PassiveItem.IsFlagSetForCharacter(this, typeof(PegasusBootsItem));
		int num = ((!flag) ? 1 : 2);
		if (flag && this.HasActiveBonusSynergy(CustomSynergyType.TRIPLE_JUMP, false))
		{
			num++;
		}
		if (this.DodgeRollIsBlink)
		{
			num = 1;
		}
		return !this.IsDodgeRolling || this.m_currentDodgeRollDepth < num;
	}

	// Token: 0x06008194 RID: 33172 RVA: 0x00349434 File Offset: 0x00347634
	private bool StartDodgeRoll(Vector2 direction)
	{
		if (direction == Vector2.zero)
		{
			return false;
		}
		if (!this.CheckDodgeRollDepth())
		{
			return false;
		}
		if (this.IsFlying && !this.CanDodgeRollWhileFlying)
		{
			return false;
		}
		if (this.OnPreDodgeRoll != null)
		{
			this.OnPreDodgeRoll(this);
		}
		if (this.IsStationary)
		{
			return false;
		}
		if (base.knockbackDoer)
		{
			base.knockbackDoer.ClearContinuousKnockbacks();
		}
		this.lockedDodgeRollDirection = direction;
		this.m_rollDamagedEnemies.Clear();
		base.spriteAnimator.Stop();
		this.m_dodgeRollTimer = 0f;
		this.m_dodgeRollState = ((!this.rollStats.hasPreDodgeDelay) ? PlayerController.DodgeRollState.InAir : PlayerController.DodgeRollState.PreRollDelay);
		this.m_currentDodgeRollDepth++;
		if (this.OnRollStarted != null)
		{
			this.OnRollStarted(this, this.lockedDodgeRollDirection);
		}
		if (this.DodgeRollIsBlink)
		{
			this.IsEthereal = true;
			this.IsVisible = false;
			this.PlayDodgeRollAnimation(direction);
		}
		else
		{
			this.PlayDodgeRollAnimation(direction);
			if (this.CurrentGun != null)
			{
				this.CurrentGun.HandleDodgeroll(this.rollStats.GetModifiedTime(this));
			}
			if (this.CurrentGun == null || string.IsNullOrEmpty(this.CurrentGun.dodgeAnimation))
			{
				this.ToggleGunRenderers(false, "dodgeroll");
			}
			this.ToggleHandRenderers(false, "dodgeroll");
		}
		if (this.CurrentFireMeterValue > 0f)
		{
			this.CurrentFireMeterValue = Mathf.Max(0f, this.CurrentFireMeterValue -= 0.5f);
			if (this.CurrentFireMeterValue == 0f)
			{
				this.IsOnFire = false;
			}
		}
		return true;
	}

	// Token: 0x06008195 RID: 33173 RVA: 0x00349608 File Offset: 0x00347808
	public bool ForceStartDodgeRoll(Vector2 vec)
	{
		return this.StartDodgeRoll(vec);
	}

	// Token: 0x06008196 RID: 33174 RVA: 0x00349614 File Offset: 0x00347814
	public bool ForceStartDodgeRoll()
	{
		Vector2 vector = this.AdjustInputVector(this.m_activeActions.Move.Vector, BraveInput.MagnetAngles.movementCardinal, BraveInput.MagnetAngles.movementOrdinal);
		return this.StartDodgeRoll(vector);
	}

	// Token: 0x06008197 RID: 33175 RVA: 0x00349654 File Offset: 0x00347854
	protected bool CanBlinkToPoint(Vector2 point, Vector2 centerOffset)
	{
		bool flag = this.IsValidPlayerPosition(point + centerOffset);
		if (flag && this.CurrentRoom != null)
		{
			CellData cellData = GameManager.Instance.Dungeon.data[point.ToIntVector2(VectorConversions.Floor)];
			if (cellData == null)
			{
				return false;
			}
			RoomHandler nearestRoom = cellData.nearestRoom;
			if (cellData.type != CellType.FLOOR)
			{
				flag = false;
			}
			if (this.CurrentRoom.IsSealed && nearestRoom != this.CurrentRoom)
			{
				flag = false;
			}
			if (this.CurrentRoom.IsSealed && cellData.isExitCell)
			{
				flag = false;
			}
			if (nearestRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED || nearestRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
			{
				flag = false;
			}
		}
		if (this.CurrentRoom == null)
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x06008198 RID: 33176 RVA: 0x0034971C File Offset: 0x0034791C
	protected void UpdateBlinkShadow(Vector2 delta, bool canWarpDirectly)
	{
		if (this.m_extantBlinkShadow == null)
		{
			GameObject gameObject = new GameObject("blinkshadow");
			this.m_extantBlinkShadow = tk2dSprite.AddComponent(gameObject, base.sprite.Collection, base.sprite.spriteId);
			this.m_extantBlinkShadow.transform.position = this.m_cachedBlinkPosition + (base.sprite.transform.position.XY() - base.specRigidbody.UnitCenter);
			tk2dSpriteAnimator tk2dSpriteAnimator = this.m_extantBlinkShadow.gameObject.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimator.Library = base.spriteAnimator.Library;
			this.m_extantBlinkShadow.renderer.material.SetColor(this.m_overrideFlatColorID, (!canWarpDirectly) ? new Color(0.4f, 0f, 0f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f));
			this.m_extantBlinkShadow.usesOverrideMaterial = true;
			this.m_extantBlinkShadow.FlipX = base.sprite.FlipX;
			this.m_extantBlinkShadow.FlipY = base.sprite.FlipY;
			if (this.OnBlinkShadowCreated != null)
			{
				this.OnBlinkShadowCreated(this.m_extantBlinkShadow);
			}
		}
		else
		{
			if (delta == Vector2.zero)
			{
				this.m_extantBlinkShadow.spriteAnimator.Stop();
				this.m_extantBlinkShadow.SetSprite(base.sprite.Collection, base.sprite.spriteId);
			}
			else
			{
				string baseAnimationName = this.GetBaseAnimationName(delta, this.m_currentGunAngle, false, true);
				if (!string.IsNullOrEmpty(baseAnimationName) && !this.m_extantBlinkShadow.spriteAnimator.IsPlaying(baseAnimationName))
				{
					this.m_extantBlinkShadow.spriteAnimator.Play(baseAnimationName);
				}
			}
			this.m_extantBlinkShadow.renderer.material.SetColor(this.m_overrideFlatColorID, (!canWarpDirectly) ? new Color(0.4f, 0f, 0f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f));
			this.m_extantBlinkShadow.transform.position = this.m_cachedBlinkPosition + (base.sprite.transform.position.XY() - base.specRigidbody.UnitCenter);
		}
		this.m_extantBlinkShadow.FlipX = base.sprite.FlipX;
		this.m_extantBlinkShadow.FlipY = base.sprite.FlipY;
	}

	// Token: 0x06008199 RID: 33177 RVA: 0x003499D0 File Offset: 0x00347BD0
	protected void ClearBlinkShadow()
	{
		if (this.m_extantBlinkShadow)
		{
			UnityEngine.Object.Destroy(this.m_extantBlinkShadow.gameObject);
			this.m_extantBlinkShadow = null;
		}
	}

	// Token: 0x0600819A RID: 33178 RVA: 0x003499FC File Offset: 0x00347BFC
	protected bool HandleStartDodgeRoll(Vector2 direction)
	{
		this.m_handleDodgeRollStartThisFrame = true;
		if (this.WasPausedThisFrame)
		{
			return false;
		}
		if (!this.CheckDodgeRollDepth())
		{
			return false;
		}
		if (this.m_dodgeRollState == PlayerController.DodgeRollState.AdditionalDelay)
		{
			return false;
		}
		if (!this.DodgeRollIsBlink && direction == Vector2.zero)
		{
			return false;
		}
		this.rollStats.blinkDistanceMultiplier = 1f;
		if (this.IsFlying && !this.CanDodgeRollWhileFlying)
		{
			return false;
		}
		bool flag = false;
		BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(this.PlayerIDX);
		if (this.DodgeRollIsBlink)
		{
			bool flag2 = false;
			if (instanceForPlayer.GetButtonDown(GungeonActions.GungeonActionType.DodgeRoll))
			{
				flag2 = true;
				base.healthHaver.TriggerInvulnerabilityPeriod(0.001f);
				instanceForPlayer.ConsumeButtonDown(GungeonActions.GungeonActionType.DodgeRoll);
			}
			if (instanceForPlayer.ActiveActions.DodgeRollAction.IsPressed)
			{
				this.m_timeHeldBlinkButton += BraveTime.DeltaTime;
				if (this.m_timeHeldBlinkButton < 0.2f)
				{
					this.m_cachedBlinkPosition = base.specRigidbody.UnitCenter;
				}
				else
				{
					Vector2 cachedBlinkPosition = this.m_cachedBlinkPosition;
					if (this.IsKeyboardAndMouse())
					{
						this.m_cachedBlinkPosition = this.unadjustedAimPoint.XY() - (base.CenterPosition - base.specRigidbody.UnitCenter);
					}
					else
					{
						this.m_cachedBlinkPosition += this.m_activeActions.Aim.Vector.normalized * BraveTime.DeltaTime * 15f;
					}
					this.m_cachedBlinkPosition = BraveMathCollege.ClampToBounds(this.m_cachedBlinkPosition, GameManager.Instance.MainCameraController.MinVisiblePoint, GameManager.Instance.MainCameraController.MaxVisiblePoint);
					this.UpdateBlinkShadow(this.m_cachedBlinkPosition - cachedBlinkPosition, this.CanBlinkToPoint(this.m_cachedBlinkPosition, base.transform.position.XY() - base.specRigidbody.UnitCenter));
				}
			}
			else if (instanceForPlayer.ActiveActions.DodgeRollAction.WasReleased || flag2)
			{
				if (direction != Vector2.zero || this.m_timeHeldBlinkButton >= 0.2f)
				{
					flag = true;
				}
			}
			else
			{
				this.m_timeHeldBlinkButton = 0f;
			}
		}
		else if (instanceForPlayer.GetButtonDown(GungeonActions.GungeonActionType.DodgeRoll))
		{
			instanceForPlayer.ConsumeButtonDown(GungeonActions.GungeonActionType.DodgeRoll);
			flag = true;
		}
		if (flag)
		{
			this.DidUnstealthyAction();
			if (GameManager.Instance.InTutorial)
			{
				GameManager.BroadcastRoomTalkDoerFsmEvent("playerDodgeRoll");
			}
			if (!this.DodgeRollIsBlink)
			{
				return this.StartDodgeRoll(direction);
			}
			if (this.m_timeHeldBlinkButton < 0.2f)
			{
				this.m_cachedBlinkPosition = base.specRigidbody.UnitCenter + direction.normalized * this.rollStats.GetModifiedDistance(this);
			}
			this.BlinkToPoint(this.m_cachedBlinkPosition);
			this.m_timeHeldBlinkButton = 0f;
		}
		return false;
	}

	// Token: 0x0600819B RID: 33179 RVA: 0x00349CFC File Offset: 0x00347EFC
	public void BlinkToPoint(Vector2 targetPoint)
	{
		this.m_cachedBlinkPosition = targetPoint;
		this.lockedDodgeRollDirection = (this.m_cachedBlinkPosition - base.specRigidbody.UnitCenter).normalized;
		Vector2 vector = base.transform.position.XY() - base.specRigidbody.UnitCenter;
		bool flag = this.CanBlinkToPoint(this.m_cachedBlinkPosition, vector);
		if (flag)
		{
			base.StartCoroutine(this.HandleBlinkDodgeRoll());
		}
		else
		{
			Vector2 vector2 = base.specRigidbody.UnitCenter - this.m_cachedBlinkPosition;
			float num = vector2.magnitude;
			Vector2? vector3 = null;
			float num2 = 0f;
			vector2 = vector2.normalized;
			while (num > 0f)
			{
				num2 += 1f;
				num -= 1f;
				Vector2 vector4 = this.m_cachedBlinkPosition + vector2 * num2;
				if (this.CanBlinkToPoint(vector4, vector))
				{
					vector3 = new Vector2?(vector4);
					break;
				}
			}
			if (vector3 != null)
			{
				Vector2 normalized = (vector3.Value - base.specRigidbody.UnitCenter).normalized;
				float num3 = Vector2.Dot(normalized, this.lockedDodgeRollDirection);
				if (num3 > 0f)
				{
					this.m_cachedBlinkPosition = vector3.Value;
					base.StartCoroutine(this.HandleBlinkDodgeRoll());
				}
				else
				{
					this.ClearBlinkShadow();
				}
			}
		}
	}

	// Token: 0x0600819C RID: 33180 RVA: 0x00349E7C File Offset: 0x0034807C
	public void DidUnstealthyAction()
	{
		if (this.OnDidUnstealthyAction != null)
		{
			this.OnDidUnstealthyAction(this);
		}
		if (this.IsPetting && this.m_pettingTarget)
		{
			this.m_pettingTarget.StopPet();
		}
	}

	// Token: 0x0600819D RID: 33181 RVA: 0x00349EBC File Offset: 0x003480BC
	protected void ContinueDodgeRollAnimation()
	{
		Vector2 vector = this.lockedDodgeRollDirection;
		vector.Normalize();
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip;
		if (Mathf.Abs(vector.x) < 0.1f)
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(((vector.y <= 0.1f) ? "dodge" : "dodge_bw") + ((!this.UseArmorlessAnim) ? string.Empty : "_armorless"));
		}
		else
		{
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(((vector.y <= 0.1f) ? "dodge_left" : "dodge_left_bw") + ((!this.UseArmorlessAnim) ? string.Empty : "_armorless"));
		}
		if (tk2dSpriteAnimationClip != null)
		{
			float num = (float)tk2dSpriteAnimationClip.frames.Length / this.rollStats.GetModifiedTime(this);
			base.spriteAnimator.Play(tk2dSpriteAnimationClip, 0f, num, false);
			int num2 = 0;
			for (int i = 0; i < tk2dSpriteAnimationClip.frames.Length; i++)
			{
				if (tk2dSpriteAnimationClip.frames[i].groundedFrame)
				{
					num2 = i;
					break;
				}
			}
			this.m_dodgeRollTimer = (float)num2 / (float)tk2dSpriteAnimationClip.frames.Length * this.rollStats.GetModifiedTime(this);
			base.spriteAnimator.SetFrame(num2);
			this.m_handlingQueuedAnimation = true;
		}
	}

	// Token: 0x0600819E RID: 33182 RVA: 0x0034A028 File Offset: 0x00348228
	protected virtual void PlayDodgeRollAnimation(Vector2 direction)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = null;
		direction.Normalize();
		if (this.m_dodgeRollState != PlayerController.DodgeRollState.PreRollDelay)
		{
			if (Mathf.Abs(direction.x) < 0.1f)
			{
				tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(((direction.y <= 0.1f) ? "dodge" : "dodge_bw") + ((!this.UseArmorlessAnim) ? string.Empty : "_armorless"));
			}
			else
			{
				tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(((direction.y <= 0.1f) ? "dodge_left" : "dodge_left_bw") + ((!this.UseArmorlessAnim) ? string.Empty : "_armorless"));
			}
			if (this.IsVisible)
			{
				Vector2 vector = new Vector2(direction.x, direction.y);
				if (Mathf.Abs(vector.x) < 0.01f)
				{
					vector.x = 0f;
				}
				if (Mathf.Abs(vector.y) < 0.01f)
				{
					vector.y = 0f;
				}
				if (this.CustomDodgeRollEffect != null)
				{
					SpawnManager.SpawnVFX(this.CustomDodgeRollEffect, this.SpriteBottomCenter, Quaternion.identity);
				}
				else
				{
					GameManager.Instance.Dungeon.dungeonDustups.InstantiateDodgeDustup(vector, this.SpriteBottomCenter);
				}
			}
		}
		if (tk2dSpriteAnimationClip != null)
		{
			float num = (float)tk2dSpriteAnimationClip.frames.Length / this.rollStats.GetModifiedTime(this);
			base.spriteAnimator.Play(tk2dSpriteAnimationClip, 0f, num, false);
			this.m_handlingQueuedAnimation = true;
		}
	}

	// Token: 0x0600819F RID: 33183 RVA: 0x0034A1E4 File Offset: 0x003483E4
	public void HandleContinueDodgeRoll()
	{
		this.m_dodgeRollTimer += BraveTime.DeltaTime;
		if (GameManager.Instance.InTutorial && GameManager.Instance.Dungeon.CellIsPit(base.specRigidbody.UnitCenter))
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerDodgeRollOverPit");
		}
		if (this.m_dodgeRollState == PlayerController.DodgeRollState.PreRollDelay)
		{
			if (this.m_dodgeRollTimer > this.rollStats.preDodgeDelay)
			{
				this.m_dodgeRollState = PlayerController.DodgeRollState.InAir;
				this.PlayDodgeRollAnimation(this.lockedDodgeRollDirection);
				this.m_dodgeRollTimer = BraveTime.DeltaTime;
			}
		}
		else if (this.m_dodgeRollState == PlayerController.DodgeRollState.InAir)
		{
			bool flag = false;
			if (this.IsSlidingOverSurface)
			{
				if (this.m_hasFiredWhileSliding)
				{
					this.ToggleGunRenderers(true, "dodgeroll");
				}
				flag = true;
				this.m_dodgeRollTimer -= BraveTime.DeltaTime;
				string text = "slide_right";
				if (this.lockedDodgeRollDirection.y > 0.1f)
				{
					text = "slide_up";
				}
				if (this.lockedDodgeRollDirection.y < -0.1f)
				{
					text = "slide_down";
				}
				if (this.UseArmorlessAnim)
				{
					text += "_armorless";
				}
				if (!base.spriteAnimator.IsPlaying(text) && base.spriteAnimator.GetClipByName(text) != null)
				{
					base.spriteAnimator.Play(text);
				}
				this.IsSlidingOverSurface = false;
				List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody, null, false);
				for (int i = 0; i < overlappingRigidbodies.Count; i++)
				{
					if (base.specRigidbody.Velocity.magnitude < 1f && overlappingRigidbodies[i].GetComponent<MajorBreakable>())
					{
						overlappingRigidbodies[i].GetComponent<MajorBreakable>().Break(Vector2.zero);
					}
					if (overlappingRigidbodies[i].GetComponent<SlideSurface>())
					{
						this.IsSlidingOverSurface = true;
						break;
					}
				}
			}
			if ((!flag || !this.IsSlidingOverSurface) && ((!this.DodgeRollIsBlink && !base.spriteAnimator.CurrentClip.name.Contains("dodge")) || this.QueryGroundedFrame()))
			{
				this.m_dodgeRollState = PlayerController.DodgeRollState.OnGround;
				this.DoVibration(Vibration.Time.Quick, Vibration.Strength.UltraLight);
				if (flag)
				{
					this.m_hasFiredWhileSliding = false;
					this.TablesDamagedThisSlide.Clear();
					this.m_dodgeRollTimer = this.rollStats.GetModifiedTime(this);
					this.ToggleHandRenderers(true, "dodgeroll");
					this.ToggleGunRenderers(true, "dodgeroll");
					this.m_handlingQueuedAnimation = false;
				}
			}
		}
		else if (this.m_dodgeRollState != PlayerController.DodgeRollState.OnGround)
		{
			if (this.m_dodgeRollState == PlayerController.DodgeRollState.Blink)
			{
				float num = this.m_dodgeRollTimer / this.rollStats.GetModifiedTime(this);
				Vector2 vector = base.CenterPosition - base.specRigidbody.UnitCenter;
				if (this.IsPrimaryPlayer)
				{
					GameManager.Instance.MainCameraController.OverridePlayerOnePosition = Vector2.Lerp(base.specRigidbody.UnitCenter, this.m_cachedBlinkPosition, num) + vector;
				}
				else
				{
					GameManager.Instance.MainCameraController.OverridePlayerTwoPosition = Vector2.Lerp(base.specRigidbody.UnitCenter, this.m_cachedBlinkPosition, num) + vector;
				}
			}
		}
	}

	// Token: 0x060081A0 RID: 33184 RVA: 0x0034A554 File Offset: 0x00348754
	private void ToggleOrbitals(bool value)
	{
		for (int i = 0; i < this.orbitals.Count; i++)
		{
			this.orbitals[i].ToggleRenderer(value);
		}
		for (int j = 0; j < this.trailOrbitals.Count; j++)
		{
			this.trailOrbitals[j].ToggleRenderer(value);
		}
	}

	// Token: 0x060081A1 RID: 33185 RVA: 0x0034A5C0 File Offset: 0x003487C0
	public void ToggleFollowerRenderers(bool value)
	{
		this.LastFollowerVisibilityState = value;
		if (this.orbitals != null)
		{
			for (int i = 0; i < this.orbitals.Count; i++)
			{
				this.orbitals[i].ToggleRenderer(value);
			}
		}
		if (this.trailOrbitals != null)
		{
			for (int j = 0; j < this.trailOrbitals.Count; j++)
			{
				this.trailOrbitals[j].ToggleRenderer(value);
			}
		}
		if (this.companions != null)
		{
			for (int k = 0; k < this.companions.Count; k++)
			{
				this.companions[k].ToggleRenderers(value);
			}
		}
	}

	// Token: 0x060081A2 RID: 33186 RVA: 0x0034A680 File Offset: 0x00348880
	public void ToggleRenderer(bool value, string reason = "")
	{
		if (string.IsNullOrEmpty(reason))
		{
			this.m_hideRenderers.ClearOverrides();
			if (!value)
			{
				this.m_hideRenderers.SetOverride("generic", true, null);
			}
		}
		else
		{
			this.m_hideRenderers.RemoveOverride("generic");
			this.m_hideRenderers.SetOverride(reason, !value, null);
		}
		bool flag = !this.m_hideRenderers.Value;
		this.m_renderer.enabled = flag;
		this.ToggleAttachedRenderers(flag);
		if (this.ShadowObject)
		{
			this.ShadowObject.GetComponent<Renderer>().enabled = flag;
		}
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, flag);
	}

	// Token: 0x060081A3 RID: 33187 RVA: 0x0034A740 File Offset: 0x00348940
	private void ToggleAttachedRenderers(bool value)
	{
		for (int i = 0; i < this.m_attachedSprites.Count; i++)
		{
			this.m_attachedSprites[i].renderer.enabled = value;
		}
	}

	// Token: 0x060081A4 RID: 33188 RVA: 0x0034A780 File Offset: 0x00348980
	public void ToggleGunRenderers(bool value, string reason = "")
	{
		if (string.IsNullOrEmpty(reason))
		{
			this.m_hideGunRenderers.ClearOverrides();
			if (!value)
			{
				this.m_hideGunRenderers.SetOverride("generic", true, null);
			}
		}
		else
		{
			this.m_hideGunRenderers.RemoveOverride("generic");
			this.m_hideGunRenderers.SetOverride(reason, !value, null);
		}
		bool flag = !this.m_hideGunRenderers.Value;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES && !ArkController.IsResettingPlayers && value)
		{
			flag = false;
		}
		if (this.CurrentGun != null)
		{
			this.CurrentGun.ToggleRenderers(flag);
		}
		if (this.CurrentSecondaryGun != null)
		{
			this.CurrentSecondaryGun.ToggleRenderers(flag);
		}
	}

	// Token: 0x060081A5 RID: 33189 RVA: 0x0034A85C File Offset: 0x00348A5C
	public void ToggleHandRenderers(bool value, string reason = "")
	{
		if (string.IsNullOrEmpty(reason))
		{
			this.m_hideHandRenderers.ClearOverrides();
			if (!value)
			{
				this.m_hideHandRenderers.SetOverride("generic", true, null);
			}
		}
		else
		{
			this.m_hideHandRenderers.RemoveOverride("generic");
			this.m_hideHandRenderers.SetOverride(reason, !value, null);
		}
		bool flag = !this.m_hideHandRenderers.Value;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES && !ArkController.IsResettingPlayers && value)
		{
			flag = false;
		}
		this.primaryHand.ForceRenderersOff = !flag;
		this.secondaryHand.ForceRenderersOff = !flag;
		if (this.CurrentGun)
		{
			if (this.CurrentGun.additionalHandState == AdditionalHandState.HideBoth)
			{
				this.primaryHand.ForceRenderersOff = true;
				this.secondaryHand.ForceRenderersOff = true;
			}
			else if (this.CurrentGun.additionalHandState == AdditionalHandState.HidePrimary)
			{
				this.primaryHand.ForceRenderersOff = true;
			}
			else if (this.CurrentGun.additionalHandState == AdditionalHandState.HideSecondary)
			{
				this.secondaryHand.ForceRenderersOff = true;
			}
		}
	}

	// Token: 0x060081A6 RID: 33190 RVA: 0x0034A99C File Offset: 0x00348B9C
	protected virtual void HandleFlipping(float gunAngle)
	{
		bool flag = false;
		if (this.CurrentGun == null)
		{
			gunAngle = BraveMathCollege.Atan2Degrees((!(this.m_playerCommandedDirection == Vector2.zero)) ? this.m_playerCommandedDirection : this.m_lastNonzeroCommandedDirection);
		}
		if (this.IsGhost)
		{
			gunAngle = 0f;
		}
		if (!this.IsSlidingOverSurface)
		{
			if (this.IsDodgeRolling)
			{
				if (this.lockedDodgeRollDirection.x < -0.1f)
				{
					gunAngle = 180f;
				}
				else if (this.lockedDodgeRollDirection.x > 0.1f)
				{
					gunAngle = 0f;
				}
			}
			else if (this.IsPetting)
			{
				gunAngle = this.m_petDirection;
			}
			else if (this.m_handlingQueuedAnimation && this.m_overrideGunAngle == null)
			{
				return;
			}
		}
		float num = 75f;
		float num2 = 105f;
		if (gunAngle <= 155f && gunAngle >= 25f)
		{
			num = 75f;
			num2 = 105f;
		}
		if (!this.SpriteFlipped && Mathf.Abs(gunAngle) > num2)
		{
			base.sprite.FlipX = true;
			base.sprite.gameObject.transform.localPosition = new Vector3(this.m_spriteDimensions.x, 0f, 0f);
			if (this.CurrentGun != null)
			{
				this.CurrentGun.HandleSpriteFlip(true);
			}
			if (this.CurrentSecondaryGun != null)
			{
				this.CurrentSecondaryGun.HandleSpriteFlip(true);
			}
			flag = true;
		}
		else if (this.SpriteFlipped && Mathf.Abs(gunAngle) < num)
		{
			base.sprite.FlipX = false;
			base.sprite.gameObject.transform.localPosition = Vector3.zero;
			if (this.CurrentGun != null)
			{
				this.CurrentGun.HandleSpriteFlip(false);
			}
			if (this.CurrentSecondaryGun != null)
			{
				this.CurrentSecondaryGun.HandleSpriteFlip(false);
			}
			flag = true;
		}
		if (this.CurrentGun != null)
		{
			this.HandleGunDepthInternal(this.CurrentGun, gunAngle, false);
		}
		if (this.CurrentSecondaryGun != null)
		{
			this.HandleGunDepthInternal(this.CurrentSecondaryGun, gunAngle, true);
		}
		base.sprite.UpdateZDepth();
		if (flag)
		{
			this.ProcessHandAttachment();
		}
	}

	// Token: 0x060081A7 RID: 33191 RVA: 0x0034AC1C File Offset: 0x00348E1C
	private void HandleGunDepthInternal(Gun targetGun, float gunAngle, bool isSecondary = false)
	{
		tk2dBaseSprite sprite = targetGun.GetSprite();
		if (targetGun.preventRotation)
		{
			sprite.HeightOffGround = 0.4f;
		}
		else if (targetGun.usesDirectionalIdleAnimations)
		{
			float num = -0.075f;
			if ((gunAngle > 0f && gunAngle <= 155f && gunAngle >= 25f) || (gunAngle <= -60f && gunAngle >= -120f))
			{
				num = 0.075f;
			}
			sprite.HeightOffGround = num;
		}
		else if (gunAngle > 0f && gunAngle <= 155f && gunAngle >= 25f)
		{
			sprite.HeightOffGround = -0.075f;
		}
		else
		{
			float num2 = ((targetGun.Handedness != GunHandedness.TwoHanded) ? (-0.075f) : 0.075f);
			if (isSecondary)
			{
				num2 = 0.075f;
			}
			sprite.HeightOffGround = num2;
		}
		sprite.UpdateZDepth();
	}

	// Token: 0x060081A8 RID: 33192 RVA: 0x0034AD0C File Offset: 0x00348F0C
	private float GetDodgeRollSpeed()
	{
		if (this.m_dodgeRollState == PlayerController.DodgeRollState.PreRollDelay)
		{
			return 0f;
		}
		float num = Mathf.Clamp01((this.m_dodgeRollTimer - BraveTime.DeltaTime) / this.rollStats.GetModifiedTime(this));
		float num2 = Mathf.Clamp01(this.m_dodgeRollTimer / this.rollStats.GetModifiedTime(this));
		float num3 = (Mathf.Clamp01(this.rollStats.speed.Evaluate(num2)) - Mathf.Clamp01(this.rollStats.speed.Evaluate(num))) * this.rollStats.GetModifiedDistance(this);
		return num3 / BraveTime.DeltaTime;
	}

	// Token: 0x060081A9 RID: 33193 RVA: 0x0034ADA4 File Offset: 0x00348FA4
	public void ProcessHandAttachment()
	{
		if (this.CurrentGun == null)
		{
			this.primaryHand.attachPoint = null;
			this.secondaryHand.attachPoint = null;
			return;
		}
		if (this.inventory.DualWielding && this.CurrentSecondaryGun != null)
		{
			this.primaryHand.attachPoint = ((!this.CurrentGun.IsPreppedForThrow) ? this.CurrentGun.PrimaryHandAttachPoint : this.CurrentGun.ThrowPrepTransform);
			this.secondaryHand.attachPoint = ((!this.CurrentSecondaryGun.IsPreppedForThrow) ? this.CurrentSecondaryGun.PrimaryHandAttachPoint : this.CurrentSecondaryGun.ThrowPrepTransform);
		}
		else if (this.CurrentGun.Handedness == GunHandedness.NoHanded)
		{
			this.primaryHand.attachPoint = null;
			this.secondaryHand.attachPoint = null;
		}
		else
		{
			if (this.CurrentGun.Handedness != GunHandedness.HiddenOneHanded)
			{
				this.primaryHand.attachPoint = ((!this.CurrentGun.IsPreppedForThrow) ? this.CurrentGun.PrimaryHandAttachPoint : this.CurrentGun.ThrowPrepTransform);
			}
			else
			{
				this.primaryHand.attachPoint = null;
			}
			if (this.CurrentGun.Handedness == GunHandedness.TwoHanded)
			{
				this.secondaryHand.attachPoint = this.CurrentGun.SecondaryHandAttachPoint;
			}
			else
			{
				this.secondaryHand.attachPoint = null;
			}
		}
		if (this.CurrentGun.additionalHandState != AdditionalHandState.None)
		{
			AdditionalHandState additionalHandState = this.CurrentGun.additionalHandState;
			if (additionalHandState != AdditionalHandState.HidePrimary)
			{
				if (additionalHandState != AdditionalHandState.HideSecondary)
				{
					if (additionalHandState == AdditionalHandState.HideBoth)
					{
						if (this.primaryHand)
						{
							this.primaryHand.attachPoint = null;
						}
						if (this.secondaryHand)
						{
							this.secondaryHand.attachPoint = null;
						}
					}
				}
				else if (this.secondaryHand)
				{
					this.secondaryHand.attachPoint = null;
				}
			}
			else if (this.primaryHand)
			{
				this.primaryHand.attachPoint = null;
			}
		}
	}

	// Token: 0x060081AA RID: 33194 RVA: 0x0034AFE0 File Offset: 0x003491E0
	private void HandleGunUnequipInternal(Gun previous)
	{
		if (previous != null)
		{
			tk2dBaseSprite sprite = previous.GetSprite();
			base.sprite.DetachRenderer(sprite);
			sprite.DetachRenderer(this.primaryHand.sprite);
			sprite.DetachRenderer(this.secondaryHand.sprite);
			SpriteOutlineManager.RemoveOutlineFromSprite(previous.GetComponent<tk2dSprite>(), false);
		}
	}

	// Token: 0x060081AB RID: 33195 RVA: 0x0034B03C File Offset: 0x0034923C
	private void HandleGunEquipInternal(Gun current, PlayerHandController hand)
	{
		if (current != null)
		{
			tk2dBaseSprite sprite = current.GetSprite();
			base.sprite.AttachRenderer(sprite);
			sprite.AttachRenderer(hand.sprite);
			if (!this.inventory.DualWielding && (!this.RenderBodyHand || current.IsTrickGun))
			{
				sprite.AttachRenderer(this.secondaryHand.sprite);
			}
			if (!current.PreventOutlines)
			{
				SpriteOutlineManager.AddOutlineToSprite(current.GetComponent<tk2dSprite>(), this.outlineColor, 0.2f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
			}
			current.ToggleRenderers(!this.m_hideGunRenderers.Value);
		}
	}

	// Token: 0x060081AC RID: 33196 RVA: 0x0034B0E8 File Offset: 0x003492E8
	private void OnGunChanged(Gun previous, Gun current, Gun previousSecondary, Gun currentSecondary, bool newGun)
	{
		this.HandleGunUnequipInternal(previous);
		this.HandleGunUnequipInternal(previousSecondary);
		this.HandleGunEquipInternal(current, this.primaryHand);
		this.HandleGunEquipInternal(currentSecondary, this.secondaryHand);
		this.HandleGunAttachPoint();
		this.ProcessHandAttachment();
		this.stats.RecalculateStats(this, false, false);
		if (current && current.ammo > current.AdjustedMaxAmmo)
		{
			ArtfulDodgerGunController component = current.GetComponent<ArtfulDodgerGunController>();
			if (!component)
			{
				current.ammo = current.AdjustedMaxAmmo;
			}
		}
		if (this.GunChanged != null)
		{
			this.GunChanged(previous, current, newGun);
		}
	}

	// Token: 0x060081AD RID: 33197 RVA: 0x0034B18C File Offset: 0x0034938C
	protected Vector2 AdjustInputVector(Vector2 rawInput, float cardinalMagnetAngle, float ordinalMagnetAngle)
	{
		float num = BraveMathCollege.ClampAngle360(BraveMathCollege.Atan2Degrees(rawInput));
		float num2 = num % 90f;
		float num3 = (num + 45f) % 90f;
		float num4 = 0f;
		if (cardinalMagnetAngle > 0f)
		{
			if (num2 < cardinalMagnetAngle)
			{
				num4 = -num2;
			}
			else if (num2 > 90f - cardinalMagnetAngle)
			{
				num4 = 90f - num2;
			}
		}
		if (ordinalMagnetAngle > 0f)
		{
			if (num3 < ordinalMagnetAngle)
			{
				num4 = -num3;
			}
			else if (num3 > 90f - ordinalMagnetAngle)
			{
				num4 = 90f - num3;
			}
		}
		num += num4;
		return (Quaternion.Euler(0f, 0f, num) * Vector3.right).XY() * rawInput.magnitude;
	}

	// Token: 0x060081AE RID: 33198 RVA: 0x0034B250 File Offset: 0x00349450
	protected void ProcessDebugInput()
	{
	}

	// Token: 0x060081AF RID: 33199 RVA: 0x0034B254 File Offset: 0x00349454
	public void ForceMoveToPoint(Vector2 targetPosition, float initialDelay = 0f, float maximumTime = 2f)
	{
		base.StartCoroutine(this.HandleForcedMove(targetPosition, false, false, initialDelay, maximumTime, null));
	}

	// Token: 0x060081B0 RID: 33200 RVA: 0x0034B26C File Offset: 0x0034946C
	public void ForceMoveInDirectionUntilThreshold(Vector2 direction, float axialThreshold, float initialDelay = 0f, float maximumTime = 1f, List<SpeculativeRigidbody> passThroughRigidbodies = null)
	{
		Vector2 centerPosition = base.CenterPosition;
		bool flag = false;
		bool flag2 = false;
		if (!Mathf.Approximately(direction.x, 0f))
		{
			centerPosition.x = axialThreshold;
			flag = true;
		}
		if (!Mathf.Approximately(direction.y, 0f))
		{
			centerPosition.y = axialThreshold;
			flag2 = true;
		}
		base.StartCoroutine(this.HandleForcedMove(centerPosition, flag, flag2, initialDelay, maximumTime, passThroughRigidbodies));
	}

	// Token: 0x060081B1 RID: 33201 RVA: 0x0034B2DC File Offset: 0x003494DC
	private IEnumerator HandleForcedMove(Vector2 targetPoint, bool axialX, bool axialY, float initialDelay = 0f, float maximumTime = 1f, List<SpeculativeRigidbody> passThroughRigidbodies = null)
	{
		this.usingForcedInput = true;
		if (initialDelay > 0f)
		{
			while (initialDelay > 0f)
			{
				this.forcedInput = Vector2.zero;
				initialDelay -= GameManager.INVARIANT_DELTA_TIME;
				yield return null;
			}
		}
		if (passThroughRigidbodies != null)
		{
			for (int i = 0; i < passThroughRigidbodies.Count; i++)
			{
				base.specRigidbody.RegisterSpecificCollisionException(passThroughRigidbodies[i]);
			}
		}
		Vector2 startPosition = base.CenterPosition;
		float elapsed = 0f;
		while (this.usingForcedInput)
		{
			Vector2 dirVec = targetPoint - base.CenterPosition;
			if (axialX != axialY)
			{
				if (axialX)
				{
					dirVec = targetPoint - base.CenterPosition.WithY(targetPoint.y);
					if (Vector2.Dot(dirVec, targetPoint - startPosition.WithY(targetPoint.y)) < 0f)
					{
						break;
					}
				}
				else
				{
					dirVec = targetPoint - base.CenterPosition.WithX(targetPoint.x);
					if (Vector2.Dot(dirVec, targetPoint - startPosition.WithX(targetPoint.x)) < 0f)
					{
						break;
					}
				}
			}
			else if (Vector2.Dot(dirVec, targetPoint - startPosition) < 0f)
			{
				break;
			}
			this.forcedInput = dirVec.normalized;
			float clampedDeltaTime = Mathf.Clamp((Time.timeScale > 0f) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME, 0f, 0.1f);
			elapsed += clampedDeltaTime;
			if (elapsed > maximumTime)
			{
				break;
			}
			yield return null;
		}
		if (passThroughRigidbodies != null)
		{
			for (int j = 0; j < passThroughRigidbodies.Count; j++)
			{
				base.specRigidbody.DeregisterSpecificCollisionException(passThroughRigidbodies[j]);
			}
		}
		this.usingForcedInput = false;
		this.forcedInput = Vector2.zero;
		yield break;
	}

	// Token: 0x060081B2 RID: 33202 RVA: 0x0034B324 File Offset: 0x00349524
	public bool IsQuickEquipGun(Gun gunToCheck)
	{
		return gunToCheck == this.m_cachedQuickEquipGun || gunToCheck == this.CurrentGun;
	}

	// Token: 0x060081B3 RID: 33203 RVA: 0x0034B348 File Offset: 0x00349548
	public void DoQuickEquip()
	{
		if (GameManager.Options.QuickSelectEnabled)
		{
			if (this.m_cachedQuickEquipGun != null && this.inventory.AllGuns.Contains(this.m_cachedQuickEquipGun) && this.CurrentGun != this.m_cachedQuickEquipGun)
			{
				Gun cachedQuickEquipGun = this.m_cachedQuickEquipGun;
				this.CacheQuickEquipGun();
				int num = this.inventory.AllGuns.IndexOf(cachedQuickEquipGun);
				int num2 = num - this.inventory.AllGuns.IndexOf(this.CurrentGun);
				this.ChangeGun(num2, false, false);
				this.m_equippedGunShift = -1;
			}
			else if (this.CurrentGun == this.m_cachedQuickEquipGun && this.m_backupCachedQuickEquipGun != null && this.inventory.AllGuns.Contains(this.m_backupCachedQuickEquipGun) && this.CurrentGun != this.m_backupCachedQuickEquipGun)
			{
				Gun backupCachedQuickEquipGun = this.m_backupCachedQuickEquipGun;
				this.CacheQuickEquipGun();
				int num3 = this.inventory.AllGuns.IndexOf(backupCachedQuickEquipGun);
				int num4 = num3 - this.inventory.AllGuns.IndexOf(this.CurrentGun);
				this.ChangeGun(num4, false, false);
				this.m_equippedGunShift = -1;
			}
			else
			{
				this.ChangeGun(-1, false, false);
			}
		}
		else
		{
			this.ChangeGun(-1, false, false);
		}
	}

	// Token: 0x060081B4 RID: 33204 RVA: 0x0034B4B4 File Offset: 0x003496B4
	protected virtual Vector2 HandlePlayerInput()
	{
		this.exceptionTracker = 0;
		if (this.m_activeActions == null)
		{
			return Vector2.zero;
		}
		Vector2 vector = Vector2.zero;
		if (this.CurrentInputState != PlayerInputState.NoMovement)
		{
			vector = this.AdjustInputVector(this.m_activeActions.Move.Vector, BraveInput.MagnetAngles.movementCardinal, BraveInput.MagnetAngles.movementOrdinal);
		}
		if (vector.magnitude > 1f)
		{
			vector.Normalize();
		}
		this.HandleStartDodgeRoll(vector);
		CollisionData collisionData = null;
		if (vector.x > 0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Right, out collisionData, true, false, null, false))
		{
			vector.x = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (vector.x < -0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Left, out collisionData, true, false, null, false))
		{
			vector.x = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (vector.y > 0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Up, out collisionData, true, false, null, false))
		{
			vector.y = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (vector.y < -0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Down, out collisionData, true, false, null, false))
		{
			vector.y = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (this.IsGhost)
		{
			GameOptions.ControllerBlankControl controllerBlankControl = ((!this.IsPrimaryPlayer) ? GameManager.Options.additionalBlankControlTwo : GameManager.Options.additionalBlankControl);
			bool flag = controllerBlankControl == GameOptions.ControllerBlankControl.BOTH_STICKS_DOWN && this.m_activeActions.CheckBothSticksButton();
			if (Time.timeScale > 0f)
			{
				bool flag2 = false;
				if (this.m_activeActions.Device != null)
				{
					flag2 |= this.m_activeActions.Device.Action1.WasPressed || this.m_activeActions.Device.Action2.WasPressed || this.m_activeActions.Device.Action3.WasPressed || this.m_activeActions.Device.Action4.WasPressed || this.m_activeActions.MenuSelectAction.WasPressed;
				}
				if (this.IsKeyboardAndMouse() && Input.GetMouseButtonDown(0))
				{
					flag2 = true;
				}
				if (this.m_blankCooldownTimer <= 0f && (flag2 || this.m_activeActions.ShootAction.WasPressed || this.m_activeActions.UseItemAction.WasPressed || this.m_activeActions.BlankAction.WasPressed || flag))
				{
					this.DoGhostBlank();
				}
			}
			return vector;
		}
		BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(this.PlayerIDX);
		if (this.AcceptingNonMotionInput)
		{
			bool flag3 = this.IsKeyboardAndMouse() && !GameManager.Options.DisableQuickGunKeys;
			if (flag3)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					this.ChangeToGunSlot(0, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					this.ChangeToGunSlot(1, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					this.ChangeToGunSlot(2, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					this.ChangeToGunSlot(3, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					this.ChangeToGunSlot(4, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					this.ChangeToGunSlot(5, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					this.ChangeToGunSlot(6, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					this.ChangeToGunSlot(7, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha9))
				{
					this.ChangeToGunSlot(8, false);
				}
				if (Input.GetKeyDown(KeyCode.Alpha0))
				{
					this.ChangeToGunSlot(9, false);
				}
			}
			this.m_equippedGunShift = 0;
			if (!this.m_gunWasDropped && !GameUIRoot.Instance.MetalGearActive && !Minimap.Instance.IsFullscreen)
			{
				if (this.m_activeActions.GunDownAction.WasReleased)
				{
					if (!this.m_gunChangePressedWhileMetalGeared)
					{
						this.ChangeGun(1, false, false);
					}
					this.m_gunChangePressedWhileMetalGeared = false;
				}
				if (this.m_activeActions.GunUpAction.WasReleased)
				{
					if (!this.m_gunChangePressedWhileMetalGeared)
					{
						this.ChangeGun(-1, false, false);
					}
					this.m_gunChangePressedWhileMetalGeared = false;
				}
				if (this.inventory.DualWielding && this.m_activeActions.SwapDualGunsAction.WasPressed)
				{
					this.inventory.SwapDualGuns();
				}
				if (this.m_activeActions.GunQuickEquipAction.WasReleased)
				{
					this.DoQuickEquip();
				}
			}
			if ((this.m_activeActions.GunQuickEquipAction.IsPressed || this.ForceMetalGearMenu) && !GameManager.IsBossIntro && !Minimap.Instance.IsFullscreen && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES)
			{
				this.m_metalGearFrames++;
				this.m_metalGearTimer += GameManager.INVARIANT_DELTA_TIME;
				float num = 0.175f;
				if (this.m_metalGearTimer > num && !this.m_metalWasGeared)
				{
					this.m_metalWasGeared = true;
					this.m_metalGearTimer = 0f;
					this.m_metalGearFrames = 0;
					GameUIRoot.Instance.TriggerMetalGearGunSelect(this);
				}
			}
			else
			{
				this.m_metalWasGeared = false;
				this.m_metalGearTimer = 0f;
				this.m_metalGearFrames = 0;
			}
			if (this.m_activeActions.DropGunAction.IsPressed && this.CurrentGun != null && this.inventory.AllGuns.Count > 1 && !this.inventory.GunLocked.Value && this.CurrentGun.CanActuallyBeDropped(this) && !this.m_gunWasDropped)
			{
				this.m_dropGunTimer += BraveTime.DeltaTime;
				if (this.m_dropGunTimer > 0.5f)
				{
					this.m_gunWasDropped = true;
					this.m_dropGunTimer = 0f;
					this.ForceDropGun(this.CurrentGun);
					this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
				}
			}
			else if (!this.m_activeActions.DropGunAction.IsPressed)
			{
				this.m_gunWasDropped = false;
				this.m_dropGunTimer = 0f;
			}
			if (!this.m_itemWasDropped)
			{
				bool wasReleased = this.m_activeActions.ItemUpAction.WasReleased;
				if (wasReleased)
				{
					this.ChangeItem(1);
				}
				else if (this.m_activeActions.ItemDownAction.WasReleased)
				{
					this.ChangeItem(-1);
				}
			}
			if (this.m_activeActions.DropItemAction.IsPressed && this.CurrentItem != null && this.CurrentItem.CanActuallyBeDropped(this) && !this.m_itemWasDropped && !this.m_preventItemSwitching)
			{
				this.m_dropItemTimer += BraveTime.DeltaTime;
				if (this.m_dropItemTimer > 0.5f)
				{
					this.m_itemWasDropped = true;
					this.m_dropItemTimer = 0f;
					this.DropActiveItem(this.CurrentItem, 4f, false);
					this.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
				}
			}
			else if (!this.m_activeActions.DropItemAction.IsPressed)
			{
				this.m_itemWasDropped = false;
				this.m_dropItemTimer = 0f;
			}
			bool wasPressed = this.m_activeActions.ReloadAction.WasPressed;
			if (wasPressed && this.CurrentGun != null)
			{
				this.CurrentGun.Reload();
				if (this.CurrentGun.OnReloadPressed != null)
				{
					this.CurrentGun.OnReloadPressed(this, this.CurrentGun, true);
				}
				if (this.CurrentSecondaryGun)
				{
					this.CurrentSecondaryGun.Reload();
					if (this.CurrentSecondaryGun.OnReloadPressed != null)
					{
						this.CurrentSecondaryGun.OnReloadPressed(this, this.CurrentSecondaryGun, true);
					}
				}
				if (this.OnReloadPressed != null)
				{
					this.OnReloadPressed(this, this.CurrentGun);
				}
			}
			bool buttonDown = instanceForPlayer.GetButtonDown(GungeonActions.GungeonActionType.UseItem);
			bool flag4 = true;
			if (buttonDown && (!this.IsDodgeRolling || (this.CurrentItem && this.CurrentItem.usableDuringDodgeRoll)))
			{
				this.UseItem();
				if (flag4)
				{
					instanceForPlayer.ConsumeButtonDown(GungeonActions.GungeonActionType.UseItem);
				}
			}
			GameOptions.ControllerBlankControl controllerBlankControl2 = ((!this.IsPrimaryPlayer) ? GameManager.Options.additionalBlankControlTwo : GameManager.Options.additionalBlankControl);
			bool flag5 = controllerBlankControl2 == GameOptions.ControllerBlankControl.BOTH_STICKS_DOWN && this.m_activeActions.CheckBothSticksButton();
			if (Time.timeScale > 0f && this.m_blankCooldownTimer <= 0f && (this.m_activeActions.BlankAction.WasPressed || flag5))
			{
				this.DoConsumableBlank();
			}
			if (Minimap.Instance != null && !GameUIRoot.Instance.MetalGearActive)
			{
				bool wasPressed2 = this.m_activeActions.MapAction.WasPressed;
				bool flag6 = false;
				if (wasPressed2)
				{
					Minimap.Instance.ToggleMinimap(true, flag6);
				}
			}
		}
		if (this.CurrentInputState == PlayerInputState.AllInput || this.CurrentInputState == PlayerInputState.FoyerInputOnly)
		{
			IPlayerInteractable playerInteractable = null;
			if (this.m_currentRoom != null)
			{
				playerInteractable = this.m_currentRoom.GetNearestInteractable(base.CenterPosition, 1f, this);
			}
			if (playerInteractable != this.m_lastInteractionTarget || this.ForceRefreshInteractable)
			{
				this.exceptionTracker = 100;
				if (this.m_lastInteractionTarget is MonoBehaviour && !(this.m_lastInteractionTarget as MonoBehaviour))
				{
					this.m_lastInteractionTarget = null;
				}
				this.exceptionTracker = 101;
				if (this.m_lastInteractionTarget != null)
				{
					this.m_lastInteractionTarget.OnExitRange(this);
					this.exceptionTracker = 102;
					if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && this.m_lastInteractionTarget != null)
					{
						for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
						{
							this.exceptionTracker = 103;
							PlayerController playerController = GameManager.Instance.AllPlayers[i];
							if (playerController && !(playerController == this))
							{
								if (!playerController.healthHaver.IsDead)
								{
									if (playerController.CurrentRoom != null)
									{
										this.exceptionTracker = 104;
										if (this.m_lastInteractionTarget == playerController.CurrentRoom.GetNearestInteractable(playerController.CenterPosition, 1f, playerController))
										{
											this.m_lastInteractionTarget.OnEnteredRange(playerController);
										}
										this.exceptionTracker = 105;
									}
								}
							}
						}
					}
				}
				if (playerInteractable != null)
				{
					playerInteractable.OnEnteredRange(this);
				}
				this.m_lastInteractionTarget = playerInteractable;
			}
			if (playerInteractable != null && this.m_activeActions.InteractAction.WasPressed)
			{
				if (this.IsDodgeRolling)
				{
					this.ToggleGunRenderers(true, "dodgeroll");
					this.ToggleHandRenderers(true, "dodgeroll");
				}
				GameUIRoot.Instance.levelNameUI.BanishLevelNameText();
				bool flag7;
				string text = playerInteractable.GetAnimationState(this, out flag7);
				playerInteractable.Interact(this);
				if (this.IsSlidingOverSurface)
				{
					text = string.Empty;
				}
				if (!(playerInteractable is ShopItemController))
				{
					this.DidUnstealthyAction();
				}
				if (text != string.Empty)
				{
					this.HandleFlipping((float)((!flag7) ? 0 : 180));
					this.m_handlingQueuedAnimation = true;
					string text2 = ((!(this.CurrentGun == null) || this.ForceHandless) ? "_hand" : "_twohands");
					string text3 = ((!this.UseArmorlessAnim) ? string.Empty : "_armorless");
					if (this.RenderBodyHand && base.spriteAnimator.GetClipByName(text + text2 + text3) != null)
					{
						base.spriteAnimator.Play(text + text2 + text3);
					}
					else if (base.spriteAnimator.GetClipByName(text + text3) != null)
					{
						base.spriteAnimator.Play(text + text3);
					}
					this.m_overrideGunAngle = new float?((float)((!flag7) ? 0 : 180));
				}
			}
			else if (playerInteractable == null && this.m_activeActions.InteractAction.WasPressed && !this.IsPetting && !this.IsInCombat && !this.IsDodgeRolling && !this.m_handlingQueuedAnimation)
			{
				List<AIActor> allEnemies = StaticReferenceManager.AllEnemies;
				for (int j = 0; j < allEnemies.Count; j++)
				{
					AIActor aiactor = allEnemies[j];
					if (aiactor && !aiactor.IsNormalEnemy && aiactor.CompanionOwner)
					{
						CompanionController component = aiactor.GetComponent<CompanionController>();
						if (component.CanBePet && Vector2.Distance(base.CenterPosition, component.specRigidbody.GetUnitCenter(ColliderType.HitBox)) <= 2.5f)
						{
							component.DoPet(this);
							base.spriteAnimator.Play("pet");
							this.ToggleGunRenderers(false, "petting");
							this.ToggleHandRenderers(false, "petting");
							this.m_petDirection = (float)((aiactor.specRigidbody.UnitCenter.x <= base.specRigidbody.UnitCenter.x) ? 180 : 0);
							this.m_pettingTarget = component;
							break;
						}
					}
				}
			}
		}
		this.ForceRefreshInteractable = false;
		if (this.AcceptingNonMotionInput || this.CurrentInputState == PlayerInputState.FoyerInputOnly)
		{
			Vector2 vector2 = this.DetermineAimPointInWorld();
			if (this.CurrentGun != null)
			{
				this.m_currentGunAngle = this.CurrentGun.HandleAimRotation(vector2, false, 1f);
				if (this.CurrentSecondaryGun)
				{
					this.CurrentSecondaryGun.HandleAimRotation(vector2, false, 1f);
				}
			}
			if (this.m_overrideGunAngle != null)
			{
				this.m_currentGunAngle = this.m_overrideGunAngle.Value;
				this.gunAttachPoint.localRotation = Quaternion.Euler(this.gunAttachPoint.localRotation.x, this.gunAttachPoint.localRotation.y, this.m_currentGunAngle);
			}
			else
			{
				this.m_currentGunAngle = (vector2 - base.specRigidbody.GetUnitCenter(ColliderType.HitBox)).ToAngle();
			}
		}
		if (this.AcceptingNonMotionInput)
		{
			base.sprite.UpdateZDepth();
			if (this.inventory.DualWielding && this.CurrentSecondaryGun)
			{
				this.HandleGunFiringInternal(this.CurrentSecondaryGun, instanceForPlayer, true);
			}
			this.HandleGunFiringInternal(this.CurrentGun, instanceForPlayer, false);
		}
		else if (this.CurrentInputState == PlayerInputState.OnlyMovement && this.CurrentGun != null && this.CurrentGun.IsCharging && instanceForPlayer.GetButton(GungeonActions.GungeonActionType.Shoot) && this.m_shouldContinueFiring)
		{
			this.CurrentGun.ContinueAttack(this.m_CanAttack, null);
		}
		return vector;
	}

	// Token: 0x060081B5 RID: 33205 RVA: 0x0034C4B4 File Offset: 0x0034A6B4
	private void HandleGunFiringInternal(Gun targetGun, BraveInput currentBraveInput, bool isSecondary)
	{
		if (targetGun != null)
		{
			bool flag = currentBraveInput.GetButtonDown(GungeonActions.GungeonActionType.Shoot) || this.forceFireDown;
			if (this.OnTriedToInitiateAttack != null && flag)
			{
				this.OnTriedToInitiateAttack(this);
			}
			if (this.SuppressThisClick)
			{
				this.exceptionTracker = 200;
				while (currentBraveInput.GetButtonDown(GungeonActions.GungeonActionType.Shoot))
				{
					currentBraveInput.ConsumeButtonDown(GungeonActions.GungeonActionType.Shoot);
					if (currentBraveInput.GetButtonUp(GungeonActions.GungeonActionType.Shoot))
					{
						currentBraveInput.ConsumeButtonUp(GungeonActions.GungeonActionType.Shoot);
					}
				}
				this.exceptionTracker = 201;
				if (!currentBraveInput.GetButton(GungeonActions.GungeonActionType.Shoot))
				{
					this.SuppressThisClick = false;
				}
			}
			else if (this.m_CanAttack && flag)
			{
				this.exceptionTracker = 202;
				bool flag2 = false;
				Gun.AttackResult attackResult = targetGun.Attack(null, null);
				flag2 |= attackResult != Gun.AttackResult.Fail && attackResult != Gun.AttackResult.OnCooldown;
				this.m_newFloorNoInput = false;
				this.exceptionTracker = 203;
				if (!this.HasFiredNonStartingGun && attackResult == Gun.AttackResult.Success && !targetGun.StarterGunForAchievement)
				{
					this.HasFiredNonStartingGun = true;
				}
				this.m_shouldContinueFiring = true;
				this.IsFiring = attackResult == Gun.AttackResult.Success && !targetGun.IsCharging;
				this.exceptionTracker = 204;
				if (attackResult == Gun.AttackResult.Success)
				{
					this.DidUnstealthyAction();
				}
				if (flag2 && !isSecondary)
				{
					currentBraveInput.ConsumeButtonDown(GungeonActions.GungeonActionType.Shoot);
				}
				this.m_controllerSemiAutoTimer = 0f;
			}
			else if ((currentBraveInput.GetButtonUp(GungeonActions.GungeonActionType.Shoot) || this.forceFireUp) && !this.KeepChargingDuringRoll)
			{
				this.exceptionTracker = 205;
				this.IsFiring = targetGun.CeaseAttack(this.m_CanAttack, null);
				if (!isSecondary)
				{
					currentBraveInput.ConsumeButtonUp(GungeonActions.GungeonActionType.Shoot);
				}
				this.m_shouldContinueFiring = false;
			}
			else if ((currentBraveInput.GetButton(GungeonActions.GungeonActionType.Shoot) || this.forceFire || this.KeepChargingDuringRoll) && this.m_shouldContinueFiring)
			{
				this.exceptionTracker = 206;
				bool flag3 = this.IsDodgeRolling && !this.IsSlidingOverSurface;
				if (this.IsSlidingOverSurface)
				{
					this.m_hasFiredWhileSliding = true;
				}
				if (this.UseFakeSemiAutoCooldown && targetGun.DefaultModule.shootStyle == ProjectileModule.ShootStyle.SemiAutomatic && !targetGun.HasShootStyle(ProjectileModule.ShootStyle.Charged) && !flag3 && targetGun.CurrentAmmo > 0)
				{
					this.m_controllerSemiAutoTimer += BraveTime.DeltaTime;
					if (this.m_controllerSemiAutoTimer > BraveInput.ControllerFakeSemiAutoCooldown && !targetGun.IsEmpty && this.m_CanAttack)
					{
						this.exceptionTracker = 207;
						targetGun.CeaseAttack(false, null);
						if (targetGun.Attack(null, null) == Gun.AttackResult.Success)
						{
							this.m_controllerSemiAutoTimer = 0f;
							this.IsFiring = !targetGun.IsCharging;
						}
					}
					else
					{
						this.exceptionTracker = 208;
						bool flag4 = targetGun.ContinueAttack(this.m_CanAttack, null);
						this.IsFiring = flag4 && !targetGun.IsCharging;
					}
				}
				else
				{
					this.exceptionTracker = 209;
					bool flag5 = targetGun.ContinueAttack(this.m_CanAttack, null);
					this.IsFiring = flag5 && !targetGun.IsCharging;
				}
				this.exceptionTracker = 210;
				if (!targetGun.IsReloading)
				{
					this.DidUnstealthyAction();
				}
			}
			else if (targetGun.IsFiring || targetGun.IsPreppedForThrow)
			{
				this.exceptionTracker = 211;
				this.IsFiring = targetGun.CeaseAttack(this.m_CanAttack, null);
				this.m_shouldContinueFiring = false;
			}
			if (this.IsFiring)
			{
				this.m_isThreatArrowing = false;
				this.m_elapsedNonalertTime = 0f;
			}
		}
	}

	// Token: 0x1700133F RID: 4927
	// (get) Token: 0x060081B6 RID: 33206 RVA: 0x0034C884 File Offset: 0x0034AA84
	private bool KeepChargingDuringRoll
	{
		get
		{
			return this.IsDodgeRolling && this.CurrentGun != null && this.CurrentGun.HasChargedProjectileReady;
		}
	}

	// Token: 0x060081B7 RID: 33207 RVA: 0x0034C8B0 File Offset: 0x0034AAB0
	public void RemoveBrokenInteractable(IPlayerInteractable ixable)
	{
		if (this.m_lastInteractionTarget == ixable)
		{
			this.m_lastInteractionTarget.OnExitRange(this);
			this.m_lastInteractionTarget = null;
		}
	}

	// Token: 0x060081B8 RID: 33208 RVA: 0x0034C8D4 File Offset: 0x0034AAD4
	private void ChangeItem(int change)
	{
		if (!this.m_preventItemSwitching)
		{
			if (this.activeItems.Count > 1)
			{
				this.CurrentItem.OnItemSwitched(this);
				this.m_selectedItemIndex += change;
				int num = (this.m_selectedItemIndex + this.activeItems.Count) % this.activeItems.Count;
				this.m_selectedItemIndex = num;
			}
			else
			{
				this.m_selectedItemIndex = 0;
			}
			if (!EncounterTrackable.SuppressNextNotification)
			{
				GameUIRoot.Instance.TemporarilyShowItemName(this.IsPrimaryPlayer);
			}
		}
	}

	// Token: 0x060081B9 RID: 33209 RVA: 0x0034C964 File Offset: 0x0034AB64
	public void CacheQuickEquipGun()
	{
		this.m_backupCachedQuickEquipGun = this.m_cachedQuickEquipGun;
		this.m_cachedQuickEquipGun = this.CurrentGun;
	}

	// Token: 0x060081BA RID: 33210 RVA: 0x0034C980 File Offset: 0x0034AB80
	public void ChangeToGunSlot(int slotIndex, bool overrideGunLock = false)
	{
		if (this.inventory.AllGuns.Count == 0)
		{
			return;
		}
		if (!this.CurrentGun)
		{
			return;
		}
		if (slotIndex < 0 || slotIndex >= this.inventory.AllGuns.Count)
		{
			return;
		}
		int num = this.inventory.AllGuns.IndexOf(this.CurrentGun);
		int num2 = slotIndex - num;
		this.ChangeGun(num2, true, overrideGunLock);
	}

	// Token: 0x060081BB RID: 33211 RVA: 0x0034C9F8 File Offset: 0x0034ABF8
	public void ChangeGun(int change, bool forceEmptySelect = false, bool overrideGunLock = false)
	{
		if (this.inventory.AllGuns.Count == 0)
		{
			return;
		}
		if (this.inventory.DualWielding && this.inventory.AllGuns.Count <= 2 && this.CurrentSecondaryGun != null)
		{
			return;
		}
		if (change % this.inventory.AllGuns.Count == 0)
		{
			return;
		}
		if (this.IsDodgeRolling)
		{
			this.CurrentGun.ToggleRenderers(true);
		}
		bool flag = GameManager.Options.HideEmptyGuns && this.IsInCombat && !forceEmptySelect;
		bool dualWielding = this.inventory.DualWielding;
		if (flag || dualWielding)
		{
			int num = 0;
			while ((flag && this.inventory.GetTargetGunWithChange(change).CurrentAmmo == 0) || (dualWielding && this.inventory.GetTargetGunWithChange(change) == this.CurrentSecondaryGun))
			{
				num++;
				change += Math.Sign(change);
				if (num >= this.inventory.AllGuns.Count)
				{
					break;
				}
			}
			if (this.inventory.GetTargetGunWithChange(change) == this.CurrentSecondaryGun)
			{
				change += Math.Sign(change);
			}
		}
		GameUIRoot.Instance.ForceClearReload(this.PlayerIDX);
		GunInventory gunInventory = this.inventory;
		int num2 = change;
		gunInventory.ChangeGun(num2, false, overrideGunLock);
		if (this.IsDodgeRolling)
		{
			this.CurrentGun.ToggleRenderers(false);
		}
		this.m_equippedGunShift = change;
	}

	// Token: 0x060081BC RID: 33212 RVA: 0x0034CB94 File Offset: 0x0034AD94
	public void ClearPerLevelData()
	{
		this.m_currentRoom = null;
		this.m_lastInteractionTarget = null;
		this.stats.ToNextLevel();
		this.m_bellygeonDepressedTiles.Clear();
		PlayerController.m_bellygeonDepressedTileTimers.Clear();
		for (int i = 0; i < this.additionalItems.Count; i++)
		{
			UnityEngine.Object.Destroy(this.additionalItems[i].gameObject);
		}
		this.additionalItems.Clear();
	}

	// Token: 0x060081BD RID: 33213 RVA: 0x0034CC0C File Offset: 0x0034AE0C
	public void BraveOnLevelWasLoaded()
	{
		this.m_newFloorNoInput = true;
		this.HasGottenKeyThisRun = false;
		this.LevelToLoadOnPitfall = string.Empty;
		this.m_cachedLevelToLoadOnPitfall = string.Empty;
		this.m_interruptingPitRespawn = false;
		this.m_cachedPosition = Vector2.zero;
		this.m_currentRoom = null;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(this).m_currentRoom = null;
		}
		if (GameManager.Instance.InTutorial)
		{
			base.sprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("ShadowCaster"));
		}
		if (GameUIRoot.Instance != null)
		{
			GameUIRoot.Instance.UpdatePlayerHealthUI(this, base.healthHaver);
			if (this.passiveItems != null && this.passiveItems.Count > 0)
			{
				for (int i = 0; i < this.passiveItems.Count; i++)
				{
					GameUIRoot.Instance.AddPassiveItemToDock(this.passiveItems[i], this);
				}
			}
			this.Blanks = Mathf.Max(this.Blanks, ((GameManager.Instance.CurrentGameType != GameManager.GameType.SINGLE_PLAYER) ? this.stats.NumBlanksPerFloorCoop : this.stats.NumBlanksPerFloor) + Mathf.FloorToInt(this.stats.GetStatValue(PlayerStats.StatType.AdditionalBlanksPerFloor)));
			if (GameManager.Instance.InTutorial)
			{
				this.Blanks = 0;
			}
			GameUIRoot.Instance.UpdatePlayerBlankUI(this);
			this.carriedConsumables.ForceUpdateUI();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameUIRoot.Instance.UpdateGunData(GameManager.Instance.GetOtherPlayer(this).inventory, 0, GameManager.Instance.GetOtherPlayer(this));
				GameUIRoot.Instance.UpdateItemData(GameManager.Instance.GetOtherPlayer(this), GameManager.Instance.GetOtherPlayer(this).CurrentItem, GameManager.Instance.GetOtherPlayer(this).activeItems);
			}
			if (this.IsGhost)
			{
				GameUIRoot.Instance.DisableCoopPlayerUI(this);
				GameUIRoot.Instance.TransitionToGhostUI(this);
			}
			else if (this.CurrentGun != null)
			{
				this.CurrentGun.ForceImmediateReload(false);
			}
			if (this.OnNewFloorLoaded != null)
			{
				this.OnNewFloorLoaded(this);
			}
		}
		if (base.knockbackDoer)
		{
			base.knockbackDoer.ClearContinuousKnockbacks();
		}
		Shader.SetGlobalFloat("_MeduziReflectionsEnabled", 0f);
		if (this.m_usesRandomStartingEquipment && !this.m_randomStartingItemsInitialized && (GameManager.Instance.IsLoadingFirstShortcutFloor || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON))
		{
			this.m_randomStartingItemsInitialized = true;
			for (int j = 0; j < this.startingPassiveItemIds.Count; j++)
			{
				if (!this.HasPassiveItem(this.startingPassiveItemIds[j]))
				{
					this.AcquirePassiveItemPrefabDirectly(PickupObjectDatabase.GetById(this.startingPassiveItemIds[j]) as PassiveItem);
				}
			}
			for (int k = 0; k < this.startingActiveItemIds.Count; k++)
			{
				if (!this.HasActiveItem(this.startingActiveItemIds[k]))
				{
					EncounterTrackable.SuppressNextNotification = true;
					PlayerItem playerItem = PickupObjectDatabase.GetById(this.startingActiveItemIds[k]) as PlayerItem;
					playerItem.Pickup(this);
					EncounterTrackable.SuppressNextNotification = false;
				}
			}
		}
	}

	// Token: 0x060081BE RID: 33214 RVA: 0x0034CF78 File Offset: 0x0034B178
	private void EnteredNewRoom(RoomHandler newRoom)
	{
		this.RealtimeEnteredCurrentRoom = Time.realtimeSinceStartup;
	}

	// Token: 0x060081BF RID: 33215 RVA: 0x0034CF88 File Offset: 0x0034B188
	public void ForceChangeRoom(RoomHandler newRoom)
	{
		RoomHandler currentRoom = this.m_currentRoom;
		this.m_currentRoom = newRoom;
		if (currentRoom != null)
		{
			currentRoom.PlayerExit(this);
			currentRoom.OnBecameInvisible(this);
		}
		this.m_currentRoom.PlayerEnter(this);
		this.EnteredNewRoom(this.m_currentRoom);
		this.m_inExitLastFrame = false;
		GameManager.Instance.MainCameraController.AssignBoundingPolygon(this.m_currentRoom.cameraBoundingPolygon);
	}

	// Token: 0x060081C0 RID: 33216 RVA: 0x0034CFF0 File Offset: 0x0034B1F0
	private void HandleRoomProcessing()
	{
		if (BraveUtility.isLoadingLevel || GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (this.m_roomBeforeExit == null)
		{
			this.m_roomBeforeExit = this.m_currentRoom;
		}
		Dungeon dungeon = GameManager.Instance.Dungeon;
		DungeonData data = dungeon.data;
		CellData cellSafe = data.GetCellSafe(PhysicsEngine.PixelToUnit(base.specRigidbody.PrimaryPixelCollider.LowerLeft).ToIntVector2(VectorConversions.Floor));
		CellData cellSafe2 = data.GetCellSafe(PhysicsEngine.PixelToUnit(base.specRigidbody.PrimaryPixelCollider.LowerRight).ToIntVector2(VectorConversions.Floor));
		CellData cellSafe3 = data.GetCellSafe(PhysicsEngine.PixelToUnit(base.specRigidbody.PrimaryPixelCollider.UpperLeft).ToIntVector2(VectorConversions.Floor));
		CellData cellSafe4 = data.GetCellSafe(PhysicsEngine.PixelToUnit(base.specRigidbody.PrimaryPixelCollider.UpperRight).ToIntVector2(VectorConversions.Floor));
		if (cellSafe == null || cellSafe2 == null || cellSafe3 == null || cellSafe4 == null)
		{
			return;
		}
		RoomHandler roomHandler = null;
		CellData cellData = null;
		if (cellSafe.isExitCell || cellSafe2.isExitCell || cellSafe3.isExitCell || cellSafe4.isExitCell)
		{
			cellData = ((!cellSafe.isExitCell) ? ((!cellSafe2.isExitCell) ? ((!cellSafe3.isExitCell) ? ((!cellSafe4.isExitCell) ? null : cellSafe4) : cellSafe3) : cellSafe2) : cellSafe);
			if (cellData != null)
			{
				roomHandler = ((cellData.connectedRoom1 == this.m_currentRoom) ? cellData.connectedRoom2 : cellData.connectedRoom1);
			}
			this.m_previousExitLinkedRoom = roomHandler;
			this.m_inExitLastFrame = true;
		}
		this.InExitCell = cellData != null;
		this.CurrentExitCell = cellData;
		if (!this.m_inExitLastFrame)
		{
			this.m_roomBeforeExit = this.m_currentRoom;
		}
		if (roomHandler == null)
		{
			roomHandler = ((cellSafe.parentRoom == this.m_currentRoom) ? ((cellSafe2.parentRoom == this.m_currentRoom) ? ((cellSafe3.parentRoom == this.m_currentRoom) ? ((cellSafe4.parentRoom == this.m_currentRoom) ? null : cellSafe4.parentRoom) : cellSafe3.parentRoom) : cellSafe2.parentRoom) : cellSafe.parentRoom);
		}
		if (roomHandler != null)
		{
			if (roomHandler.visibility == RoomHandler.VisibilityStatus.OBSCURED || roomHandler.visibility == RoomHandler.VisibilityStatus.REOBSCURED || roomHandler.IsSealed)
			{
				bool flag = cellSafe.isDoorFrameCell || cellSafe2.isDoorFrameCell || cellSafe3.isDoorFrameCell || cellSafe4.isDoorFrameCell;
				if (cellSafe.parentRoom != this.m_currentRoom && cellSafe2.parentRoom != this.m_currentRoom && cellSafe3.parentRoom != this.m_currentRoom && cellSafe4.parentRoom != this.m_currentRoom && !flag)
				{
					if (this.m_currentRoom != null)
					{
						this.m_currentRoom.PlayerExit(this);
					}
					this.m_currentRoom = roomHandler;
					this.m_currentRoom.PlayerEnter(this);
					this.EnteredNewRoom(this.m_currentRoom);
					this.m_inExitLastFrame = false;
					GameManager.Instance.MainCameraController.AssignBoundingPolygon(this.m_currentRoom.cameraBoundingPolygon);
				}
			}
			else
			{
				if (this.m_currentRoom != null)
				{
					this.m_currentRoom.OnBecameVisible(this);
				}
				if (cellData != null && cellData.exitDoor != null)
				{
					bool flag2 = cellData.exitDoor.IsOpenForVisibilityTest && (cellData.exitDoor.subsidiaryBlocker == null || !cellData.exitDoor.subsidiaryBlocker.isSealed) && (cellData.exitDoor.subsidiaryDoor == null || cellData.exitDoor.subsidiaryDoor.IsOpenForVisibilityTest);
					if (flag2)
					{
						roomHandler.OnBecameVisible(this);
					}
					else
					{
						roomHandler.OnBecameInvisible(this);
					}
				}
				else
				{
					roomHandler.OnBecameVisible(this);
				}
				if (!cellSafe.isExitCell && !cellSafe2.isExitCell && !cellSafe3.isExitCell && !cellSafe4.isExitCell && cellSafe.parentRoom == roomHandler)
				{
					this.m_inExitLastFrame = false;
					if (this.m_currentRoom != null)
					{
						this.m_currentRoom.PlayerExit(this);
					}
					this.m_currentRoom = roomHandler;
					this.m_currentRoom.PlayerEnter(this);
					this.EnteredNewRoom(this.m_currentRoom);
					GameManager.Instance.MainCameraController.AssignBoundingPolygon(this.m_currentRoom.cameraBoundingPolygon);
				}
			}
		}
		else if (this.m_inExitLastFrame)
		{
			this.m_inExitLastFrame = false;
			if (this.m_previousExitLinkedRoom != null && this.m_previousExitLinkedRoom.visibility != RoomHandler.VisibilityStatus.OBSCURED)
			{
				Pixelator.Instance.ProcessRoomAdditionalExits(IntVector2.Zero, this.m_previousExitLinkedRoom, false);
			}
		}
		for (int i = 0; i < data.rooms.Count; i++)
		{
			RoomHandler roomHandler2 = data.rooms[i];
			if (roomHandler2.visibility == RoomHandler.VisibilityStatus.CURRENT && roomHandler2 != this.m_currentRoom && roomHandler2 != roomHandler)
			{
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this);
					if (otherPlayer)
					{
						if (otherPlayer.CurrentRoom == roomHandler2)
						{
							goto IL_5D6;
						}
						if (otherPlayer.InExitCell && otherPlayer.CurrentExitCell != null && otherPlayer.CurrentExitCell.exitDoor && (otherPlayer.CurrentExitCell.exitDoor.upstreamRoom == roomHandler2 || otherPlayer.CurrentExitCell.exitDoor.downstreamRoom == roomHandler2))
						{
							goto IL_5D6;
						}
					}
				}
				roomHandler2.PlayerExit(this);
			}
			IL_5D6:;
		}
		if (this.m_currentRoom != null)
		{
			this.m_currentRoom.PlayerInCell(this, cellSafe.position, base.specRigidbody.PrimaryPixelCollider.UnitBottomLeft);
			this.m_currentRoom.PlayerInCell(this, cellSafe2.position, base.specRigidbody.PrimaryPixelCollider.UnitBottomRight);
			this.m_currentRoom.PlayerInCell(this, cellSafe3.position, base.specRigidbody.PrimaryPixelCollider.UnitTopLeft);
			this.m_currentRoom.PlayerInCell(this, cellSafe4.position, base.specRigidbody.PrimaryPixelCollider.UnitTopRight);
		}
		if (dungeon != null && dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.BELLYGEON)
		{
			IntVector2 intVector = this.SpriteBottomCenter.IntXY(VectorConversions.Floor);
			if (intVector != this.m_cachedLastCenterCellBellygeon)
			{
				this.m_cachedLastCenterCellBellygeon = intVector;
				if (this.m_bellygeonDepressedTiles.Contains(intVector))
				{
					PlayerController.m_bellygeonDepressedTileTimers[intVector] = 1f;
				}
				else
				{
					this.m_bellygeonDepressedTiles.Add(intVector);
					PlayerController.m_bellygeonDepressedTileTimers.Add(intVector, 1f);
				}
				data.TriggerFloorAnimationsInCell(intVector);
			}
			for (int j = 0; j < this.m_bellygeonDepressedTiles.Count; j++)
			{
				if (!(this.m_bellygeonDepressedTiles[j] == intVector))
				{
					PlayerController.m_bellygeonDepressedTileTimers[this.m_bellygeonDepressedTiles[j]] = PlayerController.m_bellygeonDepressedTileTimers[this.m_bellygeonDepressedTiles[j]] - BraveTime.DeltaTime;
					if (PlayerController.m_bellygeonDepressedTileTimers[this.m_bellygeonDepressedTiles[j]] <= 0f)
					{
						data.UntriggerFloorAnimationsInCell(this.m_bellygeonDepressedTiles[j]);
						PlayerController.m_bellygeonDepressedTileTimers.Remove(this.m_bellygeonDepressedTiles[j]);
						this.m_bellygeonDepressedTiles.RemoveAt(j);
						j--;
					}
				}
			}
		}
		this.HandleCurrentRoomExtraData();
	}

	// Token: 0x060081C1 RID: 33217 RVA: 0x0034D7F0 File Offset: 0x0034B9F0
	private void HandleCurrentRoomExtraData()
	{
		bool flag = false;
		if (this.IsPrimaryPlayer)
		{
			flag = true;
		}
		else if (GameManager.Instance.PrimaryPlayer.healthHaver.IsDead)
		{
			flag = true;
		}
		if (flag)
		{
			if (GameManager.Instance.Dungeon.OverrideAmbientLight)
			{
				RenderSettings.ambientLight = GameManager.Instance.Dungeon.OverrideAmbientColor;
			}
			else if (this.CurrentRoom != null && !this.CurrentRoom.area.IsProceduralRoom && this.CurrentRoom.area.runtimePrototypeData != null && this.CurrentRoom.area.runtimePrototypeData.usesCustomAmbient)
			{
				Color color = this.CurrentRoom.area.runtimePrototypeData.customAmbient;
				if (GameManager.Options.LightingQuality == GameOptions.GenericHighMedLowOption.LOW && this.CurrentRoom.area.runtimePrototypeData.usesDifferentCustomAmbientLowQuality)
				{
					color = this.CurrentRoom.area.runtimePrototypeData.customAmbientLowQuality;
				}
				Vector3 vector = new Vector3(color.r, color.g, color.b) * RenderSettings.ambientIntensity;
				Vector3 vector2 = new Vector3(RenderSettings.ambientLight.r, RenderSettings.ambientLight.g, RenderSettings.ambientLight.b);
				if (vector != vector2)
				{
					Vector3 vector3 = Vector3.MoveTowards(vector2, vector, 0.35f * GameManager.INVARIANT_DELTA_TIME);
					Color color2 = new Color(vector3.x, vector3.y, vector3.z, RenderSettings.ambientLight.a);
					RenderSettings.ambientLight = color2;
				}
			}
			else
			{
				Color color3 = ((GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW) ? GameManager.Instance.Dungeon.decoSettings.ambientLightColor : GameManager.Instance.Dungeon.decoSettings.lowQualityAmbientLightColor);
				Vector3 vector4 = new Vector3(color3.r, color3.g, color3.b) * RenderSettings.ambientIntensity;
				Vector3 vector5 = new Vector3(RenderSettings.ambientLight.r, RenderSettings.ambientLight.g, RenderSettings.ambientLight.b);
				if (vector4 != vector5)
				{
					Vector3 vector6 = Vector3.MoveTowards(vector5, vector4, 0.35f * GameManager.INVARIANT_DELTA_TIME);
					Color color4 = new Color(vector6.x, vector6.y, vector6.z, RenderSettings.ambientLight.a);
					RenderSettings.ambientLight = color4;
				}
			}
		}
	}

	// Token: 0x060081C2 RID: 33218 RVA: 0x0034DA9C File Offset: 0x0034BC9C
	private void HandleGunAttachPointInternal(Gun targetGun, bool isSecondary = false)
	{
		if (targetGun == null)
		{
			return;
		}
		Vector3 vector = this.m_startingAttachPointPosition;
		Vector3 vector2 = this.downwardAttachPointPosition;
		if (targetGun.IsForwardPosition)
		{
			vector = vector.WithX(this.m_spriteDimensions.x - vector.x);
			vector2 = vector2.WithX(this.m_spriteDimensions.x - vector2.x);
		}
		if (this.SpriteFlipped)
		{
			vector = vector.WithX(this.m_spriteDimensions.x - vector.x);
			vector2 = vector2.WithX(this.m_spriteDimensions.x - vector2.x);
		}
		float num = (float)((!this.SpriteFlipped) ? 1 : (-1));
		Vector3 vector3 = targetGun.GetCarryPixelOffset(this.characterIdentity).ToVector3();
		vector += Vector3.Scale(vector3, new Vector3(num, 1f, 1f)) * 0.0625f;
		vector2 += Vector3.Scale(vector3, new Vector3(num, 1f, 1f)) * 0.0625f;
		if (targetGun.Handedness == GunHandedness.NoHanded && this.SpriteFlipped)
		{
			vector += Vector3.Scale(targetGun.leftFacingPixelOffset.ToVector3(), new Vector3(num, 1f, 1f)) * 0.0625f;
			vector2 += Vector3.Scale(targetGun.leftFacingPixelOffset.ToVector3(), new Vector3(num, 1f, 1f)) * 0.0625f;
		}
		if (this.IsFlying)
		{
			vector += new Vector3(0f, 0.1875f, 0f);
			vector2 += new Vector3(0f, 0.1875f, 0f);
		}
		if (isSecondary)
		{
			if (targetGun.transform.parent != this.SecondaryGunPivot)
			{
				targetGun.transform.parent = this.SecondaryGunPivot;
				targetGun.transform.localRotation = Quaternion.identity;
				targetGun.HandleSpriteFlip(this.SpriteFlipped);
				targetGun.UpdateAttachTransform();
			}
			this.SecondaryGunPivot.position = this.gunAttachPoint.position + num * new Vector3(-0.75f, 0f, 0f);
		}
		else
		{
			if (targetGun.transform.parent != this.gunAttachPoint)
			{
				targetGun.transform.parent = this.gunAttachPoint;
				targetGun.transform.localRotation = Quaternion.identity;
				targetGun.HandleSpriteFlip(this.SpriteFlipped);
				targetGun.UpdateAttachTransform();
			}
			if (targetGun.IsHeroSword)
			{
				float num2 = 1f - Mathf.Abs(this.m_currentGunAngle - 90f) / 90f;
				this.gunAttachPoint.localPosition = BraveUtility.QuantizeVector(Vector3.Slerp(vector, vector2, num2), 16f);
			}
			else if (targetGun.Handedness == GunHandedness.TwoHanded)
			{
				float num3 = Mathf.PingPong(Mathf.Abs(1f - Mathf.Abs(this.m_currentGunAngle + 90f) / 90f), 1f);
				Vector2 vector4 = Vector2.zero;
				if (this.m_currentGunAngle > 0f)
				{
					vector4 = Vector2.Scale(targetGun.carryPixelUpOffset.ToVector2(), new Vector2(num, 1f)) * 0.0625f;
				}
				else
				{
					vector4 = Vector2.Scale(targetGun.carryPixelDownOffset.ToVector2(), new Vector2(num, 1f)) * 0.0625f;
				}
				if (targetGun.LockedHorizontalOnCharge)
				{
					vector4 = Vector3.Slerp(vector4, Vector2.zero, targetGun.GetChargeFraction());
				}
				if (this.m_currentGunAngle < 0f)
				{
					this.gunAttachPoint.localPosition = BraveUtility.QuantizeVector(Vector3.Slerp(vector, vector2 + vector4.ToVector3ZUp(0f), num3), 16f);
				}
				else
				{
					this.gunAttachPoint.localPosition = BraveUtility.QuantizeVector(Vector3.Slerp(vector, vector + vector4.ToVector3ZUp(0f), num3), 16f);
				}
			}
			else
			{
				this.gunAttachPoint.localPosition = BraveUtility.QuantizeVector(vector, 16f);
			}
		}
	}

	// Token: 0x060081C3 RID: 33219 RVA: 0x0034DF00 File Offset: 0x0034C100
	private void HandleGunAttachPoint()
	{
		if (this.CurrentGun)
		{
			this.HandleGunAttachPointInternal(this.CurrentGun, false);
		}
		if (this.inventory != null && this.inventory.DualWielding && this.CurrentSecondaryGun)
		{
			this.HandleGunAttachPointInternal(this.CurrentSecondaryGun, true);
		}
	}

	// Token: 0x060081C4 RID: 33220 RVA: 0x0034DF64 File Offset: 0x0034C164
	private void HandleShellCasingDisplacement()
	{
	}

	// Token: 0x060081C5 RID: 33221 RVA: 0x0034DF68 File Offset: 0x0034C168
	protected override void OnDestroy()
	{
		this.ClearOverrideShader();
		if (PassiveItem.ActiveFlagItems != null)
		{
			PassiveItem.ActiveFlagItems.Remove(this);
		}
		base.OnDestroy();
	}

	// Token: 0x060081C6 RID: 33222 RVA: 0x0034DF8C File Offset: 0x0034C18C
	public void TriggerHighStress(float duration)
	{
		if (base.healthHaver)
		{
			base.healthHaver.NextShotKills = true;
		}
		this.m_highStressTimer = duration;
	}

	// Token: 0x060081C7 RID: 33223 RVA: 0x0034DFB4 File Offset: 0x0034C1B4
	private void DoAutoAimNotification(bool warnOnly)
	{
		dfLabel nameLabel = GameUIRoot.Instance.notificationController.NameLabel;
		if (warnOnly)
		{
			GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_WARNING_TITLE")), StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_WARNING_BODY")), null, -1, UINotificationController.NotificationColor.SILVER, false, false);
		}
		else
		{
			GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_POPUP_TITLE")), StringTableManager.PostprocessString(nameLabel.ForceGetLocalizedValue("#SUPERDUPERAUTOAIM_WARNING_BODY_B")), null, -1, UINotificationController.NotificationColor.SILVER, false, false);
		}
	}

	// Token: 0x060081C8 RID: 33224 RVA: 0x0034E044 File Offset: 0x0034C244
	public void DoVibration(Vibration.Time time, Vibration.Strength strength)
	{
		BraveInput.GetInstanceForPlayer(this.PlayerIDX).DoVibration(time, strength);
	}

	// Token: 0x060081C9 RID: 33225 RVA: 0x0034E058 File Offset: 0x0034C258
	public void DoVibration(float time, Vibration.Strength strength)
	{
		BraveInput.GetInstanceForPlayer(this.PlayerIDX).DoVibration(time, strength);
	}

	// Token: 0x060081CA RID: 33226 RVA: 0x0034E06C File Offset: 0x0034C26C
	public void DoVibration(Vibration.Time time, Vibration.Strength largeMotor, Vibration.Strength smallMotor)
	{
		BraveInput.GetInstanceForPlayer(this.PlayerIDX).DoVibration(time, largeMotor, smallMotor);
	}

	// Token: 0x060081CB RID: 33227 RVA: 0x0034E084 File Offset: 0x0034C284
	public void DoScreenShakeVibration(float time, float magnitude)
	{
		BraveInput.GetInstanceForPlayer(this.PlayerIDX).DoScreenShakeVibration(time, magnitude);
	}

	// Token: 0x060081CC RID: 33228 RVA: 0x0034E098 File Offset: 0x0034C298
	public void DoSustainedVibration(Vibration.Strength strength)
	{
		BraveInput.GetInstanceForPlayer(this.PlayerIDX).DoSustainedVibration(strength);
	}

	// Token: 0x060081CD RID: 33229 RVA: 0x0034E0AC File Offset: 0x0034C2AC
	public void DoSustainedVibration(Vibration.Strength largeMotor, Vibration.Strength smallMotor)
	{
		BraveInput.GetInstanceForPlayer(this.PlayerIDX).DoSustainedVibration(largeMotor, smallMotor);
	}

	// Token: 0x17001340 RID: 4928
	// (get) Token: 0x060081CE RID: 33230 RVA: 0x0034E0C0 File Offset: 0x0034C2C0
	public Vector2 LastCommandedDirection
	{
		get
		{
			return this.m_playerCommandedDirection;
		}
	}

	// Token: 0x17001341 RID: 4929
	// (get) Token: 0x060081CF RID: 33231 RVA: 0x0034E0C8 File Offset: 0x0034C2C8
	public Vector2 NonZeroLastCommandedDirection
	{
		get
		{
			return (!(this.m_playerCommandedDirection != Vector2.zero)) ? this.m_lastNonzeroCommandedDirection : this.m_playerCommandedDirection;
		}
	}

	// Token: 0x17001342 RID: 4930
	// (get) Token: 0x060081D0 RID: 33232 RVA: 0x0034E0F0 File Offset: 0x0034C2F0
	public bool IsPetting
	{
		get
		{
			return this.m_pettingTarget != null;
		}
	}

	// Token: 0x04008345 RID: 33605
	public const float c_averageVelocityPeriod = 0.5f;

	// Token: 0x04008346 RID: 33606
	public const float s_dodgeRollBlinkMinPressTime = 0.2f;

	// Token: 0x04008347 RID: 33607
	[Header("Player Properties")]
	public PlayableCharacters characterIdentity;

	// Token: 0x04008348 RID: 33608
	[NonSerialized]
	private bool m_isTemporaryEeveeForUnlock;

	// Token: 0x04008349 RID: 33609
	[NonSerialized]
	public Texture2D portalEeveeTex;

	// Token: 0x0400834A RID: 33610
	[NonSerialized]
	public bool IsGhost;

	// Token: 0x0400834B RID: 33611
	[NonSerialized]
	public bool IsDarkSoulsHollow;

	// Token: 0x0400834C RID: 33612
	[Header("UI Stuff")]
	public string uiPortraitName;

	// Token: 0x0400834D RID: 33613
	public float BosscardSpriteFPS;

	// Token: 0x0400834E RID: 33614
	public List<Texture2D> BosscardSprites;

	// Token: 0x0400834F RID: 33615
	public PerCharacterCoopPositionData CoopBosscardOffset;

	// Token: 0x04008350 RID: 33616
	[Header("Stats")]
	public PlayerStats stats;

	// Token: 0x04008351 RID: 33617
	public DodgeRollStats rollStats;

	// Token: 0x04008352 RID: 33618
	public PitHelpers pitHelpers;

	// Token: 0x04008353 RID: 33619
	public int MAX_GUNS_HELD = 3;

	// Token: 0x04008354 RID: 33620
	public int MAX_ITEMS_HELD = 2;

	// Token: 0x04008355 RID: 33621
	[NonSerialized]
	public bool UsingAlternateStartingGuns;

	// Token: 0x04008356 RID: 33622
	[PickupIdentifier(typeof(Gun))]
	public List<int> startingGunIds;

	// Token: 0x04008357 RID: 33623
	[PickupIdentifier(typeof(Gun))]
	public List<int> startingAlternateGunIds;

	// Token: 0x04008358 RID: 33624
	[PickupIdentifier(typeof(Gun))]
	public List<int> finalFightGunIds;

	// Token: 0x04008359 RID: 33625
	public PlayerConsumables carriedConsumables;

	// Token: 0x0400835A RID: 33626
	public RandomStartingEquipmentSettings randomStartingEquipmentSettings;

	// Token: 0x0400835B RID: 33627
	public bool AllowZeroHealthState;

	// Token: 0x0400835C RID: 33628
	public bool ForceZeroHealthState;

	// Token: 0x0400835D RID: 33629
	[NonSerialized]
	public bool HealthAndArmorSwapped;

	// Token: 0x0400835E RID: 33630
	[NonSerialized]
	public List<LootModData> lootModData = new List<LootModData>();

	// Token: 0x0400835F RID: 33631
	private int m_blanks;

	// Token: 0x04008360 RID: 33632
	public Transform gunAttachPoint;

	// Token: 0x04008361 RID: 33633
	[NonSerialized]
	public Transform secondaryGunAttachPoint;

	// Token: 0x04008362 RID: 33634
	public Vector3 downwardAttachPointPosition;

	// Token: 0x04008363 RID: 33635
	private Vector3 m_startingAttachPointPosition;

	// Token: 0x04008364 RID: 33636
	public float collisionKnockbackStrength = 10f;

	// Token: 0x04008365 RID: 33637
	public PlayerHandController primaryHand;

	// Token: 0x04008366 RID: 33638
	public PlayerHandController secondaryHand;

	// Token: 0x04008367 RID: 33639
	public Vector3 unadjustedAimPoint;

	// Token: 0x04008368 RID: 33640
	private Vector2 m_lastVelocity;

	// Token: 0x04008369 RID: 33641
	public Color outlineColor;

	// Token: 0x0400836A RID: 33642
	public GameObject minimapIconPrefab;

	// Token: 0x0400836B RID: 33643
	public tk2dSpriteAnimation AlternateCostumeLibrary;

	// Token: 0x0400836C RID: 33644
	public List<ActorAudioEvent> animationAudioEvents;

	// Token: 0x0400836D RID: 33645
	public string characterAudioSpeechTag;

	// Token: 0x0400836E RID: 33646
	public bool usingForcedInput;

	// Token: 0x0400836F RID: 33647
	public Vector2 forcedInput;

	// Token: 0x04008370 RID: 33648
	public Vector2? forceAimPoint;

	// Token: 0x04008371 RID: 33649
	public bool forceFire;

	// Token: 0x04008372 RID: 33650
	public bool forceFireDown;

	// Token: 0x04008373 RID: 33651
	public bool forceFireUp;

	// Token: 0x04008374 RID: 33652
	public bool DrawAutoAim;

	// Token: 0x04008375 RID: 33653
	[NonSerialized]
	public bool PastAccessible;

	// Token: 0x04008376 RID: 33654
	public Action<PlayerController> OnIgnited;

	// Token: 0x04008377 RID: 33655
	[NonSerialized]
	public bool WasPausedThisFrame;

	// Token: 0x04008378 RID: 33656
	[NonSerialized]
	private bool m_isOnFire;

	// Token: 0x04008379 RID: 33657
	[NonSerialized]
	private RuntimeGameActorEffectData m_onFireEffectData;

	// Token: 0x0400837A RID: 33658
	[NonSerialized]
	public float CurrentFireMeterValue;

	// Token: 0x0400837B RID: 33659
	[NonSerialized]
	public float CurrentPoisonMeterValue;

	// Token: 0x0400837C RID: 33660
	[NonSerialized]
	public float CurrentDrainMeterValue;

	// Token: 0x0400837D RID: 33661
	[NonSerialized]
	public float CurrentCurseMeterValue;

	// Token: 0x0400837E RID: 33662
	[NonSerialized]
	public bool CurseIsDecaying = true;

	// Token: 0x0400837F RID: 33663
	[NonSerialized]
	public float CurrentFloorDamageCooldown;

	// Token: 0x04008380 RID: 33664
	[NonSerialized]
	public float CurrentStoneGunTimer;

	// Token: 0x04008381 RID: 33665
	[NonSerialized]
	public List<IPlayerOrbital> orbitals = new List<IPlayerOrbital>();

	// Token: 0x04008382 RID: 33666
	[NonSerialized]
	public List<PlayerOrbitalFollower> trailOrbitals = new List<PlayerOrbitalFollower>();

	// Token: 0x04008383 RID: 33667
	[NonSerialized]
	public List<AIActor> companions = new List<AIActor>();

	// Token: 0x04008384 RID: 33668
	private OverridableBool m_capableOfStealing = new OverridableBool(false);

	// Token: 0x04008385 RID: 33669
	[NonSerialized]
	public bool IsEthereal;

	// Token: 0x04008386 RID: 33670
	[NonSerialized]
	public bool HasGottenKeyThisRun;

	// Token: 0x04008387 RID: 33671
	[NonSerialized]
	public int DeathsThisRun;

	// Token: 0x04008388 RID: 33672
	private const float BasePoisonMeterDecayPerSecond = 0.5f;

	// Token: 0x04008389 RID: 33673
	private const float BaseDrainMeterDecayPerSecond = 0.1f;

	// Token: 0x0400838A RID: 33674
	private const float BaseCurseMeterDecayPerSecond = 0.5f;

	// Token: 0x0400838B RID: 33675
	[NonSerialized]
	public Color baseFlatColorOverride = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400838C RID: 33676
	[NonSerialized]
	public List<int> ActiveExtraSynergies = new List<int>();

	// Token: 0x0400838D RID: 33677
	[NonSerialized]
	public List<CustomSynergyType> CustomEventSynergies = new List<CustomSynergyType>();

	// Token: 0x0400838E RID: 33678
	[NonSerialized]
	public bool DeferredStatRecalculationRequired;

	// Token: 0x0400838F RID: 33679
	public bool ForceMetalGearMenu;

	// Token: 0x04008390 RID: 33680
	protected GungeonActions m_activeActions;

	// Token: 0x04008391 RID: 33681
	[NonSerialized]
	public bool CharacterUsesRandomGuns;

	// Token: 0x04008392 RID: 33682
	[NonSerialized]
	public bool UnderstandsGleepGlorp;

	// Token: 0x04008393 RID: 33683
	private static float AAStickTime = 0f;

	// Token: 0x04008394 RID: 33684
	private static float AANonStickTime = 0f;

	// Token: 0x04008395 RID: 33685
	private static float AALastWarnTime = -1000f;

	// Token: 0x04008396 RID: 33686
	private static bool AACanWarn = true;

	// Token: 0x04008397 RID: 33687
	private const float AAStickMultiplier = 1.5f;

	// Token: 0x04008398 RID: 33688
	private const float AAMinWarnDelay = 300f;

	// Token: 0x04008399 RID: 33689
	private const float AATotalStickTime = 660f;

	// Token: 0x0400839A RID: 33690
	private const float AAWarnTime = 300f;

	// Token: 0x0400839B RID: 33691
	private const float AAActivateTime = 600f;

	// Token: 0x0400839C RID: 33692
	public OverridableBool InfiniteAmmo = new OverridableBool(false);

	// Token: 0x0400839D RID: 33693
	public OverridableBool OnlyFinalProjectiles = new OverridableBool(false);

	// Token: 0x0400839E RID: 33694
	[NonSerialized]
	public bool IsStationary;

	// Token: 0x0400839F RID: 33695
	[NonSerialized]
	public bool IsGunLocked;

	// Token: 0x040083A1 RID: 33697
	private TeleporterController m_returnTeleporter;

	// Token: 0x040083A2 RID: 33698
	private bool m_additionalReceivesTouchDamage = true;

	// Token: 0x040083A3 RID: 33699
	public bool IsTalking;

	// Token: 0x040083A4 RID: 33700
	private bool m_wasTalkingThisFrame;

	// Token: 0x040083A5 RID: 33701
	public TalkDoerLite TalkPartner;

	// Token: 0x040083A7 RID: 33703
	private bool m_isInCombat;

	// Token: 0x040083A8 RID: 33704
	public Action OnEnteredCombat;

	// Token: 0x040083AD RID: 33709
	private float m_superDuperAutoAimTimer;

	// Token: 0x040083B2 RID: 33714
	private bool m_isVisible = true;

	// Token: 0x040083B3 RID: 33715
	[HideInInspector]
	public GunInventory inventory;

	// Token: 0x040083B4 RID: 33716
	[NonSerialized]
	private Gun m_cachedQuickEquipGun;

	// Token: 0x040083B5 RID: 33717
	[NonSerialized]
	private Gun m_backupCachedQuickEquipGun;

	// Token: 0x040083B6 RID: 33718
	[NonSerialized]
	public int maxActiveItemsHeld = 2;

	// Token: 0x040083B7 RID: 33719
	[NonSerialized]
	public int spiceCount;

	// Token: 0x040083B8 RID: 33720
	[PickupIdentifier(typeof(PlayerItem))]
	public List<int> startingActiveItemIds;

	// Token: 0x040083B9 RID: 33721
	[NonSerialized]
	public List<PlayerItem> activeItems = new List<PlayerItem>();

	// Token: 0x040083BA RID: 33722
	[PickupIdentifier(typeof(PassiveItem))]
	public List<int> startingPassiveItemIds;

	// Token: 0x040083BB RID: 33723
	[NonSerialized]
	public List<PassiveItem> passiveItems = new List<PassiveItem>();

	// Token: 0x040083BC RID: 33724
	public List<StatModifier> ownerlessStatModifiers = new List<StatModifier>();

	// Token: 0x040083BD RID: 33725
	[NonSerialized]
	public List<PickupObject> additionalItems = new List<PickupObject>();

	// Token: 0x040083BE RID: 33726
	public bool ForceHandless;

	// Token: 0x040083BF RID: 33727
	public bool HandsOnAltCostume;

	// Token: 0x040083C0 RID: 33728
	public bool SwapHandsOnAltCostume;

	// Token: 0x040083C1 RID: 33729
	public string altHandName;

	// Token: 0x040083C2 RID: 33730
	public bool hasArmorlessAnimations;

	// Token: 0x040083C3 RID: 33731
	public GameObject lostAllArmorVFX;

	// Token: 0x040083C4 RID: 33732
	public GameObject lostAllArmorAltVfx;

	// Token: 0x040083C5 RID: 33733
	public GameObject CustomDodgeRollEffect;

	// Token: 0x040083D3 RID: 33747
	public Func<Gun, Projectile, Projectile> OnPreFireProjectileModifier;

	// Token: 0x040083D4 RID: 33748
	private int m_enemiesKilled;

	// Token: 0x040083D5 RID: 33749
	private float m_gunGameDamageThreshold = 200f;

	// Token: 0x040083D6 RID: 33750
	private const float c_gunGameDamageThreshold = 200f;

	// Token: 0x040083D7 RID: 33751
	private float m_gunGameElapsed;

	// Token: 0x040083D8 RID: 33752
	private const float c_gunGameElapsedThreshold = 20f;

	// Token: 0x040083DA RID: 33754
	private const float c_fireMeterChargeRate = 0.666666f;

	// Token: 0x040083DB RID: 33755
	private const float c_fireMeterRollingChargeRate = 0.2f;

	// Token: 0x040083DC RID: 33756
	private const float c_fireMeterRollDecrease = 0.5f;

	// Token: 0x040083DD RID: 33757
	public Action<PlayerController, Chest> OnChestBroken;

	// Token: 0x040083E6 RID: 33766
	public Action<Projectile, PlayerController> OnHitByProjectile;

	// Token: 0x040083E7 RID: 33767
	public Action<PlayerController, Gun> OnReloadPressed;

	// Token: 0x040083E8 RID: 33768
	public Action<PlayerController, Gun> OnReloadedGun;

	// Token: 0x040083EC RID: 33772
	public Action<FlippableCover> OnTableFlipped;

	// Token: 0x040083ED RID: 33773
	public Action<FlippableCover> OnTableFlipCompleted;

	// Token: 0x040083EE RID: 33774
	public Action<PlayerController> OnNewFloorLoaded;

	// Token: 0x040083EF RID: 33775
	[HideInInspector]
	public Vector2 knockbackComponent;

	// Token: 0x040083F0 RID: 33776
	[HideInInspector]
	public Vector2 immutableKnockbackComponent;

	// Token: 0x040083F1 RID: 33777
	[HideInInspector]
	public OverridableBool ImmuneToPits = new OverridableBool(false);

	// Token: 0x040083F3 RID: 33779
	private MeshRenderer m_renderer;

	// Token: 0x040083F4 RID: 33780
	private CoinBloop m_blooper;

	// Token: 0x040083F5 RID: 33781
	private KeyBullet m_setupKeyBullet;

	// Token: 0x040083F6 RID: 33782
	public float RealtimeEnteredCurrentRoom;

	// Token: 0x040083F7 RID: 33783
	private RoomHandler m_currentRoom;

	// Token: 0x040083F8 RID: 33784
	private Vector3 m_spriteDimensions;

	// Token: 0x040083F9 RID: 33785
	private int m_equippedGunShift;

	// Token: 0x040083FA RID: 33786
	private List<tk2dBaseSprite> m_attachedSprites = new List<tk2dBaseSprite>();

	// Token: 0x040083FB RID: 33787
	private List<float> m_attachedSpriteDepths = new List<float>();

	// Token: 0x040083FC RID: 33788
	[NonSerialized]
	public Dictionary<string, GameObject> SpawnedSubobjects = new Dictionary<string, GameObject>();

	// Token: 0x040083FD RID: 33789
	private PlayerInputState m_inputState;

	// Token: 0x040083FE RID: 33790
	private OverridableBool m_disableInput = new OverridableBool(false);

	// Token: 0x040083FF RID: 33791
	protected bool m_shouldContinueFiring;

	// Token: 0x04008400 RID: 33792
	protected bool m_handlingQueuedAnimation;

	// Token: 0x04008401 RID: 33793
	private bool m_interruptingPitRespawn;

	// Token: 0x04008402 RID: 33794
	private bool m_skipPitRespawn;

	// Token: 0x04008403 RID: 33795
	private Vector2 lockedDodgeRollDirection;

	// Token: 0x04008404 RID: 33796
	private int m_selectedItemIndex;

	// Token: 0x04008405 RID: 33797
	private IPlayerInteractable m_lastInteractionTarget;

	// Token: 0x04008406 RID: 33798
	private List<IPlayerInteractable> m_leapInteractables = new List<IPlayerInteractable>();

	// Token: 0x04008407 RID: 33799
	private float m_currentGunAngle;

	// Token: 0x04008408 RID: 33800
	private float? m_overrideGunAngle;

	// Token: 0x04008409 RID: 33801
	[NonSerialized]
	public MineCartController currentMineCart;

	// Token: 0x0400840A RID: 33802
	public MineCartController previousMineCart;

	// Token: 0x0400840B RID: 33803
	protected PlayerController.DodgeRollState m_dodgeRollState = PlayerController.DodgeRollState.None;

	// Token: 0x0400840C RID: 33804
	private float m_dodgeRollTimer;

	// Token: 0x0400840D RID: 33805
	private bool m_isSlidingOverSurface;

	// Token: 0x0400840E RID: 33806
	private Vector3 m_cachedAimDirection = Vector3.right;

	// Token: 0x0400840F RID: 33807
	private bool m_cachedGrounded = true;

	// Token: 0x04008412 RID: 33810
	private bool m_highAccuracyAimMode;

	// Token: 0x04008413 RID: 33811
	private Vector2 m_previousAimVector;

	// Token: 0x04008418 RID: 33816
	private int m_masteryTokensCollectedThisRun;

	// Token: 0x04008419 RID: 33817
	[NonSerialized]
	public bool EverHadMap;

	// Token: 0x0400841A RID: 33818
	[NonSerialized]
	public tk2dSpriteAnimation OverrideAnimationLibrary;

	// Token: 0x0400841B RID: 33819
	[NonSerialized]
	private tk2dSpriteAnimation BaseAnimationLibrary;

	// Token: 0x0400841C RID: 33820
	[NonSerialized]
	public bool PlayerIsRatTransformed;

	// Token: 0x0400841D RID: 33821
	private string m_overridePlayerSwitchState;

	// Token: 0x0400841E RID: 33822
	public int PlayerIDX = -1;

	// Token: 0x0400841F RID: 33823
	[NonSerialized]
	public int NumRoomsCleared;

	// Token: 0x04008420 RID: 33824
	[NonSerialized]
	public string LevelToLoadOnPitfall;

	// Token: 0x04008421 RID: 33825
	[NonSerialized]
	private string m_cachedLevelToLoadOnPitfall;

	// Token: 0x04008422 RID: 33826
	private const bool c_coopSynergies = true;

	// Token: 0x04008423 RID: 33827
	[NonSerialized]
	public bool ZeroVelocityThisFrame;

	// Token: 0x04008424 RID: 33828
	private float dx9counter;

	// Token: 0x04008425 RID: 33829
	[NonSerialized]
	public bool IsUsingAlternateCostume;

	// Token: 0x04008426 RID: 33830
	private bool m_usingCustomHandType;

	// Token: 0x04008427 RID: 33831
	private int m_baseHandId;

	// Token: 0x04008428 RID: 33832
	private tk2dSpriteCollectionData m_baseHandCollection;

	// Token: 0x04008429 RID: 33833
	private StatModifier m_turboSpeedModifier;

	// Token: 0x0400842A RID: 33834
	private StatModifier m_turboEnemyBulletModifier;

	// Token: 0x0400842B RID: 33835
	private StatModifier m_turboRollSpeedModifier;

	// Token: 0x0400842C RID: 33836
	public bool FlatColorOverridden;

	// Token: 0x0400842D RID: 33837
	private bool m_usesRandomStartingEquipment;

	// Token: 0x0400842E RID: 33838
	private bool m_randomStartingItemsInitialized;

	// Token: 0x0400842F RID: 33839
	private int m_randomStartingEquipmentSeed = -1;

	// Token: 0x04008430 RID: 33840
	public bool IsCurrentlyCoopReviving;

	// Token: 0x04008431 RID: 33841
	private string[] confettiPaths;

	// Token: 0x04008432 RID: 33842
	private float m_coopRoomTimer;

	// Token: 0x04008433 RID: 33843
	private Material[] m_cachedOverrideMaterials;

	// Token: 0x04008434 RID: 33844
	private bool m_isStartingTeleport;

	// Token: 0x04008435 RID: 33845
	protected float m_elapsedNonalertTime;

	// Token: 0x04008436 RID: 33846
	public Action<float, bool, HealthHaver> OnAnyEnemyReceivedDamage;

	// Token: 0x04008437 RID: 33847
	private bool m_cloneWaitingForCoopDeath;

	// Token: 0x04008438 RID: 33848
	public Action LostArmor;

	// Token: 0x04008439 RID: 33849
	private bool m_revenging;

	// Token: 0x0400843A RID: 33850
	private AfterImageTrailController m_hollowAfterImage;

	// Token: 0x0400843B RID: 33851
	private Color m_ghostUnchargedColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400843C RID: 33852
	private Color m_ghostChargedColor = new Color(0.2f, 0.3f, 1f, 1f);

	// Token: 0x0400843D RID: 33853
	private bool m_isCoopArrowing;

	// Token: 0x0400843E RID: 33854
	private bool m_isThreatArrowing;

	// Token: 0x0400843F RID: 33855
	private AIActor m_threadArrowTarget;

	// Token: 0x04008440 RID: 33856
	public Action<PlayerController> OnRealPlayerDeath;

	// Token: 0x04008441 RID: 33857
	private bool m_suppressItemSwitchTo;

	// Token: 0x04008442 RID: 33858
	protected GameObject BlankVFXPrefab;

	// Token: 0x04008443 RID: 33859
	private Color m_alienDamageColor = new Color(1f, 0f, 0f, 1f);

	// Token: 0x04008444 RID: 33860
	private Color m_alienBlankColor = new Color(0.35f, 0.35f, 1f, 1f);

	// Token: 0x04008445 RID: 33861
	protected Coroutine m_currentActiveItemDestructionCoroutine;

	// Token: 0x04008446 RID: 33862
	protected float m_postDodgeRollGunTimer;

	// Token: 0x04008447 RID: 33863
	private const float AIM_VECTOR_MAGNITUDE_CUTOFF = 0.4f;

	// Token: 0x04008448 RID: 33864
	public OverridableBool AdditionalCanDodgeRollWhileFlying = new OverridableBool(false);

	// Token: 0x04008449 RID: 33865
	private bool m_handleDodgeRollStartThisFrame;

	// Token: 0x0400844A RID: 33866
	private float m_timeHeldBlinkButton;

	// Token: 0x0400844B RID: 33867
	private Vector2 m_cachedBlinkPosition;

	// Token: 0x0400844C RID: 33868
	private tk2dSprite m_extantBlinkShadow;

	// Token: 0x0400844D RID: 33869
	private int m_currentDodgeRollDepth;

	// Token: 0x0400844E RID: 33870
	public Action<tk2dSprite> OnBlinkShadowCreated;

	// Token: 0x0400844F RID: 33871
	public List<FlippableCover> TablesDamagedThisSlide = new List<FlippableCover>();

	// Token: 0x04008450 RID: 33872
	private bool m_hasFiredWhileSliding;

	// Token: 0x04008451 RID: 33873
	[NonSerialized]
	public bool LastFollowerVisibilityState = true;

	// Token: 0x04008452 RID: 33874
	private bool m_gunChangePressedWhileMetalGeared;

	// Token: 0x04008453 RID: 33875
	private int exceptionTracker;

	// Token: 0x04008454 RID: 33876
	private bool m_interactedThisFrame;

	// Token: 0x04008455 RID: 33877
	private bool m_preventItemSwitching;

	// Token: 0x04008456 RID: 33878
	protected RoomHandler m_roomBeforeExit;

	// Token: 0x04008457 RID: 33879
	protected RoomHandler m_previousExitLinkedRoom;

	// Token: 0x04008458 RID: 33880
	protected bool m_inExitLastFrame;

	// Token: 0x04008459 RID: 33881
	private List<IntVector2> m_bellygeonDepressedTiles = new List<IntVector2>();

	// Token: 0x0400845A RID: 33882
	private static Dictionary<IntVector2, float> m_bellygeonDepressedTileTimers = new Dictionary<IntVector2, float>(new IntVector2EqualityComparer());

	// Token: 0x0400845B RID: 33883
	private IntVector2 m_cachedLastCenterCellBellygeon = IntVector2.NegOne;

	// Token: 0x0400845C RID: 33884
	private float m_highStressTimer;

	// Token: 0x0400845D RID: 33885
	private Vector2 m_cachedTeleportSpot;

	// Token: 0x0400845E RID: 33886
	private OverridableBool m_hideRenderers = new OverridableBool(false);

	// Token: 0x0400845F RID: 33887
	private OverridableBool m_hideGunRenderers = new OverridableBool(false);

	// Token: 0x04008460 RID: 33888
	private OverridableBool m_hideHandRenderers = new OverridableBool(false);

	// Token: 0x04008461 RID: 33889
	private CellVisualData.CellFloorType? m_prevFloorType;

	// Token: 0x04008462 RID: 33890
	protected List<AIActor> m_rollDamagedEnemies = new List<AIActor>();

	// Token: 0x04008463 RID: 33891
	protected Vector2 m_playerCommandedDirection;

	// Token: 0x04008464 RID: 33892
	private Vector2 m_lastNonzeroCommandedDirection;

	// Token: 0x04008465 RID: 33893
	private float m_controllerSemiAutoTimer;

	// Token: 0x04008466 RID: 33894
	private float m_startingMovementSpeed;

	// Token: 0x04008467 RID: 33895
	private float m_maxIceFactor;

	// Token: 0x04008468 RID: 33896
	private float m_blankCooldownTimer;

	// Token: 0x04008469 RID: 33897
	public float gunReloadDisplayTimer;

	// Token: 0x0400846A RID: 33898
	private float m_dropGunTimer;

	// Token: 0x0400846B RID: 33899
	private float m_metalGearTimer;

	// Token: 0x0400846C RID: 33900
	private int m_metalGearFrames;

	// Token: 0x0400846D RID: 33901
	private bool m_gunWasDropped;

	// Token: 0x0400846E RID: 33902
	private bool m_metalWasGeared;

	// Token: 0x0400846F RID: 33903
	private float m_dropItemTimer;

	// Token: 0x04008470 RID: 33904
	private bool m_itemWasDropped;

	// Token: 0x04008471 RID: 33905
	private const float GunDropTimerThreshold = 0.5f;

	// Token: 0x04008472 RID: 33906
	private const float MetalGearTimerThreshold = 0.175f;

	// Token: 0x04008473 RID: 33907
	private const float CoopGhostBlankCooldown = 5f;

	// Token: 0x04008474 RID: 33908
	private float c_iceVelocityMinClamp = 0.125f;

	// Token: 0x04008475 RID: 33909
	private bool m_newFloorNoInput;

	// Token: 0x04008476 RID: 33910
	private bool m_allowMoveAsAim;

	// Token: 0x04008477 RID: 33911
	private float m_petDirection;

	// Token: 0x04008478 RID: 33912
	public CompanionController m_pettingTarget;

	// Token: 0x020015D8 RID: 5592
	public enum DodgeRollState
	{
		// Token: 0x0400847E RID: 33918
		PreRollDelay,
		// Token: 0x0400847F RID: 33919
		InAir,
		// Token: 0x04008480 RID: 33920
		OnGround,
		// Token: 0x04008481 RID: 33921
		None,
		// Token: 0x04008482 RID: 33922
		AdditionalDelay,
		// Token: 0x04008483 RID: 33923
		Blink
	}

	// Token: 0x020015D9 RID: 5593
	public enum EscapeSealedRoomStyle
	{
		// Token: 0x04008485 RID: 33925
		DEATH_SEQUENCE,
		// Token: 0x04008486 RID: 33926
		ESCAPE_SPIN,
		// Token: 0x04008487 RID: 33927
		NONE,
		// Token: 0x04008488 RID: 33928
		TELEPORTER,
		// Token: 0x04008489 RID: 33929
		GRIP_MASTER
	}
}
