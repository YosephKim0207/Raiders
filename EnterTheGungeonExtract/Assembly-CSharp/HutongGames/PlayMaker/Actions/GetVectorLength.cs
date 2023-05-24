using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009BD RID: 2493
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Get Vector3 Length.")]
	public class GetVectorLength : FsmStateAction
	{
		// Token: 0x060035F0 RID: 13808 RVA: 0x001148A8 File Offset: 0x00112AA8
		public override void Reset()
		{
			this.vector3 = null;
			this.storeLength = null;
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x001148B8 File Offset: 0x00112AB8
		public override void OnEnter()
		{
			this.DoVectorLength();
			base.Finish();
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x001148C8 File Offset: 0x00112AC8
		private void DoVectorLength()
		{
			if (this.vector3 == null)
			{
				return;
			}
			if (this.storeLength == null)
			{
				return;
			}
			this.storeLength.Value = this.vector3.Value.magnitude;
		}

		// Token: 0x0400273A RID: 10042
		public FsmVector3 vector3;

		// Token: 0x0400273B RID: 10043
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeLength;
	}
}
