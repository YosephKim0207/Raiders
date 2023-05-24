using System;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Twirl")]
public class TwirlEffect : ImageEffectBase
{
	// Token: 0x06003D95 RID: 15765 RVA: 0x001345DC File Offset: 0x001327DC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
	}

	// Token: 0x04002FFB RID: 12283
	public Vector2 radius = new Vector2(0.3f, 0.3f);

	// Token: 0x04002FFC RID: 12284
	public float angle = 50f;

	// Token: 0x04002FFD RID: 12285
	public Vector2 center = new Vector2(0.5f, 0.5f);
}
