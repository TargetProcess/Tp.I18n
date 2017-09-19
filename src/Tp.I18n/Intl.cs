using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jeffijoe.MessageFormat;
using Jeffijoe.MessageFormat.Formatting;

namespace Tp.I18n
{
    public class Intl : IIntl
    {
        private readonly IMessageFormatter _messageFormatter;

        public Intl(IMessageFormatter messageFormatter)
        {
            _messageFormatter = messageFormatter;
        }

        public IFormattedMessage GetFormattedMessage(string token)
        {
            return GetFormattedMessage(token, new {});
        }

        public IFormattedMessage GetFormattedMessage(string token, object data)
        {
            try
            {
                var dataDictionary = ObjectToDictionary(data);
                var flattenedData = dataDictionary.ToDictionary(
                    pair => pair.Key,
                    pair => (object) pair.Value?.ToString());
                var value = _messageFormatter.FormatMessage(token, flattenedData);
                return new FormattedMessage(token, dataDictionary, value);
            }
            catch (VariableNotFoundException e)
            {
                throw new KeyNotFoundException(e.Message, e);
            }
        }

        private static IDictionary<string, object> ObjectToDictionary(object obj)
        {
            return obj
                .GetType()
                .GetProperties()
                .ToDictionary(x => x.Name, x =>
                {
                    var value = x.GetValue(obj, null);
                    var maybeFormattedMessage = value as IFormattedMessage;
                    if (maybeFormattedMessage == null)
                    {
                        return value?.ToString();
                    }

                    return (object) maybeFormattedMessage;
                });
        }
    }
}