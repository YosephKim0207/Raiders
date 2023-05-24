using System;
using InControl;

// Token: 0x0200180C RID: 6156
public static class UIControllerButtonHelper
{
	// Token: 0x06009126 RID: 37158 RVA: 0x003D5554 File Offset: 0x003D3754
	public static string GetUnifiedControllerButtonTag(InputControlType controlType, GameOptions.ControllerSymbology symbology, GungeonActions gungeonActions = null)
	{
		string controllerButtonSpriteName = UIControllerButtonHelper.GetControllerButtonSpriteName(controlType, symbology, null);
		if (string.IsNullOrEmpty(controllerButtonSpriteName))
		{
			return controlType.ToString();
		}
		return "[sprite \"" + controllerButtonSpriteName + "\"]";
	}

	// Token: 0x06009127 RID: 37159 RVA: 0x003D5598 File Offset: 0x003D3798
	public static string GetUnifiedControllerButtonTag(string weirdControl, GameOptions.ControllerSymbology symbology)
	{
		string text = string.Empty;
		if (weirdControl != null)
		{
			if (!(weirdControl == "RightStick"))
			{
				if (weirdControl == "Teleporter")
				{
					text = "teleport_active_001";
				}
			}
			else if (symbology == GameOptions.ControllerSymbology.PS4)
			{
				text = "ps4_RS";
			}
			else if (symbology == GameOptions.ControllerSymbology.Switch)
			{
				text = "xbone_RS";
			}
			else
			{
				text = "xbone_RS";
			}
		}
		return "[sprite \"" + text + "\"]";
	}

	// Token: 0x06009128 RID: 37160 RVA: 0x003D5628 File Offset: 0x003D3828
	public static string GetControllerButtonSpriteName(InputControlType controlType, GameOptions.ControllerSymbology symbology, GungeonActions gungeonActions = null)
	{
		if (symbology == GameOptions.ControllerSymbology.PS4)
		{
			switch (controlType)
			{
			case InputControlType.LeftStickUp:
				return "ps4_LS_up";
			case InputControlType.LeftStickDown:
				return "ps4_LS_down";
			case InputControlType.LeftStickLeft:
				return "ps4_LS_left";
			case InputControlType.LeftStickRight:
				return "ps4_LS_right";
			case InputControlType.LeftStickButton:
				return "ps4_L3";
			case InputControlType.RightStickUp:
				return "ps4_RS_up";
			case InputControlType.RightStickDown:
				return "ps4_RS_down";
			case InputControlType.RightStickLeft:
				return "ps4_RS_left";
			case InputControlType.RightStickRight:
				return "ps4_RS_right";
			case InputControlType.RightStickButton:
				return "ps4_R3";
			case InputControlType.DPadUp:
				return "ps4_dpad_up";
			case InputControlType.DPadDown:
				return "ps4_dpad_down";
			case InputControlType.DPadLeft:
				return "ps4_dpad_left";
			case InputControlType.DPadRight:
				return "ps4_dpad_right";
			case InputControlType.LeftTrigger:
				return "ps4_L2";
			case InputControlType.RightTrigger:
				return "ps4_R2";
			case InputControlType.LeftBumper:
				return "ps4_L1";
			case InputControlType.RightBumper:
				return "ps4_R1";
			case InputControlType.Action1:
				return "ps4_cross";
			case InputControlType.Action2:
				return "ps4_circle";
			case InputControlType.Action3:
				return "ps4_square";
			case InputControlType.Action4:
				return "ps4_triangle";
			default:
				switch (controlType)
				{
				case InputControlType.Start:
					return "ps4_options_share";
				case InputControlType.Select:
					return "ps4_options_share";
				default:
					if (controlType != InputControlType.TouchPadButton)
					{
						return string.Empty;
					}
					return "ps4_flat";
				case InputControlType.Options:
					return "ps4_options_share";
				case InputControlType.Pause:
					return "ps4_options_share";
				}
				break;
			}
		}
		else if (symbology == GameOptions.ControllerSymbology.Xbox)
		{
			switch (controlType)
			{
			case InputControlType.LeftStickUp:
			case InputControlType.LeftStickDown:
			case InputControlType.LeftStickLeft:
			case InputControlType.LeftStickRight:
				return "xbone_LS";
			case InputControlType.LeftStickButton:
				return "xbone_L3";
			case InputControlType.RightStickUp:
			case InputControlType.RightStickDown:
			case InputControlType.RightStickLeft:
			case InputControlType.RightStickRight:
				return "xbone_RS";
			case InputControlType.RightStickButton:
				return "xbone_R3";
			case InputControlType.DPadUp:
				return "xbone_dpad_up";
			case InputControlType.DPadDown:
				return "xbone_dpad_down";
			case InputControlType.DPadLeft:
				return "xbone_dpad_left";
			case InputControlType.DPadRight:
				return "xbone_dpad_right";
			case InputControlType.LeftTrigger:
				return "xbone_LT";
			case InputControlType.RightTrigger:
				return "xbone_RT";
			case InputControlType.LeftBumper:
				return "xbone_LB";
			case InputControlType.RightBumper:
				return "xbone_RB";
			case InputControlType.Action1:
				return "xbone_a";
			case InputControlType.Action2:
				return "xbone_b";
			case InputControlType.Action3:
				return "xbone_x";
			case InputControlType.Action4:
				return "xbone_y";
			default:
				switch (controlType)
				{
				case InputControlType.Back:
					return "xbone_select";
				case InputControlType.Start:
					return "xbone_start";
				case InputControlType.Select:
					return "xbone_select";
				case InputControlType.Options:
					return "xbone_start";
				case InputControlType.Pause:
					return "xbone_start";
				}
				return string.Empty;
			}
		}
		else
		{
			if (symbology != GameOptions.ControllerSymbology.Switch)
			{
				return string.Empty;
			}
			switch (controlType)
			{
			case InputControlType.LeftStickUp:
				return "ps4_LS";
			case InputControlType.LeftStickDown:
				return "ps4_LS";
			case InputControlType.LeftStickLeft:
				return "ps4_LS";
			case InputControlType.LeftStickRight:
				return "ps4_LS";
			case InputControlType.LeftStickButton:
				return "switch_l3";
			case InputControlType.RightStickUp:
				return "ps4_RS";
			case InputControlType.RightStickDown:
				return "ps4_RS";
			case InputControlType.RightStickLeft:
				return "ps4_RS";
			case InputControlType.RightStickRight:
				return "ps4_RS";
			case InputControlType.RightStickButton:
				return "switch_r3";
			case InputControlType.DPadUp:
				return "switch_dpad_u";
			case InputControlType.DPadDown:
				return "switch_dpad_d";
			case InputControlType.DPadLeft:
				return "switch_dpad_l";
			case InputControlType.DPadRight:
				return "switch_dpad_r";
			case InputControlType.LeftTrigger:
				return "switch_zl";
			case InputControlType.RightTrigger:
				return "switch_zr";
			case InputControlType.LeftBumper:
				return "switch_l";
			case InputControlType.RightBumper:
				return "switch_r";
			case InputControlType.Action1:
				return "switch_b";
			case InputControlType.Action2:
				return "switch_a";
			case InputControlType.Action3:
				return "switch_y";
			case InputControlType.Action4:
				return "switch_x";
			default:
				switch (controlType)
				{
				case InputControlType.Start:
					return "switch_plus";
				case InputControlType.Select:
					return "switch_minus";
				case InputControlType.Options:
					return "switch_plus";
				case InputControlType.Pause:
					return "switch_minus";
				}
				return string.Empty;
			}
		}
	}
}
