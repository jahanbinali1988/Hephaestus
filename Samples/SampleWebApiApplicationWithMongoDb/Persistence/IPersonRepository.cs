﻿using Hephaestus.Repository.Abstraction.Contract;
using SampleWebApiApplicationWithMongoDb.Models;
using System;

namespace SampleWebApiApplicationWithMongoDb.Persistence
{
    public interface IPersonRepository : IRepository<PersonEntity, Guid>
    {
    }
}
