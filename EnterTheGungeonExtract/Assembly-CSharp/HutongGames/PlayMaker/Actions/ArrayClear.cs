using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008DB RID: 2267
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Sets all items in an Array to their default value: 0, empty string, false, or null depending on their type. Optionally defines a reset value to use.")]
	public class ArrayClear : FsmStateAction
	{
		// Token: 0x06003234 RID: 12852 RVA: 0x001085E0 File Offset: 0x001067E0
		public override void Reset()
		{
			this.array = null;
			this.resetValue = new FsmVar
			{
				useVariable = true
			};
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x00108608 File Offset: 0x00106808
		public override void OnEnter()
		{
			int length = this.array.Length;
			this.array.Reset();
			this.array.Resize(length);
			if (!this.resetValue.IsNone)
			{
				this.resetValue.UpdateValue();
				object value = this.resetValue.GetValue();
				for (int i = 0; i < length; i++)
				{
					this.array.Set(i, value);
				}
			}
			base.Finish();
		}

		// Token: 0x0400234D RID: 9037
		[Tooltip("The Array Variable to clear.")]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		// Token: 0x0400234E RID: 9038
		[Tooltip("Optional reset value. Leave as None for default value.")]
		[MatchElementType("array")]
		public FsmVar resetValue;
	}
}
