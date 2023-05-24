using System;
using UnityEngine;

// Token: 0x02000B76 RID: 2934
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Correction (Ramp)")]
public class ColorCorrectionEffect : ImageEffectBase
{
	// Token: 0x06003D5F RID: 15711 RVA: 0x00133108 File Offset: 0x00131308
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetTexture("_RampTex", this.textureRamp);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04002FBA RID: 12218
	public Texture textureRamp;
}
