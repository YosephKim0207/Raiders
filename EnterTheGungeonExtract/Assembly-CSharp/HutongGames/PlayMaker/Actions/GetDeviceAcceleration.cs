using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000975 RID: 2421
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Gets the last measured linear acceleration of a device and stores it in a Vector3 Variable.")]
	public class GetDeviceAcceleration : FsmStateAction
	{
		// Token: 0x060034B6 RID: 13494 RVA: 0x00111078 File Offset: 0x0010F278
		public override void Reset()
		{
			this.storeVector = null;
			this.storeX = null;
			this.storeY = null;
			this.storeZ = null;
			this.multiplier = 1f;
			this.everyFrame = false;
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x001110B0 File Offset: 0x0010F2B0
		public override void OnEnter()
		{
			this.DoGetDeviceAcceleration();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x001110CC File Offset: 0x0010F2CC
		public override void OnUpdate()
		{
			this.DoGetDeviceAcceleration();
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x001110D4 File Offset: 0x0010F2D4
		private void DoGetDeviceAcceleration()
		{
			Vector3 vector = new Vector3(Input.acceleration.x, Input.acceleration.y, Input.acceleration.z);
			if (!this.multiplier.IsNone)
			{
				vector *= this.multiplier.Value;
			}
			this.storeVector.Value = vector;
			this.storeX.Value = vector.x;
			this.storeY.Value = vector.y;
			this.storeZ.Value = vector.z;
		}

		// Token: 0x040025EB RID: 9707
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector;

		// Token: 0x040025EC RID: 9708
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		// Token: 0x040025ED RID: 9709
		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		// Token: 0x040025EE RID: 9710
		[UIHint(UIHint.Variable)]
		public FsmFloat storeZ;

		// Token: 0x040025EF RID: 9711
		public FsmFloat multiplier;

		// Token: 0x040025F0 RID: 9712
		public bool everyFrame;
	}
}
