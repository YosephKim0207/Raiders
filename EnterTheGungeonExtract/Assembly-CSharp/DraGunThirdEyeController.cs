using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001025 RID: 4133
public class DraGunThirdEyeController : MonoBehaviour
{
	// Token: 0x06005AA2 RID: 23202 RVA: 0x00229D58 File Offset: 0x00227F58
	public void Awake()
	{
		this.m_particleRenderers = new List<Renderer>(this.particleSystems.Count);
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.m_particleRenderers.Add(this.particleSystems[i].GetComponent<Renderer>());
		}
	}

	// Token: 0x06005AA3 RID: 23203 RVA: 0x00229DB4 File Offset: 0x00227FB4
	public void LateUpdate()
	{
		GameObject gameObject = this.AttachPoint;
		if (this.IntroDummy.activeSelf)
		{
			gameObject = this.IntroAttachPoint;
		}
		else if (this.RoarDummy.activeSelf)
		{
			gameObject = this.RoarAttachPoint;
		}
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			this.m_particleRenderers[i].enabled = true;
			if (gameObject.activeSelf)
			{
				this.particleSystems[i].enableEmission = true;
				base.transform.position = gameObject.transform.position;
			}
			else
			{
				this.particleSystems[i].enableEmission = false;
			}
			if (GameManager.IsBossIntro)
			{
				this.particleSystems[i].Simulate(GameManager.INVARIANT_DELTA_TIME, true, false);
			}
			else if (this.particleSystems[i].isPaused)
			{
				this.particleSystems[i].Play();
			}
		}
	}

	// Token: 0x0400540B RID: 21515
	public GameObject IntroDummy;

	// Token: 0x0400540C RID: 21516
	public GameObject IntroAttachPoint;

	// Token: 0x0400540D RID: 21517
	public GameObject RoarDummy;

	// Token: 0x0400540E RID: 21518
	public GameObject RoarAttachPoint;

	// Token: 0x0400540F RID: 21519
	public GameObject AttachPoint;

	// Token: 0x04005410 RID: 21520
	public List<ParticleSystem> particleSystems;

	// Token: 0x04005411 RID: 21521
	private List<Renderer> m_particleRenderers;
}
