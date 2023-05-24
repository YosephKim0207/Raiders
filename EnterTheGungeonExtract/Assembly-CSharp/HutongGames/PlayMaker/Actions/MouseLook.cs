using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A19 RID: 2585
	[Tooltip("Rotates a GameObject based on mouse movement. Minimum and Maximum values can be used to constrain the rotation.")]
	[ActionCategory(ActionCategory.Input)]
	public class MouseLook : FsmStateAction
	{
		// Token: 0x06003760 RID: 14176 RVA: 0x0011D5E8 File Offset: 0x0011B7E8
		public override void Reset()
		{
			this.gameObject = null;
			this.axes = MouseLook.RotationAxes.MouseXAndY;
			this.sensitivityX = 15f;
			this.sensitivityY = 15f;
			this.minimumX = new FsmFloat
			{
				UseVariable = true
			};
			this.maximumX = new FsmFloat
			{
				UseVariable = true
			};
			this.minimumY = -60f;
			this.maximumY = 60f;
			this.everyFrame = true;
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0011D674 File Offset: 0x0011B874
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			Rigidbody component = ownerDefaultTarget.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.freezeRotation = true;
			}
			this.rotationX = ownerDefaultTarget.transform.localRotation.eulerAngles.y;
			this.rotationY = ownerDefaultTarget.transform.localRotation.eulerAngles.x;
			this.DoMouseLook();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x0011D71C File Offset: 0x0011B91C
		public override void OnUpdate()
		{
			this.DoMouseLook();
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x0011D724 File Offset: 0x0011B924
		private void DoMouseLook()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Transform transform = ownerDefaultTarget.transform;
			MouseLook.RotationAxes rotationAxes = this.axes;
			if (rotationAxes != MouseLook.RotationAxes.MouseXAndY)
			{
				if (rotationAxes != MouseLook.RotationAxes.MouseX)
				{
					if (rotationAxes == MouseLook.RotationAxes.MouseY)
					{
						transform.localEulerAngles = new Vector3(-this.GetYRotation(), transform.localEulerAngles.y, 0f);
					}
				}
				else
				{
					transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, this.GetXRotation(), 0f);
				}
			}
			else
			{
				transform.localEulerAngles = new Vector3(this.GetYRotation(), this.GetXRotation(), 0f);
			}
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x0011D7EC File Offset: 0x0011B9EC
		private float GetXRotation()
		{
			this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX.Value;
			this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			return this.rotationX;
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x0011D840 File Offset: 0x0011BA40
		private float GetYRotation()
		{
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY.Value;
			this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
			return this.rotationY;
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x0011D894 File Offset: 0x0011BA94
		private static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
		{
			if (!min.IsNone && angle < min.Value)
			{
				angle = min.Value;
			}
			if (!max.IsNone && angle > max.Value)
			{
				angle = max.Value;
			}
			return angle;
		}

		// Token: 0x0400293C RID: 10556
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400293D RID: 10557
		[Tooltip("The axes to rotate around.")]
		public MouseLook.RotationAxes axes;

		// Token: 0x0400293E RID: 10558
		[Tooltip("Sensitivity of movement in X direction.")]
		[RequiredField]
		public FsmFloat sensitivityX;

		// Token: 0x0400293F RID: 10559
		[Tooltip("Sensitivity of movement in Y direction.")]
		[RequiredField]
		public FsmFloat sensitivityY;

		// Token: 0x04002940 RID: 10560
		[Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
		[HasFloatSlider(-360f, 360f)]
		public FsmFloat minimumX;

		// Token: 0x04002941 RID: 10561
		[Tooltip("Clamp rotation around X axis. Set to None for no clamping.")]
		[HasFloatSlider(-360f, 360f)]
		public FsmFloat maximumX;

		// Token: 0x04002942 RID: 10562
		[Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
		[HasFloatSlider(-360f, 360f)]
		public FsmFloat minimumY;

		// Token: 0x04002943 RID: 10563
		[Tooltip("Clamp rotation around Y axis. Set to None for no clamping.")]
		[HasFloatSlider(-360f, 360f)]
		public FsmFloat maximumY;

		// Token: 0x04002944 RID: 10564
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002945 RID: 10565
		private float rotationX;

		// Token: 0x04002946 RID: 10566
		private float rotationY;

		// Token: 0x02000A1A RID: 2586
		public enum RotationAxes
		{
			// Token: 0x04002948 RID: 10568
			MouseXAndY,
			// Token: 0x04002949 RID: 10569
			MouseX,
			// Token: 0x0400294A RID: 10570
			MouseY
		}
	}
}
