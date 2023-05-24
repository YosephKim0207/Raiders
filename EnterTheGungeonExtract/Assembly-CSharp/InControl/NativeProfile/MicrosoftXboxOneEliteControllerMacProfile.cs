using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006F8 RID: 1784
	public class MicrosoftXboxOneEliteControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06002923 RID: 10531 RVA: 0x000AEC70 File Offset: 0x000ACE70
		public MicrosoftXboxOneEliteControllerMacProfile()
		{
			base.Name = "Microsoft Xbox One Elite Controller";
			base.Meta = "Microsoft Xbox One Elite Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(739)
				}
			};
		}
	}
}
