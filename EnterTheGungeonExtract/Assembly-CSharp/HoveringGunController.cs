using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020016F2 RID: 5874
public class HoveringGunController : BraveBehaviour, IPlayerOrbital
{
	// Token: 0x06008884 RID: 34948 RVA: 0x003894C4 File Offset: 0x003876C4
	public void Initialize(Gun targetGun, PlayerController owner)
	{
		this.m_targetGun = targetGun;
		this.m_owner = owner;
		this.m_parentTransform = new GameObject("hover rotator").transform;
		this.m_parentTransform.parent = base.transform.parent;
		base.transform.parent = this.m_parentTransform;
		base.sprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.MiddleCenter);
		base.sprite.SetSprite(targetGun.sprite.Collection, targetGun.sprite.spriteId);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		this.m_shootPointTransform = new GameObject("shoot point").transform;
		this.m_shootPointTransform.parent = base.transform;
		this.m_shootPointTransform.localPosition = targetGun.barrelOffset.localPosition;
		if (this.Position == HoveringGunController.HoverPosition.CIRCULATE)
		{
			this.SetOrbitalTier(PlayerOrbital.CalculateTargetTier(this.m_owner, this));
			this.SetOrbitalTierIndex(PlayerOrbital.GetNumberOfOrbitalsInTier(this.m_owner, this.GetOrbitalTier()));
			this.m_owner.orbitals.Add(this);
			this.m_ownerCenterAverage = this.m_owner.CenterPosition;
		}
		if (this.Trigger == HoveringGunController.FireType.ON_DODGED_BULLET)
		{
			this.m_owner.OnDodgedProjectile += this.HandleDodgedProjectileFire;
		}
		if (this.Trigger == HoveringGunController.FireType.ON_FIRED_GUN)
		{
			this.m_owner.PostProcessProjectile += this.HandleFiredGun;
		}
		if (this.Aim == HoveringGunController.AimType.NEAREST_ENEMY)
		{
			this.m_fireCooldown = 0.25f;
		}
		this.UpdatePosition();
		LootEngine.DoDefaultSynergyPoof(base.sprite.WorldCenter, false);
		this.m_initialized = true;
	}

	// Token: 0x06008885 RID: 34949 RVA: 0x0038966C File Offset: 0x0038786C
	private void HandleFiredGun(Projectile arg1, float arg2)
	{
		if (this.m_fireCooldown <= 0f)
		{
			this.Fire();
		}
	}

	// Token: 0x06008886 RID: 34950 RVA: 0x00389684 File Offset: 0x00387884
	private void HandleDodgedProjectileFire(Projectile sourceProjectile)
	{
		if (this.m_fireCooldown <= 0f && sourceProjectile.collidesWithPlayer)
		{
			this.Fire();
		}
	}

	// Token: 0x06008887 RID: 34951 RVA: 0x003896A8 File Offset: 0x003878A8
	public void LateUpdate()
	{
		if (!this.m_initialized)
		{
			return;
		}
		if (Dungeon.IsGenerating || GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		this.UpdatePosition();
		this.UpdateFiring();
	}

	// Token: 0x06008888 RID: 34952 RVA: 0x003896DC File Offset: 0x003878DC
	private void AimAt(Vector2 point, bool instant = false)
	{
		Vector2 vector = point - base.sprite.WorldCenter;
		float num = BraveMathCollege.Atan2Degrees(vector);
		this.m_currentAimTarget = num;
		if (instant)
		{
			this.m_parentTransform.localRotation = Quaternion.Euler(0f, 0f, this.m_currentAimTarget);
		}
	}

	// Token: 0x06008889 RID: 34953 RVA: 0x00389730 File Offset: 0x00387930
	private void UpdatePosition()
	{
		HoveringGunController.AimType aim = this.Aim;
		if (aim != HoveringGunController.AimType.NEAREST_ENEMY)
		{
			if (aim == HoveringGunController.AimType.PLAYER_AIM)
			{
				this.AimAt(this.m_owner.unadjustedAimPoint.XY(), false);
			}
		}
		else
		{
			bool flag = false;
			if (this.m_owner && this.m_owner.CurrentRoom != null)
			{
				float num = -1f;
				AIActor nearestEnemy = this.m_owner.CurrentRoom.GetNearestEnemy(this.m_owner.CenterPosition, out num, true, false);
				if (nearestEnemy)
				{
					this.m_hasEnemyTarget = true;
					this.AimAt(nearestEnemy.CenterPosition, false);
					flag = true;
				}
			}
			if (!flag)
			{
				this.m_hasEnemyTarget = false;
				this.AimAt(this.m_owner.unadjustedAimPoint.XY(), false);
			}
		}
		this.m_parentTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.MoveTowardsAngle(this.m_parentTransform.localRotation.eulerAngles.z, this.m_currentAimTarget, this.AimRotationAngularSpeed * BraveTime.DeltaTime));
		bool flag2 = this.m_parentTransform.localRotation.eulerAngles.z > 90f && this.m_parentTransform.localRotation.eulerAngles.z < 270f;
		if (flag2 && !base.sprite.FlipY)
		{
			base.transform.localPosition += new Vector3(0f, base.sprite.GetUntrimmedBounds().extents.y, 0f);
			this.m_shootPointTransform.localPosition = this.m_shootPointTransform.localPosition.WithY(-this.m_shootPointTransform.localPosition.y);
			base.sprite.FlipY = true;
		}
		else if (!flag2 && base.sprite.FlipY)
		{
			base.sprite.FlipY = false;
			base.transform.localPosition -= new Vector3(0f, base.sprite.GetUntrimmedBounds().extents.y, 0f);
			this.m_shootPointTransform.localPosition = this.m_shootPointTransform.localPosition.WithY(-this.m_shootPointTransform.localPosition.y);
		}
		HoveringGunController.HoverPosition position = this.Position;
		if (position != HoveringGunController.HoverPosition.OVERHEAD)
		{
			if (position == HoveringGunController.HoverPosition.CIRCULATE)
			{
				this.HandleOrbitalMotion();
			}
		}
		else
		{
			this.m_parentTransform.position = (this.m_owner.CenterPosition + new Vector2(0f, 1.5f)).ToVector3ZisY(0f);
			base.sprite.HeightOffGround = 2f;
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x0600888A RID: 34954 RVA: 0x00389A4C File Offset: 0x00387C4C
	private void HandleOrbitalMotion()
	{
		Vector2 centerPosition = this.m_owner.CenterPosition;
		if (Vector2.Distance(centerPosition, this.m_parentTransform.position.XY()) > 20f)
		{
			this.m_parentTransform.position = centerPosition.ToVector3ZUp(0f);
			this.m_ownerCenterAverage = centerPosition;
			if (base.specRigidbody)
			{
				base.specRigidbody.Reinitialize();
			}
		}
		Vector2 vector = centerPosition - this.m_ownerCenterAverage;
		float num = Mathf.Lerp(0.1f, 15f, vector.magnitude / 4f);
		float num2 = Mathf.Min(num * BraveTime.DeltaTime, vector.magnitude);
		float num3 = 360f / (float)PlayerOrbital.GetNumberOfOrbitalsInTier(this.m_owner, this.GetOrbitalTier()) * (float)this.GetOrbitalTierIndex() + BraveTime.ScaledTimeSinceStartup * this.GetOrbitalRotationalSpeed();
		Vector2 vector2 = this.m_ownerCenterAverage + (centerPosition - this.m_ownerCenterAverage).normalized * num2;
		Vector2 vector3 = vector2 + (Quaternion.Euler(0f, 0f, num3) * Vector3.right * this.GetOrbitalRadius()).XY();
		this.m_ownerCenterAverage = vector2;
		vector3 = vector3.Quantize(0.0625f);
		Vector2 vector4 = (vector3 - this.m_parentTransform.position.XY()) / BraveTime.DeltaTime;
		if (base.specRigidbody)
		{
			base.specRigidbody.Velocity = vector4;
		}
		else
		{
			this.m_parentTransform.position = vector3.ToVector3ZisY(0f);
			base.sprite.HeightOffGround = 0.5f;
			base.sprite.UpdateZDepth();
		}
		this.m_orbitalAngle = num3 % 360f;
	}

	// Token: 0x0600888B RID: 34955 RVA: 0x00389C24 File Offset: 0x00387E24
	private void UpdateFiring()
	{
		if (this.m_fireCooldown <= 0f)
		{
			HoveringGunController.FireType trigger = this.Trigger;
			if (trigger != HoveringGunController.FireType.ON_RELOAD)
			{
				if (trigger != HoveringGunController.FireType.ON_COOLDOWN)
				{
					if (trigger != HoveringGunController.FireType.ON_DODGED_BULLET)
					{
					}
				}
				else if (this.Aim != HoveringGunController.AimType.NEAREST_ENEMY || this.m_hasEnemyTarget)
				{
					this.Fire();
				}
			}
			else if (this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.IsReloading && (!this.OnlyOnEmptyReload || this.m_owner.CurrentGun.ClipShotsRemaining <= 0))
			{
				this.Fire();
			}
		}
		else
		{
			this.m_fireCooldown = (this.m_fireCooldown -= BraveTime.DeltaTime);
		}
	}

	// Token: 0x17001458 RID: 5208
	// (get) Token: 0x0600888C RID: 34956 RVA: 0x00389D10 File Offset: 0x00387F10
	private Vector2 ShootPoint
	{
		get
		{
			return this.m_shootPointTransform.position.XY();
		}
	}

	// Token: 0x0600888D RID: 34957 RVA: 0x00389D24 File Offset: 0x00387F24
	private void Fire()
	{
		this.m_fireCooldown = this.CooldownTime;
		Projectile currentProjectile = this.m_targetGun.DefaultModule.GetCurrentProjectile();
		bool flag = currentProjectile.GetComponent<BeamController>() != null;
		if (!string.IsNullOrEmpty(this.ShootAudioEvent))
		{
			AkSoundEngine.PostEvent(this.ShootAudioEvent, base.gameObject);
		}
		if (flag)
		{
			this.m_owner.StartCoroutine(this.HandleFireShortBeam(currentProjectile, this.m_owner, this.ShootDuration));
			this.m_fireCooldown = Mathf.Max(this.m_fireCooldown, this.ShootDuration);
		}
		else if (this.m_targetGun.Volley != null)
		{
			if (this.ShootDuration > 0f)
			{
				base.StartCoroutine(this.FireVolleyForDuration(this.m_targetGun.Volley, this.m_owner, this.ShootDuration));
			}
			else
			{
				this.FireVolley(this.m_targetGun.Volley, this.m_owner, this.m_parentTransform.eulerAngles.z, new Vector2?(this.ShootPoint));
			}
		}
		else
		{
			ProjectileModule defaultModule = this.m_targetGun.DefaultModule;
			Projectile currentProjectile2 = defaultModule.GetCurrentProjectile();
			if (currentProjectile2)
			{
				float angleForShot = defaultModule.GetAngleForShot(1f, 1f, null);
				if (!flag)
				{
					this.DoSingleProjectile(currentProjectile2, this.m_owner, this.m_parentTransform.eulerAngles.z + angleForShot, new Vector2?(this.ShootPoint), true);
				}
			}
		}
	}

	// Token: 0x0600888E RID: 34958 RVA: 0x00389EBC File Offset: 0x003880BC
	private IEnumerator HandleFireShortBeam(Projectile projectileToSpawn, PlayerController source, float duration)
	{
		float elapsed = 0f;
		BeamController beam = this.BeginFiringBeam(projectileToSpawn, source, this.m_parentTransform.eulerAngles.z, new Vector2?(this.ShootPoint));
		yield return null;
		while (elapsed < duration)
		{
			if (!this.m_shootPointTransform || !this)
			{
				break;
			}
			if (!this.m_parentTransform)
			{
				break;
			}
			elapsed += BraveTime.DeltaTime;
			this.ContinueFiringBeam(beam, source, this.m_parentTransform.eulerAngles.z, new Vector2?(this.ShootPoint));
			yield return null;
		}
		this.CeaseBeam(beam);
		if (!string.IsNullOrEmpty(this.FinishedShootingAudioEvent) && this)
		{
			AkSoundEngine.PostEvent(this.FinishedShootingAudioEvent, base.gameObject);
		}
		yield break;
	}

	// Token: 0x0600888F RID: 34959 RVA: 0x00389EEC File Offset: 0x003880EC
	private IEnumerator FireVolleyForDuration(ProjectileVolleyData volley, PlayerController source, float duration)
	{
		float elapsed = 0f;
		float cooldown = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			cooldown -= BraveTime.DeltaTime;
			if (cooldown <= 0f)
			{
				this.FireVolley(volley, source, this.m_parentTransform.eulerAngles.z, new Vector2?(this.ShootPoint));
				cooldown = this.m_targetGun.DefaultModule.cooldownTime;
				for (int i = 0; i < volley.projectiles.Count; i++)
				{
					if (volley.projectiles[i].shootStyle == ProjectileModule.ShootStyle.Charged)
					{
						cooldown = Mathf.Max(cooldown, volley.projectiles[i].maxChargeTime);
						cooldown = Mathf.Max(cooldown, 0.5f);
					}
				}
			}
			yield return null;
		}
		this.m_fireCooldown = this.CooldownTime;
		if (!string.IsNullOrEmpty(this.FinishedShootingAudioEvent))
		{
			AkSoundEngine.PostEvent(this.FinishedShootingAudioEvent, base.gameObject);
		}
		yield break;
	}

	// Token: 0x06008890 RID: 34960 RVA: 0x00389F1C File Offset: 0x0038811C
	private void FireVolley(ProjectileVolleyData volley, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
	{
		if (!string.IsNullOrEmpty(this.OnEveryShotAudioEvent))
		{
			AkSoundEngine.PostEvent(this.OnEveryShotAudioEvent, base.gameObject);
		}
		for (int i = 0; i < volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = volley.projectiles[i];
			Projectile currentProjectile = projectileModule.GetCurrentProjectile();
			if (currentProjectile)
			{
				float angleForShot = projectileModule.GetAngleForShot(1f, 1f, null);
				bool flag = currentProjectile.GetComponent<BeamController>() != null;
				if (!flag)
				{
					this.DoSingleProjectile(currentProjectile, source, targetAngle + angleForShot, overrideSpawnPoint, false);
				}
			}
		}
	}

	// Token: 0x06008891 RID: 34961 RVA: 0x00389FCC File Offset: 0x003881CC
	private void DoSingleProjectile(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint, bool doAudio = false)
	{
		if (doAudio && !string.IsNullOrEmpty(this.OnEveryShotAudioEvent))
		{
			AkSoundEngine.PostEvent(this.OnEveryShotAudioEvent, base.gameObject);
		}
		if (this.ConsumesTargetGunAmmo && this.m_targetGun && this.m_owner.inventory.AllGuns.Contains(this.m_targetGun))
		{
			if (this.m_targetGun.ammo == 0)
			{
				return;
			}
			if (UnityEngine.Random.value < this.ChanceToConsumeTargetGunAmmo)
			{
				this.m_targetGun.LoseAmmo(1);
			}
		}
		Vector2 vector = ((overrideSpawnPoint == null) ? source.specRigidbody.UnitCenter : overrideSpawnPoint.Value);
		GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.Euler(0f, 0f, targetAngle), true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = source;
		component.Shooter = source.specRigidbody;
		source.DoPostProcessProjectile(component);
		BounceProjModifier component2 = component.GetComponent<BounceProjModifier>();
		if (component2)
		{
			component2.numberOfBounces = Mathf.Min(3, component2.numberOfBounces);
		}
	}

	// Token: 0x06008892 RID: 34962 RVA: 0x0038A0F8 File Offset: 0x003882F8
	private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
	{
		Vector2 vector = ((overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value);
		GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = source;
		BeamController component2 = gameObject.GetComponent<BeamController>();
		component2.Owner = source;
		component2.HitsPlayers = false;
		component2.HitsEnemies = true;
		Vector3 vector2 = BraveMathCollege.DegreesToVector(targetAngle, 1f);
		component2.Direction = vector2;
		component2.Origin = vector;
		return component2;
	}

	// Token: 0x06008893 RID: 34963 RVA: 0x0038A190 File Offset: 0x00388390
	private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint)
	{
		Vector2 vector = ((overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value);
		beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
		beam.Origin = vector;
		beam.LateUpdatePosition(vector);
	}

	// Token: 0x06008894 RID: 34964 RVA: 0x0038A1E0 File Offset: 0x003883E0
	private void CeaseBeam(BeamController beam)
	{
		beam.CeaseAttack();
	}

	// Token: 0x06008895 RID: 34965 RVA: 0x0038A1E8 File Offset: 0x003883E8
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			this.m_owner.OnDodgedProjectile -= this.HandleDodgedProjectileFire;
		}
		if (this.m_owner)
		{
			this.m_owner.PostProcessProjectile -= this.HandleFiredGun;
		}
		if (!string.IsNullOrEmpty(this.FinishedShootingAudioEvent))
		{
			AkSoundEngine.PostEvent(this.FinishedShootingAudioEvent, base.gameObject);
		}
		if (this.Position == HoveringGunController.HoverPosition.CIRCULATE)
		{
			for (int i = 0; i < this.m_owner.orbitals.Count; i++)
			{
				if (this.m_owner.orbitals[i].GetOrbitalTier() == this.GetOrbitalTier() && this.m_owner.orbitals[i].GetOrbitalTierIndex() > this.GetOrbitalTierIndex())
				{
					this.m_owner.orbitals[i].SetOrbitalTierIndex(this.m_owner.orbitals[i].GetOrbitalTierIndex() - 1);
				}
			}
			this.m_owner.orbitals.Remove(this);
		}
		LootEngine.DoDefaultSynergyPoof(base.sprite.WorldCenter, false);
	}

	// Token: 0x06008896 RID: 34966 RVA: 0x0038A328 File Offset: 0x00388528
	public void Reinitialize()
	{
		if (base.specRigidbody)
		{
			base.specRigidbody.Reinitialize();
		}
		this.m_ownerCenterAverage = this.m_owner.CenterPosition;
	}

	// Token: 0x06008897 RID: 34967 RVA: 0x0038A358 File Offset: 0x00388558
	public Transform GetTransform()
	{
		return this.m_parentTransform;
	}

	// Token: 0x06008898 RID: 34968 RVA: 0x0038A360 File Offset: 0x00388560
	public void ToggleRenderer(bool visible)
	{
		base.sprite.renderer.enabled = visible;
	}

	// Token: 0x06008899 RID: 34969 RVA: 0x0038A374 File Offset: 0x00388574
	public int GetOrbitalTier()
	{
		return this.m_orbitalTier;
	}

	// Token: 0x0600889A RID: 34970 RVA: 0x0038A37C File Offset: 0x0038857C
	public void SetOrbitalTier(int tier)
	{
		this.m_orbitalTier = tier;
	}

	// Token: 0x0600889B RID: 34971 RVA: 0x0038A388 File Offset: 0x00388588
	public int GetOrbitalTierIndex()
	{
		return this.m_orbitalTierIndex;
	}

	// Token: 0x0600889C RID: 34972 RVA: 0x0038A390 File Offset: 0x00388590
	public void SetOrbitalTierIndex(int tierIndex)
	{
		this.m_orbitalTierIndex = tierIndex;
	}

	// Token: 0x0600889D RID: 34973 RVA: 0x0038A39C File Offset: 0x0038859C
	public float GetOrbitalRadius()
	{
		return 2.5f;
	}

	// Token: 0x0600889E RID: 34974 RVA: 0x0038A3A4 File Offset: 0x003885A4
	public float GetOrbitalRotationalSpeed()
	{
		return 120f;
	}

	// Token: 0x04008DE8 RID: 36328
	public HoveringGunController.HoverPosition Position;

	// Token: 0x04008DE9 RID: 36329
	public HoveringGunController.FireType Trigger;

	// Token: 0x04008DEA RID: 36330
	public HoveringGunController.AimType Aim;

	// Token: 0x04008DEB RID: 36331
	public float AimRotationAngularSpeed = 360f;

	// Token: 0x04008DEC RID: 36332
	public float ShootDuration = 2f;

	// Token: 0x04008DED RID: 36333
	public float CooldownTime = 1f;

	// Token: 0x04008DEE RID: 36334
	public bool OnlyOnEmptyReload;

	// Token: 0x04008DEF RID: 36335
	public bool ConsumesTargetGunAmmo;

	// Token: 0x04008DF0 RID: 36336
	public float ChanceToConsumeTargetGunAmmo = 1f;

	// Token: 0x04008DF1 RID: 36337
	public string ShootAudioEvent;

	// Token: 0x04008DF2 RID: 36338
	public string OnEveryShotAudioEvent;

	// Token: 0x04008DF3 RID: 36339
	public string FinishedShootingAudioEvent;

	// Token: 0x04008DF4 RID: 36340
	private bool m_initialized;

	// Token: 0x04008DF5 RID: 36341
	private Transform m_parentTransform;

	// Token: 0x04008DF6 RID: 36342
	private Transform m_shootPointTransform;

	// Token: 0x04008DF7 RID: 36343
	private Gun m_targetGun;

	// Token: 0x04008DF8 RID: 36344
	private PlayerController m_owner;

	// Token: 0x04008DF9 RID: 36345
	private float m_currentAimTarget;

	// Token: 0x04008DFA RID: 36346
	private bool m_hasEnemyTarget;

	// Token: 0x04008DFB RID: 36347
	private float m_fireCooldown;

	// Token: 0x04008DFC RID: 36348
	private Vector2 m_ownerCenterAverage;

	// Token: 0x04008DFD RID: 36349
	private float m_orbitalAngle;

	// Token: 0x04008DFE RID: 36350
	private int m_orbitalTier;

	// Token: 0x04008DFF RID: 36351
	private int m_orbitalTierIndex;

	// Token: 0x020016F3 RID: 5875
	public enum HoverPosition
	{
		// Token: 0x04008E01 RID: 36353
		OVERHEAD,
		// Token: 0x04008E02 RID: 36354
		CIRCULATE
	}

	// Token: 0x020016F4 RID: 5876
	public enum FireType
	{
		// Token: 0x04008E04 RID: 36356
		ON_RELOAD,
		// Token: 0x04008E05 RID: 36357
		ON_COOLDOWN,
		// Token: 0x04008E06 RID: 36358
		ON_DODGED_BULLET,
		// Token: 0x04008E07 RID: 36359
		ON_FIRED_GUN
	}

	// Token: 0x020016F5 RID: 5877
	public enum AimType
	{
		// Token: 0x04008E09 RID: 36361
		NEAREST_ENEMY,
		// Token: 0x04008E0A RID: 36362
		PLAYER_AIM
	}
}
