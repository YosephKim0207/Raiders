using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	// Token: 0x02000842 RID: 2114
	[AddComponentMenu("Path-o-logical/PoolManager/SpawnPool")]
	public sealed class SpawnPool : MonoBehaviour, IList<Transform>, ICollection<Transform>, IEnumerable<Transform>, IEnumerable
	{
		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002E20 RID: 11808 RVA: 0x000EFA24 File Offset: 0x000EDC24
		// (set) Token: 0x06002E21 RID: 11809 RVA: 0x000EFA2C File Offset: 0x000EDC2C
		public bool dontDestroyOnLoad
		{
			get
			{
				return this._dontDestroyOnLoad;
			}
			set
			{
				this._dontDestroyOnLoad = value;
				if (this.group != null)
				{
					UnityEngine.Object.DontDestroyOnLoad(this.group.gameObject);
				}
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002E22 RID: 11810 RVA: 0x000EFA58 File Offset: 0x000EDC58
		// (set) Token: 0x06002E23 RID: 11811 RVA: 0x000EFA60 File Offset: 0x000EDC60
		public Transform group { get; private set; }

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x000EFA6C File Offset: 0x000EDC6C
		public Dictionary<string, PrefabPool> prefabPools
		{
			get
			{
				Dictionary<string, PrefabPool> dictionary = new Dictionary<string, PrefabPool>();
				for (int i = 0; i < this._prefabPools.Count; i++)
				{
					dictionary[this._prefabPools[i].prefabGO.name] = this._prefabPools[i];
				}
				return dictionary;
			}
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000EFAC4 File Offset: 0x000EDCC4
		private void Awake()
		{
			if (this._dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			this.group = base.transform;
			if (this.poolName == string.Empty)
			{
				this.poolName = this.group.name.Replace("Pool", string.Empty);
				this.poolName = this.poolName.Replace("(Clone)", string.Empty);
			}
			if (this.logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0}: Initializing..", this.poolName));
			}
			for (int i = 0; i < this._perPrefabPoolOptions.Count; i++)
			{
				if (this._perPrefabPoolOptions[i].prefab == null)
				{
					Debug.LogWarning(string.Format("Initialization Warning: Pool '{0}' contains a PrefabPool with no prefab reference. Skipping.", this.poolName));
				}
				else
				{
					this._perPrefabPoolOptions[i].inspectorInstanceConstructor();
					this.CreatePrefabPool(this._perPrefabPoolOptions[i]);
				}
			}
			PoolManager.Pools.Add(this);
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000EFBE4 File Offset: 0x000EDDE4
		private void OnDestroy()
		{
			if (this.logMessages)
			{
				Debug.Log(string.Format("SpawnPool {0}: Destroying...", this.poolName));
			}
			PoolManager.Pools.Remove(this);
			base.StopAllCoroutines();
			this._spawned.Clear();
			foreach (PrefabPool prefabPool in this._prefabPools)
			{
				prefabPool.SelfDestruct();
			}
			this._prefabPools.Clear();
			this.prefabs._Clear();
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000EFC94 File Offset: 0x000EDE94
		public void CreatePrefabPool(PrefabPool prefabPool)
		{
			if (this.GetPrefabPool(prefabPool.prefab) == null)
			{
				prefabPool.spawnPool = this;
				this._prefabPools.Add(prefabPool);
				if (this.prefabs.ContainsKey(prefabPool.prefab.name))
				{
					Debug.LogError("Duplicate prefab name: " + prefabPool.prefab.name);
				}
				else
				{
					this.prefabs._Add(prefabPool.prefab.name, prefabPool.prefab);
				}
			}
			if (!prefabPool.preloaded)
			{
				if (this.logMessages)
				{
					Debug.Log(string.Format("SpawnPool {0}: Preloading {1} {2}", this.poolName, prefabPool.preloadAmount, prefabPool.prefab.name));
				}
				prefabPool.PreloadInstances();
			}
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000EFD70 File Offset: 0x000EDF70
		public void Add(Transform instance, string prefabName, bool despawn, bool parent)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				if (this._prefabPools[i].prefabGO == null)
				{
					Debug.LogError("Unexpected Error: PrefabPool.prefabGO is null");
					return;
				}
				if (this._prefabPools[i].prefabGO.name == prefabName)
				{
					this._prefabPools[i].AddUnpooled(instance, despawn);
					if (this.logMessages)
					{
						Debug.Log(string.Format("SpawnPool {0}: Adding previously unpooled instance {1}", this.poolName, instance.name));
					}
					if (parent)
					{
						instance.parent = this.group;
					}
					if (!despawn)
					{
						this._spawned.Add(instance);
					}
					return;
				}
			}
			Debug.LogError(string.Format("SpawnPool {0}: PrefabPool {1} not found.", this.poolName, prefabName));
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000EFE58 File Offset: 0x000EE058
		public void Add(Transform item)
		{
			string text = "Use SpawnPool.Spawn() to properly add items to the pool.";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000EFE74 File Offset: 0x000EE074
		public void Remove(Transform item)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				if (this._prefabPools[i]._spawned.Contains(item) || this._prefabPools[i]._despawned.Contains(item))
				{
					this._prefabPools[i].RemoveInstance(item);
				}
			}
			this._spawned.Remove(item);
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000EFEF4 File Offset: 0x000EE0F4
		public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot, Transform parent)
		{
			int i = 0;
			Transform transform;
			while (i < this._prefabPools.Count)
			{
				if (this._prefabPools[i].prefabGO == prefab.gameObject)
				{
					transform = this._prefabPools[i].SpawnInstance(pos, rot);
					if (transform == null)
					{
						return null;
					}
					if (parent != null)
					{
						transform.parent = parent;
					}
					else if (!this.dontReparent && transform.parent != this.group)
					{
						transform.parent = this.group;
					}
					this._spawned.Add(transform);
					transform.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
					return transform;
				}
				else
				{
					i++;
				}
			}
			PrefabPool prefabPool = new PrefabPool(prefab);
			this.CreatePrefabPool(prefabPool);
			transform = prefabPool.SpawnInstance(pos, rot);
			if (parent != null)
			{
				transform.parent = parent;
			}
			else
			{
				transform.parent = this.group;
			}
			this._spawned.Add(transform);
			transform.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
			return transform;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000F0024 File Offset: 0x000EE224
		public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
		{
			Transform transform = this.Spawn(prefab, pos, rot, null);
			if (transform == null)
			{
				return null;
			}
			return transform;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000F004C File Offset: 0x000EE24C
		public Transform Spawn(Transform prefab)
		{
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity);
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x000F0060 File Offset: 0x000EE260
		public Transform Spawn(Transform prefab, Transform parent)
		{
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000F0074 File Offset: 0x000EE274
		public Transform Spawn(string prefabName)
		{
			Transform transform = this.prefabs[prefabName];
			return this.Spawn(transform);
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000F0098 File Offset: 0x000EE298
		public Transform Spawn(string prefabName, Transform parent)
		{
			Transform transform = this.prefabs[prefabName];
			return this.Spawn(transform, parent);
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000F00BC File Offset: 0x000EE2BC
		public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot)
		{
			Transform transform = this.prefabs[prefabName];
			return this.Spawn(transform, pos, rot);
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000F00E0 File Offset: 0x000EE2E0
		public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot, Transform parent)
		{
			Transform transform = this.prefabs[prefabName];
			return this.Spawn(transform, pos, rot, parent);
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000F0108 File Offset: 0x000EE308
		public AudioSource Spawn(AudioSource prefab, Vector3 pos, Quaternion rot)
		{
			return this.Spawn(prefab, pos, rot, null);
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000F0114 File Offset: 0x000EE314
		public AudioSource Spawn(AudioSource prefab)
		{
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x000F0128 File Offset: 0x000EE328
		public AudioSource Spawn(AudioSource prefab, Transform parent)
		{
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000F013C File Offset: 0x000EE33C
		public AudioSource Spawn(AudioSource prefab, Vector3 pos, Quaternion rot, Transform parent)
		{
			Transform transform = this.Spawn(prefab.transform, pos, rot, parent);
			if (transform == null)
			{
				return null;
			}
			AudioSource component = transform.GetComponent<AudioSource>();
			component.Play();
			base.StartCoroutine(this.ListForAudioStop(component));
			return component;
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000F0184 File Offset: 0x000EE384
		public ParticleSystem Spawn(ParticleSystem prefab, Vector3 pos, Quaternion rot)
		{
			return this.Spawn(prefab, pos, rot, null);
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000F0190 File Offset: 0x000EE390
		public ParticleSystem Spawn(ParticleSystem prefab, Vector3 pos, Quaternion rot, Transform parent)
		{
			Transform transform = this.Spawn(prefab.transform, pos, rot, parent);
			if (transform == null)
			{
				return null;
			}
			ParticleSystem component = transform.GetComponent<ParticleSystem>();
			base.StartCoroutine(this.ListenForEmitDespawn(component));
			return component;
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000F01D4 File Offset: 0x000EE3D4
		public void Despawn(Transform instance, PrefabPool prefabPool = null)
		{
			bool flag = false;
			if (prefabPool != null)
			{
				if (prefabPool._spawned.Contains(instance))
				{
					flag = prefabPool.DespawnInstance(instance);
				}
				else if (prefabPool._despawned.Contains(instance))
				{
					Debug.LogError(string.Format("SpawnPool {0}: {1} has already been despawned. You cannot despawn something more than once!", this.poolName, instance.name));
					return;
				}
			}
			if (!flag)
			{
				for (int i = 0; i < this._prefabPools.Count; i++)
				{
					if (this._prefabPools[i]._spawned.Contains(instance))
					{
						flag = this._prefabPools[i].DespawnInstance(instance);
						break;
					}
					if (this._prefabPools[i]._despawned.Contains(instance))
					{
						Debug.LogError(string.Format("SpawnPool {0}: {1} has already been despawned. You cannot despawn something more than once!", this.poolName, instance.name));
						return;
					}
				}
			}
			if (!flag)
			{
				Debug.LogError(string.Format("SpawnPool {0}: {1} not found in SpawnPool", this.poolName, instance.name));
				return;
			}
			this._spawned.Remove(instance);
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000F02F4 File Offset: 0x000EE4F4
		public void Despawn(Transform instance, Transform parent)
		{
			instance.parent = parent;
			this.Despawn(instance, null);
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x000F0308 File Offset: 0x000EE508
		public void Despawn(Transform instance, float seconds)
		{
			base.StartCoroutine(this.DoDespawnAfterSeconds(instance, seconds, false, null));
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x000F031C File Offset: 0x000EE51C
		public void Despawn(Transform instance, float seconds, Transform parent)
		{
			base.StartCoroutine(this.DoDespawnAfterSeconds(instance, seconds, true, parent));
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000F0330 File Offset: 0x000EE530
		private IEnumerator DoDespawnAfterSeconds(Transform instance, float seconds, bool useParent, Transform parent)
		{
			GameObject go = instance.gameObject;
			while (seconds > 0f)
			{
				yield return null;
				if (!go.activeInHierarchy)
				{
					yield break;
				}
				seconds -= BraveTime.DeltaTime;
			}
			if (useParent)
			{
				this.Despawn(instance, parent);
			}
			else
			{
				this.Despawn(instance, null);
			}
			yield break;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000F0368 File Offset: 0x000EE568
		public void DespawnAll()
		{
			List<Transform> list = new List<Transform>(this._spawned);
			for (int i = 0; i < list.Count; i++)
			{
				this.Despawn(list[i], null);
			}
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000F03A8 File Offset: 0x000EE5A8
		public bool IsSpawned(Transform instance)
		{
			return this._spawned.Contains(instance);
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000F03B8 File Offset: 0x000EE5B8
		public PrefabPool GetPrefabPool(Transform prefab)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				if (this._prefabPools[i].prefabGO == null)
				{
					Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null", this.poolName));
				}
				if (this._prefabPools[i].prefabGO == prefab.gameObject)
				{
					return this._prefabPools[i];
				}
			}
			return null;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000F0444 File Offset: 0x000EE644
		public PrefabPool GetPrefabPool(GameObject prefab)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				if (this._prefabPools[i].prefabGO == null)
				{
					Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null", this.poolName));
				}
				if (this._prefabPools[i].prefabGO == prefab)
				{
					return this._prefabPools[i];
				}
			}
			return null;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000F04C8 File Offset: 0x000EE6C8
		public Transform GetPrefab(Transform instance)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				if (this._prefabPools[i].Contains(instance))
				{
					return this._prefabPools[i].prefab;
				}
			}
			return null;
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000F051C File Offset: 0x000EE71C
		public GameObject GetPrefab(GameObject instance)
		{
			for (int i = 0; i < this._prefabPools.Count; i++)
			{
				if (this._prefabPools[i].Contains(instance.transform))
				{
					return this._prefabPools[i].prefabGO;
				}
			}
			return null;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000F0574 File Offset: 0x000EE774
		private IEnumerator ListForAudioStop(AudioSource src)
		{
			yield return null;
			while (src.isPlaying)
			{
				yield return null;
			}
			this.Despawn(src.transform, null);
			yield break;
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000F0598 File Offset: 0x000EE798
		private IEnumerator ListenForEmitDespawn(ParticleSystem emitter)
		{
			yield return new WaitForSeconds(emitter.startDelay + 0.25f);
			float safetimer = 0f;
			while (emitter.IsAlive(true))
			{
				if (!PoolManagerUtils.activeInHierarchy(emitter.gameObject))
				{
					emitter.Clear(true);
					yield break;
				}
				safetimer += BraveTime.DeltaTime;
				if (safetimer > this.maxParticleDespawnTime)
				{
					Debug.LogWarning(string.Format("SpawnPool {0}: Timed out while listening for all particles to die. Waited for {1}sec.", this.poolName, this.maxParticleDespawnTime));
				}
				yield return null;
			}
			this.Despawn(emitter.transform, null);
			yield break;
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000F05BC File Offset: 0x000EE7BC
		public override string ToString()
		{
			List<string> list = new List<string>();
			foreach (Transform transform in this._spawned)
			{
				list.Add(transform.name);
			}
			return string.Join(", ", list.ToArray());
		}

		// Token: 0x1700088D RID: 2189
		public Transform this[int index]
		{
			get
			{
				return this._spawned[index];
			}
			set
			{
				throw new NotImplementedException("Read-only.");
			}
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000F0650 File Offset: 0x000EE850
		public bool Contains(Transform item)
		{
			string text = "Use IsSpawned(Transform instance) instead.";
			throw new NotImplementedException(text);
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000F066C File Offset: 0x000EE86C
		public void CopyTo(Transform[] array, int arrayIndex)
		{
			this._spawned.CopyTo(array, arrayIndex);
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002E4B RID: 11851 RVA: 0x000F067C File Offset: 0x000EE87C
		public int Count
		{
			get
			{
				return this._spawned.Count;
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000F068C File Offset: 0x000EE88C
		public IEnumerator<Transform> GetEnumerator()
		{
			for (int i = 0; i < this._spawned.Count; i++)
			{
				yield return this._spawned[i];
			}
			yield break;
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000F06A8 File Offset: 0x000EE8A8
		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < this._spawned.Count; i++)
			{
				yield return this._spawned[i];
			}
			yield break;
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000F06C4 File Offset: 0x000EE8C4
		public int IndexOf(Transform item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000F06CC File Offset: 0x000EE8CC
		public void Insert(int index, Transform item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000F06D4 File Offset: 0x000EE8D4
		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000F06DC File Offset: 0x000EE8DC
		public void Clear()
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002E52 RID: 11858 RVA: 0x000F06E4 File Offset: 0x000EE8E4
		public bool IsReadOnly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000F06EC File Offset: 0x000EE8EC
		bool ICollection<Transform>.Remove(Transform item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001F31 RID: 7985
		public string poolName = string.Empty;

		// Token: 0x04001F32 RID: 7986
		public bool matchPoolScale;

		// Token: 0x04001F33 RID: 7987
		public bool matchPoolLayer;

		// Token: 0x04001F34 RID: 7988
		public bool dontReparent;

		// Token: 0x04001F35 RID: 7989
		public bool _dontDestroyOnLoad;

		// Token: 0x04001F36 RID: 7990
		public bool logMessages;

		// Token: 0x04001F37 RID: 7991
		public List<PrefabPool> _perPrefabPoolOptions = new List<PrefabPool>();

		// Token: 0x04001F38 RID: 7992
		public Dictionary<object, bool> prefabsFoldOutStates = new Dictionary<object, bool>();

		// Token: 0x04001F39 RID: 7993
		public float maxParticleDespawnTime = 300f;

		// Token: 0x04001F3B RID: 7995
		public PrefabsDict prefabs = new PrefabsDict();

		// Token: 0x04001F3C RID: 7996
		public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

		// Token: 0x04001F3D RID: 7997
		private List<PrefabPool> _prefabPools = new List<PrefabPool>();

		// Token: 0x04001F3E RID: 7998
		internal List<Transform> _spawned = new List<Transform>();
	}
}
