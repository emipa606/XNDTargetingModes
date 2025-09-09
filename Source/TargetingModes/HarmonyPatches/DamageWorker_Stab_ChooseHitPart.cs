using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace TargetingModes;

[HarmonyPatch(typeof(DamageWorker_Stab), "ChooseHitPart")]
public static class DamageWorker_Stab_ChooseHitPart
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionList = instructions.ToList();
        var done = false;
        var getRandomNotMissingPart = AccessTools.Method(typeof(HediffSet), nameof(HediffSet.GetRandomNotMissingPart));
        var resolvePrioritizedPart = AccessTools.Method(typeof(TargetingModesUtility),
            nameof(TargetingModesUtility.ResolvePrioritizedPart));
        foreach (var codeInstruction in instructionList)
        {
            var instruction = codeInstruction;
            if (!done && instruction.opcode == OpCodes.Callvirt &&
                (MethodInfo)instruction.operand == getRandomNotMissingPart)
            {
                yield return instruction;
                yield return new CodeInstruction(OpCodes.Stloc_0);
                yield return new CodeInstruction(OpCodes.Ldloc_0);
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                yield return new CodeInstruction(OpCodes.Ldarg_2);
                instruction = new CodeInstruction(OpCodes.Call, resolvePrioritizedPart);
                done = true;
            }

            yield return instruction;
        }
    }
}