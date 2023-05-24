using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B23 RID: 2851
	[Tooltip("Starts location service updates. Last location coordinates can be retrieved with GetLocationInfo.")]
	[ActionCategory(ActionCategory.Device)]
	public class StartLocationServiceUpdates : FsmStateAction
	{
		// Token: 0x06003C10 RID: 15376 RVA: 0x0012EA10 File Offset: 0x0012CC10
		public override void Reset()
		{
			this.maxWait = 20f;
			this.desiredAccuracy = 10f;
			this.updateDistance = 10f;
			this.successEvent = null;
			this.failedEvent = null;
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x0012EA50 File Offset: 0x0012CC50
		public override void OnEnter()
		{
			base.Finish();
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x0012EA58 File Offset: 0x0012CC58
		public override void OnUpdate()
		{
		}

		// Token: 0x04002E40 RID: 11840
		[Tooltip("Maximum time to wait in seconds before failing.")]
		public FsmFloat maxWait;

		// Token: 0x04002E41 RID: 11841
		public FsmFloat desiredAccuracy;

		// Token: 0x04002E42 RID: 11842
		public FsmFloat updateDistance;

		// Token: 0x04002E43 RID: 11843
		[Tooltip("Event to send when the location services have started.")]
		public FsmEvent successEvent;

		// Token: 0x04002E44 RID: 11844
		[Tooltip("Event to send if the location services fail to start.")]
		public FsmEvent failedEvent;
	}
}
