using System;

namespace ExternalSort.Core
{
    public interface IExternalWriter<in TWritableType> : IDisposable
        where TWritableType : class, IComparable
    {
        void OpenFile(string filename);
        long LengthFile { get; }
        void Write(TWritableType type);
    }
}
