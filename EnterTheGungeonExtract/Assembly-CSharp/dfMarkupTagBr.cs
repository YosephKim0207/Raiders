using System;

// Token: 0x020004B2 RID: 1202
[dfMarkupTagInfo("br")]
public class dfMarkupTagBr : dfMarkupTag
{
	// Token: 0x06001BE1 RID: 7137 RVA: 0x00083D2C File Offset: 0x00081F2C
	public dfMarkupTagBr()
		: base("br")
	{
		this.IsClosedTag = true;
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x00083D40 File Offset: 0x00081F40
	public dfMarkupTagBr(dfMarkupTag original)
		: base(original)
	{
		this.IsClosedTag = true;
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x00083D50 File Offset: 0x00081F50
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		container.AddLineBreak();
	}
}
