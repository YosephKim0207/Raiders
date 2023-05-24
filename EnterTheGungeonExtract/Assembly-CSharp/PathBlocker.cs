using System;
using UnityEngine;

// Token: 0x020015B2 RID: 5554
public class PathBlocker : BraveBehaviour
{
	// Token: 0x06007F6E RID: 32622 RVA: 0x00336EFC File Offset: 0x003350FC
	public static void BlockRigidbody(SpeculativeRigidbody rigidbody, bool blockGoopsToo)
	{
		foreach (PixelCollider pixelCollider in rigidbody.PixelColliders)
		{
			if (!pixelCollider.IsTrigger)
			{
				if (pixelCollider.CollisionLayer == CollisionLayer.LowObstacle || pixelCollider.CollisionLayer == CollisionLayer.HighObstacle || pixelCollider.CollisionLayer == CollisionLayer.EnemyBlocker)
				{
					if (pixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Line)
					{
						Vector2 vector = rigidbody.transform.position.XY() + PhysicsEngine.PixelToUnit(new IntVector2(pixelCollider.ManualLeftX, pixelCollider.ManualLeftY));
						Vector2 vector2 = rigidbody.transform.position.XY() + PhysicsEngine.PixelToUnit(new IntVector2(pixelCollider.ManualRightX, pixelCollider.ManualRightY));
						float num = Vector2.Distance(vector, vector2);
						Vector2 normalized = (vector2 - vector).normalized;
						for (float num2 = 0f; num2 <= num; num2 += 0.1f)
						{
							IntVector2 intVector = (vector + normalized * num2).ToIntVector2(VectorConversions.Floor);
							GameManager.Instance.Dungeon.data[intVector].isOccupied = true;
							if (blockGoopsToo)
							{
								GameManager.Instance.Dungeon.data[intVector].forceDisallowGoop = true;
							}
						}
						GameManager.Instance.Dungeon.data[vector2.ToIntVector2(VectorConversions.Floor)].isOccupied = true;
						if (blockGoopsToo)
						{
							GameManager.Instance.Dungeon.data[vector2.ToIntVector2(VectorConversions.Floor)].forceDisallowGoop = true;
						}
					}
					else
					{
						IntVector2 intVector2 = pixelCollider.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
						IntVector2 intVector3 = pixelCollider.UnitTopRight.ToIntVector2(VectorConversions.Ceil);
						for (int i = intVector2.x; i < intVector3.x; i++)
						{
							for (int j = intVector2.y; j < intVector3.y; j++)
							{
								if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(new IntVector2(i, j)))
								{
									GameManager.Instance.Dungeon.data[i, j].isOccupied = true;
									if (blockGoopsToo)
									{
										GameManager.Instance.Dungeon.data[i, j].forceDisallowGoop = true;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06007F6F RID: 32623 RVA: 0x00337198 File Offset: 0x00335398
	public void Start()
	{
		if (!base.specRigidbody)
		{
			return;
		}
		base.specRigidbody.Initialize();
		PathBlocker.BlockRigidbody(base.specRigidbody, this.BlocksGoopsToo);
	}

	// Token: 0x0400821C RID: 33308
	public bool BlocksGoopsToo;
}
