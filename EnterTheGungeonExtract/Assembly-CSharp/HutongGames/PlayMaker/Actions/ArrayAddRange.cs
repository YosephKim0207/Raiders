using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008DA RID: 2266
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Add values to an array.")]
	public class ArrayAddRange : FsmStateAction
	{
		// Token: 0x06003230 RID: 12848 RVA: 0x00108534 File Offset: 0x00106734
		public override void Reset()
		{
			this.array = null;
			this.variables = new FsmVar[2];
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x0010854C File Offset: 0x0010674C
		public override void OnEnter()
		{
			this.DoAddRange();
			base.Finish();
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x0010855C File Offset: 0x0010675C
		private void DoAddRange()
		{
			int num = this.variables.Length;
			if (num > 0)
			{
				this.array.Resize(this.array.Length + num);
				foreach (FsmVar fsmVar in this.variables)
				{
					fsmVar.UpdateValue();
					this.array.Set(this.array.Length - num, fsmVar.GetValue());
					num--;
				}
			}
		}

		// Token: 0x0400234B RID: 9035
		[Tooltip("The Array Variable to use.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		// Token: 0x0400234C RID: 9036
		[Tooltip("The variables to add.")]
		[RequiredField]
		[MatchElementType("array")]
		public FsmVar[] variables;
	}
}
