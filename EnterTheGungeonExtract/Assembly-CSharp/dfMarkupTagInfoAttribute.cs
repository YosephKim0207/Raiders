using System;

// Token: 0x0200049D RID: 1181
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class dfMarkupTagInfoAttribute : Attribute
{
	// Token: 0x06001B6E RID: 7022 RVA: 0x00081BA4 File Offset: 0x0007FDA4
	public dfMarkupTagInfoAttribute(string tagName)
	{
		this.TagName = tagName;
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06001B6F RID: 7023 RVA: 0x00081BB4 File Offset: 0x0007FDB4
	// (set) Token: 0x06001B70 RID: 7024 RVA: 0x00081BBC File Offset: 0x0007FDBC
	public string TagName { get; set; }
}
