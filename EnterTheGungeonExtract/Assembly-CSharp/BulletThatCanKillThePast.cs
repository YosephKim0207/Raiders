using System;
using UnityEngine;

// Token: 0x02001365 RID: 4965
public class BulletThatCanKillThePast : PassiveItem
{
	// Token: 0x06007080 RID: 28800 RVA: 0x002CA28C File Offset: 0x002C848C
	private void Start()
	{
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
		if (!this.m_pickedUp)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		}
		Shader.SetGlobalFloat("_MapActive", 0f);
	}

	// Token: 0x06007081 RID: 28801 RVA: 0x002CA2D8 File Offset: 0x002C84D8
	protected override void Update()
	{
		base.Update();
		if (!this.m_pickedUp && base.gameObject.layer != LayerMask.NameToLayer("Unpixelated"))
		{
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
		}
	}

	// Token: 0x06007082 RID: 28802 RVA: 0x002CA328 File Offset: 0x002C8528
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		SimpleSpriteRotator[] componentsInChildren = base.GetComponentsInChildren<SimpleSpriteRotator>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
		GameManager.Instance.PrimaryPlayer.PastAccessible = true;
		Shader.SetGlobalFloat("_MapActive", 1f);
		base.Pickup(player);
	}

	// Token: 0x06007083 RID: 28803 RVA: 0x002CA398 File Offset: 0x002C8598
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<BulletThatCanKillThePast>().m_pickedUpThisRun = true;
		debrisObject.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
		return debrisObject;
	}

	// Token: 0x06007084 RID: 28804 RVA: 0x002CA3D0 File Offset: 0x002C85D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
