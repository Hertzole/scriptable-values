using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        [TestCase(false, TestName = "GetRange")]
        [TestCase(true, TestName = "Slice")]
        public void GetRange(bool slice)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            // Act
            List<int> range = slice ? list.Slice(2, 5) : list.GetRange(2, 5);

            // Assert
            Assert.AreEqual(5, range.Count);
            Assert.AreEqual(3, range[0]);
            Assert.AreEqual(4, range[1]);
            Assert.AreEqual(5, range[2]);
            Assert.AreEqual(6, range[3]);
            Assert.AreEqual(7, range[4]);
        }

        [Test]
        [TestCase(false, TestName = "GetRange")]
        [TestCase(true, TestName = "Slice")]
        public void GetRange_Validations(bool slice)
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() =>
            {
                if (slice)
                {
                    list.Slice(-1, 5);
                }
                else
                {
                    list.GetRange(-1, 5);
                }
            });

            AssertThrows<ArgumentOutOfRangeException>(() =>
            {
                if (slice)
                {
                    list.Slice(0, -1);
                }
                else
                {
                    list.GetRange(0, -1);
                }
            });

            AssertThrows<ArgumentException>(() =>
            {
                if (slice)
                {
                    list.Slice(0, 11);
                }
                else
                {
                    list.GetRange(0, 11);
                }
            });

            AssertThrows<ArgumentException>(() =>
            {
                if (slice)
                {
                    list.Slice(5, 6);
                }
                else
                {
                    list.GetRange(5, 6);
                }
            });

            AssertThrows<ArgumentException>(() =>
            {
                if (slice)
                {
                    list.Slice(11, 0);
                }
                else
                {
                    list.GetRange(11, 0);
                }
            });

            AssertThrows<ArgumentException>(() =>
            {
                if (slice)
                {
                    list.Slice(10, 1);
                }
                else
                {
                    list.GetRange(10, 1);
                }
            });
        }

        [Test]
        public void GetRange_DestinationList_NormalList()
        {
            GetRange_DestinationList_Test(new List<int>());
        }

        [Test]
        public void GetRange_DestinationList_ScriptableList()
        {
            // Arrange
            var newList = CreateInstance<TestScriptableList>();

            // Act
            GetRange_DestinationList_Test(newList);

            // Cleanup
            newList.Clear();
        }

        private void GetRange_DestinationList_Test(IList<int> destinationList)
        {
            Test(false);
            Test(true);

            void Test(bool slice)
            {
                // Arrange
                list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                // Add dummy items to make sure the list is cleared first.
                for (int i = 0; i < 100; i++)
                {
                    destinationList.Add(i);
                }

                // Act
                if (slice)
                {
                    list.Slice(2, 5, destinationList);
                }
                else
                {
                    list.GetRange(2, 5, destinationList);
                }

                // Assert
                Assert.AreEqual(5, destinationList.Count);
                Assert.AreEqual(3, destinationList[0]);
                Assert.AreEqual(4, destinationList[1]);
                Assert.AreEqual(5, destinationList[2]);
                Assert.AreEqual(6, destinationList[3]);
                Assert.AreEqual(7, destinationList[4]);

                list.Clear();
            }
        }

        [Test]
        public void GetRange_DestinationList_Validations_NormalList()
        {
            GetRange_DestinationList_Validations_Test(new List<int>());
        }

        [Test]
        public void GetRange_DestinationList_Validations_ScriptableList()
        {
            // Arrange
            var newList = CreateInstance<TestScriptableList>();

            // Act
            GetRange_DestinationList_Validations_Test(newList);

            // Cleanup
            newList.Clear();
        }

        private void GetRange_DestinationList_Validations_Test(IList<int> destinationList)
        {
            Test(false);
            Test(true);

            void Test(bool slice)
            {
                // Arrange
                list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

                // Act & Assert
                AssertThrows<ArgumentOutOfRangeException>(() =>
                {
                    if (slice)
                    {
                        list.Slice(-1, 5, destinationList);
                    }
                    else
                    {
                        list.GetRange(-1, 5, destinationList);
                    }
                });

                AssertThrows<ArgumentOutOfRangeException>(() =>
                {
                    if (slice)
                    {
                        list.Slice(0, -1, destinationList);
                    }
                    else
                    {
                        list.GetRange(0, -1, destinationList);
                    }
                });

                AssertThrows<ArgumentException>(() =>
                {
                    if (slice)
                    {
                        list.Slice(0, 11, destinationList);
                    }
                    else
                    {
                        list.GetRange(0, 11, destinationList);
                    }
                });

                AssertThrows<ArgumentException>(() =>
                {
                    if (slice)
                    {
                        list.Slice(5, 6, destinationList);
                    }
                    else
                    {
                        list.GetRange(5, 6, destinationList);
                    }
                });

                AssertThrows<ArgumentException>(() =>
                {
                    if (slice)
                    {
                        list.Slice(11, 0, destinationList);
                    }
                    else
                    {
                        list.GetRange(11, 0, destinationList);
                    }
                });

                AssertThrows<ArgumentException>(() =>
                {
                    if (slice)
                    {
                        list.Slice(10, 1, destinationList);
                    }
                    else
                    {
                        list.GetRange(10, 1, destinationList);
                    }
                });

                AssertThrows<ArgumentNullException>(() =>
                {
                    if (slice)
                    {
                        list.Slice(0, 1, null!);
                    }
                    else
                    {
                        list.GetRange(0, 1, null!);
                    }
                });

                list.Clear();
            }
        }

        [Test]
        public void GetRange_DestinationListIsReadOnly_ThrowsException()
        {
            list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            AssertThrows<ArgumentException>(() => list.GetRange(0, 5, Array.AsReadOnly(new[] { 1, 2, 3 })));
            AssertThrows<ArgumentException>(() => list.Slice(0, 5, Array.AsReadOnly(new[] { 1, 2, 3 })));
        }
    }
}