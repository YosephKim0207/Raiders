using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B20 RID: 2848
	[Tooltip("Smoothly Rotates a Game Object so its forward vector points at a Target. The target can be defined as a Game Object or a world Position. If you specify both, then the position will be used as a local offset from the object's position.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SmoothLookAt : FsmStateAction
	{
		// Token: 0x06003C00 RID: 15360 RVA: 0x0012E108 File Offset: 0x0012C308
		public override void Reset()
		{
			this.gameObject = null;
			this.targetObject = null;
			this.targetPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.upVector = new FsmVector3
			{
				UseVariable = true
			};
			this.keepVertical = true;
			this.debug = false;
			this.speed = 5f;
			this.finishTolerance = 1f;
			this.finishEvent = null;
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x0012E18C File Offset: 0x0012C38C
		public override void OnEnter()
		{
			this.previousGo = null;
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x0012E198 File Offset: 0x0012C398
		public override void OnLateUpdate()
		{
			this.DoSmoothLookAt();
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x0012E1A0 File Offset: 0x0012C3A0
		private void DoSmoothLookAt()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			GameObject value = this.targetObject.Value;
			if (value == null && this.targetPosition.IsNone)
			{
				return;
			}
			if (this.previousGo != ownerDefaultTarget)
			{
				this.lastRotation = ownerDefaultTarget.transform.rotation;
				this.desiredRotation = this.lastRotation;
				this.previousGo = ownerDefaultTarget;
			}
			Vector3 vector;
			if (value != null)
			{
				vector = (this.targetPosition.IsNone ? value.transform.position : value.transform.TransformPoint(this.targetPosition.Value));
			}
			else
			{
				vector = this.targetPosition.Value;
			}
			if (this.keepVertical.Value)
			{
				vector.y = ownerDefaultTarget.transform.position.y;
			}
			Vector3 vector2 = vector - ownerDefaultTarget.transform.position;
			if (vector2 != Vector3.zero && vector2.sqrMagnitude > 0f)
			{
				this.desiredRotation = Quaternion.LookRotation(vector2, (!this.upVector.IsNone) ? this.upVector.Value : Vector3.up);
			}
			this.lastRotation = Quaternion.Slerp(this.lastRotation, this.desiredRotation, this.speed.Value * Time.deltaTime);
			ownerDefaultTarget.transform.rotation = this.lastRotation;
			if (this.debug.Value)
			{
				Debug.DrawLine(ownerDefaultTarget.transform.position, vector, Color.grey);
			}
			if (this.finishEvent != null)
			{
				Vector3 vector3 = vector - ownerDefaultTarget.transform.position;
				float num = Vector3.Angle(vector3, ownerDefaultTarget.transform.forward);
				if (Mathf.Abs(num) <= this.finishTolerance.Value)
				{
					base.Fsm.Event(this.finishEvent);
				}
			}
		}

		// Token: 0x04002E22 RID: 11810
		[Tooltip("The GameObject to rotate to face a target.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E23 RID: 11811
		[Tooltip("A target GameObject.")]
		public FsmGameObject targetObject;

		// Token: 0x04002E24 RID: 11812
		[Tooltip("A target position. If a Target Object is defined, this is used as a local offset.")]
		public FsmVector3 targetPosition;

		// Token: 0x04002E25 RID: 11813
		[Tooltip("Used to keep the game object generally upright. If left undefined the world y axis is used.")]
		public FsmVector3 upVector;

		// Token: 0x04002E26 RID: 11814
		[Tooltip("Force the game object to remain vertical. Useful for characters.")]
		public FsmBool keepVertical;

		// Token: 0x04002E27 RID: 11815
		[Tooltip("How fast the look at moves.")]
		[HasFloatSlider(0.5f, 15f)]
		public FsmFloat speed;

		// Token: 0x04002E28 RID: 11816
		[Tooltip("Draw a line in the Scene View to the look at position.")]
		public FsmBool debug;

		// Token: 0x04002E29 RID: 11817
		[Tooltip("If the angle to the target is less than this, send the Finish Event below. Measured in degrees.")]
		public FsmFloat finishTolerance;

		// Token: 0x04002E2A RID: 11818
		[Tooltip("Event to send if the angle to target is less than the Finish Tolerance.")]
		public FsmEvent finishEvent;

		// Token: 0x04002E2B RID: 11819
		private GameObject previousGo;

		// Token: 0x04002E2C RID: 11820
		private Quaternion lastRotation;

		// Token: 0x04002E2D RID: 11821
		private Quaternion desiredRotation;
	}
}
