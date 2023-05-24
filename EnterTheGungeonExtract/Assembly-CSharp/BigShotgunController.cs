using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013A0 RID: 5024
public class BigShotgunController : MonoBehaviour
{
	// Token: 0x060071D4 RID: 29140 RVA: 0x002D3C84 File Offset: 0x002D1E84
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x060071D5 RID: 29141 RVA: 0x002D3C94 File Offset: 0x002D1E94
	private void LateUpdate()
	{
		if (this.m_gun && this.m_gun.IsReloading && this.m_gun.CurrentOwner is PlayerController)
		{
			PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
			if (playerController.CurrentRoom != null)
			{
				playerController.CurrentRoom.ApplyActionToNearbyEnemies(playerController.CenterPosition, this.SuckRadius, new Action<AIActor, float>(this.ProcessEnemy));
			}
		}
	}

	// Token: 0x060071D6 RID: 29142 RVA: 0x002D3D18 File Offset: 0x002D1F18
	private void ProcessEnemy(AIActor target, float distance)
	{
		for (int i = 0; i < this.TargetEnemies.Length; i++)
		{
			if (target.EnemyGuid == this.TargetEnemies[i])
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(target));
				target.EraseFromExistence(true);
				break;
			}
		}
	}

	// Token: 0x060071D7 RID: 29143 RVA: 0x002D3D7C File Offset: 0x002D1F7C
	private IEnumerator HandleEnemySuck(AIActor target)
	{
		Transform copySprite = this.CreateEmptySprite(target);
		Vector3 startPosition = copySprite.transform.position;
		float elapsed = 0f;
		float duration = 0.5f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (this.m_gun && copySprite)
			{
				Vector3 position = this.m_gun.PrimaryHandAttachPoint.position;
				float num = elapsed / duration * (elapsed / duration);
				copySprite.position = Vector3.Lerp(startPosition, position, num);
				copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
				copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), num);
			}
			yield return null;
		}
		if (copySprite)
		{
			UnityEngine.Object.Destroy(copySprite.gameObject);
		}
		if (this.m_gun)
		{
			this.m_gun.GainAmmo(1);
		}
		yield break;
	}

	// Token: 0x060071D8 RID: 29144 RVA: 0x002D3DA0 File Offset: 0x002D1FA0
	private Transform CreateEmptySprite(AIActor target)
	{
		GameObject gameObject = new GameObject("suck image");
		gameObject.layer = target.gameObject.layer;
		tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
		gameObject.transform.parent = SpawnManager.Instance.VFX;
		tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
		tk2dSprite.transform.position = target.sprite.transform.position;
		GameObject gameObject2 = new GameObject("image parent");
		gameObject2.transform.position = tk2dSprite.WorldCenter;
		tk2dSprite.transform.parent = gameObject2.transform;
		if (target.optionalPalette != null)
		{
			tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
		}
		return gameObject2.transform;
	}

	// Token: 0x04007346 RID: 29510
	[EnemyIdentifier]
	public string[] TargetEnemies;

	// Token: 0x04007347 RID: 29511
	public float SuckRadius = 8f;

	// Token: 0x04007348 RID: 29512
	private Gun m_gun;
}
