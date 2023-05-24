using System;
using System.Runtime.InteropServices;

// Token: 0x020016B2 RID: 5810
public class SCEPadWrapper
{
	// Token: 0x0600875F RID: 34655
	[DllImport("PS4NativePad")]
	public static extern int PadReadState(int handle, out SCEPadWrapper.ScePadData pData);

	// Token: 0x04008C52 RID: 35922
	private const int SCE_PAD_MAX_TOUCH_NUM = 2;

	// Token: 0x04008C53 RID: 35923
	private const int SCE_PAD_MAX_DEVICE_UNIQUE_DATA_SIZE = 12;

	// Token: 0x04008C54 RID: 35924
	public const int SCE_OK = 0;

	// Token: 0x020016B3 RID: 5811
	public enum ScePadButtonDataOffset : uint
	{
		// Token: 0x04008C56 RID: 35926
		SCE_PAD_BUTTON_L3 = 2U,
		// Token: 0x04008C57 RID: 35927
		SCE_PAD_BUTTON_R3 = 4U,
		// Token: 0x04008C58 RID: 35928
		SCE_PAD_BUTTON_OPTIONS = 8U,
		// Token: 0x04008C59 RID: 35929
		SCE_PAD_BUTTON_UP = 16U,
		// Token: 0x04008C5A RID: 35930
		SCE_PAD_BUTTON_RIGHT = 32U,
		// Token: 0x04008C5B RID: 35931
		SCE_PAD_BUTTON_DOWN = 64U,
		// Token: 0x04008C5C RID: 35932
		SCE_PAD_BUTTON_LEFT = 128U,
		// Token: 0x04008C5D RID: 35933
		SCE_PAD_BUTTON_L2 = 256U,
		// Token: 0x04008C5E RID: 35934
		SCE_PAD_BUTTON_R2 = 512U,
		// Token: 0x04008C5F RID: 35935
		SCE_PAD_BUTTON_L1 = 1024U,
		// Token: 0x04008C60 RID: 35936
		SCE_PAD_BUTTON_R1 = 2048U,
		// Token: 0x04008C61 RID: 35937
		SCE_PAD_BUTTON_TRIANGLE = 4096U,
		// Token: 0x04008C62 RID: 35938
		SCE_PAD_BUTTON_CIRCLE = 8192U,
		// Token: 0x04008C63 RID: 35939
		SCE_PAD_BUTTON_CROSS = 16384U,
		// Token: 0x04008C64 RID: 35940
		SCE_PAD_BUTTON_SQUARE = 32768U,
		// Token: 0x04008C65 RID: 35941
		SCE_PAD_BUTTON_TOUCH_PAD = 1048576U,
		// Token: 0x04008C66 RID: 35942
		SCE_PAD_BUTTON_INTERCEPTED = 2147483648U
	}

	// Token: 0x020016B4 RID: 5812
	public struct ScePadAnalogStick
	{
		// Token: 0x04008C67 RID: 35943
		public byte x;

		// Token: 0x04008C68 RID: 35944
		public byte y;
	}

	// Token: 0x020016B5 RID: 5813
	public struct ScePadAnalogButtons
	{
		// Token: 0x04008C69 RID: 35945
		public byte l2;

		// Token: 0x04008C6A RID: 35946
		public byte r2;

		// Token: 0x04008C6B RID: 35947
		private byte pad1;

		// Token: 0x04008C6C RID: 35948
		private byte pad2;
	}

	// Token: 0x020016B6 RID: 5814
	public struct ScePadTouch
	{
		// Token: 0x04008C6D RID: 35949
		private ushort x;

		// Token: 0x04008C6E RID: 35950
		private ushort y;

		// Token: 0x04008C6F RID: 35951
		private byte id;

		// Token: 0x04008C70 RID: 35952
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		private byte[] reserve;
	}

	// Token: 0x020016B7 RID: 5815
	public struct ScePadTouchData
	{
		// Token: 0x04008C71 RID: 35953
		private byte touchNum;

		// Token: 0x04008C72 RID: 35954
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		private byte[] reserve;

		// Token: 0x04008C73 RID: 35955
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		private SCEPadWrapper.ScePadTouch[] touch;
	}

	// Token: 0x020016B8 RID: 5816
	public struct ScePadExtensionUnitData
	{
		// Token: 0x04008C74 RID: 35956
		private uint extensionUnitId;

		// Token: 0x04008C75 RID: 35957
		private byte reserve;

		// Token: 0x04008C76 RID: 35958
		private byte dataLength;

		// Token: 0x04008C77 RID: 35959
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		private byte[] data;
	}

	// Token: 0x020016B9 RID: 5817
	public struct SceFQuaternion
	{
		// Token: 0x04008C78 RID: 35960
		private float x;

		// Token: 0x04008C79 RID: 35961
		private float y;

		// Token: 0x04008C7A RID: 35962
		private float z;

		// Token: 0x04008C7B RID: 35963
		private float w;
	}

	// Token: 0x020016BA RID: 5818
	public struct SceFVector3
	{
		// Token: 0x04008C7C RID: 35964
		private float x;

		// Token: 0x04008C7D RID: 35965
		private float y;

		// Token: 0x04008C7E RID: 35966
		private float z;
	}

	// Token: 0x020016BB RID: 5819
	public struct ScePadData
	{
		// Token: 0x04008C7F RID: 35967
		public uint buttons;

		// Token: 0x04008C80 RID: 35968
		public SCEPadWrapper.ScePadAnalogStick leftStick;

		// Token: 0x04008C81 RID: 35969
		public SCEPadWrapper.ScePadAnalogStick rightStick;

		// Token: 0x04008C82 RID: 35970
		public SCEPadWrapper.ScePadAnalogButtons analogButtons;

		// Token: 0x04008C83 RID: 35971
		public bool connected;
	}
}
