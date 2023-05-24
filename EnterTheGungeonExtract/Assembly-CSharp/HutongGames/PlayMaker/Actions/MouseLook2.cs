using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A1B RID: 2587
	[Tooltip("Rotates a GameObject based on mouse movement. Minimum and Maximum values can be used to constrain the rotation.")]
	[ActionCategory(ActionCategory.Input)]
	public class MouseLook2 : ComponentAction<Rigidbody>
	{
		// Token: 0x06003768 RID: 14184 RVA: 0x0011D8E8 File Offset: 0x0011BAE8
		public override void Reset()
		{
			this.gameObject = null;
			this.axes = MouseLook2.RotationAxes.MouseXAndY;
			this.sensitivityX = 15f;
			this.sensitivityY = 15f;
			this.minimumX = -360f;
			this.maximumX = 360f;
			this.minimumY = -60f;
			this.maximumY = 60f;
			this.everyFrame = true;
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x0011D96C File Offset: 0x0011BB6C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			if (!base.UpdateCache(ownerDefaultTarget) && base.rigidbody)
			{
				base.rigidbody.freezeRotation = true;
			}
			this.DoMouseLook();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x0011D9E0 File Offset: 0x0011BBE0
		public override void OnUpdate()
		{
			this.DoMouseLook();
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x0011D9E8 File Offset: 0x0011BBE8
		private void DoMouseLook()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Transform transform = ownerDefaultTarget.transform;
			MouseLook2.RotationAxes rotationAxes = this.axes;
			if (rotationAxes != MouseLook2.RotationAxes.MouseXAndY)
			{
				if (rotationAxes != MouseLook2.RotationAxes.MouseX)
				{
					if (rotationAxes == MouseLook2.RotationAxes.MouseY)
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

		// Token: 0x0600376C RID: 14188 RVA: 0x0011DAB0 File Offset: 0x0011BCB0
		private float GetXRotation()
		{
			this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX.Value;
			this.rotationX = MouseLook2.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			return this.rotationX;
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x0011DB04 File Offset: 0x0011BD04
		private float GetYRotation()
		{
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY.Value;
			this.rotationY = MouseLook2.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
			return this.rotationY;
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x0011DB58 File Offset: 0x0011BD58
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

		// Token: 0x0400294B RID: 10571
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400294C RID: 10572
		[Tooltip("The axes to rotate around.")]
		public MouseLook2.RotationAxes axes;

		// Token: 0x0400294D RID: 10573
		[RequiredField]
		public FsmFloat sensitivityX;

		// Token: 0x0400294E RID: 10574
		[RequiredField]
		public FsmFloat sensitivityY;

		// Token: 0x0400294F RID: 10575
		[HasFloatSlider(-360f, 360f)]
		[RequiredField]
		public FsmFloat minimumX;

		// Token: 0x04002950 RID: 10576
		[HasFloatSlider(-360f, 360f)]
		[RequiredField]
		public FsmFloat maximumX;

		// Token: 0x04002951 RID: 10577
		[HasFloatSlider(-360f, 360f)]
		[RequiredField]
		public FsmFloat minimumY;

		// Token: 0x04002952 RID: 10578
		[HasFloatSlider(-360f, 360f)]
		[RequiredField]
		public FsmFloat maximumY;

		// Token: 0x04002953 RID: 10579
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002954 RID: 10580
		private float rotationX;

		// Token: 0x04002955 RID: 10581
		private float rotationY;

		// Token: 0x02000A1C RID: 2588
		public enum RotationAxes
		{
			// Token: 0x04002957 RID: 10583
			MouseXAndY,
			// Token: 0x04002958 RID: 10584
			MouseX,
			// Token: 0x04002959 RID: 10585
			MouseY
		}
	}
}
