using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020012A4 RID: 4772
public class MeduziWaterHelper : BraveBehaviour
{
	// Token: 0x06006AC3 RID: 27331 RVA: 0x0029DE24 File Offset: 0x0029C024
	private void Start()
	{
		AIActor component = base.transform.parent.GetComponent<AIActor>();
		this.m_room = component.ParentRoom;
		base.transform.parent = this.m_room.hierarchyParent;
		this.m_cachedReflectionsEnabled = GameManager.Options.RealtimeReflections;
		this.ToggleToState(this.m_cachedReflectionsEnabled);
	}

	// Token: 0x06006AC4 RID: 27332 RVA: 0x0029DE80 File Offset: 0x0029C080
	private void Update()
	{
		if (this.m_cachedReflectionsEnabled != GameManager.Options.RealtimeReflections)
		{
			this.m_cachedReflectionsEnabled = GameManager.Options.RealtimeReflections;
			this.ToggleToState(this.m_cachedReflectionsEnabled);
		}
	}

	// Token: 0x06006AC5 RID: 27333 RVA: 0x0029DEB4 File Offset: 0x0029C0B4
	private void ToggleToState(bool refl)
	{
		if (!this.m_quadInstance)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ReflectionQuadPrefab);
			this.m_quadInstance = gameObject.transform;
			this.m_quadInstance.position = this.m_room.GetCenterCell().ToVector3();
			this.m_quadInstance.position = this.m_quadInstance.position.WithZ(this.m_quadInstance.position.y - 16f);
		}
		Material sharedMaterial = this.m_quadInstance.GetComponent<MeshRenderer>().sharedMaterial;
		if (refl)
		{
			this.m_quadInstance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Reflection"));
			sharedMaterial.shader = ShaderCache.Acquire("Brave/ReflectionOnly");
			this.floorWaterMaterial.SetColor("_LightCausticColor", new Color(0.5f, 0.5f, 0.5f));
		}
		else
		{
			this.m_quadInstance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
			sharedMaterial.shader = ShaderCache.Acquire("Particles/Additive");
			this.floorWaterMaterial.SetColor("_LightCausticColor", new Color(1f, 1f, 1f));
		}
	}

	// Token: 0x06006AC6 RID: 27334 RVA: 0x0029DFF4 File Offset: 0x0029C1F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006762 RID: 26466
	public GameObject ReflectionQuadPrefab;

	// Token: 0x04006763 RID: 26467
	public Material floorWaterMaterial;

	// Token: 0x04006764 RID: 26468
	private Transform m_quadInstance;

	// Token: 0x04006765 RID: 26469
	private RoomHandler m_room;

	// Token: 0x04006766 RID: 26470
	private bool m_cachedReflectionsEnabled;
}
