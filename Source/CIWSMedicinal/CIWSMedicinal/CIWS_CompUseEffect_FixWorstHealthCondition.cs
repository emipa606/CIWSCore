using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace CIWSMedicinal
{
    public class CIWS_CompUseEffect_FixWorstHealthCondition : CompUseEffect_FixWorstHealthCondition
    {
        private float HandCoverageAbsWithChildren => ThingDefOf.Human.race.body.GetPartsWithDef(BodyPartDefOf.Hand)
            .First().coverageAbsWithChildren;

        public override void DoEffect(Pawn usedBy)
        {
            for (var i = 0; i < 5; i++)
            {
                base.DoEffect(usedBy);
                var hediff = FindLifeThreateningHediff(usedBy);
                if (hediff != null)
                {
                    Cure(hediff);
                }

                if (HealthUtility.TicksUntilDeathDueToBloodLoss(usedBy) < 2500)
                {
                    var hediff2 = FindMostBleedingHediff(usedBy);
                    if (hediff2 != null)
                    {
                        Cure(hediff2);
                    }
                }

                if (usedBy.health.hediffSet.GetBrain() != null)
                {
                    var hediff_Injury =
                        FindPermanentInjury(usedBy, Gen.YieldSingle(usedBy.health.hediffSet.GetBrain()));
                    if (hediff_Injury != null)
                    {
                        Cure(hediff_Injury);
                    }
                }

                var bodyPartRecord = FindBiggestMissingBodyPart(usedBy, HandCoverageAbsWithChildren);
                if (bodyPartRecord != null)
                {
                    Cure(bodyPartRecord, usedBy);
                }

                var hediff_Injury2 = FindPermanentInjury(usedBy, from x in usedBy.health.hediffSet.GetNotMissingParts()
                    where x.def == BodyPartDefOf.Eye
                    select x);
                if (hediff_Injury2 != null)
                {
                    Cure(hediff_Injury2);
                }

                var hediff3 = FindImmunizableHediffWhichCanKill(usedBy);
                if (hediff3 != null)
                {
                    Cure(hediff3);
                }

                var hediff4 = FindNonInjuryMiscBadHediff(usedBy, true);
                if (hediff4 != null)
                {
                    Cure(hediff4);
                }

                var hediff5 = FindNonInjuryMiscBadHediff(usedBy, false);
                if (hediff5 != null)
                {
                    Cure(hediff5);
                }

                if (usedBy.health.hediffSet.GetBrain() != null)
                {
                    var hediff_Injury3 = FindInjury(usedBy, Gen.YieldSingle(usedBy.health.hediffSet.GetBrain()));
                    if (hediff_Injury3 != null)
                    {
                        Cure(hediff_Injury3);
                    }
                }

                var bodyPartRecord2 = FindBiggestMissingBodyPart(usedBy);
                if (bodyPartRecord2 != null)
                {
                    Cure(bodyPartRecord2, usedBy);
                }

                var hediff_Addiction = FindAddiction(usedBy);
                if (hediff_Addiction != null)
                {
                    Cure(hediff_Addiction);
                }

                var hediff_Injury4 = FindPermanentInjury(usedBy);
                if (hediff_Injury4 != null)
                {
                    Cure(hediff_Injury4);
                }

                var hediff_Injury5 = FindInjury(usedBy);
                if (hediff_Injury5 != null)
                {
                    Cure(hediff_Injury5);
                }
            }
        }

        private Hediff FindLifeThreateningHediff(Pawn pawn)
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

        private Hediff FindMostBleedingHediff(Pawn pawn)
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
                    !CanEverKill(hediff1))
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

        private Hediff FindNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill)
        {
            Hediff hediff = null;
            var num = -1f;
            var hediffs = pawn.health.hediffSet.hediffs;
            foreach (var hediff1 in hediffs)
            {
                if (!hediff1.Visible || !hediff1.def.isBad || !hediff1.def.everCurableByItem ||
                    hediff1 is Hediff_Injury || hediff1 is Hediff_MissingPart || hediff1 is Hediff_Addiction ||
                    hediff1 is Hediff_AddedPart || onlyIfCanKill && !CanEverKill(hediff1))
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

        private BodyPartRecord FindBiggestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
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

        private Hediff_Addiction FindAddiction(Pawn pawn)
        {
            var hediffs = pawn.health.hediffSet.hediffs;
            foreach (var hediff in hediffs)
            {
                if (hediff is Hediff_Addiction { Visible: true } hediff_Addiction &&
                    hediff_Addiction.def.everCurableByItem)
                {
                    return hediff_Addiction;
                }
            }

            return null;
        }

        private Hediff_Injury FindPermanentInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
        {
            Hediff_Injury hediff_Injury = null;
            var hediffs = pawn.health.hediffSet.hediffs;
            foreach (var hediff in hediffs)
            {
                if (hediff is Hediff_Injury { Visible: true } hediff_Injury2 && hediff_Injury2.IsPermanent() &&
                    hediff_Injury2.def.everCurableByItem &&
                    (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) &&
                    (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
                {
                    hediff_Injury = hediff_Injury2;
                }
            }

            return hediff_Injury;
        }

        private Hediff_Injury FindInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
        {
            Hediff_Injury hediff_Injury = null;
            var hediffs = pawn.health.hediffSet.hediffs;
            foreach (var hediff in hediffs)
            {
                if (hediff is Hediff_Injury { Visible: true } hediff_Injury2 && hediff_Injury2.def.everCurableByItem &&
                    (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) &&
                    (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
                {
                    hediff_Injury = hediff_Injury2;
                }
            }

            return hediff_Injury;
        }

        private void Cure(Hediff hediff)
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

        private void Cure(BodyPartRecord part, Pawn pawn)
        {
            pawn.health.RestorePart(part);
            Messages.Message("MessageBodyPartCuredByItem".Translate(part.LabelCap), pawn,
                MessageTypeDefOf.PositiveEvent);
        }

        private bool CanEverKill(Hediff hediff)
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
}