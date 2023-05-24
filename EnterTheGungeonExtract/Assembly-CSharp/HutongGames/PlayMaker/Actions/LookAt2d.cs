using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A64 RID: 2660
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Rotates a 2d Game Object on it's z axis so its forward vector points at a 2d or 3d position.")]
	public class LookAt2d : FsmStateAction
	{
		// Token: 0x06003897 RID: 14487 RVA: 0x00122690 File Offset: 0x00120890
		public override void Reset()
		{
			this.gameObject = null;
			this.vector2Target = null;
			this.vector3Target = new FsmVector3
			{
				UseVariable = true
			};
			this.debug = false;
			this.debugLineColor = Color.green;
			this.everyFrame = true;
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x001226E4 File Offset: 0x001208E4
		public override void OnEnter()
		{
			this.DoLookAt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x00122700 File Offset: 0x00120900
		public override void OnUpdate()
		{
			this.DoLookAt();
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x00122708 File Offset: 0x00120908
		private void DoLookAt()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = new Vector3(this.vector2Target.Value.x, this.vector2Target.Value.y, 0f);
			if (!this.vector3Target.IsNone)
			{
				vector += this.vector3Target.Value;
			}
			Vector3 vector2 = vector - ownerDefaultTarget.transform.position;
			vector2.Normalize();
			float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			ownerDefaultTarget.transform.rotation = Quaternion.Euler(0f, 0f, num - this.rotationOffset.Value);
			if (this.debug.Value)
			{
				Debug.DrawLine(ownerDefaultTarget.transform.position, vector, this.debugLineColor.Value);
			}
		}

		// Token: 0x04002AD2 RID: 10962
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002AD3 RID: 10963
		[Tooltip("The 2d position to Look At.")]
		public FsmVector2 vector2Target;

		// Token: 0x04002AD4 RID: 10964
		[Tooltip("The 3d position to Look At. If not set to none, will be added to the 2d target")]
		public FsmVector3 vector3Target;

		// Token: 0x04002AD5 RID: 10965
		[Tooltip("Set the GameObject starting offset. In degrees. 0 if your object is facing right, 180 if facing left etc...")]
		public FsmFloat rotationOffset;

		// Token: 0x04002AD6 RID: 10966
		[Tooltip("Draw a debug line from the GameObject to the Target.")]
		[Title("Draw Debug Line")]
		public FsmBool debug;

		// Token: 0x04002AD7 RID: 10967
		[Tooltip("Color to use for the debug line.")]
		public FsmColor debugLineColor;

		// Token: 0x04002AD8 RID: 10968
		[Tooltip("Repeat every frame.")]
		public bool everyFrame = true;
	}
}
