using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011A0 RID: 4512
public class InvisibleBridgeMaker : DungeonPlaceableBehaviour
{
	// Token: 0x06006464 RID: 25700 RVA: 0x0026E73C File Offset: 0x0026C93C
	private void Start()
	{
		this.RegenerateBridge();
		GameManager.Instance.PrimaryPlayer.OnReceivedDamage += this.HandlePlayerDamaged;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.SecondaryPlayer.OnReceivedDamage += this.HandlePlayerDamaged;
		}
	}

	// Token: 0x06006465 RID: 25701 RVA: 0x0026E798 File Offset: 0x0026C998
	protected override void OnDestroy()
	{
		if (!GameManager.HasInstance)
		{
			return;
		}
		if (GameManager.Instance.PrimaryPlayer)
		{
			GameManager.Instance.PrimaryPlayer.OnReceivedDamage -= this.HandlePlayerDamaged;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
		{
			GameManager.Instance.SecondaryPlayer.OnReceivedDamage -= this.HandlePlayerDamaged;
		}
		base.OnDestroy();
	}

	// Token: 0x06006466 RID: 25702 RVA: 0x0026E824 File Offset: 0x0026CA24
	private void HandlePlayerDamaged(PlayerController obj)
	{
		if (obj.CurrentRoom == base.GetAbsoluteParentRoom())
		{
			this.RegenerateBridge();
		}
	}

	// Token: 0x06006467 RID: 25703 RVA: 0x0026E840 File Offset: 0x0026CA40
	private void AddPlatformPosition(IntVector2 position, List<IntVector2> points, HashSet<IntVector2> positions)
	{
		positions.Add(position);
		positions.Add(position + IntVector2.Up);
		positions.Add(position + IntVector2.Right);
		positions.Add(position + IntVector2.One);
		points.Add(position);
	}

	// Token: 0x06006468 RID: 25704 RVA: 0x0026E894 File Offset: 0x0026CA94
	private IntVector2 RotateDir(IntVector2 curDir)
	{
		if (curDir.x > 0)
		{
			if (UnityEngine.Random.value < 0.5f)
			{
				return IntVector2.Up;
			}
			return IntVector2.Down;
		}
		else if (curDir.y < 0)
		{
			if (UnityEngine.Random.value < 0.5f)
			{
				return IntVector2.Right;
			}
			return IntVector2.Right;
		}
		else if (curDir.y > 0)
		{
			if (UnityEngine.Random.value < 0.5f)
			{
				return IntVector2.Right;
			}
			return IntVector2.Right;
		}
		else
		{
			if (curDir.x >= 0)
			{
				return curDir;
			}
			if (UnityEngine.Random.value < 0.5f)
			{
				return IntVector2.Up;
			}
			return IntVector2.Down;
		}
	}

	// Token: 0x06006469 RID: 25705 RVA: 0x0026E944 File Offset: 0x0026CB44
	private bool IsPositionValid(IntVector2 testPosition, IntVector2 minPosition, IntVector2 maxPosition, HashSet<IntVector2> usedPositions)
	{
		return !usedPositions.Contains(testPosition) && testPosition.y >= minPosition.y && testPosition.y < maxPosition.y;
	}

	// Token: 0x0600646A RID: 25706 RVA: 0x0026E97C File Offset: 0x0026CB7C
	private MovingPlatform CreateNewPlatform(IntVector2 position)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.InvisiblePlatform2x2.gameObject, position.ToVector3(), Quaternion.identity);
		gameObject.GetComponent<SpeculativeRigidbody>().Reinitialize();
		return gameObject.GetComponent<MovingPlatform>();
	}

	// Token: 0x0600646B RID: 25707 RVA: 0x0026E9B8 File Offset: 0x0026CBB8
	private void RegenerateBridge()
	{
		for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
		{
			if (StaticReferenceManager.AllDebris[i].transform.position.GetAbsoluteRoom() == base.GetAbsoluteParentRoom())
			{
				StaticReferenceManager.AllDebris[i].ForceUpdatePitfall();
			}
		}
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		IntVector2 intVector2 = intVector + new IntVector2(this.GetWidth(), this.GetHeight());
		int num = UnityEngine.Random.Range(intVector.y, intVector2.y - 1);
		List<IntVector2> list = new List<IntVector2>();
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		IntVector2 intVector3 = new IntVector2(intVector.x, num);
		IntVector2 intVector4 = IntVector2.Right;
		this.AddPlatformPosition(intVector3, list, hashSet);
		int num2 = 0;
		while (intVector2.x - intVector3.x > 3)
		{
			IntVector2 intVector5 = intVector3 + intVector4 + intVector4;
			if (!this.IsPositionValid(intVector5, intVector, intVector2, hashSet))
			{
				intVector4 = IntVector2.Right;
				intVector5 = intVector3 + intVector4 + intVector4;
				num2 = 0;
			}
			this.AddPlatformPosition(intVector5, list, hashSet);
			num2++;
			if (num2 > 2 && UnityEngine.Random.value < 0.5f)
			{
				IntVector2 intVector6 = this.RotateDir(intVector4);
				if (!this.IsPositionValid(intVector5 + intVector6 + intVector6, intVector, intVector2, hashSet))
				{
					intVector6 = -1 * intVector6;
				}
				if (!this.IsPositionValid(intVector5 + intVector6 + intVector6, intVector, intVector2, hashSet))
				{
					intVector6 = intVector4;
				}
				if (intVector6 != intVector4)
				{
					num2 = 0;
				}
				intVector4 = intVector6;
			}
			intVector3 = intVector5;
		}
		for (int j = 0; j < this.m_extantPlatforms.Count; j++)
		{
			this.m_extantPlatforms[j].ClearCells();
			UnityEngine.Object.Destroy(this.m_extantPlatforms[j].gameObject);
		}
		this.m_extantPlatforms.Clear();
		for (int k = 0; k < list.Count; k++)
		{
			if (k >= this.m_extantPlatforms.Count)
			{
				this.m_extantPlatforms.Add(this.CreateNewPlatform(list[k]));
			}
			else
			{
				this.m_extantPlatforms[k].ClearCells();
				this.m_extantPlatforms[k].transform.position = list[k].ToVector3();
				this.m_extantPlatforms[k].specRigidbody.Reinitialize();
				this.m_extantPlatforms[k].MarkCells();
			}
		}
		if (this.m_extantPlatforms.Count > list.Count)
		{
			for (int l = list.Count; l < this.m_extantPlatforms.Count; l++)
			{
				this.m_extantPlatforms[l].ClearCells();
				UnityEngine.Object.Destroy(this.m_extantPlatforms[l].gameObject);
				this.m_extantPlatforms.RemoveAt(l);
				l--;
			}
		}
	}

	// Token: 0x04005FF3 RID: 24563
	public MovingPlatform InvisiblePlatform2x2;

	// Token: 0x04005FF4 RID: 24564
	private List<MovingPlatform> m_extantPlatforms = new List<MovingPlatform>();
}
