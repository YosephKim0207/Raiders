using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A3 RID: 2467
	[Tooltip("Gets the value of any public property or field on the targeted Unity Object and stores it in a variable. E.g., Drag and drop any component attached to a Game Object to access its properties.")]
	[ActionTarget(typeof(GameObject), "targetProperty", false)]
	[ActionCategory(ActionCategory.UnityObject)]
	[ActionTarget(typeof(Component), "targetProperty", false)]
	public class GetProperty : FsmStateAction
	{
		// Token: 0x0600357E RID: 13694 RVA: 0x0011354C File Offset: 0x0011174C
		public override void Reset()
		{
			this.targetProperty = new FsmProperty
			{
				setProperty = false
			};
			this.everyFrame = false;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x00113574 File Offset: 0x00111774
		public override void OnEnter()
		{
			this.targetProperty.GetValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x00113594 File Offset: 0x00111794
		public override void OnUpdate()
		{
			this.targetProperty.GetValue();
		}

		// Token: 0x040026D0 RID: 9936
		public FsmProperty targetProperty;

		// Token: 0x040026D1 RID: 9937
		public bool everyFrame;
	}
}
