using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200125E RID: 4702
public class BestForLastChallengeModifier : ChallengeModifier
{
	// Token: 0x06006961 RID: 26977 RVA: 0x00294018 File Offset: 0x00292218
	private bool IsValidEnemy(AIActor testEnemy)
	{
		return testEnemy && !testEnemy.IsHarmlessEnemy && (!testEnemy.healthHaver || !testEnemy.healthHaver.PreventAllDamage) && (!testEnemy.GetComponent<ExplodeOnDeath>() || testEnemy.IsSignatureEnemy);
	}

	// Token: 0x06006962 RID: 26978 RVA: 0x00294080 File Offset: 0x00292280
	private void Start()
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		this.m_room = currentRoom;
		currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear, ref this.m_activeEnemies);
		int num = UnityEngine.Random.Range(0, this.m_activeEnemies.Count);
		for (int i = 0; i < this.m_activeEnemies.Count; i++)
		{
			if (i == num)
			{
				if (this.IsValidEnemy(this.m_activeEnemies[i]))
				{
					Vector2 vector = ((!this.m_activeEnemies[i].sprite) ? Vector2.up : (Vector2.up * (this.m_activeEnemies[i].sprite.WorldTopCenter.y - this.m_activeEnemies[i].sprite.WorldBottomCenter.y)));
					GameObject gameObject = this.m_activeEnemies[i].PlayEffectOnActor(this.KingVFX, vector, true, false, false);
					Bounds bounds = this.m_activeEnemies[i].sprite.GetBounds();
					Vector3 vector2 = this.m_activeEnemies[i].transform.position + new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.max.y, 0f).Quantize(0.0625f);
					vector2.y = this.m_activeEnemies[i].transform.position.y + this.m_activeEnemies[i].sprite.GetUntrimmedBounds().max.y;
					vector2.x -= gameObject.GetComponent<tk2dSprite>().GetBounds().extents.x;
					vector2.y -= gameObject.GetComponent<tk2dSprite>().GetBounds().extents.y;
					gameObject.transform.position = vector2;
					this.m_instanceVFX = gameObject;
					ChallengeManager.Instance.StartCoroutine(this.DelayedSpawnIcon());
					this.m_activeEnemies[i].healthHaver.PreventAllDamage = true;
					this.m_king = this.m_activeEnemies[i].healthHaver;
				}
				else
				{
					num++;
				}
			}
		}
		this.m_isActive = true;
	}

	// Token: 0x06006963 RID: 26979 RVA: 0x0029431C File Offset: 0x0029251C
	private IEnumerator DelayedSpawnIcon()
	{
		Bounds vfxBounds = this.m_instanceVFX.GetComponent<tk2dSprite>().GetBounds();
		float elapsed = 0f;
		float duration = 0.25f;
		if (this.m_king && this.m_king.specRigidbody)
		{
			while (this.m_king && this.m_king.specRigidbody.HitboxPixelCollider.UnitDimensions.x <= 0f)
			{
				yield return null;
			}
		}
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			if (this.m_instanceVFX && this.m_king)
			{
				this.m_instanceVFX.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
				if (this.m_king.specRigidbody && this.m_king.specRigidbody.HitboxPixelCollider != null && this.m_king.specRigidbody.HitboxPixelCollider.ColliderGenerationMode != PixelCollider.PixelColliderGeneration.BagelCollider)
				{
					Vector3 vector = this.m_king.specRigidbody.HitboxPixelCollider.UnitTopCenter;
					vector.x -= vfxBounds.extents.x;
					vector.y += vfxBounds.extents.y;
					this.m_instanceVFX.transform.position = vector;
				}
				else
				{
					Bounds bounds = this.m_king.sprite.GetBounds();
					Vector3 vector2 = this.m_king.transform.position + new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.max.y, 0f).Quantize(0.0625f);
					vector2.y = this.m_king.transform.position.y + this.m_king.sprite.GetBounds().max.y;
					vector2.x -= vfxBounds.extents.x;
					vector2.y -= vfxBounds.extents.y;
					this.m_instanceVFX.transform.position = vector2;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006964 RID: 26980 RVA: 0x00294338 File Offset: 0x00292538
	private void Update()
	{
		if (this.m_isActive)
		{
			this.m_room.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear, ref this.m_activeEnemies);
			if (this.m_king && this.m_activeEnemies.Count == 1)
			{
				this.m_king.PreventAllDamage = false;
				if (this.m_instanceVFX)
				{
					this.m_instanceVFX.GetComponent<tk2dSprite>().SetSprite("lastmanstanding_check_001");
				}
			}
		}
	}

	// Token: 0x040065C6 RID: 26054
	public GameObject KingVFX;

	// Token: 0x040065C7 RID: 26055
	private bool m_isActive;

	// Token: 0x040065C8 RID: 26056
	private HealthHaver m_king;

	// Token: 0x040065C9 RID: 26057
	private RoomHandler m_room;

	// Token: 0x040065CA RID: 26058
	private List<AIActor> m_activeEnemies = new List<AIActor>();

	// Token: 0x040065CB RID: 26059
	private GameObject m_instanceVFX;
}
