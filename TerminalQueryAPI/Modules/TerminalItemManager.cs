using LevelGeneration;
using System;
using System.Collections.Generic;
using GTFO.API.Extensions;

namespace TerminalQueryAPI.Modules;
internal static class TerminalItemManager
{
    internal static Dictionary<string, QueryDelegate> QueryOverrides = new Dictionary<string, QueryDelegate>();
    internal static Dictionary<string, QueryDelegate> TemporaryQueryOverrides = new Dictionary<string, QueryDelegate>();

    // basically ripped from TerminalConsumables.TerminalItemManager by Dinorush
    internal static bool TryDoQueryOutput(iTerminalItem terminalItem, LG_ComputerTerminalCommandInterpreter interpreter)
    {
        string itemKey = terminalItem.TerminalItemKey;

        // Not a registered query override
        QueryDelegate? queryDelegate;
        if (!TemporaryQueryOverrides.TryGetValue(itemKey, out queryDelegate) && !QueryOverrides.TryGetValue(itemKey, out queryDelegate))
            return false;

        string pingStatus = interpreter.GetPingStatus(terminalItem);
        var details = interpreter.GetDefaultDetails(terminalItem, pingStatus);
        List<string> defaultDetails = [.. details];
        defaultDetails = FixDefaultDetails(defaultDetails);

        List<string> output = new();

        if (queryDelegate != null)
            output = queryDelegate.Invoke(defaultDetails);
        else
            output = defaultDetails;

        interpreter.AddOutput(TerminalLineType.SpinningWaitDone, "Querying " + itemKey, 3f);
        interpreter.AddOutput(output.ToIl2Cpp());
        // `AddOutput(List<string>)` automatically adds the spacing lines above and below

        return true;
    }

    public static List<string> FixDefaultDetails(List<string> defaultDetails)
    {
        List<string> outlist = new List<string>(5);
        outlist.Add(defaultDetails[0]); // --------------------

        outlist.AddRange(defaultDetails[1].Split('\n'));

        return outlist;
    }

    public static void Clear()
    {
        QueryOverrides.Clear();
        TemporaryQueryOverrides.Clear();
    }
}
