using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C3D RID: 3133
	[Tooltip("Get the pixelPerfect flag of a TextMesh. \nNOTE: The Game Object must have a tk2dTextMesh attached.")]
	[ActionCategory("2D Toolkit/TextMesh")]
	public class Tk2dTextMeshGetPixelPerfect : FsmStateAction
	{
		// Token: 0x06004386 RID: 17286 RVA: 0x0015D404 File Offset: 0x0015B604
		public override void Reset()
		{
			this.gameObject = null;
			this.pixelPerfect = null;
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x0015D414 File Offset: 0x0015B614
		public override void OnEnter()
		{
			base.Finish();
		}

		// Token: 0x0400359D RID: 13725
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dTextMesh component attached.")]
		[RequiredField]
		[CheckForComponent(typeof(tk2dTextMesh))]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400359E RID: 13726
		[UIHint(UIHint.Variable)]
		[Tooltip("(Deprecated in 2D Toolkit 2.0) Is the text pixelPerfect")]
		[RequiredField]
		public FsmBool pixelPerfect;
	}
}
