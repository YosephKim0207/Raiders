using System;

// Token: 0x020003B0 RID: 944
public static class dfSpriteFlipExtensions
{
	// Token: 0x060011C5 RID: 4549 RVA: 0x00052BC8 File Offset: 0x00050DC8
	public static bool IsSet(this dfSpriteFlip value, dfSpriteFlip flag)
	{
		return flag == (value & flag);
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x00052BD0 File Offset: 0x00050DD0
	public static dfSpriteFlip SetFlag(this dfSpriteFlip value, dfSpriteFlip flag, bool on)
	{
		if (on)
		{
			return value | flag;
		}
		return value & ~flag;
	}
}
