using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager = TerminalQueryAPI.Modules.TerminalItemManager;

namespace TerminalQueryAPI;

// summaries and such heavily based on (and in some areas copied) from Dinorush/TerminalConsumables

/// <summary>
/// Returns the lines of text to print for a query command. Each element represents one line.
/// </summary>
/// <param name="defaultDetails">The default details provided by the interpreter, in the format
/// <code>
/// ----------------------------------------------------------------
/// ID: terminalItem.TerminalItemKey
/// ITEM STATUS: terminalItem.FloorItemStatus
/// LOCATION: terminalItem.FloorItemLocation
/// PING STATUS: ...</code>
/// </param>
/// <returns></returns>
public delegate List<string> QueryDelegate(List<string> defaultDetails);
public class QueryableAPI
{
    public const string PLUGIN_GUID = "JarheadHME.TerminalQueryAPI";

    #region Permanent Query Overrides
    /// <summary>
    /// Modifys the Query delegate registered for the provided Terminal Item Key. If the provided key is not currently registered for query, it will be registered.
    /// </summary>
    /// <param name="terminalKey">The Terminal Item to modify's key</param>
    /// <param name="queryDel">The delegate used to get the lines to print to the terminal</param>
    /// <returns>Returns false if the key isn't already registered, and registering fails</returns>
    public static bool ModifyQueryableItem(string terminalKey, QueryDelegate? queryDel = null)
    {
        if (Manager.QueryOverrides.ContainsKey(terminalKey))
        {
            Manager.QueryOverrides[terminalKey] = queryDel;
            return true;
        }

        // Terminal item not already registered
        if (RegisterQueryableItem(terminalKey, queryDel)) 
            return true;

        // Failed to register
        Logger.Error($"Failed to register terminal item `{terminalKey}`");
        return false;
    }
    /// <summary>
    /// Modifys the Query delegate registered for the provided Terminal Item. If the provided item is not currently registered for query, it will be registered.
    /// </summary>
    /// <param name="terminalItem">The Terminal Item to modify</param>
    /// <param name="queryDel">The delegate used to get the lines to print to the terminal</param>
    /// <returns>Returns false if the key isn't already registered, and registering fails</returns>
    public static bool ModifyQueryableItem(iTerminalItem terminalItem, QueryDelegate? queryDel = null) => ModifyQueryableItem(terminalItem.TerminalItemKey, queryDel);
    /// <summary>
    /// Modifys the Query delegate registered for the provided Terminal Item. If the provided item is not currently registered for query, it will be registered.
    /// </summary>
    /// <param name="terminalItem">The Terminal Item to modify</param>
    /// <param name="queryDel">The delegate used to get the lines to print to the terminal</param>
    /// <returns>Returns false if the key isn't already registered, and registering fails</returns>
    public static bool ModifyQueryableItem(LG_GenericTerminalItem terminalItem, QueryDelegate? queryDel = null) => ModifyQueryableItem(terminalItem.TerminalItemKey, queryDel);

    /// <summary>
    /// Registers the provided Terminal Item Key to Query properly, using the provided QueryDelegate
    /// </summary>
    /// <param name="terminalKey">The Terminal Item Key to register for querying</param>
    /// <param name="queryDel">The delegate to use to get the Query text</param>
    /// <returns>Returns true if the key is successfully registered, false otherwise</returns>
    public static bool RegisterQueryableItem(string terminalKey, QueryDelegate? queryDel = null)
    {
        if (string.IsNullOrEmpty(terminalKey))
        {
            Logger.Error("Tried to register a terminal item but the item's key was null or empty!");
            return false;
        }

        if (Manager.QueryOverrides.ContainsKey(terminalKey))
        {
            Logger.Error($"Tried to register queryable terminal item `{terminalKey}`, but an item with that key was already registered!");
            return false;
        }

        if (!Manager.QueryOverrides.TryAdd(terminalKey, queryDel))
        {
            // Just for safety, I believe it should only be false if the key's already present which was already checked for, but just in case.
            Logger.Error($"Tried registering queryable terminal item `{terminalKey}`, but failed for some reason?");
            return false;
        }

        return true;
    }
    /// <summary>
    /// Registers the provided Terminal Item to Query properly, using the provided QueryDelegate
    /// </summary>
    /// <param name="terminalItem">the Terminal Item to register for querying</param>
    /// <param name="queryDel">The delegate to use to get the Query text</param>
    /// <returns>Returns true if the item is successfully registered, false otherwise</returns>
    public static bool RegisterQueryableItem(iTerminalItem terminalItem, QueryDelegate? queryDel = null) => RegisterQueryableItem(terminalItem.TerminalItemKey, queryDel);
    /// <summary>
    /// Registers the provided Terminal Item to Query properly, using the provided QueryDelegate
    /// </summary>
    /// <param name="terminalItem">the Terminal Item to register for querying</param>
    /// <param name="queryDel">The delegate to use to get the Query text</param>
    /// <returns>Returns true if the item is successfully registered, false otherwise</returns>
    public static bool RegisterQueryableItem(LG_GenericTerminalItem terminalItem, QueryDelegate? queryDel = null) => RegisterQueryableItem(terminalItem.TerminalItemKey, queryDel);
    #endregion


    #region Temprary Query Overrides

    /// <summary>
    /// Modifys the Temporary Query delegate registered for the provided Terminal Item Key. If the provided key is not currently registered for query, it will be registered.
    /// </summary>
    /// <param name="terminalKey">The Terminal Item to modify's key</param>
    /// <param name="queryDel">The delegate used to get the lines to print to the terminal</param>
    /// <returns>Returns false if the key isn't already registered, and registering fails</returns>
    public static bool ModifyTempQueryableItem(string terminalKey, QueryDelegate? queryDel = null)
    {
        if (Manager.TemporaryQueryOverrides.ContainsKey(terminalKey))
        {
            Manager.TemporaryQueryOverrides[terminalKey] = queryDel;
            return true;
        }

        // Terminal item not already registered
        if (RegisterTempQueryableItem(terminalKey, queryDel))
            return true;

        // Failed to register
        Logger.Error($"Failed to register terminal item `{terminalKey}`");
        return false;
    }
    /// <summary>
    /// Modifys the Temporary Query delegate registered for the provided Terminal Item. If the provided item is not currently registered for query, it will be registered.
    /// </summary>
    /// <param name="terminalItem">The Terminal Item to modify</param>
    /// <param name="queryDel">The delegate used to get the lines to print to the terminal</param>
    /// <returns>Returns false if the key isn't already registered, and registering fails</returns>
    public static bool ModifyTempQueryableItem(iTerminalItem terminalItem, QueryDelegate? queryDel = null) => ModifyTempQueryableItem(terminalItem.TerminalItemKey, queryDel);
    /// <summary>
    /// Modifys the Temporary Query delegate registered for the provided Terminal Item. If the provided item is not currently registered for query, it will be registered.
    /// </summary>
    /// <param name="terminalItem">The Terminal Item to modify</param>
    /// <param name="queryDel">The delegate used to get the lines to print to the terminal</param>
    /// <returns>Returns false if the key isn't already registered, and registering fails</returns>
    public static bool ModifyTempQueryableItem(LG_GenericTerminalItem terminalItem, QueryDelegate? queryDel = null) => ModifyTempQueryableItem(terminalItem.TerminalItemKey, queryDel);

    /// <summary>
    /// Registers the provided Terminal Item Key to Query properly, using the provided QueryDelegate.
    /// This differs from the Standard registration in that it can be Deregistered, and will fallback to a previously set override if provided.
    /// This is for the purposes of temporarily overriding text for an item based on its current state.
    /// </summary>
    /// <param name="terminalKey">The Terminal Item Key to register for querying</param>
    /// <param name="queryDel">The delegate to use to get the Query text</param>
    /// <returns>Returns true if the key is successfully registered, false otherwise</returns>
    public static bool RegisterTempQueryableItem(string terminalKey, QueryDelegate? queryDel = null)
    {
        if (string.IsNullOrEmpty(terminalKey))
        {
            Logger.Error("Tried to register a terminal item but the item's key was null or empty!");
            return false;
        }

        if (Manager.TemporaryQueryOverrides.ContainsKey(terminalKey))
        {
            Logger.Error($"Tried to register queryable terminal item `{terminalKey}`, but an item with that key was already registered!");
            return false;
        }

        if (!Manager.TemporaryQueryOverrides.TryAdd(terminalKey, queryDel))
        {
            // Just for safety, I believe it should only be false if the key's already present which was already checked for, but just in case.
            Logger.Error($"Tried registering queryable terminal item `{terminalKey}`, but failed for some reason?");
            return false;
        }

        return true;
    }
    /// <summary>
    /// Registers the provided Terminal Item to Query properly, using the provided QueryDelegate.
    /// This differs from the Standard registration in that it can be Deregistered, and will fallback to a previously set override if provided.
    /// This is for the purposes of temporarily overriding text for an item based on its current state.
    /// </summary>
    /// <param name="terminalItem">the Terminal Item to register for querying</param>
    /// <param name="queryDel">The delegate to use to get the Query text</param>
    /// <returns>Returns true if the item is successfully registered, false otherwise</returns>
    public static bool RegisterTempQueryableItem(iTerminalItem terminalItem, QueryDelegate? queryDel = null) => RegisterTempQueryableItem(terminalItem.TerminalItemKey, queryDel);
    /// <summary>
    /// Registers the provided Terminal Item to Query properly, using the provided QueryDelegate.
    /// This differs from the Standard registration in that it can be Deregistered, and will fallback to a previously set override if provided.
    /// This is for the purposes of temporarily overriding text for an item based on its current state.
    /// </summary>
    /// <param name="terminalItem">the Terminal Item to register for querying</param>
    /// <param name="queryDel">The delegate to use to get the Query text</param>
    /// <returns>Returns true if the item is successfully registered, false otherwise</returns>
    public static bool RegisterTempQueryableItem(LG_GenericTerminalItem terminalItem, QueryDelegate? queryDel = null) => RegisterTempQueryableItem(terminalItem.TerminalItemKey, queryDel);

    /// <summary>
    /// Deregisters the provided Terminal Item from the Temporary Query Override list
    /// </summary>
    /// <param name="terminalItem">the Terminal Item Key to deregister for query overriding</param>
    /// <returns>Returns true if the item is successfully deregistered, false otherwise. The item not being currently registered returns false.</returns>
    public static bool DeregisterTempQueryableItem(string terminalKey)
    {
        if (string.IsNullOrEmpty(terminalKey))
        {
            Logger.Error("Tried to deregister a terminal item but the item's key was null or empty!");
            return false;
        }

        if (!Manager.TemporaryQueryOverrides.ContainsKey(terminalKey))
        {
            return false;
        }

        return Manager.TemporaryQueryOverrides.Remove(terminalKey);
    }
    /// <summary>
    /// Deregisters the provided Terminal Item from the Temporary Query Override list
    /// </summary>
    /// <param name="terminalItem">the Terminal Item to deregister for query overriding</param>
    /// <returns>Returns true if the item is successfully deregistered, false otherwise. The item not being currently registered returns false.</returns>
    public static bool DeregisterTempQueryableItem(iTerminalItem terminalItem) => DeregisterTempQueryableItem(terminalItem.TerminalItemKey);
    /// <summary>
    /// Deregisters the provided Terminal Item from the Temporary Query Override list
    /// </summary>
    /// <param name="terminalItem">the Terminal Item to deregister for query overriding</param>
    /// <returns>Returns true if the item is successfully deregistered, false otherwise. The item not being currently registered returns false.</returns>
    public static bool DeregisterTempQueryableItem(LG_GenericTerminalItem terminalItem) => DeregisterTempQueryableItem(terminalItem.TerminalItemKey);

    #endregion
}
