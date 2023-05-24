using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AEF RID: 2799
	[Tooltip("Sets the Text used by the GUIText Component attached to a Game Object.")]
	[ActionCategory(ActionCategory.GUIElement)]
	public class SetGUIText : ComponentAction<GUIText>
	{
		// Token: 0x06003B28 RID: 15144 RVA: 0x0012B770 File Offset: 0x00129970
		public override void Reset()
		{
			this.gameObject = null;
			this.text = string.Empty;
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0012B78C File Offset: 0x0012998C
		public override void OnEnter()
		{
			this.DoSetGUIText();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x0012B7A8 File Offset: 0x001299A8
		public override void OnUpdate()
		{
			this.DoSetGUIText();
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0012B7B0 File Offset: 0x001299B0
		private void DoSetGUIText()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.guiText.text = this.text.Value;
			}
		}

		// Token: 0x04002D6B RID: 11627
		[RequiredField]
		[CheckForComponent(typeof(GUIText))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D6C RID: 11628
		[UIHint(UIHint.TextArea)]
		public FsmString text;

		// Token: 0x04002D6D RID: 11629
		public bool everyFrame;
	}
}
