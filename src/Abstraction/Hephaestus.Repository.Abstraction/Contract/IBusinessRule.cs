using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface IBusinessRule
    {
        Task<bool> IsBroken();

        string Message { get; }

        public string[] Properties { get; }

        public string ErrorType { get; }
    }
}
