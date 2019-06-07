using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted
{
    public abstract class GameExtension
    {
        public List<Type> components = new List<Type>();
        public List<Type> systems = new List<Type>();

        /// <summary>
        /// Use initialize to add to the scene, system, and component lists.
        /// </summary>
        public virtual void Intitialize() { }
    }
}
