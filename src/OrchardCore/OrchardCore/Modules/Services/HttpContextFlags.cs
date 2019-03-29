using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace OrchardCore.Modules
{
    class HttpContextFlags : IHttpContextFlags
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextFlags(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsSet(object flag)
        {
            return _httpContextAccessor.HttpContext.Items.ContainsKey(flag);
        }

        public void Set(object flag)
        {
            _httpContextAccessor.HttpContext.Items.Add(flag, true);
        }


        public void Unset(object flag)
        {
            _httpContextAccessor.HttpContext.Items.Remove(flag);
        }

        public IDisposable Using(object flag)
        {
            var existedPreviously = false;
            if (IsSet(flag))
            {
                existedPreviously = true;
            }
            else
            {
                _httpContextAccessor.HttpContext.Items.Add(flag, true);
            }
            return new HttpContextItemSetter(_httpContextAccessor, flag, existedPreviously);
        }

        private class HttpContextItemSetter : IDisposable
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly object _flag;
            private readonly bool _existedPreviously;

            public HttpContextItemSetter(IHttpContextAccessor httpContextAccessor, object flag, bool existedPreviously)
            {
                _httpContextAccessor = httpContextAccessor;
                _flag = flag;
                _existedPreviously = existedPreviously;
            }

            public void Dispose()
            {
                if (!_existedPreviously)
                {
                    _httpContextAccessor.HttpContext.Items.Remove(_flag);
                }                
            }
        }
    }
}
