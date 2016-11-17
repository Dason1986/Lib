using Library.ComponentModel.Logic;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Library.Infrastructure.Application
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseMultiThreadingLogicService : BaseLogicService
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseMultiThreadingLogicService()
        {


        }
        #region property

        int _threadCount = 3;
        int _batSize = 20;
        /// <summary>
        /// 
        /// </summary>
        public int ThreadCount
        {
            get
            {
                return _threadCount;
            }
            protected set
            {
                if (value < 1 || value > 10) throw new Exception();
                if (_threadCount != value) _threadCount = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected long TotalRecord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected long CompletedRecord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BatchSize
        {
            get
            {
                return _batSize;
            }
            protected set
            {
                if (value < 2 || value > 100) throw new Exception();
                if (_batSize != value) _batSize = value;
            }
        }
        #endregion
        #region event

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<LogicServiceProgress> Progress;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<LogicServiceFailure> Failure;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="beginIndex"></param>
        /// <param name="endIndex"></param>
        protected void OnFailure(Exception error, long beginIndex, long endIndex)
        {
            var handler = Failure;
            if (handler == null) return;

            SynchronizationContext.Current.Post(n =>
            {
                handler.Invoke(this, new LogicServiceFailure(error, beginIndex, endIndex));
            }, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginIndex"></param>
        /// <param name="endIndex"></param>
        protected void OnProgress(long beginIndex, long endIndex)
        {
            var handler = Progress;
            if (handler == null) return;

            SynchronizationContext.Current.Post(n =>
            {
                handler.Invoke(this, new LogicServiceProgress(TotalRecord, CompletedRecord, beginIndex, endIndex));
            }, null);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        protected virtual void ThreadPross(long begin, long end)
        {
            int size = BatchSize;
            //      Logger.Info(string.Format("begin:{0} end:{1}", begin, end - 1));
            for (long index = begin; index < end; index = index + size)
            {
                var endindex = index + size - 1;
                if (endindex >= end) endindex = end;
                try
                {
                    ThreadProssSize((int)index, (int)endindex);
                    CompletedRecord = CompletedRecord + (endindex - index);
                    OnProgress(index, endindex);
                }
                catch (Exception ex)
                {
                    Logger.ErrorByContent(ex, "處理失敗",new Dictionary<string, object>() { { "begin", index}, { "end", endindex} });
                    OnFailure(ex, index, endindex);

                }



            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected sealed override void OnDowrok()
        {

            IMultiThreadingOption option = this.ServiceOption as IMultiThreadingOption;
            if (option != null)
            {
                ThreadCount = option.ThreadCount;
                BatchSize = option.BatchSize;
            }
            TotalRecord = GetTotalRecord();
            ThreadPross();




        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract int GetTotalRecord();
        /// <summary>
        /// 
        /// </summary>
        protected virtual void ThreadPross()
        {


            var total = TotalRecord / BatchSize;
            if (total <= ThreadCount || ThreadCount == 1)
            {

                ThreadPross(0, (int)TotalRecord - 1);

            }
            else
            {
                Thread[] thrads = new Thread[ThreadCount];
                var totalsize = (decimal)TotalRecord / ThreadCount;
                for (int i = 0; i < ThreadCount; i++)
                {
                    int currentindex = i;
                    thrads[currentindex] = new Thread(n =>
                    {
                        var beging = (long)Math.Ceiling(totalsize * currentindex);
                        var end = (long)Math.Floor(beging + totalsize);
                        if (end >= TotalRecord)
                        {
                            end = (int)TotalRecord;
                        }

                        ThreadPross(beging, end - 1);

                    });

                }
                for (int i = 0; i < ThreadCount; i++)
                {
                    thrads[i].Start();
                }
                for (int i = 0; i < ThreadCount; i++)
                {
                    thrads[i].Join();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginindex"></param>
        /// <param name="endindex"></param>
        protected abstract void ThreadProssSize(int beginindex, int endindex);
    }
}