using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000719 RID: 1817
	public class RockCandyXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002944 RID: 10564 RVA: 0x000AFC44 File Offset: 0x000ADE44
		public RockCandyXbox360ControllerMacProfile()
		{
			base.Name = "Rock Candy Xbox 360 Controller";
			base.Meta = "Rock Candy Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(543)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64254)
				}
			};
		}
	}
}
