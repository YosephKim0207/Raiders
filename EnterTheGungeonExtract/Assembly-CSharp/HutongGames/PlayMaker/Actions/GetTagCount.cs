using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B5 RID: 2485
	[Tooltip("Gets the number of Game Objects in the scene with the specified Tag.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetTagCount : FsmStateAction
	{
		// Token: 0x060035D0 RID: 13776 RVA: 0x001141E8 File Offset: 0x001123E8
		public override void Reset()
		{
			this.tag = "Untagged";
			this.storeResult = null;
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x00114204 File Offset: 0x00112404
		public override void OnEnter()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.tag.Value);
			if (this.storeResult != null)
			{
				this.storeResult.Value = ((array == null) ? 0 : array.Length);
			}
			base.Finish();
		}

		// Token: 0x04002713 RID: 10003
		[UIHint(UIHint.Tag)]
		public FsmString tag;

		// Token: 0x04002714 RID: 10004
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt storeResult;
	}
}
