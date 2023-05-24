using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000997 RID: 2455
	[Tooltip("Gets the pressed state of the specified Mouse Button and stores it in a Bool Variable. See Unity Input Manager doc.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButton : FsmStateAction
	{
		// Token: 0x0600354B RID: 13643 RVA: 0x00112F0C File Offset: 0x0011110C
		public override void Reset()
		{
			this.button = MouseButton.Left;
			this.storeResult = null;
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x00112F1C File Offset: 0x0011111C
		public override void OnEnter()
		{
			this.storeResult.Value = Input.GetMouseButton((int)this.button);
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x00112F34 File Offset: 0x00111134
		public override void OnUpdate()
		{
			this.storeResult.Value = Input.GetMouseButton((int)this.button);
		}

		// Token: 0x040026AD RID: 9901
		[Tooltip("The mouse button to test.")]
		[RequiredField]
		public MouseButton button;

		// Token: 0x040026AE RID: 9902
		[Tooltip("Store the pressed state in a Bool Variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool storeResult;
	}
}
