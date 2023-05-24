using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020012D5 RID: 4821
public class FlippableCover : BraveBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x17001000 RID: 4096
	// (get) Token: 0x06006BEB RID: 27627 RVA: 0x002A7518 File Offset: 0x002A5718
	public bool IsBroken
	{
		get
		{
			return !(this.m_breakable == null) && this.m_breakable.IsDestroyed;
		}
	}

	// Token: 0x17001001 RID: 4097
	// (get) Token: 0x06006BEC RID: 27628 RVA: 0x002A7538 File Offset: 0x002A5738
	public bool IsFlipped
	{
		get
		{
			return this.m_flipped;
		}
	}

	// Token: 0x17001002 RID: 4098
	// (get) Token: 0x06006BED RID: 27629 RVA: 0x002A7540 File Offset: 0x002A5740
	public DungeonData.Direction DirectionFlipped
	{
		get
		{
			return this.m_flipDirection;
		}
	}

	// Token: 0x17001003 RID: 4099
	// (get) Token: 0x06006BEE RID: 27630 RVA: 0x002A7548 File Offset: 0x002A5748
	// (set) Token: 0x06006BEF RID: 27631 RVA: 0x002A7550 File Offset: 0x002A5750
	public bool PreventPitFalls { get; set; }

	// Token: 0x06006BF0 RID: 27632 RVA: 0x002A755C File Offset: 0x002A575C
	public void Awake()
	{
		base.specRigidbody = base.GetComponentInChildren<SpeculativeRigidbody>();
		this.m_slide = base.GetComponentInChildren<SlideSurface>();
	}

	// Token: 0x06006BF1 RID: 27633 RVA: 0x002A7578 File Offset: 0x002A5778
	private void Start()
	{
		if (base.sprite == null)
		{
			base.sprite = base.transform.GetChild(0).GetComponent<tk2dSprite>();
		}
		if (base.spriteAnimator == null)
		{
			base.spriteAnimator = base.transform.GetChild(0).GetComponent<tk2dSpriteAnimator>();
		}
		base.sprite.AdditionalFlatForwardPercentage = 0.125f;
		base.sprite.IsPerpendicular = this.flipStyle == FlippableCover.FlipStyle.NO_FLIPS && base.sprite.IsPerpendicular;
		base.sprite.HeightOffGround = ((!this.UsesCustomHeightsOffGround) ? 0f : this.CustomStartHeightOffGround);
		base.sprite.UpdateZDepth();
		if (this.shadowSprite != null)
		{
			this.shadowSprite.IsPerpendicular = false;
			this.shadowSprite.usesOverrideMaterial = true;
			this.shadowSprite.HeightOffGround = -1f;
			this.shadowSprite.UpdateZDepth();
			this.m_shadowSpriteAnimator = this.shadowSprite.GetComponent<tk2dSpriteAnimator>();
		}
		this.m_breakable = base.GetComponentInChildren<MajorBreakable>();
		if (this.m_breakable != null)
		{
			MajorBreakable breakable = this.m_breakable;
			breakable.OnDamaged = (Action<float>)Delegate.Combine(breakable.OnDamaged, new Action<float>(this.Damaged));
			MajorBreakable breakable2 = this.m_breakable;
			breakable2.OnBreak = (Action)Delegate.Combine(breakable2.OnBreak, new Action(this.DestroyCover));
			if (this.prebreakFrames.Length > 0)
			{
				this.m_breakable.MinHitPointsFromNonExplosions = 1f;
			}
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody2.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.OnPostMovement));
		if (base.specRigidbody.PixelColliders.Count > 1)
		{
			base.specRigidbody.PixelColliders[1].Enabled = false;
		}
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		if (absoluteRoom != null)
		{
			absoluteRoom.Entered += this.HandleParentRoomEntered;
			if (GameManager.Instance.BestActivePlayer && absoluteRoom == GameManager.Instance.BestActivePlayer.CurrentRoom)
			{
				this.m_hasRoomEnteredProcessed = true;
			}
		}
		if (this.flipStyle == FlippableCover.FlipStyle.NO_FLIPS)
		{
			this.RemoveFromRoomHierarchy();
			base.specRigidbody.CanBePushed = true;
		}
	}

	// Token: 0x06006BF2 RID: 27634 RVA: 0x002A7810 File Offset: 0x002A5A10
	private void HandleParentRoomEntered(PlayerController p)
	{
		if (this.m_hasRoomEnteredProcessed)
		{
			return;
		}
		this.m_hasRoomEnteredProcessed = true;
		if (p && p.HasActiveBonusSynergy(CustomSynergyType.GILDED_TABLES, false) && UnityEngine.Random.value < 0.15f)
		{
			this.m_isGilded = true;
			base.sprite.usesOverrideMaterial = true;
			tk2dSprite tk2dSprite = base.sprite as tk2dSprite;
			tk2dSprite.GenerateUV2 = true;
			Material material = UnityEngine.Object.Instantiate<Material>(base.sprite.renderer.material);
			material.DisableKeyword("TINTING_OFF");
			material.EnableKeyword("TINTING_ON");
			material.SetColor("_OverrideColor", new Color(1f, 0.77f, 0f));
			material.DisableKeyword("EMISSIVE_OFF");
			material.EnableKeyword("EMISSIVE_ON");
			material.SetFloat("_EmissivePower", 1.75f);
			material.SetFloat("_EmissiveColorPower", 1f);
			base.sprite.renderer.material = material;
			Shader shader = Shader.Find("Brave/ItemSpecific/MetalSkinLayerShader");
			MeshRenderer component = base.sprite.GetComponent<MeshRenderer>();
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == shader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material2 = new Material(shader);
			material2.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material2;
			component.sharedMaterials = sharedMaterials;
			tk2dSprite.ForceBuild();
		}
	}

	// Token: 0x06006BF3 RID: 27635 RVA: 0x002A79A8 File Offset: 0x002A5BA8
	protected void ClearOutlines()
	{
		this.outlineNorth.SetActive(false);
		this.outlineEast.SetActive(false);
		this.outlineSouth.SetActive(false);
		this.outlineWest.SetActive(false);
		this.m_lastOutlineDirection = (DungeonData.Direction)(-1);
	}

	// Token: 0x06006BF4 RID: 27636 RVA: 0x002A79E4 File Offset: 0x002A5BE4
	protected void ToggleOutline(DungeonData.Direction dir)
	{
		if (this.IsBroken)
		{
			return;
		}
		if (this.flipStyle == FlippableCover.FlipStyle.NO_FLIPS)
		{
			return;
		}
		switch (dir)
		{
		case DungeonData.Direction.NORTH:
			if (this.flipStyle != FlippableCover.FlipStyle.ONLY_FLIPS_LEFT_RIGHT)
			{
				this.outlineNorth.SetActive(!this.outlineNorth.activeSelf);
			}
			break;
		case DungeonData.Direction.EAST:
			if (this.flipStyle != FlippableCover.FlipStyle.ONLY_FLIPS_UP_DOWN)
			{
				this.outlineEast.SetActive(!this.outlineEast.activeSelf);
			}
			break;
		case DungeonData.Direction.SOUTH:
			if (this.flipStyle != FlippableCover.FlipStyle.ONLY_FLIPS_LEFT_RIGHT)
			{
				this.outlineSouth.SetActive(!this.outlineSouth.activeSelf);
			}
			break;
		case DungeonData.Direction.WEST:
			if (this.flipStyle != FlippableCover.FlipStyle.ONLY_FLIPS_UP_DOWN)
			{
				this.outlineWest.SetActive(!this.outlineWest.activeSelf);
			}
			break;
		}
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006BF5 RID: 27637 RVA: 0x002A7B00 File Offset: 0x002A5D00
	private void Update()
	{
		if (base.spriteAnimator.IsPlaying(base.spriteAnimator.CurrentClip))
		{
			base.spriteAnimator.ForceInvisibleSpriteUpdate();
			if (base.specRigidbody)
			{
				base.specRigidbody.ForceRegenerate(null, null);
			}
		}
		if (this.m_shouldDisplayOutline)
		{
			DungeonData.Direction inverseDirection = DungeonData.GetInverseDirection(this.GetFlipDirection(this.m_lastInteractingPlayer.specRigidbody));
			if (inverseDirection != this.m_lastOutlineDirection)
			{
				this.ToggleOutline(this.m_lastOutlineDirection);
				this.ToggleOutline(inverseDirection);
			}
			this.m_lastOutlineDirection = inverseDirection;
		}
		if (this.m_makeBreakableTimer > 0f)
		{
			this.m_makeBreakableTimer -= BraveTime.DeltaTime;
			if (this.m_makeBreakableTimer <= 0f)
			{
				this.m_breakable.MinHitPointsFromNonExplosions = 0f;
				if (!this.m_flipped && this.m_breakable && !GameManager.Instance.InTutorial)
				{
					this.m_breakable.ApplyDamage(this.DamageReceivedOnSlide, Vector2.zero, false, false, false);
				}
			}
		}
	}

	// Token: 0x06006BF6 RID: 27638 RVA: 0x002A7C30 File Offset: 0x002A5E30
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006BF7 RID: 27639 RVA: 0x002A7C38 File Offset: 0x002A5E38
	public float GetDistanceToPoint(Vector2 point)
	{
		if (base.specRigidbody == null || base.sprite == null)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.sprite.GetBounds().size);
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x06006BF8 RID: 27640 RVA: 0x002A7CB0 File Offset: 0x002A5EB0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006BF9 RID: 27641 RVA: 0x002A7CB8 File Offset: 0x002A5EB8
	public void OnEnteredRange(PlayerController interactor)
	{
		this.m_lastInteractingPlayer = interactor;
		if (!this)
		{
			return;
		}
		this.m_shouldDisplayOutline = true;
	}

	// Token: 0x06006BFA RID: 27642 RVA: 0x002A7CD4 File Offset: 0x002A5ED4
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		this.ClearOutlines();
		this.m_shouldDisplayOutline = false;
	}

	// Token: 0x06006BFB RID: 27643 RVA: 0x002A7CF0 File Offset: 0x002A5EF0
	public void Interact(PlayerController player)
	{
		this.Flip(player.specRigidbody);
		player.DoVibration(Vibration.Time.Quick, Vibration.Strength.UltraLight);
		this.ClearOutlines();
		this.m_shouldDisplayOutline = false;
	}

	// Token: 0x06006BFC RID: 27644 RVA: 0x002A7D14 File Offset: 0x002A5F14
	public DungeonData.Direction GetFlipDirection(SpeculativeRigidbody flipperRigidbody)
	{
		bool flag = flipperRigidbody.UnitRight <= base.specRigidbody.UnitLeft;
		bool flag2 = flipperRigidbody.UnitLeft >= base.specRigidbody.UnitRight;
		bool flag3 = flipperRigidbody.UnitBottom >= base.specRigidbody.UnitTop;
		bool flag4 = flipperRigidbody.UnitTop <= base.specRigidbody.UnitBottom;
		if (flag && !flag3 && !flag4)
		{
			return DungeonData.Direction.EAST;
		}
		if (flag2 && !flag3 && !flag4)
		{
			return DungeonData.Direction.WEST;
		}
		if (flag3 && !flag && !flag2)
		{
			return DungeonData.Direction.SOUTH;
		}
		if (flag4 && !flag && !flag2)
		{
			return DungeonData.Direction.NORTH;
		}
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		PlayerController component = flipperRigidbody.GetComponent<PlayerController>();
		bool flag5 = component && component.IsSlidingOverSurface;
		if (flag && flag3)
		{
			vector = flipperRigidbody.UnitBottomRight;
			vector2 = base.specRigidbody.UnitTopLeft;
		}
		else if (flag2 && flag3)
		{
			vector = flipperRigidbody.UnitBottomLeft;
			vector2 = base.specRigidbody.UnitTopRight;
		}
		else if (flag && flag4)
		{
			vector = flipperRigidbody.UnitTopRight;
			vector2 = base.specRigidbody.UnitBottomLeft;
		}
		else if (flag2 && flag4)
		{
			vector = flipperRigidbody.UnitTopLeft;
			vector2 = base.specRigidbody.UnitBottomRight;
		}
		else if (this.m_slide && flag5)
		{
			vector = flipperRigidbody.UnitCenter;
			vector2 = base.specRigidbody.UnitCenter;
		}
		else
		{
			Debug.LogError("Something about this table and flipper is TOTALLY WRONG MAN (way #1)");
		}
		Vector2 vector3 = vector - vector2;
		if (vector3 == Vector2.zero)
		{
			if (flag4)
			{
				return DungeonData.Direction.NORTH;
			}
			if (flag3)
			{
				return DungeonData.Direction.SOUTH;
			}
		}
		if (this.m_slide && flag5)
		{
			vector3 = -component.Velocity;
			if (!component.IsSlidingOverSurface)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_SLID_OVER_TABLE, 1f);
			}
			component.IsSlidingOverSurface = true;
			if (!component.TablesDamagedThisSlide.Contains(this))
			{
				component.TablesDamagedThisSlide.Add(this);
				if (this.m_breakable && !GameManager.Instance.InTutorial)
				{
					this.m_breakable.ApplyDamage(this.DamageReceivedOnSlide, Vector2.zero, false, false, false);
				}
			}
		}
		Vector2 majorAxis = BraveUtility.GetMajorAxis(vector3);
		if (majorAxis.x < 0f)
		{
			return DungeonData.Direction.EAST;
		}
		if (majorAxis.x > 0f)
		{
			return DungeonData.Direction.WEST;
		}
		if (majorAxis.y < 0f)
		{
			return DungeonData.Direction.NORTH;
		}
		if (majorAxis.y > 0f)
		{
			return DungeonData.Direction.SOUTH;
		}
		Debug.LogError("Something about this table and flipper is TOTALLY WRONG MAN (way #2)");
		return DungeonData.Direction.NORTH;
	}

	// Token: 0x06006BFD RID: 27645 RVA: 0x002A7FFC File Offset: 0x002A61FC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		switch (this.GetFlipDirection(interactor.specRigidbody))
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
		return "error";
	}

	// Token: 0x06006BFE RID: 27646 RVA: 0x002A8060 File Offset: 0x002A6260
	private void MakePerpendicularOnFlipped(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		base.sprite.IsPerpendicular = true;
		if (this.m_flipDirection == DungeonData.Direction.NORTH || this.m_flipDirection == DungeonData.Direction.SOUTH)
		{
			float num = ((this.m_flipDirection != DungeonData.Direction.NORTH) ? this.CustomSouthFlippedHeightOffGround : this.CustomNorthFlippedHeightOffGround);
			base.sprite.HeightOffGround = ((!this.UsesCustomHeightsOffGround) ? (-1.5f) : num);
			if (this.shadowSprite != null)
			{
				this.shadowSprite.HeightOffGround = ((!this.UsesCustomHeightsOffGround) ? (-1.5f) : (-1.75f));
			}
		}
		else
		{
			float num2 = ((this.m_flipDirection != DungeonData.Direction.EAST) ? this.CustomWestFlippedHeightOffGround : this.CustomEastFlippedHeightOffGround);
			base.sprite.HeightOffGround = ((!this.UsesCustomHeightsOffGround) ? (-1f) : num2);
			if (this.shadowSprite != null)
			{
				this.shadowSprite.HeightOffGround = -1.5f;
			}
		}
		base.sprite.UpdateZDepth();
		if (this.shadowSprite != null)
		{
			this.shadowSprite.UpdateZDepth();
		}
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.MakePerpendicularOnFlipped));
	}

	// Token: 0x06006BFF RID: 27647 RVA: 0x002A81B8 File Offset: 0x002A63B8
	private IEnumerator DelayedMakePerpendicular(float time)
	{
		yield return new WaitForSeconds(time);
		this.MakePerpendicularOnFlipped(null, null);
		yield break;
	}

	// Token: 0x06006C00 RID: 27648 RVA: 0x002A81DC File Offset: 0x002A63DC
	private IEnumerator DelayedBreakBreakables(float time)
	{
		yield return new WaitForSeconds(time);
		this.BreakBreakablesFlippedUpon(this.m_flipDirection);
		yield break;
	}

	// Token: 0x06006C01 RID: 27649 RVA: 0x002A8200 File Offset: 0x002A6400
	public void Flip(DungeonData.Direction flipDirection)
	{
		if (this.IsFlipped)
		{
			return;
		}
		if (GameManager.Instance.InTutorial)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerFlippedTable");
		}
		AkSoundEngine.PostEvent("Play_OBJ_table_flip_01", base.gameObject);
		GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round)).DeregisterInteractable(this);
		if (this.m_breakable != null)
		{
			this.m_breakable.TriggerTemporaryDestructibleVFXClear();
		}
		this.m_flipDirection = flipDirection;
		if (!string.IsNullOrEmpty(this.flipAnimation))
		{
			tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.FlipCompleted));
			base.spriteAnimator.Play(this.GetAnimName(this.flipAnimation, this.m_flipDirection));
			if (this.m_flipDirection == DungeonData.Direction.NORTH)
			{
				this.MakePerpendicularOnFlipped(null, null);
			}
			else
			{
				base.StartCoroutine(this.DelayedMakePerpendicular(base.spriteAnimator.CurrentClip.BaseClipLength / 2.25f));
			}
			base.StartCoroutine(this.DelayedBreakBreakables(base.spriteAnimator.CurrentClip.BaseClipLength / 2f));
			if (this.m_flipDirection == DungeonData.Direction.SOUTH)
			{
				base.sprite.IsPerpendicular = true;
			}
		}
		else
		{
			base.sprite.IsPerpendicular = true;
			if (this.m_flipDirection == DungeonData.Direction.NORTH || this.m_flipDirection == DungeonData.Direction.SOUTH)
			{
				float num = ((this.m_flipDirection != DungeonData.Direction.NORTH) ? this.CustomSouthFlippedHeightOffGround : this.CustomNorthFlippedHeightOffGround);
				base.sprite.HeightOffGround = ((!this.UsesCustomHeightsOffGround) ? (-1.5f) : num);
			}
			this.BreakBreakablesFlippedUpon(this.m_flipDirection);
			this.FlipCompleted(null, null);
		}
		if (this.m_flipperPlayer && this.m_flipperPlayer.OnTableFlipped != null)
		{
			this.m_flipperPlayer.OnTableFlipped(this);
		}
		if (!string.IsNullOrEmpty(this.shadowFlipAnimation) && this.m_shadowSpriteAnimator != null)
		{
			this.m_shadowSpriteAnimator.Play(this.GetAnimName(this.shadowFlipAnimation, this.m_flipDirection));
		}
		bool flag = false;
		for (int i = 0; i < this.flipSubElements.Count; i++)
		{
			if ((this.flipSubElements[i].isMandatory || UnityEngine.Random.value < this.flipSubElements[i].spawnChance) && (!this.flipSubElements[i].requiresDirection || this.flipSubElements[i].requiredDirection == flipDirection))
			{
				if (this.flipSubElements[i].onlyOneOfThese)
				{
					if (flag)
					{
						goto IL_2D6;
					}
					flag = true;
				}
				base.StartCoroutine(this.ProcessSubElement(this.flipSubElements[i], flipDirection));
			}
			IL_2D6:;
		}
		this.m_occupiedCells.UpdateCells();
		if (this.DelayMoveable)
		{
			base.StartCoroutine(this.HandleDelayedMoveability());
		}
		else
		{
			base.specRigidbody.CanBePushed = true;
		}
		if (this.m_flipperPlayer)
		{
			base.StartCoroutine(this.HandleDelayedVibration(this.m_flipperPlayer));
		}
		if (base.specRigidbody.PixelColliders.Count >= 2)
		{
			base.specRigidbody.PixelColliders[1].Enabled = true;
		}
		this.m_flipped = true;
		base.sprite.UpdateZDepth();
		if (this.shadowSprite)
		{
			this.shadowSprite.UpdateZDepth();
		}
		SurfaceDecorator component = base.GetComponent<SurfaceDecorator>();
		if (component != null)
		{
			component.Destabilize(DungeonData.GetIntVector2FromDirection(this.m_flipDirection).ToVector2());
		}
	}

	// Token: 0x06006C02 RID: 27650 RVA: 0x002A85D8 File Offset: 0x002A67D8
	private IEnumerator HandleDelayedMoveability()
	{
		yield return new WaitForSeconds(this.MoveableDelay);
		base.specRigidbody.CanBePushed = true;
		yield break;
	}

	// Token: 0x06006C03 RID: 27651 RVA: 0x002A85F4 File Offset: 0x002A67F4
	private IEnumerator HandleDelayedVibration(PlayerController player)
	{
		yield return new WaitForSeconds(this.VibrationDelay);
		if (player)
		{
			player.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
		}
		yield break;
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x002A8618 File Offset: 0x002A6818
	private IEnumerator ProcessSubElement(FlippableSubElement element, DungeonData.Direction flipDirection)
	{
		yield return new WaitForSeconds(element.flipDelay);
		element.Trigger(flipDirection, base.sprite);
		yield break;
	}

	// Token: 0x06006C05 RID: 27653 RVA: 0x002A8644 File Offset: 0x002A6844
	public void ForceSetFlipper(PlayerController flipper)
	{
		this.m_flipperPlayer = flipper;
	}

	// Token: 0x06006C06 RID: 27654 RVA: 0x002A8650 File Offset: 0x002A6850
	public void Flip(SpeculativeRigidbody flipperRigidbody)
	{
		if (this.IsFlipped)
		{
			return;
		}
		base.specRigidbody.PixelColliders[1].Enabled = true;
		this.RemoveFromRoomHierarchy();
		DungeonData.Direction flipDirection = this.GetFlipDirection(flipperRigidbody);
		if (this.flipStyle == FlippableCover.FlipStyle.NO_FLIPS)
		{
			return;
		}
		if (this.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_LEFT_RIGHT)
		{
			if (flipDirection == DungeonData.Direction.NORTH || flipDirection == DungeonData.Direction.SOUTH)
			{
				return;
			}
		}
		else if (this.flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_UP_DOWN && (flipDirection == DungeonData.Direction.EAST || flipDirection == DungeonData.Direction.WEST))
		{
			return;
		}
		AkSoundEngine.PostEvent("Play_OBJ_table_flip_01", base.gameObject);
		if (this.m_breakable != null)
		{
			this.m_breakable.TriggerTemporaryDestructibleVFXClear();
		}
		if (flipperRigidbody.gameActor && flipperRigidbody.gameActor is PlayerController)
		{
			this.m_flipperPlayer = flipperRigidbody.gameActor as PlayerController;
			this.ForceBlank(2f, 0.5f);
			this.m_flipperPlayer.healthHaver.TriggerInvulnerabilityPeriod(-1f);
		}
		this.Flip(flipDirection);
		GameActor gameActor = flipperRigidbody.gameActor;
		if (gameActor is PlayerController)
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.TABLES_FLIPPED, 1f);
		}
	}

	// Token: 0x17001004 RID: 4100
	// (get) Token: 0x06006C07 RID: 27655 RVA: 0x002A8784 File Offset: 0x002A6984
	public bool IsGilded
	{
		get
		{
			return this.m_isGilded;
		}
	}

	// Token: 0x06006C08 RID: 27656 RVA: 0x002A878C File Offset: 0x002A698C
	private void FlipCompleted(tk2dSpriteAnimator tk2DSpriteAnimator, tk2dSpriteAnimationClip tk2DSpriteAnimationClip)
	{
		this.m_occupiedCells.UpdateCells();
		base.sprite.UpdateZDepth();
		if (this.m_flipperPlayer && this.m_flipperPlayer.OnTableFlipCompleted != null)
		{
			this.m_flipperPlayer.OnTableFlipCompleted(this);
		}
		if (this.m_isGilded)
		{
			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			if (absoluteRoom != null)
			{
				List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					if (activeEnemies[i] && activeEnemies[i].IsNormalEnemy)
					{
						activeEnemies[i].AssignedCurrencyToDrop += UnityEngine.Random.Range(2, 6);
					}
				}
			}
			this.m_isGilded = false;
		}
		tk2DSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(tk2DSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.FlipCompleted));
	}

	// Token: 0x06006C09 RID: 27657 RVA: 0x002A8888 File Offset: 0x002A6A88
	private void BreakBreakablesFlippedUpon(DungeonData.Direction flipDirection)
	{
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		for (int i = 0; i < StaticReferenceManager.AllMinorBreakables.Count; i++)
		{
			if (!StaticReferenceManager.AllMinorBreakables[i].IsBroken && !StaticReferenceManager.AllMinorBreakables[i].debris && StaticReferenceManager.AllMinorBreakables[i].transform.position.GetAbsoluteRoom() == absoluteRoom)
			{
				SpeculativeRigidbody specRigidbody = StaticReferenceManager.AllMinorBreakables[i].specRigidbody;
				if (specRigidbody && base.specRigidbody)
				{
					if (BraveMathCollege.DistBetweenRectangles(specRigidbody.UnitBottomLeft, specRigidbody.UnitDimensions, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions) < 0.5f)
					{
						StaticReferenceManager.AllMinorBreakables[i].Break();
					}
				}
			}
		}
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x002A8984 File Offset: 0x002A6B84
	private void RemoveFromRoomHierarchy()
	{
		Transform hierarchyParent = base.transform.position.GetAbsoluteRoom().hierarchyParent;
		Transform transform = base.transform;
		while (transform.parent != null)
		{
			if (transform.parent == hierarchyParent)
			{
				transform.parent = null;
				break;
			}
			transform = transform.parent;
		}
	}

	// Token: 0x06006C0B RID: 27659 RVA: 0x002A89E8 File Offset: 0x002A6BE8
	public void Damaged(float damage)
	{
		if (this.m_flipped || this.prebreakFramesUnflipped == null || this.prebreakFramesUnflipped.Length == 0)
		{
			for (int i = this.prebreakFrames.Length - 1; i >= 0; i--)
			{
				if (this.m_breakable.GetCurrentHealthPercentage() <= this.prebreakFrames[i].healthPercentage / 100f)
				{
					if (this.m_flipped)
					{
						base.sprite.SetSprite(this.GetAnimName(this.prebreakFrames[i].sprite, this.m_flipDirection));
					}
					if (i == this.prebreakFrames.Length - 1 && this.m_makeBreakableTimer <= 0f)
					{
						this.m_makeBreakableTimer = 0.5f;
					}
					return;
				}
			}
		}
		else
		{
			for (int j = this.prebreakFramesUnflipped.Length - 1; j >= 0; j--)
			{
				if (this.m_breakable.GetCurrentHealthPercentage() <= this.prebreakFramesUnflipped[j].healthPercentage / 100f)
				{
					string sprite = this.prebreakFramesUnflipped[j].sprite;
					base.sprite.SetSprite(sprite);
					if (j == this.prebreakFramesUnflipped.Length - 1 && this.m_makeBreakableTimer <= 0f)
					{
						this.m_makeBreakableTimer = 0.5f;
					}
					return;
				}
			}
		}
	}

	// Token: 0x06006C0C RID: 27660 RVA: 0x002A8B4C File Offset: 0x002A6D4C
	public void DestroyCover()
	{
		this.DestroyCover(false, null);
	}

	// Token: 0x06006C0D RID: 27661 RVA: 0x002A8B6C File Offset: 0x002A6D6C
	public void DestroyCover(bool fellInPit, IntVector2? pushDirection)
	{
		if (!this.m_flipped)
		{
			SurfaceDecorator component = base.GetComponent<SurfaceDecorator>();
			if (component != null)
			{
				component.Destabilize(Vector2.zero);
			}
			this.ClearOutlines();
			GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round)).DeregisterInteractable(this);
		}
		this.m_occupiedCells.Clear();
		if (fellInPit && pushDirection != null)
		{
			base.StartCoroutine(this.StartFallAnimation(pushDirection.Value));
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.PUSH_TABLE_INTO_PIT, 0);
		}
		else if (!this.m_flipped && base.spriteAnimator.GetClipByName(this.unflippedBreakAnimation) == null)
		{
			LootEngine.DoDefaultPurplePoof(base.sprite.WorldCenter, false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			base.spriteAnimator.Play(this.m_flipped ? this.GetAnimName(this.breakAnimation, this.m_flipDirection) : this.unflippedBreakAnimation);
			if (this.BreaksOnBreakAnimation)
			{
				tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
				spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.DestroyBrokenTable));
			}
		}
		if (this.shadowSprite)
		{
			this.shadowSprite.renderer.enabled = false;
		}
		base.sprite.IsPerpendicular = false;
		base.sprite.HeightOffGround = -1.25f;
		base.sprite.UpdateZDepth();
		this.ForceBlank(2f, 0.5f);
	}

	// Token: 0x06006C0E RID: 27662 RVA: 0x002A8D18 File Offset: 0x002A6F18
	private void DestroyBrokenTable(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006C0F RID: 27663 RVA: 0x002A8D28 File Offset: 0x002A6F28
	public void ForceBlank(float overrideRadius = 25f, float overrideTimeAtMaxRadius = 0.5f)
	{
		GameObject gameObject = new GameObject("silencer");
		SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
		silencerInstance.ForceNoDamage = true;
		silencerInstance.TriggerSilencer(base.specRigidbody.UnitCenter, 50f, overrideRadius, null, 0f, 0f, 0f, 0f, 0f, 0f, overrideTimeAtMaxRadius, null, false, true);
	}

	// Token: 0x06006C10 RID: 27664 RVA: 0x002A8D88 File Offset: 0x002A6F88
	public void ConfigureOnPlacement(RoomHandler room)
	{
		base.specRigidbody.Initialize();
		this.m_occupiedCells = new OccupiedCells(base.specRigidbody, room);
	}

	// Token: 0x06006C11 RID: 27665 RVA: 0x002A8DA8 File Offset: 0x002A6FA8
	protected virtual void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		PlayerController component = rigidbodyCollision.OtherRigidbody.GetComponent<PlayerController>();
		if (component != null && rigidbodyCollision.Overlap)
		{
			component.specRigidbody.RegisterGhostCollisionException(base.specRigidbody);
		}
	}

	// Token: 0x06006C12 RID: 27666 RVA: 0x002A8DEC File Offset: 0x002A6FEC
	protected virtual void OnPostMovement(SpeculativeRigidbody rigidbody, Vector2 unitOffset, IntVector2 pixelOffset)
	{
		if (pixelOffset != IntVector2.Zero)
		{
			this.CheckForPitDeath(pixelOffset);
		}
		if (rigidbody.CanBePushed && base.sprite)
		{
			base.sprite.UpdateZDepth();
		}
		if (this.shadowSprite)
		{
			this.shadowSprite.transform.localPosition = base.sprite.transform.localPosition;
			this.shadowSprite.UpdateZDepth();
		}
		if (unitOffset != Vector2.zero)
		{
			this.m_occupiedCells.UpdateCells();
		}
	}

	// Token: 0x06006C13 RID: 27667 RVA: 0x002A8E8C File Offset: 0x002A708C
	private string GetAnimName(string name, DungeonData.Direction dir)
	{
		if (name.Contains("{0}"))
		{
			return string.Format(name, dir.ToString().ToLower());
		}
		return name;
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x002A8EB8 File Offset: 0x002A70B8
	private void CheckForPitDeath(IntVector2 dir)
	{
		if (base.specRigidbody.PixelColliders.Count == 0)
		{
			return;
		}
		if (this.PreventPitFalls)
		{
			return;
		}
		Rect rect = default(Rect);
		rect.min = base.specRigidbody.PixelColliders[0].UnitBottomLeft;
		rect.max = base.specRigidbody.PixelColliders[0].UnitTopRight;
		for (int i = 1; i < base.specRigidbody.PixelColliders.Count; i++)
		{
			rect.min = Vector2.Min(rect.min, base.specRigidbody.PixelColliders[i].UnitBottomLeft);
			rect.max = Vector2.Max(rect.max, base.specRigidbody.PixelColliders[i].UnitTopRight);
		}
		Dungeon dungeon = GameManager.Instance.Dungeon;
		List<IntVector2> list = new List<IntVector2>();
		if (dungeon.CellSupportsFalling(new Vector2(rect.xMin, rect.yMin)) && dungeon.CellSupportsFalling(new Vector2(rect.xMin, rect.yMax)) && dungeon.CellSupportsFalling(new Vector2(rect.center.x, rect.yMin)) && dungeon.CellSupportsFalling(new Vector2(rect.center.x, rect.yMax)))
		{
			list.Add(IntVector2.Left);
		}
		else if (dungeon.CellSupportsFalling(new Vector2(rect.xMax, rect.yMin)) && dungeon.CellSupportsFalling(new Vector2(rect.xMax, rect.yMax)) && dungeon.CellSupportsFalling(new Vector2(rect.center.x, rect.yMin)) && dungeon.CellSupportsFalling(new Vector2(rect.center.x, rect.yMax)))
		{
			list.Add(IntVector2.Right);
		}
		else if (dungeon.CellSupportsFalling(new Vector2(rect.xMin, rect.yMax)) && dungeon.CellSupportsFalling(new Vector2(rect.xMax, rect.yMax)) && dungeon.CellSupportsFalling(new Vector2(rect.xMin, rect.center.y)) && dungeon.CellSupportsFalling(new Vector2(rect.xMax, rect.center.y)))
		{
			list.Add(IntVector2.Up);
		}
		else if (dungeon.CellSupportsFalling(new Vector2(rect.xMin, rect.yMin)) && dungeon.CellSupportsFalling(new Vector2(rect.xMax, rect.yMin)) && dungeon.CellSupportsFalling(new Vector2(rect.xMin, rect.center.y)) && dungeon.CellSupportsFalling(new Vector2(rect.xMax, rect.center.y)))
		{
			list.Add(IntVector2.Down);
		}
		if (list.Count > 0)
		{
			IntVector2 intVector;
			if (list.Contains(dir.MajorAxis))
			{
				intVector = dir.MajorAxis;
			}
			else
			{
				intVector = list[0];
			}
			this.DestroyCover(true, new IntVector2?(intVector));
		}
	}

	// Token: 0x06006C15 RID: 27669 RVA: 0x002A92A4 File Offset: 0x002A74A4
	private IEnumerator StartFallAnimation(IntVector2 dir)
	{
		base.specRigidbody.enabled = false;
		base.sprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
		float duration = 0.5f;
		float rotation = ((dir.x == 0) ? 0f : (-Mathf.Sign((float)dir.x) * 135f));
		Vector3 velocity = dir.ToVector3() * 1.25f / duration;
		Vector3 acceleration = new Vector3(0f, -10f, 0f);
		Vector3 cachedVector = base.sprite.transform.position;
		base.transform.position = base.sprite.WorldCenter;
		base.sprite.transform.position = cachedVector;
		float timer = 0f;
		while (timer < duration)
		{
			base.transform.position += velocity * BraveTime.DeltaTime;
			base.transform.eulerAngles = base.transform.eulerAngles.WithZ(Mathf.Lerp(0f, rotation, timer / duration));
			base.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, timer / duration);
			yield return null;
			timer += BraveTime.DeltaTime;
			velocity += acceleration * BraveTime.DeltaTime;
		}
		GameManager.Instance.Dungeon.tileIndices.DoSplashAtPosition(base.transform.position);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040068E9 RID: 26857
	public FlippableCover.FlipStyle flipStyle;

	// Token: 0x040068EA RID: 26858
	public tk2dSprite shadowSprite;

	// Token: 0x040068EB RID: 26859
	public float DamageReceivedOnSlide = 30f;

	// Token: 0x040068EC RID: 26860
	[Header("Unflipped Animations")]
	public string unflippedBreakAnimation;

	// Token: 0x040068ED RID: 26861
	[Header("Directional Animations")]
	[HelpBox("{0} = east/west/south/north")]
	public string flipAnimation;

	// Token: 0x040068EE RID: 26862
	public string shadowFlipAnimation;

	// Token: 0x040068EF RID: 26863
	public string pitfallAnimation;

	// Token: 0x040068F0 RID: 26864
	public string breakAnimation;

	// Token: 0x040068F1 RID: 26865
	public BreakFrame[] prebreakFrames;

	// Token: 0x040068F2 RID: 26866
	public BreakFrame[] prebreakFramesUnflipped;

	// Token: 0x040068F3 RID: 26867
	public bool BreaksOnBreakAnimation;

	// Token: 0x040068F4 RID: 26868
	[Header("SubElements (for coffins)")]
	public List<FlippableSubElement> flipSubElements = new List<FlippableSubElement>();

	// Token: 0x040068F5 RID: 26869
	[Header("Directional Outline Sprite")]
	public GameObject outlineNorth;

	// Token: 0x040068F6 RID: 26870
	public GameObject outlineEast;

	// Token: 0x040068F7 RID: 26871
	public GameObject outlineSouth;

	// Token: 0x040068F8 RID: 26872
	public GameObject outlineWest;

	// Token: 0x040068F9 RID: 26873
	public bool UsesCustomHeightsOffGround;

	// Token: 0x040068FA RID: 26874
	public float CustomStartHeightOffGround;

	// Token: 0x040068FB RID: 26875
	public float CustomNorthFlippedHeightOffGround = -1.5f;

	// Token: 0x040068FC RID: 26876
	public float CustomEastFlippedHeightOffGround = -1.5f;

	// Token: 0x040068FD RID: 26877
	public float CustomSouthFlippedHeightOffGround = -1.5f;

	// Token: 0x040068FE RID: 26878
	public float CustomWestFlippedHeightOffGround = -1.5f;

	// Token: 0x040068FF RID: 26879
	public bool DelayMoveable;

	// Token: 0x04006900 RID: 26880
	public float MoveableDelay = 1f;

	// Token: 0x04006901 RID: 26881
	public float VibrationDelay = 0.25f;

	// Token: 0x04006902 RID: 26882
	private SlideSurface m_slide;

	// Token: 0x04006904 RID: 26884
	private bool m_hasRoomEnteredProcessed;

	// Token: 0x04006905 RID: 26885
	private bool m_isGilded;

	// Token: 0x04006906 RID: 26886
	private PlayerController m_flipperPlayer;

	// Token: 0x04006907 RID: 26887
	protected tk2dSpriteAnimator m_shadowSpriteAnimator;

	// Token: 0x04006908 RID: 26888
	protected OccupiedCells m_occupiedCells;

	// Token: 0x04006909 RID: 26889
	protected MajorBreakable m_breakable;

	// Token: 0x0400690A RID: 26890
	protected bool m_flipped;

	// Token: 0x0400690B RID: 26891
	protected DungeonData.Direction m_flipDirection;

	// Token: 0x0400690C RID: 26892
	protected bool m_shouldDisplayOutline;

	// Token: 0x0400690D RID: 26893
	protected PlayerController m_lastInteractingPlayer;

	// Token: 0x0400690E RID: 26894
	protected DungeonData.Direction m_lastOutlineDirection = (DungeonData.Direction)(-1);

	// Token: 0x0400690F RID: 26895
	protected float m_makeBreakableTimer = -1f;

	// Token: 0x020012D6 RID: 4822
	public enum FlipStyle
	{
		// Token: 0x04006911 RID: 26897
		ANY,
		// Token: 0x04006912 RID: 26898
		ONLY_FLIPS_UP_DOWN,
		// Token: 0x04006913 RID: 26899
		ONLY_FLIPS_LEFT_RIGHT,
		// Token: 0x04006914 RID: 26900
		NO_FLIPS
	}
}
