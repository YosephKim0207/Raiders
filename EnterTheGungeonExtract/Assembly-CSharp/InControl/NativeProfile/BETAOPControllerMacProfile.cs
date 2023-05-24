using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006BD RID: 1725
	public class BETAOPControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028E8 RID: 10472 RVA: 0x000AD22C File Offset: 0x000AB42C
		public BETAOPControllerMacProfile()
		{
			base.Name = "BETAOP Controller";
			base.Meta = "BETAOP Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4544),
					ProductID = new ushort?(21766)
				}
			};
		}
	}
}
