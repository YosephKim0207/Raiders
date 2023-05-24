using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001815 RID: 6165
public class UltraFortunesFavor : BraveBehaviour
{
	// Token: 0x170015CC RID: 5580
	// (get) Token: 0x06009161 RID: 37217 RVA: 0x003D8158 File Offset: 0x003D6358
	public Vector2 BulletCircleCenter
	{
		get
		{
			return base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x170015CD RID: 5581
	// (get) Token: 0x06009162 RID: 37218 RVA: 0x003D8168 File Offset: 0x003D6368
	public Vector2 BeamCircleCenter
	{
		get
		{
			return base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x170015CE RID: 5582
	// (get) Token: 0x06009163 RID: 37219 RVA: 0x003D8178 File Offset: 0x003D6378
	public Vector2 GoopCircleCenter
	{
		get
		{
			return base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
	}

	// Token: 0x06009164 RID: 37220 RVA: 0x003D8188 File Offset: 0x003D6388
	public void Awake()
	{
		this.m_enemyOverlapTimer = UnityEngine.Random.Range(2f, 4f);
	}

	// Token: 0x06009165 RID: 37221 RVA: 0x003D81A0 File Offset: 0x003D63A0
	public void Start()
	{
		base.specRigidbody.Initialize();
		if (this.bulletRadius > 0f)
		{
			IntVector2 intVector = PhysicsEngine.UnitToPixel(this.BulletCircleCenter - base.transform.position.XY());
			int num = PhysicsEngine.UnitToPixel(this.bulletRadius);
			this.m_bulletBlocker = new PixelCollider();
			this.m_bulletBlocker.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle;
			this.m_bulletBlocker.CollisionLayer = CollisionLayer.BulletBlocker;
			this.m_bulletBlocker.IsTrigger = true;
			this.m_bulletBlocker.ManualOffsetX = intVector.x - num;
			this.m_bulletBlocker.ManualOffsetY = intVector.y - num;
			this.m_bulletBlocker.ManualDiameter = num * 2;
			this.m_bulletBlocker.Regenerate(base.transform, true, true);
			base.specRigidbody.PixelColliders.Add(this.m_bulletBlocker);
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
		}
		if (this.beamRadius > 0f)
		{
			IntVector2 intVector2 = PhysicsEngine.UnitToPixel(this.BeamCircleCenter - base.transform.position.XY());
			int num2 = PhysicsEngine.UnitToPixel(this.beamRadius);
			this.m_beamReflector = new PixelCollider();
			this.m_beamReflector.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle;
			this.m_beamReflector.CollisionLayer = CollisionLayer.BeamBlocker;
			this.m_beamReflector.IsTrigger = false;
			this.m_beamReflector.ManualOffsetX = intVector2.x - num2;
			this.m_beamReflector.ManualOffsetY = intVector2.y - num2;
			this.m_beamReflector.ManualDiameter = num2 * 2;
			this.m_beamReflector.Regenerate(base.transform, true, true);
			base.specRigidbody.PixelColliders.Add(this.m_beamReflector);
		}
		if (this.bulletRadius > 0f || this.beamRadius > 0f)
		{
			PhysicsEngine.UpdatePosition(base.specRigidbody);
		}
		if (this.goopRadius > 0f)
		{
			this.m_goopExceptionId = DeadlyDeadlyGoopManager.RegisterUngoopableCircle(this.GoopCircleCenter, this.goopRadius);
			this.m_lastPosition = base.transform.position.XY();
		}
	}

	// Token: 0x06009166 RID: 37222 RVA: 0x003D83E0 File Offset: 0x003D65E0
	public void Update()
	{
		for (int i = this.m_caughtBullets.Count - 1; i >= 0; i--)
		{
			UltraFortunesFavor.ProjectileData projectileData = this.m_caughtBullets[i];
			projectileData.positionDeg += BraveTime.DeltaTime * projectileData.degPerSecond;
			Projectile projectile = this.m_caughtBullets[i].projectile;
			if (!(projectile == null))
			{
				if (!projectile)
				{
					this.m_caughtBullets[i] = null;
				}
				else
				{
					this.HitFromPoint(projectile.transform.position.XY());
					Vector2 vector = this.BulletCircleCenter - projectile.transform.position.XY();
					if (Mathf.Abs(BraveMathCollege.ClampAngle180(vector.ToAngle() - this.m_caughtBullets[i].initialVelocity.ToAngle())) > 90f)
					{
						Vector2 vector2 = Quaternion.Euler(0f, 0f, -90f * Mathf.Sign(this.m_caughtBullets[i].degPerSecond)) * (this.BulletCircleCenter - projectile.transform.position.XY());
						projectile.ManualControl = false;
						projectile.SendInDirection(this.m_caughtBullets[i].initialVelocity.magnitude * vector2.normalized, true, true);
						this.m_caughtBullets[i].projectile = null;
					}
					else
					{
						Vector2 bulletPosition = this.GetBulletPosition(projectileData.positionDeg);
						projectile.specRigidbody.Velocity = (bulletPosition - projectile.transform.position) / BraveTime.DeltaTime;
						if (projectile.shouldRotate)
						{
							projectile.transform.rotation = Quaternion.Euler(0f, 0f, 180f + (Quaternion.Euler(0f, 0f, 90f) * (this.BulletCircleCenter - bulletPosition)).XY().ToAngle());
						}
					}
				}
			}
		}
		if (this.goopRadius > 0f)
		{
			Vector2 vector3 = base.transform.position.XY();
			if (vector3 != this.m_lastPosition)
			{
				DeadlyDeadlyGoopManager.UpdateUngoopableCircle(this.m_goopExceptionId, this.GoopCircleCenter, this.goopRadius);
				this.m_lastPosition = vector3;
			}
		}
		this.m_enemyOverlapTimer -= BraveTime.DeltaTime;
		if (PhysicsEngine.HasInstance && this.m_enemyOverlapTimer <= 0f)
		{
			List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(base.specRigidbody, null, false);
			for (int j = 0; j < overlappingRigidbodies.Count; j++)
			{
				SpeculativeRigidbody speculativeRigidbody = overlappingRigidbodies[j];
				if (speculativeRigidbody && speculativeRigidbody.aiActor)
				{
					base.specRigidbody.RegisterGhostCollisionException(speculativeRigidbody);
					speculativeRigidbody.RegisterGhostCollisionException(base.specRigidbody);
				}
			}
			this.m_enemyOverlapTimer = 2f;
		}
	}

	// Token: 0x06009167 RID: 37223 RVA: 0x003D8714 File Offset: 0x003D6914
	protected override void OnDestroy()
	{
		if (this.bulletRadius > 0f)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Remove(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
		}
		DeadlyDeadlyGoopManager.DeregisterUngoopableCircle(this.m_goopExceptionId);
		base.OnDestroy();
	}

	// Token: 0x06009168 RID: 37224 RVA: 0x003D876C File Offset: 0x003D696C
	private void OnTriggerCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (!base.enabled)
		{
			return;
		}
		if (collisionData.MyPixelCollider == this.m_bulletBlocker && collisionData.OtherRigidbody != null && collisionData.OtherRigidbody.projectile != null)
		{
			Projectile projectile = collisionData.OtherRigidbody.projectile;
			Vector2 vector = this.BulletCircleCenter - projectile.transform.position.XY();
			float num = BraveMathCollege.ClampAngle180(vector.ToAngle() - projectile.specRigidbody.Velocity.ToAngle());
			float num2 = BraveMathCollege.ClampAngle360((collisionData.Contact - this.BulletCircleCenter).ToAngle());
			float num3 = Mathf.Sign(num) * projectile.specRigidbody.Velocity.magnitude / (3.1415927f * this.bulletRadius) * 180f;
			this.m_caughtBullets.Insert(Mathf.Max(0, this.m_caughtBullets.Count - 1), new UltraFortunesFavor.ProjectileData(projectile, num2, projectile.specRigidbody.Velocity, num3 * this.bulletSpeedModifier));
			projectile.specRigidbody.Velocity = Vector2.zero;
			projectile.ManualControl = true;
			collisionData.OtherRigidbody.RegisterSpecificCollisionException(collisionData.MyRigidbody);
			this.HitFromDirection(-vector);
			if (!base.talkDoer || !base.talkDoer.IsTalking)
			{
				base.SendPlaymakerEvent("takePlayerDamage");
			}
		}
	}

	// Token: 0x06009169 RID: 37225 RVA: 0x003D88E4 File Offset: 0x003D6AE4
	public Vector2 GetBeamNormal(Vector2 targetPoint)
	{
		return (targetPoint - this.BulletCircleCenter).normalized;
	}

	// Token: 0x0600916A RID: 37226 RVA: 0x003D8908 File Offset: 0x003D6B08
	public void HitFromPoint(Vector2 targetPoint)
	{
		this.HitFromDirection(targetPoint - this.BulletCircleCenter);
	}

	// Token: 0x0600916B RID: 37227 RVA: 0x003D891C File Offset: 0x003D6B1C
	private Vector2 GetBulletPosition(float angle)
	{
		return this.BulletCircleCenter + new Vector2(Mathf.Cos(angle * 0.017453292f), Mathf.Sin(angle * 0.017453292f)) * this.bulletRadius;
	}

	// Token: 0x0600916C RID: 37228 RVA: 0x003D8954 File Offset: 0x003D6B54
	private void HitFromDirection(Vector2 dir)
	{
		int num = BraveMathCollege.VectorToOctant(dir);
		if (!this.m_octantVfx[num])
		{
			Vector3 vector = Quaternion.Euler(0f, 0f, (float)(-(float)num * 45 - 90)) * new Vector3(-this.vfxOffset, 0f, 0f);
			this.m_octantVfx[num] = this.PlayEffectOnActor(this.sparkOctantVFX, vector, true, true);
			this.m_octantVfx[num].transform.rotation = Quaternion.Euler(0f, 0f, (float)(-45 + -45 * num));
		}
	}

	// Token: 0x0600916D RID: 37229 RVA: 0x003D89F0 File Offset: 0x003D6BF0
	private GameObject PlayEffectOnActor(GameObject effect, Vector3 offset, bool attached = true, bool alreadyMiddleCenter = false)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(effect, true);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		if (!alreadyMiddleCenter)
		{
			component.PlaceAtPositionByAnchor(base.sprite.WorldCenter.ToVector3ZUp(0f) + offset, tk2dBaseSprite.Anchor.MiddleCenter);
		}
		else
		{
			component.transform.position = base.sprite.WorldCenter.ToVector3ZUp(0f) + offset;
		}
		if (attached)
		{
			gameObject.transform.parent = base.transform;
			component.HeightOffGround = 0.2f;
			base.sprite.AttachRenderer(component);
		}
		return gameObject;
	}

	// Token: 0x040099A2 RID: 39330
	public GameObject sparkOctantVFX;

	// Token: 0x040099A3 RID: 39331
	public float vfxOffset = 0.625f;

	// Token: 0x040099A4 RID: 39332
	public float bulletRadius = 2f;

	// Token: 0x040099A5 RID: 39333
	public float bulletSpeedModifier = 0.8f;

	// Token: 0x040099A6 RID: 39334
	public float beamRadius = 2f;

	// Token: 0x040099A7 RID: 39335
	public float goopRadius = 2f;

	// Token: 0x040099A8 RID: 39336
	private readonly List<UltraFortunesFavor.ProjectileData> m_caughtBullets = new List<UltraFortunesFavor.ProjectileData>();

	// Token: 0x040099A9 RID: 39337
	private GameObject[] m_octantVfx = new GameObject[8];

	// Token: 0x040099AA RID: 39338
	private PixelCollider m_bulletBlocker;

	// Token: 0x040099AB RID: 39339
	private PixelCollider m_beamReflector;

	// Token: 0x040099AC RID: 39340
	private Vector2 m_lastPosition;

	// Token: 0x040099AD RID: 39341
	private int m_goopExceptionId = -1;

	// Token: 0x040099AE RID: 39342
	private float m_enemyOverlapTimer;

	// Token: 0x02001816 RID: 6166
	private class ProjectileData
	{
		// Token: 0x0600916E RID: 37230 RVA: 0x003D8A90 File Offset: 0x003D6C90
		public ProjectileData(Projectile projectile, float positionDeg, Vector2 initialVelocity, float degPerSecond)
		{
			this.projectile = projectile;
			this.positionDeg = positionDeg;
			this.initialVelocity = initialVelocity;
			this.degPerSecond = degPerSecond;
		}

		// Token: 0x040099AF RID: 39343
		public Projectile projectile;

		// Token: 0x040099B0 RID: 39344
		public float positionDeg;

		// Token: 0x040099B1 RID: 39345
		public Vector2 initialVelocity;

		// Token: 0x040099B2 RID: 39346
		public float degPerSecond;
	}
}
