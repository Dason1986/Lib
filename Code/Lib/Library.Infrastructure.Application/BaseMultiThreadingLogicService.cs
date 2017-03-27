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

        private int _threadCount = 3;
        private int _batSize = 20;

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

        #endregion property

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

        #endregion event

        private void ThreadPross(PageItem pageItem)
        {
            int index = (int)pageItem.BeginIndex;
            int endindex = (int)pageItem.EndIndex;
            try
            {
                ThreadProssSize(index, (int)endindex, pageItem.Take);
                CompletedRecord = CompletedRecord + (endindex - index);
                OnProgress(index, endindex);
            }
            catch (Exception ex)
            {
                Logger.ErrorByContent(ex, "處理失敗", new Dictionary<string, object>() { { "begin", index }, { "end", endindex } });
                OnFailure(ex, index, endindex);
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

        private struct PageItem
        {
            public long BeginIndex { get; set; }
            public long EndIndex { get; set; }
            public int Take { get; set; }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ThreadPross()
        {
            var totalPage = TotalRecord / BatchSize;
            Queue<PageItem> pages = new Queue<PageItem>();
            if (totalPage <= ThreadCount || ThreadCount == 1)
            {
                ThreadPross(new PageItem() { BeginIndex = 0, EndIndex = TotalRecord - 1, Take = (int)TotalRecord });
            }
            else
            {
                long currnet = 0;
                while (currnet < TotalRecord)
                {
                    if (currnet + _batSize >= TotalRecord)
                    {
                        pages.Enqueue(new PageItem()
                        {
                            BeginIndex = currnet,
                            EndIndex = TotalRecord - 1,
                            Take = (int)(TotalRecord - currnet)
                        });
                        currnet = currnet + TotalRecord;
                    }
                    else
                    {
                        pages.Enqueue(new PageItem()
                        {
                            BeginIndex = currnet,
                            EndIndex = currnet + _batSize - 1,
                            Take = _batSize
                        });
                        currnet = currnet + _batSize;
                    }
                }

                Thread[] thrads = new Thread[ThreadCount];
                for (int i = 0; i < ThreadCount; i++)
                {
                    thrads[i] = new Thread(() =>
                    {
                        while (pages.Count > 0)
                        {
                            var item = pages.Dequeue();
                            ThreadPross(item);
                        }
                    });
                    thrads[i].Start();
                }
                for (int i = 0; i < ThreadCount; i++)
                {
                    thrads[i].Join();
                }
            }
        }

        ///  <summary>
        ///
        ///  </summary>
        ///  <param name="beginindex"></param>
        ///  <param name="endindex"></param>
        /// <param name="take"></param>
        protected abstract void ThreadProssSize(int beginindex, int endindex, int take);
    }
}