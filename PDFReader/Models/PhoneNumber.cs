﻿using System;
using System.Collections.Generic;
using System.Text;
using PDFReader.Models.Records;

namespace PDFReader.Models
{
    public class PhoneNumber
    {
        public PhoneNumber(string number)
        {
            Number = number;
            Calls = new List<Call>();
            Sms = new List<Sms>();
            Gprs = new List<Gprs>();
        }
        
        public string Number { get; set; }
        public bool HasDetails { get; set; }
        public long MvcApnUses { get; set; }
        public long MvcApnBytes { get; set; }
        public List<Call> Calls { get; set; }
        public List<Sms> Sms { get; set; }
        public List<Gprs> Gprs { get; set; }
        public List<string> InternationalSmsNumbers { get; set; }

        // data
        public string Contract { get; set; }
        public string Plan { get; set; }
        public string DataPlan { get; set; }
        public string SmsPlan { get; set; }

        //records
        public CallRecord CallRecord { get; set; }
        public LongDistanceCallRecord LongDistanceCallRecord { get; set; }
        public SmsRecords SmsRecord { get; set; }
        public LongDistanceSmsRecord LongDistanceSmsRecord { get; set; }
        public GprsRecord GprsRecord { get; set; }
        public MmsRecord MmsRecord { get; set; }
        public RoamingCallsRecord RoamingCallRecord { get; set; }
        public RoamingSmsRecord RoamingSmsRecord { get; set; }
        public RoamingMmsRecord RoamingMmsRecord { get; set; }
        public RoamingGprsRecord RoamingGprsRecord { get; set; }
        public FinalPhoneNumberRecord FinalPhoneNumberRecord { get; set; }
    }
}
