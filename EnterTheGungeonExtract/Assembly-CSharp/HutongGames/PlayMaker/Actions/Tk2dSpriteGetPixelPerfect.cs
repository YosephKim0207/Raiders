using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C2E RID: 3118
	[Tooltip("Get the pixel perfect flag of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dClippedSprite)")]
	[ActionCategory("2D Toolkit/Sprite")]
	public class Tk2dSpriteGetPixelPerfect : FsmStateAction
	{
		// Token: 0x06004333 RID: 17203 RVA: 0x0015C894 File Offset: 0x0015AA94
		public override void OnEnter()
		{
			base.Finish();
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x0015C89C File Offset: 0x0015AA9C
		public override void Reset()
		{
			this.gameObject = null;
			this.pixelPerfect = null;
		}

		// Token: 0x04003566 RID: 13670
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dClippedSprite).")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003567 RID: 13671
		[UIHint(UIHint.Variable)]
		[Tooltip("(Deprecated in 2D Toolkit 2.0) Is the sprite pixelPerfect")]
		public FsmBool pixelPerfect;
	}
}
