using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000042 RID: 66
[InspectorDropdownName("Bosses/Blobulord/Slam1")]
public class BlobulordSlam1 : Script
{
	// Token: 0x060000F8 RID: 248 RVA: 0x00005DC4 File Offset: 0x00003FC4
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 4; i++)
		{
			float num = base.RandomAngle();
			for (int j = 0; j < 32; j++)
			{
				float num2 = num + (float)j * 11.25f;
				base.Fire(new Offset(2f, 0f, num2, string.Empty, DirectionType.Absolute), new Direction(num2, DirectionType.Absolute, -1f), new Speed(0.7f, SpeedType.Absolute), new BlobulordSlam1.SlamBullet(i));
			}
		}
		yield return base.Wait(80);
		yield break;
	}

	// Token: 0x040000FE RID: 254
	private const int NumBullets = 32;

	// Token: 0x040000FF RID: 255
	private const int NumWaves = 4;

	// Token: 0x02000043 RID: 67
	public class SlamBullet : Bullet
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00005DE0 File Offset: 0x00003FE0
		public SlamBullet(int spawnDelay)
			: base("slam", false, false, false)
		{
			this.m_spawnDelay = spawnDelay;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005DF8 File Offset: 0x00003FF8
		protected override IEnumerator Top()
		{
			int slowTime = this.m_spawnDelay * 40;
			int i = 0;
			for (;;)
			{
				yield return base.Wait(1);
				if (i == slowTime)
				{
					base.ChangeSpeed(new Speed(15f, SpeedType.Absolute), 60);
				}
				i++;
			}
			yield break;
		}

		// Token: 0x04000100 RID: 256
		private int m_spawnDelay;
	}
}
