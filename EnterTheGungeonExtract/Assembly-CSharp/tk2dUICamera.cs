using System;
using UnityEngine;

// Token: 0x02000C18 RID: 3096
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUICamera")]
public class tk2dUICamera : MonoBehaviour
{
	// Token: 0x06004265 RID: 16997 RVA: 0x001576D4 File Offset: 0x001558D4
	public void AssignRaycastLayerMask(LayerMask mask)
	{
		this.raycastLayerMask = mask;
	}

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x06004266 RID: 16998 RVA: 0x001576E0 File Offset: 0x001558E0
	public LayerMask FilteredMask
	{
		get
		{
			return this.raycastLayerMask & base.GetComponent<Camera>().cullingMask;
		}
	}

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x06004267 RID: 16999 RVA: 0x00157700 File Offset: 0x00155900
	public Camera HostCamera
	{
		get
		{
			return base.GetComponent<Camera>();
		}
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x00157708 File Offset: 0x00155908
	private void OnEnable()
	{
		if (base.GetComponent<Camera>() == null)
		{
			Debug.LogError("tk2dUICamera should only be attached to a camera.");
			base.enabled = false;
			return;
		}
		tk2dUIManager.RegisterCamera(this);
	}

	// Token: 0x06004269 RID: 17001 RVA: 0x00157734 File Offset: 0x00155934
	private void OnDisable()
	{
		tk2dUIManager.UnregisterCamera(this);
	}

	// Token: 0x040034C1 RID: 13505
	[SerializeField]
	private LayerMask raycastLayerMask = -1;
}
