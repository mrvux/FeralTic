using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public static class DX11SamplerStates
    {
        private static SamplerDescription[] descriptions;

        public static SamplerDescription GetState(SamplerStatePreset preset)
        {
            if (descriptions == null)
            {
                Initialize();
            }
            return descriptions[(int)preset];
        }

        public static SamplerDescription GetState(string presetString)
        {
            SamplerStatePreset preset;
            if (Enum.TryParse(presetString, out preset))
            {
                return GetState(preset);
            }
            else
            {
                throw new ArgumentException("preset", "Preset not found");
            }
        }

        private static void Initialize()
        {
            descriptions = new SamplerDescription[Enum.GetValues(typeof(SamplerStatePreset)).Length];
            CreateLinearWrap();
            CreateLinearBorder();
            CreateLinearClamp();
            CreateLinearMirror();

            CreatePointBorder();
            CreatePointClamp();
            CreatePointWrap();
            CreatePointMirror();
        }

        private static void CreateLinearWrap()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipLinear
            };
            descriptions[(int)SamplerStatePreset.LinearWrap] = sd;
        }

        private static void CreateLinearClamp()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipLinear
            };
            descriptions[(int)SamplerStatePreset.LinearClamp] = sd;
        }

        private static void CreateLinearBorder()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                AddressW = TextureAddressMode.Border,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipLinear,
                BorderColor=new SlimDX.Color4(0,0,0,1)
            };
            descriptions[(int)SamplerStatePreset.LinearBorder] = sd;
        }

        private static void CreateLinearMirror()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Mirror,
                AddressV = TextureAddressMode.Mirror,
                AddressW = TextureAddressMode.Mirror,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipLinear,
                BorderColor = new SlimDX.Color4(0, 0, 0, 1)
            };
            descriptions[(int)SamplerStatePreset.LinearMirror] = sd;
        }

        private static void CreatePointWrap()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipPoint
            };
            descriptions[(int)SamplerStatePreset.PointWrap] = sd;
        }

        private static void CreatePointClamp()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipPoint
            };
            descriptions[(int)SamplerStatePreset.PointClamp] = sd;
        }

        private static void CreatePointBorder()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Border,
                AddressV = TextureAddressMode.Border,
                AddressW = TextureAddressMode.Border,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipPoint,
                BorderColor = new SlimDX.Color4(0, 0, 0, 1)
            };
            descriptions[(int)SamplerStatePreset.PointBorder] = sd;
        }

        private static void CreatePointMirror()
        {
            SamplerDescription sd = new SamplerDescription()
            {
                AddressU = TextureAddressMode.Mirror,
                AddressV = TextureAddressMode.Mirror,
                AddressW = TextureAddressMode.Mirror,
                ComparisonFunction = Comparison.Always,
                Filter = SlimDX.Direct3D11.Filter.MinMagMipPoint
            };
            descriptions[(int)SamplerStatePreset.PointMirror] = sd;
        }
    }
}
