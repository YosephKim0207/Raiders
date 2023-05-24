using System;

namespace InControl
{
	// Token: 0x02000808 RID: 2056
	public abstract class UnityInputDeviceProfileBase : InputDeviceProfile
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002B9C RID: 11164
		public abstract bool IsJoystick { get; }

		// Token: 0x06002B9D RID: 11165
		public abstract bool HasJoystickName(string joystickName);

		// Token: 0x06002B9E RID: 11166
		public abstract bool HasLastResortRegex(string joystickName);

		// Token: 0x06002B9F RID: 11167
		public abstract bool HasJoystickOrRegexName(string joystickName);

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x000DD0D0 File Offset: 0x000DB2D0
		public bool IsNotJoystick
		{
			get
			{
				return !this.IsJoystick;
			}
		}
	}
}
