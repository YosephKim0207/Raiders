using System;

// Token: 0x020003A7 RID: 935
public static class dfMouseButtonsExtensions
{
	// Token: 0x060011BE RID: 4542 RVA: 0x00052B34 File Offset: 0x00050D34
	public static bool IsSet(this dfMouseButtons value, dfMouseButtons flag)
	{
		return flag == (value & flag);
	}
}
