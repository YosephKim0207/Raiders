using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C2 RID: 1730
	public class HoriBlueSoloControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028ED RID: 10477 RVA: 0x000AD48C File Offset: 0x000AB68C
		public HoriBlueSoloControllerMacProfile()
		{
			base.Name = "Hori Blue Solo Controller ";
			base.Meta = "Hori Blue Solo Controller\ton Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64001)
				}
			};
		}
	}
}
