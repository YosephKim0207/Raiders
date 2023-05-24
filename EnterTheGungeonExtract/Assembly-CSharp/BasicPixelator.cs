using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001259 RID: 4697
public class BasicPixelator : MonoBehaviour
{
	// Token: 0x06006952 RID: 26962 RVA: 0x0029383C File Offset: 0x00291A3C
	private void CheckSize()
	{
		if (this.m_camera == null)
		{
			this.m_camera = base.GetComponent<Camera>();
		}
		BraveCameraUtility.MaintainCameraAspect(this.m_camera);
		this.m_camera.orthographicSize = 8.4375f;
	}

	// Token: 0x06006953 RID: 26963 RVA: 0x00293878 File Offset: 0x00291A78
	private void OnEnable()
	{
		if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
		{
			this.FINAL_CAMERA_POSITION_OFFSET = Vector3.zero;
		}
		else
		{
			this.FINAL_CAMERA_POSITION_OFFSET = new Vector3(0.03125f, 0.03125f, 0f);
		}
		base.transform.position += this.FINAL_CAMERA_POSITION_OFFSET;
	}

	// Token: 0x06006954 RID: 26964 RVA: 0x002938D8 File Offset: 0x00291AD8
	private void OnDisable()
	{
		base.transform.position -= this.FINAL_CAMERA_POSITION_OFFSET;
	}

	// Token: 0x06006955 RID: 26965 RVA: 0x002938F8 File Offset: 0x00291AF8
	private void OnRenderImage(RenderTexture source, RenderTexture target)
	{
		this.CheckSize();
		RenderTexture temporary = RenderTexture.GetTemporary(BraveCameraUtility.H_PIXELS, BraveCameraUtility.V_PIXELS, 0, RenderTextureFormat.Default);
		if (!temporary.IsCreated())
		{
			temporary.Create();
		}
		Graphics.Blit(Pixelator.SmallBlackTexture, temporary);
		source.filterMode = FilterMode.Point;
		temporary.filterMode = FilterMode.Point;
		Graphics.Blit(source, temporary);
		Graphics.Blit(temporary, target);
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x040065AD RID: 26029
	private Vector3 FINAL_CAMERA_POSITION_OFFSET;

	// Token: 0x040065AE RID: 26030
	private Camera m_camera;
}
