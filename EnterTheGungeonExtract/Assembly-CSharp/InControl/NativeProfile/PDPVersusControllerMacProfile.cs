using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000702 RID: 1794
	public class PDPVersusControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600292D RID: 10541 RVA: 0x000AF1D8 File Offset: 0x000AD3D8
		public PDPVersusControllerMacProfile()
		{
			base.Name = "PDP Versus Controller";
			base.Meta = "PDP Versus Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63748)
				}
			};
		}
	}
}
