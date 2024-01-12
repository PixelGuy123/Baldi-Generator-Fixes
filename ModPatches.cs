using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace BBGenFixes.Patches
{
	[HarmonyPatch(typeof(LevelGenerator))]
	internal class FixGenIssues
	{

		[HarmonyPatch("Generate", MethodType.Enumerator)]
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> AddingNullsForMysteryRoom(IEnumerable<CodeInstruction> instructions) =>
			new CodeMatcher(instructions)
			.MatchForward(true,
				new CodeMatch(OpCodes.Ldloc_2),
				new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(LevelBuilder), "ec")),
				new CodeMatch(OpCodes.Ldc_I4_S, name:"16")) // WHY DOES IT HAVE TO BE A STRING?? I CAN'T USE 16 AS OPERAND
			.SetInstruction(new(OpCodes.Ldc_I4_0)) // Set to 0 because it is for some reason 16

			.MatchForward(false,
				new CodeMatch(OpCodes.Stfld, name: "<roomCount>"), // Had to be more specific, omfg
				new CodeMatch(OpCodes.Ldloc_2),
				new CodeMatch(OpCodes.Ldc_I4_1),
				new CodeMatch(OpCodes.Call, AccessTools.Method("LevelBuilder:UpdatePotentialRoomSpawns"))) // Goes to a specific line
			.Insert(Transpilers.EmitDelegate(() =>
			{
				int amount = 0;
				Object.FindObjectsOfType<MysteryRoom>().Do(_ => amount++);
				return amount;
			}),
			new CodeInstruction(OpCodes.Add)) // This Add to add to the variable duhs
			.InstructionEnumeration();

	}

	[HarmonyPatch(typeof(EnvironmentController), "Awake")]
	internal class FixInvalidConstBin
	{
		private static void Prefix(ref TileController[] ___tileControllerPre) // For some reason, 16th tile has a constbin of 0, Imma fixing that
		{
			if (___tileControllerPre.Length > 15) // May have 16th tile
				AccessTools.Field(typeof(TileController), "constBin").SetValue(___tileControllerPre[16], 16);
			
		}
	}
}
