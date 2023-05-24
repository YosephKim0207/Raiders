using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000F9E RID: 3998
public class AIBeamShooter : BraveBehaviour
{
	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x060056A9 RID: 22185 RVA: 0x00210870 File Offset: 0x0020EA70
	// (set) Token: 0x060056AA RID: 22186 RVA: 0x00210878 File Offset: 0x0020EA78
	public float LaserAngle
	{
		get
		{
			return this.m_laserAngle;
		}
		set
		{
			this.m_laserAngle = value;
			if (this.m_firingLaser)
			{
				this.CurrentAiAnimator.FacingDirection = value;
			}
		}
	}

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x060056AB RID: 22187 RVA: 0x00210898 File Offset: 0x0020EA98
	public bool IsFiringLaser
	{
		get
		{
			return this.m_firingLaser;
		}
	}

	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x060056AC RID: 22188 RVA: 0x002108A0 File Offset: 0x0020EAA0
	public Vector2 LaserFiringCenter
	{
		get
		{
			return this.beamTransform.position.XY() + this.firingEllipseCenter;
		}
	}

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x060056AD RID: 22189 RVA: 0x002108C0 File Offset: 0x0020EAC0
	public AIAnimator CurrentAiAnimator
	{
		get
		{
			return (!this.specifyAnimator) ? base.aiAnimator : this.specifyAnimator;
		}
	}

	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x060056AE RID: 22190 RVA: 0x002108E4 File Offset: 0x0020EAE4
	// (set) Token: 0x060056AF RID: 22191 RVA: 0x002108EC File Offset: 0x0020EAEC
	public float MaxBeamLength { get; set; }

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x060056B0 RID: 22192 RVA: 0x002108F8 File Offset: 0x0020EAF8
	public BeamController LaserBeam
	{
		get
		{
			return this.m_laserBeam;
		}
	}

	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x060056B1 RID: 22193 RVA: 0x00210900 File Offset: 0x0020EB00
	// (set) Token: 0x060056B2 RID: 22194 RVA: 0x00210908 File Offset: 0x0020EB08
	public bool IgnoreAiActorPlayerChecks { get; set; }

	// Token: 0x060056B3 RID: 22195 RVA: 0x00210914 File Offset: 0x0020EB14
	public void Start()
	{
		base.healthHaver.OnDamaged += this.OnDamaged;
		if (this.specifyAnimator)
		{
			this.m_bodyPart = this.specifyAnimator.GetComponent<BodyPartController>();
		}
	}

	// Token: 0x060056B4 RID: 22196 RVA: 0x00210950 File Offset: 0x0020EB50
	public void Update()
	{
	}

	// Token: 0x060056B5 RID: 22197 RVA: 0x00210954 File Offset: 0x0020EB54
	public void LateUpdate()
	{
		if (this.m_laserBeam && this.MaxBeamLength > 0f)
		{
			this.m_laserBeam.projectile.baseData.range = this.MaxBeamLength;
			this.m_laserBeam.ShowImpactOnMaxDistanceEnd = true;
		}
		if (this.m_firingLaser && this.m_laserBeam)
		{
			this.m_laserBeam.LateUpdatePosition(this.GetTrueLaserOrigin());
		}
		else if (this.m_laserBeam && this.m_laserBeam.State == BasicBeamController.BeamState.Dissipating)
		{
			this.m_laserBeam.LateUpdatePosition(this.GetTrueLaserOrigin());
		}
		else if (this.m_firingLaser && !this.m_laserBeam)
		{
			this.StopFiringLaser();
		}
	}

	// Token: 0x060056B6 RID: 22198 RVA: 0x00210A34 File Offset: 0x0020EC34
	protected override void OnDestroy()
	{
		if (this.m_laserBeam)
		{
			this.m_laserBeam.CeaseAttack();
		}
		base.OnDestroy();
	}

	// Token: 0x060056B7 RID: 22199 RVA: 0x00210A58 File Offset: 0x0020EC58
	public void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (resultValue <= 0f)
		{
			if (this.m_firingLaser)
			{
				this.chargeVfx.DestroyAll();
				this.StopFiringLaser();
			}
			if (this.m_laserBeam)
			{
				this.m_laserBeam.DestroyBeam();
				this.m_laserBeam = null;
			}
		}
	}

	// Token: 0x060056B8 RID: 22200 RVA: 0x00210AB0 File Offset: 0x0020ECB0
	public void StartFiringLaser(float laserAngle)
	{
		this.m_firingLaser = true;
		this.LaserAngle = laserAngle;
		if (this.m_bodyPart)
		{
			this.m_bodyPart.OverrideFacingDirection = true;
		}
		if (!string.IsNullOrEmpty(this.shootAnim))
		{
			this.CurrentAiAnimator.LockFacingDirection = true;
			this.CurrentAiAnimator.PlayUntilCancelled(this.shootAnim, true, null, -1f, false);
		}
		this.chargeVfx.DestroyAll();
		base.StartCoroutine(this.FireBeam((!this.beamProjectile) ? this.beamModule.GetCurrentProjectile() : this.beamProjectile));
	}

	// Token: 0x060056B9 RID: 22201 RVA: 0x00210B5C File Offset: 0x0020ED5C
	public void StopFiringLaser()
	{
		this.m_firingLaser = false;
		if (this.m_bodyPart)
		{
			this.m_bodyPart.OverrideFacingDirection = false;
		}
		if (!string.IsNullOrEmpty(this.shootAnim))
		{
			this.CurrentAiAnimator.LockFacingDirection = false;
			this.CurrentAiAnimator.EndAnimationIf(this.shootAnim);
		}
	}

	// Token: 0x060056BA RID: 22202 RVA: 0x00210BBC File Offset: 0x0020EDBC
	protected IEnumerator FireBeam(Projectile projectile)
	{
		GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(projectile.gameObject);
		this.m_laserBeam = beamObject.GetComponent<BasicBeamController>();
		this.m_laserBeam.Owner = base.aiActor;
		this.m_laserBeam.HitsPlayers = projectile.collidesWithPlayer || (!this.IgnoreAiActorPlayerChecks && base.aiActor && base.aiActor.CanTargetPlayers);
		this.m_laserBeam.HitsEnemies = projectile.collidesWithEnemies || (base.aiActor && base.aiActor.CanTargetEnemies);
		bool facingNorth = BraveMathCollege.AbsAngleBetween(this.LaserAngle, 90f) < this.northAngleTolerance;
		this.m_laserBeam.HeightOffset = this.heightOffset;
		this.m_laserBeam.RampHeightOffset = ((!facingNorth) ? this.otherRampHeight : this.northRampHeight);
		this.m_laserBeam.ContinueBeamArtToWall = !this.PreventBeamContinuation;
		bool firstFrame = true;
		while (this.m_laserBeam != null && this.m_firingLaser)
		{
			float clampedAngle = BraveMathCollege.ClampAngle360(this.LaserAngle);
			Vector2 dirVec = new Vector3(Mathf.Cos(clampedAngle * 0.017453292f), Mathf.Sin(clampedAngle * 0.017453292f)) * 10f;
			this.m_laserBeam.Origin = this.GetTrueLaserOrigin();
			this.m_laserBeam.Direction = dirVec;
			if (firstFrame)
			{
				yield return null;
				firstFrame = false;
			}
			else
			{
				facingNorth = BraveMathCollege.AbsAngleBetween(this.LaserAngle, 90f) < this.northAngleTolerance;
				this.m_laserBeam.RampHeightOffset = ((!facingNorth) ? this.otherRampHeight : this.northRampHeight);
				yield return null;
				while (Time.timeScale == 0f)
				{
					yield return null;
				}
			}
		}
		if (!this.m_firingLaser && this.m_laserBeam != null)
		{
			this.m_laserBeam.CeaseAttack();
		}
		if (this.TurnDuringDissipation && this.m_laserBeam)
		{
			this.m_laserBeam.SelfUpdate = false;
			while (this.m_laserBeam)
			{
				this.m_laserBeam.Origin = this.GetTrueLaserOrigin();
				yield return null;
			}
		}
		this.m_laserBeam = null;
		yield break;
	}

	// Token: 0x060056BB RID: 22203 RVA: 0x00210BE0 File Offset: 0x0020EDE0
	private Vector3 GetTrueLaserOrigin()
	{
		Vector2 vector = this.LaserFiringCenter;
		if (this.firingEllipseA != 0f && this.firingEllipseB != 0f)
		{
			float num = Mathf.Lerp(this.eyeballFudgeAngle, 0f, BraveMathCollege.AbsAngleBetween(90f, Mathf.Abs(BraveMathCollege.ClampAngle180(this.LaserAngle))) / 90f);
			vector = BraveMathCollege.GetEllipsePoint(vector, this.firingEllipseA, this.firingEllipseB, this.LaserAngle + num);
		}
		return vector;
	}

	// Token: 0x04004F98 RID: 20376
	public string shootAnim;

	// Token: 0x04004F99 RID: 20377
	public AIAnimator specifyAnimator;

	// Token: 0x04004F9A RID: 20378
	[Header("Beam Data")]
	public Transform beamTransform;

	// Token: 0x04004F9B RID: 20379
	public VFXPool chargeVfx;

	// Token: 0x04004F9C RID: 20380
	public Projectile beamProjectile;

	// Token: 0x04004F9D RID: 20381
	[HideInInspectorIf("beamProjectile", false)]
	public ProjectileModule beamModule;

	// Token: 0x04004F9E RID: 20382
	public bool TurnDuringDissipation = true;

	// Token: 0x04004F9F RID: 20383
	public bool PreventBeamContinuation;

	// Token: 0x04004FA0 RID: 20384
	[Header("Depth")]
	public float heightOffset = 1.9f;

	// Token: 0x04004FA1 RID: 20385
	public float northAngleTolerance = 90f;

	// Token: 0x04004FA2 RID: 20386
	public float northRampHeight;

	// Token: 0x04004FA3 RID: 20387
	public float otherRampHeight = 5f;

	// Token: 0x04004FA4 RID: 20388
	[Header("Beam Firing Point")]
	public Vector2 firingEllipseCenter;

	// Token: 0x04004FA5 RID: 20389
	public float firingEllipseA;

	// Token: 0x04004FA6 RID: 20390
	public float firingEllipseB;

	// Token: 0x04004FA7 RID: 20391
	public float eyeballFudgeAngle;

	// Token: 0x04004FAA RID: 20394
	private bool m_firingLaser;

	// Token: 0x04004FAB RID: 20395
	private float m_laserAngle;

	// Token: 0x04004FAC RID: 20396
	private BasicBeamController m_laserBeam;

	// Token: 0x04004FAD RID: 20397
	private BodyPartController m_bodyPart;
}
