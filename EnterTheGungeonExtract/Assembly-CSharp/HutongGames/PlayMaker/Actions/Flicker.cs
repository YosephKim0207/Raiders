using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000949 RID: 2377
	[ActionCategory(ActionCategory.Effects)]
	[Tooltip("Randomly flickers a Game Object on/off.")]
	public class Flicker : ComponentAction<Renderer>
	{
		// Token: 0x060033FA RID: 13306 RVA: 0x0010ECFC File Offset: 0x0010CEFC
		public override void Reset()
		{
			this.gameObject = null;
			this.frequency = 0.1f;
			this.amountOn = 0.5f;
			this.rendererOnly = true;
			this.realTime = false;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0010ED34 File Offset: 0x0010CF34
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.timer = 0f;
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x0010ED4C File Offset: 0x0010CF4C
		public override void OnUpdate()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.realTime)
			{
				this.timer = FsmTime.RealtimeSinceStartup - this.startTime;
			}
			else
			{
				this.timer += Time.deltaTime;
			}
			if (this.timer > this.frequency.Value)
			{
				bool flag = UnityEngine.Random.Range(0f, 1f) < this.amountOn.Value;
				if (this.rendererOnly)
				{
					if (base.UpdateCache(ownerDefaultTarget))
					{
						base.renderer.enabled = flag;
					}
				}
				else
				{
					ownerDefaultTarget.SetActive(flag);
				}
				this.startTime = this.timer;
				this.timer = 0f;
			}
		}

		// Token: 0x04002519 RID: 9497
		[Tooltip("The GameObject to flicker.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400251A RID: 9498
		[Tooltip("The frequency of the flicker in seconds.")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat frequency;

		// Token: 0x0400251B RID: 9499
		[Tooltip("Amount of time flicker is On (0-1). E.g. Use 0.95 for an occasional flicker.")]
		[HasFloatSlider(0f, 1f)]
		public FsmFloat amountOn;

		// Token: 0x0400251C RID: 9500
		[Tooltip("Only effect the renderer, leaving other components active.")]
		public bool rendererOnly;

		// Token: 0x0400251D RID: 9501
		[Tooltip("Ignore time scale. Useful if flickering UI when the game is paused.")]
		public bool realTime;

		// Token: 0x0400251E RID: 9502
		private float startTime;

		// Token: 0x0400251F RID: 9503
		private float timer;
	}
}
