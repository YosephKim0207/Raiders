using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AAC RID: 2732
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Rotates a Game Object around each Axis. Use a Vector3 Variable and/or XYZ components. To leave any axis unchanged, set variable to 'None'.")]
	public class Rotate : FsmStateAction
	{
		// Token: 0x060039EC RID: 14828 RVA: 0x00127520 File Offset: 0x00125720
		public override void Reset()
		{
			this.gameObject = null;
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
			this.space = Space.Self;
			this.perSecond = false;
			this.everyFrame = true;
			this.lateUpdate = false;
			this.fixedUpdate = false;
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x0012759C File Offset: 0x0012579C
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001275AC File Offset: 0x001257AC
		public override void OnEnter()
		{
			if (!this.everyFrame && !this.lateUpdate && !this.fixedUpdate)
			{
				this.DoRotate();
				base.Finish();
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x001275DC File Offset: 0x001257DC
		public override void OnUpdate()
		{
			if (!this.lateUpdate && !this.fixedUpdate)
			{
				this.DoRotate();
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x001275FC File Offset: 0x001257FC
		public override void OnLateUpdate()
		{
			if (this.lateUpdate)
			{
				this.DoRotate();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x00127620 File Offset: 0x00125820
		public override void OnFixedUpdate()
		{
			if (this.fixedUpdate)
			{
				this.DoRotate();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x00127644 File Offset: 0x00125844
		private void DoRotate()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((!this.vector.IsNone) ? this.vector.Value : new Vector3(this.xAngle.Value, this.yAngle.Value, this.zAngle.Value));
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
			if (!this.perSecond)
			{
				ownerDefaultTarget.transform.Rotate(vector, this.space);
			}
			else
			{
				ownerDefaultTarget.transform.Rotate(vector * Time.deltaTime, this.space);
			}
		}

		// Token: 0x04002C27 RID: 11303
		[RequiredField]
		[Tooltip("The game object to rotate.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C28 RID: 11304
		[UIHint(UIHint.Variable)]
		[Tooltip("A rotation vector. NOTE: You can override individual axis below.")]
		public FsmVector3 vector;

		// Token: 0x04002C29 RID: 11305
		[Tooltip("Rotation around x axis.")]
		public FsmFloat xAngle;

		// Token: 0x04002C2A RID: 11306
		[Tooltip("Rotation around y axis.")]
		public FsmFloat yAngle;

		// Token: 0x04002C2B RID: 11307
		[Tooltip("Rotation around z axis.")]
		public FsmFloat zAngle;

		// Token: 0x04002C2C RID: 11308
		[Tooltip("Rotate in local or world space.")]
		public Space space;

		// Token: 0x04002C2D RID: 11309
		[Tooltip("Rotate over one second")]
		public bool perSecond;

		// Token: 0x04002C2E RID: 11310
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002C2F RID: 11311
		[Tooltip("Perform the rotation in LateUpdate. This is useful if you want to override the rotation of objects that are animated or otherwise rotated in Update.")]
		public bool lateUpdate;

		// Token: 0x04002C30 RID: 11312
		[Tooltip("Perform the rotation in FixedUpdate. This is useful when working with rigid bodies and physics.")]
		public bool fixedUpdate;
	}
}
