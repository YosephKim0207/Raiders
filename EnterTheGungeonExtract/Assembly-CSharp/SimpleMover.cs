using System;
using UnityEngine;

// Token: 0x02001833 RID: 6195
public class SimpleMover : MonoBehaviour
{
	// Token: 0x060092AB RID: 37547 RVA: 0x003DF920 File Offset: 0x003DDB20
	private void Start()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x060092AC RID: 37548 RVA: 0x003DF930 File Offset: 0x003DDB30
	private void Update()
	{
		this.m_transform.position = this.m_transform.position + this.velocity * BraveTime.DeltaTime;
		this.velocity += this.acceleration * BraveTime.DeltaTime;
	}

	// Token: 0x060092AD RID: 37549 RVA: 0x003DF98C File Offset: 0x003DDB8C
	public void OnDespawned()
	{
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x04009A32 RID: 39474
	public Vector3 velocity;

	// Token: 0x04009A33 RID: 39475
	public Vector3 acceleration;

	// Token: 0x04009A34 RID: 39476
	private Transform m_transform;
}
