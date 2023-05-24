using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200099C RID: 2460
	[Tooltip("Gets the name of a Game Object and stores it in a String Variable.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetName : FsmStateAction
	{
		// Token: 0x06003563 RID: 13667 RVA: 0x00113134 File Offset: 0x00111334
		public override void Reset()
		{
			this.gameObject = new FsmGameObject
			{
				UseVariable = true
			};
			this.storeName = null;
			this.everyFrame = false;
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x00113164 File Offset: 0x00111364
		public override void OnEnter()
		{
			this.DoGetGameObjectName();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x00113180 File Offset: 0x00111380
		public override void OnUpdate()
		{
			this.DoGetGameObjectName();
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x00113188 File Offset: 0x00111388
		private void DoGetGameObjectName()
		{
			GameObject value = this.gameObject.Value;
			this.storeName.Value = ((!(value != null)) ? string.Empty : value.name);
		}

		// Token: 0x040026BB RID: 9915
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x040026BC RID: 9916
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeName;

		// Token: 0x040026BD RID: 9917
		public bool everyFrame;
	}
}
