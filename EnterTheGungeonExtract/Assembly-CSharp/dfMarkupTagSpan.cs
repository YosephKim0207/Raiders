using System;
using System.Collections.Generic;

// Token: 0x020004A9 RID: 1193
[dfMarkupTagInfo("span")]
public class dfMarkupTagSpan : dfMarkupTag
{
	// Token: 0x06001BBE RID: 7102 RVA: 0x00083238 File Offset: 0x00081438
	public dfMarkupTagSpan()
		: base("span")
	{
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x00083248 File Offset: 0x00081448
	public dfMarkupTagSpan(dfMarkupTag original)
		: base(original)
	{
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x00083254 File Offset: 0x00081454
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		style = base.applyTextStyleAttributes(style);
		dfMarkupBox dfMarkupBox = container;
		dfMarkupAttribute dfMarkupAttribute = base.findAttribute(new string[] { "margin" });
		if (dfMarkupAttribute != null)
		{
			dfMarkupBox = new dfMarkupBox(this, dfMarkupDisplayType.inlineBlock, style);
			dfMarkupBox.Margins = dfMarkupBorders.Parse(dfMarkupAttribute.Value);
			dfMarkupBox.Margins.top = 0;
			dfMarkupBox.Margins.bottom = 0;
			container.AddChild(dfMarkupBox);
		}
		int i = 0;
		while (i < base.ChildNodes.Count)
		{
			dfMarkupElement dfMarkupElement = base.ChildNodes[i];
			if (!(dfMarkupElement is dfMarkupString))
			{
				goto IL_B3;
			}
			dfMarkupString dfMarkupString = dfMarkupElement as dfMarkupString;
			if (!(dfMarkupString.Text == "\n"))
			{
				goto IL_B3;
			}
			if (style.PreserveWhitespace)
			{
				dfMarkupBox.AddLineBreak();
			}
			IL_BB:
			i++;
			continue;
			IL_B3:
			dfMarkupElement.PerformLayout(dfMarkupBox, style);
			goto IL_BB;
		}
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x00083334 File Offset: 0x00081534
	internal static dfMarkupTagSpan Obtain()
	{
		if (dfMarkupTagSpan.objectPool.Count > 0)
		{
			return dfMarkupTagSpan.objectPool.Dequeue();
		}
		return new dfMarkupTagSpan();
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00083358 File Offset: 0x00081558
	internal override void Release()
	{
		base.Release();
		dfMarkupTagSpan.objectPool.Enqueue(this);
	}

	// Token: 0x040015C0 RID: 5568
	private static Queue<dfMarkupTagSpan> objectPool = new Queue<dfMarkupTagSpan>();
}
