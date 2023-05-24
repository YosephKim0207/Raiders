using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x02000750 RID: 1872
	internal static class Native
	{
		// Token: 0x0600297B RID: 10619
		[DllImport("InControlNative", EntryPoint = "InControl_Init")]
		public static extern void Init(NativeInputOptions options);

		// Token: 0x0600297C RID: 10620
		[DllImport("InControlNative", EntryPoint = "InControl_Stop")]
		public static extern void Stop();

		// Token: 0x0600297D RID: 10621
		[DllImport("InControlNative", EntryPoint = "InControl_GetVersionInfo")]
		public static extern void GetVersionInfo(out NativeVersionInfo versionInfo);

		// Token: 0x0600297E RID: 10622
		[DllImport("InControlNative", EntryPoint = "InControl_GetDeviceInfo")]
		public static extern bool GetDeviceInfo(uint handle, out NativeDeviceInfo deviceInfo);

		// Token: 0x0600297F RID: 10623
		[DllImport("InControlNative", EntryPoint = "InControl_GetDeviceState")]
		public static extern bool GetDeviceState(uint handle, out IntPtr deviceState);

		// Token: 0x06002980 RID: 10624
		[DllImport("InControlNative", EntryPoint = "InControl_GetDeviceEvents")]
		public static extern int GetDeviceEvents(out IntPtr deviceEvents);

		// Token: 0x06002981 RID: 10625
		[DllImport("InControlNative", EntryPoint = "InControl_SetHapticState")]
		public static extern void SetHapticState(uint handle, byte motor0, byte motor1);

		// Token: 0x06002982 RID: 10626
		[DllImport("InControlNative", EntryPoint = "InControl_SetLightColor")]
		public static extern void SetLightColor(uint handle, byte red, byte green, byte blue);

		// Token: 0x06002983 RID: 10627
		[DllImport("InControlNative", EntryPoint = "InControl_SetLightFlash")]
		public static extern void SetLightFlash(uint handle, byte flashOnDuration, byte flashOffDuration);

		// Token: 0x04001C68 RID: 7272
		private const string LibraryName = "InControlNative";
	}
}
