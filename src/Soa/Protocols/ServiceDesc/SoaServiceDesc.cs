﻿using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Soa.Protocols.ServiceDesc
{
    public class SoaServiceDesc
    {
        public SoaServiceDesc()
        {
            Metadatas = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        ///     the id for the service(service also as a method)
        /// </summary>
        public string Id
        {
            get => GetMetadata<string>("Id");
            set => Metadatas["Id"] = value;
        }

        /// <summary>
        ///     service route path
        /// </summary>
        public string RoutePath
        {
            get => GetMetadata<string>("RoutePath");
            set => Metadatas["RoutePath"] = value;
        }

        public bool WaitExecution
        {
            get => GetMetadata<bool>("WaitExecution");
            set => Metadatas["WaitExecution"] = value;
        }

        public bool EnableAuthorization
        {
            get => GetMetadata<bool>("EnableAuthorization");
            set => Metadatas["EnableAuthorization"] = value;
        }

        public string CreatedDate
        {
            get => GetMetadata<string>("CreatedDate");
            set => Metadatas["CreatedDate"] = value;
        }

        public string CreatedBy
        {
            get => GetMetadata<string>("CreatedBy");
            set => Metadatas["CreatedBy"] = value;
        }

        public string Comment
        {
            get => GetMetadata<string>("Comment");
            set => Metadatas["Comment"] = value;
        }

        public string Roles
        {
            get => GetMetadata<string>("Roles");
            set => Metadatas["Roles"] = value;
        }

        public string ReturnType
        {
            get => GetMetadata<string>("ReturnType");
            set => Metadatas["ReturnType"] = value;
        }

        /// <summary>
        ///     other useful data
        /// </summary>
        public IDictionary<string, object> Metadatas { get; set; }

        public T GetMetadata<T>(string name, T def = default)
        {
            if (!Metadatas.ContainsKey(name))
                return def;
            return (T)Metadatas[name];
        }
    }
}