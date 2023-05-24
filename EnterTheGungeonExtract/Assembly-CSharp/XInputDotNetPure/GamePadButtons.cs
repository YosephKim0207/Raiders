using System;

namespace XInputDotNetPure
{
	// Token: 0x02000819 RID: 2073
	public struct GamePadButtons
	{
		// Token: 0x06002C29 RID: 11305 RVA: 0x000DFAB8 File Offset: 0x000DDCB8
		internal GamePadButtons(ButtonState start, ButtonState back, ButtonState leftStick, ButtonState rightStick, ButtonState leftShoulder, ButtonState rightShoulder, ButtonState a, ButtonState b, ButtonState x, ButtonState y)
		{
			this.start = start;
			this.back = back;
			this.leftStick = leftStick;
			this.rightStick = rightStick;
			this.leftShoulder = leftShoulder;
			this.rightShoulder = rightShoulder;
			this.a = a;
			this.b = b;
			this.x = x;
			this.y = y;
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x000DFB14 File Offset: 0x000DDD14
		public ButtonState Start
		{
			get
			{
				return this.start;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x000DFB1C File Offset: 0x000DDD1C
		public ButtonState Back
		{
			get
			{
				return this.back;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x000DFB24 File Offset: 0x000DDD24
		public ButtonState LeftStick
		{
			get
			{
				return this.leftStick;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x000DFB2C File Offset: 0x000DDD2C
		public ButtonState RightStick
		{
			get
			{
				return this.rightStick;
			}
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x000DFB34 File Offset: 0x000DDD34
		public ButtonState LeftShoulder
		{
			get
			{
				return this.leftShoulder;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x000DFB3C File Offset: 0x000DDD3C
		public ButtonState RightShoulder
		{
			get
			{
				return this.rightShoulder;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002C30 RID: 11312 RVA: 0x000DFB44 File Offset: 0x000DDD44
		public ButtonState A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x000DFB4C File Offset: 0x000DDD4C
		public ButtonState B
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002C32 RID: 11314 RVA: 0x000DFB54 File Offset: 0x000DDD54
		public ButtonState X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002C33 RID: 11315 RVA: 0x000DFB5C File Offset: 0x000DDD5C
		public ButtonState Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x04001E0D RID: 7693
		private ButtonState start;

		// Token: 0x04001E0E RID: 7694
		private ButtonState back;

		// Token: 0x04001E0F RID: 7695
		private ButtonState leftStick;

		// Token: 0x04001E10 RID: 7696
		private ButtonState rightStick;

		// Token: 0x04001E11 RID: 7697
		private ButtonState leftShoulder;

		// Token: 0x04001E12 RID: 7698
		private ButtonState rightShoulder;

		// Token: 0x04001E13 RID: 7699
		private ButtonState a;

		// Token: 0x04001E14 RID: 7700
		private ButtonState b;

		// Token: 0x04001E15 RID: 7701
		private ButtonState x;

		// Token: 0x04001E16 RID: 7702
		private ButtonState y;
	}
}
