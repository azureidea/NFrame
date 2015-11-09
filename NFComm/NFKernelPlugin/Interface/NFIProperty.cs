using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFrame
{
    public abstract class NFIProperty
    {
	public delegate void PropertyEventHandler(NFIDENTID self, string strProperty, NFIDataList oldVar, NFIDataList newVar);

    public abstract string GetKey();
		
	public abstract NFIDataList.VARIANT_TYPE GetType();
    public abstract NFIDataList.TData GetData();

    public abstract Int64 QueryInt();

    public abstract float QueryFloat();

    public abstract double QueryDouble();

    public abstract string QueryString();

    public abstract NFIDENTID QueryObject();

	public abstract bool SetInt(Int64 value);

	public abstract bool SetFloat(float value);

	public abstract bool SetDouble(double value);

	public abstract bool SetString(string value);

    public abstract bool SetObject(NFIDENTID value);

    public abstract bool SetData(NFIDataList.TData x);

	public abstract void RegisterCallback(PropertyEventHandler handler);
    }
}