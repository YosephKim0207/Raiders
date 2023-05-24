using System;
using UnityEngine;

// Token: 0x02001517 RID: 5399
public class ComplexLightColorAnimator : MonoBehaviour
{
	// Token: 0x06007B41 RID: 31553 RVA: 0x00315F54 File Offset: 0x00314154
	private void Start()
	{
		this.m_light = base.GetComponent<Light>();
	}

	// Token: 0x06007B42 RID: 31554 RVA: 0x00315F64 File Offset: 0x00314164
	private void Update()
	{
		float num = (Time.realtimeSinceStartup + this.timeOffset) % this.period / this.period;
		this.m_light.color = this.colorGradient.Evaluate(num);
	}

	// Token: 0x04007DC0 RID: 32192
	public Gradient colorGradient;

	// Token: 0x04007DC1 RID: 32193
	public float period = 3f;

	// Token: 0x04007DC2 RID: 32194
	public float timeOffset;

	// Token: 0x04007DC3 RID: 32195
	private Light m_light;
}
