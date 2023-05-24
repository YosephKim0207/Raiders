using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000911 RID: 2321
	[Tooltip("Moves a Game Object with a Character Controller. See also Controller Simple Move. NOTE: It is recommended that you make only one call to Move or SimpleMove per frame.")]
	[ActionCategory(ActionCategory.Character)]
	public class ControllerMove : FsmStateAction
	{
		// Token: 0x06003323 RID: 13091 RVA: 0x0010C380 File Offset: 0x0010A580
		public override void Reset()
		{
			this.gameObject = null;
			this.moveVector = new FsmVector3
			{
				UseVariable = true
			};
			this.space = Space.World;
			this.perSecond = true;
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x0010C3BC File Offset: 0x0010A5BC
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
				if (this.perSecond.Value)
				{
					this.controller.Move(vector * Time.deltaTime);
				}
				else
				{
					this.controller.Move(vector);
				}
			}
		}

		// Token: 0x04002449 RID: 9289
		[Tooltip("The GameObject to move.")]
		[CheckForComponent(typeof(CharacterController))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400244A RID: 9290
		[Tooltip("The movement vector.")]
		[RequiredField]
		public FsmVector3 moveVector;

		// Token: 0x0400244B RID: 9291
		[Tooltip("Move in local or word space.")]
		public Space space;

		// Token: 0x0400244C RID: 9292
		[Tooltip("Movement vector is defined in units per second. Makes movement frame rate independent.")]
		public FsmBool perSecond;

		// Token: 0x0400244D RID: 9293
		private GameObject previousGo;

		// Token: 0x0400244E RID: 9294
		private CharacterController controller;
	}
}
