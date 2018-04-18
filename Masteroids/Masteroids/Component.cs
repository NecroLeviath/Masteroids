using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masteroids 
    { // CR: Denna måsvinge bör skjutas bak lite
    public abstract class Component 
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);  // CR: Vet inte om gameTime hör hemma i Draw

        public abstract void Update(GameTime gameTime);

        // CR: Onödiga mellanrum.

    }
}
