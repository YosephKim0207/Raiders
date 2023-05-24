// SpawnManager에서 SpawnProjectile Call(총알이랑 Enemy랑 별개의 함수로 구분. 참고할 )
// ignoresPools를 사용하는 경우 
public static GameObject SpawnProjectile(GameObject prefab, Vector3 position, Quaternion rotation, bool ignoresPools = true) {
	if (!SpawnManager.m_instance) {
		return null;
	}
	return SpawnManager.Spawn(prefab, position, rotation, SpawnManager.m_instance.Projectiles, ignoresPools);
}



private static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, bool ignoresPools = false) {
	if (prefab == null) {
		Debug.LogError("Attempting to spawn a null prefab!");
		return null;
	}
	if (ignoresPools) {
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		gameObject.transform.parent = parent;
		return gameObject;
	}
	if (SpawnManager.m_poolManager == null) {
		SpawnManager.m_poolManager = PathologicalGames.PoolManager.Pools.Create("SpawnManager Pool");
	}
	return SpawnManager.m_poolManager.Spawn(prefab.transform, position, rotation, parent).gameObject;
}


public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot, Transform parent) {
	int i = 0;
	Transform transform;
	while (i < this._prefabPools.Count) {
		if (this._prefabPools[i].prefabGO == prefab.gameObject) {
			transform = this._prefabPools[i].SpawnInstance(pos, rot);
			if (transform == null) {
				return null;
			}
			if (parent != null) {
				transform.parent = parent;
			}
			else if (!this.dontReparent && transform.parent != this.group) {
				transform.parent = this.group;
			}
			this._spawned.Add(transform);
			transform.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
			return transform;
		}
		else {
			i++;
		}
	}
	PrefabPool prefabPool = new PrefabPool(prefab);
	this.CreatePrefabPool(prefabPool);
	transform = prefabPool.SpawnInstance(pos, rot);
	if (parent != null) {
		transform.parent = parent;
	}
	else {
		transform.parent = this.group;
	}
	this._spawned.Add(transform);
	transform.gameObject.BroadcastMessage("OnSpawned", this, SendMessageOptions.DontRequireReceiver);
	return transform;
}


// _prefabPools[]에서 사용하는 SpawnInstance()의 구현부 보기
internal Transform SpawnInstance(Vector3 pos, Quaternion rot) {
	SpawnManager.LastPrefabPool = this;
	if (this.limitInstances && this.limitFIFO && this._spawned.Count >= this.limitAmount) {
		Transform transform = this._spawned[0];
		if (this.logMessages) {
			Debug.Log(string.Format("SpawnPool {0} ({1}): LIMIT REACHED! FIFO=True. Calling despawning for {2}...", this.spawnPool.poolName, this.prefab.name, transform));
		}
		this.DespawnInstance(transform);
		this.spawnPool._spawned.Remove(transform);
	}
	Transform transform2;
	if (this._despawned.Count == 0) {
		transform2 = this.SpawnNew(pos, rot);
	}
	else {
		transform2 = null;
		while (transform2 == null) {
			if (this._despawned.Count == 0) {
				transform2 = this.SpawnNew(pos, rot);
			}
			else {
				transform2 = this._despawned[0];
				this._despawned.RemoveAt(0);
				if (transform2 != null) {
					this._spawned.Add(transform2);
				}
			}
		}
		if (this.logMessages) {
			Debug.Log(string.Format("SpawnPool {0} ({1}): respawning '{2}'.", this.spawnPool.poolName, this.prefab.name, transform2.name));
		}
		transform2.position = pos;
		transform2.rotation = rot;
		PoolManagerUtils.SetActive(transform2.gameObject, true);
	}
	return transform2;
}

public Transform SpawnNew(Vector3 pos, Quaternion rot) {
	if (this.limitInstances && this.totalCount >= this.limitAmount) {
		if (this.logMessages) {
			Debug.Log(string.Format("SpawnPool {0} ({1}): LIMIT REACHED! Not creating new instances! (Returning null)", this.spawnPool.poolName, this.prefab.name));
		}
		return null;
	}
	if (pos == Vector3.zero) {
		pos = this.spawnPool.group.position;
	}
	if (rot == Quaternion.identity) {
		rot = this.spawnPool.group.rotation;
	}
	Transform transform = UnityEngine.Object.Instantiate<Transform>(this.prefab, pos, rot);
	this.nameInstance(transform);
	if (!this.spawnPool.dontReparent) {
		transform.parent = this.spawnPool.group;
	}
	if (this.spawnPool.matchPoolScale) {
		transform.localScale = Vector3.one;
	}
	if (this.spawnPool.matchPoolLayer) {
		this.SetRecursively(transform, this.spawnPool.gameObject.layer);
	}
	this._spawned.Add(transform);
	if (this.logMessages) {
		Debug.Log(string.Format("SpawnPool {0} ({1}): Spawned new instance '{2}'.", this.spawnPool.poolName, this.prefab.name, transform.name));
	}
	return transform;
}