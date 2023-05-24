// ignoresPools를 사용하지 않는 경우 
// 아마도 pool을 사용하지 않는 경우? 이대로 투사체 call 끝남

public static GameObject SpawnProjectile(string resourcePath, Vector3 position, Quaternion rotation) {
	return SpawnManager.SpawnUnpooledInternal(BraveResources.Load<GameObject>(resourcePath, ".prefab"), position, rotation, SpawnManager.m_instance.Projectiles);
}

private static GameObject SpawnUnpooledInternal(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
	GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
	gameObject.transform.parent = parent;
	return gameObject;
}

