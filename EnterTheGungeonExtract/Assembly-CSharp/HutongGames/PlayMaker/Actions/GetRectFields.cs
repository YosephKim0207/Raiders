using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A8 RID: 2472
	[Tooltip("Get the individual fields of a Rect Variable and store them in Float Variables.")]
	[ActionCategory(ActionCategory.Rect)]
	public class GetRectFields : FsmStateAction
	{
		// Token: 0x06003595 RID: 13717 RVA: 0x00113958 File Offset: 0x00111B58
		public override void Reset()
		{
			this.rectVariable = null;
			this.storeX = null;
			this.storeY = null;
			this.storeWidth = null;
			this.storeHeight = null;
			this.everyFrame = false;
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x00113984 File Offset: 0x00111B84
		public override void OnEnter()
		{
			this.DoGetRectFields();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x001139A0 File Offset: 0x00111BA0
		public override void OnUpdate()
		{
			this.DoGetRectFields();
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x001139A8 File Offset: 0x00111BA8
		private void DoGetRectFields()
		{
			if (this.rectVariable.IsNone)
			{
				return;
			}
			this.storeX.Value = this.rectVariable.Value.x;
			this.storeY.Value = this.rectVariable.Value.y;
			this.storeWidth.Value = this.rectVariable.Value.width;
			this.storeHeight.Value = this.rectVariable.Value.height;
		}

		// Token: 0x040026E1 RID: 9953
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmRect rectVariable;

		// Token: 0x040026E2 RID: 9954
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		// Token: 0x040026E3 RID: 9955
		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		// Token: 0x040026E4 RID: 9956
		[UIHint(UIHint.Variable)]
		public FsmFloat storeWidth;

		// Token: 0x040026E5 RID: 9957
		[UIHint(UIHint.Variable)]
		public FsmFloat storeHeight;

		// Token: 0x040026E6 RID: 9958
		public bool everyFrame;
	}
}
