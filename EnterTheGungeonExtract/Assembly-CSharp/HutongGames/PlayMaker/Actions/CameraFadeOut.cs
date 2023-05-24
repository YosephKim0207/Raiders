using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000909 RID: 2313
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Fade to a fullscreen Color. NOTE: Uses OnGUI so requires a PlayMakerGUI component in the scene.")]
	public class CameraFadeOut : FsmStateAction
	{
		// Token: 0x060032EC RID: 13036 RVA: 0x0010B604 File Offset: 0x00109804
		public override void Reset()
		{
			this.color = Color.black;
			this.time = 1f;
			this.finishEvent = null;
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x0010B630 File Offset: 0x00109830
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.currentTime = 0f;
			this.colorLerp = Color.clear;
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x0010B654 File Offset: 0x00109854
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
			this.colorLerp = Color.Lerp(Color.clear, this.color.Value, this.currentTime / this.time.Value);
			if (this.currentTime > this.time.Value && this.finishEvent != null)
			{
				base.Fsm.Event(this.finishEvent);
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x0010B6F4 File Offset: 0x001098F4
		public override void OnGUI()
		{
			Color color = GUI.color;
			GUI.color = this.colorLerp;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), ActionHelpers.WhiteTexture);
			GUI.color = color;
		}

		// Token: 0x04002423 RID: 9251
		[Tooltip("Color to fade to. E.g., Fade to black.")]
		[RequiredField]
		public FsmColor color;

		// Token: 0x04002424 RID: 9252
		[Tooltip("Fade out time in seconds.")]
		[RequiredField]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat time;

		// Token: 0x04002425 RID: 9253
		[Tooltip("Event to send when finished.")]
		public FsmEvent finishEvent;

		// Token: 0x04002426 RID: 9254
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		// Token: 0x04002427 RID: 9255
		private float startTime;

		// Token: 0x04002428 RID: 9256
		private float currentTime;

		// Token: 0x04002429 RID: 9257
		private Color colorLerp;
	}
}
