using System;
using UnityEngine;

// Token: 0x020012B4 RID: 4788
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Sonic Ether/SE Natural Bloom and Dirty Lens")]
public class SENaturalBloomAndDirtyLens : MonoBehaviour
{
	// Token: 0x06006B1F RID: 27423 RVA: 0x002A1674 File Offset: 0x0029F874
	private void Start()
	{
		this.isSupported = true;
		if (!this.material)
		{
			this.material = new Material(this.shader);
		}
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
		{
			this.isSupported = false;
		}
	}

	// Token: 0x06006B20 RID: 27424 RVA: 0x002A16D0 File Offset: 0x0029F8D0
	private void OnDisable()
	{
		if (this.material)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
		}
	}

	// Token: 0x17000FE4 RID: 4068
	// (get) Token: 0x06006B21 RID: 27425 RVA: 0x002A16F0 File Offset: 0x0029F8F0
	protected int IterationCount
	{
		get
		{
			if (!Application.isPlaying)
			{
				return 1;
			}
			if (GameManager.Options != null && GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH)
			{
				return 2;
			}
			return 1;
		}
	}

	// Token: 0x06006B22 RID: 27426 RVA: 0x002A171C File Offset: 0x0029F91C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.overrideDisable)
		{
			return;
		}
		if (!this.isSupported)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (!this.material)
		{
			this.material = new Material(this.shader);
		}
		this.material.hideFlags = HideFlags.HideAndDontSave;
		this.material.SetFloat("_BloomIntensity", Mathf.Exp(this.bloomIntensity) - 1f);
		this.material.SetFloat("_LensDirtIntensity", Mathf.Exp(this.lensDirtIntensity) - 1f);
		source.filterMode = FilterMode.Bilinear;
		int num = source.width / 2;
		int num2 = source.height / 2;
		RenderTexture renderTexture = source;
		int iterationCount = this.IterationCount;
		for (int i = 0; i < 6; i++)
		{
			RenderTexture renderTexture2 = RenderTexture.GetTemporary(num, num2, 0, source.format);
			renderTexture2.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, renderTexture2, this.material, 1);
			renderTexture = renderTexture2;
			float num3;
			if (i > 1)
			{
				num3 = 1f;
			}
			else
			{
				num3 = 0.5f;
			}
			if (i == 2)
			{
				num3 = 0.75f;
			}
			for (int j = 0; j < iterationCount; j++)
			{
				this.material.SetFloat("_BlurSize", (this.blurSize * 0.5f + (float)j) * num3);
				RenderTexture renderTexture3 = RenderTexture.GetTemporary(num, num2, 0, source.format);
				renderTexture3.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture2, renderTexture3, this.material, 2);
				RenderTexture.ReleaseTemporary(renderTexture2);
				renderTexture2 = renderTexture3;
				renderTexture3 = RenderTexture.GetTemporary(num, num2, 0, source.format);
				renderTexture3.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture2, renderTexture3, this.material, 3);
				RenderTexture.ReleaseTemporary(renderTexture2);
				renderTexture2 = renderTexture3;
			}
			switch (i)
			{
			case 0:
				this.material.SetTexture("_Bloom0", renderTexture2);
				break;
			case 1:
				this.material.SetTexture("_Bloom1", renderTexture2);
				break;
			case 2:
				this.material.SetTexture("_Bloom2", renderTexture2);
				break;
			case 3:
				this.material.SetTexture("_Bloom3", renderTexture2);
				break;
			case 4:
				this.material.SetTexture("_Bloom4", renderTexture2);
				break;
			case 5:
				this.material.SetTexture("_Bloom5", renderTexture2);
				break;
			}
			RenderTexture.ReleaseTemporary(renderTexture2);
			num /= 2;
			num2 /= 2;
		}
		this.material.SetTexture("_LensDirt", this.lensDirtTexture);
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x04006807 RID: 26631
	[Range(0f, 0.4f)]
	public float bloomIntensity = 0.05f;

	// Token: 0x04006808 RID: 26632
	public Shader shader;

	// Token: 0x04006809 RID: 26633
	private Material material;

	// Token: 0x0400680A RID: 26634
	public Texture2D lensDirtTexture;

	// Token: 0x0400680B RID: 26635
	[Range(0f, 0.95f)]
	public float lensDirtIntensity = 0.05f;

	// Token: 0x0400680C RID: 26636
	private bool isSupported;

	// Token: 0x0400680D RID: 26637
	private float blurSize = 4f;

	// Token: 0x0400680E RID: 26638
	public bool inputIsHDR;

	// Token: 0x0400680F RID: 26639
	[HideInInspector]
	public bool overrideDisable;
}
