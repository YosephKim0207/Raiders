using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001297 RID: 4759
[ExecuteInEditMode]
public class ExperimentalGodrayHelper : MonoBehaviour
{
	// Token: 0x06006A83 RID: 27267 RVA: 0x0029C364 File Offset: 0x0029A564
	private IEnumerator Start()
	{
		Bounds bs = base.GetComponent<tk2dBaseSprite>().GetBounds();
		base.GetComponent<Renderer>().sharedMaterial.SetVector("_MeshBoundsCenter", bs.center);
		base.GetComponent<Renderer>().sharedMaterial.SetVector("_MeshBoundsExtents", bs.extents);
		yield return null;
		if (Application.isPlaying)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			Bounds bounds = component.mesh.bounds;
			bounds.Expand(new Vector3(0f, bounds.extents.y * 12f, 0f));
			component.mesh.bounds = bounds;
		}
		yield break;
	}
}
