using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E6 RID: 2278
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Set the value at an index. Index must be between 0 and the number of items -1. First item is index 0.")]
	public class ArraySet : FsmStateAction
	{
		// Token: 0x06003265 RID: 12901 RVA: 0x00108F28 File Offset: 0x00107128
		public override void Reset()
		{
			this.array = null;
			this.index = null;
			this.value = null;
			this.everyFrame = false;
			this.indexOutOfRange = null;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x00108F50 File Offset: 0x00107150
		public override void OnEnter()
		{
			this.DoGetValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x00108F6C File Offset: 0x0010716C
		public override void OnUpdate()
		{
			this.DoGetValue();
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x00108F74 File Offset: 0x00107174
		private void DoGetValue()
		{
			if (this.array.IsNone)
			{
				return;
			}
			if (this.index.Value >= 0 && this.index.Value < this.array.Length)
			{
				this.value.UpdateValue();
				this.array.Set(this.index.Value, this.value.GetValue());
			}
			else
			{
				base.Fsm.Event(this.indexOutOfRange);
			}
		}

		// Token: 0x0400237A RID: 9082
		[UIHint(UIHint.Variable)]
		[Tooltip("The Array Variable to use.")]
		[RequiredField]
		public FsmArray array;

		// Token: 0x0400237B RID: 9083
		[Tooltip("The index into the array.")]
		public FsmInt index;

		// Token: 0x0400237C RID: 9084
		[MatchElementType("array")]
		[RequiredField]
		[Tooltip("Set the value of the array at the specified index.")]
		public FsmVar value;

		// Token: 0x0400237D RID: 9085
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x0400237E RID: 9086
		[ActionSection("Events")]
		[Tooltip("The event to trigger if the index is out of range")]
		public FsmEvent indexOutOfRange;
	}
}
