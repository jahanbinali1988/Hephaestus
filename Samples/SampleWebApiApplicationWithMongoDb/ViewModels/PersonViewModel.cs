using System;

namespace SampleWebApiApplicationWithMongoDb.ViewModels
{
    public class PersonViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
