using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x0200049C RID: 1180
public class dfMarkupEntity
{
	// Token: 0x06001B6B RID: 7019 RVA: 0x00081A2C File Offset: 0x0007FC2C
	public dfMarkupEntity(string entityName, string entityChar)
	{
		this.EntityName = entityName;
		this.EntityChar = entityChar;
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x00081A44 File Offset: 0x0007FC44
	public static string Replace(string text)
	{
		dfMarkupEntity.buffer.EnsureCapacity(text.Length);
		dfMarkupEntity.buffer.Length = 0;
		dfMarkupEntity.buffer.Append(text);
		for (int i = 0; i < dfMarkupEntity.HTML_ENTITIES.Count; i++)
		{
			dfMarkupEntity dfMarkupEntity = dfMarkupEntity.HTML_ENTITIES[i];
			dfMarkupEntity.buffer.Replace(dfMarkupEntity.EntityName, dfMarkupEntity.EntityChar);
		}
		return dfMarkupEntity.buffer.ToString();
	}

	// Token: 0x0400157D RID: 5501
	private static List<dfMarkupEntity> HTML_ENTITIES = new List<dfMarkupEntity>
	{
		new dfMarkupEntity("&nbsp;", " "),
		new dfMarkupEntity("&quot;", "\""),
		new dfMarkupEntity("&amp;", "&"),
		new dfMarkupEntity("&lt;", "<"),
		new dfMarkupEntity("&gt;", ">"),
		new dfMarkupEntity("&#39;", "'"),
		new dfMarkupEntity("&trade;", "™"),
		new dfMarkupEntity("&copy;", "©"),
		new dfMarkupEntity("\u00a0", " ")
	};

	// Token: 0x0400157E RID: 5502
	private static StringBuilder buffer = new StringBuilder();

	// Token: 0x0400157F RID: 5503
	public string EntityName;

	// Token: 0x04001580 RID: 5504
	public string EntityChar;
}
