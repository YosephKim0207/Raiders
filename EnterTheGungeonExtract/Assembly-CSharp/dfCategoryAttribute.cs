using System;

// Token: 0x0200040B RID: 1035
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class dfCategoryAttribute : Attribute
{
	// Token: 0x06001783 RID: 6019 RVA: 0x000701AC File Offset: 0x0006E3AC
	public dfCategoryAttribute(string category)
	{
		this.Category = category;
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06001784 RID: 6020 RVA: 0x000701BC File Offset: 0x0006E3BC
	// (set) Token: 0x06001785 RID: 6021 RVA: 0x000701C4 File Offset: 0x0006E3C4
	public string Category { get; private set; }
}
