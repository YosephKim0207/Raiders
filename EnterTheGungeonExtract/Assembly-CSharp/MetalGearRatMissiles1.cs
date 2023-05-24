using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200026D RID: 621
[InspectorDropdownName("Bosses/MetalGearRat/Missiles1")]
public class MetalGearRatMissiles1 : Script
{
	// Token: 0x06000968 RID: 2408 RVA: 0x0002D648 File Offset: 0x0002B848
	protected override IEnumerator Top()
	{
		int leftDelay = 0;
		int rightDelay = 60;
		if (BraveUtility.RandomBool())
		{
			BraveUtility.Swap<int>(ref leftDelay, ref rightDelay);
		}
		Vector2 leftBasePos = base.BulletBank.GetTransform("missile left shoot point").position;
		Vector2 rightBasePos = base.BulletBank.GetTransform("missile right shoot point").position;
		int[] leftDelays = new int[MetalGearRatMissiles1.xOffsets.Length];
		int[] rightDelays = new int[MetalGearRatMissiles1.xOffsets.Length];
		for (int j = 0; j < MetalGearRatMissiles1.xOffsets.Length; j++)
		{
			leftDelays[j] = 30 + j * 30;
			rightDelays[j] = 30 + j * 30 + 15;
		}
		BraveUtility.RandomizeArray<int>(leftDelays, 0, -1);
		BraveUtility.RandomizeArray<int>(rightDelays, 0, -1);
		for (int i = 0; i < MetalGearRatMissiles1.xOffsets.Length; i++)
		{
			int dx = MetalGearRatMissiles1.xOffsets[MetalGearRatMissiles1.xOffsets.Length - 1 - i];
			int dy = MetalGearRatMissiles1.yOffsets[MetalGearRatMissiles1.xOffsets.Length - 1 - i];
			Vector2 spawnPos = leftBasePos + PhysicsEngine.PixelToUnit(new IntVector2(dx + 1, dy - 7));
			base.Fire(Offset.OverridePosition(spawnPos), new Direction(-90f, DirectionType.Absolute, -1f), new MetalGearRatMissiles1.HomingBullet(leftDelays[i] - i * 4));
			spawnPos = rightBasePos + PhysicsEngine.PixelToUnit(new IntVector2(-dx + 1, dy - 7));
			base.Fire(Offset.OverridePosition(spawnPos), new Direction(-90f, DirectionType.Absolute, -1f), new MetalGearRatMissiles1.HomingBullet(rightDelays[i] - i * 4));
			yield return base.Wait(4);
		}
		yield return base.Wait(220);
		yield break;
	}

	// Token: 0x04000996 RID: 2454
	private const int NumDeathBullets = 8;

	// Token: 0x04000997 RID: 2455
	private static int[] xOffsets = new int[] { 0, -4, -7, -11, -14, -18, -21, -28 };

	// Token: 0x04000998 RID: 2456
	private static int[] yOffsets = new int[] { 0, -7, 0, -7, 0, -7, 0, 0 };

	// Token: 0x0200026E RID: 622
	private class HomingBullet : Bullet
	{
		// Token: 0x0600096A RID: 2410 RVA: 0x0002D694 File Offset: 0x0002B894
		public HomingBullet(int fireDelay = 0)
			: base("missile", false, false, false)
		{
			this.m_fireDelay = fireDelay;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0002D6AC File Offset: 0x0002B8AC
		public override void Initialize()
		{
			this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
			BraveUtility.EnableEmission(this.Projectile.ParticleTrail, false);
			base.Initialize();
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002D6D8 File Offset: 0x0002B8D8
		protected override IEnumerator Top()
		{
			if (this.m_fireDelay > 0)
			{
				yield return base.Wait(this.m_fireDelay);
			}
			this.Projectile.spriteAnimator.Play();
			BraveUtility.EnableEmission(this.Projectile.ParticleTrail, true);
			base.PostWwiseEvent("Play_BOSS_RatMech_Missile_01", null);
			base.PostWwiseEvent("Play_WPN_YariRocketLauncher_Shot_01", null);
			float t = UnityEngine.Random.value;
			this.Speed = Mathf.Lerp(8f, 14f, t);
			Vector2 toTarget = base.BulletBank.PlayerPosition() - base.Position;
			float travelTime = toTarget.magnitude / this.Speed * 60f - 1f;
			float magnitude = BraveUtility.RandomSign() * (1f - t) * 8f;
			Vector2 offset = magnitude * toTarget.Rotate(90f).normalized;
			base.ManualControl = true;
			int startTick = base.Tick;
			Vector2 truePosition = base.Position;
			Vector2 lastPosition = base.Position;
			Vector2 velocity = toTarget.normalized * this.Speed;
			int i = 0;
			while ((float)i < travelTime)
			{
				truePosition += velocity / 60f;
				lastPosition = base.Position;
				base.Position = truePosition + offset * Mathf.Sin((float)(base.Tick - startTick) / travelTime * 3.1415927f);
				this.Direction = (base.Position - lastPosition).ToAngle();
				yield return base.Wait(1);
				i++;
			}
			Vector2 v = (base.Position - lastPosition) * 60f;
			this.Speed = v.magnitude;
			this.Direction = v.ToAngle();
			base.ManualControl = false;
			yield break;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0002D6F4 File Offset: 0x0002B8F4
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = base.RandomAngle();
			float num2 = 45f;
			for (int i = 0; i < 8; i++)
			{
				base.Fire(new Direction(num + num2 * (float)i, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), null);
				base.PostWwiseEvent("Play_WPN_smallrocket_impact_01", null);
			}
		}

		// Token: 0x04000999 RID: 2457
		private int m_fireDelay;
	}
}
