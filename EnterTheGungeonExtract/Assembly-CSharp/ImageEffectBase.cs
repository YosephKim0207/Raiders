using System;
using UnityEngine;

// Token: 0x02000B7B RID: 2939
[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class ImageEffectBase : MonoBehaviour
{
	// Token: 0x06003D79 RID: 15737 RVA: 0x001339F0 File Offset: 0x00131BF0
	protected virtual void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shader || !this.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06003D7A RID: 15738 RVA: 0x00133A2C File Offset: 0x00131C2C
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.shader);
				this.m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x00133A64 File Offset: 0x00131C64
	protected virtual void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x04002FD5 RID: 12245
	public Shader shader;

	// Token: 0x04002FD6 RID: 12246
	private Material m_Material;
}
