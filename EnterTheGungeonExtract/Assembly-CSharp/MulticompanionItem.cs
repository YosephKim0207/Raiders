using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001441 RID: 5185
public class MulticompanionItem : PassiveItem
{
	// Token: 0x060075B1 RID: 30129 RVA: 0x002EDDA0 File Offset: 0x002EBFA0
	private void CreateNewCompanion(PlayerController owner)
	{
		int num = ((!this.HasSynergy || !this.m_synergyActive) ? this.MaxNumberOfCompanions : this.SynergyMaxNumberOfCompanions);
		if (this.m_companions.Count >= num && num >= 0)
		{
			return;
		}
		string text = ((!this.HasSynergy || !this.m_synergyActive) ? this.CompanionGuid : this.SynergyCompanionGuid);
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(text);
		Vector3 vector = owner.transform.position;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			vector += new Vector3(1.125f, -0.3125f, 0f);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
		CompanionController orAddComponent = gameObject.GetOrAddComponent<CompanionController>();
		this.m_companions.Add(orAddComponent);
		orAddComponent.Initialize(owner);
		if (orAddComponent.specRigidbody)
		{
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
		}
	}

	// Token: 0x060075B2 RID: 30130 RVA: 0x002EDEB4 File Offset: 0x002EC0B4
	private void DestroyAllCompanions()
	{
		for (int i = this.m_companions.Count - 1; i >= 0; i--)
		{
			if (this.m_companions[i])
			{
				UnityEngine.Object.Destroy(this.m_companions[i].gameObject);
			}
			this.m_companions.RemoveAt(i);
		}
	}

	// Token: 0x060075B3 RID: 30131 RVA: 0x002EDF18 File Offset: 0x002EC118
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		if (this.TriggersOnRoomClear)
		{
			player.OnRoomClearEvent += this.HandleRoomCleared;
		}
		if (this.TriggersOnEnemyKill)
		{
			player.OnKilledEnemy += this.HandleEnemyKilled;
		}
		this.CreateNewCompanion(player);
	}

	// Token: 0x060075B4 RID: 30132 RVA: 0x002EDF90 File Offset: 0x002EC190
	private void HandleEnemyKilled(PlayerController p)
	{
		this.CreateNewCompanion(p);
	}

	// Token: 0x060075B5 RID: 30133 RVA: 0x002EDF9C File Offset: 0x002EC19C
	private void HandleRoomCleared(PlayerController p)
	{
		this.CreateNewCompanion(p);
	}

	// Token: 0x060075B6 RID: 30134 RVA: 0x002EDFA8 File Offset: 0x002EC1A8
	protected override void Update()
	{
		base.Update();
		for (int i = this.m_companions.Count - 1; i >= 0; i--)
		{
			if (!this.m_companions[i])
			{
				this.m_companions.RemoveAt(i);
			}
			else if (this.m_companions[i].healthHaver && this.m_companions[i].healthHaver.IsDead)
			{
				UnityEngine.Object.Destroy(this.m_companions[i].gameObject);
				this.m_companions.RemoveAt(i);
			}
		}
		if (this.m_owner && this.HasSynergy)
		{
			if (this.m_synergyActive && !this.m_owner.HasActiveBonusSynergy(this.RequiredSynergy, false))
			{
				this.DestroyAllCompanions();
				this.m_synergyActive = false;
			}
			else if (!this.m_synergyActive && this.m_owner.HasActiveBonusSynergy(this.RequiredSynergy, false))
			{
				this.DestroyAllCompanions();
				this.m_synergyActive = true;
			}
		}
	}

	// Token: 0x060075B7 RID: 30135 RVA: 0x002EE0DC File Offset: 0x002EC2DC
	private void HandleNewFloor(PlayerController obj)
	{
		this.DestroyAllCompanions();
		this.CreateNewCompanion(obj);
	}

	// Token: 0x060075B8 RID: 30136 RVA: 0x002EE0EC File Offset: 0x002EC2EC
	public override DebrisObject Drop(PlayerController player)
	{
		this.DestroyAllCompanions();
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		player.OnRoomClearEvent -= this.HandleRoomCleared;
		player.OnKilledEnemy -= this.HandleEnemyKilled;
		return base.Drop(player);
	}

	// Token: 0x060075B9 RID: 30137 RVA: 0x002EE14C File Offset: 0x002EC34C
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			PlayerController owner = this.m_owner;
			owner.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(owner.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
			this.m_owner.OnRoomClearEvent -= this.HandleRoomCleared;
			this.m_owner.OnKilledEnemy -= this.HandleEnemyKilled;
		}
		this.DestroyAllCompanions();
		base.OnDestroy();
	}

	// Token: 0x04007775 RID: 30581
	[EnemyIdentifier]
	public string CompanionGuid;

	// Token: 0x04007776 RID: 30582
	public bool HasSynergy;

	// Token: 0x04007777 RID: 30583
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007778 RID: 30584
	[EnemyIdentifier]
	public string SynergyCompanionGuid;

	// Token: 0x04007779 RID: 30585
	public int SynergyMaxNumberOfCompanions = 3;

	// Token: 0x0400777A RID: 30586
	public int MaxNumberOfCompanions = 8;

	// Token: 0x0400777B RID: 30587
	public bool TriggersOnRoomClear;

	// Token: 0x0400777C RID: 30588
	public bool TriggersOnEnemyKill;

	// Token: 0x0400777D RID: 30589
	private List<CompanionController> m_companions = new List<CompanionController>();

	// Token: 0x0400777E RID: 30590
	private bool m_synergyActive;
}
