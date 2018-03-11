/*****************************************************************************\
*                                                                             *
* MainFrame.cs -  MainFrame functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using OwLibSV;

namespace OwLib
{
    /// <summary>
    /// ����ϵͳ
    /// </summary>
    public class MainFrame : UIXmlEx, IDisposable
    {
        /// <summary>
        /// ��������ϵͳ
        /// </summary>
        public MainFrame()
        {
        }

        /// <summary>
        /// K�߿ؼ�
        /// </summary>
        private ChartEx m_chartEx;

        /// <summary>
        /// ��ѡ�ɱ��
        /// </summary>
        private GridA m_gridUserSecurities;

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        /// <summary>
        /// �����ѡ��
        /// </summary>
        /// <param name="code">����</param>
        public void AddUserSecurity(String code)
        {
            List<GridRow> rows = m_gridUserSecurities.m_rows;
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                GridRow findRow = rows[i];
                if (findRow.GetCell("colP1").GetString() == code)
                {
                    return;
                }
            }
            GridRow row = new GridRow();
            row.AllowEdit = true;
            row.Height = 30;
            m_gridUserSecurities.AddRow(row);
            ButtonA editButton = new ButtonA();
            editButton.RegisterEvent(new ControlMouseEvent(ClickEvent), EVENTID.CLICK);
            editButton.Font = new FONT("΢���ź�", 16, true, false, false);
            editButton.BackColor = COLOR.ARGB(255, 80, 80);
            editButton.ForeColor = COLOR.ARGB(255, 255, 255);
            editButton.Text = "ɾ��";
            editButton.Name = "btnGridRowEdit";
            editButton.Size = new SIZE(100, 30);
            row.EditButton = editButton;
            row.AddCell("colP1", new GridStringCell(code));
            row.AddCell("colP2", new GridStringCell());
            GridDoubleCellEx cellP3 = new GridDoubleCellEx();
            cellP3.Digit = 2;
            row.AddCell("colP3", cellP3);
            GridDoubleCellEx cellP4 = new GridDoubleCellEx();
            cellP4.Digit = 2;
            row.AddCell("colP4", cellP4);
            GridDoubleCellEx cellP5 = new GridDoubleCellEx();
            cellP5.Digit = 2;
            cellP5.IsPercent = true;
            row.AddCell("colP5", cellP5);
            GridDoubleCellEx cellP6 = new GridDoubleCellEx();
            cellP6.Digit = 2;
            row.AddCell("colP6", cellP6);
            GridDoubleCellEx cellP7 = new GridDoubleCellEx();
            cellP7.Digit = 2;
            row.AddCell("colP7", cellP7);
            GridDoubleCellEx cellP8 = new GridDoubleCellEx();
            cellP8.Digit = 2;
            row.AddCell("colP8", cellP8);
            row.AddCell("colP9", new GridDoubleCellEx());
            row.AddCell("colP10", new GridDoubleCellEx());
            List<GridCell> cells = row.GetCells();
            int cellsSize = cells.Count;
            for (int i = 0; i < cellsSize; i++)
            {
                cells[i].Style = new GridCellStyle();
                cells[i].Style.Font = new FONT("΢���ź�", 14, true, false, false);
            }
            DataCenter.UserSecurityService.Add(code);
            m_gridUserSecurities.Update();
            m_gridUserSecurities.Invalidate();
        }

        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void ClickEvent(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnAdd")
                {
                    String code = FindControl("txtCode").Text;
                    GSecurity security = new GSecurity();
                    if (SecurityService.GetSecurityByCode(code, ref security) > 0)
                    {
                        AddUserSecurity(code);
                    }
                }
                else if (name == "btnGridRowEdit")
                {
                    List<GridRow> selectedRows = m_gridUserSecurities.SelectedRows;
                    int selectedRowsSize = selectedRows.Count;
                    if (selectedRowsSize > 0)
                    {
                        DataCenter.UserSecurityService.Delete(selectedRows[0].GetCell("colP1").GetString());
                        m_gridUserSecurities.AnimateRemoveRow(selectedRows[0]);
                        m_gridUserSecurities.OnRowEditEnd();
                    }
                }
                else if (name == "btnMergeHistoryDatas")
                {
                    SecurityService.Start3();
                }
            }
        }

        /// <summary>
        /// ������Դ����
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// ����֤ȯ
        /// </summary>
        /// <param name="args"></param>
        private void DownloadHistoryDatas(object args)
        {
            int index = (int)args;
            QuoteSequencService.DownAllStockHistory(index);
        }

        /// <summary>
        /// �˳�����
        /// </summary>
        public override void Exit()
        {
        }

        /// <summary>
        /// ��Ԫ��˫���¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="cell">��Ԫ��</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (clicks == 2)
            {
                String code = cell.Row.GetCell("colP1").GetString();
                if (m_chartEx != null)
                {
                    GSecurity security = new GSecurity();
                    SecurityService.GetSecurityByCode(code, ref security);
                    m_chartEx.SearchSecurity(security);
                    GetTabControl("tabMain").SelectedIndex = 1;
                }
            }
        }

        /// <summary>
        /// ��ʷ���ݻص�
        /// </summary>
        public void HistoryDatasCallBack(OneStockKLineDataRec oneStockKLineDataRec)
        {
            List<SecurityData> securityDatas = new List<SecurityData>();
            int size = oneStockKLineDataRec.OneDayDataList.Count;
            for (int i = 0; i < size; i++)
            {
                OneDayDataRec oneDayDataRec = oneStockKLineDataRec.OneDayDataList[i];
                SecurityData securityData = new SecurityData();
                securityData.m_close = oneDayDataRec.Close;
                securityData.m_high = oneDayDataRec.High;
                securityData.m_low = oneDayDataRec.Low;
                securityData.m_open = oneDayDataRec.Open;
                securityData.m_volume = oneDayDataRec.Volume;
                securityData.m_amount = oneDayDataRec.Amount;
                securityData.m_date = oneDayDataRec.Date;
                securityDatas.Add(securityData);
            }
            HistoryDataInfo historyDataInfo = new HistoryDataInfo();
            historyDataInfo.m_codes = CStrA.ConvertEMCodeToDBCode(EMSecurityService.GetKwItemByInnerCode(oneStockKLineDataRec.Code).Code);
            historyDataInfo.m_cycle = 1440;
            historyDataInfo.m_subscription = 1;
            m_chartEx.BindHistoryData(historyDataInfo, securityDatas);
        }

        /// <summary>
        /// �Ƿ��д�����ʾ
        /// </summary>
        /// <returns>�Ƿ���ʾ</returns>
        public bool IsWindowShowing()
        {
            List<ControlA> controls = Native.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                WindowFrameA frame = controls[i] as WindowFrameA;
                if (frame != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// LV2���ݻص�
        /// </summary>
        /// <param name="fieldInt32"></param>
        /// <param name="fieldSingle"></param>
        /// <param name="fieldInt64"></param>
        /// <param name="fieldDouble"></param>
        public void LV2DatasCallBack(int code, Dictionary<FieldIndex, int> fieldInt32,
                Dictionary<FieldIndex, float> fieldSingle,
                Dictionary<FieldIndex, long> fieldInt64,
                Dictionary<FieldIndex, double> fieldDouble)
        {
            SecurityLatestData latestData = new SecurityLatestData();
            latestData.m_code = CStrA.ConvertEMCodeToDBCode(EMSecurityService.GetKwItemByInnerCode(code).Code);
            latestData.m_high = fieldSingle[FieldIndex.High];
            latestData.m_low = fieldSingle[FieldIndex.Low];
            latestData.m_open = fieldSingle[FieldIndex.Open];
            latestData.m_close = fieldSingle[FieldIndex.Now];
            latestData.m_lastClose = fieldSingle[FieldIndex.PreClose];
            latestData.m_sellPrice1 = fieldSingle[FieldIndex.SellPrice1];
            latestData.m_sellPrice2 = fieldSingle[FieldIndex.SellPrice2];
            latestData.m_sellPrice3 = fieldSingle[FieldIndex.SellPrice3];
            latestData.m_sellPrice4 = fieldSingle[FieldIndex.SellPrice4];
            latestData.m_sellPrice5 = fieldSingle[FieldIndex.SellPrice5];
            latestData.m_sellPrice6 = fieldSingle[FieldIndex.SellPrice6];
            latestData.m_sellPrice7 = fieldSingle[FieldIndex.SellPrice7];
            latestData.m_sellPrice8 = fieldSingle[FieldIndex.SellPrice8];
            latestData.m_sellPrice9 = fieldSingle[FieldIndex.SellPrice9];
            latestData.m_sellPrice10 = fieldSingle[FieldIndex.SellPrice10];
            latestData.m_sellVolume1 = fieldInt32[FieldIndex.SellVolume1];
            latestData.m_sellVolume2 = fieldInt32[FieldIndex.SellVolume2];
            latestData.m_sellVolume3 = fieldInt32[FieldIndex.SellVolume3];
            latestData.m_sellVolume4 = fieldInt32[FieldIndex.SellVolume4];
            latestData.m_sellVolume5 = fieldInt32[FieldIndex.SellVolume5];
            latestData.m_sellVolume6 = fieldInt32[FieldIndex.SellVolume6];
            latestData.m_sellVolume7 = fieldInt32[FieldIndex.SellVolume7];
            latestData.m_sellVolume8 = fieldInt32[FieldIndex.SellVolume8];
            latestData.m_sellVolume9 = fieldInt32[FieldIndex.SellVolume9];
            latestData.m_sellVolume10 = fieldInt32[FieldIndex.SellVolume10];
            latestData.m_buyPrice1 = fieldSingle[FieldIndex.BuyPrice1];
            latestData.m_buyPrice2 = fieldSingle[FieldIndex.BuyPrice2];
            latestData.m_buyPrice3 = fieldSingle[FieldIndex.BuyPrice3];
            latestData.m_buyPrice4 = fieldSingle[FieldIndex.BuyPrice4];
            latestData.m_buyPrice5 = fieldSingle[FieldIndex.BuyPrice5];
            latestData.m_buyPrice6 = fieldSingle[FieldIndex.BuyPrice6];
            latestData.m_buyPrice7 = fieldSingle[FieldIndex.BuyPrice7];
            latestData.m_buyPrice8 = fieldSingle[FieldIndex.BuyPrice8];
            latestData.m_buyPrice9 = fieldSingle[FieldIndex.BuyPrice9];
            latestData.m_buyPrice10 = fieldSingle[FieldIndex.BuyPrice10];
            latestData.m_buyVolume1 = fieldInt32[FieldIndex.BuyVolume1];
            latestData.m_buyVolume2 = fieldInt32[FieldIndex.BuyVolume2];
            latestData.m_buyVolume3 = fieldInt32[FieldIndex.BuyVolume3];
            latestData.m_buyVolume4 = fieldInt32[FieldIndex.BuyVolume4];
            latestData.m_buyVolume5 = fieldInt32[FieldIndex.BuyVolume5];
            latestData.m_buyVolume6 = fieldInt32[FieldIndex.BuyVolume6];
            latestData.m_buyVolume7 = fieldInt32[FieldIndex.BuyVolume7];
            latestData.m_buyVolume8 = fieldInt32[FieldIndex.BuyVolume8];
            latestData.m_buyVolume9 = fieldInt32[FieldIndex.BuyVolume9];
            latestData.m_buyVolume10 = fieldInt32[FieldIndex.BuyVolume10];
            m_chartEx.LatestDiv.LatestData = latestData;
        }

        /// <summary>
        /// ����XML
        /// </summary>
        /// <param name="xmlPath">XML·��</param>
        public override void Load(String xmlPath)
        {
            LoadFile(xmlPath, null);
            ControlA control = Native.GetControls()[0];
            control.BackColor = COLOR.CONTROL;
            RegisterEvents(control);
            m_gridUserSecurities = GetGrid("gridUserSecurities");
            List<String> codes = DataCenter.UserSecurityService.m_codes;
            int codesSize = codes.Count;
            for (int i = 0; i < codesSize; i++)
            {
                AddUserSecurity(codes[i]);
            }
            m_gridUserSecurities.UseAnimation = true;
            m_gridUserSecurities.GridLineColor = COLOR.EMPTY;
            m_gridUserSecurities.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridUserSecurities.RowStyle = new GridRowStyle();
            m_gridUserSecurities.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridUserSecurities.RegisterEvent(new ControlTimerEvent(TimerEvent), EVENTID.TIMER);
            m_gridUserSecurities.StartTimer(m_timerID, 1000);
            m_gridUserSecurities.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            m_chartEx = new ChartEx(this);
            DataCenter.MainUI = this;
        }

        /// <summary>
        /// �ػ�˵�����
        /// </summary>
        /// <param name="sender">���ö���</param>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        private void PaintLayoutDiv(object sender, CPaint paint, OwLib.RECT clipRect)
        {
            ControlA control = sender as ControlA;
            int width = control.Width, height = control.Height;
            OwLib.RECT drawRect = new OwLib.RECT(0, 0, width, height);
            paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR, CDraw.PCOLORS_BACKCOLOR2, drawRect, 0, 90);
        }

        /// ע���¼�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickEvent);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                ButtonA button = subControl as ButtonA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// ��ʾ��ʾ����
        /// </summary>
        /// <param name="text">�ı�</param>
        /// <param name="caption">����</param>
        /// <param name="uType">��ʽ</param>
        /// <returns>���</returns>
        public int ShowMessageBox(String text, String caption, int uType)
        {
            MessageBox.Show(text, caption);
            return 1;
        }

        /// <summary>
        /// ��ʼ������ʷ����
        /// </summary>
        public void StartDownloadHistoryDatas()
        {
            DataCenter dc = new DataCenter();
            Dictionary<String, KwItem> availableItems = EMSecurityService.KwItems;
            int availableItemsSize = availableItems.Count;
            int size = availableItemsSize / 50;
            for (int i = 0; i <= size; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(DownloadHistoryDatas));
                thread.Start(i);
            }
        }

        /// <summary>
        /// ����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="sender">���ID</param>
        private void TimerEvent(object sender, int timerID)
        {
            if (timerID == m_timerID)
            {
                List<GridRow> rows = m_gridUserSecurities.m_rows;
                int rowsSize = rows.Count;
                for (int i = 0; i < rowsSize; i++)
                {
                    GridRow row = rows[i];
                    String code = row.GetCell("colP1").GetString();
                    GSecurity security = new GSecurity();
                    row.GetCell("colP1").Style.ForeColor = COLOR.ARGB(255, 255, 255);
                    SecurityLatestData latestData = new SecurityLatestData();
                    SecurityService.GetSecurityByCode(code, ref security);
                    SecurityService.GetLatestData(code, ref latestData);
                    row.GetCell("colP2").SetString(security.m_name);
                    row.GetCell("colP2").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                    double diff = 0, diffRange = 0;
                    if (latestData.m_lastClose != 0)
                    {
                        diff = latestData.m_close - latestData.m_lastClose;
                        diffRange = 100 * (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose;
                    }
                    row.GetCell("colP3").SetDouble(latestData.m_close);
                    row.GetCell("colP3").Style.ForeColor = CDraw.GetPriceColor(latestData.m_close, latestData.m_lastClose);
                    row.GetCell("colP4").SetDouble(diff);
                    row.GetCell("colP4").Style.ForeColor = CDraw.GetPriceColor(latestData.m_close, latestData.m_lastClose);
                    row.GetCell("colP5").SetDouble(diffRange);
                    row.GetCell("colP5").Style.ForeColor = CDraw.GetPriceColor(latestData.m_close, latestData.m_lastClose);
                    row.GetCell("colP6").SetDouble(latestData.m_high);
                    row.GetCell("colP6").Style.ForeColor = CDraw.GetPriceColor(latestData.m_high, latestData.m_lastClose);
                    row.GetCell("colP7").SetDouble(latestData.m_low);
                    row.GetCell("colP7").Style.ForeColor = CDraw.GetPriceColor(latestData.m_low, latestData.m_lastClose);
                    row.GetCell("colP8").SetDouble(latestData.m_open);
                    row.GetCell("colP8").Style.ForeColor = CDraw.GetPriceColor(latestData.m_open, latestData.m_lastClose);
                    row.GetCell("colP9").SetDouble(latestData.m_volume);
                    row.GetCell("colP9").Style.ForeColor = COLOR.ARGB(80, 255, 255);
                    row.GetCell("colP10").SetDouble(latestData.m_amount);
                    row.GetCell("colP10").Style.ForeColor = COLOR.ARGB(80, 255, 255);
                }
                m_gridUserSecurities.Invalidate();
            }
        }
    }

    public class GridDoubleCellEx : GridDoubleCell
    {
        private int m_digit;

        /// <summary>
        /// ��ȡ�����ñ���λ��
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private bool m_isPercent;

        /// <summary>
        /// ��ȡ�������Ƿ�ٷֱ�
        /// </summary>
        public bool IsPercent
        {
            get { return m_isPercent; }
            set { m_isPercent = value; }
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns>��������</returns>
        public override string GetPaintText()
        {
            String text = CStr.GetValueByDigit(m_value, m_digit);
            if (m_isPercent)
            {
                text += "%";
            }
            return text;
        }
    }
}
