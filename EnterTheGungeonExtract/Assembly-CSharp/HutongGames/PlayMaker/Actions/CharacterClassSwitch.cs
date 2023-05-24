using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C53 RID: 3155
	[Tooltip("Sends an Event based on the current character.")]
	[ActionCategory(ActionCategory.Logic)]
	public class CharacterClassSwitch : FsmStateAction
	{
		// Token: 0x060043F1 RID: 17393 RVA: 0x0015EDDC File Offset: 0x0015CFDC
		public override void Reset()
		{
			this.compareTo = new PlayableCharacters[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0015EE00 File Offset: 0x0015D000
		public override void OnEnter()
		{
			this.DoCharSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0015EE1C File Offset: 0x0015D01C
		public override void OnUpdate()
		{
			this.DoCharSwitch();
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0015EE24 File Offset: 0x0015D024
		private void DoCharSwitch()
		{
			for (int i = 0; i < this.compareTo.Length; i++)
			{
				if (base.Owner && base.Owner.GetComponent<TalkDoerLite>() && base.Owner.GetComponent<TalkDoerLite>().TalkingPlayer)
				{
					if (base.Owner.GetComponent<TalkDoerLite>().TalkingPlayer.characterIdentity == this.compareTo[i])
					{
						base.Fsm.Event(this.sendEvent[i]);
						return;
					}
				}
				else if (GameManager.Instance.PrimaryPlayer.characterIdentity == this.compareTo[i])
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x0400360D RID: 13837
		[CompoundArray("Int Switches", "Compare Int", "Send Event")]
		public PlayableCharacters[] compareTo;

		// Token: 0x0400360E RID: 13838
		public FsmEvent[] sendEvent;

		// Token: 0x0400360F RID: 13839
		public bool everyFrame;
	}
}
