using System;
using UnityEngine;

// Token: 0x0200153C RID: 5436
public class MinimapRenderer : MonoBehaviour
{
	// Token: 0x06007C64 RID: 31844 RVA: 0x00320DAC File Offset: 0x0031EFAC
	private void Awake()
	{
		this.m_camera = base.GetComponent<Camera>();
		this.m_quadMaterial = this.QuadTransform.GetComponent<MeshRenderer>().material;
		this.QuadTransform.parent = this.QuadTransform.parent.parent;
		this.QuadTransform.gameObject.SetLayerRecursively(LayerMask.NameToLayer("GUI"));
		this.m_idMainTex = Shader.PropertyToID("_MainTex");
		this.m_idMaskTex = Shader.PropertyToID("_MaskTex");
	}

	// Token: 0x06007C65 RID: 31845 RVA: 0x00320E30 File Offset: 0x0031F030
	private void Start()
	{
		this.m_uiCamera = GameUIRoot.Instance.Manager.RenderCamera;
	}

	// Token: 0x06007C66 RID: 31846 RVA: 0x00320E48 File Offset: 0x0031F048
	private void CheckSize()
	{
		Rect rect = new Rect(1f - Minimap.Instance.currentXRectFactor, 1f - Minimap.Instance.currentYRectFactor, Minimap.Instance.currentXRectFactor, Minimap.Instance.currentYRectFactor);
		if (!Minimap.Instance.IsFullscreen && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !GameManager.Instance.IsLoadingLevel && (!GameManager.Instance.SecondaryPlayer.IsGhost || true))
		{
			rect.y -= 0.0875f;
		}
		this.QuadTransform.localScale = new Vector3(this.m_uiCamera.orthographicSize * 2f * 1.7777778f * rect.width, this.m_uiCamera.orthographicSize * 2f * rect.height, 1f);
		Vector3 vector = new Vector3(this.m_uiCamera.orthographicSize * this.m_uiCamera.aspect * -1f, this.m_uiCamera.orthographicSize * -1f, 0f);
		vector.x += rect.xMin * this.m_uiCamera.orthographicSize * 2f * this.m_uiCamera.aspect;
		vector.y += rect.yMin * this.m_uiCamera.orthographicSize * 2f;
		vector.x += this.QuadTransform.localScale.x * (this.m_uiCamera.aspect / 1.7777778f) / 2f;
		vector.y += this.QuadTransform.localScale.y / 2f;
		this.QuadTransform.position = (this.m_uiCamera.transform.position + vector).WithZ(3f);
		if (Minimap.Instance.IsFullscreen)
		{
			this.m_quadMaterial.SetTexture(this.m_idMaskTex, this.MapMaskFullscreen);
		}
		else
		{
			this.m_quadMaterial.SetTexture(this.m_idMaskTex, this.MapMaskSmallscreen);
		}
		int num = ((GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.HIGH) ? 960 : 1920);
		int num2 = ((GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.HIGH) ? 540 : 1080);
		if (this.m_currentQuadRenderTexture != null && (this.m_currentQuadRenderTexture.width != num || this.m_currentQuadRenderTexture.height != num2))
		{
			RenderTexture.ReleaseTemporary(this.m_currentQuadRenderTexture);
			this.m_currentQuadRenderTexture = null;
		}
		if (this.m_currentQuadRenderTexture == null)
		{
			this.m_currentQuadRenderTexture = RenderTexture.GetTemporary(num, num2);
			this.m_currentQuadRenderTexture.filterMode = FilterMode.Point;
			this.m_quadMaterial.SetTexture(this.m_idMainTex, this.m_currentQuadRenderTexture);
		}
		if (this.m_camera.targetTexture != this.m_currentQuadRenderTexture)
		{
			this.m_camera.targetTexture = this.m_currentQuadRenderTexture;
		}
	}

	// Token: 0x06007C67 RID: 31847 RVA: 0x00321194 File Offset: 0x0031F394
	private void LateUpdate()
	{
		this.CheckSize();
	}

	// Token: 0x06007C68 RID: 31848 RVA: 0x0032119C File Offset: 0x0031F39C
	private void OnDestroy()
	{
		if (this.m_currentQuadRenderTexture != null)
		{
			RenderTexture.ReleaseTemporary(this.m_currentQuadRenderTexture);
		}
	}

	// Token: 0x04007F5C RID: 32604
	public Transform QuadTransform;

	// Token: 0x04007F5D RID: 32605
	public Texture MapMaskFullscreen;

	// Token: 0x04007F5E RID: 32606
	public Texture MapMaskSmallscreen;

	// Token: 0x04007F5F RID: 32607
	private Material m_quadMaterial;

	// Token: 0x04007F60 RID: 32608
	private Camera m_camera;

	// Token: 0x04007F61 RID: 32609
	private Camera m_uiCamera;

	// Token: 0x04007F62 RID: 32610
	private int m_cmQuad;

	// Token: 0x04007F63 RID: 32611
	private int m_idMainTex;

	// Token: 0x04007F64 RID: 32612
	private int m_idMaskTex;

	// Token: 0x04007F65 RID: 32613
	private RenderTexture m_currentQuadRenderTexture;
}
