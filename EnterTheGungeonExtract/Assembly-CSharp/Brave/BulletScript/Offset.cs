using System;
using UnityEngine;

namespace Brave.BulletScript
{
	// Token: 0x0200033F RID: 831
	public class Offset : IFireParam
	{
		// Token: 0x06000CEB RID: 3307 RVA: 0x0003DD9C File Offset: 0x0003BF9C
		public Offset(float x = 0f, float y = 0f, float rotation = 0f, string transform = "", DirectionType directionType = DirectionType.Absolute)
		{
			this.x = x;
			this.y = y;
			this.rotation = rotation;
			this.transform = transform;
			this.directionType = directionType;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0003DDCC File Offset: 0x0003BFCC
		public Offset(Vector2 offset, float rotation = 0f, string transform = "", DirectionType directionType = DirectionType.Absolute)
		{
			this.x = offset.x;
			this.y = offset.y;
			this.rotation = rotation;
			this.transform = transform;
			this.directionType = directionType;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0003DE04 File Offset: 0x0003C004
		public Offset(string transform)
		{
			this.x = 0f;
			this.y = 0f;
			this.rotation = 0f;
			this.transform = transform;
			this.directionType = DirectionType.Relative;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0003DE3C File Offset: 0x0003C03C
		public Vector2 GetPosition(Bullet bullet)
		{
			Vector2? overridePosition = this.m_overridePosition;
			if (overridePosition != null)
			{
				return this.m_overridePosition.Value;
			}
			Vector2 vector = bullet.Position;
			if (!string.IsNullOrEmpty(this.transform))
			{
				vector = bullet.BulletManager.TransformOffset(bullet.Position, this.transform);
			}
			Vector2 vector2 = new Vector2(this.x, this.y);
			if (this.rotation != 0f)
			{
				vector2 = vector2.Rotate(this.rotation);
			}
			if (this.directionType != DirectionType.Absolute)
			{
				if (this.directionType == DirectionType.Relative)
				{
					vector2 = vector2.Rotate(bullet.Direction);
				}
				else
				{
					Debug.LogError("Cannot use DirectionType {0} in an Offset instance.");
				}
			}
			return vector + vector2;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0003DF04 File Offset: 0x0003C104
		public float? GetDirection(Bullet bullet)
		{
			if (string.IsNullOrEmpty(this.transform))
			{
				return null;
			}
			return new float?(bullet.BulletManager.GetTransformRotation(this.transform));
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0003DF44 File Offset: 0x0003C144
		public string GetTransformValue()
		{
			return this.transform;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0003DF4C File Offset: 0x0003C14C
		public static Offset OverridePosition(Vector2 overridePosition)
		{
			return new Offset(0f, 0f, 0f, string.Empty, DirectionType.Absolute)
			{
				m_overridePosition = new Vector2?(overridePosition)
			};
		}

		// Token: 0x04000D8C RID: 3468
		public float x;

		// Token: 0x04000D8D RID: 3469
		public float y;

		// Token: 0x04000D8E RID: 3470
		public string transform;

		// Token: 0x04000D8F RID: 3471
		public float rotation;

		// Token: 0x04000D90 RID: 3472
		public DirectionType directionType;

		// Token: 0x04000D91 RID: 3473
		private Vector2? m_overridePosition;
	}
}
