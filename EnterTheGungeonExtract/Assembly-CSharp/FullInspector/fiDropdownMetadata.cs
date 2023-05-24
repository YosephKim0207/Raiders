using System;
using FullInspector.Internal;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000619 RID: 1561
	[Serializable]
	public class fiDropdownMetadata : IGraphMetadataItemPersistent, ISerializationCallbackReceiver
	{
		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x0009E310 File Offset: 0x0009C510
		// (set) Token: 0x06002465 RID: 9317 RVA: 0x0009E320 File Offset: 0x0009C520
		public bool IsActive
		{
			get
			{
				return this._isActive.value;
			}
			set
			{
				if (value != this._isActive.target)
				{
					if (fiSettings.EnableAnimation)
					{
						this._isActive.target = value;
					}
					else
					{
						this._isActive = new fiAnimBool(value);
					}
				}
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x0009E35C File Offset: 0x0009C55C
		public float AnimPercentage
		{
			get
			{
				return this._isActive.faded;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x0009E36C File Offset: 0x0009C56C
		public bool IsAnimating
		{
			get
			{
				return this._isActive.isAnimating;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x0009E37C File Offset: 0x0009C57C
		// (set) Token: 0x06002469 RID: 9321 RVA: 0x0009E394 File Offset: 0x0009C594
		public bool ShouldDisplayDropdownArrow
		{
			get
			{
				return !this._forceDisable && this._showDropdown;
			}
			set
			{
				if (this._forceDisable && value)
				{
					return;
				}
				this._showDropdown = value;
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0009E3B0 File Offset: 0x0009C5B0
		public void InvertDefaultState()
		{
			this._invertedDefaultState = true;
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0009E3BC File Offset: 0x0009C5BC
		public void ForceHideWithoutAnimation()
		{
			this._forceDisable = false;
			this._showDropdown = true;
			this._isActive = new fiAnimBool(false);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0009E3D8 File Offset: 0x0009C5D8
		public void ForceDisable()
		{
			this._forceDisable = true;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x0009E3E4 File Offset: 0x0009C5E4
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this._serializedIsActive = this.IsActive;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0009E3F4 File Offset: 0x0009C5F4
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this._isActive = new fiAnimBool(this._serializedIsActive);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0009E408 File Offset: 0x0009C608
		bool IGraphMetadataItemPersistent.ShouldSerialize()
		{
			if (this._invertedDefaultState)
			{
				return this.IsActive;
			}
			return !this.IsActive;
		}

		// Token: 0x04001929 RID: 6441
		private fiAnimBool _isActive = new fiAnimBool(true);

		// Token: 0x0400192A RID: 6442
		[SerializeField]
		private bool _showDropdown;

		// Token: 0x0400192B RID: 6443
		private bool _invertedDefaultState;

		// Token: 0x0400192C RID: 6444
		private bool _forceDisable;

		// Token: 0x0400192D RID: 6445
		[SerializeField]
		private bool _serializedIsActive;
	}
}
