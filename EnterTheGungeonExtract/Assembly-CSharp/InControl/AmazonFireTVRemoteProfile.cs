using System;

namespace InControl
{
	// Token: 0x0200077F RID: 1919
	[AutoDiscover]
	public class AmazonFireTVRemoteProfile : UnityInputDeviceProfile
	{
		// Token: 0x06002AD5 RID: 10965 RVA: 0x000C2AE8 File Offset: 0x000C0CE8
		public AmazonFireTVRemoteProfile()
		{
			base.Name = "Amazon Fire TV Remote";
			base.Meta = "Amazon Fire TV Remote on Amazon Fire TV";
			base.DeviceClass = InputDeviceClass.Remote;
			base.DeviceStyle = InputDeviceStyle.AmazonFireTV;
			base.IncludePlatforms = new string[] { "Amazon AFT" };
			this.JoystickNames = new string[]
			{
				string.Empty,
				"Amazon Fire TV Remote"
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button0
				},
				new InputControlMapping
				{
					Handle = "Back",
					Target = InputControlType.Back,
					Source = UnityInputDeviceProfile.EscapeKey
				},
				new InputControlMapping
				{
					Handle = "Menu",
					Target = InputControlType.Menu,
					Source = UnityInputDeviceProfile.MenuKey
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				UnityInputDeviceProfile.DPadLeftMapping(UnityInputDeviceProfile.Analog4),
				UnityInputDeviceProfile.DPadRightMapping(UnityInputDeviceProfile.Analog4),
				UnityInputDeviceProfile.DPadUpMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.DPadDownMapping(UnityInputDeviceProfile.Analog5)
			};
		}
	}
}
