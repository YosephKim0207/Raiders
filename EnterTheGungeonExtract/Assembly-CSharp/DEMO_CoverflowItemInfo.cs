using System;
using UnityEngine;

// Token: 0x02000448 RID: 1096
[AddComponentMenu("Daikon Forge/Examples/Coverflow/Item Info")]
public class DEMO_CoverflowItemInfo : MonoBehaviour
{
	// Token: 0x0600192E RID: 6446 RVA: 0x000763E8 File Offset: 0x000745E8
	public void Start()
	{
		this.label = base.GetComponent<dfLabel>();
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000763F8 File Offset: 0x000745F8
	private void Update()
	{
		if (this.scroller == null || this.descriptions == null || this.descriptions.Length == 0)
		{
			return;
		}
		int num = Mathf.Max(0, Mathf.Min(this.descriptions.Length - 1, this.scroller.selectedIndex));
		this.label.Text = this.descriptions[num];
	}

	// Token: 0x040013CC RID: 5068
	public dfCoverflow scroller;

	// Token: 0x040013CD RID: 5069
	public string[] descriptions;

	// Token: 0x040013CE RID: 5070
	private dfLabel label;
}
