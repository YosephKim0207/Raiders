using System;
using UnityEngine;

// Token: 0x02000B77 RID: 2935
[AddComponentMenu("Image Effects/Contrast Stretch")]
[ExecuteInEditMode]
public class ContrastStretchEffect : MonoBehaviour
{
	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x06003D61 RID: 15713 RVA: 0x00133168 File Offset: 0x00131368
	protected Material materialLum
	{
		get
		{
			if (this.m_materialLum == null)
			{
				this.m_materialLum = new Material(this.shaderLum);
				this.m_materialLum.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_materialLum;
		}
	}

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06003D62 RID: 15714 RVA: 0x001331A0 File Offset: 0x001313A0
	protected Material materialReduce
	{
		get
		{
			if (this.m_materialReduce == null)
			{
				this.m_materialReduce = new Material(this.shaderReduce);
				this.m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_materialReduce;
		}
	}

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x06003D63 RID: 15715 RVA: 0x001331D8 File Offset: 0x001313D8
	protected Material materialAdapt
	{
		get
		{
			if (this.m_materialAdapt == null)
			{
				this.m_materialAdapt = new Material(this.shaderAdapt);
				this.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_materialAdapt;
		}
	}

	// Token: 0x1700092D RID: 2349
	// (get) Token: 0x06003D64 RID: 15716 RVA: 0x00133210 File Offset: 0x00131410
	protected Material materialApply
	{
		get
		{
			if (this.m_materialApply == null)
			{
				this.m_materialApply = new Material(this.shaderApply);
				this.m_materialApply.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_materialApply;
		}
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x00133248 File Offset: 0x00131448
	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shaderAdapt.isSupported || !this.shaderApply.isSupported || !this.shaderLum.isSupported || !this.shaderReduce.isSupported)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x001332B0 File Offset: 0x001314B0
	private void OnEnable()
	{
		for (int i = 0; i < 2; i++)
		{
			if (!this.adaptRenderTex[i])
			{
				this.adaptRenderTex[i] = new RenderTexture(1, 1, 32);
				this.adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
			}
		}
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x00133304 File Offset: 0x00131504
	private void OnDisable()
	{
		for (int i = 0; i < 2; i++)
		{
			UnityEngine.Object.DestroyImmediate(this.adaptRenderTex[i]);
			this.adaptRenderTex[i] = null;
		}
		if (this.m_materialLum)
		{
			UnityEngine.Object.DestroyImmediate(this.m_materialLum);
		}
		if (this.m_materialReduce)
		{
			UnityEngine.Object.DestroyImmediate(this.m_materialReduce);
		}
		if (this.m_materialAdapt)
		{
			UnityEngine.Object.DestroyImmediate(this.m_materialAdapt);
		}
		if (this.m_materialApply)
		{
			UnityEngine.Object.DestroyImmediate(this.m_materialApply);
		}
	}

	// Token: 0x06003D68 RID: 15720 RVA: 0x001333A8 File Offset: 0x001315A8
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height);
		Graphics.Blit(source, renderTexture, this.materialLum);
		while (renderTexture.width > 1 || renderTexture.height > 1)
		{
			int num = renderTexture.width / 2;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = renderTexture.height / 2;
			if (num2 < 1)
			{
				num2 = 1;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
			Graphics.Blit(renderTexture, temporary, this.materialReduce);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		this.CalculateAdaptation(renderTexture);
		this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
		Graphics.Blit(source, destination, this.materialApply);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x0013346C File Offset: 0x0013166C
	private void CalculateAdaptation(Texture curTexture)
	{
		int num = this.curAdaptIndex;
		this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
		float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * BraveTime.DeltaTime);
		num2 = Mathf.Clamp(num2, 0.01f, 1f);
		this.materialAdapt.SetTexture("_CurTex", curTexture);
		this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
		Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
	}

	// Token: 0x04002FBB RID: 12219
	public float adaptationSpeed = 0.02f;

	// Token: 0x04002FBC RID: 12220
	public float limitMinimum = 0.2f;

	// Token: 0x04002FBD RID: 12221
	public float limitMaximum = 0.6f;

	// Token: 0x04002FBE RID: 12222
	private RenderTexture[] adaptRenderTex = new RenderTexture[2];

	// Token: 0x04002FBF RID: 12223
	private int curAdaptIndex;

	// Token: 0x04002FC0 RID: 12224
	public Shader shaderLum;

	// Token: 0x04002FC1 RID: 12225
	private Material m_materialLum;

	// Token: 0x04002FC2 RID: 12226
	public Shader shaderReduce;

	// Token: 0x04002FC3 RID: 12227
	private Material m_materialReduce;

	// Token: 0x04002FC4 RID: 12228
	public Shader shaderAdapt;

	// Token: 0x04002FC5 RID: 12229
	private Material m_materialAdapt;

	// Token: 0x04002FC6 RID: 12230
	public Shader shaderApply;

	// Token: 0x04002FC7 RID: 12231
	private Material m_materialApply;
}
