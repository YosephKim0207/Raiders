using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A18 RID: 2584
	[Tooltip("Rotates a Game Object so its forward vector points at a Target. The Target can be specified as a GameObject or a world Position. If you specify both, then Position specifies a local offset from the target object's Position.")]
	[ActionCategory(ActionCategory.Transform)]
	public class LookAt : FsmStateAction
	{
		// Token: 0x06003758 RID: 14168 RVA: 0x0011D384 File Offset: 0x0011B584
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
			this.debugLineColor = Color.yellow;
			this.everyFrame = true;
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x0011D3F8 File Offset: 0x0011B5F8
		public override void OnEnter()
		{
			this.DoLookAt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x0011D414 File Offset: 0x0011B614
		public override void OnLateUpdate()
		{
			this.DoLookAt();
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x0011D41C File Offset: 0x0011B61C
		private void DoLookAt()
		{
			if (!this.UpdateLookAtPosition())
			{
				return;
			}
			this.go.transform.LookAt(this.lookAtPos, (!this.upVector.IsNone) ? this.upVector.Value : Vector3.up);
			if (this.debug.Value)
			{
				Debug.DrawLine(this.go.transform.position, this.lookAtPos, this.debugLineColor.Value);
			}
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x0011D4A8 File Offset: 0x0011B6A8
		public bool UpdateLookAtPosition()
		{
			if (base.Fsm == null)
			{
				return false;
			}
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				return false;
			}
			this.goTarget = this.targetObject.Value;
			if (this.goTarget == null && this.targetPosition.IsNone)
			{
				return false;
			}
			if (this.goTarget != null)
			{
				this.lookAtPos = (this.targetPosition.IsNone ? this.goTarget.transform.position : this.goTarget.transform.TransformPoint(this.targetPosition.Value));
			}
			else
			{
				this.lookAtPos = this.targetPosition.Value;
			}
			this.lookAtPosWithVertical = this.lookAtPos;
			if (this.keepVertical.Value)
			{
				this.lookAtPos.y = this.go.transform.position.y;
			}
			return true;
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x0011D5D0 File Offset: 0x0011B7D0
		public Vector3 GetLookAtPosition()
		{
			return this.lookAtPos;
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x0011D5D8 File Offset: 0x0011B7D8
		public Vector3 GetLookAtPositionWithVertical()
		{
			return this.lookAtPosWithVertical;
		}

		// Token: 0x04002930 RID: 10544
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002931 RID: 10545
		[Tooltip("The GameObject to Look At.")]
		public FsmGameObject targetObject;

		// Token: 0x04002932 RID: 10546
		[Tooltip("World position to look at, or local offset from Target Object if specified.")]
		public FsmVector3 targetPosition;

		// Token: 0x04002933 RID: 10547
		[Tooltip("Rotate the GameObject to point its up direction vector in the direction hinted at by the Up Vector. See Unity Look At docs for more details.")]
		public FsmVector3 upVector;

		// Token: 0x04002934 RID: 10548
		[Tooltip("Don't rotate vertically.")]
		public FsmBool keepVertical;

		// Token: 0x04002935 RID: 10549
		[Tooltip("Draw a debug line from the GameObject to the Target.")]
		[Title("Draw Debug Line")]
		public FsmBool debug;

		// Token: 0x04002936 RID: 10550
		[Tooltip("Color to use for the debug line.")]
		public FsmColor debugLineColor;

		// Token: 0x04002937 RID: 10551
		[Tooltip("Repeat every frame.")]
		public bool everyFrame = true;

		// Token: 0x04002938 RID: 10552
		private GameObject go;

		// Token: 0x04002939 RID: 10553
		private GameObject goTarget;

		// Token: 0x0400293A RID: 10554
		private Vector3 lookAtPos;

		// Token: 0x0400293B RID: 10555
		private Vector3 lookAtPosWithVertical;
	}
}
