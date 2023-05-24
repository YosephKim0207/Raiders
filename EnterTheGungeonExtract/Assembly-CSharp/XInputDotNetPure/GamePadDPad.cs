using System;

namespace XInputDotNetPure
{
	// Token: 0x0200081A RID: 2074
	public struct GamePadDPad
	{
		// Token: 0x06002C34 RID: 11316 RVA: 0x000DFB64 File Offset: 0x000DDD64
		internal GamePadDPad(ButtonState up, ButtonState down, ButtonState left, ButtonState right)
		{
			this.up = up;
			this.down = down;
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002C35 RID: 11317 RVA: 0x000DFB84 File Offset: 0x000DDD84
		public ButtonState Up
		{
			get
			{
				return this.up;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002C36 RID: 11318 RVA: 0x000DFB8C File Offset: 0x000DDD8C
		public ButtonState Down
		{
			get
			{
				return this.down;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002C37 RID: 11319 RVA: 0x000DFB94 File Offset: 0x000DDD94
		public ButtonState Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002C38 RID: 11320 RVA: 0x000DFB9C File Offset: 0x000DDD9C
		public ButtonState Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04001E17 RID: 7703
		private ButtonState up;

		// Token: 0x04001E18 RID: 7704
		private ButtonState down;

		// Token: 0x04001E19 RID: 7705
		private ButtonState left;

		// Token: 0x04001E1A RID: 7706
		private ButtonState right;
	}
}
