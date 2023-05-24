using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000923 RID: 2339
	[Tooltip("Activates a Camera in the scene.")]
	[ActionCategory(ActionCategory.Camera)]
	public class CutToCamera : FsmStateAction
	{
		// Token: 0x06003370 RID: 13168 RVA: 0x0010D2C8 File Offset: 0x0010B4C8
		public override void Reset()
		{
			this.camera = null;
			this.makeMainCamera = true;
			this.cutBackOnExit = false;
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x0010D2E0 File Offset: 0x0010B4E0
		public override void OnEnter()
		{
			if (this.camera == null)
			{
				base.LogError("Missing camera!");
				return;
			}
			this.oldCamera = Camera.main;
			CutToCamera.SwitchCamera(Camera.main, this.camera);
			if (this.makeMainCamera)
			{
				this.camera.tag = "MainCamera";
			}
			base.Finish();
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x0010D348 File Offset: 0x0010B548
		public override void OnExit()
		{
			if (this.cutBackOnExit)
			{
				CutToCamera.SwitchCamera(this.camera, this.oldCamera);
			}
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x0010D368 File Offset: 0x0010B568
		private static void SwitchCamera(Camera camera1, Camera camera2)
		{
			if (camera1 != null)
			{
				camera1.enabled = false;
			}
			if (camera2 != null)
			{
				camera2.enabled = true;
			}
		}

		// Token: 0x0400249F RID: 9375
		[Tooltip("The Camera to activate.")]
		[RequiredField]
		public Camera camera;

		// Token: 0x040024A0 RID: 9376
		[Tooltip("Makes the camera the new MainCamera. The old MainCamera will be untagged.")]
		public bool makeMainCamera;

		// Token: 0x040024A1 RID: 9377
		[Tooltip("Cut back to the original MainCamera when exiting this state.")]
		public bool cutBackOnExit;

		// Token: 0x040024A2 RID: 9378
		private Camera oldCamera;
	}
}
