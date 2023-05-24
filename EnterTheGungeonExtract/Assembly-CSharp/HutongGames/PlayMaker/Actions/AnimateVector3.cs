using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000888 RID: 2184
	[Tooltip("Animates the value of a Vector3 Variable using an Animation Curve.")]
	[ActionCategory(ActionCategory.AnimateVariables)]
	public class AnimateVector3 : AnimateFsmAction
	{
		// Token: 0x060030A4 RID: 12452 RVA: 0x001008EC File Offset: 0x000FEAEC
		public override void Reset()
		{
			base.Reset();
			this.vectorVariable = new FsmVector3
			{
				UseVariable = true
			};
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x00100914 File Offset: 0x000FEB14
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[3];
			this.fromFloats = new float[3];
			this.fromFloats[0] = ((!this.vectorVariable.IsNone) ? this.vectorVariable.Value.x : 0f);
			this.fromFloats[1] = ((!this.vectorVariable.IsNone) ? this.vectorVariable.Value.y : 0f);
			this.fromFloats[2] = ((!this.vectorVariable.IsNone) ? this.vectorVariable.Value.z : 0f);
			this.curves = new AnimationCurve[3];
			this.curves[0] = this.curveX.curve;
			this.curves[1] = this.curveY.curve;
			this.curves[2] = this.curveZ.curve;
			this.calculations = new AnimateFsmAction.Calculation[3];
			this.calculations[0] = this.calculationX;
			this.calculations[1] = this.calculationY;
			this.calculations[2] = this.calculationZ;
			base.Init();
			if (Math.Abs(this.delay.Value) < 0.01f)
			{
				this.UpdateVariableValue();
			}
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x00100A88 File Offset: 0x000FEC88
		private void UpdateVariableValue()
		{
			if (!this.vectorVariable.IsNone)
			{
				this.vectorVariable.Value = new Vector3(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2]);
			}
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x00100AC4 File Offset: 0x000FECC4
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

		// Token: 0x0400216D RID: 8557
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vectorVariable;

		// Token: 0x0400216E RID: 8558
		[RequiredField]
		public FsmAnimationCurve curveX;

		// Token: 0x0400216F RID: 8559
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to vectorVariable.x.")]
		public AnimateFsmAction.Calculation calculationX;

		// Token: 0x04002170 RID: 8560
		[RequiredField]
		public FsmAnimationCurve curveY;

		// Token: 0x04002171 RID: 8561
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to vectorVariable.y.")]
		public AnimateFsmAction.Calculation calculationY;

		// Token: 0x04002172 RID: 8562
		[RequiredField]
		public FsmAnimationCurve curveZ;

		// Token: 0x04002173 RID: 8563
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to vectorVariable.z.")]
		public AnimateFsmAction.Calculation calculationZ;

		// Token: 0x04002174 RID: 8564
		private bool finishInNextStep;
	}
}
