using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006EE RID: 1774
	public class MadCatzSF4FightStickRound2TEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002919 RID: 10521 RVA: 0x000AE68C File Offset: 0x000AC88C
		public MadCatzSF4FightStickRound2TEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick Round 2 TE";
			base.Meta = "Mad Catz SF4 Fight Stick Round 2 TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61496)
				}
			};
		}
	}
}
