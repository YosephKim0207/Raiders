using System;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001282 RID: 4738
public class HighPriestChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A15 RID: 27157 RVA: 0x002993CC File Offset: 0x002975CC
	private void Start()
	{
		this.m_room = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		List<AIActor> activeEnemies = this.m_room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (activeEnemies[i] && activeEnemies[i].healthHaver && activeEnemies[i].healthHaver.IsBoss)
			{
				this.m_boss = activeEnemies[i];
			}
		}
		if (this.m_boss.behaviorSpeculator)
		{
			for (int j = 0; j < this.m_boss.behaviorSpeculator.AttackBehaviors.Count; j++)
			{
				if (this.m_boss.behaviorSpeculator.AttackBehaviors[j] is AttackBehaviorGroup)
				{
					AttackBehaviorGroup attackBehaviorGroup = this.m_boss.behaviorSpeculator.AttackBehaviors[j] as AttackBehaviorGroup;
					for (int k = 0; k < attackBehaviorGroup.AttackBehaviors.Count; k++)
					{
						if (attackBehaviorGroup.AttackBehaviors[k].Behavior is HighPriestMergoBehavior)
						{
							attackBehaviorGroup.AttackBehaviors[k].Probability = 1000f;
							HighPriestMergoBehavior highPriestMergoBehavior = attackBehaviorGroup.AttackBehaviors[k].Behavior as HighPriestMergoBehavior;
							highPriestMergoBehavior.Cooldown = this.MergoCooldown;
						}
					}
				}
			}
		}
		RoomHandler room = this.m_room;
		room.OnChangedTerrifyingDarkState = (Action<bool>)Delegate.Combine(room.OnChangedTerrifyingDarkState, new Action<bool>(this.HandleDarkStateChange));
	}

	// Token: 0x06006A16 RID: 27158 RVA: 0x00299574 File Offset: 0x00297774
	private void HandleDarkStateChange(bool isDark)
	{
		if (!isDark)
		{
			this.SpawnWave();
		}
	}

	// Token: 0x06006A17 RID: 27159 RVA: 0x00299584 File Offset: 0x00297784
	private void OnDestroy()
	{
		DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(GameManager.Instance.PrimaryPlayer.CenterPosition, 100f);
	}

	// Token: 0x06006A18 RID: 27160 RVA: 0x002995A0 File Offset: 0x002977A0
	private void SpawnWave()
	{
		int numCandles = this.NumCandles;
		for (int i = 0; i < numCandles; i++)
		{
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CandleGuid);
			IntVector2? intVector = this.PreprocessSpawn(orLoadByGuid, this.m_boss.specRigidbody.UnitCenter, this.m_room);
			if (intVector != null)
			{
				AIActor.Spawn(orLoadByGuid, intVector.Value, this.m_room, true, AIActor.AwakenAnimationType.Default, true);
			}
		}
	}

	// Token: 0x06006A19 RID: 27161 RVA: 0x00299614 File Offset: 0x00297814
	private IntVector2? PreprocessSpawn(AIActor enemy, Vector2 center, RoomHandler sourceRoom)
	{
		PixelCollider groundPixelCollider = enemy.specRigidbody.GroundPixelCollider;
		IntVector2 m_enemyClearance;
		if (groundPixelCollider != null && groundPixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Manual)
		{
			m_enemyClearance = new Vector2((float)groundPixelCollider.ManualWidth / 16f, (float)groundPixelCollider.ManualHeight / 16f).ToIntVector2(VectorConversions.Ceil);
		}
		else
		{
			Debug.LogFormat("Enemy type {0} does not have a manually defined ground collider!", new object[] { enemy.name });
			m_enemyClearance = IntVector2.One;
		}
		float minDistanceSquared = 0f;
		float maxDistanceSquared = 400f;
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < m_enemyClearance.x; i++)
			{
				for (int j = 0; j < m_enemyClearance.y; j++)
				{
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
					{
						return false;
					}
				}
			}
			float num = (float)c.x + 0.5f - center.x;
			float num2 = (float)c.y + 0.5f - center.y;
			float num3 = num * num + num2 * num2;
			return num3 >= minDistanceSquared && num3 <= maxDistanceSquared;
		};
		return sourceRoom.GetRandomAvailableCell(new IntVector2?(m_enemyClearance), new CellTypes?(enemy.PathableTiles), true, cellValidator);
	}

	// Token: 0x0400668C RID: 26252
	[EnemyIdentifier]
	public string CandleGuid;

	// Token: 0x0400668D RID: 26253
	public int NumCandles = 6;

	// Token: 0x0400668E RID: 26254
	public float MergoCooldown = 25f;

	// Token: 0x0400668F RID: 26255
	private AIActor m_boss;

	// Token: 0x04006690 RID: 26256
	private RoomHandler m_room;
}
