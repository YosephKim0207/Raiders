using System;
using System.Collections;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000185 RID: 389
[InspectorDropdownName("Bosses/DraGun/Roar1")]
public class DraGunRoar1 : Script
{
	// Token: 0x060005D6 RID: 1494 RVA: 0x0001C5F8 File Offset: 0x0001A7F8
	protected override IEnumerator Top()
	{
		if (DraGunRoar1.s_xValues == null || DraGunRoar1.s_yValues == null)
		{
			DraGunRoar1.s_xValues = new int[this.NumRockets];
			DraGunRoar1.s_yValues = new int[this.NumRockets];
			for (int j = 0; j < this.NumRockets; j++)
			{
				DraGunRoar1.s_xValues[j] = j;
				DraGunRoar1.s_yValues[j] = j;
			}
		}
		DraGunController dragunController = base.BulletBank.GetComponent<DraGunController>();
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		Vector2 roomLowerLeft = area.UnitBottomLeft + new Vector2(2f, 21f);
		Vector2 dimensions = new Vector2(32f, 6f);
		Vector2 delta = new Vector2(dimensions.x / (float)this.NumRockets, dimensions.y / (float)this.NumRockets);
		BraveUtility.RandomizeArray<int>(DraGunRoar1.s_xValues, 0, -1);
		BraveUtility.RandomizeArray<int>(DraGunRoar1.s_yValues, 0, -1);
		for (int i = 0; i < this.NumRockets; i++)
		{
			int baseX = DraGunRoar1.s_xValues[i];
			int baseY = DraGunRoar1.s_yValues[i];
			Vector2 goalPos = roomLowerLeft + new Vector2(((float)baseX + UnityEngine.Random.value) * delta.x, ((float)baseY + UnityEngine.Random.value) * delta.y);
			this.FireRocket(dragunController.skyBoulder, goalPos);
			yield return base.Wait(10);
		}
		yield break;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0001C614 File Offset: 0x0001A814
	private void FireRocket(GameObject skyRocket, Vector2 target)
	{
		SkyRocket component = SpawnManager.SpawnProjectile(skyRocket, base.Position, Quaternion.identity, true).GetComponent<SkyRocket>();
		component.TargetVector2 = target;
		tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
		component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
		component.ExplosionData.ignoreList.Add(base.BulletBank.specRigidbody);
	}

	// Token: 0x040005B3 RID: 1459
	public int NumRockets = 3;

	// Token: 0x040005B4 RID: 1460
	private static int[] s_xValues;

	// Token: 0x040005B5 RID: 1461
	private static int[] s_yValues;
}
