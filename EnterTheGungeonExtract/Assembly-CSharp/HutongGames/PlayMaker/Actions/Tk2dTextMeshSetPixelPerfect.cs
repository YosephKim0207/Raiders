using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C48 RID: 3144
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Set the pixelPerfect flag of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshSetPixelPerfect : FsmStateAction
	{
		// Token: 0x060043C2 RID: 17346 RVA: 0x0015E0C0 File Offset: 0x0015C2C0
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x0015E0F8 File Offset: 0x0015C2F8
		public override void Reset()
		{
			this.gameObject = null;
			this.pixelPerfect = true;
			this.commit = true;
			this.everyframe = false;
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x0015E120 File Offset: 0x0015C320
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetPixelPerfect();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0015E140 File Offset: 0x0015C340
		public override void OnUpdate()
		{
			this.DoSetPixelPerfect();
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x0015E148 File Offset: 0x0015C348
		private void DoSetPixelPerfect()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: ");
				return;
			}
			if (this.pixelPerfect.Value)
			{
				this._textMesh.MakePixelPerfect();
				if (this.commit.Value)
				{
					this._textMesh.Commit();
				}
			}
		}

		// Token: 0x040035D5 RID: 13781
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035D6 RID: 13782
		[Tooltip("Does the text needs to be pixelPerfect")]
		[UIHint(UIHint.FsmBool)]
		public FsmBool pixelPerfect;

		// Token: 0x040035D7 RID: 13783
		[Tooltip("Commit changes")]
		[UIHint(UIHint.FsmString)]
		public FsmBool commit;

		// Token: 0x040035D8 RID: 13784
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x040035D9 RID: 13785
		private tk2dTextMesh _textMesh;
	}
}
