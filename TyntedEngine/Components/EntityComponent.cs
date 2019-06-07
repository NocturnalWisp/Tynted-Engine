using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
    public struct EntityComponent
    {
        internal int entityID;
        internal IComponent component;

        internal EntityComponent(int entityID, IComponent component)
        {
            this.entityID = entityID;
            this.component = component;
        }

        public static bool operator ==(EntityComponent a, EntityComponent b)
        {
            return (a.entityID == b.entityID) && (a.component == b.component);
        }

        public static bool operator !=(EntityComponent a, EntityComponent b)
        {
            return (a.entityID != b.entityID) || (a.component != b.component);
        }
    }
}
