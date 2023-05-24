using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008EE RID: 2286
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Gets an item in an Array Variable in another FSM.")]
	public class GetFsmArrayItem : BaseFsmVariableIndexAction
	{
		// Token: 0x0600327D RID: 12925 RVA: 0x00109678 File Offset: 0x00107878
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.storeValue = null;
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x00109698 File Offset: 0x00107898
		public override void OnEnter()
		{
			this.DoGetFsmArray();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x001096B4 File Offset: 0x001078B4
		private void DoGetFsmArray()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget, this.fsmName.Value))
			{
				return;
			}
			FsmArray fsmArray = this.fsm.FsmVariables.GetFsmArray(this.variableName.Value);
			if (fsmArray != null)
			{
				if (this.index.Value < 0 || this.index.Value >= fsmArray.Length)
				{
					base.Fsm.Event(this.indexOutOfRange);
					base.Finish();
					return;
				}
				if (fsmArray.ElementType == this.storeValue.NamedVar.VariableType)
				{
					this.storeValue.SetValue(fsmArray.Get(this.index.Value));
				}
				else
				{
					base.LogWarning("Incompatible variable type: " + this.variableName.Value);
				}
			}
			else
			{
				base.DoVariableNotFound(this.variableName.Value);
			}
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x001097BC File Offset: 0x001079BC
		public override void OnUpdate()
		{
			this.DoGetFsmArray();
		}

		// Token: 0x0400239E RID: 9118
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400239F RID: 9119
		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x040023A0 RID: 9120
		[RequiredField]
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmArray)]
		public FsmString variableName;

		// Token: 0x040023A1 RID: 9121
		[Tooltip("The index into the array.")]
		public FsmInt index;

		// Token: 0x040023A2 RID: 9122
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the value of the array at the specified index.")]
		[RequiredField]
		public FsmVar storeValue;

		// Token: 0x040023A3 RID: 9123
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;
	}
}
