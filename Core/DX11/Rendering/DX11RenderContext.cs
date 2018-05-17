﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct3D11;
using Device = SlimDX.Direct3D11.Device;
using SlimDX.DXGI;

using FeralTic.Utils;
using FeralTic.DX11.Geometry;
using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public partial class DX11RenderContext : IDisposable
    {
        public Device Device { get; private set; }
        public DeviceContext CurrentDeviceContext { get; private set; }

        public Factory1 Factory { get; private set; }
        public Adapter1 Adapter { get; private set; }
        public DXGIScreen Screen { get; private set; }

        private DeviceContext immediatecontext;

        public DefaultTextures DefaultTextures { get; private set; }
        public DX11ResourcePoolManager ResourcePool { get; private set; }
        public DX11RenderTargetStack RenderTargetStack { get; private set; }
        public DX11PrimitivesManager Primitives { get; private set; }
        public DX11RenderStateStack RenderStateStack { get; private set; }
        public DX11ResourceScheduler ResourceScheduler { get; private set; }
        public DX11BasicEffects BasicEffects { get; private set; }

        private ShaderResourceView[] nullsrvs = new ShaderResourceView[128];
        private UnorderedAccessView[] nulluavs = new UnorderedAccessView[8];



        private FeatureLevel[] GetFeatureLevels()
        {
            Version win8version = new Version(6, 2, 9200, 0);

            if (OSUtils.IsWindows8)
            {
                return new FeatureLevel[]
                {
                    (FeatureLevel)MagicNumberUtils.FeatureLevel11_1,
                    FeatureLevel.Level_11_0,
                    FeatureLevel.Level_10_1,
                    FeatureLevel.Level_10_0
                };
            }
            else
            {
                return new FeatureLevel[]
                {
                    FeatureLevel.Level_11_0,
                    FeatureLevel.Level_10_1,
                    FeatureLevel.Level_10_0
                };
            }
        }


        public DX11RenderContext(DeviceCreationFlags flags = DeviceCreationFlags.None)
        {
            this.Device = new Device(DriverType.Hardware,flags, GetFeatureLevels());
            this.immediatecontext = this.Device.ImmediateContext;
            this.CurrentDeviceContext = this.immediatecontext;
        }

        public DX11RenderContext(Factory1 factory, DXGIScreen screen, DeviceCreationFlags flags = DeviceCreationFlags.None)
        {
            this.Factory = factory;
            this.Screen = screen;
            this.Device = new Device(screen.Adapter, flags, GetFeatureLevels());
            this.immediatecontext = this.Device.ImmediateContext;
            this.CurrentDeviceContext = this.immediatecontext;
        }

        public DX11RenderContext(Adapter1 adapter, DeviceCreationFlags flags = DeviceCreationFlags.None)
        {
            this.Device = new Device(adapter, flags, GetFeatureLevels());
            this.Adapter = adapter;
            this.Factory = this.Device.Factory as Factory1;
            this.immediatecontext = this.Device.ImmediateContext;
            this.CurrentDeviceContext = this.immediatecontext;
        }

        public DX11RenderContext(Device device)
        {
            this.Device = device;
            this.Factory = this.Device.Factory as Factory1;
            this.immediatecontext = this.Device.ImmediateContext;
            this.CurrentDeviceContext = this.immediatecontext;
        }

        public void Initialize(int schedulerthreadcount = 1)
        {
            this.BasicEffects = new DX11BasicEffects(this);
            this.ResourcePool = new DX11ResourcePoolManager(this);
            this.RenderTargetStack = new DX11RenderTargetStack(this);
            this.DefaultTextures = new DefaultTextures(this);
            this.Primitives = new DX11PrimitivesManager(this);
            this.RenderStateStack = new DX11RenderStateStack(this);
            this.ResourceScheduler = new DX11ResourceScheduler(this, schedulerthreadcount);
            this.ResourceScheduler.Initialize();

            this.CheckBufferSupport();

            if (this.Factory == null)
            {
                this.Factory = this.Device.Factory as Factory1;
            }

            this.BuildFormatSampling();
        }

        private void CheckBufferSupport()
        {
            try
            {
                DX11RawBuffer raw = new DX11RawBuffer(this.Device, 16);
                raw.Dispose();
                this.computesupport = true;
            }
            catch
            {
                this.computesupport = false;
            }
        }

        public void BeginFrame()
        {
            this.ResourcePool.BeginFrame();
        }

        public void EndFrame()
        {
        }

        public void CleanUp()
        {
            this.immediatecontext.PixelShader.SetShaderResources(nullsrvs, 0, 128);
            this.immediatecontext.DomainShader.SetShaderResources(nullsrvs, 0, 128);
            this.immediatecontext.HullShader.SetShaderResources(nullsrvs, 0, 128);
            this.immediatecontext.GeometryShader.SetShaderResources(nullsrvs, 0, 128);
            this.immediatecontext.VertexShader.SetShaderResources(nullsrvs, 0, 128);
            this.CleanUpCS();
        }

        public void CleanUpPS()
        {
            this.immediatecontext.PixelShader.SetShaderResources(nullsrvs, 0, 128);
        }

        public void CleanUpCS()
        {
            this.immediatecontext.ComputeShader.SetShaderResources(nullsrvs, 0, 128);

            if (this.IsFeatureLevel11)
            {
                this.CurrentDeviceContext.ComputeShader.SetUnorderedAccessViews(nulluavs, 0, 8);
            }
            else
            {
                this.CurrentDeviceContext.ComputeShader.SetUnorderedAccessViews(nulluavs, 0, 1);
            }
        }

        public void CleanShaderStages()
        {
            this.immediatecontext.HullShader.Set(null);
            this.immediatecontext.DomainShader.Set(null);
            this.immediatecontext.VertexShader.Set(null);
            this.immediatecontext.PixelShader.Set(null);
            this.immediatecontext.GeometryShader.Set(null);
            this.immediatecontext.ComputeShader.Set(null);
        }

        public void Dispose()
        {
            this.immediatecontext.ClearState();

            this.BasicEffects.Dispose();
            this.ResourcePool.Dispose();
            this.DefaultTextures.Dispose();
            this.Primitives.Dispose();
            this.ResourceScheduler.Dispose();

            this.immediatecontext.Dispose(); 
            this.Device.Dispose();
        }

        public FeatureLevel FeatureLevel { get { return this.Device.FeatureLevel; } }

        public bool IsFeatureLevel11 { get { return this.Device.FeatureLevel >= SlimDX.Direct3D11.FeatureLevel.Level_11_0; } }
        public bool IsAtLeast101 { get { return this.Device.FeatureLevel >= SlimDX.Direct3D11.FeatureLevel.Level_10_1; } }

        private bool computesupport;

        public bool ComputeShaderSupport
        {
            get
            {
                if (this.IsFeatureLevel11) return true;

                return this.computesupport;
            }
        }
    }
}
