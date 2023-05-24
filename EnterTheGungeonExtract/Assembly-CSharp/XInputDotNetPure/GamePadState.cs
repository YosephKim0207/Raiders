using System;

namespace XInputDotNetPure
{
	// Token: 0x0200081E RID: 2078
	public struct GamePadState
	{
		// Token: 0x06002C43 RID: 11331 RVA: 0x000DFC1C File Offset: 0x000DDE1C
		internal GamePadState(bool isConnected, GamePadState.RawState rawState)
		{
			this.isConnected = isConnected;
			if (!isConnected)
			{
				rawState.dwPacketNumber = 0U;
				rawState.Gamepad.dwButtons = 0;
				rawState.Gamepad.bLeftTrigger = 0;
				rawState.Gamepad.bRightTrigger = 0;
				rawState.Gamepad.sThumbLX = 0;
				rawState.Gamepad.sThumbLY = 0;
				rawState.Gamepad.sThumbRX = 0;
				rawState.Gamepad.sThumbRY = 0;
			}
			this.packetNumber = rawState.dwPacketNumber;
			this.buttons = new GamePadButtons(((rawState.Gamepad.dwButtons & 16) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 32) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 64) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 128) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 256) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 512) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 4096) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 8192) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 16384) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 32768) == 0) ? ButtonState.Released : ButtonState.Pressed);
			this.dPad = new GamePadDPad(((rawState.Gamepad.dwButtons & 1) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 2) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 4) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 8) == 0) ? ButtonState.Released : ButtonState.Pressed);
			this.thumbSticks = new GamePadThumbSticks(new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbLX / 32767f, (float)rawState.Gamepad.sThumbLY / 32767f), new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbRX / 32767f, (float)rawState.Gamepad.sThumbRY / 32767f));
			this.triggers = new GamePadTriggers((float)rawState.Gamepad.bLeftTrigger / 255f, (float)rawState.Gamepad.bRightTrigger / 255f);
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x000DFEDC File Offset: 0x000DE0DC
		public uint PacketNumber
		{
			get
			{
				return this.packetNumber;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x000DFEE4 File Offset: 0x000DE0E4
		public bool IsConnected
		{
			get
			{
				return this.isConnected;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002C46 RID: 11334 RVA: 0x000DFEEC File Offset: 0x000DE0EC
		public GamePadButtons Buttons
		{
			get
			{
				return this.buttons;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002C47 RID: 11335 RVA: 0x000DFEF4 File Offset: 0x000DE0F4
		public GamePadDPad DPad
		{
			get
			{
				return this.dPad;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002C48 RID: 11336 RVA: 0x000DFEFC File Offset: 0x000DE0FC
		public GamePadTriggers Triggers
		{
			get
			{
				return this.triggers;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002C49 RID: 11337 RVA: 0x000DFF04 File Offset: 0x000DE104
		public GamePadThumbSticks ThumbSticks
		{
			get
			{
				return this.thumbSticks;
			}
		}

		// Token: 0x04001E20 RID: 7712
		private bool isConnected;

		// Token: 0x04001E21 RID: 7713
		private uint packetNumber;

		// Token: 0x04001E22 RID: 7714
		private GamePadButtons buttons;

		// Token: 0x04001E23 RID: 7715
		private GamePadDPad dPad;

		// Token: 0x04001E24 RID: 7716
		private GamePadThumbSticks thumbSticks;

		// Token: 0x04001E25 RID: 7717
		private GamePadTriggers triggers;

		// Token: 0x0200081F RID: 2079
		internal struct RawState
		{
			// Token: 0x04001E26 RID: 7718
			public uint dwPacketNumber;

			// Token: 0x04001E27 RID: 7719
			public GamePadState.RawState.GamePad Gamepad;

			// Token: 0x02000820 RID: 2080
			public struct GamePad
			{
				// Token: 0x04001E28 RID: 7720
				public ushort dwButtons;

				// Token: 0x04001E29 RID: 7721
				public byte bLeftTrigger;

				// Token: 0x04001E2A RID: 7722
				public byte bRightTrigger;

				// Token: 0x04001E2B RID: 7723
				public short sThumbLX;

				// Token: 0x04001E2C RID: 7724
				public short sThumbLY;

				// Token: 0x04001E2D RID: 7725
				public short sThumbRX;

				// Token: 0x04001E2E RID: 7726
				public short sThumbRY;
			}
		}

		// Token: 0x02000821 RID: 2081
		private enum ButtonsConstants
		{
			// Token: 0x04001E30 RID: 7728
			DPadUp = 1,
			// Token: 0x04001E31 RID: 7729
			DPadDown,
			// Token: 0x04001E32 RID: 7730
			DPadLeft = 4,
			// Token: 0x04001E33 RID: 7731
			DPadRight = 8,
			// Token: 0x04001E34 RID: 7732
			Start = 16,
			// Token: 0x04001E35 RID: 7733
			Back = 32,
			// Token: 0x04001E36 RID: 7734
			LeftThumb = 64,
			// Token: 0x04001E37 RID: 7735
			RightThumb = 128,
			// Token: 0x04001E38 RID: 7736
			LeftShoulder = 256,
			// Token: 0x04001E39 RID: 7737
			RightShoulder = 512,
			// Token: 0x04001E3A RID: 7738
			A = 4096,
			// Token: 0x04001E3B RID: 7739
			B = 8192,
			// Token: 0x04001E3C RID: 7740
			X = 16384,
			// Token: 0x04001E3D RID: 7741
			Y = 32768
		}
	}
}
