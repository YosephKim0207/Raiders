using System;

// Token: 0x020004AD RID: 1197
[dfMarkupTagInfo("p")]
public class dfMarkupTagParagraph : dfMarkupTag
{
	// Token: 0x06001BD1 RID: 7121 RVA: 0x000837FC File Offset: 0x000819FC
	public dfMarkupTagParagraph()
		: base("p")
	{
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x0008380C File Offset: 0x00081A0C
	public dfMarkupTagParagraph(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x00083818 File Offset: 0x00081A18
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (base.ChildNodes.Count == 0)
		{
			return;
		}
		style = base.applyTextStyleAttributes(style);
		int num = ((container.Children.Count != 0) ? style.LineHeight : 0);
		dfMarkupBox dfMarkupBox;
		if (style.BackgroundColor.a > 0.005f)
		{
			dfMarkupBoxSprite dfMarkupBoxSprite = new dfMarkupBoxSprite(this, dfMarkupDisplayType.block, style);
			dfMarkupBoxSprite.Atlas = base.Owner.Atlas;
			dfMarkupBoxSprite.Source = base.Owner.BlankTextureSprite;
			dfMarkupBoxSprite.Style.Color = style.BackgroundColor;
			dfMarkupBox = dfMarkupBoxSprite;
		}
		else
		{
			dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, style);
		}
		dfMarkupBox.Margins = new dfMarkupBorders(0, 0, num, style.LineHeight);
		dfMarkupAttribute dfMarkupAttribute = base.findAttribute(new string[] { "margin" });
		if (dfMarkupAttribute != null)
		{
			dfMarkupBox.Margins = dfMarkupBorders.Parse(dfMarkupAttribute.Value);
		}
		dfMarkupAttribute dfMarkupAttribute2 = base.findAttribute(new string[] { "padding" });
		if (dfMarkupAttribute2 != null)
		{
			dfMarkupBox.Padding = dfMarkupBorders.Parse(dfMarkupAttribute2.Value);
		}
		container.AddChild(dfMarkupBox);
		base._PerformLayoutImpl(dfMarkupBox, style);
		if (dfMarkupBox.Children.Count > 0)
		{
			dfMarkupBox.Children[dfMarkupBox.Children.Count - 1].IsNewline = true;
		}
		dfMarkupBox.FitToContents(true);
	}
}
