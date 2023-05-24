using System;
using UnityEngine;

// Token: 0x0200043C RID: 1084
[AddComponentMenu("Daikon Forge/Examples/Add-Remove Controls/Remove Child Controls")]
public class DemoRemoveAllControls : MonoBehaviour
{
	// Token: 0x060018DA RID: 6362 RVA: 0x00075300 File Offset: 0x00073500
	public void Start()
	{
		if (this.target == null)
		{
			this.target = base.GetComponent<dfControl>();
		}
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x00075320 File Offset: 0x00073520
	public void OnClick()
	{
		while (this.target.Controls.Count > 0)
		{
			dfControl dfControl = this.target.Controls[0];
			this.target.RemoveControl(dfControl);
			UnityEngine.Object.DestroyImmediate(dfControl.gameObject);
		}
	}

	// Token: 0x040013AD RID: 5037
	public dfControl target;
}
