using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200090F RID: 2319
	public abstract class ComponentAction<T> : FsmStateAction where T : Component
	{
		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x0010C118 File Offset: 0x0010A318
		protected Rigidbody rigidbody
		{
			get
			{
				return this.component as Rigidbody;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x0010C12C File Offset: 0x0010A32C
		protected Rigidbody2D rigidbody2d
		{
			get
			{
				return this.component as Rigidbody2D;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x0010C140 File Offset: 0x0010A340
		protected Renderer renderer
		{
			get
			{
				return this.component as Renderer;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x0010C154 File Offset: 0x0010A354
		protected Animation animation
		{
			get
			{
				return this.component as Animation;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x0010C168 File Offset: 0x0010A368
		protected AudioSource audio
		{
			get
			{
				return this.component as AudioSource;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x0010C17C File Offset: 0x0010A37C
		protected Camera camera
		{
			get
			{
				return this.component as Camera;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x0010C190 File Offset: 0x0010A390
		protected GUIText guiText
		{
			get
			{
				return this.component as GUIText;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x0010C1A4 File Offset: 0x0010A3A4
		protected GUITexture guiTexture
		{
			get
			{
				return this.component as GUITexture;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x0600331A RID: 13082 RVA: 0x0010C1B8 File Offset: 0x0010A3B8
		protected Light light
		{
			get
			{
				return this.component as Light;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x0600331B RID: 13083 RVA: 0x0010C1CC File Offset: 0x0010A3CC
		protected NetworkView networkView
		{
			get
			{
				return this.component as NetworkView;
			}
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x0010C1E0 File Offset: 0x0010A3E0
		protected bool UpdateCache(GameObject go)
		{
			if (go == null)
			{
				return false;
			}
			if (this.component == null || this.cachedGameObject != go)
			{
				this.component = go.GetComponent<T>();
				this.cachedGameObject = go;
				if (this.component == null)
				{
					base.LogWarning("Missing component: " + typeof(T).FullName + " on: " + go.name);
				}
			}
			return this.component != null;
		}

		// Token: 0x04002440 RID: 9280
		private GameObject cachedGameObject;

		// Token: 0x04002441 RID: 9281
		private T component;
	}
}
