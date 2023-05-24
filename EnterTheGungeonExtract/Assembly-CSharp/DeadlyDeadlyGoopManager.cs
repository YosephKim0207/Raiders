using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200113B RID: 4411
public class DeadlyDeadlyGoopManager : MonoBehaviour
{
	// Token: 0x06006178 RID: 24952 RVA: 0x00258C98 File Offset: 0x00256E98
	public static void ClearPerLevelData()
	{
		StaticReferenceManager.AllGoops.Clear();
		DeadlyDeadlyGoopManager.allGoopPositionMap.Clear();
		DeadlyDeadlyGoopManager.m_goopExceptions.Clear();
	}

	// Token: 0x06006179 RID: 24953 RVA: 0x00258CB8 File Offset: 0x00256EB8
	public static void ReinitializeData()
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = StaticReferenceManager.AllGoops[i];
			deadlyDeadlyGoopManager.ReinitializeArrays();
		}
	}

	// Token: 0x0600617A RID: 24954 RVA: 0x00258CF4 File Offset: 0x00256EF4
	public static void ForceClearGoopsInCell(IntVector2 cellPos)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = StaticReferenceManager.AllGoops[i];
			IntVector2 intVector = (cellPos.ToVector2() / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Round);
			int num = intVector.x;
			while ((float)num < (float)intVector.x + 1f / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE)
			{
				int num2 = intVector.y;
				while ((float)num2 < (float)intVector.y + 1f / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE)
				{
					deadlyDeadlyGoopManager.RemoveGoopedPosition(new IntVector2(num, num2));
					num2++;
				}
				num++;
			}
		}
	}

	// Token: 0x0600617B RID: 24955 RVA: 0x00258DAC File Offset: 0x00256FAC
	public static int CountGoopsInRadius(Vector2 centerPosition, float radius)
	{
		int num = 0;
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = StaticReferenceManager.AllGoops[i];
			num += deadlyDeadlyGoopManager.CountGoopCircle(centerPosition, radius);
		}
		return num;
	}

	// Token: 0x0600617C RID: 24956 RVA: 0x00258DF0 File Offset: 0x00256FF0
	public static void DelayedClearGoopsInRadius(Vector2 centerPosition, float radius)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = StaticReferenceManager.AllGoops[i];
			deadlyDeadlyGoopManager.RemoveGoopCircle(centerPosition, radius);
		}
	}

	// Token: 0x0600617D RID: 24957 RVA: 0x00258E2C File Offset: 0x0025702C
	public static void FreezeGoopsCircle(Vector2 position, float radius)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			StaticReferenceManager.AllGoops[i].FreezeGoopCircle(position, radius);
		}
	}

	// Token: 0x0600617E RID: 24958 RVA: 0x00258E68 File Offset: 0x00257068
	public static void IgniteGoopsCircle(Vector2 position, float radius)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			StaticReferenceManager.AllGoops[i].IgniteGoopCircle(position, radius);
		}
		for (int j = 0; j < StaticReferenceManager.AllGrasses.Count; j++)
		{
			StaticReferenceManager.AllGrasses[j].IgniteCircle(position, radius);
		}
	}

	// Token: 0x0600617F RID: 24959 RVA: 0x00258ED0 File Offset: 0x002570D0
	public static void IgniteGoopsLine(Vector2 p1, Vector2 p2, float radius)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			StaticReferenceManager.AllGoops[i].IgniteGoopLine(p1, p2, radius);
		}
	}

	// Token: 0x06006180 RID: 24960 RVA: 0x00258F0C File Offset: 0x0025710C
	public void IgniteGoopLine(Vector2 p1, Vector2 p2, float radius)
	{
		float num = 0f;
		float num2 = Vector2.Distance(p2, p1);
		while (num < num2 + radius)
		{
			this.IgniteGoopCircle(p1 + (p2 - p1).normalized * num, radius);
			num += radius;
		}
	}

	// Token: 0x06006181 RID: 24961 RVA: 0x00258F5C File Offset: 0x0025715C
	public static void ElectrifyGoopsLine(Vector2 p1, Vector2 p2, float radius)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			StaticReferenceManager.AllGoops[i].ElectrifyGoopLine(p1, p2, radius);
		}
	}

	// Token: 0x06006182 RID: 24962 RVA: 0x00258F98 File Offset: 0x00257198
	public static void FreezeGoopsLine(Vector2 p1, Vector2 p2, float radius)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			StaticReferenceManager.AllGoops[i].FreezeGoopLine(p1, p2, radius);
		}
	}

	// Token: 0x06006183 RID: 24963 RVA: 0x00258FD4 File Offset: 0x002571D4
	public void ElectrifyGoopLine(Vector2 p1, Vector2 p2, float radius)
	{
		float num = 0f;
		float num2 = Vector2.Distance(p2, p1);
		while (num < num2 + radius)
		{
			this.ElectrifyGoopCircle(p1 + (p2 - p1).normalized * num, radius);
			num += radius;
		}
	}

	// Token: 0x06006184 RID: 24964 RVA: 0x00259024 File Offset: 0x00257224
	public void FreezeGoopLine(Vector2 p1, Vector2 p2, float radius)
	{
		float num = 0f;
		float num2 = Vector2.Distance(p2, p1);
		while (num < num2 + radius)
		{
			this.FreezeGoopCircle(p1 + (p2 - p1).normalized * num, radius);
			num += radius;
		}
	}

	// Token: 0x06006185 RID: 24965 RVA: 0x00259074 File Offset: 0x00257274
	public static DeadlyDeadlyGoopManager GetGoopManagerForGoopType(GoopDefinition goopDef)
	{
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			if (StaticReferenceManager.AllGoops[i] && StaticReferenceManager.AllGoops[i].goopDefinition == goopDef)
			{
				return StaticReferenceManager.AllGoops[i];
			}
		}
		GameObject gameObject = new GameObject("goop_" + goopDef.name);
		DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = gameObject.AddComponent<DeadlyDeadlyGoopManager>();
		deadlyDeadlyGoopManager.SetTexture(goopDef.goopTexture, goopDef.worldTexture);
		deadlyDeadlyGoopManager.goopDefinition = goopDef;
		deadlyDeadlyGoopManager.InitialzeUV2IfNecessary();
		StaticReferenceManager.AllGoops.Add(deadlyDeadlyGoopManager);
		deadlyDeadlyGoopManager.InitializeParticleSystems();
		return deadlyDeadlyGoopManager;
	}

	// Token: 0x06006186 RID: 24966 RVA: 0x00259128 File Offset: 0x00257328
	public static int RegisterUngoopableCircle(Vector2 center, float radius)
	{
		float num = radius * radius;
		DeadlyDeadlyGoopManager.m_goopExceptions.Add(Tuple.Create<Vector2, float>(center, num));
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			if (StaticReferenceManager.AllGoops[i])
			{
				StaticReferenceManager.AllGoops[i].RemoveGoopCircle(center, radius);
			}
		}
		return DeadlyDeadlyGoopManager.m_goopExceptions.Count - 1;
	}

	// Token: 0x06006187 RID: 24967 RVA: 0x00259198 File Offset: 0x00257398
	public static void UpdateUngoopableCircle(int id, Vector2 center, float radius)
	{
		if (id < 0 || id >= DeadlyDeadlyGoopManager.m_goopExceptions.Count)
		{
			return;
		}
		DeadlyDeadlyGoopManager.m_goopExceptions[id].First = center;
		DeadlyDeadlyGoopManager.m_goopExceptions[id].Second = radius * radius;
		for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
		{
			if (StaticReferenceManager.AllGoops[i])
			{
				StaticReferenceManager.AllGoops[i].RemoveGoopCircle(center, radius);
			}
		}
	}

	// Token: 0x06006188 RID: 24968 RVA: 0x00259224 File Offset: 0x00257424
	public static void DeregisterUngoopableCircle(int id)
	{
		if (DeadlyDeadlyGoopManager.m_goopExceptions != null && id < DeadlyDeadlyGoopManager.m_goopExceptions.Count && id >= 0)
		{
			DeadlyDeadlyGoopManager.m_goopExceptions[id] = null;
		}
	}

	// Token: 0x06006189 RID: 24969 RVA: 0x00259254 File Offset: 0x00257454
	private static void InitializePropertyIDs()
	{
		if (DeadlyDeadlyGoopManager.TintColorPropertyID == -1)
		{
			DeadlyDeadlyGoopManager.TintColorPropertyID = Shader.PropertyToID("_TintColor");
			DeadlyDeadlyGoopManager.MainTexPropertyID = Shader.PropertyToID("_MainTex");
			DeadlyDeadlyGoopManager.WorldTexPropertyID = Shader.PropertyToID("_WorldTex");
			DeadlyDeadlyGoopManager.OpaquenessMultiplyPropertyID = Shader.PropertyToID("_OpaquenessMultiply");
			DeadlyDeadlyGoopManager.BrightnessMultiplyPropertyID = Shader.PropertyToID("_BrightnessMultiply");
		}
	}

	// Token: 0x0600618A RID: 24970 RVA: 0x002592B8 File Offset: 0x002574B8
	public void SetTexture(Texture2D goopTexture, Texture2D worldTexture)
	{
		this.m_texture = goopTexture;
		this.m_worldTexture = worldTexture;
		for (int i = 0; i < this.m_mrs.GetLength(0); i++)
		{
			for (int j = 0; j < this.m_mrs.GetLength(1); j++)
			{
				if (this.m_mrs[i, j] != null && this.m_mrs[i, j])
				{
					this.m_mrs[i, j].material.SetTexture(DeadlyDeadlyGoopManager.MainTexPropertyID, goopTexture);
					this.m_mrs[i, j].material.SetTexture(DeadlyDeadlyGoopManager.WorldTexPropertyID, worldTexture);
				}
			}
		}
	}

	// Token: 0x0600618B RID: 24971 RVA: 0x00259378 File Offset: 0x00257578
	public void Awake()
	{
		DeadlyDeadlyGoopManager.InitializePropertyIDs();
		this.ConstructUVMap();
		int num = Mathf.RoundToInt(4f * ((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE) * ((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE));
		this.m_vertexArray = new Vector3[num];
		this.m_uvArray = new Vector2[num];
		this.m_uv2Array = new Vector2[num];
		for (int i = 0; i < num; i++)
		{
			this.m_uv2Array[i] = Vector2.zero;
		}
		this.m_colorArray = new Color32[num];
		this.m_triangleArray = new int[num / 4 * 6];
		int num2 = Mathf.CeilToInt((float)GameManager.Instance.Dungeon.Width / (float)this.CHUNK_SIZE);
		int num3 = Mathf.CeilToInt((float)GameManager.Instance.Dungeon.Height / (float)this.CHUNK_SIZE);
		this.m_mrs = new MeshRenderer[num2, num3];
		this.m_meshes = new Mesh[num2, num3];
		this.m_dirtyFlags = new bool[num2, num3];
		this.m_colorDirtyFlags = new bool[num2, num3];
		this.m_shader = ShaderCache.Acquire("Brave/GoopShader");
	}

	// Token: 0x0600618C RID: 24972 RVA: 0x002594A0 File Offset: 0x002576A0
	public void ReinitializeArrays()
	{
		int num = Mathf.CeilToInt((float)GameManager.Instance.Dungeon.Width / (float)this.CHUNK_SIZE);
		int num2 = Mathf.CeilToInt((float)GameManager.Instance.Dungeon.Height / (float)this.CHUNK_SIZE);
		this.m_mrs = BraveUtility.MultidimensionalArrayResize<MeshRenderer>(this.m_mrs, num, num2);
		this.m_meshes = BraveUtility.MultidimensionalArrayResize<Mesh>(this.m_meshes, num, num2);
		this.m_dirtyFlags = BraveUtility.MultidimensionalArrayResize<bool>(this.m_dirtyFlags, num, num2);
		this.m_colorDirtyFlags = BraveUtility.MultidimensionalArrayResize<bool>(this.m_colorDirtyFlags, num, num2);
	}

	// Token: 0x0600618D RID: 24973 RVA: 0x00259538 File Offset: 0x00257738
	private Mesh GetChunkMesh(int chunkX, int chunkY)
	{
		if (this.m_meshes[chunkX, chunkY] != null)
		{
			return this.m_meshes[chunkX, chunkY];
		}
		GameObject gameObject = new GameObject(string.Format("goop_{0}_chunk_{1}_{2}", this.goopDefinition.name, chunkX, chunkY));
		gameObject.transform.position = new Vector3((float)(chunkX * this.CHUNK_SIZE), (float)(chunkY * this.CHUNK_SIZE), (float)(chunkY * this.CHUNK_SIZE) + this.goopDepth);
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();
		mesh.MarkDynamic();
		meshFilter.mesh = mesh;
		Material material = new Material(this.m_shader);
		if (this.m_texture != null)
		{
			material.SetTexture(DeadlyDeadlyGoopManager.MainTexPropertyID, this.m_texture);
			material.SetTexture(DeadlyDeadlyGoopManager.WorldTexPropertyID, this.m_worldTexture);
		}
		if (this.goopDefinition.isOily)
		{
			material.SetFloat("_OilGoop", 1f);
		}
		if (this.goopDefinition.usesOverrideOpaqueness)
		{
			material.SetFloat(DeadlyDeadlyGoopManager.OpaquenessMultiplyPropertyID, this.goopDefinition.overrideOpaqueness);
		}
		meshRenderer.material = material;
		this.m_mrs[chunkX, chunkY] = meshRenderer;
		this.m_meshes[chunkX, chunkY] = mesh;
		int num = chunkX * Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = num + Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = chunkY * Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = num3 + Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector;
				intVector.x = i;
				intVector.y = j;
				this.InitMesh(intVector, chunkX, chunkY);
			}
		}
		mesh.vertices = this.m_vertexArray;
		mesh.triangles = this.m_triangleArray;
		return mesh;
	}

	// Token: 0x0600618E RID: 24974 RVA: 0x00259754 File Offset: 0x00257954
	private void ConstructUVMap()
	{
		this.m_uvMap = new Dictionary<int, Vector2>();
		this.m_uvMap.Add(62, new Vector2(0f, 0f));
		this.m_uvMap.Add(191, new Vector2(0.375f, 0f));
		this.m_uvMap.Add(254, new Vector2(0.375f, 0f));
		this.m_uvMap.Add(124, new Vector2(0.5f, 0f));
		this.m_uvMap.Add(31, new Vector2(0.625f, 0f));
		this.m_uvMap.Add(241, new Vector2(0.75f, 0f));
		this.m_uvMap.Add(199, new Vector2(0.875f, 0f));
		this.m_uvMap.Add(14, new Vector2(0f, 0.125f));
		this.m_uvMap.Add(143, new Vector2(0.125f, 0.125f));
		this.m_uvMap.Add(238, new Vector2(0.25f, 0.125f));
		this.m_uvMap.Add(239, new Vector2(0.375f, 0f));
		this.m_uvMap.Add(221, new Vector2(0.5f, 0.125f));
		this.m_uvMap.Add(119, new Vector2(0.625f, 0.125f));
		this.m_uvMap.Add(56, new Vector2(0f, 0.25f));
		this.m_uvMap.Add(187, new Vector2(0.125f, 0.25f));
		this.m_uvMap.Add(248, new Vector2(0.25f, 0.25f));
		this.m_uvMap.Add(251, new Vector2(0.375f, 0f));
		this.m_uvMap.Add(60, new Vector2(0.5f, 0.25f));
		this.m_uvMap.Add(30, new Vector2(0.625f, 0.25f));
		this.m_uvMap.Add(225, new Vector2(0.75f, 0.25f));
		this.m_uvMap.Add(195, new Vector2(0.875f, 0.25f));
		this.m_uvMap.Add(0, new Vector2(0f, 0.375f));
		this.m_uvMap.Add(131, new Vector2(0.125f, 0.375f));
		this.m_uvMap.Add(224, new Vector2(0.25f, 0.375f));
		this.m_uvMap.Add(227, new Vector2(0.375f, 0.375f));
		this.m_uvMap.Add(126, new Vector2(0.5f, 0.375f));
		this.m_uvMap.Add(63, new Vector2(0.625f, 0.375f));
		this.m_uvMap.Add(243, new Vector2(0.75f, 0.375f));
		this.m_uvMap.Add(231, new Vector2(0.875f, 0.375f));
		this.m_uvMap.Add(253, new Vector2(0f, 0.5f));
		this.m_uvMap.Add(223, new Vector2(0.125f, 0.5f));
		this.m_uvMap.Add(127, new Vector2(0.25f, 0.5f));
		this.m_uvMap.Add(247, new Vector2(0.375f, 0.5f));
		this.m_uvMap.Add(249, new Vector2(0.5f, 0.5f));
		this.m_uvMap.Add(207, new Vector2(0.625f, 0.5f));
		this.m_uvMap.Add(252, new Vector2(0.75f, 0.5f));
		this.m_uvMap.Add(159, new Vector2(0.875f, 0.5f));
		this.m_uvMap.Add(68, new Vector2(0.25f, 0.125f));
		this.m_uvMap.Add(102, new Vector2(0.25f, 0.125f));
		this.m_uvMap.Add(204, new Vector2(0.25f, 0.125f));
		this.m_uvMap.Add(17, new Vector2(0.25f, 0.125f));
		this.m_uvMap.Add(51, new Vector2(0.25f, 0.125f));
		this.m_uvMap.Add(153, new Vector2(0.25f, 0.125f));
		this.m_uvMap.Add(16, new Vector2(0f, 0.625f));
		this.m_uvMap.Add(4, new Vector2(0.125f, 0.625f));
		this.m_uvMap.Add(64, new Vector2(0.25f, 0.625f));
		this.m_uvMap.Add(1, new Vector2(0.375f, 0.625f));
		this.m_uvMap.Add(240, new Vector2(0.5f, 0.625f));
		this.m_uvMap.Add(135, new Vector2(0.625f, 0.625f));
		this.m_uvMap.Add(120, new Vector2(0.75f, 0.625f));
		this.m_uvMap.Add(15, new Vector2(0.875f, 0.625f));
		this.m_uvMap.Add(-1, new Vector2(0f, 0.375f));
		this.m_centerUVOptions.Add(new Vector2(0.375f, 0f));
		this.m_centerUVOptions.Add(new Vector2(0.375f, 0f));
		this.m_centerUVOptions.Add(new Vector2(0.375f, 0f));
		this.m_centerUVOptions.Add(new Vector2(0.375f, 0f));
		this.m_centerUVOptions.Add(new Vector2(0.375f, 0f));
		this.m_centerUVOptions.Add(new Vector2(0f, 0.875f));
		this.m_centerUVOptions.Add(new Vector2(0.125f, 0.875f));
		this.m_centerUVOptions.Add(new Vector2(0.25f, 0.875f));
		this.m_centerUVOptions.Add(new Vector2(0.375f, 0.875f));
	}

	// Token: 0x0600618F RID: 24975 RVA: 0x00259E5C File Offset: 0x0025805C
	public void ProcessProjectile(Projectile p)
	{
		for (int i = 0; i < this.goopDefinition.goopDamageTypeInteractions.Count; i++)
		{
			GoopDefinition.GoopDamageTypeInteraction goopDamageTypeInteraction = this.goopDefinition.goopDamageTypeInteractions[i];
			bool flag = goopDamageTypeInteraction.damageType == CoreDamageTypes.Ice && p.AppliesFreeze;
			if (((p.damageTypes & goopDamageTypeInteraction.damageType) == goopDamageTypeInteraction.damageType || flag) && this.IsPositionInGoop(p.specRigidbody.UnitCenter))
			{
				if (goopDamageTypeInteraction.ignitionMode == GoopDefinition.GoopDamageTypeInteraction.GoopIgnitionMode.IGNITE)
				{
					this.IgniteGoopCircle(p.specRigidbody.UnitCenter, 1f);
				}
				else if (goopDamageTypeInteraction.ignitionMode == GoopDefinition.GoopDamageTypeInteraction.GoopIgnitionMode.DOUSE)
				{
				}
				if (goopDamageTypeInteraction.electrifiesGoop)
				{
					IntVector2 intVector = (p.specRigidbody.UnitCenter / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
					AkSoundEngine.PostEvent("Play_ENV_puddle_zap_01", GameManager.Instance.PrimaryPlayer.gameObject);
					GameManager.Instance.Dungeon.StartCoroutine(this.HandleRecursiveElectrification(intVector));
				}
				if (goopDamageTypeInteraction.freezesGoop)
				{
					this.FreezeGoopCircle(p.specRigidbody.UnitCenter, 1f);
				}
			}
		}
	}

	// Token: 0x06006190 RID: 24976 RVA: 0x00259F94 File Offset: 0x00258194
	private void ElectrifyCell(IntVector2 cellIndex)
	{
		if (!this.goopDefinition.CanBeElectrified)
		{
			return;
		}
		if (!this.m_goopedCells.ContainsKey(cellIndex) || this.m_goopedCells[cellIndex] == null)
		{
			return;
		}
		if (this.m_goopedCells[cellIndex].IsFrozen)
		{
			return;
		}
		if (this.m_goopedCells[cellIndex].remainingLifespan < this.goopDefinition.fadePeriod)
		{
			return;
		}
		if (!this.m_goopedCells[cellIndex].IsElectrified)
		{
			this.m_goopedCells[cellIndex].IsElectrified = true;
			this.m_goopedCells[cellIndex].remainingElecTimer = 0f;
		}
		this.m_goopedCells[cellIndex].remainingElectrifiedTime = this.goopDefinition.electrifiedTime;
	}

	// Token: 0x06006191 RID: 24977 RVA: 0x0025A068 File Offset: 0x00258268
	private IEnumerator HandleRecursiveElectrification(IntVector2 cellIndex)
	{
		if (!this.goopDefinition.CanBeElectrified)
		{
			yield break;
		}
		if (this.m_goopedCells[cellIndex].IsFrozen)
		{
			yield break;
		}
		if (this.m_goopedCells[cellIndex].remainingLifespan < this.goopDefinition.fadePeriod)
		{
			yield break;
		}
		Queue<IntVector2> m_positionsToElectrify = new Queue<IntVector2>();
		m_positionsToElectrify.Enqueue(cellIndex);
		this.m_lastElecSemaphore += 1U;
		uint currentSemaphore = this.m_lastElecSemaphore;
		this.m_goopedCells[cellIndex].elecTriggerSemaphore = currentSemaphore;
		int enumeratorCounter = 0;
		while (m_positionsToElectrify.Count > 0)
		{
			IntVector2 currentPos = m_positionsToElectrify.Dequeue();
			if (this.m_goopedCells.ContainsKey(currentPos) && this.m_goopedCells[currentPos] != null)
			{
				this.ElectrifyCell(currentPos);
				for (int i = 0; i < 8; i++)
				{
					DeadlyDeadlyGoopManager.GoopPositionData goopPositionData = this.m_goopedCells[currentPos].neighborGoopData[i];
					if (goopPositionData != null && goopPositionData.elecTriggerSemaphore < currentSemaphore && (!goopPositionData.IsElectrified || goopPositionData.remainingElectrifiedTime < this.goopDefinition.electrifiedTime - 0.01f))
					{
						goopPositionData.elecTriggerSemaphore = currentSemaphore;
						m_positionsToElectrify.Enqueue(goopPositionData.goopPosition);
					}
				}
				enumeratorCounter++;
				if (enumeratorCounter > 200)
				{
					yield return null;
					enumeratorCounter = 0;
				}
			}
		}
		yield break;
	}

	// Token: 0x06006192 RID: 24978 RVA: 0x0025A08C File Offset: 0x0025828C
	private void FreezeCell(IntVector2 cellIndex)
	{
		if (!this.goopDefinition.CanBeFrozen)
		{
			return;
		}
		DeadlyDeadlyGoopManager.GoopPositionData goopPositionData = this.m_goopedCells[cellIndex];
		goopPositionData.IsFrozen = true;
		goopPositionData.remainingFreezeTimer = this.goopDefinition.freezeLifespan;
	}

	// Token: 0x06006193 RID: 24979 RVA: 0x0025A0D0 File Offset: 0x002582D0
	public void ElectrifyGoopCircle(Vector2 center, float radius)
	{
		if (!this.goopDefinition.CanBeElectrified)
		{
			return;
		}
		int num = Mathf.CeilToInt((center.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.FloorToInt((center.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.CeilToInt((center.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.FloorToInt((center.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		Vector2 vector = center / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num5 = radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		for (int i = num; i < num2; i++)
		{
			bool flag = false;
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				if (this.m_goopedCells.ContainsKey(intVector) && Vector2.Distance(vector, intVector.ToVector2()) <= num5)
				{
					flag = true;
					GameManager.Instance.Dungeon.StartCoroutine(this.HandleRecursiveElectrification(intVector));
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
	}

	// Token: 0x06006194 RID: 24980 RVA: 0x0025A1E8 File Offset: 0x002583E8
	public void FreezeGoopCircle(Vector2 center, float radius)
	{
		if (!this.goopDefinition.CanBeFrozen)
		{
			return;
		}
		int num = Mathf.CeilToInt((center.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.FloorToInt((center.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.CeilToInt((center.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.FloorToInt((center.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		Vector2 vector = center / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num5 = radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				if (this.m_goopedCells.ContainsKey(intVector) && Vector2.Distance(vector, intVector.ToVector2()) <= num5)
				{
					this.FreezeCell(intVector);
				}
			}
		}
	}

	// Token: 0x06006195 RID: 24981 RVA: 0x0025A2D8 File Offset: 0x002584D8
	private void IgniteCell(IntVector2 cellIndex)
	{
		if (!this.goopDefinition.CanBeIgnited)
		{
			return;
		}
		DeadlyDeadlyGoopManager.GoopPositionData goopPositionData = this.m_goopedCells[cellIndex];
		goopPositionData.IsOnFire = true;
		if (this.goopDefinition.ignitionChangesLifetime)
		{
			goopPositionData.remainingLifespan = Mathf.Min(this.m_goopedCells[cellIndex].remainingLifespan, this.goopDefinition.ignitedLifetime);
			goopPositionData.lifespanOverridden = true;
		}
	}

	// Token: 0x06006196 RID: 24982 RVA: 0x0025A348 File Offset: 0x00258548
	public void IgniteGoopCircle(Vector2 center, float radius)
	{
		if (!this.goopDefinition.CanBeIgnited)
		{
			return;
		}
		int num = Mathf.CeilToInt((center.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.FloorToInt((center.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.CeilToInt((center.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.FloorToInt((center.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				if (this.m_goopedCells.ContainsKey(intVector))
				{
					this.IgniteCell(intVector);
				}
			}
		}
	}

	// Token: 0x06006197 RID: 24983 RVA: 0x0025A40C File Offset: 0x0025860C
	public bool ProcessGameActor(GameActor actor)
	{
		if (!this.IsPositionInGoop(actor.specRigidbody.UnitCenter))
		{
			if (this.m_containedActors.ContainsKey(actor))
			{
				this.m_containedActors.Remove(actor);
				this.EndGoopEffect(actor);
			}
			return false;
		}
		IntVector2 intVector = (actor.specRigidbody.UnitCenter / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
		PlayerController playerController = actor as PlayerController;
		if (playerController && this.goopDefinition.playerStepsChangeLifetime && playerController.IsGrounded && !playerController.IsSlidingOverSurface)
		{
			for (int i = -2; i <= 2; i++)
			{
				for (int j = -2; j <= 2; j++)
				{
					if ((float)(Mathf.Abs(i) + Mathf.Abs(j)) <= 3.5f)
					{
						IntVector2 intVector2 = new IntVector2(intVector.x + i, intVector.y + j);
						if (this.m_goopedCells.ContainsKey(intVector2))
						{
							DeadlyDeadlyGoopManager.GoopPositionData goopPositionData = this.m_goopedCells[intVector2];
							if (goopPositionData.remainingLifespan > this.goopDefinition.playerStepsLifetime)
							{
								goopPositionData.remainingLifespan = this.goopDefinition.playerStepsLifetime;
							}
						}
					}
				}
			}
		}
		if (actor.IsFlying && !this.m_goopedCells[intVector].IsOnFire)
		{
			return false;
		}
		if (actor is PlayerController)
		{
			PlayerController playerController2 = actor as PlayerController;
			if (playerController2.CurrentGun && playerController2.CurrentGun.gunName == "Mermaid Gun")
			{
				return false;
			}
		}
		if (!this.m_containedActors.ContainsKey(actor))
		{
			this.m_containedActors.Add(actor, 0f);
			this.InitialGoopEffect(actor);
		}
		else
		{
			this.m_containedActors[actor] = this.m_containedActors[actor] + BraveTime.DeltaTime;
		}
		this.DoTimelessGoopEffect(actor, intVector);
		if (actor is AIActor)
		{
			this.DoGoopEffect(actor, intVector);
		}
		else if (actor is PlayerController)
		{
			PlayerController playerController3 = actor as PlayerController;
			if (this.goopDefinition.damagesPlayers && playerController3.spriteAnimator.QueryGroundedFrame())
			{
				if (playerController3.CurrentPoisonMeterValue >= 1f)
				{
					this.DoGoopEffect(actor, intVector);
					playerController3.CurrentPoisonMeterValue -= 1f;
				}
				playerController3.IncreasePoison(BraveTime.DeltaTime / this.goopDefinition.delayBeforeDamageToPlayers);
			}
			if (this.goopDefinition.DrainsAmmo && playerController3.spriteAnimator.QueryGroundedFrame())
			{
				if (playerController3.CurrentDrainMeterValue >= 1f)
				{
					playerController3.inventory.HandleAmmoDrain(this.goopDefinition.PercentAmmoDrainPerSecond * BraveTime.DeltaTime);
				}
				else
				{
					playerController3.CurrentDrainMeterValue += BraveTime.DeltaTime / this.goopDefinition.delayBeforeDamageToPlayers;
				}
			}
		}
		return true;
	}

	// Token: 0x06006198 RID: 24984 RVA: 0x0025A70C File Offset: 0x0025890C
	public bool IsPositionOnFire(Vector2 position)
	{
		IntVector2 intVector = (position / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
		DeadlyDeadlyGoopManager.GoopPositionData goopPositionData;
		return this.m_goopedCells.TryGetValue(intVector, out goopPositionData) && goopPositionData.remainingLifespan > this.goopDefinition.fadePeriod && goopPositionData.IsOnFire;
	}

	// Token: 0x06006199 RID: 24985 RVA: 0x0025A75C File Offset: 0x0025895C
	public bool IsPositionFrozen(Vector2 position)
	{
		IntVector2 intVector = (position / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
		DeadlyDeadlyGoopManager.GoopPositionData goopPositionData;
		return this.m_goopedCells.TryGetValue(intVector, out goopPositionData) && goopPositionData.remainingLifespan > this.goopDefinition.fadePeriod && goopPositionData.IsFrozen;
	}

	// Token: 0x0600619A RID: 24986 RVA: 0x0025A7AC File Offset: 0x002589AC
	public bool IsPositionInGoop(Vector2 position)
	{
		IntVector2 intVector = (position / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
		DeadlyDeadlyGoopManager.GoopPositionData goopPositionData;
		return this.m_goopedCells.TryGetValue(intVector, out goopPositionData) && goopPositionData.remainingLifespan > this.goopDefinition.fadePeriod;
	}

	// Token: 0x0600619B RID: 24987 RVA: 0x0025A7F8 File Offset: 0x002589F8
	public void InitialGoopEffect(GameActor actor)
	{
		if (this.goopDefinition.AppliesSpeedModifier)
		{
			actor.ApplyEffect(this.goopDefinition.SpeedModifierEffect, 1f, null);
		}
	}

	// Token: 0x0600619C RID: 24988 RVA: 0x0025A824 File Offset: 0x00258A24
	public void DoTimelessGoopEffect(GameActor actor, IntVector2 goopPosition)
	{
		float num = 0f;
		CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
		if (this.m_goopedCells[goopPosition].IsOnFire)
		{
			if (actor is PlayerController && !this.goopDefinition.UsesGreenFire)
			{
				PlayerController playerController = actor as PlayerController;
				if (playerController.IsGrounded && !playerController.IsSlidingOverSurface && playerController.healthHaver && playerController.healthHaver.IsVulnerable)
				{
					playerController.IsOnFire = true;
					playerController.CurrentFireMeterValue += BraveTime.DeltaTime * 0.5f;
				}
			}
			else if (actor is AIActor)
			{
				if (this.goopDefinition.fireBurnsEnemies)
				{
					if (actor.GetResistanceForEffectType(EffectResistanceType.Fire) < 1f)
					{
						num += this.goopDefinition.fireDamagePerSecondToEnemies * BraveTime.DeltaTime;
					}
					(actor as AIActor).ApplyEffect(this.goopDefinition.fireEffect, 1f, null);
				}
				else
				{
					num += this.goopDefinition.fireDamagePerSecondToEnemies * BraveTime.DeltaTime;
				}
			}
			coreDamageTypes |= CoreDamageTypes.Fire;
		}
		if (this.m_goopedCells[goopPosition].IsElectrified)
		{
			if (actor is PlayerController)
			{
				num = Mathf.Max(num, this.goopDefinition.electrifiedDamageToPlayer);
			}
			else if (actor is AIActor)
			{
				num += this.goopDefinition.electrifiedDamagePerSecondToEnemies * BraveTime.DeltaTime;
			}
			coreDamageTypes |= CoreDamageTypes.Electric;
		}
		if (this.goopDefinition.AppliesSpeedModifierContinuously)
		{
			actor.ApplyEffect(this.goopDefinition.SpeedModifierEffect, 1f, null);
		}
		if (this.goopDefinition.AppliesDamageOverTime)
		{
			actor.ApplyEffect(this.goopDefinition.HealthModifierEffect, 1f, null);
		}
		if (actor is AIActor && (actor as AIActor).IsNormalEnemy && this.goopDefinition.AppliesCharm)
		{
			actor.ApplyEffect(this.goopDefinition.CharmModifierEffect, 1f, null);
		}
		if (actor is AIActor && (actor as AIActor).IsNormalEnemy && this.goopDefinition.AppliesCheese)
		{
			AIActor aiactor = actor as AIActor;
			if (!aiactor.IsGone && aiactor.HasBeenEngaged)
			{
				actor.ApplyEffect(this.goopDefinition.CheeseModifierEffect, BraveTime.DeltaTime * this.goopDefinition.CheeseModifierEffect.CheeseAmount, null);
			}
		}
		if (num > 0f)
		{
			actor.healthHaver.ApplyDamage(num, Vector2.zero, StringTableManager.GetEnemiesString("#GOOP", -1), coreDamageTypes, DamageCategory.Environment, false, null, false);
		}
	}

	// Token: 0x0600619D RID: 24989 RVA: 0x0025AACC File Offset: 0x00258CCC
	public void DoGoopEffect(GameActor actor, IntVector2 goopPosition)
	{
		float num = 0f;
		if (this.goopDefinition.damagesPlayers && actor is PlayerController)
		{
			num = this.goopDefinition.damageToPlayers;
		}
		else if (this.goopDefinition.damagesEnemies && actor is AIActor)
		{
			num = this.goopDefinition.damagePerSecondtoEnemies * BraveTime.DeltaTime;
		}
		if (num > 0f)
		{
			actor.healthHaver.ApplyDamage(num, Vector2.zero, StringTableManager.GetEnemiesString("#GOOP", -1), this.goopDefinition.damageTypes, DamageCategory.Environment, true, null, false);
		}
	}

	// Token: 0x0600619E RID: 24990 RVA: 0x0025AB70 File Offset: 0x00258D70
	public void EndGoopEffect(GameActor actor)
	{
		if (this.goopDefinition.AppliesSpeedModifier)
		{
			actor.RemoveEffect(this.goopDefinition.SpeedModifierEffect);
		}
	}

	// Token: 0x0600619F RID: 24991 RVA: 0x0025AB94 File Offset: 0x00258D94
	private void SetColorDirty(IntVector2 goopPosition)
	{
		IntVector2 intVector = (goopPosition.ToVector2() * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
		int num = Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.FloorToInt((float)intVector.x / (float)this.CHUNK_SIZE);
		int num3 = Mathf.FloorToInt((float)intVector.y / (float)this.CHUNK_SIZE);
		bool flag = num2 > 0 && goopPosition.x % num == 0;
		bool flag2 = num2 < this.m_colorDirtyFlags.GetLength(0) - 1 && goopPosition.x % num == num - 1;
		bool flag3 = num3 > 0 && goopPosition.y % num == 0;
		bool flag4 = num3 < this.m_colorDirtyFlags.GetLength(1) - 1 && goopPosition.y % num == num - 1;
		this.m_colorDirtyFlags[num2, num3] = true;
		if (flag)
		{
			this.m_colorDirtyFlags[num2 - 1, num3] = true;
		}
		if (flag2)
		{
			this.m_colorDirtyFlags[num2 + 1, num3] = true;
		}
		if (flag3)
		{
			this.m_colorDirtyFlags[num2, num3 - 1] = true;
		}
		if (flag4)
		{
			this.m_colorDirtyFlags[num2, num3 + 1] = true;
		}
		if (flag && flag3)
		{
			this.m_colorDirtyFlags[num2 - 1, num3 - 1] = true;
		}
		if (flag && flag4)
		{
			this.m_colorDirtyFlags[num2 - 1, num3 + 1] = true;
		}
		if (flag2 && flag3)
		{
			this.m_colorDirtyFlags[num2 + 1, num3 - 1] = true;
		}
		if (flag2 && flag4)
		{
			this.m_colorDirtyFlags[num2 + 1, num3 + 1] = true;
		}
	}

	// Token: 0x060061A0 RID: 24992 RVA: 0x0025AD60 File Offset: 0x00258F60
	private void SetDirty(IntVector2 goopPosition)
	{
		int num = (int)((float)goopPosition.x * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = (int)((float)goopPosition.y * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.FloorToInt((float)num / (float)this.CHUNK_SIZE);
		int num5 = Mathf.FloorToInt((float)num2 / (float)this.CHUNK_SIZE);
		if (num4 < 0 || num4 >= this.m_dirtyFlags.GetLength(0) || num5 < 0 || num5 >= this.m_dirtyFlags.GetLength(1))
		{
			return;
		}
		bool flag = num4 > 0 && goopPosition.x % num3 == 0;
		bool flag2 = num4 < this.m_dirtyFlags.GetLength(0) - 1 && goopPosition.x % num3 == num3 - 1;
		bool flag3 = num5 > 0 && goopPosition.y % num3 == 0;
		bool flag4 = num5 < this.m_dirtyFlags.GetLength(1) - 1 && goopPosition.y % num3 == num3 - 1;
		this.m_dirtyFlags[num4, num5] = true;
		if (flag)
		{
			this.m_dirtyFlags[num4 - 1, num5] = true;
		}
		if (flag2)
		{
			this.m_dirtyFlags[num4 + 1, num5] = true;
		}
		if (flag3)
		{
			this.m_dirtyFlags[num4, num5 - 1] = true;
		}
		if (flag4)
		{
			this.m_dirtyFlags[num4, num5 + 1] = true;
		}
		if (flag && flag3)
		{
			this.m_dirtyFlags[num4 - 1, num5 - 1] = true;
		}
		if (flag && flag4)
		{
			this.m_dirtyFlags[num4 - 1, num5 + 1] = true;
		}
		if (flag2 && flag3)
		{
			this.m_dirtyFlags[num4 + 1, num5 - 1] = true;
		}
		if (flag2 && flag4)
		{
			this.m_dirtyFlags[num4 + 1, num5 + 1] = true;
		}
	}

	// Token: 0x060061A1 RID: 24993 RVA: 0x0025AF6C File Offset: 0x0025916C
	private void InitialzeUV2IfNecessary()
	{
		if (this.goopDefinition.usesWorldTextureByDefault)
		{
			int num = Mathf.RoundToInt(4f * ((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE) * ((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE));
			for (int i = 0; i < num; i++)
			{
				this.m_uv2Array[i] = Vector2.right;
			}
		}
	}

	// Token: 0x060061A2 RID: 24994 RVA: 0x0025AFD8 File Offset: 0x002591D8
	private void InitializeParticleSystems()
	{
		string text = ((!this.goopDefinition.UsesGreenFire) ? "Gungeon_Fire_Main" : "Gungeon_Fire_Main_Green");
		string text2 = ((!this.goopDefinition.UsesGreenFire) ? "Gungeon_Fire_Intro" : "Gungeon_Fire_Intro_Green");
		string text3 = ((!this.goopDefinition.UsesGreenFire) ? "Gungeon_Fire_Outro" : "Gungeon_Fire_Outro_Green");
		GameObject gameObject = GameObject.Find(text);
		if (gameObject == null)
		{
			string text4 = ((!this.goopDefinition.UsesGreenFire) ? "Particles/Gungeon_Fire_Main_raw" : "Particles/Gungeon_Fire_Main_green");
			gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(text4, ".prefab"), Vector3.zero, Quaternion.identity);
			gameObject.name = text;
		}
		this.m_fireSystem = gameObject.GetComponent<ParticleSystem>();
		GameObject gameObject2 = GameObject.Find(text2);
		if (gameObject2 == null)
		{
			string text5 = ((!this.goopDefinition.UsesGreenFire) ? "Particles/Gungeon_Fire_Intro_raw" : "Particles/Gungeon_Fire_Intro_green");
			gameObject2 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(text5, ".prefab"), Vector3.zero, Quaternion.identity);
			gameObject2.name = text2;
		}
		this.m_fireIntroSystem = gameObject2.GetComponent<ParticleSystem>();
		GameObject gameObject3 = GameObject.Find(text3);
		if (gameObject3 == null)
		{
			string text6 = ((!this.goopDefinition.UsesGreenFire) ? "Particles/Gungeon_Fire_Outro_raw" : "Particles/Gungeon_Fire_Outro_green");
			gameObject3 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(text6, ".prefab"), Vector3.zero, Quaternion.identity);
			gameObject3.name = text3;
		}
		this.m_fireOutroSystem = gameObject3.GetComponent<ParticleSystem>();
		GameObject gameObject4 = GameObject.Find("Gungeon_Elec");
		if (gameObject4 == null)
		{
			gameObject4 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Particles/Gungeon_Elec_raw", ".prefab"), Vector3.zero, Quaternion.identity);
			gameObject4.name = "Gungeon_Elec";
		}
		this.m_elecSystem = gameObject4.GetComponent<ParticleSystem>();
	}

	// Token: 0x060061A3 RID: 24995 RVA: 0x0025B1E0 File Offset: 0x002593E0
	private void LateUpdate()
	{
		if (Time.timeScale <= 0f || GameManager.Instance.IsPaused)
		{
			return;
		}
		this.m_removalPositions.Clear();
		bool flag = false;
		bool flag2 = false;
		this.m_currentUpdateBin = (this.m_currentUpdateBin + 1) % 4;
		this.m_deltaTimes.Enqueue(BraveTime.DeltaTime);
		float num = 0f;
		for (int i = 0; i < this.m_deltaTimes.Count; i++)
		{
			num += this.m_deltaTimes[i];
		}
		foreach (IntVector2 intVector in this.m_goopedPositions)
		{
			DeadlyDeadlyGoopManager.GoopPositionData goopPositionData = this.m_goopedCells[intVector];
			if (goopPositionData.GoopUpdateBin == this.m_currentUpdateBin)
			{
				goopPositionData.unfrozeLastFrame = false;
				if (this.goopDefinition.usesAmbientGoopFX && goopPositionData.remainingLifespan > 0f && UnityEngine.Random.value < this.goopDefinition.ambientGoopFXChance && goopPositionData.SupportsAmbientVFX)
				{
					Vector3 vector = intVector.ToVector3((float)intVector.y) * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
					this.goopDefinition.ambientGoopFX.SpawnAtPosition(vector, 0f, null, null, null, null, false, null, null, false);
				}
				if (goopPositionData.IsOnFire || goopPositionData.IsElectrified || this.goopDefinition.usesLifespan || goopPositionData.lifespanOverridden || goopPositionData.selfIgnites)
				{
					if (goopPositionData.selfIgnites)
					{
						if (goopPositionData.remainingTimeTilSelfIgnition <= 0f)
						{
							goopPositionData.selfIgnites = false;
							this.IgniteCell(intVector);
						}
						else
						{
							goopPositionData.remainingTimeTilSelfIgnition -= num;
						}
					}
					if (goopPositionData.remainingLifespan > 0f)
					{
						if (!goopPositionData.IsFrozen)
						{
							goopPositionData.remainingLifespan -= num;
						}
						else
						{
							goopPositionData.remainingFreezeTimer -= num;
							if (goopPositionData.remainingFreezeTimer <= 0f)
							{
								goopPositionData.hasBeenFrozen = 1;
								goopPositionData.remainingLifespan = Mathf.Min(goopPositionData.remainingLifespan, this.goopDefinition.fadePeriod);
								goopPositionData.remainingLifespan -= num;
							}
						}
						if (this.goopDefinition.usesAcidAudio)
						{
							flag2 = true;
						}
						if (goopPositionData.remainingLifespan < this.goopDefinition.fadePeriod && goopPositionData.IsElectrified)
						{
							goopPositionData.remainingLifespan = this.goopDefinition.fadePeriod;
						}
						if (goopPositionData.remainingLifespan < this.goopDefinition.fadePeriod || goopPositionData.remainingLifespan <= 0f)
						{
							this.SetDirty(intVector);
							goopPositionData.IsOnFire = false;
							goopPositionData.IsElectrified = false;
							goopPositionData.HasPlayedFireIntro = false;
							goopPositionData.HasPlayedFireOutro = false;
							if (goopPositionData.remainingLifespan <= 0f)
							{
								this.m_removalPositions.Add(intVector);
								continue;
							}
						}
						if (goopPositionData.IsElectrified)
						{
							goopPositionData.remainingElectrifiedTime -= num;
							goopPositionData.remainingElecTimer -= num;
							if (goopPositionData.remainingElectrifiedTime <= 0f)
							{
								goopPositionData.IsElectrified = false;
								goopPositionData.remainingElectrifiedTime = 0f;
							}
							if (goopPositionData.IsElectrified && this.m_elecSystem != null && goopPositionData.remainingElecTimer <= 0f && intVector.x % 2 == 0 && intVector.y % 2 == 0)
							{
								Vector3 vector2 = intVector.ToVector3((float)intVector.y) * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE + new Vector3(UnityEngine.Random.Range(0.125f, 0.375f), UnityEngine.Random.Range(0.125f, 0.375f), 0.125f).Quantize(0.0625f);
								float num2 = UnityEngine.Random.Range(0.75f, 1.5f);
								if (UnityEngine.Random.value < 0.1f)
								{
									ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
									{
										position = vector2,
										velocity = Vector3.zero,
										startSize = this.m_fireSystem.startSize,
										startLifetime = num2,
										startColor = this.m_fireSystem.startColor,
										randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
									};
									this.m_elecSystem.Emit(emitParams, 1);
									if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
									{
										int num3 = ((GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.MEDIUM) ? 10 : 4);
										GlobalSparksDoer.DoRandomParticleBurst(num3, vector2 + new Vector3(-0.625f, -0.625f, 0f), vector2 + new Vector3(0.625f, 0.625f, 0f), Vector3.up, 120f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
									}
								}
								goopPositionData.remainingElecTimer = num2 - 0.1f;
							}
						}
						if (goopPositionData.IsFrozen)
						{
							if (goopPositionData.totalOnFireTime < 0.5f || goopPositionData.remainingLifespan < this.goopDefinition.fadePeriod)
							{
								this.SetColorDirty(intVector);
							}
							goopPositionData.totalOnFireTime += num;
							if (goopPositionData.totalOnFireTime >= this.goopDefinition.freezeSpreadTime)
							{
								for (int j = 0; j < goopPositionData.neighborGoopData.Length; j++)
								{
									if (goopPositionData.neighborGoopData[j] != null && !goopPositionData.neighborGoopData[j].IsFrozen && goopPositionData.neighborGoopData[j].hasBeenFrozen == 0)
									{
										if (UnityEngine.Random.value < 0.2f)
										{
											this.FreezeCell(goopPositionData.neighborGoopData[j].goopPosition);
										}
										else
										{
											goopPositionData.totalFrozenTime = 0f;
										}
									}
								}
							}
						}
						if (goopPositionData.IsOnFire)
						{
							flag = true;
							this.SetColorDirty(intVector);
							goopPositionData.remainingFireTimer -= num;
							goopPositionData.totalOnFireTime += num;
							if (goopPositionData.totalOnFireTime >= this.goopDefinition.igniteSpreadTime)
							{
								for (int k = 0; k < goopPositionData.neighborGoopData.Length; k++)
								{
									if (goopPositionData.neighborGoopData[k] != null && !goopPositionData.neighborGoopData[k].IsOnFire)
									{
										if (UnityEngine.Random.value < 0.2f)
										{
											this.IgniteCell(goopPositionData.neighborGoopData[k].goopPosition);
										}
										else
										{
											goopPositionData.totalOnFireTime = 0f;
										}
									}
								}
							}
						}
						if (this.m_fireSystem != null && goopPositionData.IsOnFire && goopPositionData.remainingFireTimer <= 0f && intVector.x % 2 == 0 && intVector.y % 2 == 0)
						{
							Vector3 vector3 = intVector.ToVector3((float)intVector.y) * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE + new Vector3(UnityEngine.Random.Range(0.125f, 0.375f), UnityEngine.Random.Range(0.125f, 0.375f), 0.125f).Quantize(0.0625f);
							float num4 = UnityEngine.Random.Range(1f, 1.5f);
							float num5 = UnityEngine.Random.Range(0.75f, 1f);
							if (!goopPositionData.HasPlayedFireOutro)
							{
								if (!goopPositionData.HasPlayedFireOutro && goopPositionData.remainingLifespan <= num5 + this.goopDefinition.fadePeriod && this.m_fireOutroSystem != null)
								{
									num4 = num5;
									ParticleSystem.EmitParams emitParams2 = new ParticleSystem.EmitParams
									{
										position = vector3,
										velocity = Vector3.zero,
										startSize = this.m_fireSystem.startSize,
										startLifetime = num5,
										startColor = this.m_fireSystem.startColor,
										randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
									};
									this.m_fireOutroSystem.Emit(emitParams2, 1);
									goopPositionData.HasPlayedFireOutro = true;
								}
								else if (!goopPositionData.HasPlayedFireIntro && this.m_fireIntroSystem != null)
								{
									num4 = UnityEngine.Random.Range(0.75f, 1f);
									ParticleSystem.EmitParams emitParams3 = new ParticleSystem.EmitParams
									{
										position = vector3,
										velocity = Vector3.zero,
										startSize = this.m_fireSystem.startSize,
										startLifetime = num4,
										startColor = this.m_fireSystem.startColor,
										randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
									};
									this.m_fireIntroSystem.Emit(emitParams3, 1);
									goopPositionData.HasPlayedFireIntro = true;
								}
								else
								{
									if (UnityEngine.Random.value < 0.5f)
									{
										ParticleSystem.EmitParams emitParams4 = new ParticleSystem.EmitParams
										{
											position = vector3,
											velocity = Vector3.zero,
											startSize = this.m_fireSystem.startSize,
											startLifetime = num4,
											startColor = this.m_fireSystem.startColor,
											randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
										};
										this.m_fireSystem.Emit(emitParams4, 1);
									}
									GlobalSparksDoer.DoRandomParticleBurst(UnityEngine.Random.Range(3, 6), vector3, vector3, Vector3.up * 2f, 30f, 1f, null, new float?(UnityEngine.Random.Range(0.5f, 1f)), new Color?((!this.goopDefinition.UsesGreenFire) ? Color.red : Color.green), GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT);
								}
							}
							goopPositionData.remainingFireTimer = num4 - 0.125f;
						}
					}
					else
					{
						this.m_removalPositions.Add(intVector);
					}
				}
			}
		}
		if (flag && !this.m_isPlayingFireAudio)
		{
			this.m_isPlayingFireAudio = true;
			AkSoundEngine.PostEvent("Play_ENV_oilfire_ignite_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
		else if (!flag && this.m_isPlayingFireAudio)
		{
			this.m_isPlayingFireAudio = false;
			AkSoundEngine.PostEvent("Stop_ENV_oilfire_loop_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
		if (flag2 && !this.m_isPlayingAcidAudio)
		{
			this.m_isPlayingAcidAudio = true;
			AkSoundEngine.PostEvent("Play_ENV_acidsizzle_loop_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
		else if (!flag2 && this.m_isPlayingAcidAudio)
		{
			this.m_isPlayingAcidAudio = false;
			AkSoundEngine.PostEvent("Stop_ENV_acidsizzle_loop_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
		this.RemoveGoopedPosition(this.m_removalPositions);
		for (int l = 0; l < this.m_dirtyFlags.GetLength(0); l++)
		{
			for (int m = 0; m < this.m_dirtyFlags.GetLength(1); m++)
			{
				if (this.m_dirtyFlags[l, m])
				{
					int num6 = (m * this.m_dirtyFlags.GetLength(0) + l) % 3;
					if (num6 == Time.frameCount % 3)
					{
						bool flag3 = this.HasGoopedPositionCountForChunk(l, m);
						if (flag3)
						{
							this.RebuildMeshUvsAndColors(l, m);
						}
						this.m_dirtyFlags[l, m] = false;
						this.m_colorDirtyFlags[l, m] = false;
						if (this.m_meshes[l, m] != null && !flag3)
						{
							UnityEngine.Object.Destroy(this.m_mrs[l, m].gameObject);
							UnityEngine.Object.Destroy(this.m_meshes[l, m]);
							this.m_mrs[l, m] = null;
							this.m_meshes[l, m] = null;
						}
					}
				}
				else if (this.m_colorDirtyFlags[l, m])
				{
					int num7 = (m * this.m_dirtyFlags.GetLength(0) + l) % 3;
					if (num7 == Time.frameCount % 3)
					{
						bool flag4 = this.HasGoopedPositionCountForChunk(l, m);
						if (flag4)
						{
							this.RebuildMeshColors(l, m);
						}
						this.m_colorDirtyFlags[l, m] = false;
						if (this.m_meshes[l, m] != null && !flag4)
						{
							UnityEngine.Object.Destroy(this.m_mrs[l, m].gameObject);
							UnityEngine.Object.Destroy(this.m_meshes[l, m]);
							this.m_mrs[l, m] = null;
							this.m_meshes[l, m] = null;
						}
					}
				}
			}
		}
	}

	// Token: 0x060061A4 RID: 24996 RVA: 0x0025BF54 File Offset: 0x0025A154
	private bool HasGoopedPositionCountForChunk(int chunkX, int chunkY)
	{
		int num = Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		IntVector2 intVector = new IntVector2(chunkX * num, chunkY * num);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (this.m_goopedPositions.Contains(intVector + new IntVector2(i, j)))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060061A5 RID: 24997 RVA: 0x0025BFC8 File Offset: 0x0025A1C8
	private void RebuildMeshUvsAndColors(int chunkX, int chunkY)
	{
		Mesh chunkMesh = this.GetChunkMesh(chunkX, chunkY);
		for (int i = 0; i < this.m_colorArray.Length; i++)
		{
			this.m_colorArray[i] = new Color32(0, 0, 0, 0);
		}
		int num = chunkX * Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = num + Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = chunkY * Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = num3 + Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		for (int j = num; j < num2; j++)
		{
			for (int k = num3; k < num4; k++)
			{
				IntVector2 intVector;
				intVector.x = j;
				intVector.y = k;
				DeadlyDeadlyGoopManager.GoopPositionData goopPositionData;
				if (this.m_goopedCells.TryGetValue(intVector, out goopPositionData) && goopPositionData.remainingLifespan > 0f)
				{
					if (goopPositionData.baseIndex < 0)
					{
						goopPositionData.baseIndex = this.GetGoopBaseIndex(intVector, chunkX, chunkY);
					}
					this.AssignUvsAndColors(goopPositionData, intVector, chunkX, chunkY);
				}
			}
		}
		chunkMesh.uv = this.m_uvArray;
		chunkMesh.uv2 = this.m_uv2Array;
		chunkMesh.colors32 = this.m_colorArray;
	}

	// Token: 0x060061A6 RID: 24998 RVA: 0x0025C11C File Offset: 0x0025A31C
	private void RebuildMeshColors(int chunkX, int chunkY)
	{
		Mesh chunkMesh = this.GetChunkMesh(chunkX, chunkY);
		for (int i = 0; i < this.m_colorArray.Length; i++)
		{
			this.m_colorArray[i] = new Color32(0, 0, 0, 0);
		}
		int num = chunkX * Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = num + Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = chunkY * Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = num3 + Mathf.RoundToInt((float)this.CHUNK_SIZE / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		DeadlyDeadlyGoopManager.VertexColorRebuildResult vertexColorRebuildResult = DeadlyDeadlyGoopManager.VertexColorRebuildResult.ALL_OK;
		foreach (IntVector2 intVector in this.m_goopedPositions)
		{
			DeadlyDeadlyGoopManager.GoopPositionData goopPositionData = this.m_goopedCells[intVector];
			if (goopPositionData.remainingLifespan >= 0f)
			{
				if (intVector.x >= num && intVector.x < num2 && intVector.y >= num3 && intVector.y < num4)
				{
					if (goopPositionData.baseIndex < 0)
					{
						goopPositionData.baseIndex = this.GetGoopBaseIndex(intVector, chunkX, chunkY);
					}
					if (this.goopDefinition.CanBeFrozen)
					{
						int num5 = ((!goopPositionData.IsFrozen) ? 0 : 1);
						this.m_uv2Array[goopPositionData.baseIndex] = new Vector2((float)num5, 0f);
						this.m_uv2Array[goopPositionData.baseIndex + 1] = new Vector2((float)num5, 0f);
						this.m_uv2Array[goopPositionData.baseIndex + 2] = new Vector2((float)num5, 0f);
						this.m_uv2Array[goopPositionData.baseIndex + 3] = new Vector2((float)num5, 0f);
					}
					DeadlyDeadlyGoopManager.VertexColorRebuildResult vertexColorRebuildResult2 = this.AssignVertexColors(goopPositionData, intVector, chunkX, chunkY);
					vertexColorRebuildResult = (DeadlyDeadlyGoopManager.VertexColorRebuildResult)Mathf.Max((int)vertexColorRebuildResult2, (int)vertexColorRebuildResult);
				}
			}
		}
		if (this.goopDefinition.CanBeFrozen)
		{
			chunkMesh.uv2 = this.m_uv2Array;
		}
		chunkMesh.colors32 = this.m_colorArray;
	}

	// Token: 0x060061A7 RID: 24999 RVA: 0x0025C390 File Offset: 0x0025A590
	private void PostprocessRebuildResult(int chunkX, int chunkY, DeadlyDeadlyGoopManager.VertexColorRebuildResult rr)
	{
		MeshRenderer meshRenderer = this.m_mrs[chunkX, chunkY];
		Material sharedMaterial = meshRenderer.sharedMaterial;
		if (rr == DeadlyDeadlyGoopManager.VertexColorRebuildResult.ALL_OK)
		{
			float num = ((!this.goopDefinition.usesOverrideOpaqueness) ? 0.5f : this.goopDefinition.overrideOpaqueness);
			sharedMaterial.SetFloat(DeadlyDeadlyGoopManager.OpaquenessMultiplyPropertyID, num);
			sharedMaterial.SetFloat(DeadlyDeadlyGoopManager.BrightnessMultiplyPropertyID, 1f);
		}
		else if (rr == DeadlyDeadlyGoopManager.VertexColorRebuildResult.ELECTRIFIED)
		{
			sharedMaterial.SetFloat(DeadlyDeadlyGoopManager.OpaquenessMultiplyPropertyID, 1f);
			sharedMaterial.SetFloat(DeadlyDeadlyGoopManager.BrightnessMultiplyPropertyID, 5f);
		}
	}

	// Token: 0x060061A8 RID: 25000 RVA: 0x0025C428 File Offset: 0x0025A628
	private void InitMesh(IntVector2 goopPos, int chunkX, int chunkY)
	{
		int num = chunkX * this.CHUNK_SIZE;
		int num2 = chunkY * this.CHUNK_SIZE;
		int num3 = (int)((float)num / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE + 0.5f);
		int num4 = (int)((float)num2 / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE + 0.5f);
		int num5 = goopPos.x - num3;
		int num6 = goopPos.y - num4;
		int num7 = num6 * (4 * (int)(1f / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE)) * this.CHUNK_SIZE + num5 * 4;
		bool flag = false;
		IntVector2 intVector = new IntVector2((int)((float)goopPos.x * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE), (int)((float)goopPos.y * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE));
		if (GameManager.Instance.Dungeon.data.CheckInBounds(intVector))
		{
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			flag = cellData != null && !cellData.forceDisallowGoop && cellData.IsLowerFaceWall();
		}
		if (flag)
		{
			float num8 = (float)goopPos.x * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
			float num9 = (float)goopPos.y * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
			float num10 = Mathf.Floor(num9) - num9 % 1f;
			Vector3 vector = new Vector3(num8 - (float)num, num9 - (float)num2, num10 - (float)num2);
			this.m_vertexArray[num7] = vector;
			this.m_vertexArray[num7 + 1] = vector + new Vector3(DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, 0f, 0f);
			this.m_vertexArray[num7 + 2] = vector + new Vector3(0f, DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, -DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
			this.m_vertexArray[num7 + 3] = vector + new Vector3(DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, -DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		}
		else
		{
			Vector3 vector2 = new Vector3((float)goopPos.x * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE - (float)num, (float)goopPos.y * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE - (float)num2, (float)goopPos.y * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE - (float)num2);
			this.m_vertexArray[num7] = vector2;
			this.m_vertexArray[num7 + 1] = vector2 + new Vector3(DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, 0f, 0f);
			this.m_vertexArray[num7 + 2] = vector2 + new Vector3(0f, DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
			this.m_vertexArray[num7 + 3] = vector2 + new Vector3(DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		}
		int num11 = num7 / 4 * 6;
		this.m_triangleArray[num11] = num7;
		this.m_triangleArray[num11 + 1] = num7 + 1;
		this.m_triangleArray[num11 + 2] = num7 + 2;
		this.m_triangleArray[num11 + 3] = num7 + 3;
		this.m_triangleArray[num11 + 4] = num7 + 2;
		this.m_triangleArray[num11 + 5] = num7 + 1;
	}

	// Token: 0x060061A9 RID: 25001 RVA: 0x0025C748 File Offset: 0x0025A948
	private int GetGoopBaseIndex(IntVector2 goopPos, int chunkX, int chunkY)
	{
		int num = chunkX * this.CHUNK_SIZE;
		int num2 = chunkY * this.CHUNK_SIZE;
		int num3 = (int)((float)num / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE + 0.5f);
		int num4 = (int)((float)num2 / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE + 0.5f);
		int num5 = goopPos.x - num3;
		int num6 = goopPos.y - num4;
		return num6 * (4 * (int)(1f / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE)) * this.CHUNK_SIZE + num5 * 4;
	}

	// Token: 0x060061AA RID: 25002 RVA: 0x0025C7BC File Offset: 0x0025A9BC
	private void AssignUvsAndColors(DeadlyDeadlyGoopManager.GoopPositionData goopData, IntVector2 goopPos, int chunkX, int chunkY)
	{
		Vector2 vector;
		if (this.m_uvMap.ContainsKey(goopData.NeighborsAsInt))
		{
			vector = this.m_uvMap[goopData.NeighborsAsInt];
		}
		else if (this.m_uvMap.ContainsKey(goopData.NeighborsAsIntFuckDiagonals))
		{
			vector = this.m_uvMap[goopData.NeighborsAsIntFuckDiagonals];
		}
		else
		{
			vector = this.m_uvMap[-1];
		}
		if (goopData.NeighborsAsInt == 255)
		{
			vector = this.m_centerUVOptions[Mathf.FloorToInt((float)this.m_centerUVOptions.Count * goopPos.GetHashedRandomValue())];
		}
		this.m_uvArray[goopData.baseIndex] = vector;
		this.m_uvArray[goopData.baseIndex + 1] = vector + new Vector2(0.125f, 0f);
		this.m_uvArray[goopData.baseIndex + 2] = vector + new Vector2(0f, 0.125f);
		this.m_uvArray[goopData.baseIndex + 3] = vector + new Vector2(0.125f, 0.125f);
		if (this.goopDefinition.CanBeFrozen)
		{
			int num = ((!goopData.IsFrozen) ? 0 : 1);
			this.m_uv2Array[goopData.baseIndex] = new Vector2((float)num, 0f);
			this.m_uv2Array[goopData.baseIndex + 1] = new Vector2((float)num, 0f);
			this.m_uv2Array[goopData.baseIndex + 2] = new Vector2((float)num, 0f);
			this.m_uv2Array[goopData.baseIndex + 3] = new Vector2((float)num, 0f);
		}
		this.AssignVertexColors(goopData, goopPos, chunkX, chunkY);
	}

	// Token: 0x060061AB RID: 25003 RVA: 0x0025C9C0 File Offset: 0x0025ABC0
	private DeadlyDeadlyGoopManager.VertexColorRebuildResult AssignVertexColors(DeadlyDeadlyGoopManager.GoopPositionData goopData, IntVector2 goopPos, int chunkX, int chunkY)
	{
		DeadlyDeadlyGoopManager.VertexColorRebuildResult vertexColorRebuildResult = DeadlyDeadlyGoopManager.VertexColorRebuildResult.ALL_OK;
		bool flag = false;
		Color32 color = this.goopDefinition.baseColor32;
		Color32 color2 = color;
		Color32 color3 = color;
		Color32 color4 = color;
		if (goopData.IsOnFire)
		{
			color = this.goopDefinition.fireColor32;
		}
		else if (goopData.HasOnFireNeighbor)
		{
			flag = true;
			for (int i = 0; i < 8; i++)
			{
				if (goopData.neighborGoopData[i] != null && goopData.neighborGoopData[i].IsOnFire)
				{
					switch (i)
					{
					case 0:
						color3 = this.goopDefinition.igniteColor32;
						color4 = this.goopDefinition.igniteColor32;
						break;
					case 1:
						color4 = this.goopDefinition.igniteColor32;
						break;
					case 2:
						color4 = this.goopDefinition.igniteColor32;
						color2 = this.goopDefinition.igniteColor32;
						break;
					case 3:
						color2 = this.goopDefinition.igniteColor32;
						break;
					case 4:
						color2 = this.goopDefinition.igniteColor32;
						color = this.goopDefinition.igniteColor32;
						break;
					case 5:
						color = this.goopDefinition.igniteColor32;
						break;
					case 6:
						color = this.goopDefinition.igniteColor32;
						color3 = this.goopDefinition.igniteColor32;
						break;
					case 7:
						color3 = this.goopDefinition.igniteColor32;
						break;
					}
				}
			}
		}
		else if (goopData.IsFrozen)
		{
			color = this.goopDefinition.frozenColor32;
		}
		else if (goopData.HasFrozenNeighbor)
		{
			flag = true;
			for (int j = 0; j < 8; j++)
			{
				if (goopData.neighborGoopData[j] != null && goopData.neighborGoopData[j].IsFrozen)
				{
					switch (j)
					{
					case 0:
						this.m_uv2Array[goopData.baseIndex + 2] = new Vector2(0.5f, 0f);
						color3 = this.goopDefinition.prefreezeColor32;
						this.m_uv2Array[goopData.baseIndex + 3] = new Vector2(0.5f, 0f);
						color4 = this.goopDefinition.prefreezeColor32;
						break;
					case 1:
						this.m_uv2Array[goopData.baseIndex + 3] = new Vector2(0.5f, 0f);
						color4 = this.goopDefinition.prefreezeColor32;
						break;
					case 2:
						this.m_uv2Array[goopData.baseIndex + 3] = new Vector2(0.5f, 0f);
						color4 = this.goopDefinition.prefreezeColor32;
						this.m_uv2Array[goopData.baseIndex + 1] = new Vector2(0.5f, 0f);
						color2 = this.goopDefinition.prefreezeColor32;
						break;
					case 3:
						this.m_uv2Array[goopData.baseIndex + 1] = new Vector2(0.5f, 0f);
						color2 = this.goopDefinition.prefreezeColor32;
						break;
					case 4:
						this.m_uv2Array[goopData.baseIndex + 1] = new Vector2(0.5f, 0f);
						color2 = this.goopDefinition.prefreezeColor32;
						this.m_uv2Array[goopData.baseIndex] = new Vector2(0.5f, 0f);
						color = this.goopDefinition.prefreezeColor32;
						break;
					case 5:
						this.m_uv2Array[goopData.baseIndex] = new Vector2(0.5f, 0f);
						color = this.goopDefinition.prefreezeColor32;
						break;
					case 6:
						this.m_uv2Array[goopData.baseIndex] = new Vector2(0.5f, 0f);
						color = this.goopDefinition.prefreezeColor32;
						this.m_uv2Array[goopData.baseIndex + 2] = new Vector2(0.5f, 0f);
						color3 = this.goopDefinition.prefreezeColor32;
						break;
					case 7:
						this.m_uv2Array[goopData.baseIndex + 2] = new Vector2(0.5f, 0f);
						color3 = this.goopDefinition.prefreezeColor32;
						break;
					}
				}
			}
		}
		if (goopData.remainingLifespan < this.goopDefinition.fadePeriod)
		{
			color = Color32.Lerp(this.goopDefinition.fadeColor32, color, goopData.remainingLifespan / this.goopDefinition.fadePeriod);
			if (flag)
			{
				color2 = Color32.Lerp(this.goopDefinition.fadeColor32, color2, goopData.remainingLifespan / this.goopDefinition.fadePeriod);
				color3 = Color32.Lerp(this.goopDefinition.fadeColor32, color3, goopData.remainingLifespan / this.goopDefinition.fadePeriod);
				color4 = Color32.Lerp(this.goopDefinition.fadeColor32, color4, goopData.remainingLifespan / this.goopDefinition.fadePeriod);
			}
		}
		if (flag)
		{
			this.m_colorArray[goopData.baseIndex] = color;
			this.m_colorArray[goopData.baseIndex + 1] = color2;
			this.m_colorArray[goopData.baseIndex + 2] = color3;
			this.m_colorArray[goopData.baseIndex + 3] = color4;
		}
		else
		{
			this.m_colorArray[goopData.baseIndex] = color;
			this.m_colorArray[goopData.baseIndex + 1] = color;
			this.m_colorArray[goopData.baseIndex + 2] = color;
			this.m_colorArray[goopData.baseIndex + 3] = color;
		}
		return vertexColorRebuildResult;
	}

	// Token: 0x060061AC RID: 25004 RVA: 0x0025CFD0 File Offset: 0x0025B1D0
	private void RemoveGoopedPosition(IntVector2 entry)
	{
		IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
		for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
		{
			IntVector2 intVector = entry + cardinalsAndOrdinals[i];
			DeadlyDeadlyGoopManager.GoopPositionData goopPositionData;
			if (this.m_goopedCells.TryGetValue(intVector, out goopPositionData))
			{
				goopPositionData.neighborGoopData[(i + 4) % 8] = null;
				goopPositionData.SetNeighborGoop((i + 4) % 8, false);
			}
		}
		this.m_goopedPositions.Remove(entry);
		this.m_goopedCells.Remove(entry);
		DeadlyDeadlyGoopManager.allGoopPositionMap.Remove(entry);
		this.SetDirty(entry);
	}

	// Token: 0x060061AD RID: 25005 RVA: 0x0025D064 File Offset: 0x0025B264
	private void RemoveGoopedPosition(List<IntVector2> entries)
	{
		for (int i = 0; i < entries.Count; i++)
		{
			IntVector2 intVector = entries[i];
			this.RemoveGoopedPosition(intVector);
		}
	}

	// Token: 0x060061AE RID: 25006 RVA: 0x0025D098 File Offset: 0x0025B298
	public int CountGoopCircle(Vector2 center, float radius)
	{
		if (this.m_goopedCells == null || this.m_goopedCells.Count == 0)
		{
			return 0;
		}
		int num = Mathf.FloorToInt((center.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.CeilToInt((center.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.FloorToInt((center.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.CeilToInt((center.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		Vector2 vector = center / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num5 = radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		int num6 = 0;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				if (Vector2.Distance(intVector.ToVector2(), vector) < num5 && this.m_goopedCells.ContainsKey(intVector))
				{
					num6++;
				}
			}
		}
		return num6;
	}

	// Token: 0x060061AF RID: 25007 RVA: 0x0025D198 File Offset: 0x0025B398
	public void RemoveGoopCircle(Vector2 center, float radius)
	{
		int num = Mathf.FloorToInt((center.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.CeilToInt((center.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.FloorToInt((center.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.CeilToInt((center.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		Vector2 vector = center / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num5 = radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), vector) < num5)
				{
					this.RemoveGoopedPosition(new IntVector2(i, j));
				}
			}
		}
	}

	// Token: 0x060061B0 RID: 25008 RVA: 0x0025D264 File Offset: 0x0025B464
	private void AddGoopedPosition(IntVector2 pos, float radiusFraction = 0f, bool suppressSplashes = false, int sourceId = -1, int sourceFrameCount = -1)
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		Vector2 vector = pos.ToVector2() * DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		Vector2 vector2 = vector + new Vector2(DeadlyDeadlyGoopManager.GOOP_GRID_SIZE, DeadlyDeadlyGoopManager.GOOP_GRID_SIZE) * 0.5f;
		for (int i = 0; i < DeadlyDeadlyGoopManager.m_goopExceptions.Count; i++)
		{
			if (DeadlyDeadlyGoopManager.m_goopExceptions[i] != null)
			{
				Vector2 first = DeadlyDeadlyGoopManager.m_goopExceptions[i].First;
				float second = DeadlyDeadlyGoopManager.m_goopExceptions[i].Second;
				if ((first - vector2).sqrMagnitude < second)
				{
					return;
				}
			}
		}
		if (!this.m_goopedCells.ContainsKey(pos))
		{
			IntVector2 intVector = vector.ToIntVector2(VectorConversions.Floor);
			if (!GameManager.Instance.Dungeon.data.CheckInBounds(intVector))
			{
				return;
			}
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			if (cellData == null)
			{
				return;
			}
			if (cellData != null && cellData.forceDisallowGoop)
			{
				return;
			}
			if (cellData.cellVisualData.absorbsDebris && this.goopDefinition.CanBeFrozen)
			{
				return;
			}
			if (this.goopDefinition.CanBeFrozen)
			{
				GameManager.Instance.Dungeon.data.SolidifyLavaInCell(intVector);
			}
			bool flag = cellData.IsLowerFaceWall();
			if (flag && pos.GetHashedRandomValue() > 0.75f)
			{
				flag = false;
			}
			if (cellData.type == CellType.FLOOR || flag || cellData.forceAllowGoop)
			{
				bool flag2 = false;
				int num = ((sourceFrameCount == -1) ? Time.frameCount : sourceFrameCount);
				DeadlyDeadlyGoopManager deadlyDeadlyGoopManager;
				if (DeadlyDeadlyGoopManager.allGoopPositionMap.TryGetValue(pos, out deadlyDeadlyGoopManager))
				{
					DeadlyDeadlyGoopManager.GoopPositionData goopPositionData = deadlyDeadlyGoopManager.m_goopedCells[pos];
					if (goopPositionData.frameGooped > num)
					{
						return;
					}
					if (goopPositionData.eternal)
					{
						return;
					}
					if (goopPositionData.IsOnFire)
					{
						flag2 = true;
					}
					deadlyDeadlyGoopManager.RemoveGoopedPosition(pos);
				}
				DeadlyDeadlyGoopManager.GoopPositionData goopPositionData2 = new DeadlyDeadlyGoopManager.GoopPositionData(pos, this.m_goopedCells, this.goopDefinition.GetLifespan(radiusFraction));
				goopPositionData2.frameGooped = ((sourceFrameCount == -1) ? Time.frameCount : sourceFrameCount);
				goopPositionData2.lastSourceID = sourceId;
				if (!suppressSplashes && DeadlyDeadlyGoopManager.m_DoGoopSpawnSplashes && UnityEngine.Random.value < 0.02f)
				{
					if (this.m_genericSplashPrefab == null)
					{
						this.m_genericSplashPrefab = ResourceCache.Acquire("Global VFX/Generic_Goop_Splash") as GameObject;
					}
					GameObject gameObject = SpawnManager.SpawnVFX(this.m_genericSplashPrefab, vector.ToVector3ZUp(vector.y), Quaternion.identity);
					gameObject.GetComponent<tk2dBaseSprite>().usesOverrideMaterial = true;
					gameObject.GetComponent<Renderer>().material.SetColor(DeadlyDeadlyGoopManager.TintColorPropertyID, this.goopDefinition.baseColor32);
				}
				goopPositionData2.eternal = this.goopDefinition.eternal;
				goopPositionData2.selfIgnites = this.goopDefinition.SelfIgnites;
				goopPositionData2.remainingTimeTilSelfIgnition = this.goopDefinition.selfIgniteDelay;
				this.m_goopedPositions.Add(pos);
				this.m_goopedCells.Add(pos, goopPositionData2);
				DeadlyDeadlyGoopManager.allGoopPositionMap.Add(pos, this);
				RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector);
				absoluteRoomFromPosition.RegisterGoopManagerInRoom(this);
				if (cellData.OnCellGooped != null)
				{
					cellData.OnCellGooped(cellData);
				}
				if (cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Ice)
				{
					this.FreezeCell(pos);
				}
				if (flag2 && this.goopDefinition.CanBeIgnited)
				{
					this.IgniteCell(pos);
				}
				this.SetDirty(pos);
			}
		}
		else
		{
			if (this.m_goopedCells[pos].remainingLifespan < this.goopDefinition.fadePeriod)
			{
				this.SetDirty(pos);
			}
			if (this.m_goopedCells[pos].IsOnFire && this.goopDefinition.ignitionChangesLifetime)
			{
				if (this.m_goopedCells[pos].remainingLifespan > 0f)
				{
					this.m_goopedCells[pos].remainingLifespan = this.goopDefinition.ignitedLifetime;
				}
			}
			else
			{
				if (!suppressSplashes && DeadlyDeadlyGoopManager.m_DoGoopSpawnSplashes && (this.m_goopedCells[pos].lastSourceID < 0 || this.m_goopedCells[pos].lastSourceID != sourceId) && UnityEngine.Random.value < 0.001f)
				{
					if (this.m_genericSplashPrefab == null)
					{
						this.m_genericSplashPrefab = ResourceCache.Acquire("Global VFX/Generic_Goop_Splash") as GameObject;
					}
					GameObject gameObject2 = SpawnManager.SpawnVFX(this.m_genericSplashPrefab, vector.ToVector3ZUp(vector.y), Quaternion.identity);
					gameObject2.GetComponent<tk2dBaseSprite>().usesOverrideMaterial = true;
					gameObject2.GetComponent<Renderer>().material.SetColor(DeadlyDeadlyGoopManager.TintColorPropertyID, this.goopDefinition.baseColor32);
				}
				this.m_goopedCells[pos].remainingLifespan = Mathf.Max(this.m_goopedCells[pos].remainingLifespan, this.goopDefinition.GetLifespan(radiusFraction));
				this.m_goopedCells[pos].lifespanOverridden = true;
				this.m_goopedCells[pos].HasPlayedFireOutro = false;
				this.m_goopedCells[pos].hasBeenFrozen = 0;
			}
			this.m_goopedCells[pos].lastSourceID = sourceId;
		}
	}

	// Token: 0x060061B1 RID: 25009 RVA: 0x0025D7FC File Offset: 0x0025B9FC
	public void TimedAddGoopArc(Vector2 origin, float radius, float arcDegrees, Vector2 direction, float duration = 0.5f, AnimationCurve goopCurve = null)
	{
		base.StartCoroutine(this.TimedAddGoopArc_CR(origin, radius, arcDegrees, direction, duration, goopCurve));
	}

	// Token: 0x060061B2 RID: 25010 RVA: 0x0025D814 File Offset: 0x0025BA14
	private IEnumerator TimedAddGoopArc_CR(Vector2 origin, float radius, float arcDegrees, Vector2 direction, float duration, AnimationCurve goopCurve)
	{
		float elapsed = 0f;
		float m_lastRadius = 0f;
		int sourceFrameCount = Time.frameCount;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			if (goopCurve != null)
			{
				t = Mathf.Clamp01(goopCurve.Evaluate(t));
			}
			float currentRadius = Mathf.Lerp(0.5f, radius, t).Quantize(DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
			if (m_lastRadius != currentRadius)
			{
				m_lastRadius = currentRadius;
				float num = -1f * (arcDegrees / 2f);
				float num2 = 2f * currentRadius * 3.1415927f * (arcDegrees / 360f);
				int num3 = Mathf.CeilToInt(num2);
				for (int i = 0; i < num3 + 1; i++)
				{
					float num4 = num + arcDegrees * (float)i / (float)num3;
					Vector2 vector = origin + (Quaternion.Euler(0f, 0f, num4) * (direction * currentRadius).ToVector3ZUp(0f)).XY();
					this.AddGoopCircle(vector, 0.5f, -1, false, sourceFrameCount);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061B3 RID: 25011 RVA: 0x0025D85C File Offset: 0x0025BA5C
	public void TimedAddGoopCircle(Vector2 center, float radius, float duration = 0.5f, bool suppressSplashes = false)
	{
		base.StartCoroutine(this.TimedAddGoopCircle_CR(center, radius, duration, suppressSplashes));
	}

	// Token: 0x060061B4 RID: 25012 RVA: 0x0025D870 File Offset: 0x0025BA70
	private IEnumerator TimedAddGoopCircle_CR(Vector2 center, float radius, float duration, bool suppressSplashes = false)
	{
		float elapsed = 0f;
		float m_lastRadius = 0f;
		int sourceID = UnityEngine.Random.Range(1, 1000);
		int sourceFrameCount = Time.frameCount;
		float previousRadius = 0f;
		while (elapsed < duration)
		{
			if (GameManager.Instance.IsLoadingLevel)
			{
				yield break;
			}
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			t = 1f - (t - 1f) * (t - 1f);
			float currentRadius = Mathf.Lerp(0.5f, radius, t).Quantize(DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
			if (m_lastRadius != currentRadius)
			{
				m_lastRadius = currentRadius;
				this.AddGoopRing(center, previousRadius, currentRadius, sourceID, suppressSplashes, sourceFrameCount);
			}
			previousRadius = Mathf.Max(0f, currentRadius - 1f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061B5 RID: 25013 RVA: 0x0025D8A8 File Offset: 0x0025BAA8
	public void AddGoopCircle(Vector2 center, float radius, int sourceID = -1, bool suppressSplashes = false, int sourceFrameCount = -1)
	{
		int num = Mathf.FloorToInt((center.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.CeilToInt((center.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.FloorToInt((center.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.CeilToInt((center.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		Vector2 vector = center / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num5 = radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		suppressSplashes |= radius < 1f;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				float num6 = Vector2.Distance(new Vector2((float)i, (float)j), vector);
				if (num6 < num5)
				{
					float num7 = num6 / num5;
					if (num6 < DeadlyDeadlyGoopManager.GOOP_GRID_SIZE * 2f)
					{
						num7 = 0f;
					}
					num7 = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, num7);
					this.AddGoopedPosition(intVector, num7, suppressSplashes, sourceID, sourceFrameCount);
				}
			}
		}
	}

	// Token: 0x060061B6 RID: 25014 RVA: 0x0025D9C4 File Offset: 0x0025BBC4
	public void AddGoopRing(Vector2 center, float minRadius, float maxRadius, int sourceID = -1, bool suppressSplashes = false, int sourceFrameCount = -1)
	{
		int num = Mathf.FloorToInt((center.x - maxRadius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.CeilToInt((center.x + maxRadius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.FloorToInt((center.y - maxRadius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.CeilToInt((center.y + maxRadius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		Vector2 vector = center / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num5 = minRadius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num6 = maxRadius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		suppressSplashes |= num6 < 1f;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				float num7 = Vector2.Distance(new Vector2((float)i, (float)j), vector);
				if (num7 >= num5 && num7 <= num6)
				{
					float num8 = num7 / num6;
					if (num7 < DeadlyDeadlyGoopManager.GOOP_GRID_SIZE * 2f)
					{
						num8 = 0f;
					}
					num8 = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, num8);
					this.AddGoopedPosition(intVector, num8, suppressSplashes, sourceID, sourceFrameCount);
				}
			}
		}
	}

	// Token: 0x060061B7 RID: 25015 RVA: 0x0025DAF4 File Offset: 0x0025BCF4
	public void TimedAddGoopLine(Vector2 p1, Vector2 p2, float radius, float duration)
	{
		base.StartCoroutine(this.TimedAddGoopLine_CR(p1, p2, radius, duration));
	}

	// Token: 0x060061B8 RID: 25016 RVA: 0x0025DB08 File Offset: 0x0025BD08
	private IEnumerator TimedAddGoopLine_CR(Vector2 p1, Vector2 p2, float radius, float duration)
	{
		float elapsed = 0f;
		Vector2 lastEnd = p1;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			Vector2 currentEnd = Vector2.Lerp(p1, p2, elapsed / duration);
			float curDist = Vector2.Distance(currentEnd, lastEnd);
			int steps = Mathf.CeilToInt(curDist / radius);
			for (int i = 0; i < steps; i++)
			{
				Vector2 vector = lastEnd + (currentEnd - lastEnd) * (((float)i + 1f) / (float)steps);
				this.TimedAddGoopCircle(vector, radius, 0.5f, false);
			}
			lastEnd = currentEnd;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061B9 RID: 25017 RVA: 0x0025DB40 File Offset: 0x0025BD40
	public void AddGoopLine(Vector2 p1, Vector2 p2, float radius)
	{
		Vector2 vector = Vector2.Min(p1, p2);
		Vector2 vector2 = Vector2.Max(p1, p2);
		int num = Mathf.FloorToInt((vector.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.CeilToInt((vector2.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.FloorToInt((vector.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.CeilToInt((vector2.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		Vector2 vector3 = p1 / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		Vector2 vector4 = p2 / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num5 = radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		float num6 = num5 * num5;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				float num7 = (float)intVector.x - vector3.x;
				float num8 = (float)intVector.y - vector3.y;
				float num9 = vector4.x - vector3.x;
				float num10 = vector4.y - vector3.y;
				float num11 = num7 * num9 + num8 * num10;
				float num12 = num9 * num9 + num10 * num10;
				float num13 = -1f;
				if (num12 != 0f)
				{
					num13 = num11 / num12;
				}
				float num14;
				float num15;
				if (num13 < 0f)
				{
					num14 = vector3.x;
					num15 = vector3.y;
				}
				else if (num13 > 1f)
				{
					num14 = vector4.x;
					num15 = vector4.y;
				}
				else
				{
					num14 = vector3.x + num13 * num9;
					num15 = vector3.y + num13 * num10;
				}
				float num16 = (float)intVector.x - num14;
				float num17 = (float)intVector.y - num15;
				Vector2 vector5 = new Vector2(num16, num17);
				float sqrMagnitude = vector5.sqrMagnitude;
				if (sqrMagnitude < num6)
				{
					float num18 = Mathf.Sqrt(sqrMagnitude);
					float num19 = num18 / num5;
					if (num18 < DeadlyDeadlyGoopManager.GOOP_GRID_SIZE * 2f)
					{
						num19 = 0f;
					}
					num19 = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, num19);
					this.AddGoopedPosition(intVector, num19, false, -1, -1);
				}
			}
		}
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x0025DD74 File Offset: 0x0025BF74
	public void TimedAddGoopRect(Vector2 min, Vector2 max, float duration)
	{
		base.StartCoroutine(this.TimedAddGoopRect_CR(min, max, duration));
	}

	// Token: 0x060061BB RID: 25019 RVA: 0x0025DD88 File Offset: 0x0025BF88
	public IEnumerator TimedAddGoopRect_CR(Vector2 min, Vector2 max, float duration)
	{
		float elapsed = 0f;
		float lastT = 0f;
		while (elapsed < duration)
		{
			if (GameManager.Instance.IsLoadingLevel)
			{
				yield break;
			}
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			t = 1f - (t - 1f) * (t - 1f);
			int minGoopX = Mathf.FloorToInt(min.x / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
			float maxGoopX = Mathf.Lerp((float)minGoopX, (float)Mathf.CeilToInt(max.x / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE), t);
			int minGoopY = Mathf.FloorToInt(min.y / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
			float maxGoopY = Mathf.Lerp((float)minGoopY, (float)Mathf.CeilToInt(max.y / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE), t);
			float lastMaxX = Mathf.Lerp((float)minGoopX, (float)Mathf.CeilToInt(max.x / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE), lastT);
			float lastMaxY = Mathf.Lerp((float)minGoopY, (float)Mathf.CeilToInt(max.y / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE), lastT);
			int num = minGoopX;
			while ((float)num < maxGoopX)
			{
				int num2 = minGoopY;
				while ((float)num2 < maxGoopY)
				{
					if ((float)num > lastMaxX || (float)num2 > lastMaxY)
					{
						IntVector2 intVector = new IntVector2(num, num2);
						this.AddGoopedPosition(intVector, 0f, false, -1, -1);
					}
					num2++;
				}
				num++;
			}
			lastT = t;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x0025DDB8 File Offset: 0x0025BFB8
	public void AddGoopRect(Vector2 min, Vector2 max)
	{
		int num = Mathf.FloorToInt(min.x / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.CeilToInt(max.x / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.FloorToInt(min.y / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.CeilToInt(max.y / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				IntVector2 intVector = new IntVector2(i, j);
				this.AddGoopedPosition(intVector, 0f, false, -1, -1);
			}
		}
	}

	// Token: 0x060061BD RID: 25021 RVA: 0x0025DE58 File Offset: 0x0025C058
	public void AddGoopPoints(List<Vector2> points, float radius, Vector2 excludeCenter, float excludeRadius)
	{
		Vector2 vector = Vector2Extensions.max;
		Vector2 vector2 = Vector2Extensions.min;
		for (int i = 0; i < points.Count; i++)
		{
			vector = Vector2.Min(vector, points[i]);
			vector2 = Vector2.Max(vector2, points[i]);
		}
		int num = Mathf.FloorToInt((vector.x - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num2 = Mathf.CeilToInt((vector2.x + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num3 = Mathf.FloorToInt((vector.y - radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num4 = Mathf.CeilToInt((vector2.y + radius) / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		int num5 = num2 - num + 1;
		int num6 = num4 - num3 + 1;
		int num7 = Mathf.RoundToInt(radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		DeadlyDeadlyGoopManager.s_goopPointRadius = radius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE;
		DeadlyDeadlyGoopManager.s_goopPointRadiusSquare = DeadlyDeadlyGoopManager.s_goopPointRadius * DeadlyDeadlyGoopManager.s_goopPointRadius;
		DeadlyDeadlyGoopManager.m_pointsArray.ReinitializeWithDefault(num5, num6, false, 1f, false);
		for (int j = 0; j < points.Count; j++)
		{
			DeadlyDeadlyGoopManager.s_goopPointCenter.x = (int)(points[j].x / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE) - num;
			DeadlyDeadlyGoopManager.s_goopPointCenter.y = (int)(points[j].y / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE) - num3;
			DeadlyDeadlyGoopManager.m_pointsArray.SetCircle(DeadlyDeadlyGoopManager.s_goopPointCenter.x, DeadlyDeadlyGoopManager.s_goopPointCenter.y, num7, true, new SetBackingFloatFunc(DeadlyDeadlyGoopManager.GetRadiusFraction));
		}
		int num8 = (int)(excludeCenter.x / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE) - num;
		int num9 = (int)(excludeCenter.y / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE) - num3;
		int num10 = Mathf.RoundToInt(excludeRadius / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE);
		DeadlyDeadlyGoopManager.m_pointsArray.SetCircle(num8, num9, num10, false, new SetBackingFloatFunc(DeadlyDeadlyGoopManager.GetRadiusFraction));
		for (int k = 0; k < num5; k++)
		{
			for (int l = 0; l < num6; l++)
			{
				if (DeadlyDeadlyGoopManager.m_pointsArray[k, l])
				{
					this.AddGoopedPosition(new IntVector2(num + k, num3 + l), DeadlyDeadlyGoopManager.m_pointsArray.GetFloat(k, l), false, -1, -1);
				}
			}
		}
	}

	// Token: 0x060061BE RID: 25022 RVA: 0x0025E0B8 File Offset: 0x0025C2B8
	private static float GetRadiusFraction(int x, int y, bool value, float currentFloatValue)
	{
		if (!value)
		{
			return currentFloatValue;
		}
		float num = (float)(DeadlyDeadlyGoopManager.s_goopPointCenter.x - x);
		float num2 = (float)(DeadlyDeadlyGoopManager.s_goopPointCenter.y - y);
		float num3 = num * num + num2 * num2;
		if (num3 < DeadlyDeadlyGoopManager.s_goopPointRadiusSquare)
		{
			float num4 = Mathf.Sqrt(num3);
			float num5 = num4 / DeadlyDeadlyGoopManager.s_goopPointRadius;
			if (num4 < 0.5f)
			{
				num5 = 0f;
			}
			num5 = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, num5);
			return Mathf.Min(num5, currentFloatValue);
		}
		return currentFloatValue;
	}

	// Token: 0x04005C38 RID: 23608
	public static bool DrawDebugLines = false;

	// Token: 0x04005C39 RID: 23609
	public static Dictionary<IntVector2, DeadlyDeadlyGoopManager> allGoopPositionMap = new Dictionary<IntVector2, DeadlyDeadlyGoopManager>(new IntVector2EqualityComparer());

	// Token: 0x04005C3A RID: 23610
	public static List<Tuple<Vector2, float>> m_goopExceptions = new List<Tuple<Vector2, float>>();

	// Token: 0x04005C3B RID: 23611
	private static bool m_DoGoopSpawnSplashes = true;

	// Token: 0x04005C3C RID: 23612
	public static float GOOP_GRID_SIZE = 0.25f;

	// Token: 0x04005C3D RID: 23613
	public GoopDefinition goopDefinition;

	// Token: 0x04005C3E RID: 23614
	public float goopDepth = 1.5f;

	// Token: 0x04005C3F RID: 23615
	private HashSet<IntVector2> m_goopedPositions = new HashSet<IntVector2>();

	// Token: 0x04005C40 RID: 23616
	private Dictionary<IntVector2, DeadlyDeadlyGoopManager.GoopPositionData> m_goopedCells = new Dictionary<IntVector2, DeadlyDeadlyGoopManager.GoopPositionData>(new IntVector2EqualityComparer());

	// Token: 0x04005C41 RID: 23617
	private Dictionary<int, Vector2> m_uvMap;

	// Token: 0x04005C42 RID: 23618
	private Dictionary<GameActor, float> m_containedActors = new Dictionary<GameActor, float>();

	// Token: 0x04005C43 RID: 23619
	private List<Vector2> m_centerUVOptions = new List<Vector2>();

	// Token: 0x04005C44 RID: 23620
	private bool[,] m_dirtyFlags;

	// Token: 0x04005C45 RID: 23621
	private bool[,] m_colorDirtyFlags;

	// Token: 0x04005C46 RID: 23622
	private MeshRenderer[,] m_mrs;

	// Token: 0x04005C47 RID: 23623
	private Mesh[,] m_meshes;

	// Token: 0x04005C48 RID: 23624
	private Vector3[] m_vertexArray;

	// Token: 0x04005C49 RID: 23625
	private Vector2[] m_uvArray;

	// Token: 0x04005C4A RID: 23626
	private Vector2[] m_uv2Array;

	// Token: 0x04005C4B RID: 23627
	private Color32[] m_colorArray;

	// Token: 0x04005C4C RID: 23628
	private int[] m_triangleArray;

	// Token: 0x04005C4D RID: 23629
	private List<IntVector2> m_removalPositions = new List<IntVector2>();

	// Token: 0x04005C4E RID: 23630
	private int CHUNK_SIZE = 5;

	// Token: 0x04005C4F RID: 23631
	private bool m_isPlayingFireAudio;

	// Token: 0x04005C50 RID: 23632
	private bool m_isPlayingAcidAudio;

	// Token: 0x04005C51 RID: 23633
	private Shader m_shader;

	// Token: 0x04005C52 RID: 23634
	private Texture2D m_texture;

	// Token: 0x04005C53 RID: 23635
	private Texture2D m_worldTexture;

	// Token: 0x04005C54 RID: 23636
	private static int MainTexPropertyID = -1;

	// Token: 0x04005C55 RID: 23637
	private static int WorldTexPropertyID = -1;

	// Token: 0x04005C56 RID: 23638
	private static int OpaquenessMultiplyPropertyID = -1;

	// Token: 0x04005C57 RID: 23639
	private static int BrightnessMultiplyPropertyID = -1;

	// Token: 0x04005C58 RID: 23640
	private static int TintColorPropertyID = -1;

	// Token: 0x04005C59 RID: 23641
	private uint m_lastElecSemaphore;

	// Token: 0x04005C5A RID: 23642
	private ParticleSystem m_fireSystem;

	// Token: 0x04005C5B RID: 23643
	private ParticleSystem m_fireIntroSystem;

	// Token: 0x04005C5C RID: 23644
	private ParticleSystem m_fireOutroSystem;

	// Token: 0x04005C5D RID: 23645
	private ParticleSystem m_elecSystem;

	// Token: 0x04005C5E RID: 23646
	private int m_currentUpdateBin;

	// Token: 0x04005C5F RID: 23647
	private CircularBuffer<float> m_deltaTimes = new CircularBuffer<float>(4);

	// Token: 0x04005C60 RID: 23648
	private const bool c_CULL_MESHES = true;

	// Token: 0x04005C61 RID: 23649
	private const int UPDATE_EVERY_N_FRAMES = 3;

	// Token: 0x04005C62 RID: 23650
	public Color ElecColor0 = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04005C63 RID: 23651
	public Color ElecColor2 = new Color(1f, 1f, 10f, 1f);

	// Token: 0x04005C64 RID: 23652
	public float divFactor = 8.7f;

	// Token: 0x04005C65 RID: 23653
	public float tFactor = 4.2f;

	// Token: 0x04005C66 RID: 23654
	private GameObject m_genericSplashPrefab;

	// Token: 0x04005C67 RID: 23655
	private static BitArray2D m_pointsArray = new BitArray2D(true);

	// Token: 0x04005C68 RID: 23656
	private static IntVector2 s_goopPointCenter = new IntVector2(0, 0);

	// Token: 0x04005C69 RID: 23657
	private static float s_goopPointRadius;

	// Token: 0x04005C6A RID: 23658
	private static float s_goopPointRadiusSquare;

	// Token: 0x0200113C RID: 4412
	private class GoopPositionData
	{
		// Token: 0x060061C0 RID: 25024 RVA: 0x0025E1B0 File Offset: 0x0025C3B0
		public GoopPositionData(IntVector2 position, Dictionary<IntVector2, DeadlyDeadlyGoopManager.GoopPositionData> goopData, float lifespan)
		{
			this.goopPosition = position;
			this.neighborGoopData = new DeadlyDeadlyGoopManager.GoopPositionData[8];
			this.remainingLifespan = lifespan;
			IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
			for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
			{
				IntVector2 intVector = position + cardinalsAndOrdinals[i];
				DeadlyDeadlyGoopManager.GoopPositionData goopPositionData;
				if (goopData.TryGetValue(intVector, out goopPositionData))
				{
					goopPositionData.neighborGoopData[(i + 4) % 8] = this;
					this.neighborGoopData[i] = goopData[intVector];
					goopPositionData.SetNeighborGoop((i + 4) % 8, true);
					this.SetNeighborGoop(i, true);
				}
			}
			this.GoopUpdateBin = UnityEngine.Random.Range(0, 4);
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x060061C1 RID: 25025 RVA: 0x0025E26C File Offset: 0x0025C46C
		public bool SupportsAmbientVFX
		{
			get
			{
				return this.NeighborsAsInt == 255 && this.remainingLifespan > 2f;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x060061C2 RID: 25026 RVA: 0x0025E290 File Offset: 0x0025C490
		public bool HasOnFireNeighbor
		{
			get
			{
				for (int i = 0; i < 8; i++)
				{
					if (this.neighborGoopData[i] != null && this.neighborGoopData[i].IsOnFire)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x060061C3 RID: 25027 RVA: 0x0025E2D4 File Offset: 0x0025C4D4
		public bool HasFrozenNeighbor
		{
			get
			{
				for (int i = 0; i < 8; i++)
				{
					if (this.neighborGoopData[i] != null && this.neighborGoopData[i].IsFrozen)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x060061C4 RID: 25028 RVA: 0x0025E318 File Offset: 0x0025C518
		public bool HasNonFrozenNeighbor
		{
			get
			{
				for (int i = 0; i < 8; i++)
				{
					if (this.neighborGoopData[i] == null || (!this.neighborGoopData[i].IsFrozen && !this.neighborGoopData[i].unfrozeLastFrame))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x0025E36C File Offset: 0x0025C56C
		public void SetNeighborGoop(int index, bool value)
		{
			int num = 1 << 7 - index;
			if (value)
			{
				this.NeighborsAsInt |= num;
			}
			else
			{
				this.NeighborsAsInt &= ~num;
			}
			this.NeighborsAsIntFuckDiagonals = this.NeighborsAsInt & 170;
		}

		// Token: 0x04005C6D RID: 23661
		public IntVector2 goopPosition;

		// Token: 0x04005C6E RID: 23662
		public DeadlyDeadlyGoopManager.GoopPositionData[] neighborGoopData;

		// Token: 0x04005C6F RID: 23663
		public bool IsOnFire;

		// Token: 0x04005C70 RID: 23664
		public bool IsElectrified;

		// Token: 0x04005C71 RID: 23665
		public bool IsFrozen;

		// Token: 0x04005C72 RID: 23666
		public bool HasPlayedFireIntro;

		// Token: 0x04005C73 RID: 23667
		public bool HasPlayedFireOutro;

		// Token: 0x04005C74 RID: 23668
		public bool lifespanOverridden;

		// Token: 0x04005C75 RID: 23669
		public int lastSourceID = -1;

		// Token: 0x04005C76 RID: 23670
		public int frameGooped = -1;

		// Token: 0x04005C77 RID: 23671
		public float remainingLifespan;

		// Token: 0x04005C78 RID: 23672
		public float remainingFreezeTimer;

		// Token: 0x04005C79 RID: 23673
		public int hasBeenFrozen;

		// Token: 0x04005C7A RID: 23674
		public bool unfrozeLastFrame;

		// Token: 0x04005C7B RID: 23675
		public bool eternal;

		// Token: 0x04005C7C RID: 23676
		public bool selfIgnites;

		// Token: 0x04005C7D RID: 23677
		public float remainingTimeTilSelfIgnition;

		// Token: 0x04005C7E RID: 23678
		public float remainingElectrifiedTime;

		// Token: 0x04005C7F RID: 23679
		public float remainingElecTimer;

		// Token: 0x04005C80 RID: 23680
		public uint elecTriggerSemaphore;

		// Token: 0x04005C81 RID: 23681
		public float remainingFireTimer;

		// Token: 0x04005C82 RID: 23682
		public float totalOnFireTime;

		// Token: 0x04005C83 RID: 23683
		public float totalFrozenTime;

		// Token: 0x04005C84 RID: 23684
		public int baseIndex = -1;

		// Token: 0x04005C85 RID: 23685
		public int NeighborsAsInt;

		// Token: 0x04005C86 RID: 23686
		public int NeighborsAsIntFuckDiagonals;

		// Token: 0x04005C87 RID: 23687
		public int GoopUpdateBin;
	}

	// Token: 0x0200113D RID: 4413
	private enum VertexColorRebuildResult
	{
		// Token: 0x04005C89 RID: 23689
		ALL_OK,
		// Token: 0x04005C8A RID: 23690
		ELECTRIFIED
	}
}
