using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020011E5 RID: 4581
public class ResourcefulRatMinesHiddenTrapdoor : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06006635 RID: 26165 RVA: 0x0027B804 File Offset: 0x00279A04
	private IEnumerator Start()
	{
		this.m_blendTex = new Texture2D(64, 64, TextureFormat.RGBA32, false);
		this.m_blendTexColors = new Color[4096];
		for (int i = 0; i < this.m_blendTexColors.Length; i++)
		{
			this.m_blendTexColors[i] = Color.black;
		}
		this.m_blendTex.SetPixels(this.m_blendTexColors);
		this.m_blendTex.Apply();
		this.BlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
		this.BlendMaterial.SetTexture("_BlendTex", this.m_blendTex);
		this.BlendMaterial.SetVector("_BaseWorldPosition", new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 0f));
		this.LockBlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
		this.LockBlendMaterial.SetTexture("_BlendTex", this.m_blendTex);
		this.LockBlendMaterial.SetVector("_BaseWorldPosition", new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 0f));
		RoomHandler parentRoom = base.transform.position.GetAbsoluteRoom();
		this.BlendMaterial.SetTexture("_SubTex", (parentRoom.RoomVisualSubtype != 1) ? this.StoneFloorTex : this.DirtFloorTex);
		this.LockBlendMaterial.SetTexture("_SubTex", (parentRoom.RoomVisualSubtype != 1) ? this.StoneFloorTex : this.DirtFloorTex);
		if (!StaticReferenceManager.AllRatTrapdoors.Contains(this))
		{
			StaticReferenceManager.AllRatTrapdoors.Add(this);
		}
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (this.FlightCollider)
		{
			SpeculativeRigidbody flightCollider = this.FlightCollider;
			flightCollider.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(flightCollider.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleFlightCollider));
		}
		if (!string.IsNullOrEmpty(GameManager.Instance.Dungeon.NormalRatGUID))
		{
			for (int j = 0; j < 3; j++)
			{
				parentRoom.AddSpecificEnemyToRoomProcedurally(GameManager.Instance.Dungeon.NormalRatGUID, false, null);
			}
		}
		yield break;
	}

	// Token: 0x06006636 RID: 26166 RVA: 0x0027B820 File Offset: 0x00279A20
	private void HandleFlightCollider(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (!GameManager.Instance.IsLoadingLevel && !this.Lock.IsLocked && this.m_hasCreatedRoom)
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			if (component && component.IsFlying)
			{
				this.m_timeHovering += BraveTime.DeltaTime;
				if (this.m_timeHovering > 1f)
				{
					component.ForceFall();
					this.m_timeHovering = 0f;
				}
			}
		}
	}

	// Token: 0x06006637 RID: 26167 RVA: 0x0027B8A8 File Offset: 0x00279AA8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		StaticReferenceManager.AllRatTrapdoors.Remove(this);
	}

	// Token: 0x06006638 RID: 26168 RVA: 0x0027B8BC File Offset: 0x00279ABC
	public void OnNearbyExplosion(Vector3 center)
	{
		float sqrMagnitude = (base.transform.position.XY() + new Vector2(2f, 2f) - center.XY()).sqrMagnitude;
		if (sqrMagnitude < this.ExplosionReactDistance * this.ExplosionReactDistance)
		{
			float revealPercentage = this.RevealPercentage;
			this.RevealPercentage = Mathf.Max(revealPercentage, Mathf.Min(0.3f, this.RevealPercentage + 0.125f));
			this.UpdatePlayerDustups();
			this.BlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
			this.LockBlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
		}
	}

	// Token: 0x06006639 RID: 26169 RVA: 0x0027B970 File Offset: 0x00279B70
	public void OnBlank()
	{
		if (GameManager.Instance.BestActivePlayer.CurrentRoom == base.transform.position.GetAbsoluteRoom())
		{
			float revealPercentage = this.RevealPercentage;
			this.RevealPercentage = Mathf.Max(revealPercentage, Mathf.Min(0.3f, this.RevealPercentage + 0.5f));
			this.UpdatePlayerDustups();
			this.BlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
			this.LockBlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
		}
	}

	// Token: 0x0600663A RID: 26170 RVA: 0x0027B9FC File Offset: 0x00279BFC
	private void UpdatePlayerPositions()
	{
		if (this.RevealPercentage >= 1f)
		{
			return;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			Vector2 vector = playerController.SpriteBottomCenter;
			bool flag = false;
			if (vector.x > base.transform.position.x && vector.y > base.transform.position.y && vector.x < base.transform.position.x + 4f && vector.y < base.transform.position.y + 4f && (playerController.IsGrounded || playerController.IsFlying) && !playerController.IsGhost)
			{
				flag = true;
				playerController.OverrideDustUp = ResourceCache.Acquire("Global VFX/VFX_RatDoor_DustUp") as GameObject;
				if (playerController.Velocity.magnitude > 0f)
				{
					Vector2 vector2 = vector - base.transform.position.XY();
					IntVector2 intVector = new IntVector2(Mathf.FloorToInt(vector2.x * 16f), Mathf.FloorToInt(vector2.y * 16f));
					this.SoftUpdateRadius(intVector, 10, 2f * Time.deltaTime);
				}
			}
			if (!flag && playerController.OverrideDustUp && playerController.OverrideDustUp.name.StartsWith("VFX_RatDoor_DustUp", StringComparison.Ordinal))
			{
				playerController.OverrideDustUp = null;
			}
		}
	}

	// Token: 0x0600663B RID: 26171 RVA: 0x0027BBC0 File Offset: 0x00279DC0
	private float CalcAvgRevealedness()
	{
		if (this.RevealPercentage >= 1f)
		{
			return 1f;
		}
		float num = 0f;
		for (int i = 0; i < 64; i++)
		{
			for (int j = 0; j < 64; j++)
			{
				float r = this.m_blendTexColors[j * 64 + i].r;
				num += Mathf.Max(r, this.RevealPercentage);
			}
		}
		return num / 4096f;
	}

	// Token: 0x0600663C RID: 26172 RVA: 0x0027BC3C File Offset: 0x00279E3C
	private bool SoftUpdateRadius(IntVector2 pxCenter, int radius, float amt)
	{
		bool flag = false;
		for (int i = pxCenter.x - radius; i < pxCenter.x + radius; i++)
		{
			for (int j = pxCenter.y - radius; j < pxCenter.y + radius; j++)
			{
				if (i > 0 && j > 0 && i < 64 && j < 64)
				{
					Color color = this.m_blendTexColors[j * 64 + i];
					float num = Vector2.Distance(pxCenter.ToVector2(), new Vector2((float)i, (float)j));
					float num2 = Mathf.Clamp01(((float)radius - num) / (float)radius);
					float num3 = Mathf.Clamp01(color.r + amt * num2);
					if (num3 != color.r)
					{
						color.r = num3;
						this.m_blendTexColors[j * 64 + i] = color;
						flag = true;
						this.m_blendTexDirty = true;
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x0600663D RID: 26173 RVA: 0x0027BD38 File Offset: 0x00279F38
	private void UpdateGoopedCells()
	{
		if (this.RevealPercentage >= 1f)
		{
			return;
		}
		Vector2 vector = base.transform.position.XY();
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				Vector2 vector2 = new Vector2((float)i / 4f, (float)j / 4f) + vector;
				IntVector2 intVector = (vector2 / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
				if (DeadlyDeadlyGoopManager.allGoopPositionMap.ContainsKey(intVector) && !this.m_goopedSpots.Contains(intVector))
				{
					this.m_goopedSpots.Add(intVector);
					IntVector2 intVector2 = new IntVector2(i * 4, j * 4);
					for (int k = intVector2.x; k < intVector2.x + 4; k++)
					{
						for (int l = intVector2.y; l < intVector2.y + 4; l++)
						{
							this.m_blendTexColors[l * 64 + k] = new Color(1f, 1f, 1f, 1f);
						}
					}
					this.m_blendTexDirty = true;
				}
			}
		}
	}

	// Token: 0x0600663E RID: 26174 RVA: 0x0027BE7C File Offset: 0x0027A07C
	private IEnumerator GraduallyReveal()
	{
		if (this.m_revealing)
		{
			yield break;
		}
		this.m_revealing = true;
		while (this.RevealPercentage < 1f)
		{
			this.RevealPercentage = Mathf.Clamp01(this.RevealPercentage + Time.deltaTime);
			this.UpdatePlayerDustups();
			this.BlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
			this.LockBlendMaterial.SetFloat("_BlendMin", this.RevealPercentage);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600663F RID: 26175 RVA: 0x0027BE98 File Offset: 0x0027A098
	private void UpdatePlayerDustups()
	{
		if (this.RevealPercentage >= 1f)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController && playerController.OverrideDustUp && playerController.OverrideDustUp.name.StartsWith("VFX_RatDoor_DustUp", StringComparison.Ordinal))
				{
					playerController.OverrideDustUp = null;
				}
			}
		}
	}

	// Token: 0x06006640 RID: 26176 RVA: 0x0027BF1C File Offset: 0x0027A11C
	private void LateUpdate()
	{
		if (this.RevealPercentage < 1f)
		{
			this.UpdateGoopedCells();
			this.UpdatePlayerPositions();
			if (!this.m_revealing)
			{
				float num = this.CalcAvgRevealedness();
				if (num > 0.33f)
				{
					base.StartCoroutine(this.GraduallyReveal());
				}
			}
			if (this.m_blendTexDirty)
			{
				this.m_blendTex.SetPixels(this.m_blendTexColors);
				this.m_blendTex.Apply();
			}
		}
		else if (this.Lock.Suppress)
		{
			this.Lock.Suppress = false;
			Minimap.Instance.RegisterRoomIcon(base.transform.position.GetAbsoluteRoom(), this.MinimapIcon, false);
		}
		else if (!this.m_hasCreatedRoom && !this.Lock.IsLocked)
		{
			this.Open();
		}
	}

	// Token: 0x06006641 RID: 26177 RVA: 0x0027C000 File Offset: 0x0027A200
	public void Open()
	{
		if (this.m_hasCreatedRoom)
		{
			return;
		}
		if (!this.m_hasCreatedRoom)
		{
			this.m_hasCreatedRoom = true;
			List<PrototypeDungeonRoom> list = new List<PrototypeDungeonRoom>();
			list.Add(this.TargetMinecartRoom);
			list.Add(this.FirstSecretRoom);
			list.Add(this.SecondSecretRoom);
			List<IntVector2> list2 = new List<IntVector2>();
			list2.Add(IntVector2.Zero);
			list2.Add(new IntVector2(73, 17));
			list2.Add(new IntVector2(73, 36));
			List<RoomHandler> list3 = GameManager.Instance.Dungeon.AddRuntimeRoomCluster(list, list2, new Action<RoomHandler>(this.ActuallyMakeAllTheFacewallsLookTheSameInTheRightSpots), DungeonData.LightGenerationStyle.RAT_HALLWAY);
			this.m_createdRoom = list3[0];
			for (int i = 0; i < list3.Count; i++)
			{
				list3[i].PreventMinimapUpdates = true;
			}
		}
		if (this.m_createdRoom != null)
		{
			this.AssignPitfallRoom(this.m_createdRoom);
			base.spriteAnimator.Play();
			base.StartCoroutine(this.HandleFlaggingCells());
		}
	}

	// Token: 0x06006642 RID: 26178 RVA: 0x0027C104 File Offset: 0x0027A304
	private IEnumerator HandleFlaggingCells()
	{
		IntVector2 basePosition = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = 1; i < this.placeableWidth - 1; i++)
		{
			for (int j = 1; j < this.placeableHeight - 1; j++)
			{
				IntVector2 intVector = new IntVector2(i, j) + basePosition;
				DeadlyDeadlyGoopManager.ForceClearGoopsInCell(intVector);
			}
		}
		yield return new WaitForSeconds(0.4f);
		for (int k = 1; k < this.placeableWidth - 1; k++)
		{
			for (int l = 1; l < this.placeableHeight - 1; l++)
			{
				IntVector2 intVector2 = new IntVector2(k, l) + basePosition;
				CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
				cellData.fallingPrevented = false;
			}
		}
		yield break;
	}

	// Token: 0x06006643 RID: 26179 RVA: 0x0027C120 File Offset: 0x0027A320
	public void ActuallyMakeAllTheFacewallsLookTheSameInTheRightSpots(RoomHandler target)
	{
		if (target.area.prototypeRoom != this.TargetMinecartRoom)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		for (int i = 0; i < target.area.dimensions.x; i++)
		{
			for (int j = 0; j < target.area.dimensions.y + 2; j++)
			{
				IntVector2 intVector = target.area.basePosition + new IntVector2(i, j);
				if (data.CheckInBoundsAndValid(intVector))
				{
					CellData cellData = data[intVector];
					if (data.isAnyFaceWall(intVector.x, intVector.y))
					{
						TilesetIndexMetadata.TilesetFlagType tilesetFlagType = TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER;
						if (data.isFaceWallLower(intVector.x, intVector.y))
						{
							tilesetFlagType = TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER;
						}
						int indexFromTupleArray = SecretRoomUtility.GetIndexFromTupleArray(cellData, SecretRoomUtility.metadataLookupTableRef[tilesetFlagType], cellData.cellVisualData.roomVisualTypeIndex, 0f);
						cellData.cellVisualData.faceWallOverrideIndex = indexFromTupleArray;
					}
				}
			}
		}
	}

	// Token: 0x06006644 RID: 26180 RVA: 0x0027C238 File Offset: 0x0027A438
	private void AssignPitfallRoom(RoomHandler target)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = 0; i < this.placeableWidth; i++)
		{
			for (int j = -2; j < this.placeableHeight; j++)
			{
				IntVector2 intVector2 = new IntVector2(i, j) + intVector;
				CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
				cellData.targetPitfallRoom = target;
				cellData.forceAllowGoop = false;
			}
		}
	}

	// Token: 0x06006645 RID: 26181 RVA: 0x0027C2BC File Offset: 0x0027A4BC
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = 1; i < this.placeableWidth - 1; i++)
		{
			for (int j = 1; j < this.placeableHeight - 1; j++)
			{
				IntVector2 intVector2 = new IntVector2(i, j) + intVector;
				CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
				int num;
				if (i == 1)
				{
					if (j == 1)
					{
						num = this.OverridePitGrid.bottomLeftIndices.GetIndexByWeight();
					}
					else
					{
						num = this.OverridePitGrid.topLeftIndices.GetIndexByWeight();
					}
				}
				else if (j == 1)
				{
					num = this.OverridePitGrid.bottomRightIndices.GetIndexByWeight();
				}
				else
				{
					num = this.OverridePitGrid.topRightIndices.GetIndexByWeight();
				}
				cellData.cellVisualData.pitOverrideIndex = num;
				cellData.forceAllowGoop = true;
				cellData.type = CellType.PIT;
				cellData.fallingPrevented = true;
				cellData.cellVisualData.containsObjectSpaceStamp = true;
				cellData.cellVisualData.containsWallSpaceStamp = true;
			}
		}
	}

	// Token: 0x04006207 RID: 25095
	public PrototypeDungeonRoom TargetMinecartRoom;

	// Token: 0x04006208 RID: 25096
	public PrototypeDungeonRoom FirstSecretRoom;

	// Token: 0x04006209 RID: 25097
	public PrototypeDungeonRoom SecondSecretRoom;

	// Token: 0x0400620A RID: 25098
	public TileIndexGrid OverridePitGrid;

	// Token: 0x0400620B RID: 25099
	public Material BlendMaterial;

	// Token: 0x0400620C RID: 25100
	public Material LockBlendMaterial;

	// Token: 0x0400620D RID: 25101
	public InteractableLock Lock;

	// Token: 0x0400620E RID: 25102
	public Texture2D StoneFloorTex;

	// Token: 0x0400620F RID: 25103
	public Texture2D DirtFloorTex;

	// Token: 0x04006210 RID: 25104
	public float ExplosionReactDistance = 8f;

	// Token: 0x04006211 RID: 25105
	public SpeculativeRigidbody FlightCollider;

	// Token: 0x04006212 RID: 25106
	[NonSerialized]
	public float RevealPercentage;

	// Token: 0x04006213 RID: 25107
	public GameObject MinimapIcon;

	// Token: 0x04006214 RID: 25108
	private bool m_hasCreatedRoom;

	// Token: 0x04006215 RID: 25109
	private RoomHandler m_createdRoom;

	// Token: 0x04006216 RID: 25110
	private Texture2D m_blendTex;

	// Token: 0x04006217 RID: 25111
	private Color[] m_blendTexColors;

	// Token: 0x04006218 RID: 25112
	private bool m_blendTexDirty;

	// Token: 0x04006219 RID: 25113
	private HashSet<IntVector2> m_goopedSpots = new HashSet<IntVector2>();

	// Token: 0x0400621A RID: 25114
	private float m_timeHovering;

	// Token: 0x0400621B RID: 25115
	private bool m_revealing;
}
