using System;
using UnityEngine;

// Token: 0x02000B7E RID: 2942
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Noise")]
public class NoiseEffect : MonoBehaviour
{
	// Token: 0x06003D85 RID: 15749 RVA: 0x00133D38 File Offset: 0x00131F38
	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (this.shaderRGB == null || this.shaderYUV == null)
		{
			Debug.Log("Noise shaders are not set up! Disabling noise effect.");
			base.enabled = false;
		}
		else if (!this.shaderRGB.isSupported)
		{
			base.enabled = false;
		}
		else if (!this.shaderYUV.isSupported)
		{
			this.rgbFallback = true;
		}
	}

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x06003D86 RID: 15750 RVA: 0x00133DC4 File Offset: 0x00131FC4
	protected Material material
	{
		get
		{
			if (this.m_MaterialRGB == null)
			{
				this.m_MaterialRGB = new Material(this.shaderRGB);
				this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
			}
			if (this.m_MaterialYUV == null && !this.rgbFallback)
			{
				this.m_MaterialYUV = new Material(this.shaderYUV);
				this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
			}
			return (this.rgbFallback || this.monochrome) ? this.m_MaterialRGB : this.m_MaterialYUV;
		}
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x00133E64 File Offset: 0x00132064
	protected void OnDisable()
	{
		if (this.m_MaterialRGB)
		{
			UnityEngine.Object.DestroyImmediate(this.m_MaterialRGB);
		}
		if (this.m_MaterialYUV)
		{
			UnityEngine.Object.DestroyImmediate(this.m_MaterialYUV);
		}
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x00133E9C File Offset: 0x0013209C
	private void SanitizeParameters()
	{
		this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
		this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
		this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
		this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
		this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
		this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
		this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x00133F68 File Offset: 0x00132168
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SanitizeParameters();
		if (this.scratchTimeLeft <= 0f)
		{
			this.scratchTimeLeft = UnityEngine.Random.value * 2f / this.scratchFPS;
			this.scratchX = UnityEngine.Random.value;
			this.scratchY = UnityEngine.Random.value;
		}
		this.scratchTimeLeft -= BraveTime.DeltaTime;
		Material material = this.material;
		material.SetTexture("_GrainTex", this.grainTexture);
		material.SetTexture("_ScratchTex", this.scratchTexture);
		float num = 1f / this.grainSize;
		material.SetVector("_GrainOffsetScale", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, (float)Screen.width / (float)this.grainTexture.width * num, (float)Screen.height / (float)this.grainTexture.height * num));
		material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + UnityEngine.Random.value * this.scratchJitter, this.scratchY + UnityEngine.Random.value * this.scratchJitter, (float)Screen.width / (float)this.scratchTexture.width, (float)Screen.height / (float)this.scratchTexture.height));
		material.SetVector("_Intensity", new Vector4(UnityEngine.Random.Range(this.grainIntensityMin, this.grainIntensityMax), UnityEngine.Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
		Graphics.Blit(source, destination, material);
	}

	// Token: 0x04002FDA RID: 12250
	public bool monochrome = true;

	// Token: 0x04002FDB RID: 12251
	private bool rgbFallback;

	// Token: 0x04002FDC RID: 12252
	public float grainIntensityMin = 0.1f;

	// Token: 0x04002FDD RID: 12253
	public float grainIntensityMax = 0.2f;

	// Token: 0x04002FDE RID: 12254
	public float grainSize = 2f;

	// Token: 0x04002FDF RID: 12255
	public float scratchIntensityMin = 0.05f;

	// Token: 0x04002FE0 RID: 12256
	public float scratchIntensityMax = 0.25f;

	// Token: 0x04002FE1 RID: 12257
	public float scratchFPS = 10f;

	// Token: 0x04002FE2 RID: 12258
	public float scratchJitter = 0.01f;

	// Token: 0x04002FE3 RID: 12259
	public Texture grainTexture;

	// Token: 0x04002FE4 RID: 12260
	public Texture scratchTexture;

	// Token: 0x04002FE5 RID: 12261
	public Shader shaderRGB;

	// Token: 0x04002FE6 RID: 12262
	public Shader shaderYUV;

	// Token: 0x04002FE7 RID: 12263
	private Material m_MaterialRGB;

	// Token: 0x04002FE8 RID: 12264
	private Material m_MaterialYUV;

	// Token: 0x04002FE9 RID: 12265
	private float scratchTimeLeft;

	// Token: 0x04002FEA RID: 12266
	private float scratchX;

	// Token: 0x04002FEB RID: 12267
	private float scratchY;
}
