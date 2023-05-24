using System;

namespace InControl.NativeProfile
{
	// Token: 0x020006C1 RID: 1729
	public class GuitarHeroControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060028EC RID: 10476 RVA: 0x000AD42C File Offset: 0x000AB62C
		public GuitarHeroControllerMacProfile()
		{
			base.Name = "Guitar Hero Controller";
			base.Meta = "Guitar Hero Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5168),
					ProductID = new ushort?(18248)
				}
			};
		}
	}
}
