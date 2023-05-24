using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B3 RID: 2483
	[Tooltip("Gets system date and time info and stores it in a string variable. An optional format string gives you a lot of control over the formatting (see online docs for format syntax).")]
	[ActionCategory(ActionCategory.Time)]
	public class GetSystemDateTime : FsmStateAction
	{
		// Token: 0x060035C7 RID: 13767 RVA: 0x001140D8 File Offset: 0x001122D8
		public override void Reset()
		{
			this.storeString = null;
			this.format = "MM/dd/yyyy HH:mm";
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x001140F4 File Offset: 0x001122F4
		public override void OnEnter()
		{
			this.storeString.Value = DateTime.Now.ToString(this.format.Value);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x00114138 File Offset: 0x00112338
		public override void OnUpdate()
		{
			this.storeString.Value = DateTime.Now.ToString(this.format.Value);
		}

		// Token: 0x0400270D RID: 9997
		[Tooltip("Store System DateTime as a string.")]
		[UIHint(UIHint.Variable)]
		public FsmString storeString;

		// Token: 0x0400270E RID: 9998
		[Tooltip("Optional format string. E.g., MM/dd/yyyy HH:mm")]
		public FsmString format;

		// Token: 0x0400270F RID: 9999
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
