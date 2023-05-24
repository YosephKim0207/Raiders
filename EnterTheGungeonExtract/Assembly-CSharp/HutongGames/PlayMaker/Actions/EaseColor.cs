using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200088F RID: 2191
	[Tooltip("Easing Animation - Color")]
	[ActionCategory(ActionCategory.AnimateVariables)]
	public class EaseColor : EaseFsmAction
	{
		// Token: 0x060030C2 RID: 12482 RVA: 0x001025C0 File Offset: 0x001007C0
		public override void Reset()
		{
			base.Reset();
			this.colorVariable = null;
			this.fromValue = null;
			this.toValue = null;
			this.finishInNextStep = false;
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x001025E4 File Offset: 0x001007E4
		public override void OnEnter()
		{
			base.OnEnter();
			this.fromFloats = new float[4];
			this.fromFloats[0] = this.fromValue.Value.r;
			this.fromFloats[1] = this.fromValue.Value.g;
			this.fromFloats[2] = this.fromValue.Value.b;
			this.fromFloats[3] = this.fromValue.Value.a;
			this.toFloats = new float[4];
			this.toFloats[0] = this.toValue.Value.r;
			this.toFloats[1] = this.toValue.Value.g;
			this.toFloats[2] = this.toValue.Value.b;
			this.toFloats[3] = this.toValue.Value.a;
			this.resultFloats = new float[4];
			this.finishInNextStep = false;
			this.colorVariable.Value = this.fromValue.Value;
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x00102714 File Offset: 0x00100914
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x0010271C File Offset: 0x0010091C
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.colorVariable.IsNone && this.isRunning)
			{
				this.colorVariable.Value = new Color(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
			}
			if (this.finishInNextStep)
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
					this.colorVariable.Value = new Color((!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.r : this.fromValue.Value.r) : this.toValue.Value.r, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.g : this.fromValue.Value.g) : this.toValue.Value.g, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.b : this.fromValue.Value.b) : this.toValue.Value.b, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.a : this.fromValue.Value.a) : this.toValue.Value.a);
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x040021C0 RID: 8640
		[RequiredField]
		public FsmColor fromValue;

		// Token: 0x040021C1 RID: 8641
		[RequiredField]
		public FsmColor toValue;

		// Token: 0x040021C2 RID: 8642
		[UIHint(UIHint.Variable)]
		public FsmColor colorVariable;

		// Token: 0x040021C3 RID: 8643
		private bool finishInNextStep;
	}
}
