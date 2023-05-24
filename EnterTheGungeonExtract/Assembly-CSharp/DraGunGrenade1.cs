using System;
using System.Collections;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000174 RID: 372
[InspectorDropdownName("Bosses/DraGun/Grenade1")]
public class DraGunGrenade1 : Script
{
	// Token: 0x0600058D RID: 1421 RVA: 0x0001A800 File Offset: 0x00018A00
	protected override IEnumerator Top()
	{
		bool reverse = BraveUtility.RandomBool();
		base.StartTask(this.FireWave(reverse, false, 0f));
		yield return base.Wait(75);
		base.StartTask(this.FireWave(reverse, true, 0.25f));
		yield return base.Wait(120);
		yield break;
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0001A81C File Offset: 0x00018A1C
	private IEnumerator FireWave(bool reverse, bool offset, float sinOffset)
	{
		DraGunController dragunController = base.BulletBank.GetComponent<DraGunController>();
		CellArea area = base.BulletBank.aiActor.ParentRoom.area;
		Vector2 start = area.UnitBottomLeft + new Vector2(1f, 25.5f);
		for (int i = 0; i < ((!offset) ? this.NumRockets : (this.NumRockets - 1)); i++)
		{
			float t = ((!offset) ? ((float)i) : ((float)i + 0.5f)) / ((float)this.NumRockets - 1f);
			float dx = 34f * t;
			float dy = Mathf.Sin((t * 2.5f + sinOffset) * 3.1415927f) * this.Magnitude;
			if (reverse)
			{
				dx = 34f - dx;
			}
			this.FireRocket(dragunController.skyRocket, start + new Vector2(dx, dy));
			this.FireRocket(dragunController.skyRocket, start + new Vector2(dx, -dy));
			if (Mathf.Abs(dy) < 1f)
			{
				this.FireRocket(dragunController.skyRocket, start + new Vector2(dx, this.Magnitude));
				this.FireRocket(dragunController.skyRocket, start + new Vector2(dx, -this.Magnitude));
			}
			yield return base.Wait(15);
		}
		yield break;
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0001A84C File Offset: 0x00018A4C
	private void FireRocket(GameObject skyRocket, Vector2 target)
	{
		SkyRocket component = SpawnManager.SpawnProjectile(skyRocket, base.Position, Quaternion.identity, true).GetComponent<SkyRocket>();
		component.TargetVector2 = target;
		tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
		component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
		component.ExplosionData.ignoreList.Add(base.BulletBank.specRigidbody);
	}

	// Token: 0x0400054F RID: 1359
	public int NumRockets = 11;

	// Token: 0x04000550 RID: 1360
	public float Magnitude = 4.5f;
}
