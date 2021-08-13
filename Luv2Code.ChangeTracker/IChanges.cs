using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Luv2Code.ChangeTracker
{
    public interface IChanges
    {
        object ChangeObject { get; }
        ChangeIdentifier ChangeIdentifier { get; }

        /// <summary>
        ///     Add entity of type T to changesList if not exists.
        ///     If entity exist, the modifier will be checked. If modifier equals 'Remove', entity will be removed from list
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<Changes> Add<T>(List<Changes> changesList, T compareObject) where T : class;

        /// <summary>
        ///     Add entity of type T to changesList if not exists.
        ///     If entity exist, the modifier will be checked. If modifier equals 'Remove', entity will be removed from list
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <param name="logger"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<Changes> Add<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class;

        /// <summary>
        ///     Add identity to changes list if not exists. If entity exists, return changesList
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<Changes> Update<T>(List<Changes> changesList, T compareObject) where T : class;

        /// <summary>
        ///     Add identity to changes list if not exists. If entity exists, return changesList
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <param name="logger"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<Changes> Update<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class;

        /// <summary>
        ///     Removes entity of type T from changesList if exists.
        ///     If entity dont exist, adds entity of type T to tracking list with modifier: Remove
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>ChangesList with actual values</returns>
        List<Changes> Remove<T>(List<Changes> changesList, T compareObject) where T : class;

        /// <summary>
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="compareObject"></param>
        /// <param name="logger"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<Changes> Remove<T>(List<Changes> changesList, T compareObject, ILogger logger) where T : class;

        /// <summary>
        ///     Check if changes list has changes, return true or false
        /// </summary>
        /// <param name="changesList"></param>
        /// <returns></returns>
        bool ChangesListHasChanges(List<Changes> changesList);

        /// <summary>
        ///     Check if changes list has changes, return true or false
        /// </summary>
        /// <param name="changesList"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        bool ChangesListHasChanges(List<Changes> changesList, ILogger logger);

        /// <summary>
        ///     Check if an entity has some property values changed
        /// </summary>
        /// <param name="oldObject"></param>
        /// <param name="newObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool ObjectHasChanges<T>(T oldObject, T newObject) where T : class;

        /// <summary>
        ///     Check if an entity has some property values changed
        /// </summary>
        /// <param name="oldObject"></param>
        /// <param name="newObject"></param>
        /// <param name="logger"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool ObjectHasChanges<T>(T oldObject, T newObject, ILogger logger) where T : class;
    }
}