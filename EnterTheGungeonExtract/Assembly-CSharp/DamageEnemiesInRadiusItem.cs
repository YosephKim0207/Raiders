using System;
using UnityEngine;

// Token: 0x02001393 RID: 5011
public class DamageEnemiesInRadiusItem : AffectEnemiesInRadiusItem
{
	// Token: 0x06007189 RID: 29065 RVA: 0x002D1C24 File Offset: 0x002CFE24
	protected override void DoEffect(PlayerController user)
	{
		if (this.PreventsReinforcements && user.CurrentRoom.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS)
		{
			user.CurrentRoom.ClearReinforcementLayers();
		}
		base.DoEffect(user);
	}

	// Token: 0x0600718A RID: 29066 RVA: 0x002D1C5C File Offset: 0x002CFE5C
	protected override void AffectEnemy(AIActor target)
	{
		if (target.healthHaver)
		{
			target.healthHaver.ApplyDamage(this.Damage, Vector2.zero, string.Empty, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
		}
	}

	// Token: 0x040072E8 RID: 29416
	public float Damage = 10f;

	// Token: 0x040072E9 RID: 29417
	public bool PreventsReinforcements;
}
