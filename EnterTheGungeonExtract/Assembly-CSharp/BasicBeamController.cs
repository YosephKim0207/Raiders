using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using UnityEngine;

// Token: 0x0200161B RID: 5659
public class BasicBeamController : BeamController
{
	// Token: 0x170013BF RID: 5055
	// (get) Token: 0x060083E6 RID: 33766 RVA: 0x00360C34 File Offset: 0x0035EE34
	// (set) Token: 0x060083E7 RID: 33767 RVA: 0x00360C3C File Offset: 0x0035EE3C
	public bool playerStatsModified { get; set; }

	// Token: 0x170013C0 RID: 5056
	// (get) Token: 0x060083E8 RID: 33768 RVA: 0x00360C48 File Offset: 0x0035EE48
	// (set) Token: 0x060083E9 RID: 33769 RVA: 0x00360C50 File Offset: 0x0035EE50
	public bool SelfUpdate { get; set; }

	// Token: 0x170013C1 RID: 5057
	// (get) Token: 0x060083EA RID: 33770 RVA: 0x00360C5C File Offset: 0x0035EE5C
	// (set) Token: 0x060083EB RID: 33771 RVA: 0x00360C64 File Offset: 0x0035EE64
	public BasicBeamController.BeamState State { get; set; }

	// Token: 0x170013C2 RID: 5058
	// (get) Token: 0x060083EC RID: 33772 RVA: 0x00360C70 File Offset: 0x0035EE70
	// (set) Token: 0x060083ED RID: 33773 RVA: 0x00360C78 File Offset: 0x0035EE78
	public float HeightOffset { get; set; }

	// Token: 0x170013C3 RID: 5059
	// (get) Token: 0x060083EE RID: 33774 RVA: 0x00360C84 File Offset: 0x0035EE84
	// (set) Token: 0x060083EF RID: 33775 RVA: 0x00360C8C File Offset: 0x0035EE8C
	public float RampHeightOffset { get; set; }

	// Token: 0x170013C4 RID: 5060
	// (get) Token: 0x060083F0 RID: 33776 RVA: 0x00360C98 File Offset: 0x0035EE98
	// (set) Token: 0x060083F1 RID: 33777 RVA: 0x00360CA0 File Offset: 0x0035EEA0
	public bool ContinueBeamArtToWall { get; set; }

	// Token: 0x170013C5 RID: 5061
	// (get) Token: 0x060083F2 RID: 33778 RVA: 0x00360CAC File Offset: 0x0035EEAC
	public float BoneSpeed
	{
		get
		{
			if (this.State == BasicBeamController.BeamState.Telegraphing)
			{
				return -1f;
			}
			return base.projectile.baseData.speed;
		}
	}

	// Token: 0x170013C6 RID: 5062
	// (get) Token: 0x060083F3 RID: 33779 RVA: 0x00360CD0 File Offset: 0x0035EED0
	public override bool ShouldUseAmmo
	{
		get
		{
			return this.State == BasicBeamController.BeamState.Firing;
		}
	}

	// Token: 0x170013C7 RID: 5063
	// (get) Token: 0x060083F4 RID: 33780 RVA: 0x00360CDC File Offset: 0x0035EEDC
	public string CurrentBeamAnimation
	{
		get
		{
			if (this.State == BasicBeamController.BeamState.Telegraphing)
			{
				return this.telegraphAnimations.beamAnimation;
			}
			if (this.State == BasicBeamController.BeamState.Dissipating)
			{
				return this.dissipateAnimations.beamAnimation;
			}
			return this.beamAnimation;
		}
	}

	// Token: 0x170013C8 RID: 5064
	// (get) Token: 0x060083F5 RID: 33781 RVA: 0x00360D14 File Offset: 0x0035EF14
	public string CurrentBeamStartAnimation
	{
		get
		{
			if (this.State == BasicBeamController.BeamState.Telegraphing)
			{
				return this.telegraphAnimations.beamStartAnimation;
			}
			if (this.State == BasicBeamController.BeamState.Dissipating)
			{
				return this.dissipateAnimations.beamStartAnimation;
			}
			return this.beamStartAnimation;
		}
	}

	// Token: 0x170013C9 RID: 5065
	// (get) Token: 0x060083F6 RID: 33782 RVA: 0x00360D4C File Offset: 0x0035EF4C
	public string CurrentBeamEndAnimation
	{
		get
		{
			if (this.State == BasicBeamController.BeamState.Telegraphing)
			{
				return this.telegraphAnimations.beamEndAnimation;
			}
			if (this.State == BasicBeamController.BeamState.Dissipating)
			{
				return this.dissipateAnimations.beamEndAnimation;
			}
			return this.beamEndAnimation;
		}
	}

	// Token: 0x170013CA RID: 5066
	// (get) Token: 0x060083F7 RID: 33783 RVA: 0x00360D84 File Offset: 0x0035EF84
	public bool UsesChargeSprite
	{
		get
		{
			return !string.IsNullOrEmpty(this.chargeAnimation);
		}
	}

	// Token: 0x170013CB RID: 5067
	// (get) Token: 0x060083F8 RID: 33784 RVA: 0x00360D94 File Offset: 0x0035EF94
	public bool UsesMuzzleSprite
	{
		get
		{
			return !string.IsNullOrEmpty(this.muzzleAnimation);
		}
	}

	// Token: 0x170013CC RID: 5068
	// (get) Token: 0x060083F9 RID: 33785 RVA: 0x00360DA4 File Offset: 0x0035EFA4
	public bool UsesImpactSprite
	{
		get
		{
			return !string.IsNullOrEmpty(this.impactAnimation);
		}
	}

	// Token: 0x170013CD RID: 5069
	// (get) Token: 0x060083FA RID: 33786 RVA: 0x00360DB4 File Offset: 0x0035EFB4
	public bool UsesBeamStartAnimation
	{
		get
		{
			return !string.IsNullOrEmpty(this.CurrentBeamStartAnimation);
		}
	}

	// Token: 0x170013CE RID: 5070
	// (get) Token: 0x060083FB RID: 33787 RVA: 0x00360DC4 File Offset: 0x0035EFC4
	public bool UsesBeamEndAnimation
	{
		get
		{
			return !string.IsNullOrEmpty(this.CurrentBeamEndAnimation);
		}
	}

	// Token: 0x170013CF RID: 5071
	// (get) Token: 0x060083FC RID: 33788 RVA: 0x00360DD4 File Offset: 0x0035EFD4
	public bool UsesBones
	{
		get
		{
			return this.boneType == BasicBeamController.BeamBoneType.Projectile || this.IsHoming || this.ProjectileAndBeamMotionModule != null;
		}
	}

	// Token: 0x170013D0 RID: 5072
	// (get) Token: 0x060083FD RID: 33789 RVA: 0x00360DFC File Offset: 0x0035EFFC
	public bool IsConnected
	{
		get
		{
			return this.State != BasicBeamController.BeamState.Disconnected;
		}
	}

	// Token: 0x170013D1 RID: 5073
	// (get) Token: 0x060083FE RID: 33790 RVA: 0x00360E0C File Offset: 0x0035F00C
	public float HomingRadius
	{
		get
		{
			return base.ChanceBasedHomingRadius + this.homingRadius;
		}
	}

	// Token: 0x170013D2 RID: 5074
	// (get) Token: 0x060083FF RID: 33791 RVA: 0x00360E1C File Offset: 0x0035F01C
	public float HomingAngularVelocity
	{
		get
		{
			float num = base.ChanceBasedHomingAngularVelocity + this.homingAngularVelocity;
			if (this.BoneSpeed < 0f)
			{
				return num;
			}
			return num * (this.BoneSpeed / 40f);
		}
	}

	// Token: 0x170013D3 RID: 5075
	// (get) Token: 0x06008400 RID: 33792 RVA: 0x00360E58 File Offset: 0x0035F058
	public bool IsHoming
	{
		get
		{
			return this.HomingRadius > 0f && this.HomingAngularVelocity > 0f;
		}
	}

	// Token: 0x170013D4 RID: 5076
	// (get) Token: 0x06008401 RID: 33793 RVA: 0x00360E7C File Offset: 0x0035F07C
	// (set) Token: 0x06008402 RID: 33794 RVA: 0x00360E84 File Offset: 0x0035F084
	public SpeculativeRigidbody ReflectedFromRigidbody { get; set; }

	// Token: 0x170013D5 RID: 5077
	// (get) Token: 0x06008403 RID: 33795 RVA: 0x00360E90 File Offset: 0x0035F090
	// (set) Token: 0x06008404 RID: 33796 RVA: 0x00360E98 File Offset: 0x0035F098
	public bool ShowImpactOnMaxDistanceEnd { get; set; }

	// Token: 0x170013D6 RID: 5078
	// (get) Token: 0x06008405 RID: 33797 RVA: 0x00360EA4 File Offset: 0x0035F0A4
	// (set) Token: 0x06008406 RID: 33798 RVA: 0x00360EAC File Offset: 0x0035F0AC
	public bool IsBlackBullet { get; set; }

	// Token: 0x170013D7 RID: 5079
	// (get) Token: 0x06008407 RID: 33799 RVA: 0x00360EB8 File Offset: 0x0035F0B8
	// (set) Token: 0x06008408 RID: 33800 RVA: 0x00360EC0 File Offset: 0x0035F0C0
	public ProjectileAndBeamMotionModule ProjectileAndBeamMotionModule { get; set; }

	// Token: 0x170013D8 RID: 5080
	// (get) Token: 0x06008409 RID: 33801 RVA: 0x00360ECC File Offset: 0x0035F0CC
	// (set) Token: 0x0600840A RID: 33802 RVA: 0x00360ED4 File Offset: 0x0035F0D4
	public float ProjectileScale
	{
		get
		{
			return this.m_projectileScale;
		}
		set
		{
			this.m_projectileScale = value;
		}
	}

	// Token: 0x170013D9 RID: 5081
	// (get) Token: 0x0600840B RID: 33803 RVA: 0x00360EE0 File Offset: 0x0035F0E0
	public float ApproximateDistance
	{
		get
		{
			return this.m_currentBeamDistance;
		}
	}

	// Token: 0x0600840C RID: 33804 RVA: 0x00360EE8 File Offset: 0x0035F0E8
	public void Start()
	{
		if (this.UsesDispersalParticles && this.m_dispersalParticles == null)
		{
			this.m_dispersalParticles = GlobalDispersalParticleManager.GetSystemForPrefab(this.DispersalParticleSystemPrefab);
		}
		this.m_beamQuadPixelWidth = ((this.overrideBeamQuadPixelWidth <= 0) ? 4 : this.overrideBeamQuadPixelWidth);
		this.m_beamQuadUnitWidth = (float)this.m_beamQuadPixelWidth / 16f;
		this.m_sqrNewBoneThreshold = this.m_beamQuadUnitWidth * this.m_beamQuadUnitWidth * 2f * 2f;
		this.m_beamSprite = base.gameObject.GetComponent<tk2dTiledSprite>();
		this.m_beamSprite.renderer.sortingLayerName = "Player";
		this.m_beamSpriteDimensions = this.m_beamSprite.GetUntrimmedBounds().size;
		this.m_beamSprite.dimensions = new Vector2(0f, this.m_beamSpriteDimensions.y * 16f);
		base.spriteAnimator.Play(this.beamAnimation);
		this.m_beamSprite.HeightOffGround = BasicBeamController.CurrentBeamHeightOffGround + this.HeightOffset;
		this.m_beamSprite.IsPerpendicular = false;
		this.m_beamSprite.usesOverrideMaterial = true;
		PlayerController playerController = base.projectile.Owner as PlayerController;
		if (playerController)
		{
			this.m_projectileScale = playerController.BulletScaleModifier;
		}
		if (this.IsConnected)
		{
			this.m_muzzleTransform = base.transform.Find("beam muzzle flare");
			if (this.m_muzzleTransform)
			{
				this.m_beamMuzzleSprite = this.m_muzzleTransform.GetComponent<tk2dSprite>();
				this.m_beamMuzzleAnimator = this.m_muzzleTransform.GetComponent<tk2dSpriteAnimator>();
			}
			if (this.UsesChargeSprite || this.UsesMuzzleSprite)
			{
				if (!this.m_muzzleTransform)
				{
					GameObject gameObject = new GameObject("beam muzzle flare");
					this.m_muzzleTransform = gameObject.transform;
					this.m_muzzleTransform.parent = base.transform;
					this.m_muzzleTransform.localPosition = new Vector3(0f, 0f, 0.05f);
					this.m_beamMuzzleSprite = gameObject.AddComponent<tk2dSprite>();
					this.m_beamMuzzleSprite.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
					this.m_beamMuzzleAnimator = gameObject.AddComponent<tk2dSpriteAnimator>();
					this.m_beamMuzzleAnimator.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
					this.m_beamMuzzleAnimator.Library = base.spriteAnimator.Library;
					this.m_beamSprite.AttachRenderer(this.m_beamMuzzleSprite);
					this.m_beamMuzzleSprite.HeightOffGround = 0.05f;
					this.m_beamMuzzleSprite.IsPerpendicular = false;
					this.m_beamMuzzleSprite.usesOverrideMaterial = true;
				}
				this.m_muzzleTransform.localScale = new Vector3(this.m_projectileScale, this.m_projectileScale, 1f);
			}
			if (this.m_muzzleTransform)
			{
				this.m_muzzleTransform.gameObject.SetActive(false);
			}
			if (this.usesChargeDelay)
			{
				base.renderer.enabled = false;
				this.State = BasicBeamController.BeamState.Charging;
				if (this.UsesChargeSprite)
				{
					this.m_beamMuzzleAnimator.Play(this.chargeAnimation);
					this.m_muzzleTransform.gameObject.SetActive(true);
				}
			}
			else if (this.usesTelegraph)
			{
				this.State = BasicBeamController.BeamState.Telegraphing;
				base.spriteAnimator.Play(this.CurrentBeamAnimation);
			}
			else
			{
				this.State = BasicBeamController.BeamState.Firing;
				if (this.UsesMuzzleSprite)
				{
					this.m_beamMuzzleAnimator.Play(this.muzzleAnimation);
					this.m_muzzleTransform.gameObject.SetActive(true);
				}
			}
			AIActor aiactor = base.Owner as AIActor;
			if (aiactor && aiactor.IsBlackPhantom)
			{
				this.BecomeBlackBullet();
			}
			if (GameManager.AUDIO_ENABLED && !string.IsNullOrEmpty(this.startAudioEvent))
			{
				AkSoundEngine.PostEvent(this.startAudioEvent, base.gameObject);
			}
		}
		else
		{
			this.m_muzzleTransform = base.transform.Find("beam muzzle flare");
			if (this.m_muzzleTransform)
			{
				this.m_muzzleTransform.gameObject.SetActive(false);
			}
		}
		if (this.UsesImpactSprite)
		{
			this.m_impactTransform = base.transform.Find("beam impact vfx");
			if (this.m_impactTransform)
			{
				this.m_impactSprite = this.m_impactTransform.GetComponent<tk2dSprite>();
				this.m_impactAnimator = this.m_impactTransform.GetComponent<tk2dSpriteAnimator>();
			}
			else
			{
				GameObject gameObject2 = new GameObject("beam impact vfx");
				this.m_impactTransform = gameObject2.transform;
				this.m_impactTransform.parent = base.transform;
				this.m_impactTransform.localPosition = new Vector3(0f, 0f, 0.05f);
				this.m_impactSprite = gameObject2.AddComponent<tk2dSprite>();
				this.m_impactSprite.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
				this.m_impactAnimator = gameObject2.AddComponent<tk2dSpriteAnimator>();
				this.m_impactAnimator.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
				this.m_impactAnimator.Library = base.spriteAnimator.Library;
				this.m_beamSprite.AttachRenderer(this.m_impactSprite);
				this.m_impactSprite.HeightOffGround = 0.05f;
				this.m_impactSprite.IsPerpendicular = true;
				this.m_impactSprite.usesOverrideMaterial = true;
			}
			this.m_impactTransform.localScale = new Vector3(this.m_projectileScale, this.m_projectileScale, 1f);
			this.m_impact2Transform = base.transform.Find("beam impact vfx 2");
			if (this.m_impact2Transform)
			{
				this.m_impact2Sprite = this.m_impact2Transform.GetComponent<tk2dSprite>();
				this.m_impact2Animator = this.m_impact2Transform.GetComponent<tk2dSpriteAnimator>();
			}
			else
			{
				GameObject gameObject3 = new GameObject("beam impact vfx 2");
				this.m_impact2Transform = gameObject3.transform;
				this.m_impact2Transform.parent = base.transform;
				this.m_impact2Transform.localPosition = new Vector3(0f, 0f, 0.05f);
				this.m_impact2Sprite = gameObject3.AddComponent<tk2dSprite>();
				this.m_impact2Sprite.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
				this.m_impact2Animator = gameObject3.AddComponent<tk2dSpriteAnimator>();
				this.m_impact2Animator.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
				this.m_impact2Animator.Library = base.spriteAnimator.Library;
				this.m_beamSprite.AttachRenderer(this.m_impact2Sprite);
				this.m_impact2Sprite.HeightOffGround = 0.05f;
				this.m_impact2Sprite.IsPerpendicular = true;
				this.m_impact2Sprite.usesOverrideMaterial = true;
			}
			this.m_impact2Transform.localScale = new Vector3(this.m_projectileScale, this.m_projectileScale, 1f);
			if (!this.m_impactAnimator.IsPlaying(this.impactAnimation))
			{
				this.m_impactAnimator.Play(this.impactAnimation);
			}
			if (!this.m_impact2Animator.IsPlaying(this.impactAnimation))
			{
				this.m_impact2Animator.Play(this.impactAnimation);
			}
			if (this.m_impactTransform)
			{
				this.m_impactTransform.gameObject.SetActive(false);
			}
			if (this.m_impact2Transform)
			{
				this.m_impact2Transform.gameObject.SetActive(false);
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (child.name.StartsWith("beam pierce impact vfx"))
				{
					if (this.m_pierceImpactSprites == null)
					{
						this.m_pierceImpactSprites = new List<tk2dBaseSprite>();
					}
					this.m_pierceImpactSprites.Add(child.GetComponent<tk2dBaseSprite>());
					this.m_pierceImpactSprites[this.m_pierceImpactSprites.Count - 1].gameObject.SetActive(false);
				}
			}
		}
		this.m_beamSprite.OverrideGetTiledSpriteGeomDesc = new tk2dTiledSprite.OverrideGetTiledSpriteGeomDescDelegate(this.GetTiledSpriteGeomDesc);
		this.m_beamSprite.OverrideSetTiledSpriteGeom = new tk2dTiledSprite.OverrideSetTiledSpriteGeomDelegate(this.SetTiledSpriteGeom);
		tk2dSpriteDefinition currentSpriteDef = this.m_beamSprite.GetCurrentSpriteDef();
		this.m_beamSpriteSubtileWidth = Mathf.RoundToInt(currentSpriteDef.untrimmedBoundsDataExtents.x / currentSpriteDef.texelSize.x) / this.m_beamQuadPixelWidth;
		this.m_beamSpriteUnitWidth = (float)Mathf.RoundToInt(currentSpriteDef.untrimmedBoundsDataExtents.x / currentSpriteDef.texelSize.x) / 16f;
		if (this.m_bones == null)
		{
			this.m_bones = new LinkedList<BasicBeamController.BeamBone>();
			this.m_bones.AddFirst(new BasicBeamController.BeamBone(0f, 0f, this.m_beamSpriteSubtileWidth - 1));
			this.m_bones.AddLast(new BasicBeamController.BeamBone(0f, 0f, -1));
			this.m_uvOffset = 1f;
			if (this.boneType == BasicBeamController.BeamBoneType.Projectile)
			{
				this.m_bones.First.Value.Position = base.Origin;
				this.m_bones.First.Value.Velocity = base.Direction.normalized * this.BoneSpeed;
				this.m_bones.Last.Value.Position = base.Origin;
				this.m_bones.Last.Value.Velocity = base.Direction.normalized * this.BoneSpeed;
			}
		}
		else
		{
			this.m_beamSprite.ForceBuild();
			this.m_beamSprite.UpdateZDepth();
		}
		this.m_beamGoopModifier = base.gameObject.GetComponent<GoopModifier>();
		if (this.IsConnected && base.Owner is PlayerController && !this.SkipPostProcessing)
		{
			(base.Owner as PlayerController).DoPostProcessBeam(this);
		}
		if (this.IsConnected && base.Owner is AIActor && !this.SkipPostProcessing)
		{
			AIActor aiactor2 = base.Owner as AIActor;
			if (aiactor2.CompanionOwner)
			{
				aiactor2.CompanionOwner.DoPostProcessBeam(this);
				this.ProjectileScale *= aiactor2.CompanionOwner.BulletScaleModifier;
			}
		}
		this.ProjectileAndBeamMotionModule = base.projectile.OverrideMotionModule as ProjectileAndBeamMotionModule;
	}

	// Token: 0x0600840D RID: 33805 RVA: 0x0036198C File Offset: 0x0035FB8C
	public void Update()
	{
		if (this.State == BasicBeamController.BeamState.Disconnected || this.State == BasicBeamController.BeamState.Dissipating)
		{
			if (this.boneType != BasicBeamController.BeamBoneType.Straight && ((this.m_bones.Count == 2 && Mathf.Approximately(this.m_bones.First.Value.PosX, this.m_bones.Last.Value.PosX)) || this.m_bones.Count < 2))
			{
				this.DestroyBeam();
			}
			if (this.State == BasicBeamController.BeamState.Dissipating && this.m_dissipateTimer >= this.dissipateTime)
			{
				this.DestroyBeam();
			}
		}
		if (this.SelfUpdate)
		{
			this.FrameUpdate();
		}
	}

	// Token: 0x0600840E RID: 33806 RVA: 0x00361A4C File Offset: 0x0035FC4C
	protected override void OnDestroy()
	{
		if (this.m_enemyKnockback)
		{
			this.m_enemyKnockback.EndContinuousKnockback(this.m_enemyKnockbackId);
		}
		this.m_enemyKnockback = null;
		this.m_enemyKnockbackId = -1;
		if (this.m_reflectedBeam)
		{
			this.m_reflectedBeam.CeaseAttack();
			this.m_reflectedBeam = null;
		}
		base.OnDestroy();
	}

	// Token: 0x0600840F RID: 33807 RVA: 0x00361AB0 File Offset: 0x0035FCB0
	public void ForceChargeTimer(float val)
	{
		this.m_chargeTimer = val;
	}

	// Token: 0x06008410 RID: 33808 RVA: 0x00361ABC File Offset: 0x0035FCBC
	public void FrameUpdate()
	{
		try
		{
			SpeculativeRigidbody[] ignoreRigidbodies = base.GetIgnoreRigidbodies();
			int num = 0;
			if (this.State == BasicBeamController.BeamState.Charging)
			{
				this.m_chargeTimer += BraveTime.DeltaTime;
				PixelCollider pixelCollider;
				this.HandleBeamFrame(base.Origin, base.Direction, base.HitsPlayers, base.HitsEnemies, false, out pixelCollider, ignoreRigidbodies);
			}
			else if (this.State == BasicBeamController.BeamState.Telegraphing)
			{
				this.m_telegraphTimer += BraveTime.DeltaTime;
				PixelCollider pixelCollider;
				this.HandleBeamFrame(base.Origin, base.Direction, base.HitsPlayers, base.HitsEnemies, false, out pixelCollider, ignoreRigidbodies);
			}
			else if (this.State == BasicBeamController.BeamState.Dissipating)
			{
				this.m_dissipateTimer += BraveTime.DeltaTime;
				PixelCollider pixelCollider;
				this.HandleBeamFrame(base.Origin, base.Direction, base.HitsPlayers, base.HitsEnemies, false, out pixelCollider, ignoreRigidbodies);
			}
			else if (this.State == BasicBeamController.BeamState.Firing || this.State == BasicBeamController.BeamState.Disconnected)
			{
				PixelCollider pixelCollider;
				List<SpeculativeRigidbody> list = this.HandleBeamFrame(base.Origin, base.Direction, base.HitsPlayers, base.HitsEnemies, false, out pixelCollider, ignoreRigidbodies);
				if (list != null && list.Count > 0)
				{
					float num2 = base.projectile.baseData.damage + base.DamageModifier;
					PlayerController playerController = base.projectile.Owner as PlayerController;
					if (playerController)
					{
						num2 *= playerController.stats.GetStatValue(PlayerStats.StatType.RateOfFire);
					}
					if (base.ChanceBasedShadowBullet)
					{
						num2 *= 2f;
					}
					for (int i = 0; i < list.Count; i++)
					{
						SpeculativeRigidbody speculativeRigidbody = list[i];
						if (this.OverrideHitChecks != null)
						{
							this.OverrideHitChecks(speculativeRigidbody, base.Direction);
						}
						else if (speculativeRigidbody)
						{
							if (base.Owner is AIActor)
							{
								if (speculativeRigidbody.healthHaver)
								{
									if (speculativeRigidbody.gameActor && speculativeRigidbody.gameActor is PlayerController)
									{
										bool flag = base.Owner && (base.Owner as AIActor).IsBlackPhantom;
										bool isAlive = speculativeRigidbody.healthHaver.IsAlive;
										float num3 = ((base.projectile.baseData.damage != 0f) ? 0.5f : 0f);
										HealthHaver healthHaver = speculativeRigidbody.healthHaver;
										float num4 = num3;
										Vector2 vector = base.Direction;
										string text = (base.Owner as AIActor).GetActorName();
										PixelCollider pixelCollider2 = pixelCollider;
										healthHaver.ApplyDamage(num4, vector, text, CoreDamageTypes.None, (!flag) ? DamageCategory.Normal : DamageCategory.BlackBullet, false, pixelCollider2, false);
										bool flag2 = isAlive && speculativeRigidbody.healthHaver.IsDead;
										if (base.projectile.OnHitEnemy != null)
										{
											base.projectile.OnHitEnemy(base.projectile, speculativeRigidbody, flag2);
										}
									}
									else
									{
										num2 = ((!speculativeRigidbody.aiActor) ? base.projectile.baseData.damage : ProjectileData.FixedFallbackDamageToEnemies) + base.DamageModifier;
										bool isAlive2 = speculativeRigidbody.healthHaver.IsAlive;
										HealthHaver healthHaver2 = speculativeRigidbody.healthHaver;
										float num4 = num2 * BraveTime.DeltaTime;
										Vector2 vector = base.Direction;
										string text = (base.Owner as AIActor).GetActorName();
										PixelCollider pixelCollider2 = pixelCollider;
										healthHaver2.ApplyDamage(num4, vector, text, CoreDamageTypes.None, DamageCategory.Normal, false, pixelCollider2, false);
										bool flag3 = isAlive2 && speculativeRigidbody.healthHaver.IsDead;
										if (base.projectile.OnHitEnemy != null)
										{
											base.projectile.OnHitEnemy(base.projectile, speculativeRigidbody, flag3);
										}
									}
								}
							}
							else if (speculativeRigidbody.healthHaver)
							{
								float num5 = num2;
								if (num >= 1)
								{
									int num6 = Mathf.Clamp(num - 1, 0, GameManager.Instance.PierceDamageScaling.Length - 1);
									num5 *= GameManager.Instance.PierceDamageScaling[num6];
								}
								if (speculativeRigidbody.healthHaver.IsBoss && base.projectile)
								{
									num5 *= base.projectile.BossDamageMultiplier;
								}
								if (base.projectile && base.projectile.BlackPhantomDamageMultiplier != 1f && speculativeRigidbody.aiActor && speculativeRigidbody.aiActor.IsBlackPhantom)
								{
									num5 *= base.projectile.BlackPhantomDamageMultiplier;
								}
								bool isAlive3 = speculativeRigidbody.healthHaver.IsAlive;
								string text2 = string.Empty;
								if (base.projectile)
								{
									text2 = base.projectile.OwnerName;
								}
								else
								{
									text2 = ((!(base.Owner is AIActor)) ? base.Owner.ActorName : (base.Owner as AIActor).GetActorName());
								}
								float num7 = num5 * BraveTime.DeltaTime;
								if (this.angularKnockback)
								{
									BasicBeamController.AngularKnockbackTier knockbackTier = this.GetKnockbackTier();
									if (knockbackTier != null)
									{
										num7 = num2 * knockbackTier.damageMultiplier;
										SpeculativeRigidbody specRigidbody = speculativeRigidbody.healthHaver.specRigidbody;
										if (Array.IndexOf<SpeculativeRigidbody>(ignoreRigidbodies, specRigidbody) >= 0)
										{
											num7 = 0f;
										}
										if (num7 > 0f)
										{
											AkSoundEngine.PostEvent("Play_WPN_woodbeam_impact_01", base.gameObject);
											knockbackTier.hitRigidbodyVFX.SpawnAtPosition(speculativeRigidbody.UnitCenter, 0f, null, null, null, null, false, null, null, false);
											if (knockbackTier.additionalAmmoCost > 0 && base.Gun)
											{
												base.Gun.LoseAmmo(knockbackTier.additionalAmmoCost);
											}
											if (specRigidbody)
											{
												this.TimedIgnoreRigidbodies.Add(Tuple.Create<SpeculativeRigidbody, float>(specRigidbody, knockbackTier.ignoreHitRigidbodyTime));
											}
											else
											{
												this.TimedIgnoreRigidbodies.Add(Tuple.Create<SpeculativeRigidbody, float>(speculativeRigidbody, knockbackTier.ignoreHitRigidbodyTime));
											}
										}
									}
								}
								HealthHaver healthHaver3 = speculativeRigidbody.healthHaver;
								float num4 = num7;
								Vector2 vector = base.Direction;
								string text = text2;
								CoreDamageTypes damageTypes = base.projectile.damageTypes;
								PixelCollider pixelCollider2 = pixelCollider;
								healthHaver3.ApplyDamage(num4, vector, text, damageTypes, DamageCategory.Normal, false, pixelCollider2, false);
								bool flag4 = isAlive3 && speculativeRigidbody.healthHaver.IsDead;
								if (base.projectile.OnHitEnemy != null)
								{
									base.projectile.OnHitEnemy(base.projectile, speculativeRigidbody, flag4);
								}
								num++;
							}
							if (speculativeRigidbody.majorBreakable)
							{
								speculativeRigidbody.majorBreakable.ApplyDamage(num2 * BraveTime.DeltaTime, base.Direction, false, false, false);
								Chest component = speculativeRigidbody.GetComponent<Chest>();
								if (component && BraveUtility.EnumFlagsContains((uint)base.projectile.damageTypes, 32U) > 0 && component.ChestIdentifier == Chest.SpecialChestIdentifier.SECRET_RAINBOW)
								{
									component.RevealSecretRainbow();
								}
							}
							if (speculativeRigidbody.gameActor)
							{
								GameActor gameActor = speculativeRigidbody.gameActor;
								gameActor.BeamStatusAmount += BraveTime.DeltaTime * 1.5f;
								if (gameActor.BeamStatusAmount > this.TimeToStatus || base.Owner is AIActor)
								{
									if (base.projectile.AppliesSpeedModifier && UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.statusEffectChance, BraveTime.DeltaTime))
									{
										gameActor.ApplyEffect(base.projectile.speedEffect, 1f, null);
									}
									if (base.projectile.AppliesPoison && UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.statusEffectChance, BraveTime.DeltaTime))
									{
										gameActor.ApplyEffect(base.projectile.healthEffect, 1f, null);
									}
									if (base.projectile.AppliesCharm && UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.statusEffectChance, BraveTime.DeltaTime))
									{
										gameActor.ApplyEffect(base.projectile.charmEffect, 1f, null);
									}
									if (base.projectile.AppliesFire && UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.statusEffectChance, BraveTime.DeltaTime))
									{
										if (gameActor is PlayerController)
										{
											if (base.projectile.fireEffect.AffectsPlayers)
											{
												(gameActor as PlayerController).IsOnFire = true;
											}
										}
										else
										{
											gameActor.ApplyEffect(base.projectile.fireEffect, 1f, null);
										}
									}
									if (base.projectile.AppliesStun && gameActor.behaviorSpeculator && UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.statusEffectChance, BraveTime.DeltaTime))
									{
										gameActor.behaviorSpeculator.Stun(base.projectile.AppliedStunDuration, true);
									}
								}
								if (base.projectile.AppliesFreeze)
								{
									gameActor.ApplyEffect(base.projectile.freezeEffect, BraveTime.DeltaTime * this.statusEffectAccumulateMultiplier, null);
								}
								if (base.projectile.AppliesBleed)
								{
									gameActor.ApplyEffect(base.projectile.bleedEffect, BraveTime.DeltaTime * this.statusEffectAccumulateMultiplier, base.projectile);
								}
							}
							if (this.m_beamGoopModifier)
							{
								if (this.m_beamGoopModifier.OnlyGoopOnEnemyCollision)
								{
									if (speculativeRigidbody.aiActor != null)
									{
										this.m_beamGoopModifier.SpawnCollisionGoop(speculativeRigidbody.UnitBottomCenter);
									}
								}
								else
								{
									this.m_beamGoopModifier.SpawnCollisionGoop(speculativeRigidbody.UnitBottomCenter);
								}
							}
							if (speculativeRigidbody.OnHitByBeam != null)
							{
								speculativeRigidbody.OnHitByBeam(this);
							}
							if (base.Owner is PlayerController)
							{
								(base.Owner as PlayerController).DoPostProcessBeamTick(this, speculativeRigidbody, BraveTime.DeltaTime);
							}
							if (base.Owner is AIActor)
							{
								AIActor aiactor = base.Owner as AIActor;
								if (aiactor.CompanionOwner)
								{
									aiactor.CompanionOwner.DoPostProcessBeamTick(this, speculativeRigidbody, BraveTime.DeltaTime);
									if (aiactor.CompanionOwner.CurrentGun && aiactor.CompanionOwner.CurrentGun.LuteCompanionBuffActive)
									{
										if (this.m_currentLuteScaleModifier != 1.75f)
										{
											this.m_currentLuteScaleModifier = 1.75f;
											this.ProjectileScale *= this.m_currentLuteScaleModifier;
										}
									}
									else if (this.m_currentLuteScaleModifier != 1f)
									{
										this.ProjectileScale /= this.m_currentLuteScaleModifier;
										this.m_currentLuteScaleModifier = 1f;
									}
								}
							}
						}
					}
				}
				if (this.angularKnockback)
				{
					if (list != null && list.Count > 0)
					{
						BasicBeamController.AngularKnockbackTier knockbackTier2 = this.GetKnockbackTier();
						if (knockbackTier2 != null)
						{
							for (int j = 0; j < list.Count; j++)
							{
								KnockbackDoer knockbackDoer = list[j].knockbackDoer;
								if (knockbackDoer)
								{
									Vector2 vector2 = knockbackDoer.specRigidbody.UnitCenter - base.Origin;
									if (knockbackTier2.minAngularSpeed > 0f)
									{
										if (this.averageAngularVelocity > 0f)
										{
											vector2 = vector2.Rotate(90f);
										}
										else if (this.averageAngularVelocity < 0f)
										{
											vector2 = vector2.Rotate(-90f);
										}
									}
									knockbackDoer.ApplyKnockback(vector2, base.projectile.baseData.force * knockbackTier2.knockbackMultiplier, false);
								}
							}
						}
					}
				}
				else
				{
					KnockbackDoer knockbackDoer2 = ((list == null || list.Count <= 0) ? null : list[0].knockbackDoer);
					if (knockbackDoer2 != this.m_enemyKnockback)
					{
						if (this.m_enemyKnockback)
						{
							this.m_enemyKnockback.EndContinuousKnockback(this.m_enemyKnockbackId);
						}
						if (knockbackDoer2)
						{
							this.m_enemyKnockbackId = knockbackDoer2.ApplyContinuousKnockback(knockbackDoer2.specRigidbody.UnitCenter - base.Origin, base.projectile.baseData.force);
						}
						this.m_enemyKnockback = knockbackDoer2;
					}
					if (this.m_beamGoopModifier != null)
					{
						this.HandleGoopFrame(this.m_beamGoopModifier);
					}
					this.HandleIgnitionAndFreezing();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(string.Format("Caught BasicBeamController.HandleBeamFrame() exception. i={0}, ex={1}", this.exceptionTracker, ex.ToString()));
		}
	}

	// Token: 0x06008411 RID: 33809 RVA: 0x0036279C File Offset: 0x0036099C
	private BasicBeamController.AngularKnockbackTier GetKnockbackTier()
	{
		BasicBeamController.AngularKnockbackTier angularKnockbackTier = null;
		for (int i = 0; i < this.angularKnockbackTiers.Count; i++)
		{
			BasicBeamController.AngularKnockbackTier angularKnockbackTier2 = this.angularKnockbackTiers[i];
			if (angularKnockbackTier2.minAngularSpeed >= Mathf.Abs(this.averageAngularVelocity))
			{
				break;
			}
			angularKnockbackTier = this.angularKnockbackTiers[i];
		}
		return angularKnockbackTier;
	}

	// Token: 0x06008412 RID: 33810 RVA: 0x00362804 File Offset: 0x00360A04
	public List<SpeculativeRigidbody> HandleBeamFrame(Vector2 origin, Vector2 direction, bool hitsPlayers, bool hitsEnemies, bool hitsProjectiles, out PixelCollider pixelCollider, params SpeculativeRigidbody[] ignoreRigidbodies)
	{
		this.exceptionTracker = 0;
		float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		List<SpeculativeRigidbody> list = new List<SpeculativeRigidbody>();
		pixelCollider = null;
		if (!this.m_beamSprite)
		{
			return list;
		}
		if (this.ProjectileAndBeamMotionModule is OrbitProjectileMotionModule)
		{
			this.m_currentBeamDistance = 30f + 6.2831855f * (this.ProjectileAndBeamMotionModule as OrbitProjectileMotionModule).BeamOrbitRadius;
		}
		float num2 = 0f;
		if (this.angularKnockback)
		{
			float num3 = direction.ToAngle();
			if (num3 <= 155f && num3 >= 25f)
			{
				num2 = -1f;
				if (!this.m_hasToggledGunOutline && base.Gun && base.Gun.GetSprite())
				{
					this.m_hasToggledGunOutline = true;
					SpriteOutlineManager.RemoveOutlineFromSprite(base.Gun.GetSprite(), false);
				}
			}
			else if (this.m_hasToggledGunOutline)
			{
				this.m_hasToggledGunOutline = false;
				SpriteOutlineManager.AddOutlineToSprite(base.Gun.GetSprite(), Color.black);
			}
		}
		this.m_beamSprite.HeightOffGround = BasicBeamController.CurrentBeamHeightOffGround + this.HeightOffset + num2;
		if (this.IsConnected && base.Owner is PlayerController)
		{
			bool flag = base.HandleChanceTick();
			if (flag && this.m_chanceTick < -999f)
			{
				this.m_bones.First.Value.HomingRadius = this.HomingRadius;
				this.m_bones.First.Value.HomingAngularVelocity = this.HomingAngularVelocity;
				this.m_bones.Last.Value.HomingRadius = this.HomingRadius;
				this.m_bones.Last.Value.HomingAngularVelocity = this.HomingAngularVelocity;
				this.m_chanceTick = 1f;
			}
		}
		float num4 = origin.y - BasicBeamController.CurrentBeamHeightOffGround;
		base.transform.position = origin.WithZ(num4).Quantize(0.0625f);
		if (this.m_previousAngle == null)
		{
			this.m_previousAngle = new float?(num);
		}
		if (this.angularKnockback && BraveTime.DeltaTime > 0f)
		{
			float num5 = BraveMathCollege.ClampAngle180(num - this.m_previousAngle.Value);
			float num6 = num5 / BraveTime.DeltaTime;
			this.averageAngularVelocity = BraveMathCollege.MovingAverageSpeed(this.averageAngularVelocity, num6, BraveTime.DeltaTime, this.angularSpeedAvgWindow);
		}
		if (this.State == BasicBeamController.BeamState.Charging)
		{
			if (this.UsesChargeSprite && this.rotateChargeAnimation)
			{
				this.m_beamMuzzleAnimator.transform.rotation = Quaternion.Euler(0f, 0f, direction.ToAngle());
			}
			if (this.m_chargeTimer >= this.chargeDelay)
			{
				base.GetComponent<Renderer>().enabled = true;
				if (this.UsesChargeSprite && this.rotateChargeAnimation)
				{
					this.m_beamMuzzleAnimator.transform.rotation = Quaternion.identity;
				}
				if (this.boneType == BasicBeamController.BeamBoneType.Projectile)
				{
					this.m_bones.First.Value.Position = base.Origin;
					this.m_bones.First.Value.Velocity = base.Direction.normalized * this.BoneSpeed;
					this.m_bones.Last.Value.Position = base.Origin;
					this.m_bones.Last.Value.Velocity = base.Direction.normalized * this.BoneSpeed;
				}
				if (this.usesTelegraph)
				{
					this.State = BasicBeamController.BeamState.Telegraphing;
					if (this.m_beamMuzzleSprite)
					{
						this.m_muzzleTransform.gameObject.SetActive(false);
					}
				}
				else
				{
					if (this.UsesMuzzleSprite)
					{
						this.m_muzzleTransform.gameObject.SetActive(true);
						this.m_beamMuzzleAnimator.Play(this.muzzleAnimation);
					}
					else if (this.m_beamMuzzleSprite)
					{
						this.m_muzzleTransform.gameObject.SetActive(false);
					}
					this.State = BasicBeamController.BeamState.Firing;
				}
			}
		}
		else
		{
			if (this.State == BasicBeamController.BeamState.Telegraphing && this.m_telegraphTimer > this.telegraphTime)
			{
				this.State = BasicBeamController.BeamState.Firing;
				base.spriteAnimator.Play(this.CurrentBeamAnimation);
				if (this.UsesMuzzleSprite)
				{
					this.m_beamMuzzleAnimator.Play(this.muzzleAnimation);
					this.m_muzzleTransform.gameObject.SetActive(true);
				}
			}
			int num7 = 0;
			if (this.boneType == BasicBeamController.BeamBoneType.Straight)
			{
				if (this.BoneSpeed > 0f)
				{
					float num8 = BraveTime.DeltaTime * this.BoneSpeed;
					this.m_currentBeamDistance = Mathf.Min(this.m_currentBeamDistance + num8, base.projectile.baseData.range);
					for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
					{
						linkedListNode.Value.PosX = Mathf.Min(linkedListNode.Value.PosX + num8, this.m_currentBeamDistance);
					}
					this.m_bones.First.Value.PosX = Mathf.Max(0f, this.m_bones.First.Next.Value.PosX - this.m_beamQuadUnitWidth);
					while (this.m_bones.First.Value.PosX != 0f)
					{
						int num9 = this.m_bones.First.Value.SubtileNum - 1;
						if (num9 < 0)
						{
							num9 = this.m_beamSpriteSubtileWidth - 1;
						}
						this.m_bones.AddFirst(new BasicBeamController.BeamBone(Mathf.Max(0f, this.m_bones.First.Value.PosX - this.m_beamQuadUnitWidth), 0f, num9));
						num7++;
					}
					while (this.m_bones.Count > 2 && this.m_bones.Last.Previous.Value.PosX == this.m_currentBeamDistance)
					{
						this.m_bones.RemoveLast();
					}
					if (this.TileType == BasicBeamController.BeamTileType.Flowing)
					{
						this.m_uvOffset -= num8 / this.m_beamSpriteUnitWidth;
						while (this.m_uvOffset < 0f)
						{
							this.m_uvOffset += 1f;
						}
					}
				}
				else if (this.BoneSpeed <= 0f)
				{
					this.m_currentBeamDistance = base.projectile.baseData.range;
				}
			}
			else if (this.boneType == BasicBeamController.BeamBoneType.Projectile)
			{
				float num8 = BraveTime.DeltaTime * this.BoneSpeed;
				this.m_currentBeamDistance = Mathf.Min(this.m_currentBeamDistance + num8, base.projectile.baseData.range);
				LinkedListNode<BasicBeamController.BeamBone> linkedListNode2 = this.m_bones.First;
				bool flag2 = false;
				while (linkedListNode2 != null)
				{
					linkedListNode2.Value.ApplyHoming(this.ReflectedFromRigidbody, -1f);
					linkedListNode2.Value.PosX = Mathf.Min(linkedListNode2.Value.PosX + num8, this.m_currentBeamDistance);
					linkedListNode2.Value.Position += linkedListNode2.Value.Velocity * BraveTime.DeltaTime;
					if (linkedListNode2.Value.HomingDampenMotion)
					{
						flag2 = true;
					}
					linkedListNode2 = linkedListNode2.Next;
				}
				if (flag2)
				{
					linkedListNode2 = this.m_bones.First.Next;
					Vector2 vector = this.m_bones.First.Value.Position;
					Vector2 vector2 = this.m_bones.First.Value.Velocity;
					while (linkedListNode2 != null)
					{
						if (linkedListNode2.Next != null)
						{
							Vector2 position = linkedListNode2.Next.Value.Position;
							Vector2 velocity = linkedListNode2.Next.Value.Velocity;
							Vector2 position2 = linkedListNode2.Value.Position;
							Vector2 velocity2 = linkedListNode2.Value.Velocity;
							if (linkedListNode2.Value.HomingDampenMotion)
							{
								linkedListNode2.Value.Position = 0.2f * position2 + 0.4f * position + 0.4f * vector;
								linkedListNode2.Value.Velocity = 0.2f * velocity2 + 0.4f * velocity + 0.4f * vector2;
							}
							vector = position2;
							vector2 = velocity2;
						}
						linkedListNode2 = linkedListNode2.Next;
					}
				}
				if (this.interpolateStretchedBones && this.m_bones.Count > 1)
				{
					linkedListNode2 = this.m_bones.First.Next;
					LinkedListNode<BasicBeamController.BeamBone> linkedListNode3 = null;
					while (linkedListNode2 != null)
					{
						if (Vector2.SqrMagnitude(linkedListNode2.Value.Position - linkedListNode2.Previous.Value.Position) > this.m_sqrNewBoneThreshold)
						{
							BasicBeamController.BeamBone beamBone = new BasicBeamController.BeamBone((linkedListNode2.Previous.Value.PosX + linkedListNode2.Value.PosX) / 2f, linkedListNode2.Value.RotationAngle, linkedListNode2.Value.SubtileNum);
							if (linkedListNode2.Previous.Previous != null && linkedListNode2.Next != null)
							{
								Vector2 position3 = linkedListNode2.Previous.Previous.Value.Position;
								Vector2 position4 = linkedListNode2.Next.Value.Position;
								Vector2 position5 = linkedListNode2.Previous.Value.Position;
								Vector2 vector3 = position5 + (position5 - position3).normalized * this.m_beamQuadUnitWidth;
								Vector2 position6 = linkedListNode2.Value.Position;
								Vector2 vector4 = position6 + (position6 - position4).normalized * this.m_beamQuadUnitWidth;
								beamBone.Position = BraveMathCollege.CalculateBezierPoint(0.5f, position5, vector3, vector4, position6);
							}
							else
							{
								beamBone.Position = (linkedListNode2.Previous.Value.Position + linkedListNode2.Value.Position) / 2f;
							}
							beamBone.Velocity = (linkedListNode2.Previous.Value.Velocity + linkedListNode2.Value.Velocity) / 2f;
							linkedListNode3 = this.m_bones.AddBefore(linkedListNode2, beamBone);
						}
						linkedListNode2 = linkedListNode2.Next;
					}
					if (linkedListNode3 != null)
					{
						for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode4 = linkedListNode3; linkedListNode4 != null; linkedListNode4 = linkedListNode4.Previous)
						{
							linkedListNode4.Value.SubtileNum = ((linkedListNode4.Next.Value.SubtileNum != 0) ? (linkedListNode4.Next.Value.SubtileNum - 1) : (this.m_beamSpriteSubtileWidth - 1));
						}
					}
				}
				if (this.State == BasicBeamController.BeamState.Telegraphing || this.State == BasicBeamController.BeamState.Firing || this.State == BasicBeamController.BeamState.Dissipating)
				{
					Vector2 origin2 = base.Origin;
					Vector2 position7 = this.m_bones.First.Value.Position;
					if (this.IsHoming)
					{
						this.m_previousAngle = new float?(this.m_bones.First.Value.Velocity.ToAngle());
					}
					float num10 = Mathf.Max(0f, this.m_bones.First.Next.Value.PosX - this.m_beamQuadUnitWidth);
					float num11 = Mathf.InverseLerp(0f, num8, num10);
					float num12 = this.m_previousAngle.Value + Mathf.Lerp(BraveMathCollege.ClampAngle180(num - this.m_previousAngle.Value), 0f, num11);
					this.m_bones.First.Value.PosX = num10;
					this.m_bones.First.Value.Position = Vector2.Lerp(origin2, position7, Mathf.InverseLerp(0f, num8, this.m_bones.First.Value.PosX));
					this.m_bones.First.Value.Velocity = BraveMathCollege.DegreesToVector(num12, this.BoneSpeed);
					this.m_bones.First.Value.HomingRadius = this.HomingRadius;
					this.m_bones.First.Value.HomingAngularVelocity = this.HomingAngularVelocity;
					while (this.m_bones.First.Value.PosX != 0f)
					{
						int num13 = ((this.m_bones.First.Value.SubtileNum != 0) ? (this.m_bones.First.Value.SubtileNum - 1) : (this.m_beamSpriteSubtileWidth - 1));
						num10 = Mathf.Max(0f, this.m_bones.First.Value.PosX - this.m_beamQuadUnitWidth);
						num11 = Mathf.InverseLerp(0f, num8, num10);
						num12 = this.m_previousAngle.Value + Mathf.Lerp(BraveMathCollege.ClampAngle180(num - this.m_previousAngle.Value), 0f, num11);
						this.m_bones.AddFirst(new BasicBeamController.BeamBone(num10, 0f, num13));
						this.m_bones.First.Value.Position = Vector2.Lerp(origin2, position7, num11);
						this.m_bones.First.Value.Velocity = BraveMathCollege.DegreesToVector(num12, this.BoneSpeed);
						this.m_bones.First.Value.HomingRadius = this.HomingRadius;
						this.m_bones.First.Value.HomingAngularVelocity = this.HomingAngularVelocity;
						num7++;
					}
					if (this.TileType == BasicBeamController.BeamTileType.Flowing)
					{
						this.m_uvOffset -= num8 / this.m_beamSpriteUnitWidth;
						while (this.m_uvOffset < 0f)
						{
							this.m_uvOffset += 1f;
						}
					}
				}
				else if (this.State == BasicBeamController.BeamState.Disconnected)
				{
					if (this.decayNear > 0f)
					{
						float num14 = this.m_bones.First.Value.PosX + this.decayNear * BraveTime.DeltaTime;
						this.m_bones.First.Value.PosX = num14;
						LinkedListNode<BasicBeamController.BeamBone> linkedListNode5 = this.m_bones.First.Next;
						while (linkedListNode5 != null && linkedListNode5.Value.PosX < num14)
						{
							linkedListNode5.Value.PosX = num14;
							linkedListNode5 = linkedListNode5.Next;
						}
					}
					if (this.decayFar > 0f)
					{
						this.m_currentBeamDistance -= this.decayFar * BraveTime.DeltaTime;
						LinkedListNode<BasicBeamController.BeamBone> linkedListNode6 = this.m_bones.Last;
						while (linkedListNode6 != null && linkedListNode6.Value.PosX >= this.m_currentBeamDistance)
						{
							linkedListNode6.Value.PosX = this.m_currentBeamDistance;
							linkedListNode6 = linkedListNode6.Previous;
						}
					}
				}
				float posX = this.m_bones.First.Value.PosX;
				while (this.m_bones.Count > 2 && this.m_bones.First.Next.Value.PosX <= posX)
				{
					this.m_bones.RemoveFirst();
				}
				while (this.m_bones.Count > 2 && this.m_bones.Last.Previous.Value.PosX >= this.m_currentBeamDistance)
				{
					this.m_bones.RemoveLast();
				}
			}
			if (this.UsesBones && (this.State == BasicBeamController.BeamState.Telegraphing || this.State == BasicBeamController.BeamState.Firing))
			{
				if (this.boneType == BasicBeamController.BeamBoneType.Straight)
				{
					this.m_bones.Clear();
					DungeonData data = GameManager.Instance.Dungeon.data;
					float num15 = 0f;
					Vector2 vector5 = origin;
					float num16 = num;
					float num17 = ((this.BoneSpeed >= 0f) ? this.BoneSpeed : 40f);
					float num18 = this.m_beamQuadUnitWidth / num17;
					this.m_bones.AddLast(new BasicBeamController.BeamBone(num15, vector5, BraveMathCollege.DegreesToVector(num16, num17)));
					while (num15 < this.m_currentBeamDistance)
					{
						num15 = Mathf.Min(num15 + this.m_beamQuadUnitWidth, this.m_currentBeamDistance);
						BasicBeamController.BeamBone beamBone2 = new BasicBeamController.BeamBone(num15, vector5, BraveMathCollege.DegreesToVector(num16, num17));
						beamBone2.HomingRadius = this.HomingRadius;
						beamBone2.HomingAngularVelocity = this.HomingAngularVelocity;
						this.m_bones.AddLast(beamBone2);
						BasicBeamController.BeamBone beamBone3 = beamBone2;
						float num19 = num18;
						beamBone3.ApplyHoming(null, num19);
						beamBone2.Position += beamBone2.Velocity * num18;
						vector5 = beamBone2.Position;
						if (this.ProjectileAndBeamMotionModule != null && this.ProjectileAndBeamMotionModule is OrbitProjectileMotionModule)
						{
							vector5 += this.ProjectileAndBeamMotionModule.GetBoneOffset(beamBone2, this, base.projectile.Inverted);
						}
						num16 = beamBone2.Velocity.ToAngle();
						if (num15 > this.IgnoreTilesDistance && data.isWall((int)vector5.x, (int)vector5.y) && !data.isAnyFaceWall((int)vector5.x, (int)vector5.y))
						{
							this.m_currentBeamDistance = num15;
							break;
						}
					}
				}
				for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode7 = this.m_bones.First; linkedListNode7 != null; linkedListNode7 = linkedListNode7.Next)
				{
					Vector2 vector6 = Vector2.zero;
					if (linkedListNode7.Next != null)
					{
						vector6 += (linkedListNode7.Next.Value.Position - linkedListNode7.Value.Position).normalized;
					}
					if (linkedListNode7.Previous != null)
					{
						vector6 += (linkedListNode7.Value.Position - linkedListNode7.Previous.Value.Position).normalized;
					}
					linkedListNode7.Value.RotationAngle = ((!(vector6 != Vector2.zero)) ? 0f : BraveMathCollege.Atan2Degrees(vector6));
				}
			}
			int num20 = CollisionLayerMatrix.GetMask(CollisionLayer.Projectile);
			if (!hitsPlayers)
			{
				num20 &= ~CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.EnemyBulletBlocker);
			}
			if (!hitsEnemies)
			{
				num20 &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			}
			if (hitsProjectiles)
			{
				num20 |= CollisionMask.LayerToMask(CollisionLayer.Projectile);
			}
			num20 |= CollisionMask.LayerToMask(CollisionLayer.BeamBlocker);
			if (this.m_pierceImpactSprites != null)
			{
				for (int i = 0; i < this.m_pierceImpactSprites.Count; i++)
				{
					this.m_pierceImpactSprites[i].gameObject.SetActive(false);
				}
			}
			int num21 = 0;
			int num22 = this.penetration;
			UltraFortunesFavor ultraFortunesFavor = null;
			SpeculativeRigidbody speculativeRigidbody = null;
			List<SpeculativeRigidbody> list2 = new List<SpeculativeRigidbody>(ignoreRigidbodies);
			int num23 = 0;
			Vector2 vector7;
			Vector2 lastHitNormal;
			SpeculativeRigidbody speculativeRigidbody2;
			List<PointcastResult> list3;
			bool flag3;
			bool flag4;
			bool flag5;
			do
			{
				flag3 = this.FindBeamTarget(origin, direction, this.m_currentBeamDistance, num20, out vector7, out lastHitNormal, out speculativeRigidbody2, out pixelCollider, out list3, null, list2.ToArray());
				flag4 = flag3;
				flag5 = flag3 && speculativeRigidbody2;
				if (flag3 && !speculativeRigidbody2 && this.m_beamGoopModifier && !this.m_beamGoopModifier.OnlyGoopOnEnemyCollision)
				{
					Vector3 vector8 = vector7;
					if (lastHitNormal.y < 0.8f)
					{
						vector8.y -= 1f;
					}
					this.m_beamGoopModifier.SpawnCollisionGoop(vector8);
				}
				if (flag3 && speculativeRigidbody2)
				{
					ultraFortunesFavor = speculativeRigidbody2.ultraFortunesFavor;
					if (ultraFortunesFavor)
					{
						speculativeRigidbody2 = null;
					}
					if (speculativeRigidbody2 && speculativeRigidbody2.ReflectBeams)
					{
						speculativeRigidbody = speculativeRigidbody2;
						speculativeRigidbody2 = null;
						pixelCollider = null;
						flag5 = false;
					}
					if (speculativeRigidbody2 && speculativeRigidbody2.BlockBeams)
					{
						speculativeRigidbody2 = null;
						pixelCollider = null;
						flag5 = false;
					}
					if (speculativeRigidbody2 && speculativeRigidbody2.PreventPiercing)
					{
						if (!speculativeRigidbody2.healthHaver && !speculativeRigidbody2.majorBreakable)
						{
							speculativeRigidbody2 = null;
						}
						flag5 = false;
					}
					if (hitsPlayers && speculativeRigidbody2)
					{
						PlayerController component = speculativeRigidbody2.GetComponent<PlayerController>();
						if (component && (component.spriteAnimator.QueryInvulnerabilityFrame() || !component.healthHaver.IsVulnerable || component.IsEthereal))
						{
							list2.Add(speculativeRigidbody2);
							speculativeRigidbody2 = null;
							num22++;
							component.HandleDodgedBeam(this);
						}
					}
					if (speculativeRigidbody2 && speculativeRigidbody2.minorBreakable)
					{
						if (lastHitNormal != Vector2.zero)
						{
							speculativeRigidbody2.minorBreakable.Break(-lastHitNormal);
						}
						else
						{
							speculativeRigidbody2.minorBreakable.Break();
						}
						if (this.m_beamGoopModifier && !this.m_beamGoopModifier.OnlyGoopOnEnemyCollision)
						{
							this.m_beamGoopModifier.SpawnCollisionGoop(speculativeRigidbody2.UnitBottomCenter);
						}
						speculativeRigidbody2 = null;
						num22++;
					}
					if (speculativeRigidbody2 && pixelCollider != null && pixelCollider.CollisionLayer == CollisionLayer.BeamBlocker)
					{
						TorchController component2 = speculativeRigidbody2.GetComponent<TorchController>();
						if (component2)
						{
							list2.Add(speculativeRigidbody2);
							speculativeRigidbody2 = null;
							num22++;
						}
					}
					if (this.PenetratesCover && speculativeRigidbody2 && speculativeRigidbody2.majorBreakable && speculativeRigidbody2.transform.parent && speculativeRigidbody2.transform.parent.GetComponent<FlippableCover>())
					{
						flag5 = true;
						num22++;
					}
					if (speculativeRigidbody2)
					{
						list.Add(speculativeRigidbody2);
						list2.Add(speculativeRigidbody2);
					}
					num22--;
					if (speculativeRigidbody2 && num22 >= 0 && !string.IsNullOrEmpty(this.impactAnimation))
					{
						if (this.m_pierceImpactSprites == null)
						{
							this.m_pierceImpactSprites = new List<tk2dBaseSprite>();
						}
						if (num21 >= this.m_pierceImpactSprites.Count)
						{
							this.m_pierceImpactSprites.Add(this.CreatePierceImpactEffect());
						}
						tk2dBaseSprite tk2dBaseSprite = this.m_pierceImpactSprites[num21];
						tk2dBaseSprite.gameObject.SetActive(true);
						float num24 = Mathf.Atan2(lastHitNormal.y, lastHitNormal.x) * 57.29578f;
						tk2dBaseSprite.transform.rotation = Quaternion.Euler(0f, 0f, num24);
						tk2dBaseSprite.transform.position = vector7;
						bool flag6 = lastHitNormal.y < -0.5f;
						tk2dBaseSprite.IsPerpendicular = flag6;
						tk2dBaseSprite.HeightOffGround = ((!flag6) ? 0.05f : 2f);
						tk2dBaseSprite.UpdateZDepth();
						num21++;
					}
					if (speculativeRigidbody2 && speculativeRigidbody2.hitEffectHandler)
					{
						HitEffectHandler hitEffectHandler = speculativeRigidbody2.hitEffectHandler;
						if (hitEffectHandler.additionalHitEffects != null && hitEffectHandler.additionalHitEffects.Length > 0)
						{
							hitEffectHandler.HandleAdditionalHitEffects(BraveMathCollege.DegreesToVector(base.Direction.ToAngle(), 8f), pixelCollider);
						}
					}
					if (speculativeRigidbody2 && speculativeRigidbody2.OnBeamCollision != null)
					{
						speculativeRigidbody2.OnBeamCollision(this);
					}
				}
				num23++;
			}
			while (flag5 && num22 >= 0 && num23 < 100);
			if (num23 >= 100)
			{
				Debug.LogErrorFormat("Infinite loop averted!  TELL RUBEL! {0} {1}", new object[] { base.Owner, this });
			}
			if (flag3 && speculativeRigidbody2 && speculativeRigidbody2.gameActor && this.ContinueBeamArtToWall && !speculativeRigidbody2.BlockBeams)
			{
				int num25 = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker);
				Func<SpeculativeRigidbody, bool> func = (SpeculativeRigidbody specRigidbody) => specRigidbody.gameActor;
				SpeculativeRigidbody speculativeRigidbody3;
				PixelCollider pixelCollider2;
				flag3 = this.FindBeamTarget(origin, direction, this.m_currentBeamDistance, num25, out vector7, out lastHitNormal, out speculativeRigidbody3, out pixelCollider2, out list3, func, list2.ToArray());
			}
			if (flag3)
			{
				bool flag7 = false;
				Vector2 vector9 = new Vector2(-1f, -1f);
				Vector2 vector10 = Vector2.zero;
				HitEffectHandler hitEffectHandler2 = ((!(speculativeRigidbody2 != null)) ? null : speculativeRigidbody2.hitEffectHandler);
				if (this.UsesBones)
				{
					int num26 = 0;
					bool flag8 = false;
					if (list3[num26].hitDirection == HitDirection.Forward && list3[num26].boneIndex == 0)
					{
						num26++;
						if (list3.Count == 1)
						{
							flag8 = true;
						}
					}
					if (flag8 || list3[num26].hitDirection == HitDirection.Backward)
					{
						Vector2 vector11;
						float num27;
						LinkedListNode<BasicBeamController.BeamBone> linkedListNode8;
						if (flag8)
						{
							vector11 = list3[0].hitResult.Contact;
							num27 = this.m_bones.First.Value.PosX;
							linkedListNode8 = this.m_bones.Last;
						}
						else
						{
							LinkedListNode<BasicBeamController.BeamBone> linkedListNode9 = this.m_bones.First;
							for (int j = 0; j < list3[num26].boneIndex; j++)
							{
								linkedListNode9 = linkedListNode9.Next;
							}
							vector11 = list3[num26].hitResult.Contact;
							num27 = Mathf.Lerp(linkedListNode9.Value.PosX, linkedListNode9.Previous.Value.PosX, Mathf.Clamp01(Vector2.Distance(linkedListNode9.Value.Position, vector11) / Vector2.Distance(linkedListNode9.Value.Position, linkedListNode9.Previous.Value.Position)));
							linkedListNode8 = linkedListNode9.Previous;
							flag7 = true;
							vector9 = vector11;
							vector10 = list3[num26].hitResult.Normal;
						}
						while (linkedListNode8 != null)
						{
							linkedListNode8.Value.PosX = num27;
							linkedListNode8.Value.Position = vector11;
							linkedListNode8 = linkedListNode8.Previous;
						}
						flag4 = false;
						num26++;
					}
					if (num26 < list3.Count)
					{
						if (list3[num26].hitDirection != HitDirection.Forward)
						{
							Debug.LogError("WTF?");
						}
						LinkedListNode<BasicBeamController.BeamBone> linkedListNode10 = this.m_bones.First;
						for (int k = 0; k < list3[num26].boneIndex; k++)
						{
							linkedListNode10 = linkedListNode10.Next;
						}
						float num28 = 1f;
						if (linkedListNode10.Next != null)
						{
							num28 = Mathf.Clamp01(Vector2.Distance(linkedListNode10.Value.Position, vector7) / Vector2.Distance(linkedListNode10.Value.Position, linkedListNode10.Next.Value.Position));
						}
						if (num26 + 1 < list3.Count && this.collisionSeparation && !(this.ProjectileAndBeamMotionModule is OrbitProjectileMotionModule))
						{
							num26++;
							LinkedListNode<BasicBeamController.BeamBone> linkedListNode11 = this.m_bones.First;
							for (int l = 0; l < list3[num26].boneIndex; l++)
							{
								linkedListNode11 = linkedListNode11.Next;
							}
							Vector2 contact = list3[num26].hitResult.Contact;
							float num29 = Mathf.Clamp01(Vector2.Distance(linkedListNode11.Value.Position, contact) / Vector2.Distance(linkedListNode11.Value.Position, linkedListNode11.Previous.Value.Position));
							float num30 = Mathf.Lerp(linkedListNode11.Value.PosX, linkedListNode11.Previous.Value.PosX, num29);
							this.SeparateBeam(linkedListNode11, contact, num30);
							flag4 = true;
						}
						this.m_currentBeamDistance = ((linkedListNode10.Next == null) ? linkedListNode10.Value.PosX : Mathf.Lerp(linkedListNode10.Value.PosX, linkedListNode10.Next.Value.PosX, num28));
					}
				}
				else
				{
					this.m_currentBeamDistance = (vector7 - origin).magnitude;
					if (this.m_bones.Count == 2)
					{
						this.m_bones.First.Value.PosX = 0f;
						this.m_bones.Last.Value.PosX = this.m_currentBeamDistance;
					}
				}
				for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode9 = this.m_bones.Last; linkedListNode9 != null; linkedListNode9 = linkedListNode9.Previous)
				{
					if (linkedListNode9.Value.PosX < this.m_currentBeamDistance)
					{
						break;
					}
					linkedListNode9.Value.PosX = Mathf.Min(this.m_currentBeamDistance, linkedListNode9.Value.PosX);
				}
				while (this.m_bones.Count > 2 && this.m_bones.Last.Previous.Value.PosX == this.m_currentBeamDistance)
				{
					this.m_bones.RemoveLast();
				}
				bool flag9 = !(hitEffectHandler2 != null) || !hitEffectHandler2.SuppressAllHitEffects;
				if (this.UsesImpactSprite)
				{
					if (this.State != BasicBeamController.BeamState.Telegraphing && flag9)
					{
						if (!this.m_impactTransform.gameObject.activeSelf)
						{
							this.m_impactTransform.gameObject.SetActive(true);
						}
						float num31 = Mathf.Atan2(lastHitNormal.y, lastHitNormal.x) * 57.29578f;
						this.m_impactTransform.rotation = Quaternion.Euler(0f, 0f, num31);
						this.m_impactTransform.position = vector7;
						bool flag10 = lastHitNormal.y < -0.5f;
						this.m_impactSprite.IsPerpendicular = flag10;
						this.m_impactSprite.HeightOffGround = ((!flag10) ? 0.05f : 2f);
						this.m_impactSprite.UpdateZDepth();
						if (this.m_impact2Transform && flag7)
						{
							if (!this.m_impact2Transform.gameObject.activeSelf)
							{
								this.m_impact2Transform.gameObject.SetActive(true);
							}
							float num32 = Mathf.Atan2(vector10.y, vector10.x) * 57.29578f;
							this.m_impact2Transform.rotation = Quaternion.Euler(0f, 0f, num32);
							this.m_impact2Transform.position = vector9;
							bool flag11 = vector10.y < -0.5f;
							this.m_impact2Sprite.IsPerpendicular = flag11;
							this.m_impact2Sprite.HeightOffGround = ((!flag11) ? 0.05f : 2f);
							this.m_impact2Sprite.UpdateZDepth();
						}
					}
					else if (this.UsesImpactSprite)
					{
						if (this.m_impactTransform.gameObject.activeSelf)
						{
							this.m_impactTransform.gameObject.SetActive(false);
						}
						if (this.m_impact2Transform.gameObject.activeSelf)
						{
							this.m_impact2Transform.gameObject.SetActive(false);
						}
					}
				}
			}
			else if (this.ShowImpactOnMaxDistanceEnd)
			{
				if (!this.m_impactTransform.gameObject.activeSelf)
				{
					this.m_impactTransform.gameObject.SetActive(true);
				}
				Vector2 vector12;
				if (this.m_bones.Count >= 2 && this.UsesBones)
				{
					vector12 = this.GetBonePosition(this.m_bones.Last.Value);
				}
				else
				{
					vector12 = base.Origin + base.Direction.normalized * this.m_currentBeamDistance;
				}
				this.m_impactTransform.rotation = Quaternion.identity;
				this.m_impactTransform.position = vector12;
				this.m_impactSprite.IsPerpendicular = false;
				this.m_impactSprite.HeightOffGround = 0.05f;
				this.m_impactSprite.UpdateZDepth();
				if (this.m_impact2Transform.gameObject.activeSelf)
				{
					this.m_impact2Transform.gameObject.SetActive(false);
				}
			}
			else if (this.UsesImpactSprite)
			{
				if (this.m_impactTransform.gameObject.activeSelf)
				{
					this.m_impactTransform.gameObject.SetActive(false);
				}
				if (this.m_impact2Transform.gameObject.activeSelf)
				{
					this.m_impact2Transform.gameObject.SetActive(false);
				}
			}
			if (this.UsesDispersalParticles)
			{
				if (this.boneType == BasicBeamController.BeamBoneType.Straight)
				{
					Vector2 bonePosition = this.GetBonePosition(this.m_bones.First.Value);
					Vector2 vector13 = bonePosition + BraveMathCollege.DegreesToVector(base.transform.eulerAngles.z, 1f).normalized * this.m_currentBeamDistance;
					this.DoDispersalParticles(bonePosition.ToVector3ZisY(0f), vector13.ToVector3ZisY(0f), flag3);
				}
				else
				{
					for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode12 = this.m_bones.First; linkedListNode12 != null; linkedListNode12 = linkedListNode12.Next)
					{
						this.DoDispersalParticles(linkedListNode12, linkedListNode12.Value.SubtileNum, flag3);
					}
				}
			}
			this.exceptionTracker = 0;
			if ((this.reflections > 0 || ultraFortunesFavor || speculativeRigidbody) && flag4)
			{
				this.exceptionTracker = 100;
				if (lastHitNormal.x == 0f && lastHitNormal.y == 0f)
				{
					lastHitNormal = this.m_lastHitNormal;
				}
				else
				{
					this.m_lastHitNormal = lastHitNormal;
				}
				this.exceptionTracker = 101;
				float num33 = BraveMathCollege.ClampAngle360(direction.ToAngle() + 180f);
				float num34 = lastHitNormal.ToAngle();
				if (ultraFortunesFavor)
				{
					num34 = ultraFortunesFavor.GetBeamNormal(vector7).ToAngle();
					ultraFortunesFavor.HitFromPoint(vector7);
				}
				this.exceptionTracker = 102;
				if (speculativeRigidbody && speculativeRigidbody.ReflectProjectilesNormalGenerator != null)
				{
					num34 = speculativeRigidbody.ReflectProjectilesNormalGenerator(vector7, lastHitNormal).ToAngle();
				}
				float num35 = num33 + 2f * BraveMathCollege.ClampAngle180(num34 - num33);
				Vector2 vector14 = vector7 + lastHitNormal.normalized * PhysicsEngine.PixelToUnit(2);
				if (!this.m_reflectedBeam)
				{
					this.exceptionTracker = 103;
					this.m_reflectedBeam = this.CreateReflectedBeam(vector14, BraveMathCollege.DegreesToVector(num35, 1f), !ultraFortunesFavor);
				}
				else
				{
					this.exceptionTracker = 104;
					this.m_reflectedBeam.Origin = vector14;
					this.m_reflectedBeam.Direction = BraveMathCollege.DegreesToVector(num35, 1f);
					this.m_reflectedBeam.LateUpdatePosition(vector14);
				}
				this.exceptionTracker = 1041;
				if (this.m_reflectedBeam)
				{
					this.m_reflectedBeam.penetration = num22;
					this.exceptionTracker = 39;
					if (speculativeRigidbody)
					{
						this.m_reflectedBeam.ReflectedFromRigidbody = speculativeRigidbody;
					}
					else
					{
						this.m_reflectedBeam.ReflectedFromRigidbody = null;
					}
				}
			}
			else
			{
				this.exceptionTracker = 105;
				if (this.m_reflectedBeam)
				{
					this.exceptionTracker = 106;
					this.m_reflectedBeam.CeaseAttack();
					this.m_reflectedBeam = null;
				}
			}
		}
		this.exceptionTracker = 0;
		if (this.doesScreenDistortion)
		{
			if (this.m_distortionMaterial == null)
			{
				this.m_distortionMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionLine"));
			}
			Pixelator.Instance.RegisterAdditionalRenderPass(this.m_distortionMaterial);
			Vector2 vector15 = this.m_currentBeamDistance * direction.normalized + origin;
			Vector3 vector16 = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(origin.ToVector3ZUp(0f));
			Vector3 vector17 = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(vector15.ToVector3ZUp(0f));
			Vector4 vector18 = new Vector4(vector16.x, vector16.y, this.startDistortionRadius, this.startDistortionPower);
			Vector4 vector19 = new Vector4(vector17.x, vector17.y, this.endDistortionRadius, this.endDistortionPower);
			this.m_distortionMaterial.SetVector("_WavePoint1", vector18);
			this.m_distortionMaterial.SetVector("_WavePoint2", vector19);
			this.m_distortionMaterial.SetFloat("_DistortProgress", (Mathf.Sin(Time.realtimeSinceStartup * this.distortionPulseSpeed) + 1f) * this.distortionOffsetIncrease + this.minDistortionOffset);
		}
		Vector2 vector20 = new Vector2(this.m_currentBeamDistance * 16f, this.m_beamSprite.dimensions.y);
		if (vector20 != this.m_beamSprite.dimensions)
		{
			this.m_beamSprite.dimensions = vector20;
		}
		else
		{
			this.m_beamSprite.ForceBuild();
		}
		this.m_beamSprite.UpdateZDepth();
		this.m_previousAngle = new float?(num);
		for (int m = this.TimedIgnoreRigidbodies.Count - 1; m >= 0; m--)
		{
			this.TimedIgnoreRigidbodies[m].Second -= BraveTime.DeltaTime;
			if (this.TimedIgnoreRigidbodies[m].Second <= 0f)
			{
				this.TimedIgnoreRigidbodies.RemoveAt(m);
			}
		}
		return list;
	}

	// Token: 0x06008413 RID: 33811 RVA: 0x00364EB8 File Offset: 0x003630B8
	private void DoDispersalParticles(Vector3 posStart, Vector3 posEnd, bool didImpact)
	{
		int num = Mathf.Max(Mathf.CeilToInt(Vector2.Distance(posStart.XY(), posEnd.XY()) * this.DispersalDensity), 1);
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)i / (float)num;
			Vector3 vector = Vector3.Lerp(posStart, posEnd, num2);
			float num3 = Mathf.PerlinNoise(vector.x / 3f, vector.y / 3f);
			Vector3 vector2 = Quaternion.Euler(0f, 0f, num3 * 360f) * Vector3.right;
			Vector3 vector3 = Vector3.Lerp(vector2, UnityEngine.Random.insideUnitSphere, UnityEngine.Random.Range(this.DispersalMinCoherency, this.DispersalMaxCoherency));
			ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
			{
				position = vector,
				velocity = vector3 * this.m_dispersalParticles.startSpeed,
				startSize = this.m_dispersalParticles.startSize,
				startLifetime = this.m_dispersalParticles.startLifetime,
				startColor = this.m_dispersalParticles.startColor
			};
			this.m_dispersalParticles.Emit(emitParams, 1);
		}
	}

	// Token: 0x06008414 RID: 33812 RVA: 0x00364FE4 File Offset: 0x003631E4
	private void DoDispersalParticles(LinkedListNode<BasicBeamController.BeamBone> boneNode, int subtilesPerTile, bool didImpact)
	{
		if (this.UsesDispersalParticles && boneNode.Value != null && boneNode.Next != null && boneNode.Next.Value != null)
		{
			bool flag = boneNode == this.m_bones.First;
			Vector2 bonePosition = this.GetBonePosition(boneNode.Value);
			Vector3 vector = bonePosition.ToVector3ZUp(bonePosition.y);
			LinkedListNode<BasicBeamController.BeamBone> next = boneNode.Next;
			Vector2 bonePosition2 = this.GetBonePosition(next.Value);
			Vector3 vector2 = bonePosition2.ToVector3ZUp(bonePosition2.y);
			bool flag2 = next == this.m_bones.Last && didImpact;
			float num = (float)((!flag && !flag2) ? 1 : 3);
			int num2 = Mathf.Max(Mathf.CeilToInt(Vector2.Distance(vector.XY(), vector2.XY()) * this.DispersalDensity * num), 1);
			if (flag2)
			{
				num2 = Mathf.CeilToInt((float)num2 * this.DispersalExtraImpactFactor);
			}
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)num2;
				if (flag)
				{
					num3 = Mathf.Lerp(0f, 0.5f, num3);
				}
				if (flag2)
				{
					num3 = Mathf.Lerp(0.5f, 1f, num3);
				}
				Vector3 vector3 = Vector3.Lerp(vector, vector2, num3);
				float num4 = Mathf.PerlinNoise(vector3.x / 3f, vector3.y / 3f);
				Vector3 vector4 = Quaternion.Euler(0f, 0f, num4 * 360f) * Vector3.right;
				Vector3 vector5 = Vector3.Lerp(vector4, UnityEngine.Random.insideUnitSphere, UnityEngine.Random.Range(this.DispersalMinCoherency, this.DispersalMaxCoherency));
				ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
				{
					position = vector3,
					velocity = vector5 * this.m_dispersalParticles.startSpeed,
					startSize = this.m_dispersalParticles.startSize,
					startLifetime = this.m_dispersalParticles.startLifetime,
					startColor = this.m_dispersalParticles.startColor
				};
				this.m_dispersalParticles.Emit(emitParams, 1);
			}
		}
	}

	// Token: 0x06008415 RID: 33813 RVA: 0x0036521C File Offset: 0x0036341C
	private void HandleIgnitionAndFreezing()
	{
		if (base.projectile)
		{
			if ((base.projectile.damageTypes | CoreDamageTypes.Ice) == base.projectile.damageTypes)
			{
				if (this.m_bones.Count > 2)
				{
					Vector3 vector = this.GetBonePosition(this.m_bones.First.Value);
					LinkedListNode<BasicBeamController.BeamBone> linkedListNode = this.m_bones.First.Next;
					while (linkedListNode != null)
					{
						Vector3 vector2 = this.GetBonePosition(linkedListNode.Value);
						Vector2 vector3 = vector.XY();
						Vector2 vector4 = vector2.XY();
						DeadlyDeadlyGoopManager.FreezeGoopsLine(vector3, vector4, 1f);
						linkedListNode = linkedListNode.Next;
						vector = vector2;
					}
				}
				else if (this.boneType == BasicBeamController.BeamBoneType.Straight)
				{
					Vector2 bonePosition = this.GetBonePosition(this.m_bones.First.Value);
					Vector2 vector5 = bonePosition + BraveMathCollege.DegreesToVector(base.transform.eulerAngles.z, 1f).normalized * this.m_currentBeamDistance;
					DeadlyDeadlyGoopManager.FreezeGoopsLine(bonePosition, vector5, 1f);
				}
			}
			if ((base.projectile.damageTypes | CoreDamageTypes.Fire) == base.projectile.damageTypes)
			{
				if (this.m_bones.Count > 2)
				{
					Vector3 vector6 = this.GetBonePosition(this.m_bones.First.Value);
					LinkedListNode<BasicBeamController.BeamBone> linkedListNode2 = this.m_bones.First.Next;
					while (linkedListNode2 != null)
					{
						Vector3 vector7 = this.GetBonePosition(linkedListNode2.Value);
						Vector2 vector8 = vector6.XY();
						Vector2 vector9 = vector7.XY();
						DeadlyDeadlyGoopManager.IgniteGoopsLine(vector8, vector9, 1f);
						linkedListNode2 = linkedListNode2.Next;
						vector6 = vector7;
					}
				}
				else if (this.m_bones.Count == 2)
				{
				}
			}
			if ((base.projectile.damageTypes | CoreDamageTypes.Electric) == base.projectile.damageTypes)
			{
				if (this.m_bones.Count > 2)
				{
					Vector3 vector10 = this.GetBonePosition(this.m_bones.First.Value);
					LinkedListNode<BasicBeamController.BeamBone> linkedListNode3 = this.m_bones.First.Next;
					while (linkedListNode3 != null)
					{
						Vector3 vector11 = this.GetBonePosition(linkedListNode3.Value);
						Vector2 vector12 = vector10.XY();
						Vector2 vector13 = vector11.XY();
						DeadlyDeadlyGoopManager.ElectrifyGoopsLine(vector12, vector13, 1f);
						linkedListNode3 = linkedListNode3.Next;
						vector10 = vector11;
					}
				}
				else if (this.m_bones.Count == 2)
				{
				}
			}
		}
	}

	// Token: 0x06008416 RID: 33814 RVA: 0x003654C8 File Offset: 0x003636C8
	public void HandleGoopFrame(GoopModifier gooper)
	{
		if (gooper.IsSynergyContingent && !gooper.SynergyViable)
		{
			return;
		}
		if (gooper.SpawnGoopInFlight && this.m_bones.Count >= 2)
		{
			BasicBeamController.s_goopPoints.Clear();
			float num = -this.collisionRadius * this.m_projectileScale * PhysicsEngine.Instance.PixelUnitWidth;
			float num2 = this.collisionRadius * this.m_projectileScale * PhysicsEngine.Instance.PixelUnitWidth;
			int num3 = Mathf.Max(2, Mathf.CeilToInt((num2 - num) / 0.25f));
			for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				Vector2 bonePosition = this.GetBonePosition(linkedListNode.Value);
				Vector2 vector = new Vector2(0f, 1f).Rotate(linkedListNode.Value.RotationAngle);
				for (int i = 0; i < num3; i++)
				{
					float num4 = Mathf.Lerp(num, num2, (float)i / (float)(num3 - 1));
					BasicBeamController.s_goopPoints.Add(bonePosition + vector * num4 + gooper.spawnOffset);
				}
			}
			gooper.Manager.AddGoopPoints(BasicBeamController.s_goopPoints, gooper.InFlightSpawnRadius, base.Owner.specRigidbody.UnitCenter, 1.75f);
		}
		if (gooper.SpawnAtBeamEnd)
		{
			Vector2 vector2;
			if (this.m_bones.Count >= 2 && this.UsesBones)
			{
				vector2 = this.GetBonePosition(this.m_bones.Last.Value);
			}
			else
			{
				vector2 = base.Origin + base.Direction.normalized * this.m_currentBeamDistance;
			}
			if (this.m_lastBeamEnd != null)
			{
				gooper.Manager.AddGoopLine(this.m_lastBeamEnd.Value, vector2, gooper.BeamEndRadius);
			}
			this.m_lastBeamEnd = new Vector2?(vector2);
		}
	}

	// Token: 0x06008417 RID: 33815 RVA: 0x003656C4 File Offset: 0x003638C4
	public override void LateUpdatePosition(Vector3 origin)
	{
		if (this.m_previousAngle != null)
		{
			float num = origin.y - BasicBeamController.CurrentBeamHeightOffGround;
			base.transform.position = origin.WithZ(num).Quantize(0.0625f);
		}
		if (this.State == BasicBeamController.BeamState.Charging || this.State == BasicBeamController.BeamState.Telegraphing || this.State == BasicBeamController.BeamState.Firing || this.State == BasicBeamController.BeamState.Dissipating)
		{
			base.Origin = origin;
			this.FrameUpdate();
		}
	}

	// Token: 0x06008418 RID: 33816 RVA: 0x0036574C File Offset: 0x0036394C
	private void CeaseAdditionalBehavior()
	{
		if (this.angularKnockback && this.m_hasToggledGunOutline && base.Gun && base.Gun.GetSprite())
		{
			this.m_hasToggledGunOutline = false;
			SpriteOutlineManager.AddOutlineToSprite(base.Gun.GetSprite(), Color.black);
		}
	}

	// Token: 0x06008419 RID: 33817 RVA: 0x003657B0 File Offset: 0x003639B0
	public override void CeaseAttack()
	{
		this.CeaseAdditionalBehavior();
		if (this.State == BasicBeamController.BeamState.Charging || this.State == BasicBeamController.BeamState.Telegraphing)
		{
			this.DestroyBeam();
			return;
		}
		if (this.endType == BasicBeamController.BeamEndType.Vanish)
		{
			this.DestroyBeam();
			return;
		}
		if (this.endType == BasicBeamController.BeamEndType.Dissipate)
		{
			this.State = BasicBeamController.BeamState.Dissipating;
			base.spriteAnimator.Play(this.CurrentBeamAnimation);
			this.m_dissipateTimer = 0f;
			this.SelfUpdate = true;
		}
		else if (this.endType == BasicBeamController.BeamEndType.Persist)
		{
			this.State = BasicBeamController.BeamState.Disconnected;
			if (this.ProjectileAndBeamMotionModule is OrbitProjectileMotionModule)
			{
				(this.ProjectileAndBeamMotionModule as OrbitProjectileMotionModule).BeamDestroyed();
			}
			this.SelfUpdate = true;
		}
		if (this.UsesChargeSprite || this.UsesMuzzleSprite)
		{
			this.m_muzzleTransform.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600841A RID: 33818 RVA: 0x00365890 File Offset: 0x00363A90
	public override void DestroyBeam()
	{
		if (this.m_reflectedBeam)
		{
			this.m_reflectedBeam.CeaseAttack();
			this.m_reflectedBeam = null;
		}
		if (this.m_enemyKnockback && this.m_enemyKnockbackId >= 0)
		{
			this.m_enemyKnockback.EndContinuousKnockback(this.m_enemyKnockbackId);
			this.m_enemyKnockback = null;
			this.m_enemyKnockbackId = -1;
		}
		if (this.doesScreenDistortion && this.m_distortionMaterial != null)
		{
			Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_distortionMaterial);
		}
		if (GameManager.AUDIO_ENABLED && !string.IsNullOrEmpty(this.endAudioEvent))
		{
			AkSoundEngine.PostEvent(this.endAudioEvent, base.gameObject);
		}
		if (this.ProjectileAndBeamMotionModule is OrbitProjectileMotionModule)
		{
			(this.ProjectileAndBeamMotionModule as OrbitProjectileMotionModule).BeamDestroyed();
		}
		UnityEngine.Object.Destroy(base.transform.gameObject);
	}

	// Token: 0x0600841B RID: 33819 RVA: 0x00365984 File Offset: 0x00363B84
	public override void AdjustPlayerBeamTint(Color targetTintColor, int priority, float lerpTime = 0f)
	{
		if (base.Owner is PlayerController && priority > this.m_currentTintPriority)
		{
			this.m_currentTintPriority = priority;
			this.ChangeTintColorShader(this.m_beamSprite, lerpTime, targetTintColor);
			if (this.m_beamMuzzleSprite)
			{
				this.ChangeTintColorShader(this.m_beamMuzzleSprite, lerpTime, targetTintColor);
			}
			if (this.m_impactSprite)
			{
				this.ChangeTintColorShader(this.m_impactSprite, lerpTime, targetTintColor);
			}
			if (this.m_impact2Sprite)
			{
				this.ChangeTintColorShader(this.m_impact2Sprite, lerpTime, targetTintColor);
			}
		}
	}

	// Token: 0x0600841C RID: 33820 RVA: 0x00365A1C File Offset: 0x00363C1C
	private void ChangeTintColorShader(tk2dBaseSprite baseSprite, float time, Color color)
	{
		baseSprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
		Material material = baseSprite.renderer.material;
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
		if (baseSprite.renderer.material.shader != shader)
		{
			baseSprite.renderer.material.shader = shader;
			baseSprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
			if (flag)
			{
				baseSprite.renderer.material.SetFloat("_EmissivePower", num);
				baseSprite.renderer.material.SetFloat("_EmissiveColorPower", num2);
			}
		}
		if (time == 0f)
		{
			baseSprite.renderer.sharedMaterial.SetColor("_OverrideColor", color);
		}
		else
		{
			base.StartCoroutine(this.ChangeTintColorCR(baseSprite, time, color));
		}
	}

	// Token: 0x0600841D RID: 33821 RVA: 0x00365B44 File Offset: 0x00363D44
	private IEnumerator ChangeTintColorCR(tk2dBaseSprite baseSprite, float time, Color color)
	{
		Material targetMaterial = baseSprite.renderer.sharedMaterial;
		float timer = 0f;
		while (timer < time)
		{
			targetMaterial.SetColor("_OverrideColor", Color.Lerp(Color.white, color, timer / time));
			timer += BraveTime.DeltaTime;
			yield return null;
		}
		targetMaterial.SetColor("_OverrideColor", color);
		yield break;
	}

	// Token: 0x0600841E RID: 33822 RVA: 0x00365B70 File Offset: 0x00363D70
	public void BecomeBlackBullet()
	{
		if (!this.IsBlackBullet && base.sprite)
		{
			this.IsBlackBullet = true;
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.SetFloat("_BlackBullet", 1f);
			base.sprite.renderer.material.SetFloat("_EmissivePower", -40f);
		}
	}

	// Token: 0x0600841F RID: 33823 RVA: 0x00365BEC File Offset: 0x00363DEC
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

	// Token: 0x06008420 RID: 33824 RVA: 0x00365C44 File Offset: 0x00363E44
	public void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num = Mathf.CeilToInt(dimensions.x / (float)this.m_beamQuadPixelWidth);
		if (this.TileType == BasicBeamController.BeamTileType.Flowing)
		{
			num = this.m_bones.Count - 1;
		}
		numVertices = num * 4;
		numIndices = num * 6;
	}

	// Token: 0x06008421 RID: 33825 RVA: 0x00365C8C File Offset: 0x00363E8C
	public void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		boundsCenter = Vector3.zero;
		boundsExtents = Vector3.zero;
		int num = Mathf.RoundToInt(spriteDef.untrimmedBoundsDataExtents.x / spriteDef.texelSize.x);
		int num2 = num / this.m_beamQuadPixelWidth;
		int num3 = Mathf.CeilToInt(dimensions.x / (float)this.m_beamQuadPixelWidth);
		int num4 = Mathf.CeilToInt((float)num3 / (float)num2);
		if (this.TileType == BasicBeamController.BeamTileType.Flowing)
		{
			num3 = this.m_bones.Count - 1;
			num4 = this.m_bones.Count((BasicBeamController.BeamBone b) => b.SubtileNum == 0);
			if (this.m_bones.First.Value.SubtileNum != 0)
			{
				num4++;
			}
			if (this.m_bones.Last.Value.SubtileNum == 0)
			{
				num4--;
			}
		}
		Vector2 vector = new Vector2(dimensions.x * spriteDef.texelSize.x * scale.x, dimensions.y * spriteDef.texelSize.y * scale.y);
		Vector2 vector2 = Vector2.Scale(spriteDef.texelSize, scale) * 0.1f;
		int num5 = 0;
		Vector3 vector3 = new Vector3((float)this.m_beamQuadPixelWidth * spriteDef.texelSize.x, spriteDef.untrimmedBoundsDataExtents.y, spriteDef.untrimmedBoundsDataExtents.z);
		vector3 = Vector3.Scale(vector3, scale);
		Vector3 zero = Vector3.zero;
		Quaternion quaternion = Quaternion.Euler(0f, 0f, base.Direction.ToAngle());
		LinkedListNode<BasicBeamController.BeamBone> linkedListNode = this.m_bones.First;
		for (int i = 0; i < num4; i++)
		{
			int num6 = 0;
			int num7 = num2 - 1;
			if (this.TileType == BasicBeamController.BeamTileType.GrowAtBeginning)
			{
				if (i == 0 && num3 % num2 != 0)
				{
					num6 = num2 - num3 % num2;
				}
			}
			else if (this.TileType == BasicBeamController.BeamTileType.GrowAtEnd)
			{
				if (i == num4 - 1 && num3 % num2 != 0)
				{
					num7 = num3 % num2 - 1;
				}
			}
			else if (this.TileType == BasicBeamController.BeamTileType.Flowing)
			{
				if (i == 0)
				{
					num6 = linkedListNode.Value.SubtileNum;
				}
				if (i == num4 - 1)
				{
					num7 = this.m_bones.Last.Previous.Value.SubtileNum;
				}
			}
			tk2dSpriteDefinition tk2dSpriteDefinition = spriteDef;
			if (this.UsesBeamStartAnimation && i == 0)
			{
				tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.CurrentBeamStartAnimation);
				tk2dSpriteDefinition = this.m_beamSprite.Collection.spriteDefinitions[clipByName.frames[Mathf.Min(clipByName.frames.Length - 1, base.spriteAnimator.CurrentFrame)].spriteId];
			}
			if (this.UsesBeamEndAnimation && i == num4 - 1)
			{
				tk2dSpriteAnimationClip clipByName2 = base.spriteAnimator.GetClipByName(this.CurrentBeamEndAnimation);
				tk2dSpriteDefinition = this.m_beamSprite.Collection.spriteDefinitions[clipByName2.frames[Mathf.Min(clipByName2.frames.Length - 1, base.spriteAnimator.CurrentFrame)].spriteId];
			}
			float num8 = 0f;
			if (i == 0)
			{
				if (this.TileType == BasicBeamController.BeamTileType.GrowAtBeginning)
				{
					num8 = 1f - Mathf.Abs(vector.x % (vector3.x * (float)num2)) / (vector3.x * (float)num2);
				}
				else if (this.TileType == BasicBeamController.BeamTileType.Flowing)
				{
					num8 = this.m_uvOffset;
				}
			}
			for (int j = num6; j <= num7; j++)
			{
				BasicBeamController.BeamBone beamBone = null;
				BasicBeamController.BeamBone beamBone2 = null;
				if (linkedListNode != null)
				{
					beamBone = linkedListNode.Value;
					if (linkedListNode.Next != null)
					{
						beamBone2 = linkedListNode.Next.Value;
					}
				}
				float num9 = 1f;
				if (this.TileType == BasicBeamController.BeamTileType.GrowAtBeginning)
				{
					if (i == 0 && j == 0 && (float)num3 * vector3.x >= Mathf.Abs(vector.x) + vector2.x)
					{
						num9 = Mathf.Abs(vector.x / vector3.x) - (float)(num3 - 1);
					}
				}
				else if (this.TileType == BasicBeamController.BeamTileType.GrowAtEnd)
				{
					if (Mathf.Abs(zero.x + vector3.x) > Mathf.Abs(vector.x) + vector2.x)
					{
						num9 = vector.x % vector3.x / vector3.x;
					}
				}
				else if (this.TileType == BasicBeamController.BeamTileType.Flowing)
				{
					if (i == 0 && linkedListNode == this.m_bones.First)
					{
						num9 = (beamBone2.PosX - beamBone.PosX) / this.m_beamQuadUnitWidth;
					}
					else if (i == num4 - 1 && linkedListNode.Next.Next == null)
					{
						num9 = (beamBone2.PosX - beamBone.PosX) / this.m_beamQuadUnitWidth;
					}
				}
				float num10 = 0f;
				if (this.RampHeightOffset != 0f && zero.x < 5f)
				{
					num10 = (1f - zero.x / 5f) * -this.RampHeightOffset;
				}
				if (this.UsesBones && beamBone2 != null)
				{
					float rotationAngle = beamBone.RotationAngle;
					float num11 = beamBone2.RotationAngle;
					if (Mathf.Abs(BraveMathCollege.ClampAngle180(num11 - rotationAngle)) > 90f)
					{
						num11 = BraveMathCollege.ClampAngle360(num11 + 180f);
					}
					Vector2 bonePosition = this.GetBonePosition(beamBone);
					Vector2 bonePosition2 = this.GetBonePosition(beamBone2);
					int num12 = offset + num5;
					pos[num12++] = Quaternion.Euler(0f, 0f, rotationAngle) * Vector3.Scale(new Vector3(0f, tk2dSpriteDefinition.position0.y * this.m_projectileScale, num10), scale) + (bonePosition - base.transform.position.XY());
					pos[num12++] = Quaternion.Euler(0f, 0f, num11) * Vector3.Scale(new Vector3(0f, tk2dSpriteDefinition.position1.y * this.m_projectileScale, num10), scale) + (bonePosition2 - base.transform.position.XY());
					pos[num12++] = Quaternion.Euler(0f, 0f, rotationAngle) * Vector3.Scale(new Vector3(0f, tk2dSpriteDefinition.position2.y * this.m_projectileScale, num10), scale) + (bonePosition - base.transform.position.XY());
					pos[num12++] = Quaternion.Euler(0f, 0f, num11) * Vector3.Scale(new Vector3(0f, tk2dSpriteDefinition.position3.y * this.m_projectileScale, num10), scale) + (bonePosition2 - base.transform.position.XY());
				}
				else if (this.boneType == BasicBeamController.BeamBoneType.Straight)
				{
					int num13 = offset + num5;
					pos[num13++] = quaternion * (zero + Vector3.Scale(new Vector3(0f, tk2dSpriteDefinition.position0.y * this.m_projectileScale, num10), scale));
					pos[num13++] = quaternion * (zero + Vector3.Scale(new Vector3(num9 * vector3.x, tk2dSpriteDefinition.position1.y * this.m_projectileScale, num10), scale));
					pos[num13++] = quaternion * (zero + Vector3.Scale(new Vector3(0f, tk2dSpriteDefinition.position2.y * this.m_projectileScale, num10), scale));
					pos[num13++] = quaternion * (zero + Vector3.Scale(new Vector3(num9 * vector3.x, tk2dSpriteDefinition.position3.y * this.m_projectileScale, num10), scale));
				}
				Vector2 vector4 = Vector2.Lerp(tk2dSpriteDefinition.uvs[0], tk2dSpriteDefinition.uvs[1], num8);
				Vector2 vector5 = Vector2.Lerp(tk2dSpriteDefinition.uvs[2], tk2dSpriteDefinition.uvs[3], num8 + num9 / (float)num2);
				if (this.FlipBeamSpriteLocal && base.Direction.x < 0f)
				{
					float y = vector4.y;
					vector4.y = vector5.y;
					vector5.y = y;
				}
				int num14 = offset + num5;
				if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
				{
					uv[num14++] = new Vector2(vector4.x, vector4.y);
					uv[num14++] = new Vector2(vector4.x, vector5.y);
					uv[num14++] = new Vector2(vector5.x, vector4.y);
					uv[num14++] = new Vector2(vector5.x, vector5.y);
				}
				else if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
				{
					uv[num14++] = new Vector2(vector4.x, vector4.y);
					uv[num14++] = new Vector2(vector5.x, vector4.y);
					uv[num14++] = new Vector2(vector4.x, vector5.y);
					uv[num14++] = new Vector2(vector5.x, vector5.y);
				}
				else
				{
					uv[num14++] = new Vector2(vector4.x, vector4.y);
					uv[num14++] = new Vector2(vector5.x, vector4.y);
					uv[num14++] = new Vector2(vector4.x, vector5.y);
					uv[num14++] = new Vector2(vector5.x, vector5.y);
				}
				num5 += 4;
				zero.x += vector3.x * num9;
				num8 += num9 / (float)this.m_beamSpriteSubtileWidth;
				if (linkedListNode != null)
				{
					linkedListNode = linkedListNode.Next;
				}
			}
		}
		Vector3 vector6 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector7 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		for (int k = 0; k < pos.Length; k++)
		{
			vector6 = Vector3.Min(vector6, pos[k]);
			vector7 = Vector3.Max(vector7, pos[k]);
		}
		Vector3 vector8 = (vector7 - vector6) / 2f;
		boundsCenter = vector6 + vector8;
		boundsExtents = vector8;
	}

	// Token: 0x06008422 RID: 33826 RVA: 0x0036689C File Offset: 0x00364A9C
	private bool FindBeamTarget(Vector2 origin, Vector2 direction, float distance, int collisionMask, out Vector2 targetPoint, out Vector2 targetNormal, out SpeculativeRigidbody hitRigidbody, out PixelCollider hitPixelCollider, out List<PointcastResult> boneCollisions, Func<SpeculativeRigidbody, bool> rigidbodyExcluder = null, params SpeculativeRigidbody[] ignoreRigidbodies)
	{
		bool flag = false;
		targetPoint = new Vector2(-1f, -1f);
		targetNormal = new Vector2(0f, 0f);
		hitRigidbody = null;
		hitPixelCollider = null;
		if (this.collisionType == BasicBeamController.BeamCollisionType.Rectangle)
		{
			if (!base.specRigidbody)
			{
				base.specRigidbody = base.gameObject.AddComponent<SpeculativeRigidbody>();
				base.specRigidbody.CollideWithTileMap = false;
				base.specRigidbody.CollideWithOthers = true;
				PixelCollider pixelCollider = new PixelCollider();
				pixelCollider.Enabled = false;
				pixelCollider.CollisionLayer = CollisionLayer.PlayerBlocker;
				pixelCollider.Enabled = true;
				pixelCollider.IsTrigger = true;
				pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
				pixelCollider.ManualOffsetX = 0;
				pixelCollider.ManualOffsetY = this.collisionWidth / -2;
				pixelCollider.ManualWidth = this.collisionLength;
				pixelCollider.ManualHeight = this.collisionWidth;
				base.specRigidbody.PixelColliders = new List<PixelCollider>(1);
				base.specRigidbody.PixelColliders.Add(pixelCollider);
				base.specRigidbody.Initialize();
			}
			if (this.m_cachedRectangleOrigin != origin || this.m_cachedRectangleDirection != direction)
			{
				base.specRigidbody.Position = new Position(origin);
				base.specRigidbody.PrimaryPixelCollider.SetRotationAndScale(direction.ToAngle(), Vector2.one);
				base.specRigidbody.UpdateColliderPositions();
				this.m_cachedRectangleOrigin = origin;
				this.m_cachedRectangleDirection = direction;
			}
			int num = CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox);
			if ((collisionMask & num) == num)
			{
				base.specRigidbody.PrimaryPixelCollider.CollisionLayerIgnoreOverride &= ~CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.PlayerCollider);
			}
			else
			{
				base.specRigidbody.PrimaryPixelCollider.CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.PlayerCollider);
			}
			List<CollisionData> list = new List<CollisionData>();
			base.specRigidbody.PrimaryPixelCollider.Enabled = true;
			flag = PhysicsEngine.Instance.OverlapCast(base.specRigidbody, list, false, true, null, null, false, null, null, ignoreRigidbodies);
			base.specRigidbody.PrimaryPixelCollider.Enabled = false;
			boneCollisions = new List<PointcastResult>();
			if (!flag)
			{
				return false;
			}
			targetNormal = list[0].Normal;
			targetPoint = list[0].Contact;
			hitRigidbody = list[0].OtherRigidbody;
			hitPixelCollider = list[0].OtherPixelCollider;
		}
		else if (this.UsesBones)
		{
			float num2 = -this.collisionRadius * this.m_projectileScale * PhysicsEngine.Instance.PixelUnitWidth;
			float num3 = this.collisionRadius * this.m_projectileScale * PhysicsEngine.Instance.PixelUnitWidth;
			int num4 = Mathf.Max(2, Mathf.CeilToInt((num3 - num2) / 0.25f));
			int num5;
			List<IntVector2> list2 = this.GeneratePixelCloud(num2, num3, (float)num4, out num5);
			List<IntVector2> list3 = this.GenerateLastPixelCloud(num2, num3, (float)num4);
			if (!PhysicsEngine.Instance.Pointcast(list2, list3, num4, out boneCollisions, true, true, collisionMask, new CollisionLayer?(CollisionLayer.Projectile), false, rigidbodyExcluder, num5, ignoreRigidbodies))
			{
				return false;
			}
			PointcastResult pointcastResult = boneCollisions[0];
			for (int i = 0; i < boneCollisions.Count; i++)
			{
				if (boneCollisions[i].hitDirection == HitDirection.Forward && boneCollisions[i].boneIndex > 0)
				{
					pointcastResult = boneCollisions[i];
					break;
				}
			}
			targetPoint = pointcastResult.hitResult.Contact;
			targetNormal = pointcastResult.hitResult.Normal;
			hitRigidbody = pointcastResult.hitResult.SpeculativeRigidbody;
			hitPixelCollider = pointcastResult.hitResult.OtherPixelCollider;
		}
		else
		{
			float num6 = -this.collisionRadius * this.m_projectileScale * PhysicsEngine.Instance.PixelUnitWidth;
			float num7 = this.collisionRadius * this.m_projectileScale * PhysicsEngine.Instance.PixelUnitWidth;
			int num8 = Mathf.Max(2, Mathf.CeilToInt((num7 - num6) / 0.25f));
			RaycastResult raycastResult = null;
			for (int j = 0; j < num8; j++)
			{
				float num9 = Mathf.Lerp(num6, num7, (float)j / (float)(num8 - 1));
				Vector2 vector = origin + new Vector2(0f, num9).Rotate(direction.ToAngle());
				RaycastResult raycastResult2;
				if (PhysicsEngine.Instance.RaycastWithIgnores(vector, direction.normalized, distance, out raycastResult2, true, true, collisionMask, new CollisionLayer?(CollisionLayer.Projectile), false, rigidbodyExcluder, ignoreRigidbodies))
				{
					flag = true;
					if (raycastResult == null || raycastResult2.Distance < raycastResult.Distance)
					{
						RaycastResult.Pool.Free(ref raycastResult);
						raycastResult = raycastResult2;
					}
					else
					{
						RaycastResult.Pool.Free(ref raycastResult2);
					}
				}
			}
			boneCollisions = new List<PointcastResult>();
			if (!flag)
			{
				return false;
			}
			targetNormal = raycastResult.Normal;
			targetPoint = origin + BraveMathCollege.DegreesToVector(direction.ToAngle(), raycastResult.Distance);
			hitRigidbody = raycastResult.SpeculativeRigidbody;
			hitPixelCollider = raycastResult.OtherPixelCollider;
			RaycastResult.Pool.Free(ref raycastResult);
		}
		if (hitRigidbody == null)
		{
			return true;
		}
		if (hitRigidbody.minorBreakable && !hitRigidbody.minorBreakable.OnlyBrokenByCode)
		{
			hitRigidbody.minorBreakable.Break(direction);
		}
		DebrisObject component = hitRigidbody.GetComponent<DebrisObject>();
		if (component)
		{
			component.Trigger(direction, 0.5f, 1f);
		}
		TorchController component2 = hitRigidbody.GetComponent<TorchController>();
		if (component2)
		{
			component2.BeamCollision(base.projectile);
		}
		if (hitRigidbody.projectile && hitRigidbody.projectile.collidesWithProjectiles)
		{
			hitRigidbody.projectile.BeamCollision(base.projectile);
		}
		return true;
	}

	// Token: 0x06008423 RID: 33827 RVA: 0x00366E88 File Offset: 0x00365088
	private List<IntVector2> GeneratePixelCloud(float minOffset, float maxOffset, float numOffsets, out int ignoreTileBoneCount)
	{
		ignoreTileBoneCount = -1;
		bool flag = false;
		BasicBeamController.s_pixelCloud.Clear();
		for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			Vector2 bonePosition = this.GetBonePosition(linkedListNode.Value);
			Vector2 vector = new Vector2(0f, 1f).Rotate(linkedListNode.Value.RotationAngle);
			if (this.IgnoreTilesDistance > 0f && !flag && linkedListNode.Value.PosX > this.IgnoreTilesDistance)
			{
				ignoreTileBoneCount = BasicBeamController.s_pixelCloud.Count;
				flag = true;
			}
			int num = 0;
			while ((float)num < numOffsets)
			{
				float num2 = Mathf.Lerp(minOffset, maxOffset, (float)num / (numOffsets - 1f));
				BasicBeamController.s_pixelCloud.Add(PhysicsEngine.UnitToPixel(bonePosition + vector * num2));
				num++;
			}
		}
		if (this.IgnoreTilesDistance > 0f && !flag)
		{
			ignoreTileBoneCount = BasicBeamController.s_pixelCloud.Count;
		}
		return BasicBeamController.s_pixelCloud;
	}

	// Token: 0x06008424 RID: 33828 RVA: 0x00366FA0 File Offset: 0x003651A0
	private List<IntVector2> GenerateLastPixelCloud(float minOffset, float maxOffset, float numOffsets)
	{
		BasicBeamController.s_lastPixelCloud.Clear();
		for (LinkedListNode<BasicBeamController.BeamBone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			Vector2 vector = this.GetBonePosition(linkedListNode.Value) - linkedListNode.Value.Velocity * BraveTime.DeltaTime;
			Vector2 vector2 = new Vector2(0f, 1f).Rotate(linkedListNode.Value.RotationAngle);
			int num = 0;
			while ((float)num < numOffsets)
			{
				float num2 = Mathf.Lerp(minOffset, maxOffset, (float)num / (numOffsets - 1f));
				BasicBeamController.s_lastPixelCloud.Add(PhysicsEngine.UnitToPixel(vector + vector2 * num2));
				num++;
			}
		}
		return BasicBeamController.s_lastPixelCloud;
	}

	// Token: 0x06008425 RID: 33829 RVA: 0x00367068 File Offset: 0x00365268
	private Vector2 GetBonePosition(BasicBeamController.BeamBone bone)
	{
		if (!this.UsesBones)
		{
			return base.Origin + BraveMathCollege.DegreesToVector(base.Direction.ToAngle(), bone.PosX);
		}
		if (this.ProjectileAndBeamMotionModule != null)
		{
			return bone.Position + this.ProjectileAndBeamMotionModule.GetBoneOffset(bone, this, base.projectile.Inverted);
		}
		return bone.Position;
	}

	// Token: 0x06008426 RID: 33830 RVA: 0x003670D8 File Offset: 0x003652D8
	public Vector2 GetPointOnBeam(float t)
	{
		if (this.m_bones.Count < 2)
		{
			return base.Origin;
		}
		if (this.UsesBones)
		{
			return base.Origin + base.Direction.normalized * (this.m_bones.Last.Value.Position - this.m_bones.First.Value.Position).magnitude * t;
		}
		return base.Origin + base.Direction.normalized * this.m_currentBeamDistance * t;
	}

	// Token: 0x06008427 RID: 33831 RVA: 0x00367190 File Offset: 0x00365390
	private void SeparateBeam(LinkedListNode<BasicBeamController.BeamBone> startNode, Vector2 newOrigin, float newPosX)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		gameObject.name = base.gameObject.name + " (Split)";
		BasicBeamController component = gameObject.GetComponent<BasicBeamController>();
		component.State = BasicBeamController.BeamState.Disconnected;
		component.m_bones = new LinkedList<BasicBeamController.BeamBone>();
		component.SelfUpdate = true;
		component.projectile.Owner = base.projectile.Owner;
		component.Owner = base.Owner;
		component.Gun = base.Gun;
		component.HitsPlayers = base.HitsPlayers;
		component.HitsEnemies = base.HitsEnemies;
		component.Origin = base.Origin;
		component.Direction = base.Direction;
		component.DamageModifier = base.DamageModifier;
		component.GetComponent<tk2dTiledSprite>().dimensions = this.m_beamSprite.dimensions;
		component.m_previousAngle = this.m_previousAngle;
		component.m_currentBeamDistance = this.m_currentBeamDistance;
		component.reflections = this.reflections;
		component.Origin = newOrigin;
		BasicBeamController.BeamBone beamBone = new BasicBeamController.BeamBone(startNode.Previous.Value);
		beamBone.Position = newOrigin;
		beamBone.PosX = newPosX;
		component.m_bones.AddFirst(beamBone);
		LinkedListNode<BasicBeamController.BeamBone> previous = startNode.Previous;
		while (previous.Next != null)
		{
			LinkedListNode<BasicBeamController.BeamBone> next = previous.Next;
			this.m_bones.Remove(next);
			component.m_bones.AddLast(next);
		}
	}

	// Token: 0x06008428 RID: 33832 RVA: 0x003672F4 File Offset: 0x003654F4
	private BasicBeamController CreateReflectedBeam(Vector2 pos, Vector2 dir, bool decrementReflections = true)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		gameObject.name = base.gameObject.name + " (Reflect)";
		BasicBeamController component = gameObject.GetComponent<BasicBeamController>();
		component.State = BasicBeamController.BeamState.Firing;
		component.IsReflectedBeam = true;
		component.Owner = base.Owner;
		component.Gun = base.Gun;
		component.HitsPlayers = base.HitsPlayers;
		component.HitsEnemies = base.HitsEnemies;
		component.Origin = pos;
		component.Direction = dir;
		component.DamageModifier = base.DamageModifier;
		component.usesChargeDelay = false;
		component.muzzleAnimation = string.Empty;
		component.chargeAnimation = string.Empty;
		component.beamStartAnimation = string.Empty;
		component.IgnoreTilesDistance = -1f;
		component.reflections = this.reflections;
		if (decrementReflections)
		{
			component.reflections--;
		}
		component.projectile.Owner = base.projectile.Owner;
		component.playerStatsModified = this.playerStatsModified;
		return component;
	}

	// Token: 0x06008429 RID: 33833 RVA: 0x00367400 File Offset: 0x00365600
	private tk2dBaseSprite CreatePierceImpactEffect()
	{
		GameObject gameObject = new GameObject("beam pierce impact vfx");
		Transform transform = gameObject.transform;
		transform.parent = base.transform;
		transform.localPosition = new Vector3(0f, 0f, 0.05f);
		transform.localScale = new Vector3(this.m_projectileScale, this.m_projectileScale, 1f);
		tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
		tk2dSprite.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
		tk2dSpriteAnimator tk2dSpriteAnimator = gameObject.AddComponent<tk2dSpriteAnimator>();
		tk2dSpriteAnimator.SetSprite(this.m_beamSprite.Collection, this.m_beamSprite.spriteId);
		tk2dSpriteAnimator.Library = base.spriteAnimator.Library;
		tk2dSpriteAnimator.Play(this.impactAnimation);
		this.m_beamSprite.AttachRenderer(tk2dSprite);
		tk2dSprite.HeightOffGround = 0.05f;
		tk2dSprite.IsPerpendicular = true;
		tk2dSprite.usesOverrideMaterial = true;
		return tk2dSprite;
	}

	// Token: 0x0600842A RID: 33834 RVA: 0x003674EC File Offset: 0x003656EC
	public static void SetGlobalBeamHeight(float newDepth)
	{
		BasicBeamController.CurrentBeamHeightOffGround = newDepth;
	}

	// Token: 0x0600842B RID: 33835 RVA: 0x003674F4 File Offset: 0x003656F4
	public static void ResetGlobalBeamHeight()
	{
		BasicBeamController.CurrentBeamHeightOffGround = 0.75f;
	}

	// Token: 0x0400873C RID: 34620
	public bool usesTelegraph;

	// Token: 0x0400873D RID: 34621
	[ShowInInspectorIf("usesTelegraph", true)]
	public float telegraphTime;

	// Token: 0x0400873E RID: 34622
	[ShowInInspectorIf("usesTelegraph", true)]
	public BasicBeamController.TelegraphAnims telegraphAnimations;

	// Token: 0x0400873F RID: 34623
	[Header("Beam Structure")]
	public BasicBeamController.BeamBoneType boneType;

	// Token: 0x04008740 RID: 34624
	[ShowInInspectorIf("boneType", 1, true)]
	public bool interpolateStretchedBones;

	// Token: 0x04008741 RID: 34625
	public int penetration;

	// Token: 0x04008742 RID: 34626
	public int reflections;

	// Token: 0x04008743 RID: 34627
	public bool PenetratesCover;

	// Token: 0x04008744 RID: 34628
	public bool angularKnockback;

	// Token: 0x04008745 RID: 34629
	[ShowInInspectorIf("angularKnockback", true)]
	public float angularSpeedAvgWindow = 0.15f;

	// Token: 0x04008746 RID: 34630
	public List<BasicBeamController.AngularKnockbackTier> angularKnockbackTiers;

	// Token: 0x04008747 RID: 34631
	public float homingRadius;

	// Token: 0x04008748 RID: 34632
	public float homingAngularVelocity;

	// Token: 0x04008749 RID: 34633
	public float TimeToStatus = 0.5f;

	// Token: 0x0400874A RID: 34634
	[Header("Beam Animations")]
	public BasicBeamController.BeamTileType TileType;

	// Token: 0x0400874B RID: 34635
	[CheckAnimation(null)]
	public string beamAnimation;

	// Token: 0x0400874C RID: 34636
	[CheckAnimation(null)]
	public string beamStartAnimation;

	// Token: 0x0400874D RID: 34637
	[CheckAnimation(null)]
	public string beamEndAnimation;

	// Token: 0x0400874E RID: 34638
	[Header("Beam Overlays")]
	[CheckAnimation(null)]
	public string muzzleAnimation;

	// Token: 0x0400874F RID: 34639
	[CheckAnimation(null)]
	public string chargeAnimation;

	// Token: 0x04008750 RID: 34640
	[ShowInInspectorIf("chargeAnimation", true)]
	public bool rotateChargeAnimation;

	// Token: 0x04008751 RID: 34641
	[CheckAnimation(null)]
	public string impactAnimation;

	// Token: 0x04008752 RID: 34642
	[Header("Persistence")]
	public BasicBeamController.BeamEndType endType;

	// Token: 0x04008753 RID: 34643
	[ShowInInspectorIf("endType", 1, true)]
	public float decayNear;

	// Token: 0x04008754 RID: 34644
	[ShowInInspectorIf("endType", 1, true)]
	public float decayFar;

	// Token: 0x04008755 RID: 34645
	[ShowInInspectorIf("endType", 1, true)]
	public bool collisionSeparation;

	// Token: 0x04008756 RID: 34646
	[ShowInInspectorIf("endType", 1, true)]
	public float breakAimAngle;

	// Token: 0x04008757 RID: 34647
	[ShowInInspectorIf("endType", 2, true)]
	public float dissipateTime;

	// Token: 0x04008758 RID: 34648
	[ShowInInspectorIf("endType", 2, true)]
	public BasicBeamController.TelegraphAnims dissipateAnimations;

	// Token: 0x04008759 RID: 34649
	[Header("Collision")]
	public BasicBeamController.BeamCollisionType collisionType;

	// Token: 0x0400875A RID: 34650
	[ShowInInspectorIf("collisionType", 0, true)]
	public float collisionRadius = 1.5f;

	// Token: 0x0400875B RID: 34651
	[ShowInInspectorIf("collisionType", 1, true)]
	public int collisionLength = 320;

	// Token: 0x0400875C RID: 34652
	[ShowInInspectorIf("collisionType", 1, true)]
	public int collisionWidth = 64;

	// Token: 0x0400875D RID: 34653
	[Header("Particles")]
	public bool UsesDispersalParticles;

	// Token: 0x0400875E RID: 34654
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalDensity = 3f;

	// Token: 0x0400875F RID: 34655
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalMinCoherency = 0.2f;

	// Token: 0x04008760 RID: 34656
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalMaxCoherency = 1f;

	// Token: 0x04008761 RID: 34657
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public GameObject DispersalParticleSystemPrefab;

	// Token: 0x04008762 RID: 34658
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalExtraImpactFactor = 1f;

	// Token: 0x04008763 RID: 34659
	[Header("Nonsense")]
	public bool doesScreenDistortion;

	// Token: 0x04008764 RID: 34660
	[ShowInInspectorIf("doesScreenDistortion", true)]
	public float startDistortionRadius = 0.3f;

	// Token: 0x04008765 RID: 34661
	[ShowInInspectorIf("doesScreenDistortion", true)]
	public float endDistortionRadius = 0.2f;

	// Token: 0x04008766 RID: 34662
	[ShowInInspectorIf("doesScreenDistortion", true)]
	public float startDistortionPower = 0.7f;

	// Token: 0x04008767 RID: 34663
	[ShowInInspectorIf("doesScreenDistortion", true)]
	public float endDistortionPower = 0.5f;

	// Token: 0x04008768 RID: 34664
	[ShowInInspectorIf("doesScreenDistortion", true)]
	public float distortionPulseSpeed = 25f;

	// Token: 0x04008769 RID: 34665
	[ShowInInspectorIf("doesScreenDistortion", true)]
	public float minDistortionOffset;

	// Token: 0x0400876A RID: 34666
	[ShowInInspectorIf("doesScreenDistortion", true)]
	public float distortionOffsetIncrease = 0.02f;

	// Token: 0x0400876B RID: 34667
	public int overrideBeamQuadPixelWidth = -1;

	// Token: 0x0400876C RID: 34668
	public bool FlipBeamSpriteLocal;

	// Token: 0x0400876D RID: 34669
	[Header("Audio Flags")]
	public string startAudioEvent;

	// Token: 0x0400876E RID: 34670
	public string endAudioEvent;

	// Token: 0x04008770 RID: 34672
	[NonSerialized]
	private Material m_distortionMaterial;

	// Token: 0x04008771 RID: 34673
	[NonSerialized]
	public Action<SpeculativeRigidbody, Vector2> OverrideHitChecks;

	// Token: 0x04008772 RID: 34674
	[NonSerialized]
	public bool SkipPostProcessing;

	// Token: 0x04008774 RID: 34676
	public float IgnoreTilesDistance = -1f;

	// Token: 0x0400877D RID: 34685
	private bool m_hasToggledGunOutline;

	// Token: 0x0400877E RID: 34686
	private int exceptionTracker;

	// Token: 0x0400877F RID: 34687
	private static List<Vector2> s_goopPoints = new List<Vector2>();

	// Token: 0x04008780 RID: 34688
	private Vector2? m_lastBeamEnd;

	// Token: 0x04008781 RID: 34689
	private int m_currentTintPriority = -1;

	// Token: 0x04008782 RID: 34690
	private Vector2 m_cachedRectangleOrigin;

	// Token: 0x04008783 RID: 34691
	private Vector2 m_cachedRectangleDirection;

	// Token: 0x04008784 RID: 34692
	private tk2dTiledSprite m_beamSprite;

	// Token: 0x04008785 RID: 34693
	private Transform m_muzzleTransform;

	// Token: 0x04008786 RID: 34694
	private tk2dSprite m_beamMuzzleSprite;

	// Token: 0x04008787 RID: 34695
	private tk2dSpriteAnimator m_beamMuzzleAnimator;

	// Token: 0x04008788 RID: 34696
	private Transform m_impactTransform;

	// Token: 0x04008789 RID: 34697
	private tk2dSprite m_impactSprite;

	// Token: 0x0400878A RID: 34698
	private tk2dSpriteAnimator m_impactAnimator;

	// Token: 0x0400878B RID: 34699
	private Transform m_impact2Transform;

	// Token: 0x0400878C RID: 34700
	private tk2dSprite m_impact2Sprite;

	// Token: 0x0400878D RID: 34701
	private tk2dSpriteAnimator m_impact2Animator;

	// Token: 0x0400878E RID: 34702
	private GoopModifier m_beamGoopModifier;

	// Token: 0x0400878F RID: 34703
	private List<tk2dBaseSprite> m_pierceImpactSprites;

	// Token: 0x04008790 RID: 34704
	private float m_chargeTimer;

	// Token: 0x04008791 RID: 34705
	private float m_telegraphTimer;

	// Token: 0x04008792 RID: 34706
	private float m_dissipateTimer;

	// Token: 0x04008793 RID: 34707
	private KnockbackDoer m_enemyKnockback;

	// Token: 0x04008794 RID: 34708
	private int m_enemyKnockbackId;

	// Token: 0x04008795 RID: 34709
	private Vector3 m_beamSpriteDimensions;

	// Token: 0x04008796 RID: 34710
	private int m_beamSpriteSubtileWidth;

	// Token: 0x04008797 RID: 34711
	private float m_beamSpriteUnitWidth;

	// Token: 0x04008798 RID: 34712
	private float m_uvOffset;

	// Token: 0x04008799 RID: 34713
	private float? m_previousAngle;

	// Token: 0x0400879A RID: 34714
	private float m_currentBeamDistance;

	// Token: 0x0400879B RID: 34715
	private LinkedList<BasicBeamController.BeamBone> m_bones;

	// Token: 0x0400879C RID: 34716
	private BasicBeamController m_reflectedBeam;

	// Token: 0x0400879D RID: 34717
	private Vector2 m_lastHitNormal;

	// Token: 0x0400879E RID: 34718
	private int m_beamQuadPixelWidth;

	// Token: 0x0400879F RID: 34719
	private float m_beamQuadUnitWidth;

	// Token: 0x040087A0 RID: 34720
	private float m_sqrNewBoneThreshold;

	// Token: 0x040087A1 RID: 34721
	private float m_projectileScale = 1f;

	// Token: 0x040087A2 RID: 34722
	private float m_currentLuteScaleModifier = 1f;

	// Token: 0x040087A3 RID: 34723
	private float averageAngularVelocity;

	// Token: 0x040087A4 RID: 34724
	private ParticleSystem m_dispersalParticles;

	// Token: 0x040087A5 RID: 34725
	private const float c_minGoopDistance = 1.75f;

	// Token: 0x040087A6 RID: 34726
	private static float CurrentBeamHeightOffGround = 0.75f;

	// Token: 0x040087A7 RID: 34727
	private const float c_defaultBeamHeightOffGround = 0.75f;

	// Token: 0x040087A8 RID: 34728
	private const int c_defaultBeamQuadPixelWidth = 4;

	// Token: 0x040087A9 RID: 34729
	private static readonly List<IntVector2> s_pixelCloud = new List<IntVector2>();

	// Token: 0x040087AA RID: 34730
	private static readonly List<IntVector2> s_lastPixelCloud = new List<IntVector2>();

	// Token: 0x0200161C RID: 5660
	[Serializable]
	public class AngularKnockbackTier
	{
		// Token: 0x040087AD RID: 34733
		public float minAngularSpeed;

		// Token: 0x040087AE RID: 34734
		public float damageMultiplier;

		// Token: 0x040087AF RID: 34735
		public float knockbackMultiplier;

		// Token: 0x040087B0 RID: 34736
		public float ignoreHitRigidbodyTime;

		// Token: 0x040087B1 RID: 34737
		public VFXPool hitRigidbodyVFX;

		// Token: 0x040087B2 RID: 34738
		public int additionalAmmoCost;
	}

	// Token: 0x0200161D RID: 5661
	public enum BeamState
	{
		// Token: 0x040087B4 RID: 34740
		Charging,
		// Token: 0x040087B5 RID: 34741
		Telegraphing,
		// Token: 0x040087B6 RID: 34742
		Firing,
		// Token: 0x040087B7 RID: 34743
		Dissipating,
		// Token: 0x040087B8 RID: 34744
		Disconnected
	}

	// Token: 0x0200161E RID: 5662
	public enum BeamBoneType
	{
		// Token: 0x040087BA RID: 34746
		Straight,
		// Token: 0x040087BB RID: 34747
		Projectile = 2
	}

	// Token: 0x0200161F RID: 5663
	public enum BeamTileType
	{
		// Token: 0x040087BD RID: 34749
		GrowAtEnd,
		// Token: 0x040087BE RID: 34750
		GrowAtBeginning,
		// Token: 0x040087BF RID: 34751
		Flowing
	}

	// Token: 0x02001620 RID: 5664
	public enum BeamEndType
	{
		// Token: 0x040087C1 RID: 34753
		Vanish,
		// Token: 0x040087C2 RID: 34754
		Persist,
		// Token: 0x040087C3 RID: 34755
		Dissipate
	}

	// Token: 0x02001621 RID: 5665
	public enum BeamCollisionType
	{
		// Token: 0x040087C5 RID: 34757
		Default,
		// Token: 0x040087C6 RID: 34758
		Rectangle
	}

	// Token: 0x02001622 RID: 5666
	public class BeamBone
	{
		// Token: 0x06008430 RID: 33840 RVA: 0x00367550 File Offset: 0x00365750
		public BeamBone(float posX, float rotationAngle, int subtileNum)
		{
			this.PosX = posX;
			this.RotationAngle = rotationAngle;
			this.SubtileNum = subtileNum;
		}

		// Token: 0x06008431 RID: 33841 RVA: 0x00367570 File Offset: 0x00365770
		public BeamBone(float posX, Vector2 position, Vector2 velocity)
		{
			this.PosX = posX;
			this.Position = position;
			this.Velocity = velocity;
		}

		// Token: 0x06008432 RID: 33842 RVA: 0x00367590 File Offset: 0x00365790
		public BeamBone(BasicBeamController.BeamBone other)
		{
			this.PosX = other.PosX;
			this.RotationAngle = other.RotationAngle;
			this.Position = other.Position;
			this.Velocity = other.Velocity;
			this.SubtileNum = other.SubtileNum;
			this.HomingRadius = other.HomingRadius;
			this.HomingAngularVelocity = other.HomingAngularVelocity;
			this.HomingDampenMotion = other.HomingDampenMotion;
		}

		// Token: 0x06008433 RID: 33843 RVA: 0x00367604 File Offset: 0x00365804
		public void ApplyHoming(SpeculativeRigidbody ignoreRigidbody = null, float overrideDeltaTime = -1f)
		{
			if (this.HomingRadius == 0f || this.HomingAngularVelocity == 0f)
			{
				return;
			}
			IntVector2 intVector = this.Position.ToIntVector2(VectorConversions.Floor);
			if (!GameManager.Instance.Dungeon.CellExists(intVector))
			{
				return;
			}
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector);
			List<AIActor> activeEnemies = absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies == null || activeEnemies.Count == 0)
			{
				return;
			}
			float num = float.MaxValue;
			Vector2 vector = Vector2.zero;
			AIActor aiactor = null;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i] && !activeEnemies[i].healthHaver.IsDead)
				{
					if (!ignoreRigidbody || !(activeEnemies[i].specRigidbody == ignoreRigidbody))
					{
						Vector2 vector2 = activeEnemies[i].CenterPosition - this.Position;
						float magnitude = vector2.magnitude;
						if (magnitude < num - 0.5f)
						{
							vector = vector2;
							num = magnitude;
							aiactor = activeEnemies[i];
						}
					}
				}
			}
			if (num < this.HomingRadius && aiactor != null)
			{
				float num2 = 1f - num / this.HomingRadius;
				float num3 = this.Velocity.ToAngle();
				float num4 = vector.ToAngle();
				float num5 = this.HomingAngularVelocity * num2 * ((overrideDeltaTime < 0f) ? BraveTime.DeltaTime : overrideDeltaTime);
				float num6 = Mathf.MoveTowardsAngle(num3, num4, num5);
				this.Velocity = BraveMathCollege.DegreesToVector(num6, this.Velocity.magnitude);
				if (aiactor != this.HomingTarget)
				{
					this.HomingDampenMotion = true;
				}
				this.HomingTarget = aiactor;
			}
		}

		// Token: 0x040087C7 RID: 34759
		public float PosX;

		// Token: 0x040087C8 RID: 34760
		public float RotationAngle;

		// Token: 0x040087C9 RID: 34761
		public Vector2 Position;

		// Token: 0x040087CA RID: 34762
		public Vector2 Velocity;

		// Token: 0x040087CB RID: 34763
		public int SubtileNum;

		// Token: 0x040087CC RID: 34764
		public float HomingRadius;

		// Token: 0x040087CD RID: 34765
		public float HomingAngularVelocity;

		// Token: 0x040087CE RID: 34766
		public AIActor HomingTarget;

		// Token: 0x040087CF RID: 34767
		public bool HomingDampenMotion;
	}

	// Token: 0x02001623 RID: 5667
	[Serializable]
	public class TelegraphAnims
	{
		// Token: 0x040087D0 RID: 34768
		[CheckAnimation(null)]
		public string beamAnimation;

		// Token: 0x040087D1 RID: 34769
		[CheckAnimation(null)]
		public string beamStartAnimation;

		// Token: 0x040087D2 RID: 34770
		[CheckAnimation(null)]
		public string beamEndAnimation;
	}
}
