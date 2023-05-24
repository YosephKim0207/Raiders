using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001171 RID: 4465
public class GenericDecorator : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x06006327 RID: 25383 RVA: 0x00266AA8 File Offset: 0x00264CA8
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.Decorate();
		if (this.disableRigidbodies && this.rigidbodyTrigger != null)
		{
			DebrisObject debrisObject = this.rigidbodyTrigger;
			debrisObject.OnTriggered = (Action)Delegate.Combine(debrisObject.OnTriggered, new Action(this.EnableRigidbodies));
		}
	}

	// Token: 0x06006328 RID: 25384 RVA: 0x00266B00 File Offset: 0x00264D00
	private GameObject GetSurfaceObject(Vector2 availableSpace, out Vector2 objectDimensions, out Vector2 localOrigin)
	{
		List<GameObject> list = new List<GameObject>();
		bool flag = false;
		int num = 0;
		while (!flag && num < 1000)
		{
			GameObject gameObject = this.tableTable.SelectByWeightWithoutDuplicates(list, false);
			if (gameObject == null)
			{
				break;
			}
			list.Add(gameObject);
			MinorBreakableGroupManager component = gameObject.GetComponent<MinorBreakableGroupManager>();
			if (component != null)
			{
				objectDimensions = component.GetDimensions();
				if (objectDimensions == Vector2.zero)
				{
					continue;
				}
				localOrigin = Vector2.zero;
			}
			else
			{
				tk2dSprite component2 = gameObject.GetComponent<tk2dSprite>();
				Bounds bounds = component2.GetBounds();
				objectDimensions = new Vector2(bounds.size.x, bounds.size.y);
				localOrigin = bounds.min.XY();
			}
			if (objectDimensions.x <= availableSpace.x && objectDimensions.y <= availableSpace.y)
			{
				return gameObject;
			}
			num++;
		}
		objectDimensions = new Vector2(float.MaxValue, float.MaxValue);
		localOrigin = Vector2.zero;
		return null;
	}

	// Token: 0x06006329 RID: 25385 RVA: 0x00266C30 File Offset: 0x00264E30
	public void EnableRigidbodies()
	{
		for (int i = 0; i < this.m_srbs.Count; i++)
		{
			if (this.m_srbs[i])
			{
				this.m_srbs[i].enabled = true;
			}
		}
	}

	// Token: 0x0600632A RID: 25386 RVA: 0x00266C84 File Offset: 0x00264E84
	private void PostProcessObject(GameObject placedObject)
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
					componentsInChildren[i].HeightOffGround = this.heightOffGround + 0.1f;
					this.parentSprite.AttachRenderer(componentsInChildren[i]);
				}
			}
			MinorBreakable[] componentsInChildren2 = component.GetComponentsInChildren<MinorBreakable>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].heightOffGround = 0.75f;
				componentsInChildren2[j].isImpermeableToGameActors = true;
				componentsInChildren2[j].specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBreakable;
			}
			DebrisObject[] componentsInChildren3 = component.GetComponentsInChildren<DebrisObject>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].InitializeForCollisions();
				componentsInChildren3[k].additionalHeightBoost = 0.25f;
			}
			if (this.disableRigidbodies && this.rigidbodyTrigger != null)
			{
				SpeculativeRigidbody[] componentsInChildren4 = component.GetComponentsInChildren<SpeculativeRigidbody>(true);
				for (int l = 0; l < componentsInChildren4.Length; l++)
				{
					this.m_srbs.Add(componentsInChildren4[l]);
					componentsInChildren4[l].enabled = false;
				}
			}
		}
		else
		{
			tk2dSprite component2 = placedObject.GetComponent<tk2dSprite>();
			if (component2.attachParent == null)
			{
				component2.HeightOffGround = this.heightOffGround + 0.1f;
				this.parentSprite.AttachRenderer(component2);
			}
			MinorBreakable component3 = placedObject.GetComponent<MinorBreakable>();
			if (component3 != null)
			{
				component3.heightOffGround = 0.75f;
				component3.isImpermeableToGameActors = true;
				component3.specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.BulletBreakable;
			}
			DebrisObject component4 = placedObject.GetComponent<DebrisObject>();
			if (component4 != null)
			{
				component4.InitializeForCollisions();
				component4.additionalHeightBoost = 0.25f;
			}
			if (this.disableRigidbodies && this.rigidbodyTrigger != null)
			{
				SpeculativeRigidbody component5 = placedObject.GetComponent<SpeculativeRigidbody>();
				if (component5 != null)
				{
					this.m_srbs.Add(component5);
					component5.enabled = false;
				}
			}
		}
		if (this.parentSurface != null)
		{
			this.parentSurface.RegisterAdditionalObject(placedObject);
			if (this.parentSurface.sprite != null)
			{
				this.parentSurface.sprite.UpdateZDepth();
			}
		}
		else
		{
			this.parentSprite.UpdateZDepth();
		}
	}

	// Token: 0x0600632B RID: 25387 RVA: 0x00266F18 File Offset: 0x00265118
	public void Decorate()
	{
		if (this.parentSprite == null)
		{
			this.parentSprite = base.GetComponent<tk2dSprite>();
		}
		if (this.tableTable == null)
		{
			BraveUtility.Log("Trying to decorate a SurfaceDecorator at: " + base.gameObject.name + " and failing.", Color.red, BraveUtility.LogVerbosity.CHATTY);
			return;
		}
		Vector2 vector = PhysicsEngine.PixelToUnit(this.localPixelSurfaceOrigin);
		Vector2 vector2 = PhysicsEngine.PixelToUnit(this.localPixelSurfaceDimensions);
		bool flag = vector2.x >= vector2.y;
		float num = 0f;
		float num2 = ((!flag) ? vector2.y : vector2.x);
		float num3 = ((!flag) ? vector2.x : vector2.y);
		float num4 = ((!flag) ? vector.y : vector.x);
		float num5 = ((!flag) ? vector.x : vector.y);
		while (num < num2)
		{
			float num6 = num2 - num;
			float num7 = 0f;
			float num11;
			for (float num8 = 0f; num8 < num3; num8 += num11)
			{
				float num9 = num3 - num8;
				Vector2 zero = Vector2.zero;
				Vector2 zero2 = Vector2.zero;
				Vector2 vector3 = ((!flag) ? new Vector2(num9, num6) : new Vector2(num6, num9));
				GameObject surfaceObject = this.GetSurfaceObject(vector3, out zero, out zero2);
				if (surfaceObject == null)
				{
					num = num2;
					break;
				}
				float num10 = ((!flag) ? zero.y : zero.x);
				num11 = ((!flag) ? zero.x : zero.y);
				if (num10 <= num6 && num11 <= num9)
				{
					Vector3 vector4;
					if (flag)
					{
						vector4 = base.transform.position + new Vector3(num4 + num, num5 + num8, -0.5f);
					}
					else
					{
						vector4 = base.transform.position + new Vector3(num5 + num8, num4 + num, -0.5f);
					}
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(surfaceObject, vector4 - zero2.ToVector3ZUp(0f), Quaternion.identity);
					this.PostProcessObject(gameObject);
				}
				num7 = Mathf.Max(num7, num10);
			}
			num += num7;
		}
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x0026718C File Offset: 0x0026538C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005E46 RID: 24134
	public GenericLootTable tableTable;

	// Token: 0x04005E47 RID: 24135
	public IntVector2 localPixelSurfaceOrigin;

	// Token: 0x04005E48 RID: 24136
	public IntVector2 localPixelSurfaceDimensions;

	// Token: 0x04005E49 RID: 24137
	public float heightOffGround = 0.1f;

	// Token: 0x04005E4A RID: 24138
	public bool disableRigidbodies = true;

	// Token: 0x04005E4B RID: 24139
	public DebrisObject rigidbodyTrigger;

	// Token: 0x04005E4C RID: 24140
	public tk2dSprite parentSprite;

	// Token: 0x04005E4D RID: 24141
	[HideInInspector]
	public SurfaceDecorator parentSurface;

	// Token: 0x04005E4E RID: 24142
	private List<SpeculativeRigidbody> m_srbs = new List<SpeculativeRigidbody>();
}
