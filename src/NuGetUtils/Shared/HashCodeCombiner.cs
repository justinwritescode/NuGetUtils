namespace JustinWritesCode.NuGet.Shared;

/// <summary>
/// Hash code creator, based on the original NuGet hash code combiner/ASP hash code combiner implementations
/// </summary>
internal struct HashCodeCombiner
{
    private const long Seed = 5381L;

    private bool _initialized;

    private long _combinedHash;

    internal int CombinedHash => _combinedHash.GetHashCode();

    private void AddHashCode(int i)
    {
        _combinedHash = ((_combinedHash << 5) + _combinedHash) ^ i;
    }

    internal void AddObject(int i)
    {
        CheckInitialized();
        AddHashCode(i);
    }

    internal void AddObject(bool b)
    {
        CheckInitialized();
        AddHashCode(b ? 1 : 0);
    }

    internal void AddObject<TValue>(TValue o, IEqualityComparer<TValue> comparer) where TValue : class
    {
        CheckInitialized();
        if (o != null)
        {
            AddHashCode(comparer.GetHashCode(o));
        }
    }

    internal void AddObject<T>(T o) where T : class
    {
        CheckInitialized();
        if (o != null)
        {
            AddHashCode(o.GetHashCode());
        }
    }

    internal void AddStruct<T>(T? o) where T : struct
    {
        CheckInitialized();
        if (o.HasValue)
        {
            AddHashCode(o.GetHashCode());
        }
    }

    internal void AddStruct<T>(T o) where T : struct
    {
        CheckInitialized();
        AddHashCode(o.GetHashCode());
    }

    internal void AddStringIgnoreCase(string s)
    {
        CheckInitialized();
        if (s != null)
        {
            AddHashCode(StringComparer.OrdinalIgnoreCase.GetHashCode(s));
        }
    }

    internal void AddSequence<T>(IEnumerable<T> sequence)
    {
        if (sequence == null)
        {
            return;
        }
        CheckInitialized();
        foreach (T item in sequence)
        {
            AddHashCode(item.GetHashCode());
        }
    }

    internal void AddSequence<T>(T[] array)
    {
        if (array != null)
        {
            CheckInitialized();
            for (int i = 0; i < array.Length; i++)
            {
                T val = array[i];
                AddHashCode(val.GetHashCode());
            }
        }
    }

    internal void AddSequence<T>(IList<T> list)
    {
        if (list != null)
        {
            CheckInitialized();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                AddHashCode(list[i].GetHashCode());
            }
        }
    }

    internal void AddSequence<T>(IReadOnlyList<T> list)
    {
        if (list != null)
        {
            CheckInitialized();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                AddHashCode(list[i].GetHashCode());
            }
        }
    }

    internal void AddDictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
    {
        if (dictionary == null)
        {
            return;
        }
        CheckInitialized();
        foreach (KeyValuePair<TKey, TValue> item in dictionary.OrderBy((KeyValuePair<TKey, TValue> x) => x.Key))
        {
            AddHashCode(item.Key.GetHashCode());
            AddHashCode(item.Value.GetHashCode());
        }
    }

    /// <summary>
    /// Create a unique hash code for the given set of items
    /// </summary>
    internal static int GetHashCode<T1, T2>(T1 o1, T2 o2)
    {
        HashCodeCombiner hashCodeCombiner = default(HashCodeCombiner);
        hashCodeCombiner.CheckInitialized();
        hashCodeCombiner.AddHashCode(o1.GetHashCode());
        hashCodeCombiner.AddHashCode(o2.GetHashCode());
        return hashCodeCombiner.CombinedHash;
    }

    /// <summary>
    /// Create a unique hash code for the given set of items
    /// </summary>
    internal static int GetHashCode<T1, T2, T3>(T1 o1, T2 o2, T3 o3)
    {
        HashCodeCombiner hashCodeCombiner = default(HashCodeCombiner);
        hashCodeCombiner.CheckInitialized();
        hashCodeCombiner.AddHashCode(o1.GetHashCode());
        hashCodeCombiner.AddHashCode(o2.GetHashCode());
        hashCodeCombiner.AddHashCode(o3.GetHashCode());
        return hashCodeCombiner.CombinedHash;
    }

    private void CheckInitialized()
    {
        if (!_initialized)
        {
            _combinedHash = 5381L;
            _initialized = true;
        }
    }
}
