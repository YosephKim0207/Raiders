using System;
using UnityEngine;

// Token: 0x020012C8 RID: 4808
[ExecuteInEditMode]
public class WaterBase : MonoBehaviour
{
	// Token: 0x06006B94 RID: 27540 RVA: 0x002A45B8 File Offset: 0x002A27B8
	public void UpdateShader()
	{
		if (this.waterQuality > WaterQuality.Medium)
		{
			this.sharedMaterial.shader.maximumLOD = 501;
		}
		else if (this.waterQuality > WaterQuality.Low)
		{
			this.sharedMaterial.shader.maximumLOD = 301;
		}
		else
		{
			this.sharedMaterial.shader.maximumLOD = 201;
		}
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.edgeBlend = false;
		}
		if (this.edgeBlend)
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_ON");
			Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
			if (Camera.main)
			{
				Camera.main.depthTextureMode |= DepthTextureMode.Depth;
			}
		}
		else
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
			Shader.DisableKeyword("WATER_EDGEBLEND_ON");
		}
	}

	// Token: 0x06006B95 RID: 27541 RVA: 0x002A4694 File Offset: 0x002A2894
	public void WaterTileBeingRendered(Transform tr, Camera currentCam)
	{
		if (currentCam && this.edgeBlend)
		{
			currentCam.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	// Token: 0x06006B96 RID: 27542 RVA: 0x002A46BC File Offset: 0x002A28BC
	public void Update()
	{
		if (this.sharedMaterial)
		{
			this.UpdateShader();
		}
	}

	// Token: 0x04006888 RID: 26760
	public Material sharedMaterial;

	// Token: 0x04006889 RID: 26761
	public WaterQuality waterQuality = WaterQuality.High;

	// Token: 0x0400688A RID: 26762
	public bool edgeBlend = true;
}
