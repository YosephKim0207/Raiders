using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A1 RID: 2465
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Gets the Position of a Game Object and stores it in a Vector3 Variable or each Axis in a Float Variable")]
	public class GetPosition : FsmStateAction
	{
		// Token: 0x06003576 RID: 13686 RVA: 0x00113404 File Offset: 0x00111604
		public override void Reset()
		{
			this.gameObject = null;
			this.vector = null;
			this.x = null;
			this.y = null;
			this.z = null;
			this.space = Space.World;
			this.everyFrame = false;
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x00113438 File Offset: 0x00111638
		public override void OnEnter()
		{
			this.DoGetPosition();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x00113454 File Offset: 0x00111654
		public override void OnUpdate()
		{
			this.DoGetPosition();
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x0011345C File Offset: 0x0011165C
		private void DoGetPosition()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((this.space != Space.World) ? ownerDefaultTarget.transform.localPosition : ownerDefaultTarget.transform.position);
			this.vector.Value = vector;
			this.x.Value = vector.x;
			this.y.Value = vector.y;
			this.z.Value = vector.z;
		}

		// Token: 0x040026C8 RID: 9928
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026C9 RID: 9929
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x040026CA RID: 9930
		[UIHint(UIHint.Variable)]
		public FsmFloat x;

		// Token: 0x040026CB RID: 9931
		[UIHint(UIHint.Variable)]
		public FsmFloat y;

		// Token: 0x040026CC RID: 9932
		[UIHint(UIHint.Variable)]
		public FsmFloat z;

		// Token: 0x040026CD RID: 9933
		public Space space;

		// Token: 0x040026CE RID: 9934
		public bool everyFrame;
	}
}
