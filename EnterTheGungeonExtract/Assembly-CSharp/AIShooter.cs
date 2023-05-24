using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000FA2 RID: 4002
[RequireComponent(typeof(AIBulletBank))]
public class AIShooter : BraveBehaviour
{
	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x060056F5 RID: 22261 RVA: 0x00212560 File Offset: 0x00210760
	public bool CanShootOtherEnemies
	{
		get
		{
			return base.aiActor.CanTargetEnemies;
		}
	}

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x060056F6 RID: 22262 RVA: 0x00212570 File Offset: 0x00210770
	// (set) Token: 0x060056F7 RID: 22263 RVA: 0x00212578 File Offset: 0x00210778
	public Vector2? OverrideAimPoint { get; set; }

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x060056F8 RID: 22264 RVA: 0x00212584 File Offset: 0x00210784
	// (set) Token: 0x060056F9 RID: 22265 RVA: 0x002125DC File Offset: 0x002107DC
	public Vector2? OverrideAimDirection
	{
		get
		{
			if (this.OverrideAimPoint == null)
			{
				return null;
			}
			return new Vector2?((this.OverrideAimPoint.Value - base.aiActor.CenterPosition).normalized);
		}
		set
		{
			Vector2? vector;
			if (value == null)
			{
				vector = null;
			}
			else
			{
				Vector2? vector2 = ((value == null) ? null : new Vector2?(value.GetValueOrDefault() * 5f));
				vector = ((vector2 == null) ? null : new Vector2?(base.aiActor.CenterPosition + vector2.GetValueOrDefault()));
			}
			this.OverrideAimPoint = vector;
		}
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x060056FA RID: 22266 RVA: 0x00212670 File Offset: 0x00210870
	public GunInventory Inventory
	{
		get
		{
			return this.m_inventory;
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x060056FB RID: 22267 RVA: 0x00212678 File Offset: 0x00210878
	public Transform BulletSourceTransform
	{
		get
		{
			if (this.bulletScriptAttachPoint)
			{
				return this.bulletScriptAttachPoint;
			}
			if (this.CurrentGun)
			{
				return this.CurrentGun.barrelOffset;
			}
			if (this.volley && this.volleyShootPosition)
			{
				return this.volleyShootPosition;
			}
			if (this.gunAttachPoint)
			{
				return this.gunAttachPoint;
			}
			return base.transform;
		}
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x060056FC RID: 22268 RVA: 0x002126FC File Offset: 0x002108FC
	public BulletScriptSource BraveBulletSource
	{
		get
		{
			if (this.m_cachedBraveBulletSource == null)
			{
				this.m_cachedBraveBulletSource = this.BulletSourceTransform.gameObject.GetOrAddComponent<BulletScriptSource>();
			}
			return this.m_cachedBraveBulletSource;
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x060056FD RID: 22269 RVA: 0x0021272C File Offset: 0x0021092C
	// (set) Token: 0x060056FE RID: 22270 RVA: 0x00212758 File Offset: 0x00210958
	public float AimTimeScale
	{
		get
		{
			if (base.aiActor)
			{
				return this.m_aimTimeScale * base.aiActor.LocalTimeScale;
			}
			return this.m_aimTimeScale;
		}
		set
		{
			this.m_aimTimeScale = value;
		}
	}

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x060056FF RID: 22271 RVA: 0x00212764 File Offset: 0x00210964
	public Gun EquippedGun
	{
		get
		{
			if (!this.m_hasCachedGun)
			{
				if (this.equippedGunId >= 0)
				{
					this.m_cachedGun = PickupObjectDatabase.GetById(this.equippedGunId) as Gun;
				}
				this.m_hasCachedGun = true;
			}
			return this.m_cachedGun;
		}
	}

	// Token: 0x06005700 RID: 22272 RVA: 0x002127A0 File Offset: 0x002109A0
	public void Start()
	{
		base.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x06005701 RID: 22273 RVA: 0x002127BC File Offset: 0x002109BC
	private void Update()
	{
		if (base.aiActor.HasBeenEngaged)
		{
			this.HandleGunFlipping();
		}
		if (base.aiActor.IsFalling && this.m_cachedBraveBulletSource != null)
		{
			this.m_cachedBraveBulletSource.enabled = false;
		}
		if (this.BackupAimInMoveDirection && !base.aiActor.TargetRigidbody)
		{
			this.AimInDirection(BraveMathCollege.DegreesToVector(base.aiActor.FacingDirection, 1f));
		}
	}

	// Token: 0x06005702 RID: 22274 RVA: 0x00212848 File Offset: 0x00210A48
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005703 RID: 22275 RVA: 0x00212850 File Offset: 0x00210A50
	public void ShootInDirection(Vector2 direction, string overrideBulletName = null)
	{
		if (base.healthHaver.IsDead)
		{
			return;
		}
		if (this.EquippedGun == null && this.volley != null)
		{
			this.ShootInDirection(direction, this.volley, overrideBulletName);
		}
		else if (this.EquippedGun != null && this.m_inventory.CurrentGun != null)
		{
			this.AimInDirection(direction);
			this.Shoot(overrideBulletName);
			this.m_inventory.CurrentGun.ClearCooldowns();
			this.m_inventory.CurrentGun.ClearReloadData();
		}
	}

	// Token: 0x06005704 RID: 22276 RVA: 0x002128F8 File Offset: 0x00210AF8
	public void ShootAtTarget(string overrideBulletName = null)
	{
		if (base.healthHaver.IsDead)
		{
			return;
		}
		if (!base.aiActor.OverrideTarget)
		{
			base.aiActor.OverrideTarget = null;
		}
		if (base.aiActor.TargetRigidbody == null)
		{
			return;
		}
		if (this.EquippedGun == null && this.volley != null)
		{
			this.ShootAtTarget(this.volley, overrideBulletName);
		}
		else if (this.EquippedGun != null && this.m_inventory.CurrentGun != null)
		{
			this.AimAtPoint(base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox));
			this.Shoot(overrideBulletName);
			if (this.m_inventory.CurrentGun != null)
			{
				this.m_inventory.CurrentGun.ClearCooldowns();
				this.m_inventory.CurrentGun.ClearReloadData();
			}
		}
	}

	// Token: 0x06005705 RID: 22277 RVA: 0x00212A00 File Offset: 0x00210C00
	public void CeaseAttack()
	{
		if (this.m_inventory != null && this.m_inventory.CurrentGun)
		{
			this.m_inventory.CurrentGun.CeaseAttack(true, null);
		}
	}

	// Token: 0x06005706 RID: 22278 RVA: 0x00212A38 File Offset: 0x00210C38
	public void Reload()
	{
		if (this.m_inventory.CurrentGun != null)
		{
			this.m_inventory.CurrentGun.Reload();
		}
	}

	// Token: 0x06005707 RID: 22279 RVA: 0x00212A64 File Offset: 0x00210C64
	public void ShootBulletScript(BulletScriptSelector bulletScript)
	{
		BulletScriptSource braveBulletSource = this.BraveBulletSource;
		braveBulletSource.BulletManager = base.bulletBank;
		braveBulletSource.BulletScript = bulletScript;
		braveBulletSource.Initialize();
	}

	// Token: 0x06005708 RID: 22280 RVA: 0x00212A94 File Offset: 0x00210C94
	public AIBulletBank.Entry GetBulletEntry(string overrideBulletName = null)
	{
		string text = (string.IsNullOrEmpty(overrideBulletName) ? this.bulletName : overrideBulletName);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		AIBulletBank.Entry entry = null;
		List<AIBulletBank.Entry> bullets = base.bulletBank.Bullets;
		for (int i = 0; i < bullets.Count; i++)
		{
			if (bullets[i].Name == text)
			{
				entry = bullets[i];
				break;
			}
		}
		if (entry == null)
		{
			Debug.LogError(string.Format("Unknown bullet type {0} on {1}", base.transform.name, text), base.gameObject);
			return null;
		}
		return entry;
	}

	// Token: 0x06005709 RID: 22281 RVA: 0x00212B3C File Offset: 0x00210D3C
	private void OnPreDeath(Vector2 obj)
	{
		if (this.m_cachedBraveBulletSource != null)
		{
			this.m_cachedBraveBulletSource.enabled = false;
		}
		this.ToggleGunAndHandRenderers(false, "OnPreDeath");
	}

	// Token: 0x0600570A RID: 22282 RVA: 0x00212B68 File Offset: 0x00210D68
	public void ToggleGunRenderers(bool value, string reason)
	{
		if (string.IsNullOrEmpty(reason))
		{
			this.m_hideGunRenderers.BaseValue = !value;
			if (value)
			{
				this.m_hideGunRenderers.ClearOverrides();
			}
		}
		else
		{
			this.m_hideGunRenderers.SetOverride(reason, !value, null);
		}
		this.UpdateGunRenderers();
	}

	// Token: 0x0600570B RID: 22283 RVA: 0x00212BC4 File Offset: 0x00210DC4
	public void UpdateGunRenderers()
	{
		bool flag = !this.m_hideGunRenderers.Value;
		if (this.CurrentGun != null)
		{
			this.CurrentGun.ToggleRenderers(flag);
		}
	}

	// Token: 0x0600570C RID: 22284 RVA: 0x00212C00 File Offset: 0x00210E00
	public void ToggleHandRenderers(bool value, string reason)
	{
		if (string.IsNullOrEmpty(reason))
		{
			this.m_hideHandRenderers.BaseValue = !value;
			if (value)
			{
				this.m_hideHandRenderers.ClearOverrides();
			}
		}
		else
		{
			this.m_hideHandRenderers.SetOverride(reason, !value, null);
		}
		this.UpdateHandRenderers();
	}

	// Token: 0x0600570D RID: 22285 RVA: 0x00212C5C File Offset: 0x00210E5C
	public void UpdateHandRenderers()
	{
		bool flag = !this.m_hideHandRenderers.Value;
		for (int i = 0; i < this.m_attachedHands.Count; i++)
		{
			this.m_attachedHands[i].ForceRenderersOff = !flag;
		}
	}

	// Token: 0x0600570E RID: 22286 RVA: 0x00212CAC File Offset: 0x00210EAC
	public void ToggleGunAndHandRenderers(bool value, string reason)
	{
		this.ToggleGunRenderers(value, reason);
		this.ToggleHandRenderers(value, reason);
	}

	// Token: 0x0600570F RID: 22287 RVA: 0x00212CC0 File Offset: 0x00210EC0
	public void StartPreFireAnim()
	{
		if (this.CurrentGun && !string.IsNullOrEmpty(this.CurrentGun.enemyPreFireAnimation))
		{
			this.CurrentGun.spriteAnimator.Play(this.CurrentGun.enemyPreFireAnimation);
		}
	}

	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x06005710 RID: 22288 RVA: 0x00212D10 File Offset: 0x00210F10
	public bool IsPreFireComplete
	{
		get
		{
			return !this.CurrentGun || string.IsNullOrEmpty(this.CurrentGun.enemyPreFireAnimation) || !this.CurrentGun.spriteAnimator.IsPlaying(this.CurrentGun.enemyPreFireAnimation);
		}
	}

	// Token: 0x06005711 RID: 22289 RVA: 0x00212D64 File Offset: 0x00210F64
	protected void ShootAtTarget(ProjectileVolleyData volley, string overrideBulletName = null)
	{
		if (base.aiActor.TargetRigidbody == null)
		{
			return;
		}
		for (int i = 0; i < volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = volley.projectiles[i];
			float angleForShot = projectileModule.GetAngleForShot(1f, 1f, null);
			this.ShootAtTarget(projectileModule, overrideBulletName, projectileModule.positionOffset, angleForShot);
			if (projectileModule.mirror)
			{
				this.ShootAtTarget(projectileModule, overrideBulletName, projectileModule.InversePositionOffset, -angleForShot);
			}
			projectileModule.IncrementShootCount();
		}
	}

	// Token: 0x06005712 RID: 22290 RVA: 0x00212DFC File Offset: 0x00210FFC
	protected void ShootAtTarget(ProjectileModule projectileModule, string overrideBulletName, Vector3 positionOffset, float angleOffset)
	{
		if (base.aiActor.TargetRigidbody == null)
		{
			return;
		}
		Vector2 unitCenter = base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		Vector2 vector = unitCenter - this.volleyShootPosition.position.XY();
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		GameObject gameObject = projectileModule.GetCurrentProjectile().gameObject;
		AIBulletBank.Entry bulletEntry = this.GetBulletEntry(overrideBulletName);
		if (bulletEntry != null && bulletEntry.BulletObject)
		{
			gameObject = bulletEntry.BulletObject;
		}
		GameObject gameObject2 = SpawnManager.SpawnProjectile(gameObject, this.volleyShootPosition.position + Quaternion.Euler(0f, 0f, num) * positionOffset, Quaternion.Euler(0f, 0f, num + angleOffset), true);
		Projectile component = gameObject2.GetComponent<Projectile>();
		if (bulletEntry != null && bulletEntry.OverrideProjectile)
		{
			component.baseData.SetAll(bulletEntry.ProjectileData);
		}
		if (base.aiActor && base.aiActor.IsBlackPhantom)
		{
			component.baseData.speed *= base.aiActor.BlackPhantomProperties.BulletSpeedMultiplier;
		}
		component.collidesWithEnemies = base.aiActor.CanTargetEnemies;
		component.collidesWithPlayer = base.aiActor.CanTargetPlayers;
		component.Shooter = base.specRigidbody;
		if (this.PostProcessProjectile != null)
		{
			this.PostProcessProjectile(component);
		}
	}

	// Token: 0x06005713 RID: 22291 RVA: 0x00212F98 File Offset: 0x00211198
	protected void ShootInDirection(Vector2 direction, ProjectileVolleyData volley, string overrideBulletName = null)
	{
		for (int i = 0; i < volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = volley.projectiles[i];
			float angleForShot = projectileModule.GetAngleForShot(1f, 1f, null);
			this.ShootInDirection(direction, projectileModule, overrideBulletName, projectileModule.positionOffset, angleForShot);
			if (projectileModule.mirror)
			{
				this.ShootInDirection(direction, projectileModule, overrideBulletName, projectileModule.InversePositionOffset, -angleForShot);
			}
			projectileModule.IncrementShootCount();
		}
		this.SpawnVolleyShellCasing((!(this.volleyShellTransform != null)) ? this.volleyShootPosition.position : this.volleyShellTransform.position);
		if (this.gunAttachPoint && this.volleyShellTransform && this.volleyShootVfx)
		{
			if (this.usesOctantShootVFX)
			{
				int num = BraveMathCollege.VectorToOctant(this.volleyShootPosition.position - this.volleyShellTransform.position);
				GameObject gameObject = SpawnManager.SpawnVFX(this.volleyShootVfx, this.volleyShootPosition.position, Quaternion.Euler(0f, 0f, (float)(90 + num * -45)));
				tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
				component.HeightOffGround = 2f;
				base.sprite.AttachRenderer(component);
				component.IsPerpendicular = true;
				component.usesOverrideMaterial = true;
			}
			else
			{
				GameObject gameObject2 = SpawnManager.SpawnVFX(this.volleyShootVfx, this.volleyShootPosition.position, Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(direction)));
				tk2dSprite component2 = gameObject2.GetComponent<tk2dSprite>();
				component2.HeightOffGround = 2f;
				base.sprite.AttachRenderer(component2);
				component2.IsPerpendicular = true;
				component2.usesOverrideMaterial = true;
			}
		}
		AkSoundEngine.PostEvent("Play_ANM_Gull_Shoot_01", base.gameObject);
	}

	// Token: 0x06005714 RID: 22292 RVA: 0x00213188 File Offset: 0x00211388
	protected void ShootInDirection(Vector2 direction, ProjectileModule projectileModule, string overrideBulletName, Vector3 positionOffset, float angleOffset)
	{
		float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		GameObject gameObject = projectileModule.GetCurrentProjectile().gameObject;
		AIBulletBank.Entry bulletEntry = this.GetBulletEntry(overrideBulletName);
		if (bulletEntry != null && bulletEntry.BulletObject)
		{
			gameObject = bulletEntry.BulletObject;
		}
		GameObject gameObject2 = SpawnManager.SpawnProjectile(gameObject, this.volleyShootPosition.position + Quaternion.Euler(0f, 0f, num) * positionOffset, Quaternion.Euler(0f, 0f, num + angleOffset), true);
		Projectile component = gameObject2.GetComponent<Projectile>();
		if (bulletEntry != null && bulletEntry.OverrideProjectile)
		{
			component.baseData.SetAll(bulletEntry.ProjectileData);
		}
		if (base.aiActor && base.aiActor.IsBlackPhantom)
		{
			component.baseData.speed *= base.aiActor.BlackPhantomProperties.BulletSpeedMultiplier;
		}
		component.Shooter = base.specRigidbody;
		if (base.aiActor)
		{
			component.SetOwnerSafe(base.aiActor, base.aiActor.GetActorName());
		}
		if (this.PostProcessProjectile != null)
		{
			this.PostProcessProjectile(component);
		}
	}

	// Token: 0x06005715 RID: 22293 RVA: 0x002132E0 File Offset: 0x002114E0
	protected void SpawnVolleyShellCasing(Vector3 position)
	{
		if (this.volleyShellCasing != null)
		{
			GameObject gameObject = SpawnManager.SpawnDebris(this.volleyShellCasing, position, Quaternion.identity);
			ShellCasing component = gameObject.GetComponent<ShellCasing>();
			if (component != null)
			{
				component.Trigger();
			}
			DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
			if (component2 != null)
			{
				int num = ((component2.transform.right.x <= 0f) ? (-1) : 1);
				Vector3 vector = Vector3.up * (UnityEngine.Random.value * 1.5f + 1f) + -1.5f * Vector3.right * (float)num * (UnityEngine.Random.value + 1.5f);
				Vector3 vector2 = new Vector3(vector.x, 0f, vector.y);
				float y = base.transform.position.y;
				float num2 = component2.transform.position.y - y + UnityEngine.Random.value * 0.5f;
				component2.Trigger(vector2, num2, 1f);
			}
		}
	}

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x06005716 RID: 22294 RVA: 0x00213410 File Offset: 0x00211610
	public bool OnCooldown
	{
		get
		{
			return this.m_onCooldown;
		}
	}

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x06005717 RID: 22295 RVA: 0x00213418 File Offset: 0x00211618
	// (set) Token: 0x06005718 RID: 22296 RVA: 0x00213420 File Offset: 0x00211620
	public bool ManualGunAngle { get; set; }

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x06005719 RID: 22297 RVA: 0x0021342C File Offset: 0x0021162C
	// (set) Token: 0x0600571A RID: 22298 RVA: 0x00213434 File Offset: 0x00211634
	public float GunAngle { get; set; }

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x0600571B RID: 22299 RVA: 0x00213440 File Offset: 0x00211640
	public Gun CurrentGun
	{
		get
		{
			if (this.m_inventory == null)
			{
				return null;
			}
			return this.m_inventory.CurrentGun;
		}
	}

	// Token: 0x0600571C RID: 22300 RVA: 0x0021345C File Offset: 0x0021165C
	public void Initialize()
	{
		this.m_inventory = new GunInventory(base.aiActor);
		if (this.EquippedGun != null)
		{
			this.m_inventory.AddGunToInventory(this.EquippedGun, true);
			if (this.CurrentGun.singleModule != null && this.CurrentGun.singleModule.shootStyle == ProjectileModule.ShootStyle.Burst)
			{
				this.CurrentGun.singleModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			}
			this.CurrentGun.doesScreenShake = this.doesScreenShake;
			this.CurrentGun.ammo = int.MaxValue;
			SpriteOutlineManager.AddOutlineToSprite(this.CurrentGun.GetSprite(), Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.AttachRenderer(this.CurrentGun.GetSprite());
		}
		Bounds untrimmedBounds = base.sprite.GetUntrimmedBounds();
		this.attachPointCachedPosition = this.gunAttachPoint.localPosition + PhysicsEngine.PixelToUnit(this.overallGunAttachOffset);
		this.attachPointCachedFlippedPosition = this.gunAttachPoint.localPosition.WithX(untrimmedBounds.center.x + (untrimmedBounds.center.x - this.gunAttachPoint.localPosition.x)) + PhysicsEngine.PixelToUnit(this.flippedGunAttachOffset) + PhysicsEngine.PixelToUnit(this.overallGunAttachOffset);
		if (this.handObject != null)
		{
			if (this.CurrentGun != null && this.CurrentGun.Handedness == GunHandedness.OneHanded)
			{
				this.AttachNewHandToTransform(this.CurrentGun.PrimaryHandAttachPoint);
			}
			else if (this.CurrentGun != null && this.CurrentGun.Handedness == GunHandedness.TwoHanded && this.AllowTwoHands)
			{
				this.AttachNewHandToTransform(this.CurrentGun.PrimaryHandAttachPoint);
				this.AttachNewHandToTransform(this.CurrentGun.SecondaryHandAttachPoint);
			}
		}
		this.AimAtPoint(this.gunAttachPoint.position + BraveUtility.RandomSign() * new Vector3(5f, 0f, 0f));
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x002136A4 File Offset: 0x002118A4
	protected void AttachNewHandToTransform(Transform target)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.handObject.gameObject);
		gameObject.transform.parent = base.transform;
		PlayerHandController component = gameObject.GetComponent<PlayerHandController>();
		this.CurrentGun.GetSprite().AttachRenderer(component.sprite);
		component.attachPoint = target;
		this.m_attachedHands.Add(component);
		component.ForceRenderersOff = !base.renderer.enabled;
		if (base.healthHaver)
		{
			tk2dSprite component2 = component.GetComponent<tk2dSprite>();
			base.healthHaver.RegisterBodySprite(component2, false, 0);
		}
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x0021373C File Offset: 0x0021193C
	public void AimAtOverride()
	{
		Gun currentGun = this.m_inventory.CurrentGun;
		if (currentGun == null)
		{
			return;
		}
		this.GunAngle = currentGun.HandleAimRotation(this.OverrideAimPoint.Value, false, 1f);
		this.HandleGunFlipping();
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x00213790 File Offset: 0x00211990
	public void AimAtTarget()
	{
		if (base.aiActor.TargetRigidbody == null && this.OverrideAimPoint == null)
		{
			return;
		}
		if (this.OverrideAimPoint != null)
		{
			this.AimAtOverride();
			return;
		}
		this.AimAtPoint(base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox));
	}

	// Token: 0x06005720 RID: 22304 RVA: 0x002137FC File Offset: 0x002119FC
	public void AimAtPoint(Vector2 point)
	{
		if (this.OverrideAimPoint != null)
		{
			this.AimAtOverride();
			return;
		}
		if (this.m_inventory == null)
		{
			return;
		}
		Gun currentGun = this.m_inventory.CurrentGun;
		if (currentGun == null)
		{
			return;
		}
		float num = ((!this.IsReallyBigBoy) ? 5f : 10f);
		if (Vector2.Distance(base.specRigidbody.UnitCenter, point) < num)
		{
			point = (point - base.specRigidbody.UnitCenter).normalized * num + base.specRigidbody.UnitCenter;
		}
		this.GunAngle = currentGun.HandleAimRotation(point, true, this.AimTimeScale);
		this.HandleGunFlipping();
	}

	// Token: 0x06005721 RID: 22305 RVA: 0x002138CC File Offset: 0x00211ACC
	public void AimInDirection(Vector2 direction)
	{
		if (this.OverrideAimPoint != null)
		{
			this.AimAtOverride();
			return;
		}
		Vector3 vector = base.aiActor.CenterPosition + direction * 5f;
		this.AimAtPoint(vector);
	}

	// Token: 0x06005722 RID: 22306 RVA: 0x00213920 File Offset: 0x00211B20
	public void Shoot(string overrideBulletName = null)
	{
		if (this.EquippedGun == null && this.volley != null)
		{
			for (int i = 0; i < this.volley.projectiles.Count; i++)
			{
				ProjectileModule projectileModule = this.volley.projectiles[i];
				float angleForShot = projectileModule.GetAngleForShot(1f, 1f, null);
				this.ShootAtTarget(projectileModule, overrideBulletName, projectileModule.positionOffset, angleForShot);
				if (projectileModule.mirror)
				{
					this.ShootAtTarget(projectileModule, overrideBulletName, projectileModule.InversePositionOffset, -angleForShot);
				}
				projectileModule.IncrementShootCount();
			}
		}
		else if (this.EquippedGun != null && this.m_inventory.CurrentGun != null)
		{
			Gun currentGun = this.m_inventory.CurrentGun;
			AIBulletBank.Entry bulletEntry = this.GetBulletEntry(overrideBulletName);
			if (this.PostProcessProjectile != null)
			{
				Gun gun = currentGun;
				gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, this.PostProcessProjectile);
			}
			if (bulletEntry != null)
			{
				currentGun.Attack((!bulletEntry.OverrideProjectile) ? null : bulletEntry.ProjectileData, bulletEntry.BulletObject);
			}
			else
			{
				currentGun.Attack(null, null);
			}
			Gun gun2 = currentGun;
			gun2.PostProcessProjectile = (Action<Projectile>)Delegate.Remove(gun2.PostProcessProjectile, this.PostProcessProjectile);
			if (this.m_inventory.CurrentGun != null)
			{
				this.m_inventory.CurrentGun.ClearCooldowns();
				this.m_inventory.CurrentGun.ClearReloadData();
			}
		}
	}

	// Token: 0x06005723 RID: 22307 RVA: 0x00213AC8 File Offset: 0x00211CC8
	public void ContinueShoot(Vector3 targetPosition)
	{
		this.AimAtPoint(targetPosition);
		this.m_inventory.CurrentGun.ContinueAttack(true, null);
	}

	// Token: 0x06005724 RID: 22308 RVA: 0x00213AEC File Offset: 0x00211CEC
	public void ShootAtTarget(Vector3 targetPosition)
	{
		if (this.volley == null)
		{
			this.AimAtPoint(targetPosition);
			this.Shoot(null);
			this.m_inventory.CurrentGun.ClearCooldowns();
			if (!this.shouldUseGunReload)
			{
				this.m_inventory.CurrentGun.ClearReloadData();
			}
		}
		else
		{
			this.ShootVolleyAtTarget(targetPosition);
		}
	}

	// Token: 0x06005725 RID: 22309 RVA: 0x00213B54 File Offset: 0x00211D54
	public void Cooldown()
	{
		float num = ((!(this.volley == null)) ? this.volley.projectiles[0].cooldownTime : this.m_inventory.CurrentGun.GetPrimaryCooldown());
		if (this.customShootCooldownPeriod > 0f)
		{
			num = this.customShootCooldownPeriod;
		}
		base.StartCoroutine(this.HandleFireRate(num));
	}

	// Token: 0x06005726 RID: 22310 RVA: 0x00213BC4 File Offset: 0x00211DC4
	public void Cooldown(float t)
	{
		base.StartCoroutine(this.HandleFireRate(t));
	}

	// Token: 0x06005727 RID: 22311 RVA: 0x00213BD4 File Offset: 0x00211DD4
	private void HandleGunFlipping()
	{
		if (this.CurrentGun == null)
		{
			return;
		}
		if (Mathf.Abs(this.GunAngle) > 105f)
		{
			this.gunAttachPoint.localPosition = this.attachPointCachedPosition;
			this.CurrentGun.HandleSpriteFlip(true);
		}
		else if (Mathf.Abs(this.GunAngle) < 75f)
		{
			this.gunAttachPoint.localPosition = this.attachPointCachedFlippedPosition;
			this.CurrentGun.HandleSpriteFlip(false);
		}
		if (this.CurrentGun != null)
		{
			tk2dBaseSprite sprite = this.CurrentGun.GetSprite();
			if (!this.ForceGunOnTop && this.GunAngle > 0f && this.GunAngle <= 155f && this.GunAngle >= 25f)
			{
				if (!this.CurrentGun.forceFlat)
				{
					sprite.HeightOffGround = -0.075f;
				}
				for (int i = 0; i < this.m_attachedHands.Count; i++)
				{
					this.m_attachedHands[i].handHeightFromGun = 0.05f;
					this.m_attachedHands[i].sprite.HeightOffGround = 0.05f;
				}
			}
			else
			{
				float num = ((this.CurrentGun.Handedness != GunHandedness.TwoHanded) ? (-0.075f) : 0.875f);
				if (!this.CurrentGun.forceFlat)
				{
					sprite.HeightOffGround = num;
				}
				for (int j = 0; j < this.m_attachedHands.Count; j++)
				{
					this.m_attachedHands[j].handHeightFromGun = 0.15f;
					this.m_attachedHands[j].sprite.HeightOffGround = 0.15f;
				}
			}
		}
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06005728 RID: 22312 RVA: 0x00213DB4 File Offset: 0x00211FB4
	private void ShootVolleyAtTarget(Vector3 targetPosition)
	{
		Vector3 vector = targetPosition.XY() - base.aiActor.CenterPosition;
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		for (int i = 0; i < this.volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = this.volley.projectiles[i];
			float angleForShot = projectileModule.GetAngleForShot(1f, 1f, null);
			GameObject gameObject = projectileModule.GetCurrentProjectile().gameObject;
			AIBulletBank.Entry entry = this.GetBulletEntry(null);
			if (entry != null && entry.BulletObject)
			{
				gameObject = entry.BulletObject;
			}
			GameObject gameObject2 = SpawnManager.SpawnProjectile(gameObject, base.aiActor.CenterPosition + Quaternion.Euler(0f, 0f, num + angleForShot) * projectileModule.positionOffset, Quaternion.Euler(0f, 0f, num + angleForShot), true);
			Projectile projectile = gameObject2.GetComponent<Projectile>();
			if (entry != null && entry.OverrideProjectile)
			{
				projectile.baseData.SetAll(entry.ProjectileData);
			}
			if (base.aiActor && base.aiActor.IsBlackPhantom)
			{
				projectile.baseData.speed *= base.aiActor.BlackPhantomProperties.BulletSpeedMultiplier;
			}
			projectile.Shooter = base.specRigidbody;
			if (projectileModule.mirror)
			{
				gameObject = projectileModule.GetCurrentProjectile().gameObject;
				entry = this.GetBulletEntry(null);
				if (entry != null && entry.BulletObject)
				{
					gameObject = entry.BulletObject;
				}
				gameObject2 = SpawnManager.SpawnProjectile(gameObject, base.aiActor.CenterPosition + Quaternion.Euler(0f, 0f, num + angleForShot) * projectileModule.InversePositionOffset, Quaternion.Euler(0f, 0f, num - angleForShot), true);
				projectile = gameObject2.GetComponent<Projectile>();
				if (entry != null && entry.OverrideProjectile)
				{
					projectile.baseData.SetAll(entry.ProjectileData);
				}
				if (base.aiActor && base.aiActor.IsBlackPhantom)
				{
					projectile.baseData.speed *= base.aiActor.BlackPhantomProperties.BulletSpeedMultiplier;
				}
				projectile.Shooter = base.specRigidbody;
			}
			projectileModule.IncrementShootCount();
		}
	}

	// Token: 0x06005729 RID: 22313 RVA: 0x00214064 File Offset: 0x00212264
	private IEnumerator HandleFireRate(float t)
	{
		this.m_onCooldown = true;
		yield return new WaitForSeconds(t);
		this.m_onCooldown = false;
		yield break;
	}

	// Token: 0x04004FED RID: 20461
	public ProjectileVolleyData volley;

	// Token: 0x04004FEE RID: 20462
	[HideInInspectorIf("volley", false)]
	[PickupIdentifier]
	public int equippedGunId = -1;

	// Token: 0x04004FEF RID: 20463
	[HideInInspectorIf("volley", false)]
	public bool shouldUseGunReload;

	// Token: 0x04004FF0 RID: 20464
	[ShowInInspectorIf("volley", true)]
	public Transform volleyShootPosition;

	// Token: 0x04004FF1 RID: 20465
	[ShowInInspectorIf("volley", true)]
	public GameObject volleyShellCasing;

	// Token: 0x04004FF2 RID: 20466
	[ShowInInspectorIf("volley", true)]
	public Transform volleyShellTransform;

	// Token: 0x04004FF3 RID: 20467
	[ShowInInspectorIf("volley", true)]
	public GameObject volleyShootVfx;

	// Token: 0x04004FF4 RID: 20468
	[ShowInInspectorIf("volley", true)]
	public bool usesOctantShootVFX = true;

	// Token: 0x04004FF5 RID: 20469
	[Header("Bullet Properties")]
	public string bulletName = "default";

	// Token: 0x04004FF6 RID: 20470
	public float customShootCooldownPeriod;

	// Token: 0x04004FF7 RID: 20471
	public bool doesScreenShake;

	// Token: 0x04004FF8 RID: 20472
	public bool rampBullets;

	// Token: 0x04004FF9 RID: 20473
	[ShowInInspectorIf("rampBullets", false)]
	public float rampStartHeight = 2f;

	// Token: 0x04004FFA RID: 20474
	[ShowInInspectorIf("rampBullets", false)]
	public float rampTime = 1f;

	// Token: 0x04004FFB RID: 20475
	[Header("Hands")]
	public Transform gunAttachPoint;

	// Token: 0x04004FFC RID: 20476
	[FormerlySerializedAs("bulletMLAttachPoint")]
	public Transform bulletScriptAttachPoint;

	// Token: 0x04004FFD RID: 20477
	public IntVector2 overallGunAttachOffset;

	// Token: 0x04004FFE RID: 20478
	public IntVector2 flippedGunAttachOffset;

	// Token: 0x04004FFF RID: 20479
	public PlayerHandController handObject;

	// Token: 0x04005000 RID: 20480
	public bool AllowTwoHands;

	// Token: 0x04005001 RID: 20481
	public bool ForceGunOnTop;

	// Token: 0x04005002 RID: 20482
	public bool IsReallyBigBoy;

	// Token: 0x04005003 RID: 20483
	public bool BackupAimInMoveDirection;

	// Token: 0x04005005 RID: 20485
	public Action<Projectile> PostProcessProjectile;

	// Token: 0x04005006 RID: 20486
	private BulletScriptSource m_cachedBraveBulletSource;

	// Token: 0x04005007 RID: 20487
	private float m_aimTimeScale = 1f;

	// Token: 0x04005008 RID: 20488
	private bool m_hasCachedGun;

	// Token: 0x04005009 RID: 20489
	private Gun m_cachedGun;

	// Token: 0x0400500A RID: 20490
	private OverridableBool m_hideGunRenderers = new OverridableBool(false);

	// Token: 0x0400500B RID: 20491
	private OverridableBool m_hideHandRenderers = new OverridableBool(false);

	// Token: 0x0400500E RID: 20494
	private Vector3 attachPointCachedPosition;

	// Token: 0x0400500F RID: 20495
	private Vector3 attachPointCachedFlippedPosition;

	// Token: 0x04005010 RID: 20496
	private bool m_onCooldown;

	// Token: 0x04005011 RID: 20497
	private GunInventory m_inventory;

	// Token: 0x04005012 RID: 20498
	private List<PlayerHandController> m_attachedHands = new List<PlayerHandController>();
}
