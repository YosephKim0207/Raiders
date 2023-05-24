using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000277 RID: 631
[InspectorDropdownName("Bosses/MetalGearRat/Spinners1")]
public class MetalGearRatSpinners1 : Script
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x0002E240 File Offset: 0x0002C440
	// (set) Token: 0x06000994 RID: 2452 RVA: 0x0002E248 File Offset: 0x0002C448
	private Vector2 CenterPoint { get; set; }

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x0002E254 File Offset: 0x0002C454
	// (set) Token: 0x06000996 RID: 2454 RVA: 0x0002E25C File Offset: 0x0002C45C
	private bool Done { get; set; }

	// Token: 0x06000997 RID: 2455 RVA: 0x0002E268 File Offset: 0x0002C468
	protected override IEnumerator Top()
	{
		yield return base.Wait(30);
		base.EndOnBlank = true;
		this.CenterPoint = base.BulletBank.aiActor.ParentRoom.area.UnitCenter + new Vector2(0f, 4.5f);
		this.m_circleDummies.Clear();
		base.StartTask(this.SpawnInnerRing());
		base.StartTask(this.SpawnOuterRing());
		int spinTime = 960;
		for (int i = 0; i < spinTime; i++)
		{
			for (int j = 0; j < this.m_circleDummies.Count; j++)
			{
				this.m_circleDummies[j].DoTick();
			}
			if (i == spinTime - 60)
			{
				this.Done = true;
			}
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0002E284 File Offset: 0x0002C484
	private IEnumerator SpawnInnerRing()
	{
		int spinTime = 500;
		int numCircles = 3;
		float initialRadiusBoost = 12f;
		float orbitSpeed = 360f / ((float)spinTime / 60f);
		float radius = 7.5f;
		float rotationSpeed = -45f;
		this.SpawnCircleDebris(radius, 140f, 0f, 0.35f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 130f, 0f, 0.37f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 120f, 0f, 0.31f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 110f, 0f, 0.23f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 100f, 0f, 0.27f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 90f, 0f, 0.29f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 80f, 0f, 0.27f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 70f, 0f, 0.23f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 60f, 0f, 0.31f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 50f, 0f, 0.37f, 0f, new float?(initialRadiusBoost));
		this.SpawnCircleDebris(radius, 40f, 0f, 0.35f, 0f, new float?(initialRadiusBoost));
		for (int i = 0; i < numCircles; i++)
		{
			float spawnAngle = 90f;
			this.SpawnCircle(radius, spawnAngle, orbitSpeed, rotationSpeed, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.95f, -50f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.95f, -30f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.95f, 30f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.95f, 50f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.78f, -40f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.78f, 60f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.78f, 40f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.6f, -50f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.6f, 50f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.45f, 60f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, spawnAngle, orbitSpeed, 0.25f, 60f, new float?(initialRadiusBoost));
			yield return base.Wait(spinTime / numCircles);
		}
		yield break;
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0002E2A0 File Offset: 0x0002C4A0
	private IEnumerator SpawnOuterRing()
	{
		int spinTime = 1100;
		int numCircles = 8;
		float initialRadiusBoost = 12f;
		float orbitSpeed = 360f / ((float)spinTime / 60f);
		float radius = 18.5f;
		float rotationSpeed = 45f;
		float deltaAngle = 0f;
		for (int i = 0; i < numCircles; i++)
		{
			float num = deltaAngle;
			this.SpawnCircle(radius, num, -orbitSpeed, rotationSpeed, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0f, -17.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0f, 17.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0.15f, 22.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0.3f, 22.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0.45f, 22.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0.6f, 22.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0.75f, -19f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0.75f, 19f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 0.9f, 22.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 1.05f, -17.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 1.05f, 17.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 1.2f, 22.5f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 1.2f, 0f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 1.35f, -15f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 1.35f, 15f, new float?(initialRadiusBoost));
			this.SpawnCircleDebris(radius, num, -orbitSpeed, 1.5f, 22.5f, new float?(initialRadiusBoost));
			deltaAngle += 360f / (float)numCircles;
		}
		yield return base.Wait(1);
		yield break;
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0002E2BC File Offset: 0x0002C4BC
	private void SpawnCircle(float spawnRadius, float spawnAngle, float orbitSpeed, float rotationSpeed, float? initialRadiusBoost = null)
	{
		float num = spawnRadius + ((initialRadiusBoost == null) ? 0f : initialRadiusBoost.Value);
		Vector2 vector = this.CenterPoint + BraveMathCollege.DegreesToVector(spawnAngle, num);
		MetalGearRatSpinners1.CircleDummy circleDummy = new MetalGearRatSpinners1.CircleDummy(this, this.CenterPoint, spawnRadius, spawnAngle, orbitSpeed, initialRadiusBoost);
		circleDummy.Position = vector;
		circleDummy.Direction = base.AimDirection;
		circleDummy.BulletManager = this.BulletManager;
		circleDummy.Initialize();
		this.m_circleDummies.Add(circleDummy);
		for (int i = 0; i < 21; i++)
		{
			float num2 = base.SubdivideCircle(0f, 21, i, 1f, false);
			Vector2 vector2 = vector + BraveMathCollege.DegreesToVector(num2, 5f);
			base.Fire(Offset.OverridePosition(vector2), new MetalGearRatSpinners1.CircleBullet(this, circleDummy, rotationSpeed, i, vector2 - vector));
		}
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0002E39C File Offset: 0x0002C59C
	private void SpawnCircleDebris(float spawnRadius, float spawnAngle, float orbitSpeed, float tRadius, float deltaAngle, float? initialRadiusBoost = null)
	{
		float num = spawnAngle + deltaAngle;
		float num2 = Mathf.LerpUnclamped(spawnRadius - 5f, spawnRadius + 5f, tRadius);
		Vector2 vector = this.CenterPoint + BraveMathCollege.DegreesToVector(num, num2 + ((initialRadiusBoost == null) ? 0f : initialRadiusBoost.Value));
		base.Fire(Offset.OverridePosition(vector), new MetalGearRatSpinners1.OrbitBullet(this, num2, num, orbitSpeed, initialRadiusBoost));
	}

	// Token: 0x040009D4 RID: 2516
	private const int BulletsPerCircle = 21;

	// Token: 0x040009D5 RID: 2517
	private const float CircleRadius = 5f;

	// Token: 0x040009D6 RID: 2518
	private const int NearTimeForAttack = 100;

	// Token: 0x040009D9 RID: 2521
	private List<MetalGearRatSpinners1.CircleDummy> m_circleDummies = new List<MetalGearRatSpinners1.CircleDummy>();

	// Token: 0x02000278 RID: 632
	public class CircleDummy : Bullet
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0002E40C File Offset: 0x0002C60C
		public CircleDummy(MetalGearRatSpinners1 parent, Vector2 centerPoint, float centerRadius, float centerAngle, float orbitSpeed, float? initialRadiusBoostBoost = null)
			: base("spinner", false, false, false)
		{
			this.m_parent = parent;
			this.m_centerPoint = centerPoint;
			this.m_centerRadius = centerRadius;
			this.m_centerAngle = centerAngle;
			this.m_orbitSpeed = orbitSpeed;
			this.m_initialRadiusBoost = initialRadiusBoostBoost;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002E45C File Offset: 0x0002C65C
		protected override IEnumerator Top()
		{
			float radius = this.m_centerRadius;
			base.ManualControl = true;
			for (;;)
			{
				float? initialRadiusBoost = this.m_initialRadiusBoost;
				if (initialRadiusBoost != null && base.Tick <= 60)
				{
					radius = this.m_centerRadius + Mathf.Lerp(this.m_initialRadiusBoost.Value, 0f, (float)base.Tick / 60f);
				}
				else
				{
					this.m_centerAngle += this.m_orbitSpeed / 60f;
				}
				base.Position = this.m_centerPoint + BraveMathCollege.DegreesToVector(this.m_centerAngle, radius);
				float playerDist = (this.BulletManager.PlayerPosition() - base.Position).magnitude;
				if (playerDist < 5.5f || this.NearTime < 0)
				{
					this.NearTime++;
				}
				else
				{
					this.NearTime = Mathf.Max(0, this.NearTime - 2);
				}
				if (this.NearTime >= 100)
				{
					this.FireTick = base.Tick;
					this.NearTime = -60;
				}
				yield return base.Wait(1);
			}
			yield break;
		}

		// Token: 0x040009DA RID: 2522
		public int NearTime;

		// Token: 0x040009DB RID: 2523
		public int FireTick = -1;

		// Token: 0x040009DC RID: 2524
		private MetalGearRatSpinners1 m_parent;

		// Token: 0x040009DD RID: 2525
		private MetalGearRatSpinners1.CircleDummy m_circleDummy;

		// Token: 0x040009DE RID: 2526
		private Vector2 m_centerPoint;

		// Token: 0x040009DF RID: 2527
		private float m_centerRadius;

		// Token: 0x040009E0 RID: 2528
		private float m_centerAngle;

		// Token: 0x040009E1 RID: 2529
		private float m_orbitSpeed;

		// Token: 0x040009E2 RID: 2530
		private float? m_initialRadiusBoost;
	}

	// Token: 0x0200027A RID: 634
	public class CircleBullet : Bullet
	{
		// Token: 0x060009A4 RID: 2468 RVA: 0x0002E6A8 File Offset: 0x0002C8A8
		public CircleBullet(MetalGearRatSpinners1 parent, MetalGearRatSpinners1.CircleDummy circleDummy, float rotationSpeed, int index, Vector2 offset)
			: base("spinner", false, false, false)
		{
			this.m_parent = parent;
			this.m_circleDummy = circleDummy;
			this.m_rotationSpeed = rotationSpeed;
			this.m_index = index;
			this.m_offset = offset;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0002E6E0 File Offset: 0x0002C8E0
		protected override IEnumerator Top()
		{
			this.Projectile.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.LowObstacle));
			int remainingLife = -1;
			bool isWarning = false;
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			while (!this.m_parent.Destroyed && remainingLife != 0)
			{
				if (this.m_parent.IsEnded || this.m_parent.Done)
				{
					if (remainingLife < 0)
					{
						remainingLife = UnityEngine.Random.Range(0, 60);
					}
					else
					{
						remainingLife--;
					}
				}
				this.m_offset = this.m_offset.Rotate(this.m_rotationSpeed / 60f);
				base.Position = this.m_circleDummy.Position + this.m_offset;
				if (this.m_circleDummy.FireTick == base.Tick && remainingLife < 0)
				{
					Vector2 vector = this.m_circleDummy.Position - base.Position;
					TimedBullet timedBullet = new TimedBullet(30);
					base.Fire(new Direction(vector.ToAngle(), DirectionType.Absolute, -1f), new Speed(vector.magnitude * 2f, SpeedType.Absolute), timedBullet);
					timedBullet.Projectile.IgnoreTileCollisionsFor(1f);
				}
				bool shouldWarn = false;
				if (this.m_circleDummy.NearTime > 0 && remainingLife < 0)
				{
					shouldWarn = this.m_circleDummy.NearTime + 30 >= 100;
				}
				if (shouldWarn && !isWarning)
				{
					tk2dSpriteAnimationClip defaultClip = this.Projectile.spriteAnimator.DefaultClip;
					float num = (float)this.m_circleDummy.NearTime / 60f % defaultClip.BaseClipLength;
					this.Projectile.spriteAnimator.Play(defaultClip, num, defaultClip.fps, false);
					isWarning = true;
				}
				else if (!shouldWarn && isWarning)
				{
					this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
					isWarning = false;
				}
				yield return base.Wait(1);
			}
			base.Vanish(true);
			yield break;
			yield break;
		}

		// Token: 0x040009E9 RID: 2537
		private MetalGearRatSpinners1 m_parent;

		// Token: 0x040009EA RID: 2538
		private MetalGearRatSpinners1.CircleDummy m_circleDummy;

		// Token: 0x040009EB RID: 2539
		private float m_rotationSpeed;

		// Token: 0x040009EC RID: 2540
		private int m_index;

		// Token: 0x040009ED RID: 2541
		private Vector2 m_offset;
	}

	// Token: 0x0200027C RID: 636
	public class OrbitBullet : Bullet
	{
		// Token: 0x060009AC RID: 2476 RVA: 0x0002EA5C File Offset: 0x0002CC5C
		public OrbitBullet(MetalGearRatSpinners1 parent, float radius, float angle, float orbitSpeed, float? initialRadiusBoost = null)
			: base("spinner", false, false, false)
		{
			this.m_parent = parent;
			this.m_radius = radius;
			this.m_angle = angle;
			this.m_orbitSpeed = orbitSpeed;
			this.m_initialRadiusBoost = initialRadiusBoost;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002EA94 File Offset: 0x0002CC94
		protected override IEnumerator Top()
		{
			this.Projectile.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.LowObstacle));
			float radius = this.m_radius;
			int remainingLife = -1;
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			while (!this.m_parent.Destroyed && remainingLife != 0)
			{
				if (this.m_parent.IsEnded || this.m_parent.Done)
				{
					if (remainingLife < 0)
					{
						remainingLife = UnityEngine.Random.Range(0, 60);
					}
					else
					{
						remainingLife--;
					}
				}
				float? initialRadiusBoost = this.m_initialRadiusBoost;
				if (initialRadiusBoost != null && base.Tick <= 60)
				{
					radius = this.m_radius + Mathf.Lerp(this.m_initialRadiusBoost.Value, 0f, (float)base.Tick / 60f);
				}
				else
				{
					this.m_angle += this.m_orbitSpeed / 60f;
				}
				base.Position = this.m_parent.CenterPoint + BraveMathCollege.DegreesToVector(this.m_angle, radius + Mathf.SmoothStep(-0.25f, 0.25f, Mathf.PingPong((float)base.Tick, 30f) / 30f));
				yield return base.Wait(1);
			}
			base.Vanish(true);
			yield break;
			yield break;
		}

		// Token: 0x040009F5 RID: 2549
		private MetalGearRatSpinners1 m_parent;

		// Token: 0x040009F6 RID: 2550
		private Vector2 m_centerPoint;

		// Token: 0x040009F7 RID: 2551
		private float m_radius;

		// Token: 0x040009F8 RID: 2552
		private float m_angle;

		// Token: 0x040009F9 RID: 2553
		private float m_orbitSpeed;

		// Token: 0x040009FA RID: 2554
		private float? m_initialRadiusBoost;
	}
}
