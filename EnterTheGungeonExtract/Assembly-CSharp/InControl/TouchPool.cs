using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200076C RID: 1900
	public class TouchPool
	{
		// Token: 0x06002A7A RID: 10874 RVA: 0x000C11DC File Offset: 0x000BF3DC
		public TouchPool(int capacity)
		{
			this.freeTouches = new List<Touch>(capacity);
			for (int i = 0; i < capacity; i++)
			{
				this.freeTouches.Add(new Touch());
			}
			this.usedTouches = new List<Touch>(capacity);
			this.Touches = new ReadOnlyCollection<Touch>(this.usedTouches);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000C123C File Offset: 0x000BF43C
		public TouchPool()
			: this(16)
		{
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000C1248 File Offset: 0x000BF448
		public Touch FindOrCreateTouch(int fingerId)
		{
			int count = this.usedTouches.Count;
			Touch touch;
			for (int i = 0; i < count; i++)
			{
				touch = this.usedTouches[i];
				if (touch.fingerId == fingerId)
				{
					return touch;
				}
			}
			touch = this.NewTouch();
			touch.fingerId = fingerId;
			this.usedTouches.Add(touch);
			return touch;
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000C12AC File Offset: 0x000BF4AC
		public Touch FindTouch(int fingerId)
		{
			int count = this.usedTouches.Count;
			for (int i = 0; i < count; i++)
			{
				Touch touch = this.usedTouches[i];
				if (touch.fingerId == fingerId)
				{
					return touch;
				}
			}
			return null;
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000C12F4 File Offset: 0x000BF4F4
		private Touch NewTouch()
		{
			int count = this.freeTouches.Count;
			if (count > 0)
			{
				Touch touch = this.freeTouches[count - 1];
				this.freeTouches.RemoveAt(count - 1);
				return touch;
			}
			return new Touch();
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000C1338 File Offset: 0x000BF538
		public void FreeTouch(Touch touch)
		{
			touch.Reset();
			this.freeTouches.Add(touch);
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000C134C File Offset: 0x000BF54C
		public void FreeEndedTouches()
		{
			int count = this.usedTouches.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				Touch touch = this.usedTouches[i];
				if (touch.phase == TouchPhase.Ended)
				{
					this.usedTouches.RemoveAt(i);
				}
			}
		}

		// Token: 0x04001D64 RID: 7524
		public readonly ReadOnlyCollection<Touch> Touches;

		// Token: 0x04001D65 RID: 7525
		private List<Touch> usedTouches;

		// Token: 0x04001D66 RID: 7526
		private List<Touch> freeTouches;
	}
}
