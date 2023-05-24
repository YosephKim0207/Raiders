using System;
using UnityEngine;

// Token: 0x020017BF RID: 6079
public class MenuTK2DSpriteScaler : MonoBehaviour
{
	// Token: 0x06008E5E RID: 36446 RVA: 0x003BE230 File Offset: 0x003BC430
	private void Start()
	{
		this.m_manager = UnityEngine.Object.FindObjectOfType<dfGUIManager>();
		this.m_transform = base.transform;
		this.m_cachedFullscreen = Screen.fullScreen;
	}

	// Token: 0x06008E5F RID: 36447 RVA: 0x003BE254 File Offset: 0x003BC454
	private void LateUpdate()
	{
		float num = 1f;
		if (this.m_transform.parent != null)
		{
			num = this.m_transform.parent.lossyScale.x;
		}
		if (this.m_cachedWidth != Screen.width || this.m_cachedHeight != Screen.height || this.m_cachedFullscreen != Screen.fullScreen || this.m_cachedParentScale != num)
		{
			float num2 = (float)Screen.height * this.m_manager.RenderCamera.rect.height / this.TargetResolution * 4f;
			float num3 = num2 * 16f * this.m_manager.PixelsToUnits();
			this.m_transform.localScale = new Vector3(num3 / num, num3 / num, 1f);
			this.m_transform.position = this.m_transform.position.Quantize(this.m_manager.PixelsToUnits() * num2);
			this.m_cachedParentScale = num;
			this.m_cachedWidth = Screen.width;
			this.m_cachedHeight = Screen.height;
			this.m_cachedFullscreen = Screen.fullScreen;
		}
	}

	// Token: 0x04009652 RID: 38482
	[NonSerialized]
	protected float TargetResolution = 1080f;

	// Token: 0x04009653 RID: 38483
	protected Transform m_transform;

	// Token: 0x04009654 RID: 38484
	protected dfGUIManager m_manager;

	// Token: 0x04009655 RID: 38485
	protected int m_cachedWidth;

	// Token: 0x04009656 RID: 38486
	protected int m_cachedHeight;

	// Token: 0x04009657 RID: 38487
	protected bool m_cachedFullscreen;

	// Token: 0x04009658 RID: 38488
	protected float m_cachedParentScale = 1f;
}
