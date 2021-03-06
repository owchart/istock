﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib {
    /// <summary>
    /// 
    /// </summary>
	[Serializable]
	public class SerializeFormula{
        /// <summary>
        /// 
        /// </summary>
		public SerializeFormula(){
			para = new SerializeFormulaPara[20];
			y = new double[10];
			y2 = new double[4];
		}
        /// <summary>
        /// 
        /// </summary>
		public int fid;
        /// <summary>
        /// 类型
        /// </summary>
		public int type;
        /// <summary>
        /// 名称
        /// </summary>
		public String name;
        /// <summary>
        /// 
        /// </summary>
		public String des;
        /// <summary>
        /// 
        /// </summary>
		public int subtype;
        /// <summary>
        /// 
        /// </summary>
		public int drawtype;
        /// <summary>
        /// 
        /// </summary>
		public int ynum;
        /// <summary>
        /// 
        /// </summary>
		public double[] y;
        /// <summary>
        /// 
        /// </summary>
		public int y2num;
        /// <summary>
        /// 
        /// </summary>
		public double[] y2;
        /// <summary>
        /// 密码
        /// </summary>
		public String password;
        /// <summary>
        /// 
        /// </summary>
		public String src;
        /// <summary>
        /// 
        /// </summary>
		public String paramtip;
        /// <summary>
        /// 
        /// </summary>
		public String help;
        /// <summary>
        /// 
        /// </summary>
		public String flag;
        /// <summary>
        /// 
        /// </summary>
		public int paracount;
        /// <summary>
        /// 
        /// </summary>
		public SerializeFormulaPara[] para;
	}

    /// <summary>
    /// 
    /// </summary>
	[Serializable]
	public class SerializeFormulaPara{
        /// <summary>
        /// 
        /// </summary>
		public SerializeFormulaPara(){
			uservalue = new double[20];
		}
        /// <summary>
        /// 名称
        /// </summary>
		public String name;
        /// <summary>
        /// 最小值
        /// </summary>
		public double minvalue;
        /// <summary>
        /// 最大值
        /// </summary>
		public double maxvalue;
        /// <summary>
        /// 
        /// </summary>
		public double defvalue;
        /// <summary>
        /// 
        /// </summary>
		public double step;
        /// <summary>
        /// 
        /// </summary>
		public double[] uservalue;
	}
}
