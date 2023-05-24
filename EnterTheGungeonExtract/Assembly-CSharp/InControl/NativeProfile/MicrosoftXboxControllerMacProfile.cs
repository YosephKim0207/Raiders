using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F6 RID: 1782
	public class MicrosoftXboxControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002921 RID: 10529 RVA: 0x000AEA60 File Offset: 0x000ACC60
		public MicrosoftXboxControllerMacProfile()
		{
			base.Name = "Microsoft Xbox Controller";
			base.Meta = "Microsoft Xbox Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(ushort.MaxValue),
					ProductID = new ushort?(ushort.MaxValue)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(649)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(648)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(645)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(514)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(647)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(648)
				}
			};
		}
	}
}
