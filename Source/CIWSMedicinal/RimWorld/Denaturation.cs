using RimWorld;
using Verse;

namespace CIWSMedicinal;

public class Denaturation : CompUseEffect
{
    public override void DoEffect(Pawn usedBy)
    {
        switch (usedBy.gender)
        {
            case Gender.Male:
                usedBy.gender = Gender.Female;
                refreshH(usedBy);
                Messages.Message("美丽的女性！", MessageTypeDefOf.PositiveEvent);
                break;
            case Gender.Female:
                usedBy.gender = Gender.Male;
                refreshH(usedBy);
                Messages.Message("强壮的男性！", MessageTypeDefOf.PositiveEvent);
                break;
            default:
                Messages.Message("无效！！！", MessageTypeDefOf.PositiveEvent);
                break;
        }
    }

    private static void refreshH(Pawn pawn)
    {
        if (!pawn.RaceProps.Humanlike)
        {
            return;
        }

        var headType = "_AverageNormal";
        if (Rand.Bool)
        {
            headType = "_NarrowNormal";
        }

        pawn.story.headType =
            DefDatabase<HeadTypeDef>.GetNamed(pawn.gender == Gender.Male ? $"Male{headType}" : $"Female{headType}");
        pawn.story.HairColor =
            PawnHairColors.RandomHairColor(pawn, pawn.story.SkinColor, pawn.ageTracker.AgeBiologicalYears);
        pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(pawn);
        pawn.story.bodyType = pawn.gender != Gender.Female ? BodyTypeDefOf.Male : BodyTypeDefOf.Female;
        pawn.Drawer.renderer.SetAllGraphicsDirty();
    }
}