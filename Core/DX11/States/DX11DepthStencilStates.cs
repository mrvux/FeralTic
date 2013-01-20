using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public class DX11DepthStencilStates : DX11RenderStates<DepthStencilStateDescription>
    {
        private static DX11DepthStencilStates instance;

        public static DX11DepthStencilStates Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DX11DepthStencilStates();
                    instance.Initialize();
                }
                return instance;
            }
        }

        public override string EnumName
        {
            get
            {
                return "DX11.DepthStencilPresets";
            }
        }

        protected override void Initialize()
        {
            this.CreateLessReadOnly();
            this.CreateLessRW();
            this.CreateNoDepth();
            this.CreateLessEqualReadOnly();
            this.CreateLessEqualRW();
        }

        private void CreateNoDepth()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always
            };

            this.AddState("NoDepth", ds);
        }

        private void CreateLessReadOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Less
            };

            this.AddState("LessRead", ds);
        }

        private void CreateLessEqualReadOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.LessEqual
            };

            this.AddState("LessEqualRead", ds);
        }

        private void CreateLessRW()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less
            };

            this.AddState("LessReadWrite", ds);
        }

        private void CreateLessEqualRW()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.LessEqual
            };

            this.AddState("LessEqualReadWrite", ds);
        }
    }
}
