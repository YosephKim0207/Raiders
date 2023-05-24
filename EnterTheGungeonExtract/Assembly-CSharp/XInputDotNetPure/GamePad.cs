using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	// Token: 0x02000823 RID: 2083
	public class GamePad
	{
		// Token: 0x06002C4B RID: 11339 RVA: 0x000DFF14 File Offset: 0x000DE114
		public static GamePadState GetState(PlayerIndex playerIndex)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GamePadState.RawState)));
			uint num = Imports.XInputGamePadGetState((uint)playerIndex, intPtr);
			GamePadState.RawState rawState = (GamePadState.RawState)Marshal.PtrToStructure(intPtr, typeof(GamePadState.RawState));
			return new GamePadState(num == 0U, rawState);
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x000DFF60 File Offset: 0x000DE160
		public static void SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
		{
			Imports.XInputGamePadSetState((uint)playerIndex, leftMotor, rightMotor);
		}
	}
}
