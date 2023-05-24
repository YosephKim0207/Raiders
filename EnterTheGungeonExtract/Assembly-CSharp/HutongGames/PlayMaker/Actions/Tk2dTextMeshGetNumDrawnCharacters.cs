using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C3C RID: 3132
	[ActionCategory("2D Toolkit/TextMesh")]
	[Tooltip("Get the number of drawn characters of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	public class Tk2dTextMeshGetNumDrawnCharacters : FsmStateAction
	{
		// Token: 0x06004380 RID: 17280 RVA: 0x0015D34C File Offset: 0x0015B54C
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0015D384 File Offset: 0x0015B584
		public override void Reset()
		{
			this.gameObject = null;
			this.numDrawnCharacters = null;
			this.everyframe = false;
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0015D39C File Offset: 0x0015B59C
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoGetNumDrawnCharacters();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0015D3BC File Offset: 0x0015B5BC
		public override void OnUpdate()
		{
			this.DoGetNumDrawnCharacters();
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x0015D3C4 File Offset: 0x0015B5C4
		private void DoGetNumDrawnCharacters()
		{
			if (this._textMesh == null)
			{
				base.LogWarning("Missing tk2dTextMesh component");
				return;
			}
			this.numDrawnCharacters.Value = this._textMesh.NumDrawnCharacters();
		}

		// Token: 0x04003599 RID: 13721
		[CheckForComponent(typeof(tk2dTextMesh))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400359A RID: 13722
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The number of drawn characters")]
		public FsmInt numDrawnCharacters;

		// Token: 0x0400359B RID: 13723
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x0400359C RID: 13724
		private tk2dTextMesh _textMesh;
	}
}
