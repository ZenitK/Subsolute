/*
    Generated with https://xmltocsharp.azurewebsites.net/
    Licensed under the Apache License, Version 2.0    
    http://www.apache.org/licenses/LICENSE-2.0
*/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace Xml2CSharp
{
    [XmlRoot(ElementName = "Configuration")]
    public class Configuration
    {
        [XmlAttribute(AttributeName = "Condition")]
        public string Condition { get; set; }

        [XmlText] public string Text { get; set; }
    }

    [XmlRoot(ElementName = "PropertyGroup")]
    public class PropertyGroup
    {
        [XmlElement(ElementName = "Configuration")]
        public List<Configuration> Configuration { get; set; }

        [XmlElement(ElementName = "RuntimeIdentifiers")]
        public string RuntimeIdentifiers { get; set; }

        [XmlElement(ElementName = "ProjectGuid")]
        public string ProjectGuid { get; set; }

        [XmlElement(ElementName = "EnableDefaultCompileItems")]
        public string EnableDefaultCompileItems { get; set; }

        [XmlElement(ElementName = "EnableDefaultContentItems")]
        public string EnableDefaultContentItems { get; set; }

        [XmlElement(ElementName = "TargetFramework")]
        public string TargetFramework { get; set; }

        [XmlElement(ElementName = "ServerGarbageCollection")]
        public string ServerGarbageCollection { get; set; }

        [XmlElement(ElementName = "ConcurrentGarbageCollection")]
        public string ConcurrentGarbageCollection { get; set; }

        [XmlElement(ElementName = "RetainVMGarbageCollection")]
        public string RetainVmGarbageCollection { get; set; }

        [XmlElement(ElementName = "TieredCompilation")]
        public string TieredCompilation { get; set; }

        [XmlElement(ElementName = "UseAppHost")]
        public string UseAppHost { get; set; }

        [XmlElement(ElementName = "RootNamespace")]
        public string RootNamespace { get; set; }

        [XmlElement(ElementName = "OutputType")]
        public string OutputType { get; set; }

        [XmlElement(ElementName = "PlatformTarget")]
        public string PlatformTarget { get; set; }

        [XmlElement(ElementName = "AllowUnsafeBlocks")]
        public string AllowUnsafeBlocks { get; set; }

        [XmlElement(ElementName = "AssemblyName")]
        public string AssemblyName { get; set; }

        [XmlElement(ElementName = "DefineConstants")]
        public string DefineConstants { get; set; }

        [XmlElement(ElementName = "DebugType")]
        public string DebugType { get; set; }

        [XmlElement(ElementName = "OutputPath")]
        public string OutputPath { get; set; }

        [XmlElement(ElementName = "WarningLevel")]
        public string WarningLevel { get; set; }

        [XmlElement(ElementName = "DebugSymbols")]
        public string DebugSymbols { get; set; }

        [XmlAttribute(AttributeName = "Condition")]
        public string Condition { get; set; }

        [XmlElement(ElementName = "Tailcalls")]
        public string Tailcalls { get; set; }

        [XmlElement(ElementName = "Optimize")] public string Optimize { get; set; }
    }

    [XmlRoot(ElementName = "PackageReference")]
    public class PackageReference
    {
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "ItemGroup")]
    public class ItemGroup
    {
        [XmlElement(ElementName = "PackageReference")]
        public PackageReference PackageReference { get; set; }

        [XmlElement(ElementName = "ProjectReference")]
        public List<ProjectReference> ProjectReference { get; set; }

        [XmlElement(ElementName = "Reference")]
        public Reference Reference { get; set; }

        [XmlElement(ElementName = "Compile")] public Compile Compile { get; set; }
    }

    [XmlRoot(ElementName = "ProjectReference")]
    public class ProjectReference
    {
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [XmlRoot(ElementName = "Reference")]
    public class Reference
    {
        [XmlElement(ElementName = "SpecificVersion")]
        public string SpecificVersion { get; set; }

        [XmlElement(ElementName = "HintPath")] public string HintPath { get; set; }

        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [XmlRoot(ElementName = "Compile")]
    public class Compile
    {
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [XmlRoot(ElementName = "Target")]
    public class Target
    {
        [XmlAttribute(AttributeName = "Name")] public string Name { get; set; }
    }

    [XmlRoot(ElementName = "Project")]
    public class Project
    {
        [XmlElement(ElementName = "PropertyGroup")]
        public List<PropertyGroup> PropertyGroup { get; set; }

        [XmlElement(ElementName = "ItemGroup")]
        public List<ItemGroup> ItemGroup { get; set; }

        [XmlElement(ElementName = "Target")] public Target Target { get; set; }
        [XmlAttribute(AttributeName = "Sdk")] public string Sdk { get; set; }
    }
}