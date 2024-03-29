﻿using System;
using Soa.Protocols.ServiceDesc;

namespace Soa.Protocols.Attributes
{
    /// <summary>
    ///     who has this attribute service will be register as microservice
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SoaServiceAttribute : SoaServiceDescAttribute
    {



        public SoaServiceAttribute(bool isExposureToGateway = true)
        {
            IsWaitExecution = true;
            IsExposureToGateway=isExposureToGateway;
        }

        /// <summary>
        ///     service id, if not set, default value will be generated by IServiceIdGenerator
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     invoke path of the service
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        ///     sync to invoke
        /// </summary>
        public bool IsWaitExecution { get; set; }

        /// <summary>
        ///    exposure api to the Gateway
        /// </summary>
        public bool IsExposureToGateway { get; set; }
        /// <summary>
        ///     creater for this service
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        ///     all the created and modified comment for this service
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     service create date
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        ///     before invoke this service, need to be authorization
        /// </summary>
        public bool EnableAuthorization { get; set; }

        /// <summary>
        ///     just for specify roles to invoke this service
        /// </summary>
        public string Roles { get; set; }

        public override void Apply(SoaServiceDesc descriptor)
        {
            descriptor.WaitExecution = IsWaitExecution;
            descriptor.EnableAuthorization = EnableAuthorization;
            if (!string.IsNullOrEmpty(Id))
                descriptor.Id = Id;
            if (!string.IsNullOrEmpty(RoutePath))
                descriptor.RoutePath = RoutePath;
            if (!string.IsNullOrEmpty(CreatedBy))
                descriptor.CreatedBy = CreatedBy;
            if (!string.IsNullOrEmpty(CreatedDate))
                descriptor.CreatedDate = CreatedDate;
            if (!string.IsNullOrEmpty(Comment))
                descriptor.Comment = Comment;
            if (!string.IsNullOrEmpty(Roles))
                descriptor.Roles = Roles;
        }
    }
}