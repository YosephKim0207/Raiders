using System;
using UnityEngine;

// Token: 0x020004AF RID: 1199
[dfMarkupTagInfo("h3")]
[dfMarkupTagInfo("h2")]
[dfMarkupTagInfo("h1")]
[dfMarkupTagInfo("h6")]
[dfMarkupTagInfo("h5")]
[dfMarkupTagInfo("h4")]
public class dfMarkupTagHeading : dfMarkupTag
{
	// Token: 0x06001BD7 RID: 7127 RVA: 0x000839E0 File Offset: 0x00081BE0
	public dfMarkupTagHeading()
		: base("h1")
	{
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x000839F0 File Offset: 0x00081BF0
	public dfMarkupTagHeading(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x000839FC File Offset: 0x00081BFC
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		dfMarkupBorders dfMarkupBorders = default(dfMarkupBorders);
		dfMarkupStyle dfMarkupStyle = this.applyDefaultStyles(style, ref dfMarkupBorders);
		dfMarkupStyle = base.applyTextStyleAttributes(dfMarkupStyle);
		dfMarkupAttribute dfMarkupAttribute = base.findAttribute(new string[] { "margin" });
		if (dfMarkupAttribute != null)
		{
			dfMarkupBorders = dfMarkupBorders.Parse(dfMarkupAttribute.Value);
		}
		dfMarkupBox dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.block, dfMarkupStyle);
		dfMarkupBox.Margins = dfMarkupBorders;
		container.AddChild(dfMarkupBox);
		base._PerformLayoutImpl(dfMarkupBox, dfMarkupStyle);
		dfMarkupBox.FitToContents();
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x00083A70 File Offset: 0x00081C70
	private dfMarkupStyle applyDefaultStyles(dfMarkupStyle style, ref dfMarkupBorders margins)
	{
		float num = 1f;
		float num2 = 1f;
		string tagName = base.TagName;
		if (tagName != null)
		{
			if (!(tagName == "h1"))
			{
				if (!(tagName == "h2"))
				{
					if (!(tagName == "h3"))
					{
						if (!(tagName == "h4"))
						{
							if (!(tagName == "h5"))
							{
								if (tagName == "h6")
								{
									num2 = 0.75f;
									num = 1.75f;
								}
							}
							else
							{
								num2 = 0.85f;
								num = 1.5f;
							}
						}
						else
						{
							num2 = 1.15f;
							num = 0f;
						}
					}
					else
					{
						num2 = 1.35f;
						num = 0.85f;
					}
				}
				else
				{
					num2 = 1.5f;
					num = 0.75f;
				}
			}
			else
			{
				num2 = 2f;
				num = 0.65f;
			}
		}
		style.FontSize = (int)((float)style.FontSize * num2);
		style.FontStyle = FontStyle.Bold;
		style.Align = dfMarkupTextAlign.Left;
		num *= (float)style.FontSize;
		margins.top = (margins.bottom = (int)num);
		return style;
	}
}
