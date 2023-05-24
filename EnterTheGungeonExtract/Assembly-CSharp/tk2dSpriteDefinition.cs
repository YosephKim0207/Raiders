using System;
using UnityEngine;

// Token: 0x02000BD2 RID: 3026
[Serializable]
public class tk2dSpriteDefinition
{
	// Token: 0x06004003 RID: 16387 RVA: 0x0014534C File Offset: 0x0014354C
	public Vector3[] ConstructExpensivePositions()
	{
		return new Vector3[] { this.position0, this.position1, this.position2, this.position3 };
	}

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06004004 RID: 16388 RVA: 0x001453A8 File Offset: 0x001435A8
	// (set) Token: 0x06004005 RID: 16389 RVA: 0x00145450 File Offset: 0x00143650
	public Vector3[] normals
	{
		get
		{
			if (tk2dSpriteDefinition.defaultNormals == null)
			{
				tk2dSpriteDefinition.defaultNormals = new Vector3[]
				{
					new Vector3(0f, 0f, -1f),
					new Vector3(0f, 0f, -1f),
					new Vector3(0f, 0f, -1f),
					new Vector3(0f, 0f, -1f)
				};
			}
			return tk2dSpriteDefinition.defaultNormals;
		}
		set
		{
		}
	}

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06004006 RID: 16390 RVA: 0x00145454 File Offset: 0x00143654
	// (set) Token: 0x06004007 RID: 16391 RVA: 0x00145510 File Offset: 0x00143710
	public Vector4[] tangents
	{
		get
		{
			if (tk2dSpriteDefinition.defaultTangents == null)
			{
				tk2dSpriteDefinition.defaultTangents = new Vector4[]
				{
					new Vector4(1f, 0f, 0f, 1f),
					new Vector4(1f, 0f, 0f, 1f),
					new Vector4(1f, 0f, 0f, 1f),
					new Vector4(1f, 0f, 0f, 1f)
				};
			}
			return tk2dSpriteDefinition.defaultTangents;
		}
		set
		{
		}
	}

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x06004008 RID: 16392 RVA: 0x00145514 File Offset: 0x00143714
	// (set) Token: 0x06004009 RID: 16393 RVA: 0x0014551C File Offset: 0x0014371C
	public int[] indices
	{
		get
		{
			return tk2dSpriteDefinition.defaultIndices;
		}
		set
		{
		}
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x00145520 File Offset: 0x00143720
	public BagelCollider[] GetBagelColliders(tk2dSpriteCollectionData collection, int spriteId)
	{
		return collection.GetBagelColliders(spriteId);
	}

	// Token: 0x0600400B RID: 16395 RVA: 0x0014552C File Offset: 0x0014372C
	public tk2dSpriteDefinition.AttachPoint[] GetAttachPoints(tk2dSpriteCollectionData collection, int spriteId)
	{
		return collection.GetAttachPoints(spriteId);
	}

	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x0600400C RID: 16396 RVA: 0x00145538 File Offset: 0x00143738
	public bool Valid
	{
		get
		{
			return this.name.Length != 0;
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x0600400D RID: 16397 RVA: 0x0014554C File Offset: 0x0014374C
	public bool IsTileSquare
	{
		get
		{
			if (this.m_cachedIsTileSquare == null)
			{
				this.m_cachedIsTileSquare = new bool?(this.CheckIsTileSquare());
			}
			return this.m_cachedIsTileSquare.Value;
		}
	}

	// Token: 0x0600400E RID: 16398 RVA: 0x0014557C File Offset: 0x0014377C
	private bool CheckIsTileSquare()
	{
		if (this.colliderVertices == null)
		{
			return false;
		}
		if (this.colliderVertices.Length == 2)
		{
			return Mathf.Approximately(this.colliderVertices[0].x, 0.5f) && Mathf.Approximately(this.colliderVertices[0].y, 0.5f) && Mathf.Approximately(this.colliderVertices[1].x, 0.5f) && Mathf.Approximately(this.colliderVertices[1].y, 0.5f);
		}
		if (this.colliderVertices.Length == 8)
		{
			for (int i = 0; i < 8; i++)
			{
				if (!Mathf.Approximately(this.colliderVertices[i].x, 0f) && !Mathf.Approximately(this.colliderVertices[i].x, 1f))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600400F RID: 16399 RVA: 0x00145688 File Offset: 0x00143888
	public Bounds GetBounds()
	{
		return new Bounds(new Vector3(this.boundsDataCenter.x, this.boundsDataCenter.y, this.boundsDataCenter.z), new Vector3(this.boundsDataExtents.x, this.boundsDataExtents.y, this.boundsDataExtents.z));
	}

	// Token: 0x06004010 RID: 16400 RVA: 0x001456E8 File Offset: 0x001438E8
	public Bounds GetUntrimmedBounds()
	{
		return new Bounds(new Vector3(this.untrimmedBoundsDataCenter.x, this.untrimmedBoundsDataCenter.y, this.untrimmedBoundsDataCenter.z), new Vector3(this.untrimmedBoundsDataExtents.x, this.untrimmedBoundsDataExtents.y, this.untrimmedBoundsDataExtents.z));
	}

	// Token: 0x040032E0 RID: 13024
	public string name;

	// Token: 0x040032E1 RID: 13025
	public Vector3 boundsDataCenter;

	// Token: 0x040032E2 RID: 13026
	public Vector3 boundsDataExtents;

	// Token: 0x040032E3 RID: 13027
	public Vector3 untrimmedBoundsDataCenter;

	// Token: 0x040032E4 RID: 13028
	public Vector3 untrimmedBoundsDataExtents;

	// Token: 0x040032E5 RID: 13029
	public Vector2 texelSize;

	// Token: 0x040032E6 RID: 13030
	public Vector3 position0;

	// Token: 0x040032E7 RID: 13031
	public Vector3 position1;

	// Token: 0x040032E8 RID: 13032
	public Vector3 position2;

	// Token: 0x040032E9 RID: 13033
	public Vector3 position3;

	// Token: 0x040032EA RID: 13034
	public static Vector3[] defaultNormals;

	// Token: 0x040032EB RID: 13035
	public static Vector4[] defaultTangents;

	// Token: 0x040032EC RID: 13036
	public Vector2[] uvs;

	// Token: 0x040032ED RID: 13037
	private static int[] defaultIndices = new int[] { 0, 3, 1, 2, 3, 0 };

	// Token: 0x040032EE RID: 13038
	public Material material;

	// Token: 0x040032EF RID: 13039
	[NonSerialized]
	public Material materialInst;

	// Token: 0x040032F0 RID: 13040
	public int materialId;

	// Token: 0x040032F1 RID: 13041
	public bool extractRegion;

	// Token: 0x040032F2 RID: 13042
	public int regionX;

	// Token: 0x040032F3 RID: 13043
	public int regionY;

	// Token: 0x040032F4 RID: 13044
	public int regionW;

	// Token: 0x040032F5 RID: 13045
	public int regionH;

	// Token: 0x040032F6 RID: 13046
	public tk2dSpriteDefinition.FlipMode flipped;

	// Token: 0x040032F7 RID: 13047
	public bool complexGeometry;

	// Token: 0x040032F8 RID: 13048
	public tk2dSpriteDefinition.PhysicsEngine physicsEngine;

	// Token: 0x040032F9 RID: 13049
	public tk2dSpriteDefinition.ColliderType colliderType;

	// Token: 0x040032FA RID: 13050
	public CollisionLayer collisionLayer;

	// Token: 0x040032FB RID: 13051
	[SerializeField]
	public TilesetIndexMetadata metadata;

	// Token: 0x040032FC RID: 13052
	public Vector3[] colliderVertices;

	// Token: 0x040032FD RID: 13053
	public bool colliderConvex;

	// Token: 0x040032FE RID: 13054
	public bool colliderSmoothSphereCollisions;

	// Token: 0x040032FF RID: 13055
	private bool? m_cachedIsTileSquare;

	// Token: 0x02000BD3 RID: 3027
	public enum ColliderType
	{
		// Token: 0x04003301 RID: 13057
		Unset,
		// Token: 0x04003302 RID: 13058
		None,
		// Token: 0x04003303 RID: 13059
		Box,
		// Token: 0x04003304 RID: 13060
		Mesh
	}

	// Token: 0x02000BD4 RID: 3028
	public enum PhysicsEngine
	{
		// Token: 0x04003306 RID: 13062
		Physics3D,
		// Token: 0x04003307 RID: 13063
		Physics2D
	}

	// Token: 0x02000BD5 RID: 3029
	public enum FlipMode
	{
		// Token: 0x04003309 RID: 13065
		None,
		// Token: 0x0400330A RID: 13066
		Tk2d,
		// Token: 0x0400330B RID: 13067
		TPackerCW
	}

	// Token: 0x02000BD6 RID: 3030
	[Serializable]
	public class AttachPoint
	{
		// Token: 0x06004013 RID: 16403 RVA: 0x00145780 File Offset: 0x00143980
		public void CopyFrom(tk2dSpriteDefinition.AttachPoint src)
		{
			this.name = src.name;
			this.position = src.position;
			this.angle = src.angle;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x001457A8 File Offset: 0x001439A8
		public bool CompareTo(tk2dSpriteDefinition.AttachPoint src)
		{
			return this.name == src.name && src.position == this.position && src.angle == this.angle;
		}

		// Token: 0x0400330C RID: 13068
		public string name = string.Empty;

		// Token: 0x0400330D RID: 13069
		public Vector3 position = Vector3.zero;

		// Token: 0x0400330E RID: 13070
		public float angle;
	}
}
