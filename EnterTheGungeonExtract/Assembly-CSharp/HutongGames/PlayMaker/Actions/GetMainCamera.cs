using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000993 RID: 2451
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Gets the GameObject tagged MainCamera from the scene")]
	[ActionTarget(typeof(Camera), "storeGameObject", false)]
	public class GetMainCamera : FsmStateAction
	{
		// Token: 0x0600353C RID: 13628 RVA: 0x00112AE4 File Offset: 0x00110CE4
		public override void Reset()
		{
			this.storeGameObject = null;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x00112AF0 File Offset: 0x00110CF0
		public override void OnEnter()
		{
			this.storeGameObject.Value = ((!(Camera.main != null)) ? null : Camera.main.gameObject);
			base.Finish();
		}

		// Token: 0x040026A1 RID: 9889
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;
	}
}
