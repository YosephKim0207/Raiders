using System;

// Token: 0x02001041 RID: 4161
public class HelicopterController : BraveBehaviour
{
	// Token: 0x06005B54 RID: 23380 RVA: 0x0022FD50 File Offset: 0x0022DF50
	public void Start()
	{
		base.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox));
	}
}
