using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001384 RID: 4996
public class CompanionItem : PassiveItem
{
	// Token: 0x17001120 RID: 4384
	// (get) Token: 0x0600713B RID: 28987 RVA: 0x002CF8F4 File Offset: 0x002CDAF4
	public GameObject ExtantCompanion
	{
		get
		{
			return this.m_extantCompanion;
		}
	}

	// Token: 0x0600713C RID: 28988 RVA: 0x002CF8FC File Offset: 0x002CDAFC
	private void CreateCompanion(PlayerController owner)
	{
		if (this.PreventRespawnOnFloorLoad)
		{
			return;
		}
		if (this.BabyGoodMimicOrbitalOverridden)
		{
			GameObject gameObject = PlayerOrbitalItem.CreateOrbital(owner, (!this.OverridePlayerOrbitalItem.OrbitalFollowerPrefab) ? this.OverridePlayerOrbitalItem.OrbitalPrefab.gameObject : this.OverridePlayerOrbitalItem.OrbitalFollowerPrefab.gameObject, this.OverridePlayerOrbitalItem.OrbitalFollowerPrefab, null);
			this.m_extantCompanion = gameObject;
			return;
		}
		string text = this.CompanionGuid;
		this.m_lastActiveSynergyTransformation = -1;
		if (this.UsesAlternatePastPrefab && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
		{
			text = this.CompanionPastGuid;
		}
		else if (this.Synergies.Length > 0)
		{
			for (int i = 0; i < this.Synergies.Length; i++)
			{
				if (owner.HasActiveBonusSynergy(this.Synergies[i].RequiredSynergy, false))
				{
					text = this.Synergies[i].SynergyCompanionGuid;
					this.m_lastActiveSynergyTransformation = i;
				}
			}
		}
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(text);
		Vector3 vector = owner.transform.position;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			vector += new Vector3(1.125f, -0.3125f, 0f);
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, vector, Quaternion.identity);
		this.m_extantCompanion = gameObject2;
		CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
		orAddComponent.Initialize(owner);
		if (orAddComponent.specRigidbody)
		{
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
		}
		if (orAddComponent.companionID == CompanionController.CompanionIdentifier.BABY_GOOD_MIMIC)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_GOT_BABY_MIMIC, true);
		}
	}

	// Token: 0x0600713D RID: 28989 RVA: 0x002CFAC0 File Offset: 0x002CDCC0
	public void ForceCompanionRegeneration(PlayerController owner, Vector2? overridePosition)
	{
		bool flag = false;
		Vector2 vector = Vector2.zero;
		if (this.m_extantCompanion)
		{
			flag = true;
			vector = this.m_extantCompanion.transform.position.XY();
		}
		if (overridePosition != null)
		{
			flag = true;
			vector = overridePosition.Value;
		}
		this.DestroyCompanion();
		this.CreateCompanion(owner);
		if (this.m_extantCompanion && flag)
		{
			this.m_extantCompanion.transform.position = vector.ToVector3ZisY(0f);
			SpeculativeRigidbody component = this.m_extantCompanion.GetComponent<SpeculativeRigidbody>();
			if (component)
			{
				component.Reinitialize();
			}
		}
	}

	// Token: 0x0600713E RID: 28990 RVA: 0x002CFB70 File Offset: 0x002CDD70
	public void ForceDisconnectCompanion()
	{
		this.m_extantCompanion = null;
	}

	// Token: 0x0600713F RID: 28991 RVA: 0x002CFB7C File Offset: 0x002CDD7C
	private void DestroyCompanion()
	{
		if (!this.m_extantCompanion)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.m_extantCompanion);
		this.m_extantCompanion = null;
	}

	// Token: 0x06007140 RID: 28992 RVA: 0x002CFBA4 File Offset: 0x002CDDA4
	protected override void Update()
	{
		base.Update();
		if (!Dungeon.IsGenerating && this.m_owner && this.Synergies.Length > 0)
		{
			if (!this.UsesAlternatePastPrefab || GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.CHARACTER_PAST)
			{
				bool flag = false;
				for (int i = this.Synergies.Length - 1; i >= 0; i--)
				{
					if (this.m_owner.HasActiveBonusSynergy(this.Synergies[i].RequiredSynergy, false))
					{
						if (this.m_lastActiveSynergyTransformation != i)
						{
							this.DestroyCompanion();
							this.CreateCompanion(this.m_owner);
						}
						flag = true;
						break;
					}
				}
				if (!flag && this.m_lastActiveSynergyTransformation != -1)
				{
					this.DestroyCompanion();
					this.CreateCompanion(this.m_owner);
				}
			}
		}
	}

	// Token: 0x06007141 RID: 28993 RVA: 0x002CFC84 File Offset: 0x002CDE84
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		this.CreateCompanion(player);
	}

	// Token: 0x06007142 RID: 28994 RVA: 0x002CFCB8 File Offset: 0x002CDEB8
	private void HandleNewFloor(PlayerController obj)
	{
		this.DestroyCompanion();
		if (!this.PreventRespawnOnFloorLoad)
		{
			this.CreateCompanion(obj);
		}
	}

	// Token: 0x06007143 RID: 28995 RVA: 0x002CFCD4 File Offset: 0x002CDED4
	public override DebrisObject Drop(PlayerController player)
	{
		this.DestroyCompanion();
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		return base.Drop(player);
	}

	// Token: 0x06007144 RID: 28996 RVA: 0x002CFD08 File Offset: 0x002CDF08
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			PlayerController owner = this.m_owner;
			owner.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(owner.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		}
		this.DestroyCompanion();
		base.OnDestroy();
	}

	// Token: 0x040070D3 RID: 28883
	[EnemyIdentifier]
	public string CompanionGuid;

	// Token: 0x040070D4 RID: 28884
	public bool UsesAlternatePastPrefab;

	// Token: 0x040070D5 RID: 28885
	[EnemyIdentifier]
	[ShowInInspectorIf("UsesAlternatePastPrefab", false)]
	public string CompanionPastGuid;

	// Token: 0x040070D6 RID: 28886
	public CompanionTransformSynergy[] Synergies;

	// Token: 0x040070D7 RID: 28887
	[NonSerialized]
	public bool PreventRespawnOnFloorLoad;

	// Token: 0x040070D8 RID: 28888
	[Header("For Pig Synergy")]
	public bool HasGunTransformationSacrificeSynergy;

	// Token: 0x040070D9 RID: 28889
	[ShowInInspectorIf("HasGunTransformationSacrificeSynergy", false)]
	public CustomSynergyType GunTransformationSacrificeSynergy;

	// Token: 0x040070DA RID: 28890
	[PickupIdentifier]
	[ShowInInspectorIf("HasGunTransformationSacrificeSynergy", false)]
	public int SacrificeGunID;

	// Token: 0x040070DB RID: 28891
	[ShowInInspectorIf("HasGunTransformationSacrificeSynergy", false)]
	public float SacrificeGunDuration = 30f;

	// Token: 0x040070DC RID: 28892
	[NonSerialized]
	public bool BabyGoodMimicOrbitalOverridden;

	// Token: 0x040070DD RID: 28893
	[NonSerialized]
	public PlayerOrbitalItem OverridePlayerOrbitalItem;

	// Token: 0x040070DE RID: 28894
	private int m_lastActiveSynergyTransformation = -1;

	// Token: 0x040070DF RID: 28895
	private GameObject m_extantCompanion;
}
