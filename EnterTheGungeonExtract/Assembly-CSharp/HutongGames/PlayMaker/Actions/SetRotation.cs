using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B11 RID: 2833
	[Tooltip("Sets the Rotation of a Game Object. To leave any axis unchanged, set variable to 'None'.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SetRotation : FsmStateAction
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x0012CFE4 File Offset: 0x0012B1E4
		public override void Reset()
		{
			this.gameObject = null;
			this.quaternion = null;
			this.vector = null;
			this.xAngle = new FsmFloat
			{
				UseVariable = true
			};
			this.yAngle = new FsmFloat
			{
				UseVariable = true
			};
			this.zAngle = new FsmFloat
			{
				UseVariable = true
			};
			this.space = Space.World;
			this.everyFrame = false;
			this.lateUpdate = false;
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x0012D058 File Offset: 0x0012B258
		public override void OnEnter()
		{
			if (!this.everyFrame && !this.lateUpdate)
			{
				this.DoSetRotation();
				base.Finish();
			}
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x0012D07C File Offset: 0x0012B27C
		public override void OnUpdate()
		{
			if (!this.lateUpdate)
			{
				this.DoSetRotation();
			}
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0012D090 File Offset: 0x0012B290
		public override void OnLateUpdate()
		{
			if (this.lateUpdate)
			{
				this.DoSetRotation();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0012D0B4 File Offset: 0x0012B2B4
		private void DoSetRotation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector;
			if (!this.quaternion.IsNone)
			{
				vector = this.quaternion.Value.eulerAngles;
			}
			else if (!this.vector.IsNone)
			{
				vector = this.vector.Value;
			}
			else
			{
				vector = ((this.space != Space.Self) ? ownerDefaultTarget.transform.eulerAngles : ownerDefaultTarget.transform.localEulerAngles);
			}
			if (!this.xAngle.IsNone)
			{
				vector.x = this.xAngle.Value;
			}
			if (!this.yAngle.IsNone)
			{
				vector.y = this.yAngle.Value;
			}
			if (!this.zAngle.IsNone)
			{
				vector.z = this.zAngle.Value;
			}
			if (this.space == Space.Self)
			{
				ownerDefaultTarget.transform.localEulerAngles = vector;
			}
			else
			{
				ownerDefaultTarget.transform.eulerAngles = vector;
			}
		}

		// Token: 0x04002DD8 RID: 11736
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DD9 RID: 11737
		[Tooltip("Use a stored quaternion, or vector angles below.")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion quaternion;

		// Token: 0x04002DDA RID: 11738
		[Tooltip("Use euler angles stored in a Vector3 variable, and/or set each axis below.")]
		[Title("Euler Angles")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x04002DDB RID: 11739
		public FsmFloat xAngle;

		// Token: 0x04002DDC RID: 11740
		public FsmFloat yAngle;

		// Token: 0x04002DDD RID: 11741
		public FsmFloat zAngle;

		// Token: 0x04002DDE RID: 11742
		[Tooltip("Use local or world space.")]
		public Space space;

		// Token: 0x04002DDF RID: 11743
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002DE0 RID: 11744
		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;
	}
}
