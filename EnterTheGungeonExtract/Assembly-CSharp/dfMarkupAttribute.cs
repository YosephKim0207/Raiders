using System;

// Token: 0x020004A0 RID: 1184
public class dfMarkupAttribute
{
	// Token: 0x06001B85 RID: 7045 RVA: 0x00081EF8 File Offset: 0x000800F8
	public dfMarkupAttribute(string name, string value)
	{
		this.Name = name;
		this.Value = value;
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06001B86 RID: 7046 RVA: 0x00081F10 File Offset: 0x00080110
	// (set) Token: 0x06001B87 RID: 7047 RVA: 0x00081F18 File Offset: 0x00080118
	public string Name { get; set; }

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06001B88 RID: 7048 RVA: 0x00081F24 File Offset: 0x00080124
	// (set) Token: 0x06001B89 RID: 7049 RVA: 0x00081F2C File Offset: 0x0008012C
	public string Value { get; set; }

	// Token: 0x06001B8A RID: 7050 RVA: 0x00081F38 File Offset: 0x00080138
	public override string ToString()
	{
		return string.Format("{0}='{1}'", this.Name, this.Value);
	}
}
