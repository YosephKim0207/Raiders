using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C40 RID: 3136
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Get the text of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshGetText : FsmStateAction
	{
		// Token: 0x06004394 RID: 17300 RVA: 0x0015D728 File Offset: 0x0015B928
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0015D760 File Offset: 0x0015B960
		public override void Reset()
		{
			this.gameObject = null;
			this.text = null;
			this.everyframe = false;
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0015D778 File Offset: 0x0015B978
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetText();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0015D798 File Offset: 0x0015B998
		public override void OnUpdate()
		{
			this.DoGetText();
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0015D7A0 File Offset: 0x0015B9A0
		private void DoGetText()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component: " + this._textMesh.gameObject.name);
				return;
			}
			this.text.Value = this._textMesh.text;
		}

		// Token: 0x040035B1 RID: 13745
		[CheckForComponent(typeof(tk2dTextMesh))]
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035B2 RID: 13746
		[Tooltip("The text")]
		[UIHint(UIHint.Variable)]
		public FsmString text;

		// Token: 0x040035B3 RID: 13747
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x040035B4 RID: 13748
		private tk2dTextMesh _textMesh;
	}
}
