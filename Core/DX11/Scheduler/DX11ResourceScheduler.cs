using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FeralTic.DX11
{
    public class DX11ResourceScheduler
    {
        private DX11RenderContext context;

        private List<DX11SchedulerThread> threads = new List<DX11SchedulerThread>();

        private List<IDX11ScheduledTask> tasklist = new List<IDX11ScheduledTask>();
        private object m_lock = new object();

        private int thrcount;


        public DX11ResourceScheduler(DX11RenderContext context, int threadcount = 1)
        {
            this.context = context;
            this.thrcount = threadcount;

            
        }

        public void Initialize()
        {
            for (int i = 0; i < this.thrcount; i++)
            {
                DX11SchedulerThread thread = new DX11SchedulerThread(this, context);
                this.threads.Add(thread);
                thread.Start();
            }
        }

        public void AddTask(IDX11ScheduledTask task)
        {
            lock (m_lock)
            {
                this.tasklist.Add(task);
            }
        }

        public IDX11ScheduledTask GetTask()
        {
            IDX11ScheduledTask task = null;
            lock (m_lock)
            {
                if (tasklist.Count > 0)
                {
                    task = tasklist[0];
                    tasklist.RemoveAt(0);
                }
            }
            return task;
        }

        public void Dispose()
        {
            foreach (DX11SchedulerThread thread in this.threads)
            {
                thread.Stop();
            }
        }
    }
}
