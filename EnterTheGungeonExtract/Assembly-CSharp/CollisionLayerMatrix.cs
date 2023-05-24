using System;

// Token: 0x02001829 RID: 6185
public static class CollisionLayerMatrix
{
	// Token: 0x0600926B RID: 37483 RVA: 0x003DD9C4 File Offset: 0x003DBBC4
	static CollisionLayerMatrix()
	{
		CollisionLayerMatrix.m_collisionMatrix[0] = CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider, CollisionLayer.Projectile, CollisionLayer.Pickup, CollisionLayer.Trap);
		CollisionLayerMatrix.m_collisionMatrix[1] = CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider, CollisionLayer.LowObstacle, CollisionLayer.HighObstacle, CollisionLayer.PlayerBlocker, CollisionLayer.MovingPlatform);
		CollisionLayerMatrix.m_collisionMatrix[2] = CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.PlayerCollider, CollisionLayer.Projectile, CollisionLayer.Trap);
		CollisionLayerMatrix.m_collisionMatrix[3] = CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.PlayerCollider, CollisionLayer.LowObstacle, CollisionLayer.HighObstacle, CollisionLayer.EnemyBlocker, CollisionLayer.MovingPlatform);
		CollisionLayerMatrix.m_collisionMatrix[4] = CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.EnemyHitBox, CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, CollisionLayer.BulletBreakable, CollisionLayer.EnemyBulletBlocker);
		CollisionLayerMatrix.m_collisionMatrix[5] = CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.EnemyCollider, CollisionLayer.LowObstacle, CollisionLayer.HighObstacle);
		CollisionLayerMatrix.m_collisionMatrix[6] = CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.EnemyCollider, CollisionLayer.Projectile, CollisionLayer.LowObstacle, CollisionLayer.HighObstacle);
		CollisionLayerMatrix.m_collisionMatrix[7] = CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.MovingPlatform);
		CollisionLayerMatrix.m_collisionMatrix[8] = CollisionMask.LayerToMask(CollisionLayer.Projectile);
		CollisionLayerMatrix.m_collisionMatrix[9] = CollisionMask.LayerToMask(CollisionLayer.EnemyCollider);
		CollisionLayerMatrix.m_collisionMatrix[10] = CollisionMask.LayerToMask(CollisionLayer.PlayerCollider);
		CollisionLayerMatrix.m_collisionMatrix[11] = CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.EnemyCollider, CollisionLayer.Pickup);
		CollisionLayerMatrix.m_collisionMatrix[12] = CollisionMask.LayerToMask(CollisionLayer.Projectile);
		CollisionLayerMatrix.m_collisionMatrix[13] = 0;
		CollisionLayerMatrix.m_collisionMatrix[14] = 0;
		CollisionLayerMatrix.m_collisionMatrix[15] = CollisionMask.LayerToMask(CollisionLayer.Projectile);
		CollisionLayerMatrix.m_collisionMatrix[16] = CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.EnemyHitBox);
	}

	// Token: 0x0600926C RID: 37484 RVA: 0x003DDAE4 File Offset: 0x003DBCE4
	public static int GetMask(CollisionLayer layer)
	{
		return CollisionLayerMatrix.m_collisionMatrix[(int)layer];
	}

	// Token: 0x0600926D RID: 37485 RVA: 0x003DDAF0 File Offset: 0x003DBCF0
	public static bool CanCollide(CollisionLayer a, CollisionLayer b)
	{
		int num = 1 << (int)b;
		return (CollisionLayerMatrix.m_collisionMatrix[(int)a] & num) == num;
	}

	// Token: 0x04009A0D RID: 39437
	private static int[] m_collisionMatrix = new int[17];
}
