using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000FCE RID: 4046
public class BeholsterTentacleController : BraveBehaviour
{
	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x06005836 RID: 22582 RVA: 0x0021B4BC File Offset: 0x002196BC
	public Gun Gun
	{
		get
		{
			return this.m_gun;
		}
	}

	// Token: 0x06005837 RID: 22583 RVA: 0x0021B4C4 File Offset: 0x002196C4
	public void Start()
	{
		this.m_body = base.transform.parent.GetComponent<BeholsterController>();
		this.m_gunAttachPoint = base.transform.Find("gun");
		this.m_gun = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(this.GunId)) as Gun;
		this.m_gun.transform.parent = this.m_gunAttachPoint;
		this.m_gun.NoOwnerOverride = true;
		this.m_gun.Initialize(this.m_body.aiActor);
		this.m_gun.gameObject.SetActive(true);
		this.m_gun.sprite.HeightOffGround = 0.05f;
		base.sprite.AttachRenderer(this.m_gun.sprite);
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
		this.m_gun.DefaultModule.usesOptionalFinalProjectile = false;
		if (this.RampBullets)
		{
			this.m_gun.rampBullets = true;
			this.m_gun.rampStartHeight = this.RampStartHeight;
			this.m_gun.rampTime = this.RampTime;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.3f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		SpriteOutlineManager.AddOutlineToSprite(this.m_gun.sprite, Color.black, 0.35f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_body.healthHaver.RegisterBodySprite(base.sprite, false, 0);
		this.m_cooldown = this.Cooldown;
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(delegate(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
		{
			if (!base.enabled)
			{
				return;
			}
			this.m_currentAnimation = null;
		}));
		if (this.SpawnBullets)
		{
			this.m_spawningProjectiles = new List<SpawningProjectile>();
		}
	}

	// Token: 0x06005838 RID: 22584 RVA: 0x0021B718 File Offset: 0x00219918
	public void Update()
	{
		float facingDirection = this.m_body.aiAnimator.FacingDirection;
		DirectionalAnimation.Info info;
		if (this.m_currentAnimation != null)
		{
			info = this.m_currentAnimation.GetInfo(facingDirection, false);
			base.spriteAnimator.Play(base.spriteAnimator.GetClipByName(info.name), base.spriteAnimator.ClipTimeSeconds, base.spriteAnimator.ClipFps, false);
		}
		else
		{
			info = this.IdleAnimation.GetInfo(facingDirection, false);
			base.spriteAnimator.Play(info.name);
		}
		base.sprite.FlipX = info.flipped;
		bool flag;
		bool flag2;
		if (facingDirection <= 155f && facingDirection >= 25f)
		{
			if (facingDirection < 120f && facingDirection >= 60f)
			{
				flag = this.tentacleBehindBody.Back;
				flag2 = this.gunBehindTentacle.Back;
			}
			else
			{
				flag = ((Mathf.Abs(facingDirection) >= 90f) ? this.tentacleBehindBody.BackRight : this.tentacleBehindBody.BackLeft);
				flag2 = ((Mathf.Abs(facingDirection) >= 90f) ? this.gunBehindTentacle.BackRight : this.gunBehindTentacle.BackLeft);
			}
		}
		else if (facingDirection <= -60f && facingDirection >= -120f)
		{
			flag = this.tentacleBehindBody.Forward;
			flag2 = this.gunBehindTentacle.Forward;
		}
		else
		{
			flag = ((Mathf.Abs(facingDirection) < 90f) ? this.tentacleBehindBody.ForwardRight : this.tentacleBehindBody.ForwardLeft);
			flag2 = ((Mathf.Abs(facingDirection) < 90f) ? this.gunBehindTentacle.ForwardRight : this.gunBehindTentacle.ForwardLeft);
		}
		base.sprite.HeightOffGround = ((!flag) ? 0.1f : (-0.1f));
		this.m_gun.sprite.HeightOffGround = ((!flag2) ? 0.05f : (-0.05f));
		if (this.m_body.LaserActive)
		{
			float num = this.m_body.aiAnimator.FacingDirection * 0.017453292f;
			this.m_targetLocation = this.m_body.LaserFiringCenter + new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * 10f;
		}
		else if (this.m_body.aiActor.TargetRigidbody)
		{
			this.m_targetLocation = this.m_body.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
		this.m_gunAngle = this.m_gun.HandleAimRotation(this.m_targetLocation, false, 1f);
		if (!this.m_gunFlipped && Mathf.Abs(this.m_gunAngle) > 105f)
		{
			this.m_gun.HandleSpriteFlip(true);
			this.m_gunFlipped = true;
		}
		else if (this.m_gunFlipped && Mathf.Abs(this.m_gunAngle) < 75f)
		{
			this.m_gun.HandleSpriteFlip(false);
			this.m_gunFlipped = false;
		}
		if (this.m_fireTimer > 0f)
		{
			this.m_fireTimer -= BraveTime.DeltaTime;
			if (this.m_fireTimer <= 0f || (this.SpawnBullets && (float)this.CurrentAdds >= this.MaxConcurrentAdds))
			{
				this.CeaseAttack();
			}
			else if (this.ShotCooldown <= 0f)
			{
				this.m_gun.ContinueAttack(true, this.m_overrideProjectileData);
			}
			else
			{
				this.m_shotCooldown -= BraveTime.DeltaTime;
				if (this.m_shotCooldown <= 0f)
				{
					this.m_gun.CeaseAttack(true, null);
					this.Fire(null);
					this.m_shotCooldown = this.ShotCooldown;
				}
			}
		}
		else
		{
			this.m_cooldown = Mathf.Max(0f, this.m_cooldown - BraveTime.DeltaTime);
		}
	}

	// Token: 0x06005839 RID: 22585 RVA: 0x0021BB48 File Offset: 0x00219D48
	public void StartFiring()
	{
		this.Fire(null);
		this.m_fireTimer = this.FireTime;
		this.m_shotCooldown = this.ShotCooldown;
		this.m_cooldown = this.Cooldown;
		if (this.m_gun.singleModule.shootStyle == ProjectileModule.ShootStyle.SemiAutomatic)
		{
			this.Play(this.ShootAnimation);
		}
	}

	// Token: 0x0600583A RID: 22586 RVA: 0x0021BBAC File Offset: 0x00219DAC
	public void SingleFire(float? angleOffset = null)
	{
		this.m_gun.ClearCooldowns();
		this.Fire(angleOffset);
		this.m_cooldown = this.Cooldown;
		if (this.m_gun.singleModule.shootStyle == ProjectileModule.ShootStyle.SemiAutomatic)
		{
			this.Play(this.ShootAnimation);
		}
	}

	// Token: 0x0600583B RID: 22587 RVA: 0x0021BBF8 File Offset: 0x00219DF8
	public void CeaseAttack()
	{
		this.m_gun.CeaseAttack(true, null);
		this.m_cooldown = this.Cooldown;
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x0600583C RID: 22588 RVA: 0x0021BC14 File Offset: 0x00219E14
	public bool IsReady
	{
		get
		{
			return (!this.SpawnBullets || (float)this.CurrentAdds < this.MaxConcurrentAdds) && (!this.m_cachedBulletScriptSource || this.m_cachedBulletScriptSource.IsEnded) && this.m_fireTimer <= 0f && this.m_cooldown <= 0f;
		}
	}

	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x0600583D RID: 22589 RVA: 0x0021BC88 File Offset: 0x00219E88
	public int CurrentAdds
	{
		get
		{
			if (!this.SpawnBullets)
			{
				return 0;
			}
			int num = 0;
			this.m_spawningProjectiles.RemoveAll((SpawningProjectile p) => !p);
			num += this.m_spawningProjectiles.Count;
			List<AIActor> activeEnemies = this.m_body.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				num += activeEnemies.Count - 1;
			}
			return num;
		}
	}

	// Token: 0x0600583E RID: 22590 RVA: 0x0021BD04 File Offset: 0x00219F04
	public void Play(DirectionalAnimation anim)
	{
		this.m_currentAnimation = anim;
		DirectionalAnimation.Info info = anim.GetInfo(this.m_body.aiAnimator.FacingDirection, true);
		base.sprite.FlipX = info.flipped;
		base.spriteAnimator.Play(info.name);
	}

	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x0600583F RID: 22591 RVA: 0x0021BD54 File Offset: 0x00219F54
	public BulletScriptSource BulletScriptSource
	{
		get
		{
			if (this.m_cachedBulletScriptSource == null)
			{
				this.m_cachedBulletScriptSource = this.m_gun.barrelOffset.gameObject.GetOrAddComponent<BulletScriptSource>();
			}
			return this.m_cachedBulletScriptSource;
		}
	}

	// Token: 0x06005840 RID: 22592 RVA: 0x0021BD88 File Offset: 0x00219F88
	public void ShootBulletScript(BulletScriptSelector bulletScript)
	{
		this.m_body.bulletBank.rampBullets = this.RampBullets;
		this.m_body.bulletBank.rampStartHeight = this.RampStartHeight;
		this.m_body.bulletBank.rampTime = this.RampTime;
		this.m_body.bulletBank.OverrideGun = this.Gun;
		BulletScriptSource bulletScriptSource = this.BulletScriptSource;
		bulletScriptSource.BulletManager = this.m_body.bulletBank;
		bulletScriptSource.BulletScript = bulletScript;
		bulletScriptSource.Initialize();
	}

	// Token: 0x06005841 RID: 22593 RVA: 0x0021BE14 File Offset: 0x0021A014
	private void Fire(float? angleOffset = null)
	{
		if (this.SpawnBullets && this.m_body.aiActor.TargetRigidbody)
		{
			Vector2 vector = this.m_body.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			Vector2 unitCenter = this.m_body.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			float num = (unitCenter - vector).ToAngle();
			vector += BraveMathCollege.DegreesToVector(num + UnityEngine.Random.Range(-90f, 90f), UnityEngine.Random.Range(this.MinSpawnRadius, this.MaxSpawnRadius));
			this.m_gun.HandleAimRotation(vector, false, 1f);
			this.m_gun.Attack(this.m_overrideProjectileData, null);
			if (this.m_gun.LastProjectile && this.m_gun.LastProjectile is SpawningProjectile)
			{
				SpawningProjectile spawningProjectile = this.m_gun.LastProjectile as SpawningProjectile;
				float magnitude = (vector - spawningProjectile.transform.position.XY()).magnitude;
				float num2 = magnitude / spawningProjectile.baseData.speed;
				spawningProjectile.gravity = -2f * spawningProjectile.startingHeight / (num2 * num2);
				this.m_spawningProjectiles.Add(spawningProjectile);
				this.m_gun.LastProjectile.collidesWithPlayer = false;
				this.m_gun.LastProjectile.UpdateCollisionMask();
			}
		}
		else
		{
			if (angleOffset != null)
			{
				this.m_gun.DefaultModule.angleFromAim = angleOffset.Value;
				this.m_gun.DefaultModule.angleVariance = 0f;
				this.m_gun.DefaultModule.alternateAngle = false;
			}
			this.m_gun.Attack(this.m_overrideProjectileData, null);
		}
	}

	// Token: 0x06005842 RID: 22594 RVA: 0x0021BFE8 File Offset: 0x0021A1E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005140 RID: 20800
	public DirectionalAnimation IdleAnimation;

	// Token: 0x04005141 RID: 20801
	public DirectionalAnimation ShootAnimation;

	// Token: 0x04005142 RID: 20802
	[PickupIdentifier]
	public int GunId;

	// Token: 0x04005143 RID: 20803
	public Projectile OverrideProjectile;

	// Token: 0x04005144 RID: 20804
	public bool UsesOverrideProjectileData;

	// Token: 0x04005145 RID: 20805
	public ProjectileData OverrideProjectileData;

	// Token: 0x04005146 RID: 20806
	public float FireTime;

	// Token: 0x04005147 RID: 20807
	public float ShotCooldown;

	// Token: 0x04005148 RID: 20808
	public float Cooldown;

	// Token: 0x04005149 RID: 20809
	public BeholsterTentacleController.DirectionalAnimationBool gunBehindTentacle;

	// Token: 0x0400514A RID: 20810
	public BeholsterTentacleController.DirectionalAnimationBool tentacleBehindBody;

	// Token: 0x0400514B RID: 20811
	public bool RampBullets;

	// Token: 0x0400514C RID: 20812
	[ShowInInspectorIf("RampBullets", false)]
	public float RampStartHeight = 2f;

	// Token: 0x0400514D RID: 20813
	[ShowInInspectorIf("RampBullets", false)]
	public float RampTime = 1f;

	// Token: 0x0400514E RID: 20814
	public bool SpawnBullets;

	// Token: 0x0400514F RID: 20815
	[ShowInInspectorIf("SpawnBullets", false)]
	public float MinSpawnRadius;

	// Token: 0x04005150 RID: 20816
	[ShowInInspectorIf("SpawnBullets", false)]
	public float MaxSpawnRadius;

	// Token: 0x04005151 RID: 20817
	[ShowInInspectorIf("SpawnBullets", false)]
	public float MaxConcurrentAdds;

	// Token: 0x04005152 RID: 20818
	private BulletScriptSource m_cachedBulletScriptSource;

	// Token: 0x04005153 RID: 20819
	private BeholsterController m_body;

	// Token: 0x04005154 RID: 20820
	private Gun m_gun;

	// Token: 0x04005155 RID: 20821
	private ProjectileData m_overrideProjectileData;

	// Token: 0x04005156 RID: 20822
	private Transform m_gunAttachPoint;

	// Token: 0x04005157 RID: 20823
	private float m_gunAngle;

	// Token: 0x04005158 RID: 20824
	private bool m_gunFlipped;

	// Token: 0x04005159 RID: 20825
	private DirectionalAnimation m_currentAnimation;

	// Token: 0x0400515A RID: 20826
	private float m_fireTimer;

	// Token: 0x0400515B RID: 20827
	private float m_shotCooldown;

	// Token: 0x0400515C RID: 20828
	private float m_cooldown;

	// Token: 0x0400515D RID: 20829
	private Vector2 m_targetLocation;

	// Token: 0x0400515E RID: 20830
	private List<SpawningProjectile> m_spawningProjectiles;

	// Token: 0x02000FCF RID: 4047
	[Serializable]
	public class DirectionalAnimationBool
	{
		// Token: 0x04005160 RID: 20832
		public bool Back;

		// Token: 0x04005161 RID: 20833
		public bool BackRight;

		// Token: 0x04005162 RID: 20834
		public bool ForwardRight;

		// Token: 0x04005163 RID: 20835
		public bool Forward;

		// Token: 0x04005164 RID: 20836
		public bool ForwardLeft;

		// Token: 0x04005165 RID: 20837
		public bool BackLeft;
	}
}
