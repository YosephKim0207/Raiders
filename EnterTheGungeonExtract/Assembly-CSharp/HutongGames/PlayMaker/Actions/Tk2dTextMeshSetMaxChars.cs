using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C47 RID: 3143
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Set the maximum characters number of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshSetMaxChars : FsmStateAction
	{
		// Token: 0x060043BC RID: 17340 RVA: 0x0015DFB4 File Offset: 0x0015C1B4
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x0015DFEC File Offset: 0x0015C1EC
		public override void Reset()
		{
			this.gameObject = null;
			this.maxChars = 30;
			this.commit = true;
			this.everyframe = false;
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x0015E018 File Offset: 0x0015C218
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetMaxChars();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x0015E038 File Offset: 0x0015C238
		public override void OnUpdate()
		{
			this.DoSetMaxChars();
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x0015E040 File Offset: 0x0015C240
		private void DoSetMaxChars()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			if (this._textMesh.maxChars != this.maxChars.Value)
			{
				this._textMesh.maxChars = this.maxChars.Value;
				if (this.commit.Value)
				{
					this._textMesh.Commit();
				}
			}
		}

		// Token: 0x040035D0 RID: 13776
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035D1 RID: 13777
		[Tooltip("The max number of characters")]
		[UIHint(UIHint.FsmInt)]
		public FsmInt maxChars;

		// Token: 0x040035D2 RID: 13778
		[Tooltip("Commit changes")]
		[UIHint(UIHint.FsmString)]
		public FsmBool commit;

		// Token: 0x040035D3 RID: 13779
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x040035D4 RID: 13780
		private tk2dTextMesh _textMesh;
	}
}
