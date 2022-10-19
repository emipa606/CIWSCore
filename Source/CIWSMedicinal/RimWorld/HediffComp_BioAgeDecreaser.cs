using System.Linq;
using Verse;

namespace RimWorld;

public class HediffComp_BioAgeDecreaser : HediffComp
{
    private float adultAge = -1f;
    private int ticks;

    public HediffCompProperties_BioAgeDecreaser Props => (HediffCompProperties_BioAgeDecreaser)props;

    public override void CompPostMake()
    {
        base.CompPostMake();
        adultAge = Pawn.def.race.lifeStageAges.Last().minAge;
    }

    public override void CompPostTick(ref float severityAdjustment)
    {
        base.CompPostTick(ref severityAdjustment);
        ticks++;
        if (ticks < 250)
        {
            return;
        }

        ticks = 0;
        if (Pawn.ageTracker.AgeBiologicalYearsFloat > adultAge + 0.05f)
        {
            Pawn.ageTracker.AgeBiologicalTicks -= 60000L;
        }
    }

    public override void CompExposeData()
    {
        base.CompExposeData();
        Scribe_Values.Look(ref adultAge, "adultAge");
        if (Scribe.mode == LoadSaveMode.PostLoadInit && adultAge < 0f)
        {
            adultAge = Pawn.def.race.lifeStageAges.Last().minAge;
        }
    }
}