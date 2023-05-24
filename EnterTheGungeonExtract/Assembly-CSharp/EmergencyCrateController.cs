using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020013F0 RID: 5104
public class EmergencyCrateController : BraveBehaviour
{
	// Token: 0x060073CA RID: 29642 RVA: 0x002E1058 File Offset: 0x002DF258
	public void Trigger(Vector3 startingVelocity, Vector3 startingPosition, RoomHandler room, bool crateSucks = true)
	{
		this.m_parentRoom = room;
		this.m_currentPosition = startingPosition;
		this.m_currentVelocity = startingVelocity;
		this.m_hasBeenTriggered = true;
		base.spriteAnimator.Play((!crateSucks) ? this.driftAnimationName : this.driftSucksAnimationName);
		this.m_crateSucks = crateSucks;
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
		float num = startingPosition.z / -startingVelocity.z;
		Vector3 vector = startingPosition + num * startingVelocity;
		this.m_landingTarget = SpawnManager.SpawnVFX(this.landingTargetSprite, vector, Quaternion.identity);
		this.m_landingTarget.GetComponentInChildren<tk2dSprite>().UpdateZDepth();
	}

	// Token: 0x060073CB RID: 29643 RVA: 0x002E1108 File Offset: 0x002DF308
	private void Update()
	{
		if (this.m_hasBeenTriggered)
		{
			this.m_currentPosition += this.m_currentVelocity * BraveTime.DeltaTime;
			if (this.m_currentPosition.z <= 0f)
			{
				this.m_currentPosition.z = 0f;
				this.OnLanded();
			}
			base.transform.position = BraveUtility.QuantizeVector(this.m_currentPosition.WithZ(this.m_currentPosition.y - this.m_currentPosition.z), (float)PhysicsEngine.Instance.PixelsPerUnit);
			base.sprite.HeightOffGround = this.m_currentPosition.z;
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x060073CC RID: 29644 RVA: 0x002E11CC File Offset: 0x002DF3CC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060073CD RID: 29645 RVA: 0x002E11D4 File Offset: 0x002DF3D4
	private void OnLanded()
	{
		this.m_hasBeenTriggered = false;
		base.sprite.gameObject.layer = LayerMask.NameToLayer("FG_Critical");
		base.sprite.renderer.sortingLayerName = "Background";
		base.sprite.IsPerpendicular = false;
		base.sprite.HeightOffGround = -1f;
		this.m_currentPosition.z = -1f;
		base.spriteAnimator.Play((!this.m_crateSucks) ? this.landedAnimationName : this.landedSucksAnimationName);
		this.chuteAnimator.PlayAndDestroyObject(this.chuteLandedAnimationName, null);
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget);
		}
		this.m_landingTarget = null;
		if (UnityEngine.Random.value < this.ChanceToExplode)
		{
			Exploder.Explode(base.sprite.WorldCenter, this.ExplosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
			base.StartCoroutine(this.DestroyCrateDelayed());
		}
		else if (UnityEngine.Random.value < this.ChanceToSpawnEnemy)
		{
			DungeonPlaceableVariant dungeonPlaceableVariant = this.EnemyPlaceable.SelectFromTiersFull();
			if (dungeonPlaceableVariant != null && dungeonPlaceableVariant.GetOrLoadPlaceableObject != null)
			{
				AIActor.Spawn(dungeonPlaceableVariant.GetOrLoadPlaceableObject.GetComponent<AIActor>(), base.sprite.WorldCenter.ToIntVector2(VectorConversions.Round), GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.sprite.WorldCenter.ToIntVector2(VectorConversions.Round)), true, AIActor.AwakenAnimationType.Default, true);
			}
			base.StartCoroutine(this.DestroyCrateDelayed());
		}
		else
		{
			GameObject gameObject;
			if (this.usesLootData)
			{
				gameObject = this.lootData.GetItemsForPlayer(GameManager.Instance.PrimaryPlayer, 0, null, null)[0].gameObject;
			}
			else
			{
				gameObject = this.gunTable.SelectByWeight(false);
			}
			if (gameObject.GetComponent<AmmoPickup>() != null)
			{
				AmmoPickup component = LootEngine.SpawnItem(gameObject, base.sprite.WorldCenter.ToVector3ZUp(0f) + new Vector3(-0.5f, 0.5f, 0f), Vector2.zero, 0f, false, false, false).GetComponent<AmmoPickup>();
				base.StartCoroutine(this.DestroyCrateWhenPickedUp(component));
			}
			else if (gameObject.GetComponent<Gun>() != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(base.sprite.WorldCenter.ToVector3ZUp(0f) + new Vector3(0f, 0.5f, 0f), tk2dBaseSprite.Anchor.MiddleCenter);
				Gun component2 = gameObject2.GetComponent<Gun>();
				component2.Initialize(null);
				component2.DropGun(0.5f);
				base.StartCoroutine(this.DestroyCrateWhenPickedUp(component2));
			}
			else
			{
				DebrisObject debrisObject = LootEngine.SpawnItem(gameObject, base.sprite.WorldCenter.ToVector3ZUp(0f) + new Vector3(-0.5f, 0.5f, 0f), Vector2.zero, 0f, false, false, false);
				base.StartCoroutine(this.DestroyCrateWhenPickedUp(debrisObject));
			}
		}
	}

	// Token: 0x060073CE RID: 29646 RVA: 0x002E14F8 File Offset: 0x002DF6F8
	private IEnumerator DestroyCrateDelayed()
	{
		yield return new WaitForSeconds(1.5f);
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget);
		}
		this.m_landingTarget = null;
		if (this.m_parentRoom.ExtantEmergencyCrate == base.gameObject)
		{
			this.m_parentRoom.ExtantEmergencyCrate = null;
		}
		base.spriteAnimator.Play((!this.m_crateSucks) ? this.crateDisappearAnimationName : this.shittyCrateDisappearAnimationName);
		yield break;
	}

	// Token: 0x060073CF RID: 29647 RVA: 0x002E1514 File Offset: 0x002DF714
	private IEnumerator DestroyCrateWhenPickedUp(DebrisObject spawned)
	{
		while (spawned)
		{
			yield return new WaitForSeconds(0.25f);
		}
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget);
		}
		this.m_landingTarget = null;
		if (this.m_parentRoom.ExtantEmergencyCrate == base.gameObject)
		{
			this.m_parentRoom.ExtantEmergencyCrate = null;
		}
		base.spriteAnimator.Play((!this.m_crateSucks) ? this.crateDisappearAnimationName : this.shittyCrateDisappearAnimationName);
		yield break;
	}

	// Token: 0x060073D0 RID: 29648 RVA: 0x002E1538 File Offset: 0x002DF738
	private IEnumerator DestroyCrateWhenPickedUp(AmmoPickup spawnedAmmo)
	{
		while (spawnedAmmo && !spawnedAmmo.pickedUp)
		{
			yield return new WaitForSeconds(0.25f);
		}
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget);
		}
		this.m_landingTarget = null;
		if (this.m_parentRoom.ExtantEmergencyCrate == base.gameObject)
		{
			this.m_parentRoom.ExtantEmergencyCrate = null;
		}
		base.spriteAnimator.Play((!this.m_crateSucks) ? this.crateDisappearAnimationName : this.shittyCrateDisappearAnimationName);
		yield break;
	}

	// Token: 0x060073D1 RID: 29649 RVA: 0x002E155C File Offset: 0x002DF75C
	private IEnumerator DestroyCrateWhenPickedUp(Gun spawnedGun)
	{
		while (spawnedGun && spawnedGun.IsInWorld)
		{
			yield return new WaitForSeconds(0.25f);
		}
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget);
		}
		this.m_landingTarget = null;
		if (this.m_parentRoom.ExtantEmergencyCrate == base.gameObject)
		{
			this.m_parentRoom.ExtantEmergencyCrate = null;
		}
		base.spriteAnimator.Play((!this.m_crateSucks) ? this.crateDisappearAnimationName : this.shittyCrateDisappearAnimationName);
		yield break;
	}

	// Token: 0x060073D2 RID: 29650 RVA: 0x002E1580 File Offset: 0x002DF780
	public void ClearLandingTarget()
	{
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget);
		}
		this.m_landingTarget = null;
	}

	// Token: 0x04007554 RID: 30036
	public string driftAnimationName;

	// Token: 0x04007555 RID: 30037
	public string driftSucksAnimationName;

	// Token: 0x04007556 RID: 30038
	public string landedAnimationName;

	// Token: 0x04007557 RID: 30039
	public string landedSucksAnimationName;

	// Token: 0x04007558 RID: 30040
	public string chuteLandedAnimationName;

	// Token: 0x04007559 RID: 30041
	public string crateDisappearAnimationName;

	// Token: 0x0400755A RID: 30042
	public string shittyCrateDisappearAnimationName;

	// Token: 0x0400755B RID: 30043
	public tk2dSpriteAnimator chuteAnimator;

	// Token: 0x0400755C RID: 30044
	public GameObject landingTargetSprite;

	// Token: 0x0400755D RID: 30045
	public bool usesLootData;

	// Token: 0x0400755E RID: 30046
	public LootData lootData;

	// Token: 0x0400755F RID: 30047
	public GenericLootTable gunTable;

	// Token: 0x04007560 RID: 30048
	public float ChanceToSpawnEnemy;

	// Token: 0x04007561 RID: 30049
	public DungeonPlaceable EnemyPlaceable;

	// Token: 0x04007562 RID: 30050
	public float ChanceToExplode;

	// Token: 0x04007563 RID: 30051
	public ExplosionData ExplosionData;

	// Token: 0x04007564 RID: 30052
	private bool m_hasBeenTriggered;

	// Token: 0x04007565 RID: 30053
	private Vector3 m_currentPosition;

	// Token: 0x04007566 RID: 30054
	private Vector3 m_currentVelocity;

	// Token: 0x04007567 RID: 30055
	private RoomHandler m_parentRoom;

	// Token: 0x04007568 RID: 30056
	private bool m_crateSucks;

	// Token: 0x04007569 RID: 30057
	private GameObject m_landingTarget;
}
