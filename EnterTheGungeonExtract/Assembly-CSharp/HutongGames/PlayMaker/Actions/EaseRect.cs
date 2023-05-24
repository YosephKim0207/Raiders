using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000894 RID: 2196
	[Tooltip("Easing Animation - Rect.")]
	[ActionCategory("AnimateVariables")]
	public class EaseRect : EaseFsmAction
	{
		// Token: 0x060030F5 RID: 12533 RVA: 0x00103AFC File Offset: 0x00101CFC
		public override void Reset()
		{
			base.Reset();
			this.rectVariable = null;
			this.fromValue = null;
			this.toValue = null;
			this.finishInNextStep = false;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x00103B20 File Offset: 0x00101D20
		public override void OnEnter()
		{
			base.OnEnter();
			this.fromFloats = new float[4];
			this.fromFloats[0] = this.fromValue.Value.x;
			this.fromFloats[1] = this.fromValue.Value.y;
			this.fromFloats[2] = this.fromValue.Value.width;
			this.fromFloats[3] = this.fromValue.Value.height;
			this.toFloats = new float[4];
			this.toFloats[0] = this.toValue.Value.x;
			this.toFloats[1] = this.toValue.Value.y;
			this.toFloats[2] = this.toValue.Value.width;
			this.toFloats[3] = this.toValue.Value.height;
			this.resultFloats = new float[4];
			this.finishInNextStep = false;
			this.rectVariable.Value = this.fromValue.Value;
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x00103C50 File Offset: 0x00101E50
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x00103C58 File Offset: 0x00101E58
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.rectVariable.IsNone && this.isRunning)
			{
				this.rectVariable.Value = new Rect(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2], this.resultFloats[3]);
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
				if (!this.rectVariable.IsNone)
				{
					this.rectVariable.Value = new Rect((!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.x : this.fromValue.Value.x) : this.toValue.Value.x, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.y : this.fromValue.Value.y) : this.toValue.Value.y, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.width : this.fromValue.Value.width) : this.toValue.Value.width, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.height : this.fromValue.Value.height) : this.toValue.Value.height);
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x040021FB RID: 8699
		[RequiredField]
		public FsmRect fromValue;

		// Token: 0x040021FC RID: 8700
		[RequiredField]
		public FsmRect toValue;

		// Token: 0x040021FD RID: 8701
		[UIHint(UIHint.Variable)]
		public FsmRect rectVariable;

		// Token: 0x040021FE RID: 8702
		private bool finishInNextStep;
	}
}
