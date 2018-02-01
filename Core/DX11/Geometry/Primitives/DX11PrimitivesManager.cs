using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using SlimDX.Direct3D11;

using FeralTic.DX11.Resources;
using SlimDX;

namespace FeralTic.DX11.Geometry
{
    public partial class DX11PrimitivesManager : IDisposable
    {
        private DX11RenderContext context;

        private DX11NullGeometry fulltri;
        private Effect fulltrivs;

        private Effect fulltrivsposition;
        private EffectPass fulltrivspositionpass;


        private Effect fulltrivstextransform;
        private EffectPass fulltrivstextransformpass;
        private EffectMatrixVariable fulltrivstextransformmat;

        private EffectPass vsonlypass;
        private EffectPass fullscreenpass;

        private DX11IndexedGeometry quad;
        private Effect passtroughVS;
        private InputLayout quadlayout;

        
        public DX11PrimitivesManager(DX11RenderContext context)
        {
            this.context = context;
            this.InitializeDelegates();
            Effect e = this.FullTriVS;

            DX11Effect shader = DX11Effect.FromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTriPosOnly.fx");
            this.fulltrivsposition = new Effect(context.Device, shader.ByteCode);
            this.fulltrivspositionpass = this.fulltrivs.GetTechniqueByIndex(0).GetPassByIndex(0);

            shader = DX11Effect.FromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTriTexTransform.fx");
            this.fulltrivstextransform = new Effect(context.Device, shader.ByteCode);
            this.fulltrivstextransformpass = this.fulltrivstextransform.GetTechniqueByIndex(0).GetPassByIndex(0);
            this.fulltrivstextransformmat = this.fulltrivstextransform.GetVariableByName("tTex").AsMatrix();
        }

        private float Map(float Input, float InMin, float InMax, float OutMin, float OutMax)
        {
            float range = InMax - InMin;
            float normalized = (Input - InMin) / range;
            float output = OutMin + normalized * (OutMax - OutMin);
            float min = Math.Min(OutMin, OutMax);
            float max = Math.Max(OutMin, OutMax);
            return Math.Min(Math.Max(output, min), max);
        }

        public DX11NullGeometry FullScreenTriangle
        {
            get
            {
                if (this.fulltri == null)
                {
                    this.fulltri = new DX11NullGeometry(this.context, 3);
                    this.fulltri.Topology = PrimitiveTopology.TriangleList;
                }
                return this.fulltri;
            }
        }

        public DX11IndexedGeometry FullScreenQuad
        {
            get
            {
                if (this.quad == null)
                {
                    this.quad = this.QuadTextured();
                }

                return this.quad;
            }
        }

        public InputLayout QuadLayout
        {

            get
            {

                if (this.quadlayout == null)
                {

                    this.quadlayout = new InputLayout(context.Device, this.PasstroughVS.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature, this.FullScreenQuad.InputLayout);

                }

                return this.quadlayout;

            }
        }

        public Effect FullTriVS
        {
            get
            {
                if (this.fulltrivs == null)
                {
                    DX11Effect shader = DX11Effect.FromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.VSFullTri.fx");
                    this.fulltrivs = new Effect(context.Device, shader.ByteCode);
                    this.vsonlypass = this.fulltrivs.GetTechniqueByName("FullScreenTriangleVSOnly").GetPassByIndex(0);
                    this.fullscreenpass = this.fulltrivs.GetTechniqueByName("FullScreenTriangle").GetPassByIndex(0);
                }
                return this.fulltrivs;
            }
        }

        public Effect PasstroughVS
        {
            get
            {

                if (this.passtroughVS == null)
                {

                    DX11Effect shader = DX11Effect.FromResource(Assembly.GetExecutingAssembly(), "FeralTic.Effects.DefaultVS.fx");
                    this.passtroughVS = new Effect(context.Device, shader.ByteCode);
                }
                return this.passtroughVS;
            }
        }

             
        public void ApplyFullTriVSPosition()
        {
            this.FullScreenTriangle.Bind(null);
            this.fulltrivspositionpass.Apply(this.context.CurrentDeviceContext);
        }

        public void ApplyFullTriVSTexTransform(Matrix baseTransform)
        {
            this.FullScreenTriangle.Bind(null);
            this.fulltrivstextransformpass.Apply(this.context.CurrentDeviceContext);
            this.fulltrivstextransformmat.SetMatrix(baseTransform.AsTextureTransform());
        }


        public void ApplyFullTriVS()
        {
            this.FullScreenTriangle.Bind(null);
            this.vsonlypass.Apply(this.context.CurrentDeviceContext);
        }

        public void ApplyFullTri()
        {
            this.FullScreenTriangle.Bind(null);
            this.fullscreenpass.Apply(this.context.CurrentDeviceContext);
        }

        public void Dispose()
        {
            if (this.fulltrivs != null) { this.fulltrivs.Dispose(); }
            if (this.fulltrivsposition != null) { this.fulltrivsposition.Dispose(); }
            if (this.fulltrivstextransform != null) { this.fulltrivstextransform.Dispose(); }
        }

    }
}
