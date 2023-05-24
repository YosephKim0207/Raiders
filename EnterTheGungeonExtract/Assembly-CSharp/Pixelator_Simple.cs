using System;
using UnityEngine;

// Token: 0x02001537 RID: 5431
public class Pixelator_Simple : MonoBehaviour
{
	// Token: 0x1700125B RID: 4699
	// (get) Token: 0x06007C48 RID: 31816 RVA: 0x003200DC File Offset: 0x0031E2DC
	// (set) Token: 0x06007C49 RID: 31817 RVA: 0x003200E4 File Offset: 0x0031E2E4
	public Material RenderMaterial
	{
		get
		{
			return this.m_renderMaterial;
		}
		set
		{
			this.m_renderMaterial = value;
		}
	}

	// Token: 0x06007C4A RID: 31818 RVA: 0x003200F0 File Offset: 0x0031E2F0
	private void Start()
	{
		this.slaveCamera.GetComponent<dfGUICamera>().transform.parent.GetComponent<dfGUIManager>().OverrideCamera = true;
		this.Initialize();
	}

	// Token: 0x06007C4B RID: 31819 RVA: 0x00320118 File Offset: 0x0031E318
	public void Initialize()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		if (this.renderShader != null)
		{
			this.m_renderMaterial = new Material(this.renderShader);
		}
		if (this.upsideDownShader != null)
		{
			this.m_upsideDownMaterial = new Material(this.upsideDownShader);
		}
		this.m_cachedCullingMask = this.slaveCamera.cullingMask;
	}

	// Token: 0x06007C4C RID: 31820 RVA: 0x00320190 File Offset: 0x0031E390
	private void RebuildRenderTarget(RenderTexture source)
	{
		if (Pixelator.Instance == null)
		{
			this.m_renderTarget = null;
			return;
		}
		int num = Pixelator.Instance.CurrentMacroResolutionX;
		int num2 = Pixelator.Instance.CurrentMacroResolutionY;
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			num = source.width;
			num2 = source.height;
		}
		if (this.m_renderTarget == null || this.m_renderTarget.width != num || this.m_renderTarget.height != num2)
		{
			this.m_renderTarget = new RenderTexture(num, num2, 1);
			this.m_renderTarget.filterMode = FilterMode.Point;
		}
	}

	// Token: 0x06007C4D RID: 31821 RVA: 0x00320268 File Offset: 0x0031E468
	private void OnRenderImage(RenderTexture source, RenderTexture target)
	{
		if (this.m_camera == null)
		{
			this.m_camera = base.GetComponent<Camera>();
		}
		this.RebuildRenderTarget(source);
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, source.depth);
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			temporary.filterMode = FilterMode.Point;
		}
		Graphics.Blit(Pixelator.SmallBlackTexture, temporary);
		if (this.m_renderTarget == null)
		{
			if (this.m_renderMaterial != null)
			{
				Graphics.Blit(source, target, this.m_renderMaterial);
			}
			else
			{
				Graphics.Blit(source, target);
			}
		}
		else
		{
			this.slaveCamera.CopyFrom(this.m_camera);
			this.slaveCamera.transform.position = this.slaveCamera.transform.position + CameraController.PLATFORM_CAMERA_OFFSET;
			this.slaveCamera.cullingMask = this.m_cachedCullingMask;
			this.slaveCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.slaveCamera.clearFlags = CameraClearFlags.Color;
			this.slaveCamera.backgroundColor = Color.clear;
			this.slaveCamera.targetTexture = temporary;
			this.slaveCamera.Render();
			this.slaveCamera.transform.position = this.slaveCamera.transform.position - CameraController.PLATFORM_CAMERA_OFFSET;
			Graphics.Blit(temporary, this.m_renderTarget);
			if (this.m_renderMaterial != null)
			{
				Graphics.Blit(source, temporary);
				Graphics.Blit(this.m_renderTarget, temporary, this.m_upsideDownMaterial);
				Graphics.Blit(temporary, target, this.m_renderMaterial);
			}
			else
			{
				Debug.LogError("Failing...");
				Graphics.Blit(source, target);
			}
		}
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x04007F32 RID: 32562
	public Shader renderShader;

	// Token: 0x04007F33 RID: 32563
	public Shader upsideDownShader;

	// Token: 0x04007F34 RID: 32564
	public Camera slaveCamera;

	// Token: 0x04007F35 RID: 32565
	private RenderTexture m_renderTarget;

	// Token: 0x04007F36 RID: 32566
	private Camera m_camera;

	// Token: 0x04007F37 RID: 32567
	private Material m_renderMaterial;

	// Token: 0x04007F38 RID: 32568
	private Material m_upsideDownMaterial;

	// Token: 0x04007F39 RID: 32569
	private int m_cachedCullingMask;

	// Token: 0x04007F3A RID: 32570
	private bool m_initialized;
}
