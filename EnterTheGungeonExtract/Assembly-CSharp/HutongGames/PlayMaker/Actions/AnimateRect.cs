using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000887 RID: 2183
	[ActionCategory("AnimateVariables")]
	[Tooltip("Animates the value of a Rect Variable using an Animation Curve.")]
	public class AnimateRect : AnimateFsmAction
	{
		// Token: 0x0600309F RID: 12447 RVA: 0x00100630 File Offset: 0x000FE830
		public override void Reset()
		{
			base.Reset();
			this.rectVariable = new FsmRect
			{
				UseVariable = true
			};
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x00100658 File Offset: 0x000FE858
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[4];
			this.fromFloats = new float[4];
			this.fromFloats[0] = ((!this.rectVariable.IsNone) ? this.rectVariable.Value.x : 0f);
			this.fromFloats[1] = ((!this.rectVariable.IsNone) ? this.rectVariable.Value.y : 0f);
			this.fromFloats[2] = ((!this.rectVariable.IsNone) ? this.rectVariable.Value.width : 0f);
			this.fromFloats[3] = ((!this.rectVariable.IsNone) ? this.rectVariable.Value.height : 0f);
			this.curves = new AnimationCurve[4];
			this.curves[0] = this.curveX.curve;
			this.curves[1] = this.curveY.curve;
			this.curves[2] = this.curveW.curve;
			this.curves[3] = this.curveH.curve;
			this.calculations = new AnimateFsmAction.Calculation[4];
			this.calculations[0] = this.calculationX;
			this.calculations[1] = this.calculationY;
			this.calculations[2] = this.calculationW;
			this.calculations[3] = this.calculationH;
			base.Init();
			if (Math.Abs(this.delay.Value) < 0.01f)
			{
				this.UpdateVariableValue();
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x00100820 File Offset: 0x000FEA20
		private void UpdateVariableValue()
		{
			if (!this.rectVariable.IsNone)
			{
				this.rectVariable.Value = new Rect(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
			}
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x00100870 File Offset: 0x000FEA70
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

		// Token: 0x04002163 RID: 8547
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmRect rectVariable;

		// Token: 0x04002164 RID: 8548
		[RequiredField]
		public FsmAnimationCurve curveX;

		// Token: 0x04002165 RID: 8549
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.x.")]
		public AnimateFsmAction.Calculation calculationX;

		// Token: 0x04002166 RID: 8550
		[RequiredField]
		public FsmAnimationCurve curveY;

		// Token: 0x04002167 RID: 8551
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.y.")]
		public AnimateFsmAction.Calculation calculationY;

		// Token: 0x04002168 RID: 8552
		[RequiredField]
		public FsmAnimationCurve curveW;

		// Token: 0x04002169 RID: 8553
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.width.")]
		public AnimateFsmAction.Calculation calculationW;

		// Token: 0x0400216A RID: 8554
		[RequiredField]
		public FsmAnimationCurve curveH;

		// Token: 0x0400216B RID: 8555
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to rectVariable.height.")]
		public AnimateFsmAction.Calculation calculationH;

		// Token: 0x0400216C RID: 8556
		private bool finishInNextStep;
	}
}
