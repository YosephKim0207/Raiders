using System;
using UnityEngine;

// Token: 0x02001729 RID: 5929
public class SpinningTrapController : TrapController
{
	// Token: 0x060089AD RID: 35245 RVA: 0x003943E8 File Offset: 0x003925E8
	public void Update()
	{
		this.m_rotation += 360f * BraveTime.DeltaTime / this.secondsPerRotation;
		float num = this.m_rotation;
		if (this.doQuantize)
		{
			num = BraveMathCollege.QuantizeFloat(this.m_rotation, this.multiplesOf);
		}
		this.spinningObject.transform.rotation = Quaternion.Euler(0f, 0f, num);
	}

	// Token: 0x060089AE RID: 35246 RVA: 0x00394458 File Offset: 0x00392658
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400900D RID: 36877
	public GameObject baseObject;

	// Token: 0x0400900E RID: 36878
	public GameObject spinningObject;

	// Token: 0x0400900F RID: 36879
	public float secondsPerRotation;

	// Token: 0x04009010 RID: 36880
	public bool doQuantize;

	// Token: 0x04009011 RID: 36881
	public float multiplesOf;

	// Token: 0x04009012 RID: 36882
	private float m_rotation;
}
