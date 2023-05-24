using System;
using UnityEngine;

// Token: 0x02001600 RID: 5632
public class PlayParticleSystemDuringBossIntro : MonoBehaviour
{
	// Token: 0x060082E5 RID: 33509 RVA: 0x00358670 File Offset: 0x00356870
	public void Start()
	{
		this.m_particleSystem = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x060082E6 RID: 33510 RVA: 0x00358680 File Offset: 0x00356880
	public void Update()
	{
		if (GameManager.IsBossIntro)
		{
			this.m_particleSystem.Simulate(GameManager.INVARIANT_DELTA_TIME, true, false);
			this.m_isSimulating = true;
		}
		else if (this.m_isSimulating)
		{
			this.m_particleSystem.Play();
			this.m_isSimulating = false;
		}
	}

	// Token: 0x040085FE RID: 34302
	private bool m_isSimulating;

	// Token: 0x040085FF RID: 34303
	private ParticleSystem m_particleSystem;
}
