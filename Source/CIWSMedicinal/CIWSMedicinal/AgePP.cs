using System.Linq;
using RimWorld;
using Verse;

namespace CIWSMedicinal;

public class AgePP : CompUseEffect
{
    public override void DoEffect(Pawn pawn)
    {
        var num = (long)(pawn.def.race.lifeStageAges.Last().minAge * 3600000f);
        pawn.ageTracker.AgeBiologicalTicks = num;
        var num2 = num / 3600000.0;
        Messages.Message($"年龄已成功调整到{num2}岁", MessageTypeDefOf.PositiveEvent);
    }
}