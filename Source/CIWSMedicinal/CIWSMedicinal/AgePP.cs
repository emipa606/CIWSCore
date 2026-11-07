using RimWorld;
using Verse;

namespace CIWSMedicinal;

public class AgePP : CompUseEffect
{
    public override void DoEffect(Pawn usedBy)
    {
        var num = (long)(usedBy.def.race.lifeStageAges[^1].minAge * 3600000f);
        usedBy.ageTracker.AgeBiologicalTicks = num;
        var num2 = num / 3600000.0;
        Messages.Message($"年龄已成功调整到{num2}岁", MessageTypeDefOf.PositiveEvent);
    }
}