using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02001255 RID: 4693
public static class AlienFXInterface
{
	// Token: 0x0600693C RID: 26940
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr LoadLibrary(string lpFileName);

	// Token: 0x0600693D RID: 26941
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool SetDllDirectory(string lpPathName);

	// Token: 0x0600693E RID: 26942
	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	// Token: 0x0600693F RID: 26943
	[DllImport("LightFX")]
	public static extern uint LFX_Initialize();

	// Token: 0x06006940 RID: 26944
	[DllImport("LightFX")]
	public static extern uint LFX_Update();

	// Token: 0x06006941 RID: 26945
	[DllImport("LightFX")]
	public static extern uint LFX_Reset();

	// Token: 0x06006942 RID: 26946
	[DllImport("LightFX")]
	public static extern uint LFX_Release();

	// Token: 0x06006943 RID: 26947
	[DllImport("LightFX", CallingConvention = CallingConvention.StdCall)]
	public static extern uint LFX_SetLightColor(uint p1, uint p2, ref AlienFXInterface._LFX_COLOR c);

	// Token: 0x06006944 RID: 26948
	[DllImport("LightFX")]
	public static extern uint LFX_GetNumDevices(ref uint numDevices);

	// Token: 0x06006945 RID: 26949
	[DllImport("LightFX")]
	public static extern uint LFX_GetNumLights(uint devIndex, ref uint numLights);

	// Token: 0x02001256 RID: 4694
	public struct _LFX_COLOR
	{
		// Token: 0x06006946 RID: 26950 RVA: 0x00292F1C File Offset: 0x0029111C
		public _LFX_COLOR(Color32 combinedColor)
		{
			this.red = combinedColor.r;
			this.green = combinedColor.g;
			this.blue = combinedColor.b;
			this.brightness = combinedColor.a;
		}

		// Token: 0x0400659C RID: 26012
		public byte red;

		// Token: 0x0400659D RID: 26013
		public byte green;

		// Token: 0x0400659E RID: 26014
		public byte blue;

		// Token: 0x0400659F RID: 26015
		public byte brightness;
	}
}
