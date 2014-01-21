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
            this.CreateWriteOnly();
            this.CreateLessStencilIncrement();
            this.CreateStencilLess();
            this.CreateStencilGreater();
            this.CreateStencilIncrement();
            this.CreateStencilInvert();
            this.CreateLessStencilZero();

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

        private void CreateWriteOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Always
            };

            this.AddState("WriteOnly", ds);
        }

        private void CreateLessStencilIncrement()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                }
            };

            this.AddState("LessReadStencilIncrement", ds);
        }

        private void CreateLessStencilZero()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Zero
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Zero
                }
            };

            this.AddState("LessReadStencilZero", ds);
        }

        private void CreateStencilLess()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            this.AddState("StencilLess", ds);
        }

        private void CreateStencilGreater()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Greater,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Greater,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            this.AddState("StencilGreater", ds);
        }

        private void CreateStencilIncrement()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                }
            };

            this.AddState("StencilIncrement", ds);
        }

        private void CreateStencilInvert()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Invert
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Invert
                }
            };

            this.AddState("StencilInvert", ds);
        }
    }
}
