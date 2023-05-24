using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200006D RID: 109
[InspectorDropdownName("Bosses/BossFinalBullet/GunonRing1")]
public class BossFinalBulletGunonRing1 : Script
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060001A8 RID: 424 RVA: 0x000087C8 File Offset: 0x000069C8
	// (set) Token: 0x060001A9 RID: 425 RVA: 0x000087D0 File Offset: 0x000069D0
	private float Radius { get; set; }

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060001AA RID: 426 RVA: 0x000087DC File Offset: 0x000069DC
	// (set) Token: 0x060001AB RID: 427 RVA: 0x000087E4 File Offset: 0x000069E4
	private bool Done { get; set; }

	// Token: 0x060001AC RID: 428 RVA: 0x000087F0 File Offset: 0x000069F0
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		yield return base.Wait(36);
		float startAngle = 90f;
		this.Radius = 4f;
		bool facingRight = BraveMathCollege.AbsAngleBetween(base.BulletBank.aiAnimator.FacingDirection, 0f) < 90f;
		this.SpinDirection = (float)((!facingRight) ? 1 : (-1));
		for (int i = 0; i < 8; i++)
		{
			float angle = base.SubdivideCircle(startAngle, 8, i, -this.SpinDirection, false);
			angle += this.SpinDirection * 180f / 60f * (float)(i * 20);
			base.Fire(new Offset(new Vector2(0.75f, 0f), angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new BossFinalBulletGunonRing1.RingBullet(angle, i, this));
			yield return base.Wait(20);
		}
		float deltaRadius = 0.083333336f;
		for (int j = 0; j < 60; j++)
		{
			this.Radius += deltaRadius;
			yield return base.Wait(1);
		}
		yield return base.Wait(60);
		for (int k = 0; k < 60; k++)
		{
			this.Radius -= deltaRadius;
			yield return base.Wait(1);
		}
		yield return base.Wait(30);
		this.Done = true;
		yield break;
	}

	// Token: 0x040001B5 RID: 437
	private const int NumBullets = 8;

	// Token: 0x040001B6 RID: 438
	private const float FireRadius = 0.75f;

	// Token: 0x040001B7 RID: 439
	private const float StartRadius = 4f;

	// Token: 0x040001B8 RID: 440
	private const float EndRadius = 9f;

	// Token: 0x040001B9 RID: 441
	private const int ExpandTime = 60;

	// Token: 0x040001BA RID: 442
	private const float SpinSpeed = 180f;

	// Token: 0x040001BD RID: 445
	private float SpinDirection;

	// Token: 0x0200006E RID: 110
	public class RingBullet : Bullet
	{
		// Token: 0x060001AD RID: 429 RVA: 0x0000880C File Offset: 0x00006A0C
		public RingBullet(float angle, int index, BossFinalBulletGunonRing1 parentScript)
			: base(null, false, false, false)
		{
			this.m_angle = angle;
			this.m_index = index;
			this.m_parentScript = parentScript;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008830 File Offset: 0x00006A30
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			Vector2 center = this.m_parentScript.Position;
			float radius = this.m_parentScript.Radius;
			while (!this.m_parentScript.Destroyed && !this.m_parentScript.Done)
			{
				this.m_angle += this.m_parentScript.SpinDirection * 180f / 60f;
				radius = this.m_parentScript.Radius;
				if (base.Tick <= 20)
				{
					radius = Mathf.Lerp(0.75f, 4f, (float)base.Tick / 20f);
				}
				base.Position = center + BraveMathCollege.DegreesToVector(this.m_angle, radius);
				yield return base.Wait(1);
			}
			int spinTime = this.m_index * 20;
			for (int i = 0; i < spinTime; i++)
			{
				this.m_angle += this.m_parentScript.SpinDirection * 30f / 60f;
				radius += 0.016666668f;
				base.Position = center + BraveMathCollege.DegreesToVector(this.m_angle, radius);
				yield return base.Wait(1);
			}
			if (base.BulletBank && base.BulletBank.aiAnimator)
			{
				AIAnimator aiAnimator = base.BulletBank.aiAnimator;
				string text = "bat_transform";
				Vector2? vector = new Vector2?(base.Position);
				aiAnimator.PlayVfx(text, null, null, vector);
			}
			base.Fire(new BossFinalBulletGunonRing1.BatBullet("bat"));
			base.Vanish(true);
			yield break;
		}

		// Token: 0x040001BE RID: 446
		private const float ReleaseSpinSpeed = 30f;

		// Token: 0x040001BF RID: 447
		private const float ReleaseDriftSpeed = 1f;

		// Token: 0x040001C0 RID: 448
		private float m_angle;

		// Token: 0x040001C1 RID: 449
		private int m_index;

		// Token: 0x040001C2 RID: 450
		private BossFinalBulletGunonRing1 m_parentScript;
	}

	// Token: 0x02000070 RID: 112
	public class BatBullet : Bullet
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00008B70 File Offset: 0x00006D70
		public BatBullet(string name)
			: base(name, false, false, false)
		{
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008B7C File Offset: 0x00006D7C
		protected override IEnumerator Top()
		{
			this.Direction = base.GetAimDirection(base.Position, (float)((!BraveUtility.RandomBool()) ? 1 : 0), 12f);
			base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 20);
			if (base.IsPointInTile(base.Position))
			{
				this.Projectile.IgnoreTileCollisionsFor(1f);
			}
			return null;
		}
	}
}
