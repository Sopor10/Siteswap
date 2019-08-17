using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils
{
    public sealed class Maybe<T> : IEnumerable<T>
    {
        private bool HasItem { get; }
        private T Item { get; }
 
        public Maybe()
        {
            HasItem = false;
        }
 
        public Maybe(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
 
            HasItem = true;
            Item = item;
        }
 
        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
 
            if (HasItem)
                return new Maybe<TResult>(selector(Item));
            return new Maybe<TResult>();
        }
 
        public T GetValueOrFallback(T fallbackValue)
        {
            if (fallbackValue == null)
                throw new ArgumentNullException(nameof(fallbackValue));
 
            if (HasItem)
                return Item;
            return fallbackValue;
        }
 
        public override bool Equals(object obj)
        {
            var other = obj as Maybe<T>;
            if (other == null)
                return false;
 
            return Equals(Item, other.Item);
        }
 
        public override int GetHashCode()
        {
            return HasItem ? Item.GetHashCode() : 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (HasItem)
            {
                yield return Item;
            }
        }
    }
}