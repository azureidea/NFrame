//-----------------------------------------------------------------------
// <copyright file="NFCElementModule.cs">
//     Copyright (C) 2015-2015 lvsheng.huang <https://github.com/ketoo/NFrame>
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;

namespace NFrame
{
    public class NFCElementModule : NFIElementModule
    {
        public NFCElementModule()
        {
            mhtObject = new Dictionary<string, NFIElement>();
        }

        public override void Init()
        {
            mxLogicClassModule = GetMng().GetModule<NFILogicClassModule>();
        }

        public override void AfterInit()
        {
            Load();
        }

        public override void BeforeShut() 
        {
        }

        public override void Shut()
        {
        }


        public override void Execute()
        {

        }

        public override bool Load()
        {
            mstrRootPath = "";
            ClearInstanceElement();

            Dictionary<string, NFILogicClass> xTable = mxLogicClassModule.GetElementList();
            foreach (KeyValuePair<string, NFILogicClass> kv in xTable)
            {
                LoadInstanceElement(kv.Value);
            }

            return false;
        }

        public override bool Clear()
        {
            return false;
        }

        public override bool ExistElement(string strConfigName)
        {
            if (mhtObject.ContainsKey(strConfigName))
            {
                return true;
            }

            return false;
        }

        public override Int64 QueryPropertyInt(string strConfigName, string strPropertyName)
        {
            NFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                return xElement.QueryInt(strPropertyName);
            }

            return 0;
        }

        public override float QueryPropertyFloat(string strConfigName, string strPropertyName)
        {
            NFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                 return xElement.QueryFloat(strPropertyName);
            }

            return 0;
        }

        public override double QueryPropertyDouble(string strConfigName, string strPropertyName)
        {
            NFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                xElement.QueryDouble(strPropertyName);
            }

            return 0;
        }

        public override string QueryPropertyString(string strConfigName, string strPropertyName)
        {
            NFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                return xElement.QueryString(strPropertyName);
            }

            return NFIDataList.NULL_STRING;
        }

        public override bool AddElement(string strName, NFIElement xElement)
        {
            if (!mhtObject.ContainsKey(strName))
            {
                mhtObject.Add(strName, xElement);

                return true;
            }

            return false;
        }

        public override NFIElement GetElement(string strConfigName)
        {
            if (mhtObject.ContainsKey(strConfigName))
            {
                return (NFIElement)mhtObject[strConfigName];
            }

            return null;
        }

        private void ClearInstanceElement()
        {
            mhtObject.Clear();
        }

        private void LoadInstanceElement(NFILogicClass xLogicClass)
        {
            string strLogicPath = mstrRootPath;
            strLogicPath += xLogicClass.GetInstance();

            XmlDocument xmldoc = new XmlDocument();
            //xmldoc.Load(strLogicPath);
            ///////////////////////////////////////////////////////////////////////////////////////
            StreamReader cepherReader = new StreamReader(strLogicPath); ;
            string strContent = cepherReader.ReadToEnd();
            cepherReader.Close();

            byte[] data = Convert.FromBase64String(strContent);

//             MemoryStream stream = new MemoryStream(data);
//             XmlReader x = XmlReader.Create(stream);
//             x.MoveToContent();
//             string res = x.ReadOuterXml();
            string res = System.Text.ASCIIEncoding.Default.GetString(data);

            xmldoc.LoadXml(res);
            /////////////////////////////////////////////////////////////////

            XmlNode xRoot = xmldoc.SelectSingleNode("XML");

            XmlNodeList xNodeList = xRoot.SelectNodes("Object");
            for (int i = 0; i < xNodeList.Count; ++i)
            {
                //NFCLog.Instance.Log("Class:" + xLogicClass.GetName());

                XmlNode xNodeClass = xNodeList.Item(i);
                XmlAttribute strID = xNodeClass.Attributes["ID"];

                //NFCLog.Instance.Log("ClassID:" + strID.Value);

                NFIElement xElement = GetElement(strID.Value);
                if (null == xElement)
                {
                    xElement = new NFCElement();
                    AddElement(strID.Value, xElement);
                    xLogicClass.AddConfigName(strID.Value);

                    XmlAttributeCollection xCollection = xNodeClass.Attributes;
                    for (int j = 0; j < xCollection.Count; ++j)
                    {
                        XmlAttribute xAttribute = xCollection[j];
                        NFIProperty xProperty = xLogicClass.GetPropertyManager().GetProperty(xAttribute.Name);
                        if (null != xProperty)
                        {
                            NFIDataList.VARIANT_TYPE eType = xProperty.GetType();
                            switch (eType)
                            {
                                case NFIDataList.VARIANT_TYPE.VTYPE_INT:
                                    {
                                        NFIDataList xValue = new NFCDataList();
                                        xValue.AddInt(int.Parse(xAttribute.Value));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case NFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
                                    {
                                        NFIDataList xValue = new NFCDataList();
                                        xValue.AddFloat(float.Parse(xAttribute.Value));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case NFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
                                    {
                                        NFIDataList xValue = new NFCDataList();
                                        xValue.AddDouble(double.Parse(xAttribute.Value));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case NFIDataList.VARIANT_TYPE.VTYPE_STRING:
                                    {
                                        NFIDataList xValue = new NFCDataList();
                                        xValue.AddString(xAttribute.Value);
                                        NFIProperty xTestProperty = xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case NFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
                                    {
                                        NFIDataList xValue = new NFCDataList();
                                        xValue.AddObject(new NFGUID(0, int.Parse(xAttribute.Value)));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /////////////////////////////////////////
        private NFILogicClassModule mxLogicClassModule;
        /////////////////////////////////////////
        private Dictionary<string, NFIElement> mhtObject;
        private string mstrRootPath;
    }
}