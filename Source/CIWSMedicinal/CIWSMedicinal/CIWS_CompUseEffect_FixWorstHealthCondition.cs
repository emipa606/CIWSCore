using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace CIWSMedicinal;

public class CIWS_CompUseEffect_FixWorstHealthCondition : CompUseEffect_FixWorstHealthCondition
{
    private static float HandCoverageAbsWithChildren => ThingDefOf.Human.race.body.GetPartsWithDef(BodyPartDefOf.Hand)
        .First().coverageAbsWithChildren;

    public override void DoEffect(Pawn usedBy)
    {
        for (var i = 0; i < 5; i++)
        {
            base.DoEffect(usedBy);
            var hediff = findLifeThreateningHediff(usedBy);
            if (hediff != null)
            {
                cure(hediff);
            }

            if (HealthUtility.TicksUntilDeathDueToBloodLoss(usedBy) < 2500)
            {
                var hediff2 = findMostBleedingHediff(usedBy);
                if (hediff2 != null)
                {
                    cure(hediff2);
                }
            }

            if (usedBy.health.hediffSet.GetBrain() != null)
            {
                var hediffInjury =
                    findPermanentInjury(usedBy, Gen.YieldSingle(usedBy.health.hediffSet.GetBrain()));
                if (hediffInjury != null)
                {
                    cure(hediffInjury);
                }
            }

            var bodyPartRecord = findBiggestMissingBodyPart(usedBy, HandCoverageAbsWithChildren);
            if (bodyPartRecord != null)
            {
                cure(bodyPartRecord, usedBy);
            }

            var hediffInjury2 = findPermanentInjury(usedBy, from x in usedBy.health.hediffSet.GetNotMissingParts()
                where x.def == BodyPartDefOf.Eye
                select x);
            if (hediffInjury2 != null)
            {
                cure(hediffInjury2);
            }

            var hediff3 = FindImmunizableHediffWhichCanKill(usedBy);
            if (hediff3 != null)
            {
                cure(hediff3);
            }

            var hediff4 = findNonInjuryMiscBadHediff(usedBy, true);
            if (hediff4 != null)
            {
                cure(hediff4);
            }

            var hediff5 = findNonInjuryMiscBadHediff(usedBy, false);
            if (hediff5 != null)
            {
                cure(hediff5);
            }

            if (usedBy.health.hediffSet.GetBrain() != null)
            {
                var hediffInjury3 = findInjury(usedBy, Gen.YieldSingle(usedBy.health.hediffSet.GetBrain()));
                if (hediffInjury3 != null)
                {
                    cure(hediffInjury3);
                }
            }

            var bodyPartRecord2 = findBiggestMissingBodyPart(usedBy);
            if (bodyPartRecord2 != null)
            {
                cure(bodyPartRecord2, usedBy);
            }

            var hediffAddiction = findAddiction(usedBy);
            if (hediffAddiction != null)
            {
                cure(hediffAddiction);
            }

            var hediffInjury4 = findPermanentInjury(usedBy);
            if (hediffInjury4 != null)
            {
                cure(hediffInjury4);
            }

            var hediffInjury5 = findInjury(usedBy);
            if (hediffInjury5 != null)
            {
                cure(hediffInjury5);
            }
        }
    }

    private static Hediff findLifeThreateningHediff(Pawn pawn)
    {
        Hediff hediff = null;
        var num = -1f;
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff1 in hediffs)
        {
            if (!hediff1.Visible || !hediff1.def.everCurableByItem || hediff1.FullyImmune())
            {
                continue;
            }

            if (!(hediff1.CurStage?.lifeThreatening ?? false) && (!(hediff1.def.lethalSeverity >= 0f) ||
                                                                  !(hediff1.Severity /
                                                                      hediff1.def.lethalSeverity >= 0.8f)))
            {
                continue;
            }

            var num2 = hediff1.Part?.coverageAbsWithChildren ?? 999f;
            if (hediff != null && !(num2 > num))
            {
                continue;
            }

            hediff = hediff1;
            num = num2;
        }

        return hediff;
    }

    private static Hediff findMostBleedingHediff(Pawn pawn)
    {
        var num = 0f;
        Hediff hediff = null;
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff1 in hediffs)
        {
            if (!hediff1.Visible || !hediff1.def.everCurableByItem)
            {
                continue;
            }

            var bleedRate = hediff1.BleedRate;
            if (!(bleedRate > 0f) || !(bleedRate > num) && hediff != null)
            {
                continue;
            }

            num = bleedRate;
            hediff = hediff1;
        }

        return hediff;
    }

    private Hediff FindImmunizableHediffWhichCanKill(Pawn pawn)
    {
        Hediff hediff = null;
        var num = -1f;
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff1 in hediffs)
        {
            if (!hediff1.Visible || !hediff1.def.everCurableByItem ||
                hediff1.TryGetComp<HediffComp_Immunizable>() == null || hediff1.FullyImmune() ||
                !canEverKill(hediff1))
            {
                continue;
            }

            var severity = hediff1.Severity;
            if (hediff != null && !(severity > num))
            {
                continue;
            }

            hediff = hediff1;
            num = severity;
        }

        return hediff;
    }

    private Hediff findNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill)
    {
        Hediff hediff = null;
        var num = -1f;
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff1 in hediffs)
        {
            if (!hediff1.Visible || !hediff1.def.isBad || !hediff1.def.everCurableByItem ||
                hediff1 is Hediff_Injury || hediff1 is Hediff_MissingPart || hediff1 is Hediff_Addiction ||
                hediff1 is Hediff_AddedPart || onlyIfCanKill && !canEverKill(hediff1))
            {
                continue;
            }

            var num2 = hediff1.Part?.coverageAbsWithChildren ?? 999f;
            if (hediff != null && !(num2 > num))
            {
                continue;
            }

            hediff = hediff1;
            num = num2;
        }

        return hediff;
    }

    private static BodyPartRecord findBiggestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
    {
        BodyPartRecord bodyPartRecord = null;
        foreach (var missingPartsCommonAncestor in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
        {
            if (missingPartsCommonAncestor.Part.coverageAbsWithChildren >= minCoverage &&
                !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(missingPartsCommonAncestor.Part) &&
                (bodyPartRecord == null || missingPartsCommonAncestor.Part.coverageAbsWithChildren >
                    bodyPartRecord.coverageAbsWithChildren))
            {
                bodyPartRecord = missingPartsCommonAncestor.Part;
            }
        }

        return bodyPartRecord;
    }

    private static Hediff_Addiction findAddiction(Pawn pawn)
    {
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff in hediffs)
        {
            if (hediff is Hediff_Addiction { Visible: true } hediffAddiction &&
                hediffAddiction.def.everCurableByItem)
            {
                return hediffAddiction;
            }
        }

        return null;
    }

    private static Hediff_Injury findPermanentInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
    {
        Hediff_Injury hediffInjury = null;
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff in hediffs)
        {
            if (hediff is Hediff_Injury { Visible: true } hediffInjury2 && hediffInjury2.IsPermanent() &&
                hediffInjury2.def.everCurableByItem &&
                (allowedBodyParts == null || allowedBodyParts.Contains(hediffInjury2.Part)) &&
                (hediffInjury == null || hediffInjury2.Severity > hediffInjury.Severity))
            {
                hediffInjury = hediffInjury2;
            }
        }

        return hediffInjury;
    }

    private static Hediff_Injury findInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
    {
        Hediff_Injury hediffInjury = null;
        var hediffs = pawn.health.hediffSet.hediffs;
        foreach (var hediff in hediffs)
        {
            var bodyPartRecords = allowedBodyParts as BodyPartRecord[] ?? allowedBodyParts.ToArray();
            if (hediff is Hediff_Injury { Visible: true } hediffInjury2 && hediffInjury2.def.everCurableByItem &&
                bodyPartRecords.Contains(hediffInjury2.Part) &&
                (hediffInjury == null || hediffInjury2.Severity > hediffInjury.Severity))
            {
                hediffInjury = hediffInjury2;
            }
        }

        return hediffInjury;
    }

    private static void cure(Hediff hediff)
    {
        var pawn = hediff.pawn;
        pawn.health.RemoveHediff(hediff);
        if (hediff.def.cureAllAtOnceIfCuredByItem)
        {
            var num = 0;
            while (true)
            {
                num++;
                if (num <= 10000)
                {
                    var firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def);
                    if (firstHediffOfDef == null)
                    {
                        break;
                    }

                    pawn.health.RemoveHediff(firstHediffOfDef);
                    continue;
                }

                Log.Error("Too many iterations.");
                break;
            }
        }

        Messages.Message("MessageHediffCuredByItem".Translate(hediff.LabelBase.CapitalizeFirst()), pawn,
            MessageTypeDefOf.PositiveEvent);
    }

    private static void cure(BodyPartRecord part, Pawn pawn)
    {
        pawn.health.RestorePart(part);
        Messages.Message("MessageBodyPartCuredByItem".Translate(part.LabelCap), pawn,
            MessageTypeDefOf.PositiveEvent);
    }

    private static bool canEverKill(Hediff hediff)
    {
        if (hediff.def.stages == null)
        {
            return hediff.def.lethalSeverity >= 0f;
        }

        foreach (var hediffStage in hediff.def.stages)
        {
            if (hediffStage.lifeThreatening)
            {
                return true;
            }
        }

        return hediff.def.lethalSeverity >= 0f;
    }
}