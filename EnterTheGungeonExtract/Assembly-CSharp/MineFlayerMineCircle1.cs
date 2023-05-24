using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000297 RID: 663
[InspectorDropdownName("Bosses/MineFlayer/MineCircle1")]
public class MineFlayerMineCircle1 : Script
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x00030F2C File Offset: 0x0002F12C
	protected override IEnumerator Top()
	{
		int numMines = (((double)base.BulletBank.healthHaver.GetCurrentHealthPercentage() <= 0.5) ? 12 : 9);
		int goopExceptionId = DeadlyDeadlyGoopManager.RegisterUngoopableCircle(base.BulletBank.specRigidbody.UnitCenter, 2f);
		yield return base.Wait(72);
		List<AIActor> spawnedActors = new List<AIActor>();
		Vector2 roomCenter = base.BulletBank.aiActor.ParentRoom.area.UnitCenter;
		for (int i = 0; i < numMines; i++)
		{
			float angle = UnityEngine.Random.Range(-60f, 60f) + (float)((i % 2 != 0) ? 180 : 0);
			float radius = MineFlayerMineCircle1.CircleRadii[i % MineFlayerMineCircle1.CircleRadii.Length];
			Vector2 goal = roomCenter + BraveMathCollege.DegreesToVector(angle, radius);
			base.Fire(new Direction(angle, DirectionType.Absolute, -1f), new MineFlayerMineCircle1.MineBullet(radius, goal, spawnedActors));
			yield return base.Wait(21);
		}
		yield return base.Wait(63);
		for (int j = 0; j < 19; j++)
		{
			float facingAngle = (float)((j % 2 != 0) ? 180 : 0);
			float targetAngle = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
			if (BraveMathCollege.AbsAngleBetween(facingAngle, targetAngle) < 90f && BraveUtility.RandomBool())
			{
				for (int l = 0; l < 5; l++)
				{
					float num = base.SubdivideArc(targetAngle - 25f, 50f, 5, l, false);
					base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
				}
			}
			yield return base.Wait(21);
		}
		for (int k = 0; k < spawnedActors.Count; k++)
		{
			AIActor actor = spawnedActors[k];
			if (actor && actor.healthHaver.IsAlive)
			{
				ExplodeOnDeath explodeOnDeath = actor.GetComponent<ExplodeOnDeath>();
				if (explodeOnDeath)
				{
					UnityEngine.Object.Destroy(explodeOnDeath);
				}
				actor.healthHaver.ApplyDamage(1E+10f, Vector2.zero, "Claymore Death", CoreDamageTypes.None, DamageCategory.Unstoppable, false, null, false);
				yield return base.Wait(3);
			}
		}
		DeadlyDeadlyGoopManager.DeregisterUngoopableCircle(goopExceptionId);
		yield break;
	}

	// Token: 0x04000A6E RID: 2670
	private const int FlightTime = 60;

	// Token: 0x04000A6F RID: 2671
	private const string EnemyGuid = "566ecca5f3b04945ac6ce1f26dedbf4f";

	// Token: 0x04000A70 RID: 2672
	private const float EnemyAngularSpeed = 30f;

	// Token: 0x04000A71 RID: 2673
	private const float EnemyAngularSpeedDelta = 5f;

	// Token: 0x04000A72 RID: 2674
	private const int BulletsPerSpray = 5;

	// Token: 0x04000A73 RID: 2675
	private static readonly float[] CircleRadii = new float[] { 4f, 9f, 14f };

	// Token: 0x02000298 RID: 664
	private class MineBullet : Bullet
	{
		// Token: 0x06000A27 RID: 2599 RVA: 0x00030F60 File Offset: 0x0002F160
		public MineBullet(float radius, Vector2 goalPos, List<AIActor> spawnedActors)
			: base("mine", false, false, false)
		{
			this.m_radius = radius;
			this.m_goalPos = goalPos;
			this.m_spawnedActors = spawnedActors;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00030F88 File Offset: 0x0002F188
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Direction = (this.m_goalPos - base.Position).ToAngle();
			this.Speed = Vector2.Distance(this.m_goalPos, base.Position) / 1f;
			Vector2 truePosition = base.Position;
			for (int i = 0; i < 60; i++)
			{
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + new Vector2(0f, Mathf.Sin((float)i / 60f * 3.1415927f) * 3.5f);
				yield return base.Wait(1);
			}
			Vector2 spawnPos = this.Projectile.specRigidbody.UnitBottomLeft;
			AIActor spawnedEnemy = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid("566ecca5f3b04945ac6ce1f26dedbf4f"), spawnPos, base.BulletBank.aiActor.ParentRoom, true, AIActor.AwakenAnimationType.Awaken, true);
			this.m_spawnedActors.Add(spawnedEnemy);
			CircleRoomBehavior circleRoomBehavior = spawnedEnemy.behaviorSpeculator.MovementBehaviors[0] as CircleRoomBehavior;
			circleRoomBehavior.Radius = this.m_radius;
			circleRoomBehavior.Direction = (float)((this.m_radius != 9f) ? 1 : (-1));
			float angularSpeed = 30f + UnityEngine.Random.Range(-5f, 5f);
			spawnedEnemy.MovementSpeed = angularSpeed * 0.017453292f * this.m_radius;
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000A74 RID: 2676
		private float m_radius;

		// Token: 0x04000A75 RID: 2677
		private Vector2 m_goalPos;

		// Token: 0x04000A76 RID: 2678
		private List<AIActor> m_spawnedActors;
	}
}
