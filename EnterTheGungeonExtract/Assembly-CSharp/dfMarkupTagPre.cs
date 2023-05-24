using System;

// Token: 0x020004B1 RID: 1201
[dfMarkupTagInfo("pre")]
public class dfMarkupTagPre : dfMarkupTag
{
	// Token: 0x06001BDE RID: 7134 RVA: 0x00083C08 File Offset: 0x00081E08
	public dfMarkupTagPre()
		: base("pre")
	{
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00083C18 File Offset: 0x00081E18
	public dfMarkupTagPre(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x00083C24 File Offset: 0x00081E24
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style = base.applyTextStyleAttributes(style);
		style.PreserveWhitespace = true;
		style.Preformatted = true;
		if (style.Align == dfMarkupTextAlign.Justify)
		{
			style.Align = dfMarkupTextAlign.Left;
		}
		dfMarkupBox dfMarkupBox;
		if (style.BackgroundColor.a > 0.1f)
		{
			dfMarkupBoxSprite dfMarkupBoxSprite = new dfMarkupBoxSprite(this, dfMarkupDisplayType.block, style);
			dfMarkupBoxSprite.LoadImage(base.Owner.Atlas, base.Owner.BlankTextureSprite);
			dfMarkupBoxSprite.Style.Color = style.BackgroundColor;
			dfMarkupBox = dfMarkupBoxSprite;
		}
		else
		{
			dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, style);
		}
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
		dfMarkupBox.FitToContents();
	}
}
