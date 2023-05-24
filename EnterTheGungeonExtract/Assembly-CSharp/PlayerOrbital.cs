using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001461 RID: 5217
public class PlayerOrbital : BraveBehaviour, IPlayerOrbital
{
	// Token: 0x170011A5 RID: 4517
	// (get) Token: 0x06007682 RID: 30338 RVA: 0x002F2E78 File Offset: 0x002F1078
	public PlayerController Owner
	{
		get
		{
			return this.m_owner;
		}
	}

	// Token: 0x06007683 RID: 30339 RVA: 0x002F2E80 File Offset: 0x002F1080
	public static int GetNumberOfOrbitalsInTier(PlayerController owner, int tier)
	{
		int num = 0;
		for (int i = 0; i < owner.orbitals.Count; i++)
		{
			if (owner.orbitals[i].GetOrbitalTier() == tier)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06007684 RID: 30340 RVA: 0x002F2EC8 File Offset: 0x002F10C8
	public static int CalculateTargetTier(PlayerController owner, IPlayerOrbital orbital)
	{
		float orbitalRadius = orbital.GetOrbitalRadius();
		float orbitalRotationalSpeed = orbital.GetOrbitalRotationalSpeed();
		int num = -1;
		for (int i = 0; i < owner.orbitals.Count; i++)
		{
			if (owner.orbitals[i] != orbital)
			{
				num = Mathf.Max(num, owner.orbitals[i].GetOrbitalTier());
				float orbitalRadius2 = owner.orbitals[i].GetOrbitalRadius();
				float orbitalRotationalSpeed2 = owner.orbitals[i].GetOrbitalRotationalSpeed();
				if (Mathf.Approximately(orbitalRadius2, orbitalRadius) && Mathf.Approximately(orbitalRotationalSpeed2, orbitalRotationalSpeed))
				{
					return owner.orbitals[i].GetOrbitalTier();
				}
			}
		}
		return num + 1;
	}

	// Token: 0x06007685 RID: 30341 RVA: 0x002F2F88 File Offset: 0x002F1188
	public void Initialize(PlayerController owner)
	{
		this.m_initialized = true;
		this.m_owner = owner;
		this.SetOrbitalTier(PlayerOrbital.CalculateTargetTier(owner, this));
		this.SetOrbitalTierIndex(PlayerOrbital.GetNumberOfOrbitalsInTier(owner, this.m_orbitalTier));
		Debug.LogError(string.Concat(new object[]
		{
			"new orbital tier: ",
			this.GetOrbitalTier(),
			" and index: ",
			this.GetOrbitalTierIndex()
		}));
		owner.orbitals.Add(this);
		base.sprite = base.GetComponentInChildren<tk2dSprite>();
		base.spriteAnimator = base.GetComponentInChildren<tk2dSpriteAnimator>();
		if (!this.PreventOutline)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		}
		this.m_ownerCenterAverage = this.m_owner.CenterPosition;
		if (base.specRigidbody && (this.DamagesEnemiesOnShot || this.TriggersMachoBraceOnShot))
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		if (base.specRigidbody && this.ExplodesOnTriggerCollision)
		{
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerCollisionExplosion));
		}
	}

	// Token: 0x06007686 RID: 30342 RVA: 0x002F30E0 File Offset: 0x002F12E0
	private void HandleTriggerCollisionExplosion(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (otherRigidbody && otherRigidbody.aiActor && Time.time - this.m_lastExplosionTime > 5f)
		{
			this.m_lastExplosionTime = Time.time;
			Exploder.Explode(base.specRigidbody.UnitCenter, this.TriggerExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
			this.Disappear();
		}
	}

	// Token: 0x06007687 RID: 30343 RVA: 0x002F3154 File Offset: 0x002F1354
	private void Disappear()
	{
		base.specRigidbody.enabled = false;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, false);
		base.sprite.renderer.enabled = false;
	}

	// Token: 0x06007688 RID: 30344 RVA: 0x002F3180 File Offset: 0x002F1380
	private void Reappear()
	{
		base.specRigidbody.enabled = true;
		base.sprite.renderer.enabled = true;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, true);
		base.specRigidbody.Reinitialize();
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
	}

	// Token: 0x06007689 RID: 30345 RVA: 0x002F31DC File Offset: 0x002F13DC
	public void DecoupleBabyDragun()
	{
		if (this.SourceItem)
		{
			this.SourceItem.DecoupleOrbital();
			this.m_owner.RemovePassiveItem(this.SourceItem.PickupObjectId);
		}
		this.m_owner.orbitals.Remove(this);
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600768A RID: 30346 RVA: 0x002F326C File Offset: 0x002F146C
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.projectile)
		{
			if (this.DamagesEnemiesOnShot && this.m_damageOnShotCooldown <= 0f)
			{
				if (this.m_owner)
				{
					base.StartCoroutine(this.FlashSprite(base.sprite, 1f));
					this.m_owner.CurrentRoom.ApplyActionToNearbyEnemies(this.m_owner.CenterPosition, 100f, delegate(AIActor enemy, float dist)
					{
						if (enemy && enemy.healthHaver)
						{
							enemy.healthHaver.ApplyDamage(this.DamageToEnemiesOnShot, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
						}
					});
				}
				this.m_damageOnShotCooldown = this.DamageToEnemiesOnShotCooldown;
			}
			if (this.TriggersMachoBraceOnShot && this.m_owner)
			{
				for (int i = 0; i < this.m_owner.passiveItems.Count; i++)
				{
					if (this.m_owner.passiveItems[i] is MachoBraceItem)
					{
						(this.m_owner.passiveItems[i] as MachoBraceItem).ForceTrigger(this.m_owner);
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600768B RID: 30347 RVA: 0x002F3388 File Offset: 0x002F1588
	private IEnumerator FlashSprite(tk2dBaseSprite targetSprite, float flashTime = 1f)
	{
		Color overrideColor = Color.white;
		overrideColor.a = 1f;
		if (targetSprite)
		{
			targetSprite.usesOverrideMaterial = true;
		}
		Color startColor = targetSprite.renderer.material.GetColor("_OverrideColor");
		Material targetMaterial = targetSprite.renderer.material;
		for (float elapsed = 0f; elapsed < flashTime; elapsed += BraveTime.DeltaTime)
		{
			float t = 1f - elapsed / flashTime;
			targetMaterial.SetColor("_OverrideColor", Color.Lerp(startColor, overrideColor, t));
			targetMaterial.SetFloat("_SaturationModifier", Mathf.Lerp(1f, 5f, t));
			yield return null;
		}
		targetSprite.renderer.material.SetColor("_OverrideColor", startColor);
		targetMaterial.SetFloat("_SaturationModifier", 1f);
		yield break;
	}

	// Token: 0x0600768C RID: 30348 RVA: 0x002F33AC File Offset: 0x002F15AC
	private void Update()
	{
		if (!this.m_initialized)
		{
			return;
		}
		if (this.ExplodesOnTriggerCollision && !base.specRigidbody.enabled && Time.time - this.m_lastExplosionTime > 5f)
		{
			this.Reappear();
		}
		this.HandleMotion();
		this.HandleCombat();
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < this.synergies.Length; i++)
		{
			PlayerOrbitalSynergyData playerOrbitalSynergyData = this.synergies[i];
			flag |= playerOrbitalSynergyData.HasOverrideAnimations;
			if (playerOrbitalSynergyData.HasOverrideAnimations && this.m_owner.HasActiveBonusSynergy(playerOrbitalSynergyData.SynergyToCheck, false))
			{
				flag2 = true;
				if (!base.spriteAnimator.IsPlaying(playerOrbitalSynergyData.OverrideIdleAnimation))
				{
					base.spriteAnimator.Play(playerOrbitalSynergyData.OverrideIdleAnimation);
				}
			}
		}
		if (flag && !flag2 && !string.IsNullOrEmpty(this.IdleAnimation) && !base.spriteAnimator.IsPlaying(this.IdleAnimation))
		{
			base.spriteAnimator.Play(this.IdleAnimation);
		}
		if (this.motionStyle != PlayerOrbital.OrbitalMotionStyle.ORBIT_TARGET)
		{
			this.m_retargetTimer -= BraveTime.DeltaTime;
		}
		if (this.shootProjectile && base.specRigidbody)
		{
			if (this.m_hasLuteBuff && (!this.m_owner || !this.m_owner.CurrentGun || !this.m_owner.CurrentGun.LuteCompanionBuffActive))
			{
				if (this.m_luteOverheadVfx)
				{
					UnityEngine.Object.Destroy(this.m_luteOverheadVfx);
					this.m_luteOverheadVfx = null;
				}
				if (base.specRigidbody)
				{
					SpeculativeRigidbody specRigidbody = base.specRigidbody;
					specRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Remove(specRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.UpdateVFXOnMovement));
				}
				this.m_hasLuteBuff = false;
			}
			else if (!this.m_hasLuteBuff && this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.LuteCompanionBuffActive)
			{
				GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Buff_Status");
				this.m_luteOverheadVfx = SpawnManager.SpawnVFX(gameObject, base.specRigidbody.UnitCenter.ToVector3ZisY(0f).Quantize(0.0625f) + new Vector3(0f, 1f, 0f), Quaternion.identity);
				if (base.specRigidbody)
				{
					SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
					specRigidbody2.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody2.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.UpdateVFXOnMovement));
				}
				this.m_hasLuteBuff = true;
			}
		}
		this.m_damageOnShotCooldown -= BraveTime.DeltaTime;
		this.m_shootTimer -= BraveTime.DeltaTime;
	}

	// Token: 0x0600768D RID: 30349 RVA: 0x002F36C0 File Offset: 0x002F18C0
	private void UpdateVFXOnMovement(SpeculativeRigidbody arg1, Vector2 arg2, IntVector2 arg3)
	{
		if (this.m_hasLuteBuff && this.m_luteOverheadVfx)
		{
			this.m_luteOverheadVfx.transform.position = base.specRigidbody.UnitCenter.ToVector3ZisY(0f).Quantize(0.0625f) + new Vector3(0f, 1f, 0f);
		}
	}

	// Token: 0x0600768E RID: 30350 RVA: 0x002F3730 File Offset: 0x002F1930
	protected override void OnDestroy()
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

	// Token: 0x0600768F RID: 30351 RVA: 0x002F37E0 File Offset: 0x002F19E0
	public void Reinitialize()
	{
		base.specRigidbody.Reinitialize();
		this.m_ownerCenterAverage = this.m_owner.CenterPosition;
	}

	// Token: 0x06007690 RID: 30352 RVA: 0x002F3800 File Offset: 0x002F1A00
	public void ReinitializeWithDelta(Vector2 delta)
	{
		base.specRigidbody.Reinitialize();
		this.m_ownerCenterAverage += delta;
	}

	// Token: 0x06007691 RID: 30353 RVA: 0x002F3820 File Offset: 0x002F1A20
	private void HandleMotion()
	{
		Vector2 vector = this.m_owner.CenterPosition;
		if (Vector2.Distance(vector, base.transform.position.XY()) > 20f)
		{
			base.transform.position = vector.ToVector3ZUp(0f);
			base.specRigidbody.Reinitialize();
		}
		if (this.motionStyle == PlayerOrbital.OrbitalMotionStyle.ORBIT_TARGET && this.m_currentTarget != null)
		{
			vector = this.m_currentTarget.CenterPosition;
		}
		Vector2 vector2 = vector - this.m_ownerCenterAverage;
		float num = Mathf.Lerp(0.1f, 15f, vector2.magnitude / 4f);
		float num2 = Mathf.Min(num * BraveTime.DeltaTime, vector2.magnitude);
		float num3 = 360f / (float)PlayerOrbital.GetNumberOfOrbitalsInTier(this.m_owner, this.GetOrbitalTier()) * (float)this.GetOrbitalTierIndex() + BraveTime.ScaledTimeSinceStartup * this.orbitDegreesPerSecond;
		Vector2 vector3 = this.m_ownerCenterAverage + (vector - this.m_ownerCenterAverage).normalized * num2;
		vector3 = Vector2.Lerp(vector3, vector, this.perfectOrbitalFactor);
		Vector2 vector4 = vector3 + (Quaternion.Euler(0f, 0f, num3) * Vector3.right * this.orbitRadius).XY();
		if (this.SpecialID == PlayerOrbital.SpecialOrbitalIdentifier.BABY_DRAGUN)
		{
			float num4 = Mathf.Sin(Time.time * this.SinWavelength) * this.SinAmplitude;
			vector4 += (Quaternion.Euler(0f, 0f, num3) * Vector3.right).XY().normalized * num4;
		}
		this.m_ownerCenterAverage = vector3;
		vector4 = vector4.Quantize(0.0625f);
		Vector2 vector5 = (vector4 - base.transform.position.XY()) / BraveTime.DeltaTime;
		base.specRigidbody.Velocity = vector5;
		this.m_currentAngle = num3 % 360f;
		if (this.shouldRotate)
		{
			base.transform.localRotation = Quaternion.Euler(0f, 0f, this.m_currentAngle);
		}
	}

	// Token: 0x06007692 RID: 30354 RVA: 0x002F3A5C File Offset: 0x002F1C5C
	private void AcquireTarget()
	{
		this.m_retargetTimer = 0.25f;
		this.m_currentTarget = null;
		if (this.m_owner == null || this.m_owner.CurrentRoom == null)
		{
			return;
		}
		List<AIActor> activeEnemies = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null && activeEnemies.Count > 0)
		{
			AIActor aiactor = null;
			float num = -1f;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor2 = activeEnemies[i];
				if (aiactor2 && aiactor2.HasBeenEngaged && aiactor2.IsWorthShootingAt)
				{
					float num2 = Vector2.Distance(base.transform.position.XY(), aiactor2.specRigidbody.UnitCenter);
					if (aiactor == null || num2 < num)
					{
						aiactor = aiactor2;
						num = num2;
					}
				}
			}
			if (aiactor)
			{
				this.m_currentTarget = aiactor;
			}
		}
	}

	// Token: 0x06007693 RID: 30355 RVA: 0x002F3B64 File Offset: 0x002F1D64
	private Projectile GetProjectile()
	{
		Projectile overrideProjectile = this.shootProjectile;
		for (int i = 0; i < this.synergies.Length; i++)
		{
			PlayerOrbitalSynergyData playerOrbitalSynergyData = this.synergies[i];
			if (playerOrbitalSynergyData.OverrideProjectile && this.m_owner.HasActiveBonusSynergy(playerOrbitalSynergyData.SynergyToCheck, false))
			{
				overrideProjectile = playerOrbitalSynergyData.OverrideProjectile;
			}
		}
		return overrideProjectile;
	}

	// Token: 0x06007694 RID: 30356 RVA: 0x002F3BD8 File Offset: 0x002F1DD8
	private Vector2 FindPredictedTargetPosition()
	{
		float num = this.GetProjectile().baseData.speed;
		if (num < 0f)
		{
			num = float.MaxValue;
		}
		Vector2 vector = base.transform.position.XY();
		Vector2 vector2 = ((this.m_currentTarget.specRigidbody.HitboxPixelCollider == null) ? this.m_currentTarget.specRigidbody.UnitCenter : this.m_currentTarget.specRigidbody.HitboxPixelCollider.UnitCenter);
		float num2 = Vector2.Distance(vector, vector2) / num;
		return vector2 + this.m_currentTarget.specRigidbody.Velocity * num2;
	}

	// Token: 0x06007695 RID: 30357 RVA: 0x002F3C84 File Offset: 0x002F1E84
	private void Shoot(Vector2 targetPosition, Vector2 startOffset)
	{
		Vector2 vector = base.transform.position.XY() + startOffset;
		Vector2 vector2 = targetPosition - vector;
		float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
		GameObject gameObject = this.GetProjectile().gameObject;
		GameObject gameObject2 = SpawnManager.SpawnProjectile(gameObject, vector, Quaternion.Euler(0f, 0f, num), true);
		Projectile component = gameObject2.GetComponent<Projectile>();
		component.collidesWithEnemies = true;
		component.collidesWithPlayer = false;
		component.Owner = this.m_owner;
		component.Shooter = this.m_owner.specRigidbody;
		component.TreatedAsNonProjectileForChallenge = true;
		if (this.m_owner)
		{
			if (PassiveItem.IsFlagSetForCharacter(this.m_owner, typeof(BattleStandardItem)))
			{
				component.baseData.damage *= BattleStandardItem.BattleStandardCompanionDamageMultiplier;
			}
			if (this.m_owner.CurrentGun && this.m_owner.CurrentGun.LuteCompanionBuffActive)
			{
				component.baseData.damage *= 2f;
				component.RuntimeUpdateScale(1.75f);
			}
			this.m_owner.DoPostProcessProjectile(component);
		}
	}

	// Token: 0x06007696 RID: 30358 RVA: 0x002F3DD4 File Offset: 0x002F1FD4
	public void ToggleRenderer(bool value)
	{
		base.sprite.renderer.enabled = value;
		if (!this.PreventOutline)
		{
			SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, value);
		}
	}

	// Token: 0x06007697 RID: 30359 RVA: 0x002F3E00 File Offset: 0x002F2000
	private int GetNumberToFire()
	{
		int num = this.numToShoot;
		if (this.synergies != null && this.m_owner)
		{
			for (int i = 0; i < this.synergies.Length; i++)
			{
				if (this.m_owner.HasActiveBonusSynergy(this.synergies[i].SynergyToCheck, false))
				{
					num += this.synergies[i].AdditionalShots;
				}
			}
		}
		return num;
	}

	// Token: 0x06007698 RID: 30360 RVA: 0x002F3E80 File Offset: 0x002F2080
	private float GetModifiedCooldown()
	{
		float num = this.shootCooldown;
		if (this.synergies != null && this.m_owner)
		{
			for (int i = 0; i < this.synergies.Length; i++)
			{
				if (this.m_owner.HasActiveBonusSynergy(this.synergies[i].SynergyToCheck, false))
				{
					num *= this.synergies[i].ShootCooldownMultiplier;
				}
			}
		}
		if (this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.LuteCompanionBuffActive)
		{
			num /= 1.5f;
		}
		return num;
	}

	// Token: 0x06007699 RID: 30361 RVA: 0x002F3F44 File Offset: 0x002F2144
	private void HandleCombat()
	{
		if (GameManager.Instance.IsPaused || !this.m_owner || this.m_owner.CurrentInputState != PlayerInputState.AllInput || this.m_owner.IsInputOverridden)
		{
			return;
		}
		if (this.shootProjectile == null)
		{
			return;
		}
		if (this.m_retargetTimer <= 0f)
		{
			this.m_currentTarget = null;
		}
		if (this.m_currentTarget == null || !this.m_currentTarget || this.m_currentTarget.healthHaver.IsDead)
		{
			this.AcquireTarget();
		}
		if (this.m_currentTarget == null || !this.m_currentTarget)
		{
			return;
		}
		if (this.m_shootTimer <= 0f)
		{
			this.m_shootTimer = this.GetModifiedCooldown();
			Vector2 vector = this.FindPredictedTargetPosition();
			if (!this.m_owner.IsStealthed)
			{
				int numberToFire = this.GetNumberToFire();
				for (int i = 0; i < numberToFire; i++)
				{
					Vector2 vector2 = Vector2.zero;
					if (i > 0)
					{
						vector2 = UnityEngine.Random.insideUnitCircle.normalized;
					}
					this.Shoot(vector + vector2, vector2);
				}
			}
		}
		if (this.shouldRotate)
		{
			float num = BraveMathCollege.Atan2Degrees(this.m_currentTarget.CenterPosition - base.transform.position.XY());
			base.transform.localRotation = Quaternion.Euler(0f, 0f, num - 90f);
		}
	}

	// Token: 0x0600769A RID: 30362 RVA: 0x002F40E4 File Offset: 0x002F22E4
	public Transform GetTransform()
	{
		return base.transform;
	}

	// Token: 0x0600769B RID: 30363 RVA: 0x002F40EC File Offset: 0x002F22EC
	public int GetOrbitalTier()
	{
		return this.m_orbitalTier;
	}

	// Token: 0x0600769C RID: 30364 RVA: 0x002F40F4 File Offset: 0x002F22F4
	public void SetOrbitalTier(int tier)
	{
		this.m_orbitalTier = tier;
	}

	// Token: 0x0600769D RID: 30365 RVA: 0x002F4100 File Offset: 0x002F2300
	public int GetOrbitalTierIndex()
	{
		return this.m_orbitalTierIndex;
	}

	// Token: 0x0600769E RID: 30366 RVA: 0x002F4108 File Offset: 0x002F2308
	public void SetOrbitalTierIndex(int tierIndex)
	{
		this.m_orbitalTierIndex = tierIndex;
	}

	// Token: 0x0600769F RID: 30367 RVA: 0x002F4114 File Offset: 0x002F2314
	public float GetOrbitalRadius()
	{
		return this.orbitRadius;
	}

	// Token: 0x060076A0 RID: 30368 RVA: 0x002F411C File Offset: 0x002F231C
	public float GetOrbitalRotationalSpeed()
	{
		return this.orbitDegreesPerSecond;
	}

	// Token: 0x04007862 RID: 30818
	public PlayerOrbital.SpecialOrbitalIdentifier SpecialID;

	// Token: 0x04007863 RID: 30819
	public PlayerOrbital.OrbitalMotionStyle motionStyle;

	// Token: 0x04007864 RID: 30820
	public Projectile shootProjectile;

	// Token: 0x04007865 RID: 30821
	public int numToShoot = 1;

	// Token: 0x04007866 RID: 30822
	public float shootCooldown = 1f;

	// Token: 0x04007867 RID: 30823
	public float orbitRadius = 3f;

	// Token: 0x04007868 RID: 30824
	public float orbitDegreesPerSecond = 90f;

	// Token: 0x04007869 RID: 30825
	public bool shouldRotate = true;

	// Token: 0x0400786A RID: 30826
	public float perfectOrbitalFactor;

	// Token: 0x0400786B RID: 30827
	public bool DamagesEnemiesOnShot;

	// Token: 0x0400786C RID: 30828
	public float DamageToEnemiesOnShot = 10f;

	// Token: 0x0400786D RID: 30829
	public float DamageToEnemiesOnShotCooldown = 3f;

	// Token: 0x0400786E RID: 30830
	private float m_damageOnShotCooldown;

	// Token: 0x0400786F RID: 30831
	public bool TriggersMachoBraceOnShot;

	// Token: 0x04007870 RID: 30832
	public bool PreventOutline;

	// Token: 0x04007871 RID: 30833
	public string IdleAnimation;

	// Token: 0x04007872 RID: 30834
	[Header("Synergies")]
	public PlayerOrbitalSynergyData[] synergies;

	// Token: 0x04007873 RID: 30835
	public bool ExplodesOnTriggerCollision;

	// Token: 0x04007874 RID: 30836
	public ExplosionData TriggerExplosionData;

	// Token: 0x04007875 RID: 30837
	private bool m_initialized;

	// Token: 0x04007876 RID: 30838
	private PlayerController m_owner;

	// Token: 0x04007877 RID: 30839
	private AIActor m_currentTarget;

	// Token: 0x04007878 RID: 30840
	private float m_currentAngle;

	// Token: 0x04007879 RID: 30841
	private float m_shootTimer;

	// Token: 0x0400787A RID: 30842
	private float m_retargetTimer;

	// Token: 0x0400787B RID: 30843
	private int m_orbitalTier;

	// Token: 0x0400787C RID: 30844
	private int m_orbitalTierIndex;

	// Token: 0x0400787D RID: 30845
	private Vector2 m_ownerCenterAverage;

	// Token: 0x0400787E RID: 30846
	private bool m_hasLuteBuff;

	// Token: 0x0400787F RID: 30847
	private GameObject m_luteOverheadVfx;

	// Token: 0x04007880 RID: 30848
	[NonSerialized]
	public PlayerOrbitalItem SourceItem;

	// Token: 0x04007881 RID: 30849
	private float m_lastExplosionTime;

	// Token: 0x04007882 RID: 30850
	public float SinWavelength = 3f;

	// Token: 0x04007883 RID: 30851
	public float SinAmplitude = 1f;

	// Token: 0x02001462 RID: 5218
	public enum SpecialOrbitalIdentifier
	{
		// Token: 0x04007885 RID: 30853
		NONE,
		// Token: 0x04007886 RID: 30854
		BABY_DRAGUN
	}

	// Token: 0x02001463 RID: 5219
	public enum OrbitalMotionStyle
	{
		// Token: 0x04007888 RID: 30856
		ORBIT_PLAYER_ALWAYS,
		// Token: 0x04007889 RID: 30857
		ORBIT_TARGET
	}
}
