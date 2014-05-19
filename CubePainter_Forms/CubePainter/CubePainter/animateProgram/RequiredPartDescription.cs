using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeAnimator
{
    public class RequiredPartDescription
    {
        public string name;
        public string notes;
        public List<RequiredPartDescription> requiredChildren;
        public bool loaded = false;
        public BodyPartType type;
        public string fileName;

        public RequiredPartDescription() { }
        public RequiredPartDescription(BodyPartType ntype, string nDestription, List<RequiredPartDescription> requiredSubParts)
        {
            type = ntype;
            name = BodyPartTypeManager.getName(type);
            
            notes = nDestription;
            requiredChildren = requiredSubParts;
        }
        public RequiredPartDescription getNextRequired()
        {
            if (!loaded)
            {
                
                return this;
            }
            else
            {
                
                foreach (RequiredPartDescription req in requiredChildren)
                {
                    
                    RequiredPartDescription result = req.getNextRequired();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

    }
    /*class RequiredPartDescriptionRoot : RequiredPartDescription
    {
        RequiredPartDescription mainPart;
        public RequiredPartDescriptionRoot(string nName, string nDestription, List<RequiredPartDescription> requiredSubParts, RequiredPartDescription nmainPart)
        {
            name = nName;
            notes = nDestription;
            requiredChildren = requiredSubParts;
            mainPart = nmainPart;
        }
    }*/
}
