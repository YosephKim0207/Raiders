using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ACB RID: 2763
	[Tooltip("Sets the Culling Mask used by the Camera.")]
	[ActionCategory(ActionCategory.Camera)]
	public class SetCameraCullingMask : ComponentAction<Camera>
	{
		// Token: 0x06003A86 RID: 14982 RVA: 0x001296A8 File Offset: 0x001278A8
		public override void Reset()
		{
			this.gameObject = null;
			this.cullingMask = new FsmInt[0];
			this.invertMask = false;
			this.everyFrame = false;
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x001296D0 File Offset: 0x001278D0
		public override void OnEnter()
		{
			this.DoSetCameraCullingMask();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001296EC File Offset: 0x001278EC
		public override void OnUpdate()
		{
			this.DoSetCameraCullingMask();
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001296F4 File Offset: 0x001278F4
		private void DoSetCameraCullingMask()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.camera.cullingMask = ActionHelpers.LayerArrayToLayerMask(this.cullingMask, this.invertMask.Value);
			}
		}

		// Token: 0x04002CAE RID: 11438
		[CheckForComponent(typeof(Camera))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002CAF RID: 11439
		[UIHint(UIHint.Layer)]
		[Tooltip("Cull these layers.")]
		public FsmInt[] cullingMask;

		// Token: 0x04002CB0 RID: 11440
		[Tooltip("Invert the mask, so you cull all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002CB1 RID: 11441
		public bool everyFrame;
	}
}
