using System;
using AK.Wwise;
using UnityEngine;

// Token: 0x0200190A RID: 6410
[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("Wwise/AkSurfaceReflector")]
public class AkSurfaceReflector : MonoBehaviour
{
	// Token: 0x06009DFD RID: 40445 RVA: 0x003F1E98 File Offset: 0x003F0098
	public static void AddGeometrySet(AcousticTexture acousticTexture, MeshFilter meshFilter)
	{
		if (meshFilter == null)
		{
			Debug.Log(meshFilter.name + ": No mesh found!");
		}
		else
		{
			Mesh sharedMesh = meshFilter.sharedMesh;
			Vector3[] vertices = sharedMesh.vertices;
			int[] triangles = sharedMesh.triangles;
			int num = sharedMesh.triangles.Length / 3;
			using (AkTriangleArray akTriangleArray = new AkTriangleArray(num))
			{
				for (int i = 0; i < num; i++)
				{
					using (AkTriangle triangle = akTriangleArray.GetTriangle(i))
					{
						Vector3 vector = meshFilter.transform.TransformPoint(vertices[triangles[3 * i]]);
						Vector3 vector2 = meshFilter.transform.TransformPoint(vertices[triangles[3 * i + 1]]);
						Vector3 vector3 = meshFilter.transform.TransformPoint(vertices[triangles[3 * i + 2]]);
						triangle.point0.X = vector.x;
						triangle.point0.Y = vector.y;
						triangle.point0.Z = vector.z;
						triangle.point1.X = vector2.x;
						triangle.point1.Y = vector2.y;
						triangle.point1.Z = vector2.z;
						triangle.point2.X = vector3.x;
						triangle.point2.Y = vector3.y;
						triangle.point2.Z = vector3.z;
						triangle.textureID = (uint)acousticTexture.ID;
						triangle.reflectorChannelMask = uint.MaxValue;
						triangle.strName = meshFilter.gameObject.name + "_" + i;
					}
				}
				AkSoundEngine.SetGeometry((ulong)((long)meshFilter.GetInstanceID()), akTriangleArray, (uint)num);
			}
		}
	}

	// Token: 0x06009DFE RID: 40446 RVA: 0x003F20C4 File Offset: 0x003F02C4
	public static void RemoveGeometrySet(MeshFilter meshFilter)
	{
		if (meshFilter != null)
		{
			AkSoundEngine.RemoveGeometry((ulong)((long)meshFilter.GetInstanceID()));
		}
	}

	// Token: 0x06009DFF RID: 40447 RVA: 0x003F20E0 File Offset: 0x003F02E0
	private void Awake()
	{
		this.MeshFilter = base.GetComponent<MeshFilter>();
	}

	// Token: 0x06009E00 RID: 40448 RVA: 0x003F20F0 File Offset: 0x003F02F0
	private void OnEnable()
	{
		AkSurfaceReflector.AddGeometrySet(this.AcousticTexture, this.MeshFilter);
	}

	// Token: 0x06009E01 RID: 40449 RVA: 0x003F2104 File Offset: 0x003F0304
	private void OnDisable()
	{
		AkSurfaceReflector.RemoveGeometrySet(this.MeshFilter);
	}

	// Token: 0x04009F5C RID: 40796
	public AcousticTexture AcousticTexture;

	// Token: 0x04009F5D RID: 40797
	private MeshFilter MeshFilter;
}
