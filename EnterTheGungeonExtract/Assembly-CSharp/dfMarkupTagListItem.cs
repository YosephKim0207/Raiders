using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
[dfMarkupTagInfo("li")]
public class dfMarkupTagListItem : dfMarkupTag
{
	// Token: 0x06001BCE RID: 7118 RVA: 0x0008362C File Offset: 0x0008182C
	public dfMarkupTagListItem()
		: base("li")
	{
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x0008363C File Offset: 0x0008183C
	public dfMarkupTagListItem(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x00083648 File Offset: 0x00081848
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.ChildNodes.Count == 0)
		{
			return;
		}
		float x = container.Size.x;
		dfMarkupBox dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.listItem, style);
		dfMarkupBox.Margins.top = 10;
		container.AddChild(dfMarkupBox);
		dfMarkupTagList dfMarkupTagList = base.Parent as dfMarkupTagList;
		if (dfMarkupTagList == null)
		{
			base._PerformLayoutImpl(container, style);
			return;
		}
		style.VerticalAlign = dfMarkupVerticalAlign.Baseline;
		string text = "•";
		if (dfMarkupTagList.TagName == "ol")
		{
			text = container.Children.Count + ".";
		}
		dfMarkupStyle dfMarkupStyle = style;
		dfMarkupStyle.VerticalAlign = dfMarkupVerticalAlign.Baseline;
		dfMarkupStyle.Align = dfMarkupTextAlign.Right;
		dfMarkupBoxText dfMarkupBoxText = dfMarkupBoxText.Obtain(this, dfMarkupDisplayType.inlineBlock, dfMarkupStyle);
		dfMarkupBoxText.SetText(text);
		dfMarkupBoxText.Width = dfMarkupTagList.BulletWidth;
		dfMarkupBoxText.Margins.left = style.FontSize * 2;
		dfMarkupBox.AddChild(dfMarkupBoxText);
		dfMarkupBox dfMarkupBox2 = new dfMarkupBox(this, dfMarkupDisplayType.inlineBlock, style);
		int fontSize = style.FontSize;
		float num = x - dfMarkupBoxText.Size.x - (float)dfMarkupBoxText.Margins.left - (float)fontSize;
		dfMarkupBox2.Size = new Vector2(num, (float)fontSize);
		dfMarkupBox2.Margins.left = (int)((float)style.FontSize * 0.5f);
		dfMarkupBox.AddChild(dfMarkupBox2);
		for (int i = 0; i < base.ChildNodes.Count; i++)
		{
			base.ChildNodes[i].PerformLayout(dfMarkupBox2, style);
		}
		dfMarkupBox2.FitToContents();
		if (dfMarkupBox2.Parent != null)
		{
			dfMarkupBox2.Parent.FitToContents();
		}
		dfMarkupBox.FitToContents();
	}
}
