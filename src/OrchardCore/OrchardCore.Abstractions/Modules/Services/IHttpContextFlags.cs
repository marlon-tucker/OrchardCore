using System;
using System.Collections.Generic;
using System.Text;

namespace OrchardCore.Modules
{
    public interface IHttpContextFlags
    {
        bool IsSet(object flag);

        void Set(object flag);

        void Unset(object flag);

        IDisposable Using(object flag);
    }
}
