using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200095E RID: 2398
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a GameObject variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class GameObjectChanged : FsmStateAction
	{
		// Token: 0x06003455 RID: 13397 RVA: 0x0010FCA8 File Offset: 0x0010DEA8
		public override void Reset()
		{
			this.gameObjectVariable = null;
			this.changedEvent = null;
			this.storeResult = null;
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x0010FCC0 File Offset: 0x0010DEC0
		public override void OnEnter()
		{
			if (this.gameObjectVariable.IsNone)
			{
				base.Finish();
				return;
			}
			this.previousValue = this.gameObjectVariable.Value;
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x0010FCEC File Offset: 0x0010DEEC
		public override void OnUpdate()
		{
			this.storeResult.Value = false;
			if (this.gameObjectVariable.Value != this.previousValue)
			{
				this.storeResult.Value = true;
				base.Fsm.Event(this.changedEvent);
			}
		}

		// Token: 0x0400257F RID: 9599
		[Tooltip("The GameObject variable to watch for a change.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectVariable;

		// Token: 0x04002580 RID: 9600
		[Tooltip("Event to send if the variable changes.")]
		public FsmEvent changedEvent;

		// Token: 0x04002581 RID: 9601
		[UIHint(UIHint.Variable)]
		[Tooltip("Set to True if the variable changes.")]
		public FsmBool storeResult;

		// Token: 0x04002582 RID: 9602
		private GameObject previousValue;
	}
}
