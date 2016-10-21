using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApplication4
{
    /// <summary>
    /// Divides an <see cref="IEnumerable"/> into batches of a given size and returns another <see cref="IEnumerable"/>
    /// that iterates over those batches.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Example: {1, 2, 3, 4, 5}, batch size: 3, result: {{1, 2, 3}, {4, 5}}
    /// </remarks>
    internal class BatchEnumerable<T> : IEnumerable<IEnumerable<T>>, IEnumerator<IEnumerable<T>>
    {
        private readonly IEnumerator<T> _innerEnumerator;
        private readonly Func<IReadOnlyCollection<T>, bool> _cutCondition;

        public BatchEnumerable(
            IEnumerable<T> innerEnumerable,
            Func<IReadOnlyCollection<T>, bool> cutCondition)
        {
            if (innerEnumerable == null)
            {
                throw new ArgumentNullException(nameof(innerEnumerable));
            }
            if (cutCondition == null)
            {
                throw new ArgumentNullException(nameof(cutCondition));
            }

            _cutCondition = cutCondition;
            _innerEnumerator = innerEnumerable.GetEnumerator();
        }

        public BatchEnumerable(
            IEnumerable<T> innerEnumerable,
            int batchSize)
        {
            if (innerEnumerable == null)
            {
                throw new ArgumentNullException(nameof(innerEnumerable));
            }

            if (batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            _cutCondition = batch => batch.Count >= batchSize;
            _innerEnumerator = innerEnumerable.GetEnumerator();
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            var batch = new List<T>();
            while (!_cutCondition(batch) && _innerEnumerator.MoveNext())
            {
                batch.Add(_innerEnumerator.Current);
            }

            Current = batch;
            return batch.Count != 0;
        }

        public void Reset()
        {
            _innerEnumerator.Reset();
        }

        public IEnumerable<T> Current { get; private set; }

        object IEnumerator.Current => Current;
    }
}