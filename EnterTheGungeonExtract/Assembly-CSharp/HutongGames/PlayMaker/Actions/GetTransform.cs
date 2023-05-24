using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009BA RID: 2490
	[Tooltip("Gets a Game Object's Transform and stores it in an Object Variable.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetTransform : FsmStateAction
	{
		// Token: 0x060035E2 RID: 13794 RVA: 0x001146A0 File Offset: 0x001128A0
		public override void Reset()
		{
			this.gameObject = new FsmGameObject
			{
				UseVariable = true
			};
			this.storeTransform = null;
			this.everyFrame = false;
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x001146D0 File Offset: 0x001128D0
		public override void OnEnter()
		{
			this.DoGetGameObjectName();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x001146EC File Offset: 0x001128EC
		public override void OnUpdate()
		{
			this.DoGetGameObjectName();
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001146F4 File Offset: 0x001128F4
		private void DoGetGameObjectName()
		{
			GameObject value = this.gameObject.Value;
			this.storeTransform.Value = ((!(value != null)) ? null : value.transform);
		}

		// Token: 0x04002730 RID: 10032
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x04002731 RID: 10033
		[ObjectType(typeof(Transform))]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmObject storeTransform;

		// Token: 0x04002732 RID: 10034
		public bool everyFrame;
	}
}
