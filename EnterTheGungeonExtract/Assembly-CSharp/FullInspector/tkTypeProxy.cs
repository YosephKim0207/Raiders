using System;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x02000666 RID: 1638
	public class tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo> : tkControl<TTo, TContextTo>
	{
		// Token: 0x06002572 RID: 9586 RVA: 0x000A0C34 File Offset: 0x0009EE34
		public tkTypeProxy(tkControl<TFrom, TContextFrom> control)
		{
			this._control = control;
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000A0C44 File Offset: 0x0009EE44
		private static T Cast<T>(object val)
		{
			return (T)((object)val);
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000A0C4C File Offset: 0x0009EE4C
		public override bool ShouldShow(TTo obj, TContextTo context, fiGraphMetadata metadata)
		{
			return this._control.ShouldShow(tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo>.Cast<TFrom>(obj), tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo>.Cast<TContextFrom>(context), metadata);
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000A0C70 File Offset: 0x0009EE70
		protected override TTo DoEdit(Rect rect, TTo obj, TContextTo context, fiGraphMetadata metadata)
		{
			return tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo>.Cast<TTo>(this._control.Edit(rect, tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo>.Cast<TFrom>(obj), tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo>.Cast<TContextFrom>(context), metadata));
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000A0CA0 File Offset: 0x0009EEA0
		protected override float DoGetHeight(TTo obj, TContextTo context, fiGraphMetadata metadata)
		{
			return this._control.GetHeight(tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo>.Cast<TFrom>(obj), tkTypeProxy<TFrom, TContextFrom, TTo, TContextTo>.Cast<TContextFrom>(context), metadata);
		}

		// Token: 0x0400199B RID: 6555
		private tkControl<TFrom, TContextFrom> _control;
	}
}
