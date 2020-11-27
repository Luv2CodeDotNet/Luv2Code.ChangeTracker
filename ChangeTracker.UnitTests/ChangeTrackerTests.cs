using System;
using System.Collections.Generic;
using Xunit;

namespace ChangeTracker.UnitTests
{
    public class ChangeTrackerTests
    {
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
            
            var changeTracker = new ChangeTracker(item, ChangeIdentifier.Add);
            
            Assert.IsType<ChangeTracker>(changeTracker);
            Assert.IsType<ExampleClass>(item);
        }

        [Fact]
        public void Add_ThrowNullReferenceException_ChangesListIsNull()
        {
            var sut = ChangeTracker;
            Assert.Throws<NullReferenceException>(() => sut.Add(null, new ExampleClass()));
        }
        [Fact]
        public void Add_ThrowNullReferenceException_CompareObjectIsNull()
        {
            var sut = ChangeTracker;
            Assert.Throws<NullReferenceException>(() => sut.Add<ExampleClass>(new List<ChangeTracker>(),null));
        }

        private static ChangeTracker ChangeTracker
        {
            get
            {
                var sut = new ChangeTracker();
                return sut;
            }
        }

        [Fact]
        public void Add_WithValidValues_Passes()
        {
            var sut = ChangeTracker;
            var changesList = new List<ChangeTracker>();
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
            var sut = ChangeTracker;
            var changesList = new List<ChangeTracker>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };
            changesList.Add(new ChangeTracker(compareObject,ChangeIdentifier.Add));
            
            var actual = sut.Add(changesList, compareObject);
            
            Assert.Single(actual);
            Assert.Collection(changesList, tracker => tracker.ChangeObject.Equals(compareObject));
            Assert.Equal(changesList, actual);
        }
        
        [Fact]
        public void Remove_ThrowNullReferenceExeption_ChangesListIsNull()
        {
            var sut = ChangeTracker;
            Assert.Throws<NullReferenceException>(() => sut.Remove(null, new ExampleClass()));
        }
        [Fact]
        public void Remove_ThrowNullReferenceException_CompareObjectIsNull()
        {
            var sut = ChangeTracker;
            Assert.Throws<NullReferenceException>(() => sut.Remove<ExampleClass>(new List<ChangeTracker>(),null));
        }
        
        [Fact]
        public void Remove_WithValidValues_Passes()
        {
            var sut = ChangeTracker;
            var changesList = new List<ChangeTracker>();
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
            var sut = ChangeTracker;
            var changesList = new List<ChangeTracker>();
            var compareObject = new ExampleClass
            {
                Id = 1,
                Prop1 = 2,
                Prop2 = true,
                Prop3 = "Hallo",
                Prop4 = new DateTime(2020, 12, 12)
            };
            changesList.Add(new ChangeTracker(compareObject,ChangeIdentifier.Delete));
            
            sut.Remove(changesList, compareObject);
            
            Assert.Empty(changesList);
        }
    }
}