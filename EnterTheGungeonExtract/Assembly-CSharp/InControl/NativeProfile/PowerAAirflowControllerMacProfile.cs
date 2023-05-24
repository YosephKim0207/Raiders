using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000706 RID: 1798
	public class PowerAAirflowControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06002931 RID: 10545 RVA: 0x000AF400 File Offset: 0x000AD600
		public PowerAAirflowControllerMacProfile()
		{
			base.Name = "PowerA Airflow Controller";
			base.Meta = "PowerA Airflow Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16138)
				}
			};
		}
	}
}
