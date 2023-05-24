using System;

namespace InControl
{
	// Token: 0x02000782 RID: 1922
	[AutoDiscover]
	public class AndroidTVRemoteProfile : UnityInputDeviceProfile
	{
		// Token: 0x06002AD8 RID: 10968 RVA: 0x000C31E8 File Offset: 0x000C13E8
		public AndroidTVRemoteProfile()
		{
			base.Name = "Android TV Remote";
			base.Meta = "Android TV Remote on Android TV";
			base.DeviceClass = InputDeviceClass.Remote;
			base.IncludePlatforms = new string[] { "Android" };
			this.JoystickNames = new string[]
			{
				string.Empty,
				"touch-input",
				"navigation-input"
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
