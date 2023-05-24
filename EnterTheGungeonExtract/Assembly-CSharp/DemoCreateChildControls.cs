using System;
using UnityEngine;

// Token: 0x0200043A RID: 1082
[AddComponentMenu("Daikon Forge/Examples/Add-Remove Controls/Create Child Control")]
public class DemoCreateChildControls : MonoBehaviour
{
	// Token: 0x060018D4 RID: 6356 RVA: 0x00075180 File Offset: 0x00073380
	public void Start()
	{
		if (this.target == null)
		{
			this.target = base.GetComponent<dfScrollPanel>();
		}
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000751A0 File Offset: 0x000733A0
	public void OnClick()
	{
		for (int i = 0; i < 10; i++)
		{
			dfButton dfButton = this.target.AddControl<dfButton>();
			dfButton.NormalBackgroundColor = this.colors[this.colorNum % this.colors.Length];
			dfButton.BackgroundSprite = "button-normal";
			dfButton.Text = string.Format("Button {0}", dfButton.ZOrder);
			dfButton.Anchor = dfAnchorStyle.Left | dfAnchorStyle.Right;
			dfButton.Width = this.target.Width - (float)this.target.ScrollPadding.horizontal;
		}
		this.colorNum++;
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x00075250 File Offset: 0x00073450
	public void OnDoubleClick()
	{
		this.OnClick();
	}

	// Token: 0x040013A8 RID: 5032
	public dfScrollPanel target;

	// Token: 0x040013A9 RID: 5033
	private int colorNum;

	// Token: 0x040013AA RID: 5034
	private Color32[] colors = new Color32[]
	{
		Color.white,
		Color.red,
		Color.green,
		Color.black
	};
}
