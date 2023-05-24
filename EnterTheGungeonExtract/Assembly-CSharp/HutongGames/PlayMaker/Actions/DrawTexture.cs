using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200093C RID: 2364
	[Tooltip("Draws a GUI Texture. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
	[ActionCategory(ActionCategory.GUI)]
	public class DrawTexture : FsmStateAction
	{
		// Token: 0x060033BF RID: 13247 RVA: 0x0010DF24 File Offset: 0x0010C124
		public override void Reset()
		{
			this.texture = null;
			this.screenRect = null;
			this.left = 0f;
			this.top = 0f;
			this.width = 1f;
			this.height = 1f;
			this.scaleMode = ScaleMode.StretchToFill;
			this.alphaBlend = true;
			this.imageAspect = 0f;
			this.normalized = true;
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x0010DFB0 File Offset: 0x0010C1B0
		public override void OnGUI()
		{
			if (this.texture.Value == null)
			{
				return;
			}
			this.rect = (this.screenRect.IsNone ? default(Rect) : this.screenRect.Value);
			if (!this.left.IsNone)
			{
				this.rect.x = this.left.Value;
			}
			if (!this.top.IsNone)
			{
				this.rect.y = this.top.Value;
			}
			if (!this.width.IsNone)
			{
				this.rect.width = this.width.Value;
			}
			if (!this.height.IsNone)
			{
				this.rect.height = this.height.Value;
			}
			if (this.normalized.Value)
			{
				this.rect.x = this.rect.x * (float)Screen.width;
				this.rect.width = this.rect.width * (float)Screen.width;
				this.rect.y = this.rect.y * (float)Screen.height;
				this.rect.height = this.rect.height * (float)Screen.height;
			}
			GUI.DrawTexture(this.rect, this.texture.Value, this.scaleMode, this.alphaBlend.Value, this.imageAspect.Value);
		}

		// Token: 0x040024DB RID: 9435
		[Tooltip("Texture to draw.")]
		[RequiredField]
		public FsmTexture texture;

		// Token: 0x040024DC RID: 9436
		[UIHint(UIHint.Variable)]
		[Tooltip("Rectangle on the screen to draw the texture within. Alternatively, set or override individual properties below.")]
		[Title("Position")]
		public FsmRect screenRect;

		// Token: 0x040024DD RID: 9437
		[Tooltip("Left screen coordinate.")]
		public FsmFloat left;

		// Token: 0x040024DE RID: 9438
		[Tooltip("Top screen coordinate.")]
		public FsmFloat top;

		// Token: 0x040024DF RID: 9439
		[Tooltip("Width of texture on screen.")]
		public FsmFloat width;

		// Token: 0x040024E0 RID: 9440
		[Tooltip("Height of texture on screen.")]
		public FsmFloat height;

		// Token: 0x040024E1 RID: 9441
		[Tooltip("How to scale the image when the aspect ratio of it doesn't fit the aspect ratio to be drawn within.")]
		public ScaleMode scaleMode;

		// Token: 0x040024E2 RID: 9442
		[Tooltip("Whether to alpha blend the image on to the display (the default). If false, the picture is drawn on to the display.")]
		public FsmBool alphaBlend;

		// Token: 0x040024E3 RID: 9443
		[Tooltip("Aspect ratio to use for the source image. If 0 (the default), the aspect ratio from the image is used. Pass in w/h for the desired aspect ratio. This allows the aspect ratio of the source image to be adjusted without changing the pixel width and height.")]
		public FsmFloat imageAspect;

		// Token: 0x040024E4 RID: 9444
		[Tooltip("Use normalized screen coordinates (0-1)")]
		public FsmBool normalized;

		// Token: 0x040024E5 RID: 9445
		private Rect rect;
	}
}
