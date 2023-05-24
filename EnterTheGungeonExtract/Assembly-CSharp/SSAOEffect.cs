using System;
using UnityEngine;

// Token: 0x02000B80 RID: 2944
[AddComponentMenu("Image Effects/Screen Space Ambient Occlusion")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class SSAOEffect : MonoBehaviour
{
	// Token: 0x06003D8D RID: 15757 RVA: 0x00134154 File Offset: 0x00132354
	private static Material CreateMaterial(Shader shader)
	{
		if (!shader)
		{
			return null;
		}
		return new Material(shader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
	}

	// Token: 0x06003D8E RID: 15758 RVA: 0x00134180 File Offset: 0x00132380
	private static void DestroyMaterial(Material mat)
	{
		if (mat)
		{
			UnityEngine.Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	// Token: 0x06003D8F RID: 15759 RVA: 0x00134198 File Offset: 0x00132398
	private void OnDisable()
	{
		SSAOEffect.DestroyMaterial(this.m_SSAOMaterial);
	}

	// Token: 0x06003D90 RID: 15760 RVA: 0x001341A8 File Offset: 0x001323A8
	private void Start()
	{
		if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.m_Supported = false;
			base.enabled = false;
			return;
		}
		this.CreateMaterials();
		if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
		{
			this.m_Supported = false;
			base.enabled = false;
			return;
		}
		this.m_Supported = true;
	}

	// Token: 0x06003D91 RID: 15761 RVA: 0x00134218 File Offset: 0x00132418
	private void OnEnable()
	{
		base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00134230 File Offset: 0x00132430
	private void CreateMaterials()
	{
		if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
		{
			this.m_SSAOMaterial = SSAOEffect.CreateMaterial(this.m_SSAOShader);
			this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
		}
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x00134284 File Offset: 0x00132484
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.m_Supported || !this.m_SSAOShader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.CreateMaterials();
		this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
		this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
		this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
		this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
		this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
		this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
		float fieldOfView = base.GetComponent<Camera>().fieldOfView;
		float farClipPlane = base.GetComponent<Camera>().farClipPlane;
		float num = Mathf.Tan(fieldOfView * 0.017453292f * 0.5f) * farClipPlane;
		float num2 = num * base.GetComponent<Camera>().aspect;
		this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(num2, num, farClipPlane));
		int num3;
		int num4;
		if (this.m_RandomTexture)
		{
			num3 = this.m_RandomTexture.width;
			num4 = this.m_RandomTexture.height;
		}
		else
		{
			num3 = 1;
			num4 = 1;
		}
		this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.width / (float)num3, (float)renderTexture.height / (float)num4, 0f));
		this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
		bool flag = this.m_Blur > 0;
		Graphics.Blit((!flag) ? source : null, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
		if (flag)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
			Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(temporary);
			renderTexture = temporary2;
		}
		this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
		Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x04002FEC RID: 12268
	public float m_Radius = 0.4f;

	// Token: 0x04002FED RID: 12269
	public SSAOEffect.SSAOSamples m_SampleCount = SSAOEffect.SSAOSamples.Medium;

	// Token: 0x04002FEE RID: 12270
	public float m_OcclusionIntensity = 1.5f;

	// Token: 0x04002FEF RID: 12271
	public int m_Blur = 2;

	// Token: 0x04002FF0 RID: 12272
	public int m_Downsampling = 2;

	// Token: 0x04002FF1 RID: 12273
	public float m_OcclusionAttenuation = 1f;

	// Token: 0x04002FF2 RID: 12274
	public float m_MinZ = 0.01f;

	// Token: 0x04002FF3 RID: 12275
	public Shader m_SSAOShader;

	// Token: 0x04002FF4 RID: 12276
	private Material m_SSAOMaterial;

	// Token: 0x04002FF5 RID: 12277
	public Texture2D m_RandomTexture;

	// Token: 0x04002FF6 RID: 12278
	private bool m_Supported;

	// Token: 0x02000B81 RID: 2945
	public enum SSAOSamples
	{
		// Token: 0x04002FF8 RID: 12280
		Low,
		// Token: 0x04002FF9 RID: 12281
		Medium,
		// Token: 0x04002FFA RID: 12282
		High
	}
}
