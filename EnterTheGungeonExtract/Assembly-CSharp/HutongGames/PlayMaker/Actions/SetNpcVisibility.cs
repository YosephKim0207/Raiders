using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CB0 RID: 3248
	[ActionCategory(".NPCs")]
	[Tooltip("Sets the NPC's visibility (renderers and Speculative Rigidbody).")]
	public class SetNpcVisibility : FsmStateAction
	{
		// Token: 0x06004551 RID: 17745 RVA: 0x0016769C File Offset: 0x0016589C
		public override void Reset()
		{
			this.visible = true;
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x001676AC File Offset: 0x001658AC
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (component)
			{
				SetNpcVisibility.SetVisible(component, this.visible.Value);
			}
			base.Finish();
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x001676E8 File Offset: 0x001658E8
		public static void SetVisible(TalkDoerLite talkDoer, bool visible)
		{
			talkDoer.renderer.enabled = visible;
			talkDoer.ShowOutlines = visible;
			if (talkDoer.shadow)
			{
				talkDoer.shadow.GetComponent<Renderer>().enabled = visible;
			}
			if (talkDoer.specRigidbody)
			{
				talkDoer.specRigidbody.enabled = visible;
			}
			if (talkDoer.ultraFortunesFavor)
			{
				talkDoer.ultraFortunesFavor.enabled = visible;
			}
		}

		// Token: 0x04003770 RID: 14192
		[Tooltip("Set visibility to this.")]
		public FsmBool visible;
	}
}
