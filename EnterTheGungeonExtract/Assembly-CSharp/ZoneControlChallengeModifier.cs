using System;
using System.Collections;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001288 RID: 4744
public class ZoneControlChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A2F RID: 27183 RVA: 0x0029A0AC File Offset: 0x002982AC
	private void Start()
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		int num = this.MinBoxes;
		int i = currentRoom.Cells.Count;
		for (i -= this.ExtraBoxAboveArea; i > 0; i -= this.ExtraBoxEveryArea)
		{
			num++;
		}
		num = Mathf.Clamp(num, this.MinBoxes, 11);
		this.m_instanceBox = new FlippableCover[num];
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			CellData cellData = GameManager.Instance.Dungeon.data[c];
			if (cellData == null || cellData.containsTrap || cellData.isOccupied)
			{
				return false;
			}
			for (int k = 0; k < this.m_instanceBox.Length; k++)
			{
				if (this.m_instanceBox[k] != null && Vector2.Distance(this.m_instanceBox[k].specRigidbody.UnitCenter, c.ToCenterVector2()) < 5f)
				{
					return false;
				}
			}
			return true;
		};
		for (int j = 0; j < num; j++)
		{
			IntVector2? randomAvailableCell = currentRoom.GetRandomAvailableCell(new IntVector2?(new IntVector2(4, 4)), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
			if (randomAvailableCell != null)
			{
				GameObject gameObject = this.BoxPlaceable.InstantiateObject(currentRoom, randomAvailableCell.Value + IntVector2.One - currentRoom.area.basePosition, false, false);
				this.m_instanceBox[j] = gameObject.GetComponent<FlippableCover>();
				this.m_instanceBox[j].GetComponentInChildren<tk2dSpriteAnimator>().Play("moving_box_in");
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.m_instanceBox[j].specRigidbody, null, false);
			}
		}
	}

	// Token: 0x06006A30 RID: 27184 RVA: 0x0029A1E0 File Offset: 0x002983E0
	private void UpdateAnimation(tk2dSpriteAnimator anim, bool playerRadius)
	{
		if (anim.IsPlaying("moving_box_in") || anim.IsPlaying("moving_box_out"))
		{
			return;
		}
		if (playerRadius && !anim.IsPlaying("moving_box_open"))
		{
			anim.Play("moving_box_open");
		}
		if (!playerRadius && !anim.IsPlaying("moving_box_close"))
		{
			anim.Play("moving_box_close");
		}
	}

	// Token: 0x06006A31 RID: 27185 RVA: 0x0029A250 File Offset: 0x00298450
	private void Update()
	{
		bool flag = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].IsGunLocked = true;
		}
		for (int j = 0; j < this.m_instanceBox.Length; j++)
		{
			if (this.m_instanceBox[j])
			{
				flag = true;
				bool flag2 = false;
				for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[k];
					if (Vector2.Distance(this.m_instanceBox[j].specRigidbody.UnitCenter, playerController.CenterPosition) < this.AuraRadius)
					{
						playerController.IsGunLocked = false;
						this.m_timeElapsed = Mathf.Clamp(this.m_timeElapsed + BraveTime.DeltaTime, 0f, this.WinTimer + 1f);
						flag2 = true;
					}
					else
					{
						this.m_timeElapsed = Mathf.Clamp(this.m_timeElapsed - BraveTime.DeltaTime * this.DecayScale, 0f, this.WinTimer + 1f);
					}
				}
				this.UpdateAnimation(this.m_instanceBox[j].spriteAnimator, flag2);
			}
		}
		if (!flag)
		{
			for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
			{
				GameManager.Instance.AllPlayers[l].IsGunLocked = false;
			}
		}
		float num = Mathf.Lerp(0.01f, 1f, this.m_timeElapsed / this.WinTimer);
		for (int m = 0; m < this.m_instanceBox.Length; m++)
		{
			if (this.m_instanceBox[m])
			{
				this.m_instanceBox[m].outlineEast.GetComponent<tk2dSprite>().scale = new Vector3(num, num, num);
			}
		}
		if (this.m_timeElapsed >= this.WinTimer)
		{
			this.PopBox();
		}
	}

	// Token: 0x06006A32 RID: 27186 RVA: 0x0029A454 File Offset: 0x00298654
	private void PopBox()
	{
		if (!GameManager.HasInstance || !GameManager.Instance.Dungeon)
		{
			return;
		}
		for (int i = 0; i < this.m_instanceBox.Length; i++)
		{
			if (this.m_instanceBox[i])
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleBoxPop(this.m_instanceBox[i]));
				this.m_instanceBox[i] = null;
			}
		}
	}

	// Token: 0x06006A33 RID: 27187 RVA: 0x0029A4D4 File Offset: 0x002986D4
	private IEnumerator HandleBoxPop(FlippableCover box)
	{
		float elapsed = 0f;
		float duration = box.spriteAnimator.GetClipByName("moving_box_out").BaseClipLength;
		box.spriteAnimator.Play("moving_box_out");
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			if (box.outlineNorth)
			{
				box.outlineNorth.GetComponent<tk2dSprite>().scale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			}
			if (box.outlineEast)
			{
				box.outlineEast.GetComponent<tk2dSprite>().scale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			}
			yield return null;
		}
		LootEngine.DoDefaultPurplePoof(box.specRigidbody.UnitBottomCenter, false);
		UnityEngine.Object.Destroy(box.gameObject);
		yield break;
	}

	// Token: 0x06006A34 RID: 27188 RVA: 0x0029A4F0 File Offset: 0x002986F0
	private void OnDestroy()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].IsGunLocked = false;
		}
		this.PopBox();
	}

	// Token: 0x040066A3 RID: 26275
	public DungeonPlaceable BoxPlaceable;

	// Token: 0x040066A4 RID: 26276
	public float AuraRadius = 5f;

	// Token: 0x040066A5 RID: 26277
	public float WinTimer = 10f;

	// Token: 0x040066A6 RID: 26278
	public float DecayScale = 0.1875f;

	// Token: 0x040066A7 RID: 26279
	public int MinBoxes = 2;

	// Token: 0x040066A8 RID: 26280
	public int ExtraBoxAboveArea = 60;

	// Token: 0x040066A9 RID: 26281
	public int ExtraBoxEveryArea = 30;

	// Token: 0x040066AA RID: 26282
	private FlippableCover[] m_instanceBox;

	// Token: 0x040066AB RID: 26283
	private float m_timeElapsed;
}
