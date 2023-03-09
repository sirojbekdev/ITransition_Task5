using Bogus;
using System.Text;

namespace Task5.Data
{
    public static class FakerService
    {
        private static readonly string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static List<Citizen>? _citizens;

        public static List<Citizen> FakeData(string locale, int count, int numErrors = 0)
        {
            Randomizer.Seed = new Random(123456);
            Faker<Citizen> users = new Faker<Citizen>(locale)
                .StrictMode(true)
                .RuleFor(x => x.Id, f => f.IndexVariable += 113)
                .RuleFor(x => x.Name, f => f.Person.FullName)
                .RuleFor(x => x.Phone, f => f.Person.Phone)
                .RuleFor(x => x.Address,
                    f => f.Person.Address.Suite +
                    f.Person.Address.Street +
                    f.Person.Address.City);
            _citizens = users.Generate(count);

            if (numErrors > 0)
            {
                for (int i = 0; i < _citizens.Count; i++)
                {
                    for (int e = 1; e <= numErrors; e++)
                    {

                        switch (e % 3)
                        {
                            case 0:
                                _citizens[i].Name = AddError(_citizens[i].Name, e);
                                break;
                            case 1:
                                _citizens[i].Phone = AddError(_citizens[i].Phone, e);
                                break;
                            case 2:
                                _citizens[i].Address = AddError(_citizens[i].Address, e);
                                break;
                        }
                    }
                }
            }

            return _citizens;
        }

        private static string AddChar(StringBuilder sb, int error, int length)
        {
            sb.Insert(error % length, _chars[error % length % 61]);
            return sb.ToString();
        }

        private static string RemoveChar(StringBuilder sb, int error, int length)
        {
            sb.Remove(error % length, 1);
            return sb.ToString();
        }

        private static string SwapChars(StringBuilder sb, int error, int length)
        {
            var temp = sb[error % length];
            if (sb[length - 1] % 2 == 1)
            {
                sb[error % length] = sb[(error + 1) % length];
                sb[(error + 1) % length] = temp;
            }
            else
            {
                sb[error % length] = sb[(error - 1) % length];
                sb[(error - 1) % length] = temp;
            }
            return sb.ToString();
        }

        private static string AddError(string field, int error)
        {
            var length = field.Length;
            StringBuilder sb = new(field);
            if (length < 3 || length % 5 == 4)
            {
                return AddChar(sb, error, length);
            }
            else if (length % 2 == 0 || length % 3 == 2)
            {
                return RemoveChar(sb, error, length);
            }
            else
            {
                return SwapChars(sb, error, length);
            }

        }
    }
}
