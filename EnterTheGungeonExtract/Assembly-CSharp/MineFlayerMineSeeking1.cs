using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200029B RID: 667
[InspectorDropdownName("Bosses/MineFlayer/MineSeeking1")]
public class MineFlayerMineSeeking1 : Script
{
	// Token: 0x06000A36 RID: 2614 RVA: 0x000316B0 File Offset: 0x0002F8B0
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
			Vector2 goal = roomCenter + BraveMathCollege.DegreesToVector(angle, MineFlayerMineSeeking1.CircleRadius);
			base.Fire(new Direction(angle, DirectionType.Absolute, -1f), new MineFlayerMineSeeking1.MineBullet(goal, spawnedActors, i));
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

	// Token: 0x04000A93 RID: 2707
	private const int FlightTime = 60;

	// Token: 0x04000A94 RID: 2708
	private const string EnemyGuid = "566ecca5f3b04945ac6ce1f26dedbf4f";

	// Token: 0x04000A95 RID: 2709
	private const float EnemyAngularSpeed = 30f;

	// Token: 0x04000A96 RID: 2710
	private const float EnemyAngularSpeedDelta = 5f;

	// Token: 0x04000A97 RID: 2711
	private const int BulletsPerSpray = 5;

	// Token: 0x04000A98 RID: 2712
	private static readonly float CircleRadius = 14f;

	// Token: 0x0200029C RID: 668
	private class MineBullet : Bullet
	{
		// Token: 0x06000A38 RID: 2616 RVA: 0x000316D8 File Offset: 0x0002F8D8
		public MineBullet(Vector2 goalPos, List<AIActor> spawnedActors, int index)
			: base("mine", false, false, false)
		{
			this.m_goalPos = goalPos;
			this.m_spawnedActors = spawnedActors;
			this.m_index = index;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00031700 File Offset: 0x0002F900
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
			WaitThenChargeBehavior waitThenChargeBehavior = new WaitThenChargeBehavior();
			spawnedEnemy.behaviorSpeculator.MovementBehaviors[0] = waitThenChargeBehavior;
			waitThenChargeBehavior.Delay = (float)(9 - this.m_index) * 0.35f + 0.7f * (float)this.m_index;
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000A99 RID: 2713
		private Vector2 m_goalPos;

		// Token: 0x04000A9A RID: 2714
		private List<AIActor> m_spawnedActors;

		// Token: 0x04000A9B RID: 2715
		private int m_index;
	}
}
