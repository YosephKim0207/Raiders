using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000889 RID: 2185
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Color Variable FROM-TO with assistance of Deformation Curves.")]
	public class CurveColor : CurveFsmAction
	{
		// Token: 0x060030A9 RID: 12457 RVA: 0x00100B40 File Offset: 0x000FED40
		public override void Reset()
		{
			base.Reset();
			this.colorVariable = new FsmColor
			{
				UseVariable = true
			};
			this.toValue = new FsmColor
			{
				UseVariable = true
			};
			this.fromValue = new FsmColor
			{
				UseVariable = true
			};
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x00100B90 File Offset: 0x000FED90
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[4];
			this.fromFloats = new float[4];
			this.fromFloats[0] = ((!this.fromValue.IsNone) ? this.fromValue.Value.r : 0f);
			this.fromFloats[1] = ((!this.fromValue.IsNone) ? this.fromValue.Value.g : 0f);
			this.fromFloats[2] = ((!this.fromValue.IsNone) ? this.fromValue.Value.b : 0f);
			this.fromFloats[3] = ((!this.fromValue.IsNone) ? this.fromValue.Value.a : 0f);
			this.toFloats = new float[4];
			this.toFloats[0] = ((!this.toValue.IsNone) ? this.toValue.Value.r : 0f);
			this.toFloats[1] = ((!this.toValue.IsNone) ? this.toValue.Value.g : 0f);
			this.toFloats[2] = ((!this.toValue.IsNone) ? this.toValue.Value.b : 0f);
			this.toFloats[3] = ((!this.toValue.IsNone) ? this.toValue.Value.a : 0f);
			this.curves = new AnimationCurve[4];
			this.curves[0] = this.curveR.curve;
			this.curves[1] = this.curveG.curve;
			this.curves[2] = this.curveB.curve;
			this.curves[3] = this.curveA.curve;
			this.calculations = new CurveFsmAction.Calculation[4];
			this.calculations[0] = this.calculationR;
			this.calculations[1] = this.calculationG;
			this.calculations[2] = this.calculationB;
			this.calculations[3] = this.calculationA;
			base.Init();
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x00100E1C File Offset: 0x000FF01C
		public override void OnExit()
		{
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x00100E20 File Offset: 0x000FF020
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.colorVariable.IsNone && this.isRunning)
			{
				this.clr = new Color(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
				this.colorVariable.Value = this.clr;
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
				if (!this.colorVariable.IsNone)
				{
					this.clr = new Color(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
					this.colorVariable.Value = this.clr;
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x04002175 RID: 8565
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor colorVariable;

		// Token: 0x04002176 RID: 8566
		[RequiredField]
		public FsmColor fromValue;

		// Token: 0x04002177 RID: 8567
		[RequiredField]
		public FsmColor toValue;

		// Token: 0x04002178 RID: 8568
		[RequiredField]
		public FsmAnimationCurve curveR;

		// Token: 0x04002179 RID: 8569
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.Red and toValue.Rec.")]
		public CurveFsmAction.Calculation calculationR;

		// Token: 0x0400217A RID: 8570
		[RequiredField]
		public FsmAnimationCurve curveG;

		// Token: 0x0400217B RID: 8571
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.Green and toValue.Green.")]
		public CurveFsmAction.Calculation calculationG;

		// Token: 0x0400217C RID: 8572
		[RequiredField]
		public FsmAnimationCurve curveB;

		// Token: 0x0400217D RID: 8573
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.Blue and toValue.Blue.")]
		public CurveFsmAction.Calculation calculationB;

		// Token: 0x0400217E RID: 8574
		[RequiredField]
		public FsmAnimationCurve curveA;

		// Token: 0x0400217F RID: 8575
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.Alpha and toValue.Alpha.")]
		public CurveFsmAction.Calculation calculationA;

		// Token: 0x04002180 RID: 8576
		private Color clr;

		// Token: 0x04002181 RID: 8577
		private bool finishInNextStep;
	}
}
