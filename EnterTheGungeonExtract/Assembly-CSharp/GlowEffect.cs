using System;
using UnityEngine;

// Token: 0x02000B79 RID: 2937
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Glow")]
public class GlowEffect : MonoBehaviour
{
	// Token: 0x1700092E RID: 2350
	// (get) Token: 0x06003D6D RID: 15725 RVA: 0x001335AC File Offset: 0x001317AC
	protected Material compositeMaterial
	{
		get
		{
			if (this.m_CompositeMaterial == null)
			{
				this.m_CompositeMaterial = new Material(this.compositeShader);
				this.m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_CompositeMaterial;
		}
	}

	// Token: 0x1700092F RID: 2351
	// (get) Token: 0x06003D6E RID: 15726 RVA: 0x001335E4 File Offset: 0x001317E4
	protected Material blurMaterial
	{
		get
		{
			if (this.m_BlurMaterial == null)
			{
				this.m_BlurMaterial = new Material(this.blurShader);
				this.m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_BlurMaterial;
		}
	}

	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x06003D6F RID: 15727 RVA: 0x0013361C File Offset: 0x0013181C
	protected Material downsampleMaterial
	{
		get
		{
			if (this.m_DownsampleMaterial == null)
			{
				this.m_DownsampleMaterial = new Material(this.downsampleShader);
				this.m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_DownsampleMaterial;
		}
	}

	// Token: 0x06003D70 RID: 15728 RVA: 0x00133654 File Offset: 0x00131854
	protected void OnDisable()
	{
		if (this.m_CompositeMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.m_CompositeMaterial);
		}
		if (this.m_BlurMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.m_BlurMaterial);
		}
		if (this.m_DownsampleMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.m_DownsampleMaterial);
		}
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x001336B4 File Offset: 0x001318B4
	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (this.downsampleShader == null)
		{
			Debug.Log("No downsample shader assigned! Disabling glow.");
			base.enabled = false;
		}
		else
		{
			if (!this.blurMaterial.shader.isSupported)
			{
				base.enabled = false;
			}
			if (!this.compositeMaterial.shader.isSupported)
			{
				base.enabled = false;
			}
			if (!this.downsampleMaterial.shader.isSupported)
			{
				base.enabled = false;
			}
		}
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x00133750 File Offset: 0x00131950
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2[]
		{
			new Vector2(num, num),
			new Vector2(-num, num),
			new Vector2(num, -num),
			new Vector2(-num, -num)
		});
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x001337D0 File Offset: 0x001319D0
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		this.downsampleMaterial.color = new Color(this.glowTint.r, this.glowTint.g, this.glowTint.b, this.glowTint.a / 4f);
		Graphics.Blit(source, dest, this.downsampleMaterial);
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x0013382C File Offset: 0x00131A2C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.glowIntensity = Mathf.Clamp(this.glowIntensity, 0f, 10f);
		this.blurIterations = Mathf.Clamp(this.blurIterations, 0, 30);
		this.blurSpread = Mathf.Clamp(this.blurSpread, 0.5f, 1f);
		RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
		this.DownSample4x(source, temporary);
		float num = Mathf.Clamp01((this.glowIntensity - 1f) / 4f);
		this.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + num);
		bool flag = true;
		for (int i = 0; i < this.blurIterations; i++)
		{
			if (flag)
			{
				this.FourTapCone(temporary, temporary2, i);
			}
			else
			{
				this.FourTapCone(temporary2, temporary, i);
			}
			flag = !flag;
		}
		Graphics.Blit(source, destination);
		if (flag)
		{
			this.BlitGlow(temporary, destination);
		}
		else
		{
			this.BlitGlow(temporary2, destination);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}

	// Token: 0x06003D75 RID: 15733 RVA: 0x00133968 File Offset: 0x00131B68
	public void BlitGlow(RenderTexture source, RenderTexture dest)
	{
		this.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(this.glowIntensity));
		Graphics.Blit(source, dest, this.compositeMaterial);
	}

	// Token: 0x04002FC9 RID: 12233
	public float glowIntensity = 1.5f;

	// Token: 0x04002FCA RID: 12234
	public int blurIterations = 3;

	// Token: 0x04002FCB RID: 12235
	public float blurSpread = 0.7f;

	// Token: 0x04002FCC RID: 12236
	public Color glowTint = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04002FCD RID: 12237
	public Shader compositeShader;

	// Token: 0x04002FCE RID: 12238
	private Material m_CompositeMaterial;

	// Token: 0x04002FCF RID: 12239
	public Shader blurShader;

	// Token: 0x04002FD0 RID: 12240
	private Material m_BlurMaterial;

	// Token: 0x04002FD1 RID: 12241
	public Shader downsampleShader;

	// Token: 0x04002FD2 RID: 12242
	private Material m_DownsampleMaterial;
}
