using System;
using UnityEngine;

// Token: 0x02000372 RID: 882
public static class Vibration
{
	// Token: 0x06000E94 RID: 3732 RVA: 0x000449E8 File Offset: 0x00042BE8
	public static float ConvertFromShakeMagnitude(float magnitude)
	{
		if (magnitude < 0.01f)
		{
			return 0f;
		}
		return 0.4f + Mathf.InverseLerp(0f, 1f, magnitude) * 0.6f;
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x00044A18 File Offset: 0x00042C18
	public static float ConvertTime(Vibration.Time time)
	{
		if (time == Vibration.Time.Instant)
		{
			return 0f;
		}
		if (time == Vibration.Time.Quick)
		{
			return 0.15f;
		}
		if (time == Vibration.Time.Normal)
		{
			return 0.25f;
		}
		if (time != Vibration.Time.Slow)
		{
			return 0f;
		}
		return 0.5f;
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x00044A68 File Offset: 0x00042C68
	public static float ConvertStrength(Vibration.Strength strength)
	{
		if (strength == Vibration.Strength.UltraLight)
		{
			return 0.2f;
		}
		if (strength == Vibration.Strength.Light)
		{
			return 0.4f;
		}
		if (strength == Vibration.Strength.Medium)
		{
			return 0.7f;
		}
		if (strength != Vibration.Strength.Hard)
		{
			return 0.5f;
		}
		return 1f;
	}

	// Token: 0x02000373 RID: 883
	public enum Time
	{
		// Token: 0x04000E50 RID: 3664
		Instant = 5,
		// Token: 0x04000E51 RID: 3665
		Quick = 10,
		// Token: 0x04000E52 RID: 3666
		Normal = 20,
		// Token: 0x04000E53 RID: 3667
		Slow = 30
	}

	// Token: 0x02000374 RID: 884
	public enum Strength
	{
		// Token: 0x04000E55 RID: 3669
		UltraLight = 5,
		// Token: 0x04000E56 RID: 3670
		Light = 10,
		// Token: 0x04000E57 RID: 3671
		Medium = 20,
		// Token: 0x04000E58 RID: 3672
		Hard = 30
	}
}
