using System;
using UnityEngine;

namespace XInputDotNetPure
{
	// Token: 0x0200081B RID: 2075
	public struct GamePadThumbSticks
	{
		// Token: 0x06002C39 RID: 11321 RVA: 0x000DFBA4 File Offset: 0x000DDDA4
		internal GamePadThumbSticks(GamePadThumbSticks.StickValue left, GamePadThumbSticks.StickValue right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x000DFBB4 File Offset: 0x000DDDB4
		public GamePadThumbSticks.StickValue Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000DFBBC File Offset: 0x000DDDBC
		public GamePadThumbSticks.StickValue Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04001E1B RID: 7707
		private GamePadThumbSticks.StickValue left;

		// Token: 0x04001E1C RID: 7708
		private GamePadThumbSticks.StickValue right;

		// Token: 0x0200081C RID: 2076
		public struct StickValue
		{
			// Token: 0x06002C3C RID: 11324 RVA: 0x000DFBC4 File Offset: 0x000DDDC4
			internal StickValue(float x, float y)
			{
				this.vector = new Vector2(x, y);
			}

			// Token: 0x1700084F RID: 2127
			// (get) Token: 0x06002C3D RID: 11325 RVA: 0x000DFBD4 File Offset: 0x000DDDD4
			public float X
			{
				get
				{
					return this.vector.x;
				}
			}

			// Token: 0x17000850 RID: 2128
			// (get) Token: 0x06002C3E RID: 11326 RVA: 0x000DFBE4 File Offset: 0x000DDDE4
			public float Y
			{
				get
				{
					return this.vector.y;
				}
			}

			// Token: 0x17000851 RID: 2129
			// (get) Token: 0x06002C3F RID: 11327 RVA: 0x000DFBF4 File Offset: 0x000DDDF4
			public Vector2 Vector
			{
				get
				{
					return this.vector;
				}
			}

			// Token: 0x04001E1D RID: 7709
			private Vector2 vector;
		}
	}
}
