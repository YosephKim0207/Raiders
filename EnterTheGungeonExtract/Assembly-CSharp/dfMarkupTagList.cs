using System;
using UnityEngine;

// Token: 0x020004AB RID: 1195
[dfMarkupTagInfo("ul")]
[dfMarkupTagInfo("ol")]
public class dfMarkupTagList : dfMarkupTag
{
	// Token: 0x06001BC8 RID: 7112 RVA: 0x0008345C File Offset: 0x0008165C
	public dfMarkupTagList()
		: base("ul")
	{
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x0008346C File Offset: 0x0008166C
	public dfMarkupTagList(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06001BCA RID: 7114 RVA: 0x00083478 File Offset: 0x00081678
	// (set) Token: 0x06001BCB RID: 7115 RVA: 0x00083480 File Offset: 0x00081680
	internal int BulletWidth { get; private set; }

	// Token: 0x06001BCC RID: 7116 RVA: 0x0008348C File Offset: 0x0008168C
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.ChildNodes.Count == 0)
		{
			return;
		}
		style = base.applyTextStyleAttributes(style);
		style.Align = dfMarkupTextAlign.Left;
		dfMarkupBox dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, style);
		container.AddChild(dfMarkupBox);
		this.calculateBulletWidth(style);
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			dfMarkupTag dfMarkupTag = base.ChildNodes[i] as dfMarkupTag;
			if (dfMarkupTag != null && !(dfMarkupTag.TagName != "li"))
			{
				dfMarkupTag.PerformLayout(dfMarkupBox, style);
			}
		}
		dfMarkupBox.FitToContents();
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x00083530 File Offset: 0x00081730
	private void calculateBulletWidth(dfMarkupStyle style)
	{
		if (base.TagName == "ul")
		{
			this.BulletWidth = Mathf.CeilToInt(style.Font.MeasureText("•", style.FontSize, style.FontStyle).x);
			return;
		}
		int num = 0;
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			dfMarkupTag dfMarkupTag = base.ChildNodes[i] as dfMarkupTag;
			if (dfMarkupTag != null && dfMarkupTag.TagName == "li")
			{
				num++;
			}
		}
		string text = new string('X', num.ToString().Length) + ".";
		this.BulletWidth = Mathf.CeilToInt(style.Font.MeasureText(text, style.FontSize, style.FontStyle).x);
	}
}
