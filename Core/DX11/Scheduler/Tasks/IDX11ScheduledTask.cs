using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public enum eDX11SheduleTaskStatus { Queued, Loading, Completed,Error,Aborted }

    public delegate void TaskStatusChangedDelegate(IDX11ScheduledTask task);

    public interface IDX11ScheduledTask
    {
        /// <summary>
        /// Render context for resource load
        /// </summary>
        DX11RenderContext Context { get; }

        /// <summary>
        /// Current task status
        /// </summary>
        eDX11SheduleTaskStatus Status { get; }

        /// <summary>
        /// Is this task being marked as dirty (eg, need dispose right after load)
        /// </summary>
        bool IsDirty { get; }

        event TaskStatusChangedDelegate StatusChanged;

        void Process();

        /// <summary>
        /// This is called when tasked been marked as dirty while loading
        /// </summary>
        void Dispose();

        /// <summary>
        /// Call this method in case a resource load needs abort,
        /// for example, a resource in pool queue needs to just be removed
        /// 2 cases: if resource in queue when load is called it will be ignored and put in aborted status, scheduler does it, no need
        /// if resource already loading, leave if finish and dispose resource directly after load.
        /// </summary>
        void MarkForAbort();   
    }

    public abstract class DX11AbstractLoadTask<T> : IDX11ScheduledTask where T : IDX11Resource
    {
        public T Resource { get; protected set; }

        public DX11RenderContext Context { get; protected set; }

        public bool IsDirty { get; protected set; }

        public DX11AbstractLoadTask(DX11RenderContext context)
        {
            this.Context = context;
        }

        private eDX11SheduleTaskStatus status;

        public eDX11SheduleTaskStatus Status
        {
            get { return this.status; }
        }

        public event TaskStatusChangedDelegate StatusChanged;

        private void SetStatus(eDX11SheduleTaskStatus status)
        {
            this.status = status;

            if (this.StatusChanged != null)
            {
                this.StatusChanged(this);
            }
        }

        public void Process()
        {
            if (this.IsDirty) { this.SetStatus(eDX11SheduleTaskStatus.Aborted); return; }

            this.SetStatus(eDX11SheduleTaskStatus.Loading);

            try
            {
                this.DoProcess();

                if (this.IsDirty)
                {
                    this.Dispose();
                    this.SetStatus(eDX11SheduleTaskStatus.Aborted);
                }
                else
                {
                    this.SetStatus(eDX11SheduleTaskStatus.Completed);
                }
            }
            catch
            {
                this.SetStatus(eDX11SheduleTaskStatus.Error);
            }
        }

        protected abstract void DoProcess();
        

        public void MarkForAbort()
        {
            if (this.status != eDX11SheduleTaskStatus.Completed)
            {
                this.IsDirty = true;
            }
        }

        public abstract void Dispose();
    }



}
