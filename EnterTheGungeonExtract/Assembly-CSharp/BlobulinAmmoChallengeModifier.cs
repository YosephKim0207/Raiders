using System;
using Dungeonator;

// Token: 0x02001260 RID: 4704
public class BlobulinAmmoChallengeModifier : ChallengeModifier
{
	// Token: 0x0600696C RID: 26988 RVA: 0x002947AC File Offset: 0x002929AC
	private void Start()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].PostProcessProjectile += this.ModifyProjectile;
		}
	}

	// Token: 0x0600696D RID: 26989 RVA: 0x002947F4 File Offset: 0x002929F4
	private void ModifyProjectile(Projectile proj, float somethin)
	{
		if (proj && proj.Owner is PlayerController && !proj.SpawnedFromNonChallengeItem && !proj.TreatedAsNonProjectileForChallenge && !(proj is InstantDamageOneEnemyProjectile) && !(proj is InstantlyDamageAllProjectile))
		{
			proj.OnDestruction += this.HandleProjectileDeath;
		}
	}

	// Token: 0x0600696E RID: 26990 RVA: 0x0029485C File Offset: 0x00292A5C
	private bool CellIsValid(IntVector2 cellPos)
	{
		if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(cellPos))
		{
			CellData cellData = GameManager.Instance.Dungeon.data[cellPos];
			return (cellData == null || cellData.parentRoom == null || cellData.parentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) != 0) && (cellData != null && cellData.type == CellType.FLOOR && cellData.IsPassable && cellData.parentRoom == GameManager.Instance.BestActivePlayer.CurrentRoom && !cellData.isExitCell);
		}
		return false;
	}

	// Token: 0x0600696F RID: 26991 RVA: 0x00294904 File Offset: 0x00292B04
	private void Update()
	{
		this.m_cooldown -= BraveTime.DeltaTime;
	}

	// Token: 0x06006970 RID: 26992 RVA: 0x00294918 File Offset: 0x00292B18
	private void HandleProjectileDeath(Projectile obj)
	{
		if (!this)
		{
			return;
		}
		if (obj && !obj.HasImpactedEnemy && !obj.HasDiedInAir)
		{
			float num = 0f;
			GameManager.Instance.GetPlayerClosestToPoint(obj.specRigidbody.UnitCenter, out num);
			if (num < this.SafeRadius)
			{
				return;
			}
			IntVector2 intVector = obj.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round);
			if (GameManager.Instance.Dungeon.data.isFaceWallHigher(intVector.x, intVector.y))
			{
				intVector += new IntVector2(0, -2);
			}
			else if (GameManager.Instance.Dungeon.data.isFaceWallLower(intVector.x, intVector.y))
			{
				intVector += new IntVector2(0, -1);
			}
			bool flag = this.CellIsValid(intVector);
			if (!flag)
			{
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						IntVector2 intVector2 = intVector + new IntVector2(i, j);
						flag = this.CellIsValid(intVector2);
						if (flag)
						{
							intVector = intVector2;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					IntVector2? nearestAvailableCell = GameManager.Instance.PrimaryPlayer.CurrentRoom.GetNearestAvailableCell(obj.specRigidbody.UnitCenter, new IntVector2?(IntVector2.One), new CellTypes?(CellTypes.FLOOR), false, null);
					if (nearestAvailableCell != null)
					{
						flag = true;
						intVector = nearestAvailableCell.Value;
					}
				}
			}
			if (obj.Owner is PlayerController)
			{
				if (!(obj.Owner as PlayerController).IsInCombat)
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
			if (flag && this.m_cooldown <= 0f)
			{
				this.m_cooldown = this.CooldownBetweenSpawns;
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.SpawnTargetGuid);
				AIActor.Spawn(orLoadByGuid, intVector, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector), true, AIActor.AwakenAnimationType.Default, true);
			}
		}
	}

	// Token: 0x06006971 RID: 26993 RVA: 0x00294B34 File Offset: 0x00292D34
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].PostProcessProjectile -= this.ModifyProjectile;
		}
	}

	// Token: 0x040065D4 RID: 26068
	[EnemyIdentifier]
	public string SpawnTargetGuid;

	// Token: 0x040065D5 RID: 26069
	public float CooldownBetweenSpawns = 0.2f;

	// Token: 0x040065D6 RID: 26070
	private float m_cooldown;

	// Token: 0x040065D7 RID: 26071
	public float SafeRadius = 3f;
}
