using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001201 RID: 4609
public class SecretRoomDoorBeer : MonoBehaviour
{
	// Token: 0x06006708 RID: 26376 RVA: 0x002843F4 File Offset: 0x002825F4
	private void Awake()
	{
		if (SecretRoomDoorBeer.AllSecretRoomDoors == null)
		{
			SecretRoomDoorBeer.AllSecretRoomDoors = new List<SecretRoomDoorBeer>();
		}
		SecretRoomDoorBeer.AllSecretRoomDoors.Add(this);
	}

	// Token: 0x06006709 RID: 26377 RVA: 0x00284418 File Offset: 0x00282618
	private void Start()
	{
		if (this.linkedRoom != null)
		{
			this.linkedRoom.Entered += this.HandlePlayerEnteredLinkedRoom;
		}
	}

	// Token: 0x0600670A RID: 26378 RVA: 0x0028443C File Offset: 0x0028263C
	private void Update()
	{
		if (!this.m_hasBeenAmygdalaed)
		{
			this.m_amygdalaCheckTimer -= BraveTime.DeltaTime;
			if (this.m_amygdalaCheckTimer < 0f)
			{
				this.m_amygdalaCheckTimer = 1f;
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i].HasActiveBonusSynergy(CustomSynergyType.INSIGHT, false))
					{
						RoomHandler roomHandler = ((this.exitDef.upstreamRoom != this.linkedRoom) ? this.exitDef.upstreamRoom : this.exitDef.downstreamRoom);
						if (roomHandler == null || !(roomHandler.secretRoomManager != null) || roomHandler.secretRoomManager.revealStyle != SecretRoomManager.SecretRoomRevealStyle.FireplacePuzzle)
						{
							this.GenerateAmygdala();
							this.m_hasBeenAmygdalaed = true;
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600670B RID: 26379 RVA: 0x00284528 File Offset: 0x00282728
	private void GenerateAmygdala()
	{
		string text = string.Empty;
		Vector2 zero = Vector2.zero;
		switch (this.exitDef.GetDirectionFromRoom(this.linkedRoom))
		{
		case DungeonData.Direction.NORTH:
			text = "Global VFX/Amygdala_South";
			zero = new Vector2(0f, 2f);
			break;
		case DungeonData.Direction.EAST:
			text = "Global VFX/Amygdala_West";
			zero = new Vector2(-0.25f, 2f);
			break;
		case DungeonData.Direction.SOUTH:
			text = "Global VFX/Amygdala_North";
			zero = new Vector2(0f, 1.5f);
			break;
		case DungeonData.Direction.WEST:
			text = "Global VFX/Amygdala_East";
			zero = new Vector2(0f, 2f);
			break;
		}
		this.m_amygdala = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire(text));
		this.m_amygdala.transform.position = base.transform.position + zero.ToVector3ZUp(0f);
	}

	// Token: 0x0600670C RID: 26380 RVA: 0x0028462C File Offset: 0x0028282C
	private void OnDestroy()
	{
		if (SecretRoomDoorBeer.AllSecretRoomDoors != null)
		{
			SecretRoomDoorBeer.AllSecretRoomDoors.Remove(this);
		}
	}

	// Token: 0x0600670D RID: 26381 RVA: 0x00284644 File Offset: 0x00282844
	public void SetBreakable()
	{
		if (this.m_breakable)
		{
			this.m_breakable.IsSecretDoor = false;
		}
	}

	// Token: 0x0600670E RID: 26382 RVA: 0x00284664 File Offset: 0x00282864
	private void HandlePlayerEnteredLinkedRoom(PlayerController p)
	{
		if (this.exitDef != null)
		{
			RoomHandler roomHandler = ((this.exitDef.upstreamRoom != this.linkedRoom) ? this.exitDef.upstreamRoom : this.exitDef.downstreamRoom);
			if (roomHandler != null && roomHandler.secretRoomManager != null && roomHandler.secretRoomManager.revealStyle == SecretRoomManager.SecretRoomRevealStyle.FireplacePuzzle)
			{
				return;
			}
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (PassiveItem.ActiveFlagItems.ContainsKey(GameManager.Instance.AllPlayers[i]) && PassiveItem.ActiveFlagItems[GameManager.Instance.AllPlayers[i]].ContainsKey(typeof(SnitchBrickItem)) && !this.m_hasSnitchedBricked)
			{
				this.DoSnitchBrick();
			}
		}
	}

	// Token: 0x0600670F RID: 26383 RVA: 0x0028474C File Offset: 0x0028294C
	private void DoSnitchBrick()
	{
		this.m_hasSnitchedBricked = true;
		GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_SnitchBrick");
		Vector3 vector = this.collider.colliderObject.GetComponent<SpeculativeRigidbody>().UnitCenter;
		vector += DungeonData.GetIntVector2FromDirection(this.exitDef.downstreamExit.referencedExit.exitDirection).ToVector3();
		this.m_snitchBrick = UnityEngine.Object.Instantiate<GameObject>(gameObject, vector, Quaternion.identity);
	}

	// Token: 0x06006710 RID: 26384 RVA: 0x002847C8 File Offset: 0x002829C8
	public void InitializeFireplace()
	{
	}

	// Token: 0x06006711 RID: 26385 RVA: 0x002847CC File Offset: 0x002829CC
	public void InitializeShootToBreak()
	{
		SpeculativeRigidbody component = this.collider.colliderObject.GetComponent<SpeculativeRigidbody>();
		component.PreventPiercing = true;
		this.m_breakable = this.collider.colliderObject.AddComponent<MajorBreakable>();
		this.m_breakable.IsSecretDoor = true;
		this.m_breakable.spawnShards = false;
		this.m_breakable.HitPoints = 25f;
		this.m_breakable.EnemyDamageOverride = 8;
		GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
		if (lastLoadedLevelDefinition != null)
		{
			this.m_breakable.HitPoints *= lastLoadedLevelDefinition.secretDoorHealthMultiplier;
		}
		MajorBreakable breakable = this.m_breakable;
		breakable.OnDamaged = (Action<float>)Delegate.Combine(breakable.OnDamaged, new Action<float>(this.OnDamaged));
		MajorBreakable breakable2 = this.m_breakable;
		breakable2.OnBreak = (Action)Delegate.Combine(breakable2.OnBreak, new Action(this.OnBreak));
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(BraveResources.Load<GameObject>("Global VFX/VFX_Secret_Door_Crack_01", ".prefab"));
		this.m_breakVfxSprite = gameObject.GetComponent<tk2dSprite>();
		this.m_breakFrames = new BreakFrame[]
		{
			new BreakFrame
			{
				healthPercentage = 50f,
				sprite = "secret_door_crack_generic{0}_001"
			},
			new BreakFrame
			{
				healthPercentage = 10f,
				sprite = "secret_door_crack_generic{0}_002"
			}
		};
		if (this.collider.exitDirection == DungeonData.Direction.SOUTH)
		{
			this.m_breakVfxSprite.IsPerpendicular = true;
			this.m_breakVfxSprite.transform.position = component.UnitBottomLeft;
			this.m_breakVfxSprite.HeightOffGround = -1.45f;
			this.m_breakVfxSprite.UpdateZDepth();
		}
		else
		{
			this.m_breakVfxSprite.IsPerpendicular = false;
			this.m_breakVfxSprite.HeightOffGround = 3.2f;
			if (this.collider.exitDirection == DungeonData.Direction.NORTH)
			{
				this.m_breakVfxSprite.transform.position = component.UnitBottomLeft;
			}
			else
			{
				this.m_breakVfxSprite.transform.position = component.UnitBottomLeft + new Vector2(0f, 1f);
			}
			if (this.collider.exitDirection == DungeonData.Direction.EAST)
			{
				this.m_breakVfxSprite.transform.position = this.m_breakVfxSprite.transform.position + new Vector3(-1f, 0f, 0f);
			}
			this.m_breakVfxSprite.UpdateZDepth();
		}
		this.m_breakVfxSprite.renderer.enabled = false;
		if (GameManager.Instance.InTutorial)
		{
			this.m_breakable.MaxHitPoints = this.m_breakable.HitPoints;
			this.m_breakable.HitPoints = 2f;
			this.m_breakable.ApplyDamage(1f, Vector2.zero, false, true, true);
		}
	}

	// Token: 0x06006712 RID: 26386 RVA: 0x00284AC0 File Offset: 0x00282CC0
	public void OnDamaged(float damage)
	{
		for (int i = this.m_breakFrames.Length - 1; i >= 0; i--)
		{
			if (this.m_breakable.MinHits <= 0 || i < this.m_breakable.NumHits)
			{
				if (this.m_breakable.GetCurrentHealthPercentage() <= this.m_breakFrames[i].healthPercentage / 100f)
				{
					if (this.m_breakVfxSprite)
					{
						this.m_breakVfxSprite.renderer.enabled = true;
						this.m_breakVfxSprite.SetSprite(this.GetFrameName(this.m_breakFrames[i].sprite, this.collider.exitDirection));
					}
					return;
				}
			}
		}
		if (this.m_breakVfxSprite)
		{
			this.m_breakVfxSprite.renderer.enabled = false;
		}
	}

	// Token: 0x06006713 RID: 26387 RVA: 0x00284BA8 File Offset: 0x00282DA8
	public void OnBreak()
	{
		if (this.m_snitchBrick != null)
		{
			LootEngine.DoDefaultItemPoof(this.m_snitchBrick.GetComponentInChildren<tk2dBaseSprite>().WorldCenter, false, false);
			UnityEngine.Object.Destroy(this.m_snitchBrick);
		}
		if (this.m_amygdala)
		{
			UnityEngine.Object.Destroy(this.m_amygdala);
			this.m_amygdala = null;
		}
		this.BreakOpen();
	}

	// Token: 0x06006714 RID: 26388 RVA: 0x00284C10 File Offset: 0x00282E10
	public void BreakOpen()
	{
		if (this.m_breakVfxSprite)
		{
			UnityEngine.Object.Destroy(this.m_breakVfxSprite);
		}
		AkSoundEngine.PostEvent("Play_UI_secret_reveal_01", base.gameObject);
		this.manager.IsOpen = true;
		this.manager.HandleDoorBrokenOpen(this);
		this.collider.colliderObject.GetComponent<SpeculativeRigidbody>().enabled = false;
		if (this.wallChunks != null)
		{
			for (int i = 0; i < this.wallChunks.Count; i++)
			{
				this.wallChunks[i].gameObject.SetActive(true);
				this.wallChunks[i].Trigger(true, null);
			}
		}
	}

	// Token: 0x06006715 RID: 26389 RVA: 0x00284CD0 File Offset: 0x00282ED0
	public void GeneratePotentiallyNecessaryShards()
	{
		GameObject secretRoomWallShardCollection = GameManager.Instance.Dungeon.roomMaterialDefinitions[this.manager.room.RoomVisualSubtype].GetSecretRoomWallShardCollection();
		if (secretRoomWallShardCollection != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(secretRoomWallShardCollection);
			gameObject.transform.position = base.transform.position;
			while (gameObject.transform.childCount > 0)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
				gameObject2.transform.parent = base.transform;
				if (this.wallChunks == null)
				{
					this.wallChunks = new List<BreakableChunk>();
				}
				gameObject2.SetActive(false);
				this.wallChunks.Add(gameObject2.GetComponent<BreakableChunk>());
			}
		}
	}

	// Token: 0x06006716 RID: 26390 RVA: 0x00284D94 File Offset: 0x00282F94
	private string GetFrameName(string name, DungeonData.Direction dir)
	{
		if (name.Contains("{0}"))
		{
			string text;
			switch (dir)
			{
			case DungeonData.Direction.NORTH:
				text = "_top_top";
				break;
			default:
				if (dir != DungeonData.Direction.WEST)
				{
					text = string.Empty;
				}
				else
				{
					text = "_left_top";
				}
				break;
			case DungeonData.Direction.EAST:
				text = "_right_top";
				break;
			}
			return string.Format(name, text);
		}
		return name;
	}

	// Token: 0x040062CE RID: 25294
	public static List<SecretRoomDoorBeer> AllSecretRoomDoors;

	// Token: 0x040062CF RID: 25295
	public DungeonDoorSubsidiaryBlocker subsidiaryBlocker;

	// Token: 0x040062D0 RID: 25296
	public RuntimeExitDefinition exitDef;

	// Token: 0x040062D1 RID: 25297
	public RoomHandler linkedRoom;

	// Token: 0x040062D2 RID: 25298
	public SecretRoomManager manager;

	// Token: 0x040062D3 RID: 25299
	public SecretRoomExitData collider;

	// Token: 0x040062D4 RID: 25300
	private MajorBreakable m_breakable;

	// Token: 0x040062D5 RID: 25301
	private tk2dSprite m_breakVfxSprite;

	// Token: 0x040062D6 RID: 25302
	public List<BreakableChunk> wallChunks;

	// Token: 0x040062D7 RID: 25303
	private bool m_hasSnitchedBricked;

	// Token: 0x040062D8 RID: 25304
	private GameObject m_snitchBrick;

	// Token: 0x040062D9 RID: 25305
	public BreakFrame[] m_breakFrames;

	// Token: 0x040062DA RID: 25306
	private bool m_hasBeenAmygdalaed;

	// Token: 0x040062DB RID: 25307
	private float m_amygdalaCheckTimer;

	// Token: 0x040062DC RID: 25308
	private GameObject m_amygdala;
}
