using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B4D RID: 2893
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Sets the XY channels of a Vector2 Variable. To leave any channel unchanged, set variable to 'None'.")]
	public class SetVector2XY : FsmStateAction
	{
		// Token: 0x06003CB9 RID: 15545 RVA: 0x00130C6C File Offset: 0x0012EE6C
		public override void Reset()
		{
			this.vector2Variable = null;
			this.vector2Value = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00130CB8 File Offset: 0x0012EEB8
		public override void OnEnter()
		{
			this.DoSetVector2XYZ();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00130CD4 File Offset: 0x0012EED4
		public override void OnUpdate()
		{
			this.DoSetVector2XYZ();
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x00130CDC File Offset: 0x0012EEDC
		private void DoSetVector2XYZ()
		{
			if (this.vector2Variable == null)
			{
				return;
			}
			Vector2 vector = this.vector2Variable.Value;
			if (!this.vector2Value.IsNone)
			{
				vector = this.vector2Value.Value;
			}
			if (!this.x.IsNone)
			{
				vector.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				vector.y = this.y.Value;
			}
			this.vector2Variable.Value = vector;
		}

		// Token: 0x04002EFF RID: 12031
		[Tooltip("The vector2 target")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F00 RID: 12032
		[UIHint(UIHint.Variable)]
		[Tooltip("The vector2 source")]
		public FsmVector2 vector2Value;

		// Token: 0x04002F01 RID: 12033
		[Tooltip("The x component. Override vector2Value if set")]
		public FsmFloat x;

		// Token: 0x04002F02 RID: 12034
		[Tooltip("The y component.Override vector2Value if set")]
		public FsmFloat y;

		// Token: 0x04002F03 RID: 12035
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
