using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A65 RID: 2661
	[Tooltip("Rotates a 2d Game Object on it's z axis so its forward vector points at a Target.")]
	[ActionCategory(ActionCategory.Transform)]
	public class LookAt2dGameObject : FsmStateAction
	{
		// Token: 0x0600389C RID: 14492 RVA: 0x00122824 File Offset: 0x00120A24
		public override void Reset()
		{
			this.gameObject = null;
			this.targetObject = null;
			this.debug = false;
			this.debugLineColor = Color.green;
			this.everyFrame = true;
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x00122858 File Offset: 0x00120A58
		public override void OnEnter()
		{
			this.DoLookAt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x00122874 File Offset: 0x00120A74
		public override void OnUpdate()
		{
			this.DoLookAt();
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0012287C File Offset: 0x00120A7C
		private void DoLookAt()
		{
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			this.goTarget = this.targetObject.Value;
			if (this.go == null || this.targetObject == null)
			{
				return;
			}
			Vector3 vector = this.goTarget.transform.position - this.go.transform.position;
			vector.Normalize();
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this.go.transform.rotation = Quaternion.Euler(0f, 0f, num - this.rotationOffset.Value);
			if (this.debug.Value)
			{
				Debug.DrawLine(this.go.transform.position, this.goTarget.transform.position, this.debugLineColor.Value);
			}
		}

		// Token: 0x04002AD9 RID: 10969
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002ADA RID: 10970
		[Tooltip("The GameObject to Look At.")]
		public FsmGameObject targetObject;

		// Token: 0x04002ADB RID: 10971
		[Tooltip("Set the GameObject starting offset. In degrees. 0 if your object is facing right, 180 if facing left etc...")]
		public FsmFloat rotationOffset;

		// Token: 0x04002ADC RID: 10972
		[Tooltip("Draw a debug line from the GameObject to the Target.")]
		[Title("Draw Debug Line")]
		public FsmBool debug;

		// Token: 0x04002ADD RID: 10973
		[Tooltip("Color to use for the debug line.")]
		public FsmColor debugLineColor;

		// Token: 0x04002ADE RID: 10974
		[Tooltip("Repeat every frame.")]
		public bool everyFrame = true;

		// Token: 0x04002ADF RID: 10975
		private GameObject go;

		// Token: 0x04002AE0 RID: 10976
		private GameObject goTarget;
	}
}
