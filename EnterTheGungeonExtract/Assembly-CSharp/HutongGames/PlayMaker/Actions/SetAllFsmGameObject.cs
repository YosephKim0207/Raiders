using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC0 RID: 2752
	[Tooltip("Set the value of a Game Object Variable in another All FSM. Accept null reference")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetAllFsmGameObject : FsmStateAction
	{
		// Token: 0x06003A55 RID: 14933 RVA: 0x00128F5C File Offset: 0x0012715C
		public override void Reset()
		{
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00128F60 File Offset: 0x00127160
		public override void OnEnter()
		{
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x00128F74 File Offset: 0x00127174
		private void DoSetFsmGameObject()
		{
		}

		// Token: 0x04002C8D RID: 11405
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C8E RID: 11406
		public bool everyFrame;
	}
}
