using System;
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
    }
}