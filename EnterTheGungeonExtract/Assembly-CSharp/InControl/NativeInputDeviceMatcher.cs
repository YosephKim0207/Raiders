using System;
using System.Text.RegularExpressions;

namespace InControl
{
	// Token: 0x02000757 RID: 1879
	public class NativeInputDeviceMatcher
	{
		// Token: 0x060029B4 RID: 10676 RVA: 0x000BD968 File Offset: 0x000BBB68
		internal bool Matches(NativeDeviceInfo deviceInfo)
		{
			bool flag = false;
			if (this.VendorID != null)
			{
				if (this.VendorID.Value != deviceInfo.vendorID)
				{
					return false;
				}
				flag = true;
			}
			if (this.ProductID != null)
			{
				if (this.ProductID.Value != deviceInfo.productID)
				{
					return false;
				}
				flag = true;
			}
			if (this.VersionNumber != null)
			{
				if (this.VersionNumber.Value != deviceInfo.versionNumber)
				{
					return false;
				}
				flag = true;
			}
			if (this.DriverType != null)
			{
				if (this.DriverType.Value != deviceInfo.driverType)
				{
					return false;
				}
				flag = true;
			}
			if (this.TransportType != null)
			{
				if (this.TransportType.Value != deviceInfo.transportType)
				{
					return false;
				}
				flag = true;
			}
			if (this.NameLiterals != null && this.NameLiterals.Length > 0)
			{
				int num = this.NameLiterals.Length;
				for (int i = 0; i < num; i++)
				{
					if (string.Equals(deviceInfo.name, this.NameLiterals[i], StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
				return false;
			}
			if (this.NamePatterns != null && this.NamePatterns.Length > 0)
			{
				int num2 = this.NamePatterns.Length;
				for (int j = 0; j < num2; j++)
				{
					if (Regex.IsMatch(deviceInfo.name, this.NamePatterns[j], RegexOptions.IgnoreCase))
					{
						return true;
					}
				}
				return false;
			}
			return flag;
		}

		// Token: 0x04001C90 RID: 7312
		public ushort? VendorID;

		// Token: 0x04001C91 RID: 7313
		public ushort? ProductID;

		// Token: 0x04001C92 RID: 7314
		public uint? VersionNumber;

		// Token: 0x04001C93 RID: 7315
		public NativeDeviceDriverType? DriverType;

		// Token: 0x04001C94 RID: 7316
		public NativeDeviceTransportType? TransportType;

		// Token: 0x04001C95 RID: 7317
		public string[] NameLiterals;

		// Token: 0x04001C96 RID: 7318
		public string[] NamePatterns;
	}
}
