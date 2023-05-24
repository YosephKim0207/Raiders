using System;

// Token: 0x02000867 RID: 2151
public class RaycastResult : CastResult
{
	// Token: 0x06002FB9 RID: 12217 RVA: 0x000FC920 File Offset: 0x000FAB20
	private RaycastResult()
	{
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x000FC928 File Offset: 0x000FAB28
	public static void Cleanup(RaycastResult raycastResult)
	{
		raycastResult.Contact.x = 0f;
		raycastResult.Contact.y = 0f;
		raycastResult.Normal.x = 0f;
		raycastResult.Normal.y = 0f;
		raycastResult.MyPixelCollider = null;
		raycastResult.OtherPixelCollider = null;
		raycastResult.HitPixel.x = 0;
		raycastResult.HitPixel.y = 0;
		raycastResult.LastRayPixel.x = 0;
		raycastResult.LastRayPixel.y = 0;
		raycastResult.Distance = 0f;
		raycastResult.SpeculativeRigidbody = null;
	}

	// Token: 0x04002091 RID: 8337
	public IntVector2 HitPixel;

	// Token: 0x04002092 RID: 8338
	public IntVector2 LastRayPixel;

	// Token: 0x04002093 RID: 8339
	public float Distance;

	// Token: 0x04002094 RID: 8340
	public SpeculativeRigidbody SpeculativeRigidbody;

	// Token: 0x04002095 RID: 8341
	public static ObjectPool<RaycastResult> Pool = new ObjectPool<RaycastResult>(() => new RaycastResult(), 10, new ObjectPool<RaycastResult>.Cleanup(RaycastResult.Cleanup));
}
