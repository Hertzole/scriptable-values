using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void ConvertAll()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, 4, 5 });
            List<int> before = list.ToList();

            // Act
            List<int> after = list.ConvertAll(i => 10 * i);

            // Assert
            Assert.AreEqual(before.Count, list.Count);
            Assert.AreEqual(before.Count, after.Count);

            for (int i = 0; i < before.Count; i++)
            {
                Assert.AreEqual(before[i], list[i]);
                Assert.AreEqual(10 * before[i], after[i]);
            }
        }

        [Test]
        public void ConvertAll_DestinationList_NormalList()
        {
            ConvertAllTest(new List<int>());
        }

        [Test]
        public void ConvertAll_DestinationList_ScriptableList()
        {
            // Arrange
            TestScriptableList newList = CreateInstance<TestScriptableList>();

            ConvertAllTest(newList);

            // Cleanup
            newList.Clear();
        }

        private void ConvertAllTest(IList<int> destinationList)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, 4, 5 });
            List<int> before = list.ToList();

            // Act
            list.ConvertAll(destinationList, i => 10 * i);

            // Assert
            Assert.AreEqual(before.Count, list.Count);
            Assert.AreEqual(before.Count, destinationList.Count);

            for (int i = 0; i < before.Count; i++)
            {
                Assert.AreEqual(before[i], list[i]);
                Assert.AreEqual(10 * before[i], destinationList[i]);
            }
        }

        [Test]
        public void ConvertAll_DestinationListIsReadOnly_ThrowsException()
        {
            AssertThrows<ReadOnlyException>(() => list.ConvertAll(Array.AsReadOnly(new[] { 1f, 2f, 3f }), i => 10 * i));
        }

        [Test]
        public void ConvertAll_Validations()
        {
            AssertThrows<ArgumentNullException>(() => list.ConvertAll<int>(null!));
        }

        [Test]
        public void ConvertAll_List_Validations()
        {
            AssertThrows<ArgumentNullException>(() => list.ConvertAll(new List<int>(), null!));
            AssertThrows<ArgumentNullException>(() => list.ConvertAll(null!, i => 10 * i));
        }
    }
}