using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200138E RID: 5006
public class CorpseExplodeActiveItem : PlayerItem
{
	// Token: 0x06007175 RID: 29045 RVA: 0x002D1318 File Offset: 0x002CF518
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		this.m_bulletBank = base.GetComponent<AIBulletBank>();
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
	}

	// Token: 0x06007176 RID: 29046 RVA: 0x002D13C0 File Offset: 0x002CF5C0
	protected override void OnPreDrop(PlayerController player)
	{
		base.OnPreDrop(player);
		if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
	}

	// Token: 0x06007177 RID: 29047 RVA: 0x002D1458 File Offset: 0x002CF658
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_dead_again_01", base.gameObject);
		bool flag = false;
		for (int i = 0; i < StaticReferenceManager.AllCorpses.Count; i++)
		{
			GameObject gameObject = StaticReferenceManager.AllCorpses[i];
			if (gameObject && gameObject.GetComponent<tk2dBaseSprite>() && gameObject.transform.position.GetAbsoluteRoom() == user.CurrentRoom)
			{
				flag = true;
				Vector2 worldCenter = gameObject.GetComponent<tk2dBaseSprite>().WorldCenter;
				Exploder.Explode(worldCenter, this.CorpseExplosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
				if (user.HasActiveBonusSynergy(CustomSynergyType.CORPSE_EXPLOSHOOT, false))
				{
					float num = -1f;
					AIActor nearestEnemy = user.CurrentRoom.GetNearestEnemy(worldCenter, out num, true, false);
					if (nearestEnemy)
					{
						this.FireBullet(worldCenter, nearestEnemy.CenterPosition - worldCenter);
					}
				}
				if (user.HasActiveBonusSynergy(CustomSynergyType.CRISIS_ROCK, false))
				{
					UnityEngine.Object.Instantiate<GameObject>(this.ShieldForCrisisStoneSynergy, worldCenter, Quaternion.identity);
				}
				UnityEngine.Object.Destroy(gameObject.gameObject);
			}
		}
		if (flag)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(this.ScreenShake, new Vector2?(user.CenterPosition), false);
		}
		else
		{
			base.ClearCooldowns();
		}
	}

	// Token: 0x06007178 RID: 29048 RVA: 0x002D15B0 File Offset: 0x002CF7B0
	protected override void OnDestroy()
	{
		if (this.m_pickedUp && this.LastOwner && PassiveItem.ActiveFlagItems != null && PassiveItem.ActiveFlagItems.ContainsKey(this.LastOwner) && PassiveItem.ActiveFlagItems[this.LastOwner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.LastOwner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.LastOwner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.LastOwner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.LastOwner].Remove(base.GetType());
			}
		}
		base.OnDestroy();
	}

	// Token: 0x06007179 RID: 29049 RVA: 0x002D1698 File Offset: 0x002CF898
	private void FireBullet(Vector3 shootPoint, Vector2 direction)
	{
		GameObject gameObject = this.m_bulletBank.CreateProjectileFromBank(shootPoint, BraveMathCollege.Atan2Degrees(direction), "default", null, false, true, false);
		Projectile component = gameObject.GetComponent<Projectile>();
		if (component && this.LastOwner)
		{
			component.collidesWithPlayer = false;
			component.collidesWithEnemies = true;
			component.SetOwnerSafe(this.LastOwner, this.LastOwner.ActorName);
			component.SetNewShooter(this.LastOwner.specRigidbody);
			component.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyBulletBlocker));
		}
	}

	// Token: 0x04007135 RID: 28981
	public ScreenShakeSettings ScreenShake;

	// Token: 0x04007136 RID: 28982
	public ExplosionData CorpseExplosionData;

	// Token: 0x04007137 RID: 28983
	public bool UsesCrisisStoneSynergy;

	// Token: 0x04007138 RID: 28984
	public GameObject ShieldForCrisisStoneSynergy;

	// Token: 0x04007139 RID: 28985
	private AIBulletBank m_bulletBank;
}
