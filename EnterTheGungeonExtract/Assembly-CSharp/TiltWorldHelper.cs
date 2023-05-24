using System;
using UnityEngine;

// Token: 0x0200123E RID: 4670
public class TiltWorldHelper : BraveBehaviour
{
	// Token: 0x060068AB RID: 26795 RVA: 0x0028FB94 File Offset: 0x0028DD94
	private void Update()
	{
		base.transform.position = base.transform.position.WithZ(base.transform.position.y - this.HeightOffGround);
		base.transform.rotation = Quaternion.identity;
		if (this.DoForceLayer)
		{
			base.gameObject.layer = LayerMask.NameToLayer(this.ForceLayer);
		}
	}

	// Token: 0x060068AC RID: 26796 RVA: 0x0028FC08 File Offset: 0x0028DE08
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040064EF RID: 25839
	public float HeightOffGround;

	// Token: 0x040064F0 RID: 25840
	public bool DoForceLayer;

	// Token: 0x040064F1 RID: 25841
	public string ForceLayer = "Unoccluded";
}
