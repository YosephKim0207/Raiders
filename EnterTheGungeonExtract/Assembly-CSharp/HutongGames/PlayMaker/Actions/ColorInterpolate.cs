using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200090C RID: 2316
	[Tooltip("Interpolate through an array of Colors over a specified amount of Time.")]
	[ActionCategory(ActionCategory.Color)]
	public class ColorInterpolate : FsmStateAction
	{
		// Token: 0x06003304 RID: 13060 RVA: 0x0010BD6C File Offset: 0x00109F6C
		public override void Reset()
		{
			this.colors = new FsmColor[3];
			this.time = 1f;
			this.storeColor = null;
			this.finishEvent = null;
			this.realTime = false;
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x0010BDA0 File Offset: 0x00109FA0
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.currentTime = 0f;
			if (this.colors.Length < 2)
			{
				if (this.colors.Length == 1)
				{
					this.storeColor.Value = this.colors[0].Value;
				}
				base.Finish();
			}
			else
			{
				this.storeColor.Value = this.colors[0].Value;
			}
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x0010BE1C File Offset: 0x0010A01C
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
			if (this.currentTime > this.time.Value)
			{
				base.Finish();
				this.storeColor.Value = this.colors[this.colors.Length - 1].Value;
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
				return;
			}
			float num = (float)(this.colors.Length - 1) * this.currentTime / this.time.Value;
			Color color;
			if (num.Equals(0f))
			{
				color = this.colors[0].Value;
			}
			else if (num.Equals((float)(this.colors.Length - 1)))
			{
				color = this.colors[this.colors.Length - 1].Value;
			}
			else
			{
				Color value = this.colors[Mathf.FloorToInt(num)].Value;
				Color value2 = this.colors[Mathf.CeilToInt(num)].Value;
				num -= Mathf.Floor(num);
				color = Color.Lerp(value, value2, num);
			}
			this.storeColor.Value = color;
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x0010BF74 File Offset: 0x0010A174
		public override string ErrorCheck()
		{
			return (this.colors.Length >= 2) ? null : "Define at least 2 colors to make a gradient.";
		}

		// Token: 0x04002434 RID: 9268
		[Tooltip("Array of colors to interpolate through.")]
		[RequiredField]
		public FsmColor[] colors;

		// Token: 0x04002435 RID: 9269
		[RequiredField]
		[Tooltip("Interpolation time.")]
		public FsmFloat time;

		// Token: 0x04002436 RID: 9270
		[RequiredField]
		[Tooltip("Store the interpolated color in a Color variable.")]
		[UIHint(UIHint.Variable)]
		public FsmColor storeColor;

		// Token: 0x04002437 RID: 9271
		[Tooltip("Event to send when the interpolation finishes.")]
		public FsmEvent finishEvent;

		// Token: 0x04002438 RID: 9272
		[Tooltip("Ignore TimeScale")]
		public bool realTime;

		// Token: 0x04002439 RID: 9273
		private float startTime;

		// Token: 0x0400243A RID: 9274
		private float currentTime;
	}
}
