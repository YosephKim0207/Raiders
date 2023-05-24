using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000913 RID: 2323
	[Tooltip("Moves a Game Object with a Character Controller. Velocity along the y-axis is ignored. Speed is in meters/s. Gravity is automatically applied.")]
	[ActionCategory(ActionCategory.Character)]
	public class ControllerSimpleMove : FsmStateAction
	{
		// Token: 0x0600332B RID: 13099 RVA: 0x0010C698 File Offset: 0x0010A898
		public override void Reset()
		{
			this.gameObject = null;
			this.moveVector = new FsmVector3
			{
				UseVariable = true
			};
			this.speed = 1f;
			this.space = Space.World;
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x0010C6D8 File Offset: 0x0010A8D8
		public override void OnUpdate()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.previousGo)
			{
				this.controller = ownerDefaultTarget.GetComponent<CharacterController>();
				this.previousGo = ownerDefaultTarget;
			}
			if (this.controller != null)
			{
				Vector3 vector = ((this.space != Space.World) ? ownerDefaultTarget.transform.TransformDirection(this.moveVector.Value) : this.moveVector.Value);
				this.controller.SimpleMove(vector * this.speed.Value);
			}
		}

		// Token: 0x04002459 RID: 9305
		[CheckForComponent(typeof(CharacterController))]
		[RequiredField]
		[Tooltip("The GameObject to move.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400245A RID: 9306
		[Tooltip("The movement vector.")]
		[RequiredField]
		public FsmVector3 moveVector;

		// Token: 0x0400245B RID: 9307
		[Tooltip("Multiply the movement vector by a speed factor.")]
		public FsmFloat speed;

		// Token: 0x0400245C RID: 9308
		[Tooltip("Move in local or word space.")]
		public Space space;

		// Token: 0x0400245D RID: 9309
		private GameObject previousGo;

		// Token: 0x0400245E RID: 9310
		private CharacterController controller;
	}
}
