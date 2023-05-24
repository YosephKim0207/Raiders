using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000242 RID: 578
[InspectorDropdownName("Bosses/Meduzi/Scream2 (optimized)")]
public class MeduziScream2 : Script
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x0002A8BC File Offset: 0x00028ABC
	protected override IEnumerator Top()
	{
		bool isCoop = GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER;
		SpeculativeRigidbody target = GameManager.Instance.PrimaryPlayer.specRigidbody;
		SpeculativeRigidbody target2 = ((!isCoop) ? null : GameManager.Instance.SecondaryPlayer.specRigidbody);
		int numGaps = ((!isCoop) ? 3 : 2);
		if (MeduziScream2.s_gapAngles == null || MeduziScream2.s_gapAngles.Length != numGaps)
		{
			MeduziScream2.s_gapAngles = new float[numGaps];
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
				MeduziScream2.s_gapAngles[0] = idealGapAngle;
				MeduziScream2.s_gapAngles[1] = idealGapAngle2;
			}
			else
			{
				for (int j = 0; j < numGaps; j++)
				{
					MeduziScream2.s_gapAngles[j] = base.SubdivideCircle(idealGapAngle, numGaps, j, 1f, false);
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
				float num2 = ((numGaps != 1) ? BraveMathCollege.GetNearestAngle(num, MeduziScream2.s_gapAngles) : MeduziScream2.s_gapAngles[0]);
				float num3 = BraveMathCollege.ClampAngle180(num2 - num);
				int num4 = Mathf.RoundToInt(Mathf.Abs(num3 / delta));
				if (num4 == skipCount && (num4 == 0 || num3 > 0f == skipDirection))
				{
					num4 = 100;
				}
				if (num4 <= 4)
				{
					MeduziScream2.TimedBullet timedBullet = new MeduziScream2.TimedBullet(num4, Mathf.Sign(num3));
					if (num4 == 4 && Mathf.Sign(num3) >= 0f)
					{
						int num5 = Array.IndexOf<float>(MeduziScream2.s_gapAngles, num2);
						int num6 = (num5 + 1) % MeduziScream2.s_gapAngles.Length;
						float num7 = BraveMathCollege.ClampAngle360(MeduziScream2.s_gapAngles[num5] + (float)num4 * delta);
						float num8 = BraveMathCollege.ClampAngle360(MeduziScream2.s_gapAngles[num6] - 3f * delta);
						timedBullet.SetArc(base.Position, num7, num8);
					}
					base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), timedBullet);
				}
			}
			yield return base.Wait(20);
			if (isCoop && numGaps > 1)
			{
				MeduziScream2.s_gapAngles[0] = idealGapAngle;
				MeduziScream2.s_gapAngles[1] = idealGapAngle2;
			}
			else
			{
				for (int l = 0; l < numGaps; l++)
				{
					MeduziScream2.s_gapAngles[l] = base.SubdivideCircle(idealGapAngle, numGaps, l, 1f, false);
				}
			}
			if (!isCoop)
			{
				idealGapAngle = BraveMathCollege.GetNearestAngle(base.AimDirection, MeduziScream2.s_gapAngles);
			}
			this.SafeUpdateAngle(ref idealGapAngle, target);
			if (isCoop && numGaps > 1)
			{
				this.SafeUpdateAngle(ref idealGapAngle2, target2);
			}
		}
		yield break;
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0002A8D8 File Offset: 0x00028AD8
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

	// Token: 0x060008BB RID: 2235 RVA: 0x0002A94C File Offset: 0x00028B4C
	private bool IsSafeAngle(float angle, SpeculativeRigidbody target)
	{
		float num = Vector2.Distance(target.GetUnitCenter(ColliderType.HitBox), base.Position);
		Vector2 vector = base.Position + BraveMathCollege.DegreesToVector(angle, num);
		return !GameManager.Instance.Dungeon.data.isWall((int)vector.x, (int)vector.y) && !DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.m_goopDefinition).IsPositionInGoop(vector);
	}

	// Token: 0x040008C5 RID: 2245
	private const int NumWaves = 16;

	// Token: 0x040008C6 RID: 2246
	private const int NumBulletsPerWave = 64;

	// Token: 0x040008C7 RID: 2247
	private const int NumGaps = 3;

	// Token: 0x040008C8 RID: 2248
	private const int StepOpenTime = 14;

	// Token: 0x040008C9 RID: 2249
	private const int GapHalfWidth = 3;

	// Token: 0x040008CA RID: 2250
	private const int GapHoldWaves = 6;

	// Token: 0x040008CB RID: 2251
	private const float TurnDegPerWave = 12f;

	// Token: 0x040008CC RID: 2252
	private static float[] s_gapAngles;

	// Token: 0x040008CD RID: 2253
	private GoopDefinition m_goopDefinition;

	// Token: 0x02000243 RID: 579
	private class TimedBullet : Bullet
	{
		// Token: 0x060008BD RID: 2237 RVA: 0x0002A9C8 File Offset: 0x00028BC8
		public TimedBullet(int bulletsFromSafeDir, float direction)
			: base("scream", false, false, false)
		{
			this.m_bulletsFromSafeDir = bulletsFromSafeDir;
			this.m_direction = direction;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0002A9E8 File Offset: 0x00028BE8
		public void SetArc(Vector2 center, float startAngle, float endAngle)
		{
			this.m_hasArc = true;
			this.m_arcCenter = center;
			this.m_startAngle = startAngle;
			this.m_endAngle = endAngle;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0002AA08 File Offset: 0x00028C08
		protected override IEnumerator Top()
		{
			if (this.m_bulletsFromSafeDir == 4)
			{
				if (this.m_hasArc)
				{
					BulletArcLightningController orAddComponent = this.Projectile.gameObject.GetOrAddComponent<BulletArcLightningController>();
					orAddComponent.Initialize(this.m_arcCenter, this.Speed, this.Projectile.OwnerName, this.m_startAngle, this.m_endAngle, 0.25f);
				}
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

		// Token: 0x040008CE RID: 2254
		private int m_bulletsFromSafeDir;

		// Token: 0x040008CF RID: 2255
		private float m_direction;

		// Token: 0x040008D0 RID: 2256
		private bool m_hasArc;

		// Token: 0x040008D1 RID: 2257
		private Vector2 m_arcCenter;

		// Token: 0x040008D2 RID: 2258
		private float m_startAngle;

		// Token: 0x040008D3 RID: 2259
		private float m_endAngle;
	}
}
