using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Brave.BulletScript;
using Dungeonator;
using PathologicalGames;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001605 RID: 5637
public class Projectile : BraveBehaviour
{
	// Token: 0x17001388 RID: 5000
	// (get) Token: 0x060082F3 RID: 33523 RVA: 0x003589DC File Offset: 0x00356BDC
	public static float EnemyBulletSpeedMultiplier
	{
		get
		{
			return Projectile.s_enemyBulletSpeedModfier;
		}
	}

	// Token: 0x060082F4 RID: 33524 RVA: 0x003589E4 File Offset: 0x00356BE4
	public static void UpdateEnemyBulletSpeedMultiplier()
	{
		float num = 1f;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num = GameManager.Instance.COOP_ENEMY_PROJECTILE_SPEED_MULTIPLIER;
		}
		if (GameManager.Instance.Dungeon != null)
		{
			Projectile.s_enemyBulletSpeedModfier = Projectile.s_baseEnemyBulletSpeedMultiplier * GameManager.Instance.Dungeon.GetNewPlayerSpeedMultiplier() * PlayerStats.GetTotalEnemyProjectileSpeedMultiplier() * num;
		}
		else
		{
			Projectile.s_enemyBulletSpeedModfier = Projectile.s_baseEnemyBulletSpeedMultiplier;
		}
	}

	// Token: 0x17001389 RID: 5001
	// (get) Token: 0x060082F5 RID: 33525 RVA: 0x00358A58 File Offset: 0x00356C58
	// (set) Token: 0x060082F6 RID: 33526 RVA: 0x00358A60 File Offset: 0x00356C60
	public static float BaseEnemyBulletSpeedMultiplier
	{
		get
		{
			return Projectile.s_baseEnemyBulletSpeedMultiplier;
		}
		set
		{
			Projectile.s_baseEnemyBulletSpeedMultiplier = value;
			Projectile.UpdateEnemyBulletSpeedMultiplier();
		}
	}

	// Token: 0x1700138A RID: 5002
	// (get) Token: 0x060082F7 RID: 33527 RVA: 0x00358A70 File Offset: 0x00356C70
	// (set) Token: 0x060082F8 RID: 33528 RVA: 0x00358A78 File Offset: 0x00356C78
	public BulletScriptBehavior braveBulletScript { get; set; }

	// Token: 0x1700138B RID: 5003
	// (get) Token: 0x060082F9 RID: 33529 RVA: 0x00358A84 File Offset: 0x00356C84
	// (set) Token: 0x060082FA RID: 33530 RVA: 0x00358A8C File Offset: 0x00356C8C
	[HideInInspector]
	public GameActor Owner
	{
		get
		{
			return this.m_owner;
		}
		set
		{
			this.m_owner = value;
			if (this.m_owner is AIActor)
			{
				this.OwnerName = (this.m_owner as AIActor).GetActorName();
			}
			else if (this.m_owner is PlayerController)
			{
				if (this.PossibleSourceGun == null)
				{
					this.PossibleSourceGun = (this.m_owner as PlayerController).CurrentGun;
				}
				this.OwnerName = ((!(this.m_owner as PlayerController).IsPrimaryPlayer) ? "secondaryplayer" : "primaryplayer");
			}
			this.CheckBlackPhantomness();
		}
	}

	// Token: 0x1700138C RID: 5004
	// (get) Token: 0x060082FB RID: 33531 RVA: 0x00358B34 File Offset: 0x00356D34
	// (set) Token: 0x060082FC RID: 33532 RVA: 0x00358B3C File Offset: 0x00356D3C
	public ProjectileTrapController TrapOwner { get; set; }

	// Token: 0x1700138D RID: 5005
	// (get) Token: 0x060082FD RID: 33533 RVA: 0x00358B48 File Offset: 0x00356D48
	// (set) Token: 0x060082FE RID: 33534 RVA: 0x00358B50 File Offset: 0x00356D50
	public string OwnerName { get; set; }

	// Token: 0x060082FF RID: 33535 RVA: 0x00358B5C File Offset: 0x00356D5C
	public void SetOwnerSafe(GameActor owner, string ownerName)
	{
		this.m_owner = owner;
		this.OwnerName = ownerName;
		this.CheckBlackPhantomness();
	}

	// Token: 0x1700138E RID: 5006
	// (get) Token: 0x06008300 RID: 33536 RVA: 0x00358B74 File Offset: 0x00356D74
	public float GetCachedBaseDamage
	{
		get
		{
			return this.m_cachedInitialDamage;
		}
	}

	// Token: 0x1700138F RID: 5007
	// (get) Token: 0x06008301 RID: 33537 RVA: 0x00358B7C File Offset: 0x00356D7C
	public float ModifiedDamage
	{
		get
		{
			return this.baseData.damage;
		}
	}

	// Token: 0x17001390 RID: 5008
	// (get) Token: 0x06008302 RID: 33538 RVA: 0x00358B8C File Offset: 0x00356D8C
	// (set) Token: 0x06008303 RID: 33539 RVA: 0x00358B94 File Offset: 0x00356D94
	public bool SuppressHitEffects { get; set; }

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x06008304 RID: 33540 RVA: 0x00358BA0 File Offset: 0x00356DA0
	// (remove) Token: 0x06008305 RID: 33541 RVA: 0x00358BD8 File Offset: 0x00356DD8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Projectile> OnPostUpdate;

	// Token: 0x140000CD RID: 205
	// (add) Token: 0x06008306 RID: 33542 RVA: 0x00358C10 File Offset: 0x00356E10
	// (remove) Token: 0x06008307 RID: 33543 RVA: 0x00358C48 File Offset: 0x00356E48
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Projectile> OnReflected;

	// Token: 0x140000CE RID: 206
	// (add) Token: 0x06008308 RID: 33544 RVA: 0x00358C80 File Offset: 0x00356E80
	// (remove) Token: 0x06008309 RID: 33545 RVA: 0x00358CB8 File Offset: 0x00356EB8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Projectile> OnDestruction;

	// Token: 0x17001391 RID: 5009
	// (get) Token: 0x0600830A RID: 33546 RVA: 0x00358CF0 File Offset: 0x00356EF0
	protected float LocalTimeScale
	{
		get
		{
			if (this.Owner && this.Owner is AIActor)
			{
				return (this.Owner as AIActor).LocalTimeScale;
			}
			if (this.TrapOwner)
			{
				return this.TrapOwner.LocalTimeScale;
			}
			return Time.timeScale;
		}
	}

	// Token: 0x17001392 RID: 5010
	// (get) Token: 0x0600830B RID: 33547 RVA: 0x00358D50 File Offset: 0x00356F50
	public float LocalDeltaTime
	{
		get
		{
			if (this.Owner && this.Owner is AIActor)
			{
				return (this.Owner as AIActor).LocalDeltaTime;
			}
			return BraveTime.DeltaTime;
		}
	}

	// Token: 0x17001393 RID: 5011
	// (get) Token: 0x0600830C RID: 33548 RVA: 0x00358D88 File Offset: 0x00356F88
	// (set) Token: 0x0600830D RID: 33549 RVA: 0x00358D90 File Offset: 0x00356F90
	public SpeculativeRigidbody Shooter
	{
		get
		{
			return this.m_shooter;
		}
		set
		{
			this.m_shooter = value;
			if (!this.allowSelfShooting)
			{
				base.specRigidbody.RegisterSpecificCollisionException(this.m_shooter);
			}
		}
	}

	// Token: 0x17001394 RID: 5012
	// (get) Token: 0x0600830E RID: 33550 RVA: 0x00358DB8 File Offset: 0x00356FB8
	// (set) Token: 0x0600830F RID: 33551 RVA: 0x00358DC0 File Offset: 0x00356FC0
	public float Speed
	{
		get
		{
			return this.m_currentSpeed;
		}
		set
		{
			this.m_currentSpeed = value;
		}
	}

	// Token: 0x17001395 RID: 5013
	// (get) Token: 0x06008310 RID: 33552 RVA: 0x00358DCC File Offset: 0x00356FCC
	// (set) Token: 0x06008311 RID: 33553 RVA: 0x00358DD4 File Offset: 0x00356FD4
	public Vector2 Direction
	{
		get
		{
			return this.m_currentDirection;
		}
		set
		{
			this.m_currentDirection = value;
		}
	}

	// Token: 0x17001396 RID: 5014
	// (get) Token: 0x06008312 RID: 33554 RVA: 0x00358DE0 File Offset: 0x00356FE0
	public bool CanKillBosses
	{
		get
		{
			return !(this.Owner == null) && this.Owner is PlayerController && (this.Owner as PlayerController).BossKillingMode;
		}
	}

	// Token: 0x17001397 RID: 5015
	// (get) Token: 0x06008313 RID: 33555 RVA: 0x00358E18 File Offset: 0x00357018
	// (set) Token: 0x06008314 RID: 33556 RVA: 0x00358E20 File Offset: 0x00357020
	public Projectile.ProjectileDestroyMode DestroyMode { get; set; }

	// Token: 0x17001398 RID: 5016
	// (get) Token: 0x06008315 RID: 33557 RVA: 0x00358E2C File Offset: 0x0035702C
	// (set) Token: 0x06008316 RID: 33558 RVA: 0x00358E34 File Offset: 0x00357034
	public bool Inverted { get; set; }

	// Token: 0x17001399 RID: 5017
	// (get) Token: 0x06008317 RID: 33559 RVA: 0x00358E40 File Offset: 0x00357040
	// (set) Token: 0x06008318 RID: 33560 RVA: 0x00358E48 File Offset: 0x00357048
	public Vector2 LastVelocity { get; set; }

	// Token: 0x1700139A RID: 5018
	// (get) Token: 0x06008319 RID: 33561 RVA: 0x00358E54 File Offset: 0x00357054
	// (set) Token: 0x0600831A RID: 33562 RVA: 0x00358E5C File Offset: 0x0035705C
	public bool ManualControl { get; set; }

	// Token: 0x1700139B RID: 5019
	// (get) Token: 0x0600831B RID: 33563 RVA: 0x00358E68 File Offset: 0x00357068
	// (set) Token: 0x0600831C RID: 33564 RVA: 0x00358E70 File Offset: 0x00357070
	public bool ForceBlackBullet
	{
		get
		{
			return this.m_forceBlackBullet;
		}
		set
		{
			if (this.m_forceBlackBullet != value)
			{
				this.m_forceBlackBullet = value;
				this.CheckBlackPhantomness();
			}
			else
			{
				this.m_forceBlackBullet = value;
			}
		}
	}

	// Token: 0x1700139C RID: 5020
	// (get) Token: 0x0600831D RID: 33565 RVA: 0x00358E98 File Offset: 0x00357098
	// (set) Token: 0x0600831E RID: 33566 RVA: 0x00358EA0 File Offset: 0x003570A0
	public bool IsBulletScript { get; set; }

	// Token: 0x1700139D RID: 5021
	// (get) Token: 0x0600831F RID: 33567 RVA: 0x00358EAC File Offset: 0x003570AC
	public bool CanBeKilledByExplosions
	{
		get
		{
			bool? cachedHasBeamController = this.m_cachedHasBeamController;
			if (cachedHasBeamController == null)
			{
				this.m_cachedHasBeamController = new bool?(base.GetComponent<BeamController>());
			}
			return !this.m_cachedHasBeamController.Value && !this.ImmuneToBlanks && !this.ImmuneToSustainedBlanks;
		}
	}

	// Token: 0x1700139E RID: 5022
	// (get) Token: 0x06008320 RID: 33568 RVA: 0x00358F0C File Offset: 0x0035710C
	public bool CanBeCaught
	{
		get
		{
			PierceProjModifier component = base.GetComponent<PierceProjModifier>();
			return (!(component != null) || component.BeastModeLevel == PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE) && base.sprite;
		}
	}

	// Token: 0x1700139F RID: 5023
	// (get) Token: 0x06008321 RID: 33569 RVA: 0x00358F4C File Offset: 0x0035714C
	public float ElapsedTime
	{
		get
		{
			return this.m_timeElapsed;
		}
	}

	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x06008322 RID: 33570 RVA: 0x00358F54 File Offset: 0x00357154
	// (set) Token: 0x06008323 RID: 33571 RVA: 0x00358F5C File Offset: 0x0035715C
	public Vector2? OverrideTrailPoint { get; set; }

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x06008324 RID: 33572 RVA: 0x00358F68 File Offset: 0x00357168
	// (set) Token: 0x06008325 RID: 33573 RVA: 0x00358F70 File Offset: 0x00357170
	public bool SkipDistanceElapsedCheck { get; set; }

	// Token: 0x170013A2 RID: 5026
	// (get) Token: 0x06008326 RID: 33574 RVA: 0x00358F7C File Offset: 0x0035717C
	// (set) Token: 0x06008327 RID: 33575 RVA: 0x00358F84 File Offset: 0x00357184
	public bool ImmuneToBlanks { get; set; }

	// Token: 0x170013A3 RID: 5027
	// (get) Token: 0x06008328 RID: 33576 RVA: 0x00358F90 File Offset: 0x00357190
	// (set) Token: 0x06008329 RID: 33577 RVA: 0x00358F98 File Offset: 0x00357198
	public bool ImmuneToSustainedBlanks { get; set; }

	// Token: 0x170013A4 RID: 5028
	// (get) Token: 0x0600832A RID: 33578 RVA: 0x00358FA4 File Offset: 0x003571A4
	// (set) Token: 0x0600832B RID: 33579 RVA: 0x00358FAC File Offset: 0x003571AC
	public bool ForcePlayerBlankable { get; set; }

	// Token: 0x170013A5 RID: 5029
	// (get) Token: 0x0600832C RID: 33580 RVA: 0x00358FB8 File Offset: 0x003571B8
	// (set) Token: 0x0600832D RID: 33581 RVA: 0x00358FC0 File Offset: 0x003571C0
	public bool IsReflectedBySword { get; set; }

	// Token: 0x170013A6 RID: 5030
	// (get) Token: 0x0600832E RID: 33582 RVA: 0x00358FCC File Offset: 0x003571CC
	// (set) Token: 0x0600832F RID: 33583 RVA: 0x00358FD4 File Offset: 0x003571D4
	public int LastReflectedSlashId { get; set; }

	// Token: 0x170013A7 RID: 5031
	// (get) Token: 0x06008330 RID: 33584 RVA: 0x00358FE0 File Offset: 0x003571E0
	// (set) Token: 0x06008331 RID: 33585 RVA: 0x00358FE8 File Offset: 0x003571E8
	public ProjectileTrailRendererController TrailRendererController { get; set; }

	// Token: 0x06008332 RID: 33586 RVA: 0x00358FF4 File Offset: 0x003571F4
	public static void SetGlobalProjectileDepth(float newDepth)
	{
		Projectile.CurrentProjectileDepth = newDepth;
	}

	// Token: 0x06008333 RID: 33587 RVA: 0x00358FFC File Offset: 0x003571FC
	public static void ResetGlobalProjectileDepth()
	{
		Projectile.CurrentProjectileDepth = 0.8f;
	}

	// Token: 0x06008334 RID: 33588 RVA: 0x00359008 File Offset: 0x00357208
	public void Awake()
	{
		if (this.baseData == null)
		{
			this.baseData = new ProjectileData();
		}
		if (this.BulletScriptSettings == null)
		{
			this.BulletScriptSettings = new BulletScriptSettings();
		}
		this.m_transform = base.transform;
		this.m_cachedInitialDamage = this.baseData.damage;
		if (base.specRigidbody != null)
		{
			if (this.PenetratesInternalWalls)
			{
				SpeculativeRigidbody specRigidbody = base.specRigidbody;
				specRigidbody.OnPreTileCollision = (SpeculativeRigidbody.OnPreTileCollisionDelegate)Delegate.Combine(specRigidbody.OnPreTileCollision, new SpeculativeRigidbody.OnPreTileCollisionDelegate(this.OnPreTileCollision));
			}
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
			specRigidbody3.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(specRigidbody3.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			SpeculativeRigidbody specRigidbody4 = base.specRigidbody;
			specRigidbody4.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody4.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		}
		if (!base.sprite)
		{
			base.sprite = base.GetComponentInChildren<tk2dSprite>();
		}
		if (!base.spriteAnimator && base.sprite)
		{
			base.spriteAnimator = base.sprite.spriteAnimator;
		}
		if (this.m_renderer == null)
		{
			this.m_renderer = base.GetComponentInChildren<MeshRenderer>();
		}
	}

	// Token: 0x06008335 RID: 33589 RVA: 0x00359184 File Offset: 0x00357384
	public void Reawaken()
	{
		if (!base.sprite)
		{
			base.sprite = base.GetComponentInChildren<tk2dSprite>();
		}
		if (!base.spriteAnimator && base.sprite)
		{
			base.spriteAnimator = base.sprite.spriteAnimator;
		}
		if (this.m_renderer == null)
		{
			this.m_renderer = base.GetComponentInChildren<MeshRenderer>();
		}
	}

	// Token: 0x06008336 RID: 33590 RVA: 0x003591FC File Offset: 0x003573FC
	public void RuntimeUpdateScale(float multiplier)
	{
		if (!base.sprite)
		{
			return;
		}
		float x = base.sprite.scale.x;
		float num = Mathf.Clamp(x * multiplier, 0.01f, Projectile.s_maxProjectileScale);
		this.AdditionalScaleMultiplier *= multiplier;
		base.sprite.scale = new Vector3(num, num, num);
		if (base.specRigidbody != null)
		{
			base.specRigidbody.UpdateCollidersOnScale = true;
		}
		if (num > 1.5f)
		{
			Vector3 size = base.sprite.GetBounds().size;
			if (size.x > 4f || size.y > 4f)
			{
				base.sprite.HeightOffGround = UnityEngine.Random.Range(0f, -3f);
			}
		}
	}

	// Token: 0x06008337 RID: 33591 RVA: 0x003592DC File Offset: 0x003574DC
	public virtual void Start()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		this.m_transform = base.transform;
		if (!string.IsNullOrEmpty(this.additionalStartEventName))
		{
			AkSoundEngine.PostEvent(this.additionalStartEventName, base.gameObject);
		}
		StaticReferenceManager.AddProjectile(this);
		if (base.GetComponent<BeamController>())
		{
			base.enabled = false;
			return;
		}
		if (this.m_renderer)
		{
			DepthLookupManager.ProcessRenderer(this.m_renderer);
		}
		if (base.sprite)
		{
			base.sprite.HeightOffGround = Projectile.CurrentProjectileDepth;
			this.m_currentRampHeight = 0f;
			float num = BraveMathCollege.ClampAngle360(this.m_transform.eulerAngles.z);
			if (this.Owner is PlayerController)
			{
				float num2 = (this.Owner as PlayerController).BulletScaleModifier;
				num2 = Mathf.Clamp(num2 * this.AdditionalScaleMultiplier, 0.01f, Projectile.s_maxProjectileScale);
				base.sprite.scale = new Vector3(num2, num2, num2);
				if (num2 != 1f)
				{
					if (base.specRigidbody != null)
					{
						base.specRigidbody.UpdateCollidersOnScale = true;
						base.specRigidbody.ForceRegenerate(null, null);
					}
					if (base.sprite.transform != this.m_transform)
					{
						base.sprite.transform.localPosition = Vector3.Scale(base.sprite.transform.localPosition, base.sprite.scale);
					}
					this.DoWallExitClipping(1f);
				}
				if (this.HasDefaultTint)
				{
					this.AdjustPlayerProjectileTint(this.DefaultTintColor, 0, 0f);
				}
			}
			if (this.shouldRotate && this.shouldFlipVertically)
			{
				base.sprite.FlipY = num < 270f && num > 90f;
			}
			if (this.shouldFlipHorizontally)
			{
				base.sprite.FlipX = num > 90f && num < 270f;
			}
		}
		if (base.specRigidbody != null && this.Owner is PlayerController)
		{
			base.specRigidbody.UpdateCollidersOnRotation = true;
			base.specRigidbody.UpdateCollidersOnScale = true;
		}
		if (this.isFakeBullet)
		{
			base.enabled = false;
			base.sprite.HeightOffGround = Projectile.CurrentProjectileDepth;
			base.sprite.UpdateZDepth();
			return;
		}
		if (base.specRigidbody == null)
		{
			UnityEngine.Debug.LogError("No speculative rigidbody found on projectile!", this);
		}
		if (GameManager.PVP_ENABLED && !this.TreatedAsNonProjectileForChallenge)
		{
			this.collidesWithPlayer = true;
		}
		if (this.collidesWithPlayer && this.Owner is AIActor && (this.Owner as AIActor).CompanionOwner != null)
		{
			this.collidesWithPlayer = false;
		}
		if (this.collidesWithProjectiles)
		{
			for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
			{
				base.specRigidbody.PixelColliders[i].CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);
			}
		}
		if (!this.collidesWithPlayer)
		{
			for (int j = 0; j < base.specRigidbody.PixelColliders.Count; j++)
			{
				base.specRigidbody.PixelColliders[j].CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox);
			}
		}
		if (!this.collidesWithEnemies)
		{
			for (int k = 0; k < base.specRigidbody.PixelColliders.Count; k++)
			{
				base.specRigidbody.PixelColliders[k].CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			}
		}
		if (this.Owner is PlayerController)
		{
			for (int l = 0; l < base.specRigidbody.PixelColliders.Count; l++)
			{
				base.specRigidbody.PixelColliders[l].CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyBulletBlocker);
			}
		}
		else if (this.Owner is AIActor && this.collidesWithEnemies && PassiveItem.IsFlagSetAtAll(typeof(BattleStandardItem)))
		{
			this.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
		}
		if (this.Owner is PlayerController)
		{
			this.PostprocessPlayerBullet();
		}
		if (base.specRigidbody.UpdateCollidersOnRotation)
		{
			base.specRigidbody.ForceRegenerate(null, null);
		}
		this.m_timeElapsed = 0f;
		this.LastPosition = this.m_transform.position;
		this.m_currentSpeed = this.baseData.speed;
		this.m_currentDirection = this.m_transform.right;
		if (!this.shouldRotate)
		{
			this.m_transform.rotation = Quaternion.identity;
		}
		if (this.CanKillBosses)
		{
			base.StartCoroutine(this.CheckIfBossKillShot());
		}
		if (!this.shouldRotate)
		{
			base.specRigidbody.IgnorePixelGrid = true;
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
		}
		if (this.angularVelocity != 0f)
		{
			this.angularVelocity = BraveUtility.RandomSign() * this.angularVelocity + UnityEngine.Random.Range(-this.angularVelocityVariance, this.angularVelocityVariance);
		}
		this.CheckBlackPhantomness();
	}

	// Token: 0x06008338 RID: 33592 RVA: 0x003598B0 File Offset: 0x00357AB0
	private void CheckBlackPhantomness()
	{
		if (this.CanBecomeBlackBullet && (this.ForceBlackBullet || (this.Owner is AIActor && (this.Owner as AIActor).IsBlackPhantom)))
		{
			this.BecomeBlackBullet();
		}
		else if (this.IsBlackBullet)
		{
			this.ReturnFromBlackBullet();
		}
	}

	// Token: 0x06008339 RID: 33593 RVA: 0x00359914 File Offset: 0x00357B14
	public int GetRadialBurstLimit(PlayerController source)
	{
		int num = int.MaxValue;
		for (int i = 0; i < this.AdditionalBurstLimits.Length; i++)
		{
			if (source.HasActiveBonusSynergy(this.AdditionalBurstLimits[i].RequiredSynergy, false))
			{
				num = Mathf.Min(num, this.AdditionalBurstLimits[i].limit);
			}
		}
		if (this.IsRadialBurstLimited && this.MaxRadialBurstLimit > -1)
		{
			num = Mathf.Min(num, this.MaxRadialBurstLimit);
		}
		return num;
	}

	// Token: 0x0600833A RID: 33594 RVA: 0x0035999C File Offset: 0x00357B9C
	public void CacheLayer(int targetLayer)
	{
		if (base.sprite == null)
		{
			return;
		}
		this.m_cachedLayer = base.sprite.gameObject.layer;
		base.gameObject.SetLayerRecursively(targetLayer);
	}

	// Token: 0x0600833B RID: 33595 RVA: 0x003599D4 File Offset: 0x00357BD4
	public void DecacheLayer()
	{
		if (base.sprite == null)
		{
			return;
		}
		base.gameObject.SetLayerRecursively(this.m_cachedLayer);
	}

	// Token: 0x0600833C RID: 33596 RVA: 0x003599FC File Offset: 0x00357BFC
	private void PostprocessPlayerBullet()
	{
		PlayerController playerController = this.Owner as PlayerController;
		int num = Mathf.FloorToInt(playerController.stats.GetStatValue(PlayerStats.StatType.AdditionalShotPiercing));
		if (this.PossibleSourceGun && this.PossibleSourceGun.gunClass == GunClass.SHOTGUN && playerController.HasActiveBonusSynergy(CustomSynergyType.SHOTGUN_SPEED, false))
		{
			this.baseData.speed *= 2f;
			this.baseData.force *= 3f;
			num++;
		}
		if (num > 0)
		{
			PierceProjModifier pierceProjModifier = base.GetComponent<PierceProjModifier>();
			if (pierceProjModifier == null)
			{
				pierceProjModifier = base.gameObject.AddComponent<PierceProjModifier>();
				pierceProjModifier.penetration = num;
				pierceProjModifier.penetratesBreakables = true;
				pierceProjModifier.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;
			}
			else
			{
				pierceProjModifier.penetration += num;
			}
		}
		int num2 = Mathf.FloorToInt(playerController.stats.GetStatValue(PlayerStats.StatType.AdditionalShotBounces));
		if (num2 > 0)
		{
			BounceProjModifier bounceProjModifier = base.GetComponent<BounceProjModifier>();
			if (bounceProjModifier == null)
			{
				bounceProjModifier = base.gameObject.AddComponent<BounceProjModifier>();
				bounceProjModifier.numberOfBounces = num2;
			}
			else
			{
				bounceProjModifier.numberOfBounces += num2;
			}
		}
	}

	// Token: 0x0600833D RID: 33597 RVA: 0x00359B34 File Offset: 0x00357D34
	public void AdjustPlayerProjectileTint(Color targetTintColor, int priority, float lerpTime = 0f)
	{
		if (priority > this.m_currentTintPriority || (priority == this.m_currentTintPriority && UnityEngine.Random.value < 0.5f))
		{
			this.m_currentTintPriority = priority;
			if (this.Owner is PlayerController)
			{
				this.ChangeTintColorShader(lerpTime, targetTintColor);
			}
		}
	}

	// Token: 0x0600833E RID: 33598 RVA: 0x00359B88 File Offset: 0x00357D88
	public void RemovePlayerOnlyModifiers()
	{
		HomingModifier component = base.GetComponent<HomingModifier>();
		if (component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x0600833F RID: 33599 RVA: 0x00359BB0 File Offset: 0x00357DB0
	public void MakeLookLikeEnemyBullet(bool applyScaleChanges = true)
	{
		if (base.specRigidbody && base.sprite && applyScaleChanges)
		{
			tk2dSpriteDefinition currentSpriteDef = base.sprite.GetCurrentSpriteDef();
			Bounds bounds = currentSpriteDef.GetBounds();
			float num = Mathf.Max(bounds.size.x, bounds.size.y);
			if (num < 0.5f)
			{
				float num2 = 0.5f / num;
				UnityEngine.Debug.Log(num + "|" + num2);
				base.sprite.scale = new Vector3(num2, num2, num2);
				if (num2 != 1f && base.specRigidbody != null)
				{
					base.specRigidbody.UpdateCollidersOnScale = true;
					base.specRigidbody.ForceRegenerate(null, null);
				}
			}
		}
		if (base.sprite && base.sprite.renderer)
		{
			Material sharedMaterial = base.sprite.renderer.sharedMaterial;
			base.sprite.usesOverrideMaterial = true;
			Material material = new Material(ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive"));
			material.SetTexture("_MainTex", sharedMaterial.GetTexture("_MainTex"));
			material.SetColor("_OverrideColor", new Color(1f, 1f, 1f, 1f));
			this.LerpMaterialGlow(material, 0f, 22f, 0.4f);
			material.SetFloat("_EmissiveColorPower", 8f);
			material.SetColor("_EmissiveColor", Color.red);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.red);
			base.sprite.renderer.material = material;
		}
	}

	// Token: 0x06008340 RID: 33600 RVA: 0x00359D98 File Offset: 0x00357F98
	private void HandleSparks(Vector2? overridePoint = null)
	{
		if (this.damageTypes == (this.damageTypes | CoreDamageTypes.Electric) && base.specRigidbody)
		{
			Vector2 vector = ((overridePoint == null) ? base.specRigidbody.UnitCenter : overridePoint.Value);
			Vector2 vector2 = ((this.m_lastSparksPoint == null) ? this.m_lastPosition.XY() : this.m_lastSparksPoint.Value);
			this.m_lastSparksPoint = new Vector2?(vector);
			float magnitude = (this.m_lastPosition.XY() - vector).magnitude;
			int num = (int)(magnitude * 6f);
			GlobalSparksDoer.DoLinearParticleBurst(Mathf.Max(1, num), vector2, vector, 360f, 5f, 0.5f, null, new float?(0.2f), new Color?(new Color(0.25f, 0.25f, 1f, 1f)), GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
		}
		if (this.CurseSparks && base.specRigidbody)
		{
			Vector2 vector3 = ((overridePoint == null) ? base.specRigidbody.UnitCenter : overridePoint.Value);
			Vector2 vector4 = ((this.m_lastSparksPoint == null) ? this.m_lastPosition.XY() : this.m_lastSparksPoint.Value);
			this.m_lastSparksPoint = new Vector2?(vector3);
			float magnitude2 = (this.m_lastPosition.XY() - vector3).magnitude;
			int num2 = (int)(magnitude2 * 3f);
			GlobalSparksDoer.DoLinearParticleBurst(Mathf.Max(1, num2), vector4, vector3, 360f, 5f, 0.5f, null, new float?(0.2f), new Color?(new Color(0.25f, 0.25f, 1f, 1f)), GlobalSparksDoer.SparksType.DARK_MAGICKS);
		}
	}

	// Token: 0x06008341 RID: 33601 RVA: 0x00359FA4 File Offset: 0x003581A4
	public virtual void Update()
	{
		tk2dBaseSprite sprite = base.sprite;
		bool flag = sprite;
		if (Time.frameCount != Projectile.m_cacheTick)
		{
			Projectile.m_cachedDungeon = ((!GameManager.Instance) ? null : GameManager.Instance.Dungeon);
			Projectile.m_cacheTick = Time.frameCount;
		}
		if (this.IsBlackBullet)
		{
			if (!this.ForceBlackBullet && this.Owner is AIActor && !(this.Owner as AIActor).IsBlackPhantom)
			{
				this.ReturnFromBlackBullet();
			}
			if (this.Owner && !(this.Owner is AIActor))
			{
				this.ReturnFromBlackBullet();
			}
		}
		if (this.m_isRamping && flag)
		{
			float currentRampHeight = this.m_currentRampHeight;
			if (this.m_rampTimer <= this.m_rampDuration)
			{
				this.m_currentRampHeight = Mathf.Lerp(this.m_startRampHeight, 0f, this.m_rampTimer / this.m_rampDuration);
			}
			else
			{
				this.m_currentRampHeight = 0f;
				this.m_isRamping = false;
			}
			sprite.HeightOffGround -= currentRampHeight - this.m_currentRampHeight;
			sprite.UpdateZDepthLater();
			float num = this.LocalDeltaTime;
			if (!(this.Owner is PlayerController))
			{
				num *= Projectile.EnemyBulletSpeedMultiplier;
			}
			this.m_rampTimer += num;
		}
		if (this.m_ignoreTileCollisionsTimer > 0f)
		{
			float num2 = this.LocalDeltaTime;
			if (!(this.Owner is PlayerController))
			{
				num2 *= Projectile.EnemyBulletSpeedMultiplier;
			}
			this.m_rampTimer += num2;
			this.m_ignoreTileCollisionsTimer = Mathf.Max(0f, this.m_ignoreTileCollisionsTimer - num2);
			if (this.m_ignoreTileCollisionsTimer <= 0f)
			{
				base.specRigidbody.CollideWithTileMap = true;
			}
		}
		this.HandleSparks(null);
		if (!this.IsBulletScript)
		{
			this.HandleRange();
			if (!this.ManualControl)
			{
				if (this.PreMoveModifiers != null)
				{
					this.PreMoveModifiers(this);
				}
				if (this.OverrideMotionModule != null && !this.m_usesNormalMoveRegardless)
				{
					this.OverrideMotionModule.Move(this, this.m_transform, base.sprite, base.specRigidbody, ref this.m_timeElapsed, ref this.m_currentDirection, this.Inverted, this.shouldRotate);
					this.LastVelocity = base.specRigidbody.Velocity;
				}
				else
				{
					this.Move();
				}
			}
			base.specRigidbody.Velocity *= this.LocalTimeScale;
			if (!(this.Owner is PlayerController))
			{
				base.specRigidbody.Velocity *= Projectile.EnemyBulletSpeedMultiplier;
			}
			this.DoModifyVelocity();
		}
		Vector2 vector = this.m_transform.position;
		if (this.m_isInWall && Projectile.m_cachedDungeon.data.CheckInBounds((int)vector.x, (int)vector.y))
		{
			CellData cellData = Projectile.m_cachedDungeon.data[(int)vector.x, (int)vector.y];
			if (cellData != null && cellData.type != CellType.WALL)
			{
				this.m_isInWall = false;
			}
		}
		if ((this.shouldFlipHorizontally || this.shouldFlipVertically) && flag)
		{
			if (this.shouldFlipHorizontally && this.shouldRotate && this.shouldFlipVertically)
			{
				bool flag2 = this.Direction.x < 0f;
				sprite.FlipX = flag2;
				sprite.FlipY = flag2;
			}
			else if (this.shouldFlipHorizontally)
			{
				sprite.FlipX = this.Direction.x < 0f;
			}
			else if (this.shouldRotate && this.shouldFlipVertically)
			{
				sprite.FlipY = this.Direction.x < 0f;
			}
		}
		if (Projectile.m_cachedDungeon != null && !Projectile.m_cachedDungeon.data.CheckInBounds((int)vector.x, (int)vector.y))
		{
			this.m_outOfBoundsCounter += BraveTime.DeltaTime;
			if (this.m_outOfBoundsCounter > 5f)
			{
				base.gameObject.SetActive(false);
				SpawnManager.Despawn(base.gameObject);
			}
		}
		else
		{
			this.m_outOfBoundsCounter = 0f;
		}
		if (this.damageTypes != CoreDamageTypes.None)
		{
			this.HandleGoopChecks();
		}
		if (this.m_isExitClippingTiles && this.m_distanceElapsed > this.m_exitClippingDistance)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreTileCollision = (SpeculativeRigidbody.OnPreTileCollisionDelegate)Delegate.Remove(specRigidbody.OnPreTileCollision, new SpeculativeRigidbody.OnPreTileCollisionDelegate(this.PreTileCollisionExitClipping));
			this.m_isExitClippingTiles = false;
		}
		if (this.OnPostUpdate != null)
		{
			this.OnPostUpdate(this);
		}
	}

	// Token: 0x06008342 RID: 33602 RVA: 0x0035A4B4 File Offset: 0x003586B4
	protected virtual void DoModifyVelocity()
	{
		if (this.ModifyVelocity != null)
		{
			base.specRigidbody.Velocity = this.ModifyVelocity(base.specRigidbody.Velocity);
			if (base.specRigidbody.Velocity != Vector2.zero)
			{
				this.m_currentDirection = base.specRigidbody.Velocity.normalized;
			}
		}
	}

	// Token: 0x06008343 RID: 33603 RVA: 0x0035A520 File Offset: 0x00358720
	protected void HandleGoopChecks()
	{
		IntVector2 intVector = base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round);
		if (!Projectile.m_cachedDungeon.data.CheckInBounds(intVector))
		{
			return;
		}
		RoomHandler absoluteRoomFromPosition = Projectile.m_cachedDungeon.data.GetAbsoluteRoomFromPosition(intVector);
		List<DeadlyDeadlyGoopManager> roomGoops = absoluteRoomFromPosition.RoomGoops;
		if (roomGoops != null)
		{
			for (int i = 0; i < roomGoops.Count; i++)
			{
				roomGoops[i].ProcessProjectile(this);
			}
		}
	}

	// Token: 0x06008344 RID: 33604 RVA: 0x0035A598 File Offset: 0x00358798
	public virtual void SetNewShooter(SpeculativeRigidbody newShooter)
	{
		if (base.specRigidbody)
		{
			base.specRigidbody.DeregisterSpecificCollisionException(this.m_shooter);
			if (!this.allowSelfShooting)
			{
				base.specRigidbody.RegisterSpecificCollisionException(newShooter);
			}
		}
		this.m_shooter = newShooter;
	}

	// Token: 0x06008345 RID: 33605 RVA: 0x0035A5E4 File Offset: 0x003587E4
	public void UpdateSpeed()
	{
		this.m_currentSpeed = this.baseData.speed;
	}

	// Token: 0x06008346 RID: 33606 RVA: 0x0035A5F8 File Offset: 0x003587F8
	public void UpdateCollisionMask()
	{
		if (!base.specRigidbody)
		{
			return;
		}
		if (this.collidesWithProjectiles)
		{
			for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
			{
				base.specRigidbody.PixelColliders[i].CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.Projectile);
			}
		}
		else
		{
			for (int j = 0; j < base.specRigidbody.PixelColliders.Count; j++)
			{
				base.specRigidbody.PixelColliders[j].CollisionLayerCollidableOverride &= ~CollisionMask.LayerToMask(CollisionLayer.Projectile);
			}
		}
		if (!this.collidesWithEnemies)
		{
			for (int k = 0; k < base.specRigidbody.PixelColliders.Count; k++)
			{
				base.specRigidbody.PixelColliders[k].CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			}
		}
		else
		{
			for (int l = 0; l < base.specRigidbody.PixelColliders.Count; l++)
			{
				base.specRigidbody.PixelColliders[l].CollisionLayerIgnoreOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			}
		}
		if (!this.collidesWithPlayer)
		{
			for (int m = 0; m < base.specRigidbody.PixelColliders.Count; m++)
			{
				base.specRigidbody.PixelColliders[m].CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox);
			}
		}
		else
		{
			for (int n = 0; n < base.specRigidbody.PixelColliders.Count; n++)
			{
				base.specRigidbody.PixelColliders[n].CollisionLayerIgnoreOverride &= ~CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox);
			}
		}
		if (this.Owner is PlayerController)
		{
			for (int num = 0; num < base.specRigidbody.PixelColliders.Count; num++)
			{
				base.specRigidbody.PixelColliders[num].CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyBulletBlocker);
			}
		}
		else
		{
			for (int num2 = 0; num2 < base.specRigidbody.PixelColliders.Count; num2++)
			{
				base.specRigidbody.PixelColliders[num2].CollisionLayerIgnoreOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyBulletBlocker);
			}
		}
	}

	// Token: 0x06008347 RID: 33607 RVA: 0x0035A898 File Offset: 0x00358A98
	public void SendInDirection(Vector2 dirVec, bool resetDistance, bool updateRotation = true)
	{
		if (this.shouldRotate && updateRotation)
		{
			this.m_transform.eulerAngles = new Vector3(0f, 0f, dirVec.ToAngle());
		}
		this.m_currentDirection = dirVec.normalized;
		base.specRigidbody.Velocity = this.m_currentDirection * this.m_currentSpeed * this.LocalTimeScale;
		if (this.OverrideMotionModule != null)
		{
			this.OverrideMotionModule.SentInDirection(this.baseData, this.m_transform, base.sprite, base.specRigidbody, ref this.m_timeElapsed, ref this.m_currentDirection, this.shouldRotate, dirVec, resetDistance, updateRotation);
		}
		if (resetDistance)
		{
			this.ResetDistance();
		}
	}

	// Token: 0x06008348 RID: 33608 RVA: 0x0035A95C File Offset: 0x00358B5C
	public void ResetDistance()
	{
		this.m_distanceElapsed = 0f;
	}

	// Token: 0x06008349 RID: 33609 RVA: 0x0035A96C File Offset: 0x00358B6C
	public float GetElapsedDistance()
	{
		return this.m_distanceElapsed;
	}

	// Token: 0x0600834A RID: 33610 RVA: 0x0035A974 File Offset: 0x00358B74
	public void Reflected()
	{
		if (this.OnReflected != null)
		{
			this.OnReflected(this);
		}
	}

	// Token: 0x0600834B RID: 33611 RVA: 0x0035A990 File Offset: 0x00358B90
	public IEnumerator CheckIfBossKillShot()
	{
		RoomHandler currentPlayerRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		List<AIActor> enemiesInRoom = currentPlayerRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (enemiesInRoom != null)
		{
			AIActor currentBoss = null;
			if (enemiesInRoom != null)
			{
				for (int i = 0; i < enemiesInRoom.Count; i++)
				{
					if (enemiesInRoom[i] && enemiesInRoom[i].healthHaver.IsBoss)
					{
						currentBoss = enemiesInRoom[i];
					}
				}
			}
			while (!this.m_hasImpactedObject)
			{
				bool shouldBreak = true;
				if (currentBoss != null && currentBoss && currentBoss.healthHaver && !currentBoss.healthHaver.IsDead && currentBoss.healthHaver.GetCurrentHealth() <= this.ModifiedDamage)
				{
					shouldBreak = false;
					int mask = CollisionLayerMatrix.GetMask(CollisionLayer.Projectile);
					RaycastResult raycastResult;
					if (PhysicsEngine.Instance.RaycastWithIgnores(base.specRigidbody.UnitCenter, this.m_currentDirection, this.baseData.range, out raycastResult, true, true, mask, null, false, null, new SpeculativeRigidbody[]
					{
						base.specRigidbody,
						this.Owner.specRigidbody
					}) && raycastResult.SpeculativeRigidbody == currentBoss.specRigidbody && raycastResult.Distance < this.baseData.range - this.m_distanceElapsed)
					{
						GameUIRoot.Instance.TriggerBossKillCam(this, currentBoss.specRigidbody);
					}
				}
				if (shouldBreak)
				{
					break;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600834C RID: 33612 RVA: 0x0035A9AC File Offset: 0x00358BAC
	public void HandlePassthroughHitEffects(Vector3 point)
	{
		if (this.hitEffects != null)
		{
			this.hitEffects.HandleEnemyImpact(point, 0f, null, Vector2.zero, base.specRigidbody.Velocity, false, false);
		}
	}

	// Token: 0x0600834D RID: 33613 RVA: 0x0035A9E0 File Offset: 0x00358BE0
	public void Ramp(float startHeightOffset, float duration)
	{
		if (!base.sprite)
		{
			return;
		}
		this.m_isRamping = true;
		this.m_rampDuration = duration;
		this.m_rampTimer = 0f;
		this.m_startRampHeight = startHeightOffset;
		float currentRampHeight = this.m_currentRampHeight;
		this.m_currentRampHeight = this.m_startRampHeight;
		base.sprite.HeightOffGround -= currentRampHeight - this.m_currentRampHeight;
		base.sprite.UpdateZDepthLater();
	}

	// Token: 0x0600834E RID: 33614 RVA: 0x0035AA58 File Offset: 0x00358C58
	public virtual float EstimatedTimeToTarget(Vector2 targetPoint, Vector2? overridePos = null)
	{
		Vector2 vector = ((overridePos == null) ? base.specRigidbody.UnitCenter : overridePos.Value);
		return Vector2.Distance(vector, targetPoint) / this.Speed;
	}

	// Token: 0x0600834F RID: 33615 RVA: 0x0035AA98 File Offset: 0x00358C98
	public virtual Vector2 GetPredictedTargetPosition(Vector2 targetCenter, Vector2 targetVelocity, Vector2? overridePos = null, float? overrideProjectileSpeed = null)
	{
		Vector2 vector = ((overridePos == null) ? base.specRigidbody.UnitCenter : overridePos.Value);
		float num = ((overrideProjectileSpeed == null) ? this.baseData.speed : overrideProjectileSpeed.Value);
		return BraveMathCollege.GetPredictedPosition(targetCenter, targetVelocity, vector, num);
	}

	// Token: 0x06008350 RID: 33616 RVA: 0x0035AAF8 File Offset: 0x00358CF8
	public void RemoveBulletScriptControl()
	{
		if (this.braveBulletScript)
		{
			if (this.braveBulletScript.bullet != null)
			{
				this.braveBulletScript.bullet.DontDestroyGameObject = true;
			}
			this.braveBulletScript.RemoveBullet();
			this.braveBulletScript.enabled = false;
			this.BulletScriptSettings.surviveRigidbodyCollisions = false;
			this.BulletScriptSettings.surviveTileCollisions = false;
			this.IsBulletScript = false;
		}
	}

	// Token: 0x06008351 RID: 33617 RVA: 0x0035AB6C File Offset: 0x00358D6C
	public void IgnoreTileCollisionsFor(float time)
	{
		base.specRigidbody.CollideWithTileMap = false;
		this.m_ignoreTileCollisionsTimer = time;
	}

	// Token: 0x06008352 RID: 33618 RVA: 0x0035AB84 File Offset: 0x00358D84
	public void DoWallExitClipping(float pixelMultiplier = 1f)
	{
		this.m_isExitClippingTiles = true;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreTileCollision = (SpeculativeRigidbody.OnPreTileCollisionDelegate)Delegate.Combine(specRigidbody.OnPreTileCollision, new SpeculativeRigidbody.OnPreTileCollisionDelegate(this.PreTileCollisionExitClipping));
		PixelCollider primaryPixelCollider = base.specRigidbody.PrimaryPixelCollider;
		this.m_exitClippingDistance = pixelMultiplier * Mathf.Max(primaryPixelCollider.UnitWidth, primaryPixelCollider.UnitHeight);
	}

	// Token: 0x06008353 RID: 33619 RVA: 0x0035ABE4 File Offset: 0x00358DE4
	protected virtual void Move()
	{
		this.m_timeElapsed += this.LocalDeltaTime;
		if (this.angularVelocity != 0f)
		{
			this.m_transform.RotateAround(this.m_transform.position.XY(), Vector3.forward, this.angularVelocity * this.LocalDeltaTime);
		}
		if (this.baseData.UsesCustomAccelerationCurve)
		{
			float num = Mathf.Clamp01((this.m_timeElapsed - this.baseData.IgnoreAccelCurveTime) / this.baseData.CustomAccelerationCurveDuration);
			this.m_currentSpeed = this.baseData.AccelerationCurve.Evaluate(num) * this.baseData.speed;
		}
		base.specRigidbody.Velocity = this.m_currentDirection * this.m_currentSpeed;
		this.m_currentSpeed *= 1f - this.baseData.damping * this.LocalDeltaTime;
		this.LastVelocity = base.specRigidbody.Velocity;
	}

	// Token: 0x06008354 RID: 33620 RVA: 0x0035ACF0 File Offset: 0x00358EF0
	protected virtual void HandleRange()
	{
		this.m_distanceElapsed += Vector3.Distance(this.m_lastPosition, this.m_transform.position);
		this.LastPosition = this.m_transform.position;
		if (!this.SkipDistanceElapsedCheck && this.m_distanceElapsed > this.baseData.range)
		{
			this.DieInAir(false, true, true, false);
		}
	}

	// Token: 0x06008355 RID: 33621 RVA: 0x0035AD5C File Offset: 0x00358F5C
	public void ForceDestruction()
	{
		this.HandleDestruction(null, true, true);
	}

	// Token: 0x06008356 RID: 33622 RVA: 0x0035AD68 File Offset: 0x00358F68
	protected virtual void HandleDestruction(CollisionData lcr, bool allowActorSpawns = true, bool allowProjectileSpawns = true)
	{
		this.HandleSparks((lcr == null) ? null : new Vector2?(lcr.Contact));
		if (this.hitEffects != null && this.hitEffects.HasProjectileDeathVFX)
		{
			this.hitEffects.HandleProjectileDeathVFX((lcr == null || this.hitEffects.CenterDeathVFXOnProjectile) ? base.specRigidbody.UnitCenter : lcr.Contact, 0f, null, (lcr == null) ? Vector2.zero : lcr.Normal, base.specRigidbody.Velocity, false);
		}
		if (this.braveBulletScript)
		{
			if (lcr == null)
			{
				this.braveBulletScript.HandleBulletDestruction(Bullet.DestroyType.DieInAir, null, allowProjectileSpawns);
			}
			else if (lcr.OtherRigidbody)
			{
				this.braveBulletScript.HandleBulletDestruction(Bullet.DestroyType.HitRigidbody, lcr.OtherRigidbody, allowProjectileSpawns);
			}
			else
			{
				this.braveBulletScript.HandleBulletDestruction(Bullet.DestroyType.HitTile, null, allowProjectileSpawns);
			}
		}
		if (allowProjectileSpawns && this.baseData.onDestroyBulletScript != null && !this.baseData.onDestroyBulletScript.IsNull)
		{
			if (lcr != null)
			{
				Vector2 vector = base.specRigidbody.UnitCenter;
				if (!lcr.IsInverse)
				{
					vector += PhysicsEngine.PixelToUnit(lcr.NewPixelsToMove);
				}
				SpawnManager.SpawnBulletScript(this.Owner, this.baseData.onDestroyBulletScript, new Vector2?(vector + lcr.Normal.normalized * PhysicsEngine.PixelToUnit(2)), new Vector2?(lcr.Normal), this.collidesWithEnemies, this.OwnerName);
			}
			else
			{
				SpawnManager.SpawnBulletScript(this.Owner, this.baseData.onDestroyBulletScript, new Vector2?(this.m_transform.position), new Vector2?(Vector2.up), this.collidesWithEnemies, this.OwnerName);
			}
		}
		if (!string.IsNullOrEmpty(this.spawnEnemyGuidOnDeath) && allowActorSpawns)
		{
			Vector2 unitCenter = base.specRigidbody.UnitCenter;
			IntVector2 intVector = unitCenter.ToIntVector2(VectorConversions.Floor);
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(intVector);
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.spawnEnemyGuidOnDeath);
			AIActor aiactor = AIActor.Spawn(orLoadByGuid, intVector, roomFromPosition, true, AIActor.AwakenAnimationType.Default, true);
			if (aiactor.specRigidbody)
			{
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
			}
			if (this.IsBlackBullet && aiactor)
			{
				aiactor.ForceBlackPhantom = true;
			}
		}
		if (this.OnDestruction != null)
		{
			this.OnDestruction(this);
		}
		switch (this.DestroyMode)
		{
		case Projectile.ProjectileDestroyMode.Destroy:
			if (!SpawnManager.Despawn(base.gameObject, this.m_spawnPool))
			{
				base.gameObject.SetActive(false);
			}
			break;
		case Projectile.ProjectileDestroyMode.DestroyComponent:
		{
			base.specRigidbody.Velocity = Vector2.zero;
			base.specRigidbody.DeregisterSpecificCollisionException(this.Shooter);
			for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
			{
				base.specRigidbody.PixelColliders[i].IsTrigger = true;
			}
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Remove(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
			specRigidbody3.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody3.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
			if (this.m_isExitClippingTiles)
			{
				SpeculativeRigidbody specRigidbody4 = base.specRigidbody;
				specRigidbody4.OnPreTileCollision = (SpeculativeRigidbody.OnPreTileCollisionDelegate)Delegate.Remove(specRigidbody4.OnPreTileCollision, new SpeculativeRigidbody.OnPreTileCollisionDelegate(this.PreTileCollisionExitClipping));
			}
			UnityEngine.Object.Destroy(this);
			break;
		}
		case Projectile.ProjectileDestroyMode.BecomeDebris:
		{
			base.specRigidbody.Velocity = Vector2.zero;
			base.specRigidbody.DeregisterSpecificCollisionException(this.Shooter);
			for (int j = 0; j < base.specRigidbody.PixelColliders.Count; j++)
			{
				base.specRigidbody.PixelColliders[j].IsTrigger = true;
			}
			SpeculativeRigidbody specRigidbody5 = base.specRigidbody;
			specRigidbody5.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Remove(specRigidbody5.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			SpeculativeRigidbody specRigidbody6 = base.specRigidbody;
			specRigidbody6.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody6.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			SpeculativeRigidbody specRigidbody7 = base.specRigidbody;
			specRigidbody7.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody7.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
			if (this.m_isExitClippingTiles)
			{
				SpeculativeRigidbody specRigidbody8 = base.specRigidbody;
				specRigidbody8.OnPreTileCollision = (SpeculativeRigidbody.OnPreTileCollisionDelegate)Delegate.Remove(specRigidbody8.OnPreTileCollision, new SpeculativeRigidbody.OnPreTileCollisionDelegate(this.PreTileCollisionExitClipping));
			}
			UnityEngine.Object.Destroy(base.GetComponentInChildren<SimpleSpriteRotator>());
			DebrisObject debrisObject = this.BecomeDebris((lcr != null) ? lcr.Normal.ToVector3ZUp(0.1f) : Vector3.zero, 0.5f);
			if (this.OnBecameDebris != null)
			{
				this.OnBecameDebris(debrisObject);
			}
			if (this.OnBecameDebrisGrounded != null)
			{
				DebrisObject debrisObject2 = debrisObject;
				debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, this.OnBecameDebrisGrounded);
			}
			UnityEngine.Object.Destroy(this);
			break;
		}
		}
		if (GameManager.AUDIO_ENABLED && !string.IsNullOrEmpty(this.onDestroyEventName))
		{
			AkSoundEngine.PostEvent(this.onDestroyEventName, base.gameObject);
		}
	}

	// Token: 0x06008357 RID: 33623 RVA: 0x0035B344 File Offset: 0x00359544
	public DebrisObject BecomeDebris(Vector3 force, float height)
	{
		DebrisObject orAddComponent = base.gameObject.GetOrAddComponent<DebrisObject>();
		orAddComponent.angularVelocity = (float)((!this.shouldRotate) ? 0 : 45);
		orAddComponent.angularVelocityVariance = (float)((!this.shouldRotate) ? 0 : 20);
		orAddComponent.decayOnBounce = 0.5f;
		orAddComponent.bounceCount = 1;
		orAddComponent.canRotate = this.shouldRotate;
		orAddComponent.shouldUseSRBMotion = true;
		orAddComponent.AssignFinalWorldDepth(-0.5f);
		orAddComponent.sprite = base.specRigidbody.sprite;
		orAddComponent.animatePitFall = true;
		orAddComponent.Trigger(force, height, 1f);
		return orAddComponent;
	}

	// Token: 0x06008358 RID: 33624 RVA: 0x0035B3E8 File Offset: 0x003595E8
	public void DieInAir(bool suppressInAirEffects = false, bool allowActorSpawns = true, bool allowProjectileSpawns = true, bool killedEarly = false)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (this.m_hasDiedInAir)
		{
			return;
		}
		this.m_hasDiedInAir = true;
		BeamController component = base.GetComponent<BeamController>();
		if (component)
		{
			component.DestroyBeam();
		}
		SpawnProjModifier component2 = base.GetComponent<SpawnProjModifier>();
		if (component2 != null && allowProjectileSpawns && ((component2.spawnProjectilesOnCollision && component2.spawnOnObjectCollisions) || component2.spawnProjecitlesOnDieInAir))
		{
			component2.SpawnCollisionProjectiles(this.m_transform.position.XY(), base.specRigidbody.Velocity.normalized, null, false);
		}
		ExplosiveModifier component3 = base.GetComponent<ExplosiveModifier>();
		if (component3 != null)
		{
			component3.Explode(Vector2.zero, this.ignoreDamageCaps, null);
		}
		if (!suppressInAirEffects)
		{
			this.HandleHitEffectsMidair(killedEarly);
		}
		this.HandleDestruction(null, allowActorSpawns, allowProjectileSpawns);
	}

	// Token: 0x06008359 RID: 33625 RVA: 0x0035B4D0 File Offset: 0x003596D0
	public void ChangeColor(float time, Color color)
	{
		if (!base.sprite)
		{
			return;
		}
		if (this.Owner is PlayerController && base.sprite.renderer && base.sprite.renderer.material.HasProperty("_VertexColor"))
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.SetFloat("_VertexColor", 1f);
		}
		if (time == 0f)
		{
			base.sprite.color = color;
		}
		else
		{
			base.StartCoroutine(this.ChangeColorCR(time, color));
		}
	}

	// Token: 0x0600835A RID: 33626 RVA: 0x0035B588 File Offset: 0x00359788
	private IEnumerator ChangeColorCR(float time, Color color)
	{
		float timer = 0f;
		while (timer < time)
		{
			base.sprite.color = Color.Lerp(Color.white, color, timer / time);
			timer += this.LocalDeltaTime;
			yield return null;
		}
		base.sprite.color = color;
		yield break;
	}

	// Token: 0x0600835B RID: 33627 RVA: 0x0035B5B4 File Offset: 0x003597B4
	public void ChangeTintColorShader(float time, Color color)
	{
		if (!base.sprite)
		{
			return;
		}
		base.sprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
		Material material = base.sprite.renderer.material;
		bool flag = material.HasProperty("_EmissivePower");
		float num = 0f;
		float num2 = 0f;
		if (flag)
		{
			num = material.GetFloat("_EmissivePower");
			num2 = material.GetFloat("_EmissiveColorPower");
		}
		Shader shader;
		if (!flag)
		{
			shader = ShaderCache.Acquire("tk2d/CutoutVertexColorTintableTilted");
		}
		else
		{
			shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
		}
		if (base.sprite.renderer.material.shader != shader)
		{
			base.sprite.renderer.material.shader = shader;
			base.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
			if (flag)
			{
				base.sprite.renderer.material.SetFloat("_EmissivePower", num);
				base.sprite.renderer.material.SetFloat("_EmissiveColorPower", num2);
			}
		}
		if (time == 0f)
		{
			base.sprite.renderer.sharedMaterial.SetColor("_OverrideColor", color);
		}
		else
		{
			base.StartCoroutine(this.ChangeTintColorCR(time, color));
		}
	}

	// Token: 0x0600835C RID: 33628 RVA: 0x0035B714 File Offset: 0x00359914
	private IEnumerator ChangeTintColorCR(float time, Color color)
	{
		float timer = 0f;
		Material targetMaterial = base.sprite.renderer.sharedMaterial;
		while (timer < time)
		{
			targetMaterial.SetColor("_OverrideColor", Color.Lerp(Color.white, color, timer / time));
			timer += this.LocalDeltaTime;
			yield return null;
		}
		targetMaterial.SetColor("_OverrideColor", color);
		yield break;
	}

	// Token: 0x0600835D RID: 33629 RVA: 0x0035B740 File Offset: 0x00359940
	protected void HandleWallDecals(CollisionData lcr, Transform parent)
	{
		if (lcr.Normal.y >= 0f)
		{
			return;
		}
		VFXPool vfxpool = null;
		if (this.wallDecals != null && this.wallDecals.effects.Length > 0)
		{
			for (int i = 0; i < this.wallDecals.effects.Length; i++)
			{
				for (int j = 0; j < this.wallDecals.effects[i].effects.Length; j++)
				{
					this.wallDecals.effects[i].effects[j].orphaned = false;
					this.wallDecals.effects[i].effects[j].destructible = true;
				}
			}
			vfxpool = this.wallDecals;
		}
		else
		{
			DamageTypeEffectDefinition definitionForType = GameManager.Instance.Dungeon.damageTypeEffectMatrix.GetDefinitionForType(this.damageTypes);
			if (definitionForType != null)
			{
				vfxpool = definitionForType.wallDecals;
			}
		}
		if (vfxpool != null)
		{
			float num = UnityEngine.Random.value * 0.5f - 0.25f;
			Vector3 vector = lcr.Contact.ToVector3ZUp(-0.5f);
			vector.y += num;
			vfxpool.SpawnAtPosition(vector, 0f, parent, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), new float?(0.75f + num), false, new VFXComplex.SpawnMethod(SpawnManager.SpawnDecal), null, false);
		}
	}

	// Token: 0x0600835E RID: 33630 RVA: 0x0035B8C0 File Offset: 0x00359AC0
	protected virtual void OnPreTileCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, PhysicsEngine.Tile tile, PixelCollider otherPixelCollider)
	{
		if (this.PenetratesInternalWalls)
		{
			IntVector2 position = tile.Position;
			CellData cellData = GameManager.Instance.Dungeon.data[position];
			if (cellData == null || cellData.isRoomInternal)
			{
				if (!this.m_isInWall)
				{
					CollisionData collisionData = CollisionData.Pool.Allocate();
					collisionData.Normal = BraveUtility.GetMajorAxis(this.m_transform.position.XY() - tile.Position.ToCenterVector2()).normalized;
					collisionData.Contact = tile.Position.ToCenterVector2() + collisionData.Normal / 2f;
					this.HandleHitEffectsTileMap(collisionData, false);
					CollisionData.Pool.Free(ref collisionData);
				}
				this.m_isInWall = true;
				PhysicsEngine.SkipCollision = true;
			}
		}
	}

	// Token: 0x0600835F RID: 33631 RVA: 0x0035B9A0 File Offset: 0x00359BA0
	private void PreTileCollisionExitClipping(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, PhysicsEngine.Tile tile, PixelCollider tilePixelCollider)
	{
		if (!GameManager.HasInstance || GameManager.Instance.Dungeon == null)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		int x = tile.Position.x;
		int y = tile.Position.y;
		Vector2 velocity = myRigidbody.Velocity;
		if (velocity.y > 0f && data.isFaceWallHigher(x, y))
		{
			return;
		}
		if (velocity.y < 0f && data.hasTopWall(x, y))
		{
			return;
		}
		if (velocity.x < 0f && data.isLeftSideWall(x, y))
		{
			return;
		}
		if (velocity.x > 0f && data.isRightSideWall(x, y))
		{
			return;
		}
		PhysicsEngine.SkipCollision = true;
	}

	// Token: 0x06008360 RID: 33632 RVA: 0x0035BA88 File Offset: 0x00359C88
	protected virtual void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
	{
		if (otherRigidbody == this.m_shooter && !this.allowSelfShooting)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (otherRigidbody.gameActor != null && otherRigidbody.gameActor is PlayerController && (!this.collidesWithPlayer || (otherRigidbody.gameActor as PlayerController).IsGhost || (otherRigidbody.gameActor as PlayerController).IsEthereal))
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (otherRigidbody.aiActor)
		{
			if (this.Owner is PlayerController && !otherRigidbody.aiActor.IsNormalEnemy)
			{
				PhysicsEngine.SkipCollision = true;
				return;
			}
			if (this.Owner is AIActor && !this.collidesWithEnemies && otherRigidbody.aiActor.IsNormalEnemy && !otherRigidbody.aiActor.HitByEnemyBullets)
			{
				PhysicsEngine.SkipCollision = true;
				return;
			}
		}
		if (!GameManager.PVP_ENABLED && this.Owner is PlayerController && otherRigidbody.GetComponent<PlayerController>() != null && !this.allowSelfShooting)
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
		if (GameManager.Instance.InTutorial)
		{
			PlayerController component = otherRigidbody.GetComponent<PlayerController>();
			if (component)
			{
				if (component.spriteAnimator.QueryInvulnerabilityFrame())
				{
					GameManager.BroadcastRoomTalkDoerFsmEvent("playerDodgedBullet");
				}
				else if (component.IsDodgeRolling)
				{
					GameManager.BroadcastRoomTalkDoerFsmEvent("playerAlmostDodgedBullet");
				}
				else
				{
					GameManager.BroadcastRoomTalkDoerFsmEvent("playerDidNotDodgeBullet");
				}
			}
		}
		if (otherRigidbody.healthHaver != null && otherRigidbody.healthHaver.spriteAnimator != null && otherCollider.CollisionLayer == CollisionLayer.PlayerHitBox && otherRigidbody.spriteAnimator.QueryInvulnerabilityFrame())
		{
			PhysicsEngine.SkipCollision = true;
			base.StartCoroutine(this.HandlePostInvulnerabilityFrameExceptions(otherRigidbody));
			return;
		}
		if (this.collidesWithProjectiles && this.collidesOnlyWithPlayerProjectiles && otherRigidbody.projectile && !(otherRigidbody.projectile.Owner is PlayerController))
		{
			PhysicsEngine.SkipCollision = true;
			return;
		}
	}

	// Token: 0x06008361 RID: 33633 RVA: 0x0035BCCC File Offset: 0x00359ECC
	public void ForceCollision(SpeculativeRigidbody otherRigidbody, LinearCastResult lcr)
	{
		CollisionData collisionData = CollisionData.Pool.Allocate();
		collisionData.SetAll(lcr);
		collisionData.OtherRigidbody = otherRigidbody;
		collisionData.OtherPixelCollider = otherRigidbody.PrimaryPixelCollider;
		collisionData.MyRigidbody = base.specRigidbody;
		collisionData.MyPixelCollider = base.specRigidbody.PrimaryPixelCollider;
		collisionData.Normal = (base.specRigidbody.UnitCenter - otherRigidbody.UnitCenter).normalized;
		collisionData.Contact = (otherRigidbody.UnitCenter + base.specRigidbody.UnitCenter) / 2f;
		this.OnRigidbodyCollision(collisionData);
		CollisionData.Pool.Free(ref collisionData);
	}

	// Token: 0x06008362 RID: 33634 RVA: 0x0035BD78 File Offset: 0x00359F78
	protected virtual void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (base.specRigidbody.IsGhostCollisionException(rigidbodyCollision.OtherRigidbody))
		{
			return;
		}
		GameObject gameObject = rigidbodyCollision.OtherRigidbody.gameObject;
		SpeculativeRigidbody otherRigidbody = rigidbodyCollision.OtherRigidbody;
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		bool flag;
		Projectile.HandleDamageResult handleDamageResult = this.HandleDamage(rigidbodyCollision.OtherRigidbody, rigidbodyCollision.OtherPixelCollider, out flag, component, false);
		bool flag2 = handleDamageResult != Projectile.HandleDamageResult.NO_HEALTH;
		if (this.braveBulletScript && this.braveBulletScript.bullet != null && this.BulletScriptSettings.surviveTileCollisions && !flag2 && rigidbodyCollision.OtherPixelCollider.CollisionLayer == CollisionLayer.HighObstacle)
		{
			if (!otherRigidbody.minorBreakable)
			{
				this.braveBulletScript.bullet.ManualControl = true;
				this.braveBulletScript.bullet.Position = base.specRigidbody.UnitCenter;
				PhysicsEngine.PostSliceVelocity = new Vector2?(Vector2.zero);
			}
			return;
		}
		this.HandleSparks(new Vector2?(rigidbodyCollision.Contact));
		if (flag2)
		{
			this.m_hasImpactedEnemy = true;
			if (this.OnHitEnemy != null)
			{
				this.OnHitEnemy(this, rigidbodyCollision.OtherRigidbody, flag);
			}
		}
		else if (ChallengeManager.CHALLENGE_MODE_ACTIVE && (otherRigidbody.GetComponent<BeholsterBounceRocket>() || otherRigidbody.healthHaver || otherRigidbody.GetComponent<BashelliskBodyPickupController>() || otherRigidbody.projectile))
		{
			this.m_hasImpactedEnemy = true;
		}
		PierceProjModifier pierceProjModifier = base.GetComponent<PierceProjModifier>();
		BounceProjModifier bounceProjModifier = base.GetComponent<BounceProjModifier>();
		if (this.m_hasImpactedEnemy && pierceProjModifier && otherRigidbody.healthHaver && otherRigidbody.healthHaver.IsBoss)
		{
			bool flag3 = pierceProjModifier.HandleBossImpact();
			if (flag3)
			{
				bounceProjModifier = null;
				pierceProjModifier = null;
			}
		}
		if (base.GetComponent<KeyProjModifier>())
		{
			Chest component2 = otherRigidbody.GetComponent<Chest>();
			if (component2 && component2.IsLocked && component2.ChestIdentifier != Chest.SpecialChestIdentifier.RAT)
			{
				component2.ForceUnlock();
			}
		}
		MinorBreakable minorBreakable = otherRigidbody.minorBreakable;
		MajorBreakable majorBreakable = otherRigidbody.majorBreakable;
		if (majorBreakable != null)
		{
			float num = 1f;
			if (((this.m_shooter != null && this.m_shooter.aiActor != null) || this.m_owner is AIActor) && majorBreakable.InvulnerableToEnemyBullets)
			{
				num = 0f;
			}
			if (pierceProjModifier != null && pierceProjModifier.BeastModeLevel == PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE)
			{
				if (majorBreakable.ImmuneToBeastMode)
				{
					num += 1f;
				}
				else
				{
					num = 1000f;
				}
			}
			if (!majorBreakable.IsSecretDoor || !(this.PossibleSourceGun != null) || !this.PossibleSourceGun.InfiniteAmmo)
			{
				float num2 = ((!(this.Owner is AIActor)) ? this.ModifiedDamage : ProjectileData.FixedEnemyDamageToBreakables);
				if (num2 <= 0f && GameManager.Instance.InTutorial)
				{
					majorBreakable.ApplyDamage(1.5f, base.specRigidbody.Velocity, false, false, false);
				}
				else
				{
					majorBreakable.ApplyDamage(num2 * num, base.specRigidbody.Velocity, this.Owner is AIActor, false, false);
				}
			}
		}
		if (rigidbodyCollision.OtherRigidbody.PreventPiercing)
		{
			pierceProjModifier = null;
		}
		if (!flag2 && bounceProjModifier && !minorBreakable && (!bounceProjModifier.onlyBounceOffTiles || !majorBreakable) && !pierceProjModifier && (!bounceProjModifier.useLayerLimit || rigidbodyCollision.OtherPixelCollider.CollisionLayer == bounceProjModifier.layerLimit))
		{
			this.OnTileCollision(rigidbodyCollision);
			return;
		}
		bool flag4 = majorBreakable && majorBreakable.IsSecretDoor;
		if (!majorBreakable && otherRigidbody.name.StartsWith("secret exit collider"))
		{
			flag4 = true;
		}
		if (flag4)
		{
			this.OnTileCollision(rigidbodyCollision);
			return;
		}
		if (otherRigidbody.ReflectProjectiles)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_deflect_01", GameManager.Instance.gameObject);
			if (this.IsBulletScript && bounceProjModifier && bounceProjModifier.removeBulletScriptControl)
			{
				this.RemoveBulletScriptControl();
			}
			Vector2 vector = rigidbodyCollision.Normal;
			if (otherRigidbody.ReflectProjectilesNormalGenerator != null)
			{
				vector = otherRigidbody.ReflectProjectilesNormalGenerator(rigidbodyCollision.Contact, rigidbodyCollision.Normal);
			}
			float num3 = (-rigidbodyCollision.MyRigidbody.Velocity).ToAngle();
			float num4 = vector.ToAngle();
			float num5 = BraveMathCollege.ClampAngle360(num3 + 2f * (num4 - num3));
			if (this.shouldRotate)
			{
				this.m_transform.rotation = Quaternion.Euler(0f, 0f, num5);
			}
			this.m_currentDirection = BraveMathCollege.DegreesToVector(num5, 1f);
			if (this.braveBulletScript && this.braveBulletScript.bullet != null)
			{
				this.braveBulletScript.bullet.Direction = num5;
			}
			if (!bounceProjModifier || !bounceProjModifier.suppressHitEffectsOnBounce)
			{
				this.HandleHitEffectsEnemy(rigidbodyCollision.OtherRigidbody, rigidbodyCollision, false);
			}
			Vector2 vector2 = this.m_currentDirection * this.m_currentSpeed * this.LocalTimeScale;
			PhysicsEngine.PostSliceVelocity = new Vector2?(vector2);
			if (rigidbodyCollision.OtherRigidbody)
			{
				base.specRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 0f, new float?(0.5f));
				rigidbodyCollision.OtherRigidbody.RegisterTemporaryCollisionException(base.specRigidbody, 0f, new float?(0.5f));
			}
			if (otherRigidbody.knockbackDoer && otherRigidbody.knockbackDoer.knockbackWhileReflecting)
			{
				this.HandleKnockback(otherRigidbody, component, false, false);
			}
			return;
		}
		bool flag5 = false;
		bool flag6 = false;
		if (flag2)
		{
			if (!flag || !(component != null))
			{
				flag5 = true;
			}
			if (GameManager.AUDIO_ENABLED && !string.IsNullOrEmpty(this.enemyImpactEventName))
			{
				AkSoundEngine.PostEvent("Play_WPN_" + this.enemyImpactEventName + "_impact_01", base.gameObject);
			}
		}
		else
		{
			flag6 = true;
			if (GameManager.AUDIO_ENABLED && !string.IsNullOrEmpty(this.objectImpactEventName))
			{
				AkSoundEngine.PostEvent("Play_WPN_" + this.objectImpactEventName + "_impact_01", base.gameObject);
			}
		}
		if (!Projectile.s_delayPlayerDamage || !component)
		{
			if (flag2)
			{
				if (!rigidbodyCollision.OtherRigidbody.healthHaver.IsDead || flag)
				{
					this.HandleKnockback(rigidbodyCollision.OtherRigidbody, component, flag, false);
				}
			}
			else
			{
				this.HandleKnockback(rigidbodyCollision.OtherRigidbody, component, false, false);
			}
		}
		if (!component)
		{
			AppliedEffectBase[] components = base.GetComponents<AppliedEffectBase>();
			foreach (AppliedEffectBase appliedEffectBase in components)
			{
				appliedEffectBase.AddSelfToTarget(gameObject);
			}
		}
		base.specRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 0.01f, new float?(0.5f));
		PhysicsEngine.CollisionHaltsVelocity = new bool?(false);
		Projectile projectile = rigidbodyCollision.OtherRigidbody.projectile;
		if (this.CanTransmogrify && flag2 && handleDamageResult != Projectile.HandleDamageResult.HEALTH_AND_KILLED && UnityEngine.Random.value < this.ChanceToTransmogrify && otherRigidbody.aiActor && !otherRigidbody.aiActor.IsMimicEnemy && otherRigidbody.aiActor.healthHaver && !otherRigidbody.aiActor.healthHaver.IsBoss && otherRigidbody.aiActor.healthHaver.IsVulnerable)
		{
			otherRigidbody.aiActor.Transmogrify(EnemyDatabase.GetOrLoadByGuid(this.TransmogrifyTargetGuids[UnityEngine.Random.Range(0, this.TransmogrifyTargetGuids.Length)]), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
		}
		if (pierceProjModifier != null && pierceProjModifier.preventPenetrationOfActors && flag2)
		{
			pierceProjModifier = null;
		}
		bool flag7 = false;
		bool flag8 = false;
		bool flag9 = otherRigidbody && otherRigidbody.GetComponent<PlayerOrbital>();
		if (this.BulletScriptSettings.surviveRigidbodyCollisions)
		{
			flag7 = true;
			flag8 = true;
		}
		else if (pierceProjModifier != null && pierceProjModifier.BeastModeLevel == PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE)
		{
			flag7 = true;
			flag8 = true;
		}
		else if (pierceProjModifier != null && pierceProjModifier.penetration > 0 && flag2)
		{
			pierceProjModifier.penetration--;
			flag7 = true;
			flag8 = true;
		}
		else if (pierceProjModifier != null && pierceProjModifier.penetratesBreakables && pierceProjModifier.penetration > 0)
		{
			pierceProjModifier.penetration--;
			flag7 = true;
			flag8 = true;
		}
		else if (projectile && this.projectileHitHealth > 0)
		{
			PierceProjModifier component3 = projectile.GetComponent<PierceProjModifier>();
			if ((component3 && component3.BeastModeLevel == PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE) || projectile is RobotechProjectile)
			{
				this.projectileHitHealth -= 2;
				projectile.m_hasImpactedEnemy = true;
			}
			else
			{
				this.projectileHitHealth--;
				projectile.m_hasImpactedEnemy = true;
			}
			flag7 = this.projectileHitHealth >= 0;
			flag8 = flag7;
		}
		else if (minorBreakable && this.pierceMinorBreakables)
		{
			flag7 = true;
			flag8 = true;
		}
		else if (bounceProjModifier != null && !flag2 && !this.m_hasImpactedEnemy)
		{
			bounceProjModifier.HandleChanceToDie();
			if (flag2 && bounceProjModifier.ExplodeOnEnemyBounce)
			{
				ExplosiveModifier component4 = base.GetComponent<ExplosiveModifier>();
				if (component4)
				{
					bounceProjModifier.numberOfBounces = 0;
				}
			}
			int num6 = 1;
			PierceProjModifier pierceProjModifier2 = null;
			if (otherRigidbody && otherRigidbody.projectile)
			{
				pierceProjModifier2 = otherRigidbody.GetComponent<PierceProjModifier>();
			}
			if (pierceProjModifier2 && pierceProjModifier2.BeastModeLevel == PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE)
			{
				num6 = 2;
			}
			bool flag10 = bounceProjModifier.numberOfBounces - num6 >= 0;
			flag10 &= !bounceProjModifier.useLayerLimit || rigidbodyCollision.OtherPixelCollider.CollisionLayer == bounceProjModifier.layerLimit;
			flag10 &= !flag9;
			if (flag10)
			{
				if (this.IsBulletScript && bounceProjModifier.removeBulletScriptControl)
				{
					this.RemoveBulletScriptControl();
				}
				Vector2 normal = rigidbodyCollision.Normal;
				if (rigidbodyCollision.MyRigidbody)
				{
					Vector2 velocity = rigidbodyCollision.MyRigidbody.Velocity;
					float num7 = (-velocity).ToAngle();
					float num8 = normal.ToAngle();
					float num9 = BraveMathCollege.ClampAngle360(num7 + 2f * (num8 - num7));
					if (this.shouldRotate)
					{
						this.m_transform.rotation = Quaternion.Euler(0f, 0f, num9);
					}
					this.m_currentDirection = BraveMathCollege.DegreesToVector(num9, 1f);
					this.m_currentSpeed *= 1f - bounceProjModifier.percentVelocityToLoseOnBounce;
					if (this.braveBulletScript && this.braveBulletScript.bullet != null)
					{
						this.braveBulletScript.bullet.Direction = num9;
						this.braveBulletScript.bullet.Speed *= 1f - bounceProjModifier.percentVelocityToLoseOnBounce;
					}
					Vector2 vector3 = this.m_currentDirection * this.m_currentSpeed * this.LocalTimeScale;
					vector3 = bounceProjModifier.AdjustBounceVector(this, vector3, otherRigidbody);
					if (this.shouldRotate && vector3.normalized != this.m_currentDirection)
					{
						this.m_transform.rotation = Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(vector3.normalized));
					}
					this.m_currentDirection = vector3.normalized;
					if (this is HelixProjectile)
					{
						(this as HelixProjectile).AdjustRightVector(Mathf.DeltaAngle(velocity.ToAngle(), num9));
					}
					if (this.OverrideMotionModule != null)
					{
						this.OverrideMotionModule.UpdateDataOnBounce(Mathf.DeltaAngle(velocity.ToAngle(), num9));
					}
					bounceProjModifier.Bounce(this, rigidbodyCollision.Contact, otherRigidbody);
					PhysicsEngine.PostSliceVelocity = new Vector2?(vector3);
					if (rigidbodyCollision.OtherRigidbody)
					{
						base.specRigidbody.RegisterTemporaryCollisionException(rigidbodyCollision.OtherRigidbody, 0f, new float?(0.5f));
						rigidbodyCollision.OtherRigidbody.RegisterTemporaryCollisionException(base.specRigidbody, 0f, new float?(0.5f));
					}
					flag7 = true;
				}
			}
		}
		if (flag5)
		{
			this.HandleHitEffectsEnemy(rigidbodyCollision.OtherRigidbody, rigidbodyCollision, !flag8 && !flag7);
		}
		if (flag6)
		{
			this.HandleHitEffectsObject(rigidbodyCollision.OtherRigidbody, rigidbodyCollision, !flag8 && !flag7);
		}
		this.m_hasPierced = this.m_hasPierced || flag8;
		if (!flag8 && !flag7 && !this.m_hasImpactedObject)
		{
			this.m_hasImpactedObject = true;
			for (int j = 0; j < base.specRigidbody.PixelColliders.Count; j++)
			{
				base.specRigidbody.PixelColliders[j].IsTrigger = true;
			}
			if (flag2 && base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.HandlePostCollisionPersistence(rigidbodyCollision, component));
			}
			else
			{
				this.HandleNormalProjectileDeath(rigidbodyCollision, !flag9);
				PhysicsEngine.HaltRemainingMovement = true;
			}
		}
	}

	// Token: 0x06008363 RID: 33635 RVA: 0x0035CBB4 File Offset: 0x0035ADB4
	protected virtual void OnTileCollision(CollisionData tileCollision)
	{
		if ((!this.damagesWalls || this.SuppressHitEffects) && base.specRigidbody && (base.specRigidbody.UnitWidth > 1f || base.specRigidbody.UnitHeight > 1f))
		{
			this.damagesWalls = true;
			this.SuppressHitEffects = false;
		}
		BounceProjModifier component = base.GetComponent<BounceProjModifier>();
		SpawnProjModifier component2 = base.GetComponent<SpawnProjModifier>();
		ExplosiveModifier component3 = base.GetComponent<ExplosiveModifier>();
		GoopModifier component4 = base.GetComponent<GoopModifier>();
		if (GameManager.AUDIO_ENABLED && !string.IsNullOrEmpty(this.objectImpactEventName))
		{
			AkSoundEngine.PostEvent("Play_WPN_" + this.objectImpactEventName + "_impact_01", base.gameObject);
		}
		this.HandleSparks(new Vector2?(tileCollision.Contact));
		if (this.BulletScriptSettings.surviveTileCollisions)
		{
			PhysicsEngine.PostSliceVelocity = new Vector2?(Vector2.zero);
			return;
		}
		if (component != null)
		{
			component.HandleChanceToDie();
		}
		if (this.damagesWalls)
		{
			this.HandleWallDecals(tileCollision, null);
		}
		bool flag = tileCollision.OtherRigidbody && tileCollision.OtherRigidbody.GetComponent<PlayerOrbital>();
		int num = 1;
		PierceProjModifier pierceProjModifier = null;
		if (tileCollision.OtherRigidbody && tileCollision.OtherRigidbody.projectile)
		{
			pierceProjModifier = tileCollision.OtherRigidbody.GetComponent<PierceProjModifier>();
		}
		if (pierceProjModifier && pierceProjModifier.BeastModeLevel == PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE)
		{
			num = 2;
		}
		bool flag2 = component != null && component.numberOfBounces - num >= 0;
		if (flag2)
		{
			flag2 &= !component.useLayerLimit || tileCollision.OtherPixelCollider.CollisionLayer == component.layerLimit;
			flag2 &= !flag;
		}
		if (flag2)
		{
			if (this.IsBulletScript && component.removeBulletScriptControl)
			{
				this.RemoveBulletScriptControl();
			}
			Vector2 vector = tileCollision.Normal;
			if (tileCollision.OtherRigidbody && tileCollision.OtherRigidbody.ReflectProjectilesNormalGenerator != null)
			{
				vector = tileCollision.OtherRigidbody.ReflectProjectilesNormalGenerator(tileCollision.Contact, vector);
			}
			if (component2 != null && component2.spawnProjectilesOnCollision && component2.spawnCollisionProjectilesOnBounce)
			{
				component2.SpawnCollisionProjectiles(tileCollision.PostCollisionUnitCenter, tileCollision.Normal, null, false);
			}
			if (tileCollision.MyRigidbody)
			{
				Vector2 velocity = tileCollision.MyRigidbody.Velocity;
				float num2 = (-velocity).ToAngle();
				float num3 = vector.ToAngle();
				float num4 = BraveMathCollege.ClampAngle360(num2 + 2f * (num3 - num2));
				if (this.shouldRotate)
				{
					this.m_transform.rotation = Quaternion.Euler(0f, 0f, num4);
				}
				this.m_currentDirection = BraveMathCollege.DegreesToVector(num4, 1f);
				this.m_currentSpeed *= 1f - component.percentVelocityToLoseOnBounce;
				if (this.braveBulletScript && this.braveBulletScript.bullet != null)
				{
					this.braveBulletScript.bullet.Direction = num4;
					this.braveBulletScript.bullet.Speed *= 1f - component.percentVelocityToLoseOnBounce;
				}
				if (!component.suppressHitEffectsOnBounce)
				{
					this.HandleHitEffectsTileMap(tileCollision, false);
				}
				Vector2 vector2 = this.m_currentDirection * this.m_currentSpeed * this.LocalTimeScale;
				vector2 = component.AdjustBounceVector(this, vector2, null);
				if (this.shouldRotate && vector2.normalized != this.m_currentDirection)
				{
					this.m_transform.rotation = Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(vector2.normalized));
				}
				this.m_currentDirection = vector2.normalized;
				if (this is HelixProjectile)
				{
					(this as HelixProjectile).AdjustRightVector(Mathf.DeltaAngle(velocity.ToAngle(), num4));
				}
				if (this.OverrideMotionModule != null)
				{
					this.OverrideMotionModule.UpdateDataOnBounce(Mathf.DeltaAngle(velocity.ToAngle(), num4));
				}
				component.Bounce(this, tileCollision.Contact, tileCollision.OtherRigidbody);
				PhysicsEngine.PostSliceVelocity = new Vector2?(vector2);
			}
		}
		else
		{
			if (component2 != null && component2.spawnProjectilesOnCollision)
			{
				component2.SpawnCollisionProjectiles(tileCollision.PostCollisionUnitCenter, tileCollision.Normal, null, false);
			}
			if (component4 != null)
			{
				component4.SpawnCollisionGoop(tileCollision);
			}
			if (component3 != null)
			{
				component3.Explode(tileCollision.Normal, this.ignoreDamageCaps, null);
			}
			if (!this.SuppressHitEffects)
			{
				this.HandleHitEffectsTileMap(tileCollision, true);
			}
			if (GlobalDungeonData.GUNGEON_EXPERIMENTAL)
			{
				Vector2 vector3 = tileCollision.Contact + (-1f * tileCollision.Normal).normalized * 0.5f;
				IntVector2 intVector = new IntVector2(Mathf.FloorToInt(vector3.x), Mathf.FloorToInt(vector3.y));
				GameManager.Instance.Dungeon.DestroyWallAtPosition(intVector.x, intVector.y, true);
			}
			bool flag3 = !flag;
			this.HandleDestruction(tileCollision, true, flag3);
			PhysicsEngine.HaltRemainingMovement = true;
		}
	}

	// Token: 0x06008364 RID: 33636 RVA: 0x0035D130 File Offset: 0x0035B330
	public void BeamCollision(Projectile currentProjectile)
	{
		if (!this.collidesWithProjectiles)
		{
			return;
		}
		this.DieInAir(false, true, true, false);
	}

	// Token: 0x06008365 RID: 33637 RVA: 0x0035D148 File Offset: 0x0035B348
	private IEnumerator HandlePostInvulnerabilityFrameExceptions(SpeculativeRigidbody otherRigidbody)
	{
		base.specRigidbody.RegisterSpecificCollisionException(otherRigidbody);
		float rigidbodyWidth = (float)otherRigidbody.PrimaryPixelCollider.Width / (float)PhysicsEngine.Instance.PixelsPerUnit;
		yield return new WaitForSeconds(rigidbodyWidth / this.m_currentSpeed * 2f);
		if (base.specRigidbody)
		{
			base.specRigidbody.DeregisterSpecificCollisionException(otherRigidbody);
		}
		yield break;
	}

	// Token: 0x06008366 RID: 33638 RVA: 0x0035D16C File Offset: 0x0035B36C
	private IEnumerator HandlePostCollisionPersistence(CollisionData lcr, PlayerController player)
	{
		CollisionData persistentCollisionData = CollisionData.Pool.Allocate();
		persistentCollisionData.SetAll(lcr);
		this.OverrideTrailPoint = new Vector2?(lcr.Contact);
		if (this.m_currentSpeed < 20f)
		{
			yield return new WaitForSeconds(this.persistTime);
		}
		if (Projectile.s_delayPlayerDamage && player && !player.spriteAnimator.QueryInvulnerabilityFrame())
		{
			bool flag;
			this.HandleDamage(lcr.OtherRigidbody, lcr.OtherPixelCollider, out flag, player, true);
			this.HandleKnockback(lcr.OtherRigidbody, player, flag, true);
		}
		this.HandleNormalProjectileDeath(persistentCollisionData, true);
		CollisionData.Pool.Free(ref persistentCollisionData);
		yield break;
	}

	// Token: 0x170013A8 RID: 5032
	// (get) Token: 0x06008367 RID: 33639 RVA: 0x0035D198 File Offset: 0x0035B398
	private Vector2 SafeCenter
	{
		get
		{
			if (base.specRigidbody)
			{
				return base.specRigidbody.UnitCenter;
			}
			if (this.m_transform)
			{
				return this.m_transform.position.XY();
			}
			return this.LastPosition.XY();
		}
	}

	// Token: 0x06008368 RID: 33640 RVA: 0x0035D1F0 File Offset: 0x0035B3F0
	private void HandleNormalProjectileDeath(CollisionData lcr, bool allowProjectileSpawns = true)
	{
		SpawnProjModifier component = base.GetComponent<SpawnProjModifier>();
		if (component != null && allowProjectileSpawns && component.spawnProjectilesOnCollision && component.spawnOnObjectCollisions)
		{
			Vector2 vector = this.SafeCenter;
			if (lcr != null && lcr.MyRigidbody)
			{
				vector = lcr.PostCollisionUnitCenter;
			}
			component.SpawnCollisionProjectiles(vector, lcr.Normal, lcr.OtherRigidbody, true);
		}
		GoopModifier component2 = base.GetComponent<GoopModifier>();
		if (component2 != null)
		{
			if (lcr == null)
			{
				component2.SpawnCollisionGoop(this.SafeCenter);
			}
			else
			{
				component2.SpawnCollisionGoop(lcr);
			}
		}
		ExplosiveModifier component3 = base.GetComponent<ExplosiveModifier>();
		if (component3 != null)
		{
			component3.Explode(Vector2.zero, this.ignoreDamageCaps, lcr);
		}
		this.HandleDestruction(lcr, true, allowProjectileSpawns);
	}

	// Token: 0x06008369 RID: 33641 RVA: 0x0035D2CC File Offset: 0x0035B4CC
	protected virtual Projectile.HandleDamageResult HandleDamage(SpeculativeRigidbody rigidbody, PixelCollider hitPixelCollider, out bool killedTarget, PlayerController player, bool alreadyPlayerDelayed = false)
	{
		killedTarget = false;
		if (rigidbody.ReflectProjectiles)
		{
			return Projectile.HandleDamageResult.NO_HEALTH;
		}
		if (!rigidbody.healthHaver)
		{
			return Projectile.HandleDamageResult.NO_HEALTH;
		}
		if (!alreadyPlayerDelayed && Projectile.s_delayPlayerDamage && player)
		{
			return Projectile.HandleDamageResult.HEALTH;
		}
		if (rigidbody.spriteAnimator != null && rigidbody.spriteAnimator.QueryInvulnerabilityFrame())
		{
			return Projectile.HandleDamageResult.HEALTH;
		}
		bool flag = !rigidbody.healthHaver.IsDead;
		float num = this.ModifiedDamage;
		if (this.Owner is AIActor && rigidbody && rigidbody.aiActor && (this.Owner as AIActor).IsNormalEnemy)
		{
			num = ProjectileData.FixedFallbackDamageToEnemies;
			if (rigidbody.aiActor.HitByEnemyBullets)
			{
				num /= 4f;
			}
		}
		if (this.Owner is PlayerController && this.m_hasPierced && this.m_healthHaverHitCount >= 1)
		{
			int num2 = Mathf.Clamp(this.m_healthHaverHitCount - 1, 0, GameManager.Instance.PierceDamageScaling.Length - 1);
			num *= GameManager.Instance.PierceDamageScaling[num2];
		}
		if (this.OnWillKillEnemy != null && num >= rigidbody.healthHaver.GetCurrentHealth())
		{
			this.OnWillKillEnemy(this, rigidbody);
		}
		if (rigidbody.healthHaver.IsBoss)
		{
			num *= this.BossDamageMultiplier;
		}
		if (this.BlackPhantomDamageMultiplier != 1f && rigidbody.aiActor && rigidbody.aiActor.IsBlackPhantom)
		{
			num *= this.BlackPhantomDamageMultiplier;
		}
		bool flag2 = false;
		if (this.DelayedDamageToExploders)
		{
			flag2 = rigidbody.GetComponent<ExplodeOnDeath>() && rigidbody.healthHaver.GetCurrentHealth() <= num;
		}
		if (!flag2)
		{
			HealthHaver healthHaver = rigidbody.healthHaver;
			float num3 = num;
			Vector2 velocity = base.specRigidbody.Velocity;
			string ownerName = this.OwnerName;
			CoreDamageTypes coreDamageTypes = this.damageTypes;
			DamageCategory damageCategory = ((!this.IsBlackBullet) ? DamageCategory.Normal : DamageCategory.BlackBullet);
			healthHaver.ApplyDamage(num3, velocity, ownerName, coreDamageTypes, damageCategory, false, hitPixelCollider, this.ignoreDamageCaps);
			if (player && player.OnHitByProjectile != null)
			{
				player.OnHitByProjectile(this, player);
			}
		}
		else
		{
			rigidbody.StartCoroutine(this.HandleDelayedDamage(rigidbody, num, base.specRigidbody.Velocity, hitPixelCollider));
		}
		if (this.Owner && this.Owner is AIActor && player)
		{
			(this.Owner as AIActor).HasDamagedPlayer = true;
		}
		killedTarget = flag && rigidbody.healthHaver.IsDead;
		if (!killedTarget && rigidbody.gameActor != null)
		{
			if (this.AppliesPoison && UnityEngine.Random.value < this.PoisonApplyChance)
			{
				rigidbody.gameActor.ApplyEffect(this.healthEffect, 1f, null);
			}
			if (this.AppliesSpeedModifier && UnityEngine.Random.value < this.SpeedApplyChance)
			{
				rigidbody.gameActor.ApplyEffect(this.speedEffect, 1f, null);
			}
			if (this.AppliesCharm && UnityEngine.Random.value < this.CharmApplyChance)
			{
				rigidbody.gameActor.ApplyEffect(this.charmEffect, 1f, null);
			}
			if (this.AppliesFreeze && UnityEngine.Random.value < this.FreezeApplyChance)
			{
				rigidbody.gameActor.ApplyEffect(this.freezeEffect, 1f, null);
			}
			if (this.AppliesCheese && UnityEngine.Random.value < this.CheeseApplyChance)
			{
				rigidbody.gameActor.ApplyEffect(this.cheeseEffect, 1f, null);
			}
			if (this.AppliesBleed && UnityEngine.Random.value < this.BleedApplyChance)
			{
				rigidbody.gameActor.ApplyEffect(this.bleedEffect, -1f, this);
			}
			if (this.AppliesFire && UnityEngine.Random.value < this.FireApplyChance)
			{
				rigidbody.gameActor.ApplyEffect(this.fireEffect, 1f, null);
			}
			if (this.AppliesStun && UnityEngine.Random.value < this.StunApplyChance && rigidbody.gameActor.behaviorSpeculator)
			{
				rigidbody.gameActor.behaviorSpeculator.Stun(this.AppliedStunDuration, true);
			}
			for (int i = 0; i < this.statusEffectsToApply.Count; i++)
			{
				rigidbody.gameActor.ApplyEffect(this.statusEffectsToApply[i], 1f, null);
			}
		}
		this.m_healthHaverHitCount++;
		return (!killedTarget) ? Projectile.HandleDamageResult.HEALTH : Projectile.HandleDamageResult.HEALTH_AND_KILLED;
	}

	// Token: 0x0600836A RID: 33642 RVA: 0x0035D7C0 File Offset: 0x0035B9C0
	private IEnumerator HandleDelayedDamage(SpeculativeRigidbody targetRigidbody, float damage, Vector2 damageVec, PixelCollider hitPixelCollider)
	{
		yield return new WaitForSeconds(0.5f);
		if (targetRigidbody && targetRigidbody.healthHaver)
		{
			HealthHaver healthHaver = targetRigidbody.healthHaver;
			string ownerName = this.OwnerName;
			CoreDamageTypes coreDamageTypes = this.damageTypes;
			DamageCategory damageCategory = ((!this.IsBlackBullet) ? DamageCategory.Normal : DamageCategory.BlackBullet);
			healthHaver.ApplyDamage(damage, damageVec, ownerName, coreDamageTypes, damageCategory, false, hitPixelCollider, this.ignoreDamageCaps);
		}
		yield break;
	}

	// Token: 0x0600836B RID: 33643 RVA: 0x0035D7F8 File Offset: 0x0035B9F8
	public void HandleKnockback(SpeculativeRigidbody rigidbody, PlayerController player, bool killedTarget = false, bool alreadyPlayerDelayed = false)
	{
		if (!alreadyPlayerDelayed && Projectile.s_delayPlayerDamage && player)
		{
			return;
		}
		KnockbackDoer knockbackDoer = rigidbody.knockbackDoer;
		Vector2 vector = this.LastVelocity;
		if (this.HasFixedKnockbackDirection)
		{
			vector = BraveMathCollege.DegreesToVector(this.FixedKnockbackDirection, 1f);
		}
		if (!knockbackDoer)
		{
			return;
		}
		if (killedTarget)
		{
			knockbackDoer.ApplySourcedKnockback(vector, this.baseData.force * knockbackDoer.deathMultiplier, base.gameObject, false);
		}
		else
		{
			knockbackDoer.ApplySourcedKnockback(vector, this.baseData.force, base.gameObject, false);
		}
	}

	// Token: 0x0600836C RID: 33644 RVA: 0x0035D8A0 File Offset: 0x0035BAA0
	protected virtual void HandleHitEffectsEnemy(SpeculativeRigidbody rigidbody, CollisionData lcr, bool playProjectileDeathVfx)
	{
		if (this.hitEffects == null)
		{
			return;
		}
		if (this.hitEffects.alwaysUseMidair)
		{
			this.HandleHitEffectsMidair(false);
			return;
		}
		Vector3 vector = lcr.Contact.ToVector3ZUp(-1f);
		float num = 0f;
		bool flag = false;
		if (rigidbody != null)
		{
			HitEffectHandler hitEffectHandler = rigidbody.hitEffectHandler;
			if (hitEffectHandler != null)
			{
				if (hitEffectHandler.SuppressAllHitEffects)
				{
					flag = true;
				}
				else
				{
					if (hitEffectHandler.additionalHitEffects.Length > 0)
					{
						hitEffectHandler.HandleAdditionalHitEffects(base.specRigidbody.Velocity, lcr.OtherPixelCollider);
					}
					if (hitEffectHandler.overrideHitEffectPool != null && hitEffectHandler.overrideHitEffectPool.type != VFXPoolType.None)
					{
						hitEffectHandler.overrideHitEffectPool.SpawnAtPosition(vector, num, rigidbody.transform, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), null, false, null, null, false);
						flag = true;
					}
					else if (hitEffectHandler.overrideHitEffect != null && hitEffectHandler.overrideHitEffect.effects.Length > 0)
					{
						hitEffectHandler.overrideHitEffect.SpawnAtPosition(vector, num, rigidbody.transform, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), null, false, null, null, false);
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			this.hitEffects.HandleEnemyImpact(vector, num, rigidbody.transform, lcr.Normal, base.specRigidbody.Velocity, playProjectileDeathVfx, false);
		}
	}

	// Token: 0x0600836D RID: 33645 RVA: 0x0035DA28 File Offset: 0x0035BC28
	protected void HandleHitEffectsObject(SpeculativeRigidbody srb, CollisionData lcr, bool playProjectileDeathVfx)
	{
		if (this.hitEffects == null)
		{
			return;
		}
		if (this.hitEffects.alwaysUseMidair)
		{
			this.HandleHitEffectsMidair(false);
			return;
		}
		Vector3 vector = lcr.Contact.ToVector3ZUp(-1f);
		float num = Mathf.Atan2(lcr.Normal.y, lcr.Normal.x) * 57.29578f;
		bool flag = false;
		if (srb != null)
		{
			HitEffectHandler hitEffectHandler = srb.hitEffectHandler;
			if (hitEffectHandler != null)
			{
				if (hitEffectHandler.SuppressAllHitEffects)
				{
					flag = true;
				}
				else if (hitEffectHandler.overrideMaterialDefinition != null)
				{
					VFXComplex.SpawnMethod spawnMethod = ((!this.CenterTilemapHitEffectsByProjectileVelocity) ? null : new VFXComplex.SpawnMethod(this.SpawnVFXProjectileCenter));
					if (Mathf.Abs(lcr.Normal.x) > Mathf.Abs(lcr.Normal.y))
					{
						if (this.hitEffects.HasTileMapHorizontalEffects)
						{
							this.hitEffects.HandleTileMapImpactHorizontal(vector, num, lcr.Normal, base.specRigidbody.Velocity, playProjectileDeathVfx, srb.transform, spawnMethod, new VFXComplex.SpawnMethod(this.SpawnVFXPostProcessStickyGrenades));
							flag = true;
						}
						else if (hitEffectHandler.overrideMaterialDefinition.fallbackHorizontalTileMapEffects.Length > 0)
						{
							hitEffectHandler.overrideMaterialDefinition.SpawnRandomHorizontal(vector, num, srb.transform, lcr.Normal, base.specRigidbody.Velocity);
							flag = true;
						}
					}
					else if (this.hitEffects.HasTileMapVerticalEffects)
					{
						if (lcr.Normal.y > 0f)
						{
							this.hitEffects.HandleTileMapImpactVertical(vector, -0.25f, num, lcr.Normal, base.specRigidbody.Velocity, playProjectileDeathVfx, srb.transform, spawnMethod, new VFXComplex.SpawnMethod(this.SpawnVFXPostProcessStickyGrenades));
						}
						else
						{
							this.hitEffects.HandleTileMapImpactVertical(vector, 0.25f, num, lcr.Normal, base.specRigidbody.Velocity, playProjectileDeathVfx, srb.transform, spawnMethod, new VFXComplex.SpawnMethod(this.SpawnVFXPostProcessStickyGrenades));
						}
						flag = true;
					}
					else if (hitEffectHandler.overrideMaterialDefinition.fallbackVerticalTileMapEffects.Length > 0)
					{
						hitEffectHandler.overrideMaterialDefinition.SpawnRandomVertical(vector, num, srb.transform, lcr.Normal, base.specRigidbody.Velocity);
						flag = true;
					}
					if (this.damagesWalls)
					{
						Vector3 vector2 = lcr.Normal.normalized.ToVector3ZUp(0f) * 0.1f;
						Vector3 vector3 = vector + vector2;
						float num2 = ((!(this.Owner is AIActor)) ? this.ModifiedDamage : ProjectileData.FixedEnemyDamageToBreakables);
						hitEffectHandler.overrideMaterialDefinition.SpawnRandomShard(vector3, lcr.Normal, num2);
					}
					this.HandleWallDecals(lcr, srb.transform);
				}
				else if (hitEffectHandler.overrideHitEffectPool != null && hitEffectHandler.overrideHitEffectPool.type != VFXPoolType.None)
				{
					hitEffectHandler.overrideHitEffectPool.SpawnAtPosition(vector, 0f, srb.transform, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), null, false, null, null, false);
					flag = true;
				}
				else if (hitEffectHandler.overrideHitEffect != null && hitEffectHandler.overrideHitEffect.effects.Length > 0)
				{
					hitEffectHandler.overrideHitEffect.SpawnAtPosition(vector, 0f, srb.transform, new Vector2?(lcr.Normal), new Vector2?(base.specRigidbody.Velocity), null, false, null, null, false);
					flag = true;
				}
			}
		}
		if (this is SharkProjectile)
		{
			flag = true;
		}
		if (!flag)
		{
			this.hitEffects.HandleEnemyImpact(vector, 0f, null, lcr.Normal, base.specRigidbody.Velocity, true, false);
		}
	}

	// Token: 0x0600836E RID: 33646 RVA: 0x0035DDF0 File Offset: 0x0035BFF0
	public void LerpMaterialGlow(Material targetMaterial, float startGlow, float targetGlow, float duration)
	{
		base.StartCoroutine(this.LerpMaterialGlowCR(targetMaterial, startGlow, targetGlow, duration));
	}

	// Token: 0x0600836F RID: 33647 RVA: 0x0035DE04 File Offset: 0x0035C004
	private IEnumerator LerpMaterialGlowCR(Material targetMaterial, float startGlow, float targetGlow, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += this.LocalDeltaTime;
			float t = elapsed / duration;
			if (targetMaterial != null)
			{
				targetMaterial.SetFloat("_EmissivePower", Mathf.Lerp(startGlow, targetGlow, t));
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008370 RID: 33648 RVA: 0x0035DE3C File Offset: 0x0035C03C
	public GameObject SpawnVFXPostProcessStickyGrenades(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(prefab, position, rotation);
		StickyGrenadeBuff component = base.GetComponent<StickyGrenadeBuff>();
		if (component)
		{
			StickyGrenadePersistentDebris component2 = gameObject.GetComponent<StickyGrenadePersistentDebris>();
			if (component2)
			{
				component2.InitializeSelf(component);
			}
		}
		StrafeBleedBuff component3 = base.GetComponent<StrafeBleedBuff>();
		if (component3)
		{
			StrafeBleedPersistentDebris component4 = gameObject.GetComponent<StrafeBleedPersistentDebris>();
			if (component4)
			{
				component4.InitializeSelf(component3);
			}
		}
		return gameObject;
	}

	// Token: 0x06008371 RID: 33649 RVA: 0x0035DEAC File Offset: 0x0035C0AC
	public GameObject SpawnVFXProjectileCenter(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools)
	{
		Vector3 vector = position;
		if (base.specRigidbody)
		{
			vector = base.specRigidbody.UnitCenter.ToVector3ZUp(position.z);
			float num = Vector2.Distance(vector, position);
			vector += this.LastVelocity.normalized.ToVector3ZUp(0f) * num;
		}
		return SpawnManager.SpawnVFX(prefab, vector, rotation);
	}

	// Token: 0x06008372 RID: 33650 RVA: 0x0035DF24 File Offset: 0x0035C124
	protected void HandleHitEffectsTileMap(CollisionData lcr, bool playProjectileDeathVfx)
	{
		if (this.hitEffects == null)
		{
			return;
		}
		if (this.hitEffects.alwaysUseMidair)
		{
			this.HandleHitEffectsMidair(false);
			return;
		}
		int num = Mathf.RoundToInt(lcr.Contact.x);
		int num2 = Mathf.RoundToInt(lcr.Contact.y);
		float num3 = 0f;
		if (GameManager.Instance.Dungeon.data.CheckInBounds(num, num2))
		{
			CellData cellData = GameManager.Instance.Dungeon.data[num, num2];
			if (cellData != null && cellData.diagonalWallType != DiagonalWallType.NONE)
			{
				if (cellData.diagonalWallType == DiagonalWallType.NORTHEAST || cellData.diagonalWallType == DiagonalWallType.NORTHWEST)
				{
					lcr.Normal = Vector2.down;
				}
				else
				{
					lcr.Normal = Vector2.up;
				}
			}
		}
		Vector3 vector = lcr.Contact.ToVector3ZUp(-1f);
		float num4 = Mathf.Atan2(lcr.Normal.y, lcr.Normal.x) * 57.29578f;
		VFXComplex.SpawnMethod spawnMethod = ((!this.CenterTilemapHitEffectsByProjectileVelocity) ? null : new VFXComplex.SpawnMethod(this.SpawnVFXProjectileCenter));
		if (lcr.Normal.y < -0.1f)
		{
			this.hitEffects.HandleTileMapImpactVertical(vector, 0.5f + num3, num4, lcr.Normal, base.specRigidbody.Velocity, playProjectileDeathVfx, null, spawnMethod, new VFXComplex.SpawnMethod(this.SpawnVFXPostProcessStickyGrenades));
		}
		else if (lcr.Normal.y > 0.1f)
		{
			this.hitEffects.HandleTileMapImpactVertical(vector, -0.25f + num3, num4, lcr.Normal, base.specRigidbody.Velocity, playProjectileDeathVfx, null, spawnMethod, new VFXComplex.SpawnMethod(this.SpawnVFXPostProcessStickyGrenades));
		}
		else
		{
			this.hitEffects.HandleTileMapImpactHorizontal(vector, num4, lcr.Normal, base.specRigidbody.Velocity, playProjectileDeathVfx, null, spawnMethod, new VFXComplex.SpawnMethod(this.SpawnVFXPostProcessStickyGrenades));
		}
		if (this.damagesWalls)
		{
			Vector3 vector2 = lcr.Normal.normalized.ToVector3ZUp(0f) * 0.1f;
			Vector3 vector3 = vector + vector2;
			if (GameManager.Instance.Dungeon != null)
			{
				int roomVisualTypeAtPosition = GameManager.Instance.Dungeon.data.GetRoomVisualTypeAtPosition(vector3.XY());
				float num5 = ((!(this.Owner is AIActor)) ? this.ModifiedDamage : ProjectileData.FixedEnemyDamageToBreakables);
				GameManager.Instance.Dungeon.roomMaterialDefinitions[roomVisualTypeAtPosition].SpawnRandomShard(vector3, lcr.Normal, num5);
			}
		}
	}

	// Token: 0x06008373 RID: 33651 RVA: 0x0035E1C8 File Offset: 0x0035C3C8
	protected void HandleHitEffectsMidair(bool killedEarly = false)
	{
		if (this.hitEffects == null)
		{
			return;
		}
		if (killedEarly && this.hitEffects.overrideEarlyDeathVfx != null)
		{
			SpawnManager.SpawnVFX(this.hitEffects.overrideEarlyDeathVfx, this.m_transform.position, Quaternion.identity);
			return;
		}
		if (this.hitEffects.suppressMidairDeathVfx)
		{
			return;
		}
		if (this.hitEffects.overrideMidairDeathVFX != null || this.hitEffects.alwaysUseMidair)
		{
			GameObject gameObject = SpawnManager.SpawnVFX(this.hitEffects.overrideMidairDeathVFX, this.m_transform.position, (!this.hitEffects.midairInheritsRotation) ? Quaternion.identity : this.m_transform.rotation);
			BraveBehaviour component = gameObject.GetComponent<BraveBehaviour>();
			if (this.hitEffects.midairInheritsFlip)
			{
				component.sprite.FlipX = base.sprite.FlipX;
				component.sprite.FlipY = base.sprite.FlipY;
			}
			if (this.hitEffects.overrideMidairZHeight != -1)
			{
				component.sprite.HeightOffGround = (float)this.hitEffects.overrideMidairZHeight;
			}
			if (this.hitEffects.midairInheritsVelocity)
			{
				if (component.debris)
				{
					component.debris.Trigger(base.specRigidbody.Velocity.ToVector3ZUp(0.5f), 0.1f, 1f);
				}
				else
				{
					SimpleMover orAddComponent = gameObject.GetOrAddComponent<SimpleMover>();
					orAddComponent.velocity = this.m_currentDirection * this.m_currentSpeed * 0.4f;
					if (component.spriteAnimator != null)
					{
						float num = (float)component.spriteAnimator.DefaultClip.frames.Length / component.spriteAnimator.DefaultClip.fps;
						orAddComponent.acceleration = orAddComponent.velocity / num * -1f;
					}
				}
			}
			else if (component.debris)
			{
				component.debris.Trigger(new Vector3(UnityEngine.Random.value * 2f - 1f, UnityEngine.Random.value * 2f - 1f, 5f), 0.1f, 1f);
			}
			if (component.particleSystem)
			{
				gameObject.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
			}
		}
		else if (this.hitEffects != null && base.specRigidbody)
		{
			this.hitEffects.HandleTileMapImpactVertical(this.m_transform.position, 0f, 0f, Vector2.zero, base.specRigidbody.Velocity, false, null, null, null);
		}
	}

	// Token: 0x06008374 RID: 33652 RVA: 0x0035E4AC File Offset: 0x0035C6AC
	protected override void OnDestroy()
	{
		StaticReferenceManager.RemoveProjectile(this);
		base.OnDestroy();
	}

	// Token: 0x06008375 RID: 33653 RVA: 0x0035E4BC File Offset: 0x0035C6BC
	public void OnSpawned()
	{
		if (this.m_cachedBaseData == null)
		{
			this.m_cachedCollidesWithPlayer = this.collidesWithPlayer;
			this.m_cachedCollidesWithProjectiles = this.collidesWithProjectiles;
			this.m_cachedCollidesWithEnemies = this.collidesWithEnemies;
			this.m_cachedDamagesWalls = this.damagesWalls;
			this.m_cachedBaseData = new ProjectileData(this.baseData);
			this.m_cachedBulletScriptSettings = new BulletScriptSettings(this.BulletScriptSettings);
			if (base.specRigidbody)
			{
				this.m_cachedCollideWithTileMap = base.specRigidbody.CollideWithTileMap;
				this.m_cachedCollideWithOthers = base.specRigidbody.CollideWithOthers;
			}
			if (!base.sprite)
			{
				base.sprite = base.GetComponentInChildren<tk2dSprite>();
			}
			if (!base.spriteAnimator && base.sprite)
			{
				base.spriteAnimator = base.sprite.spriteAnimator;
			}
			if (base.sprite)
			{
				this.m_cachedSpriteId = base.sprite.spriteId;
			}
		}
		if (base.enabled)
		{
			this.Start();
			base.specRigidbody.enabled = true;
			for (int i = 0; i < base.specRigidbody.PixelColliders.Count; i++)
			{
				base.specRigidbody.PixelColliders[i].IsTrigger = false;
			}
			base.specRigidbody.Reinitialize();
			if (this.TrailRenderer)
			{
				this.TrailRenderer.Clear();
			}
			if (this.CustomTrailRenderer)
			{
				this.CustomTrailRenderer.Clear();
			}
			if (this.ParticleTrail)
			{
				BraveUtility.EnableEmission(this.ParticleTrail, true);
			}
		}
		this.m_spawnPool = SpawnManager.LastPrefabPool;
	}

	// Token: 0x06008376 RID: 33654 RVA: 0x0035E684 File Offset: 0x0035C884
	public virtual void OnDespawned()
	{
		this.Cleanup();
	}

	// Token: 0x06008377 RID: 33655 RVA: 0x0035E68C File Offset: 0x0035C88C
	public void BecomeBlackBullet()
	{
		if (!this.IsBlackBullet && base.sprite)
		{
			this.ForceBlackBullet = true;
			this.IsBlackBullet = true;
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.SetFloat("_BlackBullet", 1f);
			base.sprite.renderer.material.SetFloat("_EmissivePower", -40f);
		}
	}

	// Token: 0x06008378 RID: 33656 RVA: 0x0035E70C File Offset: 0x0035C90C
	public void ReturnFromBlackBullet()
	{
		if (this.IsBlackBullet)
		{
			this.IsBlackBullet = false;
			base.sprite.renderer.material.SetFloat("_BlackBullet", 0f);
			base.sprite.usesOverrideMaterial = false;
			base.sprite.ForceUpdateMaterial();
		}
	}

	// Token: 0x06008379 RID: 33657 RVA: 0x0035E764 File Offset: 0x0035C964
	private void Cleanup()
	{
		StaticReferenceManager.RemoveProjectile(this);
		if (base.specRigidbody)
		{
			base.specRigidbody.enabled = false;
		}
		this.ManualControl = false;
		this.IsBulletScript = false;
		this.SuppressHitEffects = false;
		this.ReturnFromBlackBullet();
		this.m_forceBlackBullet = false;
		this.collidesWithPlayer = this.m_cachedCollidesWithPlayer;
		this.collidesWithProjectiles = this.m_cachedCollidesWithProjectiles;
		this.collidesWithEnemies = this.m_cachedCollidesWithEnemies;
		this.damagesWalls = this.m_cachedDamagesWalls;
		this.m_timeElapsed = 0f;
		this.m_distanceElapsed = 0f;
		this.LastPosition = Vector3.zero;
		this.m_hasImpactedObject = false;
		this.m_hasDiedInAir = false;
		this.m_hasPierced = false;
		this.m_healthHaverHitCount = 0;
		this.m_ignoreTileCollisionsTimer = 0f;
		if (this.m_cachedBaseData != null && this.baseData != null)
		{
			this.baseData.SetAll(this.m_cachedBaseData);
		}
		if (this.TrailRenderer)
		{
			this.TrailRenderer.Clear();
		}
		if (this.CustomTrailRenderer)
		{
			this.CustomTrailRenderer.Clear();
		}
		if (this.ParticleTrail)
		{
			BraveUtility.EnableEmission(this.ParticleTrail, false);
		}
		if (this.m_cachedBulletScriptSettings != null && this.BulletScriptSettings != null)
		{
			this.BulletScriptSettings.SetAll(this.m_cachedBulletScriptSettings);
		}
		if (base.specRigidbody)
		{
			base.specRigidbody.CollideWithTileMap = this.m_cachedCollideWithTileMap;
			base.specRigidbody.CollideWithOthers = this.m_cachedCollideWithOthers;
			base.specRigidbody.ClearSpecificCollisionExceptions();
		}
		if (base.spriteAnimator && !base.spriteAnimator.playAutomatically)
		{
			base.spriteAnimator.Stop();
		}
		if (base.sprite && this.m_cachedSpriteId >= 0)
		{
			base.sprite.SetSprite(this.m_cachedSpriteId);
		}
		if (base.sprite && this.m_isRamping)
		{
			this.m_isRamping = false;
			this.m_currentRampHeight = 0f;
			base.sprite.HeightOffGround = Projectile.CurrentProjectileDepth;
		}
		this.Owner = null;
		this.m_shooter = null;
		this.TrapOwner = null;
		this.OwnerName = null;
		this.m_spawnPool = null;
		if (base.specRigidbody)
		{
			base.specRigidbody.Cleanup();
		}
		this.m_initialized = false;
		if (base.specRigidbody)
		{
			PhysicsEngine.Instance.DeregisterWhenAvailable(base.specRigidbody);
		}
	}

	// Token: 0x170013A9 RID: 5033
	// (get) Token: 0x0600837A RID: 33658 RVA: 0x0035EA04 File Offset: 0x0035CC04
	// (set) Token: 0x0600837B RID: 33659 RVA: 0x0035EA0C File Offset: 0x0035CC0C
	public Vector3 LastPosition
	{
		get
		{
			return this.m_lastPosition;
		}
		set
		{
			this.m_lastPosition = value;
		}
	}

	// Token: 0x170013AA RID: 5034
	// (get) Token: 0x0600837C RID: 33660 RVA: 0x0035EA18 File Offset: 0x0035CC18
	public bool HasImpactedEnemy
	{
		get
		{
			return this.m_hasImpactedEnemy;
		}
	}

	// Token: 0x170013AB RID: 5035
	// (get) Token: 0x0600837D RID: 33661 RVA: 0x0035EA20 File Offset: 0x0035CC20
	public int NumberHealthHaversHit
	{
		get
		{
			return this.m_healthHaverHitCount;
		}
	}

	// Token: 0x170013AC RID: 5036
	// (get) Token: 0x0600837E RID: 33662 RVA: 0x0035EA28 File Offset: 0x0035CC28
	public bool HasDiedInAir
	{
		get
		{
			return this.m_hasDiedInAir;
		}
	}

	// Token: 0x0400860A RID: 34314
	public static bool s_delayPlayerDamage;

	// Token: 0x0400860B RID: 34315
	public static float s_maxProjectileScale = 3.5f;

	// Token: 0x0400860C RID: 34316
	private static float s_enemyBulletSpeedModfier = 1f;

	// Token: 0x0400860D RID: 34317
	private static float s_baseEnemyBulletSpeedMultiplier = 1f;

	// Token: 0x04008611 RID: 34321
	[NonSerialized]
	public Gun PossibleSourceGun;

	// Token: 0x04008612 RID: 34322
	[NonSerialized]
	public bool SpawnedFromOtherPlayerProjectile;

	// Token: 0x04008613 RID: 34323
	[NonSerialized]
	public float PlayerProjectileSourceGameTimeslice = -1f;

	// Token: 0x04008614 RID: 34324
	[NonSerialized]
	private GameActor m_owner;

	// Token: 0x04008615 RID: 34325
	[FormerlySerializedAs("BulletMLSettings")]
	public BulletScriptSettings BulletScriptSettings;

	// Token: 0x04008616 RID: 34326
	[EnumFlags]
	public CoreDamageTypes damageTypes;

	// Token: 0x04008617 RID: 34327
	public bool allowSelfShooting;

	// Token: 0x04008618 RID: 34328
	public bool collidesWithPlayer = true;

	// Token: 0x04008619 RID: 34329
	public bool collidesWithProjectiles;

	// Token: 0x0400861A RID: 34330
	[ShowInInspectorIf("collidesWithProjectiles", true)]
	public bool collidesOnlyWithPlayerProjectiles;

	// Token: 0x0400861B RID: 34331
	[ShowInInspectorIf("collidesWithProjectiles", true)]
	public int projectileHitHealth;

	// Token: 0x0400861C RID: 34332
	public bool collidesWithEnemies = true;

	// Token: 0x0400861D RID: 34333
	public bool shouldRotate;

	// Token: 0x0400861E RID: 34334
	[FormerlySerializedAs("shouldFlip")]
	[ShowInInspectorIf("shouldRotate", false)]
	public bool shouldFlipVertically;

	// Token: 0x0400861F RID: 34335
	public bool shouldFlipHorizontally;

	// Token: 0x04008620 RID: 34336
	public bool ignoreDamageCaps;

	// Token: 0x04008621 RID: 34337
	[NonSerialized]
	private float m_cachedInitialDamage = -1f;

	// Token: 0x04008622 RID: 34338
	public ProjectileData baseData;

	// Token: 0x04008623 RID: 34339
	public bool AppliesPoison;

	// Token: 0x04008624 RID: 34340
	public float PoisonApplyChance = 1f;

	// Token: 0x04008625 RID: 34341
	public GameActorHealthEffect healthEffect;

	// Token: 0x04008626 RID: 34342
	public bool AppliesSpeedModifier;

	// Token: 0x04008627 RID: 34343
	public float SpeedApplyChance = 1f;

	// Token: 0x04008628 RID: 34344
	public GameActorSpeedEffect speedEffect;

	// Token: 0x04008629 RID: 34345
	public bool AppliesCharm;

	// Token: 0x0400862A RID: 34346
	public float CharmApplyChance = 1f;

	// Token: 0x0400862B RID: 34347
	public GameActorCharmEffect charmEffect;

	// Token: 0x0400862C RID: 34348
	public bool AppliesFreeze;

	// Token: 0x0400862D RID: 34349
	public float FreezeApplyChance = 1f;

	// Token: 0x0400862E RID: 34350
	public GameActorFreezeEffect freezeEffect;

	// Token: 0x0400862F RID: 34351
	public bool AppliesFire;

	// Token: 0x04008630 RID: 34352
	public float FireApplyChance = 1f;

	// Token: 0x04008631 RID: 34353
	public GameActorFireEffect fireEffect;

	// Token: 0x04008632 RID: 34354
	public bool AppliesStun;

	// Token: 0x04008633 RID: 34355
	public float StunApplyChance = 1f;

	// Token: 0x04008634 RID: 34356
	public float AppliedStunDuration = 1f;

	// Token: 0x04008635 RID: 34357
	public bool AppliesBleed;

	// Token: 0x04008636 RID: 34358
	public GameActorBleedEffect bleedEffect;

	// Token: 0x04008637 RID: 34359
	public bool AppliesCheese;

	// Token: 0x04008638 RID: 34360
	public float CheeseApplyChance = 1f;

	// Token: 0x04008639 RID: 34361
	public GameActorCheeseEffect cheeseEffect;

	// Token: 0x0400863A RID: 34362
	public float BleedApplyChance = 1f;

	// Token: 0x0400863B RID: 34363
	public bool CanTransmogrify;

	// Token: 0x0400863C RID: 34364
	[ShowInInspectorIf("CanTransmogrify", false)]
	public float ChanceToTransmogrify;

	// Token: 0x0400863D RID: 34365
	[EnemyIdentifier]
	public string[] TransmogrifyTargetGuids;

	// Token: 0x0400863E RID: 34366
	[NonSerialized]
	public float BossDamageMultiplier = 1f;

	// Token: 0x0400863F RID: 34367
	[NonSerialized]
	public bool SpawnedFromNonChallengeItem;

	// Token: 0x04008640 RID: 34368
	[NonSerialized]
	public bool TreatedAsNonProjectileForChallenge;

	// Token: 0x04008641 RID: 34369
	public ProjectileImpactVFXPool hitEffects;

	// Token: 0x04008642 RID: 34370
	public bool CenterTilemapHitEffectsByProjectileVelocity;

	// Token: 0x04008643 RID: 34371
	public VFXPool wallDecals;

	// Token: 0x04008644 RID: 34372
	public bool damagesWalls = true;

	// Token: 0x04008646 RID: 34374
	public float persistTime = 0.25f;

	// Token: 0x04008647 RID: 34375
	public float angularVelocity;

	// Token: 0x04008648 RID: 34376
	public float angularVelocityVariance;

	// Token: 0x04008649 RID: 34377
	[EnemyIdentifier]
	public string spawnEnemyGuidOnDeath;

	// Token: 0x0400864A RID: 34378
	public bool HasFixedKnockbackDirection;

	// Token: 0x0400864B RID: 34379
	public float FixedKnockbackDirection;

	// Token: 0x0400864C RID: 34380
	public bool pierceMinorBreakables;

	// Token: 0x0400864D RID: 34381
	[Header("Audio Flags")]
	public string objectImpactEventName = string.Empty;

	// Token: 0x0400864E RID: 34382
	public string enemyImpactEventName = string.Empty;

	// Token: 0x0400864F RID: 34383
	public string onDestroyEventName = string.Empty;

	// Token: 0x04008650 RID: 34384
	public string additionalStartEventName = string.Empty;

	// Token: 0x04008651 RID: 34385
	[Header("Unusual Options")]
	public bool IsRadialBurstLimited;

	// Token: 0x04008652 RID: 34386
	public int MaxRadialBurstLimit = -1;

	// Token: 0x04008653 RID: 34387
	public SynergyBurstLimit[] AdditionalBurstLimits;

	// Token: 0x04008654 RID: 34388
	public bool AppliesKnockbackToPlayer;

	// Token: 0x04008655 RID: 34389
	public float PlayerKnockbackForce;

	// Token: 0x04008656 RID: 34390
	public bool HasDefaultTint;

	// Token: 0x04008657 RID: 34391
	[ShowInInspectorIf("HasDefaultTint", false)]
	public Color DefaultTintColor;

	// Token: 0x04008658 RID: 34392
	[NonSerialized]
	public bool IsCritical;

	// Token: 0x04008659 RID: 34393
	[NonSerialized]
	public float BlackPhantomDamageMultiplier = 1f;

	// Token: 0x0400865A RID: 34394
	[Header("For Brents")]
	public bool PenetratesInternalWalls;

	// Token: 0x0400865B RID: 34395
	public bool neverMaskThis;

	// Token: 0x0400865C RID: 34396
	public bool isFakeBullet;

	// Token: 0x0400865D RID: 34397
	public bool CanBecomeBlackBullet = true;

	// Token: 0x0400865E RID: 34398
	public TrailRenderer TrailRenderer;

	// Token: 0x0400865F RID: 34399
	public CustomTrailRenderer CustomTrailRenderer;

	// Token: 0x04008660 RID: 34400
	public ParticleSystem ParticleTrail;

	// Token: 0x04008661 RID: 34401
	public bool DelayedDamageToExploders;

	// Token: 0x04008662 RID: 34402
	public Action<Projectile, SpeculativeRigidbody, bool> OnHitEnemy;

	// Token: 0x04008663 RID: 34403
	public Action<Projectile, SpeculativeRigidbody> OnWillKillEnemy;

	// Token: 0x04008664 RID: 34404
	public Action<DebrisObject> OnBecameDebris;

	// Token: 0x04008665 RID: 34405
	public Action<DebrisObject> OnBecameDebrisGrounded;

	// Token: 0x0400866D RID: 34413
	[NonSerialized]
	public bool IsBlackBullet;

	// Token: 0x0400866E RID: 34414
	private bool m_forceBlackBullet;

	// Token: 0x0400866F RID: 34415
	[NonSerialized]
	public List<GameActorEffect> statusEffectsToApply = new List<GameActorEffect>();

	// Token: 0x04008671 RID: 34417
	private bool m_initialized;

	// Token: 0x04008672 RID: 34418
	private Transform m_transform;

	// Token: 0x04008673 RID: 34419
	private bool? m_cachedHasBeamController;

	// Token: 0x0400867C RID: 34428
	public float AdditionalScaleMultiplier = 1f;

	// Token: 0x0400867D RID: 34429
	private int m_cachedLayer;

	// Token: 0x0400867E RID: 34430
	private int m_currentTintPriority = -1;

	// Token: 0x0400867F RID: 34431
	public Func<Vector2, Vector2> ModifyVelocity;

	// Token: 0x04008680 RID: 34432
	[NonSerialized]
	public bool CurseSparks;

	// Token: 0x04008681 RID: 34433
	private Vector2? m_lastSparksPoint;

	// Token: 0x04008682 RID: 34434
	public Action<Projectile> PreMoveModifiers;

	// Token: 0x04008683 RID: 34435
	[NonSerialized]
	public ProjectileMotionModule OverrideMotionModule;

	// Token: 0x04008684 RID: 34436
	[NonSerialized]
	protected bool m_usesNormalMoveRegardless;

	// Token: 0x04008685 RID: 34437
	public static Dungeon m_cachedDungeon;

	// Token: 0x04008686 RID: 34438
	public static int m_cacheTick;

	// Token: 0x04008687 RID: 34439
	protected bool m_isInWall;

	// Token: 0x04008688 RID: 34440
	private SpeculativeRigidbody m_shooter;

	// Token: 0x04008689 RID: 34441
	protected float m_currentSpeed;

	// Token: 0x0400868A RID: 34442
	protected Vector2 m_currentDirection;

	// Token: 0x0400868B RID: 34443
	protected MeshRenderer m_renderer;

	// Token: 0x0400868C RID: 34444
	protected float m_timeElapsed;

	// Token: 0x0400868D RID: 34445
	protected float m_distanceElapsed;

	// Token: 0x0400868E RID: 34446
	protected Vector3 m_lastPosition;

	// Token: 0x0400868F RID: 34447
	protected bool m_hasImpactedObject;

	// Token: 0x04008690 RID: 34448
	protected bool m_hasImpactedEnemy;

	// Token: 0x04008691 RID: 34449
	protected bool m_hasDiedInAir;

	// Token: 0x04008692 RID: 34450
	protected bool m_hasPierced;

	// Token: 0x04008693 RID: 34451
	private int m_healthHaverHitCount;

	// Token: 0x04008694 RID: 34452
	private bool m_cachedCollidesWithPlayer;

	// Token: 0x04008695 RID: 34453
	private bool m_cachedCollidesWithProjectiles;

	// Token: 0x04008696 RID: 34454
	private bool m_cachedCollidesWithEnemies;

	// Token: 0x04008697 RID: 34455
	private bool m_cachedDamagesWalls;

	// Token: 0x04008698 RID: 34456
	private ProjectileData m_cachedBaseData;

	// Token: 0x04008699 RID: 34457
	private BulletScriptSettings m_cachedBulletScriptSettings;

	// Token: 0x0400869A RID: 34458
	private bool m_cachedCollideWithTileMap;

	// Token: 0x0400869B RID: 34459
	private bool m_cachedCollideWithOthers;

	// Token: 0x0400869C RID: 34460
	private int m_cachedSpriteId = -1;

	// Token: 0x0400869D RID: 34461
	private PrefabPool m_spawnPool;

	// Token: 0x0400869E RID: 34462
	private bool m_isRamping;

	// Token: 0x0400869F RID: 34463
	private float m_rampTimer;

	// Token: 0x040086A0 RID: 34464
	private float m_rampDuration;

	// Token: 0x040086A1 RID: 34465
	private float m_currentRampHeight;

	// Token: 0x040086A2 RID: 34466
	private float m_startRampHeight;

	// Token: 0x040086A3 RID: 34467
	private float m_ignoreTileCollisionsTimer;

	// Token: 0x040086A4 RID: 34468
	private float m_outOfBoundsCounter;

	// Token: 0x040086A5 RID: 34469
	private bool m_isExitClippingTiles;

	// Token: 0x040086A6 RID: 34470
	private float m_exitClippingDistance;

	// Token: 0x040086A7 RID: 34471
	public static float CurrentProjectileDepth = 0.8f;

	// Token: 0x040086A8 RID: 34472
	public const float c_DefaultProjectileDepth = 0.8f;

	// Token: 0x02001606 RID: 5638
	public enum ProjectileDestroyMode
	{
		// Token: 0x040086AB RID: 34475
		Destroy,
		// Token: 0x040086AC RID: 34476
		DestroyComponent,
		// Token: 0x040086AD RID: 34477
		BecomeDebris,
		// Token: 0x040086AE RID: 34478
		None
	}

	// Token: 0x02001607 RID: 5639
	protected enum HandleDamageResult
	{
		// Token: 0x040086B0 RID: 34480
		NO_HEALTH,
		// Token: 0x040086B1 RID: 34481
		HEALTH,
		// Token: 0x040086B2 RID: 34482
		HEALTH_AND_KILLED
	}
}
