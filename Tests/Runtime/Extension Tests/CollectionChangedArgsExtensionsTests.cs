using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    public class CollectionChangedArgsExtensionsTests : BaseRuntimeTest
    {
        [Test]
        public void ToNotifyCollectionChangedEventArgs_Add()
        {
            // Arrange
            CollectionChangedArgs<int> args = CollectionChangedArgs<int>.Add(new[] { 1, 2, 3 }, 0);

            // Act
            NotifyCollectionChangedEventArgs eventArgs = args.ToNotifyCollectionChangedEventArgs();

            // Assert
            Assert.That(eventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
            Assert.That(eventArgs.NewItems.Count, Is.EqualTo(3));
            Assert.That(eventArgs.NewStartingIndex, Is.EqualTo(0));
            Assert.That(eventArgs.NewItems, Is.EquivalentTo(args.NewItems.ToArray()));
        }

        [Test]
        public void ToNotifyCollectionChangedEventArgs_Remove()
        {
            // Arrange
            CollectionChangedArgs<int> args = CollectionChangedArgs<int>.Remove(new[] { 1, 2, 3 }, 0);

            // Act
            NotifyCollectionChangedEventArgs eventArgs = args.ToNotifyCollectionChangedEventArgs();

            // Assert
            Assert.That(eventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
            Assert.That(eventArgs.OldItems.Count, Is.EqualTo(3));
            Assert.That(eventArgs.OldStartingIndex, Is.EqualTo(0));
            Assert.That(eventArgs.OldItems, Is.EquivalentTo(args.OldItems.ToArray()));
        }

        [Test]
        public void ToNotifyCollectionChangedEventArgs_Replace()
        {
            // Arrange
            CollectionChangedArgs<int> args = CollectionChangedArgs<int>.Replace(new[] { 4, 5, 6 }, new[] { 1, 2, 3 }, 0);

            // Act
            NotifyCollectionChangedEventArgs eventArgs = args.ToNotifyCollectionChangedEventArgs();

            // Assert
            Assert.That(eventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Replace));
            Assert.That(eventArgs.NewItems.Count, Is.EqualTo(3));
            Assert.That(eventArgs.OldItems.Count, Is.EqualTo(3));
            Assert.That(eventArgs.NewStartingIndex, Is.EqualTo(0));
            Assert.That(eventArgs.OldStartingIndex, Is.EqualTo(0));
            Assert.That(eventArgs.NewItems, Is.EquivalentTo(args.NewItems.ToArray()));
            Assert.That(eventArgs.OldItems, Is.EquivalentTo(args.OldItems.ToArray()));
        }

        [Test]
        public void ToNotifyCollectionChangedEventArgs_Reset()
        {
            // Arrange
            CollectionChangedArgs<int> args = CollectionChangedArgs<int>.Clear(new[] { 1, 2, 3 });

            // Act
            NotifyCollectionChangedEventArgs eventArgs = args.ToNotifyCollectionChangedEventArgs();

            // Assert
            Assert.That(eventArgs.Action, Is.EqualTo(NotifyCollectionChangedAction.Reset));
            Assert.That(eventArgs.NewItems, Is.Null);
            Assert.That(eventArgs.OldItems, Is.Null);
        }

        [Test]
        public void ToNotifyCollectionChangedEventArgs_Invalid_ThrowsNotSupportedException()
        {
            // Arrange
            CollectionChangedArgs<int> args =
                new CollectionChangedArgs<int>((NotifyCollectionChangedAction) (-1), -1, Array.Empty<int>(), -1, Array.Empty<int>());

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => args.ToNotifyCollectionChangedEventArgs());
        }

        [Test]
        public void TryGetNewValue_ValidKey_FindsValue()
        {
            // Arrange
            CollectionChangedArgs<KeyValuePair<int, int>> args = CollectionChangedArgs<KeyValuePair<int, int>>.Add(ToKeyValuePairs(0, 1, 2, 3, 4), 0);

            // Act
            bool success = args.TryGetNewValue(2, out int newValue);

            // Assert
            Assert.That(success, Is.True);
            Assert.That(newValue, Is.EqualTo(2));
        }

        [Test]
        public void TryGetNewValue_InvalidKey_DoesNotFindValue()
        {
            // Arrange
            CollectionChangedArgs<KeyValuePair<int, int>> args = CollectionChangedArgs<KeyValuePair<int, int>>.Add(ToKeyValuePairs(0, 1, 2, 3, 4), 0);

            // Act
            bool success = args.TryGetNewValue(5, out int newValue);

            // Assert
            Assert.That(success, Is.False);
            Assert.That(newValue, Is.EqualTo(default(int)));
        }

        [Test]
        public void TryGetNewValue_EmptyNewItems_DoesNotFindValue()
        {
            // Arrange
            CollectionChangedArgs<KeyValuePair<int, int>> args = CollectionChangedArgs<KeyValuePair<int, int>>.Add(Array.Empty<KeyValuePair<int, int>>(), 0);

            // Act
            bool success = args.TryGetNewValue(2, out int newValue);

            // Assert
            Assert.That(success, Is.False);
            Assert.That(newValue, Is.EqualTo(default(int)));
        }

        [Test]
        public void TryGetOldValue_ValidKey_FindsValue()
        {
            // Arrange
            CollectionChangedArgs<KeyValuePair<int, int>> args = CollectionChangedArgs<KeyValuePair<int, int>>.Remove(ToKeyValuePairs(0, 1, 2, 3, 4), 0);

            // Act
            bool success = args.TryGetOldValue(3, out int oldValue);

            // Assert
            Assert.That(success, Is.True);
            Assert.That(oldValue, Is.EqualTo(3));
        }

        [Test]
        public void TryGetOldValue_InvalidKey_DoesNotFindValue()
        {
            // Arrange
            CollectionChangedArgs<KeyValuePair<int, int>> args = CollectionChangedArgs<KeyValuePair<int, int>>.Remove(ToKeyValuePairs(0, 1, 2, 3, 4), 0);

            // Act
            bool success = args.TryGetOldValue(5, out int oldValue);

            // Assert
            Assert.That(success, Is.False);
            Assert.That(oldValue, Is.EqualTo(default(int)));
        }

        [Test]
        public void TryGetOldValue_EmptyOldItems_DoesNotFindValue()
        {
            // Arrange
            CollectionChangedArgs<KeyValuePair<int, int>> args = CollectionChangedArgs<KeyValuePair<int, int>>.Remove(Array.Empty<KeyValuePair<int, int>>(), 0);

            // Act
            bool success = args.TryGetOldValue(2, out int oldValue);

            // Assert
            Assert.That(success, Is.False);
            Assert.That(oldValue, Is.EqualTo(default(int)));
        }

        private static KeyValuePair<int, int>[] ToKeyValuePairs(params int[] values)
        {
            KeyValuePair<int, int>[] pairs = new KeyValuePair<int, int>[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                pairs[i] = new KeyValuePair<int, int>(i, values[i]);
            }

            return pairs;
        }
    }
}