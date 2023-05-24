using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000895 RID: 2197
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Easing Animation - Vector3")]
	public class EaseVector3 : EaseFsmAction
	{
		// Token: 0x060030FA RID: 12538 RVA: 0x00103EBC File Offset: 0x001020BC
		public override void Reset()
		{
			base.Reset();
			this.vector3Variable = null;
			this.fromValue = null;
			this.toValue = null;
			this.finishInNextStep = false;
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x00103EE0 File Offset: 0x001020E0
		public override void OnEnter()
		{
			base.OnEnter();
			this.fromFloats = new float[3];
			this.fromFloats[0] = this.fromValue.Value.x;
			this.fromFloats[1] = this.fromValue.Value.y;
			this.fromFloats[2] = this.fromValue.Value.z;
			this.toFloats = new float[3];
			this.toFloats[0] = this.toValue.Value.x;
			this.toFloats[1] = this.toValue.Value.y;
			this.toFloats[2] = this.toValue.Value.z;
			this.resultFloats = new float[3];
			this.finishInNextStep = false;
			this.vector3Variable.Value = this.fromValue.Value;
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x00103FD8 File Offset: 0x001021D8
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x00103FE0 File Offset: 0x001021E0
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.vector3Variable.IsNone && this.isRunning)
			{
				this.vector3Variable.Value = new Vector3(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2]);
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
				if (!this.vector3Variable.IsNone)
				{
					this.vector3Variable.Value = new Vector3((!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.x : this.fromValue.Value.x) : this.toValue.Value.x, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.y : this.fromValue.Value.y) : this.toValue.Value.y, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value.z : this.fromValue.Value.z) : this.toValue.Value.z);
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x040021FF RID: 8703
		[RequiredField]
		public FsmVector3 fromValue;

		// Token: 0x04002200 RID: 8704
		[RequiredField]
		public FsmVector3 toValue;

		// Token: 0x04002201 RID: 8705
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		// Token: 0x04002202 RID: 8706
		private bool finishInNextStep;
	}
}
