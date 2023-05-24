using System;
using UnityEngine;

// Token: 0x0200085D RID: 2141
public class CollisionData : CastResult
{
	// Token: 0x06002F40 RID: 12096 RVA: 0x000F9334 File Offset: 0x000F7534
	private CollisionData()
	{
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x06002F41 RID: 12097 RVA: 0x000F933C File Offset: 0x000F753C
	public Vector2 PostCollisionUnitCenter
	{
		get
		{
			return this.MyRigidbody.UnitCenter + PhysicsEngine.PixelToUnit(this.NewPixelsToMove);
		}
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x000F935C File Offset: 0x000F755C
	public void SetAll(LinearCastResult res)
	{
		this.Contact = res.Contact;
		this.Normal = res.Normal;
		this.MyPixelCollider = res.MyPixelCollider;
		this.OtherPixelCollider = res.OtherPixelCollider;
		this.TimeUsed = res.TimeUsed;
		this.NewPixelsToMove = res.NewPixelsToMove;
		this.CollidedX = res.CollidedX;
		this.CollidedY = res.CollidedY;
		this.Overlap = res.Overlap;
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x000F93D8 File Offset: 0x000F75D8
	public void SetAll(CollisionData data)
	{
		this.Contact = data.Contact;
		this.Normal = data.Normal;
		this.MyPixelCollider = data.MyPixelCollider;
		this.OtherPixelCollider = data.OtherPixelCollider;
		this.TimeUsed = data.TimeUsed;
		this.NewPixelsToMove = data.NewPixelsToMove;
		this.CollidedX = data.CollidedX;
		this.CollidedY = data.CollidedY;
		this.Overlap = data.Overlap;
		this.collisionType = data.collisionType;
		this.MyRigidbody = data.MyRigidbody;
		this.OtherRigidbody = data.OtherRigidbody;
		this.TileLayerName = data.TileLayerName;
		this.TilePosition = data.TilePosition;
		this.IsPushCollision = data.IsPushCollision;
		this.IsInverse = data.IsInverse;
	}

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x06002F44 RID: 12100 RVA: 0x000F94A8 File Offset: 0x000F76A8
	public bool IsTriggerCollision
	{
		get
		{
			return (this.MyPixelCollider != null && this.MyPixelCollider.IsTrigger) || (this.OtherPixelCollider != null && this.OtherPixelCollider.IsTrigger);
		}
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x000F94E4 File Offset: 0x000F76E4
	public CollisionData GetInverse()
	{
		CollisionData collisionData = CollisionData.Pool.Allocate();
		collisionData.Contact = this.Contact;
		collisionData.Normal = -this.Normal;
		collisionData.MyPixelCollider = this.OtherPixelCollider;
		collisionData.OtherPixelCollider = this.MyPixelCollider;
		collisionData.TimeUsed = this.TimeUsed;
		collisionData.CollidedX = this.CollidedX;
		collisionData.CollidedY = this.CollidedY;
		collisionData.NewPixelsToMove = new IntVector2(-this.NewPixelsToMove.x, -this.NewPixelsToMove.y);
		collisionData.Overlap = this.Overlap;
		collisionData.collisionType = this.collisionType;
		collisionData.MyRigidbody = this.OtherRigidbody;
		collisionData.OtherRigidbody = this.MyRigidbody;
		collisionData.TileLayerName = this.TileLayerName;
		collisionData.TilePosition = this.TilePosition;
		collisionData.IsPushCollision = this.IsPushCollision;
		collisionData.IsInverse = true;
		return collisionData;
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x000F95D4 File Offset: 0x000F77D4
	public static void Cleanup(CollisionData collisionData)
	{
		collisionData.Contact.x = 0f;
		collisionData.Contact.y = 0f;
		collisionData.Normal.x = 0f;
		collisionData.Normal.y = 0f;
		collisionData.MyPixelCollider = null;
		collisionData.OtherPixelCollider = null;
		collisionData.TimeUsed = 0f;
		collisionData.CollidedX = false;
		collisionData.CollidedY = false;
		collisionData.NewPixelsToMove.x = 0;
		collisionData.NewPixelsToMove.y = 0;
		collisionData.Overlap = false;
		collisionData.collisionType = CollisionData.CollisionType.Rigidbody;
		collisionData.MyRigidbody = null;
		collisionData.OtherRigidbody = null;
		collisionData.TileLayerName = null;
		collisionData.TilePosition.x = 0;
		collisionData.TilePosition.y = 0;
		collisionData.IsPushCollision = false;
		collisionData.IsInverse = false;
	}

	// Token: 0x04002039 RID: 8249
	public float TimeUsed;

	// Token: 0x0400203A RID: 8250
	public bool CollidedX;

	// Token: 0x0400203B RID: 8251
	public bool CollidedY;

	// Token: 0x0400203C RID: 8252
	public IntVector2 NewPixelsToMove;

	// Token: 0x0400203D RID: 8253
	public bool Overlap;

	// Token: 0x0400203E RID: 8254
	public CollisionData.CollisionType collisionType;

	// Token: 0x0400203F RID: 8255
	public SpeculativeRigidbody MyRigidbody;

	// Token: 0x04002040 RID: 8256
	public SpeculativeRigidbody OtherRigidbody;

	// Token: 0x04002041 RID: 8257
	public string TileLayerName;

	// Token: 0x04002042 RID: 8258
	public IntVector2 TilePosition;

	// Token: 0x04002043 RID: 8259
	public bool IsPushCollision;

	// Token: 0x04002044 RID: 8260
	public bool IsInverse;

	// Token: 0x04002045 RID: 8261
	public static ObjectPool<CollisionData> Pool = new ObjectPool<CollisionData>(() => new CollisionData(), 10, new ObjectPool<CollisionData>.Cleanup(CollisionData.Cleanup));

	// Token: 0x0200085E RID: 2142
	public enum CollisionType
	{
		// Token: 0x04002048 RID: 8264
		Rigidbody,
		// Token: 0x04002049 RID: 8265
		TileMap,
		// Token: 0x0400204A RID: 8266
		PathEnd,
		// Token: 0x0400204B RID: 8267
		MovementRestriction,
		// Token: 0x0400204C RID: 8268
		Pushable
	}
}
