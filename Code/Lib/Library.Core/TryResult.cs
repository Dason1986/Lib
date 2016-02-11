using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    /// <summary>
    ///
    /// </summary>

    public struct TryResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="exceptions"></param>
        public TryResult(IEnumerable<Exception> exceptions)
        {
            if (exceptions != null && exceptions.Any())
            {
                _hasError = true;
                this._errors = exceptions.ToArray();
            }
            else
            {
                _hasError = false;

                this._errors = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="exceptions"></param>
        public TryResult(params Exception[] exceptions)
        {
            if (exceptions != null && exceptions.Length > 0)
            {
                _hasError = true;

                this._errors = exceptions;
            }
            else
            {
                _hasError = false;

                this._errors = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flag"></param>
        public TryResult(bool flag)
        {
            _hasError = !flag;

            this._errors = null;
        }
        /// <summary>
        ///
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }
        }

        private readonly bool _hasError;
        private readonly Exception[] _errors;

        /// <summary>
        ///
        /// </summary>
        public Exception Error
        {
            get { return _errors != null && _errors.Length > 0 ? _errors[0] : null; }
        }

        /// <summary>
        ///
        /// </summary>
        public Exception[] Errors
        {
            get { return _errors; }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static implicit operator TryResult(Exception ex)
        {
            return new TryResult(ex);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static implicit operator TryResult(bool flag)
        {
            return new TryResult(flag);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator bool(TryResult value)
        {
            return !value.HasError;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool operator true(TryResult x)
        {
            return !x.HasError;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool operator false(TryResult x)
        {
            return x.HasError;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct TryResult<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public TryResult(T value)
        {
            _value = value;
            _hasError = false;
            _error = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="exception"></param>
        public TryResult(Exception exception)
            : this(default(T), exception)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="defaultValue"></param>
        public TryResult(T defaultValue, Exception exception)
        {
            _value = defaultValue;
            _hasError = exception != null;
            this._error = exception;
        }
        private readonly T _value;
        private readonly bool _hasError;
        private readonly Exception _error;
        /// <summary>
        ///
        /// </summary>
        public Exception Error
        {
            get { return _error; }
        }

        /// <summary>
        ///
        /// </summary>
        public T Value
        {
            get { return _value; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Error == null ? string.Format("[{1}][{0}]", Value, typeof(T).FullName) : string.Format("[{0}][{1}]", Error.GetType().Name, Error.Message);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator TryResult<T>(T value)
        {
            return new TryResult<T>(value);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static implicit operator TryResult<T>(Exception ex)
        {
            return new TryResult<T>(ex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator T(TryResult<T> value)
        {
            return value.Value;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool operator true(TryResult<T> x)
        {
            return !x.HasError;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool operator false(TryResult<T> x)
        {
            return x.HasError;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TryResult<TModel> Cast<TModel>(TryResult<object> obj)
        {
            if (obj.HasError) return obj.Error;
            if (obj.Value is TModel == false)
                return new Exception(String.Format("Type Not Same [{0}] [{1}]", obj.Value.GetType().FullName, typeof(TModel).FullName));
            return (TModel)obj;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TryResult<TModel> Cast<TModel>(object obj)
        {
            if (obj == null) return new TryResult<TModel>();
            if (obj is TModel == false)
                return new Exception(String.Format("Type Not Same [{0}] [{1}]", obj.GetType().FullName, typeof(TModel).FullName));
            return new TryResult<TModel>((TModel)obj);
        }
    }
}