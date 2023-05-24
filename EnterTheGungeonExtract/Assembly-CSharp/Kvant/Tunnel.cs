using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Kvant
{
	// Token: 0x02000834 RID: 2100
	[ExecuteInEditMode]
	[AddComponentMenu("Kvant/Tunnel")]
	public class Tunnel : MonoBehaviour
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002D84 RID: 11652 RVA: 0x000ED9A8 File Offset: 0x000EBBA8
		public int slices
		{
			get
			{
				return this._slices;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002D85 RID: 11653 RVA: 0x000ED9B0 File Offset: 0x000EBBB0
		public int stacks
		{
			get
			{
				return this._totalStacks;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002D86 RID: 11654 RVA: 0x000ED9B8 File Offset: 0x000EBBB8
		// (set) Token: 0x06002D87 RID: 11655 RVA: 0x000ED9C0 File Offset: 0x000EBBC0
		public float radius
		{
			get
			{
				return this._radius;
			}
			set
			{
				this._radius = value;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002D88 RID: 11656 RVA: 0x000ED9CC File Offset: 0x000EBBCC
		// (set) Token: 0x06002D89 RID: 11657 RVA: 0x000ED9D4 File Offset: 0x000EBBD4
		public float height
		{
			get
			{
				return this._height;
			}
			set
			{
				this._height = value;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002D8A RID: 11658 RVA: 0x000ED9E0 File Offset: 0x000EBBE0
		// (set) Token: 0x06002D8B RID: 11659 RVA: 0x000ED9E8 File Offset: 0x000EBBE8
		public float offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002D8C RID: 11660 RVA: 0x000ED9F4 File Offset: 0x000EBBF4
		// (set) Token: 0x06002D8D RID: 11661 RVA: 0x000ED9FC File Offset: 0x000EBBFC
		public int noiseRepeat
		{
			get
			{
				return this._noiseRepeat;
			}
			set
			{
				this._noiseRepeat = value;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002D8E RID: 11662 RVA: 0x000EDA08 File Offset: 0x000EBC08
		// (set) Token: 0x06002D8F RID: 11663 RVA: 0x000EDA10 File Offset: 0x000EBC10
		public float noiseFrequency
		{
			get
			{
				return this._noiseFrequency;
			}
			set
			{
				this._noiseFrequency = value;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002D90 RID: 11664 RVA: 0x000EDA1C File Offset: 0x000EBC1C
		// (set) Token: 0x06002D91 RID: 11665 RVA: 0x000EDA24 File Offset: 0x000EBC24
		public int noiseDepth
		{
			get
			{
				return this._noiseDepth;
			}
			set
			{
				this._noiseDepth = value;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002D92 RID: 11666 RVA: 0x000EDA30 File Offset: 0x000EBC30
		// (set) Token: 0x06002D93 RID: 11667 RVA: 0x000EDA38 File Offset: 0x000EBC38
		public float noiseClampMin
		{
			get
			{
				return this._noiseClampMin;
			}
			set
			{
				this._noiseClampMin = value;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002D94 RID: 11668 RVA: 0x000EDA44 File Offset: 0x000EBC44
		// (set) Token: 0x06002D95 RID: 11669 RVA: 0x000EDA4C File Offset: 0x000EBC4C
		public float noiseClampMax
		{
			get
			{
				return this._noiseClampMax;
			}
			set
			{
				this._noiseClampMax = value;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x000EDA58 File Offset: 0x000EBC58
		// (set) Token: 0x06002D97 RID: 11671 RVA: 0x000EDA60 File Offset: 0x000EBC60
		public float noiseElevation
		{
			get
			{
				return this._noiseElevation;
			}
			set
			{
				this._noiseElevation = value;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002D98 RID: 11672 RVA: 0x000EDA6C File Offset: 0x000EBC6C
		// (set) Token: 0x06002D99 RID: 11673 RVA: 0x000EDA74 File Offset: 0x000EBC74
		public float noiseWarp
		{
			get
			{
				return this._noiseWarp;
			}
			set
			{
				this._noiseWarp = value;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x000EDA80 File Offset: 0x000EBC80
		// (set) Token: 0x06002D9B RID: 11675 RVA: 0x000EDA88 File Offset: 0x000EBC88
		public Material sharedMaterial
		{
			get
			{
				return this._material;
			}
			set
			{
				this._material = value;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002D9C RID: 11676 RVA: 0x000EDA94 File Offset: 0x000EBC94
		// (set) Token: 0x06002D9D RID: 11677 RVA: 0x000EDAC0 File Offset: 0x000EBCC0
		public Material material
		{
			get
			{
				if (!this._owningMaterial)
				{
					this._material = UnityEngine.Object.Instantiate<Material>(this._material);
					this._owningMaterial = true;
				}
				return this._material;
			}
			set
			{
				if (this._owningMaterial)
				{
					UnityEngine.Object.Destroy(this._material, 0.1f);
				}
				this._material = value;
				this._owningMaterial = false;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x000EDAEC File Offset: 0x000EBCEC
		// (set) Token: 0x06002D9F RID: 11679 RVA: 0x000EDAF4 File Offset: 0x000EBCF4
		public ShadowCastingMode shadowCastingMode
		{
			get
			{
				return this._castShadows;
			}
			set
			{
				this._castShadows = value;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x000EDB00 File Offset: 0x000EBD00
		// (set) Token: 0x06002DA1 RID: 11681 RVA: 0x000EDB08 File Offset: 0x000EBD08
		public bool receiveShadows
		{
			get
			{
				return this._receiveShadows;
			}
			set
			{
				this._receiveShadows = value;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002DA2 RID: 11682 RVA: 0x000EDB14 File Offset: 0x000EBD14
		// (set) Token: 0x06002DA3 RID: 11683 RVA: 0x000EDB1C File Offset: 0x000EBD1C
		public Color lineColor
		{
			get
			{
				return this._lineColor;
			}
			set
			{
				this._lineColor = value;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x000EDB28 File Offset: 0x000EBD28
		private float ZOffset
		{
			get
			{
				return Mathf.Repeat(this._offset, this._height / (float)this._totalStacks * 2f);
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002DA5 RID: 11685 RVA: 0x000EDB4C File Offset: 0x000EBD4C
		private float VOffset
		{
			get
			{
				return this.ZOffset - this._offset;
			}
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000EDB5C File Offset: 0x000EBD5C
		private void UpdateColumnAndRowCounts()
		{
			this._slices = Mathf.Clamp(this._slices, 4, 4096);
			this._stacks = Mathf.Clamp(this._stacks, 4, 4096);
			int num = this._slices * (this._stacks + 1) * 6;
			int num2 = num / 65000 + 1;
			this._stacksPerSegment = ((num2 <= 1) ? this._stacks : (this._stacks / num2 / 2 * 2));
			this._totalStacks = this._stacksPerSegment * num2;
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000EDBE8 File Offset: 0x000EBDE8
		public void NotifyConfigChange()
		{
			this._needsReset = true;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000EDBF4 File Offset: 0x000EBDF4
		private Material CreateMaterial(Shader shader)
		{
			return new Material(shader)
			{
				hideFlags = HideFlags.DontSave
			};
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000EDC14 File Offset: 0x000EBE14
		private RenderTexture CreateBuffer()
		{
			int num = this._slices * 2;
			int num2 = this._totalStacks + 1;
			return new RenderTexture(num, num2, 0, RenderTextureFormat.ARGBFloat)
			{
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Repeat
			};
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000EDC58 File Offset: 0x000EBE58
		private void UpdateKernelShader()
		{
			Material kernelMaterial = this._kernelMaterial;
			kernelMaterial.SetVector("_Extent", new Vector2(this._radius, this._height));
			kernelMaterial.SetFloat("_Offset", this.VOffset);
			kernelMaterial.SetVector("_Frequency", new Vector2((float)this._noiseRepeat, this._noiseFrequency));
			kernelMaterial.SetVector("_Amplitude", new Vector3(1f, this._noiseWarp, this._noiseWarp) * this._noiseElevation);
			kernelMaterial.SetVector("_ClampRange", new Vector2(this._noiseClampMin, this._noiseClampMax) * 1.415f);
			if (this._noiseWarp > 0f)
			{
				kernelMaterial.EnableKeyword("ENABLE_WARP");
			}
			else
			{
				kernelMaterial.DisableKeyword("ENABLE_WARP");
			}
			for (int i = 1; i <= 5; i++)
			{
				if (i == this._noiseDepth)
				{
					kernelMaterial.EnableKeyword("DEPTH" + i);
				}
				else
				{
					kernelMaterial.DisableKeyword("DEPTH" + i);
				}
			}
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000EDD98 File Offset: 0x000EBF98
		private void ResetResources()
		{
			this.UpdateColumnAndRowCounts();
			if (this._bulkMesh == null)
			{
				this._bulkMesh = new Tunnel.BulkMesh(this._slices, this._stacksPerSegment, this._totalStacks);
			}
			else
			{
				this._bulkMesh.Rebuild(this._slices, this._stacksPerSegment, this._totalStacks);
			}
			if (this._positionBuffer)
			{
				UnityEngine.Object.DestroyImmediate(this._positionBuffer);
			}
			if (this._normalBuffer1)
			{
				UnityEngine.Object.DestroyImmediate(this._normalBuffer1);
			}
			if (this._normalBuffer2)
			{
				UnityEngine.Object.DestroyImmediate(this._normalBuffer2);
			}
			this._positionBuffer = this.CreateBuffer();
			this._normalBuffer1 = this.CreateBuffer();
			this._normalBuffer2 = this.CreateBuffer();
			if (!this._kernelMaterial)
			{
				this._kernelMaterial = this.CreateMaterial(this._kernelShader);
			}
			if (!this._lineMaterial)
			{
				this._lineMaterial = this.CreateMaterial(this._lineShader);
			}
			if (!this._debugMaterial)
			{
				this._debugMaterial = this.CreateMaterial(this._debugShader);
			}
			this._lineMaterial.SetTexture("_PositionBuffer", this._positionBuffer);
			this._needsReset = false;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000EDEF0 File Offset: 0x000EC0F0
		private void Reset()
		{
			this._needsReset = true;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000EDEFC File Offset: 0x000EC0FC
		private void OnDestroy()
		{
			if (this._bulkMesh != null)
			{
				this._bulkMesh.Release();
			}
			if (this._positionBuffer)
			{
				UnityEngine.Object.DestroyImmediate(this._positionBuffer);
			}
			if (this._normalBuffer1)
			{
				UnityEngine.Object.DestroyImmediate(this._normalBuffer1);
			}
			if (this._normalBuffer2)
			{
				UnityEngine.Object.DestroyImmediate(this._normalBuffer2);
			}
			if (this._kernelMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this._kernelMaterial);
			}
			if (this._lineMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this._lineMaterial);
			}
			if (this._debugMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this._debugMaterial);
			}
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000EDFC4 File Offset: 0x000EC1C4
		private void LateUpdate()
		{
			if (this._needsReset)
			{
				this.ResetResources();
			}
			this.UpdateKernelShader();
			Graphics.Blit(null, this._positionBuffer, this._kernelMaterial, 0);
			Graphics.Blit(this._positionBuffer, this._normalBuffer1, this._kernelMaterial, 1);
			Graphics.Blit(this._positionBuffer, this._normalBuffer2, this._kernelMaterial, 2);
			this._lineMaterial.SetColor("_Color", this._lineColor);
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			MaterialPropertyBlock materialPropertyBlock2 = new MaterialPropertyBlock();
			materialPropertyBlock.SetTexture("_PositionBuffer", this._positionBuffer);
			materialPropertyBlock2.SetTexture("_PositionBuffer", this._positionBuffer);
			materialPropertyBlock.SetTexture("_NormalBuffer", this._normalBuffer1);
			materialPropertyBlock2.SetTexture("_NormalBuffer", this._normalBuffer2);
			Vector3 vector = new Vector3(0f, 0f, this.VOffset);
			materialPropertyBlock.SetVector("_MapOffset", vector);
			materialPropertyBlock2.SetVector("_MapOffset", vector);
			materialPropertyBlock.SetFloat("_UseBuffer", 1f);
			materialPropertyBlock2.SetFloat("_UseBuffer", 1f);
			Mesh mesh = this._bulkMesh.mesh;
			Vector3 vector2 = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			Vector2 vector3 = new Vector2(0.5f / (float)this._positionBuffer.width, 0f);
			vector2 += base.transform.forward * this.ZOffset;
			for (int i = 0; i < this._totalStacks; i += this._stacksPerSegment)
			{
				vector3.y = (0.5f + (float)i) / (float)this._positionBuffer.height;
				materialPropertyBlock.SetVector("_BufferOffset", vector3);
				materialPropertyBlock2.SetVector("_BufferOffset", vector3);
				if (this._material)
				{
					Graphics.DrawMesh(mesh, vector2, rotation, this._material, 0, null, 0, materialPropertyBlock, this._castShadows, this._receiveShadows);
					Graphics.DrawMesh(mesh, vector2, rotation, this._material, 0, null, 1, materialPropertyBlock2, this._castShadows, this._receiveShadows);
				}
				if (this._lineColor.a > 0f)
				{
					Graphics.DrawMesh(mesh, vector2, rotation, this._lineMaterial, 0, null, 2, materialPropertyBlock, false, false);
				}
			}
		}

		// Token: 0x04001EDA RID: 7898
		[SerializeField]
		private int _slices = 40;

		// Token: 0x04001EDB RID: 7899
		[SerializeField]
		private int _stacks = 100;

		// Token: 0x04001EDC RID: 7900
		[SerializeField]
		private float _radius = 5f;

		// Token: 0x04001EDD RID: 7901
		[SerializeField]
		private float _height = 50f;

		// Token: 0x04001EDE RID: 7902
		[SerializeField]
		private float _offset;

		// Token: 0x04001EDF RID: 7903
		[SerializeField]
		private int _noiseRepeat = 2;

		// Token: 0x04001EE0 RID: 7904
		[SerializeField]
		private float _noiseFrequency = 0.05f;

		// Token: 0x04001EE1 RID: 7905
		[SerializeField]
		[Range(1f, 5f)]
		private int _noiseDepth = 3;

		// Token: 0x04001EE2 RID: 7906
		[SerializeField]
		private float _noiseClampMin = -1f;

		// Token: 0x04001EE3 RID: 7907
		[SerializeField]
		private float _noiseClampMax = 1f;

		// Token: 0x04001EE4 RID: 7908
		[SerializeField]
		private float _noiseElevation = 1f;

		// Token: 0x04001EE5 RID: 7909
		[SerializeField]
		[Range(0f, 1f)]
		private float _noiseWarp;

		// Token: 0x04001EE6 RID: 7910
		[SerializeField]
		private Material _material;

		// Token: 0x04001EE7 RID: 7911
		private bool _owningMaterial;

		// Token: 0x04001EE8 RID: 7912
		[SerializeField]
		private ShadowCastingMode _castShadows;

		// Token: 0x04001EE9 RID: 7913
		[SerializeField]
		private bool _receiveShadows;

		// Token: 0x04001EEA RID: 7914
		[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
		[SerializeField]
		private Color _lineColor = new Color(0f, 0f, 0f, 0.4f);

		// Token: 0x04001EEB RID: 7915
		[SerializeField]
		private Shader _kernelShader;

		// Token: 0x04001EEC RID: 7916
		[SerializeField]
		private Shader _lineShader;

		// Token: 0x04001EED RID: 7917
		[SerializeField]
		private Shader _debugShader;

		// Token: 0x04001EEE RID: 7918
		private int _stacksPerSegment;

		// Token: 0x04001EEF RID: 7919
		private int _totalStacks;

		// Token: 0x04001EF0 RID: 7920
		private RenderTexture _positionBuffer;

		// Token: 0x04001EF1 RID: 7921
		private RenderTexture _normalBuffer1;

		// Token: 0x04001EF2 RID: 7922
		private RenderTexture _normalBuffer2;

		// Token: 0x04001EF3 RID: 7923
		private Tunnel.BulkMesh _bulkMesh;

		// Token: 0x04001EF4 RID: 7924
		private Material _kernelMaterial;

		// Token: 0x04001EF5 RID: 7925
		private Material _lineMaterial;

		// Token: 0x04001EF6 RID: 7926
		private Material _debugMaterial;

		// Token: 0x04001EF7 RID: 7927
		private bool _needsReset = true;

		// Token: 0x02000835 RID: 2101
		[Serializable]
		private class BulkMesh
		{
			// Token: 0x06002DAF RID: 11695 RVA: 0x000EE22C File Offset: 0x000EC42C
			public BulkMesh(int columns, int rowsPerSegment, int totalRows)
			{
				this._mesh = this.BuildMesh(columns, rowsPerSegment, totalRows);
			}

			// Token: 0x17000875 RID: 2165
			// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x000EE244 File Offset: 0x000EC444
			public Mesh mesh
			{
				get
				{
					return this._mesh;
				}
			}

			// Token: 0x06002DB1 RID: 11697 RVA: 0x000EE24C File Offset: 0x000EC44C
			public void Rebuild(int columns, int rowsPerSegment, int totalRows)
			{
				this.Release();
				this._mesh = this.BuildMesh(columns, rowsPerSegment, totalRows);
			}

			// Token: 0x06002DB2 RID: 11698 RVA: 0x000EE264 File Offset: 0x000EC464
			public void Release()
			{
				if (this._mesh != null)
				{
					UnityEngine.Object.DestroyImmediate(this._mesh);
					this._mesh = null;
				}
			}

			// Token: 0x06002DB3 RID: 11699 RVA: 0x000EE28C File Offset: 0x000EC48C
			private Mesh BuildMesh(int columns, int rows, int totalRows)
			{
				int num = rows + 1;
				float num2 = 0.5f / (float)columns;
				float num3 = 1f / (float)(totalRows + 1);
				float num4 = 0f;
				Vector2[] array = new Vector2[columns * (num - 1) * 6];
				Vector2[] array2 = new Vector2[columns * (num - 1) * 6];
				int num5 = 0;
				for (int i = 0; i < num - 1; i++)
				{
					int j = 0;
					while (j < columns)
					{
						int num6 = j * 2 + (i & 1);
						array[num5] = new Vector2(num2 * (float)num6, num4 + num3 * (float)i);
						array[num5 + 1] = new Vector2(num2 * (float)(num6 + 1), num4 + num3 * (float)(i + 1));
						array[num5 + 2] = new Vector2(num2 * (float)(num6 + 2), num4 + num3 * (float)i);
						array2[num5] = (array2[num5 + 1] = (array2[num5 + 2] = array[num5]));
						j++;
						num5 += 3;
					}
				}
				for (int k = 0; k < num - 1; k++)
				{
					int l = 0;
					while (l < columns)
					{
						int num7 = l * 2 + 2 - (k & 1);
						array[num5] = new Vector2(num2 * (float)num7, num4 + num3 * (float)k);
						array[num5 + 1] = new Vector2(num2 * (float)(num7 - 1), num4 + num3 * (float)(k + 1));
						array[num5 + 2] = new Vector2(num2 * (float)(num7 + 1), num4 + num3 * (float)(k + 1));
						array2[num5] = (array2[num5 + 1] = (array2[num5 + 2] = array[num5]));
						l++;
						num5 += 3;
					}
				}
				int[] array3 = new int[columns * (num - 1) * 3];
				int[] array4 = new int[columns * (num - 1) * 3];
				for (int m = 0; m < array3.Length; m++)
				{
					array3[m] = m;
					array4[m] = m + array3.Length;
				}
				int[] array5 = new int[columns * (num - 1) * 6];
				int num8 = 0;
				int num9 = 0;
				for (int n = 0; n < num - 1; n++)
				{
					int num10 = 0;
					while (num10 < columns)
					{
						array5[num8] = num9;
						array5[num8 + 1] = num9 + 1;
						array5[num8 + 2] = num9;
						array5[num8 + 3] = num9 + 2;
						array5[num8 + 4] = num9 + 1;
						array5[num8 + 5] = num9 + 2;
						num10++;
						num8 += 6;
						num9 += 3;
					}
				}
				Mesh mesh = new Mesh();
				mesh.subMeshCount = 3;
				mesh.vertices = new Vector3[array.Length];
				mesh.uv = array;
				mesh.uv2 = array2;
				mesh.SetIndices(array3, MeshTopology.Triangles, 0);
				mesh.SetIndices(array4, MeshTopology.Triangles, 1);
				mesh.SetIndices(array5, MeshTopology.Lines, 2);
				mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100f);
				mesh.hideFlags = HideFlags.DontSave;
				return mesh;
			}

			// Token: 0x04001EF8 RID: 7928
			private Mesh _mesh;
		}
	}
}
