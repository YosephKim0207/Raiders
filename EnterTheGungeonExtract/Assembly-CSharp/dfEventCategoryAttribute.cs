using System;

// Token: 0x020003BC RID: 956
[AttributeUsage(AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class dfEventCategoryAttribute : Attribute
{
	// Token: 0x060011EF RID: 4591 RVA: 0x00052FE4 File Offset: 0x000511E4
	public dfEventCategoryAttribute(string category)
	{
		this.Category = category;
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x060011F0 RID: 4592 RVA: 0x00052FF4 File Offset: 0x000511F4
	// (set) Token: 0x060011F1 RID: 4593 RVA: 0x00052FFC File Offset: 0x000511FC
	public string Category { get; private set; }
}
