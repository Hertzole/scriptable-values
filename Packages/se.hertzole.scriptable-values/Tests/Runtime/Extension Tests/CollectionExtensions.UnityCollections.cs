#nullable enable

using NUnit.Framework;
using Unity.Collections;

namespace Hertzole.ScriptableValues.Tests
{
    partial class CollectionExtensions
    {
        [Test]
        public void ToNativeArray_ScriptableList_Allocator_CreatesCorrectNativeArray()
        {
            // Arrange
            TestScriptableList list = CreateInstance<TestScriptableList>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // Act
            using NativeArray<int> nativeArray = list.ToNativeArray(Allocator.Temp);

            // Assert
            Assert.That(nativeArray.Length, Is.EqualTo(list.Count), "NativeArray length should match ScriptableList count.");
            for (int i = 0; i < list.Count; i++)
            {
                Assert.That(nativeArray[i], Is.EqualTo(list[i]), $"Element at index {i} should match.");
            }
        }

#if SCRIPTABLE_VALUES_UNITY_COLLECTIONS
        [Test]
        public void ToNativeArray_ScriptableList_AllocatorHandle_CreatesCorrectNativeArray()
        {
            // Arrange
            TestScriptableList list = CreateInstance<TestScriptableList>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // Act
            using NativeArray<int> nativeArray = list.ToNativeArray(AllocatorManager.Temp);

            // Assert
            Assert.That(nativeArray.Length, Is.EqualTo(list.Count), "NativeArray length should match ScriptableList count.");
            for (int i = 0; i < list.Count; i++)
            {
                Assert.That(nativeArray[i], Is.EqualTo(list[i]), $"Element at index {i} should match.");
            }
        }

        [Test]
        public void ToNativeList_ScriptableList_CreatesCorrectNativeList()
        {
            // Arrange
            TestScriptableList list = CreateInstance<TestScriptableList>();
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // Act
            using NativeList<int> nativeList = list.ToNativeList(AllocatorManager.Temp);

            // Assert
            Assert.That(nativeList.Length, Is.EqualTo(list.Count), "NativeList length should match ScriptableList count.");
            for (int i = 0; i < list.Count; i++)
            {
                Assert.That(nativeList[i], Is.EqualTo(list[i]), $"Element at index {i} should match.");
            }
        }
#endif // SCRIPTABLE_VALUES_UNITY_COLLECTIONS
    }
}