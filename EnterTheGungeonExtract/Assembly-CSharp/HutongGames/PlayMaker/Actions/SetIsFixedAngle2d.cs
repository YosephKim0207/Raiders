using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A6E RID: 2670
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Controls whether the rigidbody 2D should be prevented from rotating")]
	[Obsolete("This action is obsolete; use Constraints instead.")]
	public class SetIsFixedAngle2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x060038C8 RID: 14536 RVA: 0x00123794 File Offset: 0x00121994
		public override void Reset()
		{
			this.gameObject = null;
			this.isFixedAngle = false;
			this.everyFrame = false;
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x001237B0 File Offset: 0x001219B0
		public override void OnEnter()
		{
			this.DoSetIsFixedAngle();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x001237CC File Offset: 0x001219CC
		public override void OnUpdate()
		{
			this.DoSetIsFixedAngle();
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x001237D4 File Offset: 0x001219D4
		private void DoSetIsFixedAngle()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			if (this.isFixedAngle.Value)
			{
				base.rigidbody2d.constraints = base.rigidbody2d.constraints | RigidbodyConstraints2D.FreezeRotation;
			}
			else
			{
				base.rigidbody2d.constraints = base.rigidbody2d.constraints & ~RigidbodyConstraints2D.FreezeRotation;
			}
		}

		// Token: 0x04002B22 RID: 11042
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B23 RID: 11043
		[Tooltip("The flag value")]
		[RequiredField]
		public FsmBool isFixedAngle;

		// Token: 0x04002B24 RID: 11044
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
