using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B00 RID: 2816
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets the Main Camera.")]
	public class SetMainCamera : FsmStateAction
	{
		// Token: 0x06003B72 RID: 15218 RVA: 0x0012BFB8 File Offset: 0x0012A1B8
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x0012BFC4 File Offset: 0x0012A1C4
		public override void OnEnter()
		{
			if (this.gameObject.Value != null)
			{
				if (Camera.main != null)
				{
					Camera.main.gameObject.tag = "Untagged";
				}
				this.gameObject.Value.tag = "MainCamera";
			}
			base.Finish();
		}

		// Token: 0x04002D96 RID: 11670
		[CheckForComponent(typeof(Camera))]
		[Tooltip("The GameObject to set as the main camera (should have a Camera component).")]
		[RequiredField]
		public FsmGameObject gameObject;
	}
}
