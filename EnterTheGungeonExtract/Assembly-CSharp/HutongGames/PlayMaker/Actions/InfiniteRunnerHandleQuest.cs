using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA0 RID: 3232
	public class InfiniteRunnerHandleQuest : FsmStateAction
	{
		// Token: 0x0600451D RID: 17693 RVA: 0x001665A4 File Offset: 0x001647A4
		public override void Awake()
		{
			base.Awake();
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x001665C0 File Offset: 0x001647C0
		public override void OnEnter()
		{
			this.m_lastPosition = this.m_talkDoer.specRigidbody.UnitCenter;
			base.Owner.GetComponent<InfiniteRunnerController>().StartQuest();
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x001665E8 File Offset: 0x001647E8
		public override void OnUpdate()
		{
			base.OnUpdate();
			this.m_elapsed += BraveTime.DeltaTime;
			if (this.m_elapsed < 0.75f)
			{
				return;
			}
			if (this.m_talkDoer.CurrentPath != null)
			{
				this.m_talkDoer.specRigidbody.Velocity = this.m_talkDoer.GetPathVelocityContribution(this.m_lastPosition, 32);
				this.m_lastPosition = this.m_talkDoer.specRigidbody.UnitCenter;
			}
			else
			{
				base.Finish();
			}
		}

		// Token: 0x04003747 RID: 14151
		private TalkDoerLite m_talkDoer;

		// Token: 0x04003748 RID: 14152
		private Vector2 m_lastPosition;

		// Token: 0x04003749 RID: 14153
		private float m_elapsed;
	}
}
