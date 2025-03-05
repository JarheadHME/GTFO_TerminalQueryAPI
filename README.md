# TerminalQueryAPI

An API for plugin developers to make their custom level objects able to be Queryed on the terminals.

## Why?

The base game uses a `Func<List<string>, List<string>>` for when it's supposed to get query information. However, whenever the game tries to actually call this, it will crash with no error. So, this mod will override the query function, and if the query key has been registered, will instead use the provided delegate to get the info needed.

This plugin is heavily based off of Dinorush's [TerminalConsumables](https://github.com/Dinorush/TerminalConsumables/), in an effort to make querying available to any custom level objects.

# Usage

Attach a `iTerminalItem` class to your object (usually `LG_GenericTerminalItem`), run its `Setup` method to register it in the world (make sure its course node is set), then call `TerminalQueryAPI.QueryableAPI.RegisterQueryableItem()`. This function takes in two arguments, the first can either be your `LG_GenericTerminalItem`, the `iTerminalItem` interface, or just the terminal item's key (i.e what you type after the `QUERY` command). The second argument is a QueryDelegate, which takes in a `List<string>`, and returns a `List<string>`. 

The input list, named `defaultDetails`, is the game-obtained `defaultDetails` of the terminal object, which includes the following information:

```
----------------------------------------------------------------
ID: terminalItem.TerminalItemKey
ITEM STATUS: terminalItem.FloorItemStatus
LOCATION: terminalItem.FloorItemLocation
PING STATUS: ...
```

(`PING STATUS` will show the relevant text for if it's out of range, or in range to be pinged)

Each line is a different entry into the list, so you can filter out specific ones in your delegate if you choose. The list that is returned will output every entry to the terminal as its own line. 

If, while in the level, you wish the change the types of information that show up on the terminal, then you can freely change the delegate via `TerminalQueryAPI.QueryableAPI.ModifyQueryableItem()`, which accepts the same arguments as the Register method above, and will replace the stored delegate for that terminal item with the provided one.

If no delegate is provided, then when registered, the defaultDetails will be used.