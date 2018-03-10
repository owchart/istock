using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dataquery.Web;

namespace dataquery
{
    /// <summary>
    /// 经济业务大全方法
    /// </summary>
    public partial class EcomomyForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public EcomomyForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            if (rbCountInArea.Checked)
            {
                ds = EcomomyDataHelper.GetCountInArea();
            }
            else if (rbBondNewMarket.Checked)
            {
                ds = EcomomyDataHelper.GetBondNewMarket();
            }
            else if (rbAreaNewMarket.Checked)
            {
                ds = EcomomyDataHelper.GetAreaNewMarket();
            }
            else if (rbDepartNewMarket.Checked)
            {
                ds = EcomomyDataHelper.GetDepartNewMarket();
            }
            else if (rbBondCityList.Checked)
            {
                ds = EcomomyDataHelper.GetBondCityList();
            }
            else if (rbProvinceList.Checked)
            {
                ds = EcomomyDataHelper.GeProvinceList();
            }
            else if (rbCityList.Checked)
            {
                ds = EcomomyDataHelper.GetCityList("440000");
            }
            else if (rbBondCompanyInput.Checked)
            {
                ds = EcomomyDataHelper.GetBondCompanyInput();
            }
            else if (rbOpenAcount.Checked)
            {
                ds = EcomomyDataHelper.GetOpenAcount("沪市", "A股");
            }
            else if (rbAStockAge.Checked)
            {
                ds = EcomomyDataHelper.GetAStockAge("沪市");
            }
            else if (rbAStockMoneyAll.Checked)
            {
                ds = EcomomyDataHelper.GetAStockMoneyAll("自然人");
            }
            else if (rbOpenArea.Checked)
            {
                ds = EcomomyDataHelper.GetOpenArea("沪市");
            }
            else if (rbBondMsgList.Checked)
            {
                ds = EcomomyDataHelper.GetBondMsgList("", "", "0");
            }
            else if (rbBondCompanyDetail.Checked)
            {
                ds = EcomomyDataHelper.GetBondCompanyDetail("80000199");
            }
            this.dgvData.DataSource = ds.Tables[0];
        }
    }
}
