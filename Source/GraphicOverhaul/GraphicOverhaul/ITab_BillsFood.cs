using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using System;


namespace GraphicOverhaul

{
    public class ITab_BillsFood : ITab
    {
        private float viewHeight = 1000f;

        private Vector2 scrollPosition = default(Vector2);

        private Bill mouseoverBill;

        private static readonly Vector2 WinSize = new Vector2(420f, 480f);

        private string billsOriginator;

        protected Building_WorkTable SelTable
        {
            get
            {
                return (Building_WorkTable)base.SelThing;
            }
        }

        public ITab_BillsFood()
        {
            this.size = ITab_BillsFood.WinSize;
            this.labelKey = "TabBills";
            this.tutorTag = "Bills";
            
        }

        protected override void FillTab()
        {
            if (SelTable.def.defName == "GO_FueledStove")
            {
                billsOriginator = "FueledStove";
            }
            else if (SelTable.def.defName == "GO_ElectricStove")
            {
                billsOriginator = "ElectricStove";
            }
            else if (SelTable.def.defName == "GO_TableButcher")
            {
                billsOriginator = "TableButcher";
            }
            else if (SelTable.def.defName == "GO_TableBrewery")
            {
                billsOriginator = "Brewery";
            }
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BillsTab, KnowledgeAmount.FrameDisplayed);
            Rect rect2 = new Rect(0f, 0f, ITab_BillsFood.WinSize.x, ITab_BillsFood.WinSize.y).ContractedBy(10f);
            Func<List<FloatMenuOption>> recipeOptionsMaker = delegate
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                for (int i = 0; i < (DefDatabase<ThingDef>.GetNamed(billsOriginator, true)).AllRecipes.Count; i++)
                {
                    if ((DefDatabase<ThingDef>.GetNamed(billsOriginator, true)).AllRecipes[i].AvailableNow)
                    {
                        RecipeDef recipe = (DefDatabase<ThingDef>.GetNamed(billsOriginator, true)).AllRecipes[i];
                        list.Add(new FloatMenuOption(recipe.LabelCap, delegate
                        {
                            if (!this.SelTable.Map.mapPawns.FreeColonists.Any((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)))
                            {
                                Bill.CreateNoPawnsWithSkillDialog(recipe);
                            }
                            Bill bill = recipe.MakeNewBill();
                            this.SelTable.billStack.AddBill(bill);
                            if (recipe.conceptLearned != null)
                            {
                                PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
                            }
                            if (TutorSystem.TutorialMode)
                            {
                                TutorSystem.Notify_Event("AddBill-" + recipe.LabelCap);
                            }
                        }, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe), null));
                    }
                }
                if (!list.Any<FloatMenuOption>())
                {
                    list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
                }
                return list;
            };
            this.mouseoverBill = this.SelTable.billStack.DoListing(rect2, recipeOptionsMaker, ref this.scrollPosition, ref this.viewHeight);
        }

        public override void TabUpdate()
        {
            if (this.mouseoverBill != null)
            {
                this.mouseoverBill.TryDrawIngredientSearchRadiusOnMap(this.SelTable.Position);
                this.mouseoverBill = null;
            }
        }
    }
}
