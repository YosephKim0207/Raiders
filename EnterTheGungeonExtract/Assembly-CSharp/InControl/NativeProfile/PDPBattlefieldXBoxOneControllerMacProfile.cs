using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006FE RID: 1790
	public class PDPBattlefieldXBoxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06002929 RID: 10537 RVA: 0x000AF058 File Offset: 0x000AD258
		public PDPBattlefieldXBoxOneControllerMacProfile()
		{
			base.Name = "PDP Battlefield XBox One Controller";
			base.Meta = "PDP Battlefield XBox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(356)
				}
			};
		}
	}
}
