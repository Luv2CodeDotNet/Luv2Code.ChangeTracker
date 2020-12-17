# ChangeTracker

This library was build for situations where you need to track local changes, especially if you need to save or delete data in the database. Originally i needed to perform some calculation based on user selection (10 checkboxes) and save the output to the database.

## Methods
* Add
```csharp
    List<ChangeTracker> Add<T>(List<ChangeTracker> changesList, T compareObject) where T : class;
```

Add an item to changes list, pass <List<ChangeTracker>> and object of type T to the method to set object state as added.

* Update
```csharp
    List<ChangeTracker> Update<T>(List<ChangeTracker> changesList, T compareObject) where T : class;
```
Add an item to changes list, pass <List<ChangeTracker>> and object of type T to the method to set object state as updated.

* Remove
```csharp
    List<ChangeTracker> Remove<T>(List<ChangeTracker> changesList, T task) where T : class;
```
Add an item to changes list, pass <List<ChangeTracker>> and object of type T to the method to set object state as removed.

## ChangeIdentifier
There are three identifier at the moment. 
* Add
* Update
* Delete

## Examples

```csharp
    public class Foo
    {
        private readonly IChangeTracker _changeTracker;

        /// <summary>
        ///     Example class for using change tracker library
        /// </summary>
        /// <param name="changeTracker"></param>
        public Foo(IChangeTracker changeTracker)
        {
            _changeTracker = changeTracker;
            ChangesList = new List<ChangeTracker.ChangeTracker>();
        }

        /// <summary>
        ///     List will hold all the changes
        /// </summary>
        public List<ChangeTracker.ChangeTracker> ChangesList { get; set; }

        /// <summary>
        ///     Set object state to added in the changes list
        /// </summary>
        public void AddBar()
        {
            var changeObj = new Bar(1, "My track object");
            ChangesList = _changeTracker.Add(ChangesList, changeObj);
        }

        /// <summary>
        ///     Set object state to updated in the changes list
        /// </summary>
        public void UpdateBar()
        {
            var changeObj = new Bar(1, "My track object");
            ChangesList = _changeTracker.Update(ChangesList, changeObj);
        }

        /// <summary>
        ///     Set object state to deleted in the changes list
        /// </summary>
        public void RemoveBar()
        {
            var changeObj = new Bar(1, "My track object");
            ChangesList = _changeTracker.Remove(ChangesList, changeObj);
        }

        /// <summary>
        ///     Compare two complex object for any changes, return true if changes were detected
        /// </summary>
        public void CheckBarHasChanges()
        {
            var oldBar = new Bar(1, "My old bar");
            var newBar = new Bar(1, "My new bar");

            var returnValue = _changeTracker.ObjectHasChanges(oldBar, newBar);
        }

        /// <summary>
        ///     Check if changes list has any entities, returns true if count > 0
        /// </summary>
        public void CheckChangesListChanges()
        {
            var returnValue = _changeTracker.ChangesListHasChanges(ChangesList);
        }

        /// <summary>
        ///     You can access your tracked items by identifier and perform your operation
        /// </summary>
        public void AccessTrackedItems()
        {
            foreach (var changeTracker in ChangesList)
            {
                var added = changeTracker.ChangeIdentifier == ChangeIdentifier.Add;
                // do stuff
                var updated = changeTracker.ChangeIdentifier == ChangeIdentifier.Update;
                // do stuff
                var delete = changeTracker.ChangeIdentifier == ChangeIdentifier.Delete;
                // do stuff
            }
        }
    }

    public class Bar
    {
        public Bar(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
```

## Errors and Suggestions
If your find any errors or have a suggestion please open an issue.</br>

## Want help to improve?
Pull Requests are welcome. 
