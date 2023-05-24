using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A1F RID: 2591
	[Tooltip("Move a GameObject to another GameObject. Works like iTween Move To, but with better performance.")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=4758.0")]
	[ActionCategory(ActionCategory.Transform)]
	public class MoveObject : EaseFsmAction
	{
		// Token: 0x0600377C RID: 14204 RVA: 0x0011DF20 File Offset: 0x0011C120
		public override void Reset()
		{
			base.Reset();
			this.fromValue = null;
			this.toVector = null;
			this.finishInNextStep = false;
			this.fromVector = null;
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x0011DF44 File Offset: 0x0011C144
		public override void OnEnter()
		{
			base.OnEnter();
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.objectToMove);
			this.fromVector = ownerDefaultTarget.transform.position;
			this.toVector = this.destination.Value.transform.position;
			this.fromFloats = new float[3];
			this.fromFloats[0] = this.fromVector.Value.x;
			this.fromFloats[1] = this.fromVector.Value.y;
			this.fromFloats[2] = this.fromVector.Value.z;
			this.toFloats = new float[3];
			this.toFloats[0] = this.toVector.Value.x;
			this.toFloats[1] = this.toVector.Value.y;
			this.toFloats[2] = this.toVector.Value.z;
			this.resultFloats = new float[3];
			this.resultFloats[0] = this.fromVector.Value.x;
			this.resultFloats[1] = this.fromVector.Value.y;
			this.resultFloats[2] = this.fromVector.Value.z;
			this.finishInNextStep = false;
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x0011E0C4 File Offset: 0x0011C2C4
		public override void OnUpdate()
		{
			base.OnUpdate();
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.objectToMove);
			ownerDefaultTarget.transform.position = new Vector3(this.resultFloats[0], this.resultFloats[1], this.resultFloats[2]);
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
				ownerDefaultTarget.transform.position = new Vector3((!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toVector.Value.x : this.fromValue.Value.x) : this.toVector.Value.x, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toVector.Value.y : this.fromValue.Value.y) : this.toVector.Value.y, (!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toVector.Value.z : this.fromValue.Value.z) : this.toVector.Value.z);
				this.finishInNextStep = true;
			}
		}

		// Token: 0x0400296C RID: 10604
		[RequiredField]
		public FsmOwnerDefault objectToMove;

		// Token: 0x0400296D RID: 10605
		[RequiredField]
		public FsmGameObject destination;

		// Token: 0x0400296E RID: 10606
		private FsmVector3 fromValue;

		// Token: 0x0400296F RID: 10607
		private FsmVector3 toVector;

		// Token: 0x04002970 RID: 10608
		private FsmVector3 fromVector;

		// Token: 0x04002971 RID: 10609
		private bool finishInNextStep;
	}
}
