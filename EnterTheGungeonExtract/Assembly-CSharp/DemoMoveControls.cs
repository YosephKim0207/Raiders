using System;
using UnityEngine;

// Token: 0x0200043B RID: 1083
[AddComponentMenu("Daikon Forge/Examples/Add-Remove Controls/Move Child Control")]
public class DemoMoveControls : MonoBehaviour
{
	// Token: 0x060018D8 RID: 6360 RVA: 0x00075260 File Offset: 0x00073460
	public void OnClick()
	{
		this.from.SuspendLayout();
		this.to.SuspendLayout();
		while (this.from.Controls.Count > 0)
		{
			dfControl dfControl = this.from.Controls[0];
			this.from.RemoveControl(dfControl);
			dfControl.ZOrder = -1;
			this.to.AddControl(dfControl);
		}
		this.from.ResumeLayout();
		this.to.ResumeLayout();
		this.from.ScrollPosition = Vector2.zero;
	}

	// Token: 0x040013AB RID: 5035
	public dfScrollPanel from;

	// Token: 0x040013AC RID: 5036
	public dfScrollPanel to;
}
