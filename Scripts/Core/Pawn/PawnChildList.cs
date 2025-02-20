using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rusty.Collections
{
    /// <summary>
    /// A list that uses an internal dictionary for faster lookup. Duplicate keys are allowed, but discouraged, as they can lead
    /// to worse performance and confusing behavior.
    /// </summary>
    public class DictList<KeyT, ValueT> : IEnumerable<ValueT>
    {
        /* Public properties. */
        public int Count => Values.Count;

        /* Private properties. */
        private List<KeyT> Keys { get; set; }
        private List<ValueT> Values { get; set; }
        private Dictionary<KeyT, ValueT> Lookup { get; set; }

        /* Constructors. */
        public DictList()
        {
            Keys = new();
            Values = new();
            Lookup = new();
        }

        /* Operators. */
        /// <summary>
        /// Get the value at some index.
        /// </summary>
        public ValueT this[int index] => GetValue(index);
        /// <summary>
        /// Get the value at some key.
        /// </summary>
        public ValueT this[KeyT key] => GetValue(key);

        /* Public methods. */
        /// <summary>
        /// Add a key-value pair at the end of the collection. If the key has already been used, it will be discarded, but the
        /// value is still added.
        /// </summary>
        public void Add(KeyT key, ValueT value)
        {
            Keys.Add(key);
            Values.Add(value);
            try
            {
                Lookup.Add(key, value);
            }
            catch { }
        }

        /// <summary>
        /// Insert a key-value pair. If the key has already been used, it will be discarded, but the value is still added.
        /// </summary>
        public void Insert(int index, KeyT key, ValueT value)
        {
            Keys.Insert(index, key);
            Values.Insert(index, value);
            try
            {
                Lookup.Add(key, value);
            }
            catch
            {
                if (index < IndexOfKey(key))
                    Lookup[key] = value;
            }
        }

        /// <summary>
        /// Get the key at some index.
        /// </summary>
        public KeyT GetKey(int index)
        {
            return Keys[index];
        }

        /// <summary>
        /// Get the value at some index.
        /// </summary>
        public ValueT GetValue(int index)
        {
            return Values[index];
        }

        /// <summary>
        /// Get the value associated with some key.
        /// </summary>
        public ValueT GetValue(KeyT key)
        {
            return Lookup[key];
        }

        /// <summary>
        /// Find the first index associated with some key. Returns -1 if the key is not used in this collection.
        /// </summary>
        public int IndexOfKey(KeyT key)
        {
            return Keys.IndexOf(key);
        }

        /// <summary>
        /// Find the first index of some value. Returns -1 if the value is not used in this collection.
        /// </summary>
        public int IndexOfValue(ValueT value)
        {
            return Values.IndexOf(value);
        }

        /// <summary>
        /// Return whether a key is used in this collection.
        /// </summary>
        public bool ContainsKey(KeyT key)
        {
            return IndexOfKey(key) != -1;
        }

        /// <summary>
        /// Return whether a value is used in this collection.
        /// </summary>
        public bool ContainsValue(ValueT value)
        {
            return IndexOfValue(value) != -1;
        }

        /// <summary>
        /// Remove a key-value pair from the list.
        /// </summary>
        public void RemoveKey(KeyT key)
        {
            RemoveAt(IndexOfKey(key));
        }

        /// <summary>
        /// Remove the first instance of a value from the collection, alongside its key.
        /// </summary>
        public void RemoveValue(ValueT value)
        {
            RemoveAt(IndexOfValue(value));
        }

        /// <summary>
        /// Remove the element at some index.
        /// </summary>
        public void RemoveAt(int index)
        {
            KeyT key = Keys[index];

            try
            {
                if (Lookup[key].Equals(Values[index]))
                    Lookup.Remove(key);
            }
            catch { }

            Keys.RemoveAt(index);
            Values.RemoveAt(index);

            int newIndex = IndexOfKey(key);
            if (newIndex != -1)
                Lookup.Add(key, Values[newIndex]);
        }

        /// <summary>
        /// Clear all elements from the collection.
        /// </summary>
        public void Clear()
        {
            Keys.Clear();
            Values.Clear();
            Lookup.Clear();
        }

        /* Enumerating. */
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ValueT> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<ValueT>
        {
            public DictList<KeyT, ValueT> Values { get; set; }

            int position = -1;

            public Enumerator(DictList<KeyT, ValueT> values)
            {
                Values = values;
            }

            public bool MoveNext()
            {
                position++;
                return position < Values.Count;
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public ValueT Current
            {
                get
                {
                    try
                    {
                        return Values[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            public void Dispose() { }
        }
    }
}
namespace Rusty.Pawns
{

    /// <summary>
    /// Wrapper for list of pawn child objects that offers some convenience methods.
    /// </summary>
    public class PawnChildList<MainType> : IEnumerator<MainType> where MainType : PawnComponent
    {
        /* Public properties. */
        public int Count => Elements != null ? Elements.Count : 0;
        public object Current => GetEnumerator().Current;

        /* Private properties. */
        private Pawn Pawn { get; set; }
        private List<MainType> Elements { get; set; } = new();
        private Dictionary<string, MainType> Lookup { get; set; }
        private bool OnlyDiscoverables { get; set; }
        private bool StopAtPawnChildren { get; set; }
        MainType IEnumerator<MainType>.Current => GetEnumerator().Current;

        /* Constructors. */
        public PawnChildList(Pawn pawn, bool onlyDiscoverables, bool stopAtPawnChildren = false)
        {
            Pawn = pawn;
            Elements = new();
            Lookup = new();
            OnlyDiscoverables = onlyDiscoverables;
            StopAtPawnChildren = stopAtPawnChildren;
        }

        /* Operators. */
        public MainType this[int index] => Get(index);
        public MainType this[string name] => Get(name);

        /* Public methods. */
        public IEnumerator<MainType> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        /// <summary>
        /// IEnumerator.MoveNext.
        /// </summary>
        public bool MoveNext()
        {
            return GetEnumerator().MoveNext();
        }

        /// <summary>
        /// IEnumerator.Reset.
        /// </summary>
        public void Reset()
        {
            GetEnumerator().Reset();
        }

        /// <summary>
        /// IDisposable.Dispose.
        /// </summary>
        public void Dispose()
        {
            GetEnumerator().Dispose();
        }

        /// <summary>
        /// Create the pawn list from a node tree. Searches for all nodes of the relevant types.
        /// </summary>
        public void CreateFromNodeTree(Node root)
        {
            if (Elements == null)
                Elements = new();
            else
                Elements.Clear();

            Elements = GetRecursively(root, root, OnlyDiscoverables, StopAtPawnChildren);
        }

        /// <summary>
        /// Add a pawn child to the list.
        /// </summary>
        public void Add(MainType pawnChild)
        {
            Elements.Add(pawnChild);
        }

        /// <summary>
        /// Remove a pawn child from the list, if it is contained in the list.
        /// </summary>
        public void Remove(MainType pawnChild)
        {
            Elements.Remove(pawnChild);
        }

        /// <summary>
        /// Remove all nodes from the list.
        /// </summary>
        public void Clear()
        {
            Elements.Clear();
        }

        /// <summary>
        /// Get a node by its list index.
        /// </summary>
        public MainType Get(int index)
        {
            return Elements[index];
        }

        /// <summary>
        /// Get a node by name from the list.
        /// </summary>
        public MainType Get(string name)
        {
            // Try to find the element and return it if it was found.
            MainType main = TryGet(name);
            if (main != null)
                return main;

            // Print warning if we didn't find the element.
            GD.PushWarning($"Tried to get the {typeof(MainType).Name} '{name}', but this object could not be found on "
                + "the pawn. Make sure you didn't mispell the name, and that the object is marked as discoverable.");
            return null;
        }

        /// <summary>
        /// Get the first node in the list of some type.
        /// </summary>
        public SubType Get<SubType>() where SubType : MainType
        {
            // Try to find the element and return it if it was found.
            SubType sub = TryGet<SubType>();
            if (sub != null)
                return sub;

            // Print warning if we didn't find the element.
            GD.PushWarning($"Tried to get the first {typeof(MainType).Name} of type {typeof(SubType).Name}, but no "
                + "object of that type could be found. Make sure it's been marked as discoverable.");
            return null;
        }

        /// <summary>
        /// Get a node by type and name from the list.
        /// </summary>
        public SubType Get<SubType>(string name) where SubType : MainType
        {
            // Try to find the element and return it if it was found.
            SubType sub = TryGet<SubType>(name);
            if (sub != null)
                return sub;

            // Print warning if we didn't find the element.
            GD.PushWarning($"Tried to get the {typeof(MainType).Name} '{name}' of type {typeof(SubType).Name}, but this object "
                + "could not be found on the pawn. Make sure you didn't mispell the name, and that the object is discoverable.");
            return null;
        }

        /// <summary>
        /// Get all nodes of a specific type in the list.
        /// </summary>
        public List<SubType> GetAll<SubType>() where SubType : MainType
        {
            List<SubType> result = new();
            for (int i = 0; i < Count; i++)
            {
                MainType main = Elements[i];
                if (main is SubType sub)
                    result.Add(sub);
            }
            return result;
        }

        /// <summary>
        /// Get the first node of this type that is enabled and has all of its conditions met.
        /// </summary>
        public MainType GetFirstActive()
        {
            for (int i = 0; i < Count; i++)
            {
                MainType main = Get(i);
                if (main.CheckActive(Pawn))
                    return main;
            }
            return null;
        }

        /// <summary>
        /// Get the first node of some sub-type that is enabled and has all of its conditions met.
        /// </summary>
        public SubType GetFirstActive<SubType>() where SubType : MainType
        {
            for (int i = 0; i < Count; i++)
            {
                MainType main = Get(i);
                if (main is SubType sub && sub.CheckActive(Pawn))
                    return sub;
            }
            return null;
        }

        /// <summary>
        /// Check if the list contains a node with a specific name.
        /// </summary>
        public bool Contains(string name)
        {
            return TryGet(name) != null;
        }

        /// <summary>
        /// Check if the list contains a node of a specific type.
        /// </summary>
        public bool Contains<SubType>() where SubType : MainType
        {
            return TryGet<SubType>() != null;
        }

        /// <summary>
        /// Check if the list contains a node of a specific type and with a specific name.
        /// </summary>
        public bool Contains<SubType>(string name) where SubType : MainType
        {
            return TryGet<SubType>(name) != null;
        }

        /// <summary>
        /// Check if the list contains a specific node.
        /// </summary>
        public bool Contains(MainType item)
        {
            for (int i = 0; i < Count; i++)
            {
                MainType entry = Elements[i];
                if (entry == item)
                    return true;
            }
            return false;
        }

        /* Private methods. */
        /// <summary>
        /// Recursively searches for nodes of our desired type.
        /// </summary>
        private static List<MainType> GetRecursively(Node root, Node current, bool onlyDiscoverables, bool stopAtPawnChildren)
        {
            // Stop at pawn nodes.
            if (current != root && (current is Pawn || stopAtPawnChildren && current is PawnComponent))
                return new();

            // Examine all children of the root...
            List<MainType> results = new();
            for (int i = 0; i < current.GetChildCount(); i++)
            {
                Node child = current.GetChild(i);

                // If the child is of our desired type, add it to the list.
                if (child is MainType main)
                {
                    if (main.Discoverable || !onlyDiscoverables)
                        results.Add(main);
                }

                // Also examine the child's children recursively.
                List<MainType> recursiveResults = GetRecursively(root, child, onlyDiscoverables, stopAtPawnChildren);
                for (int j = 0; j < recursiveResults.Count; j++)
                {
                    results.Add(recursiveResults[j]);
                }
            }

            // Return all found nodes.
            return results;
        }

        private MainType TryGet(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                MainType entry = Elements[i];
                if (entry.Name == name)
                    return entry;
            }
            return null;
        }

        private SubType TryGet<SubType>() where SubType : MainType
        {
            for (int i = 0; i < Count; i++)
            {
                MainType main = Elements[i];
                if (main is SubType sub)
                    return sub;
            }
            return null;
        }

        private SubType TryGet<SubType>(string name) where SubType : MainType
        {
            for (int i = 0; i < Count; i++)
            {
                MainType main = Elements[i];
                if (main is SubType sub && main.Name == name)
                    return sub;
            }
            return null;
        }
    }
}