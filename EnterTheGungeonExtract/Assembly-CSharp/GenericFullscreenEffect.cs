using System;
using UnityEngine;

// Token: 0x0200153A RID: 5434
public class GenericFullscreenEffect : MonoBehaviour
{
	// Token: 0x1700125C RID: 4700
	// (get) Token: 0x06007C57 RID: 31831 RVA: 0x003209C0 File Offset: 0x0031EBC0
	// (set) Token: 0x06007C58 RID: 31832 RVA: 0x003209C8 File Offset: 0x0031EBC8
	public bool CacheCurrentFrameToBuffer
	{
		get
		{
			return this.m_cacheCurrentFrameToBuffer;
		}
		set
		{
			this.m_cacheCurrentFrameToBuffer = value;
		}
	}

	// Token: 0x1700125D RID: 4701
	// (get) Token: 0x06007C59 RID: 31833 RVA: 0x003209D4 File Offset: 0x0031EBD4
	public Material ActiveMaterial
	{
		get
		{
			return this.m_material;
		}
	}

	// Token: 0x06007C5A RID: 31834 RVA: 0x003209DC File Offset: 0x0031EBDC
	private void Awake()
	{
		if (this.materialInstance != null)
		{
			this.m_material = this.materialInstance;
		}
		else
		{
			this.m_material = new Material(this.shader);
		}
	}

	// Token: 0x06007C5B RID: 31835 RVA: 0x00320A14 File Offset: 0x0031EC14
	public void SetMaterial(Material m)
	{
		this.m_material = m;
	}

	// Token: 0x06007C5C RID: 31836 RVA: 0x00320A20 File Offset: 0x0031EC20
	public RenderTexture GetCachedFrame()
	{
		return this.m_cachedFrame;
	}

	// Token: 0x06007C5D RID: 31837 RVA: 0x00320A28 File Offset: 0x0031EC28
	public void ClearCachedFrame()
	{
		if (this.m_cachedFrame != null)
		{
			RenderTexture.ReleaseTemporary(this.m_cachedFrame);
		}
		this.m_cachedFrame = null;
	}

	// Token: 0x06007C5E RID: 31838 RVA: 0x00320A50 File Offset: 0x0031EC50
	private void OnRenderImage(RenderTexture source, RenderTexture target)
	{
		if (!this.dualPass)
		{
			Graphics.Blit(source, target, this.m_material);
		}
		else
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, temporary, this.m_material, 0);
			Graphics.Blit(temporary, target, this.m_material, 1);
			RenderTexture.ReleaseTemporary(temporary);
		}
		if (this.CacheCurrentFrameToBuffer)
		{
			this.ClearCachedFrame();
			this.m_cachedFrame = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			this.m_cachedFrame.filterMode = FilterMode.Point;
			Graphics.Blit(source, this.m_cachedFrame, this.m_material);
			this.CacheCurrentFrameToBuffer = false;
		}
	}

	// Token: 0x04007F4D RID: 32589
	public Shader shader;

	// Token: 0x04007F4E RID: 32590
	public bool dualPass;

	// Token: 0x04007F4F RID: 32591
	public Material materialInstance;

	// Token: 0x04007F50 RID: 32592
	private bool m_cacheCurrentFrameToBuffer;

	// Token: 0x04007F51 RID: 32593
	[SerializeField]
	protected Material m_material;

	// Token: 0x04007F52 RID: 32594
	private RenderTexture m_cachedFrame;
}
