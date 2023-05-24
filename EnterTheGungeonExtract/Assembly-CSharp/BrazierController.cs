using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001106 RID: 4358
public class BrazierController : DungeonPlaceableBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x0600601D RID: 24605 RVA: 0x0024FFEC File Offset: 0x0024E1EC
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.sprite.transform.position, bounds.max + base.sprite.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x0600601E RID: 24606 RVA: 0x002500D8 File Offset: 0x0024E2D8
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x0600601F RID: 24607 RVA: 0x002500E0 File Offset: 0x0024E2E0
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
	}

	// Token: 0x06006020 RID: 24608 RVA: 0x0025010C File Offset: 0x0024E30C
	private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody.gameActor && otherRigidbody.gameActor is PlayerController)
		{
			this.OnPlayerCollision(otherRigidbody.gameActor as PlayerController);
		}
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x00250140 File Offset: 0x0024E340
	private void OnPlayerCollision(PlayerController p)
	{
		if (p != null && (p.IsDodgeRolling || this.m_flipped))
		{
			if (!p.IsDodgeRolling && Time.realtimeSinceStartup - this.m_flipTime < 0.25f)
			{
				return;
			}
			if (this.m_flipped)
			{
				base.spriteAnimator.Play(this.directionalBreakAnims.GetAnimationForVector(this.m_cachedFlipVector));
			}
			else
			{
				base.spriteAnimator.Play(this.BreakAnimName);
			}
			base.sprite.IsPerpendicular = false;
			base.sprite.HeightOffGround = -1.25f;
			base.sprite.UpdateZDepth();
			base.specRigidbody.enabled = false;
			base.transform.position.GetAbsoluteRoom().DeregisterInteractable(this);
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
		}
	}

	// Token: 0x06006022 RID: 24610 RVA: 0x00250240 File Offset: 0x0024E440
	private void Update()
	{
		if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && !this.m_flipped && base.specRigidbody.enabled)
		{
			this.m_accumParticleCount += BraveTime.DeltaTime * 10f;
			if (this.m_accumParticleCount > 1f)
			{
				int num = Mathf.FloorToInt(this.m_accumParticleCount);
				this.m_accumParticleCount -= (float)num;
				GlobalSparksDoer.DoRandomParticleBurst(num, base.specRigidbody.UnitBottomLeft.ToVector3ZisY(0f), base.specRigidbody.UnitTopRight.ToVector3ZisY(0f), Vector3.up, 120f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
			}
		}
	}

	// Token: 0x06006023 RID: 24611 RVA: 0x00250328 File Offset: 0x0024E528
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x00250348 File Offset: 0x0024E548
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x00250364 File Offset: 0x0024E564
	public void Interact(PlayerController interactor)
	{
		this.m_flipped = true;
		Vector2 normalized = (base.specRigidbody.UnitCenter - interactor.specRigidbody.UnitCenter).normalized;
		GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round)).DeregisterInteractable(this);
		this.m_cachedFlipVector = normalized;
		base.spriteAnimator.Play(this.directionalAnimationInfo.GetAnimationForVector(normalized));
		Vector2 normalized2 = BraveUtility.GetMajorAxis(normalized).normalized;
		Vector2 vector = base.specRigidbody.UnitCenter + normalized2;
		if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			GlobalSparksDoer.DoRandomParticleBurst(UnityEngine.Random.Range(25, 40), base.specRigidbody.UnitBottomLeft.ToVector3ZisY(0f), base.specRigidbody.UnitTopRight.ToVector3ZisY(0f), Vector3.up, 120f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
		}
		DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goop).TimedAddGoopLine(vector, vector + normalized2 * this.goopLength, this.goopWidth / 2f, this.goopTime);
		this.m_flipTime = Time.realtimeSinceStartup;
		DeadlyDeadlyGoopManager.IgniteGoopsCircle(vector, 1.5f);
	}

	// Token: 0x06006026 RID: 24614 RVA: 0x002504D8 File Offset: 0x0024E6D8
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		Vector2 vector = base.specRigidbody.UnitCenter - interactor.specRigidbody.UnitCenter;
		switch (DungeonData.GetCardinalFromVector2(vector))
		{
		case DungeonData.Direction.NORTH:
			return "tablekick_up";
		case DungeonData.Direction.EAST:
			return "tablekick_right";
		case DungeonData.Direction.SOUTH:
			return "tablekick_down";
		case DungeonData.Direction.WEST:
			shouldBeFlipped = true;
			return "tablekick_right";
		}
		return "tablekick_right";
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x00250554 File Offset: 0x0024E754
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x0025055C File Offset: 0x0024E75C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		base.PlacedPosition = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = base.PlacedPosition.x; i < base.PlacedPosition.x + 2; i++)
		{
			for (int j = base.PlacedPosition.y; j < base.PlacedPosition.y + 2; j++)
			{
				GameManager.Instance.Dungeon.data[i, j].isOccupied = true;
			}
		}
	}

	// Token: 0x04005AA6 RID: 23206
	public DebrisDirectionalAnimationInfo directionalAnimationInfo;

	// Token: 0x04005AA7 RID: 23207
	public GoopDefinition goop;

	// Token: 0x04005AA8 RID: 23208
	[DwarfConfigurable]
	public float goopLength = 6f;

	// Token: 0x04005AA9 RID: 23209
	[DwarfConfigurable]
	public float goopWidth = 2f;

	// Token: 0x04005AAA RID: 23210
	[DwarfConfigurable]
	public float goopTime = 1f;

	// Token: 0x04005AAB RID: 23211
	public string BreakAnimName;

	// Token: 0x04005AAC RID: 23212
	public DebrisDirectionalAnimationInfo directionalBreakAnims;

	// Token: 0x04005AAD RID: 23213
	private float m_accumParticleCount;

	// Token: 0x04005AAE RID: 23214
	private bool m_flipped;

	// Token: 0x04005AAF RID: 23215
	private float m_flipTime = -1f;

	// Token: 0x04005AB0 RID: 23216
	private Vector2 m_cachedFlipVector;
}
