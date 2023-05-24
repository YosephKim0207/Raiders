using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A5 RID: 2469
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets a Random Game Object from the scene.\nOptionally filter by Tag.")]
	public class GetRandomObject : FsmStateAction
	{
		// Token: 0x06003586 RID: 13702 RVA: 0x00113638 File Offset: 0x00111838
		public override void Reset()
		{
			this.withTag = "Untagged";
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x00113658 File Offset: 0x00111858
		public override void OnEnter()
		{
			this.DoGetRandomObject();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x00113674 File Offset: 0x00111874
		public override void OnUpdate()
		{
			this.DoGetRandomObject();
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x0011367C File Offset: 0x0011187C
		private void DoGetRandomObject()
		{
			GameObject[] array;
			if (this.withTag.Value != "Untagged")
			{
				array = GameObject.FindGameObjectsWithTag(this.withTag.Value);
			}
			else
			{
				array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			}
			if (array.Length > 0)
			{
				this.storeResult.Value = array[UnityEngine.Random.Range(0, array.Length)];
				return;
			}
			this.storeResult.Value = null;
		}

		// Token: 0x040026D4 RID: 9940
		[UIHint(UIHint.Tag)]
		public FsmString withTag;

		// Token: 0x040026D5 RID: 9941
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject storeResult;

		// Token: 0x040026D6 RID: 9942
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
