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
            //return TrackChanges(changesList, task, ChangeIdentifier.Add);
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

        public List<ChangeTracker> Remove<T>(List<ChangeTracker> changesList, T task) where T : class
        {
            return TrackChanges(changesList, task, ChangeIdentifier.Delete);
        }

        private static List<ChangeTracker> TrackChanges<T>(List<ChangeTracker> changesList, T task, ChangeIdentifier i) where T : class
        {
            var item = new ChangeTracker(task, i);
            switch (i)
            {
                case ChangeIdentifier.Add:
                {
                    var any = changesList.Any(c => c.ChangeObject.Equals(task));
                    if (any) return changesList;
                    changesList.Add(item);
                    break;
                }
                case ChangeIdentifier.Delete:
                {
                    var any = changesList.Any(c => c.ChangeObject.Equals(task));
                    if (any)
                        changesList.Remove(item);
                    else
                        changesList.Add(item);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, null);
            }

            return changesList;
        }
    }
}