using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001C6 RID: 454
[InspectorDropdownName("Bosses/Helicopter/Flames1")]
public class HelicopterFlames1 : Script
{
	// Token: 0x060006C9 RID: 1737 RVA: 0x00020F4C File Offset: 0x0001F14C
	protected override IEnumerator Top()
	{
		List<AIActor> spawnedActors = new List<AIActor>();
		Vector2 basePos = base.BulletBank.aiActor.ParentRoom.area.UnitBottomLeft + new Vector2(5f, 22.8f);
		float height = 2.1f;
		float radius = 2f;
		float[] xPos = new float[]
		{
			(float)UnityEngine.Random.Range(4, 13),
			(float)UnityEngine.Random.Range(21, 30)
		};
		for (int i = 0; i < 2; i++)
		{
			float num = xPos[i];
			float num2 = UnityEngine.Random.Range(0f, 0.8f * height);
			for (int j = 0; j < 9; j++)
			{
				Vector2 vector = basePos + new Vector2(num - radius, (float)j * -height - num2);
				float num3 = (vector - base.Position).ToAngle();
				base.Fire(new Offset(HelicopterFlames1.s_Transforms[i * 2]), new Direction(num3, DirectionType.Absolute, -1f), new HelicopterFlames1.FlameBullet(vector, spawnedActors, 60 + 5 * j));
				vector.x += 2f * radius;
				num3 = (vector - base.Position).ToAngle();
				base.Fire(new Offset(HelicopterFlames1.s_Transforms[i * 2 + 1]), new Direction(num3, DirectionType.Absolute, -1f), new HelicopterFlames1.FlameBullet(vector, spawnedActors, 60 + 5 * j));
			}
		}
		yield return base.Wait(105);
		GoopDefinition goop = base.BulletBank.GetComponent<GoopDoer>().goopDefinition;
		DeadlyDeadlyGoopManager gooper = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goop);
		for (int k = 0; k < 2; k++)
		{
			Vector2 vector2 = basePos + new Vector2(xPos[k], 0f);
			Vector2 vector3 = vector2 + new Vector2(0f, -18f);
			gooper.TimedAddGoopLine(vector2, vector3, radius, 1f);
		}
		yield break;
	}

	// Token: 0x040006A1 RID: 1697
	private static string[] s_Transforms = new string[] { "shoot point 1", "shoot point 2", "shoot point 3", "shoot point 4" };

	// Token: 0x040006A2 RID: 1698
	public const int NumFlamesPerRow = 9;

	// Token: 0x020001C7 RID: 455
	private class FlameBullet : Bullet
	{
		// Token: 0x060006CB RID: 1739 RVA: 0x00020F98 File Offset: 0x0001F198
		public FlameBullet(Vector2 goalPos, List<AIActor> spawnedActors, int flightTime)
			: base("flame", false, false, false)
		{
			this.m_goalPos = goalPos;
			this.m_flightTime = flightTime;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00020FB8 File Offset: 0x0001F1B8
		protected override IEnumerator Top()
		{
			this.Projectile.IgnoreTileCollisionsFor((float)(this.m_flightTime - 5) / 60f);
			this.Projectile.spriteAnimator.Play();
			base.ManualControl = true;
			this.Direction = (this.m_goalPos - base.Position).ToAngle();
			this.Speed = Vector2.Distance(this.m_goalPos, base.Position) / ((float)this.m_flightTime / 60f);
			Vector2 truePosition = base.Position;
			for (int i = 0; i < this.m_flightTime; i++)
			{
				truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
				base.Position = truePosition + new Vector2(0f, Mathf.Sin((float)i / (float)this.m_flightTime * 3.1415927f) * 5f);
				yield return base.Wait(1);
			}
			yield return base.Wait(480);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040006A3 RID: 1699
		private Vector2 m_goalPos;

		// Token: 0x040006A4 RID: 1700
		private int m_flightTime;
	}
}
