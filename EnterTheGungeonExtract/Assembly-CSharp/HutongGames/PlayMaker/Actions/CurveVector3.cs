using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200088E RID: 2190
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Animates the value of a Vector3 Variable FROM-TO with assistance of Deformation Curves.")]
	public class CurveVector3 : CurveFsmAction
	{
		// Token: 0x060030BD RID: 12477 RVA: 0x00102268 File Offset: 0x00100468
		public override void Reset()
		{
			base.Reset();
			this.vectorVariable = new FsmVector3
			{
				UseVariable = true
			};
			this.toValue = new FsmVector3
			{
				UseVariable = true
			};
			this.fromValue = new FsmVector3
			{
				UseVariable = true
			};
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x001022B8 File Offset: 0x001004B8
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[3];
			this.fromFloats = new float[3];
			this.fromFloats[0] = ((!this.fromValue.IsNone) ? this.fromValue.Value.x : 0f);
			this.fromFloats[1] = ((!this.fromValue.IsNone) ? this.fromValue.Value.y : 0f);
			this.fromFloats[2] = ((!this.fromValue.IsNone) ? this.fromValue.Value.z : 0f);
			this.toFloats = new float[3];
			this.toFloats[0] = ((!this.toValue.IsNone) ? this.toValue.Value.x : 0f);
			this.toFloats[1] = ((!this.toValue.IsNone) ? this.toValue.Value.y : 0f);
			this.toFloats[2] = ((!this.toValue.IsNone) ? this.toValue.Value.z : 0f);
			this.curves = new AnimationCurve[3];
			this.curves[0] = this.curveX.curve;
			this.curves[1] = this.curveY.curve;
			this.curves[2] = this.curveZ.curve;
			this.calculations = new CurveFsmAction.Calculation[3];
			this.calculations[0] = this.calculationX;
			this.calculations[1] = this.calculationY;
			this.calculations[2] = this.calculationZ;
			base.Init();
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x001024B8 File Offset: 0x001006B8
		public override void OnExit()
		{
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x001024BC File Offset: 0x001006BC
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.vectorVariable.IsNone && this.isRunning)
			{
				this.vct = new Vector3(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2]);
				this.vectorVariable.Value = this.vct;
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
				if (!this.vectorVariable.IsNone)
				{
					this.vct = new Vector3(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2]);
					this.vectorVariable.Value = this.vct;
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x040021B5 RID: 8629
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vectorVariable;

		// Token: 0x040021B6 RID: 8630
		[RequiredField]
		public FsmVector3 fromValue;

		// Token: 0x040021B7 RID: 8631
		[RequiredField]
		public FsmVector3 toValue;

		// Token: 0x040021B8 RID: 8632
		[RequiredField]
		public FsmAnimationCurve curveX;

		// Token: 0x040021B9 RID: 8633
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.x and toValue.x.")]
		public CurveFsmAction.Calculation calculationX;

		// Token: 0x040021BA RID: 8634
		[RequiredField]
		public FsmAnimationCurve curveY;

		// Token: 0x040021BB RID: 8635
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.y and toValue.y.")]
		public CurveFsmAction.Calculation calculationY;

		// Token: 0x040021BC RID: 8636
		[RequiredField]
		public FsmAnimationCurve curveZ;

		// Token: 0x040021BD RID: 8637
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to otherwise linear move between fromValue.z and toValue.z.")]
		public CurveFsmAction.Calculation calculationZ;

		// Token: 0x040021BE RID: 8638
		private Vector3 vct;

		// Token: 0x040021BF RID: 8639
		private bool finishInNextStep;
	}
}
