using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCode_2021;

public interface IValueCollection<out T>
{
    T this[int index] { get; }
    int Count { get; }
}

[StructLayout(LayoutKind.Sequential)]
public struct ValueArray11<T> : IValueCollection<T>, IEnumerable<T>
{
    public const int Length = 11;

    public T _0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10;

    public readonly int Count => Length;

    // non reference indexer
    public readonly T this[int index] => index switch
    {
        0 => _0,
        1 => _1,
        2 => _2,
        3 => _3,
        4 => _4,
        5 => _5,
        6 => _6,
        7 => _7,
        8 => _8,
        9 => _9,
        10 => _10,
        _ => throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
            $"index {index} is out of range [0..{Length}]")
    };

    public readonly bool Contains(in T elem)
        => ValueCollections.Contains(in this, in elem);

    public readonly bool Contains(in T elem, EqualityComparer<T> eq)
        => ValueCollections.Contains(in this, in elem, eq);

    public readonly bool Contains(in T elem, int start, int count)
        => ValueCollections.Contains(in this, in elem, start, count);

    public readonly bool Contains(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.Contains(in this, in elem, start, count, eq);

    public readonly int IndexOf(in T elem)
        => ValueCollections.IndexOf(in this, in elem);

    public readonly int IndexOf(in T elem, EqualityComparer<T> eq)
        => ValueCollections.IndexOf(in this, in elem, eq);

    public readonly int IndexOf(in T elem, int start, int count)
        => ValueCollections.IndexOf(in this, in elem, start, count);

    public readonly int IndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.IndexOf(in this, in elem, start, count, eq);

    public readonly int LastIndexOf(in T elem)
        => ValueCollections.LastIndexOf(in this, in elem);

    public readonly int LastIndexOf(in T elem, EqualityComparer<T> eq)
        => ValueCollections.LastIndexOf(in this, in elem, eq);

    public readonly int LastIndexOf(in T elem, int start, int count)
        => ValueCollections.LastIndexOf(in this, in elem, start, count);

    public readonly int LastIndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.LastIndexOf(in this, in elem, start, count, eq);

    public readonly int FindIndex(Predicate<T> pred)
        => ValueCollections.FindIndex(in this, pred);

    public readonly int FindIndex(Predicate<T> pred, int start, int count)
        => ValueCollections.FindIndex(in this, pred, start, count);

    public readonly int FindLastIndex(Predicate<T> pred)
        => ValueCollections.FindLastIndex(in this, pred);

    public readonly int FindLastIndex(Predicate<T> pred, int start, int count)
        => ValueCollections.FindLastIndex(in this, pred, start, count);

    public readonly T[] ToArray()
    {
        var array = new T[Length];
        for (int i = 0; i < Length; ++i)
            array[i] = this[i];
        return array;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(_0);hash.Add(_1);hash.Add(_2);hash.Add(_4);hash.Add(_5);hash.Add(_6);
        hash.Add(_7);hash.Add(_8);hash.Add(_9);hash.Add(_10);
        return hash.ToHashCode();
    }

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);

    readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => new Enumerator(this);

    public readonly Enumerator GetEnumerator() // duck type the efficient struct-Enumerator
        => new Enumerator(this);

    [Serializable]
    public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
    {
        private ValueArray11<T> _array;
        private int _index;

        internal Enumerator(ValueArray11<T> array)
        {
            _array = array;
            _index = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_index + 1 == ValueArray11<T>.Length)
                return false;
            ++_index;
            return true;
        }

        public T Current
            => _array[_index];

        object System.Collections.IEnumerator.Current
            => Current;

        void System.Collections.IEnumerator.Reset()
        {
            _index = -1;
        }
    }
}
[StructLayout(LayoutKind.Sequential)]
public struct ValueArray7<T> : IValueCollection<T>, IEnumerable<T>
{
    public const int Length = 11;

    public T _0, _1, _2, _3, _4, _5, _6;

    public readonly int Count => Length;

    // non reference indexer
    public readonly T this[int index] => index switch
    {
        0 => _0,
        1 => _1,
        2 => _2,
        3 => _3,
        4 => _4,
        5 => _5,
        6 => _6,
        _ => throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
            $"index {index} is out of range [0..{Length}]")
    };

    public readonly bool Contains(in T elem)
        => ValueCollections.Contains(in this, in elem);

    public readonly bool Contains(in T elem, EqualityComparer<T> eq)
        => ValueCollections.Contains(in this, in elem, eq);

    public readonly bool Contains(in T elem, int start, int count)
        => ValueCollections.Contains(in this, in elem, start, count);

    public readonly bool Contains(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.Contains(in this, in elem, start, count, eq);

    public readonly int IndexOf(in T elem)
        => ValueCollections.IndexOf(in this, in elem);

    public readonly int IndexOf(in T elem, EqualityComparer<T> eq)
        => ValueCollections.IndexOf(in this, in elem, eq);

    public readonly int IndexOf(in T elem, int start, int count)
        => ValueCollections.IndexOf(in this, in elem, start, count);

    public readonly int IndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.IndexOf(in this, in elem, start, count, eq);

    public readonly int LastIndexOf(in T elem)
        => ValueCollections.LastIndexOf(in this, in elem);

    public readonly int LastIndexOf(in T elem, EqualityComparer<T> eq)
        => ValueCollections.LastIndexOf(in this, in elem, eq);

    public readonly int LastIndexOf(in T elem, int start, int count)
        => ValueCollections.LastIndexOf(in this, in elem, start, count);

    public readonly int LastIndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.LastIndexOf(in this, in elem, start, count, eq);

    public readonly int FindIndex(Predicate<T> pred)
        => ValueCollections.FindIndex(in this, pred);

    public readonly int FindIndex(Predicate<T> pred, int start, int count)
        => ValueCollections.FindIndex(in this, pred, start, count);

    public readonly int FindLastIndex(Predicate<T> pred)
        => ValueCollections.FindLastIndex(in this, pred);

    public readonly int FindLastIndex(Predicate<T> pred, int start, int count)
        => ValueCollections.FindLastIndex(in this, pred, start, count);

    public readonly T[] ToArray()
    {
        var array = new T[Length];
        for (int i = 0; i < Length; ++i)
            array[i] = this[i];
        return array;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(_0);hash.Add(_1);hash.Add(_2);hash.Add(_4);hash.Add(_5);hash.Add(_6);
        return hash.ToHashCode();
    }

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);

    readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => new Enumerator(this);

    public readonly Enumerator GetEnumerator() // duck type the efficient struct-Enumerator
        => new Enumerator(this);

    [Serializable]
    public struct Enumerator : IEnumerator<T>
    {
        private ValueArray7<T> _array;
        private int _index;

        internal Enumerator(ValueArray7<T> array)
        {
            _array = array;
            _index = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_index + 1 == ValueArray11<T>.Length)
                return false;
            ++_index;
            return true;
        }

        public T Current
            => _array[_index];

        object System.Collections.IEnumerator.Current
            => Current;

        void System.Collections.IEnumerator.Reset()
        {
            _index = -1;
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct ValueArray4<T> : IValueCollection<T>, IEnumerable<T>
{
    public const int Length = 4;

    public T _0, _1, _2, _3;

    public readonly int Count => Length;

    // non reference indexer
    public readonly T this[int index] => index switch
    {
        0 => _0,
        1 => _1,
        2 => _2,
        3 => _3,
        _ => throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
            $"index {index} is out of range [0..{Length}]")
    };

    public readonly bool Contains(in T elem)
        => ValueCollections.Contains(in this, in elem);

    public readonly bool Contains(in T elem, EqualityComparer<T> eq)
        => ValueCollections.Contains(in this, in elem, eq);

    public readonly bool Contains(in T elem, int start, int count)
        => ValueCollections.Contains(in this, in elem, start, count);

    public readonly bool Contains(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.Contains(in this, in elem, start, count, eq);

    public readonly int IndexOf(in T elem)
        => ValueCollections.IndexOf(in this, in elem);

    public readonly int IndexOf(in T elem, EqualityComparer<T> eq)
        => ValueCollections.IndexOf(in this, in elem, eq);

    public readonly int IndexOf(in T elem, int start, int count)
        => ValueCollections.IndexOf(in this, in elem, start, count);

    public readonly int IndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.IndexOf(in this, in elem, start, count, eq);

    public readonly int LastIndexOf(in T elem)
        => ValueCollections.LastIndexOf(in this, in elem);

    public readonly int LastIndexOf(in T elem, EqualityComparer<T> eq)
        => ValueCollections.LastIndexOf(in this, in elem, eq);

    public readonly int LastIndexOf(in T elem, int start, int count)
        => ValueCollections.LastIndexOf(in this, in elem, start, count);

    public readonly int LastIndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
        => ValueCollections.LastIndexOf(in this, in elem, start, count, eq);

    public readonly int FindIndex(Predicate<T> pred)
        => ValueCollections.FindIndex(in this, pred);

    public readonly int FindIndex(Predicate<T> pred, int start, int count)
        => ValueCollections.FindIndex(in this, pred, start, count);

    public readonly int FindLastIndex(Predicate<T> pred)
        => ValueCollections.FindLastIndex(in this, pred);

    public readonly int FindLastIndex(Predicate<T> pred, int start, int count)
        => ValueCollections.FindLastIndex(in this, pred, start, count);

    public readonly T[] ToArray()
    {
        var array = new T[Length];
        for (int i = 0; i < Length; ++i)
            array[i] = this[i];
        return array;
    }

    public override int GetHashCode() { return HashCode.Combine(_0, _1, _2, _3); }

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new Enumerator(this);

    readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => new Enumerator(this);

    public readonly Enumerator GetEnumerator() // duck type the efficient struct-Enumerator
        => new Enumerator(this);

    [Serializable]
    public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
    {
        private ValueArray4<T> _array;
        private int _index;

        internal Enumerator(ValueArray4<T> array)
        {
            _array = array;
            _index = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_index + 1 == ValueArray4<T>.Length)
                return false;
            ++_index;
            return true;
        }

        public T Current
            => _array[_index];

        object System.Collections.IEnumerator.Current
            => Current;

        void System.Collections.IEnumerator.Reset()
        {
            _index = -1;
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct ValueList4<T> : IValueCollection<T>, IEnumerable<T>
{
    internal int _count;
    internal ValueArray4<T> _array;

    public readonly int Capacity => ValueArray4<T>.Length;
    public readonly int Count => _count;
    public readonly bool IsEmpty => _count == 0;
    public readonly bool IsFull => _count == ValueArray4<T>.Length;
    public readonly int RemainingCapacity => ValueArray4<T>.Length - _count;

    public readonly T this[int index] => _array[index];

    public readonly T First => _array[0];
    public readonly T Last => _array[_count - 1];
    public readonly T Peek => _array[_count - 1];

    public readonly bool Contains(in T elem)
    => _array.Contains(in elem, 0, _count);
    public readonly bool Contains(in T elem, EqualityComparer<T> eq)
    => _array.Contains(in elem, 0, _count, eq);

    public readonly int IndexOf(in T elem)
    => _array.IndexOf(in elem, 0, _count);
    public readonly int IndexOf(in T elem, EqualityComparer<T> eq)
    => _array.IndexOf(in elem, 0, _count, eq);
    public readonly int IndexOf(in T elem, int start, int count)
    => _array.IndexOf(in elem, start, count);
    public readonly int IndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
    => _array.IndexOf(in elem, start, count, eq);

    public readonly int LastIndexOf(in T elem)
    => _array.LastIndexOf(in elem, 0, _count);
    public readonly int LastIndexOf(in T elem, EqualityComparer<T> eq)
    => _array.LastIndexOf(in elem, 0, _count, eq);
    public readonly int LastIndexOf(in T elem, int start, int count)
    => _array.LastIndexOf(in elem, start, count);
    public readonly int LastIndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
    => _array.LastIndexOf(in elem, start, count, eq);

    public readonly int FindIndex(Predicate<T> pred)
    => _array.FindIndex(pred, 0, _count);
    public readonly int FindIndex(Predicate<T> pred, int start, int count)
    => _array.FindIndex(pred, start, count);

    public readonly int FindLastIndex(Predicate<T> pred)
    => _array.FindLastIndex(pred, 0, _count);
    public readonly int FindLastIndex(Predicate<T> pred, int start, int count)
    => _array.FindLastIndex(pred, start, count);

    public readonly bool Exists(Predicate<T> pred)
    => _array.FindIndex(pred, 0, _count) != -1;

    public readonly bool Find(Predicate<T> pred, out T result)
    {
        var index = _array.FindIndex(pred, 0, _count);
        if (index != -1)
        {
            result = _array[index];
            return true;
        }
        result = default;
        return false;
    }
    public readonly bool FindLast(Predicate<T> pred, out T result)
    {
        var index = _array.FindLastIndex(pred, 0, _count);
        if (index != -1)
        {
            result = _array[index];
            return true;
        }
        result = default;
        return false;
    }

    public readonly ValueList4<T> FindAll(Predicate<T> pred)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");

        ValueList4<T> list = new ValueList4<T>();
        for (int i = 0; i < _count; ++i)
        {
            var item = this[i];
            if (pred(item))
                list.Add(item);
        }

        return list;
    }
    public readonly ValueList4<T> GetRange(int start, int count)
    {
        if (start < 0 || start >= _count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{_count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > _count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} + {nameof(count)} is out of range [0..{_count.ToString()}]");

        ValueList4<T> list = new ValueList4<T>();
        for (int i = start; i < end; ++i)
            list.Add(_array[i]);

        return list;
    }

    public readonly T[] ToArray()
    {
        var array = new T[_count];
        for (int i = 0; i < _count; ++i)
            array[i] = _array[i];
        return array;
    }

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
    => new Enumerator(this);

    readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    => new Enumerator(this);

    public readonly Enumerator GetEnumerator() // duck type the efficient struct-Enumerator
    => new Enumerator(this);

    [Serializable]
    public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
    {
        private ValueList4<T> _list;
        private int _index;

        internal Enumerator(ValueList4<T> list)
        {
            _list = list;
            _index = -1;
        }

        public void Dispose()
        { }

        public bool MoveNext()
        {
            if (_index + 1 == _list.Count)
                return false;
            ++_index;
            return true;
        }

        public T Current
        => _list[_index];

        object System.Collections.IEnumerator.Current
        => Current;

        void System.Collections.IEnumerator.Reset()
        {
            _index = -1;
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct ValueDeque4<T> : IValueCollection<T>, IEnumerable<T>
{
    internal int _begin, _end, _count;

    internal ValueArray4<T> _array;

    public readonly int Capacity => ValueArray4<T>.Length;
    public readonly int Count => _count;
    public readonly bool IsEmpty => _count == 0;
    public readonly bool IsFull => _count == ValueArray4<T>.Length;
    public readonly int RemainingCapacity => ValueArray4<T>.Length - _count;

    public readonly T this[int index] => _array[ToSlot(index)];

    public readonly T First => _array[_begin];
    public readonly T Last => _array[Prev(_end)];
    public readonly T Peek => _array[Prev(_end)];

    public readonly bool Contains(in T elem)
    => Contains(in elem, EqualityComparer<T>.Default);
    public readonly bool Contains(in T elem, EqualityComparer<T> eq)
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        for (var i = _begin; i != _end; i = Next(i))
        {
            if (eq.Equals(_array[i], elem))
                return true;
        }
        return false;
    }
    public readonly bool Contains(in T elem, int start, int count)
    => Contains(in elem, start, count, EqualityComparer<T>.Default);
    public readonly bool Contains(in T elem, int start, int count, EqualityComparer<T> eq)
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        if (start < 0 || start >= _count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{_count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > _count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} + {nameof(count)} is out of range [0..{_count.ToString()}]");
        for (int i = start; i < end; ++i)
        {
            if (eq.Equals(_array[ToSlot(i)], elem))
                return true;
        }
        return false;
    }

    public readonly int IndexOf(in T elem)
    => IndexOf(in elem, EqualityComparer<T>.Default);
    public readonly int IndexOf(in T elem, EqualityComparer<T> eq)
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        for (var i = 0; i < _count; ++i)
        {
            if (eq.Equals(_array[ToSlot(i)], elem))
                return i;
        }
        return -1;
    }
    public readonly int IndexOf(in T elem, int start, int count)
    => IndexOf(in elem, start, count, EqualityComparer<T>.Default);
    public readonly int IndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        if (start < 0 || start >= _count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{_count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > _count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} + {nameof(count)} is out of range [0..{_count.ToString()}]");
        for (var i = start; i < end; ++i)
        {
            if (eq.Equals(_array[ToSlot(i)], elem))
                return i;
        }
        return -1;
    }
    public readonly int LastIndexOf(in T elem)
    => LastIndexOf(in elem, EqualityComparer<T>.Default);
    public readonly int LastIndexOf(in T elem, EqualityComparer<T> eq)
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        for (var i = _count - 1; i >= 0; --i)
        {
            if (eq.Equals(_array[ToSlot(i)], elem))
                return i;
        }
        return -1;
    }
    public readonly int LastIndexOf(in T elem, int start, int count)
    => LastIndexOf(in elem, start, count, EqualityComparer<T>.Default);
    public readonly int LastIndexOf(in T elem, int start, int count, EqualityComparer<T> eq)
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        if (start < 0 || start >= _count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{_count.ToString()}]");
        int end = start - count + 1;
        if (end < 0 || end >= _count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} - {nameof(count)} + 1 is out of range [0..{_count.ToString()}]");

        for (var i = start; i >= end; --i)
        {
            if (eq.Equals(_array[ToSlot(i)], elem))
                return i;
        }
        return -1;
    }


    public readonly int FindIndex(Predicate<T> pred)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");
        for (var i = 0; i < _count; ++i)
        {
            if (pred(_array[ToSlot(i)]))
                return i;
        }
        return -1;
    }
    public readonly int FindIndex(Predicate<T> pred, int start, int count)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");
        if (start < 0 || start >= _count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{_count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > _count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} + {nameof(count)} is out of range [0..{_count.ToString()}]");

        for (var i = start; i < end; ++i)
        {
            if (pred(_array[ToSlot(i)]))
                return i;
        }
        return -1;
    }
    public readonly int FindLastIndex(Predicate<T> pred)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");

        for (var i = _count - 1; i >= 0; --i)
        {
            if (pred(_array[ToSlot(i)]))
                return i;
        }
        return -1;
    }
    public readonly int FindLastIndex(Predicate<T> pred, int start, int count)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");
        if (start < 0 || start >= _count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{_count.ToString()}]");
        int end = start - count + 1;
        if (end < 0 || end >= _count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} - {nameof(count)} + 1 is out of range [0..{_count.ToString()}]");

        for (var i = start; i >= end; --i)
        {
            if (pred(_array[ToSlot(i)]))
                return i;
        }
        return -1;
    }

    public readonly bool Exists(Predicate<T> pred)
    => FindIndex(pred) != -1;

    public readonly bool Find(Predicate<T> pred, out T result)
    {
        var index = FindIndex(pred);
        if (index != -1)
        {
            result = _array[index];
            return true;
        }
        result = default;
        return false;
    }
    public readonly bool FindLast(Predicate<T> pred, out T result)
    {
        var index = FindLastIndex(pred);
        if (index != -1)
        {
            result = _array[index];
            return true;
        }
        result = default;
        return false;
    }

    public readonly ValueDeque4<T> FindAll(Predicate<T> pred)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");

        ValueDeque4<T> list = new ValueDeque4<T>();
        for (int i = 0; i < _count; ++i)
        {
            var item = this[i];
            if (pred(item))
                list.Enqueue(item);
        }

        return list;
    }

    public readonly ValueDeque4<T> GetRange(int start, int count)
    {
        if (start < 0 || start >= _count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{_count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > _count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} + {nameof(count)} is out of range [0..{_count.ToString()}]");

        ValueDeque4<T> list = new ValueDeque4<T>();
        for (int i = start; i < end; ++i)
            list.Enqueue(_array[i]);

        return list;
    }

    public readonly T[] ToArray()
    {
        var array = new T[_count];
        for (int i = 0; i < _count; ++i)
            array[i] = this[i];
        return array;
    }

    public override int GetHashCode() { return HashCode.Combine(_array, _count); }

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
    => new Enumerator(this);

    readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    => new Enumerator(this);

    public readonly Enumerator GetEnumerator() // duck type the efficient struct-Enumerator
    => new Enumerator(this);

    [Serializable]
    public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
    {
        private ValueDeque4<T> _list;
        private int _index;

        internal Enumerator(ValueDeque4<T> list)
        {
            _list = list;
            _index = -1;
        }

        public void Dispose()
        { }

        public bool MoveNext()
        {
            if (_index + 1 == _list.Count)
                return false;
            ++_index;
            return true;
        }

        public T Current
        => _list[_index];

        object System.Collections.IEnumerator.Current
        => Current;

        void System.Collections.IEnumerator.Reset()
        {
            _index = -1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly int ToSlot(int i) => (_begin + i) % ValueArray4<T>.Length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly int ToPos(int p) => (p - _begin + ValueArray4<T>.Length) % ValueArray4<T>.Length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly int Next(int i) => (i + 1) % ValueArray4<T>.Length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly int Prev(int i) => (i - 1 + ValueArray4<T>.Length) % ValueArray4<T>.Length;
}

public static class ValueCollections
{
    public static bool Contains<TCollection, TElement>(in TCollection collection, in TElement elem)
        where TCollection : struct, IValueCollection<TElement>
        => Contains(in collection, in elem, EqualityComparer<TElement>.Default);

    public static bool Contains<TCollection, TElement>(in TCollection collection, in TElement elem,
        EqualityComparer<TElement> eq) where TCollection : struct, IValueCollection<TElement>
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        var count = collection.Count;
        for (int i = 0; i < count; ++i)
        {
            if (eq.Equals(elem, collection[i]))
                return true;
        }

        return false;
    }

    public static bool Contains<TCollection, TElement>(in TCollection collection, in TElement elem, int start,
        int count) where TCollection : struct, IValueCollection<TElement>
        => Contains(in collection, in elem, start, count, EqualityComparer<TElement>.Default);

    public static bool Contains<TCollection, TElement>(in TCollection collection, in TElement elem, int start,
        int count, EqualityComparer<TElement> eq) where TCollection : struct, IValueCollection<TElement>
    {
        if (start < 0 || start >= collection.Count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(),
                $"{nameof(start)} is out of range [0..{collection.Count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > collection.Count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(),
                $"{nameof(start)} + {nameof(count)} is out of range [0..{collection.Count.ToString()}]");
        for (int i = start; i < end; ++i)
        {
            if (eq.Equals(elem, collection[i]))
                return true;
        }

        return false;
    }

    public static int IndexOf<TCollection, TElement>(in TCollection collection, in TElement elem)
        where TCollection : struct, IValueCollection<TElement>
        => IndexOf(in collection, in elem, EqualityComparer<TElement>.Default);

    public static int IndexOf<TCollection, TElement>(in TCollection collection, in TElement elem,
        EqualityComparer<TElement> eq) where TCollection : struct, IValueCollection<TElement>
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        var count = collection.Count;
        for (int i = 0; i < count; ++i)
        {
            if (eq.Equals(elem, collection[i]))
                return i;
        }

        return -1;
    }

    public static int IndexOf<TCollection, TElement>(in TCollection collection, in TElement elem, int start,
        int count) where TCollection : struct, IValueCollection<TElement>
        => IndexOf(in collection, in elem, start, count, EqualityComparer<TElement>.Default);

    public static int IndexOf<TCollection, TElement>(in TCollection collection, in TElement elem, int start,
        int count, EqualityComparer<TElement> eq) where TCollection : struct, IValueCollection<TElement>
    {
        if (start < 0 || start >= collection.Count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(),
                $"{nameof(start)} is out of range [0..{collection.Count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > collection.Count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(),
                $"{nameof(start)} + {nameof(count)} is out of range [0..{collection.Count.ToString()}]");
        for (int i = start; i < end; ++i)
        {
            if (eq.Equals(elem, collection[i]))
                return i;
        }

        return -1;
    }

    public static int LastIndexOf<TCollection, TElement>(in TCollection collection, in TElement elem)
        where TCollection : struct, IValueCollection<TElement>
        => LastIndexOf(in collection, in elem, EqualityComparer<TElement>.Default);

    public static int LastIndexOf<TCollection, TElement>(in TCollection collection, in TElement elem,
        EqualityComparer<TElement> eq) where TCollection : struct, IValueCollection<TElement>
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        var count = collection.Count;
        for (int i = count - 1; i >= 0; ++i)
        {
            if (eq.Equals(elem, collection[i]))
                return i;
        }

        return -1;
    }

    public static int LastIndexOf<TCollection, TElement>(in TCollection collection, in TElement elem, int start,
        int count) where TCollection : struct, IValueCollection<TElement>
        => LastIndexOf(in collection, in elem, start, count, EqualityComparer<TElement>.Default);

    public static int LastIndexOf<TCollection, TElement>(in TCollection collection, in TElement elem, int start,
        int count, EqualityComparer<TElement> eq) where TCollection : struct, IValueCollection<TElement>
    {
        if (eq == null)
            throw new ArgumentNullException(nameof(eq), "Equality comparer argument can't be null");
        if (start < 0 || start >= collection.Count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(),
                $"{nameof(start)} is out of range [0..{collection.Count.ToString()}]");
        int end = start - count + 1;
        if (end < 0 || end >= collection.Count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(),
                $"{nameof(start)} - {nameof(count)} + 1 is out of range [0..{collection.Count.ToString()}]");
        for (int i = start; i >= end; --i)
        {
            if (eq.Equals(elem, collection[i]))
                return i;
        }

        return -1;
    }

    public static int FindIndex<TCollection, TElement>(in TCollection collection, Predicate<TElement> pred)
        where TCollection : struct, IValueCollection<TElement>
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");
        var count = collection.Count;
        for (int i = 0; i < count; ++i)
        {
            if (pred(collection[i]))
                return i;
        }

        return -1;
    }

    public static int FindIndex<TCollection, TElement>(in TCollection collection, Predicate<TElement> pred,
        int start, int count) where TCollection : struct, IValueCollection<TElement>
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");
        if (start < 0 || start >= collection.Count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(),
                $"{nameof(start)} is out of range [0..{collection.Count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > collection.Count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(),
                $"{nameof(start)} + {nameof(count)} is out of range [0..{collection.Count.ToString()}]");
        for (int i = start; i < end; ++i)
        {
            if (pred(collection[i]))
                return i;
        }

        return -1;
    }

    public static int FindLastIndex<TCollection, TElement>(in TCollection collection, Predicate<TElement> pred)
        where TCollection : struct, IValueCollection<TElement>
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");
        var count = collection.Count;
        for (int i = count - 1; i >= 0; --i)
        {
            if (pred(collection[i]))
                return i;
        }

        return -1;
    }

    public static int FindLastIndex<TCollection, TElement>(in TCollection collection, Predicate<TElement> pred,
        int start, int count) where TCollection : struct, IValueCollection<TElement>
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");
        if (start < 0 || start >= collection.Count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(),
                $"{nameof(start)} is out of range [0..{collection.Count.ToString()}]");
        int end = start - count + 1;
        if (end < 0 || end >= collection.Count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(),
                $"{nameof(start)} - {nameof(count)} + 1 is out of range [0..{collection.Count.ToString()}]");
        for (int i = start; i >= end; --i)
        {
            if (pred(collection[i]))
                return i;
        }

        return -1;
    }
}

public static class ValueArrayExtensions
{
    // Extensions for ValueArray4<T>:
    // ---------------------------------------

    // readonly ref to array index
    public static ref readonly T ReadRef<T>(in this ValueArray4<T> array, int index)
    {
        switch (index)
        {
            case 0: return ref array._0;
            case 1: return ref array._1;
            case 2: return ref array._2;
            case 3: return ref array._3;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                    $"index {index} is out of range [0..{array.Count}]");
        }
    }

    // writable ref to array index (there can't be a ref returning mathod inside the struct, and a writable indexer would cause defeinsive copies, so use "Set" or "Ref" insted)
    public static ref T Ref<T>(ref this ValueArray4<T> array, int index)
    {
        switch (index)
        {
            case 0: return ref array._0;
            case 1: return ref array._1;
            case 2: return ref array._2;
            case 3: return ref array._3;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                    $"index {index} is out of range [0..{array.Count}]");
        }
    }

    // Setter to array indices (there can't be a ref returning mathod inside the struct, and a writable indexer would cause defeinsive copies, so use "Set" or "Ref" insted)
    public static void Set<T>(ref this ValueArray4<T> array, int index, in T element)
        => Ref(ref array, index) = element;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // Swapping two array elements
    public static void Swap<T>(ref this ValueArray4<T> array, int index1, int index2)
    {
        var temp = array[index1];
        array.Ref(index1) = array[index2];
        array.Ref(index2) = temp;
    }

    // Extensions for ValueArray11<T>:
    // ---------------------------------------

    // readonly ref to array index
    public static ref readonly T ReadRef<T>(in this ValueArray11<T> array, int index)
    {
        switch (index)
        {
            case 0: return ref array._0;
            case 1: return ref array._1;
            case 2: return ref array._2;
            case 3: return ref array._3;
            case 4: return ref array._4;
            case 5: return ref array._5;
            case 6: return ref array._6;
            case 7: return ref array._7;
            case 8: return ref array._8;
            case 9: return ref array._9;
            case 10: return ref array._10;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                    $"index {index} is out of range [0..{array.Count}]");
        }
    }

    // writable ref to array index (there can't be a ref returning mathod inside the struct, and a writable indexer would cause defeinsive copies, so use "Set" or "Ref" insted)
    public static ref T Ref<T>(ref this ValueArray11<T> array, int index)
    {
        switch (index)
        {
            case 0: return ref array._0;
            case 1: return ref array._1;
            case 2: return ref array._2;
            case 3: return ref array._3;
            case 4: return ref array._4;
            case 5: return ref array._5;
            case 6: return ref array._6;
            case 7: return ref array._7;
            case 8: return ref array._8;
            case 9: return ref array._9;
            case 10: return ref array._10;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                    $"index {index} is out of range [0..{array.Count}]");
        }
    }

    // Setter to array indices (there can't be a ref returning mathod inside the struct, and a writable indexer would cause defeinsive copies, so use "Set" or "Ref" insted)
    public static void Set<T>(ref this ValueArray11<T> array, int index, in T element)
        => Ref(ref array, index) = element;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // Swapping two array elements
    public static void Swap<T>(ref this ValueArray11<T> array, int index1, int index2)
    {
        var temp = array[index1];
        array.Ref(index1) = array[index2];
        array.Ref(index2) = temp;
    }

    // Setter to array indices (there can't be a ref returning mathod inside the struct, and a writable indexer would cause defeinsive copies, so use "Set" or "Ref" insted)
    public static void Set<T>(ref this ValueArray7<T> array, int index, in T element)
        => Ref(ref array, index) = element;
    // writable ref to array index (there can't be a ref returning mathod inside the struct, and a writable indexer would cause defeinsive copies, so use "Set" or "Ref" insted)
    public static ref T Ref<T>(ref this ValueArray7<T> array, int index)
    {
        switch (index)
        {
            case 0: return ref array._0;
            case 1: return ref array._1;
            case 2: return ref array._2;
            case 3: return ref array._3;
            case 4: return ref array._4;
            case 5: return ref array._5;
            case 6: return ref array._6;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                    $"index {index} is out of range [0..{array.Count}]");
        }
    }

}

public static class ValueListExtensions
{

    // Extensions for ValueList4<T>:
    // ---------------------------------------
    public static ref T Ref<T>(ref this ValueList4<T> list, int index) => ref list._array.Ref(index);
    public static ref readonly T ReadRef<T>(ref this ValueList4<T> list, int index) => ref list._array.ReadRef(index);

    public static void Clear<T>(ref this ValueList4<T> list)
    {
        list._count = 0;
    }

    public static void Add<T>(ref this ValueList4<T> list, in T item)
    {
        if (list._count == list.Capacity)
            throw new InvalidOperationException($"Can not store more than {list.Capacity} in ValueList4<{typeof(T).Name}>");
        list._array.Ref(list._count++) = item;
    }

    public static void Push<T>(ref this ValueList4<T> list, in T item)
    => list.Add(in item);

    public static void AddRange<T>(ref this ValueList4<T> list, IEnumerable<T> collection)
    {
        foreach (var item in collection)
            list.Add(in item);
    }

    public static void AddAll<T>(ref this ValueList4<T> list, ReadOnlySpan<T> span)
    {
        foreach (var item in span)
            list.Add(in item);
    }

    public static void Insert<T>(ref this ValueList4<T> list, int index, in T element)
    {
        if (list._count == list.Capacity)
            throw new InvalidOperationException($"Can not store more than {list.Capacity} in ValueList4<{typeof(T).Name}>");
        if (index < 0 || index > list._count)
            throw new ArgumentOutOfRangeException(nameof(index), index.ToString(), $"{nameof(index)} is out of range [0..{list._count.ToString()}]");

        for (int i = list._count; i > index; --i)
        {
            list._array.Ref(i) = list._array[i - 1];
        }
        list._array.Ref(index) = element;
        list._count++;
    }

    public static void RemoveAt<T>(ref this ValueList4<T> list, int index)
    {
        if (index < 0 || index >= list._count)
            throw new ArgumentOutOfRangeException(nameof(index), index.ToString(), $"{nameof(index)} is out of range [0..{list._count.ToString()}]");
        --list._count;
        for (int i = index; i < list._count; ++i)
        {
            list._array.Ref(i) = list._array[i + 1];
        }
    }

    public static void RemoveAtBySwap<T>(ref this ValueList4<T> list, int index)
    {
        if (index < 0 || index >= list._count)
            throw new ArgumentOutOfRangeException(nameof(index), index.ToString(), $"{nameof(index)} is out of range [0..{list._count.ToString()}]");
        list._array.Ref(index) = list._array[--list._count];
    }

    public static T Pop<T>(ref this ValueList4<T> list)
    {
        if (list._count == 0)
            throw new InvalidOperationException("Trying to remove from empty list");
        return list._array[--list._count];
    }

    public static bool Remove<T>(ref this ValueList4<T> list, in T element)
    {
        int index = list.IndexOf(in element);
        if (index != -1)
        {
            list.RemoveAt(index);
            return true;
        }
        return false;
    }

    public static bool RemoveAll<T>(ref this ValueList4<T> list, Predicate<T> pred)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");

        int removeCount = 0;
        for (int i = 0; i < list._count; ++i)
        {
            if (removeCount != 0)
                list._array.Ref(i - removeCount) = list._array[i];

            if (pred(list._array[i]))
                ++removeCount;
        }
        list._count -= removeCount;
        return true;
    }

    public static bool RemoveRange<T>(ref this ValueList4<T> list, int start, int count)
    {
        if (start < 0 || start >= list._count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{list._count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > list._count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} + {nameof(count)} is out of range [0..{list._count.ToString()}]");

        for (int i = end; i < list._count; ++i)
        {
            list._array.Ref(i - count) = list._array[i];
        }
        list._count -= count;
        return true;
    }

    public static void Swap<T>(ref this ValueList4<T> list, int index1, int index2)
    => list._array.Swap(index1, index2);

    public static bool Reverse<T>(ref this ValueList4<T> list)
    {
        int m = list._count / 2;
        for (int i = 0; i < m; ++i)
        {
            int j = list._count - 1 - i;
            list._array.Swap(i, j);
        }
        return true;
    }

    public static bool Reverse<T>(ref this ValueList4<T> list, int start, int count)
    {
        if (start < 0 || start >= list._count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(), $"{nameof(start)} is out of range [0..{list._count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > list._count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(), $"{nameof(start)} + {nameof(count)} is out of range [0..{list._count.ToString()}]");

        int m = start + count / 2;
        for (int i = start; i < m; ++i)
        {
            int j = start + count - 1 - i;
            list._array.Swap(i, j);
        }
        return true;
    }
}

public static class ValueDequeExtensions
{

    // Extensions for ValueDeque4<T>:
    // ---------------------------------------

    public static void Clear<T>(ref this ValueDeque4<T> deque)
    {
        deque._count = deque._begin = deque._end = 0;
    }

    public static ref T Ref<T>(ref this ValueDeque4<T> deque, int index)
        => ref deque._array.Ref(deque.ToSlot(index));

    public static ref readonly T ReadRef<T>(ref this ValueDeque4<T> deque, int index)
        => ref deque._array.ReadRef(deque.ToSlot(index));

    public static ref T FirstRef<T>(ref this ValueDeque4<T> deque)
        => ref deque._array.Ref(deque._begin);

    public static ref T LastRef<T>(ref this ValueDeque4<T> deque)
        => ref deque._array.Ref(deque.Prev(deque._end));

    public static void AddLast<T>(ref this ValueDeque4<T> deque, in T elem)
    {
        if (deque._count == deque.Capacity)
            throw new InvalidOperationException(
                $"Can not store more than {deque.Capacity} in ValueDeque4<{typeof(T).Name}>");
        deque._array.Ref(deque._end) = elem;
        deque._end = deque.Next(deque._end);
        ++deque._count;
    }

    public static void DropLast<T>(ref this ValueDeque4<T> deque)
    {
        if (deque._count == 0)
            throw new InvalidOperationException($"Can not remove items from an empty ValueDeque4<{typeof(T).Name}>");
        deque._end = deque.Prev(deque._end);
        --deque._count;
    }

    public static T RemoveLast<T>(ref this ValueDeque4<T> deque)
    {
        deque.DropLast();
        return deque._array[deque._end];
    }

    public static void AddFirst<T>(ref this ValueDeque4<T> deque, in T elem)
    {
        if (deque._count == deque.Capacity)
            throw new InvalidOperationException(
                $"Can not store more than {deque.Capacity} in ValueDeque4<{typeof(T).Name}>");
        deque._begin = deque.Prev(deque._begin);
        deque._array.Ref(deque._begin) = elem;
        ++deque._count;
    }

    public static void DropFirst<T>(ref this ValueDeque4<T> deque)
    {
        if (deque._count == 0)
            throw new InvalidOperationException($"Can not remove items from an empty ValueDeque4<{typeof(T).Name}>");
        deque._begin = deque.Next(deque._begin);
        --deque._count;
    }

    public static T RemoveFirst<T>(ref this ValueDeque4<T> deque)
    {
        if (deque._count == 0)
            throw new InvalidOperationException($"Can not remove items from an empty ValueDeque4<{typeof(T).Name}>");
        var elem = deque._array[deque._begin];
        deque._begin = deque.Next(deque._begin);
        --deque._count;
        return elem;
    }

    public static void Enqueue<T>(ref this ValueDeque4<T> deque, in T item)
        => deque.AddLast(in item);

    public static T Dequeue<T>(ref this ValueDeque4<T> deque)
        => deque.RemoveFirst();

    public static void Push<T>(ref this ValueDeque4<T> deque, in T item)
        => deque.AddLast(in item);

    public static T Pop<T>(ref this ValueDeque4<T> deque)
        => deque.RemoveLast();

    public static void EnqueueRange<T>(ref this ValueDeque4<T> deque, IEnumerable<T> collection)
    {
        foreach (var item in collection)
            deque.Enqueue(in item);
    }

    public static void EnqueueAll<T>(ref this ValueDeque4<T> deque, ReadOnlySpan<T> span)
    {
        foreach (var item in span)
            deque.Enqueue(in item);
    }

    public static void Insert<T>(ref this ValueDeque4<T> deque, int index, in T element)
    {
        if (deque._count == deque.Capacity)
            throw new InvalidOperationException(
                $"Can not store more than {deque.Capacity} in ValueDeque4<{typeof(T).Name}>");
        if (index < 0 || index > deque._count)
            throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                $"{nameof(index)} is out of range [0..{deque._count.ToString()}]");

        for (int i = deque._count; i > index; --i)
        {
            deque.Ref(i) = deque[i - 1];
        }

        deque.Ref(index) = element;
        deque._count++;
    }

    public static void RemoveAt<T>(ref this ValueDeque4<T> deque, int index)
    {
        if (index < 0 || index >= deque._count)
            throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                $"{nameof(index)} is out of range [0..{deque._count.ToString()}]");
        --deque._count;
        for (int i = index; i < deque._count; ++i)
        {
            deque.Ref(i) = deque[i + 1];
        }
    }

    public static void RemoveAtBySwap<T>(ref this ValueDeque4<T> deque, int index)
    {
        if (index < 0 || index >= deque._count)
            throw new ArgumentOutOfRangeException(nameof(index), index.ToString(),
                $"{nameof(index)} is out of range [0..{deque._count.ToString()}]");
        deque.Ref(index) = deque[--deque._count];
    }

    public static bool Remove<T>(ref this ValueDeque4<T> deque, in T element)
    {
        int index = deque.IndexOf(in element);
        if (index != -1)
        {
            deque.RemoveAt(index);
            return true;
        }

        return false;
    }

    public static bool RemoveAll<T>(ref this ValueDeque4<T> deque, Predicate<T> pred)
    {
        if (pred == null)
            throw new ArgumentNullException(nameof(pred), "Predicate argument can't be null");

        int removeCount = 0;
        for (int i = 0; i < deque._count; ++i)
        {
            if (removeCount != 0)
                deque.Ref(i - removeCount) = deque[i];

            if (pred(deque[i]))
                ++removeCount;
        }

        deque._count -= removeCount;
        return true;
    }

    public static bool RemoveRange<T>(ref this ValueDeque4<T> deque, int start, int count)
    {
        if (start < 0 || start >= deque._count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(),
                $"{nameof(start)} is out of range [0..{deque._count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > deque._count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(),
                $"{nameof(start)} + {nameof(count)} is out of range [0..{deque._count.ToString()}]");

        for (int i = end; i < deque._count; ++i)
        {
            deque.Ref(i - count) = deque[i];
        }

        deque._count -= count;
        return true;
    }

    public static void Swap<T>(ref this ValueDeque4<T> deque, int index1, int index2)
        => deque._array.Swap(deque.ToSlot(index1), deque.ToSlot(index2));

    public static bool Reverse<T>(ref this ValueDeque4<T> deque)
    {
        int m = deque._count / 2;
        for (int i = 0; i < m; ++i)
        {
            int j = deque._count - 1 - i;
            deque.Swap(i, j);
        }

        return true;
    }

    public static bool Reverse<T>(ref this ValueDeque4<T> deque, int start, int count)
    {
        if (start < 0 || start >= deque._count)
            throw new ArgumentOutOfRangeException(nameof(start), start.ToString(),
                $"{nameof(start)} is out of range [0..{deque._count.ToString()}]");
        int end = start + count;
        if (end < 0 || end > deque._count)
            throw new ArgumentOutOfRangeException(nameof(count), count.ToString(),
                $"{nameof(start)} + {nameof(count)} is out of range [0..{deque._count.ToString()}]");

        int m = start + count / 2;
        for (int i = start; i < m; ++i)
        {
            int j = start + count - 1 - i;
            deque.Swap(i, j);
        }

        return true;
    }
}
