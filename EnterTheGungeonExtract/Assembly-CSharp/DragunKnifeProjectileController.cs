using System;
using UnityEngine;

// Token: 0x02001021 RID: 4129
public class DragunKnifeProjectileController : BraveBehaviour
{
	// Token: 0x06005A90 RID: 23184 RVA: 0x002298BC File Offset: 0x00227ABC
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
	}

	// Token: 0x06005A91 RID: 23185 RVA: 0x002298E8 File Offset: 0x00227AE8
	private void OnTileCollision(CollisionData tileCollision)
	{
		if (!base.projectile.Owner || !(base.projectile.Owner is AIActor))
		{
			return;
		}
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.knifeGuid);
		Vector2 contact = tileCollision.Contact;
		if (tileCollision.Normal.x < 0f)
		{
			contact.x -= PhysicsEngine.PixelToUnit(orLoadByGuid.specRigidbody.PrimaryPixelCollider.ManualWidth);
		}
		AIActor aiactor = AIActor.Spawn(orLoadByGuid, contact.ToIntVector2(VectorConversions.Round) + new IntVector2(0, -1), (base.projectile.Owner as AIActor).ParentRoom, false, AIActor.AwakenAnimationType.Default, true);
		aiactor.aiAnimator.LockFacingDirection = true;
		aiactor.aiAnimator.FacingDirection = (float)((tileCollision.Normal.x >= 0f) ? 0 : 180);
		aiactor.aiAnimator.Update();
		if (tileCollision.Normal.x < 0f)
		{
			PixelCollider primaryPixelCollider = aiactor.specRigidbody.PrimaryPixelCollider;
			int num = primaryPixelCollider.ManualWidth / 2;
			primaryPixelCollider.ManualOffsetX += num;
			primaryPixelCollider.ManualWidth -= num;
			aiactor.specRigidbody.ForceRegenerate(null, null);
		}
		else
		{
			PixelCollider primaryPixelCollider2 = aiactor.specRigidbody.PrimaryPixelCollider;
			int num2 = primaryPixelCollider2.ManualWidth / 2;
			primaryPixelCollider2.ManualWidth -= num2;
			aiactor.specRigidbody.ForceRegenerate(null, null);
		}
	}

	// Token: 0x040053FA RID: 21498
	[EnemyIdentifier]
	public string knifeGuid;
}
