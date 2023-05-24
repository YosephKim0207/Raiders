using System;

// Token: 0x0200040C RID: 1036
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class dfTooltipAttribute : Attribute
{
	// Token: 0x06001786 RID: 6022 RVA: 0x000701D0 File Offset: 0x0006E3D0
	public dfTooltipAttribute(string tooltip)
	{
		this.Tooltip = tooltip;
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06001787 RID: 6023 RVA: 0x000701E0 File Offset: 0x0006E3E0
	// (set) Token: 0x06001788 RID: 6024 RVA: 0x000701E8 File Offset: 0x0006E3E8
	public string Tooltip { get; private set; }
}
