using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001337 RID: 4919
public class ActiveSummonItem : PlayerItem
{
	// Token: 0x06006F87 RID: 28551 RVA: 0x002C320C File Offset: 0x002C140C
	public override bool CanBeUsed(PlayerController user)
	{
		return user.CurrentRoom != null && user.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear) && base.CanBeUsed(user);
	}

	// Token: 0x06006F88 RID: 28552 RVA: 0x002C3238 File Offset: 0x002C1438
	private void CreateCompanion(PlayerController owner)
	{
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
		IntVector2 intVector = IntVector2.Max(this.CustomClearance, orLoadByGuid.Clearance);
		IntVector2? intVector2 = owner.CurrentRoom.GetNearestAvailableCell(owner.transform.position.XY(), new IntVector2?(intVector), new CellTypes?(CellTypes.FLOOR), false, null);
		if (intVector2 == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, (intVector2.Value.ToVector2() + this.CustomOffset).ToVector3ZUp(0f), Quaternion.identity);
		this.m_extantCompanion = gameObject;
		CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
		orAddComponent.companionID = CompanionController.CompanionIdentifier.GATLING_GULL;
		orAddComponent.Initialize(owner);
		if (this.IsTimed)
		{
			owner.StartCoroutine(this.HandleLifespan(gameObject, owner));
		}
		if (!string.IsNullOrEmpty(this.IntroDirectionalAnimation))
		{
			AIAnimator component = orAddComponent.GetComponent<AIAnimator>();
			component.PlayUntilFinished(this.IntroDirectionalAnimation, true, null, -1f, false);
		}
		if (this.HasDoubleSynergy && owner.HasActiveBonusSynergy(this.DoubleSynergy, false))
		{
			intVector2 = owner.CurrentRoom.GetNearestAvailableCell(owner.transform.position.XY() + new Vector2(-1f, -1f), new IntVector2?(intVector), new CellTypes?(CellTypes.FLOOR), false, null);
			if (intVector2 == null)
			{
				return;
			}
			this.m_extantSecondCompanion = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, (intVector2.Value.ToVector2() + this.CustomOffset).ToVector3ZUp(0f), Quaternion.identity);
			CompanionController orAddComponent2 = this.m_extantSecondCompanion.GetOrAddComponent<CompanionController>();
			orAddComponent2.Initialize(owner);
			if (!string.IsNullOrEmpty(this.IntroDirectionalAnimation))
			{
				AIAnimator component2 = orAddComponent2.GetComponent<AIAnimator>();
				component2.PlayUntilFinished(this.IntroDirectionalAnimation, true, null, -1f, false);
			}
		}
	}

	// Token: 0x06006F89 RID: 28553 RVA: 0x002C3428 File Offset: 0x002C1628
	private void DestroyCompanion()
	{
		if (this.m_extantCompanion)
		{
			if (!string.IsNullOrEmpty(this.OutroDirectionalAnimation))
			{
				AIAnimator component = this.m_extantCompanion.GetComponent<AIAnimator>();
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleDeparture(true, component));
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_extantCompanion);
				this.m_extantCompanion = null;
			}
		}
		if (this.m_extantSecondCompanion)
		{
			if (!string.IsNullOrEmpty(this.OutroDirectionalAnimation))
			{
				AIAnimator component2 = this.m_extantSecondCompanion.GetComponent<AIAnimator>();
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleDeparture(false, component2));
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_extantSecondCompanion);
				this.m_extantSecondCompanion = null;
			}
		}
	}

	// Token: 0x06006F8A RID: 28554 RVA: 0x002C34EC File Offset: 0x002C16EC
	private IEnumerator HandleDeparture(bool isPrimary, AIAnimator anim)
	{
		anim.behaviorSpeculator.enabled = false;
		anim.specRigidbody.Velocity = Vector2.zero;
		anim.aiActor.ClearPath();
		anim.PlayForDuration(this.OutroDirectionalAnimation, 3f, true, null, -1f, false);
		float animLength = anim.GetDirectionalAnimationLength(this.OutroDirectionalAnimation);
		GameObject extantCompanion;
		if (isPrimary)
		{
			extantCompanion = this.m_extantCompanion;
			this.m_extantCompanion = null;
		}
		else
		{
			extantCompanion = this.m_extantSecondCompanion;
			this.m_extantSecondCompanion = null;
		}
		float elapsed = 0f;
		while (elapsed < animLength)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		GameObject instanceVFXObject = null;
		if (anim)
		{
			instanceVFXObject = UnityEngine.Object.Instantiate<GameObject>(this.DepartureVFXPrefab);
			tk2dBaseSprite component = instanceVFXObject.GetComponent<tk2dBaseSprite>();
			component.transform.position = anim.sprite.transform.position;
		}
		UnityEngine.Object.Destroy(extantCompanion);
		if (instanceVFXObject)
		{
			Vector3 startPosition = instanceVFXObject.transform.position;
			elapsed = 0f;
			while (elapsed < 1.5f)
			{
				elapsed += BraveTime.DeltaTime;
				instanceVFXObject.transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0f, 75f, 0f), elapsed / 1.5f);
				yield return null;
			}
			UnityEngine.Object.Destroy(instanceVFXObject);
		}
		yield break;
	}

	// Token: 0x06006F8B RID: 28555 RVA: 0x002C3518 File Offset: 0x002C1718
	protected override void DoEffect(PlayerController user)
	{
		this.DestroyCompanion();
		this.CreateCompanion(user);
	}

	// Token: 0x06006F8C RID: 28556 RVA: 0x002C3528 File Offset: 0x002C1728
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
		if (base.IsCurrentlyActive)
		{
			base.IsCurrentlyActive = false;
			if (this.m_extantCompanion)
			{
				this.DestroyCompanion();
			}
		}
	}

	// Token: 0x06006F8D RID: 28557 RVA: 0x002C355C File Offset: 0x002C175C
	private IEnumerator HandleLifespan(GameObject targetCompanion, PlayerController owner)
	{
		base.IsCurrentlyActive = true;
		float elapsed = 0f;
		this.m_activeDuration = this.Lifespan;
		this.m_activeElapsed = 0f;
		while (elapsed < this.Lifespan)
		{
			elapsed += BraveTime.DeltaTime;
			this.m_activeElapsed = elapsed;
			if (!owner || owner.CurrentRoom == null || !owner.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
			{
				break;
			}
			yield return null;
		}
		base.IsCurrentlyActive = false;
		if (this.m_extantCompanion == targetCompanion)
		{
			this.DestroyCompanion();
		}
		yield break;
	}

	// Token: 0x06006F8E RID: 28558 RVA: 0x002C3588 File Offset: 0x002C1788
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006EBD RID: 28349
	[EnemyIdentifier]
	public string CompanionGuid;

	// Token: 0x04006EBE RID: 28350
	public bool HasDoubleSynergy;

	// Token: 0x04006EBF RID: 28351
	[LongNumericEnum]
	public CustomSynergyType DoubleSynergy;

	// Token: 0x04006EC0 RID: 28352
	public IntVector2 CustomClearance;

	// Token: 0x04006EC1 RID: 28353
	public Vector2 CustomOffset;

	// Token: 0x04006EC2 RID: 28354
	public bool IsTimed;

	// Token: 0x04006EC3 RID: 28355
	public float Lifespan = 60f;

	// Token: 0x04006EC4 RID: 28356
	public string IntroDirectionalAnimation;

	// Token: 0x04006EC5 RID: 28357
	public string OutroDirectionalAnimation;

	// Token: 0x04006EC6 RID: 28358
	public GameObject DepartureVFXPrefab;

	// Token: 0x04006EC7 RID: 28359
	private GameObject m_extantCompanion;

	// Token: 0x04006EC8 RID: 28360
	private GameObject m_extantSecondCompanion;

	// Token: 0x04006EC9 RID: 28361
	private bool m_synergyActive;
}
