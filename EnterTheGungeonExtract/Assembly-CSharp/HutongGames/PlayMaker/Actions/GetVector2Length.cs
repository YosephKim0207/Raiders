using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B49 RID: 2889
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Get Vector2 Length.")]
	public class GetVector2Length : FsmStateAction
	{
		// Token: 0x06003CA7 RID: 15527 RVA: 0x001309FC File Offset: 0x0012EBFC
		public override void Reset()
		{
			this.vector2 = null;
			this.storeLength = null;
			this.everyFrame = false;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00130A14 File Offset: 0x0012EC14
		public override void OnEnter()
		{
			this.DoVectorLength();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00130A30 File Offset: 0x0012EC30
		public override void OnUpdate()
		{
			this.DoVectorLength();
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00130A38 File Offset: 0x0012EC38
		private void DoVectorLength()
		{
			if (this.vector2 == null)
			{
				return;
			}
			if (this.storeLength == null)
			{
				return;
			}
			this.storeLength.Value = this.vector2.Value.magnitude;
		}

		// Token: 0x04002EF2 RID: 12018
		[Tooltip("The Vector2 to get the length from")]
		public FsmVector2 vector2;

		// Token: 0x04002EF3 RID: 12019
		[RequiredField]
		[Tooltip("The Vector2 the length")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeLength;

		// Token: 0x04002EF4 RID: 12020
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
