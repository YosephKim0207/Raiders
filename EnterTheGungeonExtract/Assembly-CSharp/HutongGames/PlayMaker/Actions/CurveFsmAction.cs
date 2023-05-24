using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200088B RID: 2187
	[Tooltip("Animate base action - DON'T USE IT!")]
	public abstract class CurveFsmAction : FsmStateAction
	{
		// Token: 0x060030B3 RID: 12467 RVA: 0x00101124 File Offset: 0x000FF324
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
			this.distances = new float[0];
			this.endTimes = new float[0];
			this.keyOffsets = new float[0];
			this.curves = new AnimationCurve[0];
			this.finishAction = false;
			this.start = false;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x001011F4 File Offset: 0x000FF3F4
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

		// Token: 0x060030B5 RID: 12469 RVA: 0x00101284 File Offset: 0x000FF484
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
			this.distances = new float[this.fromFloats.Length];
			for (int k = 0; k < this.fromFloats.Length; k++)
			{
				this.distances[k] = this.toFloats[k] - this.fromFloats[k];
			}
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x00101574 File Offset: 0x000FF774
		public override void OnUpdate()
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
					this.startTime = FsmTime.RealtimeSinceStartup;
					this.lastTime = FsmTime.RealtimeSinceStartup - this.startTime;
				}
			}
			if (this.isRunning && !this.finishAction)
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
				for (int i = 0; i < this.curves.Length; i++)
				{
					if (this.curves[i] != null && this.curves[i].keys.Length > 0)
					{
						if (this.calculations[i] != CurveFsmAction.Calculation.None)
						{
							switch (this.calculations[i])
							{
							case CurveFsmAction.Calculation.AddToValue:
								if (!this.time.IsNone)
								{
									this.resultFloats[i] = this.fromFloats[i] + (this.distances[i] * (this.currentTime / this.time.Value) + this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time));
								}
								else
								{
									this.resultFloats[i] = this.fromFloats[i] + (this.distances[i] * (this.currentTime / this.endTimes[i]) + this.curves[i].Evaluate(this.currentTime));
								}
								break;
							case CurveFsmAction.Calculation.SubtractFromValue:
								if (!this.time.IsNone)
								{
									this.resultFloats[i] = this.fromFloats[i] + (this.distances[i] * (this.currentTime / this.time.Value) - this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time));
								}
								else
								{
									this.resultFloats[i] = this.fromFloats[i] + (this.distances[i] * (this.currentTime / this.endTimes[i]) - this.curves[i].Evaluate(this.currentTime));
								}
								break;
							case CurveFsmAction.Calculation.SubtractValueFromCurve:
								if (!this.time.IsNone)
								{
									this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) - this.distances[i] * (this.currentTime / this.time.Value) + this.fromFloats[i];
								}
								else
								{
									this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime) - this.distances[i] * (this.currentTime / this.endTimes[i]) + this.fromFloats[i];
								}
								break;
							case CurveFsmAction.Calculation.MultiplyValue:
								if (!this.time.IsNone)
								{
									this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) * this.distances[i] * (this.currentTime / this.time.Value) + this.fromFloats[i];
								}
								else
								{
									this.resultFloats[i] = this.curves[i].Evaluate(this.currentTime) * this.distances[i] * (this.currentTime / this.endTimes[i]) + this.fromFloats[i];
								}
								break;
							case CurveFsmAction.Calculation.DivideValue:
								if (!this.time.IsNone)
								{
									this.resultFloats[i] = ((this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) == 0f) ? float.MaxValue : (this.fromFloats[i] + this.distances[i] * (this.currentTime / this.time.Value) / this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time)));
								}
								else
								{
									this.resultFloats[i] = ((this.curves[i].Evaluate(this.currentTime) == 0f) ? float.MaxValue : (this.fromFloats[i] + this.distances[i] * (this.currentTime / this.endTimes[i]) / this.curves[i].Evaluate(this.currentTime)));
								}
								break;
							case CurveFsmAction.Calculation.DivideCurveByValue:
								if (!this.time.IsNone)
								{
									this.resultFloats[i] = ((this.fromFloats[i] == 0f) ? float.MaxValue : (this.curves[i].Evaluate(this.currentTime / this.time.Value * this.curves[i].keys[this.curves[i].length - 1].time) / (this.distances[i] * (this.currentTime / this.time.Value)) + this.fromFloats[i]));
								}
								else
								{
									this.resultFloats[i] = ((this.fromFloats[i] == 0f) ? float.MaxValue : (this.curves[i].Evaluate(this.currentTime) / (this.distances[i] * (this.currentTime / this.endTimes[i])) + this.fromFloats[i]));
								}
								break;
							}
						}
						else if (!this.time.IsNone)
						{
							this.resultFloats[i] = this.fromFloats[i] + this.distances[i] * (this.currentTime / this.time.Value);
						}
						else
						{
							this.resultFloats[i] = this.fromFloats[i] + this.distances[i] * (this.currentTime / this.endTimes[i]);
						}
					}
					else if (!this.time.IsNone)
					{
						this.resultFloats[i] = this.fromFloats[i] + this.distances[i] * (this.currentTime / this.time.Value);
					}
					else if (this.largestEndTime == 0f)
					{
						this.resultFloats[i] = this.fromFloats[i] + this.distances[i] * (this.currentTime / 1f);
					}
					else
					{
						this.resultFloats[i] = this.fromFloats[i] + this.distances[i] * (this.currentTime / this.largestEndTime);
					}
				}
				if (this.isRunning)
				{
					this.finishAction = true;
					for (int j = 0; j < this.endTimes.Length; j++)
					{
						if (this.currentTime < this.endTimes[j])
						{
							this.finishAction = false;
						}
					}
					this.isRunning = !this.finishAction;
				}
			}
		}

		// Token: 0x04002188 RID: 8584
		[Tooltip("Define time to use your curve scaled to be stretched or shrinked.")]
		public FsmFloat time;

		// Token: 0x04002189 RID: 8585
		[Tooltip("If you define speed, your animation will be speeded up or slowed down.")]
		public FsmFloat speed;

		// Token: 0x0400218A RID: 8586
		[Tooltip("Delayed animimation start.")]
		public FsmFloat delay;

		// Token: 0x0400218B RID: 8587
		[Tooltip("Animation curve start from any time. If IgnoreCurveOffset is true the animation starts right after the state become entered.")]
		public FsmBool ignoreCurveOffset;

		// Token: 0x0400218C RID: 8588
		[Tooltip("Optionally send an Event when the animation finishes.")]
		public FsmEvent finishEvent;

		// Token: 0x0400218D RID: 8589
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		// Token: 0x0400218E RID: 8590
		private float startTime;

		// Token: 0x0400218F RID: 8591
		private float currentTime;

		// Token: 0x04002190 RID: 8592
		private float[] endTimes;

		// Token: 0x04002191 RID: 8593
		private float lastTime;

		// Token: 0x04002192 RID: 8594
		private float deltaTime;

		// Token: 0x04002193 RID: 8595
		private float delayTime;

		// Token: 0x04002194 RID: 8596
		private float[] keyOffsets;

		// Token: 0x04002195 RID: 8597
		protected AnimationCurve[] curves;

		// Token: 0x04002196 RID: 8598
		protected CurveFsmAction.Calculation[] calculations;

		// Token: 0x04002197 RID: 8599
		protected float[] resultFloats;

		// Token: 0x04002198 RID: 8600
		protected float[] fromFloats;

		// Token: 0x04002199 RID: 8601
		protected float[] toFloats;

		// Token: 0x0400219A RID: 8602
		private float[] distances;

		// Token: 0x0400219B RID: 8603
		protected bool finishAction;

		// Token: 0x0400219C RID: 8604
		protected bool isRunning;

		// Token: 0x0400219D RID: 8605
		protected bool looping;

		// Token: 0x0400219E RID: 8606
		private bool start;

		// Token: 0x0400219F RID: 8607
		private float largestEndTime;

		// Token: 0x0200088C RID: 2188
		public enum Calculation
		{
			// Token: 0x040021A1 RID: 8609
			None,
			// Token: 0x040021A2 RID: 8610
			AddToValue,
			// Token: 0x040021A3 RID: 8611
			SubtractFromValue,
			// Token: 0x040021A4 RID: 8612
			SubtractValueFromCurve,
			// Token: 0x040021A5 RID: 8613
			MultiplyValue,
			// Token: 0x040021A6 RID: 8614
			DivideValue,
			// Token: 0x040021A7 RID: 8615
			DivideCurveByValue
		}
	}
}
