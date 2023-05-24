using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B5E RID: 2910
	[Tooltip("Adds a value to Vector3 Variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Add : FsmStateAction
	{
		// Token: 0x06003CFF RID: 15615 RVA: 0x00131AFC File Offset: 0x0012FCFC
		public override void Reset()
		{
			this.vector3Variable = null;
			this.addVector = new FsmVector3
			{
				UseVariable = true
			};
			this.everyFrame = false;
			this.perSecond = false;
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x00131B34 File Offset: 0x0012FD34
		public override void OnEnter()
		{
			this.DoVector3Add();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00131B50 File Offset: 0x0012FD50
		public override void OnUpdate()
		{
			this.DoVector3Add();
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x00131B58 File Offset: 0x0012FD58
		private void DoVector3Add()
		{
			if (this.perSecond)
			{
				this.vector3Variable.Value = this.vector3Variable.Value + this.addVector.Value * Time.deltaTime;
			}
			else
			{
				this.vector3Variable.Value = this.vector3Variable.Value + this.addVector.Value;
			}
		}

		// Token: 0x04002F4A RID: 12106
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F4B RID: 12107
		[RequiredField]
		public FsmVector3 addVector;

		// Token: 0x04002F4C RID: 12108
		public bool everyFrame;

		// Token: 0x04002F4D RID: 12109
		public bool perSecond;
	}
}
