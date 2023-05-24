using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000C23 RID: 3107
[ExecuteInEditMode]
public class SlicedVolume : MonoBehaviour
{
	// Token: 0x060042EF RID: 17135 RVA: 0x0015ADCC File Offset: 0x00158FCC
	private void OnDrawGizmos()
	{
		this.editorUpdate();
		if (this.generateNewSlices)
		{
			if (this.cloudMaterial)
			{
				this.integrityCheck();
				this.settingValuesUp(false);
				if (this.shadowCaster)
				{
					int num = this.sliceAmount;
					this.sliceAmount = 1;
					this.settingValuesUp(true);
					this.sliceAmount = num;
				}
			}
			this.generateNewSlices = false;
		}
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x0015AE38 File Offset: 0x00159038
	private void Update()
	{
		if (Application.isPlaying && this.generateNewSlices)
		{
			if (this.cloudMaterial)
			{
				this.integrityCheck();
				this.settingValuesUp(false);
				if (this.shadowCaster)
				{
					int num = this.sliceAmount;
					this.sliceAmount = 1;
					this.settingValuesUp(true);
					this.sliceAmount = num;
				}
			}
			this.generateNewSlices = false;
		}
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x0015AEA8 File Offset: 0x001590A8
	private void syncCloudAndShadowCaster()
	{
		this.shadowCasterMat.CopyPropertiesFromMaterial(this.cloudMaterial);
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x0015AEBC File Offset: 0x001590BC
	private void editorUpdate()
	{
		this.sliceAmount = ((this.sliceAmount > 1) ? this.sliceAmount : 1);
		this.segmentCount = ((this.segmentCount <= 2) ? 2 : this.segmentCount);
		if (Camera.current.name != "PreRenderCamera" && this.cloudMaterial && !this.curved && this.meshSlices)
		{
			if (Camera.current.transform.position.y > base.transform.position.y && this.cameraCloudRelation == -1)
			{
				this.cameraCloudRelation = 1;
				this.updateCloudDirection = true;
			}
			else if (Camera.current.transform.position.y < base.transform.position.y && this.cameraCloudRelation == 1)
			{
				this.cameraCloudRelation = -1;
				this.updateCloudDirection = true;
			}
			if (this.updateCloudDirection)
			{
				this.meshSlices.transform.localScale = new Vector3(Mathf.Abs(this.meshSlices.transform.localScale.x), Mathf.Abs(this.meshSlices.transform.localScale.y) * (float)this.cameraCloudRelation, Mathf.Abs(this.meshSlices.transform.localScale.z));
				this.cloudMaterial.SetVector("_CloudNormalsDirection", new Vector4(this.normalDirection.x, this.normalDirection.y * (float)this.cameraCloudRelation, this.normalDirection.z * -1f, 0f));
				this.updateCloudDirection = false;
			}
		}
		else if (this.curved && this.cloudMaterial && this.meshSlices)
		{
			this.meshSlices.transform.localScale = new Vector3(Mathf.Abs(this.meshSlices.transform.localScale.x), Mathf.Abs(this.meshSlices.transform.localScale.y) * -1f, Mathf.Abs(this.meshSlices.transform.localScale.z));
			if (this.meshShadowCaster)
			{
				this.meshShadowCaster.transform.localScale = new Vector3(Mathf.Abs(this.meshSlices.transform.localScale.x), Mathf.Abs(this.meshSlices.transform.localScale.y) * -1f, Mathf.Abs(this.meshSlices.transform.localScale.z));
			}
			this.cloudMaterial.SetVector("_CloudNormalsDirection", new Vector4(this.normalDirection.x, this.normalDirection.y * -1f, this.normalDirection.z * -1f, 0f));
		}
		if (this.transferVariables && this.cloudMaterial && this.shadowCasterMat)
		{
			this.syncCloudAndShadowCaster();
		}
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x0015B254 File Offset: 0x00159454
	private void integrityCheck()
	{
		if (!this.meshSlices)
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.name == "Clouds")
					{
						this.meshSlices = transform.gameObject;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
			if (!this.meshSlices)
			{
				this.meshSlices = new GameObject("Clouds");
				this.meshSlices.transform.parent = base.transform;
				this.meshSlices.transform.localPosition = Vector3.zero;
				this.meshSlices.AddComponent<MeshFilter>();
				this.meshSlices.AddComponent<MeshRenderer>();
				this.meshSlices.GetComponent<Renderer>().material = this.cloudMaterial;
			}
		}
		if (this.shadowCaster && !this.meshShadowCaster)
		{
			IEnumerator enumerator2 = base.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					Transform transform2 = (Transform)obj2;
					if (transform2.name == "Shadow Caster")
					{
						this.meshShadowCaster = transform2.gameObject;
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = enumerator2 as IDisposable) != null)
				{
					disposable2.Dispose();
				}
			}
			if (!this.meshShadowCaster)
			{
				this.meshShadowCaster = new GameObject("Shadow Caster");
				this.meshShadowCaster.transform.parent = base.transform;
				this.meshShadowCaster.transform.localPosition = Vector3.zero;
				this.meshShadowCaster.AddComponent<MeshFilter>();
				this.meshShadowCaster.AddComponent<MeshRenderer>();
				this.meshShadowCaster.GetComponent<Renderer>().material = this.shadowCasterMat;
			}
		}
		if (!this.shadowCaster)
		{
			if (this.meshShadowCaster)
			{
				UnityEngine.Object.DestroyImmediate(this.meshShadowCaster);
			}
			else
			{
				IEnumerator enumerator3 = base.transform.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						Transform transform3 = (Transform)obj3;
						if (transform3.name == "Shadow Caster")
						{
							UnityEngine.Object.DestroyImmediate(transform3.gameObject);
						}
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = enumerator3 as IDisposable) != null)
					{
						disposable3.Dispose();
					}
				}
			}
		}
		if (this.meshShadowCaster != null)
		{
			this.meshShadowCaster.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
		}
		this.meshSlices.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
		if (this.meshShadowCaster != null)
		{
			this.meshShadowCaster.GetComponent<MeshRenderer>().receiveShadows = false;
		}
		this.meshSlices.GetComponent<MeshRenderer>().receiveShadows = false;
	}

	// Token: 0x060042F4 RID: 17140 RVA: 0x0015B56C File Offset: 0x0015976C
	private void settingValuesUp(bool isShadowCaster)
	{
		this.vertices = new Vector3[this.segmentCount * this.segmentCount * this.sliceAmount];
		this.uvMap = new Vector2[this.vertices.Length];
		this.triangleConstructor = new int[(this.segmentCount - 1) * (this.segmentCount - 1) * this.sliceAmount * 2 * 3];
		this.vertexColor = new Color[this.vertices.Length];
		float num = 1f / ((float)this.segmentCount - 1f);
		this.posGainPerVertices = new Vector3(num * this.dimensions.x, 1f / (float)Mathf.Clamp(this.sliceAmount - 1, 1, 999999) * this.dimensions.y, num * this.dimensions.z);
		this.posGainPerUV = num;
		this.trianglesCreation(isShadowCaster);
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x0015B654 File Offset: 0x00159854
	private void trianglesCreation(bool isShadowCaster)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		float num4 = 0f;
		for (int i = 0; i < this.sliceAmount; i++)
		{
			float num5 = -1f + (float)i * (2f / (float)this.sliceAmount);
			float num6;
			if ((float)i < (float)this.sliceAmount * 0.5f)
			{
				num6 = 1f / ((float)this.sliceAmount * 0.5f) * (float)i;
			}
			else
			{
				num6 = 2f - 1f / ((float)this.sliceAmount * 0.5f) * (float)(i + 1);
			}
			if (this.sliceAmount == 1)
			{
				num6 = 1f;
			}
			for (int j = 0; j < this.segmentCount; j++)
			{
				int num7 = this.segmentCount * num;
				for (int k = 0; k < this.segmentCount; k++)
				{
					if (this.curved)
					{
						num4 = Vector3.Distance(new Vector3(this.posGainPerVertices.x * (float)k - this.dimensions.x / 2f, 0f, this.posGainPerVertices.z * (float)j - this.dimensions.z / 2f), Vector3.zero);
					}
					if (this.sliceAmount == 1)
					{
						this.vertices[k + num7] = new Vector3(this.posGainPerVertices.x * (float)k - this.dimensions.x / 2f, Mathf.Pow(num4, this.roundness) * this.intensity, this.posGainPerVertices.z * (float)j - this.dimensions.z / 2f);
					}
					else
					{
						this.vertices[k + num7] = new Vector3(this.posGainPerVertices.x * (float)k - this.dimensions.x / 2f, this.posGainPerVertices.y * (float)i - this.dimensions.y / 2f + Mathf.Pow(num4, this.roundness) * this.intensity, this.posGainPerVertices.z * (float)j - this.dimensions.z / 2f);
					}
					this.uvMap[k + num7] = new Vector2(this.posGainPerUV * (float)k, this.posGainPerUV * (float)j);
					this.vertexColor[k + num7] = new Vector4(num5, num5, num5, num6);
				}
				num++;
				if (j >= 1)
				{
					for (int l = 0; l < this.segmentCount - 1; l++)
					{
						this.triangleConstructor[num3] = l + num2 + i * this.segmentCount;
						this.triangleConstructor[1 + num3] = this.segmentCount + l + num2 + i * this.segmentCount;
						this.triangleConstructor[2 + num3] = 1 + l + num2 + i * this.segmentCount;
						this.triangleConstructor[3 + num3] = this.segmentCount + 1 + l + num2 + i * this.segmentCount;
						this.triangleConstructor[4 + num3] = 1 + l + num2 + i * this.segmentCount;
						this.triangleConstructor[5 + num3] = this.segmentCount + l + num2 + i * this.segmentCount;
						num3 += 6;
					}
					num2 += this.segmentCount;
				}
			}
		}
		if (!isShadowCaster)
		{
			Mesh mesh = new Mesh();
			mesh.Clear();
			mesh.name = "GeoSlices";
			mesh.vertices = this.vertices;
			mesh.triangles = this.triangleConstructor;
			mesh.uv = this.uvMap;
			mesh.colors = this.vertexColor;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			SlicedVolume.calculateMeshTangents(mesh);
			this.meshSlices.GetComponent<MeshFilter>().mesh = mesh;
		}
		else
		{
			Mesh mesh2 = new Mesh();
			mesh2.Clear();
			mesh2.name = "GeoSlices";
			mesh2.vertices = this.vertices;
			mesh2.triangles = this.triangleConstructor;
			mesh2.uv = this.uvMap;
			mesh2.colors = this.vertexColor;
			mesh2.RecalculateNormals();
			mesh2.RecalculateBounds();
			SlicedVolume.calculateMeshTangents(mesh2);
			this.meshShadowCaster.GetComponent<MeshFilter>().mesh = mesh2;
		}
	}

	// Token: 0x060042F6 RID: 17142 RVA: 0x0015BAF4 File Offset: 0x00159CF4
	public static void calculateMeshTangents(Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] array = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		int num = triangles.Length;
		int num2 = array.Length;
		Vector3[] array2 = new Vector3[num2];
		Vector3[] array3 = new Vector3[num2];
		Vector4[] array4 = new Vector4[num2];
		for (long num3 = 0L; num3 < (long)num; num3 += 3L)
		{
			long num4 = (long)triangles[(int)(checked((IntPtr)num3))];
			long num5 = (long)triangles[(int)(checked((IntPtr)(unchecked(num3 + 1L))))];
			long num6 = (long)triangles[(int)(checked((IntPtr)(unchecked(num3 + 2L))))];
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			Vector2 vector4;
			Vector2 vector5;
			Vector2 vector6;
			checked
			{
				vector = array[(int)((IntPtr)num4)];
				vector2 = array[(int)((IntPtr)num5)];
				vector3 = array[(int)((IntPtr)num6)];
				vector4 = uv[(int)((IntPtr)num4)];
				vector5 = uv[(int)((IntPtr)num5)];
				vector6 = uv[(int)((IntPtr)num6)];
			}
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = 1f / (num13 * num16 - num14 * num15);
			Vector3 vector7 = new Vector3((num16 * num7 - num15 * num8) * num17, (num16 * num9 - num15 * num10) * num17, (num16 * num11 - num15 * num12) * num17);
			Vector3 vector8 = new Vector3((num13 * num8 - num14 * num7) * num17, (num13 * num10 - num14 * num9) * num17, (num13 * num12 - num14 * num11) * num17);
			checked
			{
				array2[(int)((IntPtr)num4)] += vector7;
				array2[(int)((IntPtr)num5)] += vector7;
				array2[(int)((IntPtr)num6)] += vector7;
				array3[(int)((IntPtr)num4)] += vector8;
				array3[(int)((IntPtr)num5)] += vector8;
				array3[(int)((IntPtr)num6)] += vector8;
			}
		}
		for (long num18 = 0L; num18 < (long)num2; num18 += 1L)
		{
			checked
			{
				Vector3 vector9 = normals[(int)((IntPtr)num18)];
				Vector3 vector10 = array2[(int)((IntPtr)num18)];
				Vector3.OrthoNormalize(ref vector9, ref vector10);
				array4[(int)((IntPtr)num18)].x = vector10.x;
				array4[(int)((IntPtr)num18)].y = vector10.y;
				array4[(int)((IntPtr)num18)].z = vector10.z;
				array4[(int)((IntPtr)num18)].w = ((Vector3.Dot(Vector3.Cross(vector9, vector10), array3[(int)((IntPtr)num18)]) >= 0f) ? 1f : (-1f));
			}
		}
		mesh.tangents = array4;
	}

	// Token: 0x04003527 RID: 13607
	public Material cloudMaterial;

	// Token: 0x04003528 RID: 13608
	public Material shadowCasterMat;

	// Token: 0x04003529 RID: 13609
	public int sliceAmount = 25;

	// Token: 0x0400352A RID: 13610
	public int segmentCount = 3;

	// Token: 0x0400352B RID: 13611
	public Vector3 dimensions = new Vector3(1000f, 50f, 1000f);

	// Token: 0x0400352C RID: 13612
	public Vector3 normalDirection = new Vector3(1f, 1f, 1f);

	// Token: 0x0400352D RID: 13613
	public bool shadowCaster;

	// Token: 0x0400352E RID: 13614
	public bool transferVariables = true;

	// Token: 0x0400352F RID: 13615
	public bool unityFive;

	// Token: 0x04003530 RID: 13616
	public bool curved;

	// Token: 0x04003531 RID: 13617
	public float roundness = 2f;

	// Token: 0x04003532 RID: 13618
	public float intensity = 0.001f;

	// Token: 0x04003533 RID: 13619
	public bool generateNewSlices;

	// Token: 0x04003534 RID: 13620
	private bool updateCloudDirection = true;

	// Token: 0x04003535 RID: 13621
	private int cameraCloudRelation = 1;

	// Token: 0x04003536 RID: 13622
	private Color[] vertexColor;

	// Token: 0x04003537 RID: 13623
	private Vector3[] vertices;

	// Token: 0x04003538 RID: 13624
	private Vector2[] uvMap;

	// Token: 0x04003539 RID: 13625
	private int[] triangleConstructor;

	// Token: 0x0400353A RID: 13626
	private Vector3 posGainPerVertices;

	// Token: 0x0400353B RID: 13627
	private float posGainPerUV;

	// Token: 0x0400353C RID: 13628
	private GameObject meshSlices;

	// Token: 0x0400353D RID: 13629
	private GameObject meshShadowCaster;
}
