using System;

namespace InControl
{
	// Token: 0x02000697 RID: 1687
	public class MouseBindingSourceListener : BindingSourceListener
	{
		// Token: 0x0600268B RID: 9867 RVA: 0x000A57AC File Offset: 0x000A39AC
		public void Reset()
		{
			this.detectFound = Mouse.None;
			this.detectPhase = 0;
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000A57BC File Offset: 0x000A39BC
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (this.detectFound != Mouse.None && !this.IsPressed(this.detectFound) && this.detectPhase == 2)
			{
				MouseBindingSource mouseBindingSource = new MouseBindingSource(this.detectFound);
				this.Reset();
				return mouseBindingSource;
			}
			Mouse mouse = this.ListenForControl(listenOptions);
			if (mouse != Mouse.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = mouse;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000A5848 File Offset: 0x000A3A48
		private bool IsPressed(Mouse control)
		{
			if (control == Mouse.NegativeScrollWheel)
			{
				return MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
			}
			if (control != Mouse.PositiveScrollWheel)
			{
				return MouseBindingSource.ButtonIsPressed(control);
			}
			return MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000A587C File Offset: 0x000A3A7C
		private Mouse ListenForControl(BindingListenOptions listenOptions)
		{
			if (listenOptions.IncludeMouseButtons)
			{
				for (Mouse mouse = Mouse.None; mouse <= Mouse.Button9; mouse++)
				{
					if (MouseBindingSource.ButtonIsPressed(mouse))
					{
						return mouse;
					}
				}
			}
			if (listenOptions.IncludeMouseScrollWheel)
			{
				if (MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
				{
					return Mouse.NegativeScrollWheel;
				}
				if (MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
				{
					return Mouse.PositiveScrollWheel;
				}
			}
			return Mouse.None;
		}

		// Token: 0x04001A92 RID: 6802
		public static float ScrollWheelThreshold = 0.001f;

		// Token: 0x04001A93 RID: 6803
		private Mouse detectFound;

		// Token: 0x04001A94 RID: 6804
		private int detectPhase;
	}
}
