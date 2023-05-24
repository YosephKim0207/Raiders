using System;
using Dungeonator;

// Token: 0x020010AE RID: 4270
public class MakeVulnerableOnRoomClear : BraveBehaviour
{
	// Token: 0x06005E2B RID: 24107 RVA: 0x00241DE4 File Offset: 0x0023FFE4
	public void Start()
	{
		RoomHandler parentRoom = base.aiActor.ParentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
	}

	// Token: 0x06005E2C RID: 24108 RVA: 0x00241E14 File Offset: 0x00240014
	protected override void OnDestroy()
	{
		if (base.aiActor && base.aiActor.ParentRoom != null)
		{
			RoomHandler parentRoom = base.aiActor.ParentRoom;
			parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
		}
		base.OnDestroy();
	}

	// Token: 0x06005E2D RID: 24109 RVA: 0x00241E74 File Offset: 0x00240074
	private void RoomCleared()
	{
		if (base.healthHaver.PreventAllDamage)
		{
			base.healthHaver.PreventAllDamage = false;
			if (!string.IsNullOrEmpty(this.vulnerableAnim))
			{
				base.aiAnimator.PlayUntilCancelled(this.vulnerableAnim, true, null, -1f, false);
			}
			if (this.disableBehaviors)
			{
				base.aiActor.enabled = false;
				base.aiActor.IsHarmlessEnemy = true;
				base.behaviorSpeculator.InterruptAndDisable();
			}
			base.aiActor.CollisionDamage = 0f;
			base.aiActor.CollisionKnockbackStrength = 0f;
		}
	}

	// Token: 0x0400583C RID: 22588
	[CheckDirectionalAnimation(null)]
	public string vulnerableAnim;

	// Token: 0x0400583D RID: 22589
	public bool disableBehaviors = true;
}
