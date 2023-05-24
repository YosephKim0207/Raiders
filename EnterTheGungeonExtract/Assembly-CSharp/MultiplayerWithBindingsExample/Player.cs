using System;
using UnityEngine;

namespace MultiplayerWithBindingsExample
{
	// Token: 0x02000680 RID: 1664
	public class Player : MonoBehaviour
	{
		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060025DA RID: 9690 RVA: 0x000A217C File Offset: 0x000A037C
		// (set) Token: 0x060025DB RID: 9691 RVA: 0x000A2184 File Offset: 0x000A0384
		public PlayerActions Actions { get; set; }

		// Token: 0x060025DC RID: 9692 RVA: 0x000A2190 File Offset: 0x000A0390
		private void OnDisable()
		{
			if (this.Actions != null)
			{
				this.Actions.Destroy();
			}
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000A21A8 File Offset: 0x000A03A8
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000A21B8 File Offset: 0x000A03B8
		private void Update()
		{
			if (this.Actions == null)
			{
				this.cachedRenderer.material.color = new Color(1f, 1f, 1f, 0.2f);
			}
			else
			{
				this.cachedRenderer.material.color = this.GetColorFromInput();
				base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * this.Actions.Rotate.X, Space.World);
				base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * this.Actions.Rotate.Y, Space.World);
			}
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x000A2270 File Offset: 0x000A0470
		private Color GetColorFromInput()
		{
			if (this.Actions.Green)
			{
				return Color.green;
			}
			if (this.Actions.Red)
			{
				return Color.red;
			}
			if (this.Actions.Blue)
			{
				return Color.blue;
			}
			if (this.Actions.Yellow)
			{
				return Color.yellow;
			}
			return Color.white;
		}

		// Token: 0x040019C1 RID: 6593
		private Renderer cachedRenderer;
	}
}
