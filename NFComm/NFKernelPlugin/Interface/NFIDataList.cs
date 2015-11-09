﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFrame
{
    public abstract class NFIDataList
    {
        public enum VARIANT_TYPE
        {
            VTYPE_UNKNOWN,  // 未知
            VTYPE_INT,              // 32位整数
            VTYPE_FLOAT,            // 单精度浮点数
            VTYPE_DOUBLE,       // 双精度浮点数
            VTYPE_STRING,       // 字符串
            VTYPE_OBJECT,       // 对象ID
            VTYPE_MAX,
        };

        public class TData
        {
            public TData(TData x)
            {
                nType = x.nType;
                mData = x.mData;
            }

            public TData()
            {
                mData = new Object();
                nType = VARIANT_TYPE.VTYPE_UNKNOWN;
            }

            public VARIANT_TYPE nType;
            public Object mData;
        }

        public abstract bool AddInt(Int64 value);
        public abstract bool AddFloat(float value);
        public abstract bool AddDouble(double value);
        public abstract bool AddString(string value);
        public abstract bool AddObject(NFIDENTID value);

        public abstract bool SetInt(int index, Int64 value);
        public abstract bool SetFloat(int index, float value);
        public abstract bool SetDouble(int index, double value);
        public abstract bool SetString(int index, string value);
        public abstract bool SetObject(int index, NFIDENTID value);

        public abstract Int64 IntVal(int index);
        public abstract float FloatVal(int index);
        public abstract double DoubleVal(int index);
        public abstract string StringVal(int index);
        public abstract NFIDENTID ObjectVal(int index);

		public abstract int Count();
		public abstract void Clear();
        public abstract VARIANT_TYPE GetType(int index);
        public abstract TData GetData(int index);
    }
}

