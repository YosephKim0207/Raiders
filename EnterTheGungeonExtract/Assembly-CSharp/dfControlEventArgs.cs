using System;

// Token: 0x020003BD RID: 957
public class dfControlEventArgs
{
	// Token: 0x060011F2 RID: 4594 RVA: 0x00053008 File Offset: 0x00051208
	internal dfControlEventArgs(dfControl Target)
	{
		this.Source = Target;
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00053018 File Offset: 0x00051218
	// (set) Token: 0x060011F4 RID: 4596 RVA: 0x00053020 File Offset: 0x00051220
	public dfControl Source { get; internal set; }

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x060011F5 RID: 4597 RVA: 0x0005302C File Offset: 0x0005122C
	// (set) Token: 0x060011F6 RID: 4598 RVA: 0x00053034 File Offset: 0x00051234
	public bool Used { get; private set; }

	// Token: 0x060011F7 RID: 4599 RVA: 0x00053040 File Offset: 0x00051240
	public void Use()
	{
		this.Used = true;
	}
}
