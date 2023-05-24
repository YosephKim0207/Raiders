using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000700 RID: 1792
	public class PDPTitanfall2XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x0600292B RID: 10539 RVA: 0x000AF118 File Offset: 0x000AD318
		public PDPTitanfall2XboxOneControllerMacProfile()
		{
			base.Name = "PDP Titanfall 2 Xbox One Controller";
			base.Meta = "PDP Titanfall 2 Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(357)
				}
			};
		}
	}
}
