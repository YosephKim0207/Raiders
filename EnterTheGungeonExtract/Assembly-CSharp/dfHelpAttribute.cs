using System;

// Token: 0x0200040D RID: 1037
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class dfHelpAttribute : Attribute
{
	// Token: 0x06001789 RID: 6025 RVA: 0x000701F4 File Offset: 0x0006E3F4
	public dfHelpAttribute(string url)
	{
		this.HelpURL = url;
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x0600178A RID: 6026 RVA: 0x00070204 File Offset: 0x0006E404
	// (set) Token: 0x0600178B RID: 6027 RVA: 0x0007020C File Offset: 0x0006E40C
	public string HelpURL { get; private set; }
}
