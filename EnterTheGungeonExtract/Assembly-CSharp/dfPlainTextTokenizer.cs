using System;

// Token: 0x020003E8 RID: 1000
public class dfPlainTextTokenizer
{
	// Token: 0x060014EC RID: 5356 RVA: 0x00060F2C File Offset: 0x0005F12C
	public static dfList<dfMarkupToken> Tokenize(string source)
	{
		if (dfPlainTextTokenizer.singleton == null)
		{
			dfPlainTextTokenizer.singleton = new dfPlainTextTokenizer();
		}
		return dfPlainTextTokenizer.singleton.tokenize(source);
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x00060F50 File Offset: 0x0005F150
	private dfList<dfMarkupToken> tokenize(string source)
	{
		dfList<dfMarkupToken> dfList = dfList<dfMarkupToken>.Obtain();
		dfList.EnsureCapacity(this.estimateTokenCount(source));
		dfList.AutoReleaseItems = true;
		int i = 0;
		int num = 0;
		int length = source.Length;
		while (i < length)
		{
			if (source[i] == '\r')
			{
				i++;
				num = i;
			}
			else
			{
				while (i < length && !char.IsWhiteSpace(source[i]))
				{
					i++;
				}
				if (i > num)
				{
					dfList.Add(dfMarkupToken.Obtain(source, dfMarkupTokenType.Text, num, i - 1));
					num = i;
				}
				if (i < length && source[i] == '\n')
				{
					dfList.Add(dfMarkupToken.Obtain(source, dfMarkupTokenType.Newline, i, i));
					i++;
					num = i;
				}
				while (i < length && source[i] != '\n' && source[i] != '\r' && char.IsWhiteSpace(source[i]))
				{
					i++;
				}
				if (i > num)
				{
					dfList.Add(dfMarkupToken.Obtain(source, dfMarkupTokenType.Whitespace, num, i - 1));
					num = i;
				}
			}
		}
		return dfList;
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x00061064 File Offset: 0x0005F264
	private int estimateTokenCount(string source)
	{
		if (string.IsNullOrEmpty(source))
		{
			return 0;
		}
		int num = 1;
		bool flag = char.IsWhiteSpace(source[0]);
		for (int i = 1; i < source.Length; i++)
		{
			char c = source[i];
			if (char.IsControl(c))
			{
				num++;
			}
			else
			{
				bool flag2 = char.IsWhiteSpace(c);
				if (flag2 != flag)
				{
					num++;
					flag = flag2;
				}
			}
		}
		return num;
	}

	// Token: 0x040011FF RID: 4607
	private static dfPlainTextTokenizer singleton;
}
