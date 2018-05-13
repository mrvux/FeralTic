using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace FeralTic.DX11
{
    public class DX11SchedulerThread
    {
        private Thread thr;

        private DX11ResourceScheduler scheduler;
        private DX11RenderContext context;

        private bool running = false;

        private int sleepTime;

        public int SleepTime
        {
            get { return this.sleepTime; }
            set { this.sleepTime = value; }
        }
            

        public DX11SchedulerThread(DX11ResourceScheduler scheduler, DX11RenderContext context)
        {
            this.scheduler = scheduler;
            this.context = context;
            this.sleepTime = 10;
        }

        public void Start()
        {
            if (this.running) { return; }
            this.running = true;

            this.thr = new Thread(new ThreadStart(this.Run));
            this.thr.Priority = ThreadPriority.BelowNormal;
            this.thr.Start();
        }

        public void Stop()
        {
            this.running = false;
            if (this.thr != null)
            {
                // Wait for the thread to terminate. Prevents a ExecutionEngineException after generating the nodelist.xml
                if (this.thr.Join(1000))
                    this.thr = null;
            }
        }

        private void Run()
        {
            while (this.running)
            {
                IDX11ScheduledTask task = this.scheduler.GetTask();

                if (task != null)
                {
                    task.Process();
                }
                else
                {
                    if (this.sleepTime > 0)
                        Thread.Sleep(this.sleepTime);
                }
            }
        }

    }
}
