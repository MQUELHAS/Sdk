﻿using EdiFabric.Core.Model.Edi.Edifact;
using EdiFabric.Framework;
using EdiFabric.Templates.EdifactD03B;
using EdiFabric.Templates.EdifactD96A;
using EdiFabric.Templates.Padis;
using System.Reflection;

namespace EdiFabric.Sdk.Helpers.Edifact
{
    public class TemplateFactory
    {
        /// <summary>
        /// Parse the transaction explicitly.
        /// </summary>
        /// <param name="unb">The UNB.</param>
        /// <param name="ung">The UNG.</param>
        /// <param name="unh">The UNH.</param>
        /// <returns>The type to parse to.</returns>
        public static TypeInfo FullTemplateFactory(UNB unb, UNG ung, UNH unh)
        {
            if (unb.INTERCHANGESENDER_2.InterchangeSenderIdentification_1 == "SPLIT1" &&
               unh.MessageIdentifier_02.MessageType_01 == "ORDERS")
                return typeof(TSORDERSSplit).GetTypeInfo();

            if (unb.INTERCHANGESENDER_2.InterchangeSenderIdentification_1 == "INVALID1" &&
               unh.MessageIdentifier_02.MessageType_01 == "ORDERS")
                return typeof(TSORDERSValidation).GetTypeInfo();

            if (unb.INTERCHANGESENDER_2.InterchangeSenderIdentification_1 == "CUSTOM1" &&
                unh.MessageIdentifier_02.MessageType_01 == "ORDERS")
                return typeof(TSORDERSCustom1).GetTypeInfo();

            if (unb.INTERCHANGESENDER_2.InterchangeSenderIdentification_1 == "CUSTOM2" &&
                unh.MessageIdentifier_02.MessageType_01 == "ORDERS")
                return typeof(TSORDERSCustom2).GetTypeInfo();

            if (unh.MessageIdentifier_02.MessageReleaseNumber_03 == "96A" &&
                unh.MessageIdentifier_02.MessageType_01 == "ORDERS")
                return typeof(TSORDERS).GetTypeInfo();

            if (unh.MessageIdentifier_02.MessageReleaseNumber_03 == "96A" &&
                unh.MessageIdentifier_02.MessageType_01 == "INVOIC")
                return typeof(TSINVOIC).GetTypeInfo();

            if (unh.MessageIdentifier_02.MessageReleaseNumber_03 == "96A" &&
               unh.MessageIdentifier_02.MessageType_01 == "INVOIC")
                return typeof(TSINVOIC).GetTypeInfo();

            if (unh.MessageIdentifier_02.MessageType_01 == "PNRGOV")
                return typeof(TSPNRGOV).GetTypeInfo();

            if (unh.MessageIdentifier_02.MessageReleaseNumber_03 == "01B" &&
                unh.MessageIdentifier_02.MessageType_01 == "INVOIC" &&
                unh.MessageIdentifier_02.AssociationAssignedCode_05.StartsWith("EAN", System.StringComparison.Ordinal))
                return typeof(Templates.EancomD01B.TSINVOIC).GetTypeInfo();

            throw new System.Exception(string.Format("Transaction {0} for version {1} is not supported.",
                unh.MessageIdentifier_02.MessageType_01, unh.MessageIdentifier_02.MessageVersionNumber_02 + unh.MessageIdentifier_02.MessageReleaseNumber_03));
        }

        public static Assembly TrialTemplateFactory(MessageContext messageContext)
        {
            if (messageContext.Version == "D03B" && messageContext.Name == "CUSCAR")
                return Assembly.Load("EdiFabric.Sdk.Edifact.Templates.USCustoms");

            if (messageContext.Version == "D03B" && messageContext.Name == "PAXLST")
                return Assembly.Load("EdiFabric.Sdk.Edifact.Templates.USCustoms");

            if (messageContext.Version == "D13B" && messageContext.Name == "BAPLIE")
                return Assembly.Load("EdiFabric.Sdk.Edifact.Templates.Smdg");          

            if (messageContext.Format == "EDIFACT")
                return Assembly.Load("EdiFabric.Templates.Edifact");

            if (messageContext.Format == "X12")
                return Assembly.Load("EdiFabric.Templates.X12");

            throw new System.Exception(string.Format("Version {0} is not supported.", messageContext.Version));
        }
    }
}
