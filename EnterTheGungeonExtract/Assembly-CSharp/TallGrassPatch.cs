using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001233 RID: 4659
public class TallGrassPatch : MonoBehaviour
{
	// Token: 0x0600686A RID: 26730 RVA: 0x0028DFD0 File Offset: 0x0028C1D0
	private void InitializeParticleSystem()
	{
		if (this.m_fireSystem != null)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("Gungeon_Fire_Main");
		if (gameObject == null)
		{
			gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Particles/Gungeon_Fire_Main_raw", ".prefab"), Vector3.zero, Quaternion.identity);
			gameObject.name = "Gungeon_Fire_Main";
		}
		this.m_fireSystem = gameObject.GetComponent<ParticleSystem>();
		GameObject gameObject2 = GameObject.Find("Gungeon_Fire_Intro");
		if (gameObject2 == null)
		{
			gameObject2 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Particles/Gungeon_Fire_Intro_raw", ".prefab"), Vector3.zero, Quaternion.identity);
			gameObject2.name = "Gungeon_Fire_Intro";
		}
		this.m_fireIntroSystem = gameObject2.GetComponent<ParticleSystem>();
		GameObject gameObject3 = GameObject.Find("Gungeon_Fire_Outro");
		if (gameObject3 == null)
		{
			gameObject3 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Particles/Gungeon_Fire_Outro_raw", ".prefab"), Vector3.zero, Quaternion.identity);
			gameObject3.name = "Gungeon_Fire_Outro";
		}
		this.m_fireOutroSystem = gameObject3.GetComponent<ParticleSystem>();
	}

	// Token: 0x0600686B RID: 26731 RVA: 0x0028E0E8 File Offset: 0x0028C2E8
	private int GetTargetIndexForPosition(IntVector2 current)
	{
		bool flag = this.cells.Contains(current + IntVector2.North);
		bool flag2 = this.cells.Contains(current + IntVector2.South);
		bool flag3 = this.cells.Contains(current + IntVector2.South + IntVector2.South);
		int num;
		if (flag && flag2 && flag3)
		{
			num = 147;
		}
		else if (flag && flag2)
		{
			num = 146;
		}
		else if (flag && !flag2)
		{
			num = 168;
		}
		else if (!flag && flag2)
		{
			num = 124;
		}
		else
		{
			num = 168;
		}
		return num;
	}

	// Token: 0x0600686C RID: 26732 RVA: 0x0028E1AC File Offset: 0x0028C3AC
	public void IgniteCircle(Vector2 center, float radius)
	{
		for (int i = Mathf.FloorToInt(center.x - radius); i < Mathf.CeilToInt(center.x + radius); i++)
		{
			for (int j = Mathf.FloorToInt(center.y - radius); j < Mathf.CeilToInt(center.y + radius); j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), center) < radius)
				{
					this.IgniteCell(new IntVector2(i, j));
				}
			}
		}
	}

	// Token: 0x0600686D RID: 26733 RVA: 0x0028E234 File Offset: 0x0028C434
	public void IgniteCell(IntVector2 cellPosition)
	{
		if (this.cells.Contains(cellPosition))
		{
			if (this.m_fireData.ContainsKey(cellPosition))
			{
				return;
			}
			TallGrassPatch.EnflamedGrassData enflamedGrassData = default(TallGrassPatch.EnflamedGrassData);
			this.m_fireData.Add(cellPosition, enflamedGrassData);
		}
	}

	// Token: 0x0600686E RID: 26734 RVA: 0x0028E27C File Offset: 0x0028C47C
	private TallGrassPatch.EnflamedGrassData DoParticleAtPosition(IntVector2 worldPos, TallGrassPatch.EnflamedGrassData fireData)
	{
		if (this.m_fireSystem != null && fireData.ParticleTimer <= 0f)
		{
			bool flag = this.cells.Contains(worldPos + IntVector2.South);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					if (flag || j != 0)
					{
						float num = UnityEngine.Random.Range(1f, 1.5f);
						float num2 = UnityEngine.Random.Range(0.75f, 1f);
						Vector2 vector = worldPos.ToVector3() + new Vector3(0.33f + 0.33f * (float)i, 0.33f + 0.33f * (float)j, 0f);
						vector += UnityEngine.Random.insideUnitCircle / 5f;
						if (!fireData.HasPlayedFireOutro)
						{
							if (!fireData.HasPlayedFireOutro && fireData.fireTime > 3f && this.m_fireOutroSystem != null)
							{
								num = num2;
								ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
								{
									position = vector,
									velocity = Vector3.zero,
									startSize = this.m_fireSystem.startSize,
									startLifetime = num2,
									startColor = this.m_fireSystem.startColor,
									randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
								};
								this.m_fireOutroSystem.Emit(emitParams, 1);
								if (i == 1 && j == 1)
								{
									fireData.HasPlayedFireOutro = true;
								}
							}
							else if (!fireData.HasPlayedFireIntro && this.m_fireIntroSystem != null)
							{
								num = UnityEngine.Random.Range(0.75f, 1f);
								ParticleSystem.EmitParams emitParams2 = new ParticleSystem.EmitParams
								{
									position = vector,
									velocity = Vector3.zero,
									startSize = this.m_fireSystem.startSize,
									startLifetime = num,
									startColor = this.m_fireSystem.startColor,
									randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
								};
								this.m_fireIntroSystem.Emit(emitParams2, 1);
								if (i == 1 && j == 1)
								{
									fireData.HasPlayedFireIntro = true;
								}
							}
							else if (UnityEngine.Random.value < 0.5f)
							{
								ParticleSystem.EmitParams emitParams3 = new ParticleSystem.EmitParams
								{
									position = vector,
									velocity = Vector3.zero,
									startSize = this.m_fireSystem.startSize,
									startLifetime = num,
									startColor = this.m_fireSystem.startColor,
									randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
								};
								this.m_fireSystem.Emit(emitParams3, 1);
							}
						}
						if (i == 1 && j == 1)
						{
							fireData.ParticleTimer = num - 0.125f;
						}
					}
				}
			}
		}
		return fireData;
	}

	// Token: 0x0600686F RID: 26735 RVA: 0x0028E5A4 File Offset: 0x0028C7A4
	private void LateUpdate()
	{
		bool flag = false;
		for (int i = 0; i < this.cells.Count; i++)
		{
			if (this.m_fireData.ContainsKey(this.cells[i]))
			{
				TallGrassPatch.EnflamedGrassData enflamedGrassData = this.m_fireData[this.cells[i]];
				enflamedGrassData.fireTime += BraveTime.DeltaTime;
				enflamedGrassData.ParticleTimer -= BraveTime.DeltaTime;
				if (!this.m_fireData[this.cells[i]].hasEnflamedNeighbors && this.m_fireData[this.cells[i]].fireTime > 0.1f)
				{
					this.IgniteCell(this.cells[i] + IntVector2.North);
					this.IgniteCell(this.cells[i] + IntVector2.East);
					this.IgniteCell(this.cells[i] + IntVector2.South);
					this.IgniteCell(this.cells[i] + IntVector2.West);
					enflamedGrassData.hasEnflamedNeighbors = true;
				}
				if (enflamedGrassData.HasPlayedFireOutro && enflamedGrassData.ParticleTimer <= 0f)
				{
					this.RemovePosition(this.cells[i]);
					i--;
				}
				else
				{
					enflamedGrassData = this.DoParticleAtPosition(this.cells[i], enflamedGrassData);
					this.m_fireData[this.cells[i]] = enflamedGrassData;
				}
			}
		}
		if (flag && !this.m_isPlayingFireAudio)
		{
			this.m_isPlayingFireAudio = true;
			AkSoundEngine.PostEvent("Play_ENV_oilfire_ignite_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
	}

	// Token: 0x06006870 RID: 26736 RVA: 0x0028E788 File Offset: 0x0028C988
	private void RemovePosition(IntVector2 pos)
	{
		if (this.cells.Contains(pos))
		{
			this.cells.Remove(pos);
			this.BuildPatch();
		}
	}

	// Token: 0x06006871 RID: 26737 RVA: 0x0028E7B0 File Offset: 0x0028C9B0
	public void BuildPatch()
	{
		for (int i = 0; i < this.m_tiledSpritePool.Count; i++)
		{
			SpawnManager.Despawn(this.m_tiledSpritePool[i].gameObject);
			this.m_tiledSpritePool.RemoveAt(i);
			i--;
		}
		if (this.m_stripPrefab == null)
		{
			this.m_stripPrefab = (GameObject)BraveResources.Load("Global Prefabs/TallGrassStrip", ".prefab");
		}
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		for (int j = 0; j < this.cells.Count; j++)
		{
			IntVector2 intVector = this.cells[j];
			if (!hashSet.Contains(intVector))
			{
				hashSet.Add(intVector);
				int num = 1;
				int targetIndexForPosition = this.GetTargetIndexForPosition(intVector);
				IntVector2 intVector2 = intVector;
				for (;;)
				{
					intVector2 += IntVector2.Right;
					if (hashSet.Contains(intVector2))
					{
						break;
					}
					if (!this.cells.Contains(intVector2))
					{
						break;
					}
					if (targetIndexForPosition != this.GetTargetIndexForPosition(intVector2))
					{
						break;
					}
					num++;
					hashSet.Add(intVector2);
				}
				IL_114:
				GameObject gameObject = SpawnManager.SpawnVFX(this.m_stripPrefab, false);
				tk2dTiledSprite component = gameObject.GetComponent<tk2dTiledSprite>();
				component.SetSprite(GameManager.Instance.Dungeon.tileIndices.dungeonCollection, targetIndexForPosition);
				component.dimensions = new Vector2((float)(16 * num), 16f);
				gameObject.transform.position = new Vector3((float)intVector.x, (float)intVector.y, (float)intVector.y);
				this.m_tiledSpritePool.Add(component);
				if (targetIndexForPosition == 168)
				{
					component.HeightOffGround = -2f;
					component.IsPerpendicular = true;
					component.transform.position += new Vector3(0f, 0.6875f, 0f);
				}
				else if (targetIndexForPosition == 124)
				{
					component.IsPerpendicular = true;
				}
				else
				{
					component.IsPerpendicular = false;
				}
				component.UpdateZDepth();
				goto IL_20A;
				goto IL_114;
			}
			IL_20A:;
		}
		if (!StaticReferenceManager.AllGrasses.Contains(this))
		{
			StaticReferenceManager.AllGrasses.Add(this);
		}
		this.InitializeParticleSystem();
	}

	// Token: 0x040064A2 RID: 25762
	[NonSerialized]
	public List<IntVector2> cells;

	// Token: 0x040064A3 RID: 25763
	private const int INDEX_TOP = 124;

	// Token: 0x040064A4 RID: 25764
	private const int INDEX_MIDDLE = 147;

	// Token: 0x040064A5 RID: 25765
	private const int INDEX_MIDDLE_BOTTOM = 146;

	// Token: 0x040064A6 RID: 25766
	private const int INDEX_BOTTOM = 168;

	// Token: 0x040064A7 RID: 25767
	private Dictionary<IntVector2, TallGrassPatch.EnflamedGrassData> m_fireData = new Dictionary<IntVector2, TallGrassPatch.EnflamedGrassData>(new IntVector2EqualityComparer());

	// Token: 0x040064A8 RID: 25768
	private ParticleSystem m_fireSystem;

	// Token: 0x040064A9 RID: 25769
	private ParticleSystem m_fireIntroSystem;

	// Token: 0x040064AA RID: 25770
	private ParticleSystem m_fireOutroSystem;

	// Token: 0x040064AB RID: 25771
	private List<tk2dTiledSprite> m_tiledSpritePool = new List<tk2dTiledSprite>();

	// Token: 0x040064AC RID: 25772
	private bool m_isPlayingFireAudio;

	// Token: 0x040064AD RID: 25773
	private GameObject m_stripPrefab;

	// Token: 0x02001234 RID: 4660
	internal struct EnflamedGrassData
	{
		// Token: 0x040064AE RID: 25774
		public float fireTime;

		// Token: 0x040064AF RID: 25775
		public bool hasEnflamedNeighbors;

		// Token: 0x040064B0 RID: 25776
		public bool HasPlayedFireOutro;

		// Token: 0x040064B1 RID: 25777
		public bool HasPlayedFireIntro;

		// Token: 0x040064B2 RID: 25778
		public float ParticleTimer;
	}
}
