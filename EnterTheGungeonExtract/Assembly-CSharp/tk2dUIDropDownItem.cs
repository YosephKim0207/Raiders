using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C05 RID: 3077
[AddComponentMenu("2D Toolkit/UI/tk2dUIDropDownItem")]
public class tk2dUIDropDownItem : tk2dUIBaseItemControl
{
	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x0600416F RID: 16751 RVA: 0x00152D10 File Offset: 0x00150F10
	// (set) Token: 0x06004170 RID: 16752 RVA: 0x00152D18 File Offset: 0x00150F18
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = value;
		}
	}

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x06004171 RID: 16753 RVA: 0x00152D24 File Offset: 0x00150F24
	// (remove) Token: 0x06004172 RID: 16754 RVA: 0x00152D5C File Offset: 0x00150F5C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<tk2dUIDropDownItem> OnItemSelected;

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06004173 RID: 16755 RVA: 0x00152D94 File Offset: 0x00150F94
	// (set) Token: 0x06004174 RID: 16756 RVA: 0x00152DA4 File Offset: 0x00150FA4
	public string LabelText
	{
		get
		{
			return this.label.text;
		}
		set
		{
			this.label.text = value;
			this.label.Commit();
		}
	}

	// Token: 0x06004175 RID: 16757 RVA: 0x00152DC0 File Offset: 0x00150FC0
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ItemSelected;
		}
	}

	// Token: 0x06004176 RID: 16758 RVA: 0x00152DEC File Offset: 0x00150FEC
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ItemSelected;
		}
	}

	// Token: 0x06004177 RID: 16759 RVA: 0x00152E18 File Offset: 0x00151018
	private void ItemSelected()
	{
		if (this.OnItemSelected != null)
		{
			this.OnItemSelected(this);
		}
	}

	// Token: 0x0400341E RID: 13342
	public tk2dTextMesh label;

	// Token: 0x0400341F RID: 13343
	public float height;

	// Token: 0x04003420 RID: 13344
	public tk2dUIUpDownHoverButton upDownHoverBtn;

	// Token: 0x04003421 RID: 13345
	private int index;
}
