using System;
using UnityEngine;

namespace Brave.BulletScript
{
	// Token: 0x02000354 RID: 852
	public interface IBulletManager
	{
		// Token: 0x06000D6A RID: 3434
		Vector2 PlayerPosition();

		// Token: 0x06000D6B RID: 3435
		Vector2 PlayerVelocity();

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000D6C RID: 3436
		// (set) Token: 0x06000D6D RID: 3437
		float TimeScale { get; set; }

		// Token: 0x06000D6E RID: 3438
		void BulletSpawnedHandler(Bullet bullet);

		// Token: 0x06000D6F RID: 3439
		void RemoveBullet(Bullet bullet);

		// Token: 0x06000D70 RID: 3440
		void DestroyBullet(Bullet deadBullet, bool suppressInAirEffects);

		// Token: 0x06000D71 RID: 3441
		Vector2 TransformOffset(Vector2 parentPos, string transform);

		// Token: 0x06000D72 RID: 3442
		float GetTransformRotation(string transform);

		// Token: 0x06000D73 RID: 3443
		Animation GetUnityAnimation();
	}
}
