using System.Collections.Generic;

namespace Tp.I18n
{
    public class FormattedMessage : IFormattedMessage
    {
        public FormattedMessage(string token, IDictionary<string, object> data, string value)
        {
            Token = token;
            Data = data;
            Value = value;
        }

        public string Token { get; }
        public IDictionary<string, object> Data { get; }
        public string Value { get; }

        public bool Equals(IFormattedMessage other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object other)
        {
            var formattedMessage = other as IFormattedMessage;
            return formattedMessage != null && Equals(formattedMessage);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}