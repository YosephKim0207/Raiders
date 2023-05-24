using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200096A RID: 2410
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the pressed state of the specified Button and stores it in a Bool Variable. See Unity Input Manager docs.")]
	public class GetButton : FsmStateAction
	{
		// Token: 0x06003487 RID: 13447 RVA: 0x00110734 File Offset: 0x0010E934
		public override void Reset()
		{
			this.buttonName = "Fire1";
			this.storeResult = null;
			this.everyFrame = true;
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00110754 File Offset: 0x0010E954
		public override void OnEnter()
		{
			this.DoGetButton();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x00110770 File Offset: 0x0010E970
		public override void OnUpdate()
		{
			this.DoGetButton();
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x00110778 File Offset: 0x0010E978
		private void DoGetButton()
		{
			this.storeResult.Value = Input.GetButton(this.buttonName.Value);
		}

		// Token: 0x040025BC RID: 9660
		[Tooltip("The name of the button. Set in the Unity Input Manager.")]
		[RequiredField]
		public FsmString buttonName;

		// Token: 0x040025BD RID: 9661
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a bool variable.")]
		[RequiredField]
		public FsmBool storeResult;

		// Token: 0x040025BE RID: 9662
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
