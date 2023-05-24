using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B37 RID: 2871
	[Tooltip("Transforms 2d input into a 3d world space vector. E.g., can be used to transform input from a touch joystick to a movement vector.")]
	[NoActionTargets]
	[ActionCategory(ActionCategory.Input)]
	public class TransformInputToWorldSpace : FsmStateAction
	{
		// Token: 0x06003C58 RID: 15448 RVA: 0x0012FC0C File Offset: 0x0012DE0C
		public override void Reset()
		{
			this.horizontalInput = null;
			this.verticalInput = null;
			this.multiplier = 1f;
			this.mapToPlane = TransformInputToWorldSpace.AxisPlane.XZ;
			this.storeVector = null;
			this.storeMagnitude = null;
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x0012FC44 File Offset: 0x0012DE44
		public override void OnUpdate()
		{
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			if (this.relativeTo.Value == null)
			{
				TransformInputToWorldSpace.AxisPlane axisPlane = this.mapToPlane;
				if (axisPlane != TransformInputToWorldSpace.AxisPlane.XZ)
				{
					if (axisPlane != TransformInputToWorldSpace.AxisPlane.XY)
					{
						if (axisPlane == TransformInputToWorldSpace.AxisPlane.YZ)
						{
							vector = Vector3.up;
							vector2 = Vector3.forward;
						}
					}
					else
					{
						vector = Vector3.up;
						vector2 = Vector3.right;
					}
				}
				else
				{
					vector = Vector3.forward;
					vector2 = Vector3.right;
				}
			}
			else
			{
				Transform transform = this.relativeTo.Value.transform;
				TransformInputToWorldSpace.AxisPlane axisPlane2 = this.mapToPlane;
				if (axisPlane2 != TransformInputToWorldSpace.AxisPlane.XZ)
				{
					if (axisPlane2 == TransformInputToWorldSpace.AxisPlane.XY || axisPlane2 == TransformInputToWorldSpace.AxisPlane.YZ)
					{
						vector = Vector3.up;
						vector.z = 0f;
						vector = vector.normalized;
						vector2 = transform.TransformDirection(Vector3.right);
					}
				}
				else
				{
					vector = transform.TransformDirection(Vector3.forward);
					vector.y = 0f;
					vector = vector.normalized;
					vector2 = new Vector3(vector.z, 0f, -vector.x);
				}
			}
			float num = ((!this.horizontalInput.IsNone) ? this.horizontalInput.Value : 0f);
			float num2 = ((!this.verticalInput.IsNone) ? this.verticalInput.Value : 0f);
			Vector3 vector3 = num * vector2 + num2 * vector;
			vector3 *= this.multiplier.Value;
			this.storeVector.Value = vector3;
			if (!this.storeMagnitude.IsNone)
			{
				this.storeMagnitude.Value = vector3.magnitude;
			}
		}

		// Token: 0x04002EA5 RID: 11941
		[Tooltip("The horizontal input.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat horizontalInput;

		// Token: 0x04002EA6 RID: 11942
		[UIHint(UIHint.Variable)]
		[Tooltip("The vertical input.")]
		public FsmFloat verticalInput;

		// Token: 0x04002EA7 RID: 11943
		[Tooltip("Input axis are reported in the range -1 to 1, this multiplier lets you set a new range.")]
		public FsmFloat multiplier;

		// Token: 0x04002EA8 RID: 11944
		[Tooltip("The world plane to map the 2d input onto.")]
		[RequiredField]
		public TransformInputToWorldSpace.AxisPlane mapToPlane;

		// Token: 0x04002EA9 RID: 11945
		[Tooltip("Make the result relative to a GameObject, typically the main camera.")]
		public FsmGameObject relativeTo;

		// Token: 0x04002EAA RID: 11946
		[Tooltip("Store the direction vector.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeVector;

		// Token: 0x04002EAB RID: 11947
		[Tooltip("Store the length of the direction vector.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeMagnitude;

		// Token: 0x02000B38 RID: 2872
		public enum AxisPlane
		{
			// Token: 0x04002EAD RID: 11949
			XZ,
			// Token: 0x04002EAE RID: 11950
			XY,
			// Token: 0x04002EAF RID: 11951
			YZ
		}
	}
}
