using System;
using System.Collections;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x0200015A RID: 346
[InspectorDropdownName("Bosses/DraGun/BigNoseShot2")]
public class DraGunBigNoseShot2 : Script
{
	// Token: 0x0600052F RID: 1327 RVA: 0x00018EE0 File Offset: 0x000170E0
	protected override IEnumerator Top()
	{
		if (DraGunBigNoseShot2.s_xValues == null || DraGunBigNoseShot2.s_yValues == null)
		{
			DraGunBigNoseShot2.s_xValues = new int[this.NumTraps];
			DraGunBigNoseShot2.s_yValues = new int[this.NumTraps];
			for (int i = 0; i < this.NumTraps; i++)
			{
				DraGunBigNoseShot2.s_xValues[i] = i;
				DraGunBigNoseShot2.s_yValues[i] = i;
			}
		}
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		Vector2 vector = area.UnitBottomLeft + new Vector2(1f, 20f);
		Vector2 vector2 = new Vector2(34f, 11f);
		Vector2 vector3 = new Vector2(vector2.x / (float)this.NumTraps, vector2.y / (float)this.NumTraps);
		BraveUtility.RandomizeArray<int>(DraGunBigNoseShot2.s_xValues, 0, -1);
		BraveUtility.RandomizeArray<int>(DraGunBigNoseShot2.s_yValues, 0, -1);
		for (int j = 0; j < this.NumTraps; j++)
		{
			int num = DraGunBigNoseShot2.s_xValues[j];
			int num2 = DraGunBigNoseShot2.s_yValues[j];
			Vector2 vector4 = vector + new Vector2(((float)num + UnityEngine.Random.value) * vector3.x, ((float)num2 + UnityEngine.Random.value) * vector3.y);
			base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new DraGunBigNoseShot2.EnemyBullet(vector4));
		}
		return null;
	}

	// Token: 0x04000501 RID: 1281
	public int NumTraps = 5;

	// Token: 0x04000502 RID: 1282
	private static int[] s_xValues;

	// Token: 0x04000503 RID: 1283
	private static int[] s_yValues;

	// Token: 0x0200015B RID: 347
	public class EnemyBullet : Bullet
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x00019050 File Offset: 0x00017250
		public EnemyBullet(Vector2 goalPos)
			: base("homing", false, false, false)
		{
			this.m_goalPos = goalPos;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00019068 File Offset: 0x00017268
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 startPos = base.Position;
			int travelTime = (int)((this.m_goalPos - base.Position).magnitude / this.Speed * 60f);
			int lifeTime = UnityEngine.Random.Range(480, 600);
			int nextShot = 60 + UnityEngine.Random.Range(45, 90);
			AIAnimator aiAnimator = this.Projectile.sprite.aiAnimator;
			for (int i = 0; i < travelTime; i++)
			{
				base.Position = Vector2.Lerp(startPos, this.m_goalPos, (float)i / (float)travelTime);
				aiAnimator.FacingDirection = -90f;
				yield return base.Wait(1);
			}
			while (base.Tick < lifeTime)
			{
				if (base.Tick >= nextShot)
				{
					aiAnimator.PlayUntilFinished("attack", false, null, -1f, false);
					yield return base.Wait(30);
					base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), null);
					nextShot = base.Tick + UnityEngine.Random.Range(45, 90);
				}
				aiAnimator.FacingDirection = base.AimDirection;
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000504 RID: 1284
		public const int StartShootDelay = 60;

		// Token: 0x04000505 RID: 1285
		public const int MinShootTime = 45;

		// Token: 0x04000506 RID: 1286
		public const int MaxShootTime = 90;

		// Token: 0x04000507 RID: 1287
		public const int LifeTimeMin = 480;

		// Token: 0x04000508 RID: 1288
		public const int LifeTimeMax = 600;

		// Token: 0x04000509 RID: 1289
		private Vector2 m_goalPos;
	}
}
