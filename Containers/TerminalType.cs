﻿using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Containers.Admin;

namespace Containers
{
    [Serializable, XmlRoot("TerminalType")]
    [DataContract(Name = "TerminalType", Namespace = "urn:CashIn")]
    public class TerminalType
    {
        private int _Id;
        private MultiLanguageString _Name;

        [XmlElement(ElementName = "Id")]
        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [XmlElement(ElementName = "Name")]
        [DataMember(Name = "Name")]
        public MultiLanguageString Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public TerminalType()
        {
        }

        public TerminalType(int id, MultiLanguageString name)
        {
            _Id = id;
            _Name = name;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}", _Id, _Name);
        }
    }
}
