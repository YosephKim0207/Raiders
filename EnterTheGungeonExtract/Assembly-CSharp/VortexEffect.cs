using System;
using UnityEngine;

// Token: 0x02000B83 RID: 2947
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Vortex")]
public class VortexEffect : ImageEffectBase
{
	// Token: 0x06003D97 RID: 15767 RVA: 0x00134640 File Offset: 0x00132840
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
	}

	// Token: 0x04002FFE RID: 12286
	public Vector2 radius = new Vector2(0.4f, 0.4f);

	// Token: 0x04002FFF RID: 12287
	public float angle = 50f;

	// Token: 0x04003000 RID: 12288
	public Vector2 center = new Vector2(0.5f, 0.5f);
}
