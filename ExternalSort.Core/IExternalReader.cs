using System;

namespace ExternalSort.Core
{
    public interface IExternalReader<out TReadableType> : IDisposable
        where TReadableType : class, IComparable
    {
        void OpenFile(string filename);
        TReadableType Current { get; }
        bool IsCanRead { get; }
        bool IsNext();
    }
}
