using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public abstract class DX11RenderStates<T>
    {
        private Dictionary<string, T> states = new Dictionary<string, T>();

        private List<string> statekeys = new List<string>();

        protected DX11RenderStates() { }

        protected abstract void Initialize();
        public abstract string EnumName { get ; }

        protected void AddState(string key, T state)
        {
            this.statekeys.Add(key);
            this.states.Add(key, state);
        }

        public T GetState(string key)
        {
            return this.states[key];
        }

        public string[] StateKeys
        {
            get { return this.statekeys.ToArray(); }
        }
    }
}
