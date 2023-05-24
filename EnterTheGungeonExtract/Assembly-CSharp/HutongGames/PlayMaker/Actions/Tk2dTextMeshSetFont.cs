using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C45 RID: 3141
	[Tooltip("Set the font of a TextMesh. \nChanges will not be updated if commit is OFF. This is so you can change multiple parameters without reconstructing the mesh repeatedly.\n Use tk2dtextMeshCommit or set commit to true on your last change for that mesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshSetFont : FsmStateAction
	{
		// Token: 0x060043B2 RID: 17330 RVA: 0x0015DDB4 File Offset: 0x0015BFB4
		private void _getTextMesh()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._textMesh = ownerDefaultTarget.GetComponent<tk2dTextMesh>();
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0015DDEC File Offset: 0x0015BFEC
		public override void Reset()
		{
			this.gameObject = null;
			this.font = null;
			this.commit = true;
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0015DE08 File Offset: 0x0015C008
		public override void OnEnter()
		{
			this._getTextMesh();
			this.DoSetFont();
			base.Finish();
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0015DE1C File Offset: 0x0015C01C
		private void DoSetFont()
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
			this._textMesh.font = component.data;
			this._textMesh.GetComponent<Renderer>().material = component.material;
			this._textMesh.Init(true);
		}

		// Token: 0x040035C8 RID: 13768
		[CheckForComponent(typeof(tk2dTextMesh))]
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040035C9 RID: 13769
		[UIHint(UIHint.FsmGameObject)]
		[RequiredField]
		[Tooltip("The font gameObject")]
		[CheckForComponent(typeof(tk2dFont))]
		public FsmGameObject font;

		// Token: 0x040035CA RID: 13770
		[Tooltip("Commit changes")]
		[UIHint(UIHint.FsmString)]
		public FsmBool commit;

		// Token: 0x040035CB RID: 13771
		private tk2dTextMesh _textMesh;
	}
}
