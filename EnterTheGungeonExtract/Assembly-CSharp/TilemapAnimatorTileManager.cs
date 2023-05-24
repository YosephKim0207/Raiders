using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200123F RID: 4671
public class TilemapAnimatorTileManager
{
	// Token: 0x060068AD RID: 26797 RVA: 0x0028FC10 File Offset: 0x0028DE10
	public TilemapAnimatorTileManager(tk2dSpriteCollectionData sourceCollection, int sourceSpriteId, TilesetIndexMetadata metadata, int uvStart, int numUV, tk2dTileMap tilemap)
	{
		this.associatedCollection = sourceCollection;
		this.associatedSpriteId = sourceSpriteId;
		this.associatedMetadata = metadata;
		this.associatedSequence = this.associatedCollection.GetAnimationSequence(this.associatedSpriteId);
		if (this.associatedSequence.playstyle == SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.LOOPCEPTION)
		{
			this.loopceptionSequence = tilemap.SpriteCollectionInst.GetAnimationSequence(this.associatedSequence.loopceptionTarget);
		}
		this.startUVIndex = uvStart;
		this.uvCount = numUV;
		this.m_elapsed = 0f;
		this.m_cachedSequenceLength = 0f;
		for (int i = 0; i < this.associatedSequence.entries.Count; i++)
		{
			this.m_cachedSequenceLength += this.associatedSequence.entries[i].frameTime;
		}
		if (this.associatedSequence.playstyle == SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.LOOPCEPTION)
		{
			for (int j = 0; j < this.loopceptionSequence.entries.Count; j++)
			{
				this.m_cachedLoopceptionLength += this.loopceptionSequence.entries[j].frameTime;
			}
		}
		if (this.associatedSequence.randomStartFrame)
		{
			this.m_elapsed = Mathf.Lerp(0f, this.m_cachedSequenceLength, UnityEngine.Random.value);
		}
	}

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x060068AE RID: 26798 RVA: 0x0028FD74 File Offset: 0x0028DF74
	public int CurrentFrame
	{
		get
		{
			return this.m_lastTargetEntry;
		}
	}

	// Token: 0x060068AF RID: 26799 RVA: 0x0028FD7C File Offset: 0x0028DF7C
	protected void UpdateChildSectionInternal(Vector2[] refMeshUVs, tk2dTileMap tileMap, int targetEntryIndex)
	{
		SimpleTilesetAnimationSequence simpleTilesetAnimationSequence = ((!this.m_isLoopcepting) ? this.associatedSequence : this.loopceptionSequence);
		this.m_lastTargetEntry = targetEntryIndex;
		SimpleTilesetAnimationSequenceEntry simpleTilesetAnimationSequenceEntry = simpleTilesetAnimationSequence.entries[targetEntryIndex];
		tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[simpleTilesetAnimationSequenceEntry.entryIndex];
		for (int i = this.startUVIndex; i < this.startUVIndex + this.uvCount; i++)
		{
			refMeshUVs[i] = tk2dSpriteDefinition.uvs[i - this.startUVIndex];
		}
	}

	// Token: 0x060068B0 RID: 26800 RVA: 0x0028FE14 File Offset: 0x0028E014
	public void TriggerAnimationSequence()
	{
		if (!this.m_triggered)
		{
			this.m_elapsed = 0f;
			this.m_triggered = true;
		}
	}

	// Token: 0x060068B1 RID: 26801 RVA: 0x0028FE34 File Offset: 0x0028E034
	public void UntriggerAnimationSequence()
	{
		if (this.m_triggered)
		{
			this.m_elapsed = 0f;
			this.m_triggered = false;
			this.m_forceNextUpdate = true;
		}
	}

	// Token: 0x060068B2 RID: 26802 RVA: 0x0028FE5C File Offset: 0x0028E05C
	public bool UpdateRelevantSection(ref Vector2[] refMeshUVs, Mesh refMesh, tk2dTileMap tileMap, float deltaTime)
	{
		if (!this.m_forceNextUpdate)
		{
			if (this.associatedSequence.playstyle == SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.TRIGGERED_ONCE && !this.m_triggered)
			{
				return false;
			}
			if (this.associatedSequence.playstyle == SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.TRIGGERED_ONCE && this.m_elapsed > this.m_cachedSequenceLength)
			{
				return false;
			}
		}
		this.m_forceNextUpdate = false;
		float num = deltaTime;
		if (this.m_delayRemaining > 0f)
		{
			if (deltaTime <= this.m_delayRemaining)
			{
				this.m_delayRemaining -= deltaTime;
				return false;
			}
			num = deltaTime - this.m_delayRemaining;
			this.m_delayRemaining = 0f;
		}
		if (this.associatedSequence.playstyle == SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.TRIGGERED_ONCE && !this.m_triggered)
		{
			this.m_elapsed = 0f;
		}
		else
		{
			this.m_elapsed += num;
		}
		if (this.m_elapsed >= this.m_cachedSequenceLength && this.associatedSequence.playstyle == SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.DELAYED_LOOP)
		{
			float num2 = Mathf.Lerp(this.associatedSequence.loopDelayMin, this.associatedSequence.loopDelayMax, UnityEngine.Random.value);
			this.m_delayRemaining = num2 - this.m_elapsed % this.m_cachedSequenceLength;
			this.m_elapsed = 0f;
			return false;
		}
		if (this.associatedSequence.playstyle == SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.LOOPCEPTION)
		{
			if (!this.m_isLoopcepting && this.m_elapsed >= this.m_cachedSequenceLength)
			{
				if (this.m_loopceptionLoopsRemaining > 0)
				{
					this.m_loopceptionLoopsRemaining--;
					this.m_elapsed %= this.m_cachedSequenceLength;
				}
				else
				{
					this.m_isLoopcepting = true;
					this.m_elapsed %= this.m_cachedSequenceLength;
					this.m_loopceptionLoopsRemaining = UnityEngine.Random.Range(this.associatedSequence.loopceptionMin, this.associatedSequence.loopceptionMax);
				}
			}
			else if (this.m_isLoopcepting && this.m_elapsed >= this.m_cachedLoopceptionLength)
			{
				if (this.m_loopceptionLoopsRemaining > 0)
				{
					this.m_loopceptionLoopsRemaining--;
					this.m_elapsed %= this.m_cachedLoopceptionLength;
				}
				else
				{
					this.m_isLoopcepting = false;
					this.m_elapsed %= this.m_cachedLoopceptionLength;
					this.m_loopceptionLoopsRemaining = UnityEngine.Random.Range(this.associatedSequence.coreceptionMin, this.associatedSequence.coreceptionMax);
				}
			}
		}
		else if (this.associatedSequence.playstyle != SimpleTilesetAnimationSequence.TilesetSequencePlayStyle.TRIGGERED_ONCE)
		{
			this.m_elapsed %= this.m_cachedSequenceLength;
		}
		this.m_elapsed = Mathf.Clamp(this.m_elapsed, 0f, this.m_cachedSequenceLength);
		SimpleTilesetAnimationSequence simpleTilesetAnimationSequence = ((!this.m_isLoopcepting) ? this.associatedSequence : this.loopceptionSequence);
		float num3 = 0f;
		int i;
		for (i = 0; i < simpleTilesetAnimationSequence.entries.Count; i++)
		{
			num3 += simpleTilesetAnimationSequence.entries[i].frameTime;
			if (num3 >= this.m_elapsed)
			{
				break;
			}
		}
		if (i == this.m_lastTargetEntry)
		{
			return false;
		}
		this.m_lastTargetEntry = i;
		SimpleTilesetAnimationSequenceEntry simpleTilesetAnimationSequenceEntry = simpleTilesetAnimationSequence.entries[i];
		tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[simpleTilesetAnimationSequenceEntry.entryIndex];
		for (int j = this.startUVIndex; j < this.startUVIndex + this.uvCount; j++)
		{
			refMeshUVs[j] = tk2dSpriteDefinition.uvs[j - this.startUVIndex];
		}
		for (int k = 0; k < this.linkedManagers.Count; k++)
		{
			this.linkedManagers[k].UpdateChildSectionInternal(refMeshUVs, tileMap, i);
		}
		return true;
	}

	// Token: 0x040064F2 RID: 25842
	public tk2dSpriteCollectionData associatedCollection;

	// Token: 0x040064F3 RID: 25843
	public int associatedSpriteId;

	// Token: 0x040064F4 RID: 25844
	public TilesetIndexMetadata associatedMetadata;

	// Token: 0x040064F5 RID: 25845
	public SimpleTilesetAnimationSequence associatedSequence;

	// Token: 0x040064F6 RID: 25846
	public SimpleTilesetAnimationSequence loopceptionSequence;

	// Token: 0x040064F7 RID: 25847
	private bool m_isLoopcepting;

	// Token: 0x040064F8 RID: 25848
	public IntVector2 worldPosition;

	// Token: 0x040064F9 RID: 25849
	public TK2DTilemapChunkAnimator animator;

	// Token: 0x040064FA RID: 25850
	public List<TilemapAnimatorTileManager> linkedManagers = new List<TilemapAnimatorTileManager>();

	// Token: 0x040064FB RID: 25851
	public int startUVIndex;

	// Token: 0x040064FC RID: 25852
	public int uvCount;

	// Token: 0x040064FD RID: 25853
	private float m_delayRemaining;

	// Token: 0x040064FE RID: 25854
	private float m_elapsed;

	// Token: 0x040064FF RID: 25855
	private float m_cachedSequenceLength;

	// Token: 0x04006500 RID: 25856
	private float m_cachedLoopceptionLength;

	// Token: 0x04006501 RID: 25857
	private int m_lastTargetEntry;

	// Token: 0x04006502 RID: 25858
	private bool m_triggered;

	// Token: 0x04006503 RID: 25859
	private bool m_forceNextUpdate;

	// Token: 0x04006504 RID: 25860
	private int m_loopceptionLoopsRemaining;
}
