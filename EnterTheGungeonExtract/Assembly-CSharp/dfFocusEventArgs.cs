using System;

// Token: 0x020003BE RID: 958
public class dfFocusEventArgs : dfControlEventArgs
{
	// Token: 0x060011F8 RID: 4600 RVA: 0x0005304C File Offset: 0x0005124C
	internal dfFocusEventArgs(dfControl GotFocus, dfControl LostFocus, bool AllowScrolling)
		: base(GotFocus)
	{
		this.LostFocus = LostFocus;
		this.AllowScrolling = AllowScrolling;
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060011F9 RID: 4601 RVA: 0x00053064 File Offset: 0x00051264
	public dfControl GotFocus
	{
		get
		{
			return base.Source;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x060011FA RID: 4602 RVA: 0x0005306C File Offset: 0x0005126C
	// (set) Token: 0x060011FB RID: 4603 RVA: 0x00053074 File Offset: 0x00051274
	public dfControl LostFocus { get; private set; }

	// Token: 0x04001002 RID: 4098
	public bool AllowScrolling;
}
