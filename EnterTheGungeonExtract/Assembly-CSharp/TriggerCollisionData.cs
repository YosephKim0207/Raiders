using System;

// Token: 0x02000869 RID: 2153
public class TriggerCollisionData
{
	// Token: 0x06002FC1 RID: 12225 RVA: 0x000FCAE4 File Offset: 0x000FACE4
	public TriggerCollisionData(SpeculativeRigidbody specRigidbody, PixelCollider pixelCollider)
	{
		this.SpecRigidbody = specRigidbody;
		this.PixelCollider = pixelCollider;
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x000FCB04 File Offset: 0x000FAD04
	public void Reset()
	{
		this.FirstFrame = false;
		this.ContinuedCollision = false;
		this.Notified = false;
	}

	// Token: 0x0400209E RID: 8350
	public PixelCollider PixelCollider;

	// Token: 0x0400209F RID: 8351
	public SpeculativeRigidbody SpecRigidbody;

	// Token: 0x040020A0 RID: 8352
	public bool FirstFrame = true;

	// Token: 0x040020A1 RID: 8353
	public bool ContinuedCollision;

	// Token: 0x040020A2 RID: 8354
	public bool Notified;
}
