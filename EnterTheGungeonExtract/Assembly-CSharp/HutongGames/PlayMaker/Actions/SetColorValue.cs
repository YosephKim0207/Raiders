using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ACE RID: 2766
	[Tooltip("Sets the value of a Color Variable.")]
	[ActionCategory(ActionCategory.Color)]
	public class SetColorValue : FsmStateAction
	{
		// Token: 0x06003A95 RID: 14997 RVA: 0x0012991C File Offset: 0x00127B1C
		public override void Reset()
		{
			this.colorVariable = null;
			this.color = null;
			this.everyFrame = false;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x00129934 File Offset: 0x00127B34
		public override void OnEnter()
		{
			this.DoSetColorValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x00129950 File Offset: 0x00127B50
		public override void OnUpdate()
		{
			this.DoSetColorValue();
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x00129958 File Offset: 0x00127B58
		private void DoSetColorValue()
		{
			if (this.colorVariable != null)
			{
				this.colorVariable.Value = this.color.Value;
			}
		}

		// Token: 0x04002CBB RID: 11451
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor colorVariable;

		// Token: 0x04002CBC RID: 11452
		[RequiredField]
		public FsmColor color;

		// Token: 0x04002CBD RID: 11453
		public bool everyFrame;
	}
}
