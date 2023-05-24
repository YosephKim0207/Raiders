using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C86 RID: 3206
	[Tooltip("Makes the NPC a ghost.")]
	[ActionCategory(".NPCs")]
	public class BecomeGhost : FsmStateAction
	{
		// Token: 0x060044BC RID: 17596 RVA: 0x0016343C File Offset: 0x0016163C
		public override void Reset()
		{
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x00163440 File Offset: 0x00161640
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x00163454 File Offset: 0x00161654
		public override void OnEnter()
		{
			if (base.Owner && base.Owner.GetComponent<TalkDoerLite>())
			{
				base.Owner.GetComponent<TalkDoerLite>().ConvertToGhost();
			}
			base.Finish();
		}
	}
}
