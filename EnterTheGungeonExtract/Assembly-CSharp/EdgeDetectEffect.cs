using System;
using UnityEngine;

// Token: 0x02000B78 RID: 2936
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Edge Detection (Color)")]
public class EdgeDetectEffect : ImageEffectBase
{
	// Token: 0x06003D6B RID: 15723 RVA: 0x00133530 File Offset: 0x00131730
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Treshold", this.threshold * this.threshold);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04002FC8 RID: 12232
	public float threshold = 0.2f;
}
