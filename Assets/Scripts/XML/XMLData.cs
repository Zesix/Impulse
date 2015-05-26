/*****************************************
 * This file is part of Impulse Framework.

    Impulse Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    Impulse Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Impulse Framework.  If not, see <http://www.gnu.org/licenses/>.
*****************************************/

/// <summary>
/// XML data.
/// IMPORTANT: FOR XML LOADING TO BE DONE
/// THIS SCRIPT FUNCTIONS WITHOUT OTHER INPUT
/// </summary>

using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class XMLData
{
	static XmlWriterSettings settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
	
	public static void SaveObjects (string folderPath, string fileName, XMLExample[] obj)
	{
		XmlSerializerNamespaces ns = new XmlSerializerNamespaces ();
		
		XmlSerializer ourSerializer = new XmlSerializer (typeof(XMLExample[]));
		
		XmlWriter output;
		
		ns.Add ("", "");
		
		ourSerializer = new XmlSerializer (typeof(XMLExample[]));
		
		if (!Directory.Exists (folderPath)) {
			Directory.CreateDirectory (folderPath);
			
		}
		
		using (Stream ourStream = File.OpenWrite (folderPath + "/" + fileName + ".xml")) {
			using (output = XmlWriter.Create (ourStream, settings)) {
				ourSerializer.Serialize (output, obj);
				
			}
		}
	}
	
	public static XMLExample[] LoadObjects (string folderPath, string fileName)
	{
		XMLExample[] objectsLoaded;
		XmlSerializer ourSerializer = new XmlSerializer (typeof(XMLExample[]));
		
		if (File.Exists (folderPath + "/" + fileName)) {
			using (Stream load = File.OpenRead (folderPath + "/" + fileName)) {
				objectsLoaded = ourSerializer.Deserialize (load) as XMLExample[];
				
			}
			
			return objectsLoaded;
			
		} else {
			return null;
			
		}
	}
}
