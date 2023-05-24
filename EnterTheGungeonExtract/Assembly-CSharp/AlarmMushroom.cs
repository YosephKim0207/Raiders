using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020010E4 RID: 4324
public class AlarmMushroom : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06005F3A RID: 24378 RVA: 0x0024A3C4 File Offset: 0x002485C4
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerCollision));
	}

	// Token: 0x06005F3B RID: 24379 RVA: 0x0024A3F0 File Offset: 0x002485F0
	private void HandleTriggerCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_triggered)
		{
			return;
		}
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (component)
		{
			base.StartCoroutine(this.Trigger());
		}
	}

	// Token: 0x06005F3C RID: 24380 RVA: 0x0024A428 File Offset: 0x00248628
	private IEnumerator Trigger()
	{
		if (this.m_triggered)
		{
			yield break;
		}
		base.spriteAnimator.Play("alarm_mushroom_alarm");
		this.m_triggered = true;
		if (this.TriggerVFX)
		{
			SpawnManager.SpawnVFX(this.TriggerVFX, base.specRigidbody.UnitCenter + new Vector2(0f, 2f), Quaternion.identity);
		}
		yield return new WaitForSeconds(1f);
		RobotDaveIdea targetIdea = ((!GameManager.Instance.Dungeon.UsesCustomFloorIdea) ? GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultProceduralIdea : GameManager.Instance.Dungeon.FloorIdea);
		DungeonPlaceable backupEnemyPlaceable = targetIdea.ValidEasyEnemyPlaceables[UnityEngine.Random.Range(0, targetIdea.ValidEasyEnemyPlaceables.Length)];
		DungeonPlaceableVariant variant = backupEnemyPlaceable.SelectFromTiersFull();
		AIActor selectedEnemy = variant.GetOrLoadPlaceableObject.GetComponent<AIActor>();
		if (selectedEnemy)
		{
			AIActor aiactor = AIActor.Spawn(selectedEnemy, base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor) + new IntVector2(0, 2), this.m_room, true, AIActor.AwakenAnimationType.Default, true);
			aiactor.HandleReinforcementFallIntoRoom(0f);
		}
		yield return new WaitForSeconds(1f);
		this.DestroyMushroom();
		yield break;
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x0024A444 File Offset: 0x00248644
	private void DestroyMushroom()
	{
		if (this.DestroyVFX)
		{
			SpawnManager.SpawnVFX(this.DestroyVFX, base.specRigidbody.UnitCenter, Quaternion.identity);
		}
		base.spriteAnimator.PlayAndDestroyObject("alarm_mushroom_break", null);
	}

	// Token: 0x06005F3E RID: 24382 RVA: 0x0024A494 File Offset: 0x00248694
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005F3F RID: 24383 RVA: 0x0024A49C File Offset: 0x0024869C
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
	}

	// Token: 0x040059A3 RID: 22947
	public GameObject TriggerVFX;

	// Token: 0x040059A4 RID: 22948
	public GameObject DestroyVFX;

	// Token: 0x040059A5 RID: 22949
	private bool m_triggered;

	// Token: 0x040059A6 RID: 22950
	private RoomHandler m_room;
}
