#pragma warning disable 1587
/// <summary>
/// XML data.
/// IMPORTANT: FOR XML LOADING TO BE DONE
/// THIS SCRIPT FUNCTIONS WITHOUT OTHER INPUT
/// </summary>
#pragma warning restore 1587

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLData
{
	private static readonly XmlWriterSettings Settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
	
	public static void SaveObjects (string folderPath, string fileName, XMLExample[] obj)
	{
		var ns = new XmlSerializerNamespaces ();

		ns.Add ("", "");
		
		var ourSerializer = new XmlSerializer (typeof(XMLExample[]));
		
		if (!Directory.Exists (folderPath)) {
			Directory.CreateDirectory (folderPath);
			
		}
		
		using (Stream ourStream = File.OpenWrite (folderPath + "/" + fileName + ".xml"))
		{
			XmlWriter output;
			using (output = XmlWriter.Create (ourStream, Settings)) {
				ourSerializer.Serialize (output, obj);
				
			}
		}
	}
	
	public static XMLExample[] LoadObjects (string folderPath, string fileName)
	{
		var ourSerializer = new XmlSerializer (typeof(XMLExample[]));

		if (!File.Exists(folderPath + "/" + fileName)) return null;
		XMLExample[] objectsLoaded;
		using (Stream load = File.OpenRead (folderPath + "/" + fileName)) {
			objectsLoaded = ourSerializer.Deserialize (load) as XMLExample[];
				
		}
			
		return objectsLoaded;
	}
}
