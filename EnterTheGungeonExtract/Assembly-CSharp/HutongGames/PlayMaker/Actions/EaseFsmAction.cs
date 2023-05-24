using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000891 RID: 2193
	[Tooltip("Ease base action - don't use!")]
	public abstract class EaseFsmAction : FsmStateAction
	{
		// Token: 0x060030CC RID: 12492 RVA: 0x00102B50 File Offset: 0x00100D50
		public override void Reset()
		{
			this.easeType = EaseFsmAction.EaseType.linear;
			this.time = new FsmFloat
			{
				Value = 1f
			};
			this.delay = new FsmFloat
			{
				UseVariable = true
			};
			this.speed = new FsmFloat
			{
				UseVariable = true
			};
			this.reverse = new FsmBool
			{
				Value = false
			};
			this.realTime = false;
			this.finishEvent = null;
			this.ease = null;
			this.runningTime = 0f;
			this.lastTime = 0f;
			this.percentage = 0f;
			this.fromFloats = new float[0];
			this.toFloats = new float[0];
			this.resultFloats = new float[0];
			this.finishAction = false;
			this.start = false;
			this.finished = false;
			this.isRunning = false;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x00102C30 File Offset: 0x00100E30
		public override void OnEnter()
		{
			this.finished = false;
			this.isRunning = false;
			this.SetEasingFunction();
			this.runningTime = 0f;
			this.percentage = ((!this.reverse.IsNone) ? ((!this.reverse.Value) ? 0f : 1f) : 0f);
			this.finishAction = false;
			this.startTime = FsmTime.RealtimeSinceStartup;
			this.lastTime = FsmTime.RealtimeSinceStartup - this.startTime;
			this.delayTime = ((!this.delay.IsNone) ? (this.delayTime = this.delay.Value) : 0f);
			this.start = true;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x00102CFC File Offset: 0x00100EFC
		public override void OnExit()
		{
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x00102D00 File Offset: 0x00100F00
		public override void OnUpdate()
		{
			if (this.start && !this.isRunning)
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
			if (this.isRunning && !this.finished)
			{
				if (this.reverse.IsNone || !this.reverse.Value)
				{
					this.UpdatePercentage();
					if (this.percentage < 1f)
					{
						for (int i = 0; i < this.fromFloats.Length; i++)
						{
							this.resultFloats[i] = this.ease(this.fromFloats[i], this.toFloats[i], this.percentage);
						}
					}
					else
					{
						this.finishAction = true;
						this.finished = true;
						this.isRunning = false;
					}
				}
				else
				{
					this.UpdatePercentage();
					if (this.percentage > 0f)
					{
						for (int j = 0; j < this.fromFloats.Length; j++)
						{
							this.resultFloats[j] = this.ease(this.fromFloats[j], this.toFloats[j], this.percentage);
						}
					}
					else
					{
						this.finishAction = true;
						this.finished = true;
						this.isRunning = false;
					}
				}
			}
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x00102EE8 File Offset: 0x001010E8
		protected void UpdatePercentage()
		{
			if (this.realTime)
			{
				this.deltaTime = FsmTime.RealtimeSinceStartup - this.startTime - this.lastTime;
				this.lastTime = FsmTime.RealtimeSinceStartup - this.startTime;
				if (!this.speed.IsNone)
				{
					this.runningTime += this.deltaTime * this.speed.Value;
				}
				else
				{
					this.runningTime += this.deltaTime;
				}
			}
			else if (!this.speed.IsNone)
			{
				this.runningTime += Time.deltaTime * this.speed.Value;
			}
			else
			{
				this.runningTime += Time.deltaTime;
			}
			if (!this.reverse.IsNone && this.reverse.Value)
			{
				this.percentage = 1f - this.runningTime / this.time.Value;
			}
			else
			{
				this.percentage = this.runningTime / this.time.Value;
			}
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x00103020 File Offset: 0x00101220
		protected void SetEasingFunction()
		{
			switch (this.easeType)
			{
			case EaseFsmAction.EaseType.easeInQuad:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInQuad);
				break;
			case EaseFsmAction.EaseType.easeOutQuad:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutQuad);
				break;
			case EaseFsmAction.EaseType.easeInOutQuad:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutQuad);
				break;
			case EaseFsmAction.EaseType.easeInCubic:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInCubic);
				break;
			case EaseFsmAction.EaseType.easeOutCubic:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutCubic);
				break;
			case EaseFsmAction.EaseType.easeInOutCubic:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutCubic);
				break;
			case EaseFsmAction.EaseType.easeInQuart:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInQuart);
				break;
			case EaseFsmAction.EaseType.easeOutQuart:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutQuart);
				break;
			case EaseFsmAction.EaseType.easeInOutQuart:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutQuart);
				break;
			case EaseFsmAction.EaseType.easeInQuint:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInQuint);
				break;
			case EaseFsmAction.EaseType.easeOutQuint:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutQuint);
				break;
			case EaseFsmAction.EaseType.easeInOutQuint:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutQuint);
				break;
			case EaseFsmAction.EaseType.easeInSine:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInSine);
				break;
			case EaseFsmAction.EaseType.easeOutSine:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutSine);
				break;
			case EaseFsmAction.EaseType.easeInOutSine:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutSine);
				break;
			case EaseFsmAction.EaseType.easeInExpo:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInExpo);
				break;
			case EaseFsmAction.EaseType.easeOutExpo:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutExpo);
				break;
			case EaseFsmAction.EaseType.easeInOutExpo:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutExpo);
				break;
			case EaseFsmAction.EaseType.easeInCirc:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInCirc);
				break;
			case EaseFsmAction.EaseType.easeOutCirc:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutCirc);
				break;
			case EaseFsmAction.EaseType.easeInOutCirc:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutCirc);
				break;
			case EaseFsmAction.EaseType.linear:
				this.ease = new EaseFsmAction.EasingFunction(this.linear);
				break;
			case EaseFsmAction.EaseType.spring:
				this.ease = new EaseFsmAction.EasingFunction(this.spring);
				break;
			case EaseFsmAction.EaseType.bounce:
				this.ease = new EaseFsmAction.EasingFunction(this.bounce);
				break;
			case EaseFsmAction.EaseType.easeInBack:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInBack);
				break;
			case EaseFsmAction.EaseType.easeOutBack:
				this.ease = new EaseFsmAction.EasingFunction(this.easeOutBack);
				break;
			case EaseFsmAction.EaseType.easeInOutBack:
				this.ease = new EaseFsmAction.EasingFunction(this.easeInOutBack);
				break;
			case EaseFsmAction.EaseType.elastic:
				this.ease = new EaseFsmAction.EasingFunction(this.elastic);
				break;
			}
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x00103334 File Offset: 0x00101534
		protected float linear(float start, float end, float value)
		{
			return Mathf.Lerp(start, end, value);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x00103340 File Offset: 0x00101540
		protected float clerp(float start, float end, float value)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Mathf.Abs((num2 - num) / 2f);
			float num5;
			if (end - start < -num3)
			{
				float num4 = (num2 - start + end) * value;
				num5 = start + num4;
			}
			else if (end - start > num3)
			{
				float num4 = -(num2 - end + start) * value;
				num5 = start + num4;
			}
			else
			{
				num5 = start + (end - start) * value;
			}
			return num5;
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x001033B8 File Offset: 0x001015B8
		protected float spring(float start, float end, float value)
		{
			value = Mathf.Clamp01(value);
			value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
			return start + (end - start) * value;
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x0010341C File Offset: 0x0010161C
		protected float easeInQuad(float start, float end, float value)
		{
			end -= start;
			return end * value * value + start;
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x0010342C File Offset: 0x0010162C
		protected float easeOutQuad(float start, float end, float value)
		{
			end -= start;
			return -end * value * (value - 2f) + start;
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x00103444 File Offset: 0x00101644
		protected float easeInOutQuad(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value + start;
			}
			value -= 1f;
			return -end / 2f * (value * (value - 2f) - 1f) + start;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x0010349C File Offset: 0x0010169C
		protected float easeInCubic(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value + start;
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x001034AC File Offset: 0x001016AC
		protected float easeOutCubic(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * (value * value * value + 1f) + start;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x001034CC File Offset: 0x001016CC
		protected float easeInOutCubic(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value * value + start;
			}
			value -= 2f;
			return end / 2f * (value * value * value + 2f) + start;
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x00103520 File Offset: 0x00101720
		protected float easeInQuart(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value * value + start;
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x00103534 File Offset: 0x00101734
		protected float easeOutQuart(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return -end * (value * value * value * value - 1f) + start;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x00103558 File Offset: 0x00101758
		protected float easeInOutQuart(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value * value * value + start;
			}
			value -= 2f;
			return -end / 2f * (value * value * value * value - 2f) + start;
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x001035B4 File Offset: 0x001017B4
		protected float easeInQuint(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value * value * value + start;
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x001035C8 File Offset: 0x001017C8
		protected float easeOutQuint(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * (value * value * value * value * value + 1f) + start;
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x001035EC File Offset: 0x001017EC
		protected float easeInOutQuint(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value * value * value * value + start;
			}
			value -= 2f;
			return end / 2f * (value * value * value * value * value + 2f) + start;
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x00103648 File Offset: 0x00101848
		protected float easeInSine(float start, float end, float value)
		{
			end -= start;
			return -end * Mathf.Cos(value / 1f * 1.5707964f) + end + start;
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x00103668 File Offset: 0x00101868
		protected float easeOutSine(float start, float end, float value)
		{
			end -= start;
			return end * Mathf.Sin(value / 1f * 1.5707964f) + start;
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x00103688 File Offset: 0x00101888
		protected float easeInOutSine(float start, float end, float value)
		{
			end -= start;
			return -end / 2f * (Mathf.Cos(3.1415927f * value / 1f) - 1f) + start;
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x001036B4 File Offset: 0x001018B4
		protected float easeInExpo(float start, float end, float value)
		{
			end -= start;
			return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x001036DC File Offset: 0x001018DC
		protected float easeOutExpo(float start, float end, float value)
		{
			end -= start;
			return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x00103708 File Offset: 0x00101908
		protected float easeInOutExpo(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
			}
			value -= 1f;
			return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x0010377C File Offset: 0x0010197C
		protected float easeInCirc(float start, float end, float value)
		{
			end -= start;
			return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x0010379C File Offset: 0x0010199C
		protected float easeOutCirc(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * Mathf.Sqrt(1f - value * value) + start;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x001037C0 File Offset: 0x001019C0
		protected float easeInOutCirc(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
			}
			value -= 2f;
			return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x00103830 File Offset: 0x00101A30
		protected float bounce(float start, float end, float value)
		{
			value /= 1f;
			end -= start;
			if (value < 0.36363637f)
			{
				return end * (7.5625f * value * value) + start;
			}
			if (value < 0.72727275f)
			{
				value -= 0.54545456f;
				return end * (7.5625f * value * value + 0.75f) + start;
			}
			if ((double)value < 0.9090909090909091)
			{
				value -= 0.8181818f;
				return end * (7.5625f * value * value + 0.9375f) + start;
			}
			value -= 0.95454544f;
			return end * (7.5625f * value * value + 0.984375f) + start;
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x001038D8 File Offset: 0x00101AD8
		protected float easeInBack(float start, float end, float value)
		{
			end -= start;
			value /= 1f;
			float num = 1.70158f;
			return end * value * value * ((num + 1f) * value - num) + start;
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x0010390C File Offset: 0x00101B0C
		protected float easeOutBack(float start, float end, float value)
		{
			float num = 1.70158f;
			end -= start;
			value = value / 1f - 1f;
			return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x0010394C File Offset: 0x00101B4C
		protected float easeInOutBack(float start, float end, float value)
		{
			float num = 1.70158f;
			end -= start;
			value /= 0.5f;
			if (value < 1f)
			{
				num *= 1.525f;
				return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
			}
			value -= 2f;
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x001039CC File Offset: 0x00101BCC
		protected float punch(float amplitude, float value)
		{
			if (value == 0f)
			{
				return 0f;
			}
			if (value == 1f)
			{
				return 0f;
			}
			float num = 0.3f;
			float num2 = num / 6.2831855f * Mathf.Asin(0f);
			return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x00103A44 File Offset: 0x00101C44
		protected float elastic(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			if (value == 0f)
			{
				return start;
			}
			if ((value /= num) == 1f)
			{
				return start + end;
			}
			float num4;
			if (num3 == 0f || num3 < Mathf.Abs(end))
			{
				num3 = end;
				num4 = num2 / 4f;
			}
			else
			{
				num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
			}
			return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
		}

		// Token: 0x040021C8 RID: 8648
		[RequiredField]
		public FsmFloat time;

		// Token: 0x040021C9 RID: 8649
		public FsmFloat speed;

		// Token: 0x040021CA RID: 8650
		public FsmFloat delay;

		// Token: 0x040021CB RID: 8651
		public EaseFsmAction.EaseType easeType = EaseFsmAction.EaseType.linear;

		// Token: 0x040021CC RID: 8652
		public FsmBool reverse;

		// Token: 0x040021CD RID: 8653
		[Tooltip("Optionally send an Event when the animation finishes.")]
		public FsmEvent finishEvent;

		// Token: 0x040021CE RID: 8654
		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		// Token: 0x040021CF RID: 8655
		protected EaseFsmAction.EasingFunction ease;

		// Token: 0x040021D0 RID: 8656
		protected float runningTime;

		// Token: 0x040021D1 RID: 8657
		protected float lastTime;

		// Token: 0x040021D2 RID: 8658
		protected float startTime;

		// Token: 0x040021D3 RID: 8659
		protected float deltaTime;

		// Token: 0x040021D4 RID: 8660
		protected float delayTime;

		// Token: 0x040021D5 RID: 8661
		protected float percentage;

		// Token: 0x040021D6 RID: 8662
		protected float[] fromFloats = new float[0];

		// Token: 0x040021D7 RID: 8663
		protected float[] toFloats = new float[0];

		// Token: 0x040021D8 RID: 8664
		protected float[] resultFloats = new float[0];

		// Token: 0x040021D9 RID: 8665
		protected bool finishAction;

		// Token: 0x040021DA RID: 8666
		protected bool start;

		// Token: 0x040021DB RID: 8667
		protected bool finished;

		// Token: 0x040021DC RID: 8668
		protected bool isRunning;

		// Token: 0x02000892 RID: 2194
		// (Invoke) Token: 0x060030F1 RID: 12529
		protected delegate float EasingFunction(float start, float end, float value);

		// Token: 0x02000893 RID: 2195
		public enum EaseType
		{
			// Token: 0x040021DE RID: 8670
			easeInQuad,
			// Token: 0x040021DF RID: 8671
			easeOutQuad,
			// Token: 0x040021E0 RID: 8672
			easeInOutQuad,
			// Token: 0x040021E1 RID: 8673
			easeInCubic,
			// Token: 0x040021E2 RID: 8674
			easeOutCubic,
			// Token: 0x040021E3 RID: 8675
			easeInOutCubic,
			// Token: 0x040021E4 RID: 8676
			easeInQuart,
			// Token: 0x040021E5 RID: 8677
			easeOutQuart,
			// Token: 0x040021E6 RID: 8678
			easeInOutQuart,
			// Token: 0x040021E7 RID: 8679
			easeInQuint,
			// Token: 0x040021E8 RID: 8680
			easeOutQuint,
			// Token: 0x040021E9 RID: 8681
			easeInOutQuint,
			// Token: 0x040021EA RID: 8682
			easeInSine,
			// Token: 0x040021EB RID: 8683
			easeOutSine,
			// Token: 0x040021EC RID: 8684
			easeInOutSine,
			// Token: 0x040021ED RID: 8685
			easeInExpo,
			// Token: 0x040021EE RID: 8686
			easeOutExpo,
			// Token: 0x040021EF RID: 8687
			easeInOutExpo,
			// Token: 0x040021F0 RID: 8688
			easeInCirc,
			// Token: 0x040021F1 RID: 8689
			easeOutCirc,
			// Token: 0x040021F2 RID: 8690
			easeInOutCirc,
			// Token: 0x040021F3 RID: 8691
			linear,
			// Token: 0x040021F4 RID: 8692
			spring,
			// Token: 0x040021F5 RID: 8693
			bounce,
			// Token: 0x040021F6 RID: 8694
			easeInBack,
			// Token: 0x040021F7 RID: 8695
			easeOutBack,
			// Token: 0x040021F8 RID: 8696
			easeInOutBack,
			// Token: 0x040021F9 RID: 8697
			elastic,
			// Token: 0x040021FA RID: 8698
			punch
		}
	}
}
