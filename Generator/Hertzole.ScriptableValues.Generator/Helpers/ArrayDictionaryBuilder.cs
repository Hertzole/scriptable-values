using System.Collections.Generic;
using System.Collections.Immutable;

namespace Hertzole.ScriptableValues.Generator;

internal readonly ref struct ArrayDictionaryBuilder<TKey, TValue> where TKey : notnull
{
    private readonly IEqualityComparer<TKey> comparer;
    private readonly Dictionary<TKey, ArrayBuilder<TValue>> builders;

    public ArrayDictionaryBuilder(IEqualityComparer<TKey> comparer)
    {
        this.comparer = comparer;
        builders = new Dictionary<TKey, ArrayBuilder<TValue>>(comparer);
    }

    public void Add(TKey key, TValue value)
    {
        if (builders.TryGetValue(key, out ArrayBuilder<TValue> array))
        {
            array.Add(value);
        }
        else
        {
            ArrayBuilder<TValue> builder = new ArrayBuilder<TValue>();
            builder.Add(value);
            builders.Add(key, builder);
        }
    }

    public ImmutableDictionary<TKey, ImmutableArray<TValue>> ToImmutable()
    {
        ImmutableDictionary<TKey, ImmutableArray<TValue>>.Builder builder = ImmutableDictionary.CreateBuilder<TKey, ImmutableArray<TValue>>(comparer);

        foreach (KeyValuePair<TKey, ArrayBuilder<TValue>> pair in builders)
        {
            builder.Add(pair.Key, pair.Value.ToImmutable());
        }

        return builder.ToImmutable();
    }
}