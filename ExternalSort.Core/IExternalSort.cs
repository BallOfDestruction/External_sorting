using System;

namespace ExternalSort.Core
{
    public interface IExternalSort
    {
        void DoSorting<TReader, TWriter, TInnerType>(string path)
            where TInnerType : class, IComparable
            where TReader : IExternalReader<TInnerType>
            where TWriter : IExternalWriter<TInnerType>;

        bool IsSorted<TReader, TInnerType>(string path)
            where TReader : IExternalReader<TInnerType>
            where TInnerType : class, IComparable;
    }
}
