using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B12 RID: 2834
	[Tooltip("Sets the Scale of a Game Object. To leave any axis unchanged, set variable to 'None'.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SetScale : FsmStateAction
	{
		// Token: 0x06003BBE RID: 15294 RVA: 0x0012D1EC File Offset: 0x0012B3EC
		public override void Reset()
		{
			this.gameObject = null;
			this.vector = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.z = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
			this.lateUpdate = false;
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x0012D254 File Offset: 0x0012B454
		public override void OnEnter()
		{
			this.DoSetScale();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x0012D270 File Offset: 0x0012B470
		public override void OnUpdate()
		{
			if (!this.lateUpdate)
			{
				this.DoSetScale();
			}
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x0012D284 File Offset: 0x0012B484
		public override void OnLateUpdate()
		{
			if (this.lateUpdate)
			{
				this.DoSetScale();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x0012D2A8 File Offset: 0x0012B4A8
		private void DoSetScale()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((!this.vector.IsNone) ? this.vector.Value : ownerDefaultTarget.transform.localScale);
			if (!this.x.IsNone)
			{
				vector.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				vector.y = this.y.Value;
			}
			if (!this.z.IsNone)
			{
				vector.z = this.z.Value;
			}
			ownerDefaultTarget.transform.localScale = vector;
		}

		// Token: 0x04002DE1 RID: 11745
		[Tooltip("The GameObject to scale.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DE2 RID: 11746
		[UIHint(UIHint.Variable)]
		[Tooltip("Use stored Vector3 value, and/or set each axis below.")]
		public FsmVector3 vector;

		// Token: 0x04002DE3 RID: 11747
		public FsmFloat x;

		// Token: 0x04002DE4 RID: 11748
		public FsmFloat y;

		// Token: 0x04002DE5 RID: 11749
		public FsmFloat z;

		// Token: 0x04002DE6 RID: 11750
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002DE7 RID: 11751
		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;
	}
}
