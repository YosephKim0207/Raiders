using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001359 RID: 4953
public class BombCompanionAppItem : PlayerItem
{
	// Token: 0x06007057 RID: 28759 RVA: 0x002C8D80 File Offset: 0x002C6F80
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_computer_boop_01", base.gameObject);
		RoomHandler currentRoom = user.CurrentRoom;
		if (currentRoom == null)
		{
			return;
		}
		for (int i = 0; i < StaticReferenceManager.AllMinorBreakables.Count; i++)
		{
			MinorBreakable minorBreakable = StaticReferenceManager.AllMinorBreakables[i];
			if (minorBreakable.transform.position.GetAbsoluteRoom() == currentRoom)
			{
				if (!minorBreakable.IsBroken && minorBreakable.explodesOnBreak)
				{
					minorBreakable.Break();
				}
			}
		}
		List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null)
		{
			for (int j = activeEnemies.Count - 1; j >= 0; j--)
			{
				AIActor aiactor = activeEnemies[j];
				if (aiactor && !aiactor.IsSignatureEnemy)
				{
					HealthHaver healthHaver = aiactor.healthHaver;
					if (healthHaver && !healthHaver.IsDead && !healthHaver.IsBoss)
					{
						ExplodeOnDeath component = aiactor.GetComponent<ExplodeOnDeath>();
						if (component && !component.immuneToIBombApp)
						{
							healthHaver.ApplyDamage(2.1474836E+09f, Vector2.zero, "iBomb", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
						}
					}
				}
			}
		}
		List<Projectile> list = new List<Projectile>();
		for (int k = 0; k < StaticReferenceManager.AllProjectiles.Count; k++)
		{
			if (StaticReferenceManager.AllProjectiles[k] && StaticReferenceManager.AllProjectiles[k].GetComponent<ExplosiveModifier>() != null)
			{
				list.Add(StaticReferenceManager.AllProjectiles[k]);
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			list[l].DieInAir(false, true, true, false);
		}
	}

	// Token: 0x06007058 RID: 28760 RVA: 0x002C8F64 File Offset: 0x002C7164
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
