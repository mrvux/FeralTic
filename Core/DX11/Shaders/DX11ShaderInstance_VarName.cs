using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace FeralTic.DX11
{
    public partial class DX11ShaderInstance
    {
        #region Scalar
        public void SetByName(string name, bool value)
        {
            this.effect.GetVariableByName(name).AsScalar().Set(value);
        }

        public void SetByName(string name, int value)
        {
            this.effect.GetVariableByName(name).AsScalar().Set(value);
        }

        public void SetByName(string name, float value)
        {
            this.effect.GetVariableByName(name).AsScalar().Set(value);
        }
        #endregion

        #region Scalar Arrays
        public void SetByName(string name, bool[] value)
        {
            this.effect.GetVariableByName(name).AsScalar().Set(value);
        }

        public void SetByName(string name, int[] value)
        {
            this.effect.GetVariableByName(name).AsScalar().Set(value);
        }

        public void SetByName(string name, float[] value)
        {
            this.effect.GetVariableByName(name).AsScalar().Set(value);
        }
        #endregion

        #region Vectors
        public void SetByName(string name, Vector2 value)
        {
            this.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void SetByName(string name, Vector3 value)
        {
            this.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void SetByName(string name, Vector4 value)
        {
            this.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void SetByName(string name, Color4 value)
        {
            this.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void SetByName(string name, Vector4[] value)
        {
            this.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void SetByName(string name, Color4[] value)
        {
            this.effect.GetVariableByName(name).AsVector().Set(value);
        }
        #endregion

        #region Transforms
        public void SetByName(string name, Matrix value)
        {
            this.effect.GetVariableByName(name).AsMatrix().SetMatrix(value);
        }

        public void SetByName(string name, Matrix[] value)
        {
            this.effect.GetVariableByName(name).AsMatrix().SetMatrixArray(value);
        }
        #endregion

        #region Resources
        public void SetByName(string name, ShaderResourceView value)
        {
            this.effect.GetVariableByName(name).AsResource().SetResource(value);
        }

        public void SetByName(string name, ShaderResourceView[] value)
        {
            this.effect.GetVariableByName(name).AsResource().SetResourceArray(value);
        }


        public void SetByName(string name, UnorderedAccessView value)
        {
            this.effect.GetVariableByName(name).AsUnorderedAccessView().SetView(value);
        }

        public void SetByName(string name, UnorderedAccessView value, int counter = -1)
        {
            this.effect.GetVariableByName(name).AsUnorderedAccessView().SetView(value);
            this.resetuavs.Add(new CounterResetUAV(value, counter));
        }
        #endregion

        #region Samplers
        public void SetByName(string name, SamplerState sampler)
        {
            if (sampler != null)
            {
                this.effect.GetVariableByName(name).AsSampler().SetSamplerState(0,sampler);
            }
            else
            {
                this.effect.GetVariableByName(name).AsSampler().UndoSetSamplerState(0);
            }
        }
        #endregion
    }
}
