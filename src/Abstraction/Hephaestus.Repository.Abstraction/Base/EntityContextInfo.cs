using Hephaestus.Repository.Abstraction.Contract;
using System;

namespace Hephaestus.Repository.Abstraction.Base
{
    public class EntityContextInfo<T> where T : Entity
	{
		public Entity Document { get; set; }
		public CommandType CommandType { get; set; }
        public Type EntityType { get; set; }
		public ICommandProvider<T> CommandProvider { get; set; }
	}
}
