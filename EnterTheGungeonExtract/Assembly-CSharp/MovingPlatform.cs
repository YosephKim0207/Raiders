using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020015A5 RID: 5541
public class MovingPlatform : DungeonPlaceableBehaviour, IDwarfDrawable, IPlaceConfigurable
{
	// Token: 0x06007F27 RID: 32551 RVA: 0x00335978 File Offset: 0x00333B78
	public IEnumerator Start()
	{
		if (this.UsesDwarfConfigurableSize)
		{
			this.Size = new IntVector2((int)this.DwarfConfigurableWidth, (int)this.DwarfConfigurableHeight);
		}
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnEnterTrigger));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
		SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
		specRigidbody3.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody3.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.OnExitTrigger));
		SpeculativeRigidbody specRigidbody4 = base.specRigidbody;
		specRigidbody4.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody4.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.OnPostRigidbodyMovement));
		if (this.UsesDwarfConfigurableSize)
		{
			this.ForceUpdateSize();
		}
		this.m_sprite = base.GetComponent<tk2dSlicedSprite>();
		this.MarkCells();
		yield break;
	}

	// Token: 0x06007F28 RID: 32552 RVA: 0x00335994 File Offset: 0x00333B94
	public void ForceUpdateSize()
	{
		tk2dSlicedSprite component = base.GetComponent<tk2dSlicedSprite>();
		if (component != null)
		{
			component.dimensions = new Vector2((float)(16 * this.Size.x), (float)(16 * this.Size.y));
		}
		PixelCollider pixelCollider = base.specRigidbody.PixelColliders.Find((PixelCollider c) => c.CollisionLayer == CollisionLayer.MovingPlatform);
		if (pixelCollider == null)
		{
			pixelCollider = new PixelCollider();
			pixelCollider.CollisionLayer = CollisionLayer.MovingPlatform;
			base.specRigidbody.PixelColliders.Add(pixelCollider);
		}
		pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
		pixelCollider.ManualOffsetX = 0;
		pixelCollider.ManualOffsetY = 0;
		pixelCollider.ManualWidth = this.Size.x * 16;
		pixelCollider.ManualHeight = this.Size.y * 16;
		pixelCollider.Regenerate(base.specRigidbody.transform, true, true);
		base.specRigidbody.Reinitialize();
		if (this.StencilQuad != null)
		{
			float num = component.dimensions.x / 16f;
			float num2 = (component.dimensions.y + 7f) / 16f;
			float num3 = 0.4375f;
			this.StencilQuad.localScale = new Vector3(num, num2, 1f);
			this.StencilQuad.transform.localPosition = new Vector3(num / 2f, num2 / 2f - num3, 0f);
		}
	}

	// Token: 0x06007F29 RID: 32553 RVA: 0x00335B1C File Offset: 0x00333D1C
	protected override void OnDestroy()
	{
		if (this.m_room != null && base.specRigidbody && this.m_room.RoomMovingPlatforms != null)
		{
			this.m_room.RoomMovingPlatforms.Remove(base.specRigidbody);
		}
		base.OnDestroy();
	}

	// Token: 0x06007F2A RID: 32554 RVA: 0x00335B74 File Offset: 0x00333D74
	private void OnEnterTrigger(SpeculativeRigidbody obj, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (obj.gameActor && !obj.gameActor.SupportingPlatforms.Contains(this))
		{
			obj.gameActor.SupportingPlatforms.Add(this);
		}
		base.specRigidbody.RegisterCarriedRigidbody(obj);
	}

	// Token: 0x06007F2B RID: 32555 RVA: 0x00335BC4 File Offset: 0x00333DC4
	private void OnTriggerCollision(SpeculativeRigidbody obj, SpeculativeRigidbody source, CollisionData collisionData)
	{
	}

	// Token: 0x06007F2C RID: 32556 RVA: 0x00335BC8 File Offset: 0x00333DC8
	private void OnExitTrigger(SpeculativeRigidbody obj, SpeculativeRigidbody source)
	{
		if (obj && obj.gameActor)
		{
			obj.gameActor.SupportingPlatforms.Remove(this);
		}
		if (this)
		{
			base.specRigidbody.DeregisterCarriedRigidbody(obj);
		}
	}

	// Token: 0x06007F2D RID: 32557 RVA: 0x00335C1C File Offset: 0x00333E1C
	public void ClearCells()
	{
		PixelCollider primaryPixelCollider = base.specRigidbody.PrimaryPixelCollider;
		IntVector2 intVector = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.LowerLeft).ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.UpperRight).ToIntVector2(VectorConversions.Floor);
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			for (int j = intVector.y; j <= intVector2.y; j++)
			{
				List<SpeculativeRigidbody> platforms = GameManager.Instance.Dungeon.data.cellData[i][j].platforms;
				if (platforms != null)
				{
					platforms.Remove(base.specRigidbody);
				}
				if (this.AllowsForGoop)
				{
					DeadlyDeadlyGoopManager.ForceClearGoopsInCell(new IntVector2(i, j));
					GameManager.Instance.Dungeon.data.cellData[i][j].forceAllowGoop = false;
				}
			}
		}
	}

	// Token: 0x06007F2E RID: 32558 RVA: 0x00335D08 File Offset: 0x00333F08
	public void MarkCells()
	{
		PixelCollider primaryPixelCollider = base.specRigidbody.PrimaryPixelCollider;
		IntVector2 intVector = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.LowerLeft).ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.UpperRight).ToIntVector2(VectorConversions.Floor);
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			for (int j = intVector.y; j <= intVector2.y; j++)
			{
				if (GameManager.Instance.Dungeon.data.cellData[i][j] != null)
				{
					List<SpeculativeRigidbody> list = GameManager.Instance.Dungeon.data.cellData[i][j].platforms;
					if (list == null)
					{
						list = new List<SpeculativeRigidbody>();
						GameManager.Instance.Dungeon.data.cellData[i][j].platforms = list;
					}
					if (!list.Contains(base.specRigidbody))
					{
						list.Add(base.specRigidbody);
					}
					if (this.AllowsForGoop)
					{
						GameManager.Instance.Dungeon.data.cellData[i][j].forceAllowGoop = true;
					}
				}
			}
		}
	}

	// Token: 0x06007F2F RID: 32559 RVA: 0x00335E3C File Offset: 0x0033403C
	private void OnPostRigidbodyMovement(SpeculativeRigidbody specRigidbody, Vector2 unitDelta, IntVector2 pixelDelta)
	{
		if (pixelDelta == IntVector2.Zero)
		{
			return;
		}
		PixelCollider primaryPixelCollider = specRigidbody.PrimaryPixelCollider;
		IntVector2 intVector = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.LowerLeft - pixelDelta).ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.LowerLeft).ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector3 = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.UpperRight - pixelDelta).ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector4 = PhysicsEngine.PixelToUnitMidpoint(primaryPixelCollider.UpperRight).ToIntVector2(VectorConversions.Floor);
		Dungeon dungeon = GameManager.Instance.Dungeon;
		if (intVector != intVector2 || intVector3 != intVector4)
		{
			for (int i = intVector.x; i <= intVector3.x; i++)
			{
				for (int j = intVector.y; j <= intVector3.y; j++)
				{
					if (dungeon.CellExists(i, j) && dungeon.data[i, j] != null)
					{
						List<SpeculativeRigidbody> platforms = dungeon.data[i, j].platforms;
						if (platforms != null)
						{
							platforms.Remove(specRigidbody);
						}
					}
				}
			}
			for (int k = intVector2.x; k <= intVector4.x; k++)
			{
				for (int l = intVector2.y; l <= intVector4.y; l++)
				{
					if (dungeon.CellExists(k, l) && dungeon.data[k, l] != null)
					{
						List<SpeculativeRigidbody> list = dungeon.data[k, l].platforms;
						if (list == null)
						{
							list = new List<SpeculativeRigidbody>();
							GameManager.Instance.Dungeon.data.cellData[k][l].platforms = list;
						}
						if (!list.Contains(specRigidbody))
						{
							list.Add(specRigidbody);
						}
					}
				}
			}
		}
		if (this.m_sprite != null)
		{
			this.m_sprite.UpdateZDepth();
		}
	}

	// Token: 0x06007F30 RID: 32560 RVA: 0x0033604C File Offset: 0x0033424C
	public IntVector2 GetOverrideDwarfDimensions(PrototypePlacedObjectData objectData)
	{
		if (objectData.GetBoolFieldValueByName("UsesDwarfConfigurableSize"))
		{
			return new IntVector2((int)objectData.GetFieldValueByName("DwarfConfigurableWidth"), (int)objectData.GetFieldValueByName("DwarfConfigurableHeight"));
		}
		return new IntVector2(this.placeableWidth, this.placeableHeight);
	}

	// Token: 0x06007F31 RID: 32561 RVA: 0x00336098 File Offset: 0x00334298
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		if (room.RoomMovingPlatforms != null && base.specRigidbody)
		{
			room.RoomMovingPlatforms.Add(base.specRigidbody);
		}
	}

	// Token: 0x040081DE RID: 33246
	public IntVector2 Size;

	// Token: 0x040081DF RID: 33247
	[DwarfConfigurable]
	public bool UsesDwarfConfigurableSize;

	// Token: 0x040081E0 RID: 33248
	[DwarfConfigurable]
	public float DwarfConfigurableWidth = 3f;

	// Token: 0x040081E1 RID: 33249
	[DwarfConfigurable]
	public float DwarfConfigurableHeight = 3f;

	// Token: 0x040081E2 RID: 33250
	public Transform StencilQuad;

	// Token: 0x040081E3 RID: 33251
	public bool StaticForPitfall;

	// Token: 0x040081E4 RID: 33252
	public bool AllowsForGoop;

	// Token: 0x040081E5 RID: 33253
	private tk2dSlicedSprite m_sprite;

	// Token: 0x040081E6 RID: 33254
	private RoomHandler m_room;
}
