using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001285 RID: 4741
public class KingEnemyChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A21 RID: 27169 RVA: 0x00299890 File Offset: 0x00297A90
	private bool IsValidEnemy(AIActor testEnemy)
	{
		return testEnemy && !testEnemy.IsHarmlessEnemy && (!testEnemy.healthHaver || !testEnemy.healthHaver.PreventAllDamage) && (!testEnemy.GetComponent<ExplodeOnDeath>() || testEnemy.IsSignatureEnemy);
	}

	// Token: 0x06006A22 RID: 27170 RVA: 0x002998F8 File Offset: 0x00297AF8
	private void Start()
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		int num = UnityEngine.Random.Range(0, activeEnemies.Count);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (i == num)
			{
				if (this.IsValidEnemy(activeEnemies[i]))
				{
					Vector2 vector = ((!activeEnemies[i].sprite) ? Vector2.up : (Vector2.up * (activeEnemies[i].sprite.WorldTopCenter.y - activeEnemies[i].sprite.WorldBottomCenter.y)));
					GameObject gameObject = activeEnemies[i].PlayEffectOnActor(this.KingVFX, vector, true, false, false);
					if (activeEnemies[i].OverrideBuffEffectPosition)
					{
						Vector3 position = activeEnemies[i].OverrideBuffEffectPosition.position;
						position.x -= gameObject.GetComponent<tk2dSprite>().GetBounds().extents.x;
						gameObject.transform.position = position;
					}
					else if (activeEnemies[i].healthHaver && activeEnemies[i].healthHaver.IsBoss)
					{
						Vector3 vector2 = activeEnemies[i].specRigidbody.HitboxPixelCollider.UnitTopCenter;
						vector2.x -= gameObject.GetComponent<tk2dSprite>().GetBounds().extents.x;
						vector2.y += gameObject.GetComponent<tk2dSprite>().GetBounds().extents.y;
						gameObject.transform.position = vector2;
					}
					else
					{
						Bounds bounds = activeEnemies[i].sprite.GetBounds();
						Vector3 vector3 = activeEnemies[i].transform.position + new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.max.y, 0f).Quantize(0.0625f);
						vector3.y = activeEnemies[i].transform.position.y + activeEnemies[i].sprite.GetUntrimmedBounds().max.y;
						vector3.x -= gameObject.GetComponent<tk2dSprite>().GetBounds().extents.x;
						gameObject.transform.position = vector3;
					}
					activeEnemies[i].healthHaver.OnDeath += this.OnKingDeath;
					this.m_king = activeEnemies[i].healthHaver;
				}
				else
				{
					num++;
				}
			}
			else if (activeEnemies[i] && activeEnemies[i].healthHaver && !activeEnemies[i].IsMimicEnemy)
			{
				activeEnemies[i].healthHaver.PreventAllDamage = true;
			}
		}
		this.m_isActive = true;
	}

	// Token: 0x06006A23 RID: 27171 RVA: 0x00299C7C File Offset: 0x00297E7C
	private void Update()
	{
		if (this.m_isActive && (!this.m_king || this.m_king.IsDead))
		{
			this.m_isActive = false;
			this.OnKingDeath(Vector2.zero);
		}
	}

	// Token: 0x06006A24 RID: 27172 RVA: 0x00299CBC File Offset: 0x00297EBC
	private void OnKingDeath(Vector2 obj)
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (activeEnemies[i] && activeEnemies[i].healthHaver)
			{
				activeEnemies[i].healthHaver.PreventAllDamage = false;
			}
		}
	}

	// Token: 0x04006696 RID: 26262
	public GameObject KingVFX;

	// Token: 0x04006697 RID: 26263
	private bool m_isActive;

	// Token: 0x04006698 RID: 26264
	private HealthHaver m_king;
}
