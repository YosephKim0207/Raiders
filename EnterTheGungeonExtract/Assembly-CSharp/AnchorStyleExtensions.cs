using System;

// Token: 0x020003AE RID: 942
public static class AnchorStyleExtensions
{
	// Token: 0x060011C1 RID: 4545 RVA: 0x00052B9C File Offset: 0x00050D9C
	public static bool IsFlagSet(this dfAnchorStyle value, dfAnchorStyle flag)
	{
		return flag == (value & flag);
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x00052BA4 File Offset: 0x00050DA4
	public static bool IsAnyFlagSet(this dfAnchorStyle value, dfAnchorStyle flag)
	{
		return dfAnchorStyle.None != (value & flag);
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x00052BB0 File Offset: 0x00050DB0
	public static dfAnchorStyle SetFlag(this dfAnchorStyle value, dfAnchorStyle flag)
	{
		return value | flag;
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00052BB8 File Offset: 0x00050DB8
	public static dfAnchorStyle SetFlag(this dfAnchorStyle value, dfAnchorStyle flag, bool on)
	{
		if (on)
		{
			return value | flag;
		}
		return value & ~flag;
	}
}
