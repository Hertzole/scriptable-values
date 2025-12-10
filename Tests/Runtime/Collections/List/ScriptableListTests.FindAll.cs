using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void FindAll_VerifyVanilla()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, -4, -5 });
            Predicate<int> predicate = i => i >= 0;

            // Act
            List<int> result = list.FindAll(predicate);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
        }

        [Test]
        public void FindAll_VerifyDuplicates()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, -4, -5, 1, 2, 3 });
            Predicate<int> predicate = i => i >= 0;

            // Act
            List<int> result = list.FindAll(predicate);

            // Assert
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(1, result[3]);
            Assert.AreEqual(2, result[4]);
            Assert.AreEqual(3, result[5]);
        }

        [Test]
        public void FindAll_VerifyVanilla_Destination_Test_NormalList()
        {
            FindAll_VerifyVanilla_Destination_Test(new List<int>());
        }

        [Test]
        public void FindAll_VerifyVanilla_Destination_Test_ScriptableList()
        {
            // Arrange
            TestScriptableList newList = CreateInstance<TestScriptableList>();

            // Act
            FindAll_VerifyVanilla_Destination_Test(newList);

            // Cleanup
            newList.Clear();
        }

        private void FindAll_VerifyVanilla_Destination_Test(IList<int> destinationList)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, -4, -5 });
            Predicate<int> predicate = i => i >= 0;

            // Act
            list.FindAll(destinationList, predicate);

            // Assert
            Assert.AreEqual(3, destinationList.Count);
            Assert.AreEqual(1, destinationList[0]);
            Assert.AreEqual(2, destinationList[1]);
            Assert.AreEqual(3, destinationList[2]);
        }

        [Test]
        public void FindAll_VerifyDuplicates_Destination_NormalList()
        {
            FindAll_VerifyDuplicates_Test(new List<int>());
        }

        [Test]
        public void FindAll_VerifyDuplicates_Destination_ScriptableList()
        {
            // Arrange
            TestScriptableList newList = CreateInstance<TestScriptableList>();

            // Act
            FindAll_VerifyDuplicates_Test(newList);

            // Cleanup
            newList.Clear();
        }

        private void FindAll_VerifyDuplicates_Test(IList<int> destinationList)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, -4, -5, 1, 2, 3 });
            Predicate<int> predicate = i => i >= 0;

            // Act
            list.FindAll(destinationList, predicate);

            // Assert
            Assert.AreEqual(6, destinationList.Count);
            Assert.AreEqual(1, destinationList[0]);
            Assert.AreEqual(2, destinationList[1]);
            Assert.AreEqual(3, destinationList[2]);
            Assert.AreEqual(1, destinationList[3]);
            Assert.AreEqual(2, destinationList[4]);
            Assert.AreEqual(3, destinationList[5]);
        }

        [Test]
        public void FindAll_DestinationListIsReadOnly_ThrowsException()
        {
            AssertThrows<ReadOnlyException>(() => list.FindAll(Array.AsReadOnly(new[] { 1, 2, 3 }), i => i >= 0));
        }

        [Test]
        public void FindAll_Validations()
        {
            AssertThrows<ArgumentNullException>(() => list.FindAll(null!));
        }

        [Test]
        public void FindAll_List_Validations()
        {
            AssertThrows<ArgumentNullException>(() => list.FindAll(new List<int>(), null!));
            AssertThrows<ArgumentNullException>(() => list.FindAll(null!, i => i >= 0));
        }
    }
}