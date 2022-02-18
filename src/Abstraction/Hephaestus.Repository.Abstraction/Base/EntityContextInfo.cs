using Hephaestus.Repository.Abstraction.Contract;
using System;

namespace Hephaestus.Repository.Abstraction.Base
{
    public class EntityContextInfo
	{
		public object Id { get; set; }
		public object Document { get; set; }
		public CommandType CommandType { get; set; }
        public Type EntityType { get; set; }
		public ICommandProvider CommandProvider { get; set; }
	}
}
