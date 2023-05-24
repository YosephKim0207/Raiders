using System;

namespace InControl
{
	// Token: 0x020006A6 RID: 1702
	public enum InputControlType
	{
		// Token: 0x04001B02 RID: 6914
		None,
		// Token: 0x04001B03 RID: 6915
		LeftStickUp,
		// Token: 0x04001B04 RID: 6916
		LeftStickDown,
		// Token: 0x04001B05 RID: 6917
		LeftStickLeft,
		// Token: 0x04001B06 RID: 6918
		LeftStickRight,
		// Token: 0x04001B07 RID: 6919
		LeftStickButton,
		// Token: 0x04001B08 RID: 6920
		RightStickUp,
		// Token: 0x04001B09 RID: 6921
		RightStickDown,
		// Token: 0x04001B0A RID: 6922
		RightStickLeft,
		// Token: 0x04001B0B RID: 6923
		RightStickRight,
		// Token: 0x04001B0C RID: 6924
		RightStickButton,
		// Token: 0x04001B0D RID: 6925
		DPadUp,
		// Token: 0x04001B0E RID: 6926
		DPadDown,
		// Token: 0x04001B0F RID: 6927
		DPadLeft,
		// Token: 0x04001B10 RID: 6928
		DPadRight,
		// Token: 0x04001B11 RID: 6929
		LeftTrigger,
		// Token: 0x04001B12 RID: 6930
		RightTrigger,
		// Token: 0x04001B13 RID: 6931
		LeftBumper,
		// Token: 0x04001B14 RID: 6932
		RightBumper,
		// Token: 0x04001B15 RID: 6933
		Action1,
		// Token: 0x04001B16 RID: 6934
		Action2,
		// Token: 0x04001B17 RID: 6935
		Action3,
		// Token: 0x04001B18 RID: 6936
		Action4,
		// Token: 0x04001B19 RID: 6937
		Action5,
		// Token: 0x04001B1A RID: 6938
		Action6,
		// Token: 0x04001B1B RID: 6939
		Action7,
		// Token: 0x04001B1C RID: 6940
		Action8,
		// Token: 0x04001B1D RID: 6941
		Action9,
		// Token: 0x04001B1E RID: 6942
		Action10,
		// Token: 0x04001B1F RID: 6943
		Action11,
		// Token: 0x04001B20 RID: 6944
		Action12,
		// Token: 0x04001B21 RID: 6945
		Back = 100,
		// Token: 0x04001B22 RID: 6946
		Start,
		// Token: 0x04001B23 RID: 6947
		Select,
		// Token: 0x04001B24 RID: 6948
		System,
		// Token: 0x04001B25 RID: 6949
		Options,
		// Token: 0x04001B26 RID: 6950
		Pause,
		// Token: 0x04001B27 RID: 6951
		Menu,
		// Token: 0x04001B28 RID: 6952
		Share,
		// Token: 0x04001B29 RID: 6953
		Home,
		// Token: 0x04001B2A RID: 6954
		View,
		// Token: 0x04001B2B RID: 6955
		Power,
		// Token: 0x04001B2C RID: 6956
		Capture,
		// Token: 0x04001B2D RID: 6957
		Plus,
		// Token: 0x04001B2E RID: 6958
		Minus,
		// Token: 0x04001B2F RID: 6959
		PedalLeft = 150,
		// Token: 0x04001B30 RID: 6960
		PedalRight,
		// Token: 0x04001B31 RID: 6961
		PedalMiddle,
		// Token: 0x04001B32 RID: 6962
		GearUp,
		// Token: 0x04001B33 RID: 6963
		GearDown,
		// Token: 0x04001B34 RID: 6964
		Pitch = 200,
		// Token: 0x04001B35 RID: 6965
		Roll,
		// Token: 0x04001B36 RID: 6966
		Yaw,
		// Token: 0x04001B37 RID: 6967
		ThrottleUp,
		// Token: 0x04001B38 RID: 6968
		ThrottleDown,
		// Token: 0x04001B39 RID: 6969
		ThrottleLeft,
		// Token: 0x04001B3A RID: 6970
		ThrottleRight,
		// Token: 0x04001B3B RID: 6971
		POVUp,
		// Token: 0x04001B3C RID: 6972
		POVDown,
		// Token: 0x04001B3D RID: 6973
		POVLeft,
		// Token: 0x04001B3E RID: 6974
		POVRight,
		// Token: 0x04001B3F RID: 6975
		TiltX = 250,
		// Token: 0x04001B40 RID: 6976
		TiltY,
		// Token: 0x04001B41 RID: 6977
		TiltZ,
		// Token: 0x04001B42 RID: 6978
		ScrollWheel,
		// Token: 0x04001B43 RID: 6979
		[Obsolete("Use InputControlType.TouchPadButton instead.", true)]
		TouchPadTap,
		// Token: 0x04001B44 RID: 6980
		TouchPadButton,
		// Token: 0x04001B45 RID: 6981
		TouchPadXAxis,
		// Token: 0x04001B46 RID: 6982
		TouchPadYAxis,
		// Token: 0x04001B47 RID: 6983
		LeftSL,
		// Token: 0x04001B48 RID: 6984
		LeftSR,
		// Token: 0x04001B49 RID: 6985
		RightSL,
		// Token: 0x04001B4A RID: 6986
		RightSR,
		// Token: 0x04001B4B RID: 6987
		Command = 300,
		// Token: 0x04001B4C RID: 6988
		LeftStickX,
		// Token: 0x04001B4D RID: 6989
		LeftStickY,
		// Token: 0x04001B4E RID: 6990
		RightStickX,
		// Token: 0x04001B4F RID: 6991
		RightStickY,
		// Token: 0x04001B50 RID: 6992
		DPadX,
		// Token: 0x04001B51 RID: 6993
		DPadY,
		// Token: 0x04001B52 RID: 6994
		Analog0 = 400,
		// Token: 0x04001B53 RID: 6995
		Analog1,
		// Token: 0x04001B54 RID: 6996
		Analog2,
		// Token: 0x04001B55 RID: 6997
		Analog3,
		// Token: 0x04001B56 RID: 6998
		Analog4,
		// Token: 0x04001B57 RID: 6999
		Analog5,
		// Token: 0x04001B58 RID: 7000
		Analog6,
		// Token: 0x04001B59 RID: 7001
		Analog7,
		// Token: 0x04001B5A RID: 7002
		Analog8,
		// Token: 0x04001B5B RID: 7003
		Analog9,
		// Token: 0x04001B5C RID: 7004
		Analog10,
		// Token: 0x04001B5D RID: 7005
		Analog11,
		// Token: 0x04001B5E RID: 7006
		Analog12,
		// Token: 0x04001B5F RID: 7007
		Analog13,
		// Token: 0x04001B60 RID: 7008
		Analog14,
		// Token: 0x04001B61 RID: 7009
		Analog15,
		// Token: 0x04001B62 RID: 7010
		Analog16,
		// Token: 0x04001B63 RID: 7011
		Analog17,
		// Token: 0x04001B64 RID: 7012
		Analog18,
		// Token: 0x04001B65 RID: 7013
		Analog19,
		// Token: 0x04001B66 RID: 7014
		Button0 = 500,
		// Token: 0x04001B67 RID: 7015
		Button1,
		// Token: 0x04001B68 RID: 7016
		Button2,
		// Token: 0x04001B69 RID: 7017
		Button3,
		// Token: 0x04001B6A RID: 7018
		Button4,
		// Token: 0x04001B6B RID: 7019
		Button5,
		// Token: 0x04001B6C RID: 7020
		Button6,
		// Token: 0x04001B6D RID: 7021
		Button7,
		// Token: 0x04001B6E RID: 7022
		Button8,
		// Token: 0x04001B6F RID: 7023
		Button9,
		// Token: 0x04001B70 RID: 7024
		Button10,
		// Token: 0x04001B71 RID: 7025
		Button11,
		// Token: 0x04001B72 RID: 7026
		Button12,
		// Token: 0x04001B73 RID: 7027
		Button13,
		// Token: 0x04001B74 RID: 7028
		Button14,
		// Token: 0x04001B75 RID: 7029
		Button15,
		// Token: 0x04001B76 RID: 7030
		Button16,
		// Token: 0x04001B77 RID: 7031
		Button17,
		// Token: 0x04001B78 RID: 7032
		Button18,
		// Token: 0x04001B79 RID: 7033
		Button19,
		// Token: 0x04001B7A RID: 7034
		Count
	}
}
