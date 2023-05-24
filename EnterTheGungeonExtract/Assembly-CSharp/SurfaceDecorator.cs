using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200121B RID: 4635
public class SurfaceDecorator : BraveBehaviour
{
	// Token: 0x17000F56 RID: 3926
	// (get) Token: 0x060067AC RID: 26540 RVA: 0x00288FAC File Offset: 0x002871AC
	public bool IsDestabilized
	{
		get
		{
			return this.m_destabilized;
		}
	}

	// Token: 0x060067AD RID: 26541 RVA: 0x00288FB4 File Offset: 0x002871B4
	private SurfaceDecorator.ObjectPlacementData GetSurfaceObject(Vector2 availableSpace, out Vector2 objectDimensions, out Vector2 localOrigin)
	{
		List<GameObject> list = new List<GameObject>();
		bool flag = false;
		int num = 0;
		GenericLootTable overrideTableTable = this.tableTable;
		if (this.m_parentRoom.RoomMaterial.overrideTableTable != null)
		{
			overrideTableTable = this.m_parentRoom.RoomMaterial.overrideTableTable;
		}
		while (!flag && num < 1000)
		{
			GameObject gameObject = overrideTableTable.SelectByWeightWithoutDuplicates(list, false);
			if (gameObject == null)
			{
				break;
			}
			list.Add(gameObject);
			DebrisObject component = gameObject.GetComponent<DebrisObject>();
			MinorBreakableGroupManager component2 = gameObject.GetComponent<MinorBreakableGroupManager>();
			if (component2 != null)
			{
				objectDimensions = component2.GetDimensions();
				if (objectDimensions == Vector2.zero)
				{
					continue;
				}
				localOrigin = Vector2.zero;
			}
			else
			{
				tk2dSprite component3 = gameObject.GetComponent<tk2dSprite>();
				Bounds bounds = component3.GetBounds();
				objectDimensions = new Vector2(bounds.size.x, bounds.size.y);
				localOrigin = bounds.min.XY();
			}
			bool flag2 = objectDimensions.x <= availableSpace.x && objectDimensions.y <= availableSpace.y;
			bool flag3 = component != null && component.placementOptions.canBeRotated && objectDimensions.x <= availableSpace.y && objectDimensions.y <= availableSpace.x;
			if (flag2 || flag3)
			{
				SurfaceDecorator.ObjectPlacementData objectPlacementData = new SurfaceDecorator.ObjectPlacementData(gameObject);
				if (flag2 && flag3)
				{
					objectPlacementData.rotated = UnityEngine.Random.value > 0.5f;
				}
				else
				{
					objectPlacementData.rotated = !flag2;
				}
				if (objectPlacementData.rotated)
				{
					objectDimensions = new Vector2(objectDimensions.y, objectDimensions.x);
					localOrigin = new Vector2(localOrigin.y, localOrigin.x);
				}
				if (component != null && component.placementOptions.canBeFlippedHorizontally)
				{
					objectPlacementData.horizontalFlip = UnityEngine.Random.value > 0.5f;
				}
				if (component != null && component.placementOptions.canBeFlippedVertically)
				{
					objectPlacementData.verticalFlip = UnityEngine.Random.value > 0.5f;
				}
				return objectPlacementData;
			}
			num++;
		}
		objectDimensions = new Vector2(float.MaxValue, float.MaxValue);
		localOrigin = Vector2.zero;
		return null;
	}

	// Token: 0x060067AE RID: 26542 RVA: 0x00289274 File Offset: 0x00287474
	public void RegisterAdditionalObject(GameObject o)
	{
		if (this.m_surfaceObjects.Contains(o))
		{
			return;
		}
		tk2dSprite[] componentsInChildren = o.GetComponentsInChildren<tk2dSprite>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!this.m_attachedSprites.Contains(componentsInChildren[i]))
			{
				if (componentsInChildren[i].attachParent == null)
				{
					componentsInChildren[i].HeightOffGround = 0.1f;
					this.m_attachedSprites.Add(componentsInChildren[i]);
				}
			}
		}
		this.m_surfaceObjects.Add(o);
	}

	// Token: 0x060067AF RID: 26543 RVA: 0x00289300 File Offset: 0x00287500
	private void PostProcessObject(GameObject placedObject, SurfaceDecorator.ObjectPlacementData placementData)
	{
		MinorBreakableGroupManager component = placedObject.GetComponent<MinorBreakableGroupManager>();
		if (component != null)
		{
			component.Initialize();
			tk2dSprite[] componentsInChildren = component.GetComponentsInChildren<tk2dSprite>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].attachParent == null)
				{
					componentsInChildren[i].HeightOffGround = 0.1f;
					this.parentSprite.AttachRenderer(componentsInChildren[i]);
					this.m_attachedSprites.Add(componentsInChildren[i]);
				}
			}
			MinorBreakable[] componentsInChildren2 = component.GetComponentsInChildren<MinorBreakable>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].IgnoredForPotShotsModifier = true;
				componentsInChildren2[j].heightOffGround = 0.75f;
				componentsInChildren2[j].isImpermeableToGameActors = true;
				componentsInChildren2[j].parentSurface = this;
				componentsInChildren2[j].specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBreakable;
			}
			DebrisObject[] componentsInChildren3 = component.GetComponentsInChildren<DebrisObject>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].InitializeForCollisions();
				componentsInChildren3[k].additionalHeightBoost = 0.25f;
			}
		}
		else
		{
			tk2dSprite component2 = placedObject.GetComponent<tk2dSprite>();
			component2.HeightOffGround = 0.1f;
			if (!placementData.rotated)
			{
				if (placementData.horizontalFlip)
				{
					Vector2 vector = component2.GetBounds().min.XY();
					component2.FlipX = true;
					Vector2 vector2 = component2.GetBounds().min.XY();
					Vector2 vector3 = vector2 - vector;
					component2.transform.position = component2.transform.position - vector3.ToVector3ZUp(0f);
				}
				if (placementData.verticalFlip)
				{
					Vector2 vector4 = component2.GetBounds().min.XY();
					component2.FlipY = true;
					Vector2 vector5 = component2.GetBounds().min.XY();
					Vector2 vector6 = vector5 - vector4;
					component2.transform.position = component2.transform.position - vector6.ToVector3ZUp(0f);
				}
			}
			this.parentSprite.AttachRenderer(component2);
			this.m_attachedSprites.Add(component2);
			MinorBreakable component3 = placedObject.GetComponent<MinorBreakable>();
			if (component3 != null)
			{
				component3.IgnoredForPotShotsModifier = true;
				component3.heightOffGround = 0.75f;
				component3.isImpermeableToGameActors = true;
				component3.parentSurface = this;
				component3.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBreakable;
			}
			DebrisObject component4 = placedObject.GetComponent<DebrisObject>();
			if (component4 != null)
			{
				component4.InitializeForCollisions();
				component4.additionalHeightBoost = 0.25f;
			}
		}
		GenericDecorator[] componentsInChildren4 = placedObject.GetComponentsInChildren<GenericDecorator>();
		for (int l = 0; l < componentsInChildren4.Length; l++)
		{
			componentsInChildren4[l].parentSurface = this;
			componentsInChildren4[l].ConfigureOnPlacement(this.m_parentRoom);
		}
	}

	// Token: 0x060067B0 RID: 26544 RVA: 0x002895F0 File Offset: 0x002877F0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060067B1 RID: 26545 RVA: 0x002895F8 File Offset: 0x002877F8
	public void Decorate(RoomHandler parentRoom)
	{
		if (GameManager.Options.DebrisQuantity == GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			return;
		}
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round);
		if (GameManager.Instance.Dungeon.data.CheckInBounds(intVector) && GameManager.Instance.Dungeon.data[intVector].cellVisualData.containsObjectSpaceStamp && GameManager.Instance.Dungeon.data[intVector].cellVisualData.containsWallSpaceStamp)
		{
			return;
		}
		if (UnityEngine.Random.value < this.chanceToDecorate)
		{
			this.m_parentRoom = parentRoom;
			if (this.parentSprite == null)
			{
				this.parentSprite = base.GetComponent<tk2dSprite>();
			}
			if (this.tableTable == null)
			{
				BraveUtility.Log("Trying to decorate a SurfaceDecorator at: " + base.gameObject.name + " and failing.", Color.red, BraveUtility.LogVerbosity.CHATTY);
				return;
			}
			this.m_surfaceObjects = new List<GameObject>();
			this.m_attachedSprites = new List<tk2dSprite>();
			bool flag = this.localSurfaceDimensions.x >= this.localSurfaceDimensions.y;
			float num = 0f;
			float num2 = PhysicsEngine.PixelToUnit((!flag) ? this.localSurfaceDimensions.y : this.localSurfaceDimensions.x);
			float num3 = PhysicsEngine.PixelToUnit((!flag) ? this.localSurfaceDimensions.x : this.localSurfaceDimensions.y);
			float num4 = PhysicsEngine.PixelToUnit((!flag) ? this.localSurfaceOrigin.y : this.localSurfaceOrigin.x);
			float num5 = PhysicsEngine.PixelToUnit((!flag) ? this.localSurfaceOrigin.x : this.localSurfaceOrigin.y);
			while (num < num2)
			{
				float num6 = num2 - num;
				float num7 = 0f;
				float num8 = 0f;
				int num9 = 0;
				float num10 = 0f;
				List<GameObject> list = new List<GameObject>();
				while (num8 < num3)
				{
					float num11 = 1f - 1f / Mathf.Pow(2f, (float)num9);
					float num12 = num3 - num8;
					Vector2 zero = Vector2.zero;
					Vector2 zero2 = Vector2.zero;
					Vector2 vector = ((!flag) ? new Vector2(num12, num6) : new Vector2(num6, num12));
					SurfaceDecorator.ObjectPlacementData objectPlacementData = null;
					if (UnityEngine.Random.value > num11)
					{
						objectPlacementData = this.GetSurfaceObject(vector, out zero, out zero2);
					}
					if (objectPlacementData == null)
					{
						num10 = num3 - num8;
						break;
					}
					GameObject o = objectPlacementData.o;
					float num13 = ((!flag) ? zero.y : zero.x);
					float num14 = ((!flag) ? zero.x : zero.y);
					if (num13 <= num6 && num14 <= num12)
					{
						Vector3 vector2;
						if (flag)
						{
							vector2 = base.transform.position + new Vector3(num4 + num, num5 + num8, -0.5f);
						}
						else
						{
							vector2 = base.transform.position + new Vector3(num5 + num8, num4 + num, -0.5f);
						}
						Vector3 vector3 = vector2 - zero2.ToVector3ZUp(0f);
						if (objectPlacementData.rotated)
						{
							vector3 += new Vector3(zero.x, 0f, 0f);
						}
						GameObject gameObject = SpawnManager.SpawnDebris(o, vector3, (!objectPlacementData.rotated) ? Quaternion.identity : Quaternion.Euler(0f, 0f, 90f));
						this.PostProcessObject(gameObject, objectPlacementData);
						this.m_surfaceObjects.Add(gameObject);
						list.Add(gameObject);
						num9++;
					}
					num7 = Mathf.Max(num7, num13);
					num8 += num14;
					num10 = num3 - num8;
				}
				if (num7 == 0f)
				{
					num = num2;
				}
				else
				{
					num7 += 1f / (float)PhysicsEngine.Instance.PixelsPerUnit;
				}
				int num15 = Mathf.FloorToInt(num10 / 0.0625f);
				if (num15 >= 2)
				{
					int num16 = Mathf.FloorToInt((float)num15 / 2f);
					int num17 = Mathf.FloorToInt((float)num15 / 3f);
					int num18 = UnityEngine.Random.Range(-1 * num17, num17 + 1);
					IntVector2 intVector2 = ((!flag) ? new IntVector2(num16 + num18, 0) : new IntVector2(0, num16 + num18));
					for (int i = 0; i < list.Count; i++)
					{
						list[i].transform.MovePixelsLocal(intVector2);
					}
				}
				num += num7;
			}
			this.parentSprite.UpdateZDepth();
		}
	}

	// Token: 0x060067B2 RID: 26546 RVA: 0x00289AC4 File Offset: 0x00287CC4
	private float GetForceMultiplier(Vector3 objectPosition, Vector2 forceDirection)
	{
		bool flag = this.localSurfaceDimensions.x >= this.localSurfaceDimensions.y;
		bool flag2 = Mathf.Abs(forceDirection.x) > Mathf.Abs(forceDirection.y);
		if (flag2 != flag)
		{
			return 1f;
		}
		Vector2 vector = PhysicsEngine.PixelToUnit(this.localSurfaceOrigin);
		Vector2 vector2 = PhysicsEngine.PixelToUnit(this.localSurfaceDimensions);
		Vector2 vector3 = new Vector2((objectPosition.x - (base.transform.position.x + vector.x)) / vector2.x, (objectPosition.y - (base.transform.position.y + vector.y)) / vector2.y);
		if (forceDirection.x > 0f)
		{
			vector3.x = 1f - vector3.x;
		}
		if (forceDirection.y > 0f)
		{
			vector3.y = 1f - vector3.y;
		}
		float num = ((!flag) ? Mathf.Clamp01(vector3.y) : Mathf.Clamp01(vector3.x));
		return Mathf.Lerp(1f, 1.5f, num);
	}

	// Token: 0x060067B3 RID: 26547 RVA: 0x00289C20 File Offset: 0x00287E20
	public void Destabilize(Vector2 direction)
	{
		if (this.m_destabilized)
		{
			return;
		}
		this.m_destabilized = true;
		if (this.parentSprite == null)
		{
			this.parentSprite = base.GetComponent<tk2dSprite>();
		}
		Vector2 vector = Vector2.zero;
		float num = 0f;
		if (direction.x > 0f)
		{
			vector += new Vector2(5f, 0f);
			num = 0.5f;
		}
		if (direction.x < 0f)
		{
			vector += new Vector2(-5f, 0f);
			num = 0.5f;
		}
		if (direction.y > 0f)
		{
			vector += new Vector2(0f, 9f);
			num = -0.25f;
		}
		if (direction.y < 0f)
		{
			vector += new Vector2(0f, -5f);
		}
		if (this.m_attachedSprites != null)
		{
			for (int i = 0; i < this.m_attachedSprites.Count; i++)
			{
				if (this.m_attachedSprites[i])
				{
					this.parentSprite.DetachRenderer(this.m_attachedSprites[i]);
					this.m_attachedSprites[i].attachParent = null;
					this.m_attachedSprites[i].IsPerpendicular = true;
				}
			}
		}
		if (this.m_surfaceObjects != null)
		{
			for (int j = 0; j < this.m_surfaceObjects.Count; j++)
			{
				if (this.m_surfaceObjects[j])
				{
					float forceMultiplier = this.GetForceMultiplier(this.m_surfaceObjects[j].transform.position, vector);
					MinorBreakableGroupManager component = this.m_surfaceObjects[j].GetComponent<MinorBreakableGroupManager>();
					if (component != null)
					{
						component.Destabilize(vector.ToVector3ZUp(0.5f) * forceMultiplier, 0.5f + num);
					}
					else
					{
						DebrisObject component2 = this.m_surfaceObjects[j].gameObject.GetComponent<DebrisObject>();
						if (component2 != null)
						{
							Vector3 vector2 = Quaternion.Euler(0f, 0f, Mathf.Lerp(-30f, 30f, UnityEngine.Random.value)) * vector.ToVector3ZUp(0.25f + num) * forceMultiplier;
							component2.Trigger(vector2, 0.5f, 1f);
						}
						else
						{
							MinorBreakable component3 = this.m_surfaceObjects[j].GetComponent<MinorBreakable>();
							if (component3 != null)
							{
								Vector3 vector3 = Quaternion.Euler(0f, 0f, Mathf.Lerp(-30f, 30f, UnityEngine.Random.value)) * vector.ToVector3ZUp(0.25f + num) * forceMultiplier;
								component3.destroyOnBreak = true;
								component3.Break(vector3.XY());
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060067B4 RID: 26548 RVA: 0x00289F2C File Offset: 0x0028812C
	private IEnumerator DetachRenderersMomentarily()
	{
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < this.m_attachedSprites.Count; i++)
		{
			if (this.m_attachedSprites[i])
			{
				this.parentSprite.DetachRenderer(this.m_attachedSprites[i].GetComponent<tk2dBaseSprite>());
			}
		}
		yield break;
	}

	// Token: 0x040063A7 RID: 25511
	public GenericLootTable tableTable;

	// Token: 0x040063A8 RID: 25512
	public float chanceToDecorate = 1f;

	// Token: 0x040063A9 RID: 25513
	public IntVector2 localSurfaceOrigin;

	// Token: 0x040063AA RID: 25514
	public IntVector2 localSurfaceDimensions;

	// Token: 0x040063AB RID: 25515
	public float heightOffGround = 0.5f;

	// Token: 0x040063AC RID: 25516
	public tk2dSprite parentSprite;

	// Token: 0x040063AD RID: 25517
	private List<GameObject> m_surfaceObjects;

	// Token: 0x040063AE RID: 25518
	private List<tk2dSprite> m_attachedSprites;

	// Token: 0x040063AF RID: 25519
	private RoomHandler m_parentRoom;

	// Token: 0x040063B0 RID: 25520
	private bool m_destabilized;

	// Token: 0x040063B1 RID: 25521
	private const int PRIMARY_PIXEL_BUFFER = 1;

	// Token: 0x0200121C RID: 4636
	internal class ObjectPlacementData
	{
		// Token: 0x060067B5 RID: 26549 RVA: 0x00289F48 File Offset: 0x00288148
		public ObjectPlacementData(GameObject obj)
		{
			this.o = obj;
		}

		// Token: 0x040063B2 RID: 25522
		public GameObject o;

		// Token: 0x040063B3 RID: 25523
		public bool rotated;

		// Token: 0x040063B4 RID: 25524
		public bool horizontalFlip;

		// Token: 0x040063B5 RID: 25525
		public bool verticalFlip;
	}
}
