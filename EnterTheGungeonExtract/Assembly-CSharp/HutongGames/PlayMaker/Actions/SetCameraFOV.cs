using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ACC RID: 2764
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets Field of View used by the Camera.")]
	public class SetCameraFOV : ComponentAction<Camera>
	{
		// Token: 0x06003A8B RID: 14987 RVA: 0x00129748 File Offset: 0x00127948
		public override void Reset()
		{
			this.gameObject = null;
			this.fieldOfView = 50f;
			this.everyFrame = false;
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x00129768 File Offset: 0x00127968
		public override void OnEnter()
		{
			this.DoSetCameraFOV();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x00129784 File Offset: 0x00127984
		public override void OnUpdate()
		{
			this.DoSetCameraFOV();
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x0012978C File Offset: 0x0012798C
		private void DoSetCameraFOV()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.camera.fieldOfView = this.fieldOfView.Value;
			}
		}

		// Token: 0x04002CB2 RID: 11442
		[CheckForComponent(typeof(Camera))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CB3 RID: 11443
		[RequiredField]
		public FsmFloat fieldOfView;

		// Token: 0x04002CB4 RID: 11444
		public bool everyFrame;
	}
}
