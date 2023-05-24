using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200070E RID: 1806
	public class QanbaFightStickPlusMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002939 RID: 10553 RVA: 0x000AF754 File Offset: 0x000AD954
		public QanbaFightStickPlusMacProfile()
		{
			base.Name = "Qanba Fight Stick Plus";
			base.Meta = "Qanba Fight Stick Plus on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(48879)
				}
			};
		}
	}
}
