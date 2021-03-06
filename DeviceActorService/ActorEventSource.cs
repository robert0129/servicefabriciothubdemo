﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.DeviceActorService
{
    using System;
    using System.Diagnostics.Tracing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Microsoft.AzureCat.Samples.PayloadEntities;
    using Microsoft.ServiceFabric.Actors.Runtime;

    [EventSource(Name = "IoTDemo-DeviceActorService")]
    public sealed class ActorEventSource : EventSource
    {
        #region Public Static Properties

        public static ActorEventSource Current = new ActorEventSource();

        #endregion

        #region Private Static Methods

        private static string GetClassFromFilePath(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath))
            {
                return null;
            }
            FileInfo file = new FileInfo(sourceFilePath);
            return Path.GetFileNameWithoutExtension(file.Name);
        }

        #endregion

        #region Public Constructors

        static ActorEventSource()
        {
            // A workaround for the problem where ETW activities do not get tracked until Tasks infrastructure is initialized.
            // This problem will be fixed in .NET Framework 4.6.2.
            Task.Run(() => { }).Wait();
        }

        // Instance constructor is private to enforce singleton semantics
        private ActorEventSource()
        {
        }

        #endregion

        #region Public Methods

        [Event(1, Level = EventLevel.Informational, Message = "{0}")]
        public void Message(string message, [CallerFilePath] string source = "", [CallerMemberName] string method = "")
        {
            if (!this.IsEnabled())
            {
                return;
            }
            this.WriteEvent(1, $"[{GetClassFromFilePath(source) ?? "UNKNOWN"}::{method ?? "UNKNOWN"}] {message}");
        }

        [NonEvent]
        public void ActorMessage(Actor actor, string message, [CallerFilePath] string source = "", [CallerMemberName] string method = "", params object[] args)
        {
            if (!this.IsEnabled())
            {
                return;
            }
            string finalMessage = string.Format(message, args);
            this.ActorMessage(
                actor.GetType().ToString(),
                actor.Id.ToString(),
                actor.ActorService.Context.CodePackageActivationContext.ApplicationTypeName,
                actor.ActorService.Context.CodePackageActivationContext.ApplicationName,
                actor.ActorService.Context.ServiceTypeName,
                actor.ActorService.Context.ServiceName.ToString(),
                actor.ActorService.Context.PartitionId,
                actor.ActorService.Context.ReplicaId,
                actor.ActorService.Context.NodeContext.NodeName,
                GetClassFromFilePath(source) ?? "UNKNOWN",
                method ?? "UNKNOWN",
                finalMessage);
        }

        [NonEvent]
        public void ActorHostInitializationFailed(Exception e, [CallerFilePath] string source = "", [CallerMemberName] string method = "")
        {
            if (this.IsEnabled())
            {
                this.ActorHostInitializationFailed(e.ToString(), GetClassFromFilePath(source) ?? "UNKNOWN", method ?? "UNKNOWN");
            }
        }

        [NonEvent]
        public void Error(Exception e, [CallerFilePath] string source = "", [CallerMemberName] string method = "")
        {
            if (this.IsEnabled())
            {
                this.Error($"[{GetClassFromFilePath(source) ?? "UNKNOWN"}::{method ?? "UNKNOWN"}] {e}");
            }
        }

        [NonEvent]
        public void Telemetry(Device device, Payload payload)
        {
            if (device != null && payload != null && this.IsEnabled())
            {
                this.Telemetry(
                    device.DeviceId,
                    device.Name,
                    device.City,
                    device.Country,
                    device.Manufacturer,
                    device.Model,
                    device.Type,
                    device.MinThreshold,
                    device.MaxThreshold,
                    payload.Value,
                    payload.Status,
                    payload.Timestamp);
            }
        }

        [NonEvent]
        public void Alert(Device device, Payload payload)
        {
            if (device != null && payload != null && this.IsEnabled())
            {
                this.Alert(
                    device.DeviceId,
                    device.Name,
                    device.City,
                    device.Country,
                    device.Manufacturer,
                    device.Model,
                    device.Type,
                    device.MinThreshold,
                    device.MaxThreshold,
                    payload.Value,
                    payload.Status,
                    payload.Timestamp);
            }
        }

        [NonEvent]
        public void Metadata(Device device)
        {
            if (device != null && this.IsEnabled())
            {
                this.Metadata(
                    device.DeviceId,
                    device.Name,
                    device.City,
                    device.Country,
                    device.Manufacturer,
                    device.Model,
                    device.Type,
                    device.MinThreshold,
                    device.MaxThreshold);
            }
        }

        #endregion

        #region Private Instance Methods

        [Event(2, Level = EventLevel.Informational, Message = "{11}")]
        private void ActorMessage(
            string actorType,
            string actorId,
            string applicationTypeName,
            string applicationName,
            string serviceTypeName,
            string serviceName,
            Guid partitionId,
            long replicaOrInstanceId,
            string nodeName,
            string source,
            string method,
            string message)
        {
            this.WriteEvent(
                2,
                actorType,
                actorId,
                applicationTypeName,
                applicationName,
                serviceTypeName,
                serviceName,
                partitionId,
                replicaOrInstanceId,
                nodeName,
                source,
                method,
                message);
        }

        [Event(3, Level = EventLevel.Error, Message = "Actor host initialization failed: {0}")]
        private void ActorHostInitializationFailed(string exception, string source, string method)
        {
            this.WriteEvent(3, exception, source, method);
        }

        [Event(4, Level = EventLevel.Error, Message = "An error occurred: {0}")]
        private void Error(string exception)
        {
            this.WriteEvent(4, exception);
        }

        [Event(5, Level = EventLevel.Informational, Message = "[Telemetry] Id =[{0}] Value=[{9}] Timestamp=[{11}]")]
        private void Telemetry(
            long deviceId,
            string name,
            string city,
            string country,
            string manufacturer,
            string model,
            string type,
            int minThreshold,
            int maxThreshold,
            double value,
            string status,
            DateTime timestamp)
        {
            this.WriteEvent(
                5,
                deviceId,
                name,
                city,
                country,
                manufacturer,
                model,
                type,
                minThreshold,
                maxThreshold,
                value,
                status,
                timestamp);
        }

        [Event(6, Level = EventLevel.Informational, Message = "[Alert] Id =[{0}] Value=[{9}] Timestamp=[{11}]")]
        private void Alert(
            long deviceId,
            string name,
            string city,
            string country,
            string manufacturer,
            string model,
            string type,
            int minThreshold,
            int maxThreshold,
            double value,
            string status,
            DateTime timestamp)
        {
            this.WriteEvent(
                6,
                deviceId,
                name,
                city,
                country,
                manufacturer,
                model,
                type,
                minThreshold,
                maxThreshold,
                value,
                status,
                timestamp);
        }

        [Event(7, Level = EventLevel.Informational,
            Message = "[Metadata] Id =[{0}] Name=[{1}] City=[{2}] Country=[{3}] Manufacturer=[{4}] Model=[{5}] Type=[{6}] MinThreshold=[{7}] MaxThreshold=[{8}]"
            )]
        private void Metadata(
            long deviceId,
            string name,
            string city,
            string country,
            string manufacturer,
            string model,
            string type,
            int minThreshold,
            int maxThreshold)
        {
            this.WriteEvent(
                7,
                deviceId,
                name,
                city,
                country,
                manufacturer,
                model,
                type,
                minThreshold,
                maxThreshold);
        }

        #endregion
    }
}