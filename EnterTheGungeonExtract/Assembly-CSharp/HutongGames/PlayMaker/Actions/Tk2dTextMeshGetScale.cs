using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C3F RID: 3135
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Get the scale of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshGetScale : FsmStateAction
	{
		// Token: 0x0600438E RID: 17294 RVA: 0x0015D634 File Offset: 0x0015B834
		private void _getTextMesh()
		{
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				return;
			}
			this._textMesh = this.go.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0015D670 File Offset: 0x0015B870
		public override void Reset()
		{
			this.gameObject = null;
			this.scale = null;
			this.everyframe = false;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0015D688 File Offset: 0x0015B888
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetScale();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0015D6A8 File Offset: 0x0015B8A8
		public override void OnUpdate()
		{
			this.DoGetScale();
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0015D6B0 File Offset: 0x0015B8B0
		private void DoGetScale()
		{
			if (this.go == null)
			{
				return;
			}
			if (this._textMesh == null)
			{
				Debug.Log(this._textMesh);
				base.LogError("Missing tk2dTextMesh component: " + this.go.name);
				return;
			}
			this.scale.Value = this._textMesh.scale;
		}

		// Token: 0x040035AC RID: 13740
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035AD RID: 13741
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The scale")]
		public FsmVector3 scale;

		// Token: 0x040035AE RID: 13742
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x040035AF RID: 13743
		private GameObject go;

		// Token: 0x040035B0 RID: 13744
		private tk2dTextMesh _textMesh;
	}
}
