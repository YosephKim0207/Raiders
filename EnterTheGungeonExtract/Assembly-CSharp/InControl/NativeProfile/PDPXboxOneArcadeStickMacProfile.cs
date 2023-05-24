using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000704 RID: 1796
	public class PDPXboxOneArcadeStickMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x0600292F RID: 10543 RVA: 0x000AF298 File Offset: 0x000AD498
		public PDPXboxOneArcadeStickMacProfile()
		{
			base.Name = "PDP Xbox One Arcade Stick";
			base.Meta = "PDP Xbox One Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(348)
				}
			};
		}
	}
}
