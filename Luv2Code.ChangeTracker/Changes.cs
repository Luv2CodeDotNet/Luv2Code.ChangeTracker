using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Luv2Code.ChangeTracker
{
    /// <summary>
    ///     Is used to track changes in a ChangeTrackerCollection whether it was added, updated or deleted
    /// </summary>
    public readonly struct Changes : IChanges
    {
        public Changes(object changeObject, ChangeIdentifier changeIdentifier)
        {
            ChangeObject = changeObject;
            ChangeIdentifier = changeIdentifier;
        }

        /// <summary>
        ///     Object which should be tracked
        /// </summary>
        public object ChangeObject { get; }

        /// <summary>
        ///     Identifier to operations: Add, Update, Remove
        /// </summary>
        public ChangeIdentifier ChangeIdentifier { get; }

        /// <summary>
        ///     Add entity of type T to changesList if not exists.
        ///     If entity exist, the modifier will be checked. If modifier equals 'Remove', entity will be removed from list.
        ///     The updated list will be returned.
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <param name="compareObject">Any class entity</param>
        /// <typeparam name="T">Represents a class</typeparam>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will thrown if changesList or compareObject are null</exception>
        public List<Changes> Add<T>(List<Changes> changesList, T compareObject) where T : class
        {
            // Check if changesList is null
            if (changesList is null)
                // Throw NullReferenceException if changesList is null
                throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            // Check if compareObject is null
            if (compareObject is null)
                // Throw NullReferenceException if compareObject is null
                throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");
            // create new Changes item
            var item = new Changes(compareObject, ChangeIdentifier.Add);
            // check if item exists in the changesList
            var exists = changesList.Any(c => c.ChangeObject.Equals(compareObject));

            if (exists)
            {
                // get the identifier for the item
                var identifier = changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject)).ChangeIdentifier;
                // if item exists, need to check previous interaction.
                // If identifier equals 'Remove', that means, than the item was previously removed and now should be added again.
                // This action neutralizes the previous one. 
                if (identifier == ChangeIdentifier.Remove)
                    changesList.Remove(changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject)));
                // return updated changesList
                return changesList;
            }

            // add item to the changesList
            changesList.Add(item);
            // return updated changesList
            return changesList;
        }

        /// <summary>
        ///     Add entity of type T to changesList if not exists.
        ///     If entity exist, the modifier will be checked. If modifier equals 'Remove', entity will be removed from list.
        ///     The updated list will be returned.
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <param name="compareObject">Any class entity</param>
        /// <param name="logger">Ilogger instance</param>
        /// <typeparam name="T">Represents a class</typeparam>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will thrown if changesList or compareObject are null</exception>
        public List<Changes> Add<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class
        {
            if (changesList is null)
            {
                logger.LogError($"Parameter {nameof(changesList)} was null");
                throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            }

            if (compareObject is null)
            {
                logger.LogError($"Parameter {nameof(compareObject)} was null");
                throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");
            }

            var item = new Changes(compareObject, ChangeIdentifier.Add);
            logger.LogDebug("New item was created. ChangeIdentifier.Add");

            var exists = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            logger.LogDebug("Check item existence against ChangesList.");

            if (exists)
            {
                logger.LogDebug("Item was found.");
                var identifier = changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject)).ChangeIdentifier;
                logger.LogDebug("Check items identifier.");
                if (identifier == ChangeIdentifier.Remove)
                {
                    logger.LogDebug("Identifier equals 'Remove' - remove item from List");
                    changesList.Remove(changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject)));
                }

                logger.LogInformation("Item was successfully removed. Updated list returned");
                return changesList;
            }

            changesList.Add(item);
            logger.LogDebug("Item was successfully added to the changes list");
            logger.LogInformation("Updated list returned");
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
        public List<Changes> Update<T>(List<Changes> changesList, T compareObject) where T : class
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            if (compareObject is null) throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");

            var item = new Changes(compareObject, ChangeIdentifier.Update);
            var exist = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            if (exist)
                return changesList;
            changesList.Add(item);
            return changesList;
        }

        public List<Changes> Update<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class
        {
            throw new NotImplementedException();
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
        public List<Changes> Remove<T>(List<Changes> changesList, T compareObject) where T : class
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            if (compareObject is null) throw new NullReferenceException($"Parameter {nameof(compareObject)} was null");

            var item = new Changes(compareObject, ChangeIdentifier.Remove);

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

        public List<Changes> Remove<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Check if changes list has changes, return true or false
        /// </summary>
        /// <param name="changesList"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool ChangesListHasChanges(List<Changes> changesList)
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            return changesList.Count > 0;
        }

        public bool ChangesListHasChanges(List<Changes> changesList, ILogger logger)
        {
            throw new NotImplementedException();
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

            var hashCodeOld = JsonConvert.SerializeObject(oldObject);
            var hashCodeNew = JsonConvert.SerializeObject(newObject);

            var result = Equals(hashCodeOld, hashCodeNew);
            return !result;
        }

        public bool ObjectHasChanges<T>(T oldObject, T newObject, ILogger logger) where T : class
        {
            throw new NotImplementedException();
        }
    }
}