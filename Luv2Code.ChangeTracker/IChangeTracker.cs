using System.Collections.Generic;

namespace Luv2Code.ChangeTracker
{
    public interface IChangeTracker
    {
        object ChangeObject { get; }
        ChangeIdentifier ChangeIdentifier { get; }

        /// <summary>
        ///     Add entity of type T to changesList if not exists.
        ///     If entity exist, the modifier will be check. If modifier is Delete, entity will be removed from list
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        List<ChangeTracker> Add<T>(List<ChangeTracker> changesList, T compareObject) where T : class;

        /// <summary>
        ///     Add identity to changes list if not exists. If entity exists, return changesList
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        List<ChangeTracker> Update<T>(List<ChangeTracker> changesList, T compareObject) where T : class;

        /// <summary>
        ///     Removes entity of type T from changesList if exists.
        ///     If entity dont exist, adds entity of type T to tracking list with modifier: Remove
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>ChangesList with actual values</returns>
        /// <exception cref="NullReferenceException"></exception>
        List<ChangeTracker> Remove<T>(List<ChangeTracker> changesList, T compareObject) where T : class;

        /// <summary>
        ///     Check if changes list has changes, return true or false
        /// </summary>
        /// <param name="changesList"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        bool ChangesListHasChanges(List<ChangeTracker> changesList);

        /// <summary>
        ///     Check if an entity has some property values changed
        /// </summary>
        /// <param name="oldObject"></param>
        /// <param name="newObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        bool ObjectHasChanges<T>(T oldObject, T newObject) where T : class;
    }
}