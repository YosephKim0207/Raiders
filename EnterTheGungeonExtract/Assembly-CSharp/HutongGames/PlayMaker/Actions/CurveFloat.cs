using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200088A RID: 2186
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Float Variable FROM-TO with assistance of Deformation Curve.")]
	public class CurveFloat : CurveFsmAction
	{
		// Token: 0x060030AE RID: 12462 RVA: 0x00100F34 File Offset: 0x000FF134
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = new FsmFloat
			{
				UseVariable = true
			};
			this.toValue = new FsmFloat
			{
				UseVariable = true
			};
			this.fromValue = new FsmFloat
			{
				UseVariable = true
			};
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x00100F84 File Offset: 0x000FF184
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[1];
			this.fromFloats = new float[1];
			this.fromFloats[0] = ((!this.fromValue.IsNone) ? this.fromValue.Value : 0f);
			this.toFloats = new float[1];
			this.toFloats[0] = ((!this.toValue.IsNone) ? this.toValue.Value : 0f);
			this.calculations = new CurveFsmAction.Calculation[1];
			this.calculations[0] = this.calculation;
			this.curves = new AnimationCurve[1];
			this.curves[0] = this.animCurve.curve;
			base.Init();
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x0010105C File Offset: 0x000FF25C
		public override void OnExit()
		{
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x00101060 File Offset: 0x000FF260
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.floatVariable.IsNone && this.isRunning)
			{
				this.floatVariable.Value = this.resultFloats[0];
			}
			if (this.finishInNextStep && !this.looping)
			{
				base.Finish();
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
			}
			if (this.finishAction && !this.finishInNextStep)
			{
				if (!this.floatVariable.IsNone)
				{
					this.floatVariable.Value = this.resultFloats[0];
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x04002182 RID: 8578
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002183 RID: 8579
		[RequiredField]
		public FsmFloat fromValue;

		// Token: 0x04002184 RID: 8580
		[RequiredField]
		public FsmFloat toValue;

		// Token: 0x04002185 RID: 8581
		[RequiredField]
		public FsmAnimationCurve animCurve;

		// Token: 0x04002186 RID: 8582
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue and toValue.")]
		public CurveFsmAction.Calculation calculation;

		// Token: 0x04002187 RID: 8583
		private bool finishInNextStep;
	}
}
