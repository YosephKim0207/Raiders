using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C38 RID: 3128
	[Tooltip("Get the font of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetFont : FsmStateAction
	{
		// Token: 0x06004369 RID: 17257 RVA: 0x0015D034 File Offset: 0x0015B234
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0015D06C File Offset: 0x0015B26C
		public override void Reset()
		{
			this.gameObject = null;
			this.font = null;
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0015D07C File Offset: 0x0015B27C
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetFont();
			base.Finish();
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x0015D090 File Offset: 0x0015B290
		private void DoGetFont()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			GameObject value = this.font.Value;
			if (value == null)
			{
				return;
			}
			tk2dFont component = value.GetComponent<tk2dFont>();
			if (component == null)
			{
				return;
			}
		}

		// Token: 0x0400358A RID: 13706
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400358B RID: 13707
		[UIHint(UIHint.FsmGameObject)]
		[Tooltip("The font gameObject")]
		[RequiredField]
		public FsmGameObject font;

		// Token: 0x0400358C RID: 13708
		private tk2dTextMesh _textMesh;
	}
}
