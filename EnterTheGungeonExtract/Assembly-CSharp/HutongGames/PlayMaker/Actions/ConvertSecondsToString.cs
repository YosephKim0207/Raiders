using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200091F RID: 2335
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=1711.0")]
	[Tooltip("Converts Seconds to a String value representing the time.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertSecondsToString : FsmStateAction
	{
		// Token: 0x06003360 RID: 13152 RVA: 0x0010CDAC File Offset: 0x0010AFAC
		public override void Reset()
		{
			this.secondsVariable = null;
			this.stringVariable = null;
			this.everyFrame = false;
			this.format = "{1:D2}h:{2:D2}m:{3:D2}s:{10}ms";
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x0010CDD4 File Offset: 0x0010AFD4
		public override void OnEnter()
		{
			this.DoConvertSecondsToString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x0010CDF0 File Offset: 0x0010AFF0
		public override void OnUpdate()
		{
			this.DoConvertSecondsToString();
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x0010CDF8 File Offset: 0x0010AFF8
		private void DoConvertSecondsToString()
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.secondsVariable.Value);
			string text = timeSpan.Milliseconds.ToString("D3").PadLeft(2, '0');
			text = text.Substring(0, 2);
			this.stringVariable.Value = string.Format(this.format.Value, new object[]
			{
				timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, timeSpan.TotalDays, timeSpan.TotalHours, timeSpan.TotalMinutes, timeSpan.TotalSeconds, timeSpan.TotalMilliseconds,
				text
			});
		}

		// Token: 0x0400248C RID: 9356
		[Tooltip("The seconds variable to convert.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat secondsVariable;

		// Token: 0x0400248D RID: 9357
		[Tooltip("A string variable to store the time value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x0400248E RID: 9358
		[Tooltip("Format. 0 for days, 1 is for hours, 2 for minutes, 3 for seconds and 4 for milliseconds. 5 for total days, 6 for total hours, 7 for total minutes, 8 for total seconds, 9 for total milliseconds, 10 for two digits milliseconds. so {2:D2} would just show the seconds of the current time, NOT the grand total number of seconds, the grand total of seconds would be {8:F0}")]
		[RequiredField]
		public FsmString format;

		// Token: 0x0400248F RID: 9359
		[Tooltip("Repeat every frame. Useful if the seconds variable is changing.")]
		public bool everyFrame;
	}
}
