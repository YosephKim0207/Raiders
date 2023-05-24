using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000883 RID: 2179
	[Tooltip("Animates the value of a Color Variable using an Animation Curve.")]
	[ActionCategory(ActionCategory.AnimateVariables)]
	public class AnimateColor : AnimateFsmAction
	{
		// Token: 0x0600308C RID: 12428 RVA: 0x000FF658 File Offset: 0x000FD858
		public override void Reset()
		{
			base.Reset();
			this.colorVariable = new FsmColor
			{
				UseVariable = true
			};
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x000FF680 File Offset: 0x000FD880
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[4];
			this.fromFloats = new float[4];
			this.fromFloats[0] = ((!this.colorVariable.IsNone) ? this.colorVariable.Value.r : 0f);
			this.fromFloats[1] = ((!this.colorVariable.IsNone) ? this.colorVariable.Value.g : 0f);
			this.fromFloats[2] = ((!this.colorVariable.IsNone) ? this.colorVariable.Value.b : 0f);
			this.fromFloats[3] = ((!this.colorVariable.IsNone) ? this.colorVariable.Value.a : 0f);
			this.curves = new AnimationCurve[4];
			this.curves[0] = this.curveR.curve;
			this.curves[1] = this.curveG.curve;
			this.curves[2] = this.curveB.curve;
			this.curves[3] = this.curveA.curve;
			this.calculations = new AnimateFsmAction.Calculation[4];
			this.calculations[0] = this.calculationR;
			this.calculations[1] = this.calculationG;
			this.calculations[2] = this.calculationB;
			this.calculations[3] = this.calculationA;
			base.Init();
			if (Math.Abs(this.delay.Value) < 0.01f)
			{
				this.UpdateVariableValue();
			}
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000FF848 File Offset: 0x000FDA48
		private void UpdateVariableValue()
		{
			if (!this.colorVariable.IsNone)
			{
				this.colorVariable.Value = new Color(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
			}
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000FF898 File Offset: 0x000FDA98
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.isRunning)
			{
				this.UpdateVariableValue();
			}
			if (this.finishInNextStep && !this.looping)
			{
				base.Finish();
				base.Fsm.Event(this.finishEvent);
			}
			if (this.finishAction && !this.finishInNextStep)
			{
				this.UpdateVariableValue();
				this.finishInNextStep = true;
			}
		}

		// Token: 0x04002135 RID: 8501
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor colorVariable;

		// Token: 0x04002136 RID: 8502
		[RequiredField]
		public FsmAnimationCurve curveR;

		// Token: 0x04002137 RID: 8503
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to colorVariable.r.")]
		public AnimateFsmAction.Calculation calculationR;

		// Token: 0x04002138 RID: 8504
		[RequiredField]
		public FsmAnimationCurve curveG;

		// Token: 0x04002139 RID: 8505
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to colorVariable.g.")]
		public AnimateFsmAction.Calculation calculationG;

		// Token: 0x0400213A RID: 8506
		[RequiredField]
		public FsmAnimationCurve curveB;

		// Token: 0x0400213B RID: 8507
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to colorVariable.b.")]
		public AnimateFsmAction.Calculation calculationB;

		// Token: 0x0400213C RID: 8508
		[RequiredField]
		public FsmAnimationCurve curveA;

		// Token: 0x0400213D RID: 8509
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to colorVariable.a.")]
		public AnimateFsmAction.Calculation calculationA;

		// Token: 0x0400213E RID: 8510
		private bool finishInNextStep;
	}
}
