﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
 
using System;
using System.IO;
using System.Xml.Serialization;

namespace Aliyun.OpenServices.Common.Transform
{
    /// <summary>
    /// Serialize an object of type TRequest to XML stream.
    /// </summary>
    internal class XmlStreamSerializer<TRequest> : ISerializer<TRequest, Stream>
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(TRequest));

        public Stream Serialize(TRequest requestObject)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                Serializer.Serialize(stream, requestObject, namespaces);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            catch (InvalidOperationException ex)
            {
                if (stream != null)
                    stream.Close();
                
                throw new RequestSerializationException(ex.Message, ex);
            }
        }
    }
}
