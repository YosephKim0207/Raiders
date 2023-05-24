using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200153F RID: 5439
public class SpriteShadowCaster : MonoBehaviour
{
	// Token: 0x06007C82 RID: 31874 RVA: 0x00321FF4 File Offset: 0x003201F4
	private void Start()
	{
		this.m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		this.m_shadows = new List<SpriteShadow>();
	}

	// Token: 0x06007C83 RID: 31875 RVA: 0x00322018 File Offset: 0x00320218
	public Material GetMaterialInstance()
	{
		return UnityEngine.Object.Instantiate<Material>(this.material);
	}

	// Token: 0x06007C84 RID: 31876 RVA: 0x00322028 File Offset: 0x00320228
	private void Update()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, this.radius);
		Plane[] array2 = GeometryUtility.CalculateFrustumPlanes(this.m_camera);
		foreach (Collider collider in array)
		{
			tk2dSprite component = collider.GetComponent<tk2dSprite>();
			if (!(collider.name != "PlayerSprite") || !(collider.GetComponent<AIActor>() == null))
			{
				if (!(collider.GetComponent<MeshRenderer>() != null) || collider.GetComponent<MeshRenderer>().enabled)
				{
					if (GeometryUtility.TestPlanesAABB(array2, collider.bounds))
					{
						if (component != null)
						{
							bool flag = false;
							for (int j = 0; j < this.m_shadows.Count; j++)
							{
								if (this.m_shadows[j].shadowedSprite == component)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								SpriteShadow spriteShadow = new SpriteShadow(component, this);
								this.m_shadows.Add(spriteShadow);
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < this.m_shadows.Count; k++)
		{
			SpriteShadow spriteShadow2 = this.m_shadows[k];
			if (spriteShadow2.shadowedSprite == null || !spriteShadow2.shadowedSprite.enabled || !GeometryUtility.TestPlanesAABB(array2, spriteShadow2.shadowedSprite.GetComponent<Collider>().bounds) || (spriteShadow2.shadowedSprite.transform.position - base.transform.position).magnitude > this.radius)
			{
				this.m_shadows.RemoveAt(k);
				k--;
				spriteShadow2.Destroy();
			}
			else
			{
				spriteShadow2.UpdateShadow(false);
			}
		}
	}

	// Token: 0x04007F6F RID: 32623
	public float radius = 10f;

	// Token: 0x04007F70 RID: 32624
	public float shadowDepth = -0.05f;

	// Token: 0x04007F71 RID: 32625
	public Material material;

	// Token: 0x04007F72 RID: 32626
	private List<SpriteShadow> m_shadows;

	// Token: 0x04007F73 RID: 32627
	private Camera m_camera;
}
