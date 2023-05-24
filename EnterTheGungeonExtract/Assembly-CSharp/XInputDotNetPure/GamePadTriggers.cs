using System;

namespace XInputDotNetPure
{
	// Token: 0x0200081D RID: 2077
	public struct GamePadTriggers
	{
		// Token: 0x06002C40 RID: 11328 RVA: 0x000DFBFC File Offset: 0x000DDDFC
		internal GamePadTriggers(float left, float right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002C41 RID: 11329 RVA: 0x000DFC0C File Offset: 0x000DDE0C
		public float Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x000DFC14 File Offset: 0x000DDE14
		public float Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04001E1E RID: 7710
		private float left;

		// Token: 0x04001E1F RID: 7711
		private float right;
	}
}
