using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B0B RID: 2827
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Sets the Position of a Game Object. To leave any axis unchanged, set variable to 'None'.")]
	public class SetPosition : FsmStateAction
	{
		// Token: 0x06003B9D RID: 15261 RVA: 0x0012C9B0 File Offset: 0x0012ABB0
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
			this.space = Space.Self;
			this.everyFrame = false;
			this.lateUpdate = false;
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x0012CA1C File Offset: 0x0012AC1C
		public override void OnEnter()
		{
			if (!this.everyFrame && !this.lateUpdate)
			{
				this.DoSetPosition();
				base.Finish();
			}
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x0012CA40 File Offset: 0x0012AC40
		public override void OnUpdate()
		{
			if (!this.lateUpdate)
			{
				this.DoSetPosition();
			}
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0012CA54 File Offset: 0x0012AC54
		public override void OnLateUpdate()
		{
			if (this.lateUpdate)
			{
				this.DoSetPosition();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x0012CA78 File Offset: 0x0012AC78
		private void DoSetPosition()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector;
			if (this.vector.IsNone)
			{
				vector = ((this.space != Space.World) ? ownerDefaultTarget.transform.localPosition : ownerDefaultTarget.transform.position);
			}
			else
			{
				vector = this.vector.Value;
			}
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
			if (this.space == Space.World)
			{
				ownerDefaultTarget.transform.position = vector;
			}
			else
			{
				ownerDefaultTarget.transform.localPosition = vector;
			}
		}

		// Token: 0x04002DBE RID: 11710
		[Tooltip("The GameObject to position.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DBF RID: 11711
		[Tooltip("Use a stored Vector3 position, and/or set individual axis below.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x04002DC0 RID: 11712
		public FsmFloat x;

		// Token: 0x04002DC1 RID: 11713
		public FsmFloat y;

		// Token: 0x04002DC2 RID: 11714
		public FsmFloat z;

		// Token: 0x04002DC3 RID: 11715
		[Tooltip("Use local or world space.")]
		public Space space;

		// Token: 0x04002DC4 RID: 11716
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002DC5 RID: 11717
		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;
	}
}
