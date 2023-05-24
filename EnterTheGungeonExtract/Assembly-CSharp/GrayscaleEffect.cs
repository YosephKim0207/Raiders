using System;
using UnityEngine;

// Token: 0x02000B7A RID: 2938
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Grayscale")]
public class GrayscaleEffect : ImageEffectBase
{
	// Token: 0x06003D77 RID: 15735 RVA: 0x001339AC File Offset: 0x00131BAC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetTexture("_RampTex", this.textureRamp);
		base.material.SetFloat("_RampOffset", this.rampOffset);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04002FD3 RID: 12243
	public Texture textureRamp;

	// Token: 0x04002FD4 RID: 12244
	public float rampOffset;
}
