using System;
using System.Collections;
using FullInspector;
using UnityEngine;

// Token: 0x02000FE8 RID: 4072
public class BossFinalRogueLaserGun : BossFinalRogueGunController
{
	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x060058D3 RID: 22739 RVA: 0x0021EAB8 File Offset: 0x0021CCB8
	// (set) Token: 0x060058D4 RID: 22740 RVA: 0x0021EAC0 File Offset: 0x0021CCC0
	public float LaserAngle
	{
		get
		{
			return this.m_laserAngle;
		}
		set
		{
			this.m_laserAngle = value;
			base.transform.rotation = Quaternion.Euler(0f, 0f, this.m_laserAngle + 90f);
		}
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x0021EAF0 File Offset: 0x0021CCF0
	public override void Start()
	{
		base.Start();
		this.ship.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x060058D6 RID: 22742 RVA: 0x0021EB14 File Offset: 0x0021CD14
	public override void Update()
	{
		base.Update();
		if (this.m_fireTimer > 0f)
		{
			this.m_fireTimer -= BraveTime.DeltaTime;
			if (this.m_fireTimer <= 0f)
			{
				this.m_firingLaser = false;
			}
		}
	}

	// Token: 0x060058D7 RID: 22743 RVA: 0x0021EB60 File Offset: 0x0021CD60
	public void LateUpdate()
	{
		if (this.m_firingLaser && this.m_laserBeam)
		{
			this.m_laserBeam.LateUpdatePosition(this.beamTransform.position);
		}
		else if (this.m_laserBeam && this.m_laserBeam.State == BasicBeamController.BeamState.Dissipating)
		{
			this.m_laserBeam.LateUpdatePosition(this.beamTransform.position);
		}
	}

	// Token: 0x060058D8 RID: 22744 RVA: 0x0021EBDC File Offset: 0x0021CDDC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060058D9 RID: 22745 RVA: 0x0021EBE4 File Offset: 0x0021CDE4
	public override void Fire()
	{
		this.m_firingLaser = true;
		this.m_fireTimer = this.fireTime;
		this.LaserAngle = -90f;
		if (this.LightToTrigger)
		{
			this.LightToTrigger.ManuallyDoBulletSpawnedFade();
		}
		base.StartCoroutine(this.FireBeam(this.beamProjectile));
		base.StartCoroutine(this.DoGunMotionCR());
		if (this.doScreenShake)
		{
			GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.screenShake, this, false);
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x060058DA RID: 22746 RVA: 0x0021EC6C File Offset: 0x0021CE6C
	public override bool IsFinished
	{
		get
		{
			return !this.m_firingLaser && !this.m_laserBeam;
		}
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x0021EC8C File Offset: 0x0021CE8C
	public override void CeaseFire()
	{
		if (this.LightToTrigger)
		{
			this.LightToTrigger.EndEarly();
		}
		this.m_firingLaser = false;
		if (this.doScreenShake)
		{
			GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
		}
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x0021ECCC File Offset: 0x0021CECC
	public void OnPreDeath(Vector2 deathDir)
	{
		this.m_firingLaser = false;
		if (this.m_laserBeam)
		{
			this.m_laserBeam.DestroyBeam();
			this.m_laserBeam = null;
		}
	}

	// Token: 0x060058DD RID: 22749 RVA: 0x0021ECF8 File Offset: 0x0021CEF8
	private IEnumerator DoGunMotionCR()
	{
		float elapsed = 0f;
		float duration = this.sweepAwayTime;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += BraveTime.DeltaTime;
			this.LaserAngle = -90f + Mathf.SmoothStep(0f, this.sweepAngle, elapsed / duration);
			base.sprite.UpdateZDepth();
		}
		elapsed = 0f;
		duration = this.sweepBackTime;
		while (elapsed < duration)
		{
			yield return null;
			elapsed += BraveTime.DeltaTime;
			this.LaserAngle = -90f + Mathf.SmoothStep(this.sweepAngle, 0f, elapsed / duration);
			base.sprite.UpdateZDepth();
		}
		yield break;
	}

	// Token: 0x060058DE RID: 22750 RVA: 0x0021ED14 File Offset: 0x0021CF14
	protected IEnumerator FireBeam(Projectile projectile)
	{
		GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(projectile.gameObject);
		this.m_laserBeam = beamObject.GetComponent<BasicBeamController>();
		this.m_laserBeam.Owner = this.ship.aiActor;
		this.m_laserBeam.HitsPlayers = projectile.collidesWithPlayer;
		this.m_laserBeam.HitsEnemies = projectile.collidesWithEnemies;
		this.m_laserBeam.ContinueBeamArtToWall = true;
		bool firstFrame = true;
		while (this.m_laserBeam != null && this.m_firingLaser)
		{
			float clampedAngle = BraveMathCollege.ClampAngle360(this.LaserAngle);
			Vector2 dirVec = new Vector3(Mathf.Cos(clampedAngle * 0.017453292f), Mathf.Sin(clampedAngle * 0.017453292f)) * 10f;
			this.m_laserBeam.Origin = this.beamTransform.position;
			this.m_laserBeam.Direction = dirVec;
			if (firstFrame)
			{
				yield return null;
				firstFrame = false;
			}
			else
			{
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
		if (this.m_laserBeam)
		{
			this.m_laserBeam.SelfUpdate = false;
			while (this.m_laserBeam)
			{
				this.m_laserBeam.Origin = this.beamTransform.position;
				yield return null;
			}
		}
		this.m_laserBeam = null;
		yield break;
	}

	// Token: 0x040051F2 RID: 20978
	[InspectorHeader("Beam Stuff")]
	public Transform beamTransform;

	// Token: 0x040051F3 RID: 20979
	public Projectile beamProjectile;

	// Token: 0x040051F4 RID: 20980
	public float fireTime = 6f;

	// Token: 0x040051F5 RID: 20981
	public bool doScreenShake;

	// Token: 0x040051F6 RID: 20982
	[InspectorShowIf("doScreenShake")]
	public ScreenShakeSettings screenShake;

	// Token: 0x040051F7 RID: 20983
	[InspectorHeader("Gun Motion")]
	public float sweepAngle;

	// Token: 0x040051F8 RID: 20984
	public float sweepAwayTime;

	// Token: 0x040051F9 RID: 20985
	public float sweepBackTime;

	// Token: 0x040051FA RID: 20986
	public AdditionalBraveLight LightToTrigger;

	// Token: 0x040051FB RID: 20987
	private bool m_firingLaser;

	// Token: 0x040051FC RID: 20988
	private float m_laserAngle;

	// Token: 0x040051FD RID: 20989
	private float m_fireTimer;

	// Token: 0x040051FE RID: 20990
	private BasicBeamController m_laserBeam;
}
