using System;
using UnityEngine;

// Token: 0x020010AA RID: 4266
public class KillOnRoomUnseal : BraveBehaviour
{
	// Token: 0x06005E0C RID: 24076 RVA: 0x0024136C File Offset: 0x0023F56C
	public void Update()
	{
		if (base.aiActor.enabled && base.behaviorSpeculator.enabled && !base.aiActor.ParentRoom.IsSealed)
		{
			if (base.aiAnimator.IsPlaying("spawn") || base.aiAnimator.IsPlaying("awaken"))
			{
				return;
			}
			base.enabled = false;
			base.healthHaver.PreventAllDamage = false;
			base.healthHaver.ApplyDamage(100000f, Vector2.zero, "Room Clear", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
		}
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x0024140C File Offset: 0x0023F60C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
