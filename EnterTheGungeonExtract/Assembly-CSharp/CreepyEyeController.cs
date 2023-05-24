using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200128D RID: 4749
[ExecuteInEditMode]
public class CreepyEyeController : MonoBehaviour
{
	// Token: 0x06006A4D RID: 27213 RVA: 0x0029AE54 File Offset: 0x00299054
	private void Start()
	{
		this.m_parentRoom = base.transform.position.GetAbsoluteRoom();
		this.m_parentRoom.Entered += this.HandlePlayerEntered;
	}

	// Token: 0x06006A4E RID: 27214 RVA: 0x0029AE84 File Offset: 0x00299084
	private void HandlePlayerEntered(PlayerController p)
	{
		if (this.m_alreadyWarpingAutomatically)
		{
			return;
		}
		base.StartCoroutine(this.HandleAutowarpOut());
	}

	// Token: 0x06006A4F RID: 27215 RVA: 0x0029AEA0 File Offset: 0x002990A0
	private IEnumerator HandleAutowarpOut()
	{
		this.m_alreadyWarpingAutomatically = true;
		yield return new WaitForSeconds(15f);
		this.m_alreadyWarpingAutomatically = false;
		if (GameManager.Instance.BestActivePlayer.CurrentRoom == this.m_parentRoom)
		{
			for (int i = 0; i < Minimap.Instance.roomsContainingTeleporters.Count; i++)
			{
				RoomHandler roomHandler = Minimap.Instance.roomsContainingTeleporters[i];
				if (roomHandler.TeleportersActive)
				{
					TeleporterController teleporterController = ((!roomHandler.hierarchyParent) ? null : roomHandler.hierarchyParent.GetComponentInChildren<TeleporterController>(true));
					if (!teleporterController)
					{
						List<TeleporterController> componentsInRoom = roomHandler.GetComponentsInRoom<TeleporterController>();
						if (componentsInRoom.Count > 0)
						{
							teleporterController = componentsInRoom[0];
						}
					}
					if (teleporterController)
					{
						Vector2 vector = teleporterController.GetComponent<tk2dBaseSprite>().WorldCenter;
						vector -= GameManager.Instance.BestActivePlayer.SpriteDimensions.XY().WithY(0f) / 2f;
						GameManager.Instance.BestActivePlayer.TeleportToPoint(vector, true);
					}
					break;
				}
			}
		}
		yield break;
	}

	// Token: 0x06006A50 RID: 27216 RVA: 0x0029AEBC File Offset: 0x002990BC
	private void LateUpdate()
	{
		if (Application.isPlaying)
		{
			Vector2 vector = GameManager.Instance.PrimaryPlayer.CenterPosition - base.transform.position.XY();
			float num = Mathf.Lerp(0f, this.MaxPupilRadius, vector.magnitude / 7f);
			this.poopil.transform.localPosition = num * vector.normalized;
		}
		float x = this.baseSprite.GetBounds().extents.x;
		float x2 = this.poopil.GetComponent<tk2dSprite>().GetBounds().extents.x;
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.layers[i].sprite == null)
			{
				this.layers[i].sprite = this.layers[i].xform.GetComponent<tk2dSprite>();
			}
			float x3 = this.layers[i].sprite.GetBounds().extents.x;
			float num2 = 1f - x3 / x;
			float num3 = (float)i / ((float)this.layers.Length - 1f);
			num2 = Mathf.Pow(num2, Mathf.Lerp(0.75f, 1f, 1f - num3));
			float num4 = this.poopil.localPosition.magnitude / (x - x2);
			this.layers[i].xform.localPosition = this.poopil.localPosition * num2 + this.poopil.localPosition.normalized * x2 * num4 * num2;
			this.layers[i].xform.localPosition = this.layers[i].xform.localPosition.Quantize(0.0625f);
			this.layers[i].sprite.HeightOffGround = (float)i * 0.1f + 0.1f;
			this.layers[i].sprite.UpdateZDepth();
		}
		this.poopil.GetComponent<tk2dSprite>().HeightOffGround = 1f;
		this.poopil.GetComponent<tk2dSprite>().UpdateZDepth();
	}

	// Token: 0x040066C9 RID: 26313
	public const float c_TimeBeforeWarp = 15f;

	// Token: 0x040066CA RID: 26314
	public float MaxPupilRadius = 2.5f;

	// Token: 0x040066CB RID: 26315
	public CreepyEyeLayer[] layers;

	// Token: 0x040066CC RID: 26316
	public Transform poopil;

	// Token: 0x040066CD RID: 26317
	public tk2dSprite baseSprite;

	// Token: 0x040066CE RID: 26318
	private RoomHandler m_parentRoom;

	// Token: 0x040066CF RID: 26319
	private bool m_alreadyWarpingAutomatically;
}
