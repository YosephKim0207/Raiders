using System;
using UnityEngine;

// Token: 0x020010FA RID: 4346
public class AudioListenerController : MonoBehaviour
{
	// Token: 0x06005FCE RID: 24526 RVA: 0x0024E42C File Offset: 0x0024C62C
	private void Start()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x06005FCF RID: 24527 RVA: 0x0024E43C File Offset: 0x0024C63C
	private void LateUpdate()
	{
		this.m_transform.position = this.m_transform.position.WithZ(this.m_transform.position.y);
	}

	// Token: 0x04005A4D RID: 23117
	private Transform m_transform;
}
