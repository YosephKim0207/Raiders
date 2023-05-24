using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// Token: 0x0200049F RID: 1183
public class dfMarkupString : dfMarkupElement
{
	// Token: 0x06001B7A RID: 7034 RVA: 0x00081C3C File Offset: 0x0007FE3C
	public dfMarkupString(string text)
	{
		this.Text = this.processWhitespace(dfMarkupEntity.Replace(text));
		this.isWhitespace = dfMarkupString.whitespacePattern.IsMatch(this.Text);
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06001B7B RID: 7035 RVA: 0x00081C6C File Offset: 0x0007FE6C
	// (set) Token: 0x06001B7C RID: 7036 RVA: 0x00081C74 File Offset: 0x0007FE74
	public string Text { get; private set; }

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06001B7D RID: 7037 RVA: 0x00081C80 File Offset: 0x0007FE80
	public bool IsWhitespace
	{
		get
		{
			return this.isWhitespace;
		}
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x00081C88 File Offset: 0x0007FE88
	public override string ToString()
	{
		return this.Text;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x00081C90 File Offset: 0x0007FE90
	internal dfMarkupElement SplitWords()
	{
		dfMarkupTagSpan dfMarkupTagSpan = dfMarkupTagSpan.Obtain();
		int i = 0;
		int num = 0;
		int length = this.Text.Length;
		while (i < length)
		{
			while (i < length && !char.IsWhiteSpace(this.Text[i]))
			{
				i++;
			}
			if (i > num)
			{
				dfMarkupTagSpan.AddChildNode(dfMarkupString.Obtain(this.Text.Substring(num, i - num)));
				num = i;
			}
			while (i < length && this.Text[i] != '\n' && char.IsWhiteSpace(this.Text[i]))
			{
				i++;
			}
			if (i > num)
			{
				dfMarkupTagSpan.AddChildNode(dfMarkupString.Obtain(this.Text.Substring(num, i - num)));
				num = i;
			}
			if (i < length && this.Text[i] == '\n')
			{
				dfMarkupTagSpan.AddChildNode(dfMarkupString.Obtain("\n"));
				i = (num = i + 1);
			}
		}
		return dfMarkupTagSpan;
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x00081D98 File Offset: 0x0007FF98
	protected override void _PerformLayoutImpl(dfMarkupBox container, dfMarkupStyle style)
	{
		if (style.Font == null)
		{
			return;
		}
		string text = ((!style.PreserveWhitespace && this.isWhitespace) ? " " : this.Text);
		dfMarkupBoxText dfMarkupBoxText = dfMarkupBoxText.Obtain(this, dfMarkupDisplayType.inline, style);
		dfMarkupBoxText.SetText(text);
		container.AddChild(dfMarkupBoxText);
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x00081DF8 File Offset: 0x0007FFF8
	internal static dfMarkupString Obtain(string text)
	{
		if (dfMarkupString.objectPool.Count > 0)
		{
			dfMarkupString dfMarkupString = dfMarkupString.objectPool.Dequeue();
			dfMarkupString.Text = dfMarkupEntity.Replace(text);
			dfMarkupString.isWhitespace = dfMarkupString.whitespacePattern.IsMatch(dfMarkupString.Text);
			return dfMarkupString;
		}
		return new dfMarkupString(text);
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x00081E4C File Offset: 0x0008004C
	internal override void Release()
	{
		base.Release();
		dfMarkupString.objectPool.Enqueue(this);
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x00081E60 File Offset: 0x00080060
	private string processWhitespace(string text)
	{
		dfMarkupString.buffer.Length = 0;
		dfMarkupString.buffer.Append(text);
		dfMarkupString.buffer.Replace("\r\n", "\n");
		dfMarkupString.buffer.Replace("\r", "\n");
		dfMarkupString.buffer.Replace("\t", "    ");
		return dfMarkupString.buffer.ToString();
	}

	// Token: 0x04001584 RID: 5508
	private static StringBuilder buffer = new StringBuilder();

	// Token: 0x04001585 RID: 5509
	private static Regex whitespacePattern = new Regex("\\s+");

	// Token: 0x04001586 RID: 5510
	private static Queue<dfMarkupString> objectPool = new Queue<dfMarkupString>();

	// Token: 0x04001588 RID: 5512
	private bool isWhitespace;
}
