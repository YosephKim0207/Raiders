using System;
using UnityEngine;

// Token: 0x020004AE RID: 1198
[dfMarkupTagInfo("b")]
[dfMarkupTagInfo("strong")]
public class dfMarkupTagBold : dfMarkupTag
{
	// Token: 0x06001BD4 RID: 7124 RVA: 0x00083978 File Offset: 0x00081B78
	public dfMarkupTagBold()
		: base("b")
	{
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x00083988 File Offset: 0x00081B88
	public dfMarkupTagBold(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x00083994 File Offset: 0x00081B94
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style = base.applyTextStyleAttributes(style);
		if (style.FontStyle == FontStyle.Normal)
		{
			style.FontStyle = FontStyle.Bold;
		}
		else if (style.FontStyle == FontStyle.Italic)
		{
			style.FontStyle = FontStyle.BoldAndItalic;
		}
		base._PerformLayoutImpl(container, style);
	}
}
