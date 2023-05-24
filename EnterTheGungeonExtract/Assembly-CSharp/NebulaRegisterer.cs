using System;
using System.Collections;
using UnityEngine;

// Token: 0x020011BE RID: 4542
public class NebulaRegisterer : MonoBehaviour
{
	// Token: 0x06006550 RID: 25936 RVA: 0x00276948 File Offset: 0x00274B48
	private IEnumerator Start()
	{
		this.m_renderer = base.GetComponent<Renderer>();
		yield return new WaitForSeconds(0.25f);
		if (this.m_renderer)
		{
			EndTimesNebulaController endTimesNebulaController = UnityEngine.Object.FindObjectOfType<EndTimesNebulaController>();
			if (endTimesNebulaController)
			{
				endTimesNebulaController.NebulaRegisteredVisuals.Add(this.m_renderer);
			}
		}
		yield break;
	}

	// Token: 0x06006551 RID: 25937 RVA: 0x00276964 File Offset: 0x00274B64
	private void Update()
	{
		if (this.m_renderer)
		{
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW)
			{
				this.m_renderer.enabled = false;
			}
			else if (!this.m_renderer.enabled)
			{
				this.m_renderer.enabled = true;
			}
		}
	}

	// Token: 0x0400610C RID: 24844
	private Renderer m_renderer;
}
