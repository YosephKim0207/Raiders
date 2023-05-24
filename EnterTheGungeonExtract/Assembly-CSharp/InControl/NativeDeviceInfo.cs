using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x02000752 RID: 1874
	public struct NativeDeviceInfo
	{
		// Token: 0x06002984 RID: 10628 RVA: 0x000BCA48 File Offset: 0x000BAC48
		public bool HasSameVendorID(NativeDeviceInfo deviceInfo)
		{
			return this.vendorID == deviceInfo.vendorID;
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000BCA5C File Offset: 0x000BAC5C
		public bool HasSameProductID(NativeDeviceInfo deviceInfo)
		{
			return this.productID == deviceInfo.productID;
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000BCA70 File Offset: 0x000BAC70
		public bool HasSameVersionNumber(NativeDeviceInfo deviceInfo)
		{
			return this.versionNumber == deviceInfo.versionNumber;
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000BCA84 File Offset: 0x000BAC84
		public bool HasSameLocation(NativeDeviceInfo deviceInfo)
		{
			return !string.IsNullOrEmpty(this.location) && this.location == deviceInfo.location;
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x000BCAAC File Offset: 0x000BACAC
		public bool HasSameSerialNumber(NativeDeviceInfo deviceInfo)
		{
			return !string.IsNullOrEmpty(this.serialNumber) && this.serialNumber == deviceInfo.serialNumber;
		}

		// Token: 0x04001C71 RID: 7281
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string name;

		// Token: 0x04001C72 RID: 7282
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string location;

		// Token: 0x04001C73 RID: 7283
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string serialNumber;

		// Token: 0x04001C74 RID: 7284
		public ushort vendorID;

		// Token: 0x04001C75 RID: 7285
		public ushort productID;

		// Token: 0x04001C76 RID: 7286
		public uint versionNumber;

		// Token: 0x04001C77 RID: 7287
		public NativeDeviceDriverType driverType;

		// Token: 0x04001C78 RID: 7288
		public NativeDeviceTransportType transportType;

		// Token: 0x04001C79 RID: 7289
		public uint numButtons;

		// Token: 0x04001C7A RID: 7290
		public uint numAnalogs;
	}
}
