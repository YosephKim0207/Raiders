using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001703 RID: 5891
public class MultiTemporaryOrbitalLayer
{
	// Token: 0x060088EE RID: 35054 RVA: 0x0038CBF8 File Offset: 0x0038ADF8
	public void Initialize(PlayerController player, GameObject orbitalPrefab)
	{
		this.m_player = player;
		this.m_orbitalPrefab = orbitalPrefab;
		this.m_orbitals = new List<SpeculativeRigidbody>();
		for (int i = 0; i < this.targetNumberOrbitals; i++)
		{
			Vector3 vector = player.LockedApproximateSpriteCenter + Quaternion.Euler(0f, 0f, 360f / (float)this.targetNumberOrbitals * (float)i) * Vector3.right * this.circleRadius;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orbitalPrefab, vector, Quaternion.identity);
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			component.HeightOffGround = 1.5f;
			tk2dSpriteAnimator component2 = gameObject.GetComponent<tk2dSpriteAnimator>();
			component2.PlayFromFrame(UnityEngine.Random.Range(0, component2.DefaultClip.frames.Length));
			SpeculativeRigidbody component3 = gameObject.GetComponent<SpeculativeRigidbody>();
			SpeculativeRigidbody speculativeRigidbody = component3;
			speculativeRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			SpeculativeRigidbody speculativeRigidbody2 = component3;
			speculativeRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleCollision));
			SpeculativeRigidbody speculativeRigidbody3 = component3;
			speculativeRigidbody3.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(speculativeRigidbody3.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.HandleTileCollision));
			this.m_orbitals.Add(component3);
		}
	}

	// Token: 0x060088EF RID: 35055 RVA: 0x0038CD38 File Offset: 0x0038AF38
	public void Disconnect()
	{
		if (this.m_orbitals == null || this.m_orbitals.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.m_orbitals.Count; i++)
		{
			if (this.m_orbitals[i])
			{
				this.m_orbitals[i].CollideWithTileMap = true;
			}
		}
	}

	// Token: 0x060088F0 RID: 35056 RVA: 0x0038CDA8 File Offset: 0x0038AFA8
	private void HandleTileCollision(CollisionData tileCollision)
	{
		this.DestroyKnife(tileCollision.MyRigidbody);
	}

	// Token: 0x060088F1 RID: 35057 RVA: 0x0038CDB8 File Offset: 0x0038AFB8
	protected Vector3 GetTargetPositionForKniveID(Vector3 center, int i, float radiusToUse)
	{
		float num = this.rotationDegreesPerSecond * this.m_elapsed % 360f;
		return center + Quaternion.Euler(0f, 0f, num + 360f / (float)this.m_orbitals.Count * (float)i) * Vector3.right * radiusToUse;
	}

	// Token: 0x060088F2 RID: 35058 RVA: 0x0038CE18 File Offset: 0x0038B018
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
	{
		Projectile component = other.GetComponent<Projectile>();
		if (component != null && component.Owner is PlayerController)
		{
			PhysicsEngine.SkipCollision = true;
		}
		GameActor component2 = other.GetComponent<GameActor>();
		if (component2 is PlayerController)
		{
			PhysicsEngine.SkipCollision = true;
		}
		if (component2 is AIActor && !(component2 as AIActor).IsNormalEnemy)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x060088F3 RID: 35059 RVA: 0x0038CE88 File Offset: 0x0038B088
	private void HandleCollision(SpeculativeRigidbody other, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (other.GetComponent<AIActor>() != null)
		{
			HealthHaver component = other.GetComponent<HealthHaver>();
			component.ApplyDamage(this.collisionDamage, Vector2.zero, "Orbital Shield", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			this.DestroyKnife(source);
		}
		else if (other.GetComponent<Projectile>() != null)
		{
			Projectile component2 = other.GetComponent<Projectile>();
			if (component2.Owner is PlayerController)
			{
				return;
			}
			component2.DieInAir(false, true, true, false);
			this.DestroyKnife(source);
		}
	}

	// Token: 0x060088F4 RID: 35060 RVA: 0x0038CF10 File Offset: 0x0038B110
	private void DestroyKnife(SpeculativeRigidbody source)
	{
		int num = this.m_orbitals.IndexOf(source);
		if (num != -1)
		{
			this.m_orbitals.RemoveAt(num);
		}
		source.sprite.PlayEffectOnSprite(this.deathVFX, Vector3.zero, false);
		this.targetNumberOrbitals--;
		UnityEngine.Object.Destroy(source.gameObject);
	}

	// Token: 0x060088F5 RID: 35061 RVA: 0x0038CF70 File Offset: 0x0038B170
	public void Update()
	{
		if (GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating)
		{
			return;
		}
		this.m_elapsed += BraveTime.DeltaTime;
		Vector3 vector = this.m_currentShieldVelocity * BraveTime.DeltaTime;
		this.m_currentShieldCenterOffset += vector;
		this.m_cachedOffsetBase = this.m_player.LockedApproximateSpriteCenter;
		Vector3 vector2 = this.m_cachedOffsetBase + this.m_currentShieldCenterOffset;
		float num = this.circleRadius;
		while (this.m_orbitals.Count < this.targetNumberOrbitals)
		{
			this.AddOrbital();
		}
		for (int i = 0; i < this.m_orbitals.Count; i++)
		{
			if (this.m_orbitals[i] != null && this.m_orbitals[i])
			{
				Vector3 targetPositionForKniveID = this.GetTargetPositionForKniveID(vector2, i, num);
				Vector3 vector3 = targetPositionForKniveID - this.m_orbitals[i].transform.position;
				Vector2 vector4 = vector3.XY() / BraveTime.DeltaTime;
				this.m_orbitals[i].Velocity = vector4;
				this.m_orbitals[i].sprite.UpdateZDepth();
			}
		}
	}

	// Token: 0x060088F6 RID: 35062 RVA: 0x0038D0C8 File Offset: 0x0038B2C8
	private void AddOrbital()
	{
		Vector3 vector = this.m_player.LockedApproximateSpriteCenter + Quaternion.Euler(0f, 0f, 360f / (float)this.targetNumberOrbitals * (float)(this.m_orbitals.Count - 1)) * Vector3.right * this.circleRadius;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_orbitalPrefab, vector, Quaternion.identity);
		tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
		component.HeightOffGround = 1.5f;
		tk2dSpriteAnimator component2 = gameObject.GetComponent<tk2dSpriteAnimator>();
		component2.PlayFromFrame(UnityEngine.Random.Range(0, component2.DefaultClip.frames.Length));
		SpeculativeRigidbody component3 = gameObject.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody speculativeRigidbody = component3;
		speculativeRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
		SpeculativeRigidbody speculativeRigidbody2 = component3;
		speculativeRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleCollision));
		SpeculativeRigidbody speculativeRigidbody3 = component3;
		speculativeRigidbody3.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(speculativeRigidbody3.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.HandleTileCollision));
		this.m_orbitals.Add(component3);
	}

	// Token: 0x04008EAF RID: 36527
	public int targetNumberOrbitals;

	// Token: 0x04008EB0 RID: 36528
	public float collisionDamage;

	// Token: 0x04008EB1 RID: 36529
	public float circleRadius = 3f;

	// Token: 0x04008EB2 RID: 36530
	public float rotationDegreesPerSecond = 360f;

	// Token: 0x04008EB3 RID: 36531
	public GameObject deathVFX;

	// Token: 0x04008EB4 RID: 36532
	protected GameObject m_orbitalPrefab;

	// Token: 0x04008EB5 RID: 36533
	protected PlayerController m_player;

	// Token: 0x04008EB6 RID: 36534
	protected List<SpeculativeRigidbody> m_orbitals;

	// Token: 0x04008EB7 RID: 36535
	protected float m_elapsed;

	// Token: 0x04008EB8 RID: 36536
	protected float m_traversedDistance;

	// Token: 0x04008EB9 RID: 36537
	protected Vector3 m_currentShieldVelocity = Vector3.zero;

	// Token: 0x04008EBA RID: 36538
	protected Vector3 m_currentShieldCenterOffset = Vector3.zero;

	// Token: 0x04008EBB RID: 36539
	private Vector3 m_cachedOffsetBase;
}
