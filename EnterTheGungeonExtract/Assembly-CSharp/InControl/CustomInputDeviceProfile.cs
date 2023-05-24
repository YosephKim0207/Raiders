using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200077B RID: 1915
	[Obsolete("Custom profiles are deprecated. Use the bindings API instead.", false)]
	public class CustomInputDeviceProfile : UnityInputDeviceProfileBase
	{
		// Token: 0x06002ACA RID: 10954 RVA: 0x000C20B0 File Offset: 0x000C02B0
		public CustomInputDeviceProfile()
		{
			base.Name = "Custom Device Profile";
			base.Meta = "Custom Device Profile";
			base.IncludePlatforms = new string[] { "Windows", "Mac", "Linux" };
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0f;
			base.UpperDeadZone = 1f;
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000C2120 File Offset: 0x000C0320
		public sealed override bool IsJoystick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000C2124 File Offset: 0x000C0324
		public sealed override bool HasJoystickName(string joystickName)
		{
			return false;
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000C2128 File Offset: 0x000C0328
		public sealed override bool HasLastResortRegex(string joystickName)
		{
			return false;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000C212C File Offset: 0x000C032C
		public sealed override bool HasJoystickOrRegexName(string joystickName)
		{
			return false;
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000C2130 File Offset: 0x000C0330
		protected static InputControlSource KeyCodeButton(params KeyCode[] keyCodeList)
		{
			return new UnityKeyCodeSource(keyCodeList);
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000C2138 File Offset: 0x000C0338
		protected static InputControlSource KeyCodeComboButton(params KeyCode[] keyCodeList)
		{
			return new UnityKeyCodeComboSource(keyCodeList);
		}

		// Token: 0x04001D8E RID: 7566
		protected static InputControlSource MouseButton0 = new UnityMouseButtonSource(0);

		// Token: 0x04001D8F RID: 7567
		protected static InputControlSource MouseButton1 = new UnityMouseButtonSource(1);

		// Token: 0x04001D90 RID: 7568
		protected static InputControlSource MouseButton2 = new UnityMouseButtonSource(2);

		// Token: 0x04001D91 RID: 7569
		protected static InputControlSource MouseXAxis = new UnityMouseAxisSource("x");

		// Token: 0x04001D92 RID: 7570
		protected static InputControlSource MouseYAxis = new UnityMouseAxisSource("y");

		// Token: 0x04001D93 RID: 7571
		protected static InputControlSource MouseScrollWheel = new UnityMouseAxisSource("z");
	}
}
