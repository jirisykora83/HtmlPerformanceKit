﻿using System.Collections.Generic;

namespace HtmlPerformanceKit.Infrastructure
{
    internal class AttributeBufferList
    {
        private readonly List<AttributeBuffer> attributes = new List<AttributeBuffer>();

        internal int Count { get; private set; }

        internal AttributeBuffer Current { get; private set; }

        internal CharBuffer this[string name] => GetItem(name)?.Value;

        internal AttributeBuffer this[int index] => index < Count ? attributes[index] : null;

        internal void Add()
        {
            if (attributes.Count > Count)
            {
                var existingAttributeBuffer = attributes[Count];
                existingAttributeBuffer.Clear();
                Count++;

                Current = existingAttributeBuffer;
            }
            else
            {
                var newAttributeBuffer = new AttributeBuffer();
                attributes.Add(newAttributeBuffer);
                Count++;

                Current = newAttributeBuffer;
            }
        }

        internal void Clear()
        {
            Count = 0;
            Current = null;
        }

        private AttributeBuffer GetItem(string name)
        {
            for (var index = 0; index < Count; index++)
            {
                if (attributes[index].Name.Equals(name))
                {
                    return attributes[index];
                }
            }

            return null;
        }        
    }
}
