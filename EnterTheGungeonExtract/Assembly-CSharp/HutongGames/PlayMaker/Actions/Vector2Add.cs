using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B4E RID: 2894
	[Tooltip("Adds a value to Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Add : FsmStateAction
	{
		// Token: 0x06003CBE RID: 15550 RVA: 0x00130D78 File Offset: 0x0012EF78
		public override void Reset()
		{
			this.vector2Variable = null;
			this.addVector = new FsmVector2
			{
				UseVariable = true
			};
			this.everyFrame = false;
			this.perSecond = false;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00130DB0 File Offset: 0x0012EFB0
		public override void OnEnter()
		{
			this.DoVector2Add();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00130DCC File Offset: 0x0012EFCC
		public override void OnUpdate()
		{
			this.DoVector2Add();
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x00130DD4 File Offset: 0x0012EFD4
		private void DoVector2Add()
		{
			if (this.perSecond)
			{
				this.vector2Variable.Value = this.vector2Variable.Value + this.addVector.Value * Time.deltaTime;
			}
			else
			{
				this.vector2Variable.Value = this.vector2Variable.Value + this.addVector.Value;
			}
		}

		// Token: 0x04002F04 RID: 12036
		[Tooltip("The vector2 target")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F05 RID: 12037
		[Tooltip("The vector2 to add")]
		[RequiredField]
		public FsmVector2 addVector;

		// Token: 0x04002F06 RID: 12038
		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		// Token: 0x04002F07 RID: 12039
		[Tooltip("Add the value on a per second bases.")]
		public bool perSecond;
	}
}
