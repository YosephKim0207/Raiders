using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008FB RID: 2299
	[ActionCategory(ActionCategory.Effects)]
	[Tooltip("Turns a Game Object on/off in a regular repeating pattern.")]
	public class Blink : ComponentAction<Renderer>
	{
		// Token: 0x060032AC RID: 12972 RVA: 0x0010A2EC File Offset: 0x001084EC
		public override void Reset()
		{
			this.gameObject = null;
			this.timeOff = 0.5f;
			this.timeOn = 0.5f;
			this.rendererOnly = true;
			this.startOn = false;
			this.realTime = false;
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x0010A33C File Offset: 0x0010853C
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.timer = 0f;
			this.UpdateBlinkState(this.startOn.Value);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x0010A368 File Offset: 0x00108568
		public override void OnUpdate()
		{
			if (this.realTime)
			{
				this.timer = FsmTime.RealtimeSinceStartup - this.startTime;
			}
			else
			{
				this.timer += Time.deltaTime;
			}
			if (this.blinkOn && this.timer > this.timeOn.Value)
			{
				this.UpdateBlinkState(false);
			}
			if (!this.blinkOn && this.timer > this.timeOff.Value)
			{
				this.UpdateBlinkState(true);
			}
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x0010A3FC File Offset: 0x001085FC
		private void UpdateBlinkState(bool state)
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (gameObject == null)
			{
				return;
			}
			if (this.rendererOnly)
			{
				if (base.UpdateCache(gameObject))
				{
					base.renderer.enabled = state;
				}
			}
			else
			{
				gameObject.SetActive(state);
			}
			this.blinkOn = state;
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.timer = 0f;
		}

		// Token: 0x040023D2 RID: 9170
		[Tooltip("The GameObject to blink on/off.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040023D3 RID: 9171
		[Tooltip("Time to stay off in seconds.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat timeOff;

		// Token: 0x040023D4 RID: 9172
		[Tooltip("Time to stay on in seconds.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat timeOn;

		// Token: 0x040023D5 RID: 9173
		[Tooltip("Should the object start in the active/visible state?")]
		public FsmBool startOn;

		// Token: 0x040023D6 RID: 9174
		[Tooltip("Only effect the renderer, keeping other components active.")]
		public bool rendererOnly;

		// Token: 0x040023D7 RID: 9175
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		// Token: 0x040023D8 RID: 9176
		private float startTime;

		// Token: 0x040023D9 RID: 9177
		private float timer;

		// Token: 0x040023DA RID: 9178
		private bool blinkOn;
	}
}
