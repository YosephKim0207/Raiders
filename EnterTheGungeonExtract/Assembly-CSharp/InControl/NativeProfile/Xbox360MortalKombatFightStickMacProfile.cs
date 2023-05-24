using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000721 RID: 1825
	public class Xbox360MortalKombatFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600294C RID: 10572 RVA: 0x000B0018 File Offset: 0x000AE218
		public Xbox360MortalKombatFightStickMacProfile()
		{
			base.Name = "Xbox 360 Mortal Kombat Fight Stick";
			base.Meta = "Xbox 360 Mortal Kombat Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63750)
				}
			};
		}
	}
}
