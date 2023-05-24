using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000707 RID: 1799
	public class POWERAFUS1ONTournamentControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002932 RID: 10546 RVA: 0x000AF460 File Offset: 0x000AD660
		public POWERAFUS1ONTournamentControllerMacProfile()
		{
			base.Name = "POWER A FUS1ON Tournament Controller";
			base.Meta = "POWER A FUS1ON Tournament Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21399)
				}
			};
		}
	}
}
