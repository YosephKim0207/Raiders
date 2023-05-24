using System;
using System.Collections;
using UnityEngine;

// Token: 0x020014B4 RID: 5300
public class SpawnObjectOnRollItem : PassiveItem
{
	// Token: 0x06007882 RID: 30850 RVA: 0x00302CA4 File Offset: 0x00300EA4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		player.OnRollStarted += this.OnRollStarted;
		base.Pickup(player);
	}

	// Token: 0x06007883 RID: 30851 RVA: 0x00302CCC File Offset: 0x00300ECC
	private void OnRollStarted(PlayerController obj, Vector2 dirVec)
	{
		if (this.ObjectToSpawn)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ObjectToSpawn, obj.transform.position, Quaternion.identity);
			gameObject.GetComponent<tk2dSprite>().PlaceAtPositionByAnchor(obj.CenterPosition, tk2dBaseSprite.Anchor.MiddleCenter);
			if (this.DoBounce)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleObjectBounce(gameObject.transform));
			}
		}
	}

	// Token: 0x06007884 RID: 30852 RVA: 0x00302D44 File Offset: 0x00300F44
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.OnRollStarted -= this.OnRollStarted;
		debrisObject.GetComponent<SpawnObjectOnRollItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007885 RID: 30853 RVA: 0x00302D78 File Offset: 0x00300F78
	private IEnumerator HandleObjectBounce(Transform target)
	{
		float elapsed = 0f;
		Vector3 startPos = target.position;
		Vector3 adjPos = startPos;
		float yVelocity = this.BounceStartVelocity;
		while (elapsed < this.BounceDuration && target)
		{
			elapsed += BraveTime.DeltaTime;
			yVelocity -= this.GravityAcceleration * BraveTime.DeltaTime;
			adjPos += new Vector3(0f, yVelocity * BraveTime.DeltaTime, 0f);
			target.position = adjPos;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007886 RID: 30854 RVA: 0x00302D9C File Offset: 0x00300F9C
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			this.m_owner.OnRollStarted -= this.OnRollStarted;
		}
		base.OnDestroy();
	}

	// Token: 0x04007AAE RID: 31406
	public GameObject ObjectToSpawn;

	// Token: 0x04007AAF RID: 31407
	public bool DoBounce;

	// Token: 0x04007AB0 RID: 31408
	public float BounceDuration = 1f;

	// Token: 0x04007AB1 RID: 31409
	public float BounceStartVelocity = 5f;

	// Token: 0x04007AB2 RID: 31410
	public float GravityAcceleration = 10f;
}
