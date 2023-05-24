using System;
using UnityEngine;

// Token: 0x02001175 RID: 4469
public class GoopDoer : BraveBehaviour
{
	// Token: 0x06006331 RID: 25393 RVA: 0x00267418 File Offset: 0x00265618
	public void Start()
	{
		this.m_gooper = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinition);
		if (this.updateOnAnimFrames && base.spriteAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		}
		if (this.updateOnPreDeath && base.healthHaver)
		{
			base.healthHaver.OnPreDeath += this.OnPreDeath;
		}
		if (this.updateOnDeath && base.healthHaver)
		{
			base.healthHaver.OnDeath += this.OnDeath;
		}
		if (this.updateOnCollision)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}
		if (this.updateOnGrounded)
		{
			if (base.debris)
			{
				DebrisObject debris = base.debris;
				debris.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debris.OnGrounded, new Action<DebrisObject>(this.OnDebrisGrounded));
			}
			if (base.projectile is ArcProjectile)
			{
				(base.projectile as ArcProjectile).OnGrounded += this.OnProjectileGrounded;
			}
		}
		if (this.UsesDispersalParticles && this.m_dispersalParticles == null)
		{
			this.m_dispersalParticles = GlobalDispersalParticleManager.GetSystemForPrefab(this.DispersalParticleSystemPrefab);
		}
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x002675AC File Offset: 0x002657AC
	public void Update()
	{
		this.m_timeSinceLastGoop += BraveTime.DeltaTime;
		if (this.ShouldUpdate())
		{
			if (base.aiActor != null && !base.aiActor.HasBeenEngaged)
			{
				return;
			}
			if (base.aiActor != null && !base.aiActor.HasBeenAwoken)
			{
				return;
			}
			this.m_updateTimer -= BraveTime.DeltaTime;
			if (this.m_updateTimer <= 0f)
			{
				this.GoopItUp();
				this.m_updateTimer = this.updateFrequency;
			}
		}
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x00267650 File Offset: 0x00265850
	protected override void OnDestroy()
	{
		if (this.updateOnDestroy && this.m_gooper != null)
		{
			this.GoopItUp();
		}
		if (this.updateOnAnimFrames && base.spriteAnimator)
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Remove(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleAnimationEvent));
		}
		if (this.updateOnPreDeath && base.healthHaver)
		{
			base.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		if (this.updateOnDeath && base.healthHaver)
		{
			base.healthHaver.OnDeath -= this.OnDeath;
		}
		if (this.updateOnGrounded)
		{
			if (base.debris)
			{
				DebrisObject debris = base.debris;
				debris.OnGrounded = (Action<DebrisObject>)Delegate.Remove(debris.OnGrounded, new Action<DebrisObject>(this.OnDebrisGrounded));
			}
			if (base.projectile is ArcProjectile)
			{
				(base.projectile as ArcProjectile).OnGrounded -= this.OnProjectileGrounded;
			}
		}
		base.OnDestroy();
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x0026779C File Offset: 0x0026599C
	private void HandleAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNum)
	{
		if (clip.GetFrame(frameNum).eventInfo == "goop")
		{
			this.GoopItUp();
		}
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x002677C0 File Offset: 0x002659C0
	private void OnPreDeath(Vector2 finalDamageDirection)
	{
		this.GoopItUp();
	}

	// Token: 0x06006336 RID: 25398 RVA: 0x002677C8 File Offset: 0x002659C8
	private void OnDeath(Vector2 finalDamageDirection)
	{
		this.GoopItUp();
	}

	// Token: 0x06006337 RID: 25399 RVA: 0x002677D0 File Offset: 0x002659D0
	private void OnCollision(CollisionData collisionData)
	{
		this.GoopItUp();
	}

	// Token: 0x06006338 RID: 25400 RVA: 0x002677D8 File Offset: 0x002659D8
	private void OnDebrisGrounded(DebrisObject debrisObject)
	{
		this.GoopItUp();
	}

	// Token: 0x06006339 RID: 25401 RVA: 0x002677E0 File Offset: 0x002659E0
	private void OnProjectileGrounded()
	{
		this.GoopItUp();
	}

	// Token: 0x0600633A RID: 25402 RVA: 0x002677E8 File Offset: 0x002659E8
	private bool ShouldUpdate()
	{
		return this.updateTiming == GoopDoer.UpdateTiming.Always || (this.updateTiming == GoopDoer.UpdateTiming.IfMoving && (Mathf.Abs(base.specRigidbody.Velocity.x) > 0.0001f || Mathf.Abs(base.specRigidbody.Velocity.y) > 0.0001f));
	}

	// Token: 0x0600633B RID: 25403 RVA: 0x00267850 File Offset: 0x00265A50
	private void GoopItUp()
	{
		float num = this.defaultGoopRadius;
		Vector2 vector = base.transform.position;
		if (this.positionSource == GoopDoer.PositionSource.SpriteCenter)
		{
			vector = base.sprite.WorldCenter;
		}
		else if (this.positionSource == GoopDoer.PositionSource.GroundCenter)
		{
			vector = base.specRigidbody.GetUnitCenter(ColliderType.Ground);
		}
		else if (this.positionSource == GoopDoer.PositionSource.HitBoxCenter)
		{
			if (base.specRigidbody.HitboxPixelCollider == null || !base.specRigidbody.HitboxPixelCollider.Enabled)
			{
				return;
			}
			vector = base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
		else if (this.positionSource == GoopDoer.PositionSource.SpecifyGameObject)
		{
			vector = this.goopCenter.transform.position.XY();
		}
		if (this.isTimed && this.m_lastGoopPosition != Vector2.zero && Vector2.Distance(vector, this.m_lastGoopPosition) < 0.2f && this.m_timeSinceLastGoop < this.goopTime)
		{
			return;
		}
		if (this.goopSizeVaries)
		{
			if (this.goopSizeRandom)
			{
				num = UnityEngine.Random.Range(this.radiusMin, this.radiusMax);
			}
			else
			{
				num = BraveMathCollege.SmoothLerp(this.radiusMin, this.radiusMax, Mathf.PingPong(Time.time, this.varyCycleTime) / this.varyCycleTime);
			}
		}
		if (this.isTimed)
		{
			this.m_gooper.TimedAddGoopCircle(vector, num, this.goopTime, this.suppressSplashes);
		}
		else
		{
			DeadlyDeadlyGoopManager gooper = this.m_gooper;
			Vector2 vector2 = vector;
			float num2 = num;
			bool flag = this.suppressSplashes;
			gooper.AddGoopCircle(vector2, num2, -1, flag, -1);
		}
		if (this.UsesDispersalParticles)
		{
			this.DoDispersalParticles(vector, num);
		}
		this.m_lastGoopPosition = vector;
		this.m_timeSinceLastGoop = 0f;
	}

	// Token: 0x0600633C RID: 25404 RVA: 0x00267A20 File Offset: 0x00265C20
	private void DoDispersalParticles(Vector2 posStart, float radius)
	{
		int num = Mathf.RoundToInt(radius * radius * this.DispersalDensity);
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = posStart + UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(0f, radius);
			float num2 = Mathf.PerlinNoise(vector.x / 3f, vector.y / 3f);
			Vector3 vector2 = Quaternion.Euler(0f, 0f, num2 * 360f) * Vector3.right;
			Vector3 vector3 = Vector3.Lerp(vector2, UnityEngine.Random.insideUnitSphere, UnityEngine.Random.Range(this.DispersalMinCoherency, this.DispersalMaxCoherency));
			ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
			{
				position = vector,
				velocity = vector3 * this.m_dispersalParticles.startSpeed,
				startSize = this.m_dispersalParticles.startSize,
				startLifetime = this.m_dispersalParticles.startLifetime,
				startColor = this.m_dispersalParticles.startColor
			};
			this.m_dispersalParticles.Emit(emitParams, 1);
		}
	}

	// Token: 0x04005E93 RID: 24211
	public GoopDefinition goopDefinition;

	// Token: 0x04005E94 RID: 24212
	public GoopDoer.PositionSource positionSource;

	// Token: 0x04005E95 RID: 24213
	[ShowInInspectorIf("positionSource", 3, false)]
	public GameObject goopCenter;

	// Token: 0x04005E96 RID: 24214
	public GoopDoer.UpdateTiming updateTiming;

	// Token: 0x04005E97 RID: 24215
	public float updateFrequency = 0.05f;

	// Token: 0x04005E98 RID: 24216
	public bool isTimed;

	// Token: 0x04005E99 RID: 24217
	[ShowInInspectorIf("isTimed", true)]
	public float goopTime = 1f;

	// Token: 0x04005E9A RID: 24218
	[Header("Triggers")]
	public bool updateOnPreDeath;

	// Token: 0x04005E9B RID: 24219
	public bool updateOnDeath;

	// Token: 0x04005E9C RID: 24220
	public bool updateOnAnimFrames;

	// Token: 0x04005E9D RID: 24221
	public bool updateOnCollision;

	// Token: 0x04005E9E RID: 24222
	public bool updateOnGrounded;

	// Token: 0x04005E9F RID: 24223
	public bool updateOnDestroy;

	// Token: 0x04005EA0 RID: 24224
	[Header("Global Settings")]
	public float defaultGoopRadius = 1f;

	// Token: 0x04005EA1 RID: 24225
	public bool suppressSplashes;

	// Token: 0x04005EA2 RID: 24226
	public bool goopSizeVaries;

	// Token: 0x04005EA3 RID: 24227
	[ShowInInspectorIf("goopSizeVaries", false)]
	public float varyCycleTime = 1f;

	// Token: 0x04005EA4 RID: 24228
	[ShowInInspectorIf("goopSizeVaries", false)]
	public float radiusMin = 0.5f;

	// Token: 0x04005EA5 RID: 24229
	[ShowInInspectorIf("goopSizeVaries", false)]
	public float radiusMax = 1f;

	// Token: 0x04005EA6 RID: 24230
	[ShowInInspectorIf("goopSizeVaries", false)]
	public bool goopSizeRandom;

	// Token: 0x04005EA7 RID: 24231
	[Header("Particles")]
	public bool UsesDispersalParticles;

	// Token: 0x04005EA8 RID: 24232
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public float DispersalDensity = 3f;

	// Token: 0x04005EA9 RID: 24233
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public float DispersalMinCoherency = 0.2f;

	// Token: 0x04005EAA RID: 24234
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public float DispersalMaxCoherency = 1f;

	// Token: 0x04005EAB RID: 24235
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public GameObject DispersalParticleSystemPrefab;

	// Token: 0x04005EAC RID: 24236
	private float m_updateTimer;

	// Token: 0x04005EAD RID: 24237
	private DeadlyDeadlyGoopManager m_gooper;

	// Token: 0x04005EAE RID: 24238
	private ParticleSystem m_dispersalParticles;

	// Token: 0x04005EAF RID: 24239
	private Vector2 m_lastGoopPosition = Vector2.zero;

	// Token: 0x04005EB0 RID: 24240
	private float m_timeSinceLastGoop = 10f;

	// Token: 0x02001176 RID: 4470
	public enum PositionSource
	{
		// Token: 0x04005EB2 RID: 24242
		SpriteCenter,
		// Token: 0x04005EB3 RID: 24243
		GroundCenter,
		// Token: 0x04005EB4 RID: 24244
		HitBoxCenter,
		// Token: 0x04005EB5 RID: 24245
		SpecifyGameObject
	}

	// Token: 0x02001177 RID: 4471
	public enum UpdateTiming
	{
		// Token: 0x04005EB7 RID: 24247
		Always,
		// Token: 0x04005EB8 RID: 24248
		IfMoving,
		// Token: 0x04005EB9 RID: 24249
		TriggerOnly
	}
}
