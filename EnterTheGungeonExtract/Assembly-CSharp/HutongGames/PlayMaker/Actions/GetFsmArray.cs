using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008ED RID: 2285
	[Tooltip("Copy an Array Variable from another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class GetFsmArray : BaseFsmVariableAction
	{
		// Token: 0x06003279 RID: 12921 RVA: 0x00109518 File Offset: 0x00107718
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = null;
			this.storeValue = null;
			this.copyValues = true;
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x00109548 File Offset: 0x00107748
		public override void OnEnter()
		{
			this.DoSetFsmArrayCopy();
			base.Finish();
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x00109558 File Offset: 0x00107758
		private void DoSetFsmArrayCopy()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget, this.fsmName.Value))
			{
				return;
			}
			FsmArray fsmArray = this.fsm.FsmVariables.GetFsmArray(this.variableName.Value);
			if (fsmArray != null)
			{
				if (fsmArray.ElementType != this.storeValue.ElementType)
				{
					base.LogError(string.Concat(new object[]
					{
						"Can only copy arrays with the same elements type. Found <",
						fsmArray.ElementType,
						"> and <",
						this.storeValue.ElementType,
						">"
					}));
					return;
				}
				this.storeValue.Resize(0);
				if (this.copyValues)
				{
					this.storeValue.Values = fsmArray.Values.Clone() as object[];
				}
				else
				{
					this.storeValue.Values = fsmArray.Values;
				}
			}
			else
			{
				base.DoVariableNotFound(this.variableName.Value);
			}
		}

		// Token: 0x04002399 RID: 9113
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400239A RID: 9114
		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x0400239B RID: 9115
		[RequiredField]
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmArray)]
		public FsmString variableName;

		// Token: 0x0400239C RID: 9116
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the content of the array variable.")]
		[RequiredField]
		public FsmArray storeValue;

		// Token: 0x0400239D RID: 9117
		[Tooltip("If true, makes copies. if false, values share the same reference and editing one array item value will affect the source and vice versa. Warning, this only affect the current items of the source array. Adding or removing items doesn't affect other FsmArrays.")]
		public bool copyValues;
	}
}
