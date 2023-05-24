using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009EF RID: 2543
	[Tooltip("Sends Events based on the comparison of 2 Integers.")]
	[ActionCategory(ActionCategory.Logic)]
	public class IntCompare : FsmStateAction
	{
		// Token: 0x06003698 RID: 13976 RVA: 0x00117044 File Offset: 0x00115244
		public override void Reset()
		{
			this.integer1 = 0;
			this.integer2 = 0;
			this.equal = null;
			this.lessThan = null;
			this.greaterThan = null;
			this.everyFrame = false;
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x0011707C File Offset: 0x0011527C
		public override void OnEnter()
		{
			this.DoIntCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x00117098 File Offset: 0x00115298
		public override void OnUpdate()
		{
			this.DoIntCompare();
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x001170A0 File Offset: 0x001152A0
		private void DoIntCompare()
		{
			if (this.integer1.Value == this.integer2.Value)
			{
				base.Fsm.Event(this.equal);
				return;
			}
			if (this.integer1.Value < this.integer2.Value)
			{
				base.Fsm.Event(this.lessThan);
				return;
			}
			if (this.integer1.Value > this.integer2.Value)
			{
				base.Fsm.Event(this.greaterThan);
			}
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x00117134 File Offset: 0x00115334
		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(this.equal) && FsmEvent.IsNullOrEmpty(this.lessThan) && FsmEvent.IsNullOrEmpty(this.greaterThan))
			{
				return "Action sends no events!";
			}
			return string.Empty;
		}

		// Token: 0x040027F1 RID: 10225
		[RequiredField]
		public FsmInt integer1;

		// Token: 0x040027F2 RID: 10226
		[RequiredField]
		public FsmInt integer2;

		// Token: 0x040027F3 RID: 10227
		[Tooltip("Event sent if Int 1 equals Int 2")]
		public FsmEvent equal;

		// Token: 0x040027F4 RID: 10228
		[Tooltip("Event sent if Int 1 is less than Int 2")]
		public FsmEvent lessThan;

		// Token: 0x040027F5 RID: 10229
		[Tooltip("Event sent if Int 1 is greater than Int 2")]
		public FsmEvent greaterThan;

		// Token: 0x040027F6 RID: 10230
		public bool everyFrame;
	}
}
