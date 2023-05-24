using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B0C RID: 2828
	[ActionTarget(typeof(Component), "targetProperty", false)]
	[ActionCategory(ActionCategory.UnityObject)]
	[ActionTarget(typeof(GameObject), "targetProperty", false)]
	[Tooltip("Sets the value of any public property or field on the targeted Unity Object. E.g., Drag and drop any component attached to a Game Object to access its properties.")]
	public class SetProperty : FsmStateAction
	{
		// Token: 0x06003BA3 RID: 15267 RVA: 0x0012CB84 File Offset: 0x0012AD84
		public override void Reset()
		{
			this.targetProperty = new FsmProperty
			{
				setProperty = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x0012CBAC File Offset: 0x0012ADAC
		public override void OnEnter()
		{
			this.targetProperty.SetValue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x0012CBCC File Offset: 0x0012ADCC
		public override void OnUpdate()
		{
			this.targetProperty.SetValue();
		}

		// Token: 0x04002DC6 RID: 11718
		public FsmProperty targetProperty;

		// Token: 0x04002DC7 RID: 11719
		public bool everyFrame;
	}
}
