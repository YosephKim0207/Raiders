using System;
using System.Collections.Generic;

// Token: 0x020003E6 RID: 998
public class dfJapaneseMarkupTokenizer : IDisposable, IPoolable
{
	// Token: 0x060014C6 RID: 5318 RVA: 0x000600B0 File Offset: 0x0005E2B0
	public static dfList<dfMarkupToken> Tokenize(string source)
	{
		dfList<dfMarkupToken> dfList;
		using (dfJapaneseMarkupTokenizer dfJapaneseMarkupTokenizer = ((dfJapaneseMarkupTokenizer.pool.Count <= 0) ? new dfJapaneseMarkupTokenizer() : dfJapaneseMarkupTokenizer.pool.Pop()))
		{
			dfList = dfJapaneseMarkupTokenizer.tokenize(source);
		}
		return dfList;
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x00060110 File Offset: 0x0005E310
	public void Release()
	{
		this.source = null;
		this.index = 0;
		if (!dfJapaneseMarkupTokenizer.pool.Contains(this))
		{
			dfJapaneseMarkupTokenizer.pool.Add(this);
		}
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0006013C File Offset: 0x0005E33C
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

	// Token: 0x060014C9 RID: 5321 RVA: 0x000601F4 File Offset: 0x0005E3F4
	private int estimateTokenCount(string source)
	{
		if (string.IsNullOrEmpty(source))
		{
			return 0;
		}
		return source.Length;
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x0006020C File Offset: 0x0005E40C
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

	// Token: 0x060014CB RID: 5323 RVA: 0x00060278 File Offset: 0x0005E478
	private bool isValidTag(int index, bool endTag)
	{
		for (int i = 0; i < dfJapaneseMarkupTokenizer.validTags.Count; i++)
		{
			string text = dfJapaneseMarkupTokenizer.validTags[i];
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

	// Token: 0x060014CC RID: 5324 RVA: 0x0006034C File Offset: 0x0005E54C
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

	// Token: 0x060014CD RID: 5325 RVA: 0x000603D8 File Offset: 0x0005E5D8
	private dfMarkupToken parseNonWhitespace()
	{
		int num = this.index;
		int num2 = this.index;
		this.Advance(1);
		return dfMarkupToken.Obtain(this.source, dfMarkupTokenType.Text, num, num2);
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x0006040C File Offset: 0x0005E60C
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

	// Token: 0x060014CF RID: 5327 RVA: 0x000604A0 File Offset: 0x0005E6A0
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

	// Token: 0x060014D0 RID: 5328 RVA: 0x0006060C File Offset: 0x0005E80C
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

	// Token: 0x060014D1 RID: 5329 RVA: 0x00060684 File Offset: 0x0005E884
	private char Peek()
	{
		return this.Peek(0);
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x00060690 File Offset: 0x0005E890
	private char Peek(int offset)
	{
		if (this.index + offset > this.source.Length - 1)
		{
			return '\0';
		}
		return this.source[this.index + offset];
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x000606C4 File Offset: 0x0005E8C4
	private char Advance()
	{
		return this.Advance(1);
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x000606D0 File Offset: 0x0005E8D0
	private char Advance(int amount)
	{
		this.index += amount;
		return this.Peek();
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x000606E8 File Offset: 0x0005E8E8
	public void Dispose()
	{
		this.Release();
	}

	// Token: 0x040011F7 RID: 4599
	private static dfList<dfJapaneseMarkupTokenizer> pool = new dfList<dfJapaneseMarkupTokenizer>();

	// Token: 0x040011F8 RID: 4600
	private static List<string> validTags = new List<string> { "color", "sprite" };

	// Token: 0x040011F9 RID: 4601
	private string source;

	// Token: 0x040011FA RID: 4602
	private int index;
}
