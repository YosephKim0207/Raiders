using System;
using UnityEngine;

// Token: 0x020003C9 RID: 969
[Serializable]
public abstract class dfFontBase : MonoBehaviour
{
	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x0600126D RID: 4717
	// (set) Token: 0x0600126E RID: 4718
	public abstract Material Material { get; set; }

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x0600126F RID: 4719
	public abstract Texture Texture { get; }

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06001270 RID: 4720
	public abstract bool IsValid { get; }

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06001271 RID: 4721
	// (set) Token: 0x06001272 RID: 4722
	public abstract int FontSize { get; set; }

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06001273 RID: 4723
	// (set) Token: 0x06001274 RID: 4724
	public abstract int LineHeight { get; set; }

	// Token: 0x06001275 RID: 4725
	public abstract dfFontRendererBase ObtainRenderer();

	// Token: 0x06001276 RID: 4726 RVA: 0x000552A8 File Offset: 0x000534A8
	public bool IsSpriteScaledUIFont()
	{
		if (!this.m_hasCachedScaling)
		{
			this.m_cachedScaling = base.name == "04b03_df40";
		}
		return this.m_cachedScaling;
	}

	// Token: 0x04001040 RID: 4160
	private bool m_hasCachedScaling;

	// Token: 0x04001041 RID: 4161
	private bool m_cachedScaling;
}
