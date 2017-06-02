using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public static class EffectVariableHelpers
    {
        #region Scalar
        public static void Set(this EffectVariable var, bool value)
        {
            var.AsScalar().Set(value);
        }

        public static void Set(this EffectVariable var, int value)
        {
            var.AsScalar().Set(value);
        }

        public static void Set(this EffectVariable var, float value)
        {
            var.AsScalar().Set(value);
        }
        #endregion

        #region Scalar Arrays
        public static void Set(this EffectVariable var, bool[] value)
        {
            var.AsScalar().Set(value);
        }

        public static void Set(this EffectVariable var, int[] value)
        {
            var.AsScalar().Set(value);
        }

        public static void Set(this EffectVariable var, float[] value)
        {
            var.AsScalar().Set(value);
        }
        #endregion

        #region Vectors
        public static void Set(this EffectVariable var, Vector2 value)
        {
            var.AsVector().Set(value);
        }

        public static void Set(this EffectVariable var, Vector3 value)
        {
            var.AsVector().Set(value);
        }

        public static void Set(this EffectVariable var, Vector4 value)
        {
            var.AsVector().Set(value);
        }

        public static void Set(this EffectVariable var, Color4 value)
        {
            var.AsVector().Set(value);
        }

        public static void Set(this EffectVariable var, Vector4[] value)
        {
            var.AsVector().Set(value);
        }

        public static void Set(this EffectVariable var, Color4[] value)
        {
            var.AsVector().Set(value);
        }
        #endregion

        #region Transforms
        public static void Set(this EffectVariable var, Matrix value)
        {
            var.AsMatrix().SetMatrix(value);
        }

        public static void Set(this EffectVariable var, Matrix[] value)
        {
            var.AsMatrix().SetMatrixArray(value);
        }
        #endregion

        #region Resources
        public static void Set(this EffectVariable var, ShaderResourceView value)
        {
            var.AsResource().SetResource(value);
        }

        public static void Set(this EffectVariable var, ShaderResourceView[] value)
        {
            var.AsResource().SetResourceArray(value);
        }


        public static void Set(this EffectVariable var, UnorderedAccessView value)
        {
            var.AsUnorderedAccessView().SetView(value);
        }
        #endregion

        #region Samplers
        public static void Set(this EffectVariable var, SamplerState sampler)
        {
            if (sampler != null)
            {
                var.AsSampler().SetSamplerState(0, sampler);
            }
            else
            {
                var.AsSampler().UndoSetSamplerState(0);
            }
        }
        #endregion
    }

    public partial class DX11ShaderInstance
    {
        #region Scalar
        public void SetByName(string name, bool value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, int value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, float value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }
        #endregion

        #region Scalar Arrays
        public void SetByName(string name, bool[] value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, int[] value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, float[] value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }
        #endregion

        #region Vectors
        public void SetByName(string name, Vector2 value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, Vector3 value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, Vector4 value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, Color4 value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, Vector4[] value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, Color4[] value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }
        #endregion

        #region Transforms
        public void SetByName(string name, Matrix value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, Matrix[] value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }
        #endregion

        #region Resources
        public void SetByName(string name, ShaderResourceView value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, ShaderResourceView[] value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }


        public void SetByName(string name, UnorderedAccessView value)
        {
            this.effect.GetVariableByName(name).Set(value);
        }

        public void SetByName(string name, UnorderedAccessView value, int counter = -1)
        {
            this.effect.GetVariableByName(name).Set(value);
            this.resetuavs.Add(new CounterResetUAV(value, counter));
        }
        #endregion

        #region Samplers
        public void SetByName(string name, SamplerState sampler)
        {
            this.effect.GetVariableByName(name).Set(sampler);
        }
        #endregion
    }
}
