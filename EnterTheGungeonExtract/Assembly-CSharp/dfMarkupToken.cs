using System;
using UnityEngine;

// Token: 0x020003E4 RID: 996
public class dfMarkupToken : IPoolable
{
	// Token: 0x060014A9 RID: 5289 RVA: 0x0005FC88 File Offset: 0x0005DE88
	protected dfMarkupToken()
	{
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0005FC9C File Offset: 0x0005DE9C
	public static dfMarkupToken Obtain(string source, dfMarkupTokenType type, int startIndex, int endIndex)
	{
		dfMarkupToken dfMarkupToken = ((dfMarkupToken.pool.Count <= 0) ? new dfMarkupToken() : dfMarkupToken.pool.Pop());
		dfMarkupToken.inUse = true;
		dfMarkupToken.Source = source;
		dfMarkupToken.TokenType = type;
		dfMarkupToken.StartOffset = startIndex;
		dfMarkupToken.EndOffset = Mathf.Min(source.Length - 1, endIndex);
		return dfMarkupToken;
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x0005FD00 File Offset: 0x0005DF00
	public void Release()
	{
		if (!this.inUse)
		{
			return;
		}
		this.inUse = false;
		this.value = null;
		this.Source = null;
		this.TokenType = dfMarkupTokenType.Invalid;
		int num = 0;
		this.Height = num;
		this.Width = num;
		num = 0;
		this.EndOffset = num;
		this.StartOffset = num;
		this.attributes.ReleaseItems();
		dfMarkupToken.pool.Add(this);
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x060014AC RID: 5292 RVA: 0x0005FD6C File Offset: 0x0005DF6C
	public int AttributeCount
	{
		get
		{
			return this.attributes.Count;
		}
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x060014AD RID: 5293 RVA: 0x0005FD7C File Offset: 0x0005DF7C
	// (set) Token: 0x060014AE RID: 5294 RVA: 0x0005FD84 File Offset: 0x0005DF84
	public dfMarkupTokenType TokenType { get; private set; }

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x060014AF RID: 5295 RVA: 0x0005FD90 File Offset: 0x0005DF90
	// (set) Token: 0x060014B0 RID: 5296 RVA: 0x0005FD98 File Offset: 0x0005DF98
	public string Source { get; private set; }

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0005FDA4 File Offset: 0x0005DFA4
	// (set) Token: 0x060014B2 RID: 5298 RVA: 0x0005FDAC File Offset: 0x0005DFAC
	public int StartOffset { get; private set; }

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0005FDB8 File Offset: 0x0005DFB8
	// (set) Token: 0x060014B4 RID: 5300 RVA: 0x0005FDC0 File Offset: 0x0005DFC0
	public int EndOffset { get; private set; }

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0005FDCC File Offset: 0x0005DFCC
	// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0005FDD4 File Offset: 0x0005DFD4
	public int Width { get; internal set; }

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0005FDE0 File Offset: 0x0005DFE0
	// (set) Token: 0x060014B8 RID: 5304 RVA: 0x0005FDE8 File Offset: 0x0005DFE8
	public int Height { get; set; }

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0005FDF4 File Offset: 0x0005DFF4
	public int Length
	{
		get
		{
			return this.EndOffset - this.StartOffset + 1;
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x060014BA RID: 5306 RVA: 0x0005FE08 File Offset: 0x0005E008
	public string Value
	{
		get
		{
			if (this.value == null)
			{
				int num = Mathf.Min(this.EndOffset - this.StartOffset + 1, this.Source.Length - this.StartOffset);
				this.value = this.Source.Substring(this.StartOffset, num);
			}
			return this.value;
		}
	}

	// Token: 0x17000487 RID: 1159
	public char this[int index]
	{
		get
		{
			if (index < 0 || index >= this.Length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range ({2}:{1})", index, this.Length, this.Value));
			}
			return this.Source[this.StartOffset + index];
		}
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x0005FEC4 File Offset: 0x0005E0C4
	internal bool Matches(dfMarkupToken other)
	{
		int length = this.Length;
		if (length != other.Length)
		{
			return false;
		}
		for (int i = 0; i < length; i++)
		{
			if (char.ToLower(this.Source[this.StartOffset + i]) != char.ToLower(other.Source[other.StartOffset + i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x0005FF30 File Offset: 0x0005E130
	internal bool Matches(string value)
	{
		int length = this.Length;
		if (length != value.Length)
		{
			return false;
		}
		for (int i = 0; i < length; i++)
		{
			if (char.ToLower(this.Source[this.StartOffset + i]) != char.ToLower(value[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x0005FF90 File Offset: 0x0005E190
	internal void AddAttribute(dfMarkupToken key, dfMarkupToken value)
	{
		this.attributes.Add(dfMarkupTokenAttribute.Obtain(key, value));
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x0005FFA4 File Offset: 0x0005E1A4
	public dfMarkupTokenAttribute GetAttribute(int index)
	{
		if (index < 0 || index >= this.attributes.Count)
		{
			throw new IndexOutOfRangeException("Invalid attribute index: " + index);
		}
		return this.attributes[index];
	}

	// Token: 0x040011EA RID: 4586
	private static dfList<dfMarkupToken> pool = new dfList<dfMarkupToken>();

	// Token: 0x040011EB RID: 4587
	private bool inUse;

	// Token: 0x040011EC RID: 4588
	private string value;

	// Token: 0x040011ED RID: 4589
	private dfList<dfMarkupTokenAttribute> attributes = new dfList<dfMarkupTokenAttribute>();
}
