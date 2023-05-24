using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F0 RID: 2288
	[Tooltip("Set an item in an Array Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmArrayItem : BaseFsmVariableIndexAction
	{
		// Token: 0x06003286 RID: 12934 RVA: 0x00109928 File Offset: 0x00107B28
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.value = null;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x00109948 File Offset: 0x00107B48
		public override void OnEnter()
		{
			this.DoSetFsmArray();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x00109964 File Offset: 0x00107B64
		private void DoSetFsmArray()
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
				if (fsmArray.ElementType == this.value.NamedVar.VariableType)
				{
					this.value.UpdateValue();
					fsmArray.Set(this.index.Value, this.value.GetValue());
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

		// Token: 0x06003289 RID: 12937 RVA: 0x00109A74 File Offset: 0x00107C74
		public override void OnUpdate()
		{
			this.DoSetFsmArray();
		}

		// Token: 0x040023A9 RID: 9129
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040023AA RID: 9130
		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x040023AB RID: 9131
		[UIHint(UIHint.FsmArray)]
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x040023AC RID: 9132
		[Tooltip("The index into the array.")]
		public FsmInt index;

		// Token: 0x040023AD RID: 9133
		[RequiredField]
		[Tooltip("Set the value of the array at the specified index.")]
		public FsmVar value;

		// Token: 0x040023AE RID: 9134
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;
	}
}
