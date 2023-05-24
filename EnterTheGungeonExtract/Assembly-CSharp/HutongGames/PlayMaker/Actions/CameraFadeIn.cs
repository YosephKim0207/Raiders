using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000908 RID: 2312
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Fade from a fullscreen Color. NOTE: Uses OnGUI so requires a PlayMakerGUI component in the scene.")]
	public class CameraFadeIn : FsmStateAction
	{
		// Token: 0x060032E7 RID: 13031 RVA: 0x0010B4B0 File Offset: 0x001096B0
		public override void Reset()
		{
			this.color = Color.black;
			this.time = 1f;
			this.finishEvent = null;
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x0010B4DC File Offset: 0x001096DC
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.currentTime = 0f;
			this.colorLerp = this.color.Value;
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x0010B508 File Offset: 0x00109708
		public override void OnUpdate()
		{
			if (this.realTime)
			{
				this.currentTime = FsmTime.RealtimeSinceStartup - this.startTime;
			}
			else
			{
				this.currentTime += Time.deltaTime;
			}
			this.colorLerp = Color.Lerp(this.color.Value, Color.clear, this.currentTime / this.time.Value);
			if (this.currentTime > this.time.Value)
			{
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
				base.Finish();
			}
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x0010B5B0 File Offset: 0x001097B0
		public override void OnGUI()
		{
			Color color = GUI.color;
			GUI.color = this.colorLerp;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), ActionHelpers.WhiteTexture);
			GUI.color = color;
		}

		// Token: 0x0400241C RID: 9244
		[Tooltip("Color to fade from. E.g., Fade up from black.")]
		[RequiredField]
		public FsmColor color;

		// Token: 0x0400241D RID: 9245
		[HasFloatSlider(0f, 10f)]
		[RequiredField]
		[Tooltip("Fade in time in seconds.")]
		public FsmFloat time;

		// Token: 0x0400241E RID: 9246
		[Tooltip("Event to send when finished.")]
		public FsmEvent finishEvent;

		// Token: 0x0400241F RID: 9247
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		// Token: 0x04002420 RID: 9248
		private float startTime;

		// Token: 0x04002421 RID: 9249
		private float currentTime;

		// Token: 0x04002422 RID: 9250
		private Color colorLerp;
	}
}
