using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x020002BD RID: 701
public class PhaseSpiderWeb1 : Script
{
	// Token: 0x06000AC2 RID: 2754 RVA: 0x00033C5C File Offset: 0x00031E5C
	protected override IEnumerator Top()
	{
		float startDirection = base.AimDirection - 60f;
		for (int i = 0; i < 7; i++)
		{
			int baseDelay = i * 7;
			if (i % 3 == 1)
			{
				for (int j = 0; j < 13; j++)
				{
					float num = 9.230769f;
					int num2 = 0;
					if (j % 4 == 1 || j % 4 == 3)
					{
						num2 = 3;
					}
					if (j % 4 == 2)
					{
						num2 = 5;
					}
					base.Fire(new Direction(startDirection + (float)j * num, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new PhaseSpiderWeb1.WebBullet(baseDelay + num2, false));
				}
			}
			else
			{
				for (int k = 0; k < 13; k++)
				{
					float num3 = 9.230769f;
					if (k % 4 == 0)
					{
						base.Fire(new Direction(startDirection + (float)k * num3, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new PhaseSpiderWeb1.WebBullet(baseDelay, i == 0));
					}
				}
			}
			yield return base.Wait(3);
		}
		yield break;
	}

	// Token: 0x04000B4B RID: 2891
	private const int NumWaves = 7;

	// Token: 0x04000B4C RID: 2892
	private const int BulletsPerWave = 13;

	// Token: 0x04000B4D RID: 2893
	private const float WebDegrees = 120f;

	// Token: 0x04000B4E RID: 2894
	private const float BulletSpeed = 9f;

	// Token: 0x020002BE RID: 702
	private class WebBullet : Bullet
	{
		// Token: 0x06000AC3 RID: 2755 RVA: 0x00033C78 File Offset: 0x00031E78
		public WebBullet(int delayFrames, bool spawnGoop = false)
			: base((!spawnGoop) ? "default" : "web", false, false, false)
		{
			this.m_delayFrames = delayFrames;
			this.m_spawnGoop = spawnGoop;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00033CA8 File Offset: 0x00031EA8
		protected override IEnumerator Top()
		{
			if (this.m_delayFrames == 0)
			{
				yield break;
			}
			float speed = this.Speed;
			this.Speed = 0f;
			yield return base.Wait(this.m_delayFrames);
			this.Speed = speed;
			yield break;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00033CC4 File Offset: 0x00031EC4
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.m_spawnGoop && destroyType != Bullet.DestroyType.DieInAir && base.BulletBank)
			{
				GoopDoer component = base.BulletBank.GetComponent<GoopDoer>();
				DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(component.goopDefinition).AddGoopCircle(base.Position, 1.5f, -1, false, -1);
			}
			base.OnBulletDestruction(destroyType, hitRigidbody, preventSpawningProjectiles);
		}

		// Token: 0x04000B4F RID: 2895
		private int m_delayFrames;

		// Token: 0x04000B50 RID: 2896
		private bool m_spawnGoop;
	}
}
