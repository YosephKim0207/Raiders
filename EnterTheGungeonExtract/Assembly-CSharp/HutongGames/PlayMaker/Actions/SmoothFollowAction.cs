using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B1F RID: 2847
	[Tooltip("Action version of Unity's Smooth Follow script.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SmoothFollowAction : FsmStateAction
	{
		// Token: 0x06003BFD RID: 15357 RVA: 0x0012DEC0 File Offset: 0x0012C0C0
		public override void Reset()
		{
			this.gameObject = null;
			this.targetObject = null;
			this.distance = 10f;
			this.height = 5f;
			this.heightDamping = 2f;
			this.rotationDamping = 3f;
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x0012DF1C File Offset: 0x0012C11C
		public override void OnLateUpdate()
		{
			if (this.targetObject.Value == null)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.cachedObject != ownerDefaultTarget)
			{
				this.cachedObject = ownerDefaultTarget;
				this.myTransform = ownerDefaultTarget.transform;
			}
			if (this.cachedTarget != this.targetObject.Value)
			{
				this.cachedTarget = this.targetObject.Value;
				this.targetTransform = this.cachedTarget.transform;
			}
			float y = this.targetTransform.eulerAngles.y;
			float num = this.targetTransform.position.y + this.height.Value;
			float num2 = this.myTransform.eulerAngles.y;
			float num3 = this.myTransform.position.y;
			num2 = Mathf.LerpAngle(num2, y, this.rotationDamping.Value * Time.deltaTime);
			num3 = Mathf.Lerp(num3, num, this.heightDamping.Value * Time.deltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, num2, 0f);
			this.myTransform.position = this.targetTransform.position;
			this.myTransform.position -= quaternion * Vector3.forward * this.distance.Value;
			this.myTransform.position = new Vector3(this.myTransform.position.x, num3, this.myTransform.position.z);
			this.myTransform.LookAt(this.targetTransform);
		}

		// Token: 0x04002E18 RID: 11800
		[Tooltip("The game object to control. E.g. The camera.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E19 RID: 11801
		[Tooltip("The GameObject to follow.")]
		public FsmGameObject targetObject;

		// Token: 0x04002E1A RID: 11802
		[RequiredField]
		[Tooltip("The distance in the x-z plane to the target.")]
		public FsmFloat distance;

		// Token: 0x04002E1B RID: 11803
		[RequiredField]
		[Tooltip("The height we want the camera to be above the target")]
		public FsmFloat height;

		// Token: 0x04002E1C RID: 11804
		[RequiredField]
		[Tooltip("How much to dampen height movement.")]
		public FsmFloat heightDamping;

		// Token: 0x04002E1D RID: 11805
		[RequiredField]
		[Tooltip("How much to dampen rotation changes.")]
		public FsmFloat rotationDamping;

		// Token: 0x04002E1E RID: 11806
		private GameObject cachedObject;

		// Token: 0x04002E1F RID: 11807
		private Transform myTransform;

		// Token: 0x04002E20 RID: 11808
		private GameObject cachedTarget;

		// Token: 0x04002E21 RID: 11809
		private Transform targetTransform;
	}
}
