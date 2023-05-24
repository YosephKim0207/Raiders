using System;

// Token: 0x020004AA RID: 1194
[dfMarkupTagInfo("a")]
public class dfMarkupTagAnchor : dfMarkupTag
{
	// Token: 0x06001BC4 RID: 7108 RVA: 0x00083378 File Offset: 0x00081578
	public dfMarkupTagAnchor()
		: base("a")
	{
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x00083388 File Offset: 0x00081588
	public dfMarkupTagAnchor(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x00083394 File Offset: 0x00081594
	public string HRef
	{
		get
		{
			dfMarkupAttribute dfMarkupAttribute = base.findAttribute(new string[] { "href" });
			return (dfMarkupAttribute == null) ? string.Empty : dfMarkupAttribute.Value;
		}
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x000833CC File Offset: 0x000815CC
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style.TextDecoration = dfMarkupTextDecoration.Underline;
		style = base.applyTextStyleAttributes(style);
		int i = 0;
		while (i < base.ChildNodes.Count)
		{
			dfMarkupElement dfMarkupElement = base.ChildNodes[i];
			if (!(dfMarkupElement is dfMarkupString))
			{
				goto IL_63;
			}
			dfMarkupString dfMarkupString = dfMarkupElement as dfMarkupString;
			if (!(dfMarkupString.Text == "\n"))
			{
				goto IL_63;
			}
			if (style.PreserveWhitespace)
			{
				container.AddLineBreak();
			}
			IL_6B:
			i++;
			continue;
			IL_63:
			dfMarkupElement.PerformLayout(container, style);
			goto IL_6B;
		}
	}
}
