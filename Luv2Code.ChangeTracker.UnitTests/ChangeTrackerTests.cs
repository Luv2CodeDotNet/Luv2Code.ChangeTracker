using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Luv2Code.ChangeTracker.UnitTests
{
    public class ChangeTrackerTests
    {
        private static Changes Changes
        {
            get
            {
                var sut = new Changes();
                return sut;
            }
        }

        [Fact]
        public void ChangeTracker_Object()
        {
            var item = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };

            var changeTracker = new Changes(item, ChangeIdentifier.Add);

            Assert.IsType<Changes>(changeTracker);
            Assert.IsType<ExampleClass>(item);
        }

        [Fact]
        public void Add_ThrowNullReferenceException_ChangesListIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.Add(null, new ExampleClass()));
        }

        [Fact]
        public void Add_ThrowNullReferenceException_CompareObjectIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.Add<ExampleClass>(new List<Changes>(), null));
        }

        [Fact]
        public void Add_WithValidValues_Passes()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };

            var actual = sut.Add(changesList, compareObject);
            Assert.Single(actual);
            Assert.Collection(changesList, tracker => tracker.ChangeObject.Equals(compareObject));
            Assert.Equal(changesList, actual);
        }

        [Fact]
        public void Add_ReturnList_IfItemExists()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };
            changesList.Add(new Changes(compareObject, ChangeIdentifier.Add));

            var actual = sut.Add(changesList, compareObject);

            Assert.Single(actual);
            Assert.Collection(changesList, tracker => tracker.ChangeObject.Equals(compareObject));
            Assert.Equal(changesList, actual);
        }

        [Fact]
        public void Remove_ThrowNullReferenceExeption_ChangesListIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.Remove(null, new ExampleClass()));
        }

        [Fact]
        public void Remove_ThrowNullReferenceException_CompareObjectIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.Remove<ExampleClass>(new List<Changes>(), null));
        }

        [Fact]
        public void Remove_WithValidValues_Passes()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };

            var actual = sut.Remove(changesList, compareObject);

            Assert.Single(actual);
            Assert.Collection(changesList, tracker => tracker.ChangeObject.Equals(compareObject));
            Assert.Equal(changesList, actual);
        }

        [Fact]
        public void Remove_ReturnList_IfItemExists()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };
            changesList.Add(new Changes(compareObject, ChangeIdentifier.Remove));

            sut.Remove(changesList, compareObject);

            Assert.Empty(changesList);
        }

        [Fact]
        public void Remove_ThenAdd_ReturnList_Empty()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };

            sut.Remove(changesList, compareObject);
            sut.Add(changesList, compareObject);

            Assert.Empty(changesList);
        }

        [Fact]
        public void Add_ThenRemove_ReturnList_Empty()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };

            sut.Add(changesList, compareObject);
            sut.Remove(changesList, compareObject);

            Assert.Empty(changesList);
        }

        [Fact]
        public void Update_ThrowNullreferenceException_WhenChangesListIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.Update<ExampleClass>(new List<Changes>(), null));
        }

        [Fact]
        public void Update_ThrowsNullReferenceException_WhenCompareObjectIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.Update(null, new ExampleClass()));
        }

        [Fact]
        public void Update_AddNewItemToChangesList_ItemDontExists_ReturnList_Count_One()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };

            sut.Update(changesList, compareObject);

            Assert.Single(changesList);
        }

        [Fact]
        public void Update_TryAddExistingItem_ReturnList_Count_One()
        {
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };

            changesList.Add(new Changes(compareObject, ChangeIdentifier.Update));

            sut.Update(changesList, compareObject);

            Assert.Single(changesList);
        }

        [Fact]
        public void ChangesListHasChanges_ThrowsNullReferenceException_WhenCompareObjectIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.ChangesListHasChanges(null));
        }

        [Fact]
        public void HasChanges_ReturnsTrue_IfCountGreaterThanZero()
        {
            var sut = Changes;
            var list = new List<Changes> {new()};
            var actual = Changes.ChangesListHasChanges(list);
            Assert.True(actual);
        }

        [Fact]
        public void HasChanges_ReturnsFalse_IfCountIsZero()
        {
            var sut = Changes;
            var list = new List<Changes>();
            var actual = Changes.ChangesListHasChanges(list);
            Assert.False(actual);
        }

        [Fact]
        public void ObjectHasChanges_ThrowsNullReferenceException_WhenOldObjectIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.ObjectHasChanges(new ExampleClass(), null));
        }

        [Fact]
        public void ObjectHasChanges_ThrowsNullReferenceException_WhenNewObjectIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.ObjectHasChanges(null, new ExampleClass()));
        }

        [Fact]
        public void ObjectHasChanges_ReturnTrueIfObjects_AreSame()
        {
            var sut = Changes;
            var obj = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 10, 01)
            };

            var actual = sut.ObjectHasChanges(obj, obj);
            Assert.False(actual);
        }

        [Fact]
        public void ObjectHasChanges_ReturnTrueIfObjects_AreEqual()
        {
            var sut = Changes;
            var objOld = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 10, 01)
            };
            var objNew = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 10, 01)
            };

            var actual = sut.ObjectHasChanges(objOld, objNew);
            Assert.False(actual);
        }

        [Fact]
        public void ObjectHasChanges_ThrowInvalidCastException_IfObjectsTypeIsDifferent()
        {
            var sut = Changes;
            Assert.Throws<InvalidCastException>(() =>
                sut.ObjectHasChanges<object>(new List<Changes>(), new List<int>()));
        }

        [Fact]
        public void ChangesListHasChanges_ThrowNullReferenceException_IfChangesListIsNull()
        {
            var sut = Changes;
            Assert.Throws<NullReferenceException>(() => sut.ChangesListHasChanges(null));
        }

        [Fact]
        public void ObjectHasChanges_OnePropIsNull_ReturnTrue()
        {
            var sut = Changes;
            var objOld = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = null,
                Prop4 = new DateTime(2020, 10, 01)
            };
            var objNew = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = null,
                Prop4 = new DateTime(2020, 10, 01)
            };
            var actual = sut.ObjectHasChanges(objOld, objNew);

            Assert.False(actual);
        }

        [Fact]
        public void ObjectHasChanges_OnePropIsNull_ReturnFalse()
        {
            var sut = Changes;
            var objOld = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "null",
                Prop4 = new DateTime(2020, 10, 01)
            };
            var objNew = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = null,
                Prop4 = new DateTime(2020, 10, 01)
            };
            var actual = sut.ObjectHasChanges(objOld, objNew);

            Assert.True(actual);
        }

        [Fact]
        public void Add_ParameterIsNull_ThrowNullReferenceException()
        {
            // Arrange
            var sut = Changes;
            var changesList = new List<Changes>();
            var compareObject = new ExampleClass();
            var logger = new Logger<ExampleClass>(new NullLoggerFactory());

            // Assert
            Assert.Throws<NullReferenceException>(() => sut.Add(changesList, compareObject, null));
            Assert.Throws<NullReferenceException>(() => sut.Add(null, compareObject, logger));
            Assert.Throws<NullReferenceException>(() => sut.Add<ExampleClass>(changesList, null, logger));
        }
    }
}