using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C76 RID: 3190
	[ActionCategory(ActionCategory.Logic)]
	public class TestCurrentLanguageEnglish : FsmStateAction
	{
		// Token: 0x06004481 RID: 17537 RVA: 0x001623E8 File Offset: 0x001605E8
		public override void Reset()
		{
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x001623EC File Offset: 0x001605EC
		public override void OnEnter()
		{
			this.DoIDSwitch();
			base.Finish();
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x001623FC File Offset: 0x001605FC
		public override void OnUpdate()
		{
			this.DoIDSwitch();
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x00162404 File Offset: 0x00160604
		private void DoIDSwitch()
		{
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				base.Fsm.Event(this.EnglishEvent);
			}
			else
			{
				base.Fsm.Event(this.OtherEvent);
			}
		}

		// Token: 0x04003690 RID: 13968
		public FsmEvent EnglishEvent;

		// Token: 0x04003691 RID: 13969
		public FsmEvent OtherEvent;
	}
}
