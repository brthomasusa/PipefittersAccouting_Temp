#pragma warning disable CS8618

namespace PipefittersAccounting.SharedKernel.CommonValueObjects
{
    public class PointOfContact : ValueObject
    {
        protected PointOfContact() { }

        private PointOfContact(PersonName name, PhoneNumber telephone)
        {
            ContactName = name;
            ContactTelephone = telephone;
        }

        public PersonName ContactName { get; }
        public PhoneNumber ContactTelephone { get; }

        public static PointOfContact Create(PersonName name, PhoneNumber telephone)
        {
            return new PointOfContact(name, telephone);
        }
    }
}