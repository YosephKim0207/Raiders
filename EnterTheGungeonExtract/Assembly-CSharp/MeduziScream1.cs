using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200023E RID: 574
[InspectorDropdownName("Bosses/Meduzi/Scream1")]
public class MeduziScream1 : Script
{
	// Token: 0x060008A6 RID: 2214 RVA: 0x00029DAC File Offset: 0x00027FAC
	protected override IEnumerator Top()
	{
		bool isCoop = GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER;
		SpeculativeRigidbody target = GameManager.Instance.PrimaryPlayer.specRigidbody;
		SpeculativeRigidbody target2 = ((!isCoop) ? null : GameManager.Instance.SecondaryPlayer.specRigidbody);
		int numGaps = ((!isCoop) ? 3 : 2);
		if (MeduziScream1.s_gapAngles == null || MeduziScream1.s_gapAngles.Length != numGaps)
		{
			MeduziScream1.s_gapAngles = new float[numGaps];
		}
		base.EndOnBlank = true;
		this.m_goopDefinition = base.BulletBank.GetComponent<GoopDoer>().goopDefinition;
		float delta = 5.625f;
		float idealGapAngle = (target.GetUnitCenter(ColliderType.HitBox) - base.Position).ToAngle();
		float idealGapAngle2 = ((!isCoop) ? 0f : (target2.GetUnitCenter(ColliderType.HitBox) - base.Position).ToAngle());
		for (int i = 0; i < 16; i++)
		{
			if (isCoop && numGaps > 1 && BraveMathCollege.AbsAngleBetween(idealGapAngle, idealGapAngle2) < 22.5f)
			{
				numGaps = 1;
			}
			if (isCoop && numGaps > 1)
			{
				MeduziScream1.s_gapAngles[0] = idealGapAngle;
				MeduziScream1.s_gapAngles[1] = idealGapAngle2;
			}
			else
			{
				for (int j = 0; j < numGaps; j++)
				{
					MeduziScream1.s_gapAngles[j] = base.SubdivideCircle(idealGapAngle, numGaps, j, 1f, false);
				}
			}
			int skipCount = -1000;
			bool skipDirection = BraveUtility.RandomBool();
			if (i % 2 == 0)
			{
				skipCount = UnityEngine.Random.Range(0, 3);
			}
			for (int k = 0; k < 64; k++)
			{
				float num = base.SubdivideCircle(idealGapAngle, 64, k, 1f, false);
				float num2 = ((numGaps != 1) ? BraveMathCollege.GetNearestAngle(num, MeduziScream1.s_gapAngles) : MeduziScream1.s_gapAngles[0]);
				float num3 = BraveMathCollege.ClampAngle180(num2 - num);
				int num4 = Mathf.RoundToInt(Mathf.Abs(num3 / delta));
				if (num4 == skipCount && (num4 == 0 || num3 > 0f == skipDirection))
				{
					num4 = 100;
				}
				base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new MeduziScream1.TimedBullet(num4, Mathf.Sign(num3)));
			}
			yield return base.Wait(20);
			if (isCoop && numGaps > 1)
			{
				MeduziScream1.s_gapAngles[0] = idealGapAngle;
				MeduziScream1.s_gapAngles[1] = idealGapAngle2;
			}
			else
			{
				for (int l = 0; l < numGaps; l++)
				{
					MeduziScream1.s_gapAngles[l] = base.SubdivideCircle(idealGapAngle, numGaps, l, 1f, false);
				}
			}
			if (!isCoop)
			{
				idealGapAngle = BraveMathCollege.GetNearestAngle(base.AimDirection, MeduziScream1.s_gapAngles);
			}
			this.SafeUpdateAngle(ref idealGapAngle, target);
			if (isCoop && numGaps > 1)
			{
				this.SafeUpdateAngle(ref idealGapAngle2, target2);
			}
		}
		yield break;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00029DC8 File Offset: 0x00027FC8
	private void SafeUpdateAngle(ref float idealGapAngle, SpeculativeRigidbody target)
	{
		bool flag = this.IsSafeAngle(idealGapAngle + 12f, target);
		bool flag2 = this.IsSafeAngle(idealGapAngle - 12f, target);
		if ((flag && flag2) || (!flag && !flag2))
		{
			idealGapAngle += BraveUtility.RandomSign() * 12f;
		}
		else
		{
			idealGapAngle += (float)((!flag) ? (-1) : 1) * 12f;
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00029E3C File Offset: 0x0002803C
	private bool IsSafeAngle(float angle, SpeculativeRigidbody target)
	{
		float num = Vector2.Distance(target.GetUnitCenter(ColliderType.HitBox), base.Position);
		Vector2 vector = base.Position + BraveMathCollege.DegreesToVector(angle, num);
		return !GameManager.Instance.Dungeon.data.isWall((int)vector.x, (int)vector.y) && !DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.m_goopDefinition).IsPositionInGoop(vector);
	}

	// Token: 0x0400089E RID: 2206
	private const int NumWaves = 16;

	// Token: 0x0400089F RID: 2207
	private const int NumBulletsPerWave = 64;

	// Token: 0x040008A0 RID: 2208
	private const int NumGaps = 3;

	// Token: 0x040008A1 RID: 2209
	private const int StepOpenTime = 14;

	// Token: 0x040008A2 RID: 2210
	private const int GapHalfWidth = 3;

	// Token: 0x040008A3 RID: 2211
	private const int GapHoldWaves = 6;

	// Token: 0x040008A4 RID: 2212
	private const float TurnDegPerWave = 12f;

	// Token: 0x040008A5 RID: 2213
	private static float[] s_gapAngles;

	// Token: 0x040008A6 RID: 2214
	private GoopDefinition m_goopDefinition;

	// Token: 0x0200023F RID: 575
	private class TimedBullet : Bullet
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x00029EB8 File Offset: 0x000280B8
		public TimedBullet(int bulletsFromSafeDir, float direction)
			: base("scream", false, false, false)
		{
			this.m_bulletsFromSafeDir = bulletsFromSafeDir;
			this.m_direction = direction;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00029ED8 File Offset: 0x000280D8
		protected override IEnumerator Top()
		{
			if (this.m_bulletsFromSafeDir == 4)
			{
				yield return base.Wait(95);
				UVScrollTriggerableInitializer animator = this.Projectile.sprite.GetComponent<UVScrollTriggerableInitializer>();
				animator.TriggerAnimation();
				this.Projectile.Ramp(2f, 10f);
				yield return base.Wait(45);
				animator.ResetAnimation();
				yield return base.Wait(200);
				base.Vanish(false);
			}
			else if (this.m_bulletsFromSafeDir > 3)
			{
				yield return base.Wait(420);
				base.Vanish(false);
			}
			else
			{
				Vector2 origin = base.Position;
				int preDelay = 14 * this.m_bulletsFromSafeDir;
				if (preDelay > 0)
				{
					yield return base.Wait(preDelay);
				}
				float radius = Vector2.Distance(base.Position, origin);
				float angle = this.Direction;
				float deltaAngle = 0.4017857f;
				base.ManualControl = true;
				int moveTime = (3 - this.m_bulletsFromSafeDir + 1) * 14;
				for (int i = 0; i < moveTime; i++)
				{
					base.UpdateVelocity();
					radius += this.Speed / 60f;
					angle -= this.m_direction * deltaAngle;
					base.Position = origin + BraveMathCollege.DegreesToVector(angle, radius);
					yield return base.Wait(1);
				}
				base.ManualControl = false;
				this.Direction = angle;
				yield return base.Wait(1);
				yield return base.Wait(84);
				UVScrollTriggerableInitializer animator2 = this.Projectile.sprite.GetComponent<UVScrollTriggerableInitializer>();
				animator2.TriggerAnimation();
				radius = Vector2.Distance(base.Position, origin);
				base.ManualControl = true;
				moveTime = (3 - this.m_bulletsFromSafeDir + 1) * 14;
				for (int j = 0; j < moveTime; j++)
				{
					base.UpdateVelocity();
					radius += this.Speed / 60f;
					angle += this.m_direction * deltaAngle;
					base.Position = origin + BraveMathCollege.DegreesToVector(angle, radius);
					yield return base.Wait(1);
				}
				base.ManualControl = false;
				this.Direction = angle;
				yield return base.Wait(240);
				base.Vanish(true);
			}
			yield break;
		}

		// Token: 0x040008A7 RID: 2215
		private int m_bulletsFromSafeDir;

		// Token: 0x040008A8 RID: 2216
		private float m_direction;
	}
}
