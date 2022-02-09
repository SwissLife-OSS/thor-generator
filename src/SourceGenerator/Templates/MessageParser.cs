using System;
using System.Collections.Generic;
using System.Text;

namespace Thor.Generator.Templates
{
    internal class MessageParser
    {
        public static IEnumerable<Placeholder> FindPlaceholders(
            string message)
        {
            int position = 0;
            while (Skip(message, '{', ref position))
            {
                int start = position;

                if (Peek(message, '{', start))
                {
                    position += 2;
                    continue;
                }
                else if (Skip(message, '}', ref position))
                {
                    Placeholder placeholder = ParsePlaceholder(
                        start,
                        position,
                        message.Substring(
                            start + 1,
                            position - start - 1));

                    if (placeholder.IsNameBased())
                    {
                        yield return placeholder;
                    }
                }
            }
        }

        public static string ReplacePlaceholders(
            string message,
            IReadOnlyCollection<Placeholder> placeholders,
            Func<Placeholder, string> getValue)
        {
            StringBuilder newMessage = new StringBuilder();
            int index = 0;

            foreach (Placeholder placeholder in placeholders)
            {
                if (index < placeholder.Start)
                {
                    newMessage.Append(message.Substring(index, placeholder.Start - index));
                    index = placeholder.Start;
                }

                newMessage.Append(getValue(placeholder));
                index = placeholder.End + 1;
            }

            if (index < message.Length)
            {
                newMessage.Append(message.Substring(index));
            }

            return newMessage.ToString();
        }

        private static Placeholder ParsePlaceholder(
            int start,
            int end,
            string placeholder)
        {
            int index = 0;

            if (Skip(placeholder, ':', ref index))
            {
                return new Placeholder(
                    start,
                    end,
                    placeholder.Substring(0, index),
                    placeholder.Substring(index + 1));
            }

            return new Placeholder(start, end, placeholder);
        }

        private static bool Skip(string message, char token, ref int position)
        {
            while (position < message.Length && message[position] != token)
            {
                position++;
            }

            return (position < message.Length && message[position] == token);
        }

        private static bool Peek(string message, char token, int position)
        {
            int next = position + 1;
            return message.Length > next
                && message[next] == token;
        }
    }
}
