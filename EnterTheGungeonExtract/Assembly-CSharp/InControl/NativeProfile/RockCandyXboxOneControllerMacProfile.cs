using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200071A RID: 1818
	public class RockCandyXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06002945 RID: 10565 RVA: 0x000AFCD0 File Offset: 0x000ADED0
		public RockCandyXboxOneControllerMacProfile()
		{
			base.Name = "Rock Candy Xbox One Controller";
			base.Meta = "Rock Candy Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(326)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(582)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(838)
				}
			};
		}
	}
}
