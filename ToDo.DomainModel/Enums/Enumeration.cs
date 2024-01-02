using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ToDo.DomainModel.Enums
{
    /// <summary>
    /// Custom generic enumeration implementation.
    /// </summary>
    /// <valueparam name="T">Enumeration value.</valueparam>
    public abstract record Enumeration<T> : IComparable<T>
        where T : Enumeration<T>
    {
        private static readonly Lazy<Dictionary<int, T>> AllItems;
        private static readonly Lazy<Dictionary<Type, T>> AllItemsByName;

        static Enumeration()
        {
            AllItems = new Lazy<Dictionary<int, T>>(() =>
            {
                return typeof(T)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Where(x => x.FieldType == typeof(T))
                    .Select(x => x.GetValue(null))
                    .Cast<T>()
                    .ToDictionary(x => x.Id, x => x);
            });
            AllItemsByName = new Lazy<Dictionary<Type, T>>(() =>
            {
                var items = new Dictionary<Type, T>(AllItems.Value.Count);
                foreach (var item in AllItems.Value)
                {
                    if (!items.TryAdd(item.Value.Value, item.Value))
                    {
                        throw new Exception($"Value needs to be unique. '{item.Value.Value}' already exists");
                    }
                }

                return items;
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration{T}"/> class.
        /// </summary>
        /// <param name="id">Enumeration item id.</param>
        /// <param name="value">Enumeration item value.</param>
        protected Enumeration(int id, Type value)
        {
            this.Id = id;
            this.Value = value;
        }

        /// <summary>
        /// Gets enumeration item id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets enumeration item value.
        /// </summary>
        public Type Value { get; }

        /// <summary>
        /// Returns string representation of enumeration item.
        /// </summary>
        /// <returns>String containing enumeration item class value.</returns>
        public override string ToString() => this.Value.ToString();

        /// <summary>
        /// Returns all enumeration items' values.
        /// </summary>
        /// <valueparam name="T">Type of enumeration values.</valueparam>
        /// <returns>List of enumeration values.</returns>
        public static IEnumerable<T> GetAll()
        {
            return AllItems.Value.Values;
        }

        /// <summary>
        /// Returns abs difference between 2 enumeration item ids.
        /// </summary>
        /// <param name="firstId">First enumeration item for comparsion.</param>
        /// <param name="secondId">Second enumeration item for comparsion.</param>
        /// <returns>Abs difference between 2 enumeration item ids.</returns>
        public static int AbsoluteDifference(Enumeration<T> firstId, Enumeration<T> secondId)
        {
            return Math.Abs(firstId.Id - secondId.Id);
        }

        /// <summary>
        /// Returns enumeration item by value.
        /// </summary>
        /// <valueparam name="T">Type of enumeration.</valueparam>
        /// <param name="id">Id to serach by.</param>
        /// <returns>Found enumeration item.</returns>
        /// <exception cref="InvalidOperationException">Thrown if enumeration item with given id does not exist.</exception>
        public static T FromId(int id)
        {
            if (AllItems.Value.TryGetValue(id, out var matchingItem))
            {
                return matchingItem;
            }

            throw new InvalidOperationException($"'{id}' is not a valid value in {typeof(T)}");
        }

        /// <summary>
        /// Returns enumeration item by name.
        /// </summary>
        /// <valueparam name="T">Type of enumeration.</valueparam>
        /// <param name="value">Item name to serach by.</param>
        /// <returns>Found enumeration item.</returns>
        /// <exception cref="InvalidOperationException">Thrown if enumeration item with given value does not exist.</exception>
        public static T FromValue(Type value)
        {
            if (AllItemsByName.Value.TryGetValue(value, out var matchingItem))
            {
                return matchingItem;
            }

            throw new InvalidOperationException($"'{value}' is not a valid class value in {typeof(T)}");
        }

        /// <summary>
        /// Checks if given object is greater/equal/smaller than enumeration item id.
        /// </summary>
        /// <param name="other">Object to compare.</param>
        /// <returns>-1 - enumeration item id is smaller, 0 - equal, 1 - enumeration item id is greater.</returns>
        public int CompareTo(T? other) => this.Id.CompareTo(other!.Id);
    }
}