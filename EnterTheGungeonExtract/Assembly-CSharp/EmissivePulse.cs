using System;
using UnityEngine;

// Token: 0x02000F76 RID: 3958
public class EmissivePulse : MonoBehaviour
{
	// Token: 0x06005548 RID: 21832 RVA: 0x00206554 File Offset: 0x00204754
	private void Start()
	{
		this.m_id = Shader.PropertyToID("_EmissivePower");
		this.m_material = base.GetComponent<Renderer>().material;
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x00206578 File Offset: 0x00204778
	private void Update()
	{
		this.m_material.SetFloat(this.m_id, Mathf.Lerp(this.minEmissivePower, this.maxEmissivePower, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.timeSinceLevelLoad / this.period, 1f))));
	}

	// Token: 0x04004E31 RID: 20017
	public float minEmissivePower = 10f;

	// Token: 0x04004E32 RID: 20018
	public float maxEmissivePower = 20f;

	// Token: 0x04004E33 RID: 20019
	public float period = 2f;

	// Token: 0x04004E34 RID: 20020
	private Material m_material;

	// Token: 0x04004E35 RID: 20021
	private int m_id = -1;
}
