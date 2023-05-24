using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001543 RID: 5443
public class LightPulser : MonoBehaviour
{
	// Token: 0x06007C93 RID: 31891 RVA: 0x00322DDC File Offset: 0x00320FDC
	private void Start()
	{
		if (this.flicker)
		{
			base.StartCoroutine("Flicker");
		}
	}

	// Token: 0x06007C94 RID: 31892 RVA: 0x00322DF8 File Offset: 0x00320FF8
	public void AssignShadowSystem(ShadowSystem ss)
	{
		this.m_sl = ss;
	}

	// Token: 0x06007C95 RID: 31893 RVA: 0x00322E04 File Offset: 0x00321004
	private IEnumerator Flicker()
	{
		for (;;)
		{
			if (this.m_sl != null)
			{
				if (this.m_sl.uLightRange == this.normalRange)
				{
					this.m_sl.uLightRange = this.flickerRange;
				}
				else
				{
					this.m_sl.uLightRange = this.normalRange;
				}
			}
			else if (base.GetComponent<Light>().range == this.normalRange)
			{
				base.GetComponent<Light>().range = this.flickerRange;
			}
			else
			{
				base.GetComponent<Light>().range = this.normalRange;
			}
			yield return new WaitForSeconds(this.waitTime);
		}
		yield break;
	}

	// Token: 0x06007C96 RID: 31894 RVA: 0x00322E20 File Offset: 0x00321020
	private void Update()
	{
		if (!this.flicker)
		{
			if (this.m_sl != null)
			{
				this.m_sl.uLightRange = this.flickerRange + Mathf.PingPong(Time.time * this.pulseSpeed, this.normalRange - this.flickerRange);
			}
			else
			{
				base.GetComponent<Light>().range = this.flickerRange + Mathf.PingPong(Time.time * this.pulseSpeed, this.normalRange - this.flickerRange);
			}
		}
	}

	// Token: 0x04007F8E RID: 32654
	public bool flicker;

	// Token: 0x04007F8F RID: 32655
	public float pulseSpeed = 40f;

	// Token: 0x04007F90 RID: 32656
	public float waitTime = 0.05f;

	// Token: 0x04007F91 RID: 32657
	public float normalRange = 3.33f;

	// Token: 0x04007F92 RID: 32658
	public float flickerRange = 0.5f;

	// Token: 0x04007F93 RID: 32659
	private ShadowSystem m_sl;
}
