using System;
using UnityEngine;

namespace Kvant
{
	// Token: 0x02000839 RID: 2105
	[AddComponentMenu("Kvant/Tunnel Scroller")]
	[RequireComponent(typeof(Tunnel))]
	public class TunnelScroller : MonoBehaviour
	{
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002DC6 RID: 11718 RVA: 0x000EEC28 File Offset: 0x000ECE28
		// (set) Token: 0x06002DC7 RID: 11719 RVA: 0x000EEC30 File Offset: 0x000ECE30
		public float speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				this._speed = value;
			}
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000EEC3C File Offset: 0x000ECE3C
		private void Update()
		{
			if (this.m_transform == null)
			{
				this.m_transform = base.transform;
			}
			this.m_transform.Rotate(0f, 0f, this._zRotationSpeed * BraveTime.DeltaTime);
			base.GetComponent<Tunnel>().offset += this._speed * BraveTime.DeltaTime;
		}

		// Token: 0x04001F19 RID: 7961
		[SerializeField]
		private float _speed;

		// Token: 0x04001F1A RID: 7962
		[SerializeField]
		private float _zRotationSpeed;

		// Token: 0x04001F1B RID: 7963
		private Transform m_transform;
	}
}
