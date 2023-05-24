using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200044D RID: 1101
[AddComponentMenu("Daikon Forge/Examples/Object Pooling/Pooled Object")]
public class dfPooledObject : MonoBehaviour
{
	// Token: 0x14000057 RID: 87
	// (add) Token: 0x0600195F RID: 6495 RVA: 0x000772A8 File Offset: 0x000754A8
	// (remove) Token: 0x06001960 RID: 6496 RVA: 0x000772E0 File Offset: 0x000754E0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfPooledObject.SpawnEventHandler Spawned;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06001961 RID: 6497 RVA: 0x00077318 File Offset: 0x00075518
	// (remove) Token: 0x06001962 RID: 6498 RVA: 0x00077350 File Offset: 0x00075550
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event dfPooledObject.SpawnEventHandler Despawned;

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x06001963 RID: 6499 RVA: 0x00077388 File Offset: 0x00075588
	// (set) Token: 0x06001964 RID: 6500 RVA: 0x00077390 File Offset: 0x00075590
	public dfPoolManager.ObjectPool Pool { get; set; }

	// Token: 0x06001965 RID: 6501 RVA: 0x0007739C File Offset: 0x0007559C
	private void Awake()
	{
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x000773A0 File Offset: 0x000755A0
	private void OnEnable()
	{
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x000773A4 File Offset: 0x000755A4
	private void OnDisable()
	{
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000773A8 File Offset: 0x000755A8
	private void OnDestroy()
	{
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x000773AC File Offset: 0x000755AC
	public void Despawn()
	{
		this.Pool.Despawn(base.gameObject);
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000773C0 File Offset: 0x000755C0
	internal void OnSpawned()
	{
		if (this.Spawned != null)
		{
			this.Spawned(base.gameObject);
		}
		base.SendMessage("OnObjectSpawned", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x000773EC File Offset: 0x000755EC
	internal void OnDespawned()
	{
		if (this.Despawned != null)
		{
			this.Despawned(base.gameObject);
		}
		base.SendMessage("OnObjectDespawned", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x0200044E RID: 1102
	// (Invoke) Token: 0x0600196D RID: 6509
	public delegate void SpawnEventHandler(GameObject instance);
}
