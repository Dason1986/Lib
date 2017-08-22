using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceProxy
    {
        /// <summary>
        /// 
        /// </summary>
        public static Action<IContextChannel> AddHeader;
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {

        /// <summary>
        /// 
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected abstract void Dispose(bool disposing);
        /// <summary>
        /// 
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.ExplicitDispose();
        }

    }
    /// <summary>
    /// 表示用於調用WFC Service的客戶端服務代理類型。
    /// </summary>
    /// <typeparam name="T">需要调用的服务契约类型。</typeparam>
    public sealed class ServiceProxy<T> : DisposableObject
        where T : class, IDisposable
    {
        private T _client;
        private static readonly object Sync = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            lock (Sync)
            {
                Close();
            }
        }

        /// <summary>
        /// 獲取調用WCF服務的通道。
        /// </summary>
        public T Channel
        {
            get
            {
                lock (Sync)
                {
                    if (_client != null)
                    {
                        var state = ((IClientChannel)_client).State;
                        if (state == CommunicationState.Closed)
                            _client = null;
                        else
                        {
                            if (ServiceProxy.AddHeader != null)
                            {
                                ServiceProxy.AddHeader((IClientChannel)_client);
                            }
                            return _client;
                        }
                    }
                    var factory = ChannelFactoryManager.Instance.GetFactory<T>();

                    _client = factory.CreateChannel();
                    if (ServiceProxy.AddHeader != null)
                    {
                        ServiceProxy.AddHeader(_client as IClientChannel);
                    }

                    ((IClientChannel)_client).Open();
                    return _client;
                }
            }
        }

        /// <summary>
        /// 關閉並斷開客戶端通道（Client Channel）。
        /// </summary>
        /// <remarks> 
        /// 如果使用using語句對ServiceProxy進行了包裹，那麼當程序執行點離開using的
        /// 覆蓋範圍時，Close方法會被自動調用，此時客戶端無需顯式調用Close方法。反之
        /// 如果沒有使用using語句，那麼則需要顯式調用Close方法。
        /// </remarks>
        public void Close()
        {
            if (_client != null)
                ((IClientChannel)_client).Close();
        }

    }
    internal sealed class ChannelFactoryManager : DisposableObject
    {

        private static readonly Dictionary<Type, ChannelFactory> Factories = new Dictionary<Type, ChannelFactory>();
        private static readonly object Sync = new object();
        private static readonly ChannelFactoryManager instance = new ChannelFactoryManager();

        static ChannelFactoryManager() { }
        private ChannelFactoryManager() { }

        /// <summary>
        /// 獲取<c>ChannelFactoryManager</c>的單件（Singleton）实例。
        /// </summary>
        public static ChannelFactoryManager Instance
        {
            get { return instance; }
        }


        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            lock (Sync)
            {
                foreach (var factory in Factories.Keys.Select(type => Factories[type]))
                {
                    try
                    {
                        factory.Close();
                    }
                    catch
                    {
                        factory.Abort();
                    }
                }
                Factories.Clear();
            }
        }

        /// <summary>
        /// 獲取與指定服務契約的類型相關的Channel Factory實例。
        /// </summary>
        /// <typeparam name="T">服務契約的類型。</typeparam>
        /// <returns>與指定服務契約類型相關的Channel Factory實例。</returns>
        public ChannelFactory<T> GetFactory<T>()
            where T : class, IDisposable
        {
            lock (Sync)
            {
                ChannelFactory factory;
                if (Factories.TryGetValue(typeof(T), out factory)) return factory as ChannelFactory<T>;
                factory = new ChannelFactory<T>(typeof(T).Name);


                factory.Open();
                Factories.Add(typeof(T), factory);
                return (ChannelFactory<T>)factory;
            }
        }

    }
}
