using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C35 RID: 3125
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W723")]
	[Tooltip("Commit a TextMesh. This is so you can change multiple parameters without reconstructing the mesh repeatedly, simply use that after you have set all the different properties. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshCommit : FsmStateAction
	{
		// Token: 0x06004358 RID: 17240 RVA: 0x0015CDBC File Offset: 0x0015AFBC
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x0015CDF4 File Offset: 0x0015AFF4
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x0015CE00 File Offset: 0x0015B000
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoCommit();
			base.Finish();
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0015CE14 File Offset: 0x0015B014
		private void DoCommit()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			this._textMesh.Commit();
		}

		// Token: 0x0400357E RID: 13694
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400357F RID: 13695
		private tk2dTextMesh _textMesh;
	}
}
