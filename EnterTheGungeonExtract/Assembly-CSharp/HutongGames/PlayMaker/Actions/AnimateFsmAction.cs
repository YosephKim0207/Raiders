using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000885 RID: 2181
	public abstract class AnimateFsmAction : FsmStateAction
	{
		// Token: 0x06003096 RID: 12438 RVA: 0x000FFAA4 File Offset: 0x000FDCA4
		public override void Reset()
		{
			this.finishEvent = null;
			this.realTime = false;
			this.time = new FsmFloat
			{
				UseVariable = true
			};
			this.speed = new FsmFloat
			{
				UseVariable = true
			};
			this.delay = new FsmFloat
			{
				UseVariable = true
			};
			this.ignoreCurveOffset = new FsmBool
			{
				Value = true
			};
			this.resultFloats = new float[0];
			this.fromFloats = new float[0];
			this.toFloats = new float[0];
			this.endTimes = new float[0];
			this.keyOffsets = new float[0];
			this.curves = new AnimationCurve[0];
			this.finishAction = false;
			this.start = false;
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000FFB68 File Offset: 0x000FDD68
		public override void OnEnter()
		{
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.lastTime = FsmTime.RealtimeSinceStartup - this.startTime;
			this.deltaTime = 0f;
			this.currentTime = 0f;
			this.isRunning = false;
			this.finishAction = false;
			this.looping = false;
			this.delayTime = ((!this.delay.IsNone) ? (this.delayTime = this.delay.Value) : 0f);
			this.start = true;
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x000FFBF8 File Offset: 0x000FDDF8
		protected void Init()
		{
			this.endTimes = new float[this.curves.Length];
			this.keyOffsets = new float[this.curves.Length];
			this.largestEndTime = 0f;
			for (int i = 0; i < this.curves.Length; i++)
			{
				if (this.curves[i] != null && this.curves[i].keys.Length > 0)
				{
					this.keyOffsets[i] = ((this.curves[i].keys.Length <= 0) ? 0f : ((!this.time.IsNone) ? (this.time.Value / this.curves[i].keys[this.curves[i].length - 1].time * this.curves[i].keys[0].time) : this.curves[i].keys[0].time));
					this.currentTime = ((!this.ignoreCurveOffset.IsNone) ? ((!this.ignoreCurveOffset.Value) ? 0f : this.keyOffsets[i]) : 0f);
					if (!this.time.IsNone)
					{
						this.endTimes[i] = this.time.Value;
					}
					else
					{
						this.endTimes[i] = this.curves[i].keys[this.curves[i].length - 1].time;
					}
					if (this.largestEndTime < this.endTimes[i])
					{
						this.largestEndTime = this.endTimes[i];
					}
					if (!this.looping)
					{
						this.looping = ActionHelpers.IsLoopingWrapMode(this.curves[i].postWrapMode);
					}
				}
				else
				{
					this.endTimes[i] = -1f;
				}
			}
			for (int j = 0; j < this.curves.Length; j++)
			{
				if (this.largestEndTime > 0f && this.endTimes[j] == -1f)
				{
					this.endTimes[j] = this.largestEndTime;
				}
				else if (this.largestEndTime == 0f && this.endTimes[j] == -1f)
				{
					if (this.time.IsNone)
					{
						this.endTimes[j] = 1f;
					}
					else
					{
						this.endTimes[j] = this.time.Value;
					}
				}
			}
			this.UpdateAnimation();
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000FFEAC File Offset: 0x000FE0AC
		public override void OnUpdate()
		{
			this.CheckStart();
			if (this.isRunning)
			{
				this.UpdateTime();
				this.UpdateAnimation();
				this.CheckFinished();
			}
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000FFED4 File Offset: 0x000FE0D4
		private void CheckStart()
		{
			if (!this.isRunning && this.start)
			{
				if (this.delayTime >= 0f)
				{
					if (this.realTime)
					{
						this.deltaTime = FsmTime.RealtimeSinceStartup - this.startTime - this.lastTime;
						this.lastTime = FsmTime.RealtimeSinceStartup - this.startTime;
						this.delayTime -= this.deltaTime;
					}
					else
					{
						this.delayTime -= Time.deltaTime;
					}
				}
				else
				{
					this.isRunning = true;
					this.start = false;
				}
			}
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000FFF7C File Offset: 0x000FE17C
		private void UpdateTime()
		{
			if (this.realTime)
			{
				this.deltaTime = FsmTime.RealtimeSinceStartup - this.startTime - this.lastTime;
				this.lastTime = FsmTime.RealtimeSinceStartup - this.startTime;
				if (!this.speed.IsNone)
				{
					this.currentTime += this.deltaTime * this.speed.Value;
				}
				else
				{
					this.currentTime += this.deltaTime;
				}
			}
			else if (!this.speed.IsNone)
			{
				this.currentTime += Time.deltaTime * this.speed.Value;
			}
			else
			{
				this.currentTime += Time.deltaTime;
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x00100050 File Offset: 0x000FE250
		public void UpdateAnimation()
		{
			for (int i = 0; i < this.curves.Length; i++)
			{
				if (this.curves[i] != null && this.curves[i].keys.Length > 0)
				{
					if (this.calculations[i] != AnimateFsmAction.Calculation.None)
					{
						switch (this.calculations[i])
						{
						case AnimateFsmAction.Calculation.SetValue:
							if (!this.time.IsNone)
							{
								this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time);
							}
							else
							{
								this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime);
							}
							break;
						case AnimateFsmAction.Calculation.AddToValue:
							if (!this.time.IsNone)
							{
								this.resultFloats[i] = this.fromFloats[i] + this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time);
							}
							else
							{
								this.resultFloats[i] = this.fromFloats[i] + this.curves[i].Evaluate(this.currentTime);
							}
							break;
						case AnimateFsmAction.Calculation.SubtractFromValue:
							if (!this.time.IsNone)
							{
								this.resultFloats[i] = this.fromFloats[i] - this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time);
							}
							else
							{
								this.resultFloats[i] = this.fromFloats[i] - this.curves[i].Evaluate(this.currentTime);
							}
							break;
						case AnimateFsmAction.Calculation.SubtractValueFromCurve:
							if (!this.time.IsNone)
							{
								this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) - this.fromFloats[i];
							}
							else
							{
								this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime) - this.fromFloats[i];
							}
							break;
						case AnimateFsmAction.Calculation.MultiplyValue:
							if (!this.time.IsNone)
							{
								this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) * this.fromFloats[i];
							}
							else
							{
								this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime) * this.fromFloats[i];
							}
							break;
						case AnimateFsmAction.Calculation.DivideValue:
							if (!this.time.IsNone)
							{
								this.resultFloats[i] = ((this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) == 0f) ? float.MaxValue : (this.fromFloats[i] / this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time)));
							}
							else
							{
								this.resultFloats[i] = ((this.curves[i].Evaluate(this.currentTime) == 0f) ? float.MaxValue : (this.fromFloats[i] / this.curves[i].Evaluate(this.currentTime)));
							}
							break;
						case AnimateFsmAction.Calculation.DivideCurveByValue:
							if (!this.time.IsNone)
							{
								this.resultFloats[i] = ((this.fromFloats[i] == 0f) ? float.MaxValue : (this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) / this.fromFloats[i]));
							}
							else
							{
								this.resultFloats[i] = ((this.fromFloats[i] == 0f) ? float.MaxValue : (this.curves[i].Evaluate(this.currentTime) / this.fromFloats[i]));
							}
							break;
						}
					}
					else
					{
						this.resultFloats[i] = this.fromFloats[i];
					}
				}
				else
				{
					this.resultFloats[i] = this.fromFloats[i];
				}
			}
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x001005BC File Offset: 0x000FE7BC
		private void CheckFinished()
		{
			if (this.isRunning && !this.looping)
			{
				this.finishAction = true;
				for (int i = 0; i < this.endTimes.Length; i++)
				{
					if (this.currentTime < this.endTimes[i])
					{
						this.finishAction = false;
					}
				}
				this.isRunning = !this.finishAction;
			}
		}

		// Token: 0x04002143 RID: 8515
		[Tooltip("Define time to use your curve scaled to be stretched or shrinked.")]
		public FsmFloat time;

		// Token: 0x04002144 RID: 8516
		[Tooltip("If you define speed, your animation will be speeded up or slowed down.")]
		public FsmFloat speed;

		// Token: 0x04002145 RID: 8517
		[Tooltip("Delayed animimation start.")]
		public FsmFloat delay;

		// Token: 0x04002146 RID: 8518
		[Tooltip("Animation curve start from any time. If IgnoreCurveOffset is true the animation starts right after the state become entered.")]
		public FsmBool ignoreCurveOffset;

		// Token: 0x04002147 RID: 8519
		[Tooltip("Optionally send an Event when the animation finishes.")]
		public FsmEvent finishEvent;

		// Token: 0x04002148 RID: 8520
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		// Token: 0x04002149 RID: 8521
		private float startTime;

		// Token: 0x0400214A RID: 8522
		private float currentTime;

		// Token: 0x0400214B RID: 8523
		private float[] endTimes;

		// Token: 0x0400214C RID: 8524
		private float lastTime;

		// Token: 0x0400214D RID: 8525
		private float deltaTime;

		// Token: 0x0400214E RID: 8526
		private float delayTime;

		// Token: 0x0400214F RID: 8527
		private float[] keyOffsets;

		// Token: 0x04002150 RID: 8528
		protected AnimationCurve[] curves;

		// Token: 0x04002151 RID: 8529
		protected AnimateFsmAction.Calculation[] calculations;

		// Token: 0x04002152 RID: 8530
		protected float[] resultFloats;

		// Token: 0x04002153 RID: 8531
		protected float[] fromFloats;

		// Token: 0x04002154 RID: 8532
		protected float[] toFloats;

		// Token: 0x04002155 RID: 8533
		protected bool finishAction;

		// Token: 0x04002156 RID: 8534
		protected bool isRunning;

		// Token: 0x04002157 RID: 8535
		protected bool looping;

		// Token: 0x04002158 RID: 8536
		private bool start;

		// Token: 0x04002159 RID: 8537
		private float largestEndTime;

		// Token: 0x02000886 RID: 2182
		public enum Calculation
		{
			// Token: 0x0400215B RID: 8539
			None,
			// Token: 0x0400215C RID: 8540
			SetValue,
			// Token: 0x0400215D RID: 8541
			AddToValue,
			// Token: 0x0400215E RID: 8542
			SubtractFromValue,
			// Token: 0x0400215F RID: 8543
			SubtractValueFromCurve,
			// Token: 0x04002160 RID: 8544
			MultiplyValue,
			// Token: 0x04002161 RID: 8545
			DivideValue,
			// Token: 0x04002162 RID: 8546
			DivideCurveByValue
		}
	}
}
