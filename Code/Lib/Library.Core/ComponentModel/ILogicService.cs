using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Library.ComponentModel.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogicService
    {
        /// <summary>
        /// 
        /// </summary>
        IOption Option { get; set; }
        /// <summary>
        /// 
        /// </summary>
        void Start();

    }
    /// <summary>
    /// 
    /// </summary>
    public interface IOption
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public interface IMultiThreadingOption : IOption
    {
        /// <summary>
        /// 
        /// </summary>
        int ThreadCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        int BatchSize { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IOptionCommandBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        void RumCommandLine();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IOption GetOption();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOption"></typeparam>
    public interface IOptionCommandBuilder<TOption> : IOptionCommandBuilder where TOption : IOption
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new TOption GetOption();
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class EmptyOption : IOption
    {
        static EmptyOption()
        {
            Epmty = new EmptyOption();

        }
        /// <summary>
        /// 
        /// </summary>
        public static EmptyOption Epmty { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class EmptyOptionCommandBuilder : IOptionCommandBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IOption GetOption()
        {
            return EmptyOption.Epmty;
        }
        /// <summary>
        /// 
        /// </summary>

        public void RumCommandLine()
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LogicServiceProgress : EventArgs
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRecord"></param>
        /// <param name="completedRecord"></param>
        /// <param name="beginIndex"></param>
        /// <param name="endIndex"></param>
        public LogicServiceProgress(long totalRecord, long completedRecord, long beginIndex, long endIndex)
        {
            TotalRecord = totalRecord;
            CompletedRecord = completedRecord;
            BeginIndex = beginIndex;
            EndIndex = endIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        public long TotalRecord { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public long CompletedRecord { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public long BeginIndex { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public long EndIndex { get; protected set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LogicServiceFailure : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="beginIndex"></param>
        /// <param name="endIndex"></param>
        public LogicServiceFailure(Exception error, long beginIndex, long endIndex)
        {
            Error = error;
            BeginIndex = beginIndex;
            EndIndex = endIndex;
        }
        /// <summary>
        /// 
        /// </summary>
        public Exception Error { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public long BeginIndex { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public long EndIndex { get; protected set; }
    }
    /*
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
        /// <summary>
        /// 
        /// </summary>
        public bool ThrowError { get; set; }

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

                    OnFailure(ex, index, endindex);
                    if (ThrowError) throw new Exception(string.Format("{0} - {1}:處理失敗", index, endindex), ex);
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
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseLogicService : ILogicService
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseLogicService()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }
        protected NLog.ILogger Logger { get; set; }
        IOption ILogicService.Option
        {
            get
            {
                return ServiceOption;
            }

            set
            {
                ServiceOption = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected abstract IOption ServiceOption { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public event EventHandler Completed;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usetime"></param>
        protected void OnCompleted(TimeSpan usetime)
        {
            var handler = Completed;
            if (handler == null) return;
            SynchronizationContext.Current.Post(n =>
            {

                handler.Invoke(this, EventArgs.Empty);
            }, null);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (!OnVerification())
            {
                Logger.Warn(string.Format("驗證失敗"), 1);
                return;
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Logger.Info(string.Format("開始執行"), 1);
            OnDowrok();
            watch.Stop();
            OnCompleted(watch.Elapsed);
        }
        Thread threadPool;
        /// <summary>
        /// 
        /// </summary>
        public void StartAsyn()
        {
            if (threadPool != null) return;
            threadPool = new Thread(n =>
            {
                Start();
            });
            threadPool.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        protected abstract void OnDowrok();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnVerification()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void StopAsyn()
        {
            if (threadPool != null)
            {
                threadPool.Abort();
                threadPool = null;
            }
        }
    }*/
}
