using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001465 RID: 5221
public class PlayerOrbitalFollower : BraveBehaviour
{
	// Token: 0x060076A9 RID: 30377 RVA: 0x002F436C File Offset: 0x002F256C
	public void Initialize(PlayerController owner)
	{
		this.m_initialized = true;
		this.m_owner = owner;
		this.m_orbitalIndex = owner.trailOrbitals.Count;
		owner.trailOrbitals.Add(this);
		base.sprite = base.GetComponentInChildren<tk2dSprite>();
		base.spriteAnimator = base.GetComponentInChildren<tk2dSpriteAnimator>();
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
	}

	// Token: 0x060076AA RID: 30378 RVA: 0x002F43CC File Offset: 0x002F25CC
	private void DoMicroBlank()
	{
		if (this.BlankVFXPrefab == null)
		{
			this.BlankVFXPrefab = (GameObject)BraveResources.Load("Global VFX/BlankVFX_Ghost", ".prefab");
		}
		AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
		GameObject gameObject = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
		float num = 0.25f;
		if (base.sprite && base.sprite.spriteAnimator && base.sprite.spriteAnimator.GetClipByName(this.BlankAnimationName) != null)
		{
			base.sprite.spriteAnimator.PlayForDuration(this.BlankAnimationName, -1f, this.BlankIdleName, false);
		}
		silencerInstance.TriggerSilencer(base.specRigidbody.UnitCenter, 20f, this.BlankRadius, this.BlankVFXPrefab, 0f, 3f, 3f, 3f, 30f, 3f, num, this.m_owner, false, false);
	}

	// Token: 0x060076AB RID: 30379 RVA: 0x002F44D8 File Offset: 0x002F26D8
	public void ToggleRenderer(bool value)
	{
		base.sprite.renderer.enabled = value;
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, value);
	}

	// Token: 0x060076AC RID: 30380 RVA: 0x002F44F8 File Offset: 0x002F26F8
	private void Update()
	{
		if (!this.m_initialized)
		{
			return;
		}
		this.HandleMotion();
		this.HandleCombat();
		bool flag = false;
		bool flag2 = false;
		if (this.synergies != null && this.m_owner && base.spriteAnimator)
		{
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
		}
		if (flag && !flag2 && !string.IsNullOrEmpty(this.IdleAnimation) && base.spriteAnimator && !base.spriteAnimator.IsPlaying(this.IdleAnimation))
		{
			base.spriteAnimator.Play(this.IdleAnimation);
		}
		if (this.BlanksOnProjectileRadius)
		{
			this.m_blankCooldown -= BraveTime.DeltaTime;
			if (this.m_blankCooldown <= 0f)
			{
				this.HandleBlanks();
				this.m_blankCooldown = this.BlankFrequency;
			}
		}
		if (this.shouldRotate)
		{
			float num = this.m_targetAngle + this.rotationOffset;
			float z = base.transform.rotation.eulerAngles.z;
			num = Mathf.MoveTowardsAngle(z, num, this.maxRotationDegreesPerSecond * BraveTime.DeltaTime);
			if (float.IsNaN(num) || float.IsInfinity(num))
			{
				num = 0f;
			}
			base.transform.rotation = Quaternion.Euler(0f, 0f, num);
		}
		this.m_retargetTimer -= BraveTime.DeltaTime;
		this.m_shootTimer -= BraveTime.DeltaTime;
		if (this.PredictsChests)
		{
			Chest chest = null;
			float num2 = float.MaxValue;
			for (int j = 0; j < StaticReferenceManager.AllChests.Count; j++)
			{
				Chest chest2 = StaticReferenceManager.AllChests[j];
				if (chest2 && chest2.sprite && !chest2.IsOpen && !chest2.IsBroken && !chest2.IsSealed)
				{
					float num3 = Vector2.Distance(this.m_owner.CenterPosition, chest2.sprite.WorldCenter);
					if (num3 < num2)
					{
						chest = chest2;
						num2 = num3;
					}
				}
			}
			if (num2 > 5f)
			{
				chest = null;
			}
			if (this.m_lastPredictedChest != chest)
			{
				if (this.m_lastPredictedChest)
				{
					base.GetComponent<HologramDoer>().HideSprite(base.gameObject, false);
				}
				if (chest)
				{
					List<PickupObject> list = chest.PredictContents(this.m_owner);
					if (list.Count > 0 && list[0].encounterTrackable)
					{
						tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.ForceInstance.EncounterIconCollection;
						base.GetComponent<HologramDoer>().ChangeToSprite(base.gameObject, encounterIconCollection, encounterIconCollection.GetSpriteIdByName(list[0].encounterTrackable.journalData.AmmonomiconSprite));
					}
				}
				this.m_lastPredictedChest = chest;
			}
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
	}

	// Token: 0x060076AD RID: 30381 RVA: 0x002F4A38 File Offset: 0x002F2C38
	private void UpdateVFXOnMovement(SpeculativeRigidbody arg1, Vector2 arg2, IntVector2 arg3)
	{
		if (this.m_hasLuteBuff && this.m_luteOverheadVfx)
		{
			this.m_luteOverheadVfx.transform.position = base.specRigidbody.UnitCenter.ToVector3ZisY(0f).Quantize(0.0625f) + new Vector3(0f, 1f, 0f);
		}
	}

	// Token: 0x060076AE RID: 30382 RVA: 0x002F4AA8 File Offset: 0x002F2CA8
	protected override void OnDestroy()
	{
		for (int i = 0; i < this.m_owner.trailOrbitals.Count; i++)
		{
			if (this.m_owner.trailOrbitals[i].m_orbitalIndex > this.m_orbitalIndex)
			{
				this.m_owner.trailOrbitals[i].m_orbitalIndex--;
			}
		}
		this.m_owner.trailOrbitals.Remove(this);
	}

	// Token: 0x060076AF RID: 30383 RVA: 0x002F4B28 File Offset: 0x002F2D28
	private void HandleBlanks()
	{
		Vector2 unitCenter = base.specRigidbody.UnitCenter;
		float num = this.BlankRadius * this.BlankRadius;
		for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.AllProjectiles[i];
			if (projectile && projectile.collidesWithPlayer && projectile.specRigidbody)
			{
				if (!this.m_owner || !(projectile.Owner is PlayerController))
				{
					if ((unitCenter - projectile.specRigidbody.UnitCenter).sqrMagnitude < num)
					{
						this.DoMicroBlank();
						break;
					}
				}
			}
		}
	}

	// Token: 0x060076B0 RID: 30384 RVA: 0x002F4BF4 File Offset: 0x002F2DF4
	private void HandleMotion()
	{
		Vector2 vector = this.m_owner.CenterPosition;
		if (this.m_orbitalIndex > 0)
		{
			vector = this.m_owner.trailOrbitals[this.m_orbitalIndex - 1].specRigidbody.UnitCenter;
		}
		Vector2 vector2 = vector - this.m_lastOwnerCenter;
		if (vector2.sqrMagnitude > 0f)
		{
			this.m_lastTargetMotionVector = vector2.normalized;
		}
		Vector2 vector3 = vector + -1f * this.m_owner.trailOrbitals[0].m_lastTargetMotionVector * 1.25f;
		vector3 = vector3.Quantize(0.0625f);
		if (this.OverridePosition)
		{
			vector3 = this.OverrideTargetPosition.XY();
		}
		float magnitude = (vector3 - base.transform.position.XY()).magnitude;
		float num = Mathf.Lerp(0.1f, 15f, magnitude / 4f) * BraveTime.DeltaTime;
		if (this.OverridePosition)
		{
			num = 15f * BraveTime.DeltaTime;
		}
		float num2 = Mathf.Min(num, magnitude);
		Vector2 vector4 = (vector3 - base.transform.position.XY()).normalized * num2 / BraveTime.DeltaTime;
		base.specRigidbody.Velocity = vector4;
		if (this.shouldRotate)
		{
			this.m_targetAngle = BraveMathCollege.Atan2Degrees((vector - base.transform.position.XY()).normalized);
		}
		this.m_lastOwnerCenter = vector;
	}

	// Token: 0x060076B1 RID: 30385 RVA: 0x002F4D98 File Offset: 0x002F2F98
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

	// Token: 0x060076B2 RID: 30386 RVA: 0x002F4EA0 File Offset: 0x002F30A0
	private Vector2 FindPredictedTargetPosition()
	{
		float num = this.shootProjectile.baseData.speed;
		if (num < 0f)
		{
			num = float.MaxValue;
		}
		Vector2 vector = base.transform.position.XY();
		Vector2 unitCenter = this.m_currentTarget.specRigidbody.HitboxPixelCollider.UnitCenter;
		float num2 = Vector2.Distance(vector, unitCenter) / num;
		return unitCenter + this.m_currentTarget.specRigidbody.Velocity * num2;
	}

	// Token: 0x060076B3 RID: 30387 RVA: 0x002F4F20 File Offset: 0x002F3120
	private void Shoot(Vector2 targetPosition)
	{
		Vector2 vector = targetPosition - base.transform.position.XY();
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		GameObject gameObject = this.shootProjectile.gameObject;
		GameObject gameObject2 = SpawnManager.SpawnProjectile(gameObject, base.transform.position.XY(), Quaternion.Euler(0f, 0f, num), true);
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

	// Token: 0x060076B4 RID: 30388 RVA: 0x002F5074 File Offset: 0x002F3274
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

	// Token: 0x060076B5 RID: 30389 RVA: 0x002F5138 File Offset: 0x002F3338
	private void HandleCombat()
	{
		if (GameManager.Instance.IsPaused || !this.m_owner || this.m_owner.CurrentInputState != PlayerInputState.AllInput || this.m_owner.IsInputOverridden)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.synergies.Length; i++)
		{
			if (this.m_owner && this.m_owner.HasActiveBonusSynergy(this.synergies[i].SynergyToCheck, false) && this.synergies[i].EngagesFiring && this.synergies[i].OverrideProjectile)
			{
				flag = true;
				break;
			}
		}
		if (this.shootProjectile == null || flag)
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
		if (this.m_currentTarget == null)
		{
			return;
		}
		if (this.shouldRotate)
		{
			this.m_targetAngle = BraveMathCollege.Atan2Degrees(this.m_currentTarget.CenterPosition - base.transform.position.XY());
		}
		if (this.m_shootTimer <= 0f)
		{
			this.m_shootTimer = this.GetModifiedCooldown();
			Vector2 vector = this.FindPredictedTargetPosition();
			if (!this.m_owner.IsStealthed)
			{
				this.Shoot(vector);
			}
		}
	}

	// Token: 0x04007894 RID: 30868
	public Projectile shootProjectile;

	// Token: 0x04007895 RID: 30869
	public float shootCooldown = 1f;

	// Token: 0x04007896 RID: 30870
	public bool shouldRotate;

	// Token: 0x04007897 RID: 30871
	public float rotationOffset;

	// Token: 0x04007898 RID: 30872
	public float maxRotationDegreesPerSecond = 360f;

	// Token: 0x04007899 RID: 30873
	public bool BlanksOnProjectileRadius;

	// Token: 0x0400789A RID: 30874
	public float BlankRadius = 4f;

	// Token: 0x0400789B RID: 30875
	public float BlankFrequency = 3f;

	// Token: 0x0400789C RID: 30876
	[CheckAnimation(null)]
	public string BlankAnimationName;

	// Token: 0x0400789D RID: 30877
	[CheckAnimation(null)]
	public string BlankIdleName;

	// Token: 0x0400789E RID: 30878
	public string IdleAnimation;

	// Token: 0x0400789F RID: 30879
	[Header("Synergies")]
	public PlayerOrbitalSynergyData[] synergies;

	// Token: 0x040078A0 RID: 30880
	[NonSerialized]
	public bool OverridePosition;

	// Token: 0x040078A1 RID: 30881
	[NonSerialized]
	public Vector3 OverrideTargetPosition = Vector3.zero;

	// Token: 0x040078A2 RID: 30882
	public bool PredictsChests;

	// Token: 0x040078A3 RID: 30883
	private bool m_initialized;

	// Token: 0x040078A4 RID: 30884
	private PlayerController m_owner;

	// Token: 0x040078A5 RID: 30885
	private AIActor m_currentTarget;

	// Token: 0x040078A6 RID: 30886
	private float m_shootTimer;

	// Token: 0x040078A7 RID: 30887
	private float m_retargetTimer;

	// Token: 0x040078A8 RID: 30888
	private int m_orbitalIndex;

	// Token: 0x040078A9 RID: 30889
	private float m_targetAngle;

	// Token: 0x040078AA RID: 30890
	private float m_blankCooldown;

	// Token: 0x040078AB RID: 30891
	private bool m_hasLuteBuff;

	// Token: 0x040078AC RID: 30892
	private GameObject m_luteOverheadVfx;

	// Token: 0x040078AD RID: 30893
	private GameObject BlankVFXPrefab;

	// Token: 0x040078AE RID: 30894
	private Chest m_lastPredictedChest;

	// Token: 0x040078AF RID: 30895
	private Vector2 m_lastTargetMotionVector;

	// Token: 0x040078B0 RID: 30896
	private Vector2 m_lastOwnerCenter;

	// Token: 0x040078B1 RID: 30897
	private const float DIST_BETWEEN_AT_REST = 1.25f;
}
