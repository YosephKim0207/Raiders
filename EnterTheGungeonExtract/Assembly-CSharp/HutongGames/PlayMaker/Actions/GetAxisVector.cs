using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000968 RID: 2408
	[NoActionTargets]
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets a world direction Vector from 2 Input Axis. Typically used for a third person controller with Relative To set to the camera.")]
	public class GetAxisVector : FsmStateAction
	{
		// Token: 0x06003484 RID: 13444 RVA: 0x001104D0 File Offset: 0x0010E6D0
		public override void Reset()
		{
			this.horizontalAxis = "Horizontal";
			this.verticalAxis = "Vertical";
			this.multiplier = 1f;
			this.mapToPlane = GetAxisVector.AxisPlane.XZ;
			this.storeVector = null;
			this.storeMagnitude = null;
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x00110524 File Offset: 0x0010E724
		public override void OnUpdate()
		{
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			if (this.relativeTo.Value == null)
			{
				GetAxisVector.AxisPlane axisPlane = this.mapToPlane;
				if (axisPlane != GetAxisVector.AxisPlane.XZ)
				{
					if (axisPlane != GetAxisVector.AxisPlane.XY)
					{
						if (axisPlane == GetAxisVector.AxisPlane.YZ)
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
				GetAxisVector.AxisPlane axisPlane2 = this.mapToPlane;
				if (axisPlane2 != GetAxisVector.AxisPlane.XZ)
				{
					if (axisPlane2 == GetAxisVector.AxisPlane.XY || axisPlane2 == GetAxisVector.AxisPlane.YZ)
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
			float num = ((!this.horizontalAxis.IsNone && !string.IsNullOrEmpty(this.horizontalAxis.Value)) ? Input.GetAxis(this.horizontalAxis.Value) : 0f);
			float num2 = ((!this.verticalAxis.IsNone && !string.IsNullOrEmpty(this.verticalAxis.Value)) ? Input.GetAxis(this.verticalAxis.Value) : 0f);
			Vector3 vector3 = num * vector2 + num2 * vector;
			vector3 *= this.multiplier.Value;
			this.storeVector.Value = vector3;
			if (!this.storeMagnitude.IsNone)
			{
				this.storeMagnitude.Value = vector3.magnitude;
			}
		}

		// Token: 0x040025B1 RID: 9649
		[Tooltip("The name of the horizontal input axis. See Unity Input Manager.")]
		public FsmString horizontalAxis;

		// Token: 0x040025B2 RID: 9650
		[Tooltip("The name of the vertical input axis. See Unity Input Manager.")]
		public FsmString verticalAxis;

		// Token: 0x040025B3 RID: 9651
		[Tooltip("Input axis are reported in the range -1 to 1, this multiplier lets you set a new range.")]
		public FsmFloat multiplier;

		// Token: 0x040025B4 RID: 9652
		[RequiredField]
		[Tooltip("The world plane to map the 2d input onto.")]
		public GetAxisVector.AxisPlane mapToPlane;

		// Token: 0x040025B5 RID: 9653
		[Tooltip("Make the result relative to a GameObject, typically the main camera.")]
		public FsmGameObject relativeTo;

		// Token: 0x040025B6 RID: 9654
		[RequiredField]
		[Tooltip("Store the direction vector.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector;

		// Token: 0x040025B7 RID: 9655
		[Tooltip("Store the length of the direction vector.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeMagnitude;

		// Token: 0x02000969 RID: 2409
		public enum AxisPlane
		{
			// Token: 0x040025B9 RID: 9657
			XZ,
			// Token: 0x040025BA RID: 9658
			XY,
			// Token: 0x040025BB RID: 9659
			YZ
		}
	}
}
