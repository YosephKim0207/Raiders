using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C4B RID: 3147
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W719")]
	[Tooltip("Set the text of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshSetText : FsmStateAction
	{
		// Token: 0x060043D3 RID: 17363 RVA: 0x0015E5A4 File Offset: 0x0015C7A4
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0015E5DC File Offset: 0x0015C7DC
		public override void Reset()
		{
			this.gameObject = null;
			this.text = string.Empty;
			this.commit = true;
			this.everyframe = false;
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0015E608 File Offset: 0x0015C808
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetText();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0015E628 File Offset: 0x0015C828
		public override void OnUpdate()
		{
			this.DoSetText();
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x0015E630 File Offset: 0x0015C830
		private void DoSetText()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			if (this._textMesh.text != this.text.Value)
			{
				this._textMesh.text = this.text.Value;
				if (this.commit.Value)
				{
					this._textMesh.Commit();
				}
			}
		}

		// Token: 0x040035EE RID: 13806
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035EF RID: 13807
		[UIHint(UIHint.FsmString)]
		[Tooltip("The text")]
		public FsmString text;

		// Token: 0x040035F0 RID: 13808
		[UIHint(UIHint.FsmString)]
		[Tooltip("Commit changes")]
		public FsmBool commit;

		// Token: 0x040035F1 RID: 13809
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x040035F2 RID: 13810
		private tk2dTextMesh _textMesh;
	}
}
