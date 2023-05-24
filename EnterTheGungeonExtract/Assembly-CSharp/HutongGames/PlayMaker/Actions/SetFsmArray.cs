using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008EF RID: 2287
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Copy an Array Variable in another FSM.")]
	public class SetFsmArray : BaseFsmVariableAction
	{
		// Token: 0x06003282 RID: 12930 RVA: 0x001097CC File Offset: 0x001079CC
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = string.Empty;
			this.variableName = null;
			this.setValue = null;
			this.copyValues = true;
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x001097FC File Offset: 0x001079FC
		public override void OnEnter()
		{
			this.DoSetFsmArrayCopy();
			base.Finish();
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x0010980C File Offset: 0x00107A0C
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
				if (fsmArray.ElementType != this.setValue.ElementType)
				{
					base.LogError(string.Concat(new object[]
					{
						"Can only copy arrays with the same elements type. Found <",
						fsmArray.ElementType,
						"> and <",
						this.setValue.ElementType,
						">"
					}));
					return;
				}
				fsmArray.Resize(0);
				if (this.copyValues)
				{
					fsmArray.Values = this.setValue.Values.Clone() as object[];
				}
				else
				{
					fsmArray.Values = this.setValue.Values;
				}
			}
			else
			{
				base.DoVariableNotFound(this.variableName.Value);
			}
		}

		// Token: 0x040023A4 RID: 9124
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040023A5 RID: 9125
		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		// Token: 0x040023A6 RID: 9126
		[Tooltip("The name of the FSM variable.")]
		[UIHint(UIHint.FsmArray)]
		[RequiredField]
		public FsmString variableName;

		// Token: 0x040023A7 RID: 9127
		[UIHint(UIHint.Variable)]
		[Tooltip("Set the content of the array variable.")]
		[RequiredField]
		public FsmArray setValue;

		// Token: 0x040023A8 RID: 9128
		[Tooltip("If true, makes copies. if false, values share the same reference and editing one array item value will affect the source and vice versa. Warning, this only affect the current items of the source array. Adding or removing items doesn't affect other FsmArrays.")]
		public bool copyValues;
	}
}
