using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000992 RID: 2450
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Gets Location Info from a mobile device. NOTE: Use StartLocationService before trying to get location info.")]
	public class GetLocationInfo : FsmStateAction
	{
		// Token: 0x06003538 RID: 13624 RVA: 0x00112A9C File Offset: 0x00110C9C
		public override void Reset()
		{
			this.longitude = null;
			this.latitude = null;
			this.altitude = null;
			this.horizontalAccuracy = null;
			this.verticalAccuracy = null;
			this.errorEvent = null;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x00112AC8 File Offset: 0x00110CC8
		public override void OnEnter()
		{
			this.DoGetLocationInfo();
			base.Finish();
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x00112AD8 File Offset: 0x00110CD8
		private void DoGetLocationInfo()
		{
		}

		// Token: 0x0400269A RID: 9882
		[UIHint(UIHint.Variable)]
		public FsmVector3 vectorPosition;

		// Token: 0x0400269B RID: 9883
		[UIHint(UIHint.Variable)]
		public FsmFloat longitude;

		// Token: 0x0400269C RID: 9884
		[UIHint(UIHint.Variable)]
		public FsmFloat latitude;

		// Token: 0x0400269D RID: 9885
		[UIHint(UIHint.Variable)]
		public FsmFloat altitude;

		// Token: 0x0400269E RID: 9886
		[UIHint(UIHint.Variable)]
		public FsmFloat horizontalAccuracy;

		// Token: 0x0400269F RID: 9887
		[UIHint(UIHint.Variable)]
		public FsmFloat verticalAccuracy;

		// Token: 0x040026A0 RID: 9888
		[Tooltip("Event to send if the location cannot be queried.")]
		public FsmEvent errorEvent;
	}
}
