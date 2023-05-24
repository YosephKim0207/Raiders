using System;

// Token: 0x020003E5 RID: 997
public class dfMarkupTokenAttribute : IPoolable
{
	// Token: 0x060014C1 RID: 5313 RVA: 0x0005FFEC File Offset: 0x0005E1EC
	private dfMarkupTokenAttribute()
	{
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x0005FFF4 File Offset: 0x0005E1F4
	public static dfMarkupTokenAttribute Obtain(dfMarkupToken key, dfMarkupToken value)
	{
		dfMarkupTokenAttribute dfMarkupTokenAttribute = ((dfMarkupTokenAttribute.pool.Count <= 0) ? new dfMarkupTokenAttribute() : dfMarkupTokenAttribute.pool.Pop());
		dfMarkupTokenAttribute.Key = key;
		dfMarkupTokenAttribute.Value = value;
		return dfMarkupTokenAttribute;
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x00060038 File Offset: 0x0005E238
	public void Release()
	{
		if (this.Key != null)
		{
			this.Key.Release();
			this.Key = null;
		}
		if (this.Value != null)
		{
			this.Value.Release();
			this.Value = null;
		}
		if (!dfMarkupTokenAttribute.pool.Contains(this))
		{
			dfMarkupTokenAttribute.pool.Add(this);
		}
	}

	// Token: 0x040011F4 RID: 4596
	public dfMarkupToken Key;

	// Token: 0x040011F5 RID: 4597
	public dfMarkupToken Value;

	// Token: 0x040011F6 RID: 4598
	private static dfList<dfMarkupTokenAttribute> pool = new dfList<dfMarkupTokenAttribute>();
}
