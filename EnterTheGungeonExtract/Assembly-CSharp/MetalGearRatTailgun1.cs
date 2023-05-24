using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000281 RID: 641
[InspectorDropdownName("Bosses/MetalGearRat/Tailgun1")]
public class MetalGearRatTailgun1 : Script
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0002F8C4 File Offset: 0x0002DAC4
	// (set) Token: 0x060009C8 RID: 2504 RVA: 0x0002F8CC File Offset: 0x0002DACC
	private bool Center { get; set; }

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0002F8D8 File Offset: 0x0002DAD8
	// (set) Token: 0x060009CA RID: 2506 RVA: 0x0002F8E0 File Offset: 0x0002DAE0
	private bool Done { get; set; }

	// Token: 0x060009CB RID: 2507 RVA: 0x0002F8EC File Offset: 0x0002DAEC
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		MetalGearRatTailgun1.TargetDummy targetDummy = new MetalGearRatTailgun1.TargetDummy();
		targetDummy.Position = base.BulletBank.aiActor.ParentRoom.area.UnitCenter + new Vector2(0f, 4.5f);
		targetDummy.Direction = base.AimDirection;
		targetDummy.BulletManager = this.BulletManager;
		targetDummy.Initialize();
		for (int j = 0; j < 16; j++)
		{
			float num = base.SubdivideCircle(0f, 16, j, 1f, false);
			Vector2 vector = targetDummy.Position + BraveMathCollege.DegreesToVector(num, 0.75f);
			base.Fire(Offset.OverridePosition(vector), new MetalGearRatTailgun1.TargetBullet(this, targetDummy));
		}
		base.Fire(Offset.OverridePosition(targetDummy.Position), new MetalGearRatTailgun1.TargetBullet(this, targetDummy));
		for (int k = 0; k < 4; k++)
		{
			float num2 = (float)(k * 90);
			for (int l = 1; l < 4; l++)
			{
				float num3 = 0.75f + Mathf.Lerp(0f, 0.625f, (float)l / 3f);
				Vector2 vector2 = targetDummy.Position + BraveMathCollege.DegreesToVector(num2, num3);
				base.Fire(Offset.OverridePosition(vector2), new MetalGearRatTailgun1.TargetBullet(this, targetDummy));
			}
		}
		for (int i = 0; i < 360; i++)
		{
			targetDummy.DoTick();
			yield return base.Wait(1);
		}
		base.Fire(Offset.OverridePosition(targetDummy.Position + new Vector2(0f, 30f)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new MetalGearRatTailgun1.BigBullet());
		base.PostWwiseEvent("Play_BOSS_RatMech_Whistle_01", null);
		this.Center = true;
		yield return base.Wait(60);
		this.Done = true;
		yield return base.Wait(60);
		yield break;
	}

	// Token: 0x04000A1E RID: 2590
	private const int NumTargetBullets = 16;

	// Token: 0x04000A1F RID: 2591
	private const float TargetRadius = 3f;

	// Token: 0x04000A20 RID: 2592
	private const float TargetLegLength = 2.5f;

	// Token: 0x04000A21 RID: 2593
	public const int TargetTrackTime = 360;

	// Token: 0x04000A22 RID: 2594
	private const float TargetRotationSpeed = 80f;

	// Token: 0x04000A23 RID: 2595
	private const int BigOneHeight = 30;

	// Token: 0x04000A24 RID: 2596
	private const int NumDeathWaves = 4;

	// Token: 0x04000A25 RID: 2597
	private const int NumDeathBullets = 39;

	// Token: 0x02000282 RID: 642
	public class TargetDummy : Bullet
	{
		// Token: 0x060009CC RID: 2508 RVA: 0x0002F908 File Offset: 0x0002DB08
		public TargetDummy()
			: base(null, false, false, false)
		{
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002F914 File Offset: 0x0002DB14
		protected override IEnumerator Top()
		{
			for (;;)
			{
				float distToTarget = (this.BulletManager.PlayerPosition() - base.Position).magnitude;
				if (base.Tick < 30)
				{
					this.Speed = 0f;
				}
				else
				{
					float num = Mathf.Lerp(12f, 4f, Mathf.InverseLerp(7f, 4f, distToTarget));
					this.Speed = Mathf.Min(num, (float)(base.Tick - 30) / 60f * 10f);
				}
				base.ChangeDirection(new Direction(0f, DirectionType.Aim, 3f), 1);
				yield return base.Wait(1);
			}
			yield break;
		}
	}

	// Token: 0x02000284 RID: 644
	public class TargetBullet : Bullet
	{
		// Token: 0x060009D4 RID: 2516 RVA: 0x0002FA8C File Offset: 0x0002DC8C
		public TargetBullet(MetalGearRatTailgun1 parent, MetalGearRatTailgun1.TargetDummy targetDummy)
			: base("target", false, false, false)
		{
			this.m_parent = parent;
			this.m_targetDummy = targetDummy;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002FAAC File Offset: 0x0002DCAC
		protected override IEnumerator Top()
		{
			Vector2 toCenter = base.Position - this.m_targetDummy.Position;
			float angle = toCenter.ToAngle();
			float radius = toCenter.magnitude;
			float deltaRadius = radius / 60f;
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.LowObstacle));
			while (!this.m_parent.Destroyed && !this.m_parent.IsEnded && !this.m_parent.Done)
			{
				if (base.Tick < 60)
				{
					radius += deltaRadius * 3f;
				}
				if (this.m_parent.Center)
				{
					radius -= deltaRadius * 2f;
				}
				angle += 1.3333334f;
				base.Position = this.m_targetDummy.Position + BraveMathCollege.DegreesToVector(angle, radius);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			base.PostWwiseEvent("Play_BOSS_RatMech_Bomb_01", null);
			yield break;
		}

		// Token: 0x04000A2D RID: 2605
		private MetalGearRatTailgun1 m_parent;

		// Token: 0x04000A2E RID: 2606
		private MetalGearRatTailgun1.TargetDummy m_targetDummy;
	}

	// Token: 0x02000286 RID: 646
	private class BigBullet : Bullet
	{
		// Token: 0x060009DC RID: 2524 RVA: 0x0002FCF0 File Offset: 0x0002DEF0
		public BigBullet()
			: base("big_one", false, false, false)
		{
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0002FD00 File Offset: 0x0002DF00
		public override void Initialize()
		{
			this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
			base.Initialize();
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0002FD18 File Offset: 0x0002DF18
		protected override IEnumerator Top()
		{
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.specRigidbody.CollideWithOthers = false;
			yield return base.Wait(60);
			this.Speed = 0f;
			this.Projectile.spriteAnimator.Play();
			float startingAngle = base.RandomAngle();
			for (int i = 0; i < 4; i++)
			{
				bool flag = i % 2 == 0;
				for (int j = 0; j < 39; j++)
				{
					float num = startingAngle;
					int num2 = 39;
					int num3 = j;
					bool flag2 = flag;
					float num4 = base.SubdivideCircle(num, num2, num3, 1f, flag2);
					base.Fire(new Direction(num4, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedChangingBullet(10f, 17 * i, -1));
				}
			}
			yield return base.Wait(30);
			base.Vanish(true);
			yield break;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0002FD34 File Offset: 0x0002DF34
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
		}
	}
}
