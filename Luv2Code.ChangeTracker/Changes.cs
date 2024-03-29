using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Luv2Code.ChangeTracker
{
    /// <summary>
    ///     Is used to track changes in a ChangeTrackerCollection whether it was added, updated or removed
    /// </summary>
    public readonly struct Changes : IChanges
    {
        /// <summary>
        ///     Standard constructor
        /// </summary>
        /// <param name="changeObject">Object which should be tracked</param>
        /// <param name="changeIdentifier">Modifier - Add, Update, Remove</param>
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
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
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
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
        public List<Changes> Add<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class
        {
            if (logger is null) throw new NullReferenceException("ILogger instance was null");
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
        ///     Add entity to changes list if entity does not exists.
        ///     If entity exists, changesList will be returned
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <param name="compareObject">Any class entity</param>
        /// <typeparam name="T">Represents a class</typeparam>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
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

        /// <summary>
        ///     Add entity to changes list if entity does not exists.
        ///     If entity exists, changesList will be returned
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <param name="compareObject">Any class entity</param>
        /// <param name="logger">Ilogger instance</param>
        /// <typeparam name="T">Represents a class</typeparam>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
        public List<Changes> Update<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class
        {
            if (logger is null) throw new NullReferenceException("Logger was null");
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

            var item = new Changes(compareObject, ChangeIdentifier.Update);
            logger.LogDebug("New item was created. ChangeIdentifier.Update");
            var exist = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            logger.LogDebug("Check item existence against ChangesList.");
            if (exist)
            {
                logger.LogDebug("Item was found.");
                logger.LogInformation("Changes list returned");
                return changesList;
            }

            changesList.Add(item);
            logger.LogDebug("Item was successfully added to the changes list");
            logger.LogInformation("Updated list returned");
            return changesList;
        }

        /// <summary>
        ///     Removes entity from changesList if exists.
        ///     If entity does not exist, adds entity to the ChangesList with modifier: Remove
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <param name="compareObject">Any class entity</param>
        /// <typeparam name="T">Represents a class</typeparam>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
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

        /// <summary>
        ///     Removes entity from changesList if exists.
        ///     If entity does not exist, adds entity to the ChangesList with modifier: Remove
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <param name="compareObject">Any class entity</param>
        /// <param name="logger">Ilogger instance</param>
        /// <typeparam name="T">Represents a class</typeparam>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
        public List<Changes> Remove<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class
        {
            if (logger is null) throw new NullReferenceException("Logger was null");
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

            var item = new Changes(compareObject, ChangeIdentifier.Remove);
            logger.LogDebug("New item was created. ChangeIdentifier.Remove");

            var exist = changesList.Any(c => c.ChangeObject.Equals(compareObject));
            logger.LogDebug("Check item existence against ChangesList.");
            if (exist)
            {
                logger.LogDebug("Item was found.");
                var deletedItem = changesList.FirstOrDefault(c => c.ChangeObject.Equals(compareObject));
                changesList.Remove(deletedItem);
                logger.LogDebug("Item was removed from the list");
            }
            else
            {
                changesList.Add(item);
                logger.LogDebug("Item was successfully added to the changes list");
            }

            logger.LogInformation("Updated list returned");
            return changesList;
        }

        /// <summary>
        ///     Check if ChangesList has changes
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
        public bool ChangesListHasChanges(List<Changes> changesList)
        {
            if (changesList is null) throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            return changesList.Count > 0;
        }

        /// <summary>
        ///     Check if ChangesList has changes
        /// </summary>
        /// <param name="changesList">List, where all the changes a saved</param>
        /// <param name="logger">Ilogger instance</param>
        /// <returns>List of changes</returns>
        /// <exception cref="NullReferenceException">Will be thrown if changesList or compareObject are null</exception>
        public bool ChangesListHasChanges(List<Changes> changesList, ILogger logger)
        {
            if (logger is null) throw new NullReferenceException("Logger was null");
            if (changesList is null)
            {
                logger.LogError($"Parameter {nameof(changesList)} was null");
                throw new NullReferenceException($"Parameter {nameof(changesList)} was null");
            }

            return changesList.Count > 0;
        }

        /// <summary>
        ///     Compare old object with new object. Comparison is on the property level.
        ///     Each property will be compared.
        /// </summary>
        /// <param name="oldObject">Source object</param>
        /// <param name="newObject">Modified object</param>
        /// <typeparam name="T">Can be any POCO class, has to be a reference type</typeparam>
        /// <returns>true or false</returns>
        /// <exception cref="NullReferenceException">Will be thrown if oldObject or newObject are null</exception>
        /// <exception cref="InvalidCastException">Will be thrown if oldObject or newObject have different types</exception>
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

        /// <summary>
        ///     Compare old object with new object. Comparison is on the property level.
        ///     Each property will be compared.
        /// </summary>
        /// <param name="oldObject">Source object</param>
        /// <param name="newObject">Modified object</param>
        /// <param name="logger">Ilogger instance</param>
        /// <typeparam name="T">Can be any POCO class, has to be a reference type</typeparam>
        /// <returns>true or false</returns>
        /// <exception cref="NullReferenceException">Will be thrown if oldObject or newObject are null</exception>
        /// <exception cref="InvalidCastException">Will be thrown if oldObject or newObject have different types</exception>
        public bool ObjectHasChanges<T>(T oldObject, T newObject, ILogger logger) where T : class
        {
            if (logger is null) throw new NullReferenceException("Logger was null");
            if (oldObject is null)
            {
                logger.LogError($"Parameter {nameof(oldObject)} was null");
                throw new NullReferenceException($"Parameter {nameof(oldObject)} was null");
            }

            if (newObject is null)
            {
                logger.LogError($"Parameter {nameof(newObject)} was null");
                throw new NullReferenceException($"Parameter {nameof(newObject)} was null");
            }

            var typeOld = oldObject.GetType();
            logger.LogDebug($"Type of parameter {nameof(typeOld)}: {typeOld.Name}");
            var typeNew = newObject.GetType();
            logger.LogDebug($"Type of parameter {nameof(typeNew)}: {typeNew.Name}");
            if (typeOld != typeNew)
            {
                logger.LogError(
                    $"Parameters have different types. oldValue type : {typeOld}, newValue type : {typeNew}");
                throw new InvalidCastException(
                    $"Parameters have different types. oldValue type : {typeOld}, newValue type : {typeNew}");
            }

            var hashCodeOld = JsonConvert.SerializeObject(oldObject);
            logger.LogDebug($"Hashcode for {nameof(oldObject)} was generated");
            var hashCodeNew = JsonConvert.SerializeObject(newObject);
            logger.LogDebug($"Hashcode for {nameof(newObject)} was generated");

            logger.LogDebug($"Compare hashcode for {nameof(oldObject)} and {nameof(newObject)}");
            var result = Equals(hashCodeOld, hashCodeNew);
            logger.LogInformation($"Comparison is finished: {newObject} has changes: {result}");
            return !result;
        }
    }
}