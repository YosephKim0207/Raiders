using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001186 RID: 4486
public class HangingObjectController : DungeonPlaceableBehaviour, IPlayerInteractable, IEventTriggerable, IPlaceConfigurable
{
	// Token: 0x060063A4 RID: 25508 RVA: 0x0026AB24 File Offset: 0x00268D24
	private void Start()
	{
		this.m_currentHeight = this.startingHeight;
		this.m_objectTransform = this.objectSprite.transform;
		this.m_cachedStartingPosition = this.m_objectTransform.position;
		this.objectSprite.HeightOffGround = this.m_currentHeight + 1f;
		this.m_objectTransform.position = this.m_cachedStartingPosition + new Vector3(0f, this.m_currentHeight, 0f);
		this.objectSprite.UpdateZDepth();
		if (this.triggerObjectPrefab != null)
		{
			RoomEventTriggerArea eventTriggerAreaFromObject = this.m_room.GetEventTriggerAreaFromObject(this);
			if (eventTriggerAreaFromObject != null)
			{
				if (eventTriggerAreaFromObject.tempDataObject != null)
				{
					this.m_subsidiary = true;
					GameObject tempDataObject = eventTriggerAreaFromObject.tempDataObject;
					this.TriggerObjectBreakable = tempDataObject.GetComponentInChildren<MinorBreakable>();
					this.TriggerObjectBreakable.OnlyBreaksOnScreen = true;
					MinorBreakable triggerObjectBreakable = this.TriggerObjectBreakable;
					triggerObjectBreakable.OnBreak = (Action)Delegate.Combine(triggerObjectBreakable.OnBreak, new Action(this.TriggerFallBroken));
					tk2dSpriteAnimator component = this.TriggerObjectBreakable.GetComponent<tk2dSpriteAnimator>();
					component.OnPlayAnimationCalled = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(component.OnPlayAnimationCalled, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.SubTriggerAnim));
				}
				else
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.triggerObjectPrefab, eventTriggerAreaFromObject.initialPosition.ToVector3((float)eventTriggerAreaFromObject.initialPosition.y), Quaternion.identity);
					this.TriggerObjectBreakable = gameObject.GetComponentInChildren<MinorBreakable>();
					this.TriggerObjectBreakable.OnlyBreaksOnScreen = true;
					MinorBreakable triggerObjectBreakable2 = this.TriggerObjectBreakable;
					triggerObjectBreakable2.OnBreak = (Action)Delegate.Combine(triggerObjectBreakable2.OnBreak, new Action(this.TriggerFallBroken));
					this.TriggerObjectAnimator = this.TriggerObjectBreakable.GetComponent<tk2dSpriteAnimator>();
					if (this.TriggerObjectBreakable.transform.childCount > 0)
					{
						this.TriggerChainAnimator = this.TriggerObjectBreakable.transform.GetChild(0).GetComponent<tk2dSpriteAnimator>();
					}
					eventTriggerAreaFromObject.tempDataObject = gameObject;
					if (this.TriggerObjectBreakable && this.TriggerObjectBreakable.sprite)
					{
						this.TriggerObjectBreakable.sprite.IsPerpendicular = true;
						this.TriggerObjectBreakable.sprite.UpdateZDepth();
					}
				}
			}
		}
	}

	// Token: 0x060063A5 RID: 25509 RVA: 0x0026AD58 File Offset: 0x00268F58
	private void SubTriggerAnim(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		this.m_subsidiary = true;
		this.Interact(GameManager.Instance.BestActivePlayer);
	}

	// Token: 0x060063A6 RID: 25510 RVA: 0x0026AD74 File Offset: 0x00268F74
	public void Trigger(int index)
	{
		if (this.triggerObjectPrefab == null)
		{
			this.TriggerFallBroken();
		}
	}

	// Token: 0x060063A7 RID: 25511 RVA: 0x0026AD94 File Offset: 0x00268F94
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON && room.RoomVisualSubtype == 6)
		{
			room.RoomVisualSubtype = ((UnityEngine.Random.value <= 0.5f) ? 3 : 0);
			for (int i = 0; i < room.Cells.Count; i++)
			{
				room.UpdateCellVisualData(room.Cells[i].x, room.Cells[i].y);
			}
		}
	}

	// Token: 0x060063A8 RID: 25512 RVA: 0x0026AE34 File Offset: 0x00269034
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.m_hasFallen)
		{
			return 1000f;
		}
		if (this.TriggerObjectAnimator == null)
		{
			return 1000f;
		}
		tk2dBaseSprite sprite = this.TriggerObjectAnimator.Sprite;
		return Vector2.Distance(point, sprite.WorldCenter) / 2f;
	}

	// Token: 0x060063A9 RID: 25513 RVA: 0x0026AE88 File Offset: 0x00269088
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060063AA RID: 25514 RVA: 0x0026AE90 File Offset: 0x00269090
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(this.TriggerObjectAnimator.Sprite, Color.white);
	}

	// Token: 0x060063AB RID: 25515 RVA: 0x0026AEB4 File Offset: 0x002690B4
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.TriggerObjectAnimator.Sprite, false);
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x0026AED4 File Offset: 0x002690D4
	private IEnumerator HandleSubSpriteFall(tk2dSprite targetSprite, float adjustedStartHeight)
	{
		if (this.m_usedDepths == null)
		{
			this.m_usedDepths = new HashSet<float>();
		}
		this.subspritesFalling++;
		targetSprite.transform.parent = targetSprite.transform.parent.parent;
		float curHeight = this.m_currentHeight + adjustedStartHeight;
		float curVel = (float)UnityEngine.Random.Range(0, -3);
		float cachedStartY = targetSprite.transform.localPosition.y - this.m_currentHeight;
		Vector3 startPos = targetSprite.transform.position.WithY(targetSprite.transform.position.y - this.m_currentHeight);
		while (curHeight > 0f)
		{
			curVel += this.GRAVITY_ACCELERATION * BraveTime.DeltaTime;
			curHeight += curVel * BraveTime.DeltaTime;
			curHeight = Mathf.Max(0f, curHeight);
			targetSprite.HeightOffGround = curHeight;
			targetSprite.transform.position = startPos + new Vector3(0f, curHeight, 0f);
			targetSprite.UpdateZDepth();
			yield return null;
			if (!this)
			{
				yield break;
			}
		}
		float finalTargetHeight = -1.5f + cachedStartY;
		while (this.m_usedDepths.Contains(targetSprite.transform.position.y + targetSprite.transform.position.y - finalTargetHeight))
		{
			finalTargetHeight += 0.0625f;
		}
		this.m_usedDepths.Add(targetSprite.transform.position.y + targetSprite.transform.position.y - finalTargetHeight);
		targetSprite.HeightOffGround = finalTargetHeight;
		targetSprite.UpdateZDepth();
		this.subspritesFalling--;
		yield break;
	}

	// Token: 0x060063AD RID: 25517 RVA: 0x0026AF00 File Offset: 0x00269100
	private IEnumerator Fall()
	{
		if (this.DoesTriggerShake && !this.m_subsidiary)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(this.TriggerShake, null, false);
			AkSoundEngine.PostEvent("Play_WPN_grenade_blast_01", base.gameObject);
		}
		if (!this.objectSprite.gameObject.activeSelf)
		{
			this.objectSprite.gameObject.SetActive(true);
		}
		if (this.objectSprite && this.objectSprite.GetComponent<MinorBreakable>())
		{
			this.objectSprite.GetComponent<MinorBreakable>().enabled = true;
		}
		if (this.TriggerObjectAnimator != null)
		{
			this.TriggerObjectAnimator.Play();
		}
		if (this.TriggerChainAnimator != null)
		{
			this.TriggerChainAnimator.PlayAndDestroyObject(string.Empty, null);
		}
		if (this.AdditionalChainDownAnimator != null)
		{
			this.AdditionalChainDownAnimator.Play();
		}
		RelatedObjects related = ((!(this.TriggerChainAnimator != null)) ? null : this.TriggerChainAnimator.GetComponent<RelatedObjects>());
		if (related != null)
		{
			for (int i = 0; i < related.relatedObjects.Length; i++)
			{
				related.relatedObjects[i].SetActive(true);
			}
		}
		float m_currentVelocity = 0f;
		if (this.hasSubSprites)
		{
			for (int j = 0; j < this.subSprites.Length; j++)
			{
				base.StartCoroutine(this.HandleSubSpriteFall(this.subSprites[j], this.subSprites[j].transform.localPosition.y + 2f + UnityEngine.Random.Range(-1f, 1.5f)));
			}
		}
		bool hasDisabledParticles = false;
		while (this.m_currentHeight > 0f)
		{
			m_currentVelocity += this.GRAVITY_ACCELERATION * BraveTime.DeltaTime;
			this.m_currentHeight += m_currentVelocity * BraveTime.DeltaTime;
			this.m_currentHeight = Mathf.Max(0f, this.m_currentHeight);
			this.objectSprite.HeightOffGround = this.m_currentHeight;
			if (this.m_objectTransform)
			{
				this.m_objectTransform.position = this.m_cachedStartingPosition + new Vector3(0f, this.m_currentHeight, 0f);
			}
			if (this.m_currentHeight < 5f && !hasDisabledParticles)
			{
				hasDisabledParticles = true;
				for (int k = 0; k < this.additionalDestroyObjects.Length; k++)
				{
					if (this.additionalDestroyObjects[k])
					{
						if (this.additionalDestroyObjects[k].GetComponent<ParticleSystem>())
						{
							BraveUtility.EnableEmission(this.additionalDestroyObjects[k].GetComponent<ParticleSystem>(), false);
							this.additionalDestroyObjects[k] = null;
						}
					}
				}
			}
			if (this.objectSprite)
			{
				this.objectSprite.UpdateZDepth();
			}
			yield return null;
			if (!this)
			{
				yield break;
			}
		}
		if (this.hasSubSprites)
		{
			AkSoundEngine.PostEvent("Play_OBJ_boulder_crash_01", GameManager.Instance.PrimaryPlayer.gameObject);
		}
		if (!this.objectSprite)
		{
			yield break;
		}
		MinorBreakable breakable = this.objectSprite.GetComponent<MinorBreakable>();
		if (breakable != null)
		{
			breakable.Break();
		}
		if (this.DoExplosion)
		{
			if (this.m_subsidiary)
			{
				this.explosionData.doScreenShake = false;
			}
			this.explosionData.overrideRangeIndicatorEffect = this.replacementRangeEffect;
			Exploder.Explode(this.objectSprite.WorldCenter.ToVector3ZUp(0f) + this.explosionOffset, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
		}
		while (this.subspritesFalling > 0)
		{
			yield return null;
			if (!this)
			{
				yield break;
			}
		}
		if (this.destroyOnFinish)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (this.objectSprite)
		{
			this.objectSprite.HeightOffGround = ((!this.hasSubSprites) ? (-1.5f) : (-1.625f));
			this.objectSprite.UpdateZDepth();
		}
		for (int l = 0; l < this.additionalDestroyObjects.Length; l++)
		{
			if (this.additionalDestroyObjects[l])
			{
				UnityEngine.Object.Destroy(this.additionalDestroyObjects[l]);
			}
		}
		if (this.EnableRigidbodyPostFall)
		{
			this.EnableRigidbodyPostFall.enabled = true;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.EnableRigidbodyPostFall, null, false);
			this.EnableRigidbodyPostFall.FlagCellsOccupied();
		}
		yield return new WaitForSeconds(0.25f);
		if (!this)
		{
			yield break;
		}
		if (this.MakeMajorBreakableAfterwards)
		{
			MajorBreakable component = this.objectSprite.GetComponent<MajorBreakable>();
			if (this.EnableRigidbodyPostFall)
			{
				this.EnableRigidbodyPostFall.majorBreakable = component;
			}
			component.enabled = true;
			MajorBreakable majorBreakable = component;
			majorBreakable.OnBreak = (Action)Delegate.Combine(majorBreakable.OnBreak, new Action(this.HandleSubspritesLaunch));
		}
		yield break;
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x0026AF1C File Offset: 0x0026911C
	private void HandleSubspritesLaunch()
	{
		if (this.EnableRigidbodyPostFall)
		{
			this.EnableRigidbodyPostFall.enabled = false;
		}
		for (int i = 0; i < this.subSprites.Length; i++)
		{
			if (this.subSprites[i])
			{
				MinorBreakable component = this.subSprites[i].GetComponent<MinorBreakable>();
				if (component)
				{
					component.enabled = true;
				}
				DebrisObject orAddComponent = this.subSprites[i].gameObject.GetOrAddComponent<DebrisObject>();
				AkSoundEngine.PostEvent("Play_OBJ_boulder_break_01", base.gameObject);
				orAddComponent.angularVelocity = 45f;
				orAddComponent.angularVelocityVariance = 20f;
				orAddComponent.decayOnBounce = 0.5f;
				orAddComponent.bounceCount = 1;
				orAddComponent.canRotate = true;
				orAddComponent.shouldUseSRBMotion = true;
				orAddComponent.AssignFinalWorldDepth(-0.5f);
				orAddComponent.sprite = this.subSprites[i];
				orAddComponent.Trigger((this.subSprites[i].WorldCenter - this.objectSprite.WorldCenter).ToVector3ZUp(0.5f) * (float)UnityEngine.Random.Range(1, 2), (float)UnityEngine.Random.Range(1, 2), 1f);
			}
		}
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x0026B050 File Offset: 0x00269250
	public void Interact(PlayerController interactor)
	{
		this.TriggerFallInteracted();
	}

	// Token: 0x060063B0 RID: 25520 RVA: 0x0026B058 File Offset: 0x00269258
	protected void TriggerFallInteracted()
	{
		if (this.m_hasFallen)
		{
			return;
		}
		this.m_hasFallen = true;
		if (this.TriggerObjectBreakable)
		{
			if (this.TriggerObjectBreakable.specRigidbody)
			{
				this.TriggerObjectBreakable.specRigidbody.enabled = false;
			}
			this.TriggerObjectBreakable.CleanupCallbacks();
			UnityEngine.Object.Destroy(this.TriggerObjectBreakable);
		}
		this.m_room.DeregisterInteractable(this);
		base.StartCoroutine(this.Fall());
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x0026B0E0 File Offset: 0x002692E0
	protected void TriggerFallBroken()
	{
		if (this.m_hasFallen)
		{
			return;
		}
		this.m_hasFallen = true;
		this.m_room.DeregisterInteractable(this);
		base.StartCoroutine(this.Fall());
	}

	// Token: 0x060063B2 RID: 25522 RVA: 0x0026B110 File Offset: 0x00269310
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060063B3 RID: 25523 RVA: 0x0026B11C File Offset: 0x0026931C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005F33 RID: 24371
	public tk2dSprite objectSprite;

	// Token: 0x04005F34 RID: 24372
	public tk2dSpriteAnimator AdditionalChainDownAnimator;

	// Token: 0x04005F35 RID: 24373
	public bool destroyOnFinish = true;

	// Token: 0x04005F36 RID: 24374
	public GameObject[] additionalDestroyObjects;

	// Token: 0x04005F37 RID: 24375
	public bool hasSubSprites;

	// Token: 0x04005F38 RID: 24376
	public tk2dSprite[] subSprites;

	// Token: 0x04005F39 RID: 24377
	public float startingHeight = 5f;

	// Token: 0x04005F3A RID: 24378
	public bool DoExplosion = true;

	// Token: 0x04005F3B RID: 24379
	public ExplosionData explosionData;

	// Token: 0x04005F3C RID: 24380
	public Vector3 explosionOffset;

	// Token: 0x04005F3D RID: 24381
	public GameObject replacementRangeEffect;

	// Token: 0x04005F3E RID: 24382
	public GameObject triggerObjectPrefab;

	// Token: 0x04005F3F RID: 24383
	public bool DoesTriggerShake;

	// Token: 0x04005F40 RID: 24384
	public ScreenShakeSettings TriggerShake;

	// Token: 0x04005F41 RID: 24385
	public SpeculativeRigidbody EnableRigidbodyPostFall;

	// Token: 0x04005F42 RID: 24386
	public bool MakeMajorBreakableAfterwards;

	// Token: 0x04005F43 RID: 24387
	protected Transform m_objectTransform;

	// Token: 0x04005F44 RID: 24388
	protected Vector3 m_cachedStartingPosition;

	// Token: 0x04005F45 RID: 24389
	protected float m_currentHeight;

	// Token: 0x04005F46 RID: 24390
	protected bool m_hasFallen;

	// Token: 0x04005F47 RID: 24391
	protected RoomHandler m_room;

	// Token: 0x04005F48 RID: 24392
	protected MinorBreakable TriggerObjectBreakable;

	// Token: 0x04005F49 RID: 24393
	protected tk2dSpriteAnimator TriggerObjectAnimator;

	// Token: 0x04005F4A RID: 24394
	protected tk2dSpriteAnimator TriggerChainAnimator;

	// Token: 0x04005F4B RID: 24395
	private bool m_subsidiary;

	// Token: 0x04005F4C RID: 24396
	private float GRAVITY_ACCELERATION = -10f;

	// Token: 0x04005F4D RID: 24397
	private int subspritesFalling;

	// Token: 0x04005F4E RID: 24398
	private HashSet<float> m_usedDepths;
}
