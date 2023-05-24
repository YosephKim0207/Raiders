using System;

namespace Brave.BulletScript
{
	// Token: 0x02000341 RID: 833
	public class Speed : IFireParam
	{
		// Token: 0x06000CF2 RID: 3314 RVA: 0x0003DF84 File Offset: 0x0003C184
		public Speed(float speed = 0f, SpeedType type = SpeedType.Absolute)
		{
			this.speed = speed;
			this.type = type;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0003DF9C File Offset: 0x0003C19C
		public float GetSpeed(Bullet bullet)
		{
			if (this.type == SpeedType.Relative || this.type == SpeedType.Sequence)
			{
				return bullet.Speed + this.speed;
			}
			return this.speed;
		}

		// Token: 0x04000D96 RID: 3478
		public SpeedType type;

		// Token: 0x04000D97 RID: 3479
		public float speed;
	}
}
