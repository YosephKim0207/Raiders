using System;
using UnityEngine;

// Token: 0x020017EE RID: 6126
public class SimpleStatLabel : MonoBehaviour
{
	// Token: 0x0600903B RID: 36923 RVA: 0x003CF450 File Offset: 0x003CD650
	private void Start()
	{
		this.m_label = base.GetComponent<dfLabel>();
	}

	// Token: 0x0600903C RID: 36924 RVA: 0x003CF460 File Offset: 0x003CD660
	private void Update()
	{
		if (this.m_label && this.m_label.IsVisible)
		{
			int num = Mathf.FloorToInt(GameStatsManager.Instance.GetPlayerStatValue(this.stat));
			this.m_label.Text = IntToStringSansGarbage.GetStringForInt(num);
		}
	}

	// Token: 0x04009859 RID: 39001
	public TrackedStats stat;

	// Token: 0x0400985A RID: 39002
	protected dfLabel m_label;
}
