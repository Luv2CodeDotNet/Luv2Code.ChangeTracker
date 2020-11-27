using System;
using System.Collections.Generic;
using System.Linq;

namespace ChangeTracker
{
    /// <summary>
    ///     Is used to track changes in a ChangeTrackerCollection whether it was added, updated or deleted
    /// </summary>
    public readonly struct ChangeTracker : IChangeTracker
    {
        public ChangeTracker(object changeObject, ChangeIdentifier changeIdentifier)
        {
            ChangeObject = changeObject;
            ChangeIdentifier = changeIdentifier;
        }

        public object ChangeObject { get; }
        public ChangeIdentifier ChangeIdentifier { get; }

        public List<ChangeTracker> Add<T>(List<ChangeTracker> changesList, T compareObject) where T : class
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            if (compareObject is null) throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");

            var item = new ChangeTracker(compareObject, ChangeIdentifier.Add);
            
            var exists = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            if (exists) return changesList;
            changesList.Add(item);
            return changesList;
        }

        public List<ChangeTracker> Update<T>(List<ChangeTracker> changesList, T compareObject) where T : class
        {
            throw new NotImplementedException();
        }

        public List<ChangeTracker> Remove<T>(List<ChangeTracker> changesList, T compareObject) where T : class
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            if (compareObject is null) throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");

            var item = new ChangeTracker(compareObject, ChangeIdentifier.Delete);
            
            var exist = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            if (exist)
            {
                var deletedItem = changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject));
                changesList.Remove(deletedItem);
            }
            else
                changesList.Add(item);

            return changesList;
        }
    }
}