using System;

// Token: 0x0200182A RID: 6186
public static class CollisionMask
{
	// Token: 0x0600926F RID: 37487 RVA: 0x003DDB50 File Offset: 0x003DBD50
	public static int LayerToMask(CollisionLayer layer)
	{
		return 1 << (int)layer;
	}

	// Token: 0x06009270 RID: 37488 RVA: 0x003DDB58 File Offset: 0x003DBD58
	public static int LayerToMask(CollisionLayer layer1, CollisionLayer layer2)
	{
		return (1 << (int)layer1) | (1 << (int)layer2);
	}

	// Token: 0x06009271 RID: 37489 RVA: 0x003DDB68 File Offset: 0x003DBD68
	public static int LayerToMask(CollisionLayer layer1, CollisionLayer layer2, CollisionLayer layer3)
	{
		return (1 << (int)layer1) | (1 << (int)layer2) | (1 << (int)layer3);
	}

	// Token: 0x06009272 RID: 37490 RVA: 0x003DDB80 File Offset: 0x003DBD80
	public static int LayerToMask(CollisionLayer layer1, CollisionLayer layer2, CollisionLayer layer3, CollisionLayer layer4)
	{
		return (1 << (int)layer1) | (1 << (int)layer2) | (1 << (int)layer3) | (1 << (int)layer4);
	}

	// Token: 0x06009273 RID: 37491 RVA: 0x003DDBA0 File Offset: 0x003DBDA0
	public static int LayerToMask(CollisionLayer layer1, CollisionLayer layer2, CollisionLayer layer3, CollisionLayer layer4, CollisionLayer layer5)
	{
		return (1 << (int)layer1) | (1 << (int)layer2) | (1 << (int)layer3) | (1 << (int)layer4) | (1 << (int)layer5);
	}

	// Token: 0x06009274 RID: 37492 RVA: 0x003DDBC8 File Offset: 0x003DBDC8
	public static int LayerToMask(CollisionLayer layer1, CollisionLayer layer2, CollisionLayer layer3, CollisionLayer layer4, CollisionLayer layer5, CollisionLayer layer6)
	{
		return (1 << (int)layer1) | (1 << (int)layer2) | (1 << (int)layer3) | (1 << (int)layer4) | (1 << (int)layer5) | (1 << (int)layer6);
	}

	// Token: 0x06009275 RID: 37493 RVA: 0x003DDBF8 File Offset: 0x003DBDF8
	public static int LayerToMask(CollisionLayer layer1, CollisionLayer layer2, CollisionLayer layer3, CollisionLayer layer4, CollisionLayer layer5, CollisionLayer layer6, CollisionLayer layer7)
	{
		return (1 << (int)layer1) | (1 << (int)layer2) | (1 << (int)layer3) | (1 << (int)layer4) | (1 << (int)layer5) | (1 << (int)layer6) | (1 << (int)layer7);
	}

	// Token: 0x06009276 RID: 37494 RVA: 0x003DDC30 File Offset: 0x003DBE30
	public static int GetComplexEnemyVisibilityMask(bool canTargetPlayers, bool canTargetEnemies)
	{
		if (canTargetPlayers && canTargetEnemies)
		{
			return CollisionMask.BothEnemyVisibilityMask;
		}
		if (!canTargetEnemies)
		{
			return CollisionMask.StandardEnemyVisibilityMask;
		}
		if (!canTargetPlayers)
		{
			return CollisionMask.StandardPlayerVisibilityMask;
		}
		return CollisionMask.WallOnlyEnemyVisibilityMask;
	}

	// Token: 0x04009A0E RID: 39438
	public const int None = 0;

	// Token: 0x04009A0F RID: 39439
	public const int All = 2147483647;

	// Token: 0x04009A10 RID: 39440
	public static readonly int StandardPlayerVisibilityMask = CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider, CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker);

	// Token: 0x04009A11 RID: 39441
	public static readonly int StandardEnemyVisibilityMask = CollisionMask.LayerToMask(CollisionLayer.PlayerHitBox, CollisionLayer.PlayerCollider, CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, CollisionLayer.EnemyBulletBlocker);

	// Token: 0x04009A12 RID: 39442
	public static readonly int BothEnemyVisibilityMask = CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider, CollisionLayer.PlayerHitBox, CollisionLayer.PlayerCollider, CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, CollisionLayer.EnemyBulletBlocker);

	// Token: 0x04009A13 RID: 39443
	public static readonly int WallOnlyEnemyVisibilityMask = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker);
}
