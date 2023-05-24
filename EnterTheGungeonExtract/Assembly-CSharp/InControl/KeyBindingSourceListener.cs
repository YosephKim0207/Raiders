using System;

namespace InControl
{
	// Token: 0x02000692 RID: 1682
	public class KeyBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06002650 RID: 9808 RVA: 0x000A3BDC File Offset: 0x000A1DDC
		public void Reset()
		{
			this.detectFound.Clear();
			this.detectPhase = 0;
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x000A3BF0 File Offset: 0x000A1DF0
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeKeys)
			{
				return null;
			}
			if (this.detectFound.IncludeCount > 0 && !this.detectFound.IsPressed && this.detectPhase == 2)
			{
				KeyBindingSource keyBindingSource = new KeyBindingSource(this.detectFound);
				this.Reset();
				return keyBindingSource;
			}
			KeyCombo keyCombo = KeyCombo.Detect(listenOptions.IncludeModifiersAsFirstClassKeys);
			if (keyCombo.IncludeCount > 0)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = keyCombo;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x04001A6F RID: 6767
		private KeyCombo detectFound;

		// Token: 0x04001A70 RID: 6768
		private int detectPhase;
	}
}
