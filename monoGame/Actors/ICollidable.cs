using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Actors
{
    public interface ICollidable
    {
        public void IsColliding(ActorBase actor);
        public void Subscribe(ActorBase actor);
        public void Emit(ActorBase actor);

    }
}
