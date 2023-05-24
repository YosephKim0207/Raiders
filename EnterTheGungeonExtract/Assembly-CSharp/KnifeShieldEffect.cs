using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200142E RID: 5166
public class KnifeShieldEffect : BraveBehaviour
{
	// Token: 0x1700118E RID: 4494
	// (get) Token: 0x0600753F RID: 30015 RVA: 0x002EAD28 File Offset: 0x002E8F28
	public bool IsActive
	{
		get
		{
			if (this.m_currentShieldVelocity != Vector3.zero)
			{
				return false;
			}
			for (int i = 0; i < this.m_knives.Length; i++)
			{
				if (this.m_knives[i] != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06007540 RID: 30016 RVA: 0x002EAD7C File Offset: 0x002E8F7C
	public void Initialize(PlayerController player, GameObject knifePrefab)
	{
		this.m_player = player;
		this.m_knives = new SpeculativeRigidbody[this.numKnives];
		this.m_lightknife = player.HasActiveBonusSynergy(CustomSynergyType.LIGHTKNIVES, false);
		for (int i = 0; i < this.numKnives; i++)
		{
			Vector3 vector = player.LockedApproximateSpriteCenter + Quaternion.Euler(0f, 0f, 360f / (float)this.numKnives * (float)i) * Vector3.right * this.circleRadius;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(knifePrefab, vector, Quaternion.identity);
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			component.HeightOffGround = 1.5f;
			tk2dSpriteAnimator component2 = gameObject.GetComponent<tk2dSpriteAnimator>();
			component2.PlayFromFrame(UnityEngine.Random.Range(0, component2.DefaultClip.frames.Length));
			if (this.m_lightknife)
			{
				this.SetOverrideShader(component);
			}
			SpeculativeRigidbody component3 = gameObject.GetComponent<SpeculativeRigidbody>();
			SpeculativeRigidbody speculativeRigidbody = component3;
			speculativeRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			SpeculativeRigidbody speculativeRigidbody2 = component3;
			speculativeRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleCollision));
			SpeculativeRigidbody speculativeRigidbody3 = component3;
			speculativeRigidbody3.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(speculativeRigidbody3.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.HandleTileCollision));
			this.m_knives[i] = component3;
		}
	}

	// Token: 0x06007541 RID: 30017 RVA: 0x002EAEDC File Offset: 0x002E90DC
	private void SetOverrideShader(tk2dSprite spr)
	{
		spr.usesOverrideMaterial = true;
		Material material = spr.GetComponent<MeshRenderer>().material;
		if (this.m_lightknivesShader == null)
		{
			this.m_lightknivesShader = Shader.Find("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
		}
		material.shader = this.m_lightknivesShader;
		material.SetColor("_OverrideColor", new Color(0.39215687f, 0.8235294f, 0.47058824f));
		material.SetFloat("_EmissivePower", 130f);
	}

	// Token: 0x06007542 RID: 30018 RVA: 0x002EAF58 File Offset: 0x002E9158
	private void HandleTileCollision(CollisionData tileCollision)
	{
		int num = Array.IndexOf<SpeculativeRigidbody>(this.m_knives, tileCollision.MyRigidbody);
		if (num != -1)
		{
			this.m_knives[num] = null;
		}
		tileCollision.MyRigidbody.sprite.PlayEffectOnSprite(this.deathVFX, Vector3.zero, false);
		UnityEngine.Object.Destroy(tileCollision.MyRigidbody.gameObject);
	}

	// Token: 0x06007543 RID: 30019 RVA: 0x002EAFB4 File Offset: 0x002E91B4
	public void ThrowShield()
	{
		AkSoundEngine.PostEvent("Play_OBJ_daggershield_shot_01", base.gameObject);
		if (this.m_currentShieldVelocity == Vector3.zero)
		{
			Vector3 vector = this.m_player.unadjustedAimPoint - this.m_player.CenterPosition;
			this.m_currentShieldVelocity = vector.WithZ(0f).normalized * this.throwSpeed;
			for (int i = 0; i < this.m_knives.Length; i++)
			{
				if (this.m_knives[i] != null && this.m_knives[i])
				{
					this.m_knives[i].specRigidbody.CollideWithTileMap = true;
				}
			}
		}
	}

	// Token: 0x06007544 RID: 30020 RVA: 0x002EB080 File Offset: 0x002E9280
	protected Vector3 GetTargetPositionForKniveID(Vector3 center, int i, float radiusToUse)
	{
		float num = this.rotationDegreesPerSecond * this.m_elapsed % 360f;
		return center + Quaternion.Euler(0f, 0f, num + 360f / (float)this.numKnives * (float)i) * Vector3.right * radiusToUse;
	}

	// Token: 0x06007545 RID: 30021 RVA: 0x002EB0D8 File Offset: 0x002E92D8
	private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
	{
		Projectile component = other.GetComponent<Projectile>();
		if (component != null)
		{
			if (component.Owner is PlayerController)
			{
				PhysicsEngine.SkipCollision = true;
			}
			else if (this.m_lightknife)
			{
				PassiveReflectItem.ReflectBullet(component, true, this.m_player, 10f, 1f, 1f, 0f);
				this.DestroyKnife(myRigidbody);
			}
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

	// Token: 0x06007546 RID: 30022 RVA: 0x002EB180 File Offset: 0x002E9380
	private void HandleCollision(SpeculativeRigidbody other, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (other.GetComponent<AIActor>() != null)
		{
			HealthHaver component = other.GetComponent<HealthHaver>();
			float num = this.knifeDamage;
			if (this.m_lightknife)
			{
				num *= 3f;
			}
			component.ApplyDamage(num, Vector2.zero, "Knife Shield", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			int num2 = Array.IndexOf<SpeculativeRigidbody>(this.m_knives, source);
			if (num2 != -1)
			{
				this.m_knives[num2] = null;
			}
			source.sprite.PlayEffectOnSprite(this.deathVFX, Vector3.zero, false);
			UnityEngine.Object.Destroy(source.gameObject);
		}
		else if (other.GetComponent<Projectile>() != null)
		{
			Projectile component2 = other.GetComponent<Projectile>();
			if (component2.Owner is PlayerController)
			{
				return;
			}
			if (!this.m_lightknife)
			{
				component2.DieInAir(false, true, true, false);
			}
			this.remainingHealth -= component2.ModifiedDamage;
			if (this.remainingHealth <= 0f)
			{
				this.DestroyKnife(source);
			}
		}
	}

	// Token: 0x06007547 RID: 30023 RVA: 0x002EB284 File Offset: 0x002E9484
	private void DestroyKnife(SpeculativeRigidbody source)
	{
		int num = Array.IndexOf<SpeculativeRigidbody>(this.m_knives, source);
		if (num != -1)
		{
			this.m_knives[num] = null;
		}
		source.sprite.PlayEffectOnSprite(this.deathVFX, Vector3.zero, false);
		UnityEngine.Object.Destroy(source.gameObject);
	}

	// Token: 0x06007548 RID: 30024 RVA: 0x002EB2D0 File Offset: 0x002E94D0
	private void Update()
	{
		if (GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating)
		{
			return;
		}
		this.m_elapsed += BraveTime.DeltaTime;
		bool flag = this.m_currentShieldVelocity != Vector3.zero;
		Vector3 vector = this.m_currentShieldVelocity * BraveTime.DeltaTime;
		this.m_currentShieldCenterOffset += vector;
		if (!flag)
		{
			this.m_cachedOffsetBase = this.m_player.LockedApproximateSpriteCenter;
		}
		else
		{
			this.m_traversedDistance += vector.magnitude;
			if (this.m_traversedDistance > this.throwRange)
			{
				for (int i = 0; i < this.m_knives.Length; i++)
				{
					if (this.m_knives[i] != null && this.m_knives[i])
					{
						this.m_knives[i].sprite.PlayEffectOnSprite(this.deathVFX, Vector3.zero, false);
						UnityEngine.Object.Destroy(this.m_knives[i].gameObject);
						this.m_knives[i] = null;
					}
				}
			}
		}
		Vector3 vector2 = this.m_cachedOffsetBase + this.m_currentShieldCenterOffset;
		float num = this.circleRadius;
		if (flag)
		{
			num = Mathf.Lerp(this.circleRadius, this.throwRadius, this.m_traversedDistance / this.radiusChangeDistance);
		}
		for (int j = 0; j < this.numKnives; j++)
		{
			if (this.m_knives[j] != null && this.m_knives[j])
			{
				Vector3 targetPositionForKniveID = this.GetTargetPositionForKniveID(vector2, j, num);
				Vector3 vector3 = targetPositionForKniveID - this.m_knives[j].transform.position;
				Vector2 vector4 = vector3.XY() / BraveTime.DeltaTime;
				this.m_knives[j].Velocity = vector4;
				this.m_knives[j].sprite.UpdateZDepth();
			}
		}
	}

	// Token: 0x06007549 RID: 30025 RVA: 0x002EB4D8 File Offset: 0x002E96D8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007721 RID: 30497
	public int numKnives = 5;

	// Token: 0x04007722 RID: 30498
	public float knifeDamage = 5f;

	// Token: 0x04007723 RID: 30499
	public float circleRadius = 3f;

	// Token: 0x04007724 RID: 30500
	public float rotationDegreesPerSecond = 360f;

	// Token: 0x04007725 RID: 30501
	public float throwSpeed = 3f;

	// Token: 0x04007726 RID: 30502
	public float throwRange = 25f;

	// Token: 0x04007727 RID: 30503
	public float throwRadius = 3f;

	// Token: 0x04007728 RID: 30504
	public float radiusChangeDistance = 3f;

	// Token: 0x04007729 RID: 30505
	public float remainingHealth;

	// Token: 0x0400772A RID: 30506
	public GameObject deathVFX;

	// Token: 0x0400772B RID: 30507
	private bool m_lightknife;

	// Token: 0x0400772C RID: 30508
	protected PlayerController m_player;

	// Token: 0x0400772D RID: 30509
	protected SpeculativeRigidbody[] m_knives;

	// Token: 0x0400772E RID: 30510
	protected float m_elapsed;

	// Token: 0x0400772F RID: 30511
	protected float m_traversedDistance;

	// Token: 0x04007730 RID: 30512
	protected Vector3 m_currentShieldVelocity = Vector3.zero;

	// Token: 0x04007731 RID: 30513
	protected Vector3 m_currentShieldCenterOffset = Vector3.zero;

	// Token: 0x04007732 RID: 30514
	private Shader m_lightknivesShader;

	// Token: 0x04007733 RID: 30515
	private Vector3 m_cachedOffsetBase;
}
