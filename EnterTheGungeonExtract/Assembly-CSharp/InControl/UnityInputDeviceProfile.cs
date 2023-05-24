using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000807 RID: 2055
	public class UnityInputDeviceProfile : UnityInputDeviceProfileBase
	{
		// Token: 0x06002B7D RID: 11133 RVA: 0x000DC850 File Offset: 0x000DAA50
		public UnityInputDeviceProfile()
		{
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0.2f;
			base.UpperDeadZone = 0.9f;
			this.MinUnityVersion = VersionInfo.Min;
			this.MaxUnityVersion = VersionInfo.Max;
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000DC890 File Offset: 0x000DAA90
		// (set) Token: 0x06002B7F RID: 11135 RVA: 0x000DC898 File Offset: 0x000DAA98
		[SerializeField]
		public VersionInfo MinUnityVersion { get; protected set; }

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000DC8A4 File Offset: 0x000DAAA4
		// (set) Token: 0x06002B81 RID: 11137 RVA: 0x000DC8AC File Offset: 0x000DAAAC
		[SerializeField]
		public VersionInfo MaxUnityVersion { get; protected set; }

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000DC8B8 File Offset: 0x000DAAB8
		public override bool IsJoystick
		{
			get
			{
				return this.LastResortRegex != null || (this.JoystickNames != null && this.JoystickNames.Length > 0) || (this.JoystickRegex != null && this.JoystickRegex.Length > 0);
			}
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x000DC908 File Offset: 0x000DAB08
		public override bool HasJoystickName(string joystickName)
		{
			if (base.IsNotJoystick)
			{
				return false;
			}
			if (this.JoystickNames != null && this.JoystickNames.Contains(joystickName, StringComparer.OrdinalIgnoreCase))
			{
				return true;
			}
			if (this.JoystickRegex != null)
			{
				for (int i = 0; i < this.JoystickRegex.Length; i++)
				{
					if (Regex.IsMatch(joystickName, this.JoystickRegex[i], RegexOptions.IgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000DC980 File Offset: 0x000DAB80
		public override bool HasLastResortRegex(string joystickName)
		{
			return !base.IsNotJoystick && this.LastResortRegex != null && Regex.IsMatch(joystickName, this.LastResortRegex, RegexOptions.IgnoreCase);
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000DC9AC File Offset: 0x000DABAC
		public override bool HasJoystickOrRegexName(string joystickName)
		{
			return this.HasJoystickName(joystickName) || this.HasLastResortRegex(joystickName);
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x000DC9C4 File Offset: 0x000DABC4
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.IsSupportedOnThisVersionOfUnity && base.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002B87 RID: 11143 RVA: 0x000DC9DC File Offset: 0x000DABDC
		private bool IsSupportedOnThisVersionOfUnity
		{
			get
			{
				VersionInfo versionInfo = VersionInfo.UnityVersion();
				return versionInfo >= this.MinUnityVersion && versionInfo <= this.MaxUnityVersion;
			}
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000DCA10 File Offset: 0x000DAC10
		protected static InputControlSource Button(int index)
		{
			return new UnityButtonSource(index);
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000DCA18 File Offset: 0x000DAC18
		protected static InputControlSource Analog(int index)
		{
			return new UnityAnalogSource(index);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000DCA20 File Offset: 0x000DAC20
		protected static InputControlMapping LeftStickLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000DCA64 File Offset: 0x000DAC64
		protected static InputControlMapping LeftStickRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000DCAA8 File Offset: 0x000DACA8
		protected static InputControlMapping LeftStickUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000DCAEC File Offset: 0x000DACEC
		protected static InputControlMapping LeftStickDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000DCB30 File Offset: 0x000DAD30
		protected static InputControlMapping RightStickLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000DCB74 File Offset: 0x000DAD74
		protected static InputControlMapping RightStickRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000DCBB8 File Offset: 0x000DADB8
		protected static InputControlMapping RightStickUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000DCBFC File Offset: 0x000DADFC
		protected static InputControlMapping RightStickDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000DCC40 File Offset: 0x000DAE40
		protected static InputControlMapping LeftTriggerMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Trigger",
				Target = InputControlType.LeftTrigger,
				Source = analog,
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000DCC8C File Offset: 0x000DAE8C
		protected static InputControlMapping RightTriggerMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Trigger",
				Target = InputControlType.RightTrigger,
				Source = analog,
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000DCCD8 File Offset: 0x000DAED8
		protected static InputControlMapping DPadLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000DCD1C File Offset: 0x000DAF1C
		protected static InputControlMapping DPadRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000DCD60 File Offset: 0x000DAF60
		protected static InputControlMapping DPadUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000DCDA4 File Offset: 0x000DAFA4
		protected static InputControlMapping DPadDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000DCDE8 File Offset: 0x000DAFE8
		protected static InputControlMapping DPadUpMapping2(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000DCE2C File Offset: 0x000DB02C
		protected static InputControlMapping DPadDownMapping2(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x04001DA6 RID: 7590
		[SerializeField]
		protected string[] JoystickNames;

		// Token: 0x04001DA7 RID: 7591
		[SerializeField]
		protected string[] JoystickRegex;

		// Token: 0x04001DA8 RID: 7592
		[SerializeField]
		protected string LastResortRegex;

		// Token: 0x04001DAB RID: 7595
		protected static InputControlSource Button0 = UnityInputDeviceProfile.Button(0);

		// Token: 0x04001DAC RID: 7596
		protected static InputControlSource Button1 = UnityInputDeviceProfile.Button(1);

		// Token: 0x04001DAD RID: 7597
		protected static InputControlSource Button2 = UnityInputDeviceProfile.Button(2);

		// Token: 0x04001DAE RID: 7598
		protected static InputControlSource Button3 = UnityInputDeviceProfile.Button(3);

		// Token: 0x04001DAF RID: 7599
		protected static InputControlSource Button4 = UnityInputDeviceProfile.Button(4);

		// Token: 0x04001DB0 RID: 7600
		protected static InputControlSource Button5 = UnityInputDeviceProfile.Button(5);

		// Token: 0x04001DB1 RID: 7601
		protected static InputControlSource Button6 = UnityInputDeviceProfile.Button(6);

		// Token: 0x04001DB2 RID: 7602
		protected static InputControlSource Button7 = UnityInputDeviceProfile.Button(7);

		// Token: 0x04001DB3 RID: 7603
		protected static InputControlSource Button8 = UnityInputDeviceProfile.Button(8);

		// Token: 0x04001DB4 RID: 7604
		protected static InputControlSource Button9 = UnityInputDeviceProfile.Button(9);

		// Token: 0x04001DB5 RID: 7605
		protected static InputControlSource Button10 = UnityInputDeviceProfile.Button(10);

		// Token: 0x04001DB6 RID: 7606
		protected static InputControlSource Button11 = UnityInputDeviceProfile.Button(11);

		// Token: 0x04001DB7 RID: 7607
		protected static InputControlSource Button12 = UnityInputDeviceProfile.Button(12);

		// Token: 0x04001DB8 RID: 7608
		protected static InputControlSource Button13 = UnityInputDeviceProfile.Button(13);

		// Token: 0x04001DB9 RID: 7609
		protected static InputControlSource Button14 = UnityInputDeviceProfile.Button(14);

		// Token: 0x04001DBA RID: 7610
		protected static InputControlSource Button15 = UnityInputDeviceProfile.Button(15);

		// Token: 0x04001DBB RID: 7611
		protected static InputControlSource Button16 = UnityInputDeviceProfile.Button(16);

		// Token: 0x04001DBC RID: 7612
		protected static InputControlSource Button17 = UnityInputDeviceProfile.Button(17);

		// Token: 0x04001DBD RID: 7613
		protected static InputControlSource Button18 = UnityInputDeviceProfile.Button(18);

		// Token: 0x04001DBE RID: 7614
		protected static InputControlSource Button19 = UnityInputDeviceProfile.Button(19);

		// Token: 0x04001DBF RID: 7615
		protected static InputControlSource Analog0 = UnityInputDeviceProfile.Analog(0);

		// Token: 0x04001DC0 RID: 7616
		protected static InputControlSource Analog1 = UnityInputDeviceProfile.Analog(1);

		// Token: 0x04001DC1 RID: 7617
		protected static InputControlSource Analog2 = UnityInputDeviceProfile.Analog(2);

		// Token: 0x04001DC2 RID: 7618
		protected static InputControlSource Analog3 = UnityInputDeviceProfile.Analog(3);

		// Token: 0x04001DC3 RID: 7619
		protected static InputControlSource Analog4 = UnityInputDeviceProfile.Analog(4);

		// Token: 0x04001DC4 RID: 7620
		protected static InputControlSource Analog5 = UnityInputDeviceProfile.Analog(5);

		// Token: 0x04001DC5 RID: 7621
		protected static InputControlSource Analog6 = UnityInputDeviceProfile.Analog(6);

		// Token: 0x04001DC6 RID: 7622
		protected static InputControlSource Analog7 = UnityInputDeviceProfile.Analog(7);

		// Token: 0x04001DC7 RID: 7623
		protected static InputControlSource Analog8 = UnityInputDeviceProfile.Analog(8);

		// Token: 0x04001DC8 RID: 7624
		protected static InputControlSource Analog9 = UnityInputDeviceProfile.Analog(9);

		// Token: 0x04001DC9 RID: 7625
		protected static InputControlSource Analog10 = UnityInputDeviceProfile.Analog(10);

		// Token: 0x04001DCA RID: 7626
		protected static InputControlSource Analog11 = UnityInputDeviceProfile.Analog(11);

		// Token: 0x04001DCB RID: 7627
		protected static InputControlSource Analog12 = UnityInputDeviceProfile.Analog(12);

		// Token: 0x04001DCC RID: 7628
		protected static InputControlSource Analog13 = UnityInputDeviceProfile.Analog(13);

		// Token: 0x04001DCD RID: 7629
		protected static InputControlSource Analog14 = UnityInputDeviceProfile.Analog(14);

		// Token: 0x04001DCE RID: 7630
		protected static InputControlSource Analog15 = UnityInputDeviceProfile.Analog(15);

		// Token: 0x04001DCF RID: 7631
		protected static InputControlSource Analog16 = UnityInputDeviceProfile.Analog(16);

		// Token: 0x04001DD0 RID: 7632
		protected static InputControlSource Analog17 = UnityInputDeviceProfile.Analog(17);

		// Token: 0x04001DD1 RID: 7633
		protected static InputControlSource Analog18 = UnityInputDeviceProfile.Analog(18);

		// Token: 0x04001DD2 RID: 7634
		protected static InputControlSource Analog19 = UnityInputDeviceProfile.Analog(19);

		// Token: 0x04001DD3 RID: 7635
		protected static InputControlSource MenuKey = new UnityKeyCodeSource(new KeyCode[] { KeyCode.Menu });

		// Token: 0x04001DD4 RID: 7636
		protected static InputControlSource EscapeKey = new UnityKeyCodeSource(new KeyCode[] { KeyCode.Escape });

		// Token: 0x04001DD5 RID: 7637
		protected static InputControlSource MouseButton0 = new UnityMouseButtonSource(0);

		// Token: 0x04001DD6 RID: 7638
		protected static InputControlSource MouseButton1 = new UnityMouseButtonSource(1);

		// Token: 0x04001DD7 RID: 7639
		protected static InputControlSource MouseButton2 = new UnityMouseButtonSource(2);

		// Token: 0x04001DD8 RID: 7640
		protected static InputControlSource MouseXAxis = new UnityMouseAxisSource("x");

		// Token: 0x04001DD9 RID: 7641
		protected static InputControlSource MouseYAxis = new UnityMouseAxisSource("y");

		// Token: 0x04001DDA RID: 7642
		protected static InputControlSource MouseScrollWheel = new UnityMouseAxisSource("z");
	}
}
