using System;
using UnityEngine;

// Token: 0x02000866 RID: 2150
public abstract class CastResult
{
	// Token: 0x0400208D RID: 8333
	public Vector2 Contact;

	// Token: 0x0400208E RID: 8334
	public Vector2 Normal;

	// Token: 0x0400208F RID: 8335
	public PixelCollider MyPixelCollider;

	// Token: 0x04002090 RID: 8336
	public PixelCollider OtherPixelCollider;
}
