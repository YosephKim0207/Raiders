using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using PathologicalGames;
using UnityEngine;

// Token: 0x020016C7 RID: 5831
public class SpawnManager : MonoBehaviour
{
	// Token: 0x1700143F RID: 5183
	// (get) Token: 0x06008783 RID: 34691 RVA: 0x003834D8 File Offset: 0x003816D8
	// (set) Token: 0x06008784 RID: 34692 RVA: 0x003834E0 File Offset: 0x003816E0
	public static SpawnManager Instance
	{
		get
		{
			return SpawnManager.m_instance;
		}
		set
		{
			SpawnManager.m_instance = value;
		}
	}

	// Token: 0x17001440 RID: 5184
	// (get) Token: 0x06008785 RID: 34693 RVA: 0x003834E8 File Offset: 0x003816E8
	public static bool HasInstance
	{
		get
		{
			return SpawnManager.m_instance != null;
		}
	}

	// Token: 0x17001441 RID: 5185
	// (get) Token: 0x06008786 RID: 34694 RVA: 0x003834F8 File Offset: 0x003816F8
	// (set) Token: 0x06008787 RID: 34695 RVA: 0x00383524 File Offset: 0x00381724
	public static SpawnPool PoolManager
	{
		get
		{
			if (SpawnManager.m_poolManager == null)
			{
				SpawnManager.m_poolManager = PathologicalGames.PoolManager.Pools.Create("SpawnManager Pool");
			}
			return SpawnManager.m_poolManager;
		}
		set
		{
			SpawnManager.m_poolManager = value;
		}
	}

	// Token: 0x17001442 RID: 5186
	// (get) Token: 0x06008788 RID: 34696 RVA: 0x0038352C File Offset: 0x0038172C
	// (set) Token: 0x06008789 RID: 34697 RVA: 0x00383534 File Offset: 0x00381734
	public static PrefabPool LastPrefabPool { get; set; }

	// Token: 0x0600878A RID: 34698 RVA: 0x0038353C File Offset: 0x0038173C
	public void Awake()
	{
		SpawnManager.m_instance = this;
		this.CurrentObjects = 0;
		this.CurrentObjectsInRoom = 0;
		this.OnDebrisQuantityChanged();
	}

	// Token: 0x0600878B RID: 34699 RVA: 0x00383558 File Offset: 0x00381758
	public void OnDebrisQuantityChanged()
	{
		switch (GameManager.Options.DebrisQuantity)
		{
		case GameOptions.GenericHighMedLowOption.LOW:
			this.MaxObjects = 50;
			break;
		case GameOptions.GenericHighMedLowOption.MEDIUM:
			this.MaxObjects = 300;
			break;
		case GameOptions.GenericHighMedLowOption.HIGH:
			this.MaxObjects = 800;
			break;
		case GameOptions.GenericHighMedLowOption.VERY_LOW:
			this.MaxObjects = 0;
			break;
		}
	}

	// Token: 0x0600878C RID: 34700 RVA: 0x003835CC File Offset: 0x003817CC
	public void Update()
	{
		this.CurrentObjects = this.m_objects.Count;
		if (this.UsesPerRoomObjectLimit)
		{
			if (GameManager.Instance.PrimaryPlayer.CurrentRoom != null && this.m_objectsByRoom.ContainsKey(GameManager.Instance.PrimaryPlayer.CurrentRoom))
			{
				this.CurrentObjectsInRoom = this.m_objectsByRoom[GameManager.Instance.PrimaryPlayer.CurrentRoom].Count;
			}
			else
			{
				this.CurrentObjectsInRoom = 0;
			}
		}
	}

	// Token: 0x0600878D RID: 34701 RVA: 0x0038365C File Offset: 0x0038185C
	public void OnDestroy()
	{
		SpawnManager.m_instance = null;
	}

	// Token: 0x0600878E RID: 34702 RVA: 0x00383664 File Offset: 0x00381864
	public static void RegisterEphemeralObject(EphemeralObject obj)
	{
		if (!SpawnManager.m_instance)
		{
			return;
		}
		SpawnManager.m_instance.AddObject(obj);
	}

	// Token: 0x0600878F RID: 34703 RVA: 0x00383684 File Offset: 0x00381884
	public static void DeregisterEphemeralObject(EphemeralObject obj)
	{
		if (!SpawnManager.m_instance)
		{
			return;
		}
		SpawnManager.m_instance.RemoveObject(obj);
	}

	// Token: 0x06008790 RID: 34704 RVA: 0x003836A4 File Offset: 0x003818A4
	public static GameObject SpawnDebris(GameObject prefab)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.SpawnUnpooledInternal(prefab, Vector3.zero, Quaternion.identity, SpawnManager.m_instance.Debris);
	}

	// Token: 0x06008791 RID: 34705 RVA: 0x003836D4 File Offset: 0x003818D4
	public static GameObject SpawnDebris(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.SpawnUnpooledInternal(prefab, position, rotation, SpawnManager.m_instance.Debris);
	}

	// Token: 0x06008792 RID: 34706 RVA: 0x003836FC File Offset: 0x003818FC
	public static GameObject SpawnDecal(GameObject prefab)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		GameObject gameObject = SpawnManager.Spawn(prefab, SpawnManager.m_instance.Decals, false);
		if (!gameObject.GetComponent<DecalObject>())
		{
			DecalObject decalObject = gameObject.AddComponent<DecalObject>();
			decalObject.Priority = EphemeralObject.EphemeralPriority.Minor;
		}
		return gameObject;
	}

	// Token: 0x06008793 RID: 34707 RVA: 0x0038374C File Offset: 0x0038194C
	public static GameObject SpawnDecal(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		DecalObject component = prefab.GetComponent<DecalObject>();
		EphemeralObject.EphemeralPriority ephemeralPriority = ((!component) ? EphemeralObject.EphemeralPriority.Ephemeral : component.Priority);
		bool flag = false;
		SpawnManager.m_instance.ClearRoomForDecal(position.XY(), ephemeralPriority, out flag);
		if (flag)
		{
			return null;
		}
		GameObject gameObject = SpawnManager.Spawn(prefab, position, rotation, SpawnManager.m_instance.Decals, ignoresPools);
		if (!gameObject.GetComponent<DecalObject>())
		{
			DecalObject decalObject = gameObject.AddComponent<DecalObject>();
			decalObject.Priority = EphemeralObject.EphemeralPriority.Ephemeral;
		}
		tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
		if (component2 != null)
		{
			component2.IsPerpendicular = true;
			component2.UpdateZDepth();
		}
		return gameObject;
	}

	// Token: 0x06008794 RID: 34708 RVA: 0x00383800 File Offset: 0x00381A00
	public static GameObject SpawnParticleSystem(GameObject prefab)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.SpawnUnpooledInternal(prefab, Vector3.zero, Quaternion.identity, SpawnManager.m_instance.ParticleSystems);
	}

	// Token: 0x06008795 RID: 34709 RVA: 0x00383830 File Offset: 0x00381A30
	public static GameObject SpawnParticleSystem(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.SpawnUnpooledInternal(prefab, position, rotation, SpawnManager.m_instance.ParticleSystems);
	}

	// Token: 0x06008796 RID: 34710 RVA: 0x00383858 File Offset: 0x00381A58
	public static GameObject SpawnProjectile(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools = true)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.Spawn(prefab, position, rotation, SpawnManager.m_instance.Projectiles, ignoresPools);
	}

	// Token: 0x06008797 RID: 34711 RVA: 0x00383880 File Offset: 0x00381A80
	public static GameObject SpawnProjectile(string resourcePath, Vector3 position, Quaternion rotation)
	{
		return SpawnManager.SpawnUnpooledInternal(BraveResources.Load<GameObject>(resourcePath, ".prefab"), position, rotation, SpawnManager.m_instance.Projectiles);
	}

	// Token: 0x06008798 RID: 34712 RVA: 0x003838A0 File Offset: 0x00381AA0
	public static GameObject SpawnVFX(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.Spawn(prefab, position, rotation, SpawnManager.m_instance.VFX, false);
	}

	// Token: 0x06008799 RID: 34713 RVA: 0x003838C8 File Offset: 0x00381AC8
	public static GameObject SpawnVFX(GameObject prefab, bool ignoresPools = false)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.Spawn(prefab, SpawnManager.m_instance.VFX, ignoresPools);
	}

	// Token: 0x0600879A RID: 34714 RVA: 0x003838EC File Offset: 0x00381AEC
	public static GameObject SpawnVFX(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools)
	{
		if (!SpawnManager.m_instance)
		{
			return null;
		}
		return SpawnManager.Spawn(prefab, position, rotation, SpawnManager.m_instance.VFX, ignoresPools);
	}

	// Token: 0x0600879B RID: 34715 RVA: 0x00383914 File Offset: 0x00381B14
	public static bool Despawn(GameObject instance)
	{
		if (SpawnManager.m_poolManager != null)
		{
			GameObject prefab = SpawnManager.m_poolManager.GetPrefab(instance);
			if (prefab != null)
			{
				PrefabPool prefabPool = SpawnManager.m_poolManager.GetPrefabPool(prefab);
				Transform transform = instance.transform;
				if (prefabPool.despawned.Contains(transform))
				{
					return true;
				}
				if (prefabPool.spawned.Contains(transform))
				{
					SpawnManager.m_poolManager.Despawn(instance.transform, null);
					return true;
				}
			}
		}
		UnityEngine.Object.Destroy(instance);
		return false;
	}

	// Token: 0x0600879C RID: 34716 RVA: 0x0038399C File Offset: 0x00381B9C
	public static bool Despawn(GameObject instance, PrefabPool prefabPool)
	{
		if (SpawnManager.m_poolManager != null)
		{
			if (prefabPool == null)
			{
				GameObject prefab = SpawnManager.m_poolManager.GetPrefab(instance);
				if (prefab != null)
				{
					prefabPool = SpawnManager.m_poolManager.GetPrefabPool(prefab);
				}
			}
			if (prefabPool != null)
			{
				Transform transform = instance.transform;
				if (prefabPool.despawned.Contains(transform))
				{
					return true;
				}
				if (prefabPool.spawned.Contains(transform))
				{
					SpawnManager.m_poolManager.Despawn(instance.transform, prefabPool);
					return true;
				}
			}
		}
		UnityEngine.Object.Destroy(instance);
		return false;
	}

	// Token: 0x0600879D RID: 34717 RVA: 0x00383A30 File Offset: 0x00381C30
	public static void SpawnBulletScript(GameActor owner, BulletScriptSelector bulletScript, Vector2? pos = null, Vector2? direction = null, bool collidesWithEnemies = false, string ownerName = null)
	{
		if (!owner || !owner.bulletBank)
		{
			return;
		}
		Vector2 vector = ((pos == null) ? owner.specRigidbody.GetUnitCenter(ColliderType.HitBox) : pos.Value);
		AIBulletBank bulletBank = owner.bulletBank;
		SpeculativeRigidbody specRigidbody = owner.specRigidbody;
		if (ownerName == null && owner)
		{
			if (owner.bulletBank)
			{
				ownerName = owner.bulletBank.ActorName;
			}
			else if (owner is AIActor)
			{
				ownerName = (owner as AIActor).GetActorName();
			}
		}
		SpawnManager.SpawnBulletScript(owner, vector, bulletBank, bulletScript, ownerName, specRigidbody, direction, collidesWithEnemies, null);
	}

	// Token: 0x0600879E RID: 34718 RVA: 0x00383AE8 File Offset: 0x00381CE8
	public static void SpawnBulletScript(GameActor owner, Vector2 pos, AIBulletBank sourceBulletBank, BulletScriptSelector bulletScript, string ownerName, SpeculativeRigidbody sourceRigidbody = null, Vector2? direction = null, bool collidesWithEnemies = false, Action<Bullet, Projectile> OnBulletCreated = null)
	{
		GameObject gameObject = new GameObject("Temp BulletScript Spawner");
		gameObject.transform.position = pos;
		AIBulletBank aibulletBank = gameObject.AddComponent<AIBulletBank>();
		aibulletBank.Bullets = new List<AIBulletBank.Entry>();
		for (int i = 0; i < sourceBulletBank.Bullets.Count; i++)
		{
			aibulletBank.Bullets.Add(new AIBulletBank.Entry(sourceBulletBank.Bullets[i]));
		}
		aibulletBank.useDefaultBulletIfMissing = sourceBulletBank.useDefaultBulletIfMissing;
		aibulletBank.transforms = new List<Transform>(sourceBulletBank.transforms);
		aibulletBank.PlayVfx = false;
		aibulletBank.PlayAudio = false;
		aibulletBank.CollidesWithEnemies = collidesWithEnemies;
		aibulletBank.gameActor = owner;
		if (owner is AIActor)
		{
			aibulletBank.aiActor = owner as AIActor;
		}
		aibulletBank.ActorName = ownerName;
		if (OnBulletCreated != null)
		{
			aibulletBank.OnBulletSpawned += OnBulletCreated;
		}
		aibulletBank.SpecificRigidbodyException = sourceRigidbody;
		if (direction != null)
		{
			aibulletBank.FixedPlayerPosition = new Vector2?(pos + direction.Value.normalized * 5f);
		}
		BulletScriptSource bulletScriptSource = gameObject.AddComponent<BulletScriptSource>();
		bulletScriptSource.BulletManager = aibulletBank;
		bulletScriptSource.BulletScript = bulletScript;
		bulletScriptSource.Initialize();
		BulletSourceKiller bulletSourceKiller = gameObject.AddComponent<BulletSourceKiller>();
		bulletSourceKiller.BraveSource = bulletScriptSource;
	}

	// Token: 0x0600879F RID: 34719 RVA: 0x00383C34 File Offset: 0x00381E34
	public static bool IsSpawned(GameObject instance)
	{
		return SpawnManager.m_poolManager != null && SpawnManager.m_poolManager.IsSpawned(instance.transform);
	}

	// Token: 0x060087A0 RID: 34720 RVA: 0x00383C5C File Offset: 0x00381E5C
	public static bool IsPooled(GameObject instance)
	{
		if (SpawnManager.m_poolManager != null)
		{
			GameObject prefab = SpawnManager.m_poolManager.GetPrefab(instance);
			if (prefab != null)
			{
				PrefabPool prefabPool = SpawnManager.m_poolManager.GetPrefabPool(prefab);
				Transform transform = instance.transform;
				if (prefabPool.despawned.Contains(transform) || prefabPool.spawned.Contains(transform))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060087A1 RID: 34721 RVA: 0x00383CCC File Offset: 0x00381ECC
	private static GameObject Spawn(GameObject prefab, Transform parent, bool ignoresPools = false)
	{
		return SpawnManager.Spawn(prefab, Vector3.zero, Quaternion.identity, parent, ignoresPools);
	}

	// Token: 0x060087A2 RID: 34722 RVA: 0x00383CE0 File Offset: 0x00381EE0
	private static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool ignoresPools = false)
	{
		if (prefab == null)
		{
			Debug.LogError("Attempting to spawn a null prefab!");
			return null;
		}
		if (ignoresPools)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
			gameObject.transform.parent = parent;
			return gameObject;
		}
		if (SpawnManager.m_poolManager == null)
		{
			SpawnManager.m_poolManager = PathologicalGames.PoolManager.Pools.Create("SpawnManager Pool");
		}
		return SpawnManager.m_poolManager.Spawn(prefab.transform, position, rotation, parent).gameObject;
	}

	// Token: 0x060087A3 RID: 34723 RVA: 0x00383D60 File Offset: 0x00381F60
	private static GameObject SpawnUnpooledInternal(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		gameObject.transform.parent = parent;
		return gameObject;
	}

	// Token: 0x060087A4 RID: 34724 RVA: 0x00383D84 File Offset: 0x00381F84
	private void AddObject(EphemeralObject obj)
	{
		LinkedListNode<EphemeralObject> linkedListNode = this.m_objects.First;
		bool flag = false;
		if (obj.Priority != EphemeralObject.EphemeralPriority.Critical)
		{
			obj.Priority = EphemeralObject.EphemeralPriority.Minor;
		}
		while (linkedListNode != null)
		{
			if (linkedListNode.Value.Priority >= obj.Priority)
			{
				this.m_objects.AddBefore(linkedListNode, obj);
				flag = true;
				break;
			}
			linkedListNode = linkedListNode.Next;
		}
		if (!flag)
		{
			this.m_objects.AddLast(obj);
		}
		if (this.UsesPerRoomObjectLimit)
		{
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(obj.transform.position.IntXY(VectorConversions.Floor));
			this.m_objectToRoomMap.Add(obj, absoluteRoomFromPosition);
			if (!this.m_objectsByRoom.ContainsKey(absoluteRoomFromPosition))
			{
				this.m_objectsByRoom.Add(absoluteRoomFromPosition, new LinkedList<EphemeralObject>());
			}
			linkedListNode = this.m_objectsByRoom[absoluteRoomFromPosition].First;
			flag = false;
			while (linkedListNode != null)
			{
				if (linkedListNode.Value.Priority > obj.Priority)
				{
					this.m_objectsByRoom[absoluteRoomFromPosition].AddBefore(linkedListNode, obj);
					flag = true;
					break;
				}
				linkedListNode = linkedListNode.Next;
			}
			if (!flag)
			{
				this.m_objectsByRoom[absoluteRoomFromPosition].AddLast(obj);
			}
			while (this.m_objectsByRoom[absoluteRoomFromPosition].Count > SpawnManager.m_instance.MaxObjectsPerRoom && this.m_objectsByRoom[absoluteRoomFromPosition].Last.Value.Priority != EphemeralObject.EphemeralPriority.Critical)
			{
				this.m_objectsByRoom[absoluteRoomFromPosition].Last.Value.TriggerDestruction(false);
			}
		}
		if (!this.m_removalCoroutineRunning && SpawnManager.m_instance.m_objects.Count > SpawnManager.m_instance.MaxObjects)
		{
			base.StartCoroutine(this.DeferredRemovalOfObjectsAboveLimit());
		}
	}

	// Token: 0x060087A5 RID: 34725 RVA: 0x00383F68 File Offset: 0x00382168
	private IEnumerator DeferredRemovalOfObjectsAboveLimit()
	{
		this.m_removalCoroutineRunning = true;
		while (SpawnManager.m_instance && SpawnManager.m_instance.m_objects.Count > SpawnManager.m_instance.MaxObjects)
		{
			if (GameManager.Instance.IsLoadingLevel)
			{
				yield return null;
			}
			else if (SpawnManager.m_instance.m_objects.Last.Value.Priority == EphemeralObject.EphemeralPriority.Critical)
			{
				yield return null;
			}
			else
			{
				SpawnManager.m_instance.m_objects.Last.Value.TriggerDestruction(false);
				if (SpawnManager.m_instance.m_objects.Count <= SpawnManager.m_instance.MaxObjects + 50)
				{
					yield return null;
				}
			}
		}
		this.m_removalCoroutineRunning = false;
		yield break;
	}

	// Token: 0x060087A6 RID: 34726 RVA: 0x00383F84 File Offset: 0x00382184
	private void RemoveObject(EphemeralObject obj)
	{
		if (this.UsesPerRoomObjectLimit && this.m_objectToRoomMap.ContainsKey(obj))
		{
			RoomHandler roomHandler = this.m_objectToRoomMap[obj];
			this.m_objectToRoomMap.Remove(obj);
			this.m_objectsByRoom[roomHandler].Remove(obj);
		}
		this.m_objects.Remove(obj);
	}

	// Token: 0x060087A7 RID: 34727 RVA: 0x00383FE8 File Offset: 0x003821E8
	public void ClearRectOfDecals(Vector2 minPos, Vector2 maxPos)
	{
		if (this.m_objects != null)
		{
			LinkedListNode<EphemeralObject> next;
			for (LinkedListNode<EphemeralObject> linkedListNode = this.m_objects.First; linkedListNode != null; linkedListNode = next)
			{
				next = linkedListNode.Next;
				if (linkedListNode.Value is DecalObject && linkedListNode.Value.transform.position.x > minPos.x && linkedListNode.Value.transform.position.x < maxPos.x && linkedListNode.Value.transform.position.y > minPos.y && linkedListNode.Value.transform.position.y < maxPos.y)
				{
					linkedListNode.Value.TriggerDestruction(false);
				}
			}
		}
	}

	// Token: 0x060087A8 RID: 34728 RVA: 0x003840D0 File Offset: 0x003822D0
	private void ClearRoomForDecal(Vector2 pos, EphemeralObject.EphemeralPriority priority, out bool cancelAddition)
	{
		cancelAddition = false;
		float num = pos.x - (float)this.MaxDecalAreaWidth;
		float num2 = pos.x + (float)this.MaxDecalAreaWidth;
		float num3 = pos.y - (float)this.MaxDecalAreaWidth;
		float num4 = pos.y + (float)this.MaxDecalAreaWidth;
		int num5 = 0;
		LinkedListNode<EphemeralObject> next;
		for (LinkedListNode<EphemeralObject> linkedListNode = this.m_objects.First; linkedListNode != null; linkedListNode = next)
		{
			next = linkedListNode.Next;
			if (linkedListNode.Value is DecalObject && linkedListNode.Value.transform.position.x > num && linkedListNode.Value.transform.position.x < num2 && linkedListNode.Value.transform.position.y > num3 && linkedListNode.Value.transform.position.y < num4)
			{
				if (num5 < this.MaxDecalPerArea - 1)
				{
					num5++;
				}
				else if (num5 == this.MaxDecalPerArea - 1)
				{
					num5++;
					if (linkedListNode.Value.Priority < priority)
					{
						cancelAddition = true;
					}
					else
					{
						linkedListNode.Value.TriggerDestruction(false);
					}
				}
				else
				{
					linkedListNode.Value.TriggerDestruction(false);
				}
			}
		}
	}

	// Token: 0x04008CC3 RID: 36035
	public Transform Debris;

	// Token: 0x04008CC4 RID: 36036
	public Transform Decals;

	// Token: 0x04008CC5 RID: 36037
	public Transform ParticleSystems;

	// Token: 0x04008CC6 RID: 36038
	public Transform Projectiles;

	// Token: 0x04008CC7 RID: 36039
	public Transform VFX;

	// Token: 0x04008CC8 RID: 36040
	[Header("Object Limit")]
	public int MaxObjects = 255;

	// Token: 0x04008CC9 RID: 36041
	public int CurrentObjects;

	// Token: 0x04008CCA RID: 36042
	public int MaxDecalPerArea = 5;

	// Token: 0x04008CCB RID: 36043
	public int MaxDecalAreaWidth = 2;

	// Token: 0x04008CCC RID: 36044
	[Header("Per-room Object Limit")]
	public bool UsesPerRoomObjectLimit;

	// Token: 0x04008CCD RID: 36045
	[ShowInInspectorIf("UsesPerRoomObjectLimit", false)]
	public int MaxObjectsPerRoom = 100;

	// Token: 0x04008CCE RID: 36046
	[ShowInInspectorIf("UsesPerRoomObjectLimit", false)]
	public int CurrentObjectsInRoom;

	// Token: 0x04008CCF RID: 36047
	private const int MAX_OBJECTS_HIGH = 800;

	// Token: 0x04008CD0 RID: 36048
	private const int MAX_OBJECTS_MED = 300;

	// Token: 0x04008CD1 RID: 36049
	private const int MAX_OBJECTS_LOW = 50;

	// Token: 0x04008CD3 RID: 36051
	private static SpawnPool m_poolManager;

	// Token: 0x04008CD4 RID: 36052
	private bool m_removalCoroutineRunning;

	// Token: 0x04008CD5 RID: 36053
	private static SpawnManager m_instance;

	// Token: 0x04008CD6 RID: 36054
	private LinkedList<EphemeralObject> m_objects = new LinkedList<EphemeralObject>();

	// Token: 0x04008CD7 RID: 36055
	private Dictionary<EphemeralObject, RoomHandler> m_objectToRoomMap = new Dictionary<EphemeralObject, RoomHandler>();

	// Token: 0x04008CD8 RID: 36056
	private Dictionary<RoomHandler, LinkedList<EphemeralObject>> m_objectsByRoom = new Dictionary<RoomHandler, LinkedList<EphemeralObject>>();
}
