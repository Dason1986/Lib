﻿using System;

namespace Library.Domain.Data.EF
{
    public abstract class DomainModuleProvider : IDomainModuleProvider
    {
        protected DomainModuleProvider(EFContext context)
        {
            Context = context;
            UnitOfWork = new UnitOfWork(context);
        }

        public UnitOfWork UnitOfWork { get; private set; }
        protected EFContext Context { get; private set; }

        public Repository<TEntity> CreateRepository<TEntity>() where TEntity : Entity
        {
            return new Repository<TEntity>(Context);
        }

        IRepository<TEntity> IDomainModuleProvider.CreateRepository<TEntity>()
        {
            return this.CreateRepository<TEntity>();
        }

        IUnitOfWork IDomainModuleProvider.UnitOfWork { get { return this.UnitOfWork; } }

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Context != null)
                    {
                        Context.Dispose();
                    }
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                Context = null;
                UnitOfWork = null;
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~DomainModuleProvider()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}