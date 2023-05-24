using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009AB RID: 2475
	[Tooltip("Gets the Scale of a Game Object and stores it in a Vector3 Variable or each Axis in a Float Variable")]
	[ActionCategory(ActionCategory.Transform)]
	public class GetScale : FsmStateAction
	{
		// Token: 0x060035A3 RID: 13731 RVA: 0x00113C20 File Offset: 0x00111E20
		public override void Reset()
		{
			this.gameObject = null;
			this.vector = null;
			this.xScale = null;
			this.yScale = null;
			this.zScale = null;
			this.space = Space.World;
			this.everyFrame = false;
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x00113C54 File Offset: 0x00111E54
		public override void OnEnter()
		{
			this.DoGetScale();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x00113C70 File Offset: 0x00111E70
		public override void OnUpdate()
		{
			this.DoGetScale();
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x00113C78 File Offset: 0x00111E78
		private void DoGetScale()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((this.space != Space.World) ? ownerDefaultTarget.transform.localScale : ownerDefaultTarget.transform.lossyScale);
			this.vector.Value = vector;
			this.xScale.Value = vector.x;
			this.yScale.Value = vector.y;
			this.zScale.Value = vector.z;
		}

		// Token: 0x040026F1 RID: 9969
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026F2 RID: 9970
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x040026F3 RID: 9971
		[UIHint(UIHint.Variable)]
		public FsmFloat xScale;

		// Token: 0x040026F4 RID: 9972
		[UIHint(UIHint.Variable)]
		public FsmFloat yScale;

		// Token: 0x040026F5 RID: 9973
		[UIHint(UIHint.Variable)]
		public FsmFloat zScale;

		// Token: 0x040026F6 RID: 9974
		public Space space;

		// Token: 0x040026F7 RID: 9975
		public bool everyFrame;
	}
}
