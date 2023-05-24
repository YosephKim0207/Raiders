using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E2 RID: 2274
	[Tooltip("Get a Random item from an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayGetRandom : FsmStateAction
	{
		// Token: 0x06003257 RID: 12887 RVA: 0x00108D7C File Offset: 0x00106F7C
		public override void Reset()
		{
			this.array = null;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x00108D94 File Offset: 0x00106F94
		public override void OnEnter()
		{
			this.DoGetRandomValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x00108DB0 File Offset: 0x00106FB0
		public override void OnUpdate()
		{
			this.DoGetRandomValue();
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x00108DB8 File Offset: 0x00106FB8
		private void DoGetRandomValue()
		{
			if (this.storeValue.IsNone)
			{
				return;
			}
			this.storeValue.SetValue(this.array.Get(UnityEngine.Random.Range(0, this.array.Length)));
		}

		// Token: 0x04002370 RID: 9072
		[RequiredField]
		[Tooltip("The Array to use.")]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		// Token: 0x04002371 RID: 9073
		[Tooltip("Store the value in a variable.")]
		[MatchElementType("array")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVar storeValue;

		// Token: 0x04002372 RID: 9074
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
