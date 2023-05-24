using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200093A RID: 2362
	[Tooltip("Fills the screen with a Color. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
	[ActionCategory(ActionCategory.GUI)]
	public class DrawFullscreenColor : FsmStateAction
	{
		// Token: 0x060033B9 RID: 13241 RVA: 0x0010DE80 File Offset: 0x0010C080
		public override void Reset()
		{
			this.color = Color.white;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x0010DE94 File Offset: 0x0010C094
		public override void OnGUI()
		{
			Color color = GUI.color;
			GUI.color = this.color.Value;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), ActionHelpers.WhiteTexture);
			GUI.color = color;
		}

		// Token: 0x040024D9 RID: 9433
		[RequiredField]
		[Tooltip("Color. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
		public FsmColor color;
	}
}
