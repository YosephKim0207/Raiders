using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA8 RID: 3240
	[ActionCategory(".NPCs")]
	public class PassthroughInteract : FsmStateAction
	{
		// Token: 0x0600453A RID: 17722 RVA: 0x00166C2C File Offset: 0x00164E2C
		public override void Reset()
		{
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00166C30 File Offset: 0x00164E30
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x00166C38 File Offset: 0x00164E38
		public override void OnEnter()
		{
			this.TargetTalker.Interact(GameManager.Instance.PrimaryPlayer);
			base.Finish();
		}

		// Token: 0x0400375F RID: 14175
		public TalkDoerLite TargetTalker;

		// Token: 0x04003760 RID: 14176
		private TalkDoerLite m_talkDoer;
	}
}
