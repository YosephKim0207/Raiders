using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC9 RID: 2761
	[Tooltip("Sets the Background Color used by the Camera.")]
	[ActionCategory(ActionCategory.Camera)]
	public class SetBackgroundColor : ComponentAction<Camera>
	{
		// Token: 0x06003A7D RID: 14973 RVA: 0x001295B4 File Offset: 0x001277B4
		public override void Reset()
		{
			this.gameObject = null;
			this.backgroundColor = Color.black;
			this.everyFrame = false;
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x001295D4 File Offset: 0x001277D4
		public override void OnEnter()
		{
			this.DoSetBackgroundColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x001295F0 File Offset: 0x001277F0
		public override void OnUpdate()
		{
			this.DoSetBackgroundColor();
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x001295F8 File Offset: 0x001277F8
		private void DoSetBackgroundColor()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.camera.backgroundColor = this.backgroundColor.Value;
			}
		}

		// Token: 0x04002CA8 RID: 11432
		[CheckForComponent(typeof(Camera))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CA9 RID: 11433
		[RequiredField]
		public FsmColor backgroundColor;

		// Token: 0x04002CAA RID: 11434
		public bool everyFrame;
	}
}
