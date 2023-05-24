using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013CD RID: 5069
public class OrbitProjectileMotionModule : ProjectileAndBeamMotionModule
{
	// Token: 0x060072F4 RID: 29428 RVA: 0x002DADFC File Offset: 0x002D8FFC
	public static int GetOrbitersInGroup(int group)
	{
		if (OrbitProjectileMotionModule.m_currentOrbiters.ContainsKey(group))
		{
			return (OrbitProjectileMotionModule.m_currentOrbiters[group] == null) ? 0 : OrbitProjectileMotionModule.m_currentOrbiters[group].Count;
		}
		return 0;
	}

	// Token: 0x1700114D RID: 4429
	// (get) Token: 0x060072F5 RID: 29429 RVA: 0x002DAE38 File Offset: 0x002D9038
	// (set) Token: 0x060072F6 RID: 29430 RVA: 0x002DAE40 File Offset: 0x002D9040
	public float BeamOrbitRadius
	{
		get
		{
			return this.m_beamOrbitRadius;
		}
		set
		{
			this.m_beamOrbitRadius = value;
			this.m_beamOrbitRadiusCircumference = 6.2831855f * this.m_beamOrbitRadius;
		}
	}

	// Token: 0x060072F7 RID: 29431 RVA: 0x002DAE5C File Offset: 0x002D905C
	public override void UpdateDataOnBounce(float angleDiff)
	{
		if (!float.IsNaN(angleDiff))
		{
			this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
			this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
		}
	}

	// Token: 0x060072F8 RID: 29432 RVA: 0x002DAECC File Offset: 0x002D90CC
	public override void AdjustRightVector(float angleDiff)
	{
		if (!float.IsNaN(angleDiff))
		{
			this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
			this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
		}
	}

	// Token: 0x060072F9 RID: 29433 RVA: 0x002DAF3C File Offset: 0x002D913C
	private List<OrbitProjectileMotionModule> RegisterSelfWithDictionary()
	{
		if (!OrbitProjectileMotionModule.m_currentOrbiters.ContainsKey(this.OrbitGroup))
		{
			OrbitProjectileMotionModule.m_currentOrbiters.Add(this.OrbitGroup, new List<OrbitProjectileMotionModule>());
		}
		List<OrbitProjectileMotionModule> list = OrbitProjectileMotionModule.m_currentOrbiters[this.OrbitGroup];
		if (!list.Contains(this))
		{
			list.Add(this);
		}
		return list;
	}

	// Token: 0x060072FA RID: 29434 RVA: 0x002DAF98 File Offset: 0x002D9198
	private void DeregisterSelfWithDictionary()
	{
		if (OrbitProjectileMotionModule.m_currentOrbiters.ContainsKey(this.OrbitGroup))
		{
			List<OrbitProjectileMotionModule> list = OrbitProjectileMotionModule.m_currentOrbiters[this.OrbitGroup];
			list.Remove(this);
		}
	}

	// Token: 0x060072FB RID: 29435 RVA: 0x002DAFD4 File Offset: 0x002D91D4
	public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
	{
		if (this.m_isOrbiting && (!source || (!this.usesAlternateOrbitTarget && !source.Owner) || (this.usesAlternateOrbitTarget && !this.alternateOrbitTarget)))
		{
			this.m_isOrbiting = false;
		}
		if (this.m_isOrbiting && this.lifespan > 0f && m_timeElapsed > this.lifespan)
		{
			this.m_isOrbiting = false;
			this.DeregisterSelfWithDictionary();
		}
		if (this.m_isOrbiting)
		{
			Vector2 vector = ((!projectileSprite) ? projectileTransform.position.XY() : projectileSprite.WorldCenter);
			if (this.HasSpawnVFX && !this.m_hasDoneSpawnVFX)
			{
				this.m_hasDoneSpawnVFX = true;
				this.m_spawnVFXActive = true;
				this.m_activeSpawnVFX = SpawnManager.SpawnVFX(this.SpawnVFX, vector, Quaternion.identity);
				source.sprite.renderer.enabled = false;
			}
			if (this.m_hasDoneSpawnVFX)
			{
				this.m_spawnVFXElapsed += BraveTime.DeltaTime;
			}
			if (this.m_spawnVFXActive && (!this.m_activeSpawnVFX || !this.m_activeSpawnVFX.activeSelf || (this.CustomSpawnVFXElapsed > 0f && this.m_spawnVFXElapsed > this.CustomSpawnVFXElapsed)))
			{
				this.m_activeSpawnVFX = null;
				this.m_spawnVFXActive = false;
				source.sprite.renderer.enabled = true;
			}
			if (!this.m_initialized)
			{
				this.m_initialized = true;
				this.m_initialRightVector = ((!shouldRotate) ? m_currentDirection : projectileTransform.right.XY());
				this.m_initialUpVector = ((!shouldRotate) ? (Quaternion.Euler(0f, 0f, 90f) * m_currentDirection) : projectileTransform.up);
				this.m_radius = UnityEngine.Random.Range(this.MinRadius, this.MaxRadius);
				this.m_currentAngle = this.m_initialRightVector.ToAngle();
				source.OnDestruction += this.OnDestroyed;
			}
			this.RegisterSelfWithDictionary();
			m_timeElapsed += BraveTime.DeltaTime;
			float radius = this.m_radius;
			float num = source.Speed * BraveTime.DeltaTime;
			float num2 = num / (6.2831855f * radius) * 360f;
			this.m_currentAngle += num2;
			Vector2 vector2 = Vector2.zero;
			if (this.usesAlternateOrbitTarget)
			{
				vector2 = this.alternateOrbitTarget.UnitCenter;
			}
			else
			{
				vector2 = source.Owner.CenterPosition;
			}
			Vector2 vector3 = vector2 + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right * radius).XY();
			if (this.StackHelix)
			{
				float num3 = 2f;
				float num4 = 1f;
				int num5 = ((!this.ForceInvert) ? 1 : (-1));
				float num6 = (float)num5 * num4 * Mathf.Sin(source.GetElapsedDistance() / num3);
				vector3 += (vector3 - vector2).normalized * num6;
			}
			Vector2 vector4 = (vector3 - vector) / BraveTime.DeltaTime;
			m_currentDirection = vector4.normalized;
			if (shouldRotate)
			{
				float num7 = m_currentDirection.ToAngle();
				if (float.IsNaN(num7) || float.IsInfinity(num7))
				{
					num7 = 0f;
				}
				projectileTransform.localRotation = Quaternion.Euler(0f, 0f, num7);
			}
			specRigidbody.Velocity = vector4;
			if (float.IsNaN(specRigidbody.Velocity.magnitude) || Mathf.Approximately(specRigidbody.Velocity.magnitude, 0f))
			{
				source.DieInAir(false, true, true, false);
			}
			if (this.m_activeSpawnVFX)
			{
				this.m_activeSpawnVFX.transform.position = vector3.ToVector3ZisY(0f);
			}
		}
	}

	// Token: 0x060072FC RID: 29436 RVA: 0x002DB414 File Offset: 0x002D9614
	public void BeamDestroyed()
	{
		this.OnDestroyed(null);
	}

	// Token: 0x060072FD RID: 29437 RVA: 0x002DB420 File Offset: 0x002D9620
	private void OnDestroyed(Projectile obj)
	{
		this.DeregisterSelfWithDictionary();
		if (this.m_isBeam)
		{
			this.m_isBeam = false;
			OrbitProjectileMotionModule.ActiveBeams--;
		}
	}

	// Token: 0x060072FE RID: 29438 RVA: 0x002DB448 File Offset: 0x002D9648
	public override void SentInDirection(ProjectileData baseData, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool shouldRotate, Vector2 dirVec, bool resetDistance, bool updateRotation)
	{
	}

	// Token: 0x060072FF RID: 29439 RVA: 0x002DB44C File Offset: 0x002D964C
	public void RegisterAsBeam(BeamController beam)
	{
		if (!this.m_isBeam)
		{
			BasicBeamController basicBeamController = beam as BasicBeamController;
			if (basicBeamController && !basicBeamController.IsReflectedBeam)
			{
				basicBeamController.IgnoreTilesDistance = this.m_beamOrbitRadiusCircumference;
			}
			this.m_isBeam = true;
			OrbitProjectileMotionModule.ActiveBeams++;
		}
	}

	// Token: 0x06007300 RID: 29440 RVA: 0x002DB4A0 File Offset: 0x002D96A0
	public override Vector2 GetBoneOffset(BasicBeamController.BeamBone bone, BeamController sourceBeam, bool inverted)
	{
		if (sourceBeam.IsReflectedBeam)
		{
			return Vector2.zero;
		}
		PlayerController playerController = sourceBeam.Owner as PlayerController;
		Vector2 vector = playerController.unadjustedAimPoint.XY() - playerController.CenterPosition;
		float num = vector.ToAngle();
		Vector2 vector2 = bone.Position - playerController.CenterPosition;
		Vector2 vector3;
		if (bone.PosX < this.m_beamOrbitRadiusCircumference)
		{
			float num2 = bone.PosX / this.m_beamOrbitRadiusCircumference * 360f + num;
			float num3 = Mathf.Cos(0.017453292f * num2) * this.BeamOrbitRadius;
			float num4 = Mathf.Sin(0.017453292f * num2) * this.BeamOrbitRadius;
			bone.RotationAngle = num2 + 90f;
			vector3 = new Vector2(num3, num4) - vector2;
		}
		else
		{
			bone.RotationAngle = num;
			vector3 = vector.normalized * (bone.PosX - this.m_beamOrbitRadiusCircumference + this.m_beamOrbitRadius) - vector2;
		}
		if (this.StackHelix)
		{
			float num5 = 3f;
			float num6 = 1f;
			float num7 = 6f;
			int num8 = ((!(inverted ^ this.ForceInvert)) ? 1 : (-1));
			float num9 = bone.PosX - num7 * (Time.timeSinceLevelLoad % 600000f);
			float num10 = (float)num8 * num6 * Mathf.Sin(num9 * 3.1415927f / num5);
			vector3 += BraveMathCollege.DegreesToVector(bone.RotationAngle + 90f, Mathf.SmoothStep(0f, num10, bone.PosX));
		}
		return vector3;
	}

	// Token: 0x04007444 RID: 29764
	private static Dictionary<int, List<OrbitProjectileMotionModule>> m_currentOrbiters = new Dictionary<int, List<OrbitProjectileMotionModule>>();

	// Token: 0x04007445 RID: 29765
	public float MinRadius = 2f;

	// Token: 0x04007446 RID: 29766
	public float MaxRadius = 5f;

	// Token: 0x04007447 RID: 29767
	[NonSerialized]
	public float CustomSpawnVFXElapsed = -1f;

	// Token: 0x04007448 RID: 29768
	[NonSerialized]
	public bool HasSpawnVFX;

	// Token: 0x04007449 RID: 29769
	[NonSerialized]
	public GameObject SpawnVFX;

	// Token: 0x0400744A RID: 29770
	public bool ForceInvert;

	// Token: 0x0400744B RID: 29771
	private float m_radius;

	// Token: 0x0400744C RID: 29772
	private float m_currentAngle;

	// Token: 0x0400744D RID: 29773
	private bool m_initialized;

	// Token: 0x0400744E RID: 29774
	private Vector2 m_initialRightVector;

	// Token: 0x0400744F RID: 29775
	private Vector2 m_initialUpVector;

	// Token: 0x04007450 RID: 29776
	[NonSerialized]
	private bool m_isOrbiting = true;

	// Token: 0x04007451 RID: 29777
	[NonSerialized]
	public int OrbitGroup = -1;

	// Token: 0x04007452 RID: 29778
	[NonSerialized]
	private bool m_hasDoneSpawnVFX;

	// Token: 0x04007453 RID: 29779
	[NonSerialized]
	public float lifespan = -1f;

	// Token: 0x04007454 RID: 29780
	[NonSerialized]
	public bool usesAlternateOrbitTarget;

	// Token: 0x04007455 RID: 29781
	[NonSerialized]
	public SpeculativeRigidbody alternateOrbitTarget;

	// Token: 0x04007456 RID: 29782
	private float m_beamOrbitRadius = 2.75f;

	// Token: 0x04007457 RID: 29783
	private float m_beamOrbitRadiusCircumference = 17.27876f;

	// Token: 0x04007458 RID: 29784
	private bool m_spawnVFXActive;

	// Token: 0x04007459 RID: 29785
	private GameObject m_activeSpawnVFX;

	// Token: 0x0400745A RID: 29786
	private float m_spawnVFXElapsed;

	// Token: 0x0400745B RID: 29787
	public bool m_isBeam;

	// Token: 0x0400745C RID: 29788
	public static int ActiveBeams = 0;

	// Token: 0x0400745D RID: 29789
	public bool StackHelix;
}
