using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001295 RID: 4757
public class EndTimesNebulaController : MonoBehaviour
{
	// Token: 0x06006A75 RID: 27253 RVA: 0x0029BE30 File Offset: 0x0029A030
	private IEnumerator Start()
	{
		yield return null;
		this.BecomePartiallyActive();
		yield return new WaitForSeconds(0.5f);
		this.NebulaCamera.enabled = false;
		yield break;
	}

	// Token: 0x06006A76 RID: 27254 RVA: 0x0029BE4C File Offset: 0x0029A04C
	public void BecomePartiallyActive()
	{
		this.m_partiallyActiveRenderTarget = RenderTexture.GetTemporary(Pixelator.Instance.CurrentMacroResolutionX, Pixelator.Instance.CurrentMacroResolutionY, 0, RenderTextureFormat.Default);
		this.NebulaCamera.enabled = true;
		this.NebulaCamera.targetTexture = this.m_partiallyActiveRenderTarget;
		this.m_portalMaterial = BraveResources.Load("Shaders/DarkPortalMaterial", ".mat") as Material;
		if (this.m_portalMaterial)
		{
			this.m_portalMaterial.SetTexture("_PortalTex", this.m_partiallyActiveRenderTarget);
		}
		Shader.SetGlobalTexture("_EndTimesVortex", this.m_partiallyActiveRenderTarget);
		bool flag = GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW;
		if (flag)
		{
			this.NebulaClouds.generateNewSlices = true;
			this.m_nebulaMaterial = this.NebulaClouds.cloudMaterial;
		}
		else
		{
			UnityEngine.Object.Destroy(this.NebulaClouds.gameObject);
			this.m_nebulaMaterial = null;
		}
	}

	// Token: 0x06006A77 RID: 27255 RVA: 0x0029BF4C File Offset: 0x0029A14C
	private void ClearRT()
	{
		if (this.m_partiallyActiveRenderTarget != null)
		{
			RenderTexture.ReleaseTemporary(this.m_partiallyActiveRenderTarget);
			this.m_partiallyActiveRenderTarget = null;
		}
	}

	// Token: 0x06006A78 RID: 27256 RVA: 0x0029BF74 File Offset: 0x0029A174
	public void BecomeActive()
	{
		this.m_isActive = true;
		this.NebulaCamera.enabled = true;
		this.ClearRT();
		Pixelator.Instance.AdditionalBGCamera = this.NebulaCamera;
	}

	// Token: 0x06006A79 RID: 27257 RVA: 0x0029BFA0 File Offset: 0x0029A1A0
	private void Update()
	{
		if (!this.m_isActive && !GameManager.Instance.IsLoadingLevel)
		{
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW)
			{
				this.NebulaCamera.enabled = false;
			}
			else if (!this.NebulaCamera.enabled)
			{
				if (GameManager.Instance.AllPlayers != null)
				{
					for (int i = 0; i < this.NebulaRegisteredVisuals.Count; i++)
					{
						if (this.NebulaRegisteredVisuals[i].isVisible)
						{
							this.NebulaCamera.enabled = true;
						}
					}
				}
			}
			else if (this.NebulaCamera.enabled && GameManager.Instance.AllPlayers != null)
			{
				bool flag = false;
				for (int j = 0; j < this.NebulaRegisteredVisuals.Count; j++)
				{
					if (this.NebulaRegisteredVisuals[j].isVisible)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					this.NebulaCamera.enabled = false;
				}
			}
		}
		if (this.m_isActive && this.m_nebulaMaterial != null)
		{
			float y = GameManager.Instance.MainCameraController.transform.position.y;
			this.m_nebulaMaterial.SetFloat("_ZOffset", y * this.CloudParallaxFactor);
		}
		if (this.m_isActive && this.BGQuad)
		{
			float aspect = BraveCameraUtility.ASPECT;
			float num = aspect / 1.7777778f;
			if (num > 1f)
			{
				this.BGQuad.transform.localScale = new Vector3(16f * num, 9f, 1f);
			}
			else
			{
				this.BGQuad.transform.localScale = new Vector3(16f * num, 9f, 1f);
			}
		}
		if (this.m_portalMaterial)
		{
			if (this.m_playerPosID == -1)
			{
				this.m_playerPosID = Shader.PropertyToID("_PlayerPos");
			}
			Vector2 centerPosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
			Vector2 vector = ((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? Vector2.zero : GameManager.Instance.SecondaryPlayer.CenterPosition);
			this.m_portalMaterial.SetVector(this.m_playerPosID, new Vector4(centerPosition.x, centerPosition.y, vector.x, vector.y));
		}
	}

	// Token: 0x06006A7A RID: 27258 RVA: 0x0029C238 File Offset: 0x0029A438
	private void OnDestroy()
	{
		this.ClearRT();
	}

	// Token: 0x06006A7B RID: 27259 RVA: 0x0029C240 File Offset: 0x0029A440
	public void BecomeInactive(bool destroy = true)
	{
		this.m_isActive = false;
		if (Pixelator.HasInstance && Pixelator.Instance.AdditionalBGCamera == this.NebulaCamera)
		{
			Pixelator.Instance.AdditionalBGCamera = null;
		}
		if (destroy)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040066F7 RID: 26359
	public Camera NebulaCamera;

	// Token: 0x040066F8 RID: 26360
	public SlicedVolume NebulaClouds;

	// Token: 0x040066F9 RID: 26361
	public float CloudParallaxFactor = 0.5f;

	// Token: 0x040066FA RID: 26362
	public Transform BGQuad;

	// Token: 0x040066FB RID: 26363
	private bool m_isActive;

	// Token: 0x040066FC RID: 26364
	private Material m_nebulaMaterial;

	// Token: 0x040066FD RID: 26365
	private RenderTexture m_partiallyActiveRenderTarget;

	// Token: 0x040066FE RID: 26366
	private Material m_portalMaterial;

	// Token: 0x040066FF RID: 26367
	public List<Renderer> NebulaRegisteredVisuals = new List<Renderer>();

	// Token: 0x04006700 RID: 26368
	private int m_playerPosID = -1;
}
