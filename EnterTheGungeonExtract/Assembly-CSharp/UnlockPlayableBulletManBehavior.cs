using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E02 RID: 3586
public class UnlockPlayableBulletManBehavior : BehaviorBase
{
	// Token: 0x06004BEF RID: 19439 RVA: 0x0019E304 File Offset: 0x0019C504
	public override void Start()
	{
		base.Start();
		if (!this.m_aiActor || this.m_aiActor.sprite)
		{
		}
	}

	// Token: 0x06004BF0 RID: 19440 RVA: 0x0019E334 File Offset: 0x0019C534
	public override void Upkeep()
	{
		base.Upkeep();
	}

	// Token: 0x06004BF1 RID: 19441 RVA: 0x0019E33C File Offset: 0x0019C53C
	public override BehaviorResult Update()
	{
		if (this.m_aiActor.ParentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) == 1)
		{
			this.m_aloneElapsed += BraveTime.DeltaTime;
			if (this.m_aloneElapsed > 3f)
			{
				GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(this.m_aiActor.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
				Debug.Log("Setting a SEEN_SECRET_BULLETMAN flag!");
				GameStatsManager.Instance.SetNextFlag(new GungeonFlags[]
				{
					GungeonFlags.SECRET_BULLETMAN_SEEN_01,
					GungeonFlags.SECRET_BULLETMAN_SEEN_02,
					GungeonFlags.SECRET_BULLETMAN_SEEN_03,
					GungeonFlags.SECRET_BULLETMAN_SEEN_04,
					GungeonFlags.SECRET_BULLETMAN_SEEN_05
				});
				UnityEngine.Object.Destroy(this.m_gameObject);
			}
		}
		return base.Update();
	}

	// Token: 0x040041CC RID: 16844
	private float m_aloneElapsed;
}
