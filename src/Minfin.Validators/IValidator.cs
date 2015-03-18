namespace Minfin.Validators
{
    public interface IValidator
    {
        bool HasValidFormat(string nit);
        bool IsValid(string nit);
    }
}
