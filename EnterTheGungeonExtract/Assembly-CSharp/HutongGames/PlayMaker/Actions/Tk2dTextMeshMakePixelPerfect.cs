using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C42 RID: 3138
	[Tooltip("Make a TextMesh pixelPerfect. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshMakePixelPerfect : FsmStateAction
	{
		// Token: 0x060043A0 RID: 17312 RVA: 0x0015D8DC File Offset: 0x0015BADC
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0015D914 File Offset: 0x0015BB14
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x0015D920 File Offset: 0x0015BB20
		public override void OnEnter()
		{
			this._getTextMesh();
			this.MakePixelPerfect();
			base.Finish();
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0015D934 File Offset: 0x0015BB34
		private void MakePixelPerfect()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component ");
				return;
			}
			this._textMesh.MakePixelPerfect();
		}

		// Token: 0x040035B9 RID: 13753
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035BA RID: 13754
		private tk2dTextMesh _textMesh;
	}
}
