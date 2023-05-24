using System;

namespace InControl
{
	// Token: 0x02000758 RID: 1880
	public abstract class NativeInputDeviceProfile : InputDeviceProfile
	{
		// Token: 0x060029B5 RID: 10677 RVA: 0x000BDB14 File Offset: 0x000BBD14
		public NativeInputDeviceProfile()
		{
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0.2f;
			base.UpperDeadZone = 0.9f;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000BDB40 File Offset: 0x000BBD40
		internal bool Matches(NativeDeviceInfo deviceInfo)
		{
			return this.Matches(deviceInfo, this.Matchers);
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000BDB50 File Offset: 0x000BBD50
		internal bool LastResortMatches(NativeDeviceInfo deviceInfo)
		{
			return this.Matches(deviceInfo, this.LastResortMatchers);
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000BDB60 File Offset: 0x000BBD60
		private bool Matches(NativeDeviceInfo deviceInfo, NativeInputDeviceMatcher[] matchers)
		{
			if (this.Matchers != null)
			{
				int num = this.Matchers.Length;
				for (int i = 0; i < num; i++)
				{
					if (this.Matchers[i].Matches(deviceInfo))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000BDBAC File Offset: 0x000BBDAC
		protected static InputControlSource Button(int index)
		{
			return new NativeButtonSource(index);
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x000BDBB4 File Offset: 0x000BBDB4
		protected static InputControlSource Analog(int index)
		{
			return new NativeAnalogSource(index);
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000BDBBC File Offset: 0x000BBDBC
		protected static InputControlMapping LeftStickLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000BDC04 File Offset: 0x000BBE04
		protected static InputControlMapping LeftStickRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x000BDC4C File Offset: 0x000BBE4C
		protected static InputControlMapping LeftStickUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000BDC94 File Offset: 0x000BBE94
		protected static InputControlMapping LeftStickDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000BDCDC File Offset: 0x000BBEDC
		protected static InputControlMapping LeftStickUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000BDD24 File Offset: 0x000BBF24
		protected static InputControlMapping LeftStickDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000BDD6C File Offset: 0x000BBF6C
		protected static InputControlMapping RightStickLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000BDDB4 File Offset: 0x000BBFB4
		protected static InputControlMapping RightStickRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x000BDE00 File Offset: 0x000BC000
		protected static InputControlMapping RightStickUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000BDE48 File Offset: 0x000BC048
		protected static InputControlMapping RightStickDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000BDE90 File Offset: 0x000BC090
		protected static InputControlMapping RightStickUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x000BDED8 File Offset: 0x000BC0D8
		protected static InputControlMapping RightStickDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000BDF20 File Offset: 0x000BC120
		protected static InputControlMapping LeftTriggerMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Trigger",
				Target = InputControlType.LeftTrigger,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x000BDF70 File Offset: 0x000BC170
		protected static InputControlMapping RightTriggerMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Trigger",
				Target = InputControlType.RightTrigger,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x000BDFC0 File Offset: 0x000BC1C0
		protected static InputControlMapping DPadLeftMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x000BE00C File Offset: 0x000BC20C
		protected static InputControlMapping DPadRightMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x000BE058 File Offset: 0x000BC258
		protected static InputControlMapping DPadUpMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000BE0A4 File Offset: 0x000BC2A4
		protected static InputControlMapping DPadDownMapping(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000BE0F0 File Offset: 0x000BC2F0
		protected static InputControlMapping DPadUpMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000BE13C File Offset: 0x000BC33C
		protected static InputControlMapping DPadDownMapping2(int analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = NativeInputDeviceProfile.Analog(analog),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x04001C97 RID: 7319
		public NativeInputDeviceMatcher[] Matchers;

		// Token: 0x04001C98 RID: 7320
		public NativeInputDeviceMatcher[] LastResortMatchers;
	}
}
