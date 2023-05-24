using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000118 RID: 280
[InspectorDropdownName("BulletShotgunMan/ExecutionerDeathBurst1")]
public class BulletShotgunExecutionerManDeathBurst1 : Script
{
	// Token: 0x06000429 RID: 1065 RVA: 0x0001426C File Offset: 0x0001246C
	protected override IEnumerator Top()
	{
		if (base.BulletBank && base.BulletBank.aiActor && base.BulletBank.aiActor.TargetRigidbody)
		{
			this.m_targetRigidbody = base.BulletBank.aiActor.TargetRigidbody;
		}
		else if (GameManager.Instance.BestActivePlayer)
		{
			this.m_targetRigidbody = GameManager.Instance.BestActivePlayer.specRigidbody;
		}
		float deltaAngle = 60f;
		for (int j = 0; j <= 6; j++)
		{
			base.Fire(new Direction((float)j * deltaAngle, DirectionType.Absolute, -1f), new Speed(6.5f, SpeedType.Absolute), new Bullet("flashybullet", false, false, false));
		}
		float angle = UnityEngine.Random.Range(0f, 360f);
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Offset(new Vector2(1f, 0f), angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new BulletShotgunExecutionerManDeathBurst1.RingBullet(angle, this));
			yield return base.Wait(5);
		}
		yield break;
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x0600042A RID: 1066 RVA: 0x00014288 File Offset: 0x00012488
	private float RetargetAngle
	{
		get
		{
			if (this.m_targetRigidbody)
			{
				return (this.m_targetRigidbody.HitboxPixelCollider.UnitCenter - base.Position).ToAngle();
			}
			float? cachedRetargetAngle = this.m_cachedRetargetAngle;
			if (cachedRetargetAngle == null)
			{
				this.m_cachedRetargetAngle = new float?(UnityEngine.Random.Range(0f, 360f));
			}
			return this.m_cachedRetargetAngle.Value;
		}
	}

	// Token: 0x0400041A RID: 1050
	private const int NumInitialBullets = 6;

	// Token: 0x0400041B RID: 1051
	private const int NumRingBullets = 12;

	// Token: 0x0400041C RID: 1052
	private const float SpinSpeed = 540f;

	// Token: 0x0400041D RID: 1053
	private const float FireRadius = 1f;

	// Token: 0x0400041E RID: 1054
	private SpeculativeRigidbody m_targetRigidbody;

	// Token: 0x0400041F RID: 1055
	private float? m_cachedRetargetAngle;

	// Token: 0x02000119 RID: 281
	public class RingBullet : Bullet
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x00014304 File Offset: 0x00012504
		public RingBullet(float angle, BulletShotgunExecutionerManDeathBurst1 parentScript)
			: base("chain", false, false, false)
		{
			this.m_angle = angle;
			this.m_parentScript = parentScript;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00014324 File Offset: 0x00012524
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			Vector2 center = this.m_parentScript.Position;
			for (int i = 0; i < 60; i++)
			{
				this.m_angle += 9f;
				float shownAngle = this.m_angle;
				if (i >= 50)
				{
					shownAngle = Mathf.LerpAngle(this.m_angle, this.m_parentScript.RetargetAngle, (float)(i - 49) / 10f);
				}
				base.Position = center + BraveMathCollege.DegreesToVector(shownAngle, 1f);
				yield return base.Wait(1);
			}
			this.Projectile.specRigidbody.CollideWithTileMap = true;
			this.Direction = this.m_parentScript.RetargetAngle;
			this.Speed = 12f;
			base.ManualControl = false;
			yield break;
		}

		// Token: 0x04000420 RID: 1056
		private float m_angle;

		// Token: 0x04000421 RID: 1057
		private BulletShotgunExecutionerManDeathBurst1 m_parentScript;
	}
}
