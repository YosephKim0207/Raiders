using System;
using System.Collections.Generic;

// Token: 0x020003E7 RID: 999
public class dfMarkupTokenizer : IDisposable, IPoolable
{
	// Token: 0x060014D8 RID: 5336 RVA: 0x00060734 File Offset: 0x0005E934
	public static dfList<dfMarkupToken> Tokenize(string source)
	{
		dfList<dfMarkupToken> dfList;
		using (dfMarkupTokenizer dfMarkupTokenizer = ((dfMarkupTokenizer.pool.Count <= 0) ? new dfMarkupTokenizer() : dfMarkupTokenizer.pool.Pop()))
		{
			dfList = dfMarkupTokenizer.tokenize(source);
		}
		return dfList;
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x00060794 File Offset: 0x0005E994
	public void Release()
	{
		this.source = null;
		this.index = 0;
		if (!dfMarkupTokenizer.pool.Contains(this))
		{
			dfMarkupTokenizer.pool.Add(this);
		}
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x000607C0 File Offset: 0x0005E9C0
	private dfList<dfMarkupToken> tokenize(string source)
	{
		dfList<dfMarkupToken> dfList = dfList<dfMarkupToken>.Obtain();
		dfList.EnsureCapacity(this.estimateTokenCount(source));
		dfList.AutoReleaseItems = true;
		this.source = source;
		this.index = 0;
		while (this.index < source.Length)
		{
			char c = this.Peek();
			if (this.AtTagPosition())
			{
				dfMarkupToken dfMarkupToken = this.parseTag();
				if (dfMarkupToken != null)
				{
					dfList.Add(dfMarkupToken);
				}
			}
			else
			{
				dfMarkupToken dfMarkupToken2 = null;
				if (char.IsWhiteSpace(c))
				{
					if (c != '\r')
					{
						dfMarkupToken2 = this.parseWhitespace();
					}
				}
				else
				{
					dfMarkupToken2 = this.parseNonWhitespace();
				}
				if (dfMarkupToken2 == null)
				{
					this.Advance();
				}
				else
				{
					dfList.Add(dfMarkupToken2);
				}
			}
		}
		return dfList;
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x00060878 File Offset: 0x0005EA78
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
			if (char.IsControl(c) || c == '<')
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

	// Token: 0x060014DC RID: 5340 RVA: 0x000608F4 File Offset: 0x0005EAF4
	private bool AtTagPosition()
	{
		if (this.Peek() != '[')
		{
			return false;
		}
		char c = this.Peek(1);
		if (c == '/')
		{
			return char.IsLetter(this.Peek(2)) && this.isValidTag(this.index + 2, true);
		}
		return char.IsLetter(c) && this.isValidTag(this.index + 1, false);
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x00060960 File Offset: 0x0005EB60
	private bool isValidTag(int index, bool endTag)
	{
		for (int i = 0; i < dfMarkupTokenizer.validTags.Count; i++)
		{
			string text = dfMarkupTokenizer.validTags[i];
			bool flag = true;
			int num = 0;
			while (num < text.Length - 1 && num + index < this.source.Length - 1)
			{
				if (!endTag && this.source[num + index] == ' ')
				{
					break;
				}
				if (this.source[num + index] == ']')
				{
					break;
				}
				if (char.ToLowerInvariant(text[num]) != char.ToLowerInvariant(this.source[num + index]))
				{
					flag = false;
					break;
				}
				num++;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x00060A34 File Offset: 0x0005EC34
	private dfMarkupToken parseQuotedString()
	{
		char c = this.Peek();
		if (c != '"' && c != '\'')
		{
			return null;
		}
		this.Advance();
		int num = this.index;
		int num2 = this.index;
		while (this.index < this.source.Length && this.Advance() != c)
		{
			num2++;
		}
		if (this.Peek() == c)
		{
			this.Advance();
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num2);
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x00060AC0 File Offset: 0x0005ECC0
	private dfMarkupToken parseNonWhitespace()
	{
		int num = this.index;
		int num2 = this.index;
		while (this.index < this.source.Length)
		{
			char c = this.Advance();
			if (char.IsWhiteSpace(c) || this.AtTagPosition())
			{
				break;
			}
			num2++;
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num2);
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x00060B2C File Offset: 0x0005ED2C
	private dfMarkupToken parseWhitespace()
	{
		int num = this.index;
		int num2 = this.index;
		if (this.Peek() == '\n')
		{
			this.Advance();
			return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Newline, num, num);
		}
		while (this.index < this.source.Length)
		{
			char c = this.Advance();
			if (c == '\n' || c == '\r' || !char.IsWhiteSpace(c))
			{
				break;
			}
			num2++;
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Whitespace, num, num2);
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x00060BC0 File Offset: 0x0005EDC0
	private dfMarkupToken parseWord()
	{
		if (!char.IsLetter(this.Peek()))
		{
			return null;
		}
		int num = this.index;
		int num2 = this.index;
		while (this.index < this.source.Length && char.IsLetter(this.Advance()))
		{
			num2++;
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num2);
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x00060C2C File Offset: 0x0005EE2C
	private dfMarkupToken parseTag()
	{
		if (this.Peek() != '[')
		{
			return null;
		}
		char c = this.Peek(1);
		if (c == '/')
		{
			return this.parseEndTag();
		}
		this.Advance();
		c = this.Peek();
		if (!char.IsLetterOrDigit(c))
		{
			return null;
		}
		int num = this.index;
		int num2 = this.index;
		while (this.index < this.source.Length && char.IsLetterOrDigit(this.Advance()))
		{
			num2++;
		}
		dfMarkupToken dfMarkupToken = dfMarkupToken.Obtain(this.source, dfMarkupTokenType.StartTag, num, num2);
		if (this.index < this.source.Length && this.Peek() != ']')
		{
			c = this.Peek();
			if (char.IsWhiteSpace(c))
			{
				this.parseWhitespace();
			}
			int num3 = this.index;
			int num4 = this.index;
			if (this.Peek() == '"')
			{
				dfMarkupToken dfMarkupToken2 = this.parseQuotedString();
				dfMarkupToken.AddAttribute(dfMarkupToken2, dfMarkupToken2);
			}
			else
			{
				while (this.index < this.source.Length && this.Advance() != ']')
				{
					num4++;
				}
				dfMarkupToken dfMarkupToken3 = dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num3, num4);
				dfMarkupToken.AddAttribute(dfMarkupToken3, dfMarkupToken3);
			}
		}
		if (this.Peek() == ']')
		{
			this.Advance();
		}
		return dfMarkupToken;
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x00060D98 File Offset: 0x0005EF98
	private dfMarkupToken parseAttributeValue()
	{
		int num = this.index;
		int num2 = this.index;
		while (this.index < this.source.Length)
		{
			char c = this.Advance();
			if (c == ']' || char.IsWhiteSpace(c))
			{
				break;
			}
			num2++;
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num2);
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x00060E04 File Offset: 0x0005F004
	private dfMarkupToken parseEndTag()
	{
		this.Advance(2);
		int num = this.index;
		int num2 = this.index;
		while (this.index < this.source.Length && char.IsLetterOrDigit(this.Advance()))
		{
			num2++;
		}
		if (this.Peek() == ']')
		{
			this.Advance();
		}
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.EndTag, num, num2);
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x00060E7C File Offset: 0x0005F07C
	private char Peek()
	{
		return this.Peek(0);
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x00060E88 File Offset: 0x0005F088
	private char Peek(int offset)
	{
		if (this.index + offset > this.source.Length - 1)
		{
			return '\0';
		}
		return this.source[this.index + offset];
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x00060EBC File Offset: 0x0005F0BC
	private char Advance()
	{
		return this.Advance(1);
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x00060EC8 File Offset: 0x0005F0C8
	private char Advance(int amount)
	{
		this.index += amount;
		return this.Peek();
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x00060EE0 File Offset: 0x0005F0E0
	public void Dispose()
	{
		this.Release();
	}

	// Token: 0x040011FB RID: 4603
	private static dfList<dfMarkupTokenizer> pool = new dfList<dfMarkupTokenizer>();

	// Token: 0x040011FC RID: 4604
	private static List<string> validTags = new List<string> { "color", "sprite" };

	// Token: 0x040011FD RID: 4605
	private string source;

	// Token: 0x040011FE RID: 4606
	private int index;
}
