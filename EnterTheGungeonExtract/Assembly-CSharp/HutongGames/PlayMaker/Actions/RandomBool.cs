using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A9E RID: 2718
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets a Bool Variable to True or False randomly.")]
	public class RandomBool : FsmStateAction
	{
		// Token: 0x060039B5 RID: 14773 RVA: 0x0012669C File Offset: 0x0012489C
		public override void Reset()
		{
			this.storeResult = null;
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x001266A8 File Offset: 0x001248A8
		public override void OnEnter()
		{
			this.storeResult.Value = UnityEngine.Random.Range(0, 100) < 50;
			base.Finish();
		}

		// Token: 0x04002BDD RID: 11229
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
	}
}
