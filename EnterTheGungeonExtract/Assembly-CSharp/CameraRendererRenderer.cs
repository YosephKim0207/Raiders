using System;
using UnityEngine;

// Token: 0x0200151D RID: 5405
public class CameraRendererRenderer : MonoBehaviour
{
	// Token: 0x06007B55 RID: 31573 RVA: 0x00316560 File Offset: 0x00314760
	private void Start()
	{
		this.m_renderers = UnityEngine.Object.FindObjectsOfType<RendererRenderer>();
	}

	// Token: 0x06007B56 RID: 31574 RVA: 0x00316570 File Offset: 0x00314770
	private void OnRenderImage(RenderTexture source, RenderTexture target)
	{
		this.m_renderers[0].transform.position += new Vector3(3f, 0f, 0f);
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			this.m_renderers[i].GetComponent<Renderer>().sharedMaterial.SetPass(0);
		}
		this.m_renderers[0].transform.position -= new Vector3(3f, 0f, 0f);
		Graphics.Blit(source, target);
	}

	// Token: 0x04007DE4 RID: 32228
	private RendererRenderer[] m_renderers;
}
