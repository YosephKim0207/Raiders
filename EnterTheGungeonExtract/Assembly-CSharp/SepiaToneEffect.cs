using System;
using UnityEngine;

// Token: 0x02000B7F RID: 2943
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Sepia Tone")]
public class SepiaToneEffect : ImageEffectBase
{
	// Token: 0x06003D8B RID: 15755 RVA: 0x001340F0 File Offset: 0x001322F0
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, base.material);
	}
}
