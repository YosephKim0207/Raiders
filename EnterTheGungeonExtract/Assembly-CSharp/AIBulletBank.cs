using System;
using System.Collections.Generic;
using System.Diagnostics;
using Brave.BulletScript;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000FA0 RID: 4000
public class AIBulletBank : BraveBehaviour, IBulletManager
{
	// Token: 0x140000AA RID: 170
	// (add) Token: 0x060056C3 RID: 22211 RVA: 0x00211104 File Offset: 0x0020F304
	// (remove) Token: 0x060056C4 RID: 22212 RVA: 0x0021113C File Offset: 0x0020F33C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Bullet, Projectile> OnBulletSpawned;

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x060056C5 RID: 22213 RVA: 0x00211174 File Offset: 0x0020F374
	// (set) Token: 0x060056C6 RID: 22214 RVA: 0x0021117C File Offset: 0x0020F37C
	public bool PlayVfx
	{
		get
		{
			return this.m_playVfx;
		}
		set
		{
			this.m_playVfx = value;
		}
	}

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x060056C7 RID: 22215 RVA: 0x00211188 File Offset: 0x0020F388
	// (set) Token: 0x060056C8 RID: 22216 RVA: 0x00211190 File Offset: 0x0020F390
	public bool PlayAudio
	{
		get
		{
			return this.m_playAudio;
		}
		set
		{
			this.m_playAudio = value;
		}
	}

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x060056C9 RID: 22217 RVA: 0x0021119C File Offset: 0x0020F39C
	// (set) Token: 0x060056CA RID: 22218 RVA: 0x002111A4 File Offset: 0x0020F3A4
	public bool PlayShells
	{
		get
		{
			return this.m_playShells;
		}
		set
		{
			this.m_playShells = value;
		}
	}

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x060056CB RID: 22219 RVA: 0x002111B0 File Offset: 0x0020F3B0
	// (set) Token: 0x060056CC RID: 22220 RVA: 0x002111B8 File Offset: 0x0020F3B8
	public SpeculativeRigidbody FixedPlayerRigidbody { get; set; }

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x060056CD RID: 22221 RVA: 0x002111C4 File Offset: 0x0020F3C4
	// (set) Token: 0x060056CE RID: 22222 RVA: 0x002111CC File Offset: 0x0020F3CC
	public Vector2 FixedPlayerRigidbodyLastPosition { get; set; }

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x060056CF RID: 22223 RVA: 0x002111D8 File Offset: 0x0020F3D8
	// (set) Token: 0x060056D0 RID: 22224 RVA: 0x002111E0 File Offset: 0x0020F3E0
	public bool CollidesWithEnemies { get; set; }

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x060056D1 RID: 22225 RVA: 0x002111EC File Offset: 0x0020F3EC
	// (set) Token: 0x060056D2 RID: 22226 RVA: 0x002111F4 File Offset: 0x0020F3F4
	public SpeculativeRigidbody SpecificRigidbodyException { get; set; }

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x060056D3 RID: 22227 RVA: 0x00211200 File Offset: 0x0020F400
	public GameObject SoundChild
	{
		get
		{
			if (!this.m_cachedSoundChild)
			{
				this.m_cachedSoundChild = new GameObject("sound child");
				this.m_cachedSoundChild.transform.parent = base.transform;
				this.m_cachedSoundChild.transform.localPosition = Vector3.zero;
			}
			return this.m_cachedSoundChild;
		}
	}

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x060056D4 RID: 22228 RVA: 0x00211260 File Offset: 0x0020F460
	// (set) Token: 0x060056D5 RID: 22229 RVA: 0x00211268 File Offset: 0x0020F468
	public string ActorName
	{
		get
		{
			return this.m_cachedActorName;
		}
		set
		{
			this.m_cachedActorName = value;
		}
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x060056D6 RID: 22230 RVA: 0x00211274 File Offset: 0x0020F474
	// (set) Token: 0x060056D7 RID: 22231 RVA: 0x0021127C File Offset: 0x0020F47C
	public float TimeScale
	{
		get
		{
			return this.m_timeScale;
		}
		set
		{
			this.m_timeScale = value;
		}
	}

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x060056D8 RID: 22232 RVA: 0x00211288 File Offset: 0x0020F488
	// (set) Token: 0x060056D9 RID: 22233 RVA: 0x00211290 File Offset: 0x0020F490
	public bool SuppressPlayerVelocityAveraging { get; set; }

	// Token: 0x060056DA RID: 22234 RVA: 0x0021129C File Offset: 0x0020F49C
	public void Awake()
	{
		this.CollidesWithEnemies = base.aiShooter && base.aiShooter.CanShootOtherEnemies;
		this.SpecificRigidbodyException = base.specRigidbody;
		if (this.Bullets != null)
		{
			for (int i = 0; i < this.Bullets.Count; i++)
			{
				AIBulletBank.Entry entry = this.Bullets[i];
				if (entry.preloadCount > 0)
				{
					Transform[] array = new Transform[entry.preloadCount];
					for (int j = 0; j < entry.preloadCount; j++)
					{
						array[j] = SpawnManager.PoolManager.Spawn(entry.BulletObject.transform);
					}
					for (int k = 0; k < entry.preloadCount; k++)
					{
						SpawnManager.PoolManager.Despawn(array[k], null);
					}
				}
			}
		}
	}

	// Token: 0x060056DB RID: 22235 RVA: 0x00211380 File Offset: 0x0020F580
	public void Start()
	{
		if (base.aiActor)
		{
			this.m_cachedActorName = base.aiActor.GetActorName();
		}
		if (base.encounterTrackable && string.IsNullOrEmpty(this.m_cachedActorName))
		{
			this.m_cachedActorName = base.encounterTrackable.GetModifiedDisplayName();
		}
	}

	// Token: 0x060056DC RID: 22236 RVA: 0x002113E0 File Offset: 0x0020F5E0
	public void Update()
	{
		if (this.FixedPlayerRigidbody && this.FixedPlayerRigidbody.healthHaver && this.FixedPlayerRigidbody.healthHaver.IsAlive)
		{
			this.FixedPlayerRigidbodyLastPosition = this.FixedPlayerRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x060056DD RID: 22237 RVA: 0x0021143C File Offset: 0x0020F63C
	public void LateUpdate()
	{
		for (int i = 0; i < this.Bullets.Count; i++)
		{
			this.Bullets[i].m_playedEffectsThisFrame = false;
			this.Bullets[i].m_playedAudioThisFrame = false;
			this.Bullets[i].m_playedShellsThisFrame = false;
		}
	}

	// Token: 0x060056DE RID: 22238 RVA: 0x0021149C File Offset: 0x0020F69C
	protected override void OnDestroy()
	{
		if (base.aiActor && base.aiActor.TargetRigidbody && PhysicsEngine.HasInstance)
		{
			this.FixedPlayerRigidbody = base.aiActor.TargetRigidbody;
			this.FixedPlayerRigidbodyLastPosition = this.FixedPlayerRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x060056DF RID: 22239 RVA: 0x002114FC File Offset: 0x0020F6FC
	public GameObject CreateProjectileFromBank(Vector2 position, float direction, string bulletName, string spawnTransform = null, bool suppressVfx = false, bool firstBulletOfAttack = true, bool forceBlackBullet = false)
	{
		AIBulletBank.Entry bullet = this.GetBullet(bulletName);
		GameObject gameObject = bullet.BulletObject;
		if (!gameObject && base.aiShooter.CurrentGun)
		{
			gameObject = base.aiShooter.CurrentGun.singleModule.GetCurrentProjectile().gameObject;
		}
		bool flag = false;
		Projectile component = gameObject.GetComponent<Projectile>();
		if (component && component.BulletScriptSettings.preventPooling)
		{
			flag = true;
		}
		GameObject gameObject2 = SpawnManager.SpawnProjectile(gameObject, position, Quaternion.Euler(0f, 0f, direction), flag);
		Projectile component2 = gameObject2.GetComponent<Projectile>();
		if (component2 != null)
		{
			if (forceBlackBullet)
			{
				component2.ForceBlackBullet = true;
			}
			if (base.gameActor)
			{
				component2.SetOwnerSafe(base.gameActor, this.m_cachedActorName);
			}
			else if (base.encounterTrackable)
			{
				component2.OwnerName = base.encounterTrackable.GetModifiedDisplayName();
			}
			if (flag)
			{
				component2.OnSpawned();
			}
			if (bullet.suppressHitEffectsIfOffscreen || (base.healthHaver && base.healthHaver.IsBoss))
			{
				component2.hitEffects.suppressHitEffectsIfOffscreen = true;
			}
		}
		if (this.m_playAudio && bullet.PlayAudio)
		{
			bool flag2 = true;
			if (bullet.AudioLimitOncePerFrame)
			{
				flag2 &= !bullet.m_playedAudioThisFrame;
			}
			if (bullet.AudioLimitOncePerAttack)
			{
				flag2 = flag2 && firstBulletOfAttack;
			}
			if (flag2)
			{
				if (!string.IsNullOrEmpty(bullet.AudioSwitch))
				{
					AkSoundEngine.SetSwitch("WPN_Guns", bullet.AudioSwitch, this.SoundChild);
					AkSoundEngine.PostEvent(bullet.AudioEvent, this.SoundChild);
				}
				else if (this)
				{
					AkSoundEngine.PostEvent(bullet.AudioEvent, base.gameObject);
				}
				bullet.m_playedAudioThisFrame = true;
			}
		}
		if (this.m_playVfx && !suppressVfx && (!bullet.MuzzleLimitOncePerFrame || !bullet.m_playedEffectsThisFrame))
		{
			float num = direction;
			if (bullet.MuzzleInheritsTransformDirection && !string.IsNullOrEmpty(spawnTransform))
			{
				num = this.GetTransformRotation(spawnTransform);
			}
			if (bullet.MuzzleFlashEffects.type != VFXPoolType.None)
			{
				bullet.MuzzleFlashEffects.SpawnAtPosition(position, num, null, null, null, null, false, null, null, false);
				bullet.m_playedEffectsThisFrame = true;
			}
			else
			{
				Gun gun = null;
				if (base.aiShooter && base.aiShooter.enabled)
				{
					gun = base.aiShooter.CurrentGun;
				}
				if (this.OverrideGun)
				{
					gun = this.OverrideGun;
				}
				if (gun)
				{
					gun.HandleShootAnimation(null);
					gun.HandleShootEffects(null);
					bullet.m_playedEffectsThisFrame = true;
				}
			}
		}
		if (this.m_playShells && (!bullet.ShellsLimitOncePerFrame || !bullet.m_playedShellsThisFrame) && bullet.SpawnShells)
		{
			this.SpawnShellCasingAtPosition(bullet);
			bullet.m_playedShellsThisFrame = true;
		}
		Projectile component3 = gameObject2.GetComponent<Projectile>();
		if (bullet.OverrideProjectile)
		{
			component3.baseData.SetAll(bullet.ProjectileData);
			component3.UpdateSpeed();
		}
		if (base.aiActor && base.aiActor.IsBlackPhantom)
		{
			component3.baseData.speed *= base.aiActor.BlackPhantomProperties.BulletSpeedMultiplier;
			component3.UpdateSpeed();
		}
		if (GameManager.Options.DebrisQuantity != GameOptions.GenericHighMedLowOption.HIGH)
		{
			component3.damagesWalls = false;
		}
		if (base.healthHaver && base.healthHaver.IsBoss)
		{
			component3.damagesWalls = false;
		}
		bool flag3 = base.aiActor && base.aiActor.CanTargetEnemies;
		component3.collidesWithEnemies = this.CollidesWithEnemies || bullet.forceCanHitEnemies || flag3;
		component3.UpdateCollisionMask();
		component3.specRigidbody.RegisterSpecificCollisionException(this.SpecificRigidbodyException);
		component3.SendInDirection(BraveMathCollege.DegreesToVector(direction, 1f), false, true);
		if (bullet.rampBullets)
		{
			if (bullet.conditionalMinDegFromNorth <= 0f || BraveMathCollege.AbsAngleBetween(90f, base.aiAnimator.FacingDirection) > bullet.conditionalMinDegFromNorth)
			{
				component3.Ramp(bullet.rampStartHeight, bullet.rampTime);
			}
		}
		else if (this.rampBullets)
		{
			component3.Ramp(this.rampStartHeight, this.rampTime);
		}
		else if (base.aiShooter && base.aiShooter.rampBullets)
		{
			component3.Ramp(base.aiShooter.rampStartHeight, base.aiShooter.rampTime);
		}
		if (this.OnProjectileCreated != null)
		{
			this.OnProjectileCreated(component3);
		}
		return gameObject2;
	}

	// Token: 0x060056E0 RID: 22240 RVA: 0x00211A30 File Offset: 0x0020FC30
	public void PostWwiseEvent(string AudioEvent, string AudioSwitch = null)
	{
		if (!string.IsNullOrEmpty(AudioSwitch))
		{
			AkSoundEngine.SetSwitch("WPN_Guns", AudioSwitch, this.SoundChild);
			AkSoundEngine.PostEvent(AudioEvent, this.SoundChild);
		}
		else if (this)
		{
			AkSoundEngine.PostEvent(AudioEvent, base.gameObject);
		}
	}

	// Token: 0x060056E1 RID: 22241 RVA: 0x00211A84 File Offset: 0x0020FC84
	public AIBulletBank.Entry GetBullet(string bulletName = "default")
	{
		AIBulletBank.Entry entry = null;
		if (string.IsNullOrEmpty(bulletName))
		{
			bulletName = "default";
		}
		for (int i = 0; i < this.Bullets.Count; i++)
		{
			if (string.Equals(this.Bullets[i].Name, bulletName, StringComparison.OrdinalIgnoreCase))
			{
				entry = this.Bullets[i];
			}
		}
		if (entry == null && this.useDefaultBulletIfMissing)
		{
			for (int j = 0; j < this.Bullets.Count; j++)
			{
				if (this.Bullets[j].Name.ToLower() == "default")
				{
					entry = this.Bullets[j];
				}
			}
			if (entry == null && this.Bullets.Count > 0)
			{
				entry = this.Bullets[0];
			}
		}
		if (entry == null)
		{
			UnityEngine.Debug.LogError("Missing bank entry for bullet: " + bulletName + "!");
			return null;
		}
		return entry;
	}

	// Token: 0x060056E2 RID: 22242 RVA: 0x00211B8C File Offset: 0x0020FD8C
	private void SpawnShellCasingAtPosition(AIBulletBank.Entry bankEntry)
	{
		if (bankEntry.ShellPrefab != null)
		{
			float num = BraveMathCollege.ClampAngle360(bankEntry.ShellTransform.eulerAngles.z);
			Vector3 position = bankEntry.ShellTransform.position;
			GameObject gameObject = SpawnManager.SpawnDebris(bankEntry.ShellPrefab, position, Quaternion.Euler(0f, 0f, (!bankEntry.DontRotateShell) ? num : 0f));
			ShellCasing component = gameObject.GetComponent<ShellCasing>();
			if (component != null)
			{
				component.Trigger();
			}
			DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
			if (component2 != null)
			{
				float num2 = bankEntry.ShellForce + UnityEngine.Random.Range(-bankEntry.ShellForceVariance, bankEntry.ShellForceVariance);
				Vector3 vector = BraveMathCollege.DegreesToVector(num, num2);
				Vector3 vector2 = new Vector3(vector.x, 0f, vector.y);
				float num3 = base.specRigidbody.UnitBottom + bankEntry.ShellGroundOffset;
				float num4 = position.y - base.transform.position.y + 0.2f;
				float num5 = component2.transform.position.y - num3 + UnityEngine.Random.value * 0.5f;
				component2.additionalHeightBoost = num4 - num5;
				component2.Trigger(vector2, num5, 1f);
			}
		}
	}

	// Token: 0x060056E3 RID: 22243 RVA: 0x00211CF0 File Offset: 0x0020FEF0
	public Vector2 PlayerPosition()
	{
		if (this.FixedPlayerPosition != null)
		{
			return this.FixedPlayerPosition.Value;
		}
		if (this.FixedPlayerRigidbody)
		{
			if (this.FixedPlayerRigidbody.healthHaver)
			{
				return (!this.FixedPlayerRigidbody.healthHaver.IsAlive) ? this.FixedPlayerRigidbodyLastPosition : this.FixedPlayerRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
			return this.FixedPlayerRigidbody.Velocity;
		}
		else if (!base.aiActor)
		{
			Vector2 vector = ((!base.transform) ? BraveUtility.ScreenCenterWorldPoint() : base.transform.position);
			PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(vector, false);
			if (activePlayerClosestToPoint)
			{
				this.m_cachedPlayerPosition = new Vector2?(activePlayerClosestToPoint.specRigidbody.GetUnitCenter(ColliderType.HitBox));
				return this.m_cachedPlayerPosition.Value;
			}
			return BraveUtility.ScreenCenterWorldPoint();
		}
		else
		{
			if (base.aiActor.TargetRigidbody)
			{
				this.m_cachedPlayerPosition = new Vector2?(base.aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox));
				return this.m_cachedPlayerPosition.Value;
			}
			Vector2? cachedPlayerPosition = this.m_cachedPlayerPosition;
			if (cachedPlayerPosition != null)
			{
				return this.m_cachedPlayerPosition.Value;
			}
			return BraveUtility.ScreenCenterWorldPoint();
		}
	}

	// Token: 0x060056E4 RID: 22244 RVA: 0x00211E60 File Offset: 0x00210060
	public Vector2 PlayerVelocity()
	{
		if (this.FixedPlayerRigidbody)
		{
			if (this.FixedPlayerRigidbody.healthHaver && !this.FixedPlayerRigidbody.healthHaver.IsAlive)
			{
				return Vector2.zero;
			}
			PlayerController playerController = this.FixedPlayerRigidbody.gameActor as PlayerController;
			if (playerController)
			{
				return playerController.AverageVelocity;
			}
			return this.FixedPlayerRigidbody.Velocity;
		}
		else if (!base.aiActor)
		{
			Vector2 vector = ((!base.transform) ? BraveUtility.ScreenCenterWorldPoint() : base.transform.position);
			PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(vector, false);
			if (activePlayerClosestToPoint)
			{
				return (!this.SuppressPlayerVelocityAveraging) ? activePlayerClosestToPoint.AverageVelocity : activePlayerClosestToPoint.Velocity;
			}
			return Vector2.zero;
		}
		else
		{
			if (!base.aiActor.TargetRigidbody)
			{
				return Vector2.zero;
			}
			PlayerController playerController2 = base.aiActor.TargetRigidbody.gameActor as PlayerController;
			if (playerController2)
			{
				return (!this.SuppressPlayerVelocityAveraging) ? playerController2.AverageVelocity : playerController2.Velocity;
			}
			return base.aiActor.TargetRigidbody.Velocity;
		}
	}

	// Token: 0x060056E5 RID: 22245 RVA: 0x00211FBC File Offset: 0x002101BC
	public void BulletSpawnedHandler(Bullet bullet)
	{
		string text = (string.IsNullOrEmpty(bullet.BankName) ? "default" : bullet.BankName);
		GameObject gameObject = this.CreateProjectileFromBank(bullet.Position, bullet.Direction, text, bullet.SpawnTransform, bullet.SuppressVfx, bullet.FirstBulletOfAttack, bullet.ForceBlackBullet);
		Projectile component = gameObject.GetComponent<Projectile>();
		bullet.Projectile = component;
		if (component && component.BulletScriptSettings.overrideMotion)
		{
			return;
		}
		component.specRigidbody.Velocity = Vector2.zero;
		BulletScriptBehavior bulletScriptBehavior = gameObject.GetComponent<BulletScriptBehavior>();
		if (!bulletScriptBehavior)
		{
			bulletScriptBehavior = gameObject.AddComponent<BulletScriptBehavior>();
			component.braveBulletScript = bulletScriptBehavior;
		}
		component.IsBulletScript = true;
		if (this.OnBulletSpawned != null)
		{
			this.OnBulletSpawned(bullet, component);
		}
		bullet.Parent = gameObject;
		bullet.Initialize();
		bulletScriptBehavior.Initialize(bullet);
	}

	// Token: 0x060056E6 RID: 22246 RVA: 0x002120A4 File Offset: 0x002102A4
	public void RemoveBullet(Bullet deadBullet)
	{
		if (!deadBullet.DontDestroyGameObject)
		{
			if (deadBullet.Projectile && SpawnManager.HasInstance)
			{
				deadBullet.Projectile.DieInAir(false, true, true, false);
			}
			else
			{
				UnityEngine.Object.Destroy(deadBullet.Parent);
			}
		}
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x002120F8 File Offset: 0x002102F8
	public void DestroyBullet(Bullet deadBullet, bool suppressInAirEffects = false)
	{
		if (deadBullet.Parent == null)
		{
			return;
		}
		BulletScriptBehavior component = deadBullet.Parent.GetComponent<BulletScriptBehavior>();
		if (deadBullet.DontDestroyGameObject)
		{
			if (component)
			{
				component.bullet = null;
			}
			return;
		}
		if (deadBullet.Projectile && SpawnManager.HasInstance)
		{
			deadBullet.Projectile.DieInAir(suppressInAirEffects, true, true, false);
			if (component)
			{
				component.bullet = null;
			}
		}
		else
		{
			UnityEngine.Object.Destroy(deadBullet.Parent);
		}
	}

	// Token: 0x060056E8 RID: 22248 RVA: 0x0021218C File Offset: 0x0021038C
	public Transform GetTransform(string transformName)
	{
		for (int i = 0; i < this.transforms.Count; i++)
		{
			if (this.transforms[i].name == transformName)
			{
				return this.transforms[i];
			}
		}
		return null;
	}

	// Token: 0x060056E9 RID: 22249 RVA: 0x002121E0 File Offset: 0x002103E0
	public Vector2 TransformOffset(Vector2 pos, string transformName)
	{
		Transform transform = null;
		for (int i = 0; i < this.transforms.Count; i++)
		{
			if (this.transforms[i].name == transformName)
			{
				transform = this.transforms[i];
			}
		}
		if (transform == null)
		{
			return pos;
		}
		return transform.position.XY();
	}

	// Token: 0x060056EA RID: 22250 RVA: 0x00212250 File Offset: 0x00210450
	public float GetTransformRotation(string transformName)
	{
		Transform transform = null;
		for (int i = 0; i < this.transforms.Count; i++)
		{
			if (this.transforms[i].name == transformName)
			{
				transform = this.transforms[i];
			}
		}
		if (transform == null)
		{
			return 0f;
		}
		return transform.eulerAngles.z;
	}

	// Token: 0x060056EB RID: 22251 RVA: 0x002122C4 File Offset: 0x002104C4
	public Animation GetUnityAnimation()
	{
		return base.unityAnimation;
	}

	// Token: 0x04004FB8 RID: 20408
	public List<AIBulletBank.Entry> Bullets;

	// Token: 0x04004FB9 RID: 20409
	public bool useDefaultBulletIfMissing;

	// Token: 0x04004FBA RID: 20410
	public List<Transform> transforms;

	// Token: 0x04004FBB RID: 20411
	[NonSerialized]
	public bool rampBullets;

	// Token: 0x04004FBC RID: 20412
	[NonSerialized]
	public float rampStartHeight = 2f;

	// Token: 0x04004FBD RID: 20413
	[NonSerialized]
	public float rampTime = 1f;

	// Token: 0x04004FBE RID: 20414
	[NonSerialized]
	public Gun OverrideGun;

	// Token: 0x04004FC0 RID: 20416
	public Action<Projectile> OnProjectileCreated;

	// Token: 0x04004FC1 RID: 20417
	public Action<string, Projectile> OnProjectileCreatedWithSource;

	// Token: 0x04004FC2 RID: 20418
	public Vector2? FixedPlayerPosition;

	// Token: 0x04004FC7 RID: 20423
	private GameObject m_cachedSoundChild;

	// Token: 0x04004FC8 RID: 20424
	private float m_timeScale = 1f;

	// Token: 0x04004FCA RID: 20426
	private Vector2? m_cachedPlayerPosition;

	// Token: 0x04004FCB RID: 20427
	private bool m_playVfx = true;

	// Token: 0x04004FCC RID: 20428
	private bool m_playAudio = true;

	// Token: 0x04004FCD RID: 20429
	private bool m_playShells = true;

	// Token: 0x04004FCE RID: 20430
	private string m_cachedActorName;

	// Token: 0x02000FA1 RID: 4001
	[Serializable]
	public class Entry
	{
		// Token: 0x060056EC RID: 22252 RVA: 0x002122CC File Offset: 0x002104CC
		public Entry()
		{
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x0021231C File Offset: 0x0021051C
		public Entry(AIBulletBank.Entry other)
		{
			this.Name = other.Name;
			this.BulletObject = other.BulletObject;
			this.OverrideProjectile = other.OverrideProjectile;
			this.ProjectileData = new ProjectileData(other.ProjectileData);
			this.PlayAudio = other.PlayAudio;
			this.AudioSwitch = other.AudioSwitch;
			this.AudioEvent = other.AudioEvent;
			this.AudioLimitOncePerFrame = other.AudioLimitOncePerFrame;
			this.AudioLimitOncePerAttack = other.AudioLimitOncePerAttack;
			this.MuzzleFlashEffects = other.MuzzleFlashEffects;
			this.MuzzleLimitOncePerFrame = other.MuzzleLimitOncePerFrame;
			this.MuzzleInheritsTransformDirection = other.MuzzleInheritsTransformDirection;
			this.SpawnShells = other.SpawnShells;
			this.ShellTransform = other.ShellTransform;
			this.ShellPrefab = other.ShellPrefab;
			this.ShellForce = other.ShellForce;
			this.ShellForceVariance = other.ShellForceVariance;
			this.DontRotateShell = other.DontRotateShell;
			this.ShellGroundOffset = other.ShellGroundOffset;
			this.ShellsLimitOncePerFrame = other.ShellsLimitOncePerFrame;
			this.rampBullets = other.rampBullets;
			this.rampStartHeight = other.rampStartHeight;
			this.rampTime = other.rampTime;
			this.conditionalMinDegFromNorth = other.conditionalMinDegFromNorth;
			this.forceCanHitEnemies = other.forceCanHitEnemies;
			this.suppressHitEffectsIfOffscreen = other.suppressHitEffectsIfOffscreen;
			this.preloadCount = other.preloadCount;
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x002124B4 File Offset: 0x002106B4
		// (set) Token: 0x060056EF RID: 22255 RVA: 0x002124BC File Offset: 0x002106BC
		public bool m_playedAudioThisFrame { get; set; }

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x060056F0 RID: 22256 RVA: 0x002124C8 File Offset: 0x002106C8
		// (set) Token: 0x060056F1 RID: 22257 RVA: 0x002124D0 File Offset: 0x002106D0
		public bool m_playedEffectsThisFrame { get; set; }

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x060056F2 RID: 22258 RVA: 0x002124DC File Offset: 0x002106DC
		// (set) Token: 0x060056F3 RID: 22259 RVA: 0x002124E4 File Offset: 0x002106E4
		public bool m_playedShellsThisFrame { get; set; }

		// Token: 0x04004FCF RID: 20431
		public string Name;

		// Token: 0x04004FD0 RID: 20432
		public GameObject BulletObject;

		// Token: 0x04004FD1 RID: 20433
		public bool OverrideProjectile;

		// Token: 0x04004FD2 RID: 20434
		[ShowInInspectorIf("OverrideProjectile", false)]
		public ProjectileData ProjectileData;

		// Token: 0x04004FD3 RID: 20435
		[FormerlySerializedAs("BulletMlAudio")]
		public bool PlayAudio;

		// Token: 0x04004FD4 RID: 20436
		[ShowInInspectorIf("PlayAudio", true)]
		[FormerlySerializedAs("BulletMlAudioSwitch")]
		public string AudioSwitch;

		// Token: 0x04004FD5 RID: 20437
		[ShowInInspectorIf("PlayAudio", true)]
		[FormerlySerializedAs("BulletMlAudioEvent")]
		public string AudioEvent;

		// Token: 0x04004FD6 RID: 20438
		[ShowInInspectorIf("PlayAudio", true)]
		[FormerlySerializedAs("LimitBulletMlAudio")]
		public bool AudioLimitOncePerFrame = true;

		// Token: 0x04004FD7 RID: 20439
		[ShowInInspectorIf("PlayAudio", true)]
		public bool AudioLimitOncePerAttack;

		// Token: 0x04004FD8 RID: 20440
		public VFXPool MuzzleFlashEffects;

		// Token: 0x04004FD9 RID: 20441
		[ShowInInspectorIf("MuzzleFlashEffects", true)]
		[FormerlySerializedAs("LimitBulletMlVfx")]
		public bool MuzzleLimitOncePerFrame = true;

		// Token: 0x04004FDA RID: 20442
		[ShowInInspectorIf("MuzzleFlashEffects", true)]
		public bool MuzzleInheritsTransformDirection;

		// Token: 0x04004FDB RID: 20443
		public bool SpawnShells;

		// Token: 0x04004FDC RID: 20444
		[ShowInInspectorIf("SpawnShells", true)]
		public Transform ShellTransform;

		// Token: 0x04004FDD RID: 20445
		[ShowInInspectorIf("SpawnShells", true)]
		public GameObject ShellPrefab;

		// Token: 0x04004FDE RID: 20446
		[ShowInInspectorIf("SpawnShells", true)]
		public float ShellForce = 1.75f;

		// Token: 0x04004FDF RID: 20447
		[ShowInInspectorIf("SpawnShells", true)]
		public float ShellForceVariance = 0.75f;

		// Token: 0x04004FE0 RID: 20448
		[ShowInInspectorIf("SpawnShells", true)]
		public bool DontRotateShell;

		// Token: 0x04004FE1 RID: 20449
		[ShowInInspectorIf("SpawnShells", true)]
		public float ShellGroundOffset;

		// Token: 0x04004FE2 RID: 20450
		[ShowInInspectorIf("SpawnShells", true)]
		public bool ShellsLimitOncePerFrame;

		// Token: 0x04004FE3 RID: 20451
		public bool rampBullets;

		// Token: 0x04004FE4 RID: 20452
		[ShowInInspectorIf("rampBullets", true)]
		public float rampStartHeight = 2f;

		// Token: 0x04004FE5 RID: 20453
		[ShowInInspectorIf("rampBullets", true)]
		public float rampTime = 1f;

		// Token: 0x04004FE6 RID: 20454
		[ShowInInspectorIf("rampBullets", true)]
		public float conditionalMinDegFromNorth;

		// Token: 0x04004FE7 RID: 20455
		public bool forceCanHitEnemies;

		// Token: 0x04004FE8 RID: 20456
		public bool suppressHitEffectsIfOffscreen;

		// Token: 0x04004FE9 RID: 20457
		public int preloadCount;
	}
}
