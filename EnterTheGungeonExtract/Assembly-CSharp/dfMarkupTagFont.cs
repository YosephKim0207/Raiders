using System;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
[dfMarkupTagInfo("font")]
public class dfMarkupTagFont : dfMarkupTag
{
	// Token: 0x06001BE9 RID: 7145 RVA: 0x00084024 File Offset: 0x00082224
	public dfMarkupTagFont()
		: base("font")
	{
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x00084034 File Offset: 0x00082234
	public dfMarkupTagFont(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00084040 File Offset: 0x00082240
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		dfMarkupAttribute dfMarkupAttribute = base.findAttribute(new string[] { "name", "face" });
		if (dfMarkupAttribute != null)
		{
			style.Font = dfDynamicFont.FindByName(dfMarkupAttribute.Value) ?? style.Font;
		}
		dfMarkupAttribute dfMarkupAttribute2 = base.findAttribute(new string[] { "size", "font-size" });
		if (dfMarkupAttribute2 != null)
		{
			style.FontSize = dfMarkupStyle.ParseSize(dfMarkupAttribute2.Value, style.FontSize);
		}
		dfMarkupAttribute dfMarkupAttribute3 = base.findAttribute(new string[] { "color" });
		if (dfMarkupAttribute3 != null)
		{
			style.Color = dfMarkupStyle.ParseColor(dfMarkupAttribute3.Value, Color.red);
			style.Color.a = style.Opacity;
		}
		dfMarkupAttribute dfMarkupAttribute4 = base.findAttribute(new string[] { "style" });
		if (dfMarkupAttribute4 != null)
		{
			style.FontStyle = dfMarkupStyle.ParseFontStyle(dfMarkupAttribute4.Value, style.FontStyle);
		}
		base._PerformLayoutImpl(container, style);
	}
}
