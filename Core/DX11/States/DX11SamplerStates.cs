using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public class DX11SamplerStates : DX11RenderStates<SamplerDescription>
    {
        private static DX11SamplerStates instance;

        public static DX11SamplerStates Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DX11SamplerStates();
                    instance.Initialize();
                }
                return instance;
            }
        }

        public override string EnumName
        {
            get
            {
                return "DX11.SamplerPresets";
            }
        }

        protected override void Initialize()
        {
            this.CreateLinearWrap();
            this.CreateLinearBorder();
            this.CreateLinearClamp();

            this.CreatePointBorder();
            this.CreatePointClamp();
            this.CreatePointWrap();
        }

        private void CreateLinearWrap()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipLinear
            };
            this.AddState("LinearWrap", sd);
        }

        private void CreateLinearClamp()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipLinear
            };
            this.AddState("LinearClamp", sd);
        }

        private void CreateLinearBorder()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                AddressW = TextureAddressMode.Border,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipLinear,
                BorderColor=new SlimDX.Color4(1,0,0,0)
            };
            this.AddState("LinearBorder", sd);
        }

        private void CreatePointWrap()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipPoint
            };
            this.AddState("PointWrap", sd);
        }

        private void CreatePointClamp()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipPoint
            };
            this.AddState("PointClamp", sd);
        }

        private void CreatePointBorder()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                AddressW = TextureAddressMode.Border,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipPoint,
                BorderColor = new SlimDX.Color4(1, 0, 0, 0)
            };
            this.AddState("PointBorder", sd);
        }
    }
}
