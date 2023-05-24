using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EB8 RID: 3768
	[Serializable]
	public class VisualStyleImpactData
	{
		// Token: 0x06004FAA RID: 20394 RVA: 0x001BACDC File Offset: 0x001B8EDC
		public void SpawnRandomVertical(Vector3 position, float rotation, Transform enemy, Vector2 sourceNormal, Vector2 sourceVelocity)
		{
			VFXComplex vfxcomplex = this.fallbackVerticalTileMapEffects[UnityEngine.Random.Range(0, this.fallbackVerticalTileMapEffects.Length)];
			float num = (float)(Mathf.FloorToInt(position.y) - 1);
			vfxcomplex.SpawnAtPosition(position.x, num, position.y - num, rotation, null, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), false, null, false);
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x001BAD3C File Offset: 0x001B8F3C
		public void SpawnRandomHorizontal(Vector3 position, float rotation, Transform enemy, Vector2 sourceNormal, Vector2 sourceVelocity)
		{
			VFXComplex vfxcomplex = this.fallbackHorizontalTileMapEffects[UnityEngine.Random.Range(0, this.fallbackHorizontalTileMapEffects.Length)];
			vfxcomplex.SpawnAtPosition(position, rotation, enemy, new Vector2?(sourceNormal), new Vector2?(sourceVelocity), null, false, null, null, false);
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x001BAD84 File Offset: 0x001B8F84
		public void SpawnRandomShard(Vector3 position, Vector2 collisionNormal)
		{
			GameObject gameObject = this.wallShards[UnityEngine.Random.Range(0, this.wallShards.Length)];
			if (gameObject != null)
			{
				GameObject gameObject2 = SpawnManager.SpawnDebris(gameObject, position, Quaternion.identity);
				DebrisObject component = gameObject2.GetComponent<DebrisObject>();
				component.angularVelocity = UnityEngine.Random.Range(0.5f, 1.5f) * component.angularVelocity;
				float num = ((Mathf.Abs(collisionNormal.y) <= 0.1f) ? 0f : 0.25f);
				component.Trigger(Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-30, 30)) * collisionNormal.ToVector3ZUp(0f) * UnityEngine.Random.Range(0f, 4f), UnityEngine.Random.Range(0.1f, 0.5f) + num, 1f);
			}
		}

		// Token: 0x040047A2 RID: 18338
		[SerializeField]
		public string annotation;

		// Token: 0x040047A3 RID: 18339
		[SerializeField]
		public GameObject[] wallShards;

		// Token: 0x040047A4 RID: 18340
		[SerializeField]
		public VFXComplex[] fallbackVerticalTileMapEffects;

		// Token: 0x040047A5 RID: 18341
		[SerializeField]
		public VFXComplex[] fallbackHorizontalTileMapEffects;
	}
}
