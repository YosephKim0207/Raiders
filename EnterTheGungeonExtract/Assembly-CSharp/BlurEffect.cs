using System;
using UnityEngine;

// Token: 0x02000B75 RID: 2933
[AddComponentMenu("Image Effects/Blur")]
[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x06003D57 RID: 15703 RVA: 0x00132EC4 File Offset: 0x001310C4
	protected Material material
	{
		get
		{
			if (BlurEffect.m_Material == null)
			{
				BlurEffect.m_Material = new Material(this.blurShader);
				BlurEffect.m_Material.hideFlags = HideFlags.DontSave;
			}
			return BlurEffect.m_Material;
		}
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x00132EF8 File Offset: 0x001310F8
	protected void OnDisable()
	{
		if (BlurEffect.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(BlurEffect.m_Material);
		}
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x00132F14 File Offset: 0x00131114
	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.blurShader || !this.material.shader.isSupported)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06003D5A RID: 15706 RVA: 0x00132F60 File Offset: 0x00131160
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06003D5B RID: 15707 RVA: 0x00132FE0 File Offset: 0x001311E0
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06003D5C RID: 15708 RVA: 0x00133058 File Offset: 0x00131258
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
		this.DownSample4x(source, temporary);
		bool flag = true;
		for (int i = 0; i < this.iterations; i++)
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
		if (flag)
		{
			Graphics.Blit(temporary, destination);
		}
		else
		{
			Graphics.Blit(temporary2, destination);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}

	// Token: 0x04002FB6 RID: 12214
	public int iterations = 3;

	// Token: 0x04002FB7 RID: 12215
	public float blurSpread = 0.6f;

	// Token: 0x04002FB8 RID: 12216
	public Shader blurShader;

	// Token: 0x04002FB9 RID: 12217
	private static Material m_Material;
}
