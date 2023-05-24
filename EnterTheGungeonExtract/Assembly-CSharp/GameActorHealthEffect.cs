using System;
using UnityEngine;

// Token: 0x02000E28 RID: 3624
[Serializable]
public class GameActorHealthEffect : GameActorEffect
{
	// Token: 0x06004CB1 RID: 19633 RVA: 0x001A36D0 File Offset: 0x001A18D0
	public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		if (this.AffectsEnemies && actor is AIActor)
		{
			actor.healthHaver.ApplyDamage(this.DamagePerSecondToEnemies * BraveTime.DeltaTime, Vector2.zero, this.effectIdentifier, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);
		}
		if (this.ignitesGoops)
		{
			DeadlyDeadlyGoopManager.IgniteGoopsCircle(actor.CenterPosition, 0.5f);
		}
	}

	// Token: 0x040042BF RID: 17087
	public float DamagePerSecondToEnemies = 10f;

	// Token: 0x040042C0 RID: 17088
	public bool ignitesGoops;
}
