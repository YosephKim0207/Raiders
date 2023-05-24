using System;
using System.Collections.Generic;
using Dungeonator;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA7 RID: 3239
	[Tooltip("Unlocks all truth chests in the current room")]
	[ActionCategory(".NPCs")]
	public class OpenTruthChest : FsmStateAction
	{
		// Token: 0x06004534 RID: 17716 RVA: 0x00166A98 File Offset: 0x00164C98
		public override void Reset()
		{
			this.delay = 0f;
			this.openOnEarlyFinish = true;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x00166AB8 File Offset: 0x00164CB8
		public override void OnEnter()
		{
			this.m_opened = false;
			if (this.delay.Value <= 0f)
			{
				this.OpenChest();
				base.Finish();
			}
			else
			{
				this.m_vanishTimer = this.delay.Value;
			}
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x00166AF8 File Offset: 0x00164CF8
		public override void OnUpdate()
		{
			this.m_vanishTimer -= BraveTime.DeltaTime;
			if (this.m_vanishTimer <= 0f)
			{
				this.OpenChest();
				base.Finish();
			}
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x00166B28 File Offset: 0x00164D28
		public override void OnExit()
		{
			if (this.openOnEarlyFinish.Value && !this.m_opened)
			{
				this.OpenChest();
			}
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x00166B4C File Offset: 0x00164D4C
		private void OpenChest()
		{
			if (this.m_opened)
			{
				return;
			}
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.Owner.transform.position.IntXY(VectorConversions.Floor));
			List<Chest> componentsInRoom = roomFromPosition.GetComponentsInRoom<Chest>();
			for (int i = 0; i < componentsInRoom.Count; i++)
			{
				if (componentsInRoom[i].name.ToLowerInvariant().Contains("truth"))
				{
					componentsInRoom[i].IsLocked = false;
					componentsInRoom[i].IsSealed = false;
					tk2dSpriteAnimator componentInChildren = componentsInRoom[i].transform.Find("lock").GetComponentInChildren<tk2dSpriteAnimator>();
					if (componentInChildren != null)
					{
						componentInChildren.StopAndResetFrame();
						componentInChildren.PlayAndDestroyObject("truth_lock_open", null);
					}
				}
			}
			this.m_opened = true;
		}

		// Token: 0x0400375B RID: 14171
		[Tooltip("Seconds to wait before opening the chest.")]
		public FsmFloat delay;

		// Token: 0x0400375C RID: 14172
		[Tooltip("If true, the chest will open if this action ends early.")]
		public FsmBool openOnEarlyFinish;

		// Token: 0x0400375D RID: 14173
		private float m_vanishTimer;

		// Token: 0x0400375E RID: 14174
		private bool m_opened;
	}
}
