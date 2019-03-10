using System.Collections.Generic;
using Verse;

namespace GraphicOverhaul
{
    public class RoomRoleWorker_FancyKitchen : RoomRoleWorker
    {
        public override float GetScore(Room room)
        {
            int num = 0;
            List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
            for (int i = 0; i < containedAndAdjacentThings.Count; i++)
            {
                Thing thing = containedAndAdjacentThings[i];
                if (thing.def.designationCategory == DefDatabase<DesignationCategoryDef>.GetNamed("GO_Kitchen", true))
                {
                    num++;
                   /* for (int j = 0; j < thing.def.AllRecipes.Count; j++)
                    {
                        RecipeDef recipeDef = thing.def.AllRecipes[j];
                        for (int k = 0; k < recipeDef.products.Count; k++)
                        {
                            ThingDef thingDef = recipeDef.products[k].thingDef;
                            if (thingDef.IsNutritionGivingIngestible && thingDef.ingestible.HumanEdible)
                            {
                                
                               
                            }
                        }
                    }*/
                }
                
            }
            return (float)num * 14f;
        }
    }
}