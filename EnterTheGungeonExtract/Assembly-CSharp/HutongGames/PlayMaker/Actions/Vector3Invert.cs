using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B63 RID: 2915
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Reverses the direction of a Vector3 Variable. Same as multiplying by -1.")]
	public class Vector3Invert : FsmStateAction
	{
		// Token: 0x06003D16 RID: 15638 RVA: 0x001320A4 File Offset: 0x001302A4
		public override void Reset()
		{
			this.vector3Variable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x001320B4 File Offset: 0x001302B4
		public override void OnEnter()
		{
			this.vector3Variable.Value = this.vector3Variable.Value * -1f;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x001320E8 File Offset: 0x001302E8
		public override void OnUpdate()
		{
			this.vector3Variable.Value = this.vector3Variable.Value * -1f;
		}

		// Token: 0x04002F63 RID: 12131
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F64 RID: 12132
		public bool everyFrame;
	}
}
