using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
[dfMarkupTagInfo("i")]
[dfMarkupTagInfo("em")]
public class dfMarkupTagItalic : dfMarkupTag
{
	// Token: 0x06001BDB RID: 7131 RVA: 0x00083BA0 File Offset: 0x00081DA0
	public dfMarkupTagItalic()
		: base("i")
	{
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x00083BB0 File Offset: 0x00081DB0
	public dfMarkupTagItalic(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00083BBC File Offset: 0x00081DBC
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style = base.applyTextStyleAttributes(style);
		if (style.FontStyle == FontStyle.Normal)
		{
			style.FontStyle = FontStyle.Italic;
		}
		else if (style.FontStyle == FontStyle.Bold)
		{
			style.FontStyle = FontStyle.BoldAndItalic;
		}
		base._PerformLayoutImpl(container, style);
	}
}
