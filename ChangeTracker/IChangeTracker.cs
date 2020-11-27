using System.Collections.Generic;

namespace ChangeTracker
{
    public interface IChangeTracker
    { 
        object ChangeObject { get; }
        ChangeIdentifier ChangeIdentifier { get; }
        List<ChangeTracker> Add<T>(List<ChangeTracker> changesList, T compareObject) where T : class;
        List<ChangeTracker> Update<T>(List<ChangeTracker> changesList, T compareObject) where T : class;
        List<ChangeTracker> Remove<T>(List<ChangeTracker> changesList, T task) where T : class;
    }
}