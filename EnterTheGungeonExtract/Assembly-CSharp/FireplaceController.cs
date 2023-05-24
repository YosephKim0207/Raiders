using System;
using System.Collections;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001158 RID: 4440
public class FireplaceController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x0600628D RID: 25229 RVA: 0x0026317C File Offset: 0x0026137C
	private IEnumerator Start()
	{
		yield return null;
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		yield return null;
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
			if (roomHandler.IsSecretRoom && roomHandler.secretRoomManager != null && roomHandler.secretRoomManager.revealStyle == SecretRoomManager.SecretRoomRevealStyle.FireplacePuzzle)
			{
				this.OnInteract = (Action)Delegate.Combine(this.OnInteract, new Action(roomHandler.secretRoomManager.OpenDoor));
			}
		}
		IntVector2 baseCellPosition = base.transform.position.IntXY(VectorConversions.Floor);
		for (int j = 0; j < 8; j++)
		{
			for (int k = 3; k < 7; k++)
			{
				if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(baseCellPosition + new IntVector2(j, k)))
				{
					GameManager.Instance.Dungeon.data[baseCellPosition + new IntVector2(j, k)].isOccupied = true;
				}
			}
			for (int l = 3; l < 6; l++)
			{
				if (j != 3 || l != 2)
				{
					if (j != 4 || l != 2)
					{
						GameManager.Instance.Dungeon.data[baseCellPosition + new IntVector2(j, l)].forceDisallowGoop = true;
					}
				}
			}
			for (int m = 0; m < 5; m++)
			{
				GameManager.Instance.Dungeon.data[baseCellPosition + new IntVector2(j, m)].containsTrap = true;
			}
			if (j >= 2 && j <= 5)
			{
				for (int n = 2; n < 4; n++)
				{
					GameManager.Instance.Dungeon.data[baseCellPosition + new IntVector2(j, n)].IsFireplaceCell = true;
				}
			}
		}
		CellData cellData = GameManager.Instance.Dungeon.data[baseCellPosition + new IntVector2(3, 2)];
		cellData.OnCellGooped = (Action<CellData>)Delegate.Combine(cellData.OnCellGooped, new Action<CellData>(this.HandleGooped));
		CellData cellData2 = GameManager.Instance.Dungeon.data[baseCellPosition + new IntVector2(4, 2)];
		cellData2.OnCellGooped = (Action<CellData>)Delegate.Combine(cellData2.OnCellGooped, new Action<CellData>(this.HandleGooped));
		SpeculativeRigidbody specRigidbody = this.GrateRigidbody.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleCollision));
		SpeculativeRigidbody specRigidbody2 = this.GrateRigidbody.specRigidbody;
		specRigidbody2.OnHitByBeam = (Action<BasicBeamController>)Delegate.Combine(specRigidbody2.OnHitByBeam, new Action<BasicBeamController>(this.HandleBeamCollision));
		yield return new WaitForSeconds(0.25f);
		baseCellPosition = base.transform.position.IntXY(VectorConversions.Floor);
		OccupiedCells oCells = new OccupiedCells(baseCellPosition + new IntVector2(0, 3), new IntVector2(8, 4), base.transform.position.GetAbsoluteRoom());
		oCells.FlagCells();
		Pathfinder.Instance.FlagRoomAsDirty(base.transform.position.GetAbsoluteRoom());
		yield break;
	}

	// Token: 0x0600628E RID: 25230 RVA: 0x00263198 File Offset: 0x00261398
	private void HandleGooped(CellData obj)
	{
		if (obj != null)
		{
			IntVector2 intVector = (obj.position.ToCenterVector2() / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
			if (DeadlyDeadlyGoopManager.allGoopPositionMap.ContainsKey(intVector))
			{
				DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = DeadlyDeadlyGoopManager.allGoopPositionMap[intVector];
				GoopDefinition goopDefinition = deadlyDeadlyGoopManager.goopDefinition;
				if (!goopDefinition.CanBeIgnited)
				{
					this.OnFireExtinguished();
				}
			}
		}
	}

	// Token: 0x0600628F RID: 25231 RVA: 0x002631FC File Offset: 0x002613FC
	private void HandleBeamCollision(BasicBeamController obj)
	{
		GoopModifier component = obj.GetComponent<GoopModifier>();
		if (component && component.goopDefinition != null && !component.goopDefinition.CanBeIgnited)
		{
			this.OnFireExtinguished();
		}
	}

	// Token: 0x06006290 RID: 25232 RVA: 0x00263244 File Offset: 0x00261444
	private void HandleCollision(CollisionData rigidbodyCollision)
	{
		Projectile projectile = rigidbodyCollision.OtherRigidbody.projectile;
		if (projectile && projectile.GetComponent<GoopModifier>())
		{
			GoopModifier component = projectile.GetComponent<GoopModifier>();
			if (component.goopDefinition != null && !component.goopDefinition.CanBeIgnited)
			{
				this.OnFireExtinguished();
			}
		}
	}

	// Token: 0x06006291 RID: 25233 RVA: 0x002632A8 File Offset: 0x002614A8
	private void OnFireExtinguished()
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		CellData cellData = GameManager.Instance.Dungeon.data[intVector + new IntVector2(3, 2)];
		cellData.OnCellGooped = (Action<CellData>)Delegate.Remove(cellData.OnCellGooped, new Action<CellData>(this.HandleGooped));
		CellData cellData2 = GameManager.Instance.Dungeon.data[intVector + new IntVector2(4, 2)];
		cellData2.OnCellGooped = (Action<CellData>)Delegate.Remove(cellData2.OnCellGooped, new Action<CellData>(this.HandleGooped));
		SpeculativeRigidbody specRigidbody = this.GrateRigidbody.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleCollision));
		SpeculativeRigidbody specRigidbody2 = this.GrateRigidbody.specRigidbody;
		specRigidbody2.OnHitByBeam = (Action<BasicBeamController>)Delegate.Remove(specRigidbody2.OnHitByBeam, new Action<BasicBeamController>(this.HandleBeamCollision));
		this.GrateRigidbody.enabled = false;
		this.GrateRigidbody.spriteAnimator.Play();
		tk2dBaseSprite component = this.FireObject.GetComponent<tk2dBaseSprite>();
		GlobalSparksDoer.DoRandomParticleBurst(25, component.WorldBottomLeft, component.WorldTopRight, Vector3.up, 70f, 0.5f, null, new float?(0.75f), new Color?(new Color(4f, 0.3f, 0f)), GlobalSparksDoer.SparksType.SOLID_SPARKLES);
		GlobalSparksDoer.DoRandomParticleBurst(25, component.WorldBottomLeft, component.WorldTopRight, Vector3.up, 70f, 0.5f, null, new float?(1.5f), new Color?(new Color(4f, 0.3f, 0f)), GlobalSparksDoer.SparksType.SOLID_SPARKLES);
		GlobalSparksDoer.DoRandomParticleBurst(25, component.WorldBottomLeft, component.WorldTopRight, Vector3.up, 70f, 0.5f, null, new float?(2.25f), new Color?(new Color(4f, 0.3f, 0f)), GlobalSparksDoer.SparksType.SOLID_SPARKLES);
		GlobalSparksDoer.DoRandomParticleBurst(25, component.WorldBottomLeft, component.WorldTopRight, Vector3.up, 70f, 0.5f, null, new float?(3f), new Color?(new Color(4f, 0.3f, 0f)), GlobalSparksDoer.SparksType.SOLID_SPARKLES);
		this.FireObject.SetActive(false);
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x0026353C File Offset: 0x0026173C
	private void Update()
	{
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x00263540 File Offset: 0x00261740
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06006294 RID: 25236 RVA: 0x0026354C File Offset: 0x0026174C
	public float GetDistanceToPoint(Vector2 point)
	{
		return Vector2.Distance(point, this.InteractPoint.position.XY());
	}

	// Token: 0x06006295 RID: 25237 RVA: 0x00263564 File Offset: 0x00261764
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006296 RID: 25238 RVA: 0x0026356C File Offset: 0x0026176C
	public void Interact(PlayerController interactor)
	{
		if (this.m_flipped)
		{
			return;
		}
		this.m_flipped = true;
		AkSoundEngine.PostEvent("Play_OBJ_secret_switch_01", base.gameObject);
		AkSoundEngine.PostEvent("Play_OBJ_wall_reveal_01", base.gameObject);
		ScreenShakeSettings screenShakeSettings = new ScreenShakeSettings(0.2f, 7f, 1f, 0f, Vector2.left);
		GameManager.Instance.MainCameraController.DoScreenShake(screenShakeSettings, null, false);
		if (this.OnInteract != null)
		{
			this.OnInteract();
		}
	}

	// Token: 0x06006297 RID: 25239 RVA: 0x00263600 File Offset: 0x00261800
	public void OnEnteredRange(PlayerController interactor)
	{
		Debug.Log("ENTERED RANGE");
	}

	// Token: 0x06006298 RID: 25240 RVA: 0x0026360C File Offset: 0x0026180C
	public void OnExitRange(PlayerController interactor)
	{
		Debug.Log("EXITED RANGE");
	}

	// Token: 0x04005D93 RID: 23955
	public GoopDefinition oilGoop;

	// Token: 0x04005D94 RID: 23956
	public GameObject FireObject;

	// Token: 0x04005D95 RID: 23957
	public SpeculativeRigidbody GrateRigidbody;

	// Token: 0x04005D96 RID: 23958
	public Vector2 goopMinOffset;

	// Token: 0x04005D97 RID: 23959
	public Vector2 goopDimensions;

	// Token: 0x04005D98 RID: 23960
	public Transform InteractPoint;

	// Token: 0x04005D99 RID: 23961
	private bool m_flipped;

	// Token: 0x04005D9A RID: 23962
	private Action OnInteract;
}
