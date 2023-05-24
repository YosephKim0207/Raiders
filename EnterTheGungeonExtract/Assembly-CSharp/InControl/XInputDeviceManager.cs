using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using XInputDotNetPure;

namespace InControl
{
	// Token: 0x02000816 RID: 2070
	public class XInputDeviceManager : InputDeviceManager
	{
		// Token: 0x06002C19 RID: 11289 RVA: 0x000DF70C File Offset: 0x000DD90C
		public XInputDeviceManager()
		{
			if (InputManager.XInputUpdateRate == 0U)
			{
				this.timeStep = Mathf.FloorToInt(Time.fixedDeltaTime * 1000f);
			}
			else
			{
				this.timeStep = Mathf.FloorToInt(1f / InputManager.XInputUpdateRate * 1000f);
			}
			this.bufferSize = (int)Math.Max(InputManager.XInputBufferSize, 1U);
			for (int i = 0; i < 4; i++)
			{
				this.gamePadState[i] = new RingBuffer<GamePadState>(this.bufferSize);
			}
			this.StartWorker();
			for (int j = 0; j < 4; j++)
			{
				this.devices.Add(new XInputDevice(j, this));
			}
			this.Update(0UL, 0f);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000DF7E8 File Offset: 0x000DD9E8
		private void StartWorker()
		{
			if (this.thread == null)
			{
				this.thread = new Thread(new ThreadStart(this.Worker));
				this.thread.IsBackground = true;
				this.thread.Start();
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000DF824 File Offset: 0x000DDA24
		private void StopWorker()
		{
			if (this.thread != null)
			{
				this.thread.Abort();
				this.thread.Join();
				this.thread = null;
			}
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000DF850 File Offset: 0x000DDA50
		private void Worker()
		{
			for (;;)
			{
				for (int i = 0; i < 4; i++)
				{
					this.gamePadState[i].Enqueue(GamePad.GetState((PlayerIndex)i));
				}
				Thread.Sleep(this.timeStep);
			}
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000DF894 File Offset: 0x000DDA94
		internal GamePadState GetState(int deviceIndex)
		{
			return this.gamePadState[deviceIndex].Dequeue();
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000DF8A4 File Offset: 0x000DDAA4
		public override void Update(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 4; i++)
			{
				XInputDevice xinputDevice = this.devices[i] as XInputDevice;
				if (!xinputDevice.IsConnected)
				{
					xinputDevice.GetState();
				}
				if (xinputDevice.IsConnected != this.deviceConnected[i])
				{
					if (xinputDevice.IsConnected)
					{
						InputManager.AttachDevice(xinputDevice);
					}
					else
					{
						InputManager.DetachDevice(xinputDevice);
					}
					this.deviceConnected[i] = xinputDevice.IsConnected;
				}
			}
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000DF924 File Offset: 0x000DDB24
		public override void Destroy()
		{
			this.StopWorker();
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000DF92C File Offset: 0x000DDB2C
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				return false;
			}
			try
			{
				GamePad.GetState(PlayerIndex.One);
			}
			catch (DllNotFoundException ex)
			{
				if (errors != null)
				{
					errors.Add(ex.Message + ".dll could not be found or is missing a dependency.");
				}
				return false;
			}
			return true;
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000DF994 File Offset: 0x000DDB94
		internal static void Enable()
		{
			List<string> list = new List<string>();
			if (XInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.HideDevicesWithProfile(typeof(Xbox360WinProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWinProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWin10Profile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWin10AEProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF310ModeXWinProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF510ModeXWinProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF710ModeXWinProfile));
				InputManager.AddDeviceManager<XInputDeviceManager>();
			}
			else
			{
				foreach (string text in list)
				{
					Logger.LogError(text);
				}
			}
		}

		// Token: 0x04001E04 RID: 7684
		private bool[] deviceConnected = new bool[4];

		// Token: 0x04001E05 RID: 7685
		private const int maxDevices = 4;

		// Token: 0x04001E06 RID: 7686
		private RingBuffer<GamePadState>[] gamePadState = new RingBuffer<GamePadState>[4];

		// Token: 0x04001E07 RID: 7687
		private Thread thread;

		// Token: 0x04001E08 RID: 7688
		private int timeStep;

		// Token: 0x04001E09 RID: 7689
		private int bufferSize;
	}
}
