using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public class DX11RasterizerStates : DX11RenderStates<RasterizerStateDescription>
    {
        private static DX11RasterizerStates instance;

        public static DX11RasterizerStates Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DX11RasterizerStates();
                    instance.Initialize();
                }
                return instance;
            }
        }


        public override string EnumName
        {
            get
            {
                return "DX11.RasterizerPresets";
            }
        }

        protected override void Initialize()
        {
            this.CreateBackCullSimple();
            this.CreateFrontCullSimple();
            this.CreateNoCullSimple();
            this.CreateLine();
        }

        private void CreateNoCullSimple()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f             
            };
            this.AddState("NoCullSimple", rsd);

            rsd.FillMode = FillMode.Wireframe;
            this.AddState("NoCullWireframe", rsd);
        }

        private void CreateBackCullSimple()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            this.AddState("BackCullSimple", rsd);

            rsd.FillMode = FillMode.Wireframe;
            this.AddState("BackCullWireframe", rsd);
        }

        private void CreateFrontCullSimple()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.Front,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            this.AddState("FrontCullSimple", rsd);

            rsd.FillMode = FillMode.Wireframe;
            this.AddState("FrontCullWireframe", rsd);
        }

        private void CreateLine()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = true,
                IsDepthClipEnabled = true,
                IsFrontCounterclockwise = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };

            this.AddState("LineAlpha", rsd);

            rsd.IsMultisampleEnabled = true;
            this.AddState("LineQuadrilateral", rsd);
        }
    }
}
