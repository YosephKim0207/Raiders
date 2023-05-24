using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009BC RID: 2492
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Get the XYZ channels of a Vector3 Variable and store them in Float Variables.")]
	public class GetVector3XYZ : FsmStateAction
	{
		// Token: 0x060035EB RID: 13803 RVA: 0x001147C0 File Offset: 0x001129C0
		public override void Reset()
		{
			this.vector3Variable = null;
			this.storeX = null;
			this.storeY = null;
			this.storeZ = null;
			this.everyFrame = false;
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001147E8 File Offset: 0x001129E8
		public override void OnEnter()
		{
			this.DoGetVector3XYZ();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x00114804 File Offset: 0x00112A04
		public override void OnUpdate()
		{
			this.DoGetVector3XYZ();
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x0011480C File Offset: 0x00112A0C
		private void DoGetVector3XYZ()
		{
			if (this.vector3Variable == null)
			{
				return;
			}
			if (this.storeX != null)
			{
				this.storeX.Value = this.vector3Variable.Value.x;
			}
			if (this.storeY != null)
			{
				this.storeY.Value = this.vector3Variable.Value.y;
			}
			if (this.storeZ != null)
			{
				this.storeZ.Value = this.vector3Variable.Value.z;
			}
		}

		// Token: 0x04002735 RID: 10037
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		// Token: 0x04002736 RID: 10038
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		// Token: 0x04002737 RID: 10039
		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		// Token: 0x04002738 RID: 10040
		[UIHint(UIHint.Variable)]
		public FsmFloat storeZ;

		// Token: 0x04002739 RID: 10041
		public bool everyFrame;
	}
}
