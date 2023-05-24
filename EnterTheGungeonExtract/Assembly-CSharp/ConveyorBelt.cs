using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E57 RID: 3671
public class ConveyorBelt : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06004E27 RID: 20007 RVA: 0x001AF224 File Offset: 0x001AD424
	private IEnumerator Start()
	{
		this.Velocity = new Vector2(this.VelocityX, this.VelocityY);
		IntVector2 Size = new IntVector2(Mathf.FloorToInt(this.ConveyorWidth), Mathf.FloorToInt(this.ConveyorHeight));
		if (this.IsHorizontal)
		{
			this.ModuleAnimators[0].transform.position = base.transform.position + new Vector3(0f, 0f, 0f);
			this.ModuleAnimators[0].GetComponent<tk2dTiledSprite>().dimensions = new Vector2((float)(Size.x * 16), 16f);
			this.ModuleAnimators[1].GetComponent<tk2dTiledSprite>().dimensions = new Vector2((float)(Size.x * 16), (float)((Size.y - 2) * 16));
			this.ModuleAnimators[1].transform.position = base.transform.position + new Vector3(0f, 1f, 0f);
			this.ModuleAnimators[2].GetComponent<tk2dTiledSprite>().dimensions = new Vector2((float)(Size.x * 16), 16f);
			this.ModuleAnimators[2].transform.position = base.transform.position + new Vector3(0f, (float)(Size.y - 1), 0f);
			this.ShadowObjects[0].transform.position = base.transform.position + new Vector3(0f, 0f, 0f);
			this.ShadowObjects[1].transform.position = base.transform.position + new Vector3(0f, 1f, 0f);
			(this.ShadowObjects[1] as tk2dTiledSprite).dimensions = new Vector2(16f, (float)((Size.y - 2) * 16));
			this.ShadowObjects[2].transform.position = base.transform.position + new Vector3(0f, (float)(Size.y - 1), 0f);
			this.ShadowObjects[3].transform.position = base.transform.position + new Vector3((float)(Size.x - 1), 0f, 0f);
			this.ShadowObjects[4].transform.position = base.transform.position + new Vector3((float)(Size.x - 1) + 0.3125f, 1f, 0f);
			(this.ShadowObjects[4] as tk2dTiledSprite).dimensions = new Vector2(16f, (float)((Size.y - 2) * 16));
			this.ShadowObjects[5].transform.position = base.transform.position + new Vector3((float)(Size.x - 1), (float)(Size.y - 1), 0f);
		}
		else
		{
			this.ModuleAnimators[0].transform.position = base.transform.position + new Vector3(0f, 0f, 0f);
			this.ModuleAnimators[0].GetComponent<tk2dTiledSprite>().dimensions = new Vector2(16f, (float)(Size.y * 16));
			this.ModuleAnimators[1].GetComponent<tk2dTiledSprite>().dimensions = new Vector2((float)((Size.x - 2) * 16), (float)(Size.y * 16));
			this.ModuleAnimators[1].transform.position = base.transform.position + new Vector3(1f, 0f, 0f);
			this.ModuleAnimators[2].GetComponent<tk2dTiledSprite>().dimensions = new Vector2(16f, (float)(Size.y * 16));
			this.ModuleAnimators[2].transform.position = base.transform.position + new Vector3((float)(Size.x - 1), 0f, 0f);
			this.ShadowObjects[0].transform.position = base.transform.position + new Vector3(0f, 0f, 0f);
			this.ShadowObjects[1].transform.position = base.transform.position + new Vector3(1f, 0f, 0f);
			(this.ShadowObjects[1] as tk2dTiledSprite).dimensions = new Vector2((float)((Size.x - 2) * 16), 16f);
			this.ShadowObjects[2].transform.position = base.transform.position + new Vector3((float)(Size.x - 1), 0f, 0f);
			this.ShadowObjects[3].transform.position = base.transform.position + new Vector3(0f, (float)(Size.y - 1), 0f);
			this.ShadowObjects[4].transform.position = base.transform.position + new Vector3(1f, (float)(Size.y - 1) + 0.3125f, 0f);
			(this.ShadowObjects[4] as tk2dTiledSprite).dimensions = new Vector2((float)((Size.x - 2) * 16), 16f);
			this.ShadowObjects[5].transform.position = base.transform.position + new Vector3((float)(Size.x - 1), (float)(Size.y - 1), 0f);
		}
		for (int i = 0; i < this.ModuleAnimators.Count; i++)
		{
			this.ModuleAnimators[i].Sprite.UpdateZDepth();
		}
		for (int j = 0; j < this.ShadowObjects.Count; j++)
		{
			this.ShadowObjects[j].IsPerpendicular = false;
			this.ShadowObjects[j].UpdateZDepth();
		}
		base.specRigidbody.PrimaryPixelCollider.ManualWidth = Size.x * 16;
		base.specRigidbody.PrimaryPixelCollider.ManualHeight = Size.y * 16;
		if (this.IsHorizontal)
		{
			base.specRigidbody.PrimaryPixelCollider.ManualOffsetY = 4;
			base.specRigidbody.PrimaryPixelCollider.ManualHeight = base.specRigidbody.PrimaryPixelCollider.ManualHeight - 8;
		}
		else
		{
			base.specRigidbody.PrimaryPixelCollider.ManualOffsetX = 4;
			base.specRigidbody.PrimaryPixelCollider.ManualWidth = base.specRigidbody.PrimaryPixelCollider.ManualWidth - 8;
		}
		base.specRigidbody.Reinitialize();
		base.specRigidbody.RegenerateColliders = true;
		yield return null;
		base.specRigidbody.RegenerateColliders = true;
		PhysicsEngine.UpdatePosition(base.specRigidbody);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnEnterTrigger));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody2.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.OnExitTrigger));
		yield return new WaitForSeconds(0.5f);
		PhysicsEngine.UpdatePosition(base.specRigidbody);
		yield break;
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001AF240 File Offset: 0x001AD440
	public void Update()
	{
		tk2dSpriteAnimator tk2dSpriteAnimator;
		for (int i = 0; i < this.ModuleAnimators.Count; i++)
		{
			tk2dSpriteAnimator = this.ModuleAnimators[i];
			string text = this.PositiveVelocityAnims[i];
			string text2 = this.NegativeVelocityAnims[i];
			if (tk2dSpriteAnimator)
			{
				if (tk2dSpriteAnimator.CurrentClip != null && tk2dSpriteAnimator.CurrentClip.frames != null && tk2dSpriteAnimator.CurrentClip.frames.Length > 0)
				{
					float num = 1f / ((float)(tk2dSpriteAnimator.CurrentClip.frames.Length * 2) / tk2dSpriteAnimator.CurrentClip.fps);
					float num2 = this.Velocity.magnitude / 8f / num;
					tk2dSpriteAnimator.ClipFps = tk2dSpriteAnimator.CurrentClip.fps * num2;
				}
				if (this.Velocity.x != 0f)
				{
					if (this.Velocity.x > 0f && !tk2dSpriteAnimator.IsPlaying(text))
					{
						tk2dSpriteAnimator.Play(text);
					}
					else if (this.Velocity.x < 0f && !tk2dSpriteAnimator.IsPlaying(text2))
					{
						tk2dSpriteAnimator.Play(text2);
					}
				}
				else if (this.Velocity.y > 0f && !tk2dSpriteAnimator.IsPlaying(text))
				{
					tk2dSpriteAnimator.Play(text);
				}
				else if (this.Velocity.y < 0f && !tk2dSpriteAnimator.IsPlaying(text2))
				{
					tk2dSpriteAnimator.Play(text2);
				}
			}
		}
		tk2dSpriteAnimator = this.ModuleAnimators[0];
		int num3 = (int)tk2dSpriteAnimator.clipTime;
		int num4 = (int)(tk2dSpriteAnimator.clipTime + tk2dSpriteAnimator.ClipFps * BraveTime.DeltaTime);
		int num5 = (num4 - num3) * 2;
		IntVector2 zero = IntVector2.Zero;
		if (this.Velocity.x != 0f)
		{
			zero = new IntVector2((this.Velocity.x <= 0f) ? (-num5) : num5, 0);
		}
		else if (this.Velocity.y != 0f)
		{
			zero = new IntVector2(0, (this.Velocity.y <= 0f) ? (-num5) : num5);
		}
		for (int j = 0; j < this.m_rigidbodiesOnPlatform.Count; j++)
		{
			if (this.m_rigidbodiesOnPlatform[j])
			{
				if (GameManager.Instance.Dungeon.CellSupportsFalling(this.m_rigidbodiesOnPlatform[j].UnitCenter) || base.specRigidbody.ContainsPoint(this.m_rigidbodiesOnPlatform[j].UnitCenter, 2147483647, true))
				{
					if (this.m_rigidbodiesOnPlatform[j].gameActor)
					{
						if (this.m_rigidbodiesOnPlatform[j].gameActor.IsGrounded)
						{
							this.m_rigidbodiesOnPlatform[j].specRigidbody.ImpartedPixelsToMove = zero;
						}
					}
					else
					{
						this.m_rigidbodiesOnPlatform[j].Velocity += this.Velocity;
					}
				}
			}
		}
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001AF59C File Offset: 0x001AD79C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001AF5A4 File Offset: 0x001AD7A4
	private void OnEnterTrigger(SpeculativeRigidbody obj, SpeculativeRigidbody source, CollisionData collisionData)
	{
		if (this.m_rigidbodiesOnPlatform.Contains(obj))
		{
			return;
		}
		if (obj.gameActor && obj.gameActor is PlayerController)
		{
			PlayerController playerController = obj.gameActor as PlayerController;
			if (PassiveItem.IsFlagSetForCharacter(playerController, typeof(HeavyBootsItem)))
			{
				return;
			}
		}
		this.m_rigidbodiesOnPlatform.Add(obj);
		base.specRigidbody.RegisterCarriedRigidbody(obj);
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x001AF620 File Offset: 0x001AD820
	private void OnExitTrigger(SpeculativeRigidbody obj, SpeculativeRigidbody source)
	{
		if (!this.m_rigidbodiesOnPlatform.Contains(obj))
		{
			return;
		}
		this.m_rigidbodiesOnPlatform.Remove(obj);
		if (this)
		{
			base.specRigidbody.DeregisterCarriedRigidbody(obj);
		}
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x001AF658 File Offset: 0x001AD858
	public void PostFieldConfiguration(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round);
		int num = 0;
		while ((float)num < this.ConveyorWidth)
		{
			int num2 = 0;
			while ((float)num2 < this.ConveyorHeight)
			{
				IntVector2 intVector2 = intVector + new IntVector2(num, num2);
				CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
				if (cellData != null)
				{
					cellData.containsTrap = true;
					cellData.cellVisualData.RequiresPitBordering = true;
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06004E2D RID: 20013 RVA: 0x001AF6E8 File Offset: 0x001AD8E8
	public void ConfigureOnPlacement(RoomHandler room)
	{
	}

	// Token: 0x0400447E RID: 17534
	[DwarfConfigurable]
	public float ConveyorWidth = 4f;

	// Token: 0x0400447F RID: 17535
	[DwarfConfigurable]
	public float ConveyorHeight = 3f;

	// Token: 0x04004480 RID: 17536
	[DwarfConfigurable]
	public float VelocityX;

	// Token: 0x04004481 RID: 17537
	[DwarfConfigurable]
	public float VelocityY;

	// Token: 0x04004482 RID: 17538
	public bool IsHorizontal;

	// Token: 0x04004483 RID: 17539
	public List<tk2dBaseSprite> ShadowObjects;

	// Token: 0x04004484 RID: 17540
	public List<tk2dSpriteAnimator> ModuleAnimators;

	// Token: 0x04004485 RID: 17541
	public List<string> NegativeVelocityAnims;

	// Token: 0x04004486 RID: 17542
	public List<string> PositiveVelocityAnims;

	// Token: 0x04004487 RID: 17543
	private Vector2 Velocity;

	// Token: 0x04004488 RID: 17544
	private List<SpeculativeRigidbody> m_rigidbodiesOnPlatform = new List<SpeculativeRigidbody>();
}
