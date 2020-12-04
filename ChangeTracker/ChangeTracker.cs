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

        /// <summary>
        ///     Add entity of type T to changesList if not exists.
        ///     If entity exist, the modifier will be check. If modifier is Delete, entity will be removed from list
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<ChangeTracker> Add<T>(List<ChangeTracker> changesList, T compareObject) where T : class
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            if (compareObject is null) throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");

            var item = new ChangeTracker(compareObject, ChangeIdentifier.Add);

            var exists = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            if (exists)
            {
                var identifier = changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject)).ChangeIdentifier;
                if (identifier == ChangeIdentifier.Delete)
                    changesList.Remove(changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject)));
                return changesList;
            }

            changesList.Add(item);
            return changesList;
        }

        /// <summary>
        ///     Add identity to changes list if not exists. If entity exists, return changesList
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ChangeTracker> Update<T>(List<ChangeTracker> changesList, T compareObject) where T : class
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            if (compareObject is null) throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");

            var item = new ChangeTracker(compareObject, ChangeIdentifier.Update);
            var exist = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            if (exist)
                return changesList;
            changesList.Add(item);
            return changesList;
        }

        /// <summary>
        ///     Removes entity of type T from changesList if exists.
        ///     If entity dont exist, adds entity of type T to tracking list with modifier: Remove
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>ChangesList with actual values</returns>
        /// <exception cref="NullReferenceException"></exception>
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
            {
                changesList.Add(item);
            }

            return changesList;
        }

        /// <summary>
        ///     Check if changes list has changes, return true or false
        /// </summary>
        /// <param name="changesList"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool ChangesListHasChanges(List<ChangeTracker> changesList)
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            return changesList.Count > 0;
        }

        /// <summary>
        ///     Check if an entity has some property values changed
        /// </summary>
        /// <param name="oldObject"></param>
        /// <param name="newObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public bool ObjectHasChanges<T>(T oldObject, T newObject) where T : class
        {
            if (oldObject is null) throw new NullReferenceException($"Parameter {nameof(oldObject)} was null");
            if (newObject is null) throw new NullReferenceException($"Parameter {nameof(newObject)} was null");

            var typeOld = oldObject.GetType();
            var typeNew = newObject.GetType();

            if (typeOld != typeNew)
                throw new InvalidCastException(
                    $"Parameters have different types. oldValue type : {typeOld}, newValue type : {typeNew}");
            var hashCodeOld = string.Empty;

            var propsOld = typeOld.GetProperties();

            foreach (var info in propsOld)
                hashCodeOld += oldObject.GetType().GetProperty(info.Name)?.GetValue(oldObject, null).ToString();

            var hashCodeNew = string.Empty;

            var propsNew = typeOld.GetProperties();

            foreach (var info in propsNew)
                hashCodeNew += newObject.GetType().GetProperty(info.Name)?.GetValue(newObject, null).ToString();

            return Equals(hashCodeOld, hashCodeNew);
        }
    }
}