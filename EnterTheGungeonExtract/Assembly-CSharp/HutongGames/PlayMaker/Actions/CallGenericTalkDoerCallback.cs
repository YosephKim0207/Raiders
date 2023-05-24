using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C8C RID: 3212
	[ActionCategory(".NPCs")]
	public class CallGenericTalkDoerCallback : FsmStateAction
	{
		// Token: 0x060044CA RID: 17610 RVA: 0x001639FC File Offset: 0x00161BFC
		public override void Reset()
		{
			this.everyFrame = false;
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x00163A08 File Offset: 0x00161C08
		private void DoCallbacks()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (this.CallCallbackA.Value && component.OnGenericFSMActionA != null)
			{
				component.OnGenericFSMActionA();
			}
			if (this.CallCallbackB.Value && component.OnGenericFSMActionB != null)
			{
				component.OnGenericFSMActionB();
			}
			if (this.CallCallbackC.Value && component.OnGenericFSMActionC != null)
			{
				component.OnGenericFSMActionC();
			}
			if (this.CallCallbackD.Value && component.OnGenericFSMActionD != null)
			{
				component.OnGenericFSMActionD();
			}
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x00163ABC File Offset: 0x00161CBC
		public override void OnEnter()
		{
			if (!this.everyFrame)
			{
				this.DoCallbacks();
				base.Finish();
			}
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x00163AD8 File Offset: 0x00161CD8
		public override void OnUpdate()
		{
			this.DoCallbacks();
		}

		// Token: 0x040036DA RID: 14042
		public FsmBool CallCallbackA;

		// Token: 0x040036DB RID: 14043
		public FsmBool CallCallbackB;

		// Token: 0x040036DC RID: 14044
		public FsmBool CallCallbackC;

		// Token: 0x040036DD RID: 14045
		public FsmBool CallCallbackD;

		// Token: 0x040036DE RID: 14046
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
