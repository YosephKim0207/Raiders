using System;
using UnityEngine;

// Token: 0x0200153B RID: 5435
public class MinimapFullscreenEffect : MonoBehaviour
{
	// Token: 0x06007C60 RID: 31840 RVA: 0x00320B24 File Offset: 0x0031ED24
	private void Awake()
	{
		this.m_camera = base.GetComponent<Camera>();
		this.m_cachedCullingMask = this.m_camera.cullingMask;
		this.m_camera.cullingMask = 0;
		if (this.materialInstance != null)
		{
			this.m_material = this.materialInstance;
		}
		else
		{
			this.m_material = new Material(this.shader);
		}
	}

	// Token: 0x06007C61 RID: 31841 RVA: 0x00320B90 File Offset: 0x0031ED90
	public void SetMaterial(Material m)
	{
		this.m_material = m;
	}

	// Token: 0x06007C62 RID: 31842 RVA: 0x00320B9C File Offset: 0x0031ED9C
	private void OnRenderImage(RenderTexture source, RenderTexture target)
	{
		if (GameManager.Instance.IsFoyer)
		{
			return;
		}
		this.slaveCamera.CopyFrom(this.m_camera);
		this.slaveCamera.clearFlags = CameraClearFlags.Color;
		Rect rect = new Rect(1f - Minimap.Instance.currentXRectFactor, 1f - Minimap.Instance.currentYRectFactor, Minimap.Instance.currentXRectFactor, Minimap.Instance.currentYRectFactor);
		if (!Minimap.Instance.IsFullscreen && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !GameManager.Instance.IsLoadingLevel && (!GameManager.Instance.SecondaryPlayer.IsGhost || true))
		{
			rect.y -= 0.0875f;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
		Graphics.Blit(Pixelator.SmallBlackTexture, temporary);
		this.slaveCamera.cullingMask = this.m_cachedCullingMask;
		this.slaveCamera.targetTexture = temporary;
		this.slaveCamera.Render();
		Rect rect2 = BraveCameraUtility.GetRect();
		Vector4 vector = new Vector4(rect.xMin + rect2.xMin, rect.yMin + rect2.yMin, rect.width * rect2.width, rect.height * rect2.height);
		Vector4 vector2 = new Vector4(rect.xMin, rect.yMin, rect.width, rect.height);
		if (this.m_bgTexID == -1)
		{
			this.m_bgTexID = Shader.PropertyToID("_BGTex");
			this.m_bgTexUVID = Shader.PropertyToID("_BGTexUV");
			this.m_cameraRectID = Shader.PropertyToID("_CameraRect");
		}
		this.m_material.SetTexture(this.m_bgTexID, temporary);
		this.m_material.SetVector(this.m_bgTexUVID, vector2);
		this.m_material.SetVector(this.m_cameraRectID, vector);
		Graphics.Blit(source, target, this.m_material);
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x04007F53 RID: 32595
	public Shader shader;

	// Token: 0x04007F54 RID: 32596
	public Material materialInstance;

	// Token: 0x04007F55 RID: 32597
	public Camera slaveCamera;

	// Token: 0x04007F56 RID: 32598
	protected Camera m_camera;

	// Token: 0x04007F57 RID: 32599
	protected Material m_material;

	// Token: 0x04007F58 RID: 32600
	protected int m_cachedCullingMask;

	// Token: 0x04007F59 RID: 32601
	private int m_bgTexID = -1;

	// Token: 0x04007F5A RID: 32602
	private int m_bgTexUVID = -1;

	// Token: 0x04007F5B RID: 32603
	private int m_cameraRectID = -1;
}
