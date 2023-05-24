using System;
using UnityEngine;

namespace Brave.BulletScript
{
	// Token: 0x02000343 RID: 835
	public class Direction : IFireParam
	{
		// Token: 0x06000CF4 RID: 3316 RVA: 0x0003DFCC File Offset: 0x0003C1CC
		public Direction(float direction = 0f, DirectionType type = DirectionType.Absolute, float maxFrameDelta = -1f)
		{
			this.direction = direction;
			this.type = type;
			this.maxFrameDelta = maxFrameDelta;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0003DFEC File Offset: 0x0003C1EC
		public float GetDirection(Bullet bullet, float? overrideBaseDirection = null)
		{
			float num;
			if (this.type == DirectionType.Aim)
			{
				num = (bullet.BulletManager.PlayerPosition() - bullet.Position).ToAngle() + this.direction;
			}
			else if (this.type == DirectionType.Relative || this.type == DirectionType.Sequence)
			{
				float num2 = ((overrideBaseDirection == null) ? bullet.Direction : overrideBaseDirection.Value);
				num = num2 + this.direction;
			}
			else
			{
				num = this.direction;
			}
			if (this.maxFrameDelta > 0f)
			{
				float num3 = BraveMathCollege.ClampAngle180(num - bullet.Direction);
				num = bullet.Direction + Mathf.Clamp(num3, -this.maxFrameDelta, this.maxFrameDelta);
			}
			return num;
		}

		// Token: 0x04000D9D RID: 3485
		public DirectionType type;

		// Token: 0x04000D9E RID: 3486
		public float direction;

		// Token: 0x04000D9F RID: 3487
		public float maxFrameDelta;
	}
}
