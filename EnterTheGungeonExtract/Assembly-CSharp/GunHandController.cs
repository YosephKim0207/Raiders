using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001094 RID: 4244
public class GunHandController : BraveBehaviour
{
	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x06005D67 RID: 23911 RVA: 0x0023CF14 File Offset: 0x0023B114
	public Gun Gun
	{
		get
		{
			return this.m_gun;
		}
	}

	// Token: 0x06005D68 RID: 23912 RVA: 0x0023CF1C File Offset: 0x0023B11C
	public void Start()
	{
		this.m_body = base.transform.parent.GetComponent<AIActor>();
		Transform transform = new GameObject("gun").transform;
		transform.parent = base.transform;
		transform.localPosition = Vector3.zero;
		this.m_gun = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(this.GunId)) as Gun;
		this.m_gun.transform.parent = transform;
		this.m_gun.NoOwnerOverride = true;
		this.m_gun.Initialize(this.m_body);
		this.m_gun.gameObject.SetActive(true);
		this.m_gun.sprite.HeightOffGround = 0.05f;
		this.m_body.sprite.AttachRenderer(this.m_gun.sprite);
		if (this.handObject && this.m_gun)
		{
			PlayerHandController playerHandController = this.AttachNewHandToTransform(this.m_gun.PrimaryHandAttachPoint);
			this.m_body.healthHaver.RegisterBodySprite(playerHandController.sprite, false, 0);
		}
		if (this.OverrideProjectile)
		{
			List<Projectile> list = new List<Projectile>();
			list.Add(this.OverrideProjectile);
			this.m_gun.DefaultModule.projectiles = list;
		}
		if (this.UsesOverrideProjectileData)
		{
			this.m_overrideProjectileData = this.OverrideProjectileData;
		}
		else
		{
			this.m_overrideProjectileData = new ProjectileData(this.m_gun.singleModule.projectiles[0].baseData)
			{
				damage = 0.5f
			};
		}
		this.m_gun.ammo = int.MaxValue;
		this.m_gun.DefaultModule.numberOfShotsInClip = 0;
		if (this.RampBullets)
		{
			this.m_gun.rampBullets = true;
			this.m_gun.rampStartHeight = this.RampStartHeight;
			this.m_gun.rampTime = this.RampTime;
		}
		SpriteOutlineManager.AddOutlineToSprite(this.m_gun.sprite, Color.black, 0.35f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_cooldown = this.Cooldown;
	}

	// Token: 0x06005D69 RID: 23913 RVA: 0x0023D144 File Offset: 0x0023B344
	public void Update()
	{
		float facingDirection = this.m_body.aiAnimator.FacingDirection;
		bool flag = ((!this.isEightWay) ? this.gunBehindBody.IsBehindBody(facingDirection) : this.gunBehindBodyEight.IsBehindBody(facingDirection));
		this.m_gun.sprite.HeightOffGround = ((!flag) ? 0.1f : (-0.2f));
		if (this.m_body.TargetRigidbody)
		{
			this.m_targetLocation = this.m_body.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
		this.m_gunAngle = this.m_gun.HandleAimRotation(this.m_targetLocation, false, 1f);
		if (this.GunFlipMaster)
		{
			if (this.GunFlipMaster.m_gunFlipped != this.m_gunFlipped)
			{
				this.m_gun.HandleSpriteFlip(this.GunFlipMaster.m_gunFlipped);
				this.m_gunFlipped = this.GunFlipMaster.m_gunFlipped;
			}
		}
		else if (!this.m_gunFlipped && Mathf.Abs(this.m_gunAngle) > 105f)
		{
			this.m_gun.HandleSpriteFlip(true);
			this.m_gunFlipped = true;
		}
		else if (this.m_gunFlipped && Mathf.Abs(this.m_gunAngle) < 75f)
		{
			this.m_gun.HandleSpriteFlip(false);
			this.m_gunFlipped = false;
		}
		if (this.m_isFiring)
		{
			this.m_shotCooldown -= BraveTime.DeltaTime;
			if (this.m_shotCooldown <= 0f)
			{
				this.Fire(null);
				this.m_shotCooldown = this.ShotCooldown;
				if (this.m_shotsFired >= this.NumShots)
				{
					this.CeaseAttack();
					this.m_isFiring = false;
				}
			}
		}
		else
		{
			this.m_cooldown = Mathf.Max(0f, this.m_cooldown - BraveTime.DeltaTime);
		}
	}

	// Token: 0x06005D6A RID: 23914 RVA: 0x0023D348 File Offset: 0x0023B548
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005D6B RID: 23915 RVA: 0x0023D350 File Offset: 0x0023B550
	public void StartFiring()
	{
		this.m_isFiring = true;
		this.m_shotsFired = 0;
		if (this.PreFireDelay > 0f)
		{
			this.m_shotCooldown = this.PreFireDelay;
		}
		else
		{
			this.Fire(null);
			this.m_shotCooldown = this.ShotCooldown;
		}
	}

	// Token: 0x06005D6C RID: 23916 RVA: 0x0023D3A8 File Offset: 0x0023B5A8
	public void CeaseAttack()
	{
		if (this.m_gun)
		{
			this.m_gun.CeaseAttack(true, null);
		}
		this.m_cooldown = this.Cooldown;
	}

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x06005D6D RID: 23917 RVA: 0x0023D3D4 File Offset: 0x0023B5D4
	public bool IsReady
	{
		get
		{
			return !this.m_isFiring && this.m_cooldown <= 0f;
		}
	}

	// Token: 0x06005D6E RID: 23918 RVA: 0x0023D3F4 File Offset: 0x0023B5F4
	private void Fire(float? angleOffset = null)
	{
		if (!this.m_gun)
		{
			return;
		}
		if (angleOffset != null)
		{
			this.m_gun.DefaultModule.angleFromAim = angleOffset.Value;
			this.m_gun.DefaultModule.angleVariance = 0f;
			this.m_gun.DefaultModule.alternateAngle = false;
		}
		this.m_gun.CeaseAttack(true, null);
		this.m_gun.ClearCooldowns();
		this.m_gun.ClearReloadData();
		this.m_gun.Attack(this.m_overrideProjectileData, null);
		this.m_shotsFired++;
	}

	// Token: 0x06005D6F RID: 23919 RVA: 0x0023D4A0 File Offset: 0x0023B6A0
	private PlayerHandController AttachNewHandToTransform(Transform target)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.handObject.gameObject);
		gameObject.transform.parent = base.transform;
		PlayerHandController component = gameObject.GetComponent<PlayerHandController>();
		this.m_gun.GetSprite().AttachRenderer(component.sprite);
		component.attachPoint = target;
		this.m_attachedHands.Add(component);
		if (base.healthHaver)
		{
			tk2dSprite component2 = component.GetComponent<tk2dSprite>();
			base.healthHaver.RegisterBodySprite(component2, false, 0);
		}
		return component;
	}

	// Token: 0x04005748 RID: 22344
	[PickupIdentifier]
	[Header("Gun")]
	public int GunId = -1;

	// Token: 0x04005749 RID: 22345
	public Projectile OverrideProjectile;

	// Token: 0x0400574A RID: 22346
	public bool UsesOverrideProjectileData;

	// Token: 0x0400574B RID: 22347
	public ProjectileData OverrideProjectileData;

	// Token: 0x0400574C RID: 22348
	public GunHandController GunFlipMaster;

	// Token: 0x0400574D RID: 22349
	[Header("Hands")]
	public PlayerHandController handObject;

	// Token: 0x0400574E RID: 22350
	public bool isEightWay;

	// Token: 0x0400574F RID: 22351
	public GunHandController.DirectionalAnimationBoolSixWay gunBehindBody;

	// Token: 0x04005750 RID: 22352
	public GunHandController.DirectionalAnimationBoolEightWay gunBehindBodyEight;

	// Token: 0x04005751 RID: 22353
	[Header("Shooting")]
	public float PreFireDelay;

	// Token: 0x04005752 RID: 22354
	public int NumShots;

	// Token: 0x04005753 RID: 22355
	public float ShotCooldown;

	// Token: 0x04005754 RID: 22356
	public float Cooldown;

	// Token: 0x04005755 RID: 22357
	public bool RampBullets;

	// Token: 0x04005756 RID: 22358
	[ShowInInspectorIf("RampBullets", false)]
	public float RampStartHeight = 2f;

	// Token: 0x04005757 RID: 22359
	[ShowInInspectorIf("RampBullets", false)]
	public float RampTime = 1f;

	// Token: 0x04005758 RID: 22360
	private AIActor m_body;

	// Token: 0x04005759 RID: 22361
	private Gun m_gun;

	// Token: 0x0400575A RID: 22362
	private ProjectileData m_overrideProjectileData;

	// Token: 0x0400575B RID: 22363
	private Transform m_gunAttachPoint;

	// Token: 0x0400575C RID: 22364
	private float m_gunAngle;

	// Token: 0x0400575D RID: 22365
	private bool m_gunFlipped;

	// Token: 0x0400575E RID: 22366
	private bool m_isFiring;

	// Token: 0x0400575F RID: 22367
	private int m_shotsFired;

	// Token: 0x04005760 RID: 22368
	private float m_shotCooldown;

	// Token: 0x04005761 RID: 22369
	private float m_cooldown;

	// Token: 0x04005762 RID: 22370
	private Vector2 m_targetLocation;

	// Token: 0x04005763 RID: 22371
	private List<PlayerHandController> m_attachedHands = new List<PlayerHandController>();

	// Token: 0x02001095 RID: 4245
	[Serializable]
	public class DirectionalAnimationBoolSixWay
	{
		// Token: 0x06005D71 RID: 23921 RVA: 0x0023D530 File Offset: 0x0023B730
		public bool IsBehindBody(float angle)
		{
			if (angle <= 155f && angle >= 25f)
			{
				if (angle < 120f && angle >= 60f)
				{
					return this.Back;
				}
				return (Mathf.Abs(angle) >= 90f) ? this.BackRight : this.BackLeft;
			}
			else
			{
				if (angle <= -60f && angle >= -120f)
				{
					return this.Forward;
				}
				return (Mathf.Abs(angle) < 90f) ? this.ForwardRight : this.ForwardLeft;
			}
		}

		// Token: 0x04005764 RID: 22372
		public bool Back;

		// Token: 0x04005765 RID: 22373
		public bool BackRight;

		// Token: 0x04005766 RID: 22374
		public bool ForwardRight;

		// Token: 0x04005767 RID: 22375
		public bool Forward;

		// Token: 0x04005768 RID: 22376
		public bool ForwardLeft;

		// Token: 0x04005769 RID: 22377
		public bool BackLeft;
	}

	// Token: 0x02001096 RID: 4246
	[Serializable]
	public class DirectionalAnimationBoolEightWay
	{
		// Token: 0x06005D73 RID: 23923 RVA: 0x0023D5D8 File Offset: 0x0023B7D8
		public bool IsBehindBody(float angle)
		{
			angle = BraveMathCollege.ClampAngle360(angle);
			if (angle < 22.5f)
			{
				return this.East;
			}
			if (angle < 67.5f)
			{
				return this.NorthEast;
			}
			if (angle < 112.5f)
			{
				return this.North;
			}
			if (angle < 157.5f)
			{
				return this.NorthWest;
			}
			if (angle < 202.5f)
			{
				return this.West;
			}
			if (angle < 247.5f)
			{
				return this.SouthWest;
			}
			if (angle < 292.5f)
			{
				return this.South;
			}
			if (angle < 337.5f)
			{
				return this.SouthEast;
			}
			return this.East;
		}

		// Token: 0x0400576A RID: 22378
		public bool North;

		// Token: 0x0400576B RID: 22379
		public bool NorthEast;

		// Token: 0x0400576C RID: 22380
		public bool East;

		// Token: 0x0400576D RID: 22381
		public bool SouthEast;

		// Token: 0x0400576E RID: 22382
		public bool South;

		// Token: 0x0400576F RID: 22383
		public bool SouthWest;

		// Token: 0x04005770 RID: 22384
		public bool West;

		// Token: 0x04005771 RID: 22385
		public bool NorthWest;
	}
}
