using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200088D RID: 2189
	[ActionCategory("AnimateVariables")]
	[Tooltip("Animates the value of a Rect Variable FROM-TO with assistance of Deformation Curves.")]
	public class CurveRect : CurveFsmAction
	{
		// Token: 0x060030B8 RID: 12472 RVA: 0x00101E74 File Offset: 0x00100074
		public override void Reset()
		{
			base.Reset();
			this.rectVariable = new FsmRect
			{
				UseVariable = true
			};
			this.toValue = new FsmRect
			{
				UseVariable = true
			};
			this.fromValue = new FsmRect
			{
				UseVariable = true
			};
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x00101EC4 File Offset: 0x001000C4
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[4];
			this.fromFloats = new float[4];
			this.fromFloats[0] = ((!this.fromValue.IsNone) ? this.fromValue.Value.x : 0f);
			this.fromFloats[1] = ((!this.fromValue.IsNone) ? this.fromValue.Value.y : 0f);
			this.fromFloats[2] = ((!this.fromValue.IsNone) ? this.fromValue.Value.width : 0f);
			this.fromFloats[3] = ((!this.fromValue.IsNone) ? this.fromValue.Value.height : 0f);
			this.toFloats = new float[4];
			this.toFloats[0] = ((!this.toValue.IsNone) ? this.toValue.Value.x : 0f);
			this.toFloats[1] = ((!this.toValue.IsNone) ? this.toValue.Value.y : 0f);
			this.toFloats[2] = ((!this.toValue.IsNone) ? this.toValue.Value.width : 0f);
			this.toFloats[3] = ((!this.toValue.IsNone) ? this.toValue.Value.height : 0f);
			this.curves = new AnimationCurve[4];
			this.curves[0] = this.curveX.curve;
			this.curves[1] = this.curveY.curve;
			this.curves[2] = this.curveW.curve;
			this.curves[3] = this.curveH.curve;
			this.calculations = new CurveFsmAction.Calculation[4];
			this.calculations[0] = this.calculationX;
			this.calculations[1] = this.calculationY;
			this.calculations[2] = this.calculationW;
			this.calculations[2] = this.calculationH;
			base.Init();
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x00102150 File Offset: 0x00100350
		public override void OnExit()
		{
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x00102154 File Offset: 0x00100354
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.rectVariable.IsNone && this.isRunning)
			{
				this.rct = new Rect(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
				this.rectVariable.Value = this.rct;
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
				if (!this.rectVariable.IsNone)
				{
					this.rct = new Rect(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
					this.rectVariable.Value = this.rct;
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x040021A8 RID: 8616
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmRect rectVariable;

		// Token: 0x040021A9 RID: 8617
		[RequiredField]
		public FsmRect fromValue;

		// Token: 0x040021AA RID: 8618
		[RequiredField]
		public FsmRect toValue;

		// Token: 0x040021AB RID: 8619
		[RequiredField]
		public FsmAnimationCurve curveX;

		// Token: 0x040021AC RID: 8620
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.x and toValue.x.")]
		public CurveFsmAction.Calculation calculationX;

		// Token: 0x040021AD RID: 8621
		[RequiredField]
		public FsmAnimationCurve curveY;

		// Token: 0x040021AE RID: 8622
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.y and toValue.y.")]
		public CurveFsmAction.Calculation calculationY;

		// Token: 0x040021AF RID: 8623
		[RequiredField]
		public FsmAnimationCurve curveW;

		// Token: 0x040021B0 RID: 8624
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.width and toValue.width.")]
		public CurveFsmAction.Calculation calculationW;

		// Token: 0x040021B1 RID: 8625
		[RequiredField]
		public FsmAnimationCurve curveH;

		// Token: 0x040021B2 RID: 8626
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.height and toValue.height.")]
		public CurveFsmAction.Calculation calculationH;

		// Token: 0x040021B3 RID: 8627
		private Rect rct;

		// Token: 0x040021B4 RID: 8628
		private bool finishInNextStep;
	}
}
