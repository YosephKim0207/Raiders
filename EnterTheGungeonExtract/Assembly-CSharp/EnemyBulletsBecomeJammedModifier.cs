using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020013AE RID: 5038
public class EnemyBulletsBecomeJammedModifier : MonoBehaviour
{
	// Token: 0x0600722C RID: 29228 RVA: 0x002D5F7C File Offset: 0x002D417C
	private void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_sprite = this.m_projectile.sprite;
	}

	// Token: 0x0600722D RID: 29229 RVA: 0x002D5F9C File Offset: 0x002D419C
	private void Update()
	{
		if (Dungeon.IsGenerating)
		{
			return;
		}
		Vector2 vector = ((!this.m_sprite) ? this.m_projectile.transform.position.XY() : this.m_sprite.WorldCenter);
		for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.AllProjectiles[i];
			if (projectile && projectile.Owner is AIActor && !projectile.IsBlackBullet)
			{
				float sqrMagnitude = (projectile.transform.position.XY() - vector).sqrMagnitude;
				if (sqrMagnitude < this.EffectRadius)
				{
					projectile.BecomeBlackBullet();
				}
			}
		}
	}

	// Token: 0x04007393 RID: 29587
	public float EffectRadius = 1f;

	// Token: 0x04007394 RID: 29588
	private Projectile m_projectile;

	// Token: 0x04007395 RID: 29589
	private tk2dBaseSprite m_sprite;
}
