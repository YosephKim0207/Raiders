using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200098D RID: 2445
	[Tooltip("Gets the pressed state of a Key.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetKey : FsmStateAction
	{
		// Token: 0x06003525 RID: 13605 RVA: 0x001128BC File Offset: 0x00110ABC
		public override void Reset()
		{
			this.key = KeyCode.None;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x001128D4 File Offset: 0x00110AD4
		public override void OnEnter()
		{
			this.DoGetKey();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x001128F0 File Offset: 0x00110AF0
		public override void OnUpdate()
		{
			this.DoGetKey();
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x001128F8 File Offset: 0x00110AF8
		private void DoGetKey()
		{
			this.storeResult.Value = Input.GetKey(this.key);
		}

		// Token: 0x0400268D RID: 9869
		[Tooltip("The key to test.")]
		[RequiredField]
		public KeyCode key;

		// Token: 0x0400268E RID: 9870
		[Tooltip("Store if the key is down (True) or up (False).")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool storeResult;

		// Token: 0x0400268F RID: 9871
		[Tooltip("Repeat every frame. Useful if you're waiting for a key press/release.")]
		public bool everyFrame;
	}
}
