using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D9 RID: 2265
	[Tooltip("Add an item to the end of an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayAdd : FsmStateAction
	{
		// Token: 0x0600322C RID: 12844 RVA: 0x001084B8 File Offset: 0x001066B8
		public override void Reset()
		{
			this.array = null;
			this.value = null;
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x001084C8 File Offset: 0x001066C8
		public override void OnEnter()
		{
			this.DoAddValue();
			base.Finish();
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x001084D8 File Offset: 0x001066D8
		private void DoAddValue()
		{
			this.array.Resize(this.array.Length + 1);
			this.value.UpdateValue();
			this.array.Set(this.array.Length - 1, this.value.GetValue());
		}

		// Token: 0x04002349 RID: 9033
		[Tooltip("The Array Variable to use.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		// Token: 0x0400234A RID: 9034
		[Tooltip("Item to add.")]
		[RequiredField]
		[MatchElementType("array")]
		public FsmVar value;
	}
}
