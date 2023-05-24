using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B6 RID: 2486
	[Tooltip("Gets various useful Time measurements.")]
	[ActionCategory(ActionCategory.Time)]
	public class GetTimeInfo : FsmStateAction
	{
		// Token: 0x060035D3 RID: 13779 RVA: 0x00114258 File Offset: 0x00112458
		public override void Reset()
		{
			this.getInfo = GetTimeInfo.TimeInfo.TimeSinceLevelLoad;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x00114270 File Offset: 0x00112470
		public override void OnEnter()
		{
			this.DoGetTimeInfo();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x0011428C File Offset: 0x0011248C
		public override void OnUpdate()
		{
			this.DoGetTimeInfo();
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x00114294 File Offset: 0x00112494
		private void DoGetTimeInfo()
		{
			switch (this.getInfo)
			{
			case GetTimeInfo.TimeInfo.DeltaTime:
				this.storeValue.Value = Time.deltaTime;
				break;
			case GetTimeInfo.TimeInfo.TimeScale:
				this.storeValue.Value = Time.timeScale;
				break;
			case GetTimeInfo.TimeInfo.SmoothDeltaTime:
				this.storeValue.Value = Time.smoothDeltaTime;
				break;
			case GetTimeInfo.TimeInfo.TimeInCurrentState:
				this.storeValue.Value = base.State.StateTime;
				break;
			case GetTimeInfo.TimeInfo.TimeSinceStartup:
				this.storeValue.Value = Time.time;
				break;
			case GetTimeInfo.TimeInfo.TimeSinceLevelLoad:
				this.storeValue.Value = Time.timeSinceLevelLoad;
				break;
			case GetTimeInfo.TimeInfo.RealTimeSinceStartup:
				this.storeValue.Value = FsmTime.RealtimeSinceStartup;
				break;
			case GetTimeInfo.TimeInfo.RealTimeInCurrentState:
				this.storeValue.Value = FsmTime.RealtimeSinceStartup - base.State.RealStartTime;
				break;
			default:
				this.storeValue.Value = 0f;
				break;
			}
		}

		// Token: 0x04002715 RID: 10005
		public GetTimeInfo.TimeInfo getInfo;

		// Token: 0x04002716 RID: 10006
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeValue;

		// Token: 0x04002717 RID: 10007
		public bool everyFrame;

		// Token: 0x020009B7 RID: 2487
		public enum TimeInfo
		{
			// Token: 0x04002719 RID: 10009
			DeltaTime,
			// Token: 0x0400271A RID: 10010
			TimeScale,
			// Token: 0x0400271B RID: 10011
			SmoothDeltaTime,
			// Token: 0x0400271C RID: 10012
			TimeInCurrentState,
			// Token: 0x0400271D RID: 10013
			TimeSinceStartup,
			// Token: 0x0400271E RID: 10014
			TimeSinceLevelLoad,
			// Token: 0x0400271F RID: 10015
			RealTimeSinceStartup,
			// Token: 0x04002720 RID: 10016
			RealTimeInCurrentState
		}
	}
}
