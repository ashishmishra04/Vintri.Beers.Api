namespace Vintri.Beers.Model.Validation
{
    public class ValidationError
    {
        public string Message { get; }
        public string Property { get; }

        public ValidationError(string message, string property)
        {
            Message = message;
            Property = property;
        }

        public override string ToString()
        {
            return $"{Property} - {Message}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == typeof (ValidationError) && Equals((ValidationError)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Message.GetHashCode()*397) ^ Property.GetHashCode();
            }
        }

        public static bool operator ==(ValidationError left, ValidationError right)
        {
            return left != null && left.Equals(right);
        }

        public static bool operator !=(ValidationError left, ValidationError right)
        {
            return left != null && !left.Equals(right);
        }
    }
}
