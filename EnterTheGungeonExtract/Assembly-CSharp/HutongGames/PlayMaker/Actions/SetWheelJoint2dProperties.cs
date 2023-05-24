using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A72 RID: 2674
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Sets the various properties of a WheelJoint2d component")]
	public class SetWheelJoint2dProperties : FsmStateAction
	{
		// Token: 0x060038DB RID: 14555 RVA: 0x00123A84 File Offset: 0x00121C84
		public override void Reset()
		{
			this.useMotor = new FsmBool
			{
				UseVariable = true
			};
			this.motorSpeed = new FsmFloat
			{
				UseVariable = true
			};
			this.maxMotorTorque = new FsmFloat
			{
				UseVariable = true
			};
			this.angle = new FsmFloat
			{
				UseVariable = true
			};
			this.dampingRatio = new FsmFloat
			{
				UseVariable = true
			};
			this.frequency = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x00123B10 File Offset: 0x00121D10
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				this._wj2d = ownerDefaultTarget.GetComponent<WheelJoint2D>();
				if (this._wj2d != null)
				{
					this._motor = this._wj2d.motor;
					this._suspension = this._wj2d.suspension;
				}
			}
			this.SetProperties();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x00123B94 File Offset: 0x00121D94
		public override void OnUpdate()
		{
			this.SetProperties();
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x00123B9C File Offset: 0x00121D9C
		private void SetProperties()
		{
			if (this._wj2d == null)
			{
				return;
			}
			if (!this.useMotor.IsNone)
			{
				this._wj2d.useMotor = this.useMotor.Value;
			}
			if (!this.motorSpeed.IsNone)
			{
				this._motor.motorSpeed = this.motorSpeed.Value;
				this._wj2d.motor = this._motor;
			}
			if (!this.maxMotorTorque.IsNone)
			{
				this._motor.maxMotorTorque = this.maxMotorTorque.Value;
				this._wj2d.motor = this._motor;
			}
			if (!this.angle.IsNone)
			{
				this._suspension.angle = this.angle.Value;
				this._wj2d.suspension = this._suspension;
			}
			if (!this.dampingRatio.IsNone)
			{
				this._suspension.dampingRatio = this.dampingRatio.Value;
				this._wj2d.suspension = this._suspension;
			}
			if (!this.frequency.IsNone)
			{
				this._suspension.frequency = this.frequency.Value;
				this._wj2d.suspension = this._suspension;
			}
		}

		// Token: 0x04002B2E RID: 11054
		[CheckForComponent(typeof(WheelJoint2D))]
		[Tooltip("The WheelJoint2d target")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B2F RID: 11055
		[Tooltip("Should a motor force be applied automatically to the Rigidbody2D?")]
		[ActionSection("Motor")]
		public FsmBool useMotor;

		// Token: 0x04002B30 RID: 11056
		[Tooltip("The desired speed for the Rigidbody2D to reach as it moves with the joint.")]
		public FsmFloat motorSpeed;

		// Token: 0x04002B31 RID: 11057
		[Tooltip("The maximum force that can be applied to the Rigidbody2D at the joint to attain the target speed.")]
		public FsmFloat maxMotorTorque;

		// Token: 0x04002B32 RID: 11058
		[ActionSection("Suspension")]
		[Tooltip("The world angle along which the suspension will move. This provides 2D constrained motion similar to a SliderJoint2D. This is typically how suspension works in the real world.")]
		public FsmFloat angle;

		// Token: 0x04002B33 RID: 11059
		[Tooltip("The amount by which the suspension spring force is reduced in proportion to the movement speed.")]
		public FsmFloat dampingRatio;

		// Token: 0x04002B34 RID: 11060
		[Tooltip("The frequency at which the suspension spring oscillates.")]
		public FsmFloat frequency;

		// Token: 0x04002B35 RID: 11061
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x04002B36 RID: 11062
		private WheelJoint2D _wj2d;

		// Token: 0x04002B37 RID: 11063
		private JointMotor2D _motor;

		// Token: 0x04002B38 RID: 11064
		private JointSuspension2D _suspension;
	}
}
