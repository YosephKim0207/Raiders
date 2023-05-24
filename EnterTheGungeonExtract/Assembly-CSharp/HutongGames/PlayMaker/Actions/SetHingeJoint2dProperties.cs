using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A6D RID: 2669
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Sets the various properties of a HingeJoint2d component")]
	public class SetHingeJoint2dProperties : FsmStateAction
	{
		// Token: 0x060038C3 RID: 14531 RVA: 0x0012352C File Offset: 0x0012172C
		public override void Reset()
		{
			this.useLimits = new FsmBool
			{
				UseVariable = true
			};
			this.min = new FsmFloat
			{
				UseVariable = true
			};
			this.max = new FsmFloat
			{
				UseVariable = true
			};
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
			this.everyFrame = false;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x001235B8 File Offset: 0x001217B8
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				this._joint = ownerDefaultTarget.GetComponent<HingeJoint2D>();
				if (this._joint != null)
				{
					this._motor = this._joint.motor;
					this._limits = this._joint.limits;
				}
			}
			this.SetProperties();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x0012363C File Offset: 0x0012183C
		public override void OnUpdate()
		{
			this.SetProperties();
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x00123644 File Offset: 0x00121844
		private void SetProperties()
		{
			if (this._joint == null)
			{
				return;
			}
			if (!this.useMotor.IsNone)
			{
				this._joint.useMotor = this.useMotor.Value;
			}
			if (!this.motorSpeed.IsNone)
			{
				this._motor.motorSpeed = this.motorSpeed.Value;
				this._joint.motor = this._motor;
			}
			if (!this.maxMotorTorque.IsNone)
			{
				this._motor.maxMotorTorque = this.maxMotorTorque.Value;
				this._joint.motor = this._motor;
			}
			if (!this.useLimits.IsNone)
			{
				this._joint.useLimits = this.useLimits.Value;
			}
			if (!this.min.IsNone)
			{
				this._limits.min = this.min.Value;
				this._joint.limits = this._limits;
			}
			if (!this.max.IsNone)
			{
				this._limits.max = this.max.Value;
				this._joint.limits = this._limits;
			}
		}

		// Token: 0x04002B17 RID: 11031
		[CheckForComponent(typeof(HingeJoint2D))]
		[Tooltip("The HingeJoint2d target")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B18 RID: 11032
		[Tooltip("Should limits be placed on the range of rotation?")]
		[ActionSection("Limits")]
		public FsmBool useLimits;

		// Token: 0x04002B19 RID: 11033
		[Tooltip("Lower angular limit of rotation.")]
		public FsmFloat min;

		// Token: 0x04002B1A RID: 11034
		[Tooltip("Upper angular limit of rotation")]
		public FsmFloat max;

		// Token: 0x04002B1B RID: 11035
		[Tooltip("Should a motor force be applied automatically to the Rigidbody2D?")]
		[ActionSection("Motor")]
		public FsmBool useMotor;

		// Token: 0x04002B1C RID: 11036
		[Tooltip("The desired speed for the Rigidbody2D to reach as it moves with the joint.")]
		public FsmFloat motorSpeed;

		// Token: 0x04002B1D RID: 11037
		[Tooltip("The maximum force that can be applied to the Rigidbody2D at the joint to attain the target speed.")]
		public FsmFloat maxMotorTorque;

		// Token: 0x04002B1E RID: 11038
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x04002B1F RID: 11039
		private HingeJoint2D _joint;

		// Token: 0x04002B20 RID: 11040
		private JointMotor2D _motor;

		// Token: 0x04002B21 RID: 11041
		private JointAngleLimits2D _limits;
	}
}
