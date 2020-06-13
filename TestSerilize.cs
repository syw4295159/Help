using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

[Serializable]
public class TestSerilize
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlElement("List")]
    public List<int> List { get; set; }
}
