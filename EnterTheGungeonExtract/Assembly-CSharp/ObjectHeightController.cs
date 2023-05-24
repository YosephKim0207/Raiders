using System;
using UnityEngine;

// Token: 0x020015A9 RID: 5545
public class ObjectHeightController : MonoBehaviour
{
	// Token: 0x06007F3F RID: 32575 RVA: 0x003363E4 File Offset: 0x003345E4
	private void Start()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x06007F40 RID: 32576 RVA: 0x003363F4 File Offset: 0x003345F4
	private void Update()
	{
		this.m_transform.position = this.m_transform.position.WithZ(this.m_transform.position.y - this.heightOffGround);
	}

	// Token: 0x040081F0 RID: 33264
	public float heightOffGround = 0.5f;

	// Token: 0x040081F1 RID: 33265
	private Transform m_transform;
}
